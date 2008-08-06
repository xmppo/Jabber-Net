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
using System.Collections.Generic;

using bedrock.util;

namespace bedrock.collections
{
    /// <summary>
    /// A set of strings, backed into a BitArray.  Any given string that is inserted
    /// into any instance of a StringSet increases the size of all StringSets over time.
    /// </summary>
    [SVN(@"$Id$")]
    public class StringSet : IEnumerable, IEnumerable<string>, ICloneable
	{
        private BitArray m_bits = null;

        // List<T>.Add doesn't return an int.
        private static ArrayList s_strings = new ArrayList();
        private static Dictionary<string, int> s_bits = new Dictionary<string,int>();

        /// <summary>
        /// Create a new StringSet, which is empty, but sized for all strings seen so far.
        /// </summary>
        public StringSet()
        {
            m_bits = new BitArray(s_strings.Count);
        }

        /// <summary>
        /// Create a new set with the contents of another set.
        /// </summary>
        /// <param name="other"></param>
        public StringSet(StringSet other)
        {
            if (other != null)
                m_bits = (BitArray)other.m_bits.Clone();
        }

        /// <summary>
        /// Create a set with one string in it.
        /// </summary>
        /// <param name="str"></param>
        public StringSet(string str) : this()
        {
            if (str != null)
                this.Add(str);
        }

        /// <summary>
        /// Create a set containing all of the strings from the specified array.
        /// </summary>
        /// <param name="arr"></param>
        public StringSet(string[] arr) : this()
        {
            if (arr != null)
                Add(arr);
        }

        private static int GetStringValue(string s)
        {
            int val = -1;
            
            lock (s_bits)
            {
                if (!s_bits.TryGetValue(s, out val))
                {
                    s_bits[s] = val = s_strings.Add(s);
                }
            }
            return val;
        }

        /// <summary>
        /// Add a string to this set.  If it is already in the set, this is a no-op.
        /// </summary>
        /// <param name="s"></param>
        public void Add(string s)
        {
            int val = GetStringValue(s);
            if (val >= m_bits.Length)
                m_bits.Length = s_strings.Count;
            m_bits[val] = true;
        }

        /// <summary>
        /// Add all of the strings from the given set to this set.
        /// </summary>
        /// <param name="set"></param>
        public void Add(StringSet set)
        {
            // Lengthen this one to be able to hold everything in the other set, as well.
            m_bits.Length = set.m_bits.Length = Math.Max(m_bits.Length, set.m_bits.Length);
            m_bits.Or(set.m_bits);
        }

        /// <summary>
        /// Add all of the strings from the given array to this set.
        /// </summary>
        /// <param name="arr"></param>
        public void Add(string[] arr)
        {
            foreach (string s in arr)
                Add(s);
        }

        /// <summary>
        /// Remove the given string from this set.
        /// </summary>
        /// <param name="s"></param>
        public void Remove(string s)
        {
            int val = GetStringValue(s);
            if (val >= m_bits.Length)
                return;
            m_bits[val] = false;
        }

        /// <summary>
        /// Remove all of the strings from the given set from this set.
        /// </summary>
        /// <param name="set"></param>
        public void Remove(StringSet set)
        {
            m_bits.Length = set.m_bits.Length = Math.Max(m_bits.Length, set.m_bits.Length);
            // Not is destructive.  Stupid.
            BitArray os = (BitArray)set.m_bits.Clone();
            os.Not();
            m_bits.And(os);
        }

        /// <summary>
        /// Clear all of the strings from this set.
        /// </summary>
        public void Clear()
        {
            m_bits.SetAll(false);
        }

        /// <summary>
        /// Is this string in the set?
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public bool Contains(string s)
        {
            int val = GetStringValue(s);
            if (val >= m_bits.Length)
                return false;
            return m_bits[val];            
        }

        /// <summary>
        /// Gets or sets whether this string is in the set.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public bool this[string s]
        {
            get { return Contains(s); }
            set
            {
                if (value)
                    Add(s);
                else
                    Remove(s);
            }
        }

        /// <summary>
        /// How many strings are in the set?
        /// May be slower than you expect, at the moment.
        /// </summary>
        public int Count
        {
            get
            {
                int c = 0;
                foreach (bool b in m_bits)
                {
                    if (b)
                        c++;
                }
                return c;
            }
        }

        #region operators
        /// <summary>
        /// Add two StringSets together, returning a new set.
        /// </summary>
        /// <param name="one"></param>
        /// <param name="two"></param>
        /// <returns></returns>
        public static StringSet operator +(StringSet one, StringSet two)
        {
            StringSet n = new StringSet(one);
            n.Add(two);
            return n;
        }

        /// <summary>
        /// Returns a new set containing the contents of the first set as well as the
        /// other string.
        /// </summary>
        /// <param name="set"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        public static StringSet operator +(StringSet set, string str)
        {
            StringSet n = new StringSet(set);
            n.Add(str);
            return n;
        }

        /// <summary>
        /// Returns a new set containing everything from the first set that isn't in
        /// the second set.
        /// </summary>
        /// <param name="one"></param>
        /// <param name="two"></param>
        /// <returns></returns>
        public static StringSet operator -(StringSet one, StringSet two)
        {
            StringSet n = new StringSet(one);
            n.Remove(two);
            return n;
        }

        /// <summary>
        /// Returns a new set containing everything from the first except the specified
        /// string.
        /// </summary>
        /// <param name="set"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        public static StringSet operator -(StringSet set, string str)
        {
            StringSet n = new StringSet(set);
            n.Remove(str);
            return n;
        }

        /// <summary>
        /// Is this set equal to another one?
        /// Warning: this is about 32x slower than it should be.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if ((object)this == obj)
                return true;
            StringSet set = obj as StringSet;
            if (set == null)
                return false;

            // BitArray.Equals is useless.  
            // You'd also like to just compare the internal ints, 
            // but BitArray is sealed.  Thx, MS.

            // it's easiest to just stretch everythin out to the longest.
            int max = Math.Max(this.m_bits.Length, set.m_bits.Length);
            if (this.m_bits.Length != max)
                this.m_bits.Length = max;
            if (set.m_bits.Length != max)
                set.m_bits.Length = max;

            for (int i = 0; i < max; i++)
            {
                if (this.m_bits[i] != set.m_bits[i])
                    return false;
            }
            
            return true;
        }

        /// <summary>
        /// Hashcode for the current contents of the list.
        /// SLOW!
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            // Again, if I wrote a BitArray, it's GetHashCode would work, probably by
            // adding and rotating the ints.
            int hash = 0;
            foreach (string s in this)
            {
                hash ^= s.GetHashCode();
            }
            return hash;
        }
        #endregion

        /// <summary>
        /// Get all of the strings that are currently in the set.
        /// No guarantee of order.
        /// </summary>
        /// <returns></returns>
        public string[] GetStrings()
        {
            string[] ret = new string[this.Count];
            int i = 0;
            foreach (string s in this)
            {
                ret[i++] = s;
            }
            return ret;
        }

        /// <summary>
        /// All of the strings from the set, newline-separated.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            System.IO.StringWriter sw = new System.IO.StringWriter();
            for (int i = 0; i < m_bits.Length; i++)
            {
                if (m_bits[i])
                    sw.WriteLine(s_strings[i]);
            }
            return sw.ToString();
        }

        #region IEnumerable Members

        /// <summary>
        /// Enumerate over the strings in the set.
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator()
        {
            return new StringSetEnumerator(this);
        }

        #endregion

        #region IEnumerable<string> Members

        /// <summary>
        /// Enumerate over the strings in the set.
        /// </summary>
        /// <returns></returns>
        IEnumerator<string> IEnumerable<string>.GetEnumerator()
        {
            return new StringSetEnumerator(this);
        }

        #endregion

        #region ICloneable Members

        /// <summary>
        /// Return a copy of this set.
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return new StringSet(this);
        }

        #endregion

        private class StringSetEnumerator : IEnumerator<string>
        {
            StringSet m_set;
            int m_cur = -1;

            public StringSetEnumerator(StringSet set)
            {
                m_set = set;
            }

            #region IEnumerator<string> Members

            public string Current
            {
                get 
                {
                    if ((m_cur < 0) || (m_cur >= m_set.m_bits.Length))
                        throw new InvalidOperationException("Call to current outside of MoveNext");
                    return (string)StringSet.s_strings[m_cur];
                }
            }

            #endregion

            #region IDisposable Members

            public void Dispose()
            {
                m_set = null;
            }

            #endregion

            #region IEnumerator Members

            object IEnumerator.Current
            {
                get 
                {
                    return this.Current;
                }
            }

            public bool MoveNext()
            {
                while (++m_cur < m_set.m_bits.Length)
                {
                    if (m_set.m_bits[m_cur])
                        return true;
                }
                return false;
            }

            public void Reset()
            {
                m_cur = -1;
            }

            #endregion
        }
    }
}
