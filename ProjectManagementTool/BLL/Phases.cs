using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectManagementTool.BLL
{
    public class Phases
    {
        private DBGetData getdt;
        public Phases()
        {
            getdt = new DBGetData();
        }
        public string GetPhaseforStatus(string actualDocumentUID, string currentStatus)
        {
            string SubmittalUID = getdt.GetSubmittalUID_By_ActualDocumentUID(new Guid(actualDocumentUID));
            string phase = getdt.GetPhaseforStatus(new Guid(getdt.GetFlowUIDBySubmittalUID(new Guid(SubmittalUID))), currentStatus);
            if (string.IsNullOrEmpty(phase))
            {
                phase = currentStatus;
                //if (currentStatus == "Code A-CE Approval" || currentStatus == "Client CE GFC Approval")
                //{
                //    phase = "Approved";
                //}
                //if (currentStatus == "Code B-CE Approval" || currentStatus == "Code C-CE Approval")
                //{
                //    phase = "Client Approval";
                //}
                if (currentStatus == "Code A-CE Approval")
                {
                    phase = "Approved By BWSSB Under Code A";

                }
                else if (currentStatus == "Code B-CE Approval")
                {
                    phase = "Approved By BWSSB Under Code B";
                }
                else if (currentStatus == "Code C-CE Approval")
                {
                    phase = "Under Client Approval Process";

                }
                else if (currentStatus == "Client CE GFC Approval")
                {
                    phase = "Approved GFC by BWSSB";
                }

            }
            return phase;
        }
        
    }
}