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
using System.Xml;

using bedrock.util;
using jabber.protocol;

namespace jabber.protocol.stream
{
    [Dash]
    public enum ConditionType
    {
        UNSPECIFIED = -1,
        host_gone,
        host_unknown,
        improper_addressing,
        internal_server_error,
        remote_connection_failed,
        remote_stream_error,
        see_other_uri,
        system_shutdown,
        undefined_condition
    };

    public enum BodyType
    {
        UNSPECIFIED = -1,
        error,
        terminate
    };
    
    [SVN(@"$id$")]
    public class Body : Element
    {
        
        public Body(XmlDocument doc) : base("body", URI.HTTP_BIND, doc)
        {
        }

        public Body(string prefix, XmlQualifiedName qname, XmlDocument doc) :
            base(prefix, qname, doc)
        {
        }

        public XmlElement Contents
        {
            get { return GetFirstChildElement(); }
            set
            {
                InnerXml = "";
                AddChild(value);
            }
        }

        public string[] Accept
        {
            get { return GetAttr("accept").Split(new char[] {','}); }
            set { SetAttr("accept", string.Join(",", value)); }
        }

        public long Ack
        {
            get { return GetLongAttr("ack"); }
            set { SetLongAttr("ack", value); }
        }

        public string AuthID
        {
            get { return GetAttr("authid"); }
            set { SetAttr("authid", value); }
        }

        public string[] Charsets
        {
            get { return GetAttr("accept").Split(new char[] {' '}); }
            set { SetAttr("accept", string.Join(" ", value)); }
        }

        public ConditionType Condition
        {
            get { return GetEnumAttr<ConditionType>("condition"); }
            set { SetEnumAttr("condition", value); }
        }

        public string Content
        {
            get { return GetAttr("content"); }
            set { SetAttr("content", value); }
        }

        public string From
        {
            get { return GetAttr("from"); }
            set { SetAttr("from", value); }
        }

        public int Hold
        {
            get { return GetIntAttr("hold"); }
            set { SetIntAttr("hold", value); }
        }

        public int Inactivity
        {
            get { return GetIntAttr("inactivity"); }
            set { SetIntAttr("inactivity", value); }
        }
        
        public string Key
        {
            get { return GetAttr("key"); }
            set { SetAttr("key", value); }
        }
        
        public int MaxPause
        {
            get { return GetIntAttr("maxpause"); }
            set { SetIntAttr("maxpause", value); }
        }
        
        public string NewKey
        {
            get { return GetAttr("newkey"); }
            set { SetAttr("newkey", value); }
        }
        
        public int Pause
        {
            get { return GetIntAttr("pause"); }
            set { SetIntAttr("pause", value); }
        }
        
        public int Polling
        {
            get { return GetIntAttr("polling"); }
            set { SetIntAttr("polling", value); }
        }
        
        public int Report
        {
            get { return GetIntAttr("report"); }
            set { SetIntAttr("report", value); }
        }
        
        public int Requests
        {
            get { return GetIntAttr("requests"); }
            set { SetIntAttr("requests", value); }
        }

        public long RID
        {
            get { return GetLongAttr("rid"); }
            set { SetLongAttr("rid", value); }
        }

        public string Route
        {
            get { return GetAttr("route"); }
            set { SetAttr("route", value); }
        }

        public bool Secure
        {
            get
            {
                string s = GetAttr("secure");
                switch (s)
                {
                case "true":
                    return true;
                case "false":
                    return false;
                default:
                    return false;
                }
            }
            set
            {
                if (value)
                    SetAttr("secure", "true");
                else
                    SetAttr("secure", null);
            }
        }

        public string SID
        {
            get { return GetAttr("sid"); }
            set { SetAttr("sid", value); }
        }
        
        public string Stream
        {
            get { return GetAttr("stream"); }
            set { SetAttr("stream", value); }
        }
        
        public int Time
        {
            get { return GetIntAttr("time"); }
            set { SetIntAttr("time", value); }
        }

        public string To
        {
            get { return GetAttr("to"); }
            set { SetAttr("to", value); }
        }

        public BodyType Type
        {
            get { return GetEnumAttr<BodyType>("type"); }
            set { SetEnumAttr("type", value); }         
        }
        
        public string Ver
        {
            get { return GetAttr("ver"); }
            set { SetAttr("ver", value); }
        }

        public string Wait
        {
            get { return GetAttr("wait"); }
            set { SetAttr("wait", value); }
        }
   }
}