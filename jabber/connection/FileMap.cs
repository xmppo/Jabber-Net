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
 * Jabber-Net can be used under either JOSL or the GPL.
 * See LICENSE.txt for details.
 * --------------------------------------------------------------------------*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;

using jabber.protocol;

namespace jabber.connection
{
    /// <summary>
    /// A dictionary backed into a file.  Any modification to the dictionary re-writes the file, so 
    /// writes are somewhat costly.  Reads are cached lazily.
    /// </summary>
	public class FileMap<T>
        where T : Element
	{
        private const string NS = "http://cursive.net/xml/FileMap";

        private string m_fileName = null;
        private ElementFactory m_factory = null;
        private Dictionary<string, T> m_cache = null;

        /// <summary>
        /// Create a file map.
        /// </summary>
        /// <param name="fileName">Valid file name, either absoulte, or relative 
        /// to the current working directory.</param>
        /// <param name="factory">Element factory to create elements of a given type.  If null,
        /// Elements will always be created, and T MUST be Element.</param>
        public FileMap(string fileName, ElementFactory factory)
        {
            Factory = factory;
            FileName = fileName;
        }

        /// <summary>
        /// The ElementFactory that determines the type of the elements being stored.
        /// </summary>
        public ElementFactory Factory
        {
            get { return m_factory; }
            set
            {
                if (value == null)
                {
                    if (!typeof(T).Equals(typeof(Element)))
                        throw new ArgumentException("Factory must not be null if type is not XmlElement", "factory");
                    m_factory = new ElementFactory();
                }
                else
                    m_factory = value;
            }
        }

        /// <summary>
        /// The name of the file to manage.
        /// </summary>
        public string FileName
        {
            get { return m_fileName; }
            set 
            {
                if (value == null)
                    throw new ArgumentNullException("fileName");
                if (value == "")
                    throw new ArgumentOutOfRangeException("fileName");

                lock (this)
                {
                    if (value == null)
                        throw new ArgumentNullException("FileName");
                    if (m_fileName == value)
                        return;
                    m_fileName = value;
                    Read();
                }
            }
        }

        private void Flush()
        {
            Debug.Assert(m_cache != null, "m_cache should be initialized in constructor");

            XmlDocument doc = new XmlDocument();
            XmlElement fm = doc.CreateElement("fm", NS);
            doc.AppendChild(fm);

            XmlElement val;
            lock (this)
            {
                foreach (KeyValuePair<string, T> kv in m_cache)
                {
                    val = doc.CreateElement("val", NS);
                    val.SetAttribute("name", kv.Key);
                    if (kv.Value != null)
                        val.AppendChild(doc.ImportNode(kv.Value, true));
                    fm.AppendChild(val);
                }
            }
            XmlWriter xw = new XmlTextWriter(m_fileName, System.Text.Encoding.UTF8);
            doc.WriteTo(xw);
            xw.Close();
        }

        private void Read()
        {
            lock (this)
            {
                m_cache = new Dictionary<string, T>();
                try
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(m_fileName);
                    XmlElement fm = doc.DocumentElement;
                    foreach (XmlElement val in fm.GetElementsByTagName("val"))
                    {
                        string name = val.GetAttribute("name");
                        if (name == "")
                            continue;
                        foreach (XmlNode child in val.ChildNodes)
                        {
                            if (child is XmlElement)
                            {
                                m_cache[name] = (T)Element.AddTypes((XmlElement)child, m_factory);
                                break;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("WARNING: " + ex.ToString());
                }
            }
        }

        /// <summary>
        /// Remove the specified key and value
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Remove(string key)
        {
            bool ret = m_cache.Remove(key);
            if (ret)
                Flush();
            return ret;
        }

        /// <summary>
        /// Get or set the XmlElement associated with the given key.
        /// If the key already has a value, it WILL NOT be overridden; you 
        /// MUST call Clear or Remove first.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public T this[string key]
        {
            get
            {
                T val = null;
                if (m_cache.TryGetValue(key, out val))
                    return val;
                return null;
            }
            set
            {
                if (m_cache.ContainsKey(key))
                    return;
                m_cache[key] = value;
                Flush();
            }
        }

        /// <summary>
        /// How many key/value pairs are stored?
        /// </summary>
        public int Count
        {
            get { return m_cache.Count; }
        }

        /// <summary>
        /// Clear all stored keys/values.
        /// </summary>
        public void Clear()
        {
            if (Count == 0)
                return;
            m_cache.Clear();
            Flush();
        }

        /// <summary>
        /// Is the given key in the map?
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Contains(string key)
        {
            return m_cache.ContainsKey(key);
        }
    }
}
