using System;

namespace stringprep.unicode
{
	/// <summary>
	/// Summary description for Util.
	/// </summary>
	public class Util
	{
        /// <summary>
        /// The requested character was not found.
        /// </summary>
        public const char NOT_FOUND = '\xffff';

        /// <summary>
        /// Look up the given character in a character pair string, 
        /// and return the following character.
        /// 
        /// Character pair strings are used to reduce start-up
        /// time and .dll size, relative to char[,2].  They consist 
        /// of tuples of characters, one after the other, that make 
        /// up a map from the first character to the second character
        /// of each pair.
        /// </summary>
        /// <param name="ch">Character to look up in the first character
        /// of each pair</param>
        /// <param name="lookup">The character pair string to look in.</param>
        /// <returns>0xffff if not found</returns>
        public static char Find(char ch, string lookup)
        {
            int start = 0;
            int end = lookup.Length;

            if ((ch < lookup[0]) || (ch > lookup[end - 2]))
                return NOT_FOUND;

            int half;
            while (true)
            {
                half = ((start + end) / 4) * 2;
                if (ch == lookup[half])
                    return lookup[half+1];
                else if (half == start)
                    break; // done
                else if (ch > lookup[half])
                    start = half;
                else
                    end = half;
            }

            return NOT_FOUND;
        }    
    }
}
