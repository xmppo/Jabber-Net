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
 * Portions Copyright (c) 2003 Joe Hildebrand.
 * 
 * Acknowledgements
 * 
 * Special thanks to the Jabber Open Source Contributors for their
 * suggestions and support of Jabber.
 * 
 * --------------------------------------------------------------------------*/
using System;

namespace stringprep.unicode
{
	public class Combining
	{
        private static bool s_init = false;
        private static byte[,] s_classes = null;
        private static byte[] s_pages = null;
        private static object s_lock = new object();

        /// <summary>
        /// What is the combining class for the given character?
        /// </summary>
        /// <param name="c">Character to look up</param>
        /// <returns>Combining class for this character</returns>
        public static int Class(char c) 
        {
            if (!s_init)
            {
                lock (s_lock)
                {
                    if (!s_init)
                    {
                        s_classes = (byte[,]) ResourceLoader.LoadRes("Combining.Classes");
                        s_pages = (byte[]) ResourceLoader.LoadRes("Combining.Pages");
                        s_init = true;
                    }
                }
            }
            int page = c >> 8;
            if (s_pages[page] == 255)
                return 0;
            else
                return s_classes[s_pages[page], c & 0xff];
        }
	}
}
