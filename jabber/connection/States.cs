/* --------------------------------------------------------------------------
 * Copyrights
 * 
 * Portions created by or assigned to Cursive Systems, Inc. are 
 * Copyright (c) 2002-2005 Cursive Systems, Inc.  All Rights Reserved.  Contact
 * information for Cursive Systems, Inc. is available at
 * http://www.cursive.net/.
 *
 * License
 * 
 * Jabber-Net can be used under either JOSL or the GPL.  
 * See LICENSE.txt for details.
 * --------------------------------------------------------------------------*/
using System;

using bedrock.util;

namespace jabber.connection
{
    /// <summary>
    /// Base class for all states.
    /// </summary>
    [RCS(@"$Header$")]
    public abstract class BaseState
    {
    }

    /// <summary>
    /// Up and running.  If subclasses change the state transition
    /// approach, they should end at the RunningState state.
    /// </summary>
    [RCS(@"$Header$")]
    public class RunningState : BaseState
    {
        /// <summary>
        /// The instance that is always used.
        /// </summary>
        public static readonly BaseState Instance = new RunningState();
    }

    /// <summary>
    /// Not connected.
    /// </summary>
    [RCS(@"$Header$")]
    public class ClosedState : BaseState
    {
        /// <summary>
        /// The instance that is always used.
        /// </summary>
        public static readonly BaseState Instance = new ClosedState();
    }

    /// <summary>
    /// In the process of connecting.  DNS lookup, socket setup, etc.
    /// </summary>
    [RCS(@"$Header$")]
    public class ConnectingState : BaseState
    {
        /// <summary>
        /// The instance that is always used.
        /// </summary>
        public static readonly BaseState Instance = new ConnectingState();
    }

    /// <summary>
    /// Have a connected socket.
    /// </summary>
    [RCS(@"$Header$")]
    public class ConnectedState : BaseState
    {
        /// <summary>
        /// The instance that is always used.
        /// </summary>
        public static readonly BaseState Instance = new ConnectedState();
    }

    /// <summary>
    /// Got the stream:stream.
    /// </summary>
    [RCS(@"$Header$")]
    public class StreamState : BaseState
    {
        /// <summary>
        /// The instance that is always used.
        /// </summary>
        public static readonly BaseState Instance = new StreamState();
    }

    /// <summary>
    /// A close was requested, but hasn't yet finalized.
    /// </summary>
    [RCS(@"$Header$")]
    public class ClosingState : BaseState
    {
        /// <summary>
        /// The instance that is always used.
        /// </summary>
        public static readonly BaseState Instance = new ClosingState();
    }

    /// <summary>
    /// Paused, waiting for reconnect timeout.
    /// </summary>
    [RCS(@"$Header$")]
    public class ReconnectingState : BaseState
    {
        /// <summary>
        /// The instance that is always used.
        /// </summary>
        public static readonly BaseState Instance = new ReconnectingState();
    }

    /// <summary>
    /// Accepting incoming socket connections.
    /// </summary>
    [RCS(@"$Header$")]
    public class AcceptingState : BaseState
    {
        /// <summary>
        /// The instance that is always used.
        /// </summary>
        public static readonly BaseState Instance = new AcceptingState();
    }
    /// <summary>
    /// Old-style auth, iq:auth or handshake.
    /// </summary>
    [RCS(@"$Header$")]
    public class NonSASLAuthState : BaseState
    {
        /// <summary>
        /// The instance that is always used.
        /// </summary>
        public static readonly BaseState Instance = new NonSASLAuthState();
    }
    /// <summary>
    /// Waiting for the server to send the features element
    /// </summary>
    [RCS(@"$Header$")]
    public class ServerFeaturesState : BaseState
    {
        /// <summary>
        /// The instance that is always used.
        /// </summary>
        public static readonly BaseState Instance = new ServerFeaturesState();
    }
    /// <summary>
    /// Start-TLS is starting to TLS.
    /// </summary>
    [RCS(@"$Header$")]
    public class StartTLSState : BaseState
    {
        /// <summary>
        /// The instance that is always used.
        /// </summary>
        public static readonly BaseState Instance = new StartTLSState();
    }
    /// <summary>
    /// SASL Authentication in process
    /// </summary>
    [RCS(@"$Header$")]
    public class SASLState : BaseState
    {
        /// <summary>
        /// The instance that is always used.
        /// </summary>
        public static readonly BaseState Instance = new SASLState();
    }
    /// <summary>
    /// SASL Authentication finished.  Restarting the stream for the last time.
    /// </summary>
    [RCS(@"$Header$")]
    public class SASLAuthedState : BaseState
    {
        /// <summary>
        /// The instance that is always used.
        /// </summary>
        public static readonly BaseState Instance = new SASLAuthedState();
    }
    /// <summary>
    /// Binding session
    /// </summary>
    [RCS(@"$Header$")]
    public class BindState : BaseState
    {
        /// <summary>
        /// The instance that is always used.
        /// </summary>
        public static readonly BaseState Instance = new BindState();
    }
    /// <summary>
    /// 
    /// </summary>
    [RCS(@"$Header$")]
    public class X509AuthenticationState : BaseState
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly BaseState Instance = new X509AuthenticationState();
    }
}
