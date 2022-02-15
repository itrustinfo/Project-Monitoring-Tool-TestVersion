using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagementTool._modal_pages
{
    public partial class add_submittal : System.Web.UI.Page
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
                    LoadDropDowns();
                    BindDocumentFlows();
                    LoadSubmittalTypeMaster();
                    DDLDocumentFlow_SelectedIndexChanged(sender, e);
                    if (WebConfigurationManager.AppSettings["Domain"] == "NJSEI")
                    {
                        synchDisplay.Visible = false;
                    }
                    else if (WebConfigurationManager.AppSettings["Domain"] == "LNT")
                    {
                        chkSync.Checked = true;
                    }
                        //
                        if (Request.QueryString["type"] == "add")
                     {
                        if (Request.QueryString["TaskUID"] != null)
                        {
                            DataTable dttasks = getdata.GetTaskDetails_TaskUID(Request.QueryString["TaskUID"]);
                            if (dttasks.Rows.Count > 0)
                            {
                                txttaskname.Text = dttasks.Rows[0]["Name"].ToString();
                                HiddenParentTask.Value = dttasks.Rows[0]["ParentTaskID"].ToString();
                                DDLWorkPackage.SelectedValue= dttasks.Rows[0]["WorkPackageUID"].ToString();
                                //PRefNumber = getdata.GetDocumentSubmittleRef_Number(new Guid(dttasks.Rows[0]["ProjectUID"].ToString()));
                                //txtReferenceNumber.Text = PRefNumber;
                                Bind_CategoriesforWorkPackage(new Guid(dttasks.Rows[0]["WorkPackageUID"].ToString()));
                            }
                        }
                        else if (Request.QueryString["ProjectUID"] != null && Request.QueryString["WorkPackageUID"] == null)
                        {
                            BindDocument_Category_Master();
                            string PrjName = getdata.getProjectNameby_ProjectUID(new Guid(Request.QueryString["ProjectUID"]));
                            txttaskname.Text = PrjName;

                        }
                        else if (Request.QueryString["ProjectUID"] != null && Request.QueryString["WorkPackageUID"] != null)
                        {
                            string WorkPkgName = getdata.getWorkPackageNameby_WorkPackageUID(new Guid(Request.QueryString["WorkPackageUID"]));
                            txttaskname.Text = WorkPkgName;
                            Bind_CategoriesforWorkPackage(new Guid(Request.QueryString["WorkPackageUID"]));
                        }
                        LinkActivity.Visible = false;
                    }
                    else
                    {
                        LinkActivity.Visible = true;
                        BindDocument_Category_for_Workpackage(Request.QueryString["WrkUID"]);
                        BindDocument();
                        LinkActivity.HRef = "/_modal_pages/choose-activity.aspx?WorkUID=" + Request.QueryString["WrkUID"];
                    }

                    if (Session["ActivityUID"] != null)
                    {
                        ActivityUID.Value = Session["ActivityUID"].ToString();
                        string ActivityType = Session["ActivityUID"].ToString().Split('*')[0];
                        if (ActivityType == "WkPkg")
                        {
                            txttaskname.Text = getdata.getWorkPackageNameby_WorkPackageUID(new Guid(Session["ActivityUID"].ToString().Split('*')[1]));
                        }
                        else
                        {
                            txttaskname.Text = getdata.getTaskNameby_TaskUID(new Guid(Session["ActivityUID"].ToString().Split('*')[1]));
                        }
                        Session["ActivityUID"] = null;
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
            //DDLWorkPackage.DataSource = getdata.GetWorkPackages_By_ProjectUID(new Guid(DDlProject.SelectedValue));
            DDLWorkPackage.DataSource=ds;
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
            DDLDocumentFlow.Items.Insert(0, new ListItem("--Select--", ""));
            //DDLDocumentFlow.SelectedIndex = 2;

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

        private void BindDocument_Category_Master()
        {
            DataSet ds = getdata.GetActualDocument_Type_Master();
            DDLDocumentCategory.DataTextField = "ActualDocumentType";
            DDLDocumentCategory.DataValueField = "ActualDocumenTypeUID";
            DDLDocumentCategory.DataSource = ds;
            DDLDocumentCategory.DataBind();
            DDLDocumentCategory.Items.Insert(0, new ListItem("--Select--", ""));
        }
        private void Bind_CategoriesforWorkPackage(Guid WorkPackageUID)
        {
            DataSet ds = getdata.WorkPackageCategory_Selectby_WorkPackageUID(WorkPackageUID);
            DDLDocumentCategory.DataTextField = "WorkPackageCategory_Name";
            DDLDocumentCategory.DataValueField = "WorkPackageCategory_UID";
            DDLDocumentCategory.DataSource = ds;
            DDLDocumentCategory.DataBind();
            DDLDocumentCategory.Items.Insert(0, new ListItem("--Select--", ""));
        }
        protected void DDLDocumentFlow_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Success-1');</script>");
                if (DDLDocumentFlow.SelectedValue != "")
                {
                    //Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Success-2');</script>");
                    DocumentFlowChanged();
                }
                else
                {
                    ForPhotograph.Visible = false;
                    S1Display.Visible = false;
                    S1Date.Visible = false;
                    S2Display.Visible = false;
                    S2Date.Visible = false;
                    S3Display.Visible = false;
                    S3Date.Visible = false;
                    S4Display.Visible = false;
                    S4Date.Visible = false;
                    S5Display.Visible = false;
                    S5Date.Visible = false;

                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error : " + ex.Message + "');</script>");
            }
            
        }

        public void DocumentFlowChanged()
        {
            //Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Success-3');</script>");
             DataSet ds = getdata.GetDocumentFlows_by_UID(new Guid(DDLDocumentFlow.SelectedValue));
            if (ds.Tables[0].Rows.Count > 0)
            {
                //dFlow.Visible = true;
                //Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Success-4');</script>");
               // LoadDropDowns();
                //Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Success-5');</script>");
                if (ds.Tables[0].Rows[0]["Steps_Count"].ToString() == "-1")
                {
                    ForPhotograph.Visible = true;
                    lblPhotograph.Text = "Submitted Month";
                    //lblStep1Date.Text = ds.Tables[0].Rows[0]["FlowStep1_DisplayName"].ToString() + " Target Date";
                    lblStep1Display.Text = ds.Tables[0].Rows[0]["FlowStep1_DisplayName"].ToString();
                    int year = DateTime.Now.Year - 5;
                    for (int i = 1; i < 25; i++)
                    {
                        year = year + 1;
                        DDLYear.Items.Add(new ListItem(year.ToString(), year.ToString()));
                    }

                    S1Display.Visible = true;
                    S1Date.Visible = false;
                    S2Display.Visible = false;
                    S2Date.Visible = false;
                    S3Display.Visible = false;
                    S3Date.Visible = false;
                    S4Display.Visible = false;
                    S4Date.Visible = false;
                    S5Display.Visible = false;
                    S5Date.Visible = false;
                }
                else if (ds.Tables[0].Rows[0]["Steps_Count"].ToString() == "1")
                {
                    lblStep1Display.Text = ds.Tables[0].Rows[0]["FlowStep1_DisplayName"].ToString();
                    lblStep1Date.Text = ds.Tables[0].Rows[0]["FlowStep1_DisplayName"].ToString() + " Target Date";
                    ForPhotograph.Visible = false;
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
                    ForPhotograph.Visible = false;
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
                    ForPhotograph.Visible = false;
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
                    ForPhotograph.Visible = false;
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
                    ForPhotograph.Visible = false;
                    S1Display.Visible = true;
                    S1Date.Visible = true;
                    S2Display.Visible = true;
                    S2Date.Visible = true;
                    S3Display.Visible = true;
                    S3Date.Visible = true;
                    S4Display.Visible = true;
                    S4Date.Visible = true;
                    S5Display.Visible = true;
                    S5Date.Visible = true;
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
                if (ds.Tables[0].Rows[0]["Steps_Count"].ToString() == "-1")
                {

                }
                else if (ds.Tables[0].Rows[0]["Steps_Count"].ToString() == "1")
                {
                    if (ds.Tables[0].Rows[0]["FlowStep1_Duration"].ToString() != "-1")
                    {
                        dtSubTargetDate.Text = CDate1.AddDays(Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep1_Duration"].ToString())).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    }
                }
                else if (ds.Tables[0].Rows[0]["Steps_Count"].ToString() == "2")
                {
                    if (ds.Tables[0].Rows[0]["FlowStep1_Duration"].ToString() != "-1")
                    {
                        dtSubTargetDate.Text = CDate1.AddDays(Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep1_Duration"].ToString())).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    }
                    if (ds.Tables[0].Rows[0]["FlowStep2_Duration"].ToString() != "-1")
                    {
                        dtQualTargetDate.Text = CDate1.AddDays(Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep2_Duration"].ToString())).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    }
                }
                else if (ds.Tables[0].Rows[0]["Steps_Count"].ToString() == "3")
                {
                    if (ds.Tables[0].Rows[0]["FlowStep1_Duration"].ToString() != "-1")
                    {
                        dtSubTargetDate.Text = CDate1.AddDays(Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep1_Duration"].ToString())).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    }
                    if (ds.Tables[0].Rows[0]["FlowStep2_Duration"].ToString() != "-1")
                    {
                        dtQualTargetDate.Text = CDate1.AddDays(Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep2_Duration"].ToString())).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    }
                    if (ds.Tables[0].Rows[0]["FlowStep3_Duration"].ToString() != "-1")
                    {
                        dtRev_B_TargetDate.Text = CDate1.AddDays(Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep3_Duration"].ToString())).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    }
                    //Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Success-8');</script>");
                }
                else if (ds.Tables[0].Rows[0]["Steps_Count"].ToString() == "4")
                {
                    if (ds.Tables[0].Rows[0]["FlowStep1_Duration"].ToString() != "-1")
                    {
                        dtSubTargetDate.Text = CDate1.AddDays(Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep1_Duration"].ToString())).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    }
                    if (ds.Tables[0].Rows[0]["FlowStep2_Duration"].ToString() != "-1")
                    {
                        dtQualTargetDate.Text = CDate1.AddDays(Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep2_Duration"].ToString())).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    }
                    if (ds.Tables[0].Rows[0]["FlowStep3_Duration"].ToString() != "-1")
                    {
                        dtRev_B_TargetDate.Text = CDate1.AddDays(Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep3_Duration"].ToString())).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    }
                    if (ds.Tables[0].Rows[0]["FlowStep4_Duration"].ToString() != "-1")
                    {
                        dtRevTargetDate.Text = CDate1.AddDays(Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep4_Duration"].ToString())).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    }
                }
                else 
                {
                    if (ds.Tables[0].Rows[0]["FlowStep1_Duration"].ToString() != "-1")
                    {
                        dtSubTargetDate.Text = CDate1.AddDays(Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep1_Duration"].ToString())).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    }
                    if (ds.Tables[0].Rows[0]["FlowStep2_Duration"].ToString() != "-1")
                    {
                        dtQualTargetDate.Text = CDate1.AddDays(Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep2_Duration"].ToString())).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    }
                    if (ds.Tables[0].Rows[0]["FlowStep3_Duration"].ToString() != "-1")
                    {
                        dtRev_B_TargetDate.Text = CDate1.AddDays(Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep3_Duration"].ToString())).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    }
                    if (ds.Tables[0].Rows[0]["FlowStep4_Duration"].ToString() != "-1")
                    {
                        dtRevTargetDate.Text = CDate1.AddDays(Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep4_Duration"].ToString())).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    }
                    if (ds.Tables[0].Rows[0]["dtAppTargetDate"].ToString() != "-1")
                    {
                        dtRevTargetDate.Text = CDate1.AddDays(Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep5_Duration"].ToString())).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    }
                    
                }

            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e) // add Submittal
        {
            if (Page.IsValid)
            {
                try
                {
                    if (DDLDocumentCategory.SelectedValue == "")
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Please select Document Category.If Category not exists, add it.');</script>");
                    }
                    else if (DDLDocumentFlow.SelectedIndex==1 && !getdata.IsValidDate(dtSubTargetDate.Text))
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Datetime format is not supported. Please contact System admin.');</script>");
                    }
                    else if(dtSubTargetDate.Text != "" ? ! getdata.IsValidDate(dtSubTargetDate.Text) : false)
                    //else if ((!getdata.IsValidDate(dtSubTargetDate.Text != "" ? dtSubTargetDate.Text : DateTime.Now.ToString("dd/MM/yyyy")) || !getdata.IsValidDate(dtQualTargetDate.Text != "" ? dtQualTargetDate.Text : DateTime.Now.ToString("dd/MM/yyyy")) || !getdata.IsValidDate(dtRev_B_TargetDate.Text != "" ? dtRev_B_TargetDate.Text : DateTime.Now.ToString("dd/MM/yyyy"))))
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Datetime format is not supported. Please contact System admin.');</script>");
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

                        if (Request.QueryString["TaskUID"] != null && ActivityUID.Value == "" && Request.QueryString["TaskUID"] != Guid.Empty.ToString())
                        {
                            TaskUID = new Guid(Request.QueryString["TaskUID"].ToString());
                            Subject = "Added new Submittal " + txtDocumentName.Text;
                            DataTable dttasks = getdata.GetTaskDetails_TaskUID(Request.QueryString["TaskUID"]);
                            if (dttasks.Rows.Count > 0)
                            {
                                projectId = new Guid(dttasks.Rows[0]["ProjectUID"].ToString());
                                workpackageid = new Guid(dttasks.Rows[0]["workPackageUID"].ToString());
                                txttaskname.Text = dttasks.Rows[0]["Name"].ToString();
                            }
                        }
                        else if (Request.QueryString["WorkPackageUID"] != null && ActivityUID.Value == "")
                        {
                            Subject = "Added new Submittal " + txtDocumentName.Text;
                            workpackageid = new Guid(Request.QueryString["WorkPackageUID"]);
                            DataSet dsWorkpackage = getdata.GetWorkPackages_By_WorkPackageUID(workpackageid);
                            if (dsWorkpackage.Tables[0].Rows.Count > 0)
                            {
                                projectId = new Guid(dsWorkpackage.Tables[0].Rows[0]["ProjectUID"].ToString());
                            }
                        }
                        else if (Request.QueryString["WrkUID"] != null && ActivityUID.Value == "")
                        {
                            Subject = "Added new Submittal " + txtDocumentName.Text;
                            workpackageid = new Guid(Request.QueryString["WrkUID"]);
                            DataSet dsWorkpackage = getdata.GetWorkPackages_By_WorkPackageUID(workpackageid);
                            if (dsWorkpackage.Tables[0].Rows.Count > 0)
                            {
                                projectId = new Guid(dsWorkpackage.Tables[0].Rows[0]["ProjectUID"].ToString());
                            }
                        }
                        else if (ActivityUID.Value != "")
                        {
                            string ActivityType = ActivityUID.Value.ToString().Split('*')[0];
                            if (ActivityType == "WkPkg")
                            {
                                workpackageid = new Guid(ActivityUID.Value.ToString().Split('*')[1]);
                                DataSet dsWorkpackage = getdata.GetWorkPackages_By_WorkPackageUID(workpackageid);
                                if (dsWorkpackage.Tables[0].Rows.Count > 0)
                                {
                                    projectId = new Guid(dsWorkpackage.Tables[0].Rows[0]["ProjectUID"].ToString());
                                }
                            }
                            else
                            {
                                TaskUID = new Guid(ActivityUID.Value.ToString().Split('*')[1]);
                                DataTable dttasks = getdata.GetTaskDetails_TaskUID(TaskUID.ToString());
                                if (dttasks.Rows.Count > 0)
                                {
                                    projectId = new Guid(dttasks.Rows[0]["ProjectUID"].ToString());
                                    workpackageid = new Guid(dttasks.Rows[0]["workPackageUID"].ToString());
                                }
                            }
                        }
                        else if (Request.QueryString["ProjectUID"] != null)
                        {
                            Subject = "Added new Submittal " + txtDocumentName.Text;
                            projectId = new Guid(Request.QueryString["ProjectUID"]);
                        }


                        string sDate1 = "", sDate2 = "", sDate3 = "", sDate4 = "", sDate5 = "", DocPath = "", DocStartString = "", CoverPagePath = "";
                        DateTime CDate1 = DateTime.Now, CDate2 = DateTime.Now, CDate3 = DateTime.Now, CDate4 = DateTime.Now, CDate5 = DateTime.Now, DocStartDate = DateTime.Now;

                        int result = 0;
                        //
                        string Remarks = txtremarks.Text;
                        string SubDocTypeMaster = "";
                        int estimateddocs = int.Parse(txtestdocs.Text);
                        string IsSync = "N";
                        if(ddlSubDocType.SelectedIndex != 0)
                        {
                            SubDocTypeMaster = ddlSubDocType.SelectedItem.ToString();
                        }
                        // for synch
                        if(chkSync.Checked)
                        {
                            IsSync = "Y";
                        }
                        //for syncing flow 2 submittal to contractor for ONTB

                        if(DDLDocumentFlow.SelectedItem.ToString() == "Flow 2")
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


                            //
                            //sDate1 = dtSubTargetDate.Text;
                            //sDate1 = sDate1.Split('/')[1] + "/" + sDate1.Split('/')[0] + "/" + sDate1.Split('/')[2];
                            //CDate1 = Convert.ToDateTime(sDate1);
                            ////

                            //sDate2 = dtQualTargetDate.Text;
                            //sDate2 = sDate2.Split('/')[1] + "/" + sDate2.Split('/')[0] + "/" + sDate2.Split('/')[2];
                            //CDate2 = Convert.ToDateTime(sDate2);

                            //sDate3 = dtRev_B_TargetDate.Text;
                            //sDate3 = sDate3.Split('/')[1] + "/" + sDate3.Split('/')[0] + "/" + sDate3.Split('/')[2];
                            //CDate3 = Convert.ToDateTime(sDate3);

                            //result = getdata.DoumentMaster_Insert_or_Update_Flow_2(sDocumentUID, workpackageid, projectId, TaskUID, txtDocumentName.Text, new Guid(DDLDocumentCategory.SelectedValue),
                            //"", "Submittle Document", 0.0, new Guid(DDLDocumentFlow.SelectedValue), DocStartDate, new Guid(ddlSubmissionUSer.SelectedValue), CDate1,
                            //new Guid(ddlQualityEngg.SelectedValue), CDate2, new Guid(ddlReviewer_B.SelectedValue), CDate3);
                            if (dsFlow.Tables[0].Rows[0]["Steps_Count"].ToString() == "-1")
                            {
                                sDate1 = "01/" + DDLMonth.SelectedValue + "/" + DDLYear.SelectedValue;
                                sDate1 = getdata.ConvertDateFormat(sDate1);
                                CDate1 = Convert.ToDateTime(sDate1);

                                result = getdata.DoumentMaster_Insert_or_Update_Flow_0(sDocumentUID, workpackageid, projectId, TaskUID, txtDocumentName.Text, new Guid(DDLDocumentCategory.SelectedValue),
                                "", "Submittle Document", 0.0, new Guid(DDLDocumentFlow.SelectedValue), DocStartDate, new Guid(ddlSubmissionUSer.SelectedValue), CDate1,estimateddocs,Remarks,SubDocTypeMaster, IsSync);
                            }
                            else if (dsFlow.Tables[0].Rows[0]["Steps_Count"].ToString() == "1")
                            {
                                //
                                sDate1 = dtSubTargetDate.Text != "" ? dtSubTargetDate.Text : CDate1.ToString("dd/MM/yyyy");
                                //sDate1 = sDate1.Split('/')[1] + "/" + sDate1.Split('/')[0] + "/" + sDate1.Split('/')[2];
                                sDate1 = getdata.ConvertDateFormat(sDate1);
                                CDate1 = Convert.ToDateTime(sDate1);
                                result = getdata.DoumentMaster_Insert_or_Update_Flow_0(sDocumentUID, workpackageid, projectId, TaskUID, txtDocumentName.Text, new Guid(DDLDocumentCategory.SelectedValue),
                                "", "Submittle Document", 0.0, new Guid(DDLDocumentFlow.SelectedValue), DocStartDate, new Guid(ddlSubmissionUSer.SelectedValue), CDate1, estimateddocs, Remarks, SubDocTypeMaster, IsSync);

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
                                new Guid(ddlQualityEngg.SelectedValue), CDate2, estimateddocs, Remarks, SubDocTypeMaster, IsSync);
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
                                new Guid(ddlQualityEngg.SelectedValue), CDate2, new Guid(ddlReviewer_B.SelectedValue), CDate3, estimateddocs, Remarks, SubDocTypeMaster, IsSync);
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
                                new Guid(ddlQualityEngg.SelectedValue), CDate2, new Guid(ddlReviewer_B.SelectedValue), CDate3, new Guid(ddlReviewer.SelectedValue), CDate4, estimateddocs, Remarks, SubDocTypeMaster, IsSync);
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
                                new Guid(ddlQualityEngg.SelectedValue), CDate2, new Guid(ddlReviewer_B.SelectedValue), CDate3, new Guid(ddlReviewer.SelectedValue), CDate4, new Guid(ddlApproval.SelectedValue), CDate5, estimateddocs, Remarks, SubDocTypeMaster, IsSync);
                            }
                        }

                        if (result > 0)
                        {
                            if (Request.QueryString["DocID"] == null)
                            {
                                string sHtmlString = string.Empty;
                                DataSet dsUser = new DataSet();
                                if (Session["TypeOfUser"].ToString() == "U" || Session["TypeOfUser"].ToString() == "MD" || Session["TypeOfUser"].ToString() == "VP")
                                {
                                    dsUser = getdata.getAllUsers();
                                }
                                else if (Session["TypeOfUser"].ToString() == "PA")
                                {
                                    //ds = getdata.getUsers_by_ProjectUnder(new Guid(DDlProject.SelectedValue));
                                    dsUser = getdata.GetUsers_under_ProjectUID(new Guid(DDlProject.SelectedValue));
                                }
                                else
                                {
                                    dsUser = getdata.GetUsers_under_ProjectUID(new Guid(DDlProject.SelectedValue));
                                }
                                if (dsUser.Tables[0].Rows.Count > 0)
                                {
                                    string CC = string.Empty;
                                    string ToEmailID = "";
                                    string ActivityName = HiddenParentTask.Value != "" ? getdata.getTaskNameby_TaskUID(new Guid(HiddenParentTask.Value)) + " -> " + txttaskname.Text : txttaskname.Text;
                                    for (int i = 1; i < dsUser.Tables[0].Rows.Count; i++)
                                    {
                                        if (dsUser.Tables[0].Rows[i]["UserUID"].ToString() == ddlSubmissionUSer.SelectedValue)
                                        {
                                            ToEmailID = dsUser.Tables[0].Rows[i]["EmailID"].ToString();
                                        }
                                        else
                                        {
                                            if (getdata.GetUserMailAccess(new Guid(dsUser.Tables[0].Rows[i]["UserUID"].ToString()), "documentmail") != 0)
                                            {
                                                CC += dsUser.Tables[0].Rows[i]["EmailID"].ToString() + ",";
                                            }
                                        }
                                    }
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
                                    sHtmlString += "<div style='width:100%; float:left;'><br/>Dear All,<br/><br/><span style='font-weight:bold;'>" + ddlSubmissionUSer.SelectedItem.Text + " added a new Submittal. Below are the details,</span> <br/><br/></div>";
                                    sHtmlString += "<div style='width:100%; float:left;'><table style='width:100%;'>" +
                                                    "<tr><td><b>Workpackage </b></td><td style='text-align:center;'><b>:</b></td><td>" + DDLWorkPackage.SelectedItem.Text + "</td></tr>" +
                                                    "<tr><td><b>Activity Name </b></td><td style='text-align:center;'><b>:</b></td><td>" + ActivityName + "</td></tr>" +
                                                    "<tr><td><b>Submittal Name </b></td><td style='text-align:center;'><b>:</b></td><td>" + txtDocumentName.Text + "</td></tr>" +
                                                    "<tr><td><b>Submittal Category </b></td><td style='text-align:center;'><b>:</b></td><td>" + DDLDocumentCategory.SelectedItem.Text + "</td></tr>" +
                                                    "<tr><td><b>Submitted By </b></td><td style='text-align:center;'><b>:</b></td><td>" + ddlSubmissionUSer.SelectedItem.Text + "</td></tr>" +
                                                    "<tr><td><b>Date </b></td><td style='text-align:center;'><b>:</b></td><td>" + DateTime.Now.ToString("dd MMM yyyy") + "</td></tr>";
                                    sHtmlString += "</table></div>";
                                    sHtmlString += "<div style='width:100%; float:left;'><br/><br/>Sincerely, <br/> Project Monitoring Tool.</div></div></body></html>";
                                    // string ret = getdata.SendMail(ToEmailID, Subject, sHtmlString, CC, "");
                                    // added on 02/11/2020
                                    DataTable dtemailCred = getdata.GetEmailCredentials();
                                    Guid MailUID = Guid.NewGuid();
                                    if (ToEmailID == "")
                                    {
                                        ToEmailID = getdata.GetUserEmail_By_UserUID_New(new Guid(Session["UserUID"].ToString()));
                                    }
                                    getdata.StoreEmaildataToMailQueue(MailUID, new Guid(Session["UserUID"].ToString()), dtemailCred.Rows[0][0].ToString(), ToEmailID, Subject, sHtmlString, CC, "");
                                    //
                                }
                            }
                            ActivityUID.Value = "";
                            loading.Visible = false;
                            if (TaskUID == Guid.Empty)
                            {
                                Session["SelectedTaskUID"] = workpackageid;
                                Session["ViewDocBy"] = Request.QueryString["ViewDocumentBy"];
                            }
                            else
                            {
                                Session["SelectedTaskUID"] = TaskUID;
                                Session["ViewDocBy"] = Request.QueryString["ViewDocumentBy"];
                            }

                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
                        }
                        else if (result == -1)
                        {
                            loading.Visible = false;
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Submittal Name already exists. Try with different name.');</script>");
                        }
                        else
                        {
                            ActivityUID.Value = "";
                            loading.Visible = false;
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('There is a problem with this feature. Please contact Administrator');</script>");
                        }
                    }
                }
                catch (Exception ex)
                {
                    loading.Visible = false;
                    ActivityUID.Value = "";
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error:" + ex.Message + "');</script>");
                }
            }
        }

        protected void BindDocument()
        {
            DataSet ds = getdata.getDocumentsbyDocID(new Guid(Request.QueryString["DocID"].ToString()));
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["TaskUID"].ToString() == "00000000-0000-0000-0000-000000000000")
                {
                    txttaskname.Text = getdata.getWorkPackageNameby_WorkPackageUID(new Guid(ds.Tables[0].Rows[0]["WorkPackageUID"].ToString()));
                }
                else if (ds.Tables[0].Rows[0]["TaskUID"].ToString() == "00000000-0000-0000-0000-000000000000" && ds.Tables[0].Rows[0]["WorkPackageUID"].ToString() == "00000000-0000-0000-0000-000000000000")
                {
                    txttaskname.Text = getdata.getProjectNameby_ProjectUID(new Guid(ds.Tables[0].Rows[0]["ProjectUID"].ToString()));
                }
                else
                {
                    txttaskname.Text = getdata.getTaskNameby_TaskUID(new Guid(ds.Tables[0].Rows[0]["TaskUID"].ToString()));
                }
                txtDocumentName.Text = ds.Tables[0].Rows[0]["DocName"].ToString();
                DDLDocumentCategory.SelectedValue = new Guid(ds.Tables[0].Rows[0]["Doc_Category"].ToString()).ToString();
                DDLDocumentFlow.SelectedValue = ds.Tables[0].Rows[0]["FlowUID"].ToString();
                //
                txtremarks.Text = ds.Tables[0].Rows[0]["Remarks"].ToString();
                ddlSubDocType.SelectedValue = ds.Tables[0].Rows[0]["DocumentSearchType"].ToString();
                if (ds.Tables[0].Rows[0]["EstimatedDocuments"] != System.DBNull.Value)
                {
                    txtestdocs.Text = ds.Tables[0].Rows[0]["EstimatedDocuments"].ToString();
                }
                else
                {
                    txtestdocs.Text = "0";
                }
                if (ds.Tables[0].Rows[0]["IsSync"] != System.DBNull.Value)
                {
                    if(ds.Tables[0].Rows[0]["IsSync"].ToString() == "Y")
                    {
                        chkSync.Checked = true;
                    }
                }
               
                //
                DataSet dsFlow = getdata.GetDocumentFlows_by_UID(new Guid(DDLDocumentFlow.SelectedValue));
                if (dsFlow.Tables[0].Rows.Count > 0)
                {
                    if (dsFlow.Tables[0].Rows[0]["Steps_Count"].ToString() == "-1")
                    {
                        ForPhotograph.Visible = true;
                        lblPhotograph.Text = "Submitted Month";
                        //lblStep1Date.Text = ds.Tables[0].Rows[0]["FlowStep1_DisplayName"].ToString() + " Target Date";
                        lblStep1Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep1_DisplayName"].ToString();
                        int year = DateTime.Now.Year - 5;
                        for (int i = 1; i < 25; i++)
                        {
                            year = year + 1;
                            DDLYear.Items.Add(new ListItem(year.ToString(), year.ToString()));
                        }

                        S1Display.Visible = true;
                        S1Date.Visible = false;
                        S2Display.Visible = false;
                        S2Date.Visible = false;
                        S3Display.Visible = false;
                        S3Date.Visible = false;
                        S4Display.Visible = false;
                        S4Date.Visible = false;
                        S5Display.Visible = false;
                        S5Date.Visible = false;
                        ddlSubmissionUSer.SelectedValue = ds.Tables[0].Rows[0]["FlowStep1_UserUID"].ToString();
                        string sMonth= Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep1_TargetDate"].ToString()).Month.ToString();
                        DDLYear.SelectedValue = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep1_TargetDate"].ToString()).Year.ToString();
                        DDLMonth.SelectedValue = sMonth.Length == 1 ? ("0" + sMonth) : sMonth;
                    }
                    else if (dsFlow.Tables[0].Rows[0]["Steps_Count"].ToString() == "1")
                    {
                        lblStep1Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep1_DisplayName"].ToString();
                        lblStep1Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep1_DisplayName"].ToString() + " Target Date";
                        ForPhotograph.Visible = false;
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
                    else if (dsFlow.Tables[0].Rows[0]["Steps_Count"].ToString() == "2")
                    {
                        lblStep1Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep1_DisplayName"].ToString();
                        lblStep1Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep1_DisplayName"].ToString() + " Target Date";
                        lblStep2Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep2_DisplayName"].ToString();
                        lblStep2Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep2_DisplayName"].ToString() + " Target Date";
                        ForPhotograph.Visible = false;
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
                    else if (dsFlow.Tables[0].Rows[0]["Steps_Count"].ToString() == "3")
                    {
                        lblStep1Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep1_DisplayName"].ToString();
                        lblStep1Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep1_DisplayName"].ToString() + " Target Date";
                        lblStep2Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep2_DisplayName"].ToString();
                        lblStep2Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep2_DisplayName"].ToString() + " Target Date";
                        lblStep3Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep3_DisplayName"].ToString();
                        lblStep3Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep3_DisplayName"].ToString() + " Target Date";
                        ForPhotograph.Visible = false;
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
                    else if (dsFlow.Tables[0].Rows[0]["Steps_Count"].ToString() == "4")
                    {
                        lblStep1Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep1_DisplayName"].ToString();
                        lblStep1Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep1_DisplayName"].ToString() + " Target Date";
                        lblStep2Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep2_DisplayName"].ToString();
                        lblStep2Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep2_DisplayName"].ToString() + " Target Date";
                        lblStep3Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep3_DisplayName"].ToString();
                        lblStep3Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep3_DisplayName"].ToString() + " Target Date";
                        lblStep4Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep4_DisplayName"].ToString();
                        lblStep4Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep4_DisplayName"].ToString() + " Target Date";
                        ForPhotograph.Visible = false;
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
                        ForPhotograph.Visible = false;
                        S1Display.Visible = true;
                        S1Date.Visible = true;
                        S2Display.Visible = true;
                        S2Date.Visible = true;
                        S3Display.Visible = true;
                        S3Date.Visible = true;
                        S4Display.Visible = true;
                        S4Date.Visible = true;
                        S5Display.Visible = true;
                        S5Date.Visible = true;
                        lblStep1Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep1_DisplayName"].ToString();
                        lblStep1Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep1_DisplayName"].ToString() + " Target Date";
                        lblStep2Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep2_DisplayName"].ToString();
                        lblStep2Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep2_DisplayName"].ToString() + " Target Date";
                        lblStep3Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep3_DisplayName"].ToString();
                        lblStep3Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep3_DisplayName"].ToString() + " Target Date";
                        lblStep4Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep4_DisplayName"].ToString();
                        lblStep4Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep4_DisplayName"].ToString() + " Target Date";
                        lblStep5Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep5_DisplayName"].ToString();
                        lblStep5Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep5_DisplayName"].ToString() + " Target Date";
                    }

                    if (dsFlow.Tables[0].Rows[0]["Steps_Count"].ToString() != "-1")
                    {
                        ddlSubmissionUSer.SelectedValue = ds.Tables[0].Rows[0]["FlowStep1_UserUID"].ToString();
                        ddlQualityEngg.SelectedValue = ds.Tables[0].Rows[0]["FlowStep2_UserUID"].ToString();
                        ddlReviewer_B.SelectedValue = ds.Tables[0].Rows[0]["FlowStep3_UserUID"].ToString();
                        ddlReviewer.SelectedValue = ds.Tables[0].Rows[0]["FlowStep4_UserUID"].ToString();
                        ddlApproval.SelectedValue = ds.Tables[0].Rows[0]["FlowStep5_UserUID"].ToString();
                        if (ds.Tables[0].Rows[0]["FlowStep1_TargetDate"].ToString() != null && ds.Tables[0].Rows[0]["FlowStep1_TargetDate"].ToString() != "")
                        {
                            dtSubTargetDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep1_TargetDate"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                        }
                        if (ds.Tables[0].Rows[0]["FlowStep2_TargetDate"].ToString() != null && ds.Tables[0].Rows[0]["FlowStep2_TargetDate"].ToString() != "")
                        {
                            
                            dtQualTargetDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep2_TargetDate"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                        }
                        if (ds.Tables[0].Rows[0]["FlowStep3_TargetDate"].ToString() != null && ds.Tables[0].Rows[0]["FlowStep3_TargetDate"].ToString() != "")
                        {
                            
                            dtRev_B_TargetDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep3_TargetDate"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                        }
                        if (ds.Tables[0].Rows[0]["FlowStep4_TargetDate"].ToString() != null && ds.Tables[0].Rows[0]["FlowStep4_TargetDate"].ToString() != "")
                        {
                            
                            dtRevTargetDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep4_TargetDate"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                        }
                        if (ds.Tables[0].Rows[0]["FlowStep5_TargetDate"].ToString() != null && ds.Tables[0].Rows[0]["FlowStep5_TargetDate"].ToString() != "")
                        {
                            
                            dtAppTargetDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep5_TargetDate"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                        }
                    }
                    

                    DataSet actualdocuments = getdata.ActualDocuments_SelectBy_DocumentUID(new Guid(Request.QueryString["DocID"].ToString()));
                    if (actualdocuments.Tables[0].Rows.Count > 0)
                    {
                        DDLDocumentFlow.Enabled = false;
                        if (DDLDocumentFlow.SelectedItem.ToString() == "Flow 1")
                        {
                            lblflowmsg.Visible = true;
                        }
                    }
                    else
                    {
                        if (DDLDocumentFlow.SelectedItem.ToString() == "Flow 1")
                        {
                            DDLDocumentFlow.Enabled = true;
                        }
                        else
                        {
                            DDLDocumentFlow.Enabled = false;
                        }
                        lblflowmsg.Visible = false;
                    }
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

        private void LoadSubmittalTypeMaster()
        {
            
            ddlSubDocType.DataSource =getdata.getsubmittalDoctypeMaster();
            ddlSubDocType.DataTextField = "Type";
            ddlSubDocType.DataValueField = "Type";
            ddlSubDocType.DataBind();
            ddlSubDocType.Items.Insert(0, new ListItem("--Select--", ""));
        }
    }
}