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
using System.Collections.Generic;
using System.Threading;
using System.Reflection;
using System.Diagnostics;

using bedrock.util;

namespace jabber.protocol
{
    /// <summary>
    /// How should the marked-up entity be rendered in XML?  Only used
    /// for enums that are going to be put in attributes at the moment.
    /// TODO: support namespaces, use for element definitions.
    /// </summary>
    [SVN(@"$Id$")]
    [AttributeUsage(AttributeTargets.Field)]
    public class XMLAttribute : Attribute
    {
        private string m_name;

        /// <summary>
        /// Create
        /// </summary>
        /// <param name="name"></param>
        public XMLAttribute(string name)
        {
            m_name = name;
        }

        /// <summary>
        /// The string to use when converting to and from XML.
        /// </summary>
        public string Name
        {
            get { return m_name; }
        }
    }

    /// <summary>
    /// Parse enums
    /// </summary>
    [SVN(@"$Id$")]
    public class EnumParser
	{
        private static Dictionary<Type, Dictionary<string, object>> s_vals = 
            new Dictionary<Type, Dictionary<string, object>>();

        private static Dictionary<Type, Dictionary<object, string>> s_strings =
            new Dictionary<Type, Dictionary<object, string>>();

        private static bool IsDash(Type t)
        {
            object[] da = t.GetCustomAttributes(typeof(DashAttribute), false);
            return (da.Length > 0);
        }

        private static Dictionary<string, object> GetValHash(Type t)
        {
            Dictionary<string, object> map = null;
            if (!s_vals.TryGetValue(t, out map))
            {
                s_vals[t] = map = new Dictionary<string, object>();
                bool dash = IsDash(t);

                FieldInfo[] fields = t.GetFields(BindingFlags.Public | BindingFlags.Static);
                foreach (FieldInfo fi in fields)
                {
                    object[] attrs = fi.GetCustomAttributes(typeof(XMLAttribute), false);
                    object val = fi.GetValue(null);
                    if (attrs.Length > 0)
                    {
                        string name = ((XMLAttribute)attrs[0]).Name;
                        map[name] = val;
                    }
                    if (dash)
                        map[fi.Name.Replace("_", "-")] = val;
                    else
                        map[fi.Name] = val;
                }
            }
            return map;
        }

        private static Dictionary<object, string> GetStringHash(Type t)
        {
            Dictionary<object, string> map = null;
            string name;

            if (!s_strings.TryGetValue(t, out map))
            {
                s_strings[t] = map = new Dictionary<object, string>();

                bool dash = IsDash(t);

                FieldInfo[] fields = t.GetFields(BindingFlags.Public | BindingFlags.Static);
                foreach (FieldInfo fi in fields)
                {
                    object[] attrs = fi.GetCustomAttributes(typeof(XMLAttribute), false);
                    object val = fi.GetValue(null);
                    if (attrs.Length > 0)
                        name = ((XMLAttribute)attrs[0]).Name;
                    else
                    {
                        if (dash)
                            name = fi.Name.Replace('_', '-');
                        else
                            name = fi.Name;
                    }
                    map[val] = name;
                }
            }
            return map;
        }

        /// <summary>
        /// Parse a string into an enum value for the given type T.  
        /// Any errors map to -1.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T Parse<T>(string value)
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException("Type must be enum");

            Dictionary<string, object> map = GetValHash(typeof(T));
            object val = null;
            if (!map.TryGetValue(value, out val))
                return (T)(object)(-1);
            return (T)val;
        }

        /// <summary>
        /// Parse a string into an enum value for the given type.  
        /// Any errors map to -1.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static object Parse(string value, Type t)
        {
            if (!t.IsEnum)
                throw new ArgumentException("Type must be enum");

            Dictionary<string, object> map = GetValHash(t);
            object val = null;
            if (!map.TryGetValue(value, out val))
                return (object)(-1);
            return val;
        }

        /// <summary>
        /// Convert an enum value into its string representation.
        /// any -1 value gets mapped to null.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToString(object value)
        {
            Type t = value.GetType();
            if (!t.IsEnum)
                throw new ArgumentException("Type must be enum");

            if ((int)value == -1)
                return null;

            Dictionary<object, string> map = GetStringHash(t);
            string val = null;
            bool found = map.TryGetValue(value, out val);
            Debug.Assert(found, "Tried to convert an unknown enum value to string");
            return val;
        }
	}
}
