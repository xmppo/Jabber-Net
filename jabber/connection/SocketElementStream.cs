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
using System.ComponentModel;
using System.Diagnostics;
using System.Xml;

using bedrock.util;
using bedrock.net;

using jabber.protocol;

namespace jabber.connection
{
    /// <summary>
    /// Summary description for SocketElementStream.
    /// </summary>
    [RCS(@"$Header$")]
    abstract public class SocketElementStream : 
        System.ComponentModel.Component, 
        ISocketEventListener
    {
        /// <summary>
        /// Character encoding.  UTF-8.
        /// </summary>
        protected static readonly System.Text.Encoding ENC = System.Text.Encoding.UTF8;

        private SocketWatcher  m_watcher    = null;
        private AsyncSocket    m_sock       = null;
        private AsyncSocket    m_accept     = null;
        private XmlDocument    m_doc        = new XmlDocument();
        private ElementStream  m_stream     = null;
        private int            m_keepAlive  = 20;
        private BaseState      m_state      = ClosedState.Instance;
        private string         m_server     = "jabber.com";
        private string         m_streamID   = null;
        private object         m_stateLock  = new object();

        private int            m_port       = 5222;
        private int            m_autoReconnect = 30;

        private ISynchronizeInvoke m_invoker = null;
        
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = new System.ComponentModel.Container();

        /// <summary>
        /// Required for Windows.Forms Class Composition Designer support
        /// </summary>
        /// <param name="container"></param>
        public SocketElementStream(System.ComponentModel.IContainer container)
        {
            container.Add(this);
			m_watcher = new SocketWatcher(5);
        }

        /// <summary>
        /// Create a SocketElementStream
        /// </summary>
        public SocketElementStream()
        {
			m_watcher = new SocketWatcher(5);
		}

		/// <summary>
		/// Create a SocketElementStream with an existing SocketWatcher, so that you can do
		/// lots of concurrent connections.
		/// </summary>
		/// <param name="watcher"></param>
		public SocketElementStream(SocketWatcher watcher)
		{
			m_watcher = watcher;
		}

        /// <summary>
        /// Set up the element stream.  This is the place to add factories.
        /// </summary>
        protected virtual void InitializeStream()
        {
            m_stream = new ElementStream();
            m_stream.OnDocumentStart += new ProtocolHandler(OnDocumentStart);
            m_stream.OnDocumentEnd += new bedrock.ObjectHandler(OnDocumentEnd);
            m_stream.OnElement += new ProtocolHandler(OnElement);
        }

        /// <summary>
        /// Text was written to the server.  Use for debugging only.  
        /// Will NOT be complete nodes at a time.
        /// </summary>
        [Category("Debug")]
        public event bedrock.TextHandler OnWriteText;
        /// <summary>
        /// Text was read from the server.  Use for debugging only.  
        /// Will NOT be complete nodes at a time.
        /// </summary>
        [Category("Debug")]
        public event bedrock.TextHandler OnReadText;
        /// <summary>
        /// Some error occurred when processing.  
        /// The connection has been closed.
        /// </summary>
        [Category("Stream")]
        public event bedrock.ExceptionHandler OnError;
        /// <summary>
        /// Get notified for every jabber packet.  
        /// This is a union of OnPresence, OnMessage, and OnIQ.
        /// Use this *or* the others, but not both, as a matter of style.
        /// </summary>
        [Category("Stream")]
        public event ProtocolHandler OnProtocol;
        /// <summary>
        /// The connection is complete, and the user is authenticated.
        /// </summary>
        [Category("Stream")]
        public event bedrock.ObjectHandler OnAuthenticate;
        /// <summary>
        /// The connection is disconnected
        /// </summary>
        [Category("Stream")]
        public event bedrock.ObjectHandler OnDisconnect;
        
        /// <summary>
        /// The server to connect to.  
        /// Once .Net and bedrock support SRV lookups,
        /// so should this.
        /// </summary>
        [Description("The server to connect to.")]
        [DefaultValue("jabber.com")]
        [Category("Jabber")]
        public string Server
        {
            get { return m_server; }
            set { m_server = value; }
        }

        /// <summary>
        /// The TCP port to connect to.
        /// </summary>
        [Description("The TCP port to connect to.")]
        [DefaultValue(5222)]
        [Category("Jabber")]
        public int Port
        {
            get { return m_port; }
            set { m_port = value; }
        }

        /// <summary>
        /// Invoke() all callbacks on this control.
        /// </summary>
        [Description("Invoke all callbakcs on this control")]
        [DefaultValue(null)]
        [Category("Jabber")]
        public ISynchronizeInvoke InvokeControl
        {
            get { return m_invoker; }
            set { m_invoker = value; }
        }

        /// <summary>
        /// Time, in seconds, between keep-alive spaces.
        /// </summary>
        [Description("Time, in seconds, between keep-alive spaces.")]
        [Category("Jabber")]
        [DefaultValue(20)]
        public int KeepAlive
        {
            get { return m_keepAlive; }
            set { m_keepAlive = value; }
        }

        /// <summary>
        /// Seconds before automatically reconnecting if the connection drops.  -1 to disable.
        /// </summary>
        [Description("Automatically reconnect a connection.")]
        [DefaultValue(30)]
        [Category("Automation")]
        public int AutoReconnect
        {
            get { return m_autoReconnect; }
            set { m_autoReconnect = value; }
        }

        /// <summary>
        /// The id attribute from the stream:stream element sent by the server.
        /// </summary>
        [Browsable(false)]
        [DefaultValue(null)]
        public string StreamID
        {
            get { return m_streamID; }
            set { m_streamID = value; }
        }

        /// <summary>
        /// The outbound document.
        /// </summary>
        [Browsable(false)]
        public XmlDocument Document
        {
            get { return m_doc; }
        }

        /// <summary>
        /// The current state of the connection.  
        /// Lock on StateLock before accessing.
        /// </summary>
        [Browsable(false)]
        protected virtual BaseState State
        {
            get { return m_state; }
            set { m_state = value; }
        }

        /// <summary>
        /// A lock for the state info.
        /// </summary>
        [Browsable(false)]
        protected object StateLock
        {
            get { return m_stateLock; }
        }

        /// <summary>
        /// Have we authenticated?  Locks on StateLock
        /// </summary>
        [Browsable(false)]
        [DefaultValue(false)]
        public virtual bool IsAuthenticated
        {
            get { lock (StateLock) { return m_state == RunningState.Instance; } }
            set
            {
                lock (StateLock)
                {
                    if (value)
                    {
                        m_state = RunningState.Instance;
                    }
                    else
                        Close();
                }
                if (OnAuthenticate != null)
                    CheckedInvoke(OnAuthenticate, new object[] {this});
            }
        }

        /// <summary>
        /// The namespace for this connection.
        /// </summary>
        [Browsable(false)]
        protected abstract string NS
        {
            get;
        }

        /// <summary>
        /// The host for the to attribute of the stream:stream for this connection.
        /// </summary>
        [Browsable(false)]
        protected virtual string Host
        {
            get { return m_server; }
        }

        /// <summary>
        /// Add new packet types that the incoming stream knows how to create.  
        /// If you create your own packet types, create a packet factory as well. 
        /// </summary>
        /// <param name="factory"></param>
        public void AddFactory(IPacketTypes factory)
        {
            m_stream.AddFactory(factory);
        }

        /// <summary>
        /// Add a type to the packet factory.
        /// </summary>
        /// <param name="localName">Local Name (e.g. query)</param>
        /// <param name="ns">Namespace URI (e.g. jabber:iq:roster)</param>
        /// <param name="t">Type to create</param>
        public void AddType(string localName, string ns, Type t)
        {
            m_stream.AddType(localName, ns, t);
        }

        /// <summary>
        /// Write these bytes to the socket.
        /// </summary>
        /// <param name="buf"></param>
        public void Write(byte[] buf)
        {
            m_sock.Write(buf);
        }

        /// <summary>
        /// Write this string to the socket, encoded as UTF-8.
        /// </summary>
        /// <param name="buf"></param>
        public void Write(string buf)
        {
            m_sock.Write(ENC.GetBytes(buf));
        }

        /// <summary>
        /// Send the given packet to the server.
        /// </summary>
        /// <param name="elem"></param>
        public void Write(XmlElement elem)
        {
            Write(elem.OuterXml);
        }

        /// <summary>
        /// Start connecting to the server.  This is async.
        /// </summary>
        public virtual void Connect()
        {
            Debug.Assert(m_port > 0);
            Debug.Assert(m_server != null);

            lock (StateLock)
            {
                InitializeStream();
                Address addr = new Address(m_server, m_port);
                m_state = ConnectingState.Instance;
                m_sock = m_watcher.CreateConnectSocket(this, addr, m_keepAlive * 1000);
            }
        }

        /// <summary>
        /// Close down the connection, as gracefully as possible.
        /// </summary>
        public virtual void Close()
        {
            lock (StateLock)
            {
                if (m_state == RunningState.Instance)
                {
                    Write("</stream:stream>");
                }
                if (m_state != ClosedState.Instance)
                {
                    m_state = ClosingState.Instance;
                    m_sock.Close();
                }
            }
        }

        /// <summary>
        /// Check to see if this method needs to be invoked.  
        /// If so, invokes it on the Invoker, otherwise just calls the method in this thread.
        /// </summary>
        /// <param name="method"></param>
        /// <param name="args"></param>
        protected void CheckedInvoke(MulticastDelegate method, object[] args)
        {
            try
            {
                if ((m_invoker != null) && m_invoker.InvokeRequired)
                    m_invoker.BeginInvoke(method, args);
                else
                    // ew.  this is probably slow.
                    method.DynamicInvoke(args);
            }
            catch (System.Reflection.TargetInvocationException e)
            {
                throw e.InnerException;
            }
        }

        /// <summary>
        /// Got the initial start tag.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="tag">The start tag as if it had been a full element, with no child elements.</param>
        protected virtual void OnDocumentStart(object sender, System.Xml.XmlElement tag)
        {
            jabber.protocol.stream.Stream str = tag as jabber.protocol.stream.Stream;
            if (str == null)
                return;
            m_streamID = str.ID;
        }
        
        /// <summary>
        /// The end tag was received.
        /// </summary>
        /// <param name="sender"></param>
        protected virtual void OnDocumentEnd(object sender)
        {
            lock (StateLock)
            {
                m_state = ClosingState.Instance;
                m_sock.Close();
            }
        }

        /// <summary>
        /// We received an element.  Invoke the OnProtocol event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="tag"></param>
        protected virtual void OnElement(object sender, System.Xml.XmlElement tag)
        {
            if (OnProtocol != null)
                CheckedInvoke(OnProtocol, new object[] {this, tag});
        }

        /// <summary>
        /// Fire the OnError event.
        /// </summary>
        /// <param name="e"></param>
        protected void FireOnError(Exception e)
        {
            if (OnError != null)
                CheckedInvoke(OnError, new object[] {this, e});

            Close();
        }

        /// <summary>
        /// Start accepting connections.
        /// </summary>
        protected virtual void BeginAccept()
        {
            this.State = AcceptingState.Instance;
            m_accept = m_watcher.CreateListenSocket(this, new Address(this.Port), true);
            m_accept.RequestAccept();
        }

        #region Implementation of ISocketEventListener
        ISocketEventListener ISocketEventListener.GetListener(AsyncSocket listen_sock, System.Net.IPAddress addr)
        {
            return this;
        }

        bool ISocketEventListener.OnAccept(bedrock.net.AsyncSocket newsocket)
        {
            Debug.Assert(this.State == AcceptingState.Instance);
            //Tracer.Trace(TraceLevel.Verbose, "Accepted new socket");

            this.State = ConnectedState.Instance;

            m_sock = newsocket;
            m_sock.RequestRead();

            m_accept.Close();

            return false;           
        }

        bool ISocketEventListener.OnRead(bedrock.net.AsyncSocket sock, byte[] buf)
        {
            m_stream.Push(buf);

            if (OnReadText != null)
                CheckedInvoke(OnReadText, new object[] {sock, ENC.GetString(buf)});

            return true;
        }

        void ISocketEventListener.OnWrite(bedrock.net.AsyncSocket sock, byte[] buf)
        {
            if (OnWriteText != null)
                CheckedInvoke(OnWriteText, new object[] {sock, ENC.GetString(buf)});
        }

        void ISocketEventListener.OnError(bedrock.net.AsyncSocket sock, System.Exception ex)
        {
            if (OnError != null)
                CheckedInvoke(OnError, new object[] {sock, ex});
        }

        void ISocketEventListener.OnConnect(bedrock.net.AsyncSocket sock)
        {
            this.State = ConnectedState.Instance;
            jabber.protocol.stream.Stream str = new jabber.protocol.stream.Stream(m_doc, NS);
            str.To = this.Host;
            Write(str.StartTag());
            sock.RequestRead();
        }

        private void Reconnect(object state)
        {
            Connect();
        }

        void ISocketEventListener.OnClose(bedrock.net.AsyncSocket sock)
        {
            lock (StateLock)
            {
                BaseState old = m_state;
                m_state = ClosedState.Instance;
                m_sock = null;


                // close was requested, or autoreconnect turned off.
                if ((old == ClosingState.Instance) && (m_autoReconnect >= 0))
                {
                    new System.Threading.Timer(new System.Threading.TimerCallback(Reconnect), 
                        null, 
                        m_autoReconnect * 1000, 
                        System.Threading.Timeout.Infinite );
                }
            }
            if (OnDisconnect != null)
                CheckedInvoke(OnDisconnect, new object[]{this});
        }
        #endregion
    }
}
