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
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Xml;

using bedrock.util;
using bedrock.net;

using jabber.protocol;

namespace jabber.connection
{
    /// <summary>
    /// The types of proxies we support.
    /// </summary>
    public enum ProxyType
    {
        /// <summary>
        /// no proxy
        /// </summary>
        None,

        /// <summary>
        /// socks5 as in http://archive.socks.permeo.com/rfc/rfc1928.txt
        /// </summary>
        Socks5
    }


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
        private BaseSocket     m_sock       = null;
        private BaseSocket     m_accept     = null;
        private XmlDocument    m_doc        = new XmlDocument();
        private ElementStream  m_stream     = null;
        private BaseState      m_state      = ClosedState.Instance;
        private string         m_server     = "jabber.com";
        private string         m_streamID   = null;
        private object         m_stateLock  = new object();
        private ArrayList      m_callbacks  = new ArrayList();
        private int            m_keepAlive  = 20000;
        private Timer          m_timer      = null;

        private int            m_port       = 5222;
        private int            m_autoReconnect = 30000;

        private ProxyType      m_ProxyType = ProxyType.None;
        private string         m_ProxyHost = null;
        private int            m_ProxyPort = 1080;
        private string         m_ProxyUsername = null;
        private string         m_ProxyPassword = null;

        private XmlNamespaceManager m_ns;

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
            m_watcher = new SocketWatcher();
            m_ns = new XmlNamespaceManager(m_doc.NameTable);
            m_timer = new Timer(new TimerCallback(DoKeepAlive), null, Timeout.Infinite, Timeout.Infinite);
        }

        /// <summary>
        /// Create a SocketElementStream
        /// </summary>
        public SocketElementStream()
        {
            m_watcher = new SocketWatcher();
            m_ns = new XmlNamespaceManager(m_doc.NameTable);
            m_timer = new Timer(new TimerCallback(DoKeepAlive), null, Timeout.Infinite, Timeout.Infinite);
        }

		/// <summary>
		/// Create a SocketElementStream out of an accepted socket.
		/// </summary>
		/// <param name="aso"></param>
		public SocketElementStream(AsyncSocket aso)
		{
			m_watcher = aso.SocketWatcher;
			m_ns = new XmlNamespaceManager(m_doc.NameTable);
            m_timer = new Timer(new TimerCallback(DoKeepAlive), null, Timeout.Infinite, Timeout.Infinite);
            InitializeStream();
			m_state = jabber.connection.AcceptingState.Instance;
		}

        /// <summary>
        /// Create a SocketElementStream with an existing SocketWatcher, so that you can do
        /// lots of concurrent connections.
        /// </summary>
        /// <param name="watcher"></param>
        public SocketElementStream(SocketWatcher watcher)
        {
            m_watcher = watcher;
            m_ns = new XmlNamespaceManager(m_doc.NameTable);
            m_timer = new Timer(new TimerCallback(DoKeepAlive), null, Timeout.Infinite, Timeout.Infinite);
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
        [DefaultValue(20f)]
        public float KeepAlive
        {
            get { return m_keepAlive / 1000f; }
            set 
            { 
                lock (m_stateLock)
                {
                    m_keepAlive = (int)(value * 1000f);
                    if (value <= 0)
                    {
                        m_timer.Change(Timeout.Infinite, Timeout.Infinite);
                        m_timer.Dispose();
                        m_timer = null;
                    }
                    else
                    {
                        m_timer = new Timer(new TimerCallback(DoKeepAlive), null, m_keepAlive, m_keepAlive);
                    }
                }            
            }
        }

        /// <summary>
        /// Seconds before automatically reconnecting if the connection drops.  -1 to disable.
        /// </summary>
        [Description("Automatically reconnect a connection.")]
        [DefaultValue(30)]
        [Category("Automation")]
        public float AutoReconnect
        {
            get { return m_autoReconnect / 1000f; }
            set { m_autoReconnect = (int)(value * 1000f); }
        }

        /// <summary>
        /// the type of proxy... none, socks5
        /// </summary>
        [Description("The type of proxy... none, socks5, etc.")]
        [DefaultValue(ProxyType.None)]
        [Category("Proxy")]
        public ProxyType Proxy
        {
            get { return m_ProxyType; }
            set { m_ProxyType = value; }
        }
 
        /// <summary>
        /// the host running the proxy
        /// </summary>
        [Description("the host running the proxy")]
        [DefaultValue(null)]
        [Category("Proxy")]
        public string ProxyHost
        {
            get { return m_ProxyHost; }
            set { m_ProxyHost = value; }
        }

        /// <summary>
        /// the port to talk to the proxy host on
        /// </summary>
        [Description("the port to talk to the proxy host on")]
        [DefaultValue(1080)]
        [Category("Proxy")]
        public int ProxyPort
        {
            get { return m_ProxyPort; }
            set { m_ProxyPort = value; }
        }

        /// <summary>
        /// the auth username for the socks5 proxy
        /// </summary>
        [Description("the auth username for the socks5 proxy")]
        [DefaultValue(null)]
        [Category("Proxy")]
        public string ProxyUsername
        {
            get { return m_ProxyUsername; }
            set { m_ProxyUsername = value; }
        }

        /// <summary>
        /// the auth password for the socks5 proxy
        /// </summary>
        [Description("the auth password for the socks5 proxy")]
        [DefaultValue(null)]
        [Category("Proxy")]
        public string ProxyPassword
        {
            get { return m_ProxyPassword; }
            set { m_ProxyPassword = value; }
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
                switch (m_ProxyType)
                {
                    case ProxyType.Socks5:
                        Socks5Proxy proxy = new Socks5Proxy(this);
                        proxy.Socket   = new AsyncSocket(m_watcher, m_sock.Listener);
                        proxy.Host     = m_ProxyHost;
                        proxy.Port     = m_ProxyPort;
                        proxy.Username = m_ProxyUsername;
                        proxy.Password = m_ProxyPassword;
                        m_sock = proxy;
                        break;
                        
                    case None:
                        m_sock = new AsyncSocket(m_watcher, this);
                        break;
                }
                m_sock.Connect(addr);
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
                Debug.WriteLine("Exception passed along by SocketElementStream: " + e.ToString());
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
            CheckAll(tag);
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
            CheckAll(tag);
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
            m_accept = m_watcher.CreateListenSocket(this, new Address(this.Port));
            m_accept.RequestAccept();
        }

        #region Implementation of ISocketEventListener
        /// <summary>
        /// An accept socket is about to be bound, or a connect socket is about to connect, 
        /// or an incoming socket just came in.  Use this as an opportunity to 
        /// </summary>
        /// <param name="new_sock">The new socket that is about to be connected.</param>
        void ISocketEventListener.OnInit(AsyncSocket new_sock)
        {
        }

        ISocketEventListener ISocketEventListener.GetListener(AsyncSocket listen_sock)
        {
            return this;
        }

        bool ISocketEventListener.OnAccept(bedrock.net.AsyncSocket newsocket)
        {
            Debug.Assert(this.State == AcceptingState.Instance);

            this.State = ConnectedState.Instance;

            m_sock = newsocket;
            m_sock.RequestRead();

            m_accept.Close();

            return false;           
        }

        bool ISocketEventListener.OnRead(bedrock.net.AsyncSocket sock, byte[] buf, int offset, int count)
        {
            m_timer.Change(m_keepAlive, m_keepAlive);
            m_stream.Push(buf, offset, count);

            if (OnReadText != null)
                CheckedInvoke(OnReadText, new object[] {sock, ENC.GetString(buf, offset, count)});

            return true;
        }

        void ISocketEventListener.OnWrite(bedrock.net.AsyncSocket sock, byte[] buf, int offset, int count)
        {
            m_timer.Change(m_keepAlive, m_keepAlive);

            if (OnWriteText != null)
                CheckedInvoke(OnWriteText, new object[] {sock, ENC.GetString(buf, offset, count)});
        }

        void ISocketEventListener.OnError(bedrock.net.AsyncSocket sock, System.Exception ex)
        {
            lock (m_stateLock)
            {
                m_timer.Change(Timeout.Infinite, Timeout.Infinite);

                BaseState old = m_state;
                m_state = ClosedState.Instance;
                m_sock = null;

                // close was requested, or autoreconnect turned off.
                if ((old != ClosingState.Instance) && (m_autoReconnect >= 0))
                {
                    new System.Threading.Timer(new System.Threading.TimerCallback(Reconnect), 
                        null, 
                        m_autoReconnect, 
                        System.Threading.Timeout.Infinite );
                }
            }

            if (OnError != null)
                CheckedInvoke(OnError, new object[] {sock, ex});
        }

        void ISocketEventListener.OnConnect(bedrock.net.AsyncSocket sock)
        {
            lock (m_stateLock)
            {
                this.State = ConnectedState.Instance;
                jabber.protocol.stream.Stream str = new jabber.protocol.stream.Stream(m_doc, NS);
                str.To = this.Host;
                Write(str.StartTag());
                sock.RequestRead();
                m_timer.Change(m_keepAlive, m_keepAlive);
            }
        }

        private void Reconnect(object state)
        {
            Connect();
        }

        void ISocketEventListener.OnClose(bedrock.net.AsyncSocket sock)
        {
            lock (StateLock)
            {
                m_timer.Change(Timeout.Infinite, Timeout.Infinite);

                BaseState old = m_state;
                m_state = ClosedState.Instance;
                m_sock = null;

                // close was requested, or autoreconnect turned off.
                if ((old != ClosingState.Instance) && (m_autoReconnect >= 0))
                {
                    new System.Threading.Timer(new System.Threading.TimerCallback(Reconnect), 
                        null, 
                        m_autoReconnect, 
                        System.Threading.Timeout.Infinite );
                }
            }
            if (OnDisconnect != null)
                CheckedInvoke(OnDisconnect, new object[]{this});
        }
        #endregion


        /// <summary>
        /// Register a callback, so that if a packet arrives that matches the given xpath expression,
        /// the callback fires.  Use <see cref="AddNamespace"/> to add namespace prefixes.
        /// </summary>
        /// <example>jc.AddCallback("self::iq[@type='result']/roster:query", new ProtocolHandler(GotRoster));</example>
        /// <param name="xpath">The xpath expression to search for</param>
        /// <param name="cb">The callback to call when the xpath matches</param>
        /// <returns>A guid that can be used to unregister the callback</returns>
        public Guid AddCallback(string xpath, ProtocolHandler cb)
        {
            CallbackData cbd = new CallbackData(xpath, cb);
            m_callbacks.Add(cbd);
            return cbd.Guid;
        }

        /// <summary>
        /// Remove a callback added with <see cref="AddCallback"/>.
        /// </summary>
        /// <param name="guid"></param>
        public void RemoveCallback(Guid guid)
        {
            int count = 0;
            foreach (CallbackData cbd in m_callbacks)
            {
                if (cbd.Guid == guid)
                {
                    m_callbacks.RemoveAt(count);
                    return;
                }
                count++;
            }
            throw new ArgumentException("Unknown Guid", "guid");
        }

        /// <summary>
        /// Add a namespace prefix, for use with callback xpath expressions added with <see cref="AddCallback"/>.
        /// </summary>
        /// <param name="prefix">The prefix to use</param>
        /// <param name="uri">The URI associated with the prefix</param>
        public void AddNamespace(string prefix, string uri)
        {
            m_ns.AddNamespace(prefix, uri);
        }

        private void CheckAll(XmlElement elem)
        {
            foreach (CallbackData cbd in m_callbacks)
            {
                cbd.Check(this, elem);
            }
        }

        private void DoKeepAlive(object state)
        {
            lock (m_stateLock)
            {
                if (m_state != RunningState.Instance)
                {
                    return;
                }
            }
            Write(new byte[] {32});
        }

        private class CallbackData
        {
            private Guid            m_guid = Guid.NewGuid();
            private ProtocolHandler m_cb;
            private string          m_xpath;

            public CallbackData(string xpath, ProtocolHandler cb)
            {
                Debug.Assert(cb != null);
                m_cb = cb;
                m_xpath = xpath;
            }

            public Guid Guid
            {
                get { return m_guid; }
            }

            public string XPath
            {
                get { return m_xpath; }
            }

            public void Check(SocketElementStream sender, XmlElement elem)
            {
                try
                {
                    XmlNode n = elem.SelectSingleNode(m_xpath, sender.m_ns);
                    if (n != null)
                    {
                        sender.CheckedInvoke(m_cb, new object[] {sender, elem} );
                    }
                }
                catch (Exception e)
                {
                    sender.FireOnError(e);
                }
            }
        }
    }
}
