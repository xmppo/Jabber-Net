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
 * See LICENSE.txt for details.
 * --------------------------------------------------------------------------*/
using System;

using System.Collections;
using bedrock.util;

namespace bedrock.collections
{
    /// <summary>
    /// The different ways a set can be implemented.
    /// </summary>
    [SVN(@"$Id$")]
    public enum SetImplementation
    {
        /// <summary>
        /// Hash table.
        /// </summary>
        Hashtable,
        /// <summary>
        /// Red/Black tree.
        /// </summary>
        Tree,
        /// <summary>
        /// Skip List.
        /// </summary>
        SkipList
    }

    /// <summary>
    /// Set backed into a Tree.
    /// </summary>
    [SVN(@"$Id$")]
    public class Set : ISet
    {
        private static readonly object s_nothing = new object();

        private IDictionary m_dict;

        /// <summary>
        /// Creates a new, empty Set backed into a hash table.
        /// </summary>
        public Set()
        {
            m_dict = new Hashtable();
        }

        /// <summary>
        /// Creates a set with the given back-end implementation.
        /// </summary>
        /// <param name="impl">How to implement the set.</param>
        public Set(SetImplementation impl)
        {
            switch (impl)
            {
                case SetImplementation.Hashtable:
                    m_dict = new Hashtable();
                    break;
                case SetImplementation.Tree:
                    m_dict = new Tree();
                    break;
                case SetImplementation.SkipList:
                    m_dict = new SkipList();
                    break;
                default:
                    throw new NotImplementedException("Unknown SetImplementation");
            }
        }

        #region Implementation of ISet
        /// <summary>
        /// Adds an object to the set.
        /// </summary>
        /// <param name="o">The object to add</param>
        /// <exception cref="ArgumentException">object was already in the set.</exception>
        public void Add(object o)
        {
            if (!m_dict.Contains(o))
                m_dict.Add(o, s_nothing);
        }

        /// <summary>
        /// Removes the given object from the set.
        /// There is no exception thrown if the object is not in the set.
        /// </summary>
        /// <param name="o">The object to remove.</param>
        public void Remove(object o)
        {
            m_dict.Remove(o);
        }

        /// <summary>
        /// Removes all items from the set.
        /// </summary>
        public void Clear()
        {
            m_dict.Clear();
        }

        /// <summary>
        /// Determines if the given object is in the set.
        /// </summary>
        /// <param name="o">The object to search for.</param>
        /// <returns>True if the object is in the set.</returns>
        public bool Contains(object o)
        {
            return m_dict.Contains(o);
        }

        /// <summary>
        /// Returns a new collection that contains all of the items that
        /// are in this set or the other set.
        /// </summary>
        /// <param name="other">Second set to combine.</param>
        /// <returns>Combined set.</returns>
        public bedrock.collections.ISet Union(bedrock.collections.ISet other)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns a new collection that contains all of the items that
        /// are in this list *and* the other set.
        /// </summary>
        /// <param name="other">
        /// Other set to intersect with.
        /// </param>
        /// <returns>Combined set.</returns>
        public bedrock.collections.ISet Intersection(bedrock.collections.ISet other)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Implementation of ICollection
        /// <summary>
        /// Copies the elements of the ICollection to an Array, starting at a particular Array index.
        /// </summary>
        /// <param name="array">The array to copy.</param>
        /// <param name="index">The index to start at.</param>
        public void CopyTo(System.Array array, int index)
        {
            int count = index;
            foreach (object o in this)
            {
                array.SetValue(o, count);
                count++;
            }
        }

        /// <summary>
        /// Gets a value indicating whether access to the ICollection is synchronized (thread-safe).
        /// </summary>
        public bool IsSynchronized
        {
            get { return m_dict.IsSynchronized; }
        }

        /// <summary>
        /// Gets the number of items in the set.
        /// </summary>
        public int Count
        {
            get { return m_dict.Count; }
        }

        /// <summary>
        /// Gets an object that can be used to synchronize access to the ICollection.
        /// </summary>
        public object SyncRoot
        {
            get { return m_dict.SyncRoot; }
        }
        #endregion

        #region Implementation of IEnumerable
        /// <summary>
        /// Returns an enumerator that iterates through all items in the set.
        /// </summary>
        /// <returns>An IEnumerator for the entire set.</returns>
        public System.Collections.IEnumerator GetEnumerator()
        {
            return new TreeSetEnumerator(m_dict);
        }
        #endregion

        private class TreeSetEnumerator : IEnumerator
        {
            private IEnumerator m_enum;

            public TreeSetEnumerator(IDictionary e)
            {
                m_enum = e.GetEnumerator();
            }

            #region Implementation of IEnumerator
            public void Reset()
            {
                m_enum.Reset();
            }

            public bool MoveNext()
            {
                return m_enum.MoveNext();
            }

            public object Current
            {
                get
                {
                    DictionaryEntry entry = (DictionaryEntry) m_enum.Current;
                    return entry.Key;
                }
            }
            #endregion
        }
    }
}
