using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace ProjectManagementTool.DAL
{
    public class Constants
    {
        public enum UserTypeEnum
        {
            Contractor = 1,
            Client = 2,
            ONTB = 3,
            NJSEI = 4
        }

        public static List<string> ProjectsForPhaseSearch = ConfigurationManager.AppSettings["ProjectsForPhaseSearch"].ToString().Split(',').ToList();
        public static string ProjectReferenceName = ConfigurationManager.AppSettings["Domain"].ToString() == "NJSEI" ? "NJSEI Reference #" : "ONTB Reference #";

        /// <summary>
        /// This dictionary is for the final phases for the status. Key is status and value is phases
        /// </summary>
        public static Dictionary<string, string> DicFinalStatusAndPhase = new Dictionary<string, string>()
        {
            { "Code A-CE Approval", "Approved By BWSSB Under Code A" },
            { "Code B-CE Approval", "Approved By BWSSB Under Code B"},
            { "Code C-CE Approval", "Under Client Approval Process" },
            { "Client CE GFC Approval", "Approved GFC by BWSSB" },
        };
    }
}