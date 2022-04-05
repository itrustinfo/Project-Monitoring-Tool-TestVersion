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
using ProjectManagementTool.DAL;
using ProjectManager.DAL;

namespace ProjectManagementTool._content_pages.document_search_updatepanel
{
    public partial class _default : System.Web.UI.Page
    {
        DBGetData getdt = new DBGetData();
        TaskUpdate gettk = new TaskUpdate();
        GeneralDocuments GD = new GeneralDocuments();
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
                    if (WebConfigurationManager.AppSettings["Domain"].ToString().ToUpper() == "NJSEI")
                        lblProjRefNO.Text = "NJSEI Reference #";
                    else
                        lblProjRefNO.Text = "ONTB Reference #";

                    SelectedProjectWorkpackage("Project");
                    if (Session["searchedit"] != null)
                    {
                        DDlProject.SelectedValue = Session["PUID"].ToString();
                        txtSubmittal.Text = Session["sSubmittal"].ToString();
                        txtDocName.Text = Session["sDocName"].ToString();
                        dtInDate.Text = Session["sDate"].ToString();
                        dtInToDate.Text = Session["sDateTo"].ToString();
                        dtDocDate.Text = Session["sDocDate"].ToString();
                        dtDocToDate.Text = Session["sDocDateTo"].ToString();
                    }
                    DDlProject_SelectedIndexChanged(sender, e);
                    //ddlPhase.Attributes["style"] = "display: none;";
                    //BindStatus();
                    //BindType();
                    // BindDocuments();
                    BindTypeGD();
                    //BindPhases();
                    ViewState["SortDireaction"] = "";
                    ViewState["_sortDirection"] = "";
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

        private void BindPhases(Guid WorkpackageUID)
        {
            DataSet dtStatus = getdt.GetStatusForSearch(new Guid(DDLWorkPackage.SelectedValue));
            DataTable dt = getdt.GetPhasesAndStatusForSearch(WorkpackageUID);
            if (dt != null)
            {
                ddlPhase.DataTextField = "Phase";
                ddlPhase.DataValueField = "Phase";
                ddlPhase.DataSource = dt;
                ddlPhase.DataBind();
                
            }

            if(dtStatus != null && dtStatus.Tables[0].Rows.Count > 0)
            {
                var isExist = dtStatus.Tables[0].AsEnumerable().Where(r => r.Field<string>("ActualDocument_CurrentStatus") == "Code A-CE Approval").FirstOrDefault();
                if(isExist != null)
                {
                    ddlPhase.Items.Add(new ListItem { Text = "Approved By BWSSB Under Code A", Value = "Code A-CE Approval" });
                }
                isExist = dtStatus.Tables[0].AsEnumerable().Where(r => r.Field<string>("ActualDocument_CurrentStatus") == "Code B-CE Approval").FirstOrDefault();
                if (isExist != null)
                {
                    ddlPhase.Items.Add(new ListItem { Text = "Approved By BWSSB Under Code B", Value = "Code B-CE Approval" });
                }
                isExist = dtStatus.Tables[0].AsEnumerable().Where(r => r.Field<string>("ActualDocument_CurrentStatus") == "Code C-CE Approval").FirstOrDefault();
                if (isExist != null)
                {
                    ddlPhase.Items.Add(new ListItem { Text = "Under Client Approval Process", Value = "Code C-CE Approval" });
                }
                isExist = dtStatus.Tables[0].AsEnumerable().Where(r => r.Field<string>("ActualDocument_CurrentStatus") == "Client CE GFC Approval").FirstOrDefault();
                if (isExist != null)
                {
                    ddlPhase.Items.Add(new ListItem { Text = "Approved GFC by BWSSB", Value = "Client CE GFC Approval" });
                }
            }
            
            ddlPhase.Items.Insert(0, "All");
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

        private void BindStatus()
        {
            ddlstatus.DataSource = getdt.GetStatusForSearch(new Guid(DDLWorkPackage.SelectedValue));
            ddlstatus.DataTextField = "ActualDocument_CurrentStatus";
            ddlstatus.DataValueField = "ActualDocument_CurrentStatus";
            ddlstatus.DataBind();
            ddlstatus.Items.Insert(0, "All");
        }

        private void BindType()
        {
            DataSet ds = getdt.GetDoctypeForSearch(new Guid(DDLWorkPackage.SelectedValue));
            string name = "";
            ddlType.Items.Clear();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (dr["DocumentType"].ToString() == "Word")
                {  
                    if (name != dr["DocumentType"].ToString())
                    {
                        name = dr["DocumentType"].ToString();
                        ddlType.Items.Add(new ListItem(dr["DocumentType"].ToString(), ".doc"));
                    }
                   
                }
                else if (dr["DocumentType"].ToString() == "Excel")
                {
                    if (name != dr["DocumentType"].ToString())
                    {
                        name = dr["DocumentType"].ToString();
                        ddlType.Items.Add(new ListItem(dr["DocumentType"].ToString(), ".xls"));
                    }

                }
                else
                {
                    ddlType.Items.Add(new ListItem(dr["DocumentType"].ToString(), dr["DocumentExtension"].ToString()));
                }
            }

            //ddlType.DataSource = getdt.GetDoctypeForSearch(new Guid(DDLWorkPackage.SelectedValue));
            //ddlType.DataTextField = "DocumentType";
            //ddlType.DataValueField = "DocumentExtension";
            //ddlType.DataBind();
            ddlType.Items.Insert(0, "All");
        }

        private void BindTypeGD()
        {
            DataSet ds = GD.GetDoctypeForSearchGD();
            string name = "";
            ddlDocType.Items.Clear();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (dr["DocumentType"].ToString() == "Word")
                {

                    if (name != dr["DocumentType"].ToString())
                    {
                        name = dr["DocumentType"].ToString();
                        ddlDocType.Items.Add(new ListItem(dr["DocumentType"].ToString(), ".doc"));
                    }

                }
                else if (dr["DocumentType"].ToString() == "Excel")
                {
                    if (name != dr["DocumentType"].ToString())
                    {
                        name = dr["DocumentType"].ToString();
                        ddlDocType.Items.Add(new ListItem(dr["DocumentType"].ToString(), ".xls"));
                    }

                }
                else
                {
                    ddlDocType.Items.Add(new ListItem(dr["DocumentType"].ToString(), dr["DocumentExtension"].ToString()));
                }
            }

            //ddlType.DataSource = getdt.GetDoctypeForSearch(new Guid(DDLWorkPackage.SelectedValue));
            //ddlType.DataTextField = "DocumentType";
            //ddlType.DataValueField = "DocumentExtension";
            //ddlType.DataBind();
            ddlDocType.Items.Insert(0, "All");
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

            DDlProject.DataTextField = "ProjectName";
            DDlProject.DataValueField = "ProjectUID";
            DDlProject.DataSource = ds;
            DDlProject.DataBind();
            DDlProject.Items.Add("General Documents");
            DDlProject.Items.Insert(0, "-Select-");

        }

        protected void DDlProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDlProject.SelectedValue == "-Select-")
            {
                ddlPhase.Attributes["style"] = "display: none;";
                ddlstatus.Attributes["style"] = "";
                status.Text = "Status";
            }
            if (DDlProject.SelectedValue != "-Select-")
            {
                UpdatePanel2.Update();
            
                ddlstatus.DataSource = null;
                ddlstatus.DataBind();
                ddlstatus.Items.Clear();
                //
                ddlType.DataSource = null;
                ddlType.DataBind();
                ddlType.Items.Clear();
                //

                lbldocNos.Text = "0";
                UpdatePanel6.Update();
                GrdDocuments.Visible = false;
                DataSet ds = new DataSet();
               if(DDlProject.SelectedItem.ToString() == "General Documents")
                {
                    BindDocumentsGD();
                    divMain.Visible = false;
                    divGeneral.Visible = true;
                    divWP.Visible = false;
                }
                else {
                    divMain.Visible = true;
                    divGeneral.Visible = false;
                    divWP.Visible = true;
                    //ds = getdt.GetWorkPackages_By_ProjectUID(new Guid(DDlProject.SelectedValue));
                    if (Session["TypeOfUser"].ToString() == "U" || Session["TypeOfUser"].ToString() == "MD" || Session["TypeOfUser"].ToString() == "VP")
                    {
                        ds = getdt.GetWorkPackages_By_ProjectUID(new Guid(DDlProject.SelectedValue));
                    }
                    else if (Session["TypeOfUser"].ToString() == "PA")
                    {
                        ds = getdt.GetWorkPackages_ForUser_by_ProjectUID(new Guid(Session["UserUID"].ToString()), new Guid(DDlProject.SelectedValue));
                    }
                    else
                    {
                        ds = getdt.GetWorkPackages_ForUser_by_ProjectUID(new Guid(Session["UserUID"].ToString()), new Guid(DDlProject.SelectedValue));
                    }
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DDLWorkPackage.DataTextField = "Name";
                        DDLWorkPackage.DataValueField = "WorkPackageUID";
                        DDLWorkPackage.DataSource = ds;
                        DDLWorkPackage.DataBind();
                        DDLWorkPackage.Items.Insert(0, "-Select-");
                        // BindStatus();
                        //  BindType();
                        SelectedProjectWorkpackage("Workpackage");
                        if (Session["Project_Workpackage"] != null)
                        {
                            DDLWorkPackage_SelectedIndexChanged(sender, e);
                        }
                        if (Session["searchedit"] != null)
                        {
                            ddlstatus.SelectedValue = Session["sStatus"].ToString();
                            ddlType.SelectedValue = Session["sType"].ToString();
                        }
                        Session["Project_Workpackage"] = DDlProject.SelectedValue + "_" + DDLWorkPackage.SelectedValue;
                        // BindDocuments();
                    }
                    else
                    {
                        DDLWorkPackage.DataSource = null;
                        DDLWorkPackage.DataBind();
                        DDLWorkPackage.Items.Clear();
                        DDLWorkPackage.Items.Insert(0, "-Select-");
                        lbldocNos.Text = "0";
                    }
                }
            }
            else
            {
                UpdatePanel2.Update();
               
                GrdDocuments.Visible = false;
                DDLWorkPackage.DataSource = null;
                DDLWorkPackage.DataBind();
                DDLWorkPackage.Items.Insert(0, "-Select-");
                ddlstatus.DataSource = null;
                ddlstatus.DataBind();
                //
                ddlType.DataSource = null;
                ddlType.DataBind();
            }
          
        }

        private void SelectedProjectWorkpackage(string pType)
        {
            if (!IsPostBack && Session["Project_Workpackage"] != null)
            {
                string[] selectedValue = Session["Project_Workpackage"].ToString().Split('_');
                if (selectedValue.Length > 1)
                {
                    if (pType == "Project")
                    {
                        DDlProject.SelectedValue = selectedValue[0];
                    }
                    else
                    {
                        DDLWorkPackage.SelectedValue = selectedValue[1];
                    }
                }
                else
                {
                    if (pType == "Project")
                    {
                        DDlProject.SelectedValue = Session["Project_Workpackage"].ToString();
                    }
                }
            }

        }

        protected void GrdDocuments_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdDocuments.PageIndex = e.NewPageIndex;
            HiddenPaging.Value = "true";
            Session["PageIndex"] = GrdDocuments.PageIndex;
            BindDocuments();
           
        }

        private void BindDocuments()
        {
            if (Session["searchedit"] != null)
            {
                GrdDocuments.PageIndex = Convert.ToInt32(Session["PageIndex"]);
            }

          
            Session["searchedit"] = null;
            Session["sSubmittal"] = txtSubmittal.Text;
            Session["sDocName"] = txtDocName.Text;
            Session["sDate"] = dtInDate.Text;
            Session["sDocDate"] = dtDocDate.Text;
            Session["sType"] = ddlType.SelectedValue;
            Session["sStatus"] = ddlstatus.SelectedValue;
            Session["sDateTo"] = dtInToDate.Text;
            Session["sDocDateTo"] = dtDocToDate.Text;

            DateTime InDate = DateTime.Now;
            DateTime DocumentDate = DateTime.Now;
            DateTime InToDate = DateTime.Now;
            DateTime DocumentToDate = DateTime.Now;
            if (!string.IsNullOrEmpty(dtInDate.Text))
            {
                InDate = Convert.ToDateTime(getdt.ConvertDateFormat(dtInDate.Text));
            }
            if (string.IsNullOrEmpty(dtInToDate.Text))
            {
                InToDate = InDate;
            }
           
            if (!string.IsNullOrEmpty(dtDocDate.Text))
            {
                DocumentDate = Convert.ToDateTime(getdt.ConvertDateFormat(dtDocDate.Text));
            }
            if (string.IsNullOrEmpty(dtDocToDate.Text))
            {
                DocumentToDate = DocumentDate;
            }
            if (!string.IsNullOrEmpty(dtInToDate.Text))
            {
                InToDate = Convert.ToDateTime(getdt.ConvertDateFormat(dtInToDate.Text));
            }
            if (!string.IsNullOrEmpty(dtDocToDate.Text))
            {
                DocumentToDate = Convert.ToDateTime(getdt.ConvertDateFormat(dtDocToDate.Text));
            }


            // validations
            if (dtInDate.Text == "" && dtInToDate.Text != "")
            {
                //Page.ClientScript.RegisterStartupScript(Page.GetType(), "Alert", "<script language='javascript'>alert('Please enter Incoming Recv From Date');</script>");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "CLOSE", "alert('Please enter Incoming Recv From Date.');", true);
                return;
            }
            else if (dtInDate.Text != "" && dtInToDate.Text == "")
            {
                //Page.ClientScript.RegisterStartupScript(Page.GetType(), "Alert", "<script language='javascript'>alert('Please enter Incoming Recv To Date');</script>");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "CLOSE", "alert('Please enter Incoming Recv To Date.');", true);
                return;
            }
            else if (dtInDate.Text != "" && dtInToDate.Text != "")
            {
                if (InDate > InToDate)
                {
                    //Page.ClientScript.RegisterStartupScript(Page.GetType(), "Alert", "<script language='javascript'>alert('Incoming Recv From Date cannot be greater than Incoming Recv To Date');</script>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "CLOSE", "alert('Incoming Recv From Date cannot be greater than Incoming Recv To Date.');", true);
                    return;
                }

            }

            if (dtDocDate.Text == "" && dtDocToDate.Text != "")
            {
                //Page.ClientScript.RegisterStartupScript(Page.GetType(), "Alert", "<script language='javascript'>alert('Please enter Document From Date');</script>");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "CLOSE", "alert('Please enter Document From Date.');", true);
                return;
            }
            else if (dtDocDate.Text != "" && dtDocToDate.Text == "")
            {
                //Page.ClientScript.RegisterStartupScript(Page.GetType(), "Alert", "<script language='javascript'>alert('Please enter Document To Date');</script>");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "CLOSE", "alert('Please enter Document To Date');", true);
                return;
            }
            else if (dtDocDate.Text != "" && dtDocToDate.Text != "")
            {
                if (DocumentDate > DocumentToDate)
                {
                    //Page.ClientScript.RegisterStartupScript(Page.GetType(), "Alert", "<script language='javascript'>alert('Document From Date cannot be greater than Document To Date');</script>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "CLOSE", "alert('Document From Date cannot be greater than Document To Date');", true);
                    return;
                }
            }
            DataSet ds = new DataSet();
            string statusValue = "";

            bool IsPhaseSearch = false;
            if (status.Text == "Phase" && ddlPhase.SelectedItem.Text != "All")
            {
                IsPhaseSearch = true;
                if (Constants.DicFinalStatusAndPhase.ContainsKey(ddlPhase.SelectedValue))
                {
                    IsPhaseSearch = false;
                    statusValue = ddlPhase.SelectedValue;
                }
            }

            if (IsPhaseSearch)
            {   
                ds = getdt.ActualDocuments_SelectBy_WorkpackageUID_SearchPhase(new Guid(DDlProject.SelectedValue), new Guid(DDLWorkPackage.SelectedValue), txtDocName.Text, ddlType.SelectedValue, txtSubmittal.Text, ddlPhase.SelectedValue, InDate, DocumentDate, InToDate, DocumentToDate, 4, txtOntbRef.Text, txtOriginatorRef.Text);

                if (dtDocDate.Text != "" && dtInDate.Text != "")
                {
                    ds = new DataSet();
                    ds = getdt.ActualDocuments_SelectBy_WorkpackageUID_SearchPhase(new Guid(DDlProject.SelectedValue), new Guid(DDLWorkPackage.SelectedValue), txtDocName.Text, ddlType.SelectedValue, txtSubmittal.Text, ddlPhase.SelectedValue, InDate, DocumentDate, InToDate, DocumentToDate, 3, txtOntbRef.Text, txtOriginatorRef.Text);
                }
                else if (dtInDate.Text != "")
                {
                    ds = new DataSet();
                    ds = getdt.ActualDocuments_SelectBy_WorkpackageUID_SearchPhase(new Guid(DDlProject.SelectedValue), new Guid(DDLWorkPackage.SelectedValue), txtDocName.Text, ddlType.SelectedValue, txtSubmittal.Text, ddlPhase.SelectedValue, InDate, DocumentDate, InToDate, DocumentToDate, 1, txtOntbRef.Text, txtOriginatorRef.Text);
                }
                else if (dtDocDate.Text != "")
                {
                    ds = new DataSet();
                    ds = getdt.ActualDocuments_SelectBy_WorkpackageUID_SearchPhase(new Guid(DDlProject.SelectedValue), new Guid(DDLWorkPackage.SelectedValue), txtDocName.Text, ddlType.SelectedValue, txtSubmittal.Text, ddlPhase.SelectedValue, InDate, DocumentDate, InToDate, DocumentToDate, 2, txtOntbRef.Text, txtOriginatorRef.Text);
                }
            }
            else
            {
                if(string.IsNullOrEmpty(statusValue))
                    statusValue = ddlstatus.SelectedValue;
                ds = getdt.ActualDocuments_SelectBy_WorkpackageUID_Search(new Guid(DDlProject.SelectedValue), new Guid(DDLWorkPackage.SelectedValue), txtDocName.Text, ddlType.SelectedValue, txtSubmittal.Text, statusValue, InDate, DocumentDate, InToDate, DocumentToDate, 4, txtOntbRef.Text, txtOriginatorRef.Text);
                if (dtDocDate.Text != "" && dtInDate.Text != "")
                {
                    ds = new DataSet();
                    ds = getdt.ActualDocuments_SelectBy_WorkpackageUID_Search(new Guid(DDlProject.SelectedValue), new Guid(DDLWorkPackage.SelectedValue), txtDocName.Text, ddlType.SelectedValue, txtSubmittal.Text, statusValue, InDate, DocumentDate, InToDate, DocumentToDate, 3, txtOntbRef.Text, txtOriginatorRef.Text);
                }
                else if (dtInDate.Text != "")
                {
                    ds = new DataSet();
                    ds = getdt.ActualDocuments_SelectBy_WorkpackageUID_Search(new Guid(DDlProject.SelectedValue), new Guid(DDLWorkPackage.SelectedValue), txtDocName.Text, ddlType.SelectedValue, txtSubmittal.Text, statusValue, InDate, DocumentDate, InToDate, DocumentToDate, 1, txtOntbRef.Text, txtOriginatorRef.Text);
                }
                else if (dtDocDate.Text != "")
                {
                    ds = new DataSet();
                    ds = getdt.ActualDocuments_SelectBy_WorkpackageUID_Search(new Guid(DDlProject.SelectedValue), new Guid(DDLWorkPackage.SelectedValue), txtDocName.Text, ddlType.SelectedValue, txtSubmittal.Text, statusValue, InDate, DocumentDate, InToDate, DocumentToDate, 2, txtOntbRef.Text, txtOriginatorRef.Text);
                }
            }

            
            if (ds.Tables[0].Rows.Count > 0)
            {
                lbldocNos.Text = ds.Tables[0].Rows.Count.ToString();
            }
            else
            {
                lbldocNos.Text = "0";
            }
            
            //  DataSet ds = getdt.ActualDocuments_SelectBy_WorkpackageUID_Search(new Guid(DDLWorkPackage.SelectedValue),"","","","",DocDate,1);



            //if (txtDocName.Text != "" && ddlType.SelectedIndex ==0 && txtSubmittal.Text =="" && ddlstatus.SelectedIndex == 0  && dtDocumentDate.Text == "")
            //{
            //    ds = getdt.ActualDocuments_SelectBy_WorkpackageUID_Search(new Guid(DDLWorkPackage.SelectedValue), txtDocName.Text, "", "", "", DocDate, 2);
            //}
            //else if (txtDocName.Text == "" && ddlType.SelectedIndex != 0 && txtSubmittal.Text == "" && ddlstatus.SelectedIndex == 0 && dtDocumentDate.Text == "")
            //{
            //    ds = getdt.ActualDocuments_SelectBy_WorkpackageUID_Search(new Guid(DDLWorkPackage.SelectedValue), txtDocName.Text, ddlType.SelectedValue, "", "", DocDate, 3);
            //}
            //else if (txtDocName.Text == "" && ddlType.SelectedIndex == 0 && txtSubmittal.Text != "" && ddlstatus.SelectedIndex == 0 && dtDocumentDate.Text == "")
            //{
            //    ds = getdt.ActualDocuments_SelectBy_WorkpackageUID_Search(new Guid(DDLWorkPackage.SelectedValue), txtDocName.Text, ddlType.SelectedValue, txtSubmittal.Text, "", DocDate, 4);

            //}
            //else if (txtDocName.Text == "" && ddlType.SelectedIndex == 0 && txtSubmittal.Text == "" && ddlstatus.SelectedIndex != 0 && dtDocumentDate.Text == "")
            //{
            //    ds = getdt.ActualDocuments_SelectBy_WorkpackageUID_Search(new Guid(DDLWorkPackage.SelectedValue), txtDocName.Text, ddlType.SelectedValue, txtSubmittal.Text, ddlstatus.SelectedValue, DocDate, 5);
            //}
            //else if (txtDocName.Text != "" && ddlType.SelectedIndex != 0 && txtSubmittal.Text != "" && ddlstatus.SelectedIndex != 0 && dtDocumentDate.Text =="" )
            //{
            //    ds = getdt.ActualDocuments_SelectBy_WorkpackageUID_Search(new Guid(DDLWorkPackage.SelectedValue), txtDocName.Text, ddlType.SelectedValue, txtSubmittal.Text, ddlstatus.SelectedValue, DocDate, 6);
            //}
            //else if (txtDocName.Text != "" && ddlType.SelectedIndex != 0 && txtSubmittal.Text != "" && ddlstatus.SelectedIndex == 0 && dtDocumentDate.Text == "")
            //{
            //    ds = getdt.ActualDocuments_SelectBy_WorkpackageUID_Search(new Guid(DDLWorkPackage.SelectedValue), txtDocName.Text, ddlType.SelectedValue, txtSubmittal.Text, ddlstatus.SelectedValue, DocDate, 7);
            //}
            //else if (txtDocName.Text != "" && ddlType.SelectedIndex != 0 && txtSubmittal.Text == "" && ddlstatus.SelectedIndex == 0 && dtDocumentDate.Text == "")
            //{
            //    ds = getdt.ActualDocuments_SelectBy_WorkpackageUID_Search(new Guid(DDLWorkPackage.SelectedValue), txtDocName.Text, ddlType.SelectedValue, txtSubmittal.Text, ddlstatus.SelectedValue, DocDate, 8);
            //}
            //else if (txtDocName.Text != "" && ddlType.SelectedIndex == 0 && txtSubmittal.Text != "" && ddlstatus.SelectedIndex == 0 && dtDocumentDate.Text == "")
            //{
            //    ds = getdt.ActualDocuments_SelectBy_WorkpackageUID_Search(new Guid(DDLWorkPackage.SelectedValue), txtDocName.Text, ddlType.SelectedValue, txtSubmittal.Text, ddlstatus.SelectedValue, DocDate, 9);
            //}
            //else if (txtDocName.Text == "" && ddlType.SelectedIndex != 0 && txtSubmittal.Text != "" && ddlstatus.SelectedIndex == 0 && dtDocumentDate.Text == "")
            //{
            //    ds = getdt.ActualDocuments_SelectBy_WorkpackageUID_Search(new Guid(DDLWorkPackage.SelectedValue), txtDocName.Text, ddlType.SelectedValue, txtSubmittal.Text, ddlstatus.SelectedValue, DocDate, 10);
            //}
            //else if (txtDocName.Text != "" && ddlType.SelectedIndex == 0 && txtSubmittal.Text == "" && ddlstatus.SelectedIndex != 0 && dtDocumentDate.Text == "")
            //{
            //    ds = getdt.ActualDocuments_SelectBy_WorkpackageUID_Search(new Guid(DDLWorkPackage.SelectedValue), txtDocName.Text, ddlType.SelectedValue, txtSubmittal.Text, ddlstatus.SelectedValue, DocDate, 11);
            //}
            //else if (txtDocName.Text == "" && ddlType.SelectedIndex == 0 && txtSubmittal.Text != "" && ddlstatus.SelectedIndex != 0 && dtDocumentDate.Text == "")
            //{
            //    ds = getdt.ActualDocuments_SelectBy_WorkpackageUID_Search(new Guid(DDLWorkPackage.SelectedValue), txtDocName.Text, ddlType.SelectedValue, txtSubmittal.Text, ddlstatus.SelectedValue, DocDate, 12);
            //}
            //else if (txtDocName.Text == "" && ddlType.SelectedIndex != 0 && txtSubmittal.Text == "" && ddlstatus.SelectedIndex != 0 && dtDocumentDate.Text == "")
            //{
            //    ds = getdt.ActualDocuments_SelectBy_WorkpackageUID_Search(new Guid(DDLWorkPackage.SelectedValue), txtDocName.Text, ddlType.SelectedValue, txtSubmittal.Text, ddlstatus.SelectedValue, DocDate, 13);
            //}
            //else if (txtDocName.Text != "" && ddlType.SelectedIndex != 0 && txtSubmittal.Text == "" && ddlstatus.SelectedIndex != 0 && dtDocumentDate.Text == "")
            //{
            //    ds = getdt.ActualDocuments_SelectBy_WorkpackageUID_Search(new Guid(DDLWorkPackage.SelectedValue), txtDocName.Text, ddlType.SelectedValue, txtSubmittal.Text, ddlstatus.SelectedValue, DocDate, 14);
            //}
            //else if (txtDocName.Text != "" && ddlType.SelectedIndex == 0 && txtSubmittal.Text != "" && ddlstatus.SelectedIndex != 0 && dtDocumentDate.Text == "")
            //{
            //    ds = getdt.ActualDocuments_SelectBy_WorkpackageUID_Search(new Guid(DDLWorkPackage.SelectedValue), txtDocName.Text, ddlType.SelectedValue, txtSubmittal.Text, ddlstatus.SelectedValue, DocDate, 15);
            //}
            //else if (txtDocName.Text == "" && ddlType.SelectedIndex != 0 && txtSubmittal.Text != "" && ddlstatus.SelectedIndex != 0 && dtDocumentDate.Text == "")
            //{
            //    ds = getdt.ActualDocuments_SelectBy_WorkpackageUID_Search(new Guid(DDLWorkPackage.SelectedValue), txtDocName.Text, ddlType.SelectedValue, txtSubmittal.Text, ddlstatus.SelectedValue, DocDate, 16);
            //}
            //else if (txtDocName.Text == "" && ddlType.SelectedIndex == 0 && txtSubmittal.Text == "" && ddlstatus.SelectedIndex == 0 && dtDocumentDate.Text != "")
            //{
            //    ds = getdt.ActualDocuments_SelectBy_WorkpackageUID_Search(new Guid(DDLWorkPackage.SelectedValue), txtDocName.Text, ddlType.SelectedValue, txtSubmittal.Text, ddlstatus.SelectedValue, DocDate, 17);
            //}
            //else if (txtDocName.Text != "" && ddlType.SelectedIndex == 0 && txtSubmittal.Text == "" && ddlstatus.SelectedIndex == 0 && dtDocumentDate.Text != "")
            //{
            //    ds = getdt.ActualDocuments_SelectBy_WorkpackageUID_Search(new Guid(DDLWorkPackage.SelectedValue), txtDocName.Text, ddlType.SelectedValue, txtSubmittal.Text, ddlstatus.SelectedValue, DocDate, 18);
            //}
            //else if (txtDocName.Text == "" && ddlType.SelectedIndex != 0 && txtSubmittal.Text == "" && ddlstatus.SelectedIndex == 0 && dtDocumentDate.Text != "")
            //{
            //    ds = getdt.ActualDocuments_SelectBy_WorkpackageUID_Search(new Guid(DDLWorkPackage.SelectedValue), txtDocName.Text, ddlType.SelectedValue, txtSubmittal.Text, ddlstatus.SelectedValue, DocDate, 19);
            //}
            //else if (txtDocName.Text == "" && ddlType.SelectedIndex == 0 && txtSubmittal.Text != "" && ddlstatus.SelectedIndex == 0 && dtDocumentDate.Text != "")
            //{
            //    ds = getdt.ActualDocuments_SelectBy_WorkpackageUID_Search(new Guid(DDLWorkPackage.SelectedValue), txtDocName.Text, ddlType.SelectedValue, txtSubmittal.Text, ddlstatus.SelectedValue, DocDate, 20);
            //}
            //else if (txtDocName.Text == "" && ddlType.SelectedIndex == 0 && txtSubmittal.Text == "" && ddlstatus.SelectedIndex != 0 && dtDocumentDate.Text != "")
            //{
            //    ds = getdt.ActualDocuments_SelectBy_WorkpackageUID_Search(new Guid(DDLWorkPackage.SelectedValue), txtDocName.Text, ddlType.SelectedValue, txtSubmittal.Text, ddlstatus.SelectedValue, DocDate, 21);
            //}
            //
            
            
            GrdDocuments.DataSource = ds;
            GrdDocuments.DataBind();
            
            ViewState["datatable"] = ds.Tables[0];

            //
            if (ds.Tables[0].Rows.Count > 10  && int.Parse(txtPageSize.Text) > 10)
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "Key", "<script>MakeStaticHeader('" + GrdDocuments.ClientID + "', 600, 1300 , 55 ,true); </script>", false);
            }
        }

        protected void GrdDocuments_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            //LinkButton lnkbtn;
            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (WebConfigurationManager.AppSettings["Domain"].ToString().ToUpper() == "NJSEI")
                    e.Row.Cells[5].Text = "NJSEI Reference #";
                else
                    e.Row.Cells[5].Text = "ONTB Reference #";
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataSet ds = getdt.getTop1_DocumentStatusSelect(new Guid(e.Row.Cells[0].Text));
                Label lblDocumentName = (Label)e.Row.FindControl("lblDocumentName");
                
                if (ds != null)
                {
                    if (ds.Tables[0].Rows[0]["DocumentType"].ToString() == "General Document")
                    {
                        e.Row.Cells[8].Text = "No History";
                    }
                    //
                    if (ds.Tables[0].Rows[0]["ActivityType"].ToString() != "" && ds.Tables[0].Rows[0]["TopVersion"].ToString() != "")
                    {
                        //e.Row.Cells[1].Text = ds.Tables[0].Rows[0]["TopVersion"].ToString();

                      
                        string newVersionFileName = Path.GetFileNameWithoutExtension(Server.MapPath(ds.Tables[0].Rows[0]["Doc_Path"].ToString()));
                        lblDocumentName.Text = newVersionFileName.Substring(0, (newVersionFileName.Length - 2)) + " [ Ver. " + ds.Tables[0].Rows[0]["TopVersion"].ToString() + " ]"; ;
                       
                    }
                }

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
                            //    e.Row.Cells[4].Text = "Client Approval";
                            //}
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

                }
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
                    filename = Path.GetFileName(ds.Tables[0].Rows[0]["Doc_FileName"].ToString());
                }
                else
                {
                    ds1 = getdt.ActualDocuments_SelectBy_ActualDocumentUID(new Guid(UID));
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        if (ds1.Tables[0].Rows[0]["ActualDocument_Path"].ToString() != "")
                        {
                            path = Server.MapPath(ds1.Tables[0].Rows[0]["ActualDocument_Path"].ToString());
                            filename = ds1.Tables[0].Rows[0]["ActualDocument_Name"].ToString() + ds1.Tables[0].Rows[0]["ActualDocument_Type"].ToString();
                        }
                    }
                }
                // added on  20/10/2020
                //ds.Clear();
                //ds = getdt.ActualDocuments_SelectBy_ActualDocumentUID(new Guid(UID));
                //if (ds.Tables[0].Rows.Count > 0)
                //{
                //    if (ds.Tables[0].Rows[0]["ActualDocument_Name"].ToString() != "")
                //    {
                //        filename = ds.Tables[0].Rows[0]["ActualDocument_Name"].ToString() + ds.Tables[0].Rows[0]["ActualDocument_Type"].ToString();
                //    }
                //}
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
                        // Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('File does not exists.');</script>");
                        //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "CLOSE", "<script language='javascript'>alert('File does not exists.');</script>",true);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "CLOSE", "alert('File does not exists.');", true);
                    }
                }
                catch (Exception ex)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Document not uploaded for this flow.Please Contact Document controller!');</script>");
                }
            }
        }

        protected void GrdDocuments_Sorting(object sender, GridViewSortEventArgs e)
        {

            DataTable dataTable = ViewState["datatable"] as DataTable;

            //if (dataTable != null)
            //{
            //    DataView dataView = new DataView(dataTable);
            //    dataView.Sort = e.SortExpression + " " + ConvertSortDirectionToSql(e.SortDirection);

            //    GrdDocuments.DataSource = dataView;
            //    GrdDocuments.DataBind();
            //}
            SetSortDirection(ViewState["SortDireaction"].ToString());
            if (dataTable != null)
            {
                //Sort the data.
                dataTable.DefaultView.Sort = e.SortExpression + " " + ViewState["_sortDirection"].ToString();
                GrdDocuments.DataSource = dataTable;
                GrdDocuments.DataBind();
                ViewState["SortDireaction"] = ViewState["_sortDirection"];
            }
        }

       

        //private string ConvertSortDirectionToSql(SortDirection sortDirection)
        //{
        //    string newSortDirection = String.Empty;

        //    switch (sortDirection)
        //    {
        //        case SortDirection.Ascending:
        //            newSortDirection = "ASC";
        //            break;

        //        case SortDirection.Descending:
        //            newSortDirection = "DESC";
        //            break;
        //    }

        //    return newSortDirection;
        //}

        protected void SetSortDirection(string sortDirection)
        {
            if (sortDirection == "ASC")
            {
                ViewState["_sortDirection"] = "DESC";
            }
            else
            {
                ViewState["_sortDirection"] = "ASC";
            }
        }

        protected void btnSubmit_Click1(object sender, EventArgs e)
        {
            try
            {
                //Session["searchedit"] = null;
                //Session["sSubmittal"] = txtSubmittal.Text;
                //Session["sDocName"] = txtDocName.Text;
                //Session["sDate"] = dtInDate.Text;
                //Session["sType"] = ddlType.SelectedValue;
                //Session["sStatus"] = ddlstatus.SelectedValue;
                if (DDlProject.SelectedIndex != 0 && DDLWorkPackage.SelectedIndex != 0)
                {
                    GrdDocuments.Visible = true;
                    UpdatePanel3.Update();
                    GrdDocuments.PageSize = int.Parse(txtPageSize.Text);
                    if (HiddenPaging.Value != "true")
                    {
                        GrdDocuments.PageIndex = 0;
                    }
                    BindDocuments();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnClearSearch_Click(object sender, EventArgs e)
        {
            Session["searchedit"] = null;
            Response.Redirect("default.aspx");
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

        protected void btnSubmitGD_Click(object sender, EventArgs e)
        {
            try
            {
                UpdatePanel5.Update();
                grdGeneralDocuments.PageSize = int.Parse(txtGDRecords.Text);
                if (HiddenPaging.Value != "true")
                {
                    grdGeneralDocuments.PageIndex = 0;
                }
                BindDocumentsGD();
            }
            catch(Exception ex)
            {

            }
        }

        protected void bthClearGD_Click(object sender, EventArgs e)
        {
            UpdatePanel5.Update();
            Session["searchedit"] = null;
            txtDocNameGD.Text = "";
            ddlDocType.SelectedIndex = 0;
            dtInDateGD.Text = "";
            dtInToDateGD.Text = "";
            dtDocToDateGD.Text = "";
            dtDocDateGD.Text = "";
            BindDocumentsGD();
        }

        private void BindDocumentsGD()
        {
            //if (Session["searchedit"] != null)
            //{
            //    grdGeneralDocuments.PageIndex = Convert.ToInt32(Session["PageIndex"]);
            //}


            //Session["searchedit"] = null;
            //Session["sSubmittal"] = txtSubmittal.Text;
            //Session["sDocName"] = txtDocName.Text;
            //Session["sDate"] = dtInDate.Text;
            //Session["sDocDate"] = dtDocDate.Text;
            //Session["sType"] = ddlType.SelectedValue;
            //Session["sStatus"] = ddlstatus.SelectedValue;
            //Session["sDateTo"] = dtInToDate.Text;
            //Session["sDocDateTo"] = dtDocToDate.Text;

            DateTime InDate = DateTime.Now;
            DateTime DocumentDate = DateTime.Now;
            DateTime InToDate = DateTime.Now;
            DateTime DocumentToDate = DateTime.Now;
            if (!string.IsNullOrEmpty(dtInDateGD.Text))
            {
                InDate = Convert.ToDateTime(getdt.ConvertDateFormat(dtInDateGD.Text));
            }
            if (string.IsNullOrEmpty(dtInToDateGD.Text))
            {
                InToDate = InDate;
            }

            if (!string.IsNullOrEmpty(dtDocDateGD.Text))
            {
                DocumentDate = Convert.ToDateTime(getdt.ConvertDateFormat(dtDocDateGD.Text));
            }
            if (string.IsNullOrEmpty(dtDocToDateGD.Text))
            {
                DocumentToDate = DocumentDate;
            }
            if (!string.IsNullOrEmpty(dtInToDateGD.Text))
            {
                InToDate = Convert.ToDateTime(getdt.ConvertDateFormat(dtInToDateGD.Text));
            }
            if (!string.IsNullOrEmpty(dtDocToDateGD.Text))
            {
                DocumentToDate = Convert.ToDateTime(getdt.ConvertDateFormat(dtDocToDateGD.Text));
            }


            // validations
            if (dtInDateGD.Text == "" && dtInToDateGD.Text != "")
            {
                // Page.ClientScript.RegisterStartupScript(Page.GetType(), "Alert", "<script language='javascript'>alert('Please enter Incoming Recv From Date');</script>");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "CLOSE", "alert('Please enter Incoming Recv From Date');", true);
                return;
            }
            else if (dtInDateGD.Text != "" && dtInToDateGD.Text == "")
            {
                //Page.ClientScript.RegisterStartupScript(Page.GetType(), "Alert", "<script language='javascript'>alert('Please enter Incoming Recv To Date');</script>");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "CLOSE", "alert('Please enter Incoming Recv To Date');", true);
                return;
            }
            else if (dtInDateGD.Text != "" && dtInToDateGD.Text != "")
            {
                if (InDate > InToDate)
                {
                    // Page.ClientScript.RegisterStartupScript(Page.GetType(), "Alert", "<script language='javascript'>alert('Incoming Recv From Date cannot be greater than Incoming Recv To Date');</script>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "CLOSE", "alert('Incoming Recv From Date cannot be greater than Incoming Recv To Date');", true);
                    return;
                }

            }

            if (dtDocDateGD.Text == "" && dtDocToDateGD.Text != "")
            {
                //Page.ClientScript.RegisterStartupScript(Page.GetType(), "Alert", "<script language='javascript'>alert('Please enter Document From Date');</script>");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "CLOSE", "alert('Please enter Document From Date');", true);

                return;
            }
            else if (dtDocDateGD.Text != "" && dtDocToDateGD.Text == "")
            {
                // Page.ClientScript.RegisterStartupScript(Page.GetType(), "Alert", "<script language='javascript'>alert('Please enter Document To Date');</script>");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "CLOSE", "alert('Please enter Document To Date');", true);
                return;
            }
            else if (dtDocDateGD.Text != "" && dtDocToDateGD.Text != "")
            {
                if (DocumentDate > DocumentToDate)
                {
                    //Page.ClientScript.RegisterStartupScript(Page.GetType(), "Alert", "<script language='javascript'>alert('Document From Date cannot be greater than Document To Date');</script>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "CLOSE", "alert('Document From Date cannot be greater than Document To Date');", true);
                    return;
                }
            }

            DataSet ds = GD.GeneralDocuments_Search(txtDocNameGD.Text, ddlDocType.SelectedValue, InDate, DocumentDate, InToDate, DocumentToDate, 4);
            if (dtDocDate.Text != "" && dtInDate.Text != "")
            {
                ds = new DataSet();
                ds = GD.GeneralDocuments_Search(txtDocNameGD.Text, ddlDocType.SelectedValue, InDate, DocumentDate, InToDate, DocumentToDate, 3);
            }
            else if (dtInDateGD.Text != "")
            {
                ds = new DataSet();
                ds = GD.GeneralDocuments_Search(txtDocNameGD.Text, ddlDocType.SelectedValue, InDate, DocumentDate, InToDate, DocumentToDate, 1);
            }
            else if (dtDocDateGD.Text != "")
            {
                ds = new DataSet();
                ds = GD.GeneralDocuments_Search(txtDocNameGD.Text, ddlDocType.SelectedValue, InDate, DocumentDate, InToDate, DocumentToDate, 2);
            }
            if (ds.Tables[0].Rows.Count > 0)
            {
                lblGDnos.Text = ds.Tables[0].Rows.Count.ToString();
            }
            else
            {
                lblGDnos.Text = "0";
            }
            grdGeneralDocuments.DataSource = ds;
            grdGeneralDocuments.DataBind();

            ViewState["datatable"] = ds.Tables[0];

            //
            if (ds.Tables[0].Rows.Count > 10 && int.Parse(txtPageSize.Text) > 10)
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "Key", "<script>MakeStaticHeaderNew('" + grdGeneralDocuments.ClientID + "', 600, 1300 , 55 ,true); </script>", false);
            }
        }

        protected void grdGeneralDocuments_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void grdGeneralDocuments_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string UID = e.CommandArgument.ToString();
            if (e.CommandName == "download")
            {
                string path = string.Empty;
                DataSet ds = GD.GeneralDocuments_SelectBy_GeneralDocumentUID(new Guid(UID));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    path = Server.MapPath(ds.Tables[0].Rows[0]["GeneralDocument_Path"].ToString());

                    string getExtension = System.IO.Path.GetExtension(path);
                    string outPath = path.Replace(getExtension, "") + "_download" + getExtension;
                    getdt.DecryptFile(path, outPath);
                    System.IO.FileInfo file = new System.IO.FileInfo(outPath);

                    if (file.Exists)
                    {

                        int Cnt = getdt.DocumentHistroy_InsertorUpdate(Guid.NewGuid(), new Guid(UID), new Guid(Session["UserUID"].ToString()), "Downloaded", "General Documents");
                        if (Cnt <= 0)
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code: DDH-01. there is a problem with updating histroy. Please contact system admin.');</script>");
                        }
                        Response.Clear();

                        Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);

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
                      //  Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('File does not exists.');</script>");
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "CLOSE", "alert('File does not exists.');", true);
                    }
                }
            }
        }

        protected void grdGeneralDocuments_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataTable dataTable = ViewState["datatable"] as DataTable;

            
            SetSortDirection(ViewState["SortDireaction"].ToString());
            if (dataTable != null)
            {
                //Sort the data.
                dataTable.DefaultView.Sort = e.SortExpression + " " + ViewState["_sortDirection"].ToString();
                grdGeneralDocuments.DataSource = dataTable;
                grdGeneralDocuments.DataBind();
                ViewState["SortDireaction"] = ViewState["_sortDirection"];
            }
        }

        protected void grdGeneralDocuments_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdGeneralDocuments.PageIndex = e.NewPageIndex;
            HiddenPaging.Value = "true";
          //  Session["PageIndex"] = GrdDocuments.PageIndex;
            BindDocumentsGD();
        }

        protected void DDLWorkPackage_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDLWorkPackage.SelectedValue != "-Select-")
            {
                if (Session["IsContractor"].ToString() == "Y")
                {
                    var IsProjFound = Constants.ProjectsForPhaseSearch.Where(r => r == DDlProject.SelectedItem.Text).FirstOrDefault();
                    if (!string.IsNullOrEmpty(IsProjFound))
                    {
                        ddlstatus.Attributes["style"] = "display: none;";
                        ddlPhase.Attributes["style"] = "";
                        status.Text = "Phase";
                        BindPhases(new Guid(DDLWorkPackage.SelectedValue));
                    }
                    else
                    {
                        ddlPhase.Attributes["style"] = "display: none;";
                        ddlstatus.Attributes["style"] = "";
                        status.Text = "Status";
                    }
                }
                else
                {
                    ddlPhase.Attributes["style"] = "display: none;";
                    ddlstatus.Attributes["style"] = "";
                    status.Text = "Status";
                }

                UpdatePanel2.Update();
                ddlstatus.DataSource = null;
                ddlstatus.DataBind();
                //
                ddlType.DataSource = null;
                ddlType.DataBind();
                lbldocNos.Text = "0";
                divMain.Visible = true;
                divGeneral.Visible = false;
                divWP.Visible = true;
                GrdDocuments.Visible = false;


                BindStatus();
                //bind phase

                BindType();
                
                if (Session["searchedit"] != null)
                {
                    ddlstatus.SelectedValue = Session["sStatus"].ToString();
                    ddlType.SelectedValue = Session["sType"].ToString();
                }
                Session["Project_Workpackage"] = DDlProject.SelectedValue + "_" + DDLWorkPackage.SelectedValue;
                // BindDocuments();
            }
            else
            {
                UpdatePanel2.Update();
                GrdDocuments.Visible = false;
                lbldocNos.Text = "0";
                ddlstatus.DataSource = null;
                ddlstatus.DataBind();
                //
                ddlType.DataSource = null;
                ddlType.DataBind();
            }
        }

        public string GetTaskHierarchy_By_DocumentUID(string DocumentUID)
        {
            return getdt.GetTaskHierarchy_By_DocumentUID(new Guid(DocumentUID));
        }
    }
}