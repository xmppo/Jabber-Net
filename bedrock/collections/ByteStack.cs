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
 * Copyright (c) 2002-2004 Cursive Systems, Inc.  All Rights Reserved.  Contact
 * information for Cursive Systems, Inc. is available at http://www.cursive.net/.
 *
 * Portions Copyright (c) 2002-2004 Joe Hildebrand.
 * 
 * Acknowledgements
 * 
 * Special thanks to the Jabber Open Source Contributors for their
 * suggestions and support of Jabber.
 * 
 * --------------------------------------------------------------------------*/
using System;
using bedrock.util;
namespace bedrock.collections
{
    /// <summary>
    /// A type-safe stack for bytes, implemented as a growable  
    /// buffer.
    /// </summary>
    [RCS(@"$Header$")]
    public class ByteStack
    {
        private const double GROWTH_FACTOR = 1.5d;
        private const int    DEFAULT_SIZE  = 16;
        private const int    MIN_SIZE      = 2;
        private static readonly System.Text.Encoding ENCODING = System.Text.Encoding.Default;
        private int    m_count    = 0;
        private int    m_capacity = 0;
        private byte[] m_buffer;
        /// <summary>
        /// Create an instance with the default initial capacity.
        /// </summary>
        public ByteStack() : this(DEFAULT_SIZE)
        {
        }
        /// <summary>
        /// Create an instance with the given initial capacity.
        /// </summary>
        /// <param name="initialSize">The initial capacity</param>
        public ByteStack(int initialSize)
        {
            if (initialSize < MIN_SIZE)
            {
                initialSize = DEFAULT_SIZE;
            }
            m_capacity = initialSize;
            m_buffer   = new byte[m_capacity];
        }
        /// <summary>
        /// Create an instance with the given initial value.  The initial size
        /// will be grown from the size of the given bytes.  A copy is made of 
        /// the given bytes.
        /// </summary>
        /// <param name="start"> </param>
        public ByteStack(byte[] start)
        {
            m_count  = m_capacity = start.Length;
            m_buffer = start;
            IncreaseSize();
        }
        /// <summary>
        /// Increase the size of the stack by GROWTH_FACTOR times.
        /// </summary>
        private void IncreaseSize()
        {
            m_capacity = (int) (m_capacity * GROWTH_FACTOR);
            // if the size is 1, we'll never get bigger.
            if (m_capacity < MIN_SIZE)
            {
                m_capacity = DEFAULT_SIZE;
            }
            byte[] newBuf = new byte[m_capacity];
            Buffer.BlockCopy(m_buffer, 0, newBuf, 0, m_count);
            m_buffer = newBuf;
        }
        /// <summary>
        /// How may bytes are currently on the stack?
        /// </summary>
        public int Count
        {
            get 
            {
                return m_count;
            }
        }
        /// <summary>
        /// How many bytes can the stack currently hold?
        /// </summary>
        public int Capacity
        {
            get 
            {
                return m_capacity;
            }
        }
        /// <summary>
        /// Push a byte onto the stack.
        /// </summary>
        /// <param name="b"> </param>
        public void Push(byte b)
        {
            if (m_count >= m_capacity)
            {
                IncreaseSize();
            }
            m_buffer[m_count] = b;
            m_count++;
        }
        /// <summary>
        /// Pop a byte off of the stack.
        /// </summary>
        public byte Pop()
        {
            if (m_count <= 0)
            {
                throw new InvalidOperationException("Empty stack");
            }
            m_count--;
            byte b =  m_buffer[m_count];
            return b;
        }
        /// <summary>
        /// Non-destructively read the byte on the top of the stack.
        /// </summary>
        public byte Peek()
        {
            if (m_count <= 0)
            {
                throw new InvalidOperationException("Empty stack");
            }
            return m_buffer[m_count - 1];
        }
        /// <summary>
        /// Convert to byte[] by making a trimmed copy.
        /// </summary>
        /// <param name="bs"> </param>
        public static implicit operator byte[](ByteStack bs)
        {
            if (bs.m_count == 0)
            {
                return new byte[0];
            }
            byte[] newBuf = new byte[bs.m_count];
            Buffer.BlockCopy(bs.m_buffer, 0, newBuf, 0, bs.m_count);
            return newBuf;
        }
        /// <summary>
        /// Convert to a string, using the default encoding.  This is probably not
        /// right, but it's really nice for debugging and testing.
        /// </summary>
        public override string ToString()
        {
            return ENCODING.GetString(m_buffer, 0, m_count);
        }
    }
}
