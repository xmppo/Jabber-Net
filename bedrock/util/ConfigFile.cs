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
using System.Xml;
using System.IO;
using System.Diagnostics;
using System.Collections;
using bedrock.util;
namespace bedrock.util
{
    /// <summary>
    /// XML configuration file manager.
    /// TODO: this still needs some work.
    /// </summary>
    [RCS(@"$Header$")]
    public class ConfigFile
    {
        private XmlDocument m_doc;
        private static Hashtable s_instances = new Hashtable();
        /// <summary>
        /// Singleton factory
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static ConfigFile GetInstance(string name)
        {
            ConfigFile inst = (ConfigFile) s_instances[name];
            if (inst == null)
            {
                lock (s_instances.SyncRoot)
                {
                    if (inst == null)
                    {
                        inst = new ConfigFile(name);
                        s_instances[name] = inst;
                    }
                }
            }
            return inst;
        }
        private ConfigFile(string name)
        {
            // Don't call Tracer from here!
            m_doc = new XmlDocument();
                        string d = Path.GetDirectoryName(System.Environment.GetCommandLineArgs()[0]);
            DirectoryInfo p;
            while (d != null)
            {
                FileInfo fi = new FileInfo(Path.Combine(d, name));
                if (fi.Exists)
                {
                    m_doc.Load(fi.FullName);
                    return;
                }
                p = fi.Directory.Parent;
                if (p == null)
                    break;
                d = p.FullName;
            }
            
            throw new FileNotFoundException(name);
        }
        /// <summary>
        /// Get the configuration file XML node associated 
        /// with a given XPath query.
        /// </summary>
        /// <param name="xpath"></param>
        /// <returns></returns>
        public XmlNode GetNode(string xpath)
        {
            return m_doc.SelectSingleNode(xpath);
            //ConfigFile f;
        }
        /// <summary>
        /// Get the configuration file XML nodes associated with a give XPath query
        /// </summary>
        /// <param name="xpath"></param>
        /// <returns></returns>
        public XmlNodeList GetNodes(string xpath)
        {
            return m_doc.SelectNodes(xpath);
        }
        /// <summary>
        /// Get the configuration file string associated 
        /// with a given XPath query, or null if not found.
        /// </summary>
        public string this[string xpath]
        {
            get
            {
                return this[xpath, null];
            }
        }
        /// <summary>
        /// Get the configuration file string associated 
        /// with a given XPath query, or defaultValue if not found.
        /// </summary>
        public string this[string xpath, string defaultValue]
        {
            get
            {  
                string val;  
                XmlNode n = m_doc.SelectSingleNode(xpath);
                if (n != null)
                {
                    val = n.InnerText;
                }
                else
                {
                    val = defaultValue;
                }
                return val;
            }
        }
    }
}
