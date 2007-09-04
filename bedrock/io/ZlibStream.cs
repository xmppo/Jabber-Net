/* --------------------------------------------------------------------------
 * Copyrights
 *
 * Portions created by or assigned to Cursive Systems, Inc. are
 * Copyright (c) 2002-2007 Cursive Systems, Inc.  All Rights Reserved.  Contact
 * information for Cursive Systems, Inc. is available at
 * http://www.cursive.net/.
 *
 * License
 *
 * Jabber-Net can be used under either JOSL or the GPL.
 * See LICENSE.txt for details.
 * --------------------------------------------------------------------------*/
#if !NO_COMPRESSION
using System;
using System.IO;
using System.Diagnostics;

using ComponentAce.Compression.Libs.zlib;
using bedrock.util;

namespace bedrock.io
{
    /// <summary>
    /// Compression failed.
    /// </summary>
    public class CompressionFailedException : ApplicationException
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="message"></param>
        public CompressionFailedException(string message) : base(message) { }

        /// <summary>
        ///
        /// </summary>
        public CompressionFailedException() : base() { }
    }


    /// <summary>
    /// Wrap two ComponentAce.Compression.Libs.zlib.ZStream's, one in and one out.
    /// The existing wrappers in the project are uni-directional.
    /// 
    /// No, System.IO.Compression.GZipStream won't work, because they didn't expose 
    /// compression levels or flush types.
    /// </summary>
    [SVN(@"$Id$")]
    public class ZlibStream : Stream
    {
        private Stream m_stream = null;
        private ZStream m_in;
        private ZStream m_out;
        private int m_flush = zlibConst.Z_PARTIAL_FLUSH;

        private const int bufsize = 1024;
        private byte[] m_inbuf;
        private byte[] m_outbuf;

        /// <summary>
        /// Wrap a bi-directional stream in a compression stream.
        /// </summary>
        /// <param name="innerStream">The stream to wrap.</param>
        public ZlibStream(Stream innerStream)
        {
            init(innerStream);
        }

        /// <summary>
        /// Wrap a bi-directional stream in a compression stream.
        /// </summary>
        /// <param name="innerStream">The stream to wrap.</param>
        /// <param name="flush">The flush type.  TODO: doc these.</param>
        public ZlibStream(Stream innerStream, int flush)
        {
            m_flush = flush;
            init(innerStream);
        }


        private void init(Stream innerStream)
        {
            m_stream = innerStream;
            if (m_stream.CanRead)
            {
                m_in = new ZStream();
                int ret = m_in.inflateInit();
                if (ret != zlibConst.Z_OK)
                    throw new CompressionFailedException("Unable to initialize zlib for deflate: " + ret);
                m_inbuf = new byte[bufsize];
                m_in.avail_in = 0;
                m_in.next_in = m_inbuf;
                m_in.next_in_index = 0;
            }

            if (m_stream.CanWrite)
            {
                m_out = new ZStream();
                int ret = m_out.deflateInit(zlibConst.Z_DEFAULT_COMPRESSION);
                if (ret != zlibConst.Z_OK)
                    throw new CompressionFailedException("Unable to initialize zlib for inflate: " + ret);
                m_outbuf = new byte[bufsize];
                m_out.next_out = m_outbuf;
            }
        }

        /// <summary>
        /// Is the underlying stream readable?
        /// </summary>
        public override bool CanRead
        {
            get { return m_stream.CanRead; }
        }

        /// <summary>
        /// No seeking on compressed streams.  That sounds hard.
        /// </summary>
        public override bool CanSeek
        {
            get { return false; }
        }

        /// <summary>
        /// Is the underlying stream writable?
        /// </summary>
        public override bool CanWrite
        {
            get { return m_stream.CanWrite; }
        }

        /// <summary>
        /// This just flushes the stream, but doesn't perform zlib flushing.
        /// </summary>
        public override void Flush()
        {
            m_stream.Flush();
        }

        /// <summary>
        /// Not implemented.
        /// </summary>
        public override long Length
        {
            get { throw new NotImplementedException("The method or operation is not implemented."); }
        }

        /// <summary>
        /// Not implemented.
        /// </summary>
        public override long Position
        {
            get { throw new NotImplementedException("The method or operation is not implemented."); }
            set { throw new NotImplementedException("The method or operation is not implemented."); }
        }

        /// <summary>
        /// Start an async read. Implemented locally, since Stream.BeginRead() isn't really asynch.
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            if (count <= 0)
                throw new ArgumentException("Can't read 0 bytes", "count");

            if (m_in.avail_in == 0)
            {
                m_in.next_out = buffer;
                m_in.next_out_index = offset;
                m_in.avail_out = count;
                m_in.next_in_index = 0;
                return m_stream.BeginRead(m_inbuf, 0, bufsize, callback, state);
            }
            ZlibStreamAsyncResult ar = new ZlibStreamAsyncResult(state);
            callback(ar);
            return ar;
        }

        /// <summary>
        /// Complete a pending read, when the callback passed to BeginRead fires.
        /// </summary>
        /// <param name="asyncResult"></param>
        /// <returns></returns>
        public override int EndRead(IAsyncResult asyncResult)
        {
            if (!(asyncResult is ZlibStreamAsyncResult))
                m_in.avail_in = m_stream.EndRead(asyncResult);
            return Inflate();
        }

        /// <summary>
        /// Synchronous read.  Decompresses.
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public override int Read(byte[] buffer, int offset, int count)
        {
            if (count <= 0)
                return 0;

            m_in.next_out = buffer;
            m_in.next_out_index = offset;
            m_in.avail_out = count;

            if (m_in.avail_in == 0)
            {
                m_in.next_in_index = 0;
                m_in.avail_in = m_stream.Read(m_inbuf, 0, bufsize);
                if (m_in.avail_in == 0)
                    return 0;
            }
            return Inflate();
        }

        private int Inflate()
        {
            int count = m_in.avail_out;
            int err = m_in.inflate(m_flush);
            if ((err != zlibConst.Z_OK) && (err != zlibConst.Z_STREAM_END))
            {
                if (err == zlibConst.Z_BUF_ERROR)
                    return 0;
                if (err != zlibConst.Z_OK)
                    throw new CompressionFailedException("Decompress failed: " + err);
            }
            return (count - m_in.avail_out);
        }

        /// <summary>
        /// Not implemented.
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="origin"></param>
        /// <returns></returns>
        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException("The method or operation is not implemented.");
        }

        /// <summary>
        /// Not implemented.
        /// </summary>
        /// <param name="value"></param>
        public override void SetLength(long value)
        {
            throw new NotImplementedException("The method or operation is not implemented.");
        }

        /// <summary>
        /// Begin an asynch write, compressing first.  Implemented locally, since Stream.BeginWrite isn't asynch.
        /// Note: may call Write() on the underlying stream more than once.
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            if (count <= 0)
                throw new ArgumentException("Can't write 0 bytes", "count");

            m_out.next_in = buffer;
            m_out.next_in_index = offset;
            m_out.avail_in = count;

            m_out.next_out_index = 0;
            m_out.avail_out = bufsize;
            int err = m_out.deflate(m_flush);
            if (err != zlibConst.Z_STREAM_END)
            {
                if (err != zlibConst.Z_OK)
                {
                    ZlibStreamAsyncResult res = new ZlibStreamAsyncResult(state, new CompressionFailedException("Compress failed: " + err));
                    callback(res);
                    return res;
                }
            }
            if (m_out.avail_in == 0)
                return m_stream.BeginWrite(m_outbuf, 0, bufsize - m_out.avail_out, callback, state);
            else
                return m_stream.BeginWrite(m_outbuf, 0, bufsize - m_out.avail_out, new AsyncCallback(IntermediateWrite), new ZlibState(callback, state));
        }

        private void IntermediateWrite(IAsyncResult asyncResult)
        {
            ZlibState state = (ZlibState)asyncResult.AsyncState;
            try
            {
                m_stream.EndWrite(asyncResult);
            }
            catch (Exception e)
            {
                ZlibStreamAsyncResult res = new ZlibStreamAsyncResult(state.state, e);
                state.callback(res);
                return;
            }

            m_out.next_out_index = 0;
            m_out.avail_out = bufsize;
            int err = m_out.deflate(m_flush);
            if (err != zlibConst.Z_STREAM_END)
            {
                if (err != zlibConst.Z_OK)
                {
                    ZlibStreamAsyncResult res = new ZlibStreamAsyncResult(state.state, new CompressionFailedException("Compress failed: " + err));
                    state.callback(res);
                    return;
                }
            }
            if (m_out.avail_in == 0)
                m_stream.BeginWrite(m_outbuf, 0, bufsize - m_out.avail_out, state.callback, state.state);
            else
                m_stream.BeginWrite(m_outbuf, 0, bufsize - m_out.avail_out, new AsyncCallback(IntermediateWrite), state);
        }

        /// <summary>
        /// Complete a pending write, when the callback given to BeginWrite is called.
        /// </summary>
        /// <param name="asyncResult"></param>
        public override void EndWrite(IAsyncResult asyncResult)
        {
            if (asyncResult is ZlibStreamAsyncResult)
            {
                ZlibStreamAsyncResult ar = (ZlibStreamAsyncResult)asyncResult;
                if (ar.Exception != null)
                    throw ar.Exception;
            }
            else
                m_stream.EndWrite(asyncResult);
            return;
        }

        /// <summary>
        /// Synchronous write, after compressing.
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        public override void Write(byte[] buffer, int offset, int count)
        {
            if (count <= 0)
                return;
            m_out.next_in = buffer;
            m_out.next_in_index = offset;
            m_out.avail_in = count;

            while (m_out.avail_in > 0)
            {
                m_out.next_out_index = 0;
                m_out.avail_out = bufsize;
                int err = m_out.deflate(m_flush);
                if (err != zlibConst.Z_STREAM_END)
                {
                    if (err != zlibConst.Z_OK)
                        throw new CompressionFailedException("Compress failed: " + err);
                }
                m_stream.Write(m_outbuf, 0, bufsize - m_out.avail_out);
            }
        }

        private class ZlibStreamAsyncResult : IAsyncResult
        {
            private object m_state = null;
            private Exception m_exception;

            public ZlibStreamAsyncResult(object state)
            {
                m_state = state;
            }

            public ZlibStreamAsyncResult(object state, Exception except)
            {
                m_state = state;
                m_exception = except;
            }

            public Exception Exception
            {
                get { return m_exception; }
            }

            #region IAsyncResult Members

            public object AsyncState
            {
                get { return m_state; }
            }

            public System.Threading.WaitHandle AsyncWaitHandle
            {
                get { throw new Exception("The method or operation is not implemented."); }
            }

            public bool CompletedSynchronously
            {
                get { return true; }
            }

            public bool IsCompleted
            {
                get { return true; }
            }

            #endregion
        }

        private class ZlibState
        {
            public AsyncCallback callback;
            public object state;

            public ZlibState(AsyncCallback callback, object state)
            {
                this.callback = callback;
                this.state = state;
            }
        }
    }
}
#endif