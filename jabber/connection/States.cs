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

using bedrock.util;

namespace jabber.connection
{
    /// <summary>
    /// Represents the base class for all states.
    /// </summary>
    [SVN(@"$Id$")]
    public abstract class BaseState
    {
    }

    /// <summary>
    /// Specifies the state is up and running.  If subclasses change the
    /// state transition approach, they should end at the RunningState state.
    /// </summary>
    [SVN(@"$Id$")]
    public class RunningState : BaseState
    {
        /// <summary>
        /// Returns the instance of the running state.
        /// </summary>
        public static readonly BaseState Instance = new RunningState();
    }

    /// <summary>
    /// Specifies the state is not connected.
    /// </summary>
    [SVN(@"$Id$")]
    public class ClosedState : BaseState
    {
        /// <summary>
        /// Returns the instance of the closed state.
        /// </summary>
        public static readonly BaseState Instance = new ClosedState();
    }

    /// <summary>
    /// Specifies the state is in the process of connecting such as
    /// DNS lookup, socket setup, and so on.
    /// </summary>
    [SVN(@"$Id$")]
    public class ConnectingState : BaseState
    {
        /// <summary>
        /// Returns the instance of the connecting state.
        /// </summary>
        public static readonly BaseState Instance = new ConnectingState();
    }

    /// <summary>
    /// Specifies the state is in the "connected socket" state.
    /// </summary>
    [SVN(@"$Id$")]
    public class ConnectedState : BaseState
    {
        /// <summary>
        /// The instance that is always used.
        /// </summary>
        public static readonly BaseState Instance = new ConnectedState();
    }

    /// <summary>
    /// Specifies the state is in the "stream:stream has been received" state.
    /// </summary>
    [SVN(@"$Id$")]
    public class StreamState : BaseState
    {
        /// <summary>
        /// Returns the instance of the XMPP stream state.
        /// </summary>
        public static readonly BaseState Instance = new StreamState();
    }

    /// <summary>
    /// Specifies the state is in a closing state.
    /// A close was requested, but hasn't yet finalized.
    /// </summary>
    [SVN(@"$Id$")]
    public class ClosingState : BaseState
    {
        /// <summary>
        /// Returns the instance for the closing state.
        /// </summary>
        public static readonly BaseState Instance = new ClosingState();
    }

    /// <summary>
    /// Specifies the state is in a paused state waiting for reconnect timeout.
    /// </summary>
    [SVN(@"$Id$")]
    public class ReconnectingState : BaseState
    {
        /// <summary>
        /// Returns the instance of the reconnecting state.
        /// </summary>
        public static readonly BaseState Instance = new ReconnectingState();
    }

    /// <summary>
    /// Specifies the state is in the "Accepting incoming socket connections" state.
    /// </summary>
    [SVN(@"$Id$")]
    public class AcceptingState : BaseState
    {
        /// <summary>
        /// Returns the instance of the accepting state.
        /// </summary>
        public static readonly BaseState Instance = new AcceptingState();
    }
    /// <summary>
    /// Specifies the state is in Old-style auth, iq:auth or handshake.
    /// </summary>
    [SVN(@"$Id$")]
    public class NonSASLAuthState : BaseState
    {
        /// <summary>
        /// Returns the instance of the non SASL authentication state.
        /// </summary>
        public static readonly BaseState Instance = new NonSASLAuthState();
    }
    /// <summary>
    /// Specifies the state is in waiting for the server to send the features element.
    /// </summary>
    [SVN(@"$Id$")]
    public class ServerFeaturesState : BaseState
    {
        /// <summary>
        /// Returns the instance of the server features state.
        /// </summary>
        public static readonly BaseState Instance = new ServerFeaturesState();
    }
    /// <summary>
    /// Specifies the state is in Start-TLS.
    /// </summary>
    [SVN(@"$Id$")]
    public class StartTLSState : BaseState
    {
        /// <summary>
        /// Returns the instance of the Start-TLS state.
        /// </summary>
        public static readonly BaseState Instance = new StartTLSState();
    }
    /// <summary>
    /// Specifies the state is in the compression state.
    /// </summary>
    [SVN(@"$Id$")]
    public class CompressionState : BaseState
    {
        /// <summary>
        /// The instance that is always used.
        /// </summary>
        public static readonly BaseState Instance = new CompressionState();
    }
    /// <summary>
    /// Specifies the state is in SASL Authentication.
    /// </summary>
    [SVN(@"$Id$")]
    public class SASLState : BaseState
    {
        /// <summary>
        /// Returns the instance of the SASL state.
        /// </summary>
        public static readonly BaseState Instance = new SASLState();
    }
    /// <summary>
    /// Specifies the state is in the SASL Authentication has finished state.
    /// Restarting the stream for the last time.
    /// </summary>
    [SVN(@"$Id$")]
    public class SASLAuthedState : BaseState
    {
        /// <summary>
        /// Returns the instance for the SASL authentication state.
        /// </summary>
        public static readonly BaseState Instance = new SASLAuthedState();
    }
    /// <summary>
    /// SASL Authentication failed.  On some servers you can re-try, or register.
    /// </summary>
    [SVN(@"$Id$")]
    public class SASLFailedState : BaseState
    {
        /// <summary>
        /// The instance that is always used.
        /// </summary>
        public static readonly BaseState Instance = new SASLFailedState();
    }
    /// <summary>
    /// Specifies the state is in the "Binding session" state.
    /// </summary>
    [SVN(@"$Id$")]
    public class BindState : BaseState
    {
        /// <summary>
        /// Returns the instance for the Bind state.
        /// </summary>
        public static readonly BaseState Instance = new BindState();
    }

}
