
using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace ProjectManagementTool._modal_pages
{
    public partial class view_documenthistory : System.Web.UI.Page
    {
        DBGetData getdata = new DBGetData();
        DataSet ds = new DataSet();
        string next = string.Empty;
        string prevstatus = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Username"] == null)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
            }
            else
            {
                DataSet ds = getdata.getTop1_DocumentStatusSelect(new Guid(Request.QueryString["DocID"].ToString()));
                ViewState["isHide"] = false;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["ActivityType"].ToString() == "Code A")
                    {
                        if (Session["isDownloadNJSE"].ToString() == "false")
                        {
                            GrdDocStatus.Columns[6].Visible = false;
                            GrdDocStatus.Columns[8].Visible = false;
                            ViewState["isHide"] = "true";

                        }
                    }
                    else if (ds.Tables[0].Rows[0]["ActivityType"].ToString().Contains("Approved"))
                    {
                        if (Session["isDownloadNJSE"].ToString() == "false")
                        {
                            GrdDocStatus.Columns[6].Visible = false;
                            GrdDocStatus.Columns[8].Visible = false;
                            ViewState["isHide"] = "true";
                        }
                    }
                }
                if (!IsPostBack)
                {
                    //grdDocuments.DataSource = getdata.getDocumentsbyDocID(new Guid(Request.QueryString["DocID"].ToString()));
                    //grdDocuments.DataBind();
                    //

                    //
                    AddStatus.HRef = "/_modal_pages/add-documentstatus.aspx?DocID=" + Request.QueryString["DocID"].ToString() + "&ProjectUID=" + Request.QueryString["ProjectUID"] + "&FlowUID=" + Request.QueryString["FlowUID"];
                    BindDocument();
                    BindDocStatus();
                    ShowDocumentProcessTaken_in_Days(new Guid(Request.QueryString["DocID"].ToString()));
                    HideorShowAddStatusButton();

                    if (Session["IsClient"].ToString() == "Y")
                    {
                        GrdDocStatus.Columns[10].Visible = false;
                        GrdDocStatus.Columns[9].Visible = false;

                    }
                    else if (Session["IsContractor"].ToString() == "Y")
                    {
                        GrdDocStatus.Columns[10].Visible = false;
                       
                    }
                    else if (Session["IsONTB"].ToString() == "Y")
                    {
                        GrdDocStatus.Columns[9].Visible = false;
                        GrdDocStatus.Columns[10].Visible = false;

                    }

                    if (Session["TypeOfUser"].ToString() == "U")
                    {
                        GrdDocStatus.Columns[10].Visible = true;
                    }
                    else
                    {
                        GrdDocStatus.Columns[10].Visible = false;
                    }
                }
            }
        }

        public void ShowDocumentProcessTaken_in_Days(Guid DocumentUID)
        {
            DataSet ds = getdata.GetDocumentProcess_in_Days(DocumentUID);
            if (ds.Tables[0].Rows.Count > 0)
            {
                LblTotalDays.Text = "Total Numer of Days taken : " + ds.Tables[0].Rows[0]["TotDays"].ToString();
            }
        }
        private void BindDocument()
        {
            //DataSet ds = getdata.getDocumentsbyDocID(new Guid(Request.QueryString["DocID"].ToString()));
            DataSet ds = getdata.ActualDocuments_SelectBy_ActualDocumentUID(new Guid(Request.QueryString["DocID"].ToString()));
            if (ds.Tables[0].Rows.Count > 0)
            {
                LblDocName.Text = ds.Tables[0].Rows[0]["ActualDocument_Name"].ToString();
                lblWorkPackage.Text = getdata.getWorkPackageNameby_WorkPackageUID(new Guid(ds.Tables[0].Rows[0]["WorkPackageUID"].ToString()));

                //if (ds.Tables[0].Rows[0]["ActualDocument_CurrentStatus"].ToString() == "Client Approved" || ds.Tables[0].Rows[0]["ActualDocument_CurrentStatus"].ToString() == "Client Approve")
                //{
                //    AddStatus.Visible = false;
                //}
                //else
                //{
                //    AddStatus.Visible = true;
                //}
                DataSet dsFlow = getdata.GetDocumentFlows_by_UID(new Guid(ds.Tables[0].Rows[0]["FlowUID"].ToString()));
                if (dsFlow.Tables[0].Rows.Count > 0)
                {
                    //lblSubmissionUser.Text = getdata.getUserNameby_UID(new Guid(ds.Tables[0].Rows[0]["SubmissionUserUID"].ToString()));
                    //lblSubmissionTargetDate.Text = ds.Tables[0].Rows[0]["Sub_TargetDate"].ToString() != "" ? Convert.ToDateTime(ds.Tables[0].Rows[0]["Sub_TargetDate"].ToString()).ToString("dd/MM/yyyy") : "";
                    if (dsFlow.Tables[0].Rows[0]["Steps_Count"].ToString() == "2")
                    {
                        //lblQulaityEngg.Text = getdata.getUserNameby_UID(new Guid(ds.Tables[0].Rows[0]["QuallityEng_UID"].ToString()));
                        //lblQualityTargetDate.Text = ds.Tables[0].Rows[0]["QuallityEng_TargetDate"].ToString() != "" ? Convert.ToDateTime(ds.Tables[0].Rows[0]["QuallityEng_TargetDate"].ToString()).ToString("dd/MM/yyyy") : "";
                    }
                    else if (dsFlow.Tables[0].Rows[0]["Steps_Count"].ToString() == "3")
                    {
                        //lblReviewUser.Text = getdata.getUserNameby_UID(new Guid(ds.Tables[0].Rows[0]["Reviewer_B_UID"].ToString()));
                        //lblReviewTargetDate.Text = ds.Tables[0].Rows[0]["Reviewer_B_TragetDate"].ToString() != "" ? Convert.ToDateTime(ds.Tables[0].Rows[0]["Reviewer_B_TragetDate"].ToString()).ToString("dd/MM/yyyy") : "";
                        //lblQulaityEngg.Text = getdata.getUserNameby_UID(new Guid(ds.Tables[0].Rows[0]["QuallityEng_UID"].ToString()));
                        //lblQualityTargetDate.Text = ds.Tables[0].Rows[0]["QuallityEng_TargetDate"].ToString() != "" ? Convert.ToDateTime(ds.Tables[0].Rows[0]["QuallityEng_TargetDate"].ToString()).ToString("dd/MM/yyyy") : "";
                        //lblApprovalUser.Text = getdata.getUserNameby_UID(new Guid(ds.Tables[0].Rows[0]["ApprovalUser_UID"].ToString()));
                        //lblApprovalTargetDate.Text = ds.Tables[0].Rows[0]["App_TargetDate"].ToString() != "" ? Convert.ToDateTime(ds.Tables[0].Rows[0]["App_TargetDate"].ToString()).ToString("dd/MM/yyyy") : "";
                    }
                    else if (dsFlow.Tables[0].Rows[0]["Steps_Count"].ToString() == "4")
                    {
                        //lblReviewUser.Text = getdata.getUserNameby_UID(new Guid(ds.Tables[0].Rows[0]["ReviewerUserUID"].ToString()));
                        //lblReviewTargetDate.Text = ds.Tables[0].Rows[0]["Review_TargetDate"].ToString() != "" ? Convert.ToDateTime(ds.Tables[0].Rows[0]["Review_TargetDate"].ToString()).ToString("dd/MM/yyyy") : "";
                        //lblQulaityEngg.Text = getdata.getUserNameby_UID(new Guid(ds.Tables[0].Rows[0]["QuallityEng_UID"].ToString()));
                        //lblQualityTargetDate.Text = ds.Tables[0].Rows[0]["QuallityEng_TargetDate"].ToString() != "" ? Convert.ToDateTime(ds.Tables[0].Rows[0]["QuallityEng_TargetDate"].ToString()).ToString("dd/MM/yyyy") : "";
                        //lblApprovalUser.Text = getdata.getUserNameby_UID(new Guid(ds.Tables[0].Rows[0]["ApprovalUser_UID"].ToString()));
                        //lblApprovalTargetDate.Text = ds.Tables[0].Rows[0]["App_TargetDate"].ToString() != "" ? Convert.ToDateTime(ds.Tables[0].Rows[0]["App_TargetDate"].ToString()).ToString("dd/MM/yyyy") : "";
                    }
                    else
                    {
                        //lblReviewUser.Text = getdata.getUserNameby_UID(new Guid(ds.Tables[0].Rows[0]["ReviewerUserUID"].ToString()));
                        //lblReviewTargetDate.Text = ds.Tables[0].Rows[0]["Review_TargetDate"].ToString() != "" ? Convert.ToDateTime(ds.Tables[0].Rows[0]["Review_TargetDate"].ToString()).ToString("dd/MM/yyyy") : "";
                        //lblQulaityEngg.Text = getdata.getUserNameby_UID(new Guid(ds.Tables[0].Rows[0]["QuallityEng_UID"].ToString()));
                        //lblQualityTargetDate.Text = ds.Tables[0].Rows[0]["QuallityEng_TargetDate"].ToString() != "" ? Convert.ToDateTime(ds.Tables[0].Rows[0]["QuallityEng_TargetDate"].ToString()).ToString("dd/MM/yyyy") : "";
                        //lblApprovalUser.Text = getdata.getUserNameby_UID(new Guid(ds.Tables[0].Rows[0]["ApprovalUser_UID"].ToString()));
                        //lblApprovalTargetDate.Text = ds.Tables[0].Rows[0]["App_TargetDate"].ToString() != "" ? Convert.ToDateTime(ds.Tables[0].Rows[0]["App_TargetDate"].ToString()).ToString("dd/MM/yyyy") : "";
                    }
                }


            }
        }
        //protected void BindDocStatus()
        //{
        //    //GrdDocStatus.DataSource = getdata.getDocumentStatusList(new Guid(Request.QueryString["DocID"].ToString()));

        //    DataSet documentSTatusList = getdata.getActualDocumentStatusList(new Guid(Request.QueryString["DocID"].ToString()));
        //    if (Session["IsContractor"].ToString() == "Y")
        //    {
        //        DataTable dtNewTable = new DataTable();
        //        dtNewTable.Columns.Add("Sl");
        //        dtNewTable.Columns.Add("StatusUID", typeof(Guid));
        //        dtNewTable.Columns.Add("ActivityType");
        //        dtNewTable.Columns.Add("Phase");
        //        var phaseAndStatus = getdata.GetPhaseAndCurrentStatusByDocument(new Guid(Request.QueryString["DocID"].ToString()));
        //        int counter = 0;
        //        foreach(DataRow dr in documentSTatusList.Tables[0].Rows)
        //        {
        //            var statusUID = dr["StatusUID"].ToString();
        //            string phaseForCurrentSTatus = string.Empty;
        //            var phase = phaseAndStatus.AsEnumerable().Where(r => r.Field<string>("Current_Status") == dr["ActivityType"].ToString()).FirstOrDefault();
        //            if(phase != null)
        //            {
        //                phaseForCurrentSTatus = phase[1].ToString();
        //            }
        //            dtNewTable.Rows.Add(counter.ToString(), new Guid( dr["StatusUID"].ToString()), dr["ActivityType"].ToString(), phaseForCurrentSTatus);
        //            counter++;
        //        }

        //        List<Guid> StatusUIDToRemove = new List<Guid>();

        //        //First remove all the empty phases
        //        var EmptyPhases = dtNewTable.AsEnumerable().Where(r => r.Field<string>("Phase") == string.Empty);
        //        if(EmptyPhases.Any())
        //        {
        //            StatusUIDToRemove = EmptyPhases.Select(r => r.Field<Guid>("StatusUID")).ToList();
        //            dtNewTable.AsEnumerable().Where(x => x.Field<string>("Phase") == string.Empty).ToList().ForEach(r => r.Delete());
        //        }

        //        var groupByPhase = dtNewTable.AsEnumerable().GroupBy(r => r.Field<string>("Phase")).Select(r => new{r.Key, r});


        //        foreach(var eachPhase in groupByPhase)
        //        {
        //            var eachItem = eachPhase.r.ToList();
        //            if(eachItem.Count > 1)
        //            {
        //                for (int count = 0; count < eachItem.Count -1; count++ )
        //                {
        //                    StatusUIDToRemove.Add(new Guid(eachItem[count]["StatusUID"].ToString()));
        //                }
        //            }
        //        }
        //        if(StatusUIDToRemove.Count > 0)
        //        {
        //            documentSTatusList.Tables[0].AsEnumerable().Where(x => StatusUIDToRemove.Contains(x.Field<Guid>("StatusUID"))).ToList().ForEach(r => r.Delete());
        //        }
        //    }

        //    GrdDocStatus.DataSource = documentSTatusList;
        //    GrdDocStatus.DataBind();

        //}

        //changed on 29/03/2022
        protected void BindDocStatus()
        {
            //GrdDocStatus.DataSource = getdata.getDocumentStatusList(new Guid(Request.QueryString["DocID"].ToString()));

            DataSet documentSTatusList = getdata.getActualDocumentStatusList(new Guid(Request.QueryString["DocID"].ToString()));
            string SubmittalUID = getdata.GetSubmittalUID_By_ActualDocumentUID(new Guid(Request.QueryString["DocID"].ToString()));
            string Flowtype = getdata.GetFlowTypeBySubmittalUID(new Guid(SubmittalUID));
            if (Flowtype == "STP")
            {
                if (Session["IsContractor"].ToString() == "Y")
                {
                    DataTable dtNewTable = new DataTable();
                    dtNewTable.Columns.Add("Sl");
                    dtNewTable.Columns.Add("StatusUID", typeof(Guid));
                    dtNewTable.Columns.Add("ActivityType");
                    dtNewTable.Columns.Add("Phase");
                    var phaseAndStatus = getdata.GetPhaseAndCurrentStatusByDocument(new Guid(Request.QueryString["DocID"].ToString()));
                    int counter = 0;
                    foreach (DataRow dr in documentSTatusList.Tables[0].Rows)
                    {
                        var statusUID = dr["StatusUID"].ToString();
                        string phaseForCurrentSTatus = string.Empty;
                        var phase = phaseAndStatus.AsEnumerable().Where(r => r.Field<string>("Current_Status") == dr["ActivityType"].ToString()).FirstOrDefault();
                        if (phase != null)
                        {
                            phaseForCurrentSTatus = phase[1].ToString();
                        }
                        dtNewTable.Rows.Add(counter.ToString(), new Guid(dr["StatusUID"].ToString()), dr["ActivityType"].ToString(), phaseForCurrentSTatus);
                        counter++;
                    }

                    List<Guid> StatusUIDToRemove = new List<Guid>();
                    DataTable dtNewIntermediate = dtNewTable.Copy();
                    //First remove all the empty phases
                    var EmptyPhases = dtNewTable.AsEnumerable().Where(r => r.Field<string>("Phase") == string.Empty);
                    if (EmptyPhases.Any())
                    {
                        foreach (var eachEmptyPhase in EmptyPhases)
                        {
                            string activity = eachEmptyPhase["ActivityType"].ToString();
                            if (activity.ToLower().Contains("client ce") || activity.ToLower().Contains("code a-ce approval"))
                            {

                            }
                            else
                            {
                                StatusUIDToRemove.Add(new Guid(eachEmptyPhase["StatusUID"].ToString()));
                            }
                        }
                        dtNewIntermediate.AsEnumerable().Where(x => StatusUIDToRemove.Contains(x.Field<Guid>("StatusUID"))).ToList().ForEach(r => r.Delete());
                    }

                    var groupByPhase = dtNewTable.AsEnumerable().GroupBy(r => r.Field<string>("Phase").ToLower()).Select(r => new { r.Key, r });
                    foreach (var eachPhase in groupByPhase)
                    {
                        var eachItem = eachPhase.r.ToList();
                        if (eachItem.Count > 1)
                        {
                            if (eachPhase.Key == "reconciliation")
                            {
                                //for (int count = 1; count < eachItem.Count; count++)
                                //{
                                //    StatusUIDToRemove.Add(new Guid(eachItem[count]["StatusUID"].ToString()));
                                //}
                            }
                            else
                            {
                                for (int count = 0; count < eachItem.Count - 1; count++)
                                {
                                    StatusUIDToRemove.Add(new Guid(eachItem[count]["StatusUID"].ToString()));
                                }
                            }
                        }
                    }
                    if (StatusUIDToRemove.Count > 0)
                    {
                        dtNewIntermediate.AsEnumerable().Where(x => StatusUIDToRemove.Contains(x.Field<Guid>("StatusUID"))).ToList().ForEach(r => r.Delete());

                        string statusIDRemove = RemovePhaseFromDataTable("Approved", "Under Client Approval Process", dtNewIntermediate);
                        if (!string.IsNullOrEmpty(statusIDRemove))
                            StatusUIDToRemove.Add(new Guid(statusIDRemove));

                        statusIDRemove = RemovePhaseFromDataTable("Network Design DTL Reviewed", "Network Design by ONTB", dtNewIntermediate);
                        if (!string.IsNullOrEmpty(statusIDRemove))
                            StatusUIDToRemove.Add(new Guid(statusIDRemove));

                        statusIDRemove = RemovePhaseFromDataTable("ONTB DTL Approved", "Review by ONTB", dtNewIntermediate);
                        if (!string.IsNullOrEmpty(statusIDRemove))
                            StatusUIDToRemove.Add(new Guid(statusIDRemove));

                        statusIDRemove = RemovePhaseFromDataTable("ONTB DTL Approved", "ONTB Specialist Verified", dtNewIntermediate);
                        if (!string.IsNullOrEmpty(statusIDRemove))
                            StatusUIDToRemove.Add(new Guid(statusIDRemove));

                        statusIDRemove = RemovePhaseFromDataTable("Approved by ONTB", "Review by ONTB", dtNewIntermediate);
                        if (!string.IsNullOrEmpty(statusIDRemove))
                            StatusUIDToRemove.Add(new Guid(statusIDRemove));




                        dtNewIntermediate.AsEnumerable().Where(x => StatusUIDToRemove.Contains(x.Field<Guid>("StatusUID"))).ToList().ForEach(r => r.Delete());

                        documentSTatusList.Tables[0].AsEnumerable().Where(x => StatusUIDToRemove.Contains(x.Field<Guid>("StatusUID"))).ToList().ForEach(r => r.Delete());
                    }
                }
            }

            GrdDocStatus.DataSource = documentSTatusList;
            GrdDocStatus.DataBind();

        }

        private string RemovePhaseFromDataTable(string PhaseExist, string RemovePhase, DataTable dtNewIntermediate)
        {
            string StatusUIDToRemove = string.Empty;
            var isPhaseExist = dtNewIntermediate.AsEnumerable().Where(r => r.Field<string>("Phase").ToLower().Contains(PhaseExist.ToLower())).FirstOrDefault();
            if (isPhaseExist != null)
            {
                var isRemovePhaseExist = dtNewIntermediate.AsEnumerable().Where(r => r.Field<string>("Phase").ToLower() == RemovePhase.ToLower()).FirstOrDefault();
                if (isRemovePhaseExist != null)
                {
                    StatusUIDToRemove = isRemovePhaseExist["StatusUID"].ToString();
                }
            }
            return StatusUIDToRemove;
        }

        private void HideorShowAddStatusButton()
        {
            DataSet ds = getdata.getTop1_DocumentStatusSelect(new Guid(Request.QueryString["DocID"]));

            // Aruns old code commented out
            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    if (ds.Tables[0].Rows[0]["ActivityType"].ToString() == "Code A" || ds.Tables[0].Rows[0]["ActivityType"].ToString() =="Reply" || ds.Tables[0].Rows[0]["ActivityType"].ToString() == "Received")
            //    {
            //        AddStatus.Visible = true;
                //  string Approver = getdata.GetSubmittal_Approver_By_DocumentUID(new Guid(Request.QueryString["DocID"]));
            //        if (Approver != "")
            //        {
            //            if (Session["UserUID"].ToString().ToUpper() != Approver.ToUpper())
            //            {
            //                AddStatus.Visible = false;
            //            }
            //            else
            //            {
            //                AddStatus.Visible = true;
            //            }
            //        }
            //    }
            //    //else if (ds.Tables[0].Rows[0]["ActivityType"].ToString() == "Client Approved")
            //    //{
            //    //    AddStatus.Visible = false;
            //    //}
            //    else
            //    {
            //        string Reviewer = getdata.GetSubmittal_Reviewer_By_DocumentUID(new Guid(Request.QueryString["DocID"]));
            //        if (Reviewer != "")
            //        {
            //            if (Session["UserUID"].ToString().ToUpper() != Reviewer.ToUpper())
            //            {
            //                AddStatus.Visible = false;
            //            }
            //            else
            //            {
            //                AddStatus.Visible = true;
            //            }
            //        }
            //    }
            //}

            // New Code for next user check for status change on 06/02/2022
            DataSet dsNext = getdata.GetNextStep_By_DocumentUID(new Guid(Request.QueryString["DocID"]), ds.Tables[0].Rows[0]["ActivityType"].ToString());
            AddStatus.Visible = false;
            DataSet dsUser = new DataSet();
            foreach (DataRow dr in dsNext.Tables[0].Rows)
            {
                dsUser = getdata.GetNextUser_By_DocumentUID(new Guid(Request.QueryString["DocID"]),int.Parse(dr["ForFlow_Step"].ToString()));
                if (dsUser.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow druser in dsUser.Tables[0].Rows)
                    {
                        if (Session["UserUID"].ToString().ToUpper() == druser["Approver"].ToString().ToUpper())
                        {
                            AddStatus.Visible = true;
                            goto afterloop;
                        }
                        else
                        {
                            AddStatus.Visible = false;

                        }
                    }
                }
                else
                {
                    AddStatus.Visible = false;
                }
            }
            //
            afterloop:
            if (ds.Tables[0].Rows[0]["ActivityType"].ToString() == "Accepted")
            {
                if (getdata.checkUserAddedDocumentstatus(new Guid(Request.QueryString["DocID"].ToString()), new Guid(Session["UserUID"].ToString()), ds.Tables[0].Rows[0]["ActivityType"].ToString()) > 0)
                {
                    AddStatus.Visible = false;
                }
            }
            //
        }

        protected void GrdDocStatus_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (Session["IsContractor"].ToString() == "Y")
                {
                    
                    e.Row.Cells[3].Visible = false;
                    e.Row.Cells[4].Visible = false;
                    e.Row.Cells[12].Visible = false;
                }
            }
                if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton imgShowHide = (ImageButton)e.Row.FindControl("imgProductsShow");
                Show_Hide_ProductsGrid(imgShowHide, e);
                
                    
               
                //ImageButton imgShowHide = (sender as ImageButton);
                //GridViewRow row = (imgShowHide.NamingContainer as GridViewRow);
                //if (imgShowHide.CommandArgument == "Show")
                //{
                //    e.Row.FindControl("pnlDocuemnt").Visible = true;
                //    imgShowHide.CommandArgument = "Hide";
                //    imgShowHide.ImageUrl = "~/_assets/images/minus.png";
                //    string orderId = (e.Row.NamingContainer as GridView).DataKeys[e.Row.RowIndex].Value.ToString();
                //    GridView gvDocumentVersion = e.Row.FindControl("gvDocumentVersion") as GridView;
                //    BindDocumentVesrion(orderId, gvDocumentVersion);
                //}
                //else
                //{
                //    e.Row.FindControl("pnlDocuemnt").Visible = false;
                //    imgShowHide.CommandArgument = "Show";
                //    imgShowHide.ImageUrl = "~/_assets/images/plus.png";
                //}
                //if (e.Row.Cells[3].Text == "Submitted for Code B Approval")
                //{
                //    e.Row.Cells[3].Text = e.Row.Cells[5].Text;
                //}
                //else if (e.Row.Cells[3].Text == "Code B Approval")
                //{
                //    e.Row.Cells[3].Text = e.Row.Cells[6].Text;
                //}
                //else if (e.Row.Cells[3].Text == "Submitted for Code A Approval")
                //{
                //    e.Row.Cells[3].Text = e.Row.Cells[7].Text;
                //}
                //else if (e.Row.Cells[3].Text == "Code A Approval")
                //{
                //    e.Row.Cells[3].Text = e.Row.Cells[8].Text;
                //}
                //string sDate = getdata.GetDocumentPlannedDate(new Guid(Request.QueryString["DocID"]), new Guid(Session["UserUID"].ToString()), Session["TypeOfUser"].ToString(), e.Row.Cells[2].Text);
                //if (sDate != "")
                //{
                //    e.Row.Cells[2].Text = Convert.ToDateTime(sDate).ToShortDateString();
                //}
                //else
                //{
                //    e.Row.Cells[2].Text = "";
                //}
                //DataRowView row = e.Row.DataItem as DataRowView;
                if (e.Row.Cells[3].Text == "Submitted")
                {
                    //e.Row.Cells[10].Text = "";

                    LinkButton lnk = (LinkButton)e.Row.FindControl("lnkdelete");
                    lnk.Enabled = false;
                    lnk.Text = "";

                    e.Row.Cells[9].Enabled = false;
                    e.Row.Cells[9].Text = "N/A";
                    //HtmlAnchor lnkResubmit = e.Row.FindControl("UploadDoc") as HtmlAnchor;
                    //if (lnkResubmit != null)
                    //{
                    //    lnkResubmit.Visible = false;
                    //}
                    //HyperLink lnkResubmit = e.Row.FindControl("UploadDoc") as HyperLink;
                    //lnkResubmit.Style.Add("display", "none");
                    //lnkResubmit.InnerText = "N/A";
                    //lnkResubmit.Disabled = true;

                }
                if (e.Row.Cells[3].Text == "Closed")
                {
                    e.Row.Cells[9].Enabled = false;
                    e.Row.Cells[9].Text = "N/A";
                }
                if (e.Row.Cells[5].Text == "&nbsp;")
                {
                    LinkButton lnk = (LinkButton)e.Row.FindControl("lnkdown");
                    lnk.Enabled = false;
                    lnk.Text = "No link review file";
                    //e.Row.Cells[9].Enabled = false;
                }
                if (e.Row.Cells[7].Text == "&nbsp;")
                {
                    LinkButton lnk = (LinkButton)e.Row.FindControl("lnkcoverdownload");
                    lnk.Enabled = false;
                    lnk.Text = "No cover letter file";
                    //e.Row.Cells[9].Enabled = false;
                }
                string SubmittalUID = getdata.GetSubmittalUID_By_ActualDocumentUID(new Guid(Request.QueryString["DocID"].ToString()));
                string Flowtype = getdata.GetFlowTypeBySubmittalUID(new Guid(SubmittalUID));
                if(Flowtype == "STP")
                {
                    if (Session["IsContractor"].ToString() == "Y")
                    {

                        string phase = getdata.GetPhaseforStatus(new Guid(getdata.GetFlowUIDBySubmittalUID(new Guid(SubmittalUID))), e.Row.Cells[3].Text);
                        //string phase = getdata.GetPhaseforStatus(new Guid(Request.QueryString["FlowUID"]), e.Row.Cells[3].Text);
                        e.Row.Cells[1].Text = phase;
                        e.Row.Cells[3].Visible = false;
                        e.Row.Cells[4].Visible = false;
                        e.Row.Cells[12].Visible = false;
                        if (e.Row.Cells[3].Text == "Code A-CE Approval")
                        {
                            e.Row.Cells[1].Text = "Approved By BWSSB Under Code A";

                        }
                        else if (e.Row.Cells[3].Text == "Code B-CE Approval")
                        {
                            e.Row.Cells[1].Text = "Approved By BWSSB Under Code B";
                        }
                        else if (e.Row.Cells[3].Text == "Code C-CE Approval")
                        {
                            e.Row.Cells[1].Text = "Under Client Approval Process";

                        }
                        else if (e.Row.Cells[3].Text == "Client CE GFC Approval")
                        {
                            e.Row.Cells[1].Text = "Approved GFC by BWSSB";
                        }

                    }
                    else
                    {

                        //added on 10/06/2022 for saladins changes for comments section
                        string FlowName = getdata.GetFlowName_by_SubmittalID(new Guid(SubmittalUID));
                        if (FlowName == "Works A")
                        {
                            if (e.Row.Cells[3].Text == "Accepted" && prevstatus == "Reconciliation")
                            {
                                DataSet dsMUSers = getdata.GetNextUser_By_DocumentUID(new Guid(Request.QueryString["DocID"].ToString()), 3);
                                string FlowUID = getdata.GetFlowUIDBySubmittalUID(new Guid(getdata.GetSubmittalUID_By_ActualDocumentUID(new Guid(Request.QueryString["DocID"]))));
                                e.Row.Cells[4].Text += "<br/>" + "--------------------" + "<br/>";
                                foreach (DataRow druser in dsMUSers.Tables[0].Rows)
                                {
                                    //if (getdata.checkUserAddedDocumentstatus(new Guid(Request.QueryString["DocID"].ToString()), new Guid(druser["Approver"].ToString()), "Accepted") == 0)
                                    //{
                                    DataSet dsuserf = getdata.GetCategoryNameforUser(new Guid(Request.QueryString["ProjectUID"]), new Guid(druser["Approver"].ToString()), new Guid(FlowUID));
                                    foreach (DataRow drf in dsuserf.Tables[0].Rows)
                                    {
                                        if (!e.Row.Cells[4].Text.Contains(drf["WorkPackageCategory_Name"].ToString()))
                                        {
                                            e.Row.Cells[4].Text += drf["WorkPackageCategory_Name"].ToString() + " -- Pending" + "<br/>";
                                        }
                                    }
                                    //}
                                }
                            }
                        }
                        else if ((FlowName == "Works B" || FlowName=="Vendor Approval"))
                        {
                            if (e.Row.Cells[3].Text == "Accepted" && prevstatus == "Reconciliation")
                            {
                                DataSet dsMUSers = getdata.GetNextUser_By_DocumentUID(new Guid(Request.QueryString["DocID"].ToString()), 3);
                                string FlowUID = getdata.GetFlowUIDBySubmittalUID(new Guid(getdata.GetSubmittalUID_By_ActualDocumentUID(new Guid(Request.QueryString["DocID"]))));
                                e.Row.Cells[4].Text += "<br/>" + "--------------------" + "<br/>";
                                foreach (DataRow druser in dsMUSers.Tables[0].Rows)
                                {
                                    //if (getdata.checkUserAddedDocumentstatus(new Guid(Request.QueryString["DocID"].ToString()), new Guid(druser["Approver"].ToString()), "Accepted") == 0)
                                    //{
                                        e.Row.Cells[4].Text += getdata.getUserNameby_UID(new Guid(druser["Approver"].ToString())) + " -- Pending" + "<br/>";
                                    //}
                                }
                            }
                        }
                        //
                        string phase = getdata.GetPhaseforStatus_CE(new Guid(getdata.GetFlowUIDBySubmittalUID(new Guid(SubmittalUID))), e.Row.Cells[3].Text);
                        //string phase = getdata.GetPhaseforStatus(new Guid(Request.QueryString["FlowUID"]), e.Row.Cells[3].Text);
                        e.Row.Cells[1].Text = phase;

                        if (e.Row.Cells[3].Text == "Code A-CE Approval")
                        {
                            e.Row.Cells[1].Text = "Approved By BWSSB Under Code A";

                        }
                        else if (e.Row.Cells[3].Text == "Code B-CE Approval")
                        {
                            e.Row.Cells[1].Text = "Approved By BWSSB Under Code B";
                        }
                        else if (e.Row.Cells[3].Text == "Code C-CE Approval")
                        {
                            e.Row.Cells[1].Text = "Under Client Approval Process";

                        }
                        else if (e.Row.Cells[3].Text == "Client CE GFC Approval")
                        {
                            e.Row.Cells[1].Text = "Approved GFC by BWSSB";
                        }
                        else if (e.Row.Cells[3].Text == "Accepted-PMC Comments")
                        {
                            e.Row.Cells[3].Text = "Accepted";
                        }


                        if (e.Row.Cells[11].Text == "Y")
                        {
                            e.Row.BackColor = System.Drawing.Color.Red;
                        }
                        //total days
                        if (string.IsNullOrEmpty(next))
                        {
                            next = e.Row.Cells[2].Text;
                        }
                        else
                        {

                            e.Row.Cells[12].Text = (Convert.ToDateTime(e.Row.Cells[2].Text) - Convert.ToDateTime(next)).TotalDays.ToString() + " day(s)";
                            next = e.Row.Cells[2].Text;
                        }
                    }
                    //
                    if(e.Row.Cells[3].Text.Contains("-") && e.Row.Cells[3].Text.Contains("Approval"))
                    {
                        string dataText = e.Row.Cells[3].Text.Split('-')[0]  + "-"  + "<br/>"  + e.Row.Cells[3].Text.Split('-')[1];
                        e.Row.Cells[3].Text = dataText;
                    }
                }
                //
                prevstatus = e.Row.Cells[3].Text;
                //if (Session["TypeOfUser"].ToString() == "U" || Session["TypeOfUser"].ToString() == "PA")
                //{
                //    e.Row.Cells[10].Visible = false;

                //}
                //else
                //{
                //    e.Row.Cells[10].Visible = true;
                //}
            }
        }

        protected void Show_Hide_ProductsGrid(object sender, EventArgs e)
        {
            ImageButton imgShowHide = (sender as ImageButton);
            GridViewRow row = (imgShowHide.NamingContainer as GridViewRow);
            if (imgShowHide.CommandArgument == "Show")
            {
                row.FindControl("pnlDocuemnt").Visible = true;
                imgShowHide.CommandArgument = "Hide";
                imgShowHide.ImageUrl = "~/_assets/images/minus.png";
                string orderId = (row.NamingContainer as GridView).DataKeys[row.RowIndex].Value.ToString();
                GridView gvDocumentVersion = row.FindControl("gvDocumentVersion") as GridView;
                BindDocumentVesrion(orderId, gvDocumentVersion);
            }
            else
            {
                row.FindControl("pnlDocuemnt").Visible = false;
                imgShowHide.CommandArgument = "Show";
                imgShowHide.ImageUrl = "~/_assets/images/plus.png";
               
            }
        }
        private void BindDocumentVesrion(string DocStatus_UID, GridView GrdVersion)
        {
            GrdVersion.ToolTip = DocStatus_UID.ToString();
            GrdVersion.DataSource = getdata.getDocumentVersions_by_StatusUID(new Guid(DocStatus_UID));
            GrdVersion.DataBind();
        }

        public string GetDocumentType(string DocumentExtn)
        {
            string retval = getdata.GetDocumentMasterType_by_Extension(DocumentExtn);
            if (retval == null || retval == "")
            {
                return "N/A";
            }
            else
            {
                return retval;
            }
        }
        protected void gvDocumentVersion_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string UID = e.CommandArgument.ToString();
            if (e.CommandName == "download")
            {

                DataSet ds = getdata.getDocumentVersions_by_VersioUID(new Guid(UID));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string path = Server.MapPath(ds.Tables[0].Rows[0]["Doc_FileName"].ToString());
                    //   File.Decrypt(path);
                    string getExtension = System.IO.Path.GetExtension(path);
                    string outPath = path.Replace(getExtension, "") + "_download" + getExtension;
                    getdata.DecryptFile(path, outPath);
                    System.IO.FileInfo file = new System.IO.FileInfo(outPath);

                    if (file.Exists)
                    {
                        int Cnt = getdata.DocumentHistroy_InsertorUpdate(Guid.NewGuid(), new Guid(ds.Tables[0].Rows[0]["DocumentUID"].ToString()), new Guid(Session["UserUID"].ToString()), "Downloaded", "Documents");
                        if (Cnt <= 0)
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code: DDH-01. there is a problem with updating histroy. Please contact system admin.');</script>");
                        }

                        Response.Clear();

                        Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);

                        Response.AddHeader("Content-Length", file.Length.ToString());

                        Response.ContentType = "application/octet-stream";

                        Response.WriteFile(file.FullName);

                        Response.End();

                    }

                    else
                    {

                        //Response.Write("This file does not exist.");
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('File does not exist.');</script>");

                    }
                }
            }

            if (e.CommandName == "coverletter")
            {

                DataSet ds = getdata.getDocumentVersions_by_VersioUID(new Guid(UID));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string path = Server.MapPath(ds.Tables[0].Rows[0]["Doc_CoverLetter"].ToString());
                    //   File.Decrypt(path);
                    string getExtension = System.IO.Path.GetExtension(path);
                    string outPath = path.Replace(getExtension, "") + "_download" + getExtension;
                    getdata.DecryptFile(path, outPath);
                    System.IO.FileInfo file = new System.IO.FileInfo(outPath);

                    if (file.Exists)
                    {
                        int Cnt = getdata.DocumentHistroy_InsertorUpdate(Guid.NewGuid(), new Guid(ds.Tables[0].Rows[0]["DocumentUID"].ToString()), new Guid(Session["UserUID"].ToString()), "Downloaded", "Cover Letter");
                        if (Cnt <= 0)
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code: DDH-01. there is a problem with updating histroy. Please contact system admin.');</script>");
                        }

                        Response.Clear();

                        Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);

                        Response.AddHeader("Content-Length", file.Length.ToString());

                        Response.ContentType = "application/octet-stream";

                        Response.WriteFile(file.FullName);

                        Response.End();

                    }

                    else
                    {

                        //Response.Write("This file does not exist.");
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('File does not exist.');</script>");

                    }
                }
            }
        }

        protected void GrdDocStatus_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string StatusUID = e.CommandArgument.ToString();
            if (e.CommandName == "download")
            {
                DataSet ds = getdata.getDocumentStatusList_by_StatusUID(new Guid(StatusUID));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string path = Server.MapPath(ds.Tables[0].Rows[0]["LinkToReviewFile"].ToString());

                    string getExtension = System.IO.Path.GetExtension(path);
                    string outPath = path.Replace(getExtension, "") + "_download" + getExtension;
                    getdata.DecryptFile(path, outPath);
                    System.IO.FileInfo file = new System.IO.FileInfo(outPath);

                    if (file.Exists)
                    {

                        int Cnt = getdata.DocumentHistroy_InsertorUpdate(Guid.NewGuid(), new Guid(ds.Tables[0].Rows[0]["DocumentUID"].ToString()), new Guid(Session["UserUID"].ToString()), "Downloaded", "Documents");
                        if (Cnt <= 0)
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code: DDH-01. there is a problem with updating histroy. Please contact system admin.');</script>");
                        }

                        Response.Clear();

                        Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);

                        Response.AddHeader("Content-Length", file.Length.ToString());

                        Response.ContentType = "application/octet-stream";

                        Response.WriteFile(file.FullName);

                        Response.End();

                    }

                    else
                    {

                        //Response.Write("This file does not exist.");
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('File does not exist.');</script>");

                    }
                }
            }
            if (e.CommandName == "Cover Download")
            {
                DataSet ds = getdata.getDocumentStatusList_by_StatusUID(new Guid(StatusUID));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string path = Server.MapPath(ds.Tables[0].Rows[0]["CoverLetterFile"].ToString());

                    string getExtension = System.IO.Path.GetExtension(path);
                    string outPath = path.Replace(getExtension, "") + "_download" + getExtension;
                    getdata.DecryptFile(path, outPath);
                    System.IO.FileInfo file = new System.IO.FileInfo(outPath);

                    if (file.Exists)
                    {

                        Response.Clear();

                        Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);

                        Response.AddHeader("Content-Length", file.Length.ToString());

                        Response.ContentType = "application/octet-stream";

                        Response.WriteFile(file.FullName);

                        Response.End();

                    }

                    else
                    {

                        //Response.Write("This file does not exist.");
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('File does not exist.');</script>");

                    }
                }
            }
            if (e.CommandName == "delete")
            {
                int cnt = getdata.Document_Status_Delete(new Guid(StatusUID), new Guid(Session["UserUID"].ToString()));
                if (cnt > 0)
                {
                    BindDocStatus();
                }
            }
        }

        protected void gvDocumentVersion_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (ViewState["isHide"].ToString() == "true")
                {
                    e.Row.Cells[3].Text = "";
                }
                
                if (e.Row.Cells[6].Text == "&nbsp;")
                {
                    LinkButton lnk = (LinkButton)e.Row.FindControl("lnkCoverLetterDownload");
                    if (lnk != null)
                    {
                        lnk.Enabled = false;
                        lnk.Text = "No File";
                    }
                    //e.Row.Cells[9].Enabled = false;
                }
            }
        }

        protected void GrdDocStatus_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }
    }
}