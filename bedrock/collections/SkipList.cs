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
using System.Collections;
using System.Diagnostics;

using bedrock.util;

namespace bedrock.collections
{
    /// <summary>
    /// Summary description for SkipList.
    /// </summary>
    [RCS(@"$Header$")]
    public class SkipList : IEnumerable, IDictionary
    {
        /// <summary>
        /// The default probability for adding new node levels.
        /// .25 provides a good balance of speed and size.
        /// .5 will be slightly less variable in run time, 
        /// and take up more space
        /// </summary>
        public const float DEFAULT_PROBABILITY = 0.25F;

        /// <summary>
        /// The maximum depth for searching.
        /// log(1/p, n), where n is the max number of 
        /// expected nodes.  For the defaults, n = 4096.
        /// The list will continue to work for larger lists,
        /// but performance will start to degrade toward
        /// that of a linear search to further you get
        /// above n.
        /// TODO: automatically reset max_level when Length
        /// goes above n.
        /// </summary>
        public const int DEFAULT_MAX_LEVEL = 6;

        private float        m_probability;
        private int          m_max_level = DEFAULT_MAX_LEVEL;
        private SkipListNode m_header;
        private Random       m_rand = new Random();
        private IComparer    m_comparator = System.Collections.Comparer.Default;
        private int          m_count = 0;

        /// <summary>
        /// Create a skiplist with the default probability (0.25).
        /// </summary>
        public SkipList() : this(DEFAULT_PROBABILITY, DEFAULT_MAX_LEVEL)
        {
        }

        /// <summary>
        /// Create a skiplist with the default max_level.
        /// </summary>
        /// <param name="probability">Probability of adding a new level</param>
        public SkipList(float probability) : this(probability, DEFAULT_MAX_LEVEL)
        {
        }

        /// <summary>
        /// Create a skiplist.
        /// </summary>
        /// <param name="probability">Probability of adding a new level</param>
        /// <param name="max_level">Highest level in the list</param>
        public SkipList(float probability, int max_level)
        {
            m_probability = probability;
            m_max_level = max_level;
            m_header = new SkipListNode(1, new Ali(), null);
        }

        /// <summary>
        /// The current number of elements in the list.
        /// </summary>
        public int Count
        {
            get { return m_count; }
        }

        /// <summary>
        /// Add an item to the list.
        /// </summary>
        /// <param name="key">Key for later retrieval.  
        /// Must implement IComparable.</param>
        /// <param name="val">The value to store</param>
        /// <exception cref="ArgumentException">Thrown if the same key is added twice</exception>
        /// <exception cref="ArgumentNullException">Thrown if key is null</exception>
        public void Add(object key, object val)
        {
            if (key == null)
                throw new ArgumentNullException("key");

            SkipListNode update = new SkipListNode(m_max_level);
            SkipListNode n = m_header;
            SkipListNode next;

            for (int i=m_header.Level-1; i>=0; i--)
            {
                next = n[i];
                while ((next != null) &&
                       (m_comparator.Compare(next.Key, key) < 0))
                {
                    n = next;
                    next = n[i];
                }
                update[i] = n;
            }
            if ((n.Level > 0) && 
                (n[0] != null) && 
                (m_comparator.Compare(n[0].Key, key) == 0))
            { // already here
                //n.Value = val;
                throw new ArgumentException("Can't add the same key twice", "key");
            }
            else
            { // need to insert
                int level = RandomLevel();
                int s = m_header.Level;
                if (level > s)
                {
                    // this shouldn't happen any more.
                    //Debug.Assert(false);
                    m_header.Level = level;
                    for (int i=s; i<level; i++)
                    {
                        update[i] = m_header;
                    }
                }

                n = new SkipListNode(level, key, val);
                for (int i=0; i<level; i++)
                {
                    n[i] = update[i][i];
                    update[i][i] = n;
                }
                m_count++;
            }
        }

        /// <summary>
        /// Is the given key found in the tree?
        /// </summary>
        /// <param name="key">The key to search for</param>
        /// <returns></returns>
        public bool Contains(object key)
        {
            return GetNode(key) != null;
        }

        /// <summary>
        /// Lookup the key, and return the corresponding value, or null if not found.
        /// </summary>
        public object this[object key]
        {
            get
            {
                SkipListNode n = GetNode(key);
                if (n == null)
                    return null;
                return n.Value;
            }
            set
            {
                Add(key, value);
            }
        }

        /// <summary>
        /// Remove all of the items from the list.
        /// </summary>
        public void Clear()
        {
            m_header = new SkipListNode(1, new Ali(), null);
            m_count = 0;
        }

        /// <summary>
        /// Remove the item associated with this key from the list.
        /// </summary>
        /// <param name="key">Object that implements IComparable</param>
        public void Remove(object key)
        {
            if (key == null)
                throw new ArgumentNullException("key");

            SkipListNode update = new SkipListNode(m_max_level);
            SkipListNode n = m_header;
            SkipListNode next;

            for (int i=m_header.Level-1; i>=0; i--)
            {
                next = n[i];
                while ((next != null) &&
                       (m_comparator.Compare(next.Key, key) < 0))
                {
                    n = next;
                    next = n[i];
                }
                update[i] = n;
            }   
            if (n.Level == 0)
                return; // or assert

            n = n[0];
            if ((n == null) ||
                (m_comparator.Compare(n.Key, key) != 0))
            { // not found
                return;  // or assert
            }

            for (int i=0; i<m_header.Level; i++)
            {
                if (update[i][i] != n)
                    break;
                update[i][i] = n[i];
            }
            // TODO: reset m_header level
            m_count--;
        }

        /// <summary>
        /// Returns false, for now.
        /// </summary>
        public bool IsFixedSize
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Returns false, for now.
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// All of the keys of the list.
        /// </summary>
        public System.Collections.ICollection Keys
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// All of the values of the list.
        /// </summary>
        public System.Collections.ICollection Values
        {
            get
            {
                object[] vals = new object[m_count];
                CopyTo(vals, 0);
                return vals;
            }
        }

        /// <summary>
        /// Iterate over the list
        /// </summary>
        /// <returns></returns>
        public IDictionaryEnumerator GetEnumerator()
        {
            return new SkipListEnumerator(this);
        }

        #region IEnumerable
        /// <summary>
        /// Iterate over the list
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return new SkipListEnumerator(this);
        }
        #endregion

        /// <summary>
        /// Copy the *values* from this list to the given array.
        /// It's not clear from the .Net docs wether this should be
        /// entries or values, so I chose values.
        /// </summary>
        /// <param name="array">The array to copy into</param>
        /// <param name="arrayIndex">The index to start at</param>
        public void CopyTo(System.Array array, int arrayIndex)
        {
            if (array == null)
                throw new ArgumentNullException("array");
            if (array.Rank != 1)
                throw new ArgumentException("Array must be single dimensional", "array");
            if (arrayIndex < 0) 
                throw new ArgumentOutOfRangeException("arrayIndex", "starting index may not be negative");
            if (array.Length - arrayIndex < m_count)
                throw new ArgumentException("Array too small", "array");
        
            int count = arrayIndex;
            foreach (DictionaryEntry e in this)
            {
                array.SetValue(e.Value, count++);
            }
        }

        /// <summary>
        /// Returns false, for now
        /// </summary>
        public bool IsSynchronized
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Not implemented, yet.
        /// </summary>
        public object SyncRoot
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        private SkipListNode GetNode(object key)
        {
            if (key == null)
                throw new ArgumentNullException("key");

            if (m_count == 0)
                return null;

            SkipListNode n = m_header;
            SkipListNode next;

            for(int i=m_header.Level-1; i>=0; i--) 
            {
                next = n[i];
                while((next != null) &&
                    (m_comparator.Compare(next.Key, key) < 0))
                {
                    n = next;
                    next = n[i];
                }
            }

            // n should always be level > 0, now.
            n = n[0];

            if( (n != null) && (m_comparator.Compare(n.Key, key) == 0))
                return n;
            else
                return null;
        }

        private int RandomLevel()
        {
            int level = 1;
            while ((level < m_max_level-1) && (m_rand.NextDouble() < m_probability))
            {
                level++;
            }

            return level;
        }

        private class SkipListNode
        {
            private SkipListNode[] m_next;
            private object m_key;
            private object m_value;

            public SkipListNode(int level) : this(level, null, null)
            {
            }

            public SkipListNode(int level, object key, object val)
            {
                m_next  = new SkipListNode[level];
                for (int i=0; i<level; i++)
                    m_next[i] = null;
                m_key   = key;
                m_value = val;
            }

            public SkipListNode this[int i]
            {
                get { return m_next[i]; }
                set { m_next[i] = value; }
            }

            public int Level
            {
                get { return m_next.Length; }
                set 
                {
                    Debug.Assert(value > m_next.Length);
                    SkipListNode[] n = new SkipListNode[value];
                    Array.Copy(m_next, 0, n, 0, m_next.Length);
                    for (int i=m_next.Length; i<value; i++)
                    {
                        n[i] = null;
                    }
                    m_next = n;
                }
            }

            public object Key
            {
                get { return m_key; }
                set { m_key = value; }
            }

            public object Value
            {
                get { return m_value; }
                set { m_value = value; }
            }
        }

        /// <summary>
        /// An object that is the greatest.
        /// </summary>
        private class Ali : IComparable
        {
            int IComparable.CompareTo(object obj)
            {
                return 1;
            }
        }

        private class SkipListEnumerator : IDictionaryEnumerator
        {
            private SkipList     m_list;
            private SkipListNode m_node;

            public SkipListEnumerator(SkipList list)
            {
                m_list = list;
                m_node = m_list.m_header;
            }

            public bool MoveNext()
            {
                Debug.Assert(m_node != null);

                m_node = m_node[0];
                return m_node != null;
            }

            public void Reset()
            {
                m_node = m_list.m_header;
            }

            public object Current
            {
                get
                {
                    if (m_node == m_list.m_header)
                        throw new InvalidOperationException("Call MoveNext, first");
                    return Entry;
                }
            }

            public System.Collections.DictionaryEntry Entry
            {
                get
                {
                    return new System.Collections.DictionaryEntry(m_node.Key, m_node.Value);
                }
            }

            public object Key
            {
                get
                {
                    return m_node.Key;
                }
            }

            public object Value
            {
                get
                {
                    return m_node.Value;
                }
            }
        }
    }
}
