using System;
using stringprep.steps;

namespace stringprep
{
	/// <summary>
	/// Summary description for XmppNode.
	/// </summary>
    public class XmppNode : Profile
    {
        private static readonly ProhibitStep XmppNodeprepProhibit;
        
        static XmppNode()
        {
            XmppNodeprepProhibit = new ProhibitStep(new Prohibit[] 
                {
                    new Prohibit('"'),
                    new Prohibit('&'),
                    new Prohibit('\''),
                    new Prohibit('/'),
                    new Prohibit(':'),
                    new Prohibit('<'),
                    new Prohibit('>'),
                    new Prohibit('@')
                }, "XMPP Node");
        }

        public XmppNode() : 
            base( new ProfileStep[] {   B_1, B_2, NFKC,
                                        C_1_1, C_1_2, C_2_1, C_2_2,
                                        C_3, C_4, C_5, C_6, C_7, C_8, C_9,
                                        XmppNodeprepProhibit,
                                        BIDI, UNASSIGNED} )
		{
		}
	}
}
