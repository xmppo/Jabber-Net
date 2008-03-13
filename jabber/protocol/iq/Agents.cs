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

namespace jabber.protocol.iq
{
    // <pre>
    // <iq from='jabber.org' id='jcl_17' to='hildjj@jabber.org/Work' type='result'>
    //   <query xmlns='jabber:iq:agents'>
    //     <agent jid='users.jabber.org'>
    //       <name>Jabber User Directory</name>
    //       <service>jud</service>
    //       <search/>
    //       <register/>
    //     </agent>
    //   </query>
    // </iq>
    // </pre>
    /// <summary>
    /// IQ packet with an agents query element inside.
    /// </summary>
    [SVN(@"$Id$")]
    public class AgentsIQ : jabber.protocol.client.TypedIQ<AgentsQuery>
    {
        /// <summary>
        /// Create an agents IQ packet.
        /// </summary>
        /// <param name="doc"></param>
        public AgentsIQ(XmlDocument doc) : base(doc)
        {
        }
    }

    /// <summary>
    /// An agents query element.
    /// </summary>
    [SVN(@"$Id$")]
    public class AgentsQuery : Element
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="doc"></param>
        public AgentsQuery(XmlDocument doc) : base("query", URI.AGENTS, doc)
        {
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public AgentsQuery(string prefix, XmlQualifiedName qname, XmlDocument doc) :
            base(prefix, qname, doc)
        {
        }

        /// <summary>
        /// Add an agent to the list
        /// </summary>
        /// <returns></returns>
        public Agent AddAgent()
        {
            return CreateChildElement<Agent>();
        }

        /// <summary>
        /// Get the list of agents
        /// </summary>
        /// <returns></returns>
        public Agent[] GetAgents()
        {
            return GetElements<Agent>().ToArray();
        }
    }

    /// <summary>
    /// Agent items
    /// </summary>
    [SVN(@"$Id$")]
    public class Agent : Element
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="doc"></param>
        public Agent(XmlDocument doc) : base("agent", URI.AGENTS, doc)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public Agent(string prefix, XmlQualifiedName qname, XmlDocument doc) :
            base(prefix, qname, doc)
        {
        }

        /// <summary>
        /// The agent's JID
        /// </summary>
        public JID JID
        {
            get { return GetAttr("jid"); }
            set { this.SetAttr("jid", value.ToString()); }
        }

        /// <summary>
        /// The agent's name
        /// </summary>
        public string AgentName
        {
            get { return GetElem("name"); }
            set { SetElem("name", value); }
        }

        /// <summary>
        /// The agent's description
        /// </summary>
        public string Description
        {
            get { return GetElem("description"); }
            set { SetElem("description", value); }
        }

        /// <summary>
        /// Is the agent a transport?
        /// </summary>
        public bool Transport
        {
            get { return this["transport"] != null; }
            set
            {
                if (value)
                {
                    SetElem("transport", null);
                }
                else
                {
                    RemoveElem("transport");
                }
            }
        }

        /// <summary>
        /// Is the agent for groupchat?
        /// </summary>
        public bool Groupchat
        {
            get { return this["groupchat"] != null; }
            set
            {
                if (value)
                {
                    SetElem("groupchat", null);
                }
                else
                {
                    RemoveElem("groupchat");
                }
            }
        }

        /// <summary>
        /// The agent service name.
        /// </summary>
        public string Service
        {
            get { return GetElem("service"); }
            set { SetElem("service", value); }
        }

        /// <summary>
        /// Is the agent a registrar?
        /// </summary>
        public bool Register
        {
            get { return this["register"] != null; }
            set
            {
                if (value)
                {
                    SetElem("register", null);
                }
                else
                {
                    RemoveElem("register");
                }
            }
        }

        /// <summary>
        /// Is the agent for JUD?
        /// </summary>
        public bool Search
        {
            get { return this["search"] != null; }
            set
            {
                if (value)
                {
                    SetElem("search", null);
                }
                else
                {
                    RemoveElem("search");
                }
            }
        }
    }
}
