/* --------------------------------------------------------------------------
 * Copyrights
 *
 * Portions created by or assigned to Cursive Systems, Inc. are
 * Copyright (c) 2002-2008 Cursive Systems, Inc.  All Rights Reserved.  Contact
 * information for Cursive Systems, Inc. is available at
 * http://www.cursive.net/.
 *
 * License
 *
 * Jabber-Net is licensed under the LGPL.
 * See licenses/Jabber-Net_LGPLv3.txt for details.
 * --------------------------------------------------------------------------*/

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ComponentAce.Compression.Libs.zlib;

namespace JabberNet.bedrock.io
{
    /// <summary>
    /// Compression failed.
    /// </summary>
    public class CompressionFailedException : Exception
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
    public class ZlibStream : Stream
    {
        /// <summary>
        /// Is zlib supported?  Note, this will throw an exception if the library is missing.
        /// </summary>
        public static bool Supported
        {
            get
            {
                return zlibConst.version() != "";
            }
        }

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

        public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            if (count <= 0)
                throw new ArgumentException("Can't read 0 bytes", "count");

            m_in.next_out = buffer;
            m_in.next_out_index = offset;
            m_in.avail_out = count;
            if (m_in.avail_in == 0)
            {
                m_in.next_in_index = 0;
                return m_stream.ReadAsync(m_inbuf, 0, bufsize, cancellationToken);
            }

            return Task.FromResult(Inflate());
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

        public override async Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
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
                    throw new CompressionFailedException("Compress failed: " + err);
                }
            }
            if (m_out.avail_in == 0)
                await m_stream.WriteAsync(m_outbuf, 0, bufsize - m_out.avail_out, cancellationToken);
            else
            {
                await m_stream.WriteAsync(m_outbuf, 0, bufsize - m_out.avail_out, cancellationToken);
                await IntermediateWrite(cancellationToken);
            }
        }

        private async Task IntermediateWrite(CancellationToken cancellationToken)
        {
            m_out.next_out_index = 0;
            m_out.avail_out = bufsize;
            int err = m_out.deflate(m_flush);
            if (err != zlibConst.Z_STREAM_END)
            {
                if (err != zlibConst.Z_OK)
                {
                    throw new CompressionFailedException("Compress failed: " + err);
                }
            }
            if (m_out.avail_in == 0)
                await m_stream.WriteAsync(m_outbuf, 0, bufsize - m_out.avail_out, cancellationToken);
            else
            {
                await m_stream.WriteAsync(m_outbuf, 0, bufsize - m_out.avail_out, cancellationToken);
                await IntermediateWrite(cancellationToken);
            }
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
    }
}
