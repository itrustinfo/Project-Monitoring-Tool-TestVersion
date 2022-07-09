using ProjectManagementTool.DAL;
using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagementTool._content_pages.report_mis_details
{
    public partial class _default : System.Web.UI.Page
    {   

        DBGetData getdt = new DBGetData();
        TaskUpdate gettk = new TaskUpdate();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Username"] == null)
            {
                Response.Redirect("~/Login.aspx");
            }
            if (!IsPostBack)
            {
                BindData();
            }
        }

        private void BindData()
        {
            string projectName = Request.QueryString["ProjectName"];
            string flowName = Request.QueryString["FlowName"];
            string type = Request.QueryString["type"];

            LblDocumentHeading.Text = "Document List > " + projectName + " > " + flowName + " > " + type;

            DateTime startDate = Convert.ToDateTime("01-" + DateTime.Now.ToString("MMM-yyyy"));
            DataTable dtStatus = getdt.DocumentStatusSummary_ByDate(startDate);

            List<string> reconciliationPendingStatus = new List<string>() { "Reconciliation", "Contractor Submitted 9 Copies" };

            DataTable dt = new DataTable();
            EnumerableRowCollection<DataRow> dataRows = null;
            if(type == "Total Submission")
            {
                dataRows = dtStatus.AsEnumerable().Where(r =>
                                                            r.Field<string>("ProjectName") == projectName &&
                                                            r.Field<string>("Flow_Name") == flowName);
            }
            else if(type == "Reconciliation Pending")
            {
                dataRows = dtStatus.AsEnumerable().Where(r =>
                                                 r.Field<string>("ProjectName") == projectName &&
                                                 r.Field<string>("Flow_Name") == flowName &&
                                                 reconciliationPendingStatus.Contains(r.Field<string>("ActualDocument_CurrentStatus")));
            }
            else if(type == "Reconciliation Rejected")
            {
                dataRows = dtStatus.AsEnumerable().Where(r =>
                                                 r.Field<string>("ProjectName") == projectName &&
                                                 r.Field<string>("Flow_Name") == flowName &&
                                                 !reconciliationPendingStatus.Contains(r.Field<string>("ActualDocument_CurrentStatus")) &&
                                                 Constants.ReconciliationRejectedStatus.Contains(r.Field<string>("ActualDocument_CurrentStatus")));
            }
            else if (type == "Reconciliation Accepted")
            {
                dataRows = dtStatus.AsEnumerable().Where(r =>
                                                 r.Field<string>("ProjectName") == projectName &&
                                                 r.Field<string>("Flow_Name") == flowName &&
                                                 !reconciliationPendingStatus.Contains(r.Field<string>("ActualDocument_CurrentStatus")) &&
                                                 !r.Field<string>("ActualDocument_CurrentStatus").Contains("Rejected"));
            }
            else if(type == "PMC Review")
            {
                dataRows = dtStatus.AsEnumerable().Where(r =>
                                                 r.Field<string>("ProjectName") == projectName &&
                                                 r.Field<string>("Flow_Name") == flowName &&
                                                 Constants.PmcReview.Contains(r.Field<string>("ActualDocument_CurrentStatus")));
            }
            else if(type == "Project Co-ordinator")
            {
                dataRows = dtStatus.AsEnumerable().Where(r =>
                                                 r.Field<string>("ProjectName") == projectName &&
                                                 r.Field<string>("Flow_Name") == flowName &&
                                                 Constants.underProjectCoordinatorReview.Contains(r.Field<string>("ActualDocument_CurrentStatus")));
            }
            else if(type == "Meeting with EE")
            {
                dataRows = dtStatus.AsEnumerable().Where(r =>
                                                 r.Field<string>("ProjectName") == projectName &&
                                                 r.Field<string>("Flow_Name") == flowName &&
                                                 r.Field<string>("ActualDocument_CurrentStatus") == "Meeting With EE or CE");
            }
            else if(type == "Review")
            {
                dataRows = dtStatus.AsEnumerable().Where(r =>
                                                 r.Field<string>("ProjectName") == projectName &&
                                                 r.Field<string>("Flow_Name") == flowName &&
                                                 Constants.DtlReview.Contains(r.Field<string>("ActualDocument_CurrentStatus")));
            }
            else if(type == "DTL Rejected")
            {
                dataRows = dtStatus.AsEnumerable().Where(r =>
                                                 r.Field<string>("ProjectName") == projectName &&
                                                 r.Field<string>("Flow_Name") == flowName &&
                                                 Constants.DtlRejected.Contains(r.Field<string>("ActualDocument_CurrentStatus")));
            }
            else if(type == "DTL Internal Meeting")
            {
                dataRows = dtStatus.AsEnumerable().Where(r =>
                                                 r.Field<string>("ProjectName") == projectName &&
                                                 r.Field<string>("Flow_Name") == flowName &&
                                                 r.Field<string>("ActualDocument_CurrentStatus") == "Internal Meeting called by DTL");
            }
            else if (type == "DTL Back To Contractor")
            {
                dataRows = dtStatus.AsEnumerable().Where(r =>
                                                 r.Field<string>("ProjectName") == projectName &&
                                                 r.Field<string>("Flow_Name") == flowName &&
                                                  Constants.DTLBacktoContractor.Contains(r.Field<string>("ActualDocument_CurrentStatus")));
            }
            else if(type == "AEE Approval")
            {
                dataRows = dtStatus.AsEnumerable().Where(r =>
                                                 r.Field<string>("ProjectName") == projectName &&
                                                 r.Field<string>("Flow_Name") == flowName &&
                                                 Constants.AeeApproval.Contains(r.Field<string>("ActualDocument_CurrentStatus")));
            }
            else if (type == "EE Approval")
            {
                dataRows = dtStatus.AsEnumerable().Where(r =>
                                                 r.Field<string>("ProjectName") == projectName &&
                                                 r.Field<string>("Flow_Name") == flowName &&
                                                 r.Field<string>("ActualDocument_CurrentStatus").Contains("AEE Approval"));
            }
            else if (type == "ACE Approval")
            {
                dataRows = dtStatus.AsEnumerable().Where(r =>
                                                 r.Field<string>("ProjectName") == projectName &&
                                                 r.Field<string>("Flow_Name") == flowName &&
                                                 r.Field<string>("ActualDocument_CurrentStatus").Contains(" EE Approval"));
            }
            else if (type == "CE Approval")
            {
                dataRows = dtStatus.AsEnumerable().Where(r =>
                                                 r.Field<string>("ProjectName") == projectName &&
                                                 r.Field<string>("Flow_Name") == flowName &&
                                                 r.Field<string>("ActualDocument_CurrentStatus").Contains("ACE Approval"));
            }
            else if (type == "Back to Contractor Stage 1")
            {
                dataRows = dtStatus.AsEnumerable().Where(r =>
                                                 r.Field<string>("ProjectName") == projectName &&
                                                 r.Field<string>("Flow_Name") == flowName &&
                                                 r.Field<string>("ActualDocument_CurrentStatus") == "Network Design EE Approval");
            }
            else if (type == "Client Approved")
            {
                dataRows = dtStatus.AsEnumerable().Where(r =>
                                                 r.Field<string>("ProjectName") == projectName &&
                                                 r.Field<string>("Flow_Name") == flowName &&
                                                 r.Field<string>("ActualDocument_CurrentStatus") == "Client CE GFC Approval");
            }
            else if (type == "Code A")
            {
                dataRows = dtStatus.AsEnumerable().Where(r =>
                                                 r.Field<string>("ProjectName") == projectName &&
                                                 r.Field<string>("Flow_Name") == flowName &&
                                                 r.Field<string>("ActualDocument_CurrentStatus") == "Code A-CE Approval");
            }
            else if (type == "Code B")
            {
                dataRows = dtStatus.AsEnumerable().Where(r =>
                                                 r.Field<string>("ProjectName") == projectName &&
                                                 r.Field<string>("Flow_Name") == flowName &&
                                                 r.Field<string>("ActualDocument_CurrentStatus") == "Code B-CE Approval");
            }
            else if (type == "Code C")
            {
                dataRows = dtStatus.AsEnumerable().Where(r =>
                                                 r.Field<string>("ProjectName") == projectName &&
                                                 r.Field<string>("Flow_Name") == flowName &&
                                                 r.Field<string>("ActualDocument_CurrentStatus") == "Code C-CE Approval");
            }
            else if (type == "Code D")
            {
                dataRows = dtStatus.AsEnumerable().Where(r =>
                                                 r.Field<string>("ProjectName") == projectName &&
                                                 r.Field<string>("Flow_Name") == flowName &&
                                                 r.Field<string>("ActualDocument_CurrentStatus") == "Code D-CE Approval");
            }



            if (dataRows != null && dataRows.FirstOrDefault() != null)
            {
                dt = dataRows.CopyToDataTable();
            }

            lblTotalcount.Text = "Total Document : " + dt.Rows.Count.ToString();
            GrdDocuments.DataSource = dt;
            GrdDocuments.DataBind();
        }

        public string GetTaskHierarchy_By_DocumentUID(string DocumentUID)
        {
            return getdt.GetTaskHierarchy_By_DocumentUID(new Guid(DocumentUID));
        }

        public string GetSubmittalName(string DocumentID)
        {
            return getdt.getDocumentName_by_DocumentUID(new Guid(DocumentID));
        }
        public string GetDocumentTypeIcon(string DocumentExtn)
        {
            return getdt.GetDocumentTypeMasterIcon_by_Extension(DocumentExtn);
        }

        public string GetDocumentName(string DocumentExtn)
        {
            string retval = getdt.GetDocumentMasterType_by_Extension(DocumentExtn);
            if (retval == null || retval == "")
            {
                return "N/A";
            }
            else
            {
                return retval;
            }
        }


        protected void GrdDocuments_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                DataSet ds = getdt.getTop1_DocumentStatusSelect(new Guid(e.Row.Cells[0].Text));
                Label lblDocumentName = (Label)e.Row.FindControl("lblDocumentName");

                DateTime? acceptedRejDate = getdt.GetDocumentAcceptedRecejtedDate(new Guid(e.Row.Cells[0].Text));
                if (acceptedRejDate != null)
                {
                    e.Row.Cells[14].Text = Convert.ToDateTime(acceptedRejDate).ToString("dd/MM/yyyy");
                }
                else
                {
                    e.Row.Cells[14].Text = "";
                }
                //
                if (ds != null)
                {


                    if (ds.Tables[0].Rows[0]["DocumentType"].ToString() == "General Document")
                    {
                        e.Row.Cells[8 + 2].Text = "No History";
                    }
                }
                if (ds.Tables[0].Rows[0]["ActivityType"].ToString() != "" && ds.Tables[0].Rows[0]["TopVersion"].ToString() != "")
                {
                    //e.Row.Cells[1].Text = ds.Tables[0].Rows[0]["TopVersion"].ToString();


                    string newVersionFileName = Path.GetFileNameWithoutExtension(Server.MapPath(ds.Tables[0].Rows[0]["Doc_Path"].ToString()));
                    lblDocumentName.Text = newVersionFileName.Substring(0, (newVersionFileName.Length - 2)) + " [ Ver. " + ds.Tables[0].Rows[0]["TopVersion"].ToString() + " ]";
                    e.Row.Cells[4 + 2].Text = ds.Tables[0].Rows[0]["ActivityType"].ToString();
                }
                //
                if (Session["IsContractor"].ToString() == "Y")
                {
                    string SubmittalUID = getdt.GetSubmittalUID_By_ActualDocumentUID(new Guid(e.Row.Cells[0].Text));
                    string phase = getdt.GetPhaseforStatus(new Guid(getdt.GetFlowUIDBySubmittalUID(new Guid(SubmittalUID))), e.Row.Cells[4 + 2].Text);
                    //string phase = getdata.GetPhaseforStatus(new Guid(Request.QueryString["FlowUID"]), e.Row.Cells[3].Text);

                    string Flowtype = getdt.GetFlowTypeBySubmittalUID(new Guid(SubmittalUID));
                    if (Flowtype == "STP")
                    {
                        if (string.IsNullOrEmpty(phase))
                        {

                            //if (e.Row.Cells[4].Text == "Code A-CE Approval" || e.Row.Cells[4].Text == "Client CE GFC Approval")
                            //{
                            //    e.Row.Cells[4].Text = "Approved";

                            //}
                            //if (e.Row.Cells[4].Text == "Code B-CE Approval" || e.Row.Cells[3].Text == "Code C-CE Approval")
                            //{
                            //    e.Row.Cells[4].Text = "Under Client Approval Process";
                            //}
                            //
                            if (e.Row.Cells[4 + 2].Text == "Code A-CE Approval")
                            {
                                e.Row.Cells[4 + 2].Text = "Approved By BWSSB Under Code A";

                            }
                            else if (e.Row.Cells[4 + 2].Text == "Code B-CE Approval")
                            {
                                e.Row.Cells[4 + 2].Text = "Approved By BWSSB Under Code B";
                            }
                            else if (e.Row.Cells[4 + 2].Text == "Code C-CE Approval")
                            {
                                e.Row.Cells[4 + 2].Text = "Under Client Approval Process";

                            }
                            else if (e.Row.Cells[4 + 2].Text == "Client CE GFC Approval")
                            {
                                e.Row.Cells[4 + 2].Text = "Approved GFC by BWSSB";
                            }
                        }
                        else
                        {
                            e.Row.Cells[4 + 2].Text = phase;
                        }
                    }

                }
                //
                if (e.Row.Cells[4 + 2].Text.Contains("Reconciliation"))
                {
                    //e.Row.Cells[1].Text = GetSubmittalName(getdt.GetSubmittalUID_By_ActualDocumentUID(new Guid(e.Row.Cells[0].Text)));
                }
                //
                if (Request.QueryString["UserUID"] != null)
                {
                    e.Row.Visible = false;
                    DataSet dsNext = getdt.GetNextStep_By_DocumentUID(new Guid(e.Row.Cells[0].Text), ds.Tables[0].Rows[0]["ActivityType"].ToString());
                    DataSet dsNxtUser = new DataSet();
                    foreach (DataRow dr in dsNext.Tables[0].Rows)
                    {
                        dsNxtUser = getdt.GetNextUser_By_DocumentUID(new Guid(e.Row.Cells[0].Text), int.Parse(dr["ForFlow_Step"].ToString()));
                        if (dsNxtUser.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow druser in dsNxtUser.Tables[0].Rows)
                            {
                                if (Session["UserUID"].ToString().ToUpper() == druser["Approver"].ToString().ToUpper())
                                {
                                    e.Row.Visible = true;
                                    return;
                                }
                                else
                                {
                                    e.Row.Visible = false;

                                }
                            }
                        }
                        else
                        {
                            e.Row.Visible = false;
                        }
                    }
                }
                //
                if(e.Row.Cells[10].Text == "Mahadevpura" || e.Row.Cells[10].Text == "Mahadevpura")
                {
                    e.Row.Cells[10].Text = "Works B";
                }
            }
        }
        protected void GrdDocuments_DataBound(object sender, EventArgs e)
        {
        }
        protected void GrdDocuments_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string UID = e.CommandArgument.ToString();
            if (e.CommandName == "download")
            {
                string path = string.Empty;
                string filename = string.Empty;
                DataSet ds1 = null;
                DataSet ds = getdt.getLatest_DocumentVerisonSelect(new Guid(UID));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    path = Server.MapPath(ds.Tables[0].Rows[0]["Doc_FileName"].ToString());
                    //File.Decrypt(path);
                }
                else
                {
                    ds1 = getdt.ActualDocuments_SelectBy_ActualDocumentUID(new Guid(UID));
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        if (ds1.Tables[0].Rows[0]["ActualDocument_Path"].ToString() != "")
                        {
                            path = Server.MapPath(ds1.Tables[0].Rows[0]["ActualDocument_Path"].ToString());
                        }
                    }
                }
                // added on  20/10/2020
                ds.Clear();
                ds = getdt.ActualDocuments_SelectBy_ActualDocumentUID(new Guid(UID));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["ActualDocument_Name"].ToString() != "")
                    {
                        filename = ds.Tables[0].Rows[0]["ActualDocument_Name"].ToString() + ds.Tables[0].Rows[0]["ActualDocument_Type"].ToString();
                    }
                }
                //
                try
                {
                    string getExtension = System.IO.Path.GetExtension(path);
                    string outPath = path.Replace(getExtension, "") + "_download" + getExtension;
                    getdt.DecryptFile(path, outPath);
                    System.IO.FileInfo file = new System.IO.FileInfo(outPath);

                    if (file.Exists)
                    {
                        int Cnt = getdt.DocumentHistroy_InsertorUpdate(Guid.NewGuid(), new Guid(UID), new Guid(Session["UserUID"].ToString()), "Downloaded", "Documents");
                        if (Cnt <= 0)
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code: DDH-01. there is a problem with updating histroy. Please contact system admin.');</script>");
                        }
                        Response.Clear();

                        // Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
                        Response.AddHeader("Content-Disposition", "attachment; filename=" + filename);
                        Response.AddHeader("Content-Length", file.Length.ToString());

                        Response.ContentType = "application/octet-stream";

                        Response.WriteFile(file.FullName);

                        Response.Flush();

                        try
                        {
                            if (File.Exists(outPath))
                            {
                                File.Delete(outPath);
                            }
                        }
                        catch (Exception ex)
                        {
                            //throw
                        }

                        Response.End();

                    }
                    else
                    {

                        //Response.Write("This file does not exist.");
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('File does not exists.');</script>");

                    }
                }
                catch (Exception ex)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Document not uploaded for this flow.Please Contact Document controller!');</script>");
                }
            }
        }
    }
}