using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagementTool._modal_pages
{
    public partial class add_submittalsforcategory : System.Web.UI.Page
    {
        DBGetData getdata = new DBGetData();
        TaskUpdate TKUpdate = new TaskUpdate();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Username"] == null)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
            }
            else
            {
                this.UnobtrusiveValidationMode = System.Web.UI.UnobtrusiveValidationMode.None;
                if (!IsPostBack)
                {
                    
                    BindProject();
                    BindWorkPackage();
                    //LoadDropDowns();
                    BindDocumentFlows();
                    DDLDocumentFlow_SelectedIndexChanged(sender, e);
                    if (Session["ActivityUID"] != null)
                    {
                        lblActivityName.Visible = true;
                        string ActivityType = Session["ActivityUID"].ToString().Split('*')[0];
                        if (ActivityType == "WkPkg")
                        {
                            lblActivityName.Text = getdata.getWorkPackageNameby_WorkPackageUID(new Guid(Session["ActivityUID"].ToString().Split('*')[1]));
                        }
                        else
                        {
                            lblActivityName.Text = getdata.getTaskNameby_TaskUID(new Guid(Session["ActivityUID"].ToString().Split('*')[1]));
                        }
                        
                        LinkActivity.Visible = false;
                    }
                    else
                    {
                        lblActivityName.Visible = false;
                        LinkActivity.Visible = true;
                    }
                    if (Request.QueryString["type"] == "add")
                    {
                        DataSet ds = getdata.WorkPackageCategory_Selectby_CategoryID(new Guid(Request.QueryString["CategoryID"]));
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            Bind_CategoriesforWorkPackage(new Guid(ds.Tables[0].Rows[0]["WorkPackageUID"].ToString()));
                            LinkActivity.HRef = "/_modal_pages/choose-activity.aspx?WorkUID=" + ds.Tables[0].Rows[0]["WorkPackageUID"].ToString();
                        }
                    }
                }
            }
        }

        private void BindProject()
        {
            DataTable ds = new DataTable();
            if (Session["TypeOfUser"].ToString() == "U" || Session["TypeOfUser"].ToString() =="MD" || Session["TypeOfUser"].ToString() == "VP")
            {

                ds = TKUpdate.GetProjects();
            }
            else if (Session["TypeOfUser"].ToString() == "PA")
            {

                //ds = TKUpdate.GetProjects_by_UserUID(new Guid(Session["UserUID"].ToString()));
                ds = TKUpdate.GetAssignedProjects_by_UserUID(new Guid(Session["UserUID"].ToString()));
            }
            else
            {

                //ds = TKUpdate.GetProjects();
                ds = TKUpdate.GetAssignedProjects_by_UserUID(new Guid(Session["UserUID"].ToString()));
            }
            DDlProject.DataTextField = "ProjectName";
            DDlProject.DataValueField = "ProjectUID";
            DDlProject.DataSource = ds;
            DDlProject.DataBind();

            if (Request.QueryString["PrjUID"] != null)
            {
                DDlProject.SelectedValue = Request.QueryString["PrjUID"].ToString();
            }

        }

        private void BindWorkPackage()
        {
            DataSet ds = new DataSet();
            if (Session["TypeOfUser"].ToString() == "U" || Session["TypeOfUser"].ToString() == "MD" || Session["TypeOfUser"].ToString() == "VP")
            {
                ds = getdata.GetWorkPackages_By_ProjectUID(new Guid(DDlProject.SelectedValue));
            }
            else if (Session["TypeOfUser"].ToString() == "PA")
            {
                ds = getdata.GetWorkPackages_ForUser_by_ProjectUID(new Guid(Session["UserUID"].ToString()), new Guid(DDlProject.SelectedValue));
            }
            else
            {
                ds = getdata.GetWorkPackages_ForUser_by_ProjectUID(new Guid(Session["UserUID"].ToString()), new Guid(DDlProject.SelectedValue));
            }

            DDLWorkPackage.DataTextField = "Name";
            DDLWorkPackage.DataValueField = "WorkPackageUID";
            DDLWorkPackage.DataSource = ds;
            DDLWorkPackage.DataBind();

            if (Request.QueryString["WorkPackageUID"] != null)
            {
                DDLWorkPackage.SelectedValue = Request.QueryString["WorkPackageUID"].ToString();
            }
        }
        private void BindDocumentFlows()
        {
            DataSet ds = getdata.GetDocumentFlows();
            DDLDocumentFlow.DataTextField = "Flow_Name";
            DDLDocumentFlow.DataValueField = "FlowMasterUID";
            DDLDocumentFlow.DataSource = ds;
            DDLDocumentFlow.DataBind();
            DDLDocumentFlow.Items.Insert(0, "--Select--");
            DDLDocumentFlow.SelectedIndex = 1;
        }

        private void LoadDropDowns()
        {
            try
            {
                if (Request.QueryString["PrjUID"] != null)
                {
                    DDlProject.SelectedValue = Request.QueryString["PrjUID"].ToString();
                }


                DataSet ds = new DataSet();
                if (Session["TypeOfUser"].ToString() == "U" || Session["TypeOfUser"].ToString() =="MD" || Session["TypeOfUser"].ToString() == "VP")
                {
                    ds = getdata.getAllUsers();
                }
                else if (Session["TypeOfUser"].ToString() == "PA")
                {
                    //ds = getdata.getUsers_by_ProjectUnder(new Guid(DDlProject.SelectedValue));
                    ds = getdata.GetUsers_under_ProjectUID(new Guid(DDlProject.SelectedValue));
                }
                else
                {
                    ds = getdata.GetUsers_under_ProjectUID(new Guid(DDlProject.SelectedValue));
                }

                //ddlSubmissionUSer.DataSource = getdata.getUsers("S");
                ddlSubmissionUSer.DataSource = ds;
                ddlSubmissionUSer.DataTextField = "UserName";
                ddlSubmissionUSer.DataValueField = "UserUID";
                ddlSubmissionUSer.DataBind();
                ddlSubmissionUSer.Items.Insert(0, new ListItem("--Select--", ""));
                //
                //ddlQualityEngg.DataSource = getdata.getUsers("C");
                ddlQualityEngg.DataSource = ds;
                ddlQualityEngg.DataTextField = "UserName";
                ddlQualityEngg.DataValueField = "UserUID";
                ddlQualityEngg.DataBind();
                ddlQualityEngg.Items.Insert(0, new ListItem("--Select--", ""));
                //
                //ddlReviewer.DataSource = getdata.getUsers("R");
                ddlReviewer.DataSource = ds;
                ddlReviewer.DataTextField = "UserName";
                ddlReviewer.DataValueField = "UserUID";
                ddlReviewer.DataBind();
                ddlReviewer.Items.Insert(0, new ListItem("--Select--", ""));

                //
                //ddlReviewer_B.DataSource = getdata.getUsers("R");
                ddlReviewer_B.DataSource = ds;
                ddlReviewer_B.DataTextField = "UserName";
                ddlReviewer_B.DataValueField = "UserUID";
                ddlReviewer_B.DataBind();
                ddlReviewer_B.Items.Insert(0, new ListItem("--Select--", ""));
                //
                //ddlApproval.DataSource = getdata.getUsers("A");
                ddlApproval.DataSource = ds;
                ddlApproval.DataTextField = "UserName";
                ddlApproval.DataValueField = "UserUID";
                ddlApproval.DataBind();
                ddlApproval.Items.Insert(0, new ListItem("--Select--", ""));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Bind_CategoriesforWorkPackage(Guid WorkPackageUID)
        {
            DataSet ds = getdata.WorkPackageCategory_Selectby_WorkPackageUID(WorkPackageUID);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DDLDocumentCategory.DataTextField = "WorkPackageCategory_Name";
                DDLDocumentCategory.DataValueField = "WorkPackageCategory_UID";
                DDLDocumentCategory.DataSource = ds;
                DDLDocumentCategory.DataBind();
                DDLDocumentCategory.SelectedValue = new Guid(Request.QueryString["CategoryID"]).ToString();
            }
           
        }
        protected void DDLDocumentFlow_SelectedIndexChanged(object sender, EventArgs e)
        {
            DocumentFlowChanged();
        }

        public void DocumentFlowChanged()
        {
            if (DDLDocumentFlow.SelectedValue != "")
            {
                DataSet ds = getdata.GetDocumentFlows_by_UID(new Guid(DDLDocumentFlow.SelectedValue));
                if (ds.Tables[0].Rows.Count > 0)
                {

                    //dFlow.Visible = true;
                    //Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Success-4');</script>");
                    LoadDropDowns();
                    //Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Success-5');</script>");
                    if (ds.Tables[0].Rows[0]["Steps_Count"].ToString() == "1")
                    {
                        lblStep1Display.Text = ds.Tables[0].Rows[0]["FlowStep1_DisplayName"].ToString();
                        lblStep1Date.Text = ds.Tables[0].Rows[0]["FlowStep1_DisplayName"].ToString() + " Target Date";
                        S1Display.Visible = true;
                        S1Date.Visible = true;
                        S2Display.Visible = false;
                        S2Date.Visible = false;
                        S3Display.Visible = false;
                        S3Date.Visible = false;
                        S4Display.Visible = false;
                        S4Date.Visible = false;
                        S5Display.Visible = false;
                        S5Date.Visible = false;
                    }
                    else if (ds.Tables[0].Rows[0]["Steps_Count"].ToString() == "2")
                    {
                        lblStep1Display.Text = ds.Tables[0].Rows[0]["FlowStep1_DisplayName"].ToString();
                        lblStep1Date.Text = ds.Tables[0].Rows[0]["FlowStep1_DisplayName"].ToString() + " Target Date";
                        lblStep2Display.Text = ds.Tables[0].Rows[0]["FlowStep2_DisplayName"].ToString();
                        lblStep2Date.Text = ds.Tables[0].Rows[0]["FlowStep2_DisplayName"].ToString() + " Target Date";
                        S1Display.Visible = true;
                        S1Date.Visible = true;
                        S2Display.Visible = true;
                        S2Date.Visible = true;
                        S3Display.Visible = false;
                        S3Date.Visible = false;
                        S4Display.Visible = false;
                        S4Date.Visible = false;
                        S5Display.Visible = false;
                        S5Date.Visible = false;
                    }
                    else if (ds.Tables[0].Rows[0]["Steps_Count"].ToString() == "3")
                    {
                        //Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Success-6');</script>");
                        lblStep1Display.Text = ds.Tables[0].Rows[0]["FlowStep1_DisplayName"].ToString();
                        lblStep1Date.Text = ds.Tables[0].Rows[0]["FlowStep1_DisplayName"].ToString() + " Target Date";
                        lblStep2Display.Text = ds.Tables[0].Rows[0]["FlowStep2_DisplayName"].ToString();
                        lblStep2Date.Text = ds.Tables[0].Rows[0]["FlowStep2_DisplayName"].ToString() + " Target Date";
                        lblStep3Display.Text = ds.Tables[0].Rows[0]["FlowStep3_DisplayName"].ToString();
                        lblStep3Date.Text = ds.Tables[0].Rows[0]["FlowStep3_DisplayName"].ToString() + " Target Date";
                        S1Display.Visible = true;
                        S1Date.Visible = true;
                        S2Display.Visible = true;
                        S2Date.Visible = true;
                        S3Display.Visible = true;
                        S3Date.Visible = true;
                        S4Display.Visible = false;
                        S4Date.Visible = false;
                        S5Display.Visible = false;
                        S5Date.Visible = false;
                    }
                    else if (ds.Tables[0].Rows[0]["Steps_Count"].ToString() == "4")
                    {
                        lblStep1Display.Text = ds.Tables[0].Rows[0]["FlowStep1_DisplayName"].ToString();
                        lblStep1Date.Text = ds.Tables[0].Rows[0]["FlowStep1_DisplayName"].ToString() + " Target Date";
                        lblStep2Display.Text = ds.Tables[0].Rows[0]["FlowStep2_DisplayName"].ToString();
                        lblStep2Date.Text = ds.Tables[0].Rows[0]["FlowStep2_DisplayName"].ToString() + " Target Date";
                        lblStep3Display.Text = ds.Tables[0].Rows[0]["FlowStep3_DisplayName"].ToString();
                        lblStep3Date.Text = ds.Tables[0].Rows[0]["FlowStep3_DisplayName"].ToString() + " Target Date";
                        lblStep4Display.Text = ds.Tables[0].Rows[0]["FlowStep4_DisplayName"].ToString();
                        lblStep4Date.Text = ds.Tables[0].Rows[0]["FlowStep4_DisplayName"].ToString() + " Target Date";
                        S1Display.Visible = true;
                        S1Date.Visible = true;
                        S2Display.Visible = true;
                        S2Date.Visible = true;
                        S3Display.Visible = true;
                        S3Date.Visible = true;
                        S4Display.Visible = true;
                        S4Date.Visible = true;
                        S5Display.Visible = false;
                        S5Date.Visible = false;
                    }
                    else
                    {
                        lblStep1Display.Text = ds.Tables[0].Rows[0]["FlowStep1_DisplayName"].ToString();
                        lblStep1Date.Text = ds.Tables[0].Rows[0]["FlowStep1_DisplayName"].ToString() + " Target Date";
                        lblStep2Display.Text = ds.Tables[0].Rows[0]["FlowStep2_DisplayName"].ToString();
                        lblStep2Date.Text = ds.Tables[0].Rows[0]["FlowStep2_DisplayName"].ToString() + " Target Date";
                        lblStep3Display.Text = ds.Tables[0].Rows[0]["FlowStep3_DisplayName"].ToString();
                        lblStep3Date.Text = ds.Tables[0].Rows[0]["FlowStep3_DisplayName"].ToString() + " Target Date";
                        lblStep4Display.Text = ds.Tables[0].Rows[0]["FlowStep4_DisplayName"].ToString();
                        lblStep4Date.Text = ds.Tables[0].Rows[0]["FlowStep4_DisplayName"].ToString() + " Target Date";
                        lblStep5Display.Text = ds.Tables[0].Rows[0]["FlowStep5_DisplayName"].ToString();
                        lblStep5Date.Text = ds.Tables[0].Rows[0]["FlowStep5_DisplayName"].ToString() + " Target Date";
                    }
                    string sDate1 = "";
                    DateTime CDate1 = DateTime.Now;


                    sDate1 = DateTime.Now.ToString("dd/MM/yyyy");
                    //sDate1 = sDate1.Split('/')[1] + "/" + sDate1.Split('/')[0] + "/" + sDate1.Split('/')[2];
                    sDate1 = getdata.ConvertDateFormat(sDate1);
                    CDate1 = Convert.ToDateTime(sDate1);

                    //dtSubTargetDate.Text = CDate1.AddDays(Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep1_Duration"].ToString())).ToString("yyyy-MM-dd");
                    //dtSubTargetDate.Text = CDate1.ToString("dd/MM/yyyy");
                    //dtQualTargetDate.Text = CDate1.AddDays(Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep2_Duration"].ToString())).ToString("dd/MM/yyyy");

                    //dtRev_B_TargetDate.Text = CDate1.AddDays(Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep3_Duration"].ToString())).ToString("dd/MM/yyyy");

                    //string dt = Convert.ToDateTime((dtSubTargetDate.FindControl("txtDate") as TextBox).Text).ToString("dd/MM/yyyy");
                    //string dt = CDate1;
                    if (ds.Tables[0].Rows[0]["Steps_Count"].ToString() == "1")
                    {
                        dtSubTargetDate.Text = CDate1.AddDays(Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep1_Duration"].ToString())).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    }
                    else if (ds.Tables[0].Rows[0]["Steps_Count"].ToString() == "2")
                    {
                        dtSubTargetDate.Text = CDate1.AddDays(Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep1_Duration"].ToString())).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                        dtQualTargetDate.Text = CDate1.AddDays(Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep2_Duration"].ToString())).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    }
                    else if (ds.Tables[0].Rows[0]["Steps_Count"].ToString() == "3")
                    {
                        dtSubTargetDate.Text = CDate1.AddDays(Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep1_Duration"].ToString())).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                        dtQualTargetDate.Text = CDate1.AddDays(Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep2_Duration"].ToString())).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                        dtRev_B_TargetDate.Text = CDate1.AddDays(Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep3_Duration"].ToString())).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                        //Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Success-8');</script>");
                    }
                    else if (ds.Tables[0].Rows[0]["Steps_Count"].ToString() == "4")
                    {
                        dtSubTargetDate.Text = CDate1.AddDays(Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep1_Duration"].ToString())).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                        dtQualTargetDate.Text = CDate1.AddDays(Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep2_Duration"].ToString())).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                        dtRev_B_TargetDate.Text = CDate1.AddDays(Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep3_Duration"].ToString())).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                        dtRevTargetDate.Text = CDate1.AddDays(Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep4_Duration"].ToString())).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        dtSubTargetDate.Text = CDate1.AddDays(Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep1_Duration"].ToString())).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                        dtQualTargetDate.Text = CDate1.AddDays(Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep2_Duration"].ToString())).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                        dtRev_B_TargetDate.Text = CDate1.AddDays(Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep3_Duration"].ToString())).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                        dtRevTargetDate.Text = CDate1.AddDays(Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep4_Duration"].ToString())).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                        dtAppTargetDate.Text = CDate1.AddDays(Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep5_Duration"].ToString())).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    }

                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    if (DDLDocumentCategory.SelectedValue == "")
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Please select Document Category.If Category not exists, add it.');</script>");
                    }
                    else if (Session["ActivityUID"] == null)
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Please choose an Activity to Link');</script>");
                    }
                    else
                    {
                        loading.Visible = true;
                        Guid sDocumentUID = new Guid();
                        if (Request.QueryString["DocID"] != null)
                        {
                            sDocumentUID = new Guid(Request.QueryString["DocID"]);
                        }
                        else
                        {
                            sDocumentUID = Guid.NewGuid();
                        }

                        Guid TaskUID = Guid.Empty;
                        string TaskName = string.Empty;
                        string Subject = string.Empty;
                        Guid projectId = Guid.Empty;
                        Guid workpackageid = Guid.Empty;

                        if (Request.QueryString["CategoryID"] != null)
                        {
                            DataSet ds = getdata.WorkPackageCategory_Selectby_CategoryID(new Guid(Request.QueryString["CategoryID"]));
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                Subject = "Submittal added for category " + ds.Tables[0].Rows[0]["WorkPackageCategory_Name"].ToString();
                                projectId = new Guid(getdata.getProjectUIDby_WorkpackgeUID(new Guid(ds.Tables[0].Rows[0]["WorkPackageUID"].ToString())));
                                workpackageid = new Guid(ds.Tables[0].Rows[0]["WorkPackageUID"].ToString());
                            }
                            TaskUID = new Guid(Session["ActivityUID"].ToString().Split('*')[1]);
                        }

                        string sDate1 = "", sDate2 = "", sDate3 = "", sDate4 = "", sDate5 = "", DocPath = "", DocStartString = "", CoverPagePath = "";
                        DateTime CDate1 = DateTime.Now, CDate2 = DateTime.Now, CDate3 = DateTime.Now, CDate4 = DateTime.Now, CDate5 = DateTime.Now, DocStartDate = DateTime.Now;
                        string sRemarks = txtremarks.Text;
                        string SubDocTypeMaster = "";
                        int estimateddocs = int.Parse(txtestdocs.Text);
                        int result = 0;
                        string IsSync = "N";
                        if (ddlSubDocType.SelectedIndex != 0)
                        {
                            SubDocTypeMaster = ddlSubDocType.SelectedItem.ToString();
                        }
                        // for synch
                        if (chkSync.Checked)
                        {
                            IsSync = "Y";
                        }
                        //
                        DataSet dsFlow = getdata.GetDocumentFlows_by_UID(new Guid(DDLDocumentFlow.SelectedValue));
                        if (dsFlow.Tables[0].Rows.Count > 0)
                        {
                            DocStartString = DateTime.Now.ToString("dd/MM/yyyy");
                            //DocStartString = DocStartString.Split('/')[1] + "/" + DocStartString.Split('/')[0] + "/" + DocStartString.Split('/')[2];
                            DocStartString = getdata.ConvertDateFormat(DocStartString);
                            DocStartDate = Convert.ToDateTime(DocStartString);


                            if (dsFlow.Tables[0].Rows[0]["Steps_Count"].ToString() == "1")
                            {
                                //
                                sDate1 = dtSubTargetDate.Text != "" ? dtSubTargetDate.Text : CDate1.ToString("dd/MM/yyyy");
                                //sDate1 = sDate1.Split('/')[1] + "/" + sDate1.Split('/')[0] + "/" + sDate1.Split('/')[2];
                                sDate1 = getdata.ConvertDateFormat(sDate1);
                                CDate1 = Convert.ToDateTime(sDate1);
                                result = getdata.DoumentMaster_Insert_or_Update_Flow_0(sDocumentUID, workpackageid, projectId, TaskUID, txtDocumentName.Text, new Guid(DDLDocumentCategory.SelectedValue),
                                "", "Submittle Document", 0.0, new Guid(DDLDocumentFlow.SelectedValue), DocStartDate, new Guid(ddlSubmissionUSer.SelectedValue), CDate1,estimateddocs,sRemarks,SubDocTypeMaster, IsSync);

                            }
                            else if (dsFlow.Tables[0].Rows[0]["Steps_Count"].ToString() == "2")
                            {
                                //
                                sDate1 = dtSubTargetDate.Text != "" ? dtSubTargetDate.Text : CDate1.ToString("dd/MM/yyyy");
                                //sDate1 = sDate1.Split('/')[1] + "/" + sDate1.Split('/')[0] + "/" + sDate1.Split('/')[2];
                                sDate1 = getdata.ConvertDateFormat(sDate1);
                                CDate1 = Convert.ToDateTime(sDate1);
                                //
                                sDate2 = dtQualTargetDate.Text != "" ? dtQualTargetDate.Text : CDate2.ToString("dd/MM/yyyy");
                                //sDate2 = sDate2.Split('/')[1] + "/" + sDate2.Split('/')[0] + "/" + sDate2.Split('/')[2];
                                sDate2 = getdata.ConvertDateFormat(sDate2);
                                CDate2 = Convert.ToDateTime(sDate2);
                                result = getdata.DoumentMaster_Insert_or_Update_Flow_1(sDocumentUID, workpackageid, projectId, TaskUID, txtDocumentName.Text, new Guid(DDLDocumentCategory.SelectedValue),
                                "", "Submittle Document", 0.0, new Guid(DDLDocumentFlow.SelectedValue), DocStartDate, new Guid(ddlSubmissionUSer.SelectedValue), CDate1,
                                new Guid(ddlQualityEngg.SelectedValue), CDate2,estimateddocs, sRemarks, SubDocTypeMaster, IsSync);
                            }
                            else if (dsFlow.Tables[0].Rows[0]["Steps_Count"].ToString() == "3")
                            {
                                //
                                sDate1 = dtSubTargetDate.Text != "" ? dtSubTargetDate.Text : CDate1.ToString("dd/MM/yyyy");
                                //sDate1 = sDate1.Split('/')[1] + "/" + sDate1.Split('/')[0] + "/" + sDate1.Split('/')[2];
                                sDate1 = getdata.ConvertDateFormat(sDate1);
                                CDate1 = Convert.ToDateTime(sDate1);
                                //

                                sDate2 = dtQualTargetDate.Text != "" ? dtQualTargetDate.Text : CDate2.ToString("dd/MM/yyyy");
                                //sDate2 = sDate2.Split('/')[1] + "/" + sDate2.Split('/')[0] + "/" + sDate2.Split('/')[2];
                                sDate2 = getdata.ConvertDateFormat(sDate2);
                                CDate2 = Convert.ToDateTime(sDate2);

                                sDate3 = dtRev_B_TargetDate.Text != "" ? dtRev_B_TargetDate.Text : CDate3.ToString("dd/MM/yyyy");
                                //sDate3 = sDate3.Split('/')[1] + "/" + sDate3.Split('/')[0] + "/" + sDate3.Split('/')[2];
                                sDate3 = getdata.ConvertDateFormat(sDate3);
                                CDate3 = Convert.ToDateTime(sDate3);

                                result = getdata.DoumentMaster_Insert_or_Update_Flow_2(sDocumentUID, workpackageid, projectId, TaskUID, txtDocumentName.Text, new Guid(DDLDocumentCategory.SelectedValue),
                                "", "Submittle Document", 0.0, new Guid(DDLDocumentFlow.SelectedValue), DocStartDate, new Guid(ddlSubmissionUSer.SelectedValue), CDate1,
                                new Guid(ddlQualityEngg.SelectedValue), CDate2, new Guid(ddlReviewer_B.SelectedValue), CDate3, estimateddocs, sRemarks, SubDocTypeMaster, IsSync);
                            }
                            else if (dsFlow.Tables[0].Rows[0]["Steps_Count"].ToString() == "4")
                            {
                                //
                                sDate1 = dtSubTargetDate.Text != "" ? dtSubTargetDate.Text : CDate1.ToString("dd/MM/yyyy");
                                //sDate1 = sDate1.Split('/')[1] + "/" + sDate1.Split('/')[0] + "/" + sDate1.Split('/')[2];
                                sDate1 = getdata.ConvertDateFormat(sDate1);
                                CDate1 = Convert.ToDateTime(sDate1);

                                sDate2 = dtQualTargetDate.Text != "" ? dtQualTargetDate.Text : CDate2.ToString("dd/MM/yyyy");
                                //sDate2 = sDate2.Split('/')[1] + "/" + sDate2.Split('/')[0] + "/" + sDate2.Split('/')[2];
                                sDate2 = getdata.ConvertDateFormat(sDate2);
                                CDate2 = Convert.ToDateTime(sDate2);

                                //
                                sDate3 = dtRev_B_TargetDate.Text != "" ? dtRev_B_TargetDate.Text : CDate3.ToString("dd/MM/yyyy");
                                //sDate3 = sDate3.Split('/')[1] + "/" + sDate3.Split('/')[0] + "/" + sDate3.Split('/')[2];
                                sDate3 = getdata.ConvertDateFormat(sDate3);
                                CDate3 = Convert.ToDateTime(sDate3);

                                //
                                sDate4 = dtRevTargetDate.Text != "" ? dtRevTargetDate.Text : CDate4.ToString("dd/MM/yyyy");
                                //sDate4 = sDate4.Split('/')[1] + "/" + sDate4.Split('/')[0] + "/" + sDate4.Split('/')[2];
                                sDate4 = getdata.ConvertDateFormat(sDate4);
                                CDate4 = Convert.ToDateTime(sDate4);

                                result = getdata.DoumentMaster_Insert_or_Update_Flow_3(sDocumentUID, workpackageid, projectId, TaskUID, txtDocumentName.Text, new Guid(DDLDocumentCategory.SelectedValue),
                                "", "Submittle Document", 0.0, new Guid(DDLDocumentFlow.SelectedValue), DocStartDate, new Guid(ddlSubmissionUSer.SelectedValue), CDate1,
                                new Guid(ddlQualityEngg.SelectedValue), CDate2, new Guid(ddlReviewer_B.SelectedValue), CDate3, new Guid(ddlReviewer.SelectedValue), CDate4, estimateddocs, sRemarks, SubDocTypeMaster, IsSync);
                            }
                            else
                            {

                                sDate1 = dtSubTargetDate.Text != "" ? dtSubTargetDate.Text : CDate1.ToString("dd/MM/yyyy");
                                //sDate1 = sDate1.Split('/')[1] + "/" + sDate1.Split('/')[0] + "/" + sDate1.Split('/')[2];
                                sDate1 = getdata.ConvertDateFormat(sDate1);
                                CDate1 = Convert.ToDateTime(sDate1);
                                //

                                sDate2 = dtQualTargetDate.Text != "" ? dtQualTargetDate.Text : CDate2.ToString("dd/MM/yyyy");
                                //sDate2 = sDate2.Split('/')[1] + "/" + sDate2.Split('/')[0] + "/" + sDate2.Split('/')[2];
                                sDate2 = getdata.ConvertDateFormat(sDate2);
                                CDate2 = Convert.ToDateTime(sDate2);

                                sDate3 = dtRev_B_TargetDate.Text != "" ? dtRev_B_TargetDate.Text : CDate3.ToString("dd/MM/yyyy");
                                //sDate3 = sDate3.Split('/')[1] + "/" + sDate3.Split('/')[0] + "/" + sDate3.Split('/')[2];
                                sDate3 = getdata.ConvertDateFormat(sDate3);
                                CDate3 = Convert.ToDateTime(sDate3);


                                sDate4 = dtRevTargetDate.Text != "" ? dtRevTargetDate.Text : CDate4.ToString("dd/MM/yyyy");
                                //sDate4 = sDate4.Split('/')[1] + "/" + sDate4.Split('/')[0] + "/" + sDate4.Split('/')[2];
                                sDate4 = getdata.ConvertDateFormat(sDate4);
                                CDate4 = Convert.ToDateTime(sDate4);
                                //
                                sDate5 = dtAppTargetDate.Text != "" ? dtAppTargetDate.Text : CDate4.ToString("dd/MM/yyyy");
                                //sDate5 = sDate5.Split('/')[1] + "/" + sDate5.Split('/')[0] + "/" + sDate5.Split('/')[2];
                                sDate5 = getdata.ConvertDateFormat(sDate5);
                                CDate5 = Convert.ToDateTime(sDate5);

                                result = getdata.DoumentMaster_Insert_or_Update_Flow_4(sDocumentUID, workpackageid, projectId, TaskUID, txtDocumentName.Text, new Guid(DDLDocumentCategory.SelectedValue),
                                "", "Submittle Document", 0.0, new Guid(DDLDocumentFlow.SelectedValue), DocStartDate, new Guid(ddlSubmissionUSer.SelectedValue), CDate1,
                                new Guid(ddlQualityEngg.SelectedValue), CDate2, new Guid(ddlReviewer_B.SelectedValue), CDate3, new Guid(ddlReviewer.SelectedValue), CDate4, new Guid(ddlApproval.SelectedValue), CDate5, estimateddocs, sRemarks, SubDocTypeMaster, IsSync);
                            }

                            //result = getdata.DoumentMaster_Insert_or_Update_Flow_2(sDocumentUID, workpackageid, projectId, TaskUID, txtDocumentName.Text, new Guid(DDLDocumentCategory.SelectedValue),
                            //"", "Submittle Document", 0.0, new Guid(DDLDocumentFlow.SelectedValue), DocStartDate, new Guid(ddlSubmissionUSer.SelectedValue), CDate1,
                            //new Guid(ddlQualityEngg.SelectedValue), CDate2, new Guid(ddlReviewer_B.SelectedValue), CDate3);

                            //if (dsFlow.Tables[0].Rows[0]["Steps_Count"].ToString() == "2")
                            //{
                            //    //
                            //    sDate1 = (dtSubTargetDate.FindControl("txtDate") as TextBox).Text;
                            //    sDate1 = sDate1.Split('/')[1] + "/" + sDate1.Split('/')[0] + "/" + sDate1.Split('/')[2];
                            //    CDate1 = Convert.ToDateTime(sDate1);
                            //    //
                            //    sDate2 = (dtQualTargetDate.FindControl("txtDate") as TextBox).Text;
                            //    sDate2 = sDate2.Split('/')[1] + "/" + sDate2.Split('/')[0] + "/" + sDate2.Split('/')[2];
                            //    CDate2 = Convert.ToDateTime(sDate2);
                            //    result = getdata.DoumentMaster_Insert_or_Update_Flow_1(sDocumentUID, workpackageid, projectId, TaskUID, txtDocumentName.Text, new Guid(DDLDocumentCategory.SelectedValue),
                            //    "", "Submittle Document", 0.0, new Guid(DDLDocumentFlow.SelectedValue), DocStartDate, new Guid(ddlSubmissionUSer.SelectedValue), CDate1,
                            //    new Guid(ddlQualityEngg.SelectedValue), CDate2);
                            //}
                            //else if (dsFlow.Tables[0].Rows[0]["Steps_Count"].ToString() == "3")
                            //{
                            //    //
                            //    sDate1 = (dtSubTargetDate.FindControl("txtDate") as TextBox).Text;
                            //    sDate1 = sDate1.Split('/')[1] + "/" + sDate1.Split('/')[0] + "/" + sDate1.Split('/')[2];
                            //    CDate1 = Convert.ToDateTime(sDate1);
                            //    //

                            //    sDate2 = (dtQualTargetDate.FindControl("txtDate") as TextBox).Text;
                            //    sDate2 = sDate2.Split('/')[1] + "/" + sDate2.Split('/')[0] + "/" + sDate2.Split('/')[2];
                            //    CDate2 = Convert.ToDateTime(sDate2);

                            //    sDate3 = (dtRev_B_TargetDate.FindControl("txtDate") as TextBox).Text;
                            //    sDate3 = sDate3.Split('/')[1] + "/" + sDate3.Split('/')[0] + "/" + sDate3.Split('/')[2];
                            //    CDate3 = Convert.ToDateTime(sDate3);

                            //    result = getdata.DoumentMaster_Insert_or_Update_Flow_2(sDocumentUID, workpackageid, projectId, TaskUID, txtDocumentName.Text, new Guid(DDLDocumentCategory.SelectedValue),
                            //    "", "Submittle Document", 0.0, new Guid(DDLDocumentFlow.SelectedValue), DocStartDate, new Guid(ddlSubmissionUSer.SelectedValue), CDate1,
                            //    new Guid(ddlQualityEngg.SelectedValue), CDate2, new Guid(ddlReviewer_B.SelectedValue), CDate3);
                            //}
                            //else if (dsFlow.Tables[0].Rows[0]["Steps_Count"].ToString() == "4")
                            //{
                            //    //
                            //    sDate1 = (dtSubTargetDate.FindControl("txtDate") as TextBox).Text;
                            //    sDate1 = sDate1.Split('/')[1] + "/" + sDate1.Split('/')[0] + "/" + sDate1.Split('/')[2];
                            //    CDate1 = Convert.ToDateTime(sDate1);

                            //    sDate2 = (dtQualTargetDate.FindControl("txtDate") as TextBox).Text;
                            //    sDate2 = sDate2.Split('/')[1] + "/" + sDate2.Split('/')[0] + "/" + sDate2.Split('/')[2];
                            //    CDate2 = Convert.ToDateTime(sDate2);

                            //    //
                            //    sDate3 = (dtRev_B_TargetDate.FindControl("txtDate") as TextBox).Text;
                            //    sDate3 = sDate3.Split('/')[1] + "/" + sDate3.Split('/')[0] + "/" + sDate3.Split('/')[2];
                            //    CDate3 = Convert.ToDateTime(sDate3);

                            //    //
                            //    sDate4 = (dtRevTargetDate.FindControl("txtDate") as TextBox).Text;
                            //    sDate4 = sDate4.Split('/')[1] + "/" + sDate4.Split('/')[0] + "/" + sDate4.Split('/')[2];
                            //    CDate4 = Convert.ToDateTime(sDate4);

                            //    result = getdata.DoumentMaster_Insert_or_Update_Flow_3(sDocumentUID, workpackageid, projectId, TaskUID, txtDocumentName.Text, new Guid(DDLDocumentCategory.SelectedValue),
                            //    "", "Submittle Document", 0.0, new Guid(DDLDocumentFlow.SelectedValue), DocStartDate, new Guid(ddlSubmissionUSer.SelectedValue), CDate1,
                            //    new Guid(ddlQualityEngg.SelectedValue), CDate2, new Guid(ddlReviewer_B.SelectedValue), CDate3, new Guid(ddlReviewer.SelectedValue), CDate4);
                            //}
                            //else
                            //{

                            //    sDate1 = (dtSubTargetDate.FindControl("txtDate") as TextBox).Text;
                            //    sDate1 = sDate1.Split('/')[1] + "/" + sDate1.Split('/')[0] + "/" + sDate1.Split('/')[2];
                            //    CDate1 = Convert.ToDateTime(sDate1);
                            //    //

                            //    sDate2 = (dtQualTargetDate.FindControl("txtDate") as TextBox).Text;
                            //    sDate2 = sDate2.Split('/')[1] + "/" + sDate2.Split('/')[0] + "/" + sDate2.Split('/')[2];
                            //    CDate2 = Convert.ToDateTime(sDate2);

                            //    sDate3 = (dtRev_B_TargetDate.FindControl("txtDate") as TextBox).Text;
                            //    sDate3 = sDate3.Split('/')[1] + "/" + sDate3.Split('/')[0] + "/" + sDate3.Split('/')[2];
                            //    CDate3 = Convert.ToDateTime(sDate3);


                            //    sDate4 = (dtRevTargetDate.FindControl("txtDate") as TextBox).Text;
                            //    sDate4 = sDate4.Split('/')[1] + "/" + sDate4.Split('/')[0] + "/" + sDate4.Split('/')[2];
                            //    CDate4 = Convert.ToDateTime(sDate4);
                            //    //
                            //    sDate5 = (dtAppTargetDate.FindControl("txtDate") as TextBox).Text;
                            //    sDate5 = sDate5.Split('/')[1] + "/" + sDate5.Split('/')[0] + "/" + sDate5.Split('/')[2];
                            //    CDate5 = Convert.ToDateTime(sDate5);

                            //    result = getdata.DoumentMaster_Insert_or_Update_Flow_4(sDocumentUID, workpackageid, projectId, TaskUID, txtDocumentName.Text, new Guid(DDLDocumentCategory.SelectedValue),
                            //    "", "Submittle Document", 0.0, new Guid(DDLDocumentFlow.SelectedValue), DocStartDate, new Guid(ddlSubmissionUSer.SelectedValue), CDate1,
                            //    new Guid(ddlQualityEngg.SelectedValue), CDate2, new Guid(ddlReviewer_B.SelectedValue), CDate3, new Guid(ddlReviewer.SelectedValue), CDate4, new Guid(ddlApproval.SelectedValue), CDate5);
                            //}
                        }

                        if (result > 0)
                        {
                            loading.Visible = false;
                            Session["SelectedTaskUID"] = Request.QueryString["CategoryID"];
                            Session["ViewDocBy"] = Request.QueryString["ViewDocumentBy"];
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
                            Session["ActivityUID"] = null;
                        }
                        else if (result == -1)
                        {
                            loading.Visible = false;
                            Session["ActivityUID"] = null;
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Submittal Name already exists. Try with different name.');</script>");
                        }
                        else
                        {
                            loading.Visible = false;
                            Session["ActivityUID"] = null;
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('There is a problem with this feature. Please contact Administrator');</script>");
                        }
                    }
                }
                catch (Exception ex)
                {
                    loading.Visible = false;
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error:" + ex.Message + "');</script>");
                }
            }
        }

        private void BindDocument_Category_for_Workpackage(string WorkPackagesID)
        {
            DataSet ds = getdata.WorkPackageCategory_Selectby_WorkPackageUID(new Guid(WorkPackagesID));
            DDLDocumentCategory.DataTextField = "WorkPackageCategory_Name";
            DDLDocumentCategory.DataValueField = "WorkPackageCategory_UID";
            DDLDocumentCategory.DataSource = ds;
            DDLDocumentCategory.DataBind();

        }
    }
}