/* --------------------------------------------------------------------------
 * Copyrights
 * 
 * Portions created by or assigned to Cursive Systems, Inc. are 
 * Copyright (c) 2002-2004 Cursive Systems, Inc.  All Rights Reserved.  Contact
 * information for Cursive Systems, Inc. is available at
 * http://www.cursive.net/.
 *
 * License
 * 
 * Jabber-Net can be used under either JOSL or the GPL.  
 * See LICENSE.txt for details.
 * --------------------------------------------------------------------------*/
using System;

using System.Diagnostics;
using System.Threading;
using System.Xml;

using bedrock.io;
using bedrock.util;

namespace jabber.protocol
{
	/// <summary>
	/// Summary description for AsynchElementStream.
	/// </summary>
    [RCS(@"$Header$")]
    public class AsynchElementStream : ElementStream
	{
        private object m_parseLock = new object();

        /// <summary>
        /// Create an instance.
        /// </summary>
        public AsynchElementStream() : base(new PipeStream(true))
        {
        }

        /// <summary>
        /// Put bytes into the parser.
        /// If in synchronous mode, after storing a copy of the given buffer, 
        /// the parser is called in this thread.  Otherwise, an async wakeup
        /// is given to the parser.
        /// </summary>
        /// <param name="buf">The bytes to put into the parse stream</param>
        public void Push(byte[] buf)
        {
            Debug.Assert(m_stream != null);
            if (buf.Length > 0)
            {
                m_stream.Write(buf, 0, buf.Length);
                
                // heh.  I was thinking about doing this for scale,
                // but it also has the nice side-effect of getting
                // around any potential dead-locks here.
                ThreadPool.QueueUserWorkItem(new WaitCallback(Parse));
            }
        }

        /// <summary>
        /// Put bytes into the parser.
        /// If in synchronous mode, after storing a copy of the given buffer, 
        /// the parser is called in this thread.  Otherwise, an async wakeup
        /// is given to the parser.
        /// </summary>
        /// <param name="buf">The bytes to put into the parse stream</param>
        /// <param name="offset">Offset into buf to start at</param>
        /// <param name="length">Number of bytes to write</param>
        public void Push(byte[] buf, int offset, int length)
        {
            Debug.Assert(m_stream != null);
            if (length > 0)
            {
                m_stream.Write(buf, offset, length);
                ThreadPool.QueueUserWorkItem(new WaitCallback(Parse));
            }
        }

        /// <summary>
        /// Read one element from the pipe stream.  This may block if a partial
        /// protocol element has come in, but should pick back up when 
        /// HandleReceive is called again.
        /// </summary>
        private void Parse(object ignored)
        {
            lock (m_parseLock)
            {
                try
                {
                    PipeStream ps = m_stream as PipeStream;
                    Debug.Assert(ps != null);

                    while (!ps.IsEmpty)
                    {
                        // skip extraneous nonsense.  For the
                        // stream:stream tag, we read the whole start
                        // tag.  For other protocol elements, we read
                        // the entire tag, including children, in one
                        // gulp.  quit (don't block) if we get to the
                        // end of the nonsense between protocol tags.
                        ps.AutoClose = true;
                        bool more;
                        while (more = m_reader.Read())
                        {
                            if (m_reader.NodeType == XmlNodeType.Element)
                                break;
                        }
                        if (!more)
                        {
                            return;
                        }
                        // block when we're in a tag we care about.
                        ps.AutoClose = false;
                        switch (m_reader.Depth)
                        {
                            case 0:  // stream:stream
                                FireOnDocumentStart(m_loader.ReadStartTag());
                                break;

                            case 1:  // protocol element
                                FireOnElement((XmlElement) m_loader.ReadCurrentNode());
                                break;
                            default:
                                throw new InvalidOperationException("Protocol de-synchronized: " + m_reader.Name);
                        }
                        //TODO: detect the end of document, and call OnDocumentEnd();
                    }
                }
                catch(Exception e)
                {
                    Debug.WriteLine(e.ToString());
                    FireOnDocumentEnd();
                    m_stream.Close();
                    return;
                }
            }
        }
	}
}
