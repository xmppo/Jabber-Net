/* --------------------------------------------------------------------------
 *
 * License
 *
 * The contents of this file are subject to the Jabber Open Source License
 * Version 1.0 (the "License").  You may not copy or use this file, in either
 * source code or executable form, except in compliance with the License.  You
 * may obtain a copy of the License at http://www.jabber.com/license/ or at
 * http://www.opensource.org/.  
 *
 * Software distributed under the License is distributed on an "AS IS" basis,
 * WITHOUT WARRANTY OF ANY KIND, either express or implied.  See the License
 * for the specific language governing rights and limitations under the
 * License.
 *
 * Copyrights
 * 
 * Portions created by or assigned to Cursive Systems, Inc. are 
 * Copyright (c) 2002 Cursive Systems, Inc.  All Rights Reserved.  Contact
 * information for Cursive Systems, Inc. is available at http://www.cursive.net/.
 *
 * Portions Copyright (c) 2002 Joe Hildebrand.
 * 
 * Acknowledgements
 * 
 * Special thanks to the Jabber Open Source Contributors for their
 * suggestions and support of Jabber.
 * 
 * --------------------------------------------------------------------------*/
using System;
using System.IO;
using System.Collections;
using System.Threading;
using System.Diagnostics;
using bedrock.util;
namespace bedrock.io
{
    /// <summary>
    /// Replacement for System.IO.MemoryStream that can be both read and written to, 
    /// with reads being destructive.
    /// </summary>
    [RCS(@"$Header$")]
    public class PipeStream : Stream
    {
        // Queue is implemented as a growable circular array.
        private Queue  m_queue      = new Queue();  
        private Queue  m_partial    = new Queue();
        private byte[] m_leftOver   = null;
        private int    m_leftOffset = 0;
        private bool   m_closed     = false;
        private bool   m_autoClose  = false;
        private object m_readLock   = new object();

        /// <summary>
        /// Create a new PipeStream with AutoClose off.
        /// </summary>
        public PipeStream()
        {
        }
        /// <summary>
        /// New PipeStream with the specified AutoClose setting.
        /// </summary>
        /// <param name="AutoClose"></param>
        public PipeStream(bool AutoClose) : this()
        {
            m_autoClose = AutoClose;
        }
        /// <summary>
        /// When AutoClose is true, Read() always returns 0 when there
        /// is nothing currently ready to read.  
        /// </summary>
        public bool AutoClose
        {
            get
            {
                return m_autoClose;
            }
            set
            {
                m_autoClose = value;
            }
        }
        /// <summary>
        /// Are there bytes to read?
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return (m_leftOver == null) && (m_queue.Count == 0);
            }
        }
        /// <summary>
        /// Has the stream been forcibly closed, using Close()?
        /// </summary>
        public bool IsOpen
        {
            get
            {
                return ! m_closed;
            }
        }
        #region System.IO.Stream
        /// <summary>
        /// Write bytes to the stream.   The bytes are trimmed and copied internally.
        /// </summary>
        /// <param name="buffer">Buffer to read from</param>
        /// <param name="offset">Where to start reading</param>
        /// <param name="count">How many bytes to write</param>
        public override void Write(byte[] buffer, int offset, int count)
        {
            lock (m_readLock)
            {
                if (m_closed)
                    throw new InvalidOperationException("Cannot write to closed stream");
                Debug.Assert(count > 0);
                // this hack is a little better than the read one byte hack.  
                // Chunk at >'s.  This tends to be correct, and not add as many 
                // calls to Read().
                int end = offset + count;
                int mark = offset;
                byte[] buf;
                for (int i=offset; i<end; i++)
                {
                    if (buffer[i] == '>')
                    {
                        while (m_partial.Count > 0)
                            m_queue.Enqueue(m_partial.Dequeue());

                        buf = new byte[i - mark + 1];
                        Buffer.BlockCopy(buffer, mark, buf, 0, i - mark + 1);
                        m_queue.Enqueue(buf);
                        mark = i + 1;
                    }
                }
                // something partial.  anything parsable would have ended with a '>'.
                if (mark < end)
                {
                    buf = new byte[end - mark];
                    Buffer.BlockCopy(buffer, mark, buf, 0, end - mark);
                    m_partial.Enqueue(buf);
                }
                Monitor.Pulse(m_readLock);
            }
        }
        /// <summary>
        /// Equivalent of Write(buffer, 0, buffer.Length).  The bytes are copied internally.
        /// </summary>
        /// <param name="buffer">Buffer to insert.</param>
        public void Write (byte[] buffer)
        {
            Write(buffer, 0, buffer.Length);
        }
        /// <summary>
        /// Read from the stream into the given buffer.
        /// </summary>
        /// <param name="buffer">Buffer to copy bytes into</param>
        /// <param name="offset">Where to start writing</param>
        /// <param name="count">Maximum bytes to read</param>
        /// <returns>-1 if closed, 0 if AutoClosed, otherwise number of bytes in the read chunk.</returns>
        public override int Read(byte[] buffer, int offset, int count)
        {
            lock (m_readLock)
            {
                if (m_closed)
                {
                    return -1;
                }
                byte[] buf = null;
                int read   = 0;
                if (m_leftOver != null)
                {
                    buf        = m_leftOver;
                    m_leftOver = null;
                }
                else
                {
                    if (m_queue.Count == 0)
                    {
                        if (m_autoClose)
                        {
                            return 0;
                        }
                        // Note: this gives up the lock
                        Monitor.Wait(m_readLock);
                    }
                    buf = (byte[]) m_queue.Dequeue();
                    m_leftOffset = 0;
                }
                // read up to count bytes from the next chunk, or from the leftover bytes
                int len    = buf.Length - m_leftOffset;
                read     = Math.Min(len, count);
                int left = len - read;
                Buffer.BlockCopy(buf, m_leftOffset, buffer, offset, read);
                if (left > 0)
                {
                    m_leftOffset += read;
                    m_leftOver = buf;
                }
                return read;
            }
        }
        /// <summary>
        /// Not supported.
        /// </summary>
        /// <param name="value"></param>
        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }
        /// <summary>
        /// Not supported.
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="origin"></param>
        /// <returns></returns>
        public override long Seek(long offset, SeekOrigin origin)
        {
            // this one could be done, but I didn't need it.
            throw new NotSupportedException();
        }
        /// <summary>
        /// No-op.
        /// </summary>
        public override void Flush()
        {
        }
        /// <summary>
        /// Clear out any pending data, and don't allow more reads or writes.
        /// This really isn't necessary or useful.
        /// </summary>
        public override void Close()
        {
            lock (m_readLock)
            {
                m_queue.Clear();
                m_leftOver = null;
                m_closed   = true;
                Monitor.Pulse(m_readLock);
            }
        }
        /// <summary>
        /// Not supported.
        /// </summary>
        public override long Position
        {
            set
            {
                throw new NotSupportedException();
            }
            get
            {
                throw new NotSupportedException();
            }
        }
        /// <summary>
        /// How many bytes can currently be read?
        /// </summary>
        public override long Length
        {
            get
            {
                lock (m_readLock)
                {
                    long count = (m_leftOver == null) ? 0 : m_leftOver.Length;
                    foreach (byte[] b in m_queue)
                    {
                        count += b.Length;
                    }
                    return count;
                }
            }
        }
        /// <summary>
        /// Is there data available to be read?
        /// </summary>
        public bool DataAvailable
        {
            get
            {
                return (m_leftOver != null) || (m_queue.Count > 0);
            }
        }
        /// <summary>
        /// True.
        /// </summary>
        public override bool CanWrite
        {
            get
            {
                return true;
            }
        }
        /// <summary>
        /// False.
        /// </summary>
        public override bool CanSeek
        {
            get
            {
                return false;
            }
        }
        /// <summary>
        /// True.
        /// </summary>
        public override bool CanRead
        {
            get
            {
                return true;
            }
        }
        #endregion
    }
}
