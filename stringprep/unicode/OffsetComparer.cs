using System;
using System.Collections;

namespace stringprep.unicode
{
	/// <summary>
	/// Compare a character to the first character in a char array.
	/// </summary>
	public class OffsetComparer : IComparer
	{
        #region IComparer Members
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int Compare(object x, object y)
        {
            char[] cx = (char[]) x;
            char cy = (char) y;

            return cx[0].CompareTo(cy);
        }
        #endregion
    }
}
