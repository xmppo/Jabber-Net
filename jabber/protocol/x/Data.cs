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
using System.Xml;

using bedrock.util;

namespace jabber.protocol.x
{
    /// <summary>
    /// XData types.
    /// </summary>
    [RCS(@"$Header$")]
    public enum XDataType
    {
        /// <summary>
        /// This packet contains a form to fill out. Display it to the user (if your program can).
        /// </summary>
        form,
        /// <summary>
        /// The form is filled out, and this is the data that is being returned from the form.
        /// </summary>
        submit,
        /// <summary>
        /// Data results being returned from a search, or some other query.
        /// </summary>
        result
    }

	/// <summary>
	/// jabber:x:data support, as in http://www.jabber.org/jeps/jep-0004.html.
	/// </summary>
	[RCS(@"$Header$")]
    public class Data : Element
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="doc"></param>
        public Data(XmlDocument doc) : base("x", URI.XDATA, doc)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public Data(string prefix, XmlQualifiedName qname, XmlDocument doc) :
            base(prefix, qname, doc)
        {
        }

        
        /// <summary>
        /// Form instructions.
        /// </summary>
        public string Instructions
        {
            get { return GetElem("instructions"); }
            set { SetElem("instructions", value); }
        }
        
        /// <summary>
        /// The form title, for display at the top of a window.
        /// </summary>
        public string Title
        {
            get { return GetElem("title"); }
            set { SetElem("title", value); }
        }

        /// <summary>
        /// Type of this XData.
        /// </summary>
        public XDataType Type
        {
            get { return (XDataType)GetEnumAttr("type", typeof(XDataType)); }
            set { SetAttribute("type", value.ToString());}
        }

        /// <summary>
        /// List of form fields
        /// </summary>
        /// <returns></returns>
        public Field[] GetFields()
        {
            XmlNodeList nl = GetElementsByTagName("field", URI.XDATA);
            Field[] fields = new Field[nl.Count];
            int i=0;
            foreach (XmlNode n in nl)
            {
                fields[i] = (Field) n;
                i++;
            }
            return fields;
        }

        /// <summary>
        /// Add a form field
        /// </summary>
        /// <returns></returns>
        public Field AddField()
        {
            Field f = new Field(this.OwnerDocument);
            AddChild(f);
            return f;
        }

        /// <summary>
        /// Get a field with the specified variable name.
        /// </summary>
        /// <param name="var"></param>
        /// <returns></returns>
        public Field GetField(string var)
        {
            XmlNodeList nl = GetElementsByTagName("field", URI.XDATA);
            foreach (XmlNode n in nl)
            {
                Field f = (Field) n;
                if (f.Var == var)
                    return f;
            }
            return null;
        }
    }

    /// <summary>
    /// Types of fields.  This enum doesn't exactly match the JEP, 
    /// since most of the field types aren't valid identifiers in C#.
    /// </summary>
    [RCS(@"$Header$")]
    public enum FieldType
    {
        /// <summary>
        /// Single-line text, and default.
        /// </summary>
        text_single,
        /// <summary>
        /// Password-style single line text.  Text obscured by *'s.
        /// </summary>
        text_private,
        /// <summary>
        /// Multi-line text
        /// </summary>
        text_multi,
        /// <summary>
        /// Multi-select list
        /// </summary>
        list_multi,
        /// <summary>
        /// Single-select list
        /// </summary>
        list_single,
        /// <summary>
        /// Checkbox
        /// </summary>
        boolean,
        /// <summary>
        /// Fixed text.
        /// </summary>
        Fixed,
        /// <summary>
        /// Hidden field.  Value is returned to sender as sent.
        /// </summary>
        hidden,
        /// <summary>
        /// Jabber ID.
        /// </summary>
        jid
    }

    /// <summary>
    /// Form field.
    /// </summary>
    [RCS(@"$Header$")]
    public class Field : Element
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="doc"></param>
        public Field(XmlDocument doc) : base("field", URI.XDATA, doc)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public Field(string prefix, XmlQualifiedName qname, XmlDocument doc) :
            base(prefix, qname, doc)
        {
        }    

        /// <summary>
        /// Field type.
        /// </summary>
        public FieldType Type
        {
            get 
            {
                switch (GetAttribute("type"))
                {
                    case "text-single":
                        return FieldType.text_single;
                    case "text-private":
                        return FieldType.text_private;
                    case "text-multi":
                        return FieldType.text_multi;
                    case "list-multi": 
                        return FieldType.list_multi;
                    case "list-single":
                        return FieldType.list_single;
                    case "boolean":
                        return FieldType.boolean;
                    case "fixed": 
                        return FieldType.Fixed;
                    case "hidden":
                        return FieldType.hidden;
                    case "jid": 
                        return FieldType.jid;
                    default:
                        throw new ArgumentException("Unknown x:data field type: " + GetAttribute("type"));
                }
            }
            set 
            {
                switch (value)
                {
                    case FieldType.text_single:
                        SetAttribute("type", "text-single");
                        break;
                    case FieldType.text_private:
                        SetAttribute("type", "text-private");
                        break;
                    case FieldType.text_multi:
                        SetAttribute("type", "text-multi");
                        break;
                    case FieldType.list_multi:
                        SetAttribute("type", "list-multi");
                        break;
                    case FieldType.list_single:
                        SetAttribute("type", "list-single");
                        break;
                    case FieldType.boolean:
                        SetAttribute("type", "boolean");
                        break;
                    case FieldType.Fixed:
                        SetAttribute("type", "fixed");
                        break;
                    case FieldType.hidden:
                        SetAttribute("type", "hidden");
                        break;
                    case FieldType.jid:
                        SetAttribute("type", "jid");
                        break;
                    default:
                        throw new ArgumentException("Unknown x:data field type: " + value);
                }
            }
        }

        /// <summary>
        /// Field label.  Will return Var if no label is found.
        /// </summary>
        public string Label
        {
            get 
            {
                string lbl = GetAttribute("label");
                if (lbl == null)
                    lbl = Var;
                return lbl;
            }
            set { SetAttribute("label", value); }
        }

        /// <summary>
        /// Field variable name.
        /// </summary>
        public string Var
        {
            get { return GetAttribute("var"); }
            set { SetAttribute("var", value); }
        }

        /// <summary>
        /// Is this a required field?
        /// </summary>
        public bool IsRequired
        {
            get { return this["required"] != null; }
            set 
            { 
                if (value)
                    this.SetElem("required", null);
                else
                    this.RemoveElem("required");
            }
        }

        /// <summary>
        /// The field value.
        /// </summary>
        public string Val
        {
            get { return GetElem("value"); }
            set { SetElem("value", value); }
        }

        /// <summary>
        /// Value for type='boolean' fields
        /// </summary>
        public bool BoolVal
        {
            get 
            {
                string sval = Val;
                return !((sval == null) || (sval == "0"));
            }
            set
            {
                Val = value ? "1" : "0";
            }
        }

        /// <summary>
        /// Values for type='list-multi' fields
        /// </summary>
        public string[] Vals
        {
            get
            {
                XmlNodeList nl = GetElementsByTagName("value", URI.XDATA);
                string[] results = new string[nl.Count];
                int i=0;
                foreach (XmlElement el in nl)
                {
                    results[i++] = el.InnerText;
                }
                return results;
            }
            set
            {
                RemoveElems("value", URI.XDATA);
                foreach (string s in value)
                {
                    XmlElement val = this.OwnerDocument.CreateElement("value", URI.XDATA);
                    val.InnerText = s;
                    this.AppendChild(val);
                }
            }
        }

        /// <summary>
        /// The field description
        /// </summary>
        public string Desc
        {
            get { return GetElem("desc"); }
            set { SetElem("desc", value); }
        }

        /// <summary>
        /// List of field options
        /// </summary>
        /// <returns></returns>
        public Option[] GetOptions()
        {
            XmlNodeList nl = GetElementsByTagName("option", URI.XDATA);
            Option[] options = new Option[nl.Count];
            int i=0;
            foreach (XmlNode n in nl)
            {
                options[i] = (Option) n;
                i++;
            }
            return options;
        }

        /// <summary>
        /// Add a field option
        /// </summary>
        /// <returns></returns>
        public Option AddOption()
        {
            Option o = new Option(this.OwnerDocument);
            AddChild(o);
            return o;
        }
    }

    /// <summary>
    /// Field options, for list-single and list-multi type fields.
    /// </summary>
    [RCS(@"$Header$")]
    public class Option : Element
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="doc"></param>
        public Option(XmlDocument doc) : base("option", URI.XDATA, doc)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public Option(string prefix, XmlQualifiedName qname, XmlDocument doc) :
            base(prefix, qname, doc)
        {
        }    

        /// <summary>
        /// Option label
        /// </summary>
        public string Label
        {
            get { return GetAttribute("label"); }
            set { SetAttribute("label", value); }
        }

        /// <summary>
        /// The option value.
        /// </summary>
        public string Val
        {
            get { return GetElem("value"); }
            set { SetElem("value", value); }
        }    
    }
}
