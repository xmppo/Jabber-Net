using System;

namespace stringprep.unicode
{
	public class Compose
	{
        private static int Index(char c)
        {
            int p = c >> 8;
            if (ComposeData.Table[p] >= DecomposeData.MAX_TABLE_INDEX)
                return ComposeData.Table[p] - DecomposeData.MAX_TABLE_INDEX;
            else
                return ComposeData.Data[ComposeData.Table[p], c & 0xff];
        }

        public static bool Combine(char a, char b, out char result)
        {
            int index_a, index_b;

            index_a = Index(a);
            if ((index_a >= ComposeData.FIRST_SINGLE_START) && (index_a < ComposeData.SECOND_START))
            {
                if (b == ComposeData.FirstSingle[index_a - ComposeData.FIRST_SINGLE_START, 0])
                {
                    result = ComposeData.FirstSingle[index_a - ComposeData.FIRST_SINGLE_START, 1];
                    return true;
                }
                else
                {
                    result = '\x0';
                    return false;
                }
            }

            index_b = Index(b);
            if (index_b >= ComposeData.SECOND_SINGLE_START)
            {
                if (a ==
                    ComposeData.SecondSingle[index_b - ComposeData.SECOND_SINGLE_START, 0])
                {
                    result =  ComposeData.SecondSingle[index_b - ComposeData.SECOND_SINGLE_START, 1];
                    return true;
                }
                else
                {
                    result = '\x0';
                    return false;
                }
            }

            if ((index_a >= ComposeData.FIRST_START) && 
                (index_a < ComposeData.FIRST_SINGLE_START) && 
                (index_b >= ComposeData.SECOND_START) && 
                (index_a < ComposeData.SECOND_SINGLE_START))
            {
                char res = ComposeData.Array[index_a - ComposeData.FIRST_START, 
                    index_b - ComposeData.SECOND_START];

                if (res != '\x0')
                {
                    result = res;
                    return true;
                }
            }

            result = '\x0';
            return false;
        }
    }
}
