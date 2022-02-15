using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using iTextSharp.text;
using iTextSharp.text.pdf;
using ProjectManager.DAL;

namespace ProjectManagementTool._content_pages.document_user_count
{
    public partial class _default : System.Web.UI.Page
    {
        DBGetData getdt = new DBGetData();
        TaskUpdate gettk = new TaskUpdate();
        int totalcount = 0;
        int totalactcount = 0;
        int totalgdcount = 0;
        int totaldwnldcount = 0;
        int totalviewcount = 0;
        int grandtotalcount = 0;
        int grandtotaldwnldcount = 0;
        int grandtotalviewcount = 0;
        int GrandTotalDocumentLinkSent = 0;
        int TotalDocumentLinkSentCount = 0;
        string name = string.Empty;
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
                    BindProject();
                    LoadUsers();
                }
            }
        }

        private void BindProject()
        {
            DataTable ds = new DataTable();
            if (Session["TypeOfUser"].ToString() == "U" || Session["TypeOfUser"].ToString() == "MD" || Session["TypeOfUser"].ToString() == "VP")
            {
                ds = gettk.GetProjects();
            }
            else if (Session["TypeOfUser"].ToString() == "PA")
            {
                ds = gettk.GetAssignedProjects_by_UserUID(new Guid(Session["UserUID"].ToString()));
            }
            else
            {
                ds = gettk.GetAssignedProjects_by_UserUID(new Guid(Session["UserUID"].ToString()));
            }

            ddlProject.DataTextField = "ProjectName";
            ddlProject.DataValueField = "ProjectUID";
            ddlProject.DataSource = ds;
            ddlProject.DataBind();
            ddlProject.Items.Add("General Documents");
           
            ddlProject.Items.Insert(0, "All Projects");
            ddlProject.Items.Insert(0, "Select Project");


        }

        private void LoadUsers()
        {
            try
            {
                ddlusers.Items.Clear();
                if (ddlProject.SelectedValue == "All Projects" || ddlProject.SelectedValue == "General Documents")
                {
                    ddlusers.DataSource = getdt.GetUserList();
                    ddlusers.DataTextField = "Name";
                    ddlusers.DataValueField = "UserUID";
                    ddlusers.DataBind();
                }
                else if (ddlProject.SelectedValue != "All Projects" && ddlProject.SelectedIndex != 0)
                {
                    ddlusers.DataSource = getdt.GetUserListByPrj(new Guid(ddlProject.SelectedValue));
                    ddlusers.DataTextField = "Name";
                    ddlusers.DataValueField = "UserUID";
                    ddlusers.DataBind();
                }
                ddlusers.Items.Insert(0, "All Users");
                ddlusers.Items.Insert(0, "Select User");

                if (ddlProject.SelectedValue == "General Documents")
                {
                    DDLStatus.SelectedValue = "Submitted";
                    DDLStatus.Enabled = false;
                }
                else
                {
                    DDLStatus.SelectedIndex = 0;
                    DDLStatus.Enabled = true;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    
        private void LoadProjects()
        {
            try
            {
                if (RDBReportView.SelectedValue == "User" && ddlusers.SelectedValue != "All Users")
                {
                    grdDocumenthistoryAll.Visible = false;
                    GrdDcumentHistory.Visible = true;
                    if (ddlProject.SelectedIndex != 0 && ddlProject.SelectedValue == "All Projects")
                    {
                        GrdDcumentHistory.DataSource = getdt.GetUserProjects(new Guid(ddlusers.SelectedValue), 1, Guid.NewGuid());
                        GrdDcumentHistory.DataBind();
                    }
                    else if (ddlProject.SelectedIndex != 0 && ddlProject.SelectedValue != "All Projects" && ddlProject.SelectedValue != "General Documents")
                    {
                        GrdDcumentHistory.DataSource = getdt.GetUserProjects(new Guid(ddlusers.SelectedValue), 2, new Guid(ddlProject.SelectedValue));
                        GrdDcumentHistory.DataBind();
                    }
                    else if (ddlProject.SelectedValue == "General Documents")
                    {
                        GrdDcumentHistory.DataSource = getdt.GetUserProjects(new Guid(ddlusers.SelectedValue), 3, Guid.NewGuid());
                        GrdDcumentHistory.DataBind();
                    }
                    if (GrdDcumentHistory.Rows.Count > 10)
                    {
                        ScriptManager.RegisterStartupScript(Page, this.GetType(), "Key", "<script>MakeStaticHeaderNew('" + GrdDcumentHistory.ClientID + "', 600, 1300 , 55 ,false); </script>", false);
                    }
                }
                else if (ddlusers.SelectedValue == "All Users")
                {
                    lblDocumentsNo.Text = "";
                    grdDocumenthistoryAll.Visible = true;
                    GrdDcumentHistory.Visible = false;

                    DataTable dt = new DataTable();
                    DataRow dr;


                    // Add Columns to datatablse
                    dt.Columns.Add(new DataColumn("ProjectName")); //'ColumnName1' represents name of datafield in grid
                    dt.Columns.Add(new DataColumn("ProjectName1"));
                    dt.Columns.Add(new DataColumn("ProjectUID1"));
                    dt.Columns.Add(new DataColumn("ProjectUID2"));
                    dt.Columns.Add(new DataColumn("ProjectUID3"));
                    dt.Columns.Add(new DataColumn("ProjectUID4"));
                    bool IsExits = false;
                    foreach (DataRow gvr in getdt.GetUserList().Tables[0].Rows)
                    {
                        IsExits = false;
                        if (ddlProject.SelectedValue != "All Projects" && ddlProject.SelectedValue != "General Documents")
                        {
                            foreach (DataRow gr in getdt.GetUserProjects(new Guid(gvr["UserUID"].ToString()), 5, new Guid(ddlProject.SelectedValue)).Tables[0].Rows)
                            {
                                IsExits = true;
                                dr = dt.NewRow();
                                dr["ProjectName"] = gvr["UserUID"].ToString();
                                dr["ProjectName1"] = gr["ProjectName"].ToString();
                                dr["ProjectUID1"] = gr["ProjectUID"].ToString();
                                dr["ProjectUID2"] = gr["ProjectUID"].ToString();
                                dr["ProjectUID3"] = gr["ProjectUID"].ToString();
                                dr["ProjectUID4"] = gr["ProjectUID"].ToString();
                                dt.Rows.Add(dr);
                            }
                        }
                        else if (ddlProject.SelectedValue == "General Documents")
                        {
                            foreach (DataRow gr in getdt.GetUserProjects(new Guid(gvr["UserUID"].ToString()), 3, Guid.NewGuid()).Tables[0].Rows)
                            {
                                IsExits = true;
                                dr = dt.NewRow();
                                dr["ProjectName"] = gvr["UserUID"].ToString();
                                dr["ProjectName1"] = gr["ProjectName"].ToString();
                                dr["ProjectUID1"] = gr["ProjectUID"].ToString();
                                dr["ProjectUID2"] = gr["ProjectUID"].ToString();
                                dr["ProjectUID3"] = gr["ProjectUID"].ToString();
                                dr["ProjectUID4"] = gr["ProjectUID"].ToString();
                                dt.Rows.Add(dr);
                            }
                        }
                        else
                        {
                            foreach (DataRow gr in getdt.GetUserProjects(new Guid(gvr["UserUID"].ToString()), 4, Guid.NewGuid()).Tables[0].Rows)
                            {
                                IsExits = true;
                                dr = dt.NewRow();
                                dr["ProjectName"] = gvr["UserUID"].ToString();
                                dr["ProjectName1"] = gr["ProjectName"].ToString();
                                dr["ProjectUID1"] = gr["ProjectUID"].ToString();
                                dr["ProjectUID2"] = gr["ProjectUID"].ToString();
                                dr["ProjectUID3"] = gr["ProjectUID"].ToString();
                                dr["ProjectUID4"] = gr["ProjectUID"].ToString();
                                dt.Rows.Add(dr);
                            }
                        }

                        if (IsExits)
                        {
                            dr = dt.NewRow();
                            dr["ProjectName"] = "";
                            dr["ProjectName1"] = "Total";
                            dr["ProjectUID1"] = "";
                            dr["ProjectUID2"] = "";
                            dr["ProjectUID3"] = "";
                            dr["ProjectUID4"] = "";
                            dt.Rows.Add(dr);
                        }
                    }
                    grdDocumenthistoryAll.DataSource = dt;
                    grdDocumenthistoryAll.DataBind();
                    if (dt.Rows.Count > 10)
                    {
                        ScriptManager.RegisterStartupScript(Page, this.GetType(), "Key", "<script>MakeStaticHeaderNew('" + grdDocumenthistoryAll.ClientID + "', 600, 1300 , 55 ,false); </script>", false);
                    }
                }
                else
                {
                    lblDocumentsNo.Text = "";
                    grdDocumenthistoryAll.Visible = false;
                    GrdDcumentHistory.Visible = false;
                    GrdProjectwiseSummary.Visible = true;
                    DataTable dt = new DataTable();
                    DataRow dr;


                    // Add Columns to datatablse
                    //dt.Columns.Add(new DataColumn("ProjectName")); //'ColumnName1' represents name of datafield in grid
                    dt.Columns.Add(new DataColumn("ProjectName1"));
                    dt.Columns.Add(new DataColumn("ProjectUID1"));
                    dt.Columns.Add(new DataColumn("ProjectUID2"));
                    dt.Columns.Add(new DataColumn("ProjectUID3"));
                    dt.Columns.Add(new DataColumn("ProjectUID4"));
                    DataSet ds;
                    if (ddlProject.SelectedValue == "All Projects")
                    {
                        ds = getdt.GetProjects_by_Type(Guid.NewGuid(), "All");
                    }
                    else if (ddlProject.SelectedValue == "General Documents")
                    {
                        ds = getdt.GetProjects_by_Type(Guid.NewGuid(), "General Documents");
                    }
                    else
                    {
                        ds = getdt.GetProjects_by_Type(new Guid(ddlProject.SelectedValue), "Project");
                    }
                    foreach (DataRow gvr in ds.Tables[0].Rows)
                    {
                        dr = dt.NewRow();
                        dr["ProjectName1"] = gvr["ProjectName"].ToString();
                        dr["ProjectUID1"] = gvr["ProjectUID"].ToString();
                        dr["ProjectUID2"] = gvr["ProjectUID"].ToString();
                        dr["ProjectUID3"] = gvr["ProjectUID"].ToString();
                        dr["ProjectUID4"] = gvr["ProjectUID"].ToString();
                        dt.Rows.Add(dr);
                    }
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        dr = dt.NewRow();
                        dr["ProjectName1"] = "Total";
                        dr["ProjectUID1"] = "";
                        dr["ProjectUID2"] = "";
                        dr["ProjectUID3"] = "";
                        dr["ProjectUID4"] = "";
                        dt.Rows.Add(dr);
                    }
                    GrdProjectwiseSummary.DataSource = dt;
                    GrdProjectwiseSummary.DataBind();
                    if (dt.Rows.Count > 10)
                    {
                        ScriptManager.RegisterStartupScript(Page, this.GetType(), "Key", "<script>MakeStaticHeaderNew('" + GrdProjectwiseSummary.ClientID + "', 600, 1300 , 55 ,false); </script>", false);
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void LoadDocuments()
        {
            try
            {
                DateTime FromDate = DateTime.Now;
                DateTime ToDate = DateTime.Now;
                DataSet ds = new DataSet();
                if (!string.IsNullOrEmpty(dtFromDate.Text))
                {
                    FromDate = Convert.ToDateTime(getdt.ConvertDateFormat(dtFromDate.Text));
                }
                if (!string.IsNullOrEmpty(dtToDate.Text))
                {
                    ToDate = Convert.ToDateTime(getdt.ConvertDateFormat(dtToDate.Text));
                }
                if (RDBReportView.SelectedValue == "User" && ddlProject.SelectedValue == "All Projects" && ddlusers.SelectedValue != "All Users")
                {
                    if (dtFromDate.Text != "" && dtToDate.Text != "")
                    {
                        GrdDocuments.DataSource = getdt.GetAllUserDocuments(new Guid(ddlusers.SelectedValue), FromDate, ToDate, 1, DDLStatus.SelectedValue, DDLFilter.SelectedValue);
                        ds = getdt.GetAllUserDocuments(new Guid(ddlusers.SelectedValue), FromDate, ToDate, 1, DDLStatus.SelectedValue, DDLFilter.SelectedValue);
                    }
                    else
                    {
                        GrdDocuments.DataSource = getdt.GetAllUserDocuments(new Guid(ddlusers.SelectedValue), FromDate, ToDate, 0, DDLStatus.SelectedValue, DDLFilter.SelectedValue);
                        ds = getdt.GetAllUserDocuments(new Guid(ddlusers.SelectedValue), FromDate, ToDate, 0, DDLStatus.SelectedValue, DDLFilter.SelectedValue);
                    }
                }
                else if (RDBReportView.SelectedValue == "User" && ddlProject.SelectedValue != "All Projects" && ddlusers.SelectedValue == "All Users" && ddlProject.SelectedValue != "General Documents")
                {
                    if (dtFromDate.Text != "" && dtToDate.Text != "")
                    {
                        GrdDocuments.DataSource = getdt.GetProjectwiseDocuments_For_AllUsers(FromDate, ToDate, 1, new Guid(ddlProject.SelectedValue), DDLStatus.SelectedValue, DDLFilter.SelectedValue);
                        ds = getdt.GetProjectwiseDocuments_For_AllUsers(FromDate, ToDate, 1, new Guid(ddlProject.SelectedValue), DDLStatus.SelectedValue, DDLFilter.SelectedValue);
                    }
                    else
                    {
                        GrdDocuments.DataSource = getdt.GetProjectwiseDocuments_For_AllUsers(FromDate, ToDate, 0, new Guid(ddlProject.SelectedValue), DDLStatus.SelectedValue, DDLFilter.SelectedValue);
                        ds = getdt.GetProjectwiseDocuments_For_AllUsers(FromDate, ToDate, 0, new Guid(ddlProject.SelectedValue), DDLStatus.SelectedValue, DDLFilter.SelectedValue);
                    }
                }
                else if (RDBReportView.SelectedValue == "User" && ddlProject.SelectedValue != "All Projects" && ddlusers.SelectedValue != "All Users" && ddlProject.SelectedValue != "General Documents")
                {
                    if (dtFromDate.Text != "" && dtToDate.Text != "")
                    {
                        GrdDocuments.DataSource = getdt.GetAllUserDocumentsByPrj(new Guid(ddlusers.SelectedValue), FromDate, ToDate, 1, new Guid(ddlProject.SelectedValue), DDLStatus.SelectedValue, DDLFilter.SelectedValue);
                        ds = getdt.GetAllUserDocumentsByPrj(new Guid(ddlusers.SelectedValue), FromDate, ToDate, 1, new Guid(ddlProject.SelectedValue), DDLStatus.SelectedValue, DDLFilter.SelectedValue);
                    }
                    else
                    {
                        GrdDocuments.DataSource = getdt.GetAllUserDocumentsByPrj(new Guid(ddlusers.SelectedValue), FromDate, ToDate, 0, new Guid(ddlProject.SelectedValue), DDLStatus.SelectedValue, DDLFilter.SelectedValue);
                        ds = getdt.GetAllUserDocumentsByPrj(new Guid(ddlusers.SelectedValue), FromDate, ToDate, 0, new Guid(ddlProject.SelectedValue), DDLStatus.SelectedValue, DDLFilter.SelectedValue);

                    }
                }
                else if (RDBReportView.SelectedValue == "User" && ddlProject.SelectedValue == "All Projects" && ddlusers.SelectedValue == "All Users")
                {
                    if (dtFromDate.Text != "" && dtToDate.Text != "")
                    {
                        GrdDocuments.DataSource = getdt.GetAllProject_AllUser_Documents(FromDate, ToDate, 1, DDLStatus.SelectedValue, DDLFilter.SelectedValue);
                        ds = getdt.GetAllProject_AllUser_Documents(FromDate, ToDate, 1, DDLStatus.SelectedValue, DDLFilter.SelectedValue);
                    }
                    else
                    {
                        GrdDocuments.DataSource = getdt.GetAllProject_AllUser_Documents(FromDate, ToDate, 0, DDLStatus.SelectedValue, DDLFilter.SelectedValue);
                        ds = getdt.GetAllProject_AllUser_Documents(FromDate, ToDate, 0, DDLStatus.SelectedValue, DDLFilter.SelectedValue);
                    }
                }
                else if (RDBReportView.SelectedValue == "User" && ddlProject.SelectedValue == "General Documents" && ddlusers.SelectedValue != "All Users")
                {
                    if (dtFromDate.Text != "" && dtToDate.Text != "")
                    {
                        GrdDocuments.DataSource = getdt.GetGDByUser(new Guid(ddlusers.SelectedValue), FromDate, ToDate, 1, DDLFilter.SelectedValue);
                        ds = getdt.GetGDByUser(new Guid(ddlusers.SelectedValue), FromDate, ToDate, 1, DDLFilter.SelectedValue);

                    }
                    else
                    {
                        GrdDocuments.DataSource = getdt.GetGDByUser(new Guid(ddlusers.SelectedValue), FromDate, ToDate, 0, DDLFilter.SelectedValue);
                        ds = getdt.GetGDByUser(new Guid(ddlusers.SelectedValue), FromDate, ToDate, 0, DDLFilter.SelectedValue);

                    }
                }
                else if (RDBReportView.SelectedValue == "Project" && ddlProject.SelectedValue != "All Projects" && ddlProject.SelectedValue != "General Documents")
                {
                    if (dtFromDate.Text != "" && dtToDate.Text != "")
                    {
                        GrdDocuments.DataSource = getdt.GetProjectwiseDocuments_For_AllUsers(FromDate, ToDate, 1, new Guid(ddlProject.SelectedValue), DDLStatus.SelectedValue, DDLFilter.SelectedValue);
                        ds = getdt.GetProjectwiseDocuments_For_AllUsers(FromDate, ToDate, 1, new Guid(ddlProject.SelectedValue), DDLStatus.SelectedValue, DDLFilter.SelectedValue);
                    }
                    else
                    {
                        GrdDocuments.DataSource = getdt.GetProjectwiseDocuments_For_AllUsers(FromDate, ToDate, 0, new Guid(ddlProject.SelectedValue), DDLStatus.SelectedValue, DDLFilter.SelectedValue);
                        ds = getdt.GetProjectwiseDocuments_For_AllUsers(FromDate, ToDate, 0, new Guid(ddlProject.SelectedValue), DDLStatus.SelectedValue, DDLFilter.SelectedValue);
                    }
                }
                else
                {
                    if (dtFromDate.Text != "" && dtToDate.Text != "")
                    {

                        GrdDocuments.DataSource = getdt.GetAllGeneralDocuments(FromDate, ToDate, 1, DDLFilter.SelectedValue);
                        ds = getdt.GetAllGeneralDocuments(FromDate, ToDate, 1, DDLFilter.SelectedValue);
                    }
                    else
                    {
                        GrdDocuments.DataSource = getdt.GetAllGeneralDocuments(FromDate, ToDate, 0, DDLFilter.SelectedValue);
                        ds = getdt.GetAllGeneralDocuments(FromDate, ToDate, 0, DDLFilter.SelectedValue);
                    }
                }
               GrdDocuments.PageSize = int.Parse(txtPageSize.Text);
                GrdDocuments.DataBind();
                if (ddlPages.SelectedValue == "0")
                {
                    grdDocumentsPDF.AllowPaging = false;
                    grdDocumentsPDF.DataSource = ds;
                    grdDocumentsPDF.DataBind();
                }
                else // current page
                {
                    grdDocumentsPDF.AllowPaging = true;
                    grdDocumentsPDF.PageSize = int.Parse(txtPageSize.Text);
                    grdDocumentsPDF.PageIndex = GrdDocuments.PageIndex;
                    grdDocumentsPDF.PagerSettings.Visible = false;
                    grdDocumentsPDF.DataSource = ds;
                    grdDocumentsPDF.DataBind();
                }
                lblDocumentsNo.Text = "Total No. Of Documents : " + ds.Tables[0].Rows.Count;

                ScriptManager.RegisterStartupScript(Page, this.GetType(), "Key2", "<script>MakeStaticHeader('" + GrdDocuments.ClientID + "', 600, 1300 , 55 ,true); </script>", false);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if(ddlProject.SelectedIndex == 0)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "Alert", "<script language='javascript'>alert('Please select project !');</script>");
                return;
            }
            if (RDBReportView.SelectedValue== "User" &&  ddlusers.SelectedIndex == 0)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "Alert", "<script language='javascript'>alert('Please select user !');</script>");
                return;
            }
            if (ddlOptions.SelectedIndex == 0)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "Alert", "<script language='javascript'>alert('Please select type of report !');</script>");
                return;
            }

            if (!string.IsNullOrEmpty(dtFromDate.Text) && dtToDate.Text == "")
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "Alert", "<script language='javascript'>alert('Please enter To Date !');</script>");
                return;
            }
            if (dtFromDate.Text =="" && dtToDate.Text != "")
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "Alert", "<script language='javascript'>alert('Please enter From Date !');</script>");
                return;
            }
           
            lblDocumentsNo.Visible = true;
            btnExcel.Visible = true;
            btnPDF.Visible = true;
            btnPrint.Visible = true;

            if (RDBReportView.SelectedValue == "User")
            {
                GrdProjectwiseSummary.Visible = false;
                if (ddlOptions.SelectedIndex == 1) //Summary details
                {
                    LoadProjects();
                    divsummary.Visible = true;
                    ddlPages.Visible = false;
                    divDetails.Visible = false;
                }
                else if (ddlOptions.SelectedIndex == 2) // All Details
                {
                    divsummary.Visible = false;
                    divDetails.Visible = true;
                    if (btnExcel.Visible)
                    {
                        ddlPages.Visible = true;
                    }
                    LoadDocuments();
                }
            }
            else
            {
                if (ddlOptions.SelectedIndex == 1) //Summary details
                {
                    divsummary.Visible = true;
                    ddlPages.Visible = false;
                    divDetails.Visible = false;
                    GrdProjectwiseSummary.Visible = true;
                    LoadProjects();
                }
                else
                {
                    GrdProjectwiseSummary.Visible = false;
                    divsummary.Visible = false;
                    divDetails.Visible = true;
                    if (btnExcel.Visible)
                    {
                        ddlPages.Visible = true;
                    }
                    LoadDocuments();
                }
            }
            
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            dtFromDate.Text = "";
            dtToDate.Text = "";
            divsummary.Visible = false;
            divDetails.Visible = false;
            ddlOptions.SelectedIndex = 0;
            ddlProject.SelectedIndex = 0;
            ddlusers.SelectedIndex = 0;
            lblDocumentsNo.Text = "";
            txtPageSize.Text = "10";
            btnExcel.Visible = false;
            btnPDF.Visible = false;
            ddlPages.Visible = false;
            btnPrint.Visible = false;
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

        protected void GrdDcumentHistory_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DateTime FromDate = DateTime.Now;
            DateTime ToDate = DateTime.Now;
            if (!string.IsNullOrEmpty(dtFromDate.Text))
            {
                FromDate = Convert.ToDateTime(getdt.ConvertDateFormat(dtFromDate.Text));
            }
            if (!string.IsNullOrEmpty(dtToDate.Text))
            {
                ToDate = Convert.ToDateTime(getdt.ConvertDateFormat(dtToDate.Text));
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Guid PrjUID = new Guid(e.Row.Cells[2].Text);
                e.Row.Cells[0].Text = ddlusers.SelectedItem.ToString();
                if (name != e.Row.Cells[0].Text)
                {
                    totalcount = 0;
                    totalviewcount = 0;
                    totaldwnldcount = 0;
                    TotalDocumentLinkSentCount = 0;
                    e.Row.Cells[0].Text = ddlusers.SelectedItem.ToString();
                    name = e.Row.Cells[0].Text;
                }
                else
                {
                    e.Row.Cells[0].Text = "";
                }
                if (e.Row.Cells[1].Text != "Total")
                {
                    if (e.Row.Cells[1].Text == "General Documents")
                    {
                        if (dtFromDate.Text != "" && dtToDate.Text != "")
                        {
                            e.Row.Cells[2].Text = getdt.GetGDByUsercount(new Guid(ddlusers.SelectedValue), FromDate, ToDate, 1,"All").ToString();
                            e.Row.Cells[3].Text = getdt.GetDocCountViewDownldGD(new Guid(ddlusers.SelectedValue), FromDate, ToDate, 1, "Downloaded").ToString();
                            e.Row.Cells[4].Text = getdt.GetDocCountViewDownldGD(new Guid(ddlusers.SelectedValue), FromDate, ToDate, 1, "Viewed").ToString();
                            e.Row.Cells[5].Text = getdt.GetUserDocLinkSentCount_GD(new Guid(ddlusers.SelectedValue), FromDate, ToDate, 1).ToString();

                        }
                        else
                        {
                            e.Row.Cells[2].Text = getdt.GetGDByUsercount(new Guid(ddlusers.SelectedValue), FromDate, ToDate, 0,"All").ToString();
                            e.Row.Cells[3].Text = getdt.GetDocCountViewDownldGD(new Guid(ddlusers.SelectedValue), FromDate, ToDate, 0, "Downloaded").ToString();
                            e.Row.Cells[4].Text = getdt.GetDocCountViewDownldGD(new Guid(ddlusers.SelectedValue), FromDate, ToDate, 0, "Viewed").ToString();
                            e.Row.Cells[5].Text = getdt.GetUserDocLinkSentCount_GD(new Guid(ddlusers.SelectedValue), FromDate, ToDate, 0).ToString();
                        }
                        totalgdcount = int.Parse(e.Row.Cells[2].Text);
                    }
                    else
                    {

                        if (dtFromDate.Text != "" && dtToDate.Text != "")
                        {
                            e.Row.Cells[2].Text = getdt.GetProjectdocumnetscountByPrj(new Guid(ddlusers.SelectedValue), PrjUID, FromDate, ToDate, 1).ToString();
                            e.Row.Cells[3].Text = getdt.GetDocCountViewDownld(new Guid(ddlusers.SelectedValue), PrjUID, FromDate, ToDate, 1, "Downloaded").ToString();
                            e.Row.Cells[4].Text = getdt.GetDocCountViewDownld(new Guid(ddlusers.SelectedValue), PrjUID, FromDate, ToDate, 1, "Viewed").ToString();
                            e.Row.Cells[5].Text=getdt.GetDocLinkSentCount(new Guid(ddlusers.SelectedValue), PrjUID, FromDate, ToDate, 1).ToString();
                        }
                        else
                        {
                            e.Row.Cells[2].Text = getdt.GetProjectdocumnetscountByPrj(new Guid(ddlusers.SelectedValue), PrjUID, FromDate, ToDate, 0).ToString();
                            e.Row.Cells[3].Text = getdt.GetDocCountViewDownld(new Guid(ddlusers.SelectedValue), PrjUID, FromDate, ToDate, 0, "Downloaded").ToString();
                            e.Row.Cells[4].Text = getdt.GetDocCountViewDownld(new Guid(ddlusers.SelectedValue), PrjUID, FromDate, ToDate, 0, "Viewed").ToString();
                            e.Row.Cells[5].Text = getdt.GetDocLinkSentCount(new Guid(ddlusers.SelectedValue), PrjUID, FromDate, ToDate, 0).ToString();
                        }

                        totalactcount += int.Parse(e.Row.Cells[2].Text);
                    }
                    totalcount += int.Parse(e.Row.Cells[2].Text);
                    grandtotalcount += int.Parse(e.Row.Cells[2].Text);
                    totaldwnldcount += int.Parse(e.Row.Cells[3].Text);
                    totalviewcount += int.Parse(e.Row.Cells[4].Text);
                    TotalDocumentLinkSentCount += int.Parse(e.Row.Cells[5].Text);
                }
                else if (e.Row.Cells[1].Text == "Total")
                {
                    e.Row.Cells[1].Font.Bold = true;
                    e.Row.Cells[2].Text = totalcount.ToString();
                    e.Row.Cells[3].Text = totaldwnldcount.ToString();
                    e.Row.Cells[4].Text = totalviewcount.ToString();
                    e.Row.Cells[5].Text = TotalDocumentLinkSentCount.ToString();
                }


                lblDocumentsNo.Text = "Total No. Of Docs : " + grandtotalcount + " : No of Activity Docs : " + totalactcount + " : No. Of General Docs : " + totalgdcount;


            }
        }

        protected void grdDocumenthistoryAll_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DateTime FromDate = DateTime.Now;
            DateTime ToDate = DateTime.Now;
            if (!string.IsNullOrEmpty(dtFromDate.Text))
            {
                FromDate = Convert.ToDateTime(getdt.ConvertDateFormat(dtFromDate.Text));
            }
            if (!string.IsNullOrEmpty(dtToDate.Text))
            {
                ToDate = Convert.ToDateTime(getdt.ConvertDateFormat(dtToDate.Text));
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[2].Text != "&nbsp;") // if it is not total row
                {
                    Guid PrjUID = new Guid(e.Row.Cells[2].Text);
                    Guid UserUID = new Guid(e.Row.Cells[0].Text);
                    e.Row.Cells[0].Text = getdt.getUserNameby_UID(UserUID);
                    if (name != e.Row.Cells[0].Text)
                    {
                        totalcount = 0;
                        totalviewcount = 0;
                        totaldwnldcount = 0;
                        TotalDocumentLinkSentCount = 0;
                        name = e.Row.Cells[0].Text;
                    }
                    else
                    {
                        e.Row.Cells[0].Text = "";
                    }

                    if (e.Row.Cells[1].Text == "General Documents")
                    {
                        if (dtFromDate.Text != "" && dtToDate.Text != "")
                        {
                            e.Row.Cells[2].Text = getdt.GetGDByUsercount(UserUID, FromDate, ToDate, 1,"All").ToString();
                            e.Row.Cells[3].Text = getdt.GetDocCountViewDownldGD(UserUID, FromDate, ToDate, 1, "Downloaded").ToString();
                            e.Row.Cells[4].Text = getdt.GetDocCountViewDownldGD(UserUID, FromDate, ToDate, 1, "Viewed").ToString();
                            e.Row.Cells[5].Text = getdt.GetUserDocLinkSentCount_GD(UserUID, FromDate, ToDate, 1).ToString();

                        }
                        else
                        {
                            e.Row.Cells[2].Text = getdt.GetGDByUsercount(UserUID, FromDate, ToDate, 0,"All").ToString();
                            e.Row.Cells[3].Text = getdt.GetDocCountViewDownldGD(UserUID, FromDate, ToDate, 0, "Downloaded").ToString();
                            e.Row.Cells[4].Text = getdt.GetDocCountViewDownldGD(UserUID, FromDate, ToDate, 0, "Viewed").ToString();
                            e.Row.Cells[5].Text = getdt.GetUserDocLinkSentCount_GD(UserUID, FromDate, ToDate, 0).ToString();

                        }
                        totalgdcount += int.Parse(e.Row.Cells[2].Text);
                    }
                    else
                    {

                        if (dtFromDate.Text != "" && dtToDate.Text != "")
                        {
                            e.Row.Cells[2].Text = getdt.GetProjectdocumnetscountByPrj(UserUID, PrjUID, FromDate, ToDate, 1).ToString();
                            e.Row.Cells[3].Text = getdt.GetDocCountViewDownld(UserUID, PrjUID, FromDate, ToDate, 1, "Downloaded").ToString();
                            e.Row.Cells[4].Text = getdt.GetDocCountViewDownld(UserUID, PrjUID, FromDate, ToDate, 1, "Viewed").ToString();
                            e.Row.Cells[5].Text = getdt.GetDocLinkSentCount(UserUID, PrjUID, FromDate, ToDate, 1).ToString();
                        }
                        else
                        {
                            e.Row.Cells[2].Text = getdt.GetProjectdocumnetscountByPrj(UserUID, PrjUID, FromDate, ToDate, 0).ToString();
                            e.Row.Cells[3].Text = getdt.GetDocCountViewDownld(UserUID, PrjUID, FromDate, ToDate, 0, "Downloaded").ToString();
                            e.Row.Cells[4].Text = getdt.GetDocCountViewDownld(UserUID, PrjUID, FromDate, ToDate, 0, "Viewed").ToString();
                            e.Row.Cells[5].Text = getdt.GetDocLinkSentCount(UserUID, PrjUID, FromDate, ToDate, 0).ToString();
                        }
                        totalactcount += int.Parse(e.Row.Cells[2].Text);
                    }
                    totalcount += int.Parse(e.Row.Cells[2].Text);
                    grandtotalcount += int.Parse(e.Row.Cells[2].Text);
                    totaldwnldcount += int.Parse(e.Row.Cells[3].Text);
                    totalviewcount += int.Parse(e.Row.Cells[4].Text);
                    grandtotaldwnldcount += int.Parse(e.Row.Cells[3].Text);
                    grandtotalviewcount += int.Parse(e.Row.Cells[4].Text);
                    TotalDocumentLinkSentCount+= int.Parse(e.Row.Cells[5].Text);
                    GrandTotalDocumentLinkSent += int.Parse(e.Row.Cells[5].Text);
                }
                else
                {
                    e.Row.Cells[1].Font.Bold = true;
                    e.Row.Cells[2].Text = totalcount.ToString();
                    e.Row.Cells[3].Text = totaldwnldcount.ToString();
                    e.Row.Cells[4].Text = totalviewcount.ToString();
                    e.Row.Cells[5].Text = TotalDocumentLinkSentCount.ToString();
                }
                lblDocumentsNo.Text = "Total No. Of Docs : " + grandtotalcount + " : No of Activity Docs : " + totalactcount + " : No. Of General Docs : " + totalgdcount + " : Downloaded Docs : " + grandtotaldwnldcount + " : Viewed Docs : " + grandtotalviewcount + " : Document Link Sent : " + GrandTotalDocumentLinkSent;
                ViewState["grandtotalcount"] = grandtotalcount;
                ViewState["grandtotaldwnldcount"] = grandtotaldwnldcount;
                ViewState["grandtotalviewcount"] = grandtotalviewcount;
                ViewState["GrandTotalDocumentLinkSent"] = GrandTotalDocumentLinkSent;
               

            }
        }

        protected void GrdProjectwiseSummary_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DateTime FromDate = DateTime.Now;
            DateTime ToDate = DateTime.Now;
            if (!string.IsNullOrEmpty(dtFromDate.Text))
            {
                FromDate = Convert.ToDateTime(getdt.ConvertDateFormat(dtFromDate.Text));
            }
            if (!string.IsNullOrEmpty(dtToDate.Text))
            {
                ToDate = Convert.ToDateTime(getdt.ConvertDateFormat(dtToDate.Text));
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[2].Text != "&nbsp;")
                {
                    Guid PrjUID = new Guid(e.Row.Cells[1].Text);
                    
                        if (e.Row.Cells[0].Text == "General Documents")
                        {
                            if (dtFromDate.Text != "" && dtToDate.Text != "")
                            {
                                e.Row.Cells[1].Text = getdt.GetGeneralDocuments(FromDate, ToDate, 1, "All").ToString();
                                e.Row.Cells[2].Text = getdt.GetDocCountViewDownldGD_withoutUser(FromDate, ToDate, 1, "Downloaded").ToString();
                                e.Row.Cells[3].Text = getdt.GetDocCountViewDownldGD_withoutUser(FromDate, ToDate, 1, "Viewed").ToString();
                                e.Row.Cells[4].Text = getdt.GetUserDocLinkSentCount_GD_WithoutUser(FromDate, ToDate, 1).ToString();
                            }
                            else
                            {
                                e.Row.Cells[1].Text = getdt.GetGeneralDocuments(FromDate, ToDate, 0, "All").ToString();
                                e.Row.Cells[2].Text = getdt.GetDocCountViewDownldGD_withoutUser(FromDate, ToDate, 0, "Downloaded").ToString();
                                e.Row.Cells[3].Text = getdt.GetDocCountViewDownldGD_withoutUser(FromDate, ToDate, 0, "Viewed").ToString();
                                e.Row.Cells[4].Text = getdt.GetUserDocLinkSentCount_GD_WithoutUser(FromDate, ToDate, 0).ToString();
                            }

                            totalgdcount += int.Parse(e.Row.Cells[1].Text);
                        }
                        else
                        {
                            if (dtFromDate.Text != "" && dtToDate.Text != "")
                            {
                                e.Row.Cells[1].Text = getdt.GetProjectdocumnetscountByPrj_WithoutUser(PrjUID, FromDate, ToDate, 1).ToString();
                                e.Row.Cells[2].Text = getdt.GetDocCountViewDownld_WithoutUser(PrjUID, FromDate, ToDate, 1, "Downloaded").ToString();
                                e.Row.Cells[3].Text = getdt.GetDocCountViewDownld_WithoutUser(PrjUID, FromDate, ToDate, 1, "Viewed").ToString();
                                e.Row.Cells[4].Text = getdt.GetDocLinkSentCount_WithoutUser(PrjUID, FromDate, ToDate, 1).ToString();
                            }
                            else
                            {
                                e.Row.Cells[1].Text = getdt.GetProjectdocumnetscountByPrj_WithoutUser(PrjUID, FromDate, ToDate, 0).ToString();
                                e.Row.Cells[2].Text = getdt.GetDocCountViewDownld_WithoutUser(PrjUID, FromDate, ToDate, 0, "Downloaded").ToString();
                                e.Row.Cells[3].Text = getdt.GetDocCountViewDownld_WithoutUser(PrjUID, FromDate, ToDate, 0, "Viewed").ToString();
                                e.Row.Cells[4].Text = getdt.GetDocLinkSentCount_WithoutUser(PrjUID, FromDate, ToDate, 0).ToString();
                            }
                            totalactcount += int.Parse(e.Row.Cells[2].Text);
                        }
                        totalcount += int.Parse(e.Row.Cells[1].Text);
                        grandtotalcount += int.Parse(e.Row.Cells[1].Text);
                        totaldwnldcount += int.Parse(e.Row.Cells[2].Text);
                        totalviewcount += int.Parse(e.Row.Cells[3].Text);
                        grandtotaldwnldcount += int.Parse(e.Row.Cells[2].Text);
                        grandtotalviewcount += int.Parse(e.Row.Cells[3].Text);
                        TotalDocumentLinkSentCount += int.Parse(e.Row.Cells[4].Text);
                        GrandTotalDocumentLinkSent += int.Parse(e.Row.Cells[4].Text);
                }
                else
                {
                    e.Row.Cells[0].Font.Bold = true;
                    e.Row.Cells[1].Text = totalcount.ToString();
                    e.Row.Cells[2].Text = totaldwnldcount.ToString();
                    e.Row.Cells[3].Text = totalviewcount.ToString();
                    e.Row.Cells[4].Text = TotalDocumentLinkSentCount.ToString();
                }

                lblDocumentsNo.Text = "Total No. Of Docs : " + grandtotalcount + " : No of Activity Docs : " + totalactcount + " : No. Of General Docs : " + totalgdcount + " : Downloaded Docs : " + grandtotaldwnldcount + " : Viewed Docs : " + grandtotalviewcount + " : Document Link Sent : " + GrandTotalDocumentLinkSent;
                ViewState["grandtotalcount"] = grandtotalcount;
                ViewState["grandtotaldwnldcount"] = grandtotaldwnldcount;
                ViewState["grandtotalviewcount"] = grandtotalviewcount;
                ViewState["GrandTotalDocumentLinkSent"] = GrandTotalDocumentLinkSent;
            }
        }

        protected void GrdDocuments_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HtmlGenericControl divD = (HtmlGenericControl)e.Row.FindControl("divD");
                HtmlGenericControl divGD = (HtmlGenericControl)e.Row.FindControl("divGD");
                DateTime FromDate = DateTime.Now;
                DateTime ToDate = DateTime.Now;
                if (!string.IsNullOrEmpty(dtFromDate.Text))
                {
                    FromDate = Convert.ToDateTime(getdt.ConvertDateFormat(dtFromDate.Text));
                }
                if (!string.IsNullOrEmpty(dtToDate.Text))
                {
                    ToDate = Convert.ToDateTime(getdt.ConvertDateFormat(dtToDate.Text));
                }
                if (e.Row.Cells[2].Text == "" || e.Row.Cells[2].Text == "&nbsp;") // if it is general document
                {
                    divD.Visible = false;
                    divGD.Visible = true;
                }
                if(!string.IsNullOrEmpty(e.Row.Cells[6].Text))
                {
                    if (e.Row.Cells[2].Text == "" || e.Row.Cells[2].Text == "&nbsp;") // if it is general document
                    {
                        if (dtFromDate.Text != "" && dtToDate.Text != "" && ddlusers.SelectedValue != "All Users" && ddlusers.SelectedValue != "Select User")
                        {
                            e.Row.Cells[6].Text = getdt.GetDocCountViewDownldGDByDoc(new Guid(ddlusers.SelectedValue), FromDate, ToDate, 1, "Downloaded", new Guid(e.Row.Cells[6].Text)).ToString();
                        }
                        else if (dtFromDate.Text == "" && dtToDate.Text == "" && ddlusers.SelectedValue != "All Users" && ddlusers.SelectedValue != "Select User")
                        {
                            e.Row.Cells[6].Text = getdt.GetDocCountViewDownldGDByDoc(new Guid(ddlusers.SelectedValue), FromDate, ToDate, 0, "Downloaded", new Guid(e.Row.Cells[6].Text)).ToString();
                        }
                        else if (dtFromDate.Text != "" && dtToDate.Text != "" && ddlusers.SelectedValue == "All Users")
                        {
                            e.Row.Cells[6].Text=getdt.GetDownloadDocumentCount_By_DocUID_For_GD(FromDate, ToDate, 1, "Downloaded", new Guid(e.Row.Cells[6].Text)).ToString();
                        }
                        else if (dtFromDate.Text != "" && dtToDate.Text != "" && ddlusers.SelectedValue == "Select User")
                        {
                            e.Row.Cells[6].Text = getdt.GetDownloadDocumentCount_By_DocUID_For_GD(FromDate, ToDate, 1, "Downloaded", new Guid(e.Row.Cells[6].Text)).ToString();
                        }
                        else
                        {
                            e.Row.Cells[6].Text = getdt.GetDownloadDocumentCount_By_DocUID_For_GD(FromDate, ToDate, 0, "Downloaded", new Guid(e.Row.Cells[6].Text)).ToString();
                        }
                    }
                    else
                    {
                        if (dtFromDate.Text != "" && dtToDate.Text != "" && ddlusers.SelectedValue != "All Users" && ddlusers.SelectedValue != "Select User")
                        {
                            e.Row.Cells[6].Text = getdt.GetDocCountViewDownldByDoc(new Guid(ddlusers.SelectedValue), FromDate, ToDate, 1, "Downloaded", new Guid(e.Row.Cells[6].Text)).ToString();
                        }
                        else if (dtFromDate.Text == "" && dtToDate.Text == "" && ddlusers.SelectedValue != "All Users" && ddlusers.SelectedValue != "Select User")
                        {
                            e.Row.Cells[6].Text = getdt.GetDocCountViewDownldByDoc(new Guid(ddlusers.SelectedValue), FromDate, ToDate, 0, "Downloaded", new Guid(e.Row.Cells[6].Text)).ToString();
                        }
                        else if (dtFromDate.Text != "" && dtToDate.Text != "" && ddlusers.SelectedValue == "All Users")
                        {
                            e.Row.Cells[6].Text = getdt.GetDownloadDocumentCount_by_DocumentUID(FromDate, ToDate, 1, "Downloaded", new Guid(e.Row.Cells[6].Text)).ToString();
                        }
                        else if (dtFromDate.Text != "" && dtToDate.Text != "" && ddlusers.SelectedValue == "Select User")
                        {
                            e.Row.Cells[6].Text = getdt.GetDownloadDocumentCount_by_DocumentUID(FromDate, ToDate, 1, "Downloaded", new Guid(e.Row.Cells[6].Text)).ToString();
                        }
                        else
                        {
                            e.Row.Cells[6].Text = getdt.GetDownloadDocumentCount_by_DocumentUID(FromDate, ToDate, 0, "Downloaded", new Guid(e.Row.Cells[6].Text)).ToString();
                        }
                    }
                }
                string DocumentUID = GrdDocuments.DataKeys[e.Row.RowIndex].Values[0].ToString();

                DataSet ds = getdt.GetDocumentSent_by_DocumentUID(new Guid(DocumentUID));
                if (ds.Tables[0].Rows.Count <= 0)
                {
                    //Label lblcnt = (Label)e.Row.FindControl("LblCount");
                    //lblcnt.Text = "0";
                    //lblcnt.Enabled = false;
                    e.Row.Cells[7].Text = "0";
                    e.Row.Cells[7].Enabled = false;
                }
                else
                {
                    Label lblcnt = (Label)e.Row.FindControl("LblCount");
                    lblcnt.Text = ds.Tables[0].Rows.Count.ToString();
                    e.Row.Cells[7].Enabled = true;
                    lblcnt.Enabled = true;
                }
            }
        }

        protected void ddlProject_SelectedIndexChanged(object sender, EventArgs e)
        {
           // if(ddlProject.SelectedIndex != 0 && ddlProject.SelectedItem.ToString() != "General Documents" && ddlProject.SelectedItem.ToString() != "All Projects")
           //{
           //     DataSet ds = getdt.GetProject_by_ProjectUID(new Guid(ddlProject.SelectedValue));
           //     if(ds.Tables[0].Rows.Count > 0)
           //     {
           //         dtFromDate.Text =DateTime.Parse(ds.Tables[0].Rows[0]["StartDate"].ToString()).ToString("dd/MM/yyyy");
           //         dtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
           //     }
           // }
           // else if(ddlProject.SelectedItem.ToString() == "All Projects")
           // {
           //     List<DateTime> lstdt = new List<DateTime>();
           //     DataSet ds = new DataSet();
           //     for (int i= 2;i < ddlProject.Items.Count;i++)
           //     {
           //         if (ddlProject.Items[i].Text != "General Documents")
           //         {
           //             ds.Clear();
           //             ds = getdt.GetProject_by_ProjectUID(new Guid(ddlProject.Items[i].Value));
           //             if (ds.Tables[0].Rows.Count > 0)
           //             {
           //                 lstdt.Add(DateTime.Parse(ds.Tables[0].Rows[0]["StartDate"].ToString()));
           //             }
           //         }
           //     }
           //     dtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
           //     dtFromDate.Text = lstdt.Min(p => p).ToString("dd/MM/yyyy");
           // }
            LoadUsers();

            //if (RDBReportView.SelectedValue == "Project" && ddlProject.SelectedValue == "All Projects")
            //{
            //    if (ddlOptions.Items.Count == 3)
            //    {
            //        ddlOptions.Items.RemoveAt(2);
            //    }
            //}
            //else
            //{
            //    if (ddlOptions.Items.Count == 2)
            //    {
            //        ddlOptions.Items.Insert(2, new System.Web.UI.WebControls.ListItem("All Details", "2"));
            //    }
                
            //}
        }

        protected void ddlOptions_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlOptions.SelectedValue == "2")
            {
                DivStatus.Visible = true;
                DivFilter.Visible = true;
                divDetialsPaging.Visible = true;
                if (btnExcel.Visible)
                {
                    ddlPages.Visible = true;
                }
            }
            else
            {
                DivStatus.Visible = false;
                DivFilter.Visible = false;
                ddlPages.Visible = false;
                divDetialsPaging.Visible = false;
            }
            btnPDF.Visible = false;
            btnPrint.Visible = false;
            btnExcel.Visible = false;
            ddlPages.Visible = false;

        }

        protected void GrdDocuments_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdDocuments.PageIndex = e.NewPageIndex;
            LoadDocuments();
        }

        public string GetUserName(string UserUID)
        {
            return getdt.getUserNameby_UID(new Guid(UserUID));
        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                StringBuilder StrExport = new StringBuilder();

                StringWriter stw = new StringWriter();
                HtmlTextWriter htextw = new HtmlTextWriter(stw);
                htextw.AddStyleAttribute("font-size", "9pt");
                htextw.AddStyleAttribute("color", "Black");
                string fromdate = "";
                string todate = "";
                string documentsNo = "";
                string page = "";
                if (ddlOptions.SelectedIndex == 1)
                {
                    if (RDBReportView.SelectedValue == "User" && ddlusers.SelectedValue != "All Users")
                    {
                        GrdDcumentHistory.RenderControl(htextw);
                        documentsNo = lblDocumentsNo.Text;
                    }
                    else if (RDBReportView.SelectedValue == "Project" && ddlusers.SelectedValue == "Select User")
                    {
                        GrdProjectwiseSummary.RenderControl(htextw);
                        documentsNo = lblDocumentsNo.Text;
                    }
                    else
                    {
                        grdDocumenthistoryAll.RenderControl(htextw); //Name of the Panel
                        documentsNo = lblDocumentsNo.Text;
                    }
                }
                else if (ddlOptions.SelectedIndex == 2) // All Details
                {
                    if (ddlPages.SelectedValue == "0") // All Pages
                    {
                        LoadDocuments();
                        grdDocumentsPDF.Visible = true;
                        grdDocumentsPDF.RenderControl(htextw); //Name of the Panel
                        documentsNo = lblDocumentsNo.Text;
                        page = "All";
                    }
                    else
                    {
                        LoadDocuments();
                        grdDocumentsPDF.Visible = true;
                        grdDocumentsPDF.RenderControl(htextw);
                        documentsNo ="Total No. Of Documents : " +  grdDocumentsPDF.Rows.Count.ToString() ;
                        page = (grdDocumentsPDF.PageIndex + 1).ToString();
                    }
                }

                if(dtFromDate.Text == "")
                {
                    fromdate = getDates()[0];
                }
                else
                {
                    fromdate = dtFromDate.Text;
                }
                if (dtToDate.Text == "")
                {
                    todate = getDates()[1];
                }
                else
                {
                    todate = dtToDate.Text;
                }
                //var sb1 = new StringBuilder();
                //GridDiv.RenderControl(new HtmlTextWriter(new StringWriter(sb1)));
                string Heading = "";
                if (RDBReportView.SelectedValue == "User")
                {
                    Heading = "User Wise Document Upload";
                }
                else
                {
                    Heading = "Project Wise Document Upload";
                }
                string s = htextw.InnerWriter.ToString();
                grdDocumentsPDF.Visible = false;
                string HTMLstring = "<html><body>" +
                    "<div style='width:100%; margin:auto;'><div style='width:100%; float:left; line-height:25px; font-size:10pt;' align='center'>" +
                    "<asp:Label ID='Lbl1' runat='server' Font-Bold='true'>" + WebConfigurationManager.AppSettings["Domain"] + " Report Name: "+ Heading +"</asp:Label><br />" +
                     "<asp:Label ID='Lbl4' runat='server'>Project Name : " + ddlProject.SelectedItem.Text + "&nbsp;&nbsp;User : " + ddlusers.SelectedItem.Text + "&nbsp;&nbsp;Report  : " + ddlOptions.SelectedItem.ToString() + "&nbsp;&nbsp;Start Date : " + fromdate + "&nbsp;&nbsp;End Date : " + todate + "</asp:Label><br/>" +
                    "<asp:Label ID='Lbl6' runat='server' >" + documentsNo +  "</asp:Label><br />" +
                     "<div align='right'><asp:Label ID='Lbl6' runat='server' >" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + "</asp:Label></div><br />" +
                    "</div> <div style='width:100%; float:left; height:10px;'>&nbsp;&nbsp;&nbsp;</div>" +
                    "<div style='width:100%; float:left;'>" +
                    s +
                    "</div>" +
                    "</div></body></html>";
                string status = "";
                if(DDLStatus.SelectedIndex == 0)
                {
                    status = "All";
                }
                else
                {
                    status = DDLStatus.SelectedItem.Text;
                }
                if(ddlOptions.SelectedIndex == 2) // All Details
                {
                    HTMLstring = "<html><body>" +
                    "<div style='width:100%; margin:auto;'><div style='width:100%; float:left; line-height:10px; font-size:10pt;' align='center'>" +
                    "<asp:Label ID='Lbl1' runat='server' Font-Bold='true'>" + WebConfigurationManager.AppSettings["Domain"] + " Report Name: User/Project Wise Document Upload Report</asp:Label><br />" +
                      "<asp:Label ID='Lbl4' runat='server'>Project Name : " + ddlProject.SelectedItem.Text + "&nbsp;&nbsp;User : " + ddlusers.SelectedItem.Text + "&nbsp;&nbsp;Report  : " + ddlOptions.SelectedItem.ToString() + "&nbsp;&nbsp;Start Date : " + fromdate + "&nbsp;&nbsp;End Date : " + todate + "&nbsp;&nbsp;Status : " + status + "&nbsp;&nbsp;Filter : " + DDLFilter.SelectedItem.Text + "&nbsp;&nbsp;Page No : " + page + "</asp:Label><br/>" +
                    "<asp:Label ID='Lbl6' runat='server' >" + documentsNo + "</asp:Label><br />" +
                   "<div align='right'><asp:Label ID='Lbl6' runat='server' >" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + "</asp:Label></div><br />" +
                    "</div> <div style='width:100%; float:left; height:10px;'>&nbsp;&nbsp;&nbsp;</div>" +
                    "<div style='width:100%; float:left;'>" +
                    s +
                    "</div>" +
                    "</div></body></html>";
                }
                string strFile = "";
                if (RDBReportView.SelectedValue == "User")
                {
                    strFile = "Report_Userwise_" + ddlOptions.SelectedItem.ToString() + "_" + DateTime.Now.Ticks + ".xls";
                }
                else
                {
                    strFile = "Report_Projectwise_" + ddlOptions.SelectedItem.ToString() + "_" + DateTime.Now.Ticks + ".xls";
                }
                
                string strcontentType = "application/excel";
                Response.ClearContent();
                Response.ClearHeaders();
                Response.BufferOutput = true;
                Response.ContentType = strcontentType;
                Response.AddHeader("Content-Disposition", "attachment; filename=" + strFile);
                //  Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                //Response.AppendHeader("Content-Disposition", string.Format("attachment;filename={0}.xls", strFile));
                Response.Write(HTMLstring);
                Response.Flush();
                Response.Close();
                Response.End();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            //required to avoid the runtime error "  
            //Control 'GridView1' of type 'GridView' must be placed inside a form tag with runat=server."  
        }

        protected void btnPDF_Click(object sender, EventArgs e)
        {
            if (ddlOptions.SelectedIndex == 1) // Summary details
            {
                if (RDBReportView.SelectedValue == "User" && ddlusers.SelectedValue == "All Users")
                {
                    ExporttoPDF(grdDocumenthistoryAll, 1);
                }
                else if (RDBReportView.SelectedValue == "Project" && ddlusers.SelectedValue == "Select User")
                {
                    ExporttoPDF(GrdProjectwiseSummary, 2);
                }
                else
                {
                    ExporttoPDF(GrdDcumentHistory, 2);
                }
            }
            else if (ddlOptions.SelectedIndex == 2) // All details
            {
                if (ddlPages.SelectedValue == "0")
                {
                    ExporttoPDFDetails(grdDocumentsPDF, 2);
                    
                }
                else //current page
                {
                    LoadDocuments();
                    ExporttoPDFDetails(grdDocumentsPDF, 2);
                }
                grdDocumentsPDF.Visible = false;
            }
        }

        private void ExporttoPDF(GridView gd,int type)
        {
            try
            {
                
                DateTime CDate1 = DateTime.Now;
                GridView gdRp = new GridView();
                gdRp = gd;
               
                int noOfColumns = 0, noOfRows = 0;
                DataTable tbl = null;
                string fromdate = "";
                string todate = "";
                if (dtFromDate.Text == "")
                {
                    fromdate = getDates()[0];
                }
                else
                {
                    fromdate = dtFromDate.Text;
                }
                if (dtToDate.Text == "")
                {
                    todate = getDates()[1];
                }
                else
                {
                    todate = dtToDate.Text;
                }
                if (gdRp.AutoGenerateColumns)
                {
                    tbl = gdRp.DataSource as DataTable; // Gets the DataSource of the GridView Control.
                    noOfColumns = tbl.Columns.Count;
                    noOfRows = tbl.Rows.Count;
                }
                else
                {
                    noOfColumns = gdRp.Columns.Count;
                    noOfRows = gdRp.Rows.Count;
                }

                float HeaderTextSize = 9;
                float ReportNameSize = 9;
                float ReportTextSize = 9;
                float ApplicationNameSize = 13;
                string ProjectName = ddlProject.SelectedItem.ToString();
                string Username = "";
                string Heading = "";
                if (RDBReportView.SelectedValue == "User")
                {
                    Username = ddlusers.SelectedItem.ToString();

                    //if (Username.Length > 30)
                    //{
                    //    Username = Username.Substring(0, 29) + "..";
                    //}
                    Heading = "User Wise Document Upload";
                }
                else
                {
                    Heading = "Project Wise Document Upload";
                }

                //
               



                //
                //if(ProjectName.Length > 30)
                //{
                //    ProjectName = ProjectName.Substring(0, 29) + "..";
                //}


                //float HeaderTextSize = 15;
                //float ReportNameSize = 18;
                //float ReportTextSize = 15;
                //float ApplicationNameSize = 20;


                // Creates a PDF document

                Document document = null;
                //if (LandScape == true)
                //{
                // Sets the document to A4 size and rotates it so that the     orientation of the page is Landscape.
                document = new Document(PageSize.A4.Rotate(), 0, 0, 0, 0);
                //}
                //else
                //{
                //    document = new Document(PageSize.A4, 0, 0, 15, 5);
                //}

                // Creates a PdfPTable with column count of the table equal to no of columns of the gridview or gridview datasource.
                iTextSharp.text.pdf.PdfPTable mainTable = new iTextSharp.text.pdf.PdfPTable(noOfColumns);

                // Sets the first 4 rows of the table as the header rows which will be repeated in all the pages.
                mainTable.HeaderRows = 4;

                // Creates a PdfPTable with 2 columns to hold the header in the exported PDF.
                iTextSharp.text.pdf.PdfPTable headerTable = new iTextSharp.text.pdf.PdfPTable(1);

                // Creates a phrase to hold the application name at the left hand side of the header.
                Phrase phApplicationName = new Phrase(Environment.NewLine + WebConfigurationManager.AppSettings["Domain"] + " Report Name: " + Heading + " (" + ddlOptions.SelectedItem.ToString() + ")", FontFactory.GetFont("Arial", ApplicationNameSize, iTextSharp.text.Font.BOLD));

                // Creates a PdfPCell which accepts a phrase as a parameter.
                PdfPCell clApplicationName = new PdfPCell(phApplicationName);
                // Sets the border of the cell to zero.
                clApplicationName.Border = PdfPCell.NO_BORDER;
                // Sets the Horizontal Alignment of the PdfPCell to left.
                clApplicationName.HorizontalAlignment = Element.ALIGN_CENTER;

                // Creates a phrase to show the current date at the right hand side of the header.
                //Phrase phDate = new Phrase(DateTime.Now.Date.ToString("dd/MM/yyyy"), FontFactory.GetFont("Arial", ApplicationNameSize, iTextSharp.text.Font.NORMAL));

                //// Creates a PdfPCell which accepts the date phrase as a parameter.
                //PdfPCell clDate = new PdfPCell(phDate);
                //// Sets the Horizontal Alignment of the PdfPCell to right.
                //clDate.HorizontalAlignment = Element.ALIGN_RIGHT;
                //// Sets the border of the cell to zero.
                //clDate.Border = PdfPCell.NO_BORDER;

                // Adds the cell which holds the application name to the headerTable.
                headerTable.AddCell(clApplicationName);
                // Adds the cell which holds the date to the headerTable.
                //  headerTable.AddCell(clDate);
                // Sets the border of the headerTable to zero.
                headerTable.DefaultCell.Border = PdfPCell.NO_BORDER;

                // Creates a PdfPCell that accepts the headerTable as a parameter and then adds that cell to the main PdfPTable.
                PdfPCell cellHeader = new PdfPCell(headerTable);
                cellHeader.Border = PdfPCell.NO_BORDER;
                // Sets the column span of the header cell to noOfColumns.
                cellHeader.Colspan = noOfColumns;
                // Adds the above header cell to the table.
                mainTable.AddCell(cellHeader);

                // Creates a phrase which holds the file name.
                int len = 100 - ProjectName.Length;
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(ProjectName);
                // sb.Append("".PadLeft(len, ' ').Replace(" ", " "));
                for(int i=0;i <= len;i ++)
                {
                    sb.Append("");
                }
               // sb.Append("".PadLeft(len, ' ').Replace(" ", " "));

                if (ddlProject.SelectedIndex == 1)
                {
                    len = 100 - fromdate.Length - 2;
                }
                else
                {
                    len = 100 - fromdate.Length;
                }
               
                System.Text.StringBuilder sbdate = new System.Text.StringBuilder();
                sbdate.Append(fromdate);
                //sbdate.Append("".PadLeft(len+9, ' ').Replace(" ", " "));
                for (int i = 0; i <= len; i++)
                {
                    sbdate.Append("");
                }

                //
                Phrase phHeader = new Phrase();
                if (RDBReportView.SelectedValue == "User")
                {
                    //phHeader = new Phrase("Project Name :" + sb + ", User :" + Username + Environment.NewLine + lblDocumentsNo.Text + Environment.NewLine + "Start Date :" + fromdate + ", End Date : " + todate, FontFactory.GetFont("Arial", ReportNameSize, iTextSharp.text.Font.BOLD));
                    phHeader = new Phrase("Project Name : " + sb + " , User : " + Username  + Environment.NewLine + "Start Date : " + sbdate + " , End Date : " + todate, FontFactory.GetFont("Arial", ReportNameSize, iTextSharp.text.Font.BOLD));
                }
                else
                {
                    //phHeader = new Phrase("Project Name :" + ProjectName  + Environment.NewLine + lblDocumentsNo.Text + Environment.NewLine + "Start Date :" + fromdate + ", End Date : " + todate, FontFactory.GetFont("Arial", ReportNameSize, iTextSharp.text.Font.BOLD));
                    phHeader = new Phrase("Project Name : " + ProjectName   + Environment.NewLine + "Start Date : " + sbdate + " , End Date : " + todate, FontFactory.GetFont("Arial", ReportNameSize, iTextSharp.text.Font.BOLD));
                }
                PdfPCell clHeader = new PdfPCell(phHeader);
                clHeader.Colspan = noOfColumns;
                clHeader.Border = PdfPCell.NO_BORDER;
                clHeader.HorizontalAlignment = Element.ALIGN_LEFT;
                mainTable.AddCell(clHeader);

              

                // Creates a phrase for a new line.
                Phrase phSpace = new Phrase("\n");
                PdfPCell clSpace = new PdfPCell(phSpace);
                clSpace.Border = PdfPCell.NO_BORDER;
                clSpace.Colspan = noOfColumns;
                mainTable.AddCell(clSpace);

                // Sets the gridview column names as table headers.
                for (int i = 0; i < noOfColumns; i++)
                {
                    Phrase ph = null;

                    if (gdRp.AutoGenerateColumns)
                    {
                        ph = new Phrase(tbl.Columns[i].ColumnName.Replace("<br/>", Environment.NewLine), FontFactory.GetFont("Arial", HeaderTextSize, iTextSharp.text.Font.BOLD));
                    }
                    else
                    {
                        ph = new Phrase(gdRp.Columns[i].HeaderText.Replace("<br/>", Environment.NewLine), FontFactory.GetFont("Arial", HeaderTextSize, iTextSharp.text.Font.BOLD));
                    }
                    PdfPCell cl = new PdfPCell(ph);
                    if (i == 0)
                    {
                        cl.HorizontalAlignment = Element.ALIGN_LEFT;
                    }
                    else if (i == 1)
                    {
                        if (RDBReportView.SelectedValue == "User")
                        {
                            cl.HorizontalAlignment = Element.ALIGN_LEFT;
                        }
                        else
                        {
                            cl.HorizontalAlignment = Element.ALIGN_CENTER;
                        }
                    }
                    else
                    {
                        cl.HorizontalAlignment = Element.ALIGN_CENTER;
                    }
                    mainTable.AddCell(cl);
                }

                // Reads the gridview rows and adds them to the mainTable
                for (int rowNo = 0; rowNo <= noOfRows; rowNo++)
                {
                    if (rowNo != noOfRows)
                    {
                        for (int columnNo = 0; columnNo < noOfColumns; columnNo++)
                        {
                            if (gdRp.AutoGenerateColumns)
                            {
                                string s = gdRp.Rows[rowNo].Cells[columnNo].Text.Trim();
                                Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                                PdfPCell cl = new PdfPCell(ph);
                                cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                mainTable.AddCell(cl);
                            }
                            else
                            {
                                if (columnNo == 3)
                                {
                                    if (gdRp.Columns[columnNo] is TemplateField)
                                    {
                                        DataBoundLiteralControl lc = gdRp.Rows[rowNo].Cells[columnNo].Controls[0] as DataBoundLiteralControl;
                                        string s = lc.Text.Trim();
                                        Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                                        PdfPCell cl = new PdfPCell(ph);
                                        cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                        mainTable.AddCell(cl);
                                    }
                                    else
                                    {
                                        string s = gdRp.Rows[rowNo].Cells[columnNo].Text.Trim();
                                        s = s.Replace("&nbsp;", "");
                                        Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                                        PdfPCell cl = new PdfPCell(ph);
                                        cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                        mainTable.AddCell(cl);
                                    }
                                }
                                else if (columnNo == 5)
                                {
                                    if (gdRp.Columns[columnNo] is TemplateField)
                                    {
                                        DataBoundLiteralControl lc = gdRp.Rows[rowNo].Cells[columnNo].Controls[0] as DataBoundLiteralControl;
                                        string s = lc.Text.Trim();
                                        Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                                        PdfPCell cl = new PdfPCell(ph);
                                        cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                        mainTable.AddCell(cl);
                                    }
                                    else
                                    {
                                        string s = gdRp.Rows[rowNo].Cells[columnNo].Text.Trim();
                                        s = s.Replace("&nbsp;", "");
                                        Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                                        PdfPCell cl = new PdfPCell(ph);
                                        cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                        mainTable.AddCell(cl);
                                    }
                                }
                                else
                                {
                                    if (gdRp.Columns[columnNo] is TemplateField)
                                    {
                                        DataBoundLiteralControl lc = gdRp.Rows[rowNo].Cells[columnNo].Controls[0] as DataBoundLiteralControl;
                                        string s = lc.Text.Trim();
                                        Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                                        PdfPCell cl = new PdfPCell(ph);
                                        cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                        mainTable.AddCell(cl);
                                    }
                                    else
                                    {
                                        string s = gdRp.Rows[rowNo].Cells[columnNo].Text.Trim();
                                        s = s.Replace("&nbsp;", "");
                                        if (s.Contains("Total"))
                                        {
                                            Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.BOLD));
                                            PdfPCell cl = new PdfPCell(ph);
                                            cl.HorizontalAlignment = Element.ALIGN_LEFT;
                                            mainTable.AddCell(cl);
                                        }
                                        else if (columnNo == 0)
                                        {
                                            
                                            Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                                            PdfPCell cl = new PdfPCell(ph);
                                            cl.HorizontalAlignment = Element.ALIGN_LEFT;
                                            mainTable.AddCell(cl);
                                        }
                                        else if (columnNo == 1)
                                        {
                                            if (RDBReportView.SelectedValue == "User")
                                            {
                                                Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                                                PdfPCell cl = new PdfPCell(ph);
                                                cl.HorizontalAlignment = Element.ALIGN_LEFT;
                                                mainTable.AddCell(cl);
                                            }
                                            else
                                            {
                                                Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                                                PdfPCell cl = new PdfPCell(ph);
                                                cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                                mainTable.AddCell(cl);
                                            }
                                        }
                                        else 
                                        {
                                            Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                                            PdfPCell cl = new PdfPCell(ph);
                                            cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                            mainTable.AddCell(cl);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (type == 1)
                        {
                            for (int columnNo = 0; columnNo < noOfColumns; columnNo++)
                            {
                                string s = "Total";

                                if (columnNo == 1)
                                {
                                    Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.BOLD));
                                    PdfPCell cl = new PdfPCell(ph);
                                    cl.HorizontalAlignment = Element.ALIGN_LEFT;
                                    mainTable.AddCell(cl);
                                }
                                else if (columnNo == 2)
                                {
                                    s = ViewState["grandtotalcount"].ToString();// Session["AwardedValue"].ToString();
                                    Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.BOLD));
                                    PdfPCell cl = new PdfPCell(ph);
                                    cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                    mainTable.AddCell(cl);
                                }
                                else if (columnNo == 3)
                                {
                                    s = ViewState["grandtotaldwnldcount"].ToString();// Session["ExpenditureOverallTotal"].ToString();
                                    Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.BOLD));
                                    PdfPCell cl = new PdfPCell(ph);
                                    cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                    mainTable.AddCell(cl);
                                }
                                else if (columnNo == 4)
                                {
                                    s = ViewState["grandtotalviewcount"].ToString();// Session["TargetedOverallTotal"].ToString() + "%";
                                    Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.BOLD));
                                    PdfPCell cl = new PdfPCell(ph);
                                    cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                    mainTable.AddCell(cl);
                                }
                                else if (columnNo == 5)
                                {
                                    s = ViewState["GrandTotalDocumentLinkSent"].ToString();// Session["AchievedOverallTotal"].ToString() + "%";
                                    Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.BOLD));
                                    PdfPCell cl = new PdfPCell(ph);
                                    cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                    mainTable.AddCell(cl);
                                }
                                else
                                {
                                    Phrase ph = new Phrase("", FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.BOLD));
                                    PdfPCell cl = new PdfPCell(ph);
                                    cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                    mainTable.AddCell(cl);
                                }

                            }
                        }

                    }

                    // Tells the mainTable to complete the row even if any cell is left incomplete.
                    mainTable.CompleteRow();
                }

                // Gets the instance of the document created and writes it to the output stream of the Response object.
                PdfWriter.GetInstance(document, Response.OutputStream);
                //PdfWriter.GetInstance(document, new FileStream(Server.MapPath("~") + "mypdf.pdf", FileMode.Create));
                // Creates a footer for the PDF document.
                iTextSharp.text.Font foot = new iTextSharp.text.Font();
                foot.Size = 10;

                len = 174;
                System.Text.StringBuilder time = new System.Text.StringBuilder();
                time.Append(DateTime.Now.ToString("hh:mm tt"));
                time.Append("".PadLeft(len, ' ').Replace(" ", " "));
                HeaderFooter pdfFooter = new HeaderFooter(new Phrase("Date : " + DateTime.Now.ToString("dd/MM/yyyy") + " Time " + time + "  Page: ", foot), new Phrase(Environment.NewLine + " ", foot));
                pdfFooter.Alignment = Element.ALIGN_CENTER;
                //pdfFooter.Border = iTextSharp.text.Rectangle.NO_BORDER;

                //// Sets the document footer to pdfFooter.
                document.Footer = pdfFooter;

               // document.Footer = new HeaderFooter(new Phrase("        Date : " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + "   Page: ", foot), new Phrase(Environment.NewLine + " ", foot));
                

                // Opens the document.
                document.Open();
                // Adds the mainTable to the document.
                document.Add(mainTable);
                // Closes the document.
                document.Close();
              

                //Response.Buffer = true;
                Response.ContentType = "application/pdf";
                // Response.Cache.SetCacheability(HttpCacheability.NoCache);
                //Response.AddHeader("content-disposition", "attachment; filename= SampleExport.pdf");
                if (RDBReportView.SelectedValue == "User")
                {
                    Response.AddHeader("content-disposition", "attachment;filename=Report_UserDocumetUpload_SummaryDetails_" + DateTime.Now.Ticks + ".pdf");
                }
                else
                {
                    Response.AddHeader("content-disposition", "attachment;filename=Report_ProjectwiseDocumetUpload_SummaryDetails_" + DateTime.Now.Ticks + ".pdf");
                }
                
                //Response.AddHeader("content-disposition", "inline;filename=Report_UserDocumetUpload_SummaryDetails_" + DateTime.Now.Ticks + ".pdf");
               // Response.AppendHeader("Content-Disposition", "attachment; filename=FileName.pdf");
               // Response.TransmitFile(Server.MapPath("~/RegExcel/Sample.pdf"));
                Response.End();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void grdDocumentsPDF_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
              //  HtmlGenericControl divD = (HtmlGenericControl)e.Row.FindControl("divD");
              //  HtmlGenericControl divGD = (HtmlGenericControl)e.Row.FindControl("divGD");
                DateTime FromDate = DateTime.Now;
                DateTime ToDate = DateTime.Now;
                if (!string.IsNullOrEmpty(dtFromDate.Text))
                {
                    FromDate = Convert.ToDateTime(getdt.ConvertDateFormat(dtFromDate.Text));
                }
                if (!string.IsNullOrEmpty(dtToDate.Text))
                {
                    ToDate = Convert.ToDateTime(getdt.ConvertDateFormat(dtToDate.Text));
                }
                if (e.Row.Cells[2].Text == "" || e.Row.Cells[2].Text == "&nbsp;") // if it is general document
                {
                  //  divD.Visible = false;
                  //  divGD.Visible = true;
                }
                if (!string.IsNullOrEmpty(e.Row.Cells[6].Text))
                {
                    if (e.Row.Cells[2].Text == "" || e.Row.Cells[2].Text == "&nbsp;") // if it is general document
                    {
                        if (dtFromDate.Text != "" && dtToDate.Text != "" && ddlusers.SelectedValue != "All Users" && ddlusers.SelectedValue != "Select User")
                        {
                            e.Row.Cells[6].Text = getdt.GetDocCountViewDownldGDByDoc(new Guid(ddlusers.SelectedValue), FromDate, ToDate, 1, "Downloaded", new Guid(e.Row.Cells[6].Text)).ToString();
                        }
                        else if (dtFromDate.Text == "" && dtToDate.Text == "" && ddlusers.SelectedValue != "All Users" && ddlusers.SelectedValue != "Select User")
                        {
                            e.Row.Cells[6].Text = getdt.GetDocCountViewDownldGDByDoc(new Guid(ddlusers.SelectedValue), FromDate, ToDate, 0, "Downloaded", new Guid(e.Row.Cells[6].Text)).ToString();
                        }
                        else if (dtFromDate.Text != "" && dtToDate.Text != "" && ddlusers.SelectedValue == "All Users")
                        {
                            e.Row.Cells[6].Text = getdt.GetDownloadDocumentCount_By_DocUID_For_GD(FromDate, ToDate, 1, "Downloaded", new Guid(e.Row.Cells[6].Text)).ToString();
                        }
                        else if (dtFromDate.Text != "" && dtToDate.Text != "" && ddlusers.SelectedValue == "Select User")
                        {
                            e.Row.Cells[6].Text = getdt.GetDownloadDocumentCount_By_DocUID_For_GD(FromDate, ToDate, 1, "Downloaded", new Guid(e.Row.Cells[6].Text)).ToString();
                        }
                        else
                        {
                            e.Row.Cells[6].Text = getdt.GetDownloadDocumentCount_By_DocUID_For_GD(FromDate, ToDate, 0, "Downloaded", new Guid(e.Row.Cells[6].Text)).ToString();
                        }
                    }
                    else
                    {
                        if (dtFromDate.Text != "" && dtToDate.Text != "" && ddlusers.SelectedValue != "All Users" && ddlusers.SelectedValue != "Select User")
                        {
                            e.Row.Cells[6].Text = getdt.GetDocCountViewDownldByDoc(new Guid(ddlusers.SelectedValue), FromDate, ToDate, 1, "Downloaded", new Guid(e.Row.Cells[6].Text)).ToString();
                        }
                        else if (dtFromDate.Text == "" && dtToDate.Text == "" && ddlusers.SelectedValue != "All Users" && ddlusers.SelectedValue != "Select User")
                        {
                            e.Row.Cells[6].Text = getdt.GetDocCountViewDownldByDoc(new Guid(ddlusers.SelectedValue), FromDate, ToDate, 0, "Downloaded", new Guid(e.Row.Cells[6].Text)).ToString();
                        }
                        else if (dtFromDate.Text != "" && dtToDate.Text != "" && ddlusers.SelectedValue == "All Users")
                        {
                            e.Row.Cells[6].Text = getdt.GetDownloadDocumentCount_by_DocumentUID(FromDate, ToDate, 1, "Downloaded", new Guid(e.Row.Cells[6].Text)).ToString();
                        }
                        else if (dtFromDate.Text != "" && dtToDate.Text != "" && ddlusers.SelectedValue == "Select User")
                        {
                            e.Row.Cells[6].Text = getdt.GetDownloadDocumentCount_by_DocumentUID(FromDate, ToDate, 1, "Downloaded", new Guid(e.Row.Cells[6].Text)).ToString();
                        }
                        else
                        {
                            e.Row.Cells[6].Text = getdt.GetDownloadDocumentCount_by_DocumentUID(FromDate, ToDate, 0, "Downloaded", new Guid(e.Row.Cells[6].Text)).ToString();
                        }
                    }
                }
                string DocumentUID = grdDocumentsPDF.DataKeys[e.Row.RowIndex].Values[0].ToString();

                DataSet ds = getdt.GetDocumentSent_by_DocumentUID(new Guid(DocumentUID));
                if (ds.Tables[0].Rows.Count <= 0)
                {
                    //Label lblcnt = (Label)e.Row.FindControl("LblCount");
                    //lblcnt.Text = "0";
                    //lblcnt.Enabled = false;
                    e.Row.Cells[7].Text = "0";
                    e.Row.Cells[7].Enabled = true;
                }
                else
                {

                    e.Row.Cells[7].Text = ds.Tables[0].Rows.Count.ToString();
                    e.Row.Cells[7].Enabled = true;
                    
                }
            }
        }

        private void ExporttoPDFDetails(GridView gd, int type)
        {
            try
            {

                DateTime CDate1 = DateTime.Now;
                GridView gdRp = new GridView();
                gdRp = gd;

                int noOfColumns = 0, noOfRows = 0;
                DataTable tbl = null;
                string fromdate = "";
                string todate = "";
                if (dtFromDate.Text == "")
                {
                    fromdate = getDates()[0];
                }
                else
                {
                    fromdate = dtFromDate.Text;
                }
                if (dtToDate.Text == "")
                {
                    todate = getDates()[1];
                }
                else
                {
                    todate = dtToDate.Text;
                }
                if (gdRp.AutoGenerateColumns)
                {
                    tbl = gdRp.DataSource as DataTable; // Gets the DataSource of the GridView Control.
                    noOfColumns = tbl.Columns.Count;
                    noOfRows = tbl.Rows.Count;
                }
                else
                {
                    noOfColumns = gdRp.Columns.Count;
                    noOfRows = gdRp.Rows.Count;
                }

                float HeaderTextSize = 9;
                float ReportNameSize = 9;
                float ReportTextSize = 9;
                float ApplicationNameSize = 13;
                string ProjectName = ddlProject.SelectedItem.ToString();
                string Username = ddlusers.SelectedItem.ToString();
                //if (ProjectName.Length > 30)
                //{
                //    ProjectName = ProjectName.Substring(0, 29) + "..";
                //}
                //if (Username.Length > 30)
                //{
                //    Username = Username.Substring(0, 29) + "..";
                //}
                //float HeaderTextSize = 15;
                //float ReportNameSize = 18;
                //float ReportTextSize = 15;
                //float ApplicationNameSize = 20;
                string status = "";
                string page = (gd.PageIndex + 1).ToString();
                if (DDLStatus.SelectedIndex == 0)
                {
                    status = "All";
                    
                }
                else
                {
                    status = DDLStatus.SelectedItem.Text;
                }
                if(ddlPages.SelectedIndex == 0)
                {
                    page = "All";
                }

                // Creates a PDF document

                Document document = null;
                //if (LandScape == true)
                //{
                // Sets the document to A4 size and rotates it so that the     orientation of the page is Landscape.
                document = new Document(PageSize.A4.Rotate(), 0, 0, 0, 0);
                //}
                //else
                //{
                //    document = new Document(PageSize.A4, 0, 0, 15, 5);
                //}

                // Creates a PdfPTable with column count of the table equal to no of columns of the gridview or gridview datasource.
                iTextSharp.text.pdf.PdfPTable mainTable = new iTextSharp.text.pdf.PdfPTable(noOfColumns);

                // Sets the first 4 rows of the table as the header rows which will be repeated in all the pages.
                mainTable.HeaderRows = 4;

                // Creates a PdfPTable with 2 columns to hold the header in the exported PDF.
                iTextSharp.text.pdf.PdfPTable headerTable = new iTextSharp.text.pdf.PdfPTable(1);

                // Creates a phrase to hold the application name at the left hand side of the header.
                Phrase phApplicationName = new Phrase(Environment.NewLine + WebConfigurationManager.AppSettings["Domain"] + " Report Name: User / Project Wise Document Upload (" + ddlOptions.SelectedItem.ToString() + ")", FontFactory.GetFont("Arial", ApplicationNameSize, iTextSharp.text.Font.BOLD));

                // Creates a PdfPCell which accepts a phrase as a parameter.
                PdfPCell clApplicationName = new PdfPCell(phApplicationName);
                // Sets the border of the cell to zero.
                clApplicationName.Border = PdfPCell.NO_BORDER;
                // Sets the Horizontal Alignment of the PdfPCell to left.
                clApplicationName.HorizontalAlignment = Element.ALIGN_CENTER;

                // Creates a phrase to show the current date at the right hand side of the header.
                //Phrase phDate = new Phrase(DateTime.Now.Date.ToString("dd/MM/yyyy"), FontFactory.GetFont("Arial", ApplicationNameSize, iTextSharp.text.Font.NORMAL));

                //// Creates a PdfPCell which accepts the date phrase as a parameter.
                //PdfPCell clDate = new PdfPCell(phDate);
                //// Sets the Horizontal Alignment of the PdfPCell to right.
                //clDate.HorizontalAlignment = Element.ALIGN_RIGHT;
                //// Sets the border of the cell to zero.
                //clDate.Border = PdfPCell.NO_BORDER;

                // Adds the cell which holds the application name to the headerTable.
                headerTable.AddCell(clApplicationName);
                // Adds the cell which holds the date to the headerTable.
                //  headerTable.AddCell(clDate);
                // Sets the border of the headerTable to zero.
                headerTable.DefaultCell.Border = PdfPCell.NO_BORDER;

                // Creates a PdfPCell that accepts the headerTable as a parameter and then adds that cell to the main PdfPTable.
                PdfPCell cellHeader = new PdfPCell(headerTable);
                cellHeader.Border = PdfPCell.NO_BORDER;
                // Sets the column span of the header cell to noOfColumns.
                cellHeader.Colspan = noOfColumns;
                // Adds the above header cell to the table.
                mainTable.AddCell(cellHeader);
                //
                int len = 100 - ProjectName.Length;
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(ProjectName);
                //sb.Append("".PadLeft(len, ' ').Replace(" ", " "));
                if (ddlProject.SelectedIndex == 1)
                {
                    len = 100 - fromdate.Length - 2;
                }
                else
                {
                    len = 100 - fromdate.Length;
                }
                //len = 100 - lblDocumentsNo.Text.Length;
                System.Text.StringBuilder sbdate = new System.Text.StringBuilder();
                sbdate.Append(lblDocumentsNo.Text);
                //sbdate.Append("".PadLeft(len-5, ' ').Replace(" ", " "));
                //
                System.Text.StringBuilder dateL = new System.Text.StringBuilder();
                dateL.Append(fromdate);
                //dateL.Append("".PadLeft(len+8, ' ').Replace(" ", " "));
                //len = 50 - Username.Length;
                //System.Text.StringBuilder userL = new System.Text.StringBuilder();
                //userL.Append(Username);
                //userL.Append("".PadLeft(len, ' ').Replace(" ", " "));

                //len = 50 - fromdate.Length;
                //System.Text.StringBuilder dateL = new System.Text.StringBuilder();
                //dateL.Append(fromdate);
                //dateL.Append("".PadLeft(len - 10, ' ').Replace(" ", " "));
                //

                // Creates a phrase which holds the file name.
                Phrase phHeader = new Phrase("Project Name : " + sb + " , User : " + Username  + Environment.NewLine  + sbdate  +" , Status : " + status + Environment.NewLine +   "Start Date : " + dateL + " , End Date : " + todate, FontFactory.GetFont("Arial", ReportNameSize, iTextSharp.text.Font.BOLD));
                PdfPCell clHeader = new PdfPCell(phHeader);
                clHeader.Colspan = noOfColumns;
                clHeader.Border = PdfPCell.NO_BORDER;
                clHeader.HorizontalAlignment = Element.ALIGN_LEFT;
                mainTable.AddCell(clHeader);



                // Creates a phrase for a new line.
                Phrase phSpace = new Phrase("\n");
                PdfPCell clSpace = new PdfPCell(phSpace);
                clSpace.Border = PdfPCell.NO_BORDER;
                clSpace.Colspan = noOfColumns;
                mainTable.AddCell(clSpace);

                // Sets the gridview column names as table headers.
                for (int i = 0; i < noOfColumns; i++)
                {
                    Phrase ph = null;

                    if (gdRp.AutoGenerateColumns)
                    {
                        ph = new Phrase(tbl.Columns[i].ColumnName.Replace("<br/>", Environment.NewLine), FontFactory.GetFont("Arial", HeaderTextSize, iTextSharp.text.Font.BOLD));
                    }
                    else
                    {
                        ph = new Phrase(gdRp.Columns[i].HeaderText.Replace("<br/>", Environment.NewLine), FontFactory.GetFont("Arial", HeaderTextSize, iTextSharp.text.Font.BOLD));
                    }
                    PdfPCell cl = new PdfPCell(ph);
                    if (i >= 0 && i <= 3)
                    {
                        cl.HorizontalAlignment = Element.ALIGN_LEFT;
                    }
                    else
                    {
                        cl.HorizontalAlignment = Element.ALIGN_CENTER;
                    }
                    mainTable.AddCell(cl);
                }

                // Reads the gridview rows and adds them to the mainTable
                for (int rowNo = 0; rowNo <= noOfRows; rowNo++)
                {
                    if (rowNo != noOfRows)
                    {
                        for (int columnNo = 0; columnNo < noOfColumns; columnNo++)
                        {
                            if (gdRp.AutoGenerateColumns)
                            {
                                string s = gdRp.Rows[rowNo].Cells[columnNo].Text.Trim();
                                Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                                PdfPCell cl = new PdfPCell(ph);
                                cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                mainTable.AddCell(cl);
                            }
                            else
                            {
                                if (columnNo == 3)
                                {
                                    if (gdRp.Columns[columnNo] is TemplateField)
                                    {
                                        DataBoundLiteralControl lc = gdRp.Rows[rowNo].Cells[columnNo].Controls[0] as DataBoundLiteralControl;
                                        string s = lc.Text.Trim();
                                        Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                                        PdfPCell cl = new PdfPCell(ph);
                                        cl.HorizontalAlignment = Element.ALIGN_LEFT;
                                        mainTable.AddCell(cl);

                                       
                                    }
                                    else
                                    {
                                        string s = gdRp.Rows[rowNo].Cells[columnNo].Text.Trim();
                                        s = s.Replace("&nbsp;", "");
                                        Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                                        PdfPCell cl = new PdfPCell(ph);
                                        cl.HorizontalAlignment = Element.ALIGN_LEFT;
                                        mainTable.AddCell(cl);
                                    }
                                }
                                else if (columnNo == 5)
                                {
                                    if (gdRp.Columns[columnNo] is TemplateField)
                                    {
                                        DataBoundLiteralControl lc = gdRp.Rows[rowNo].Cells[columnNo].Controls[0] as DataBoundLiteralControl;
                                        string s = lc.Text.Trim();
                                        Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                                        PdfPCell cl = new PdfPCell(ph);
                                        cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                        mainTable.AddCell(cl);
                                    }
                                    else
                                    {
                                        string s = gdRp.Rows[rowNo].Cells[columnNo].Text.Trim();
                                        s = s.Replace("&nbsp;", "");
                                        Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                                        PdfPCell cl = new PdfPCell(ph);
                                        cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                        mainTable.AddCell(cl);
                                    }
                                }
                                else
                                {
                                    if (gdRp.Columns[columnNo] is TemplateField)
                                    {
                                        DataBoundLiteralControl lc = gdRp.Rows[rowNo].Cells[columnNo].Controls[0] as DataBoundLiteralControl;
                                        string s = lc.Text.Trim();
                                        Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                                        PdfPCell cl = new PdfPCell(ph);
                                        if (columnNo >= 0 && columnNo <= 3)
                                        {
                                            cl.HorizontalAlignment = Element.ALIGN_LEFT;
                                        }
                                        else
                                        {
                                            cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                        }
                                        mainTable.AddCell(cl);
                                    }
                                    else
                                    {
                                        string s = gdRp.Rows[rowNo].Cells[columnNo].Text.Trim();
                                        s = s.Replace("&nbsp;", "");
                                        Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                                        PdfPCell cl = new PdfPCell(ph);
                                        if (columnNo >= 0 && columnNo <= 3)
                                        {
                                            cl.HorizontalAlignment = Element.ALIGN_LEFT;
                                        }
                                        else
                                        {
                                            cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                        }
                                        mainTable.AddCell(cl);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (type == 1)
                        {
                            for (int columnNo = 0; columnNo < noOfColumns; columnNo++)
                            {
                                string s = "Total";
                                if (columnNo == 1)
                                {
                                    Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.BOLD));
                                    PdfPCell cl = new PdfPCell(ph);
                                    cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                    mainTable.AddCell(cl);
                                }
                                else if (columnNo == 2)
                                {
                                    s = ViewState["grandtotalcount"].ToString();// Session["AwardedValue"].ToString();
                                    Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.BOLD));
                                    PdfPCell cl = new PdfPCell(ph);
                                    cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                    mainTable.AddCell(cl);
                                }
                                else if (columnNo == 3)
                                {
                                    s = ViewState["grandtotaldwnldcount"].ToString();// Session["ExpenditureOverallTotal"].ToString();
                                    Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.BOLD));
                                    PdfPCell cl = new PdfPCell(ph);
                                    cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                    mainTable.AddCell(cl);
                                }
                                else if (columnNo == 4)
                                {
                                    s = ViewState["grandtotalviewcount"].ToString();// Session["TargetedOverallTotal"].ToString() + "%";
                                    Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.BOLD));
                                    PdfPCell cl = new PdfPCell(ph);
                                    cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                    mainTable.AddCell(cl);
                                }
                                else if (columnNo == 5)
                                {
                                    s = ViewState["GrandTotalDocumentLinkSent"].ToString();// Session["AchievedOverallTotal"].ToString() + "%";
                                    Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.BOLD));
                                    PdfPCell cl = new PdfPCell(ph);
                                    cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                    mainTable.AddCell(cl);
                                }
                                else
                                {
                                    Phrase ph = new Phrase("", FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.BOLD));
                                    PdfPCell cl = new PdfPCell(ph);
                                    cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                    mainTable.AddCell(cl);
                                }
                            }
                        }

                    }

                    // Tells the mainTable to complete the row even if any cell is left incomplete.
                    mainTable.CompleteRow();
                }

                // Gets the instance of the document created and writes it to the output stream of the Response object.
                PdfWriter.GetInstance(document, Response.OutputStream);

                // Creates a footer for the PDF document.
                //HeaderFooter pdfFooter = new HeaderFooter(new Phrase(), true);
                //pdfFooter.Alignment = Element.ALIGN_CENTER;
                //pdfFooter.Border = iTextSharp.text.Rectangle.NO_BORDER;
                //pdfFooter.Bottom = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                //// Sets the document footer to pdfFooter.
                //document.Footer = pdfFooter;
                iTextSharp.text.Font foot = new iTextSharp.text.Font();
                foot.Size = 10;

                len = 174;
                System.Text.StringBuilder time = new System.Text.StringBuilder();
                time.Append(DateTime.Now.ToString("hh:mm tt"));
                time.Append("".PadLeft(len, ' ').Replace(" ", " "));
                HeaderFooter pdfFooter = new HeaderFooter(new Phrase("Date : " + DateTime.Now.ToString("dd/MM/yyyy") + " Time " + time + "  Page: ", foot), new Phrase(Environment.NewLine + " ", foot));
                pdfFooter.Alignment = Element.ALIGN_CENTER;
                //pdfFooter.Border = iTextSharp.text.Rectangle.NO_BORDER;

                //// Sets the document footer to pdfFooter.
                document.Footer = pdfFooter;
                //document.Footer = new HeaderFooter(new Phrase("        Date : " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + "   Page: ", foot), new Phrase(Environment.NewLine + " ", foot));


                // Opens the document.
                document.Open();
                // Adds the mainTable to the document.
                document.Add(mainTable);
                // Closes the document.
                document.Close();
                

                Response.ContentType = "application/pdf";
                //Response.AddHeader("content-disposition", "attachment; filename= SampleExport.pdf");
                Response.AddHeader("content-disposition", "attachment;filename=Report_UserDocumetUpload_AllDetails_" + DateTime.Now.Ticks + ".pdf");
                Response.End();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string[] getDates()
        {
            string[] Dates = new string[2];
            if (ddlProject.SelectedIndex != 0 && ddlProject.SelectedItem.ToString() != "General Documents" && ddlProject.SelectedItem.ToString() != "All Projects")
            {
                DataSet ds = getdt.GetProject_by_ProjectUID(new Guid(ddlProject.SelectedValue));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    Dates[0] = DateTime.Parse(ds.Tables[0].Rows[0]["StartDate"].ToString()).ToString("dd/MM/yyyy");
                    Dates[1] = DateTime.Now.ToString("dd/MM/yyyy");
                }
            }
            else if (ddlProject.SelectedItem.ToString() == "All Projects")
            {
                List<DateTime> lstdt = new List<DateTime>();
                DataSet ds = new DataSet();
                for (int i = 2; i < ddlProject.Items.Count; i++)
                {
                    if (ddlProject.Items[i].Text != "General Documents")
                    {
                        ds.Clear();
                        ds = getdt.GetProject_by_ProjectUID(new Guid(ddlProject.Items[i].Value));
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            lstdt.Add(DateTime.Parse(ds.Tables[0].Rows[0]["StartDate"].ToString()));
                        }
                    }
                }
                Dates[1] = DateTime.Now.ToString("dd/MM/yyyy");
                Dates[0] = lstdt.Min(p => p).ToString("dd/MM/yyyy");
            }
            return Dates;
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            if (ddlOptions.SelectedIndex == 1) // Summary details
            {
                if (RDBReportView.SelectedValue == "User" && ddlusers.SelectedValue == "All Users")
                {
                    PrinttoPDF(grdDocumenthistoryAll, 1);
                }
                else if (RDBReportView.SelectedValue == "Project" && ddlusers.SelectedValue == "Select User")
                {
                    PrinttoPDF(GrdProjectwiseSummary, 2);
                }
                else
                {
                    PrinttoPDF(GrdDcumentHistory, 2);
                }
            }
            else if (ddlOptions.SelectedIndex == 2) // All details
            {
                if (ddlPages.SelectedValue == "0")
                {
                    PrinttoPDFDetails(grdDocumentsPDF, 2);

                }
                else //current page
                {
                    LoadDocuments();
                    PrinttoPDFDetails(grdDocumentsPDF, 2);
                }
                grdDocumentsPDF.Visible = false;
            }
        }

        private void PrinttoPDF(GridView gd, int type)
        {
            try
            {

                DateTime CDate1 = DateTime.Now;
                GridView gdRp = new GridView();
                gdRp = gd;

                int noOfColumns = 0, noOfRows = 0;
                DataTable tbl = null;
                string fromdate = "";
                string todate = "";
                if (dtFromDate.Text == "")
                {
                    fromdate = getDates()[0];
                }
                else
                {
                    fromdate = dtFromDate.Text;
                }
                if (dtToDate.Text == "")
                {
                    todate = getDates()[1];
                }
                else
                {
                    todate = dtToDate.Text;
                }
                if (gdRp.AutoGenerateColumns)
                {
                    tbl = gdRp.DataSource as DataTable; // Gets the DataSource of the GridView Control.
                    noOfColumns = tbl.Columns.Count;
                    noOfRows = tbl.Rows.Count;
                }
                else
                {
                    noOfColumns = gdRp.Columns.Count;
                    noOfRows = gdRp.Rows.Count;
                }

                float HeaderTextSize = 9;
                float ReportNameSize = 9;
                float ReportTextSize = 9;
                float ApplicationNameSize = 13;
                string ProjectName = ddlProject.SelectedItem.ToString();
                string Username = "";
                string Heading = "";
                if (RDBReportView.SelectedValue == "User")
                {
                    Username = ddlusers.SelectedItem.ToString();
                    //if (Username.Length > 30)
                    //{
                    //    Username = Username.Substring(0, 29) + "..";
                    //}
                    Heading = "User Wise Document Upload";
                }
                else
                {
                    Heading = "Project Wise Document Upload";
                }
               
                    
                //if (ProjectName.Length > 30)
                //{
                //    ProjectName = ProjectName.Substring(0, 29) + "..";
                //}
                

                //float HeaderTextSize = 15;
                //float ReportNameSize = 18;
                //float ReportTextSize = 15;
                //float ApplicationNameSize = 20;


                // Creates a PDF document

                Document document = null;
                //if (LandScape == true)
                //{
                // Sets the document to A4 size and rotates it so that the     orientation of the page is Landscape.
                document = new Document(PageSize.A4.Rotate(), 0, 0, 0, 0);
                //}
                //else
                //{
                //    document = new Document(PageSize.A4, 0, 0, 15, 5);
                //}

                // Creates a PdfPTable with column count of the table equal to no of columns of the gridview or gridview datasource.
                iTextSharp.text.pdf.PdfPTable mainTable = new iTextSharp.text.pdf.PdfPTable(noOfColumns);

                // Sets the first 4 rows of the table as the header rows which will be repeated in all the pages.
                mainTable.HeaderRows = 4;

                // Creates a PdfPTable with 2 columns to hold the header in the exported PDF.
                iTextSharp.text.pdf.PdfPTable headerTable = new iTextSharp.text.pdf.PdfPTable(1);

                // Creates a phrase to hold the application name at the left hand side of the header.
                Phrase phApplicationName = new Phrase(Environment.NewLine + WebConfigurationManager.AppSettings["Domain"] + " Report Name: " + Heading + " (" + ddlOptions.SelectedItem.ToString() + ")", FontFactory.GetFont("Arial", ApplicationNameSize, iTextSharp.text.Font.BOLD));

                // Creates a PdfPCell which accepts a phrase as a parameter.
                PdfPCell clApplicationName = new PdfPCell(phApplicationName);
                // Sets the border of the cell to zero.
                clApplicationName.Border = PdfPCell.NO_BORDER;
                // Sets the Horizontal Alignment of the PdfPCell to left.
                clApplicationName.HorizontalAlignment = Element.ALIGN_CENTER;

                // Creates a phrase to show the current date at the right hand side of the header.
                //Phrase phDate = new Phrase(DateTime.Now.Date.ToString("dd/MM/yyyy"), FontFactory.GetFont("Arial", ApplicationNameSize, iTextSharp.text.Font.NORMAL));

                //// Creates a PdfPCell which accepts the date phrase as a parameter.
                //PdfPCell clDate = new PdfPCell(phDate);
                //// Sets the Horizontal Alignment of the PdfPCell to right.
                //clDate.HorizontalAlignment = Element.ALIGN_RIGHT;
                //// Sets the border of the cell to zero.
                //clDate.Border = PdfPCell.NO_BORDER;

                // Adds the cell which holds the application name to the headerTable.
                headerTable.AddCell(clApplicationName);
                // Adds the cell which holds the date to the headerTable.
                //  headerTable.AddCell(clDate);
                // Sets the border of the headerTable to zero.
                headerTable.DefaultCell.Border = PdfPCell.NO_BORDER;

                // Creates a PdfPCell that accepts the headerTable as a parameter and then adds that cell to the main PdfPTable.
                PdfPCell cellHeader = new PdfPCell(headerTable);
                cellHeader.Border = PdfPCell.NO_BORDER;
                // Sets the column span of the header cell to noOfColumns.
                cellHeader.Colspan = noOfColumns;
                // Adds the above header cell to the table.
                mainTable.AddCell(cellHeader);

                // Creates a phrase which holds the file name.
                int len = 100 - ProjectName.Length;
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(ProjectName);
              //  sb.Append("".PadLeft(len, ' ').Replace(" ", " "));

                if (ddlProject.SelectedIndex == 1)
                {
                    len = 100 - fromdate.Length - 2;
                }
                else
                {
                    len = 100 - fromdate.Length;
                }
                System.Text.StringBuilder sbdate = new System.Text.StringBuilder();
                sbdate.Append(fromdate);
               // sbdate.Append("".PadLeft(len + 9, ' ').Replace(" ", " "));

                //
                Phrase phHeader = new Phrase();
                if (RDBReportView.SelectedValue == "User")
                {
                    //phHeader = new Phrase("Project Name :" + sb + ", User :" + Username + Environment.NewLine + lblDocumentsNo.Text + Environment.NewLine + "Start Date :" + fromdate + ", End Date : " + todate, FontFactory.GetFont("Arial", ReportNameSize, iTextSharp.text.Font.BOLD));
                    phHeader = new Phrase("Project Name : " + sb + " , User : " + Username + Environment.NewLine + "Start Date : " + sbdate + " , End Date : " + todate, FontFactory.GetFont("Arial", ReportNameSize, iTextSharp.text.Font.BOLD));
                }
                else
                {
                    //phHeader = new Phrase("Project Name :" + ProjectName  + Environment.NewLine + lblDocumentsNo.Text + Environment.NewLine + "Start Date :" + fromdate + ", End Date : " + todate, FontFactory.GetFont("Arial", ReportNameSize, iTextSharp.text.Font.BOLD));
                    phHeader = new Phrase("Project Name : " + ProjectName + Environment.NewLine + "Start Date : " + sbdate + " , End Date : " + todate, FontFactory.GetFont("Arial", ReportNameSize, iTextSharp.text.Font.BOLD));
                }

                PdfPCell clHeader = new PdfPCell(phHeader);
                clHeader.Colspan = noOfColumns;
                clHeader.Border = PdfPCell.NO_BORDER;
                clHeader.HorizontalAlignment = Element.ALIGN_LEFT;
                mainTable.AddCell(clHeader);



                // Creates a phrase for a new line.
                Phrase phSpace = new Phrase("\n");
                PdfPCell clSpace = new PdfPCell(phSpace);
                clSpace.Border = PdfPCell.NO_BORDER;
                clSpace.Colspan = noOfColumns;
                mainTable.AddCell(clSpace);

                // Sets the gridview column names as table headers.
                for (int i = 0; i < noOfColumns; i++)
                {
                    Phrase ph = null;

                    if (gdRp.AutoGenerateColumns)
                    {
                        ph = new Phrase(tbl.Columns[i].ColumnName.Replace("<br/>", Environment.NewLine), FontFactory.GetFont("Arial", HeaderTextSize, iTextSharp.text.Font.BOLD));
                    }
                    else
                    {
                        ph = new Phrase(gdRp.Columns[i].HeaderText.Replace("<br/>", Environment.NewLine), FontFactory.GetFont("Arial", HeaderTextSize, iTextSharp.text.Font.BOLD));
                    }
                    PdfPCell cl = new PdfPCell(ph);
                    if (i == 0)
                    {
                        cl.HorizontalAlignment = Element.ALIGN_LEFT;
                    }
                    else if (i == 1)
                    {
                        if (RDBReportView.SelectedValue == "User")
                        {
                            cl.HorizontalAlignment = Element.ALIGN_LEFT;
                        }
                        else
                        {
                            cl.HorizontalAlignment = Element.ALIGN_CENTER;
                        }
                    }
                    else
                    {
                        cl.HorizontalAlignment = Element.ALIGN_CENTER;
                    }
                    mainTable.AddCell(cl);
                }

                // Reads the gridview rows and adds them to the mainTable
                for (int rowNo = 0; rowNo <= noOfRows; rowNo++)
                {
                    if (rowNo != noOfRows)
                    {
                        for (int columnNo = 0; columnNo < noOfColumns; columnNo++)
                        {
                            if (gdRp.AutoGenerateColumns)
                            {
                                string s = gdRp.Rows[rowNo].Cells[columnNo].Text.Trim();
                                Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                                PdfPCell cl = new PdfPCell(ph);
                                cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                mainTable.AddCell(cl);
                            }
                            else
                            {
                                if (columnNo == 3)
                                {
                                    if (gdRp.Columns[columnNo] is TemplateField)
                                    {
                                        DataBoundLiteralControl lc = gdRp.Rows[rowNo].Cells[columnNo].Controls[0] as DataBoundLiteralControl;
                                        string s = lc.Text.Trim();
                                        Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                                        PdfPCell cl = new PdfPCell(ph);
                                        cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                        mainTable.AddCell(cl);
                                    }
                                    else
                                    {
                                        string s = gdRp.Rows[rowNo].Cells[columnNo].Text.Trim();
                                        s = s.Replace("&nbsp;", "");
                                        Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                                        PdfPCell cl = new PdfPCell(ph);
                                        cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                        mainTable.AddCell(cl);
                                    }
                                }
                                else if (columnNo == 5)
                                {
                                    if (gdRp.Columns[columnNo] is TemplateField)
                                    {
                                        DataBoundLiteralControl lc = gdRp.Rows[rowNo].Cells[columnNo].Controls[0] as DataBoundLiteralControl;
                                        string s = lc.Text.Trim();
                                        Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                                        PdfPCell cl = new PdfPCell(ph);
                                        cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                        mainTable.AddCell(cl);
                                    }
                                    else
                                    {
                                        string s = gdRp.Rows[rowNo].Cells[columnNo].Text.Trim();
                                        s = s.Replace("&nbsp;", "");
                                        Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                                        PdfPCell cl = new PdfPCell(ph);
                                        cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                        mainTable.AddCell(cl);
                                    }
                                }
                                else
                                {
                                    if (gdRp.Columns[columnNo] is TemplateField)
                                    {
                                        DataBoundLiteralControl lc = gdRp.Rows[rowNo].Cells[columnNo].Controls[0] as DataBoundLiteralControl;
                                        string s = lc.Text.Trim();
                                        Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                                        PdfPCell cl = new PdfPCell(ph);
                                        cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                        mainTable.AddCell(cl);
                                    }
                                    else
                                    {
                                        string s = gdRp.Rows[rowNo].Cells[columnNo].Text.Trim();
                                        s = s.Replace("&nbsp;", "");
                                        if (s.Contains("Total"))
                                        {
                                            Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.BOLD));
                                            PdfPCell cl = new PdfPCell(ph);
                                            cl.HorizontalAlignment = Element.ALIGN_LEFT;
                                            mainTable.AddCell(cl);
                                        }
                                        else if (columnNo == 0)
                                        {

                                            Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                                            PdfPCell cl = new PdfPCell(ph);
                                            cl.HorizontalAlignment = Element.ALIGN_LEFT;
                                            mainTable.AddCell(cl);
                                        }
                                        else if (columnNo == 1)
                                        {
                                            if (RDBReportView.SelectedValue == "User")
                                            {
                                                Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                                                PdfPCell cl = new PdfPCell(ph);
                                                cl.HorizontalAlignment = Element.ALIGN_LEFT;
                                                mainTable.AddCell(cl);
                                            }
                                            else
                                            {
                                                Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                                                PdfPCell cl = new PdfPCell(ph);
                                                cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                                mainTable.AddCell(cl);
                                            }
                                        }
                                        else
                                        {
                                            Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                                            PdfPCell cl = new PdfPCell(ph);
                                            cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                            mainTable.AddCell(cl);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (type == 1)
                        {
                            for (int columnNo = 0; columnNo < noOfColumns; columnNo++)
                            {
                                string s = "Total";

                                if (columnNo == 1)
                                {
                                    Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.BOLD));
                                    PdfPCell cl = new PdfPCell(ph);
                                    cl.HorizontalAlignment = Element.ALIGN_LEFT;
                                    mainTable.AddCell(cl);
                                }
                                else if (columnNo == 2)
                                {
                                    s = ViewState["grandtotalcount"].ToString();// Session["AwardedValue"].ToString();
                                    Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.BOLD));
                                    PdfPCell cl = new PdfPCell(ph);
                                    cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                    mainTable.AddCell(cl);
                                }
                                else if (columnNo == 3)
                                {
                                    s = ViewState["grandtotaldwnldcount"].ToString();// Session["ExpenditureOverallTotal"].ToString();
                                    Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.BOLD));
                                    PdfPCell cl = new PdfPCell(ph);
                                    cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                    mainTable.AddCell(cl);
                                }
                                else if (columnNo == 4)
                                {
                                    s = ViewState["grandtotalviewcount"].ToString();// Session["TargetedOverallTotal"].ToString() + "%";
                                    Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.BOLD));
                                    PdfPCell cl = new PdfPCell(ph);
                                    cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                    mainTable.AddCell(cl);
                                }
                                else if (columnNo == 5)
                                {
                                    s = ViewState["GrandTotalDocumentLinkSent"].ToString();// Session["AchievedOverallTotal"].ToString() + "%";
                                    Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.BOLD));
                                    PdfPCell cl = new PdfPCell(ph);
                                    cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                    mainTable.AddCell(cl);
                                }
                                else
                                {
                                    Phrase ph = new Phrase("", FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.BOLD));
                                    PdfPCell cl = new PdfPCell(ph);
                                    cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                    mainTable.AddCell(cl);
                                }


                            }
                        }

                    }

                    // Tells the mainTable to complete the row even if any cell is left incomplete.
                    mainTable.CompleteRow();
                }

                // Gets the instance of the document created and writes it to the output stream of the Response object.
                //PdfWriter.GetInstance(document, Response.OutputStream);
                PdfWriter.GetInstance(document, new FileStream(Server.MapPath("~") + "/RegExcel/printpdf.pdf", FileMode.Create));
              
                // Creates a footer for the PDF document.
                iTextSharp.text.Font foot = new iTextSharp.text.Font();
                foot.Size = 10;

                len = 174;
                System.Text.StringBuilder time = new System.Text.StringBuilder();
                time.Append(DateTime.Now.ToString("hh:mm tt"));
                time.Append("".PadLeft(len, ' ').Replace(" ", " "));
                HeaderFooter pdfFooter = new HeaderFooter(new Phrase("Date : " + DateTime.Now.ToString("dd/MM/yyyy") + " Time " + time + "  Page: ", foot), new Phrase(Environment.NewLine + " ", foot));
                pdfFooter.Alignment = Element.ALIGN_CENTER;
                //pdfFooter.Border = iTextSharp.text.Rectangle.NO_BORDER;

                //// Sets the document footer to pdfFooter.
                document.Footer = pdfFooter;

                // document.Footer = new HeaderFooter(new Phrase("        Date : " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + "   Page: ", foot), new Phrase(Environment.NewLine + " ", foot));


                // Opens the document.
                document.Open();
                // Adds the mainTable to the document.
                document.Add(mainTable);
                // Closes the document.
                document.Close();
                // Response.Redirect("~/PDFPRint.aspx");
                Session["Print"] = true;
                Response.Write("<script>window.open ('/PDFPRint.aspx','_blank');</script>");
                //Response.Buffer = true;
                //Response.ContentType = "application/pdf";
                //// Response.Cache.SetCacheability(HttpCacheability.NoCache);
                ////Response.AddHeader("content-disposition", "attachment; filename= SampleExport.pdf");
                //Response.AddHeader("content-disposition", "attachment;filename=Report_UserDocumetUpload_SummaryDetails_" + DateTime.Now.Ticks + ".pdf");
                ////Response.AddHeader("content-disposition", "inline;filename=Report_UserDocumetUpload_SummaryDetails_" + DateTime.Now.Ticks + ".pdf");
                //// Response.AppendHeader("Content-Disposition", "attachment; filename=FileName.pdf");
                //// Response.TransmitFile(Server.MapPath("~/RegExcel/Sample.pdf"));
                //Response.End();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void PrinttoPDFDetails(GridView gd, int type)
        {
            try
            {

                DateTime CDate1 = DateTime.Now;
                GridView gdRp = new GridView();
                gdRp = gd;

                int noOfColumns = 0, noOfRows = 0;
                DataTable tbl = null;
                string fromdate = "";
                string todate = "";
                if (dtFromDate.Text == "")
                {
                    fromdate = getDates()[0];
                }
                else
                {
                    fromdate = dtFromDate.Text;
                }
                if (dtToDate.Text == "")
                {
                    todate = getDates()[1];
                }
                else
                {
                    todate = dtToDate.Text;
                }
                if (gdRp.AutoGenerateColumns)
                {
                    tbl = gdRp.DataSource as DataTable; // Gets the DataSource of the GridView Control.
                    noOfColumns = tbl.Columns.Count;
                    noOfRows = tbl.Rows.Count;
                }
                else
                {
                    noOfColumns = gdRp.Columns.Count;
                    noOfRows = gdRp.Rows.Count;
                }

                float HeaderTextSize = 9;
                float ReportNameSize = 9;
                float ReportTextSize = 9;
                float ApplicationNameSize = 13;
                string ProjectName = ddlProject.SelectedItem.ToString();
                string Username = ddlusers.SelectedItem.ToString();
                //if (ProjectName.Length > 30)
                //{
                //    ProjectName = ProjectName.Substring(0, 29) + "..";
                //}
                //if (Username.Length > 30)
                //{
                //    Username = Username.Substring(0, 29) + "..";
                //}
                //float HeaderTextSize = 15;
                //float ReportNameSize = 18;
                //float ReportTextSize = 15;
                //float ApplicationNameSize = 20;
                string status = "";
                string page = (gd.PageIndex + 1).ToString();
                if (DDLStatus.SelectedIndex == 0)
                {
                    status = "All";

                }
                else
                {
                    status = DDLStatus.SelectedItem.Text;
                }
                if (ddlPages.SelectedIndex == 0)
                {
                    page = "All";
                }

                // Creates a PDF document

                Document document = null;
                //if (LandScape == true)
                //{
                // Sets the document to A4 size and rotates it so that the     orientation of the page is Landscape.
                document = new Document(PageSize.A4.Rotate(), 0, 0, 0, 0);
                //}
                //else
                //{
                //    document = new Document(PageSize.A4, 0, 0, 15, 5);
                //}

                // Creates a PdfPTable with column count of the table equal to no of columns of the gridview or gridview datasource.
                iTextSharp.text.pdf.PdfPTable mainTable = new iTextSharp.text.pdf.PdfPTable(noOfColumns);

                // Sets the first 4 rows of the table as the header rows which will be repeated in all the pages.
                mainTable.HeaderRows = 4;

                // Creates a PdfPTable with 2 columns to hold the header in the exported PDF.
                iTextSharp.text.pdf.PdfPTable headerTable = new iTextSharp.text.pdf.PdfPTable(1);

                // Creates a phrase to hold the application name at the left hand side of the header.
                Phrase phApplicationName = new Phrase(Environment.NewLine + WebConfigurationManager.AppSettings["Domain"] + " Report Name: User / Project Wise Document Upload (" + ddlOptions.SelectedItem.ToString() + ")", FontFactory.GetFont("Arial", ApplicationNameSize, iTextSharp.text.Font.BOLD));

                // Creates a PdfPCell which accepts a phrase as a parameter.
                PdfPCell clApplicationName = new PdfPCell(phApplicationName);
                // Sets the border of the cell to zero.
                clApplicationName.Border = PdfPCell.NO_BORDER;
                // Sets the Horizontal Alignment of the PdfPCell to left.
                clApplicationName.HorizontalAlignment = Element.ALIGN_CENTER;

                // Creates a phrase to show the current date at the right hand side of the header.
                //Phrase phDate = new Phrase(DateTime.Now.Date.ToString("dd/MM/yyyy"), FontFactory.GetFont("Arial", ApplicationNameSize, iTextSharp.text.Font.NORMAL));

                //// Creates a PdfPCell which accepts the date phrase as a parameter.
                //PdfPCell clDate = new PdfPCell(phDate);
                //// Sets the Horizontal Alignment of the PdfPCell to right.
                //clDate.HorizontalAlignment = Element.ALIGN_RIGHT;
                //// Sets the border of the cell to zero.
                //clDate.Border = PdfPCell.NO_BORDER;

                // Adds the cell which holds the application name to the headerTable.
                headerTable.AddCell(clApplicationName);
                // Adds the cell which holds the date to the headerTable.
                //  headerTable.AddCell(clDate);
                // Sets the border of the headerTable to zero.
                headerTable.DefaultCell.Border = PdfPCell.NO_BORDER;

                // Creates a PdfPCell that accepts the headerTable as a parameter and then adds that cell to the main PdfPTable.
                PdfPCell cellHeader = new PdfPCell(headerTable);
                cellHeader.Border = PdfPCell.NO_BORDER;
                // Sets the column span of the header cell to noOfColumns.
                cellHeader.Colspan = noOfColumns;
                // Adds the above header cell to the table.
                mainTable.AddCell(cellHeader);

                // Creates a phrase which holds the file name.
                int len = 100 - ProjectName.Length;
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(ProjectName);
               // sb.Append("".PadLeft(len, ' ').Replace(" ", " "));
                if (ddlProject.SelectedIndex == 1)
                {
                    len = 100 - fromdate.Length - 2;
                }
                else
                {
                    len = 100 - fromdate.Length;
                }
                //len = 100 - lblDocumentsNo.Text.Length;
                //len = 100 - lblDocumentsNo.Text.Length;
                System.Text.StringBuilder sbdate = new System.Text.StringBuilder();
                sbdate.Append(lblDocumentsNo.Text);
              //  sbdate.Append("".PadLeft(len - 5, ' ').Replace(" ", " "));
                //
                System.Text.StringBuilder dateL = new System.Text.StringBuilder();
                dateL.Append(fromdate);
                //  dateL.Append("".PadLeft(len + 8, ' ').Replace(" ", " "));
                //len = 50 - Username.Length;
                //System.Text.StringBuilder userL = new System.Text.StringBuilder();
                //userL.Append(Username);
                //userL.Append("".PadLeft(len, ' ').Replace(" ", " "));

                //len = 50 - fromdate.Length;
                //System.Text.StringBuilder dateL = new System.Text.StringBuilder();
                //dateL.Append(fromdate);
                //dateL.Append("".PadLeft(len - 10, ' ').Replace(" ", " "));
                //

                // Creates a phrase which holds the file name.
                Phrase phHeader = new Phrase("Project Name : " + sb + " , User : " + Username + Environment.NewLine + sbdate + " , Status : " + status + Environment.NewLine + "Start Date : " + dateL + " , End Date : " + todate, FontFactory.GetFont("Arial", ReportNameSize, iTextSharp.text.Font.BOLD));
                PdfPCell clHeader = new PdfPCell(phHeader);
                clHeader.Colspan = noOfColumns;
                clHeader.Border = PdfPCell.NO_BORDER;
                clHeader.HorizontalAlignment = Element.ALIGN_LEFT;
                mainTable.AddCell(clHeader);



                // Creates a phrase for a new line.
                Phrase phSpace = new Phrase("\n");
                PdfPCell clSpace = new PdfPCell(phSpace);
                clSpace.Border = PdfPCell.NO_BORDER;
                clSpace.Colspan = noOfColumns;
                mainTable.AddCell(clSpace);

                // Sets the gridview column names as table headers.
                for (int i = 0; i < noOfColumns; i++)
                {
                    Phrase ph = null;

                    if (gdRp.AutoGenerateColumns)
                    {
                        ph = new Phrase(tbl.Columns[i].ColumnName.Replace("<br/>", Environment.NewLine), FontFactory.GetFont("Arial", HeaderTextSize, iTextSharp.text.Font.BOLD));
                    }
                    else
                    {
                        ph = new Phrase(gdRp.Columns[i].HeaderText.Replace("<br/>", Environment.NewLine), FontFactory.GetFont("Arial", HeaderTextSize, iTextSharp.text.Font.BOLD));
                    }
                    PdfPCell cl = new PdfPCell(ph);
                    cl.HorizontalAlignment = Element.ALIGN_CENTER;
                    mainTable.AddCell(cl);
                }

                // Reads the gridview rows and adds them to the mainTable
                for (int rowNo = 0; rowNo <= noOfRows; rowNo++)
                {
                    if (rowNo != noOfRows)
                    {
                        for (int columnNo = 0; columnNo < noOfColumns; columnNo++)
                        {
                            if (gdRp.AutoGenerateColumns)
                            {
                                string s = gdRp.Rows[rowNo].Cells[columnNo].Text.Trim();
                                Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                                PdfPCell cl = new PdfPCell(ph);
                                cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                mainTable.AddCell(cl);
                            }
                            else
                            {
                                if (columnNo == 3)
                                {
                                    if (gdRp.Columns[columnNo] is TemplateField)
                                    {
                                        DataBoundLiteralControl lc = gdRp.Rows[rowNo].Cells[columnNo].Controls[0] as DataBoundLiteralControl;
                                        string s = lc.Text.Trim();
                                        Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                                        PdfPCell cl = new PdfPCell(ph);
                                        cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                        mainTable.AddCell(cl);


                                    }
                                    else
                                    {
                                        string s = gdRp.Rows[rowNo].Cells[columnNo].Text.Trim();
                                        s = s.Replace("&nbsp;", "");
                                        Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                                        PdfPCell cl = new PdfPCell(ph);
                                        cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                        mainTable.AddCell(cl);
                                    }
                                }
                                else if (columnNo == 5)
                                {
                                    if (gdRp.Columns[columnNo] is TemplateField)
                                    {
                                        DataBoundLiteralControl lc = gdRp.Rows[rowNo].Cells[columnNo].Controls[0] as DataBoundLiteralControl;
                                        string s = lc.Text.Trim();
                                        Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                                        PdfPCell cl = new PdfPCell(ph);
                                        cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                        mainTable.AddCell(cl);
                                    }
                                    else
                                    {
                                        string s = gdRp.Rows[rowNo].Cells[columnNo].Text.Trim();
                                        s = s.Replace("&nbsp;", "");
                                        Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                                        PdfPCell cl = new PdfPCell(ph);
                                        cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                        mainTable.AddCell(cl);
                                    }
                                }
                                else
                                {
                                    if (gdRp.Columns[columnNo] is TemplateField)
                                    {
                                        DataBoundLiteralControl lc = gdRp.Rows[rowNo].Cells[columnNo].Controls[0] as DataBoundLiteralControl;
                                        string s = lc.Text.Trim();
                                        Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                                        PdfPCell cl = new PdfPCell(ph);
                                        if (columnNo >= 0 && columnNo <= 3)
                                        {
                                            cl.HorizontalAlignment = Element.ALIGN_LEFT;
                                        }
                                        else
                                        {
                                            cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                        }
                                        mainTable.AddCell(cl);
                                    }
                                    else
                                    {
                                        string s = gdRp.Rows[rowNo].Cells[columnNo].Text.Trim();
                                        s = s.Replace("&nbsp;", "");
                                        Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                                        PdfPCell cl = new PdfPCell(ph);
                                        if (columnNo >= 0 && columnNo <= 3)
                                        {
                                            cl.HorizontalAlignment = Element.ALIGN_LEFT;
                                        }
                                        else
                                        {
                                            cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                        }
                                        mainTable.AddCell(cl);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (type == 1)
                        {
                            for (int columnNo = 0; columnNo < noOfColumns; columnNo++)
                            {
                                string s = "Total";
                                if (columnNo == 1)
                                {
                                    Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.BOLD));
                                    PdfPCell cl = new PdfPCell(ph);
                                    cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                    mainTable.AddCell(cl);
                                }
                                else if (columnNo == 2)
                                {
                                    s = ViewState["grandtotalcount"].ToString();// Session["AwardedValue"].ToString();
                                    Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.BOLD));
                                    PdfPCell cl = new PdfPCell(ph);
                                    cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                    mainTable.AddCell(cl);
                                }
                                else if (columnNo == 3)
                                {
                                    s = ViewState["grandtotaldwnldcount"].ToString();// Session["ExpenditureOverallTotal"].ToString();
                                    Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.BOLD));
                                    PdfPCell cl = new PdfPCell(ph);
                                    cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                    mainTable.AddCell(cl);
                                }
                                else if (columnNo == 4)
                                {
                                    s = ViewState["grandtotalviewcount"].ToString();// Session["TargetedOverallTotal"].ToString() + "%";
                                    Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.BOLD));
                                    PdfPCell cl = new PdfPCell(ph);
                                    cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                    mainTable.AddCell(cl);
                                }
                                else if (columnNo == 5)
                                {
                                    s = ViewState["GrandTotalDocumentLinkSent"].ToString();// Session["AchievedOverallTotal"].ToString() + "%";
                                    Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.BOLD));
                                    PdfPCell cl = new PdfPCell(ph);
                                    cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                    mainTable.AddCell(cl);
                                }
                                else
                                {
                                    Phrase ph = new Phrase("", FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.BOLD));
                                    PdfPCell cl = new PdfPCell(ph);
                                    cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                    mainTable.AddCell(cl);
                                }
                            }
                        }

                    }

                    // Tells the mainTable to complete the row even if any cell is left incomplete.
                    mainTable.CompleteRow();
                }

                // Gets the instance of the document created and writes it to the output stream of the Response object.
                //PdfWriter.GetInstance(document, Response.OutputStream);
                PdfWriter.GetInstance(document, new FileStream(Server.MapPath("~") + "/RegExcel/printpdf.pdf", FileMode.Create));

                // Creates a footer for the PDF document.
                //HeaderFooter pdfFooter = new HeaderFooter(new Phrase(), true);
                //pdfFooter.Alignment = Element.ALIGN_CENTER;
                //pdfFooter.Border = iTextSharp.text.Rectangle.NO_BORDER;
                //pdfFooter.Bottom = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                //// Sets the document footer to pdfFooter.
                //document.Footer = pdfFooter;
                iTextSharp.text.Font foot = new iTextSharp.text.Font();
                foot.Size = 10;

                len = 174;
                System.Text.StringBuilder time = new System.Text.StringBuilder();
                time.Append(DateTime.Now.ToString("hh:mm tt"));
                time.Append("".PadLeft(len, ' ').Replace(" ", " "));
                HeaderFooter pdfFooter = new HeaderFooter(new Phrase("Date : " + DateTime.Now.ToString("dd/MM/yyyy") + " Time " + time + "  Page: ", foot), new Phrase(Environment.NewLine + " ", foot));
                pdfFooter.Alignment = Element.ALIGN_CENTER;
                //pdfFooter.Border = iTextSharp.text.Rectangle.NO_BORDER;

                //// Sets the document footer to pdfFooter.
                document.Footer = pdfFooter;
                //document.Footer = new HeaderFooter(new Phrase("        Date : " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + "   Page: ", foot), new Phrase(Environment.NewLine + " ", foot));


                // Opens the document.
                document.Open();
                // Adds the mainTable to the document.
                document.Add(mainTable);
                // Closes the document.
                document.Close();
                Session["Print"] = true;
                Response.Write("<script>window.open ('/PDFPRint.aspx','_blank');</script>");
                //Response.ContentType = "application/pdf";
                ////Response.AddHeader("content-disposition", "attachment; filename= SampleExport.pdf");
                //Response.AddHeader("content-disposition", "attachment;filename=Report_UserDocumetUpload_SummaryDetails_" + DateTime.Now.Ticks + ".pdf");
                //Response.End();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void RDBReportView_SelectedIndexChanged(object sender, EventArgs e)
        {
            DivOptions.Visible = true;
            DivActionButtons.Visible = true;
            DivProjectSelection.Visible = true;
            DivStatus.Visible = false;
            DivFilter.Visible = false;
            ddlOptions.SelectedIndex = 0;
            ddlusers.SelectedIndex = 0;
            ddlProject.SelectedIndex = 0;
            divsummary.Visible = false;
            divDetails.Visible = false;
            divDetialsPaging.Visible = false;
            dtFromDate.Text = "";
            dtToDate.Text = "";
            lblDocumentsNo.Visible = false;
            btnExcel.Visible = false;
            btnPDF.Visible = false;
            btnPrint.Visible = false;
            ddlPages.Visible = false;
            if (RDBReportView.SelectedValue == "Project")
            {
                ddlusers.Enabled = false;
                if (ddlOptions.Items.Count == 3)
                {
                    ddlOptions.Items.RemoveAt(2);
                }
            }
            else
            {
                if (ddlOptions.Items.Count == 2)
                {
                    ddlOptions.Items.Insert(2, new System.Web.UI.WebControls.ListItem("All Details", "2"));
                }
                ddlusers.Enabled = true;                
            }
        }

        //private void PrinttoPDF(GridView gd, int type)
        //{
        //    try
        //    {

        //        DateTime CDate1 = DateTime.Now;
        //        GridView gdRp = new GridView();
        //        gdRp = gd;

        //        int noOfColumns = 0, noOfRows = 0;
        //        DataTable tbl = null;
        //        string fromdate = "";
        //        string todate = "";
        //        if (dtFromDate.Text == "")
        //        {
        //            fromdate = getDates()[0];
        //        }
        //        else
        //        {
        //            fromdate = dtFromDate.Text;
        //        }
        //        if (dtToDate.Text == "")
        //        {
        //            todate = getDates()[1];
        //        }
        //        else
        //        {
        //            todate = dtToDate.Text;
        //        }
        //        if (gdRp.AutoGenerateColumns)
        //        {
        //            tbl = gdRp.DataSource as DataTable; // Gets the DataSource of the GridView Control.
        //            noOfColumns = tbl.Columns.Count;
        //            noOfRows = tbl.Rows.Count;
        //        }
        //        else
        //        {
        //            noOfColumns = gdRp.Columns.Count;
        //            noOfRows = gdRp.Rows.Count;
        //        }

        //        float HeaderTextSize = 9;
        //        float ReportNameSize = 9;
        //        float ReportTextSize = 9;
        //        float ApplicationNameSize = 13;
        //        string ProjectName = ddlProject.SelectedItem.ToString();
        //        string Username = "";
        //        string Heading = "";
        //        if (RDBReportView.SelectedValue == "User")
        //        {
        //            Username = ddlusers.SelectedItem.ToString();
        //            if (Username.Length > 30)
        //            {
        //                Username = Username.Substring(0, 29) + "..";
        //            }
        //            Heading = "User Wise Document Upload";
        //        }
        //        else
        //        {
        //            Heading = "Project Wise Document Upload";
        //        }


        //        if (ProjectName.Length > 30)
        //        {
        //            ProjectName = ProjectName.Substring(0, 29) + "..";
        //        }


        //        //float HeaderTextSize = 15;
        //        //float ReportNameSize = 18;
        //        //float ReportTextSize = 15;
        //        //float ApplicationNameSize = 20;


        //        // Creates a PDF document

        //        Document document = null;
        //        //if (LandScape == true)
        //        //{
        //        // Sets the document to A4 size and rotates it so that the     orientation of the page is Landscape.
        //        document = new Document(PageSize.A4.Rotate(), 0, 0, 0, 0);
        //        //}
        //        //else
        //        //{
        //        //    document = new Document(PageSize.A4, 0, 0, 15, 5);
        //        //}

        //        // Creates a PdfPTable with column count of the table equal to no of columns of the gridview or gridview datasource.
        //        iTextSharp.text.pdf.PdfPTable mainTable = new iTextSharp.text.pdf.PdfPTable(noOfColumns);

        //        // Sets the first 4 rows of the table as the header rows which will be repeated in all the pages.
        //        mainTable.HeaderRows = 4;

        //        // Creates a PdfPTable with 2 columns to hold the header in the exported PDF.
        //        iTextSharp.text.pdf.PdfPTable headerTable = new iTextSharp.text.pdf.PdfPTable(1);

        //        // Creates a phrase to hold the application name at the left hand side of the header.
        //        Phrase phApplicationName = new Phrase(Environment.NewLine + WebConfigurationManager.AppSettings["Domain"] + " Report Name: " + Heading + "", FontFactory.GetFont("Arial", ApplicationNameSize, iTextSharp.text.Font.BOLD));

        //        // Creates a PdfPCell which accepts a phrase as a parameter.
        //        PdfPCell clApplicationName = new PdfPCell(phApplicationName);
        //        // Sets the border of the cell to zero.
        //        clApplicationName.Border = PdfPCell.NO_BORDER;
        //        // Sets the Horizontal Alignment of the PdfPCell to left.
        //        clApplicationName.HorizontalAlignment = Element.ALIGN_CENTER;

        //        // Creates a phrase to show the current date at the right hand side of the header.
        //        //Phrase phDate = new Phrase(DateTime.Now.Date.ToString("dd/MM/yyyy"), FontFactory.GetFont("Arial", ApplicationNameSize, iTextSharp.text.Font.NORMAL));

        //        //// Creates a PdfPCell which accepts the date phrase as a parameter.
        //        //PdfPCell clDate = new PdfPCell(phDate);
        //        //// Sets the Horizontal Alignment of the PdfPCell to right.
        //        //clDate.HorizontalAlignment = Element.ALIGN_RIGHT;
        //        //// Sets the border of the cell to zero.
        //        //clDate.Border = PdfPCell.NO_BORDER;

        //        // Adds the cell which holds the application name to the headerTable.
        //        headerTable.AddCell(clApplicationName);
        //        // Adds the cell which holds the date to the headerTable.
        //        //  headerTable.AddCell(clDate);
        //        // Sets the border of the headerTable to zero.
        //        headerTable.DefaultCell.Border = PdfPCell.NO_BORDER;

        //        // Creates a PdfPCell that accepts the headerTable as a parameter and then adds that cell to the main PdfPTable.
        //        PdfPCell cellHeader = new PdfPCell(headerTable);
        //        cellHeader.Border = PdfPCell.NO_BORDER;
        //        // Sets the column span of the header cell to noOfColumns.
        //        cellHeader.Colspan = noOfColumns;
        //        // Adds the above header cell to the table.
        //        mainTable.AddCell(cellHeader);

        //        // Creates a phrase which holds the file name.
        //        Phrase phHeader = new Phrase();
        //        if (RDBReportView.SelectedValue == "User")
        //        {
        //            phHeader = new Phrase("Project Name :" + ProjectName + ", User :" + Username + ", Report : " + ddlOptions.SelectedItem.ToString() + ", Start Date :" + fromdate + ", End Date : " + todate + Environment.NewLine + lblDocumentsNo.Text, FontFactory.GetFont("Arial", ReportNameSize, iTextSharp.text.Font.NORMAL));
        //        }
        //        else
        //        {
        //            phHeader = new Phrase("Project Name :" + ProjectName + ", Report : " + ddlOptions.SelectedItem.ToString() + ", Start Date :" + fromdate + ", End Date : " + todate + Environment.NewLine + lblDocumentsNo.Text, FontFactory.GetFont("Arial", ReportNameSize, iTextSharp.text.Font.NORMAL));
        //        }

        //        PdfPCell clHeader = new PdfPCell(phHeader);
        //        clHeader.Colspan = noOfColumns;
        //        clHeader.Border = PdfPCell.NO_BORDER;
        //        clHeader.HorizontalAlignment = Element.ALIGN_LEFT;
        //        mainTable.AddCell(clHeader);



        //        // Creates a phrase for a new line.
        //        Phrase phSpace = new Phrase("\n");
        //        PdfPCell clSpace = new PdfPCell(phSpace);
        //        clSpace.Border = PdfPCell.NO_BORDER;
        //        clSpace.Colspan = noOfColumns;
        //        mainTable.AddCell(clSpace);

        //        // Sets the gridview column names as table headers.
        //        for (int i = 0; i < noOfColumns; i++)
        //        {
        //            Phrase ph = null;

        //            if (gdRp.AutoGenerateColumns)
        //            {
        //                ph = new Phrase(tbl.Columns[i].ColumnName.Replace("<br/>", Environment.NewLine), FontFactory.GetFont("Arial", HeaderTextSize, iTextSharp.text.Font.BOLD));
        //            }
        //            else
        //            {
        //                ph = new Phrase(gdRp.Columns[i].HeaderText.Replace("<br/>", Environment.NewLine), FontFactory.GetFont("Arial", HeaderTextSize, iTextSharp.text.Font.BOLD));
        //            }
        //            PdfPCell cl = new PdfPCell(ph);
        //            cl.HorizontalAlignment = Element.ALIGN_CENTER;
        //            mainTable.AddCell(cl);
        //        }

        //        // Reads the gridview rows and adds them to the mainTable
        //        for (int rowNo = 0; rowNo <= noOfRows; rowNo++)
        //        {
        //            if (rowNo != noOfRows)
        //            {
        //                for (int columnNo = 0; columnNo < noOfColumns; columnNo++)
        //                {
        //                    if (gdRp.AutoGenerateColumns)
        //                    {
        //                        string s = gdRp.Rows[rowNo].Cells[columnNo].Text.Trim();
        //                        Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
        //                        PdfPCell cl = new PdfPCell(ph);
        //                        cl.HorizontalAlignment = Element.ALIGN_CENTER;
        //                        mainTable.AddCell(cl);
        //                    }
        //                    else
        //                    {
        //                        if (columnNo == 3)
        //                        {
        //                            if (gdRp.Columns[columnNo] is TemplateField)
        //                            {
        //                                DataBoundLiteralControl lc = gdRp.Rows[rowNo].Cells[columnNo].Controls[0] as DataBoundLiteralControl;
        //                                string s = lc.Text.Trim();
        //                                Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
        //                                PdfPCell cl = new PdfPCell(ph);
        //                                cl.HorizontalAlignment = Element.ALIGN_CENTER;
        //                                mainTable.AddCell(cl);
        //                            }
        //                            else
        //                            {
        //                                string s = gdRp.Rows[rowNo].Cells[columnNo].Text.Trim();
        //                                s = s.Replace("&nbsp;", "");
        //                                Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
        //                                PdfPCell cl = new PdfPCell(ph);
        //                                cl.HorizontalAlignment = Element.ALIGN_CENTER;
        //                                mainTable.AddCell(cl);
        //                            }
        //                        }
        //                        else if (columnNo == 5)
        //                        {
        //                            if (gdRp.Columns[columnNo] is TemplateField)
        //                            {
        //                                DataBoundLiteralControl lc = gdRp.Rows[rowNo].Cells[columnNo].Controls[0] as DataBoundLiteralControl;
        //                                string s = lc.Text.Trim();
        //                                Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
        //                                PdfPCell cl = new PdfPCell(ph);
        //                                cl.HorizontalAlignment = Element.ALIGN_CENTER;
        //                                mainTable.AddCell(cl);
        //                            }
        //                            else
        //                            {
        //                                string s = gdRp.Rows[rowNo].Cells[columnNo].Text.Trim();
        //                                s = s.Replace("&nbsp;", "");
        //                                Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
        //                                PdfPCell cl = new PdfPCell(ph);
        //                                cl.HorizontalAlignment = Element.ALIGN_CENTER;
        //                                mainTable.AddCell(cl);
        //                            }
        //                        }
        //                        else
        //                        {
        //                            if (gdRp.Columns[columnNo] is TemplateField)
        //                            {
        //                                DataBoundLiteralControl lc = gdRp.Rows[rowNo].Cells[columnNo].Controls[0] as DataBoundLiteralControl;
        //                                string s = lc.Text.Trim();
        //                                Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
        //                                PdfPCell cl = new PdfPCell(ph);
        //                                cl.HorizontalAlignment = Element.ALIGN_CENTER;
        //                                mainTable.AddCell(cl);
        //                            }
        //                            else
        //                            {
        //                                string s = gdRp.Rows[rowNo].Cells[columnNo].Text.Trim();
        //                                s = s.Replace("&nbsp;", "");
        //                                Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
        //                                PdfPCell cl = new PdfPCell(ph);
        //                                cl.HorizontalAlignment = Element.ALIGN_CENTER;
        //                                mainTable.AddCell(cl);
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                if (type == 1)
        //                {
        //                    for (int columnNo = 0; columnNo < noOfColumns; columnNo++)
        //                    {
        //                        string s = "Grand Total";

        //                        if (columnNo == 1)
        //                        {
        //                            Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.BOLD));
        //                            PdfPCell cl = new PdfPCell(ph);
        //                            cl.HorizontalAlignment = Element.ALIGN_CENTER;
        //                            mainTable.AddCell(cl);
        //                        }
        //                        else if (columnNo == 2)
        //                        {
        //                            s = ViewState["grandtotalcount"].ToString();// Session["AwardedValue"].ToString();
        //                            Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.BOLD));
        //                            PdfPCell cl = new PdfPCell(ph);
        //                            cl.HorizontalAlignment = Element.ALIGN_CENTER;
        //                            mainTable.AddCell(cl);
        //                        }
        //                        else if (columnNo == 3)
        //                        {
        //                            s = ViewState["grandtotaldwnldcount"].ToString();// Session["ExpenditureOverallTotal"].ToString();
        //                            Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.BOLD));
        //                            PdfPCell cl = new PdfPCell(ph);
        //                            cl.HorizontalAlignment = Element.ALIGN_CENTER;
        //                            mainTable.AddCell(cl);
        //                        }
        //                        else if (columnNo == 4)
        //                        {
        //                            s = ViewState["grandtotalviewcount"].ToString();// Session["TargetedOverallTotal"].ToString() + "%";
        //                            Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.BOLD));
        //                            PdfPCell cl = new PdfPCell(ph);
        //                            cl.HorizontalAlignment = Element.ALIGN_CENTER;
        //                            mainTable.AddCell(cl);
        //                        }
        //                        else if (columnNo == 5)
        //                        {
        //                            s = ViewState["GrandTotalDocumentLinkSent"].ToString();// Session["AchievedOverallTotal"].ToString() + "%";
        //                            Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.BOLD));
        //                            PdfPCell cl = new PdfPCell(ph);
        //                            cl.HorizontalAlignment = Element.ALIGN_CENTER;
        //                            mainTable.AddCell(cl);
        //                        }
        //                        else
        //                        {
        //                            Phrase ph = new Phrase("", FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.BOLD));
        //                            PdfPCell cl = new PdfPCell(ph);
        //                            cl.HorizontalAlignment = Element.ALIGN_CENTER;
        //                            mainTable.AddCell(cl);
        //                        }


        //                    }
        //                }

        //            }

        //            // Tells the mainTable to complete the row even if any cell is left incomplete.
        //            mainTable.CompleteRow();
        //        }

        //        // Gets the instance of the document created and writes it to the output stream of the Response object.
        //        //PdfWriter.GetInstance(document, Response.OutputStream);
        //        PdfWriter.GetInstance(document, new FileStream(Server.MapPath("~") + "/RegExcel/printpdf.pdf", FileMode.Create));

        //        // Creates a footer for the PDF document.
        //        iTextSharp.text.Font foot = new iTextSharp.text.Font();
        //        foot.Size = 10;

        //        HeaderFooter pdfFooter = new HeaderFooter(new Phrase("Date : " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + "   Page: ", foot), new Phrase(Environment.NewLine + " ", foot));
        //        pdfFooter.Alignment = Element.ALIGN_CENTER;
        //        //pdfFooter.Border = iTextSharp.text.Rectangle.NO_BORDER;

        //        //// Sets the document footer to pdfFooter.
        //        document.Footer = pdfFooter;

        //        // document.Footer = new HeaderFooter(new Phrase("        Date : " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + "   Page: ", foot), new Phrase(Environment.NewLine + " ", foot));


        //        // Opens the document.
        //        document.Open();
        //        // Adds the mainTable to the document.
        //        document.Add(mainTable);
        //        // Closes the document.
        //        document.Close();
        //        // Response.Redirect("~/PDFPRint.aspx");
        //        Session["Print"] = true;
        //        Response.Write("<script>window.open ('/PDFPRint.aspx','_blank');</script>");
        //        //Response.Buffer = true;
        //        //Response.ContentType = "application/pdf";
        //        //// Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //        ////Response.AddHeader("content-disposition", "attachment; filename= SampleExport.pdf");
        //        //Response.AddHeader("content-disposition", "attachment;filename=Report_UserDocumetUpload_SummaryDetails_" + DateTime.Now.Ticks + ".pdf");
        //        ////Response.AddHeader("content-disposition", "inline;filename=Report_UserDocumetUpload_SummaryDetails_" + DateTime.Now.Ticks + ".pdf");
        //        //// Response.AppendHeader("Content-Disposition", "attachment; filename=FileName.pdf");
        //        //// Response.TransmitFile(Server.MapPath("~/RegExcel/Sample.pdf"));
        //        //Response.End();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //private void PrinttoPDFDetails(GridView gd, int type)
        //{
        //    try
        //    {

        //        DateTime CDate1 = DateTime.Now;
        //        GridView gdRp = new GridView();
        //        gdRp = gd;

        //        int noOfColumns = 0, noOfRows = 0;
        //        DataTable tbl = null;
        //        string fromdate = "";
        //        string todate = "";
        //        if (dtFromDate.Text == "")
        //        {
        //            fromdate = getDates()[0];
        //        }
        //        else
        //        {
        //            fromdate = dtFromDate.Text;
        //        }
        //        if (dtToDate.Text == "")
        //        {
        //            todate = getDates()[1];
        //        }
        //        else
        //        {
        //            todate = dtToDate.Text;
        //        }
        //        if (gdRp.AutoGenerateColumns)
        //        {
        //            tbl = gdRp.DataSource as DataTable; // Gets the DataSource of the GridView Control.
        //            noOfColumns = tbl.Columns.Count;
        //            noOfRows = tbl.Rows.Count;
        //        }
        //        else
        //        {
        //            noOfColumns = gdRp.Columns.Count;
        //            noOfRows = gdRp.Rows.Count;
        //        }

        //        float HeaderTextSize = 9;
        //        float ReportNameSize = 9;
        //        float ReportTextSize = 9;
        //        float ApplicationNameSize = 13;
        //        string ProjectName = ddlProject.SelectedItem.ToString();
        //        string Username = ddlusers.SelectedItem.ToString();
        //        if (ProjectName.Length > 30)
        //        {
        //            ProjectName = ProjectName.Substring(0, 29) + "..";
        //        }
        //        if (Username.Length > 30)
        //        {
        //            Username = Username.Substring(0, 29) + "..";
        //        }
        //        float HeaderTextSize = 15;
        //        float ReportNameSize = 18;
        //        float ReportTextSize = 15;
        //        float ApplicationNameSize = 20;
        //        string status = "";
        //        string page = (gd.PageIndex + 1).ToString();
        //        if (DDLStatus.SelectedIndex == 0)
        //        {
        //            status = "All";

        //        }
        //        else
        //        {
        //            status = DDLStatus.SelectedItem.Text;
        //        }
        //        if (ddlPages.SelectedIndex == 0)
        //        {
        //            page = "All";
        //        }

        //        Creates a PDF document

        //        Document document = null;
        //        if (LandScape == true)
        //        {
        //            Sets the document to A4 size and rotates it so that the     orientation of the page is Landscape.
        //           document = new Document(PageSize.A4.Rotate(), 0, 0, 0, 0);
        //        }
        //        else
        //        {
        //            document = new Document(PageSize.A4, 0, 0, 15, 5);
        //        }

        //        Creates a PdfPTable with column count of the table equal to no of columns of the gridview or gridview datasource.
        //        iTextSharp.text.pdf.PdfPTable mainTable = new iTextSharp.text.pdf.PdfPTable(noOfColumns);

        //        Sets the first 4 rows of the table as the header rows which will be repeated in all the pages.
        //       mainTable.HeaderRows = 4;

        //        Creates a PdfPTable with 2 columns to hold the header in the exported PDF.
        //       iTextSharp.text.pdf.PdfPTable headerTable = new iTextSharp.text.pdf.PdfPTable(1);

        //        Creates a phrase to hold the application name at the left hand side of the header.
        //        Phrase phApplicationName = new Phrase(Environment.NewLine + WebConfigurationManager.AppSettings["Domain"] + " Report Name: User / Project Wise Document Upload", FontFactory.GetFont("Arial", ApplicationNameSize, iTextSharp.text.Font.BOLD));

        //        Creates a PdfPCell which accepts a phrase as a parameter.
        //       PdfPCell clApplicationName = new PdfPCell(phApplicationName);
        //        Sets the border of the cell to zero.
        //        clApplicationName.Border = PdfPCell.NO_BORDER;
        //        Sets the Horizontal Alignment of the PdfPCell to left.
        //       clApplicationName.HorizontalAlignment = Element.ALIGN_CENTER;

        //        Creates a phrase to show the current date at the right hand side of the header.
        //        Phrase phDate = new Phrase(DateTime.Now.Date.ToString("dd/MM/yyyy"), FontFactory.GetFont("Arial", ApplicationNameSize, iTextSharp.text.Font.NORMAL));

        //        // Creates a PdfPCell which accepts the date phrase as a parameter.
        //        PdfPCell clDate = new PdfPCell(phDate);
        //        // Sets the Horizontal Alignment of the PdfPCell to right.
        //        clDate.HorizontalAlignment = Element.ALIGN_RIGHT;
        //        // Sets the border of the cell to zero.
        //        clDate.Border = PdfPCell.NO_BORDER;

        //        Adds the cell which holds the application name to the headerTable.
        //       headerTable.AddCell(clApplicationName);
        //        Adds the cell which holds the date to the headerTable.
        //          headerTable.AddCell(clDate);
        //        Sets the border of the headerTable to zero.
        //        headerTable.DefaultCell.Border = PdfPCell.NO_BORDER;

        //        Creates a PdfPCell that accepts the headerTable as a parameter and then adds that cell to the main PdfPTable.
        //        PdfPCell cellHeader = new PdfPCell(headerTable);
        //        cellHeader.Border = PdfPCell.NO_BORDER;
        //        Sets the column span of the header cell to noOfColumns.
        //        cellHeader.Colspan = noOfColumns;
        //        Adds the above header cell to the table.
        //        mainTable.AddCell(cellHeader);

        //        Creates a phrase which holds the file name.
        //        Phrase phHeader = new Phrase("Project Name :" + ProjectName + ", User :" + Username + ", Report : " + ddlOptions.SelectedItem.ToString() + "Start Date :" + fromdate + ", End Date : " + todate + " , Status : " + status + " , Filter : " + DDLFilter.SelectedItem.Text + ", Page No : " + page + Environment.NewLine + lblDocumentsNo.Text, FontFactory.GetFont("Arial", ReportNameSize, iTextSharp.text.Font.NORMAL));
        //        PdfPCell clHeader = new PdfPCell(phHeader);
        //        clHeader.Colspan = noOfColumns;
        //        clHeader.Border = PdfPCell.NO_BORDER;
        //        clHeader.HorizontalAlignment = Element.ALIGN_LEFT;
        //        mainTable.AddCell(clHeader);



        //        Creates a phrase for a new line.
        //       Phrase phSpace = new Phrase("\n");
        //       PdfPCell clSpace = new PdfPCell(phSpace);
        //        clSpace.Border = PdfPCell.NO_BORDER;
        //        clSpace.Colspan = noOfColumns;
        //        mainTable.AddCell(clSpace);

        //        Sets the gridview column names as table headers.
        //        for (int i = 0; i < noOfColumns; i++)
        //        {
        //            Phrase ph = null;

        //            if (gdRp.AutoGenerateColumns)
        //            {
        //                ph = new Phrase(tbl.Columns[i].ColumnName.Replace("<br/>", Environment.NewLine), FontFactory.GetFont("Arial", HeaderTextSize, iTextSharp.text.Font.BOLD));
        //            }
        //            else
        //            {
        //                ph = new Phrase(gdRp.Columns[i].HeaderText.Replace("<br/>", Environment.NewLine), FontFactory.GetFont("Arial", HeaderTextSize, iTextSharp.text.Font.BOLD));
        //            }
        //            PdfPCell cl = new PdfPCell(ph);
        //            cl.HorizontalAlignment = Element.ALIGN_CENTER;
        //            mainTable.AddCell(cl);
        //        }

        //        Reads the gridview rows and adds them to the mainTable
        //        for (int rowNo = 0; rowNo <= noOfRows; rowNo++)
        //        {
        //            if (rowNo != noOfRows)
        //            {
        //                for (int columnNo = 0; columnNo < noOfColumns; columnNo++)
        //                {
        //                    if (gdRp.AutoGenerateColumns)
        //                    {
        //                        string s = gdRp.Rows[rowNo].Cells[columnNo].Text.Trim();
        //                        Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
        //                        PdfPCell cl = new PdfPCell(ph);
        //                        cl.HorizontalAlignment = Element.ALIGN_CENTER;
        //                        mainTable.AddCell(cl);
        //                    }
        //                    else
        //                    {
        //                        if (columnNo == 3)
        //                        {
        //                            if (gdRp.Columns[columnNo] is TemplateField)
        //                            {
        //                                DataBoundLiteralControl lc = gdRp.Rows[rowNo].Cells[columnNo].Controls[0] as DataBoundLiteralControl;
        //                                string s = lc.Text.Trim();
        //                                Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
        //                                PdfPCell cl = new PdfPCell(ph);
        //                                cl.HorizontalAlignment = Element.ALIGN_CENTER;
        //                                mainTable.AddCell(cl);


        //                            }
        //                            else
        //                            {
        //                                string s = gdRp.Rows[rowNo].Cells[columnNo].Text.Trim();
        //                                s = s.Replace("&nbsp;", "");
        //                                Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
        //                                PdfPCell cl = new PdfPCell(ph);
        //                                cl.HorizontalAlignment = Element.ALIGN_CENTER;
        //                                mainTable.AddCell(cl);
        //                            }
        //                        }
        //                        else if (columnNo == 5)
        //                        {
        //                            if (gdRp.Columns[columnNo] is TemplateField)
        //                            {
        //                                DataBoundLiteralControl lc = gdRp.Rows[rowNo].Cells[columnNo].Controls[0] as DataBoundLiteralControl;
        //                                string s = lc.Text.Trim();
        //                                Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
        //                                PdfPCell cl = new PdfPCell(ph);
        //                                cl.HorizontalAlignment = Element.ALIGN_CENTER;
        //                                mainTable.AddCell(cl);
        //                            }
        //                            else
        //                            {
        //                                string s = gdRp.Rows[rowNo].Cells[columnNo].Text.Trim();
        //                                s = s.Replace("&nbsp;", "");
        //                                Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
        //                                PdfPCell cl = new PdfPCell(ph);
        //                                cl.HorizontalAlignment = Element.ALIGN_CENTER;
        //                                mainTable.AddCell(cl);
        //                            }
        //                        }
        //                        else
        //                        {
        //                            if (gdRp.Columns[columnNo] is TemplateField)
        //                            {
        //                                DataBoundLiteralControl lc = gdRp.Rows[rowNo].Cells[columnNo].Controls[0] as DataBoundLiteralControl;
        //                                string s = lc.Text.Trim();
        //                                Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
        //                                PdfPCell cl = new PdfPCell(ph);
        //                                cl.HorizontalAlignment = Element.ALIGN_CENTER;
        //                                mainTable.AddCell(cl);
        //                            }
        //                            else
        //                            {
        //                                string s = gdRp.Rows[rowNo].Cells[columnNo].Text.Trim();
        //                                s = s.Replace("&nbsp;", "");
        //                                Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
        //                                PdfPCell cl = new PdfPCell(ph);
        //                                cl.HorizontalAlignment = Element.ALIGN_CENTER;
        //                                mainTable.AddCell(cl);
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                if (type == 1)
        //                {
        //                    for (int columnNo = 0; columnNo < noOfColumns; columnNo++)
        //                    {
        //                        string s = "Grand Total";
        //                        if (columnNo == 1)
        //                        {
        //                            Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.BOLD));
        //                            PdfPCell cl = new PdfPCell(ph);
        //                            cl.HorizontalAlignment = Element.ALIGN_CENTER;
        //                            mainTable.AddCell(cl);
        //                        }
        //                        else if (columnNo == 2)
        //                        {
        //                            s = ViewState["grandtotalcount"].ToString();// Session["AwardedValue"].ToString();
        //                            Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.BOLD));
        //                            PdfPCell cl = new PdfPCell(ph);
        //                            cl.HorizontalAlignment = Element.ALIGN_CENTER;
        //                            mainTable.AddCell(cl);
        //                        }
        //                        else if (columnNo == 3)
        //                        {
        //                            s = ViewState["grandtotaldwnldcount"].ToString();// Session["ExpenditureOverallTotal"].ToString();
        //                            Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.BOLD));
        //                            PdfPCell cl = new PdfPCell(ph);
        //                            cl.HorizontalAlignment = Element.ALIGN_CENTER;
        //                            mainTable.AddCell(cl);
        //                        }
        //                        else if (columnNo == 4)
        //                        {
        //                            s = ViewState["grandtotalviewcount"].ToString();// Session["TargetedOverallTotal"].ToString() + "%";
        //                            Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.BOLD));
        //                            PdfPCell cl = new PdfPCell(ph);
        //                            cl.HorizontalAlignment = Element.ALIGN_CENTER;
        //                            mainTable.AddCell(cl);
        //                        }
        //                        else if (columnNo == 5)
        //                        {
        //                            s = ViewState["GrandTotalDocumentLinkSent"].ToString();// Session["AchievedOverallTotal"].ToString() + "%";
        //                            Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.BOLD));
        //                            PdfPCell cl = new PdfPCell(ph);
        //                            cl.HorizontalAlignment = Element.ALIGN_CENTER;
        //                            mainTable.AddCell(cl);
        //                        }
        //                        else
        //                        {
        //                            Phrase ph = new Phrase("", FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.BOLD));
        //                            PdfPCell cl = new PdfPCell(ph);
        //                            cl.HorizontalAlignment = Element.ALIGN_CENTER;
        //                            mainTable.AddCell(cl);
        //                        }
        //                    }
        //                }

        //            }

        //            Tells the mainTable to complete the row even if any cell is left incomplete.
        //            mainTable.CompleteRow();
        //        }

        //        Gets the instance of the document created and writes it to the output stream of the Response object.
        //       PdfWriter.GetInstance(document, Response.OutputStream);
        //        PdfWriter.GetInstance(document, new FileStream(Server.MapPath("~") + "/RegExcel/printpdf.pdf", FileMode.Create));

        //        Creates a footer for the PDF document.

        //       HeaderFooter pdfFooter = new HeaderFooter(new Phrase(), true);
        //        pdfFooter.Alignment = Element.ALIGN_CENTER;
        //        pdfFooter.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //        pdfFooter.Bottom = iTextSharp.text.Rectangle.BOTTOM_BORDER;
        //        // Sets the document footer to pdfFooter.
        //        document.Footer = pdfFooter;
        //        iTextSharp.text.Font foot = new iTextSharp.text.Font();
        //        foot.Size = 10;

        //        HeaderFooter pdfFooter = new HeaderFooter(new Phrase("Date : " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + "   Page: ", foot), new Phrase(Environment.NewLine + " ", foot));
        //        pdfFooter.Alignment = Element.ALIGN_CENTER;
        //        pdfFooter.Border = iTextSharp.text.Rectangle.NO_BORDER;

        //        // Sets the document footer to pdfFooter.
        //        document.Footer = pdfFooter;
        //        document.Footer = new HeaderFooter(new Phrase("        Date : " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + "   Page: ", foot), new Phrase(Environment.NewLine + " ", foot));


        //        Opens the document.
        //       document.Open();
        //        Adds the mainTable to the document.
        //        document.Add(mainTable);
        //        Closes the document.
        //       document.Close();
        //        Session["Print"] = true;
        //        Response.Write("<script>window.open ('/PDFPRint.aspx','_blank');</script>");
        //        Response.ContentType = "application/pdf";
        //        //Response.AddHeader("content-disposition", "attachment; filename= SampleExport.pdf");
        //        Response.AddHeader("content-disposition", "attachment;filename=Report_UserDocumetUpload_SummaryDetails_" + DateTime.Now.Ticks + ".pdf");
        //        Response.End();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}


    }
}