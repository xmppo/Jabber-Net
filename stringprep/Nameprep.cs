using System;
using stringprep.steps;

namespace stringprep
{
	/// <summary>
	/// Summary description for Nameprep.
	/// </summary>
	public class Nameprep : Profile
	{
        public Nameprep() : 
            base( new ProfileStep[] {   B_1, B_2, NFKC,
                                        C_1_2, C_2_2, C_3, C_4, C_5, C_6, C_7, C_8, C_9,
                                        BIDI, UNASSIGNED} )
        {
        }
	}
}
