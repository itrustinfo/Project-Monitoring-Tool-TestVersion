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

        public static Dictionary<string, string> NameOfWorkMapWithProjectNameForDailyReport = new Dictionary<string, string>()
        {
            { "CP-26", "JICA Assisted Bengaluru Water Supply and Sewerage Project (Phase III) – CP 26: Sewerage Facilities to 110 Villages: Design, Engineering, Construction and Commissioning of Sewerage Treatment Plants and Intermediate Sewerage Pumping Stations with Operation & Maintenance thereof for Seven Years (Works-A) and procurement and constructions of main sewer including Manholes in Bommanahalli & Mahadevapura Zones (K & C Valley Catchment) (Works B) under JICA Loan ID – P266" }
        };

        public static List<string> ReconciliationPendingStatus = new List<string>() { "Reconciliation", "Contractor Submitted 9 Copies" };
        public static List<string> ReconciliationRejectedStatus = new List<string>() { "Rejected", "Rejected 9 Copies" };
        public static List<string> underProjectCoordinatorReview = new List<string>() { "Accepted-PMC Comments", "Network Design by ONTB", "Review By ONTB"};
        public static List<string> DtlRejected = new List<string>() { "Rejected by Client", "Rejected by PMC" };
        public static List<string> DtlReview = new List<string>() { "Recommended-Code A",
                                                                    "Recommended-Code B",
                                                                    "Recommended-Code C",
                                                                    "Recommended-Code D",
                                                                    "Network Design Reviewed by Project Co-ordinator",
                                                                    "Recommend-Rejected by Client",
                                                                    "Recommend-Rejected by PMC",
        "Back to DTL by AEE","Back to DTL by EE","No Action by PC","PC Recommended-Code A","PC Recommended-Code B","PC Recommended-Code C","PC Recommended-Code D"};
        public static List<string> AeeApproval = new List<string>() { "Code A", "Code B", "Network Design DTL Reviewed", "ONTB DTL Verified" };
        public static List<string> DTLBacktoContractor = new List<string>() { "Code C", "Code D","Document Rejected sent Back to Contractor"};

        public static List<string> PmcReview = new List<string>() { "Accepted", "Accepted 9 Copies", "Back to PMC by DTL","Back to PMC by PC"};
    }
}