using System;
using stringprep.steps;

namespace stringprep
{
	public class Plain : Profile
	{
        public Plain() : 
            base( new ProfileStep[] {   C_2_1, C_2_2, 
                                        C_3, C_4, C_5, C_6, C_8, C_9,
                                        BIDI } )
		{
		}
	}
}
