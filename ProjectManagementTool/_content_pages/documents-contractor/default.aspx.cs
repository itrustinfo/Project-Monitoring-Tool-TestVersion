using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using ProjectManager.DAL;

namespace ProjectManagementTool._content_pages.documents_contractor
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
            else
            {
                if (!IsPostBack)
                {
                    if (Request.QueryString["UserUID"] != null)
                    {
                        DataSet ds = getdt.GetNextUserDocuments(new Guid(Request.QueryString["PrjUID"]), new Guid(Request.QueryString["WkpgUID"]));
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                           
                            GrdDocuments.DataSource = getdt.GetNextUserDocuments(new Guid(Request.QueryString["PrjUID"]), new Guid(Request.QueryString["WkpgUID"])).Tables[0].AsEnumerable()
                     .OrderByDescending(r => r.Field<DateTime>("IncomingRec_Date"))
                     .CopyToDataTable();
                            GrdDocuments.DataBind();
                        }
                        else
                        {
                            GrdDocuments.DataSource = getdt.GetNextUserDocuments(new Guid(Request.QueryString["PrjUID"]), new Guid(Request.QueryString["WkpgUID"])).Tables[0].AsEnumerable()
                     .OrderByDescending(r => r.Field<DateTime>("IncomingRec_Date"))
                     .CopyToDataTable();
                            GrdDocuments.DataBind();
                        }
                        LblDocumentHeading.Text = "Document List";
                        lblTotalcount.Visible = false;
                        GrdDocuments.Columns[13].Visible = false;
                        GrdDocuments.Columns[14].Visible = false;
                        GrdDocuments.Columns[15].Visible = false;
                        //  lblTotalcount.Text = "Total Count : " + GrdDocuments.Rows.Count.ToString();
                        //if (GrdDocuments.Rows.Count > 15)
                        //{
                        //    ScriptManager.RegisterStartupScript(Page, this.GetType(), "Key", "<script>MakeStaticHeader('" + GrdDocuments.ClientID + "', 700, 1300 , 45 ,true); </script>", false);
                        //}
                    }
                    else if (Request.QueryString["PrjUID"] != null)
                    {
                        if (Request.QueryString["type"].ToString() == "Contractor")
                        {
                            DataSet ds = getdt.GetDashboardContractotDocsSubmitted_Details(new Guid(Request.QueryString["PrjUID"]));
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                GrdDocuments.DataSource = getdt.GetDashboardContractotDocsSubmitted_Details(new Guid(Request.QueryString["PrjUID"])).Tables[0].AsEnumerable()
                 .OrderByDescending(r => r.Field<DateTime>("IncomingRec_Date"))
                 .CopyToDataTable();
                                GrdDocuments.DataBind();
                            }
                            else
                            {
                                GrdDocuments.DataSource = ds;
                                GrdDocuments.DataBind();
                            }
                            LblDocumentHeading.Text = "Document List for Contractor -> ONTB";
                            GrdDocuments.Columns[13].Visible = false;
                            GrdDocuments.Columns[14].Visible = false;
                            GrdDocuments.Columns[15].Visible = false;
                            lblTotalcount.Text = "Total Count : " + GrdDocuments.Rows.Count.ToString();
                            if (GrdDocuments.Rows.Count > 15)
                            {
                                ScriptManager.RegisterStartupScript(Page, this.GetType(), "Key", "<script>MakeStaticHeader('" + GrdDocuments.ClientID + "', 700, 1300 , 45 ,true); </script>", false);
                            }
                        }
                        else if (Request.QueryString["type"].ToString() == "Recon")
                        {
                            DataSet ds = getdt.GetDashboardReconciliationDocs_Details(new Guid(Request.QueryString["PrjUID"]));
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                GrdDocuments.DataSource = ds.Tables[0].AsEnumerable()
                     .OrderByDescending(r => r.Field<DateTime>("IncomingRec_Date"))
                     .CopyToDataTable();
                                GrdDocuments.DataBind();
                            }
                            else
                            {
                                GrdDocuments.DataSource = ds;
                                GrdDocuments.DataBind();
                            }
                            LblDocumentHeading.Text = "Document List for Reconciliation Docs";
                            lblTotalcount.Text = "Total Count : " + GrdDocuments.Rows.Count.ToString();
                            //added on 09/06/2022 after inputs from saladin
                            GrdDocuments.Columns[7].Visible = true;
                            GrdDocuments.Columns[8].Visible = false;
                            GrdDocuments.HeaderRow.Cells[7].Text = "Contractor Uploaded date";
                            //
                            if (Session["IsContractor"].ToString() == "Y")
                            {
                                GrdDocuments.Columns[13].Visible = false;
                                GrdDocuments.Columns[14].Visible = false;
                                GrdDocuments.Columns[15].Visible = false;

                            }
                            else
                            {
                                GrdDocuments.Columns[13].Visible = true;
                                GrdDocuments.Columns[14].Visible = true;
                                GrdDocuments.Columns[15].Visible = true;
                                btnSubmit.Visible = true;
                            }
                            if (GrdDocuments.Rows.Count > 15)
                            {
                                ScriptManager.RegisterStartupScript(Page, this.GetType(), "Key", "<script>MakeStaticHeader('" + GrdDocuments.ClientID + "', 700, 1300 , 45 ,true); </script>", false);
                            }
                        }
                        else
                        {
                            DataSet ds = getdt.GetDashboardONTBtoContractorDocs_Details(new Guid(Request.QueryString["PrjUID"]));
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                GrdDocuments.DataSource = getdt.GetDashboardONTBtoContractorDocs_Details(new Guid(Request.QueryString["PrjUID"])).Tables[0].AsEnumerable()
                 .OrderByDescending(r => r.Field<DateTime>("IncomingRec_Date"))
                 .CopyToDataTable();
                                GrdDocuments.DataBind();
                            }
                            else
                            {
                                GrdDocuments.DataSource = ds;
                                GrdDocuments.DataBind();
                            }
                            LblDocumentHeading.Text = "Document List for ONTB -> Contractor";
                            lblTotalcount.Text = "Total Count : " + GrdDocuments.Rows.Count.ToString();
                            GrdDocuments.Columns[13].Visible = false;
                            GrdDocuments.Columns[14].Visible = false;
                            GrdDocuments.Columns[15].Visible = false;
                            if (GrdDocuments.Rows.Count > 15)
                            {
                                ScriptManager.RegisterStartupScript(Page, this.GetType(), "Key", "<script>MakeStaticHeader('" + GrdDocuments.ClientID + "', 700, 1300 , 45 ,true); </script>", false);
                            }
                        }
                    }
                    
                        Session["isDownloadNJSE"] = false;
                    DataSet dscheck = new DataSet();
                    dscheck = getdt.GetUsertypeFunctionality_Mapping(Session["TypeOfUser"].ToString());
                    if (dscheck.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dscheck.Tables[0].Rows)
                        {

                            if (dr["Code"].ToString() == "FN") // DOWNLOAD APPROVED DOCUMENTS (FINAL APPROVED BY CLIENT)
                            {
                                ViewState["isDownloadClient"] = "true";
                                Session["isDownloadClient"] = "true";
                            }
                            if (dr["Code"].ToString() == "FM") // DOWNLOAD APPROVED DOCUMENTS (FINAL APPROVED BY NJSEI)
                            {
                                ViewState["isDownloadNJSE"] = "true";
                                Session["isDownloadNJSE"] = "true";
                            }
                        }
                    }
                }
            }
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

        //added on 09/06/2022
        public string GetFlowName(string SubmittalUID)
        {
            string retval = getdt.GetFlowName_by_SubmittalID(new Guid(SubmittalUID));
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

            //LinkButton lnkbtn;
            //if (e.Row.RowType == DataControlRowType.Header)
            //{
            //    foreach (TableCell cell in e.Row.Cells)
            //    {
            //        lnkbtn = (LinkButton)cell.Controls[0];
            //        if (!string.IsNullOrEmpty(GrdDocuments.SortExpression))
            //        {
            //            if (GrdDocuments.SortExpression.Equals(lnkbtn.Text))
            //            {
            //                cell.BackColor = System.Drawing.Color.Crimson;
            //            }
            //        }
            //    }
            //}
            //----------


            if (e.Row.RowType == DataControlRowType.DataRow)
            {
               
                DataSet ds = getdt.getTop1_DocumentStatusSelect(new Guid(e.Row.Cells[0].Text));
                Label lblDocumentName = (Label)e.Row.FindControl("lblDocumentName");
                //
                if (ds != null)
                {


                    if (ds.Tables[0].Rows[0]["DocumentType"].ToString() == "General Document")
                    {
                        e.Row.Cells[11].Text = "No History";
                    }
                }
                if (ds.Tables[0].Rows[0]["ActivityType"].ToString() != "" && ds.Tables[0].Rows[0]["TopVersion"].ToString() != "")
                {
                    //e.Row.Cells[1].Text = ds.Tables[0].Rows[0]["TopVersion"].ToString();


                    //string newVersionFileName = Path.GetFileNameWithoutExtension(Server.MapPath(ds.Tables[0].Rows[0]["Doc_Path"].ToString()));
                   // lblDocumentName.Text = newVersionFileName.Substring(0, (newVersionFileName.Length - 2)) + " [ Ver. " + ds.Tables[0].Rows[0]["TopVersion"].ToString() + " ]";
                    e.Row.Cells[4].Text = ds.Tables[0].Rows[0]["ActivityType"].ToString();
                }
                //
                if (Session["IsContractor"].ToString() == "Y")
                {
                    string SubmittalUID = getdt.GetSubmittalUID_By_ActualDocumentUID(new Guid(e.Row.Cells[0].Text));
                    string phase = getdt.GetPhaseforStatus(new Guid(getdt.GetFlowUIDBySubmittalUID(new Guid(SubmittalUID))), e.Row.Cells[4].Text);
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
                            if (e.Row.Cells[4].Text == "Code A-CE Approval")
                            {
                                e.Row.Cells[4].Text = "Approved By BWSSB Under Code A";

                            }
                            else if (e.Row.Cells[4].Text == "Code B-CE Approval")
                            {
                                e.Row.Cells[4].Text = "Approved By BWSSB Under Code B";
                            }
                            else if (e.Row.Cells[4].Text == "Code C-CE Approval")
                            {
                                e.Row.Cells[4].Text = "Under Client Approval Process";

                            }
                            else if (e.Row.Cells[4].Text == "Client CE GFC Approval")
                            {
                                e.Row.Cells[4].Text = "Approved GFC by BWSSB";
                            }
                        }
                        else
                        {
                            e.Row.Cells[4].Text = phase;
                        }
                    }

                }
                //
                if (e.Row.Cells[4].Text.Contains("Reconciliation"))
                {
                    e.Row.Cells[1].Text = GetSubmittalName(getdt.GetSubmittalUID_By_ActualDocumentUID(new Guid(e.Row.Cells[0].Text)));
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
                                    if (ds.Tables[0].Rows[0]["ActivityType"].ToString() == "Accepted")
                                    {
                                        if (getdt.checkUserAddedDocumentstatus(new Guid(e.Row.Cells[0].Text), new Guid(HttpContext.Current.Session["UserUID"].ToString()), ds.Tables[0].Rows[0]["ActivityType"].ToString()) == 0)
                                        {
                                            e.Row.Visible = true;
                                            return;
                                        }
                                        else
                                        {
                                            e.Row.Visible = false;
                                            return;
                                        }
                                    }
                                    
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
               
            }
        }

        protected void GrdDocuments_DataBound(object sender, EventArgs e)
        {
            //int sortedColumnPosition = 0;
            //LinkButton lnkbtn;

            //// Gets position of column whose header text matches SortExpression
            //// of the GridView when column is sorted
            //foreach (TableCell cell in GrdDocuments.HeaderRow.Cells)
            //{
            //    lnkbtn = (LinkButton)cell.Controls[0];
            //    if (lnkbtn.Text == GrdDocuments.SortExpression)
            //    {
            //        break;
            //    }
            //    sortedColumnPosition++;
            //}
            //if (!string.IsNullOrEmpty(GrdDocuments.SortExpression))
            //{
            //    foreach (GridViewRow row in GrdDocuments.Rows)
            //    {
            //        row.Cells[sortedColumnPosition].BackColor = System.Drawing.Color.LavenderBlush;
            //    }
            //}
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

        protected void btnSubmit_Click(object sender, EventArgs e) // reconciliation submit
        {
            try
            {
                foreach (GridViewRow row in GrdDocuments.Rows)
                {
                    
                    RadioButtonList rbSelect = (RadioButtonList)row.FindControl("rdActionsList");
                    if(rbSelect.SelectedValue != "")
                    { 
                    TextBox txtremarks = (TextBox)row.FindControl("txtremarks");
                    TextBox txtrefNo = (TextBox)row.FindControl("txtONTBRefNo");
                        string status = rbSelect.SelectedValue;
                        string remarks = txtremarks.Text;
                        string EmailHeading = string.Empty;
                        string DocumentUID = row.Cells[0].Text;
                        string OriginatorRefNo = string.Empty;
                        string ProjectRefNo = string.Empty;
                        if (status == "Accept")
                        {
                            if (String.IsNullOrEmpty(txtrefNo.Text.Trim()))
                            {
                                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Please Enter Reference No !');</script>");
                                return;
                            }
                        }

                        if (!String.IsNullOrEmpty(txtrefNo.Text.Trim()))
                        {
                            if (getdt.checkPrjRefNoExists(new Guid(Request.QueryString["PrjUID"]), txtrefNo.Text.Trim()))
                            {
                                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('ONTB Reference No already Exists.Please choose another Ref No. !');</script>");
                                return;
                            }
                        }
                       
                   string documentname = getdt.GetActualDocumentName_by_ActualDocumentUID(new Guid(DocumentUID));
                        //
                        if (status =="Accept")
                    {
                            if (row.Cells[4].Text == "Contractor Submitted 9 Copies")
                            {
                                status = "Accepted 9 Copies";
                            }
                            else
                            {
                                status = "Accepted";
                            }
                            EmailHeading = "Under Review by ONTB";
                    }
                    else
                    {
                            if (row.Cells[4].Text == "Contractor Submitted 9 Copies")
                            {
                                status = "Rejected 9 Copies";
                            }
                            else
                            {
                                status = "Rejected";
                            }
                            EmailHeading = "Rejected by ONTB";
                        }
                        //
                       

                        string sDate1 = "";
                        DateTime CDate1 = DateTime.Now;
                        //
                        sDate1 = CDate1.ToString("dd/MM/yyyy");
                        //sDate1 = sDate1.Split('/')[1] + "/" + sDate1.Split('/')[0] + "/" + sDate1.Split('/')[2];
                        sDate1 = getdt.ConvertDateFormat(sDate1);
                        CDate1 = Convert.ToDateTime(sDate1);
                        string DocPath = "";
                    string Subject = Session["Username"].ToString() + " added a new Status.";
                    string sHtmlString = string.Empty;
                    Guid StatusUID = Guid.NewGuid();
                    string CoverPagePath = string.Empty;

                    int Cnt = getdt.InsertorUpdateDocumentStatus(StatusUID, new Guid(DocumentUID), 1, status, 0, CDate1, DocPath,
                        new Guid(Session["UserUID"].ToString()), remarks, status, "", CDate1, CoverPagePath, "Current", "ONTB");
                        if (Cnt > 0)
                        {
                           
                            //DataSet ds = getdata.getAllUsers();
                            DataSet ds = new DataSet();
                            //Update the targte dates 
                            getdt.StoreFreshTargetDatesforStatusChange(new Guid(DocumentUID), status);

                            //Update refno for ONTB ......
                            getdt.UpdateActualDocsRefNo(new Guid(DocumentUID), txtrefNo.Text, "", 1);
                            string RefUID = getdt.GetTopRefNoHistory(new Guid(DocumentUID));
                            if (!string.IsNullOrEmpty(RefUID))
                            {
                                getdt.UpdateRefNoHistory(new Guid(RefUID), txtrefNo.Text, "", 1);
                            }

                            ds = getdt.GetUsers_under_ProjectUID(new Guid(Request.QueryString["PrjUID"]));
                            //
                            DataSet dsTasks = getdt.ActualDocuments_SelectBy_ActualDocumentUID(new Guid(DocumentUID));
                            if (dsTasks.Tables[0].Rows.Count > 0)
                            {
                                OriginatorRefNo = dsTasks.Tables[0].Rows[0]["Ref_Number"].ToString();
                                ProjectRefNo = dsTasks.Tables[0].Rows[0]["ProjectRef_Number"].ToString();
                            }
                            //

                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                string CC = string.Empty;
                                string ToEmailID = "";
                                string sUserName = "";
                                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                {
                                    if (ds.Tables[0].Rows[i]["UserUID"].ToString() == Session["UserUID"].ToString())
                                    {
                                        ToEmailID = ds.Tables[0].Rows[i]["EmailID"].ToString();
                                        sUserName = ds.Tables[0].Rows[i]["UserName"].ToString();
                                    }
                                    else
                                    {
                                        if (getdt.GetUserMailAccess(new Guid(ds.Tables[0].Rows[i]["UserUID"].ToString()), "documentmail") != 0)
                                        {
                                            CC += ds.Tables[0].Rows[i]["EmailID"].ToString() + ",";
                                        }

                                    }
                                }

                                //
                                CC = CC.TrimEnd(',');
                                sHtmlString = "<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>" + "<html xmlns='http://www.w3.org/1999/xhtml'>" +
                                  "<head>" + "<meta http-equiv='Content-Type' content='text/html; charset=utf-8' />" + "<style>table, th, td {border: 1px solid black; padding:6px;}</style></head>" +
                                     "<body style='font-family:Verdana, Arial, sans-serif; font-size:12px; font-style:normal;'>";
                                sHtmlString += "<div style='width:80%; float:left; padding:1%; border:2px solid #011496; border-radius:5px;'>" +
                                                   "<div style='float:left; width:100%; border-bottom:2px solid #011496;'>";
                                if (WebConfigurationManager.AppSettings["Domain"] == "NJSEI")
                                {
                                    sHtmlString += "<div style='float:left; width:7%;'><img src='https://dm.njsei.com/_assets/images/NJSEI%20Logo.jpg' width='50' /></div>";
                                }
                                else
                                {
                                    sHtmlString += "<div style='float:left; width:7%;'><h2>" + WebConfigurationManager.AppSettings["Domain"] + "</h2></div>";
                                }
                                sHtmlString += "<div style='float:left; width:70%;'><h2 style='margin-top:10px;'>Project Monitoring Tool</h2></div>" +
                                           "</div>";
                                sHtmlString += "<div style='width:100%; float:left;'><br/>Dear Users,<br/><br/><span style='font-weight:bold;'>" + sUserName + " has changed " + documentname + " status.</span> <br/><br/></div>";
                                sHtmlString += "<div style='width:100%; float:left;'><table style='width:100%;'>" +
                                                "<tr><td><b>Document Name </b></td><td style='text-align:center;'><b>:</b></td><td>" + documentname + "</td></tr>" +
                                                "<tr><td><b>Status </b></td><td style='text-align:center;'><b>:</b></td><td>" + status + "</td></tr>" +
                                               "<tr><td><b>Originator Ref. Number </b></td><td style='text-align:center;'><b>:</b></td><td>" + OriginatorRefNo + "</td></tr>" +
                                                    "<tr><td><b>ONTB Ref. Number</b></td><td style='text-align:center;'><b>:</b></td><td>" + ProjectRefNo + "</td></tr>" +
                                                      "<tr><td><b>Date </b></td><td style='text-align:center;'><b>:</b></td><td>" + DateTime.Now.ToString("dd MMM yyyy") + "</td></tr>" +
                                                "<tr><td><b>Comments </b></td><td style='text-align:center;'><b>:</b></td><td>" + remarks + "</td></tr>";
                                sHtmlString += "</table></div>";
                                sHtmlString += "<div style='width:100%; float:left;'><br/><br/>Sincerely, <br/> Project Monitoring Tool.</div></div></body></html>";

                                //sHtmlString = "<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>" + "<html xmlns='http://www.w3.org/1999/xhtml'>" +
                                //       "<head>" + "<meta http-equiv='Content-Type' content='text/html; charset=utf-8' />" + "</head>" +
                                //          "<body style='font-family:Verdana, Arial, sans-serif; font-size:12px; font-style:normal;'>";
                                //sHtmlString += "<div style='float:left; width:100%; height:30px;'>" +
                                //                   "Dear, " + "Users" +
                                //                   "<br/><br/></div>";
                                //sHtmlString += "<div style='width:100%; float:left;'><div style='width:100%; float:left;'>Below are the Status details. <br/><br/></div>";
                                //sHtmlString += "<div style='width:100%; float:left;'><div style='width:100%; float:left;'>Document Name : " + DDLDocument.SelectedItem.Text + "<br/></div>";
                                //sHtmlString += "<div style='width:100%; float:left;'><div style='width:100%; float:left;'>Status : " + DDlStatus.SelectedItem.Text + "<br/></div>";
                                //sHtmlString += "<div style='width:100%; float:left;'><div style='width:100%; float:left;'>Date : " + CDate1.ToShortDateString() + "<br/></div>";
                                //sHtmlString += "<div style='width:100%; float:left;'><br/><br/>Sincerely, <br/> Project Manager.</div></div></body></html>";
                                //string ret = getdata.SendMail(ds.Tables[0].Rows[0]["EmailID"].ToString(), Subject, sHtmlString, CC, Server.MapPath(DocPath));
                                // added on 02/11/2020
                                DataTable dtemailCred = getdt.GetEmailCredentials();
                                Guid MailUID = Guid.NewGuid();
                                getdt.StoreEmaildataToMailQueue(MailUID, new Guid(Session["UserUID"].ToString()), dtemailCred.Rows[0][0].ToString(), ToEmailID, Subject, sHtmlString, CC, "");
                                //
                                // added on 07/01/2022 for sending mail to next user in line to change status...
                                DataSet dsnew = getdt.getTop1_DocumentStatusSelect(new Guid(DocumentUID));
                                DataSet dsNext = getdt.GetNextStep_By_DocumentUID(new Guid(DocumentUID), dsnew.Tables[0].Rows[0]["ActivityType"].ToString());

                                sHtmlString = "<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>" + "<html xmlns='http://www.w3.org/1999/xhtml'>" +
                                "<head>" + "<meta http-equiv='Content-Type' content='text/html; charset=utf-8' />" + "<style>table, th, td {border: 1px solid black; padding:6px;}</style></head>" +
                                   "<body style='font-family:Verdana, Arial, sans-serif; font-size:12px; font-style:normal;'>";
                                sHtmlString += "<div style='width:80%; float:left; padding:1%; border:2px solid #011496; border-radius:5px;'>" +
                                                   "<div style='float:left; width:100%; border-bottom:2px solid #011496;'>";
                                if (WebConfigurationManager.AppSettings["Domain"] == "NJSEI")
                                {
                                    sHtmlString += "<div style='float:left; width:7%;'><img src='https://dm.njsei.com/_assets/images/NJSEI%20Logo.jpg' width='50' /></div>";
                                }
                                else
                                {
                                    sHtmlString += "<div style='float:left; width:7%;'><h2>" + WebConfigurationManager.AppSettings["Domain"] + "</h2></div>";
                                }
                                sHtmlString += "<div style='float:left; width:70%;'><h2 style='margin-top:10px;'>Project Monitoring Tool</h2></div>" +
                                           "</div>";
                                sHtmlString += "<div style='width:100%; float:left;'><br/>Dear Users,<br/><br/><span style='font-weight:bold;'>" + sUserName + " has changed " + documentname + " status.</span> <br/><br/></div>";
                                sHtmlString += "<div style='width:100%; float:left;'><table style='width:100%;'>" +
                                                "<tr><td><b>Document Name </b></td><td style='text-align:center;'><b>:</b></td><td>" + documentname + "</td></tr>" +
                                                "<tr><td><b>Status </b></td><td style='text-align:center;'><b>:</b></td><td>" + status + "</td></tr>" +
                                                 "<tr><td><b>Originator Ref. Number </b></td><td style='text-align:center;'><b>:</b></td><td>" + OriginatorRefNo + "</td></tr>" +
                                                    "<tr><td><b>ONTB Ref. Number</b></td><td style='text-align:center;'><b>:</b></td><td>" + ProjectRefNo + "</td></tr>" +

                                                "<tr><td><b>Date </b></td><td style='text-align:center;'><b>:</b></td><td>" + DateTime.Now.ToString("dd MMM yyyy") + "</td></tr>" +
                                                "<tr><td><b>Comments </b></td><td style='text-align:center;'><b>:</b></td><td>" + remarks + "</td></tr>";
                                sHtmlString += "</table><br /><br /><div style='color: red'>Kindly note that you are to act on this to complete the next step in document flow.</div></div>";
                                sHtmlString += "<div style='width:100%; float:left;'><br/><br/>Sincerely, <br/> Project Monitoring Tool.</div></div></body></html>";
                                Subject = Subject + ".Kindly complete the next step !";
                                string next = string.Empty;
                                DataSet dsNxtUser = new DataSet();
                                foreach (DataRow dr in dsNext.Tables[0].Rows)
                                {
                                    dsNxtUser = getdt.GetNextUser_By_DocumentUID(new Guid(DocumentUID), int.Parse(dr["ForFlow_Step"].ToString()));
                                    foreach (DataRow druser in dsNxtUser.Tables[0].Rows)
                                    {
                                        ToEmailID = getdt.GetUserEmail_By_UserUID_New(new Guid(druser["Approver"].ToString()));
                                        if (!next.Contains(ToEmailID))
                                        {
                                            getdt.StoreEmaildataToMailQueue(Guid.NewGuid(), new Guid(Session["UserUID"].ToString()), dtemailCred.Rows[0][0].ToString(), ToEmailID, Subject, sHtmlString, "", "");
                                            next += ToEmailID;
                                        }

                                    }
                                }
                                // mail to contractor
                                Subject = "Document Status Changed !" ;
                                DataSet dsMUSers = getdt.GetNextUser_By_DocumentUID(new Guid(DocumentUID), 1);
                                if (dsMUSers.Tables[0].Rows.Count > 0)
                                {
                                    foreach (DataRow druser in dsMUSers.Tables[0].Rows)
                                    {
                                        ToEmailID= getdt.GetUserEmail_By_UserUID_New(new Guid(druser["Approver"].ToString())) ;
                                    }
                                }
                                sHtmlString = "<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>" + "<html xmlns='http://www.w3.org/1999/xhtml'>" +
                                "<head>" + "<meta http-equiv='Content-Type' content='text/html; charset=utf-8' />" + "<style>table, th, td {border: 1px solid black; padding:6px;}</style></head>" +
                                   "<body style='font-family:Verdana, Arial, sans-serif; font-size:12px; font-style:normal;'>";
                                sHtmlString += "<div style='width:80%; float:left; padding:1%; border:2px solid #011496; border-radius:5px;'>" +
                                                   "<div style='float:left; width:100%; border-bottom:2px solid #011496;'>";
                                if (WebConfigurationManager.AppSettings["Domain"] == "NJSEI")
                                {
                                    sHtmlString += "<div style='float:left; width:7%;'><img src='https://dm.njsei.com/_assets/images/NJSEI%20Logo.jpg' width='50' /></div>";
                                }
                                else
                                {
                                    sHtmlString += "<div style='float:left; width:7%;'><h2>" + WebConfigurationManager.AppSettings["Domain"] + "</h2></div>";
                                }
                                sHtmlString += "<div style='float:left; width:70%;'><h2 style='margin-top:10px;'>Project Monitoring Tool</h2></div>" +
                                           "</div>";
                                sHtmlString += "<div style='width:100%; float:left;'><br/>Dear Contractor,<br/><br/><span style='font-weight:bold;'>Document " + documentname + " "  + EmailHeading +  ".</span> <br/><br/></div>";
                                sHtmlString += "<div style='width:100%; float:left;'><table style='width:100%;'>" +
                                                "<tr><td><b>Document Name </b></td><td style='text-align:center;'><b>:</b></td><td>" + documentname + "</td></tr>" +
                                                "<tr><td><b>Status </b></td><td style='text-align:center;'><b>:</b></td><td>" + status + "</td></tr>" +
                                               "<tr><td><b>Originator Ref. Number </b></td><td style='text-align:center;'><b>:</b></td><td>" + OriginatorRefNo + "</td></tr>" +
                                                    "<tr><td><b>ONTB Ref. Number</b></td><td style='text-align:center;'><b>:</b></td><td>" + ProjectRefNo + "</td></tr>" +

                                                "<tr><td><b>Date </b></td><td style='text-align:center;'><b>:</b></td><td>" + DateTime.Now.ToString("dd MMM yyyy") + "</td></tr>" +
                                                "<tr><td><b>Comments </b></td><td style='text-align:center;'><b>:</b></td><td>" + remarks + "</td></tr>";
                                sHtmlString += "</table></div>";
                                sHtmlString += "<div style='width:100%; float:left;'><br/><br/>Sincerely, <br/> Project Monitoring Tool.</div></div></body></html>";
                                getdt.StoreEmaildataToMailQueue(Guid.NewGuid(), new Guid(Session["UserUID"].ToString()), dtemailCred.Rows[0][0].ToString(), ToEmailID, Subject, sHtmlString, CC, "");

                                //

                                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
                            }
                            else
                            {

                                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error: There is a problem with this feature. Please contact system admin');</script>");

                            }
                        }
                    }
                }

            }
            catch(Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error:Please Contact System Admin !');</script>");

            }
        }
    }
}