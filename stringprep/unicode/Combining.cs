/* --------------------------------------------------------------------------
 * Copyrights
 * 
 * Portions created by or assigned to Cursive Systems, Inc. are 
 * Copyright (c) 2002-2004 Cursive Systems, Inc.  All Rights Reserved.  Contact
 * information for Cursive Systems, Inc. is available at
 * http://www.cursive.net/.
 *
 * License
 * 
 * Jabber-Net can be used under either JOSL or the GPL.  
 * See LICENSE.txt for details.
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
