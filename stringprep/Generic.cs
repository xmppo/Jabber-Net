using System;
using stringprep.steps;

namespace stringprep
{
	/// <summary>
	/// Summary description for Generic.
	/// </summary>
    public class Generic : Profile
    {
        public const string foo = "FOO!";
        public Generic() : 
            base( new ProfileStep[] {   B_1, B_2, B_3, NFKC,
                                        C_1_1, C_1_2, C_2_1, C_2_2,
                                        C_3, C_4, C_5, C_6, C_7, C_8, C_9,
                                        BIDI, UNASSIGNED} )
        {
        }
    }
}
