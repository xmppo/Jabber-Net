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
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using STrace = System.Diagnostics.Trace;
using bedrock.util;
namespace bedrock.util
{
    /// <summary>
    /// Trace logging the way <i>I</i> wanted it.
    /// </summary>
    [RCS(@"$Header$")]
    public class Tracer
    {
        private static TraceSwitch s_level = null;
        
        /// <summary>
        /// 
        /// </summary>
        public static TraceLevel Level
        {
            get 
            {
                if (s_level == null)
                    return TraceLevel.Off;
                return s_level.Level;
            }
            set 
            {
                if (s_level == null)
                    throw new InvalidOperationException("call Initialize() first");
                s_level.Level = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Conditional("TRACE")] 
        static public void Trace(TraceLevel level, string msg)
        {
            if (s_level != null)
                STrace.WriteLineIf(s_level.Level >= level, msg);
        }

        /// <summary>
        /// 
        /// </summary>
        [Conditional("TRACE")] 
        static public void Trace(TraceLevel level, string format, params object[] args)
        {
            if (s_level != null)
                STrace.WriteLineIf(s_level.Level >= level, String.Format(format, args));
        }
        
        /// <summary>
        /// 
        /// </summary>
        [Conditional("TRACE")]
        public static void Initialize(string switchName, TraceLevel initialLevel, TraceListener[] listeners)
        {
            Debug.Assert(s_level == null, "Only call Initialize once, please");
            s_level       = new TraceSwitch(switchName, "Logging level for " + switchName);
            s_level.Level = initialLevel;
            
            if (listeners == null)
                return;
            if (listeners.Length > 0)
            {
                STrace.Listeners.Clear();
            }
            foreach (TraceListener tl in listeners)
            {
                STrace.Listeners.Add(tl);
            }            
        }
    }
}
