using CISanityTester.Sessions;

namespace CISanityTester.SolutionExplorer.Scripts
{
    [TestSuit]
    class _15_4_DTNX
    {
        string branchname = "main";
        
 
        public void AT2173()
        {
            // create new server session
            ServerSession ne74_tl1 = SessionExplorer.GetSession("10.220.17.74", 9090);
            // login 
            ne74_tl1.Write("act-username::secadmin:c::Infinera");
            // Get latest build version
            string output = CMD.Write("p4 counter " + branchname + "_ppc_nebuilds_success");
            

        }
    }
}
