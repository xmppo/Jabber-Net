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

namespace jabber.connection
{
    /// <summary>
    /// Base class for all states.
    /// </summary>
    public abstract class BaseState
    {
    }

    /// <summary>
    /// Up and running.  If subclasses change the state transition
    /// approach, they should end at the RunningState state.
    /// </summary>
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
    public class AcceptingState : BaseState
    {
        /// <summary>
        /// The instance that is always used.
        /// </summary>
        public static readonly BaseState Instance = new AcceptingState();
    }
}
