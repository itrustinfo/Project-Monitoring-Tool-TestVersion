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
                   // LoadDropDowns();
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
                                DDLWorkPackage.SelectedValue = dttasks.Rows[0]["WorkPackageUID"].ToString();
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
                        DDLDocumentCategory.Enabled = false;
                        LinkActivity.HRef = "/_modal_pages/choose-activity.aspx?WorkUID=" + Request.QueryString["WrkUID"];
                        //

                        if (Session["IsContractor"].ToString() == "Y")
                        {
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
                            S6Display.Visible = false;
                            S6Date.Visible = false;
                            S7Display.Visible = false;
                            S7Date.Visible = false;
                            S8Display.Visible = false;
                            S8Date.Visible = false;
                            S9Display.Visible = false;
                            S9Date.Visible = false;
                            S10Display.Visible = false;
                            S10Date.Visible = false;
                            S11Display.Visible = false;
                            S11Date.Visible = false;
                            S12Display.Visible = false;
                            S12Date.Visible = false;
                            S13Display.Visible = false;
                            S13Date.Visible = false;
                            S14Display.Visible = false;
                            S14Date.Visible = false;
                            S15Display.Visible = false;
                            S15Date.Visible = false;
                            S16Display.Visible = false;
                            S16Date.Visible = false;
                            S17Display.Visible = false;
                            S17Date.Visible = false;
                            S18Display.Visible = false;
                            S18Date.Visible = false;
                            S19Display.Visible = false;
                            S19Date.Visible = false;
                            S20Display.Visible = false;
                            S20Date.Visible = false;
                        }
                        //
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
                string WorkPackageUID = string.Empty;
                DataSet ds = new DataSet();
                DataTable dsMusers = new DataTable();
                //if (Session["TypeOfUser"].ToString() == "U" || Session["TypeOfUser"].ToString() =="MD" || Session["TypeOfUser"].ToString() == "VP")
                //{
                //    ds = getdata.getAllUsers();
                //}
                //else if (Session["TypeOfUser"].ToString() == "PA")
                //{
                //    //ds = getdata.getUsers_by_ProjectUnder(new Guid(DDlProject.SelectedValue));
                //    ds = getdata.GetUsers_under_ProjectUID(new Guid(DDlProject.SelectedValue));
                //}
                //else
                //{
                // ds = getdata.GetUsers_under_ProjectUID(new Guid(DDlProject.SelectedValue));
                if (Request.QueryString["WorkPackageUID"] != null)
                {
                    ds = getdata.GetUsers_under_WorkpackageUID(new Guid(Request.QueryString["WorkPackageUID"].ToString()));
                    WorkPackageUID = Request.QueryString["WorkPackageUID"].ToString();
                }
                else if (Request.QueryString["WrkUID"] != null)
                {
                    ds = getdata.GetUsers_under_WorkpackageUID(new Guid(Request.QueryString["WrkUID"].ToString()));
                    WorkPackageUID = Request.QueryString["WrkUID"].ToString();
                }

                    //}

                    //ddlSubmissionUSer.DataSource = getdata.getUsers("S");
                //    ddlSubmissionUSer.DataSource = ds;
                //ddlSubmissionUSer.DataTextField = "UserName";
                //ddlSubmissionUSer.DataValueField = "UserUID";
                //ddlSubmissionUSer.DataBind();
                //ddlSubmissionUSer.Items.Insert(0, new ListItem("--Select--", ""));
                dsMusers = getdata.FlowMasterUser_Select(new Guid(DDLDocumentFlow.SelectedValue), new Guid(WorkPackageUID), new Guid(DDLDocumentCategory.SelectedValue), 1);

                if (dsMusers.Rows.Count > 0)
                {
                    ddlSubmissionUSer.DataSource = dsMusers;
                    ddlSubmissionUSer.DataTextField = "Name";
                    ddlSubmissionUSer.DataValueField = "UserUID";
                    ddlSubmissionUSer.DataBind();
                    ddlSubmissionUSer.SelectedIndex = 0;
                   
                }
                else
                {
                    ddlSubmissionUSer.DataSource = ds;
                    ddlSubmissionUSer.DataTextField = "UserName";
                    ddlSubmissionUSer.DataValueField = "UserUID";
                    ddlSubmissionUSer.DataBind();
                    ddlSubmissionUSer.Items.Insert(0, new ListItem("--Select--", ""));
                }

                //
                //ddlQualityEngg.DataSource = getdata.getUsers("C"); /step 2
                dsMusers = getdata.FlowMasterUser_Select(new Guid(DDLDocumentFlow.SelectedValue), new Guid(WorkPackageUID), new Guid(DDLDocumentCategory.SelectedValue), 2);
                if (dsMusers.Rows.Count > 0)
                {
                    ddlQualityEngg.DataSource = dsMusers;
                    ddlQualityEngg.DataTextField = "Name";
                    ddlQualityEngg.DataValueField = "UserUID";
                    ddlQualityEngg.DataBind();
                    //
                    foreach (ListItem item in ddlQualityEngg.Items)
                        item.Selected = true;
                }
                else
                {
                    ddlQualityEngg.DataSource = ds;
                    ddlQualityEngg.DataTextField = "UserName";
                    ddlQualityEngg.DataValueField = "UserUID";
                    ddlQualityEngg.DataBind();
                }
                // ddlQualityEngg.Items.Insert(0, new ListItem("--Select--", ""));
                //
                //ddlReviewer.DataSource = getdata.getUsers("R"); /step 3
                dsMusers = getdata.FlowMasterUser_Select(new Guid(DDLDocumentFlow.SelectedValue), new Guid(WorkPackageUID), new Guid(DDLDocumentCategory.SelectedValue), 3);
                if (dsMusers.Rows.Count > 0)
                {
                    ddlReviewer_B.DataSource = dsMusers;
                    ddlReviewer_B.DataTextField = "Name";
                    ddlReviewer_B.DataValueField = "UserUID";
                    ddlReviewer_B.DataBind();
                    //
                    foreach (ListItem item in ddlReviewer_B.Items)
                        item.Selected = true;
                }
                else
                {
                    ddlReviewer_B.DataSource = ds;
                    ddlReviewer_B.DataTextField = "UserName";
                    ddlReviewer_B.DataValueField = "UserUID";
                    ddlReviewer_B.DataBind();
                }
               
                //  ddlReviewer.Items.Insert(0, new ListItem("--Select--", ""));

                //
                //ddlReviewer_B.DataSource = getdata.getUsers("R"); //step 4
                dsMusers = getdata.FlowMasterUser_Select(new Guid(DDLDocumentFlow.SelectedValue), new Guid(WorkPackageUID), new Guid(DDLDocumentCategory.SelectedValue), 4);
                if (dsMusers.Rows.Count > 0)
                {
                    ddlReviewer.DataSource = dsMusers;
                    ddlReviewer.DataTextField = "Name";
                    ddlReviewer.DataValueField = "UserUID";
                    ddlReviewer.DataBind();
                    //
                    foreach (ListItem item in ddlReviewer.Items)
                        item.Selected = true;
                }
                else
                {
                    ddlReviewer.DataSource = ds;
                    ddlReviewer.DataTextField = "UserName";
                    ddlReviewer.DataValueField = "UserUID";
                    ddlReviewer.DataBind();
                }
                //  ddlReviewer_B.Items.Insert(0, new ListItem("--Select--", ""));
                //
                //ddlApproval.DataSource = getdata.getUsers("A"); //step 5
                dsMusers = getdata.FlowMasterUser_Select(new Guid(DDLDocumentFlow.SelectedValue), new Guid(WorkPackageUID), new Guid(DDLDocumentCategory.SelectedValue), 5);
                if (dsMusers.Rows.Count > 0)
                {
                    ddlApproval.DataSource = dsMusers;
                    ddlApproval.DataTextField = "Name";
                    ddlApproval.DataValueField = "UserUID";
                    ddlApproval.DataBind();
                    foreach (ListItem item in ddlApproval.Items)
                        item.Selected = true;
                }
                else
                {
                    ddlApproval.DataSource = ds;
                    ddlApproval.DataTextField = "UserName";
                    ddlApproval.DataValueField = "UserUID";
                    ddlApproval.DataBind();
                }
                // ddlApproval.Items.Insert(0, new ListItem("--Select--", ""));

                // added on 03/03/2022 for new 15 entries
                dsMusers = getdata.FlowMasterUser_Select(new Guid(DDLDocumentFlow.SelectedValue), new Guid(WorkPackageUID), new Guid(DDLDocumentCategory.SelectedValue), 6);
                if (dsMusers.Rows.Count > 0)
                {
                    dlUser6.DataSource = dsMusers;
                    dlUser6.DataTextField = "Name";
                    dlUser6.DataValueField = "UserUID";
                    dlUser6.DataBind();
                    foreach (ListItem item in dlUser6.Items)
                        item.Selected = true;
                }
                else
                {

                    dlUser6.DataSource = ds;
                    dlUser6.DataTextField = "UserName";
                    dlUser6.DataValueField = "UserUID";
                    dlUser6.DataBind();
                }
                // dlUser6.Items.Insert(0, new ListItem("--Select--", ""));
                dsMusers = getdata.FlowMasterUser_Select(new Guid(DDLDocumentFlow.SelectedValue), new Guid(WorkPackageUID), new Guid(DDLDocumentCategory.SelectedValue), 7);
                if (dsMusers.Rows.Count > 0)
                {
                    dlUser7.DataSource = dsMusers;
                    dlUser7.DataTextField = "Name";
                    dlUser7.DataValueField = "UserUID";
                    dlUser7.DataBind();
                    foreach (ListItem item in dlUser7.Items)
                        item.Selected = true;
                }
                else
                {
                    dlUser7.DataSource = ds;
                    dlUser7.DataTextField = "UserName";
                    dlUser7.DataValueField = "UserUID";
                    dlUser7.DataBind();
                }
                //  dlUser7.Items.Insert(0, new ListItem("--Select--", ""));
                dsMusers = getdata.FlowMasterUser_Select(new Guid(DDLDocumentFlow.SelectedValue), new Guid(WorkPackageUID), new Guid(DDLDocumentCategory.SelectedValue), 8);
                if (dsMusers.Rows.Count > 0)
                {
                    dlUser8.DataSource = dsMusers;
                    dlUser8.DataTextField = "Name";
                    dlUser8.DataValueField = "UserUID";
                    dlUser8.DataBind();
                    foreach (ListItem item in dlUser8.Items)
                        item.Selected = true;
                }
                else
                {
                    dlUser8.DataSource = ds;
                    dlUser8.DataTextField = "UserName";
                    dlUser8.DataValueField = "UserUID";
                    dlUser8.DataBind();
                }
                // dlUser8.Items.Insert(0, new ListItem("--Select--", ""));
                dsMusers = getdata.FlowMasterUser_Select(new Guid(DDLDocumentFlow.SelectedValue), new Guid(WorkPackageUID), new Guid(DDLDocumentCategory.SelectedValue), 9);
                if (dsMusers.Rows.Count > 0)
                {
                    dlUser9.DataSource = dsMusers;
                    dlUser9.DataTextField = "Name";
                    dlUser9.DataValueField = "UserUID";
                    dlUser9.DataBind();
                    foreach (ListItem item in dlUser9.Items)
                        item.Selected = true;
                }
                else
                {
                    dlUser9.DataSource = ds;
                    dlUser9.DataTextField = "UserName";
                    dlUser9.DataValueField = "UserUID";
                    dlUser9.DataBind();
                }
                // dlUser9.Items.Insert(0, new ListItem("--Select--", ""));
                dsMusers = getdata.FlowMasterUser_Select(new Guid(DDLDocumentFlow.SelectedValue), new Guid(WorkPackageUID), new Guid(DDLDocumentCategory.SelectedValue), 10);
                if (dsMusers.Rows.Count > 0)
                {
                    dlUser10.DataSource = dsMusers;
                    dlUser10.DataTextField = "Name";
                    dlUser10.DataValueField = "UserUID";
                    dlUser10.DataBind();
                    foreach (ListItem item in dlUser10.Items)
                        item.Selected = true;
                }
                else
                {
                    dlUser10.DataSource = ds;
                    dlUser10.DataTextField = "UserName";
                    dlUser10.DataValueField = "UserUID";
                    dlUser10.DataBind();
                }
                // dlUser10.Items.Insert(0, new ListItem("--Select--", ""));
                dsMusers = getdata.FlowMasterUser_Select(new Guid(DDLDocumentFlow.SelectedValue), new Guid(WorkPackageUID), new Guid(DDLDocumentCategory.SelectedValue), 11);
                if (dsMusers.Rows.Count > 0)
                {
                    dlUser11.DataSource = dsMusers;
                    dlUser11.DataTextField = "Name";
                    dlUser11.DataValueField = "UserUID";
                    dlUser11.DataBind();
                    foreach (ListItem item in dlUser11.Items)
                        item.Selected = true;
                }
                else
                {
                    dlUser11.DataSource = ds;
                    dlUser11.DataTextField = "UserName";
                    dlUser11.DataValueField = "UserUID";
                    dlUser11.DataBind();
                }
                // dlUser11.Items.Insert(0, new ListItem("--Select--", ""));
                dsMusers = getdata.FlowMasterUser_Select(new Guid(DDLDocumentFlow.SelectedValue), new Guid(WorkPackageUID), new Guid(DDLDocumentCategory.SelectedValue), 12);
                if (dsMusers.Rows.Count > 0)
                {
                    dlUser12.DataSource = dsMusers;
                    dlUser12.DataTextField = "Name";
                    dlUser12.DataValueField = "UserUID";
                    dlUser12.DataBind();
                    foreach (ListItem item in dlUser12.Items)
                        item.Selected = true;
                }
                else
                {
                    dlUser12.DataSource = ds;
                    dlUser12.DataTextField = "UserName";
                    dlUser12.DataValueField = "UserUID";
                    dlUser12.DataBind();
                }
                //  dlUser12.Items.Insert(0, new ListItem("--Select--", ""));
                dsMusers = getdata.FlowMasterUser_Select(new Guid(DDLDocumentFlow.SelectedValue), new Guid(WorkPackageUID), new Guid(DDLDocumentCategory.SelectedValue), 13);
                if (dsMusers.Rows.Count > 0)
                {
                    dlUser13.DataSource = dsMusers;
                    dlUser13.DataTextField = "Name";
                    dlUser13.DataValueField = "UserUID";
                    dlUser13.DataBind();
                    foreach (ListItem item in dlUser13.Items)
                        item.Selected = true;
                }
                else
                {
                    dlUser13.DataSource = ds;
                    dlUser13.DataTextField = "UserName";
                    dlUser13.DataValueField = "UserUID";
                    dlUser13.DataBind();
                }
                // dlUser13.Items.Insert(0, new ListItem("--Select--", ""));
                dsMusers = getdata.FlowMasterUser_Select(new Guid(DDLDocumentFlow.SelectedValue), new Guid(WorkPackageUID), new Guid(DDLDocumentCategory.SelectedValue), 14);
                if (dsMusers.Rows.Count > 0)
                {
                    dlUser14.DataSource = dsMusers;
                    dlUser14.DataTextField = "Name";
                    dlUser14.DataValueField = "UserUID";
                    dlUser14.DataBind();
                    foreach (ListItem item in dlUser14.Items)
                        item.Selected = true;
                }
                else
                {
                    dlUser14.DataSource = ds;
                    dlUser14.DataTextField = "UserName";
                    dlUser14.DataValueField = "UserUID";
                    dlUser14.DataBind();
                }
                // dlUser14.Items.Insert(0, new ListItem("--Select--", ""));
                dsMusers = getdata.FlowMasterUser_Select(new Guid(DDLDocumentFlow.SelectedValue), new Guid(WorkPackageUID), new Guid(DDLDocumentCategory.SelectedValue), 15);
                if (dsMusers.Rows.Count > 0)
                {
                    dlUser15.DataSource = dsMusers;
                    dlUser15.DataTextField = "Name";
                    dlUser15.DataValueField = "UserUID";
                    dlUser15.DataBind();
                    foreach (ListItem item in dlUser15.Items)
                        item.Selected = true;
                }
                else
                {
                    dlUser15.DataSource = ds;
                    dlUser15.DataTextField = "UserName";
                    dlUser15.DataValueField = "UserUID";
                    dlUser15.DataBind();
                }
                // dlUser15.Items.Insert(0, new ListItem("--Select--", ""));

                dsMusers = getdata.FlowMasterUser_Select(new Guid(DDLDocumentFlow.SelectedValue), new Guid(WorkPackageUID), new Guid(DDLDocumentCategory.SelectedValue), 16);
                if (dsMusers.Rows.Count > 0)
                {
                    dlUser16.DataSource = dsMusers;
                    dlUser16.DataTextField = "Name";
                    dlUser16.DataValueField = "UserUID";
                    dlUser16.DataBind();
                    foreach (ListItem item in dlUser16.Items)
                        item.Selected = true;
                }
                else
                {
                    dlUser16.DataSource = ds;
                    dlUser16.DataTextField = "UserName";
                    dlUser16.DataValueField = "UserUID";
                    dlUser16.DataBind();
                }
                // dlUser16.Items.Insert(0, new ListItem("--Select--", ""));
                dsMusers = getdata.FlowMasterUser_Select(new Guid(DDLDocumentFlow.SelectedValue), new Guid(WorkPackageUID), new Guid(DDLDocumentCategory.SelectedValue), 17);
                if (dsMusers.Rows.Count > 0)
                {
                    dlUser17.DataSource = dsMusers;
                    dlUser17.DataTextField = "Name";
                    dlUser17.DataValueField = "UserUID";
                    dlUser17.DataBind();
                    foreach (ListItem item in dlUser17.Items)
                        item.Selected = true;
                }
                else
                {
                    dlUser17.DataSource = ds;
                    dlUser17.DataTextField = "UserName";
                    dlUser17.DataValueField = "UserUID";
                    dlUser17.DataBind();
                }
                // dlUser17.Items.Insert(0, new ListItem("--Select--", ""));
                dsMusers = getdata.FlowMasterUser_Select(new Guid(DDLDocumentFlow.SelectedValue), new Guid(WorkPackageUID), new Guid(DDLDocumentCategory.SelectedValue), 18);
                if (dsMusers.Rows.Count > 0)
                {
                    dlUser18.DataSource = dsMusers;
                    dlUser18.DataTextField = "Name";
                    dlUser18.DataValueField = "UserUID";
                    dlUser18.DataBind();
                    foreach (ListItem item in dlUser18.Items)
                        item.Selected = true;
                }
                else
                {
                    dlUser18.DataSource = ds;
                    dlUser18.DataTextField = "UserName";
                    dlUser18.DataValueField = "UserUID";
                    dlUser18.DataBind();
                }
                // dlUser18.Items.Insert(0, new ListItem("--Select--", ""));
                dsMusers = getdata.FlowMasterUser_Select(new Guid(DDLDocumentFlow.SelectedValue), new Guid(WorkPackageUID), new Guid(DDLDocumentCategory.SelectedValue), 19);
                if (dsMusers.Rows.Count > 0)
                {
                    dlUser19.DataSource = dsMusers;
                    dlUser19.DataTextField = "Name";
                    dlUser19.DataValueField = "UserUID";
                    dlUser19.DataBind();
                    foreach (ListItem item in dlUser19.Items)
                        item.Selected = true;
                }
                else
                {
                    dlUser19.DataSource = ds;
                    dlUser19.DataTextField = "UserName";
                    dlUser19.DataValueField = "UserUID";
                    dlUser19.DataBind();
                }
                // dlUser19.Items.Insert(0, new ListItem("--Select--", ""));
                dsMusers = getdata.FlowMasterUser_Select(new Guid(DDLDocumentFlow.SelectedValue), new Guid(WorkPackageUID), new Guid(DDLDocumentCategory.SelectedValue), 20);
                if (dsMusers.Rows.Count > 0)
                {
                    dlUser20.DataSource = dsMusers;
                    dlUser20.DataTextField = "Name";
                    dlUser20.DataValueField = "UserUID";
                    dlUser20.DataBind();
                    foreach (ListItem item in dlUser20.Items)
                        item.Selected = true;
                }
                else
                {
                    dlUser20.DataSource = ds;
                    dlUser20.DataTextField = "UserName";
                    dlUser20.DataValueField = "UserUID";
                    dlUser20.DataBind();
                }
              //  dlUser20.Items.Insert(0, new ListItem("--Select--", ""));

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
                    //
                    S6Display.Visible = false;
                    S6Date.Visible = false;
                    S7Display.Visible = false;
                    S7Date.Visible = false;
                    S8Display.Visible = false;
                    S8Date.Visible = false;
                    S9Display.Visible = false;
                    S9Date.Visible = false;
                    S10Display.Visible = false;
                    S10Date.Visible = false;
                    S11Display.Visible = false;
                    S11Date.Visible = false;
                    S12Display.Visible = false;
                    S12Date.Visible = false;
                    S13Display.Visible = false;
                    S13Date.Visible = false;
                    S14Display.Visible = false;
                    S14Date.Visible = false;
                    S15Display.Visible = false;
                    S15Date.Visible = false;
                    S16Display.Visible = false;
                    S16Date.Visible = false;
                    S17Display.Visible = false;
                    S17Date.Visible = false;
                    S18Display.Visible = false;
                    S18Date.Visible = false;
                    S19Display.Visible = false;
                    S19Date.Visible = false;
                    S20Display.Visible = false;
                    S20Date.Visible = false;

                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error : " + ex.Message + "');</script>");
            }
            
        }

        public void DocumentFlowChanged()
        {
            LoadDropDowns();
            //hide all displays at start
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
            S6Display.Visible = false;
            S6Date.Visible = false;
            S7Display.Visible = false;
            S7Date.Visible = false;
            S8Display.Visible = false;
            S8Date.Visible = false;
            S9Display.Visible = false;
            S9Date.Visible = false;
            S10Display.Visible = false;
            S10Date.Visible = false;
            S11Display.Visible = false;
            S11Date.Visible = false;
            S12Display.Visible = false;
            S12Date.Visible = false;
            S13Display.Visible = false;
            S13Date.Visible = false;
            S14Display.Visible = false;
            S14Date.Visible = false;
            S15Display.Visible = false;
            S15Date.Visible = false;
            S16Display.Visible = false;
            S16Date.Visible = false;
            S17Display.Visible = false;
            S17Date.Visible = false;
            S18Display.Visible = false;
            S18Date.Visible = false;
            S19Display.Visible = false;
            S19Date.Visible = false;
            S20Display.Visible = false;
            S20Date.Visible = false;
            //

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
                else if (ds.Tables[0].Rows[0]["Steps_Count"].ToString() == "5")
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
                else if (ds.Tables[0].Rows[0]["Steps_Count"].ToString() == "6")
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
                    S6Display.Visible = true;
                    S6Date.Visible = true;
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
                    lblStep6Display.Text = ds.Tables[0].Rows[0]["FlowStep6_DisplayName"].ToString();
                    lblStep6Date.Text = ds.Tables[0].Rows[0]["FlowStep6_DisplayName"].ToString() + " Target Date";
                }
                else if (ds.Tables[0].Rows[0]["Steps_Count"].ToString() == "7")
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
                    S6Display.Visible = true;
                    S6Date.Visible = true;
                    S7Display.Visible = true;
                    S7Date.Visible = true;
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
                    lblStep6Display.Text = ds.Tables[0].Rows[0]["FlowStep6_DisplayName"].ToString();
                    lblStep6Date.Text = ds.Tables[0].Rows[0]["FlowStep6_DisplayName"].ToString() + " Target Date";
                    lblStep7Display.Text = ds.Tables[0].Rows[0]["FlowStep7_DisplayName"].ToString();
                    lblStep7Date.Text = ds.Tables[0].Rows[0]["FlowStep7_DisplayName"].ToString() + " Target Date";

                }
                else if (ds.Tables[0].Rows[0]["Steps_Count"].ToString() == "8")
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
                    S6Display.Visible = true;
                    S6Date.Visible = true;
                    S7Display.Visible = true;
                    S7Date.Visible = true;
                    S8Display.Visible = true;
                    S8Date.Visible = true;
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
                    lblStep6Display.Text = ds.Tables[0].Rows[0]["FlowStep6_DisplayName"].ToString();
                    lblStep6Date.Text = ds.Tables[0].Rows[0]["FlowStep6_DisplayName"].ToString() + " Target Date";
                    lblStep7Display.Text = ds.Tables[0].Rows[0]["FlowStep7_DisplayName"].ToString();
                    lblStep7Date.Text = ds.Tables[0].Rows[0]["FlowStep7_DisplayName"].ToString() + " Target Date";
                    lblStep8Display.Text = ds.Tables[0].Rows[0]["FlowStep8_DisplayName"].ToString();
                    lblStep8Date.Text = ds.Tables[0].Rows[0]["FlowStep8_DisplayName"].ToString() + " Target Date";
                }
                else if (ds.Tables[0].Rows[0]["Steps_Count"].ToString() == "9")
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
                    S6Display.Visible = true;
                    S6Date.Visible = true;
                    S7Display.Visible = true;
                    S7Date.Visible = true;
                    S8Display.Visible = true;
                    S8Date.Visible = true;
                    S9Display.Visible = true;
                    S9Date.Visible = true;
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
                    lblStep6Display.Text = ds.Tables[0].Rows[0]["FlowStep6_DisplayName"].ToString();
                    lblStep6Date.Text = ds.Tables[0].Rows[0]["FlowStep6_DisplayName"].ToString() + " Target Date";
                    lblStep7Display.Text = ds.Tables[0].Rows[0]["FlowStep7_DisplayName"].ToString();
                    lblStep7Date.Text = ds.Tables[0].Rows[0]["FlowStep7_DisplayName"].ToString() + " Target Date";
                    lblStep8Display.Text = ds.Tables[0].Rows[0]["FlowStep8_DisplayName"].ToString();
                    lblStep8Date.Text = ds.Tables[0].Rows[0]["FlowStep8_DisplayName"].ToString() + " Target Date";
                    lblStep9Display.Text = ds.Tables[0].Rows[0]["FlowStep9_DisplayName"].ToString();
                    lblStep9Date.Text = ds.Tables[0].Rows[0]["FlowStep9_DisplayName"].ToString() + " Target Date";
                }
                else if (ds.Tables[0].Rows[0]["Steps_Count"].ToString() == "10")
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
                    S6Display.Visible = true;
                    S6Date.Visible = true;
                    S7Display.Visible = true;
                    S7Date.Visible = true;
                    S8Display.Visible = true;
                    S8Date.Visible = true;
                    S9Display.Visible = true;
                    S9Date.Visible = true;
                    S10Display.Visible = true;
                    S10Date.Visible = true;
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
                    lblStep6Display.Text = ds.Tables[0].Rows[0]["FlowStep6_DisplayName"].ToString();
                    lblStep6Date.Text = ds.Tables[0].Rows[0]["FlowStep6_DisplayName"].ToString() + " Target Date";
                    lblStep7Display.Text = ds.Tables[0].Rows[0]["FlowStep7_DisplayName"].ToString();
                    lblStep7Date.Text = ds.Tables[0].Rows[0]["FlowStep7_DisplayName"].ToString() + " Target Date";
                    lblStep8Display.Text = ds.Tables[0].Rows[0]["FlowStep8_DisplayName"].ToString();
                    lblStep8Date.Text = ds.Tables[0].Rows[0]["FlowStep8_DisplayName"].ToString() + " Target Date";
                    lblStep9Display.Text = ds.Tables[0].Rows[0]["FlowStep9_DisplayName"].ToString();
                    lblStep9Date.Text = ds.Tables[0].Rows[0]["FlowStep9_DisplayName"].ToString() + " Target Date";
                    lblStep10Display.Text = ds.Tables[0].Rows[0]["FlowStep10_DisplayName"].ToString();
                    lblStep10Date.Text = ds.Tables[0].Rows[0]["FlowStep10_DisplayName"].ToString() + " Target Date";
                }
                else if (ds.Tables[0].Rows[0]["Steps_Count"].ToString() == "11")
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
                    S6Display.Visible = true;
                    S6Date.Visible = true;
                    S7Display.Visible = true;
                    S7Date.Visible = true;
                    S8Display.Visible = true;
                    S8Date.Visible = true;
                    S9Display.Visible = true;
                    S9Date.Visible = true;
                    S10Display.Visible = true;
                    S10Date.Visible = true;
                    S11Display.Visible = true;
                    S11Date.Visible = true;
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
                    lblStep6Display.Text = ds.Tables[0].Rows[0]["FlowStep6_DisplayName"].ToString();
                    lblStep6Date.Text = ds.Tables[0].Rows[0]["FlowStep6_DisplayName"].ToString() + " Target Date";
                    lblStep7Display.Text = ds.Tables[0].Rows[0]["FlowStep7_DisplayName"].ToString();
                    lblStep7Date.Text = ds.Tables[0].Rows[0]["FlowStep7_DisplayName"].ToString() + " Target Date";
                    lblStep8Display.Text = ds.Tables[0].Rows[0]["FlowStep8_DisplayName"].ToString();
                    lblStep8Date.Text = ds.Tables[0].Rows[0]["FlowStep8_DisplayName"].ToString() + " Target Date";
                    lblStep9Display.Text = ds.Tables[0].Rows[0]["FlowStep9_DisplayName"].ToString();
                    lblStep9Date.Text = ds.Tables[0].Rows[0]["FlowStep9_DisplayName"].ToString() + " Target Date";
                    lblStep10Display.Text = ds.Tables[0].Rows[0]["FlowStep10_DisplayName"].ToString();
                    lblStep10Date.Text = ds.Tables[0].Rows[0]["FlowStep10_DisplayName"].ToString() + " Target Date";
                    lblStep11Display.Text = ds.Tables[0].Rows[0]["FlowStep11_DisplayName"].ToString();
                    lblStep11Date.Text = ds.Tables[0].Rows[0]["FlowStep11_DisplayName"].ToString() + " Target Date";
                }
                else if (ds.Tables[0].Rows[0]["Steps_Count"].ToString() == "12")
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
                    S6Display.Visible = true;
                    S6Date.Visible = true;
                    S7Display.Visible = true;
                    S7Date.Visible = true;
                    S8Display.Visible = true;
                    S8Date.Visible = true;
                    S9Display.Visible = true;
                    S9Date.Visible = true;
                    S10Display.Visible = true;
                    S10Date.Visible = true;
                    S11Display.Visible = true;
                    S11Date.Visible = true;
                    S12Display.Visible = true;
                    S12Date.Visible = true;
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
                    lblStep6Display.Text = ds.Tables[0].Rows[0]["FlowStep6_DisplayName"].ToString();
                    lblStep6Date.Text = ds.Tables[0].Rows[0]["FlowStep6_DisplayName"].ToString() + " Target Date";
                    lblStep7Display.Text = ds.Tables[0].Rows[0]["FlowStep7_DisplayName"].ToString();
                    lblStep7Date.Text = ds.Tables[0].Rows[0]["FlowStep7_DisplayName"].ToString() + " Target Date";
                    lblStep8Display.Text = ds.Tables[0].Rows[0]["FlowStep8_DisplayName"].ToString();
                    lblStep8Date.Text = ds.Tables[0].Rows[0]["FlowStep8_DisplayName"].ToString() + " Target Date";
                    lblStep9Display.Text = ds.Tables[0].Rows[0]["FlowStep9_DisplayName"].ToString();
                    lblStep9Date.Text = ds.Tables[0].Rows[0]["FlowStep9_DisplayName"].ToString() + " Target Date";
                    lblStep10Display.Text = ds.Tables[0].Rows[0]["FlowStep10_DisplayName"].ToString();
                    lblStep10Date.Text = ds.Tables[0].Rows[0]["FlowStep10_DisplayName"].ToString() + " Target Date";
                    lblStep11Display.Text = ds.Tables[0].Rows[0]["FlowStep11_DisplayName"].ToString();
                    lblStep11Date.Text = ds.Tables[0].Rows[0]["FlowStep11_DisplayName"].ToString() + " Target Date";
                    lblStep12Display.Text = ds.Tables[0].Rows[0]["FlowStep12_DisplayName"].ToString();
                    lblStep12Date.Text = ds.Tables[0].Rows[0]["FlowStep12_DisplayName"].ToString() + " Target Date";
                }
                else if (ds.Tables[0].Rows[0]["Steps_Count"].ToString() == "13")
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
                    S6Display.Visible = true;
                    S6Date.Visible = true;
                    S7Display.Visible = true;
                    S7Date.Visible = true;
                    S8Display.Visible = true;
                    S8Date.Visible = true;
                    S9Display.Visible = true;
                    S9Date.Visible = true;
                    S10Display.Visible = true;
                    S10Date.Visible = true;
                    S11Display.Visible = true;
                    S11Date.Visible = true;
                    S12Display.Visible = true;
                    S12Date.Visible = true;
                    S13Display.Visible = true;
                    S13Date.Visible = true;
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
                    lblStep6Display.Text = ds.Tables[0].Rows[0]["FlowStep6_DisplayName"].ToString();
                    lblStep6Date.Text = ds.Tables[0].Rows[0]["FlowStep6_DisplayName"].ToString() + " Target Date";
                    lblStep7Display.Text = ds.Tables[0].Rows[0]["FlowStep7_DisplayName"].ToString();
                    lblStep7Date.Text = ds.Tables[0].Rows[0]["FlowStep7_DisplayName"].ToString() + " Target Date";
                    lblStep8Display.Text = ds.Tables[0].Rows[0]["FlowStep8_DisplayName"].ToString();
                    lblStep8Date.Text = ds.Tables[0].Rows[0]["FlowStep8_DisplayName"].ToString() + " Target Date";
                    lblStep9Display.Text = ds.Tables[0].Rows[0]["FlowStep9_DisplayName"].ToString();
                    lblStep9Date.Text = ds.Tables[0].Rows[0]["FlowStep9_DisplayName"].ToString() + " Target Date";
                    lblStep10Display.Text = ds.Tables[0].Rows[0]["FlowStep10_DisplayName"].ToString();
                    lblStep10Date.Text = ds.Tables[0].Rows[0]["FlowStep10_DisplayName"].ToString() + " Target Date";
                    lblStep11Display.Text = ds.Tables[0].Rows[0]["FlowStep11_DisplayName"].ToString();
                    lblStep11Date.Text = ds.Tables[0].Rows[0]["FlowStep11_DisplayName"].ToString() + " Target Date";
                    lblStep12Display.Text = ds.Tables[0].Rows[0]["FlowStep12_DisplayName"].ToString();
                    lblStep12Date.Text = ds.Tables[0].Rows[0]["FlowStep12_DisplayName"].ToString() + " Target Date";
                    lblStep13Display.Text = ds.Tables[0].Rows[0]["FlowStep13_DisplayName"].ToString();
                    lblStep13Date.Text = ds.Tables[0].Rows[0]["FlowStep13_DisplayName"].ToString() + " Target Date";
                }
                else if (ds.Tables[0].Rows[0]["Steps_Count"].ToString() == "14")
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
                    S6Display.Visible = true;
                    S6Date.Visible = true;
                    S7Display.Visible = true;
                    S7Date.Visible = true;
                    S8Display.Visible = true;
                    S8Date.Visible = true;
                    S9Display.Visible = true;
                    S9Date.Visible = true;
                    S10Display.Visible = true;
                    S10Date.Visible = true;
                    S11Display.Visible = true;
                    S11Date.Visible = true;
                    S12Display.Visible = true;
                    S12Date.Visible = true;
                    S13Display.Visible = true;
                    S13Date.Visible = true;
                    S14Display.Visible = true;
                    S14Date.Visible = true;
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
                    lblStep6Display.Text = ds.Tables[0].Rows[0]["FlowStep6_DisplayName"].ToString();
                    lblStep6Date.Text = ds.Tables[0].Rows[0]["FlowStep6_DisplayName"].ToString() + " Target Date";
                    lblStep7Display.Text = ds.Tables[0].Rows[0]["FlowStep7_DisplayName"].ToString();
                    lblStep7Date.Text = ds.Tables[0].Rows[0]["FlowStep7_DisplayName"].ToString() + " Target Date";
                    lblStep8Display.Text = ds.Tables[0].Rows[0]["FlowStep8_DisplayName"].ToString();
                    lblStep8Date.Text = ds.Tables[0].Rows[0]["FlowStep8_DisplayName"].ToString() + " Target Date";
                    lblStep9Display.Text = ds.Tables[0].Rows[0]["FlowStep9_DisplayName"].ToString();
                    lblStep9Date.Text = ds.Tables[0].Rows[0]["FlowStep9_DisplayName"].ToString() + " Target Date";
                    lblStep10Display.Text = ds.Tables[0].Rows[0]["FlowStep10_DisplayName"].ToString();
                    lblStep10Date.Text = ds.Tables[0].Rows[0]["FlowStep10_DisplayName"].ToString() + " Target Date";
                    lblStep11Display.Text = ds.Tables[0].Rows[0]["FlowStep11_DisplayName"].ToString();
                    lblStep11Date.Text = ds.Tables[0].Rows[0]["FlowStep11_DisplayName"].ToString() + " Target Date";
                    lblStep12Display.Text = ds.Tables[0].Rows[0]["FlowStep12_DisplayName"].ToString();
                    lblStep12Date.Text = ds.Tables[0].Rows[0]["FlowStep12_DisplayName"].ToString() + " Target Date";
                    lblStep13Display.Text = ds.Tables[0].Rows[0]["FlowStep13_DisplayName"].ToString();
                    lblStep13Date.Text = ds.Tables[0].Rows[0]["FlowStep13_DisplayName"].ToString() + " Target Date";
                    lblStep14Display.Text = ds.Tables[0].Rows[0]["FlowStep14_DisplayName"].ToString();
                    lblStep14Date.Text = ds.Tables[0].Rows[0]["FlowStep14_DisplayName"].ToString() + " Target Date";
                }
                else if (ds.Tables[0].Rows[0]["Steps_Count"].ToString() == "15")
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
                    S6Display.Visible = true;
                    S6Date.Visible = true;
                    S7Display.Visible = true;
                    S7Date.Visible = true;
                    S8Display.Visible = true;
                    S8Date.Visible = true;
                    S9Display.Visible = true;
                    S9Date.Visible = true;
                    S10Display.Visible = true;
                    S10Date.Visible = true;
                    S11Display.Visible = true;
                    S11Date.Visible = true;
                    S12Display.Visible = true;
                    S12Date.Visible = true;
                    S13Display.Visible = true;
                    S13Date.Visible = true;
                    S14Display.Visible = true;
                    S14Date.Visible = true;
                    S15Display.Visible = true;
                    S15Date.Visible = true;
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
                    lblStep6Display.Text = ds.Tables[0].Rows[0]["FlowStep6_DisplayName"].ToString();
                    lblStep6Date.Text = ds.Tables[0].Rows[0]["FlowStep6_DisplayName"].ToString() + " Target Date";
                    lblStep7Display.Text = ds.Tables[0].Rows[0]["FlowStep7_DisplayName"].ToString();
                    lblStep7Date.Text = ds.Tables[0].Rows[0]["FlowStep7_DisplayName"].ToString() + " Target Date";
                    lblStep8Display.Text = ds.Tables[0].Rows[0]["FlowStep8_DisplayName"].ToString();
                    lblStep8Date.Text = ds.Tables[0].Rows[0]["FlowStep8_DisplayName"].ToString() + " Target Date";
                    lblStep9Display.Text = ds.Tables[0].Rows[0]["FlowStep9_DisplayName"].ToString();
                    lblStep9Date.Text = ds.Tables[0].Rows[0]["FlowStep9_DisplayName"].ToString() + " Target Date";
                    lblStep10Display.Text = ds.Tables[0].Rows[0]["FlowStep10_DisplayName"].ToString();
                    lblStep10Date.Text = ds.Tables[0].Rows[0]["FlowStep10_DisplayName"].ToString() + " Target Date";
                    lblStep11Display.Text = ds.Tables[0].Rows[0]["FlowStep11_DisplayName"].ToString();
                    lblStep11Date.Text = ds.Tables[0].Rows[0]["FlowStep11_DisplayName"].ToString() + " Target Date";
                    lblStep12Display.Text = ds.Tables[0].Rows[0]["FlowStep12_DisplayName"].ToString();
                    lblStep12Date.Text = ds.Tables[0].Rows[0]["FlowStep12_DisplayName"].ToString() + " Target Date";
                    lblStep13Display.Text = ds.Tables[0].Rows[0]["FlowStep13_DisplayName"].ToString();
                    lblStep13Date.Text = ds.Tables[0].Rows[0]["FlowStep13_DisplayName"].ToString() + " Target Date";
                    lblStep14Display.Text = ds.Tables[0].Rows[0]["FlowStep14_DisplayName"].ToString();
                    lblStep14Date.Text = ds.Tables[0].Rows[0]["FlowStep14_DisplayName"].ToString() + " Target Date";
                    lblStep15Display.Text = ds.Tables[0].Rows[0]["FlowStep15_DisplayName"].ToString();
                    lblStep15Date.Text = ds.Tables[0].Rows[0]["FlowStep15_DisplayName"].ToString() + " Target Date";
                }
                else if (ds.Tables[0].Rows[0]["Steps_Count"].ToString() == "16")
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
                    S6Display.Visible = true;
                    S6Date.Visible = true;
                    S7Display.Visible = true;
                    S7Date.Visible = true;
                    S8Display.Visible = true;
                    S8Date.Visible = true;
                    S9Display.Visible = true;
                    S9Date.Visible = true;
                    S10Display.Visible = true;
                    S10Date.Visible = true;
                    S11Display.Visible = true;
                    S11Date.Visible = true;
                    S12Display.Visible = true;
                    S12Date.Visible = true;
                    S13Display.Visible = true;
                    S13Date.Visible = true;
                    S14Display.Visible = true;
                    S14Date.Visible = true;
                    S15Display.Visible = true;
                    S15Date.Visible = true;
                    S16Display.Visible = true;
                    S16Date.Visible = true;
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
                    lblStep6Display.Text = ds.Tables[0].Rows[0]["FlowStep6_DisplayName"].ToString();
                    lblStep6Date.Text = ds.Tables[0].Rows[0]["FlowStep6_DisplayName"].ToString() + " Target Date";
                    lblStep7Display.Text = ds.Tables[0].Rows[0]["FlowStep7_DisplayName"].ToString();
                    lblStep7Date.Text = ds.Tables[0].Rows[0]["FlowStep7_DisplayName"].ToString() + " Target Date";
                    lblStep8Display.Text = ds.Tables[0].Rows[0]["FlowStep8_DisplayName"].ToString();
                    lblStep8Date.Text = ds.Tables[0].Rows[0]["FlowStep8_DisplayName"].ToString() + " Target Date";
                    lblStep9Display.Text = ds.Tables[0].Rows[0]["FlowStep9_DisplayName"].ToString();
                    lblStep9Date.Text = ds.Tables[0].Rows[0]["FlowStep9_DisplayName"].ToString() + " Target Date";
                    lblStep10Display.Text = ds.Tables[0].Rows[0]["FlowStep10_DisplayName"].ToString();
                    lblStep10Date.Text = ds.Tables[0].Rows[0]["FlowStep10_DisplayName"].ToString() + " Target Date";
                    lblStep11Display.Text = ds.Tables[0].Rows[0]["FlowStep11_DisplayName"].ToString();
                    lblStep11Date.Text = ds.Tables[0].Rows[0]["FlowStep11_DisplayName"].ToString() + " Target Date";
                    lblStep12Display.Text = ds.Tables[0].Rows[0]["FlowStep12_DisplayName"].ToString();
                    lblStep12Date.Text = ds.Tables[0].Rows[0]["FlowStep12_DisplayName"].ToString() + " Target Date";
                    lblStep13Display.Text = ds.Tables[0].Rows[0]["FlowStep13_DisplayName"].ToString();
                    lblStep13Date.Text = ds.Tables[0].Rows[0]["FlowStep13_DisplayName"].ToString() + " Target Date";
                    lblStep14Display.Text = ds.Tables[0].Rows[0]["FlowStep14_DisplayName"].ToString();
                    lblStep14Date.Text = ds.Tables[0].Rows[0]["FlowStep14_DisplayName"].ToString() + " Target Date";
                    lblStep15Display.Text = ds.Tables[0].Rows[0]["FlowStep15_DisplayName"].ToString();
                    lblStep15Date.Text = ds.Tables[0].Rows[0]["FlowStep15_DisplayName"].ToString() + " Target Date";
                    lblStep16Display.Text = ds.Tables[0].Rows[0]["FlowStep16_DisplayName"].ToString();
                    lblStep16Date.Text = ds.Tables[0].Rows[0]["FlowStep16_DisplayName"].ToString() + " Target Date";
                }
                else if (ds.Tables[0].Rows[0]["Steps_Count"].ToString() == "17")
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
                    S6Display.Visible = true;
                    S6Date.Visible = true;
                    S7Display.Visible = true;
                    S7Date.Visible = true;
                    S8Display.Visible = true;
                    S8Date.Visible = true;
                    S9Display.Visible = true;
                    S9Date.Visible = true;
                    S10Display.Visible = true;
                    S10Date.Visible = true;
                    S11Display.Visible = true;
                    S11Date.Visible = true;
                    S12Display.Visible = true;
                    S12Date.Visible = true;
                    S13Display.Visible = true;
                    S13Date.Visible = true;
                    S14Display.Visible = true;
                    S14Date.Visible = true;
                    S15Display.Visible = true;
                    S15Date.Visible = true;
                    S16Display.Visible = true;
                    S16Date.Visible = true;
                    S17Display.Visible = true;
                    S17Date.Visible = true;
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
                    lblStep6Display.Text = ds.Tables[0].Rows[0]["FlowStep6_DisplayName"].ToString();
                    lblStep6Date.Text = ds.Tables[0].Rows[0]["FlowStep6_DisplayName"].ToString() + " Target Date";
                    lblStep7Display.Text = ds.Tables[0].Rows[0]["FlowStep7_DisplayName"].ToString();
                    lblStep7Date.Text = ds.Tables[0].Rows[0]["FlowStep7_DisplayName"].ToString() + " Target Date";
                    lblStep8Display.Text = ds.Tables[0].Rows[0]["FlowStep8_DisplayName"].ToString();
                    lblStep8Date.Text = ds.Tables[0].Rows[0]["FlowStep8_DisplayName"].ToString() + " Target Date";
                    lblStep9Display.Text = ds.Tables[0].Rows[0]["FlowStep9_DisplayName"].ToString();
                    lblStep9Date.Text = ds.Tables[0].Rows[0]["FlowStep9_DisplayName"].ToString() + " Target Date";
                    lblStep10Display.Text = ds.Tables[0].Rows[0]["FlowStep10_DisplayName"].ToString();
                    lblStep10Date.Text = ds.Tables[0].Rows[0]["FlowStep10_DisplayName"].ToString() + " Target Date";
                    lblStep11Display.Text = ds.Tables[0].Rows[0]["FlowStep11_DisplayName"].ToString();
                    lblStep11Date.Text = ds.Tables[0].Rows[0]["FlowStep11_DisplayName"].ToString() + " Target Date";
                    lblStep12Display.Text = ds.Tables[0].Rows[0]["FlowStep12_DisplayName"].ToString();
                    lblStep12Date.Text = ds.Tables[0].Rows[0]["FlowStep12_DisplayName"].ToString() + " Target Date";
                    lblStep13Display.Text = ds.Tables[0].Rows[0]["FlowStep13_DisplayName"].ToString();
                    lblStep13Date.Text = ds.Tables[0].Rows[0]["FlowStep13_DisplayName"].ToString() + " Target Date";
                    lblStep14Display.Text = ds.Tables[0].Rows[0]["FlowStep14_DisplayName"].ToString();
                    lblStep14Date.Text = ds.Tables[0].Rows[0]["FlowStep14_DisplayName"].ToString() + " Target Date";
                    lblStep15Display.Text = ds.Tables[0].Rows[0]["FlowStep15_DisplayName"].ToString();
                    lblStep15Date.Text = ds.Tables[0].Rows[0]["FlowStep15_DisplayName"].ToString() + " Target Date";
                    lblStep16Display.Text = ds.Tables[0].Rows[0]["FlowStep16_DisplayName"].ToString();
                    lblStep16Date.Text = ds.Tables[0].Rows[0]["FlowStep16_DisplayName"].ToString() + " Target Date";
                    lblStep17Display.Text = ds.Tables[0].Rows[0]["FlowStep17_DisplayName"].ToString();
                    lblStep17Date.Text = ds.Tables[0].Rows[0]["FlowStep17_DisplayName"].ToString() + " Target Date";
                }
                else if (ds.Tables[0].Rows[0]["Steps_Count"].ToString() == "18")
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
                    S6Display.Visible = true;
                    S6Date.Visible = true;
                    S7Display.Visible = true;
                    S7Date.Visible = true;
                    S8Display.Visible = true;
                    S8Date.Visible = true;
                    S9Display.Visible = true;
                    S9Date.Visible = true;
                    S10Display.Visible = true;
                    S10Date.Visible = true;
                    S11Display.Visible = true;
                    S11Date.Visible = true;
                    S12Display.Visible = true;
                    S12Date.Visible = true;
                    S13Display.Visible = true;
                    S13Date.Visible = true;
                    S14Display.Visible = true;
                    S14Date.Visible = true;
                    S15Display.Visible = true;
                    S15Date.Visible = true;
                    S16Display.Visible = true;
                    S16Date.Visible = true;
                    S17Display.Visible = true;
                    S17Date.Visible = true;
                    S18Display.Visible = true;
                    S18Date.Visible = true;
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
                    lblStep6Display.Text = ds.Tables[0].Rows[0]["FlowStep6_DisplayName"].ToString();
                    lblStep6Date.Text = ds.Tables[0].Rows[0]["FlowStep6_DisplayName"].ToString() + " Target Date";
                    lblStep7Display.Text = ds.Tables[0].Rows[0]["FlowStep7_DisplayName"].ToString();
                    lblStep7Date.Text = ds.Tables[0].Rows[0]["FlowStep7_DisplayName"].ToString() + " Target Date";
                    lblStep8Display.Text = ds.Tables[0].Rows[0]["FlowStep8_DisplayName"].ToString();
                    lblStep8Date.Text = ds.Tables[0].Rows[0]["FlowStep8_DisplayName"].ToString() + " Target Date";
                    lblStep9Display.Text = ds.Tables[0].Rows[0]["FlowStep9_DisplayName"].ToString();
                    lblStep9Date.Text = ds.Tables[0].Rows[0]["FlowStep9_DisplayName"].ToString() + " Target Date";
                    lblStep10Display.Text = ds.Tables[0].Rows[0]["FlowStep10_DisplayName"].ToString();
                    lblStep10Date.Text = ds.Tables[0].Rows[0]["FlowStep10_DisplayName"].ToString() + " Target Date";
                    lblStep11Display.Text = ds.Tables[0].Rows[0]["FlowStep11_DisplayName"].ToString();
                    lblStep11Date.Text = ds.Tables[0].Rows[0]["FlowStep11_DisplayName"].ToString() + " Target Date";
                    lblStep12Display.Text = ds.Tables[0].Rows[0]["FlowStep12_DisplayName"].ToString();
                    lblStep12Date.Text = ds.Tables[0].Rows[0]["FlowStep12_DisplayName"].ToString() + " Target Date";
                    lblStep13Display.Text = ds.Tables[0].Rows[0]["FlowStep13_DisplayName"].ToString();
                    lblStep13Date.Text = ds.Tables[0].Rows[0]["FlowStep13_DisplayName"].ToString() + " Target Date";
                    lblStep14Display.Text = ds.Tables[0].Rows[0]["FlowStep14_DisplayName"].ToString();
                    lblStep14Date.Text = ds.Tables[0].Rows[0]["FlowStep14_DisplayName"].ToString() + " Target Date";
                    lblStep15Display.Text = ds.Tables[0].Rows[0]["FlowStep15_DisplayName"].ToString();
                    lblStep15Date.Text = ds.Tables[0].Rows[0]["FlowStep15_DisplayName"].ToString() + " Target Date";
                    lblStep16Display.Text = ds.Tables[0].Rows[0]["FlowStep16_DisplayName"].ToString();
                    lblStep16Date.Text = ds.Tables[0].Rows[0]["FlowStep16_DisplayName"].ToString() + " Target Date";
                    lblStep17Display.Text = ds.Tables[0].Rows[0]["FlowStep17_DisplayName"].ToString();
                    lblStep17Date.Text = ds.Tables[0].Rows[0]["FlowStep17_DisplayName"].ToString() + " Target Date";
                    lblStep18Display.Text = ds.Tables[0].Rows[0]["FlowStep18_DisplayName"].ToString();
                    lblStep18Date.Text = ds.Tables[0].Rows[0]["FlowStep18_DisplayName"].ToString() + " Target Date";
                }
                else if (ds.Tables[0].Rows[0]["Steps_Count"].ToString() == "19")
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
                    S6Display.Visible = true;
                    S6Date.Visible = true;
                    S7Display.Visible = true;
                    S7Date.Visible = true;
                    S8Display.Visible = true;
                    S8Date.Visible = true;
                    S9Display.Visible = true;
                    S9Date.Visible = true;
                    S10Display.Visible = true;
                    S10Date.Visible = true;
                    S11Display.Visible = true;
                    S11Date.Visible = true;
                    S12Display.Visible = true;
                    S12Date.Visible = true;
                    S13Display.Visible = true;
                    S13Date.Visible = true;
                    S14Display.Visible = true;
                    S14Date.Visible = true;
                    S15Display.Visible = true;
                    S15Date.Visible = true;
                    S16Display.Visible = true;
                    S16Date.Visible = true;
                    S17Display.Visible = true;
                    S17Date.Visible = true;
                    S18Display.Visible = true;
                    S18Date.Visible = true;
                    S19Display.Visible = true;
                    S19Date.Visible = true;
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
                    lblStep6Display.Text = ds.Tables[0].Rows[0]["FlowStep6_DisplayName"].ToString();
                    lblStep6Date.Text = ds.Tables[0].Rows[0]["FlowStep6_DisplayName"].ToString() + " Target Date";
                    lblStep7Display.Text = ds.Tables[0].Rows[0]["FlowStep7_DisplayName"].ToString();
                    lblStep7Date.Text = ds.Tables[0].Rows[0]["FlowStep7_DisplayName"].ToString() + " Target Date";
                    lblStep8Display.Text = ds.Tables[0].Rows[0]["FlowStep8_DisplayName"].ToString();
                    lblStep8Date.Text = ds.Tables[0].Rows[0]["FlowStep8_DisplayName"].ToString() + " Target Date";
                    lblStep9Display.Text = ds.Tables[0].Rows[0]["FlowStep9_DisplayName"].ToString();
                    lblStep9Date.Text = ds.Tables[0].Rows[0]["FlowStep9_DisplayName"].ToString() + " Target Date";
                    lblStep10Display.Text = ds.Tables[0].Rows[0]["FlowStep10_DisplayName"].ToString();
                    lblStep10Date.Text = ds.Tables[0].Rows[0]["FlowStep10_DisplayName"].ToString() + " Target Date";
                    lblStep11Display.Text = ds.Tables[0].Rows[0]["FlowStep11_DisplayName"].ToString();
                    lblStep11Date.Text = ds.Tables[0].Rows[0]["FlowStep11_DisplayName"].ToString() + " Target Date";
                    lblStep12Display.Text = ds.Tables[0].Rows[0]["FlowStep12_DisplayName"].ToString();
                    lblStep12Date.Text = ds.Tables[0].Rows[0]["FlowStep12_DisplayName"].ToString() + " Target Date";
                    lblStep13Display.Text = ds.Tables[0].Rows[0]["FlowStep13_DisplayName"].ToString();
                    lblStep13Date.Text = ds.Tables[0].Rows[0]["FlowStep13_DisplayName"].ToString() + " Target Date";
                    lblStep14Display.Text = ds.Tables[0].Rows[0]["FlowStep14_DisplayName"].ToString();
                    lblStep14Date.Text = ds.Tables[0].Rows[0]["FlowStep14_DisplayName"].ToString() + " Target Date";
                    lblStep15Display.Text = ds.Tables[0].Rows[0]["FlowStep15_DisplayName"].ToString();
                    lblStep15Date.Text = ds.Tables[0].Rows[0]["FlowStep15_DisplayName"].ToString() + " Target Date";
                    lblStep16Display.Text = ds.Tables[0].Rows[0]["FlowStep16_DisplayName"].ToString();
                    lblStep16Date.Text = ds.Tables[0].Rows[0]["FlowStep16_DisplayName"].ToString() + " Target Date";
                    lblStep17Display.Text = ds.Tables[0].Rows[0]["FlowStep17_DisplayName"].ToString();
                    lblStep17Date.Text = ds.Tables[0].Rows[0]["FlowStep17_DisplayName"].ToString() + " Target Date";
                    lblStep18Display.Text = ds.Tables[0].Rows[0]["FlowStep18_DisplayName"].ToString();
                    lblStep18Date.Text = ds.Tables[0].Rows[0]["FlowStep18_DisplayName"].ToString() + " Target Date";
                    lblStep19Display.Text = ds.Tables[0].Rows[0]["FlowStep19_DisplayName"].ToString();
                    lblStep19Date.Text = ds.Tables[0].Rows[0]["FlowStep19_DisplayName"].ToString() + " Target Date";
                }
                else if (ds.Tables[0].Rows[0]["Steps_Count"].ToString() == "20")
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
                    S6Display.Visible = true;
                    S6Date.Visible = true;
                    S7Display.Visible = true;
                    S7Date.Visible = true;
                    S8Display.Visible = true;
                    S8Date.Visible = true;
                    S9Display.Visible = true;
                    S9Date.Visible = true;
                    S10Display.Visible = true;
                    S10Date.Visible = true;
                    S11Display.Visible = true;
                    S11Date.Visible = true;
                    S12Display.Visible = true;
                    S12Date.Visible = true;
                    S13Display.Visible = true;
                    S13Date.Visible = true;
                    S14Display.Visible = true;
                    S14Date.Visible = true;
                    S15Display.Visible = true;
                    S15Date.Visible = true;
                    S16Display.Visible = true;
                    S16Date.Visible = true;
                    S17Display.Visible = true;
                    S17Date.Visible = true;
                    S18Display.Visible = true;
                    S18Date.Visible = true;
                    S19Display.Visible = true;
                    S19Date.Visible = true;
                    S20Display.Visible = true;
                    S20Date.Visible = true;
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
                    lblStep6Display.Text = ds.Tables[0].Rows[0]["FlowStep6_DisplayName"].ToString();
                    lblStep6Date.Text = ds.Tables[0].Rows[0]["FlowStep6_DisplayName"].ToString() + " Target Date";
                    lblStep7Display.Text = ds.Tables[0].Rows[0]["FlowStep7_DisplayName"].ToString();
                    lblStep7Date.Text = ds.Tables[0].Rows[0]["FlowStep7_DisplayName"].ToString() + " Target Date";
                    lblStep8Display.Text = ds.Tables[0].Rows[0]["FlowStep8_DisplayName"].ToString();
                    lblStep8Date.Text = ds.Tables[0].Rows[0]["FlowStep8_DisplayName"].ToString() + " Target Date";
                    lblStep9Display.Text = ds.Tables[0].Rows[0]["FlowStep9_DisplayName"].ToString();
                    lblStep9Date.Text = ds.Tables[0].Rows[0]["FlowStep9_DisplayName"].ToString() + " Target Date";
                    lblStep10Display.Text = ds.Tables[0].Rows[0]["FlowStep10_DisplayName"].ToString();
                    lblStep10Date.Text = ds.Tables[0].Rows[0]["FlowStep10_DisplayName"].ToString() + " Target Date";
                    lblStep11Display.Text = ds.Tables[0].Rows[0]["FlowStep11_DisplayName"].ToString();
                    lblStep11Date.Text = ds.Tables[0].Rows[0]["FlowStep11_DisplayName"].ToString() + " Target Date";
                    lblStep12Display.Text = ds.Tables[0].Rows[0]["FlowStep12_DisplayName"].ToString();
                    lblStep12Date.Text = ds.Tables[0].Rows[0]["FlowStep12_DisplayName"].ToString() + " Target Date";
                    lblStep13Display.Text = ds.Tables[0].Rows[0]["FlowStep13_DisplayName"].ToString();
                    lblStep13Date.Text = ds.Tables[0].Rows[0]["FlowStep13_DisplayName"].ToString() + " Target Date";
                    lblStep14Display.Text = ds.Tables[0].Rows[0]["FlowStep14_DisplayName"].ToString();
                    lblStep14Date.Text = ds.Tables[0].Rows[0]["FlowStep14_DisplayName"].ToString() + " Target Date";
                    lblStep15Display.Text = ds.Tables[0].Rows[0]["FlowStep15_DisplayName"].ToString();
                    lblStep15Date.Text = ds.Tables[0].Rows[0]["FlowStep15_DisplayName"].ToString() + " Target Date";
                    lblStep16Display.Text = ds.Tables[0].Rows[0]["FlowStep16_DisplayName"].ToString();
                    lblStep16Date.Text = ds.Tables[0].Rows[0]["FlowStep16_DisplayName"].ToString() + " Target Date";
                    lblStep17Display.Text = ds.Tables[0].Rows[0]["FlowStep17_DisplayName"].ToString();
                    lblStep17Date.Text = ds.Tables[0].Rows[0]["FlowStep17_DisplayName"].ToString() + " Target Date";
                    lblStep18Display.Text = ds.Tables[0].Rows[0]["FlowStep18_DisplayName"].ToString();
                    lblStep18Date.Text = ds.Tables[0].Rows[0]["FlowStep18_DisplayName"].ToString() + " Target Date";
                    lblStep19Display.Text = ds.Tables[0].Rows[0]["FlowStep19_DisplayName"].ToString();
                    lblStep19Date.Text = ds.Tables[0].Rows[0]["FlowStep19_DisplayName"].ToString() + " Target Date";
                    lblStep20Display.Text = ds.Tables[0].Rows[0]["FlowStep20_DisplayName"].ToString();
                    lblStep20Date.Text = ds.Tables[0].Rows[0]["FlowStep20_DisplayName"].ToString() + " Target Date";
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
                        // dtSubTargetDate.Text = CDate1.AddDays(Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep1_Duration"].ToString())).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                        dtSubTargetDate.Text = Convert.ToDateTime(getdata.CalculateHolidayOffset(CDate1, Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep1_Duration"].ToString()))).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    }
                }
                else if (ds.Tables[0].Rows[0]["Steps_Count"].ToString() == "2")
                {
                    if (ds.Tables[0].Rows[0]["FlowStep1_Duration"].ToString() != "-1")
                    {
                        //dtSubTargetDate.Text = CDate1.AddDays(Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep1_Duration"].ToString())).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                        dtSubTargetDate.Text = Convert.ToDateTime(getdata.CalculateHolidayOffset(CDate1, Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep1_Duration"].ToString()))).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    }
                    if (ds.Tables[0].Rows[0]["FlowStep2_Duration"].ToString() != "-1")
                    {
                        // dtQualTargetDate.Text = CDate1.AddDays(Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep2_Duration"].ToString())).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                        dtQualTargetDate.Text = Convert.ToDateTime(getdata.CalculateHolidayOffset(CDate1, Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep2_Duration"].ToString()))).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

                    }
                }
                else if (ds.Tables[0].Rows[0]["Steps_Count"].ToString() == "3")
                {
                    if (ds.Tables[0].Rows[0]["FlowStep1_Duration"].ToString() != "-1")
                    {
                        // dtSubTargetDate.Text = CDate1.AddDays(Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep1_Duration"].ToString())).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                        dtSubTargetDate.Text = Convert.ToDateTime(getdata.CalculateHolidayOffset(CDate1, Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep1_Duration"].ToString()))).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    }
                    if (ds.Tables[0].Rows[0]["FlowStep2_Duration"].ToString() != "-1")
                    {
                        // dtQualTargetDate.Text = CDate1.AddDays(Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep2_Duration"].ToString())).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                        dtQualTargetDate.Text = Convert.ToDateTime(getdata.CalculateHolidayOffset(CDate1, Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep2_Duration"].ToString()))).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

                    }
                    if (ds.Tables[0].Rows[0]["FlowStep3_Duration"].ToString() != "-1")
                    {
                        //dtRev_B_TargetDate.Text = CDate1.AddDays(Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep3_Duration"].ToString())).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                        dtRev_B_TargetDate.Text = Convert.ToDateTime(getdata.CalculateHolidayOffset(CDate1, Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep3_Duration"].ToString()))).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

                    }
                    //Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Success-8');</script>");
                }
                else if (ds.Tables[0].Rows[0]["Steps_Count"].ToString() == "4")
                {
                    if (ds.Tables[0].Rows[0]["FlowStep1_Duration"].ToString() != "-1")
                    {
                        // dtSubTargetDate.Text = CDate1.AddDays(Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep1_Duration"].ToString())).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                        dtSubTargetDate.Text = Convert.ToDateTime(getdata.CalculateHolidayOffset(CDate1, Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep1_Duration"].ToString()))).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    }
                    if (ds.Tables[0].Rows[0]["FlowStep2_Duration"].ToString() != "-1")
                    {
                        // dtQualTargetDate.Text = CDate1.AddDays(Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep2_Duration"].ToString())).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                        dtQualTargetDate.Text = Convert.ToDateTime(getdata.CalculateHolidayOffset(CDate1, Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep2_Duration"].ToString()))).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

                    }
                    if (ds.Tables[0].Rows[0]["FlowStep3_Duration"].ToString() != "-1")
                    {
                        // dtRev_B_TargetDate.Text = CDate1.AddDays(Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep3_Duration"].ToString())).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                        dtRev_B_TargetDate.Text = Convert.ToDateTime(getdata.CalculateHolidayOffset(CDate1, Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep3_Duration"].ToString()))).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

                    }
                    if (ds.Tables[0].Rows[0]["FlowStep4_Duration"].ToString() != "-1")
                    {
                        // dtRevTargetDate.Text = CDate1.AddDays(Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep4_Duration"].ToString())).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                        dtRevTargetDate.Text = Convert.ToDateTime(getdata.CalculateHolidayOffset(CDate1, Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep4_Duration"].ToString()))).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    }
                }
              else if (ds.Tables[0].Rows[0]["Steps_Count"].ToString() == "5")
                {
                    if (ds.Tables[0].Rows[0]["FlowStep1_Duration"].ToString() != "-1")
                    {
                        //dtSubTargetDate.Text = CDate1.AddDays(Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep1_Duration"].ToString())).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                        dtSubTargetDate.Text = Convert.ToDateTime(getdata.CalculateHolidayOffset(CDate1, Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep1_Duration"].ToString()))).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    }
                    if (ds.Tables[0].Rows[0]["FlowStep2_Duration"].ToString() != "-1")
                    {
                        // dtQualTargetDate.Text = CDate1.AddDays(Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep2_Duration"].ToString())).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                        dtQualTargetDate.Text = Convert.ToDateTime(getdata.CalculateHolidayOffset(CDate1, Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep2_Duration"].ToString()))).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

                    }
                    if (ds.Tables[0].Rows[0]["FlowStep3_Duration"].ToString() != "-1")
                    {
                        //dtRev_B_TargetDate.Text = CDate1.AddDays(Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep3_Duration"].ToString())).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                        dtRev_B_TargetDate.Text = Convert.ToDateTime(getdata.CalculateHolidayOffset(CDate1, Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep3_Duration"].ToString()))).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

                    }
                    if (ds.Tables[0].Rows[0]["FlowStep4_Duration"].ToString() != "-1")
                    {
                        // dtRevTargetDate.Text = CDate1.AddDays(Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep4_Duration"].ToString())).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                        dtRevTargetDate.Text = Convert.ToDateTime(getdata.CalculateHolidayOffset(CDate1, Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep4_Duration"].ToString()))).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);


                    }
                    if (ds.Tables[0].Rows[0]["dtAppTargetDate"].ToString() != "-1")
                    {
                        // dtAppTargetDate.Text = CDate1.AddDays(Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep5_Duration"].ToString())).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                        dtAppTargetDate.Text = Convert.ToDateTime(getdata.CalculateHolidayOffset(CDate1, Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep5_Duration"].ToString()))).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    }

                }
                else // if greater than 5
                {
                    if (ds.Tables[0].Rows[0]["FlowStep1_Duration"].ToString() != "-1")
                    {
                        // dtSubTargetDate.Text = CDate1.AddDays(Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep1_Duration"].ToString())).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                        dtSubTargetDate.Text =Convert.ToDateTime(getdata.CalculateHolidayOffset(CDate1,Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep1_Duration"].ToString()))).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    }
                    if (ds.Tables[0].Rows[0]["FlowStep2_Duration"].ToString() != "-1")
                    {
                        // dtQualTargetDate.Text = CDate1.AddDays(Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep2_Duration"].ToString())).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                        dtQualTargetDate.Text = Convert.ToDateTime(getdata.CalculateHolidayOffset(CDate1, Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep2_Duration"].ToString()))).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

                    }
                    if (ds.Tables[0].Rows[0]["FlowStep3_Duration"].ToString() != "-1")
                    {
                        //dtRev_B_TargetDate.Text = CDate1.AddDays(Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep3_Duration"].ToString())).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                        dtRev_B_TargetDate.Text = Convert.ToDateTime(getdata.CalculateHolidayOffset(CDate1, Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep3_Duration"].ToString()))).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

                    }
                    if (ds.Tables[0].Rows[0]["FlowStep4_Duration"].ToString() != "-1")
                    {
                        //dtRevTargetDate.Text = CDate1.AddDays(Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep4_Duration"].ToString())).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                        dtRevTargetDate.Text = Convert.ToDateTime(getdata.CalculateHolidayOffset(CDate1, Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep4_Duration"].ToString()))).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);


                    }
                    if (ds.Tables[0].Rows[0]["FlowStep5_Duration"].ToString() != "-1")
                    {
                        // dtAppTargetDate.Text = CDate1.AddDays(Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep5_Duration"].ToString())).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                        dtAppTargetDate.Text = Convert.ToDateTime(getdata.CalculateHolidayOffset(CDate1, Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep5_Duration"].ToString()))).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);


                    }
                    //
                    if (ds.Tables[0].Rows[0]["FlowStep6_Duration"] != DBNull.Value)
                    {
                        // dtTargetDate6.Text = CDate1.AddDays(Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep6_Duration"].ToString())).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                        dtTargetDate6.Text = Convert.ToDateTime(getdata.CalculateHolidayOffset(CDate1, Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep6_Duration"].ToString()))).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);


                    }
                    if (ds.Tables[0].Rows[0]["FlowStep7_Duration"] != DBNull.Value)
                    {
                        // dtTargetDate7.Text = CDate1.AddDays(Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep7_Duration"].ToString())).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                        dtTargetDate7.Text = Convert.ToDateTime(getdata.CalculateHolidayOffset(CDate1, Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep7_Duration"].ToString()))).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

                    }
                    if (ds.Tables[0].Rows[0]["FlowStep8_Duration"] != DBNull.Value)
                    {
                        //  dtTargetDate8.Text = CDate1.AddDays(Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep8_Duration"].ToString())).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                        dtTargetDate8.Text = Convert.ToDateTime(getdata.CalculateHolidayOffset(CDate1, Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep8_Duration"].ToString()))).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

                    }
                    if (ds.Tables[0].Rows[0]["FlowStep9_Duration"] != DBNull.Value)
                    {
                        // dtTargetDate9.Text = CDate1.AddDays(Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep9_Duration"].ToString())).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                        dtTargetDate9.Text = Convert.ToDateTime(getdata.CalculateHolidayOffset(CDate1, Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep9_Duration"].ToString()))).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

                    }
                    if (ds.Tables[0].Rows[0]["FlowStep10_Duration"] != DBNull.Value)
                    {
                        //  dtTargetDate10.Text = CDate1.AddDays(Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep10_Duration"].ToString())).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                        dtTargetDate10.Text = Convert.ToDateTime(getdata.CalculateHolidayOffset(CDate1, Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep10_Duration"].ToString()))).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

                    }
                    if (ds.Tables[0].Rows[0]["FlowStep11_Duration"] != DBNull.Value)
                    {
                        //  dtTargetDate11.Text = CDate1.AddDays(Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep11_Duration"].ToString())).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                        dtTargetDate11.Text = Convert.ToDateTime(getdata.CalculateHolidayOffset(CDate1, Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep11_Duration"].ToString()))).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

                    }
                    if (ds.Tables[0].Rows[0]["FlowStep12_Duration"] != DBNull.Value)
                    {
                        // dtTargetDate12.Text = CDate1.AddDays(Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep12_Duration"].ToString())).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                        dtTargetDate12.Text = Convert.ToDateTime(getdata.CalculateHolidayOffset(CDate1, Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep12_Duration"].ToString()))).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

                    }
                    if (ds.Tables[0].Rows[0]["FlowStep13_Duration"] != DBNull.Value)
                    {
                        //dtTargetDate13.Text = CDate1.AddDays(Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep13_Duration"].ToString())).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                        dtTargetDate13.Text = Convert.ToDateTime(getdata.CalculateHolidayOffset(CDate1, Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep13_Duration"].ToString()))).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

                    }
                    if (ds.Tables[0].Rows[0]["FlowStep14_Duration"] != DBNull.Value)
                    {
                        //dtTargetDate14.Text = CDate1.AddDays(Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep14_Duration"].ToString())).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                        dtTargetDate14.Text = Convert.ToDateTime(getdata.CalculateHolidayOffset(CDate1, Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep14_Duration"].ToString()))).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

                    }
                    if (ds.Tables[0].Rows[0]["FlowStep15_Duration"] != DBNull.Value)
                    {
                        //  dtTargetDate15.Text = CDate1.AddDays(Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep15_Duration"].ToString())).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                        dtTargetDate15.Text = Convert.ToDateTime(getdata.CalculateHolidayOffset(CDate1, Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep15_Duration"].ToString()))).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

                    }
                    if (ds.Tables[0].Rows[0]["FlowStep16_Duration"] != DBNull.Value)
                    {
                        // dtTargetDate16.Text = CDate1.AddDays(Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep16_Duration"].ToString())).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                        dtTargetDate16.Text = Convert.ToDateTime(getdata.CalculateHolidayOffset(CDate1, Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep16_Duration"].ToString()))).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

                    }
                    if (ds.Tables[0].Rows[0]["FlowStep17_Duration"] != DBNull.Value)
                    {
                        // dtTargetDate17.Text = CDate1.AddDays(Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep17_Duration"].ToString())).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                        dtTargetDate17.Text = Convert.ToDateTime(getdata.CalculateHolidayOffset(CDate1, Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep17_Duration"].ToString()))).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);


                    }
                    if (ds.Tables[0].Rows[0]["FlowStep18_Duration"] != DBNull.Value)
                    {
                        // dtTargetDate18.Text = CDate1.AddDays(Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep18_Duration"].ToString())).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                        dtTargetDate18.Text = Convert.ToDateTime(getdata.CalculateHolidayOffset(CDate1, Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep18_Duration"].ToString()))).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

                    }
                    if (ds.Tables[0].Rows[0]["FlowStep19_Duration"] != DBNull.Value)
                    {
                        // dtTargetDate19.Text = CDate1.AddDays(Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep19_Duration"].ToString())).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                        dtTargetDate19.Text = Convert.ToDateTime(getdata.CalculateHolidayOffset(CDate1, Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep19_Duration"].ToString()))).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

                    }
                    if (ds.Tables[0].Rows[0]["FlowStep20_Duration"] != DBNull.Value)
                    {
                        //dtTargetDate20.Text = CDate1.AddDays(Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep20_Duration"].ToString())).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                        dtTargetDate20.Text = Convert.ToDateTime(getdata.CalculateHolidayOffset(CDate1, Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep20_Duration"].ToString()))).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

                    }
                }
                //
            //    if(ds.Tables[0].Rows[0]["Type"].ToString() == "STP")
              //  {
                    if (Session["IsContractor"].ToString() == "Y")
                    {
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
                        S6Display.Visible = false;
                        S6Date.Visible = false;
                        S7Display.Visible = false;
                        S7Date.Visible = false;
                        S8Display.Visible = false;
                        S8Date.Visible = false;
                        S9Display.Visible = false;
                        S9Date.Visible = false;
                        S10Display.Visible = false;
                        S10Date.Visible = false;
                        S11Display.Visible = false;
                        S11Date.Visible = false;
                        S12Display.Visible = false;
                        S12Date.Visible = false;
                        S13Display.Visible = false;
                        S13Date.Visible = false;
                        S14Display.Visible = false;
                        S14Date.Visible = false;
                        S15Display.Visible = false;
                        S15Date.Visible = false;
                        S16Display.Visible = false;
                        S16Date.Visible = false;
                        S17Display.Visible = false;
                        S17Date.Visible = false;
                        S18Display.Visible = false;
                        S18Date.Visible = false;
                        S19Display.Visible = false;
                        S19Date.Visible = false;
                        S20Display.Visible = false;
                        S20Date.Visible = false;
                    }
                    //
               // }

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


                        string sDate1 = "", sDate2 = "", sDate3 = "", sDate4 = "", sDate5 = "", sDate6 = "", sDate7 = "", sDate8 = "", sDate9 = "", sDate10 = "" , sDate11 = "", sDate12 = "", sDate13 = "", sDate14 = "", sDate15 = "", sDate16 = "", sDate17 = "", sDate18 = "", sDate19 = "", sDate20 = "", DocPath = "", DocStartString = "", CoverPagePath = "";
                        DateTime CDate1 = DateTime.Now, CDate2 = DateTime.Now, CDate3 = DateTime.Now, CDate4 = DateTime.Now, CDate5 = DateTime.Now, DocStartDate = DateTime.Now, CDate6 = DateTime.Now, CDate7 = DateTime.Now, CDate8 = DateTime.Now, CDate9 = DateTime.Now, CDate10 = DateTime.Now, CDate11 = DateTime.Now, CDate12 = DateTime.Now, CDate13 = DateTime.Now, CDate14 = DateTime.Now, CDate15 = DateTime.Now, CDate16 = DateTime.Now, CDate17 = DateTime.Now, CDate18 = DateTime.Now, CDate19 = DateTime.Now, CDate20 = DateTime.Now;

                        string FlowStep1_IsMUser = "N", FlowStep2_IsMUser = "N", FlowStep3_IsMUser = "N", FlowStep4_IsMUser = "N", FlowStep5_IsMUser = "N", FlowStep6_IsMUser = "N", FlowStep7_IsMUser = "N", FlowStep8_IsMUser = "N", FlowStep9_IsMUser = "N", FlowStep10_IsMUser = "N", FlowStep11_IsMUser = "N", FlowStep12_IsMUser = "N", FlowStep13_IsMUser = "N", FlowStep14_IsMUser = "N", FlowStep15_IsMUser = "N", FlowStep16_IsMUser = "N", FlowStep17_IsMUser = "N", FlowStep18_IsMUser = "N", FlowStep19_IsMUser = "N", FlowStep20_IsMUser = "N";

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
                                "", "Submittle Document", 0.0, new Guid(DDLDocumentFlow.SelectedValue), DocStartDate, new Guid(ddlSubmissionUSer.SelectedValue), CDate1, estimateddocs, Remarks, SubDocTypeMaster, IsSync);
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
                            else if (dsFlow.Tables[0].Rows[0]["Steps_Count"].ToString() == "5")
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
                                sDate5 = dtAppTargetDate.Text != "" ? dtAppTargetDate.Text : CDate5.ToString("dd/MM/yyyy");
                                //sDate5 = sDate5.Split('/')[1] + "/" + sDate5.Split('/')[0] + "/" + sDate5.Split('/')[2];
                                sDate5 = getdata.ConvertDateFormat(sDate5);
                                CDate5 = Convert.ToDateTime(sDate5);

                                result = getdata.DoumentMaster_Insert_or_Update_Flow_4(sDocumentUID, workpackageid, projectId, TaskUID, txtDocumentName.Text, new Guid(DDLDocumentCategory.SelectedValue),
                                "", "Submittle Document", 0.0, new Guid(DDLDocumentFlow.SelectedValue), DocStartDate, new Guid(ddlSubmissionUSer.SelectedValue), CDate1,
                                new Guid(ddlQualityEngg.SelectedValue), CDate2, new Guid(ddlReviewer_B.SelectedValue), CDate3, new Guid(ddlReviewer.SelectedValue), CDate4, new Guid(ddlApproval.SelectedValue), CDate5, estimateddocs, Remarks, SubDocTypeMaster, IsSync);
                            }
                            else // for all flows with step > 5 ( 6 to 20) added on 04/03/2022
                            {

                                sDate1 = dtSubTargetDate.Text != "" ? dtSubTargetDate.Text : CDate1.ToString("dd/MM/yyyy");
                           
                                sDate1 = getdata.ConvertDateFormat(sDate1);
                                CDate1 = Convert.ToDateTime(sDate1);
                                //

                                sDate2 = dtQualTargetDate.Text != "" ? dtQualTargetDate.Text : CDate2.ToString("dd/MM/yyyy");
                                sDate2 = getdata.ConvertDateFormat(sDate2);
                                CDate2 = Convert.ToDateTime(sDate2);
                                getdata.DeleteSubmittal_MultipleUsers(sDocumentUID, 2);
                                int selectedCount = ddlQualityEngg.Items.Cast<ListItem>().Count(li => li.Selected);
                                if (selectedCount > 1)
                                {
                                   
                                    FlowStep2_IsMUser = "Y";
                                    foreach (ListItem listItem in ddlQualityEngg.Items)
                                    {
                                        if (listItem.Selected)
                                        {
                                            getdata.InsertSubmittal_MultipleUsers(Guid.NewGuid(), sDocumentUID, 2, new Guid(listItem.Value));
                                        }
                                    }
                                }

                                sDate3 = dtRev_B_TargetDate.Text != "" ? dtRev_B_TargetDate.Text : CDate3.ToString("dd/MM/yyyy");
                                sDate3 = getdata.ConvertDateFormat(sDate3);
                                CDate3 = Convert.ToDateTime(sDate3);
                                getdata.DeleteSubmittal_MultipleUsers(sDocumentUID, 3);
                                selectedCount = ddlReviewer_B.Items.Cast<ListItem>().Count(li => li.Selected);
                                if (selectedCount > 1)
                                {
                                    
                                    FlowStep3_IsMUser = "Y";
                                    foreach (ListItem listItem in ddlReviewer_B.Items)
                                    {
                                        if (listItem.Selected)
                                        {
                                            getdata.InsertSubmittal_MultipleUsers(Guid.NewGuid(), sDocumentUID, 3, new Guid(listItem.Value));
                                        }
                                    }
                                }


                                sDate4 = dtRevTargetDate.Text != "" ? dtRevTargetDate.Text : CDate4.ToString("dd/MM/yyyy");
                                sDate4 = getdata.ConvertDateFormat(sDate4);
                                CDate4 = Convert.ToDateTime(sDate4);
                                getdata.DeleteSubmittal_MultipleUsers(sDocumentUID, 4);
                                selectedCount = ddlReviewer.Items.Cast<ListItem>().Count(li => li.Selected);
                                if (selectedCount > 1)
                                {
                                   
                                    FlowStep4_IsMUser = "Y";
                                    foreach (ListItem listItem in ddlReviewer.Items)
                                    {
                                        if (listItem.Selected)
                                        {
                                            getdata.InsertSubmittal_MultipleUsers(Guid.NewGuid(), sDocumentUID, 4, new Guid(listItem.Value));
                                        }
                                    }
                                }
                                //
                                sDate5 = dtAppTargetDate.Text != "" ? dtAppTargetDate.Text : CDate5.ToString("dd/MM/yyyy");
                                sDate5 = getdata.ConvertDateFormat(sDate5);
                                CDate5 = Convert.ToDateTime(sDate5);
                                getdata.DeleteSubmittal_MultipleUsers(sDocumentUID, 5);
                                selectedCount = ddlApproval.Items.Cast<ListItem>().Count(li => li.Selected);
                                if (selectedCount > 1)
                                {
                                   
                                    FlowStep5_IsMUser = "Y";
                                    foreach (ListItem listItem in ddlApproval.Items)
                                    {
                                        if (listItem.Selected)
                                        {
                                            getdata.InsertSubmittal_MultipleUsers(Guid.NewGuid(), sDocumentUID, 5, new Guid(listItem.Value));
                                        }
                                    }
                                }
                                //--------------------------------------------- for new steps
                                sDate6 = dtTargetDate6.Text != "" ? dtTargetDate6.Text : CDate6.ToString("dd/MM/yyyy");
                                sDate6 = getdata.ConvertDateFormat(sDate6);
                                CDate6 = Convert.ToDateTime(sDate6);
                                getdata.DeleteSubmittal_MultipleUsers(sDocumentUID, 6);
                                selectedCount = dlUser6.Items.Cast<ListItem>().Count(li => li.Selected);
                                if (selectedCount > 1)
                                {
                                  
                                    FlowStep6_IsMUser = "Y";
                                    foreach (ListItem listItem in dlUser6.Items)
                                    {
                                        if (listItem.Selected)
                                        {
                                            getdata.InsertSubmittal_MultipleUsers(Guid.NewGuid(), sDocumentUID, 6, new Guid(listItem.Value));
                                        }
                                    }
                                }

                                int steps =int.Parse(dsFlow.Tables[0].Rows[0]["Steps_Count"].ToString());
                                Guid User7 = Guid.NewGuid(), User8 = Guid.NewGuid(), User9 = Guid.NewGuid(), User10 = Guid.NewGuid(), User11 = Guid.NewGuid(), User12 = Guid.NewGuid(), User13 = Guid.NewGuid(), User14 = Guid.NewGuid(), User15 = Guid.NewGuid(), User16 = Guid.NewGuid(), User17 = Guid.NewGuid(), User18 = Guid.NewGuid(), User19 = Guid.NewGuid(), User20 = Guid.NewGuid();
                                if (steps >= 7)
                                {
                                    sDate7 = dtTargetDate7.Text != "" ? dtTargetDate7.Text : CDate7.ToString("dd/MM/yyyy");
                                    sDate7 = getdata.ConvertDateFormat(sDate7);
                                    CDate7 = Convert.ToDateTime(sDate7);
                                    User7 = new Guid(dlUser7.SelectedValue);
                                    getdata.DeleteSubmittal_MultipleUsers(sDocumentUID, 7);
                                    selectedCount = dlUser7.Items.Cast<ListItem>().Count(li => li.Selected);
                                    if (selectedCount > 1)
                                    {
                                       
                                        FlowStep7_IsMUser = "Y";
                                        foreach (ListItem listItem in dlUser7.Items)
                                        {
                                            if (listItem.Selected)
                                            {
                                                getdata.InsertSubmittal_MultipleUsers(Guid.NewGuid(), sDocumentUID, 7, new Guid(listItem.Value));
                                            }
                                        }
                                    }

                                }
                                if (steps >= 8)
                                {
                                    sDate8 = dtTargetDate8.Text != "" ? dtTargetDate8.Text : CDate8.ToString("dd/MM/yyyy");
                                    sDate8 = getdata.ConvertDateFormat(sDate8);
                                    CDate8 = Convert.ToDateTime(sDate8);
                                    User8 = new Guid(dlUser8.SelectedValue);
                                    getdata.DeleteSubmittal_MultipleUsers(sDocumentUID, 8);
                                    selectedCount = dlUser8.Items.Cast<ListItem>().Count(li => li.Selected);
                                    if (selectedCount > 1)
                                    {
                                      
                                        FlowStep8_IsMUser = "Y";
                                        foreach (ListItem listItem in dlUser8.Items)
                                        {
                                            if (listItem.Selected)
                                            {
                                                getdata.InsertSubmittal_MultipleUsers(Guid.NewGuid(), sDocumentUID, 8, new Guid(listItem.Value));
                                            }
                                        }
                                    }
                                }
                                if (steps >= 9)
                                {
                                    sDate9 = dtTargetDate9.Text != "" ? dtTargetDate9.Text : CDate9.ToString("dd/MM/yyyy");
                                    sDate9 = getdata.ConvertDateFormat(sDate9);
                                    CDate9 = Convert.ToDateTime(sDate9);
                                    User9 = new Guid(dlUser9.SelectedValue);
                                    getdata.DeleteSubmittal_MultipleUsers(sDocumentUID, 9);
                                    selectedCount = dlUser9.Items.Cast<ListItem>().Count(li => li.Selected);
                                    if (selectedCount > 1)
                                    {
                                       
                                        FlowStep9_IsMUser = "Y";
                                        foreach (ListItem listItem in dlUser9.Items)
                                        {
                                            if (listItem.Selected)
                                            {
                                                getdata.InsertSubmittal_MultipleUsers(Guid.NewGuid(), sDocumentUID, 9, new Guid(listItem.Value));
                                            }
                                        }
                                    }
                                }
                                if (steps >= 10)
                                {
                                    sDate10 = dtTargetDate10.Text != "" ? dtTargetDate10.Text : CDate10.ToString("dd/MM/yyyy");
                                    sDate10 = getdata.ConvertDateFormat(sDate10);
                                    CDate10 = Convert.ToDateTime(sDate10);
                                    User10 = new Guid(dlUser10.SelectedValue);
                                    getdata.DeleteSubmittal_MultipleUsers(sDocumentUID, 10);
                                    selectedCount = dlUser10.Items.Cast<ListItem>().Count(li => li.Selected);
                                    if (selectedCount > 1)
                                    {
                                        
                                        FlowStep10_IsMUser = "Y";
                                        foreach (ListItem listItem in dlUser10.Items)
                                        {
                                            if (listItem.Selected)
                                            {
                                                getdata.InsertSubmittal_MultipleUsers(Guid.NewGuid(), sDocumentUID, 10, new Guid(listItem.Value));
                                            }
                                        }
                                    }
                                }
                                if (steps >= 11)
                                {
                                    sDate11 = dtTargetDate11.Text != "" ? dtTargetDate11.Text : CDate11.ToString("dd/MM/yyyy");
                                    sDate11 = getdata.ConvertDateFormat(sDate11);
                                    CDate11 = Convert.ToDateTime(sDate11);
                                    User11 = new Guid(dlUser11.SelectedValue);
                                    getdata.DeleteSubmittal_MultipleUsers(sDocumentUID, 11);
                                    selectedCount = dlUser11.Items.Cast<ListItem>().Count(li => li.Selected);
                                    if (selectedCount > 1)
                                    {
                                   
                                        FlowStep11_IsMUser = "Y";
                                        foreach (ListItem listItem in dlUser11.Items)
                                        {
                                            if (listItem.Selected)
                                            {
                                                getdata.InsertSubmittal_MultipleUsers(Guid.NewGuid(), sDocumentUID, 11, new Guid(listItem.Value));
                                            }
                                        }
                                    }
                                }
                                if (steps >= 12)
                                {
                                    sDate12 = dtTargetDate12.Text != "" ? dtTargetDate12.Text : CDate12.ToString("dd/MM/yyyy");
                                    sDate12 = getdata.ConvertDateFormat(sDate12);
                                    CDate12 = Convert.ToDateTime(sDate12);
                                    User12 = new Guid(dlUser12.SelectedValue);
                                    getdata.DeleteSubmittal_MultipleUsers(sDocumentUID, 12);
                                    selectedCount = dlUser12.Items.Cast<ListItem>().Count(li => li.Selected);
                                    if (selectedCount > 1)
                                    {
                                      
                                        FlowStep12_IsMUser = "Y";
                                        foreach (ListItem listItem in dlUser12.Items)
                                        {
                                            if (listItem.Selected)
                                            {
                                                getdata.InsertSubmittal_MultipleUsers(Guid.NewGuid(), sDocumentUID, 12, new Guid(listItem.Value));
                                            }
                                        }
                                    }
                                }
                                if (steps >= 13)
                                {
                                    sDate13 = dtTargetDate13.Text != "" ? dtTargetDate13.Text : CDate13.ToString("dd/MM/yyyy");
                                    sDate13 = getdata.ConvertDateFormat(sDate13);
                                    CDate13 = Convert.ToDateTime(sDate13);
                                    User13 = new Guid(dlUser13.SelectedValue);
                                    getdata.DeleteSubmittal_MultipleUsers(sDocumentUID, 13);
                                    selectedCount = dlUser13.Items.Cast<ListItem>().Count(li => li.Selected);
                                    if (selectedCount > 1)
                                    {
                                       
                                        FlowStep13_IsMUser = "Y";
                                        foreach (ListItem listItem in dlUser13.Items)
                                        {
                                            if (listItem.Selected)
                                            {
                                                getdata.InsertSubmittal_MultipleUsers(Guid.NewGuid(), sDocumentUID, 13, new Guid(listItem.Value));
                                            }
                                        }
                                    }
                                }
                                if (steps >= 14)
                                {
                                    sDate14 = dtTargetDate14.Text != "" ? dtTargetDate14.Text : CDate14.ToString("dd/MM/yyyy");
                                    sDate14 = getdata.ConvertDateFormat(sDate14);
                                    CDate14 = Convert.ToDateTime(sDate14);
                                    User14 = new Guid(dlUser14.SelectedValue);
                                    getdata.DeleteSubmittal_MultipleUsers(sDocumentUID, 14);
                                    selectedCount = dlUser14.Items.Cast<ListItem>().Count(li => li.Selected);
                                    if (selectedCount > 1)
                                    {
                                        
                                        FlowStep14_IsMUser = "Y";
                                        foreach (ListItem listItem in dlUser14.Items)
                                        {
                                            if (listItem.Selected)
                                            {
                                                getdata.InsertSubmittal_MultipleUsers(Guid.NewGuid(), sDocumentUID, 14, new Guid(listItem.Value));
                                            }
                                        }
                                    }
                                }
                                if (steps >= 15)
                                {
                                    sDate15 = dtTargetDate15.Text != "" ? dtTargetDate15.Text : CDate15.ToString("dd/MM/yyyy");
                                    sDate15 = getdata.ConvertDateFormat(sDate15);
                                    CDate15 = Convert.ToDateTime(sDate15);
                                    User15 = new Guid(dlUser15.SelectedValue);
                                    getdata.DeleteSubmittal_MultipleUsers(sDocumentUID, 15);
                                    selectedCount = dlUser15.Items.Cast<ListItem>().Count(li => li.Selected);
                                    if (selectedCount > 1)
                                    {
                                     
                                        FlowStep15_IsMUser = "Y";
                                        foreach (ListItem listItem in dlUser15.Items)
                                        {
                                            if (listItem.Selected)
                                            {
                                                getdata.InsertSubmittal_MultipleUsers(Guid.NewGuid(), sDocumentUID, 15, new Guid(listItem.Value));
                                            }
                                        }
                                    }
                                }
                                if (steps >= 16)
                                {
                                    sDate16 = dtTargetDate16.Text != "" ? dtTargetDate16.Text : CDate16.ToString("dd/MM/yyyy");
                                    sDate16 = getdata.ConvertDateFormat(sDate16);
                                    CDate16 = Convert.ToDateTime(sDate16);
                                    User16 = new Guid(dlUser16.SelectedValue);
                                    getdata.DeleteSubmittal_MultipleUsers(sDocumentUID, 16);
                                    selectedCount = dlUser16.Items.Cast<ListItem>().Count(li => li.Selected);
                                    if (selectedCount > 1)
                                    {
                                        
                                        FlowStep16_IsMUser = "Y";
                                        foreach (ListItem listItem in dlUser16.Items)
                                        {
                                            if (listItem.Selected)
                                            {
                                                getdata.InsertSubmittal_MultipleUsers(Guid.NewGuid(), sDocumentUID, 16, new Guid(listItem.Value));
                                            }
                                        }
                                    }
                                }
                                if (steps >= 17)
                                {
                                    sDate17 = dtTargetDate17.Text != "" ? dtTargetDate17.Text : CDate17.ToString("dd/MM/yyyy");
                                    sDate17 = getdata.ConvertDateFormat(sDate17);
                                    CDate17 = Convert.ToDateTime(sDate17);
                                    User17 = new Guid(dlUser17.SelectedValue);
                                    getdata.DeleteSubmittal_MultipleUsers(sDocumentUID, 17);
                                    selectedCount = dlUser17.Items.Cast<ListItem>().Count(li => li.Selected);
                                    if (selectedCount > 1)
                                    {
                                        
                                        FlowStep17_IsMUser = "Y";
                                        foreach (ListItem listItem in dlUser17.Items)
                                        {
                                            if (listItem.Selected)
                                            {
                                                getdata.InsertSubmittal_MultipleUsers(Guid.NewGuid(), sDocumentUID, 17, new Guid(listItem.Value));
                                            }
                                        }
                                    }
                                }
                                if (steps >= 18)
                                {
                                    sDate18 = dtTargetDate18.Text != "" ? dtTargetDate18.Text : CDate18.ToString("dd/MM/yyyy");
                                    sDate18 = getdata.ConvertDateFormat(sDate18);
                                    CDate18 = Convert.ToDateTime(sDate18);
                                    User18 = new Guid(dlUser18.SelectedValue);
                                    getdata.DeleteSubmittal_MultipleUsers(sDocumentUID, 18);
                                    selectedCount = dlUser18.Items.Cast<ListItem>().Count(li => li.Selected);
                                    if (selectedCount > 1)
                                    {
                                       
                                        FlowStep18_IsMUser = "Y";
                                        foreach (ListItem listItem in dlUser18.Items)
                                        {
                                            if (listItem.Selected)
                                            {
                                                getdata.InsertSubmittal_MultipleUsers(Guid.NewGuid(), sDocumentUID, 18, new Guid(listItem.Value));
                                            }
                                        }
                                    }
                                }
                                if (steps >= 19)
                                {
                                    sDate19 = dtTargetDate19.Text != "" ? dtTargetDate19.Text : CDate19.ToString("dd/MM/yyyy");
                                    sDate19 = getdata.ConvertDateFormat(sDate19);
                                    CDate19 = Convert.ToDateTime(sDate19);
                                    User19 = new Guid(dlUser19.SelectedValue);
                                    getdata.DeleteSubmittal_MultipleUsers(sDocumentUID, 19);
                                    selectedCount = dlUser19.Items.Cast<ListItem>().Count(li => li.Selected);
                                    if (selectedCount > 1)
                                    {
                                       
                                        FlowStep19_IsMUser = "Y";
                                        foreach (ListItem listItem in dlUser19.Items)
                                        {
                                            if (listItem.Selected)
                                            {
                                                getdata.InsertSubmittal_MultipleUsers(Guid.NewGuid(), sDocumentUID, 19, new Guid(listItem.Value));
                                            }
                                        }
                                    }
                                }
                                if (steps >= 20)
                                {
                                    sDate20 = dtTargetDate20.Text != "" ? dtTargetDate20.Text : CDate20.ToString("dd/MM/yyyy");
                                    sDate20 = getdata.ConvertDateFormat(sDate20);
                                    CDate20 = Convert.ToDateTime(sDate20);
                                    User20 = new Guid(dlUser20.SelectedValue);
                                    getdata.DeleteSubmittal_MultipleUsers(sDocumentUID, 20);
                                    selectedCount = dlUser20.Items.Cast<ListItem>().Count(li => li.Selected);
                                    if (selectedCount > 1)
                                    {
                                       
                                        FlowStep20_IsMUser = "Y";
                                        foreach (ListItem listItem in dlUser20.Items)
                                        {
                                            if (listItem.Selected)
                                            {
                                                getdata.InsertSubmittal_MultipleUsers(Guid.NewGuid(), sDocumentUID, 20, new Guid(listItem.Value));
                                            }
                                        }
                                    }
                                }

                                result = getdata.DoumentMaster_Insert_or_Update_FlowAll(sDocumentUID, workpackageid, projectId, TaskUID, txtDocumentName.Text, new Guid(DDLDocumentCategory.SelectedValue),
                                "", "Submittle Document", 0.0, new Guid(DDLDocumentFlow.SelectedValue), DocStartDate, new Guid(ddlSubmissionUSer.SelectedValue), CDate1,
                                new Guid(ddlQualityEngg.SelectedValue), CDate2, new Guid(ddlReviewer_B.SelectedValue), CDate3, new Guid(ddlReviewer.SelectedValue), CDate4, new Guid(ddlApproval.SelectedValue), CDate5, estimateddocs, Remarks, SubDocTypeMaster, IsSync,
                                new Guid(dlUser6.SelectedValue),CDate6,
                                User7, CDate7,
                                  User8, CDate8,
                                   User9, CDate9,
                                    User10, CDate10,
                                     User11, CDate11,
                                      User12, CDate12,
                                      User13, CDate13,
                                        User14, CDate14,
                                        User15, CDate15,
                                          User16, CDate16,
                                           User17, CDate17,
                                           User18, CDate18,
                                            User19, CDate19,
                                              User20, CDate20,
                                              int.Parse(dsFlow.Tables[0].Rows[0]["Steps_Count"].ToString()),
                                              FlowStep1_IsMUser, FlowStep2_IsMUser, FlowStep3_IsMUser, FlowStep4_IsMUser
                                              , FlowStep5_IsMUser
                                              , FlowStep6_IsMUser
                                              , FlowStep7_IsMUser
                                              , FlowStep8_IsMUser
                                              , FlowStep9_IsMUser
                                              , FlowStep10_IsMUser
                                              , FlowStep11_IsMUser
                                              , FlowStep12_IsMUser
                                              , FlowStep13_IsMUser
                                              , FlowStep14_IsMUser
                                              , FlowStep15_IsMUser
                                              , FlowStep16_IsMUser
                                              ,FlowStep17_IsMUser
                                              , FlowStep18_IsMUser
                                              , FlowStep19_IsMUser
                                              , FlowStep20_IsMUser);
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
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error:There is a problem with this feature. Please contact Administrator.Please check if users are added for flow :" + ex.Message + "');</script>");
                }
            }
        }

        protected void BindDocument()
        {
            
            //hide all displays at start
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
            S6Display.Visible = false;
            S6Date.Visible = false;
            S7Display.Visible = false;
            S7Date.Visible = false;
            S8Display.Visible = false;
            S8Date.Visible = false;
            S9Display.Visible = false;
            S9Date.Visible = false;
            S10Display.Visible = false;
            S10Date.Visible = false;
            S11Display.Visible = false;
            S11Date.Visible = false;
            S12Display.Visible = false;
            S12Date.Visible = false;
            S13Display.Visible = false;
            S13Date.Visible = false;
            S14Display.Visible = false;
            S14Date.Visible = false;
            S15Display.Visible = false;
            S15Date.Visible = false;
            S16Display.Visible = false;
            S16Date.Visible = false;
            S17Display.Visible = false;
            S17Date.Visible = false;
            S18Display.Visible = false;
            S18Date.Visible = false;
            S19Display.Visible = false;
            S19Date.Visible = false;
            S20Display.Visible = false;
            S20Date.Visible = false;
            //
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
                LoadDropDowns();
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
                    else if (dsFlow.Tables[0].Rows[0]["Steps_Count"].ToString() == "5")
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
                    else if (dsFlow.Tables[0].Rows[0]["Steps_Count"].ToString() == "6")
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
                        S6Display.Visible = true;
                        S6Date.Visible = true;
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
                        lblStep6Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep6_DisplayName"].ToString();
                        lblStep6Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep6_DisplayName"].ToString() + " Target Date";
                    }
                    else if (dsFlow.Tables[0].Rows[0]["Steps_Count"].ToString() == "7")
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
                        S6Display.Visible = true;
                        S6Date.Visible = true;
                        S7Display.Visible = true;
                        S7Date.Visible = true;
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
                        lblStep6Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep6_DisplayName"].ToString();
                        lblStep6Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep6_DisplayName"].ToString() + " Target Date";
                        lblStep7Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep7_DisplayName"].ToString();
                        lblStep7Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep7_DisplayName"].ToString() + " Target Date";

                    }
                    else if (dsFlow.Tables[0].Rows[0]["Steps_Count"].ToString() == "8")
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
                        S6Display.Visible = true;
                        S6Date.Visible = true;
                        S7Display.Visible = true;
                        S7Date.Visible = true;
                        S8Display.Visible = true;
                        S8Date.Visible = true;
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
                        lblStep6Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep6_DisplayName"].ToString();
                        lblStep6Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep6_DisplayName"].ToString() + " Target Date";
                        lblStep7Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep7_DisplayName"].ToString();
                        lblStep7Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep7_DisplayName"].ToString() + " Target Date";
                        lblStep8Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep8_DisplayName"].ToString();
                        lblStep8Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep8_DisplayName"].ToString() + " Target Date";
                    }
                    else if (dsFlow.Tables[0].Rows[0]["Steps_Count"].ToString() == "9")
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
                        S6Display.Visible = true;
                        S6Date.Visible = true;
                        S7Display.Visible = true;
                        S7Date.Visible = true;
                        S8Display.Visible = true;
                        S8Date.Visible = true;
                        S9Display.Visible = true;
                        S9Date.Visible = true;
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
                        lblStep6Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep6_DisplayName"].ToString();
                        lblStep6Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep6_DisplayName"].ToString() + " Target Date";
                        lblStep7Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep7_DisplayName"].ToString();
                        lblStep7Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep7_DisplayName"].ToString() + " Target Date";
                        lblStep8Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep8_DisplayName"].ToString();
                        lblStep8Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep8_DisplayName"].ToString() + " Target Date";
                        lblStep9Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep9_DisplayName"].ToString();
                        lblStep9Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep9_DisplayName"].ToString() + " Target Date";
                    }
                    else if (dsFlow.Tables[0].Rows[0]["Steps_Count"].ToString() == "10")
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
                        S6Display.Visible = true;
                        S6Date.Visible = true;
                        S7Display.Visible = true;
                        S7Date.Visible = true;
                        S8Display.Visible = true;
                        S8Date.Visible = true;
                        S9Display.Visible = true;
                        S9Date.Visible = true;
                        S10Display.Visible = true;
                        S10Date.Visible = true;
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
                        lblStep6Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep6_DisplayName"].ToString();
                        lblStep6Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep6_DisplayName"].ToString() + " Target Date";
                        lblStep7Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep7_DisplayName"].ToString();
                        lblStep7Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep7_DisplayName"].ToString() + " Target Date";
                        lblStep8Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep8_DisplayName"].ToString();
                        lblStep8Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep8_DisplayName"].ToString() + " Target Date";
                        lblStep9Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep9_DisplayName"].ToString();
                        lblStep9Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep9_DisplayName"].ToString() + " Target Date";
                        lblStep10Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep10_DisplayName"].ToString();
                        lblStep10Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep10_DisplayName"].ToString() + " Target Date";
                    }
                    else if (dsFlow.Tables[0].Rows[0]["Steps_Count"].ToString() == "11")
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
                        S6Display.Visible = true;
                        S6Date.Visible = true;
                        S7Display.Visible = true;
                        S7Date.Visible = true;
                        S8Display.Visible = true;
                        S8Date.Visible = true;
                        S9Display.Visible = true;
                        S9Date.Visible = true;
                        S10Display.Visible = true;
                        S10Date.Visible = true;
                        S11Display.Visible = true;
                        S11Date.Visible = true;
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
                        lblStep6Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep6_DisplayName"].ToString();
                        lblStep6Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep6_DisplayName"].ToString() + " Target Date";
                        lblStep7Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep7_DisplayName"].ToString();
                        lblStep7Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep7_DisplayName"].ToString() + " Target Date";
                        lblStep8Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep8_DisplayName"].ToString();
                        lblStep8Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep8_DisplayName"].ToString() + " Target Date";
                        lblStep9Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep9_DisplayName"].ToString();
                        lblStep9Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep9_DisplayName"].ToString() + " Target Date";
                        lblStep10Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep10_DisplayName"].ToString();
                        lblStep10Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep10_DisplayName"].ToString() + " Target Date";
                        lblStep11Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep11_DisplayName"].ToString();
                        lblStep11Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep11_DisplayName"].ToString() + " Target Date";
                    }
                    else if (dsFlow.Tables[0].Rows[0]["Steps_Count"].ToString() == "12")
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
                        S6Display.Visible = true;
                        S6Date.Visible = true;
                        S7Display.Visible = true;
                        S7Date.Visible = true;
                        S8Display.Visible = true;
                        S8Date.Visible = true;
                        S9Display.Visible = true;
                        S9Date.Visible = true;
                        S10Display.Visible = true;
                        S10Date.Visible = true;
                        S11Display.Visible = true;
                        S11Date.Visible = true;
                        S12Display.Visible = true;
                        S12Date.Visible = true;
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
                        lblStep6Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep6_DisplayName"].ToString();
                        lblStep6Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep6_DisplayName"].ToString() + " Target Date";
                        lblStep7Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep7_DisplayName"].ToString();
                        lblStep7Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep7_DisplayName"].ToString() + " Target Date";
                        lblStep8Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep8_DisplayName"].ToString();
                        lblStep8Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep8_DisplayName"].ToString() + " Target Date";
                        lblStep9Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep9_DisplayName"].ToString();
                        lblStep9Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep9_DisplayName"].ToString() + " Target Date";
                        lblStep10Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep10_DisplayName"].ToString();
                        lblStep10Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep10_DisplayName"].ToString() + " Target Date";
                        lblStep11Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep11_DisplayName"].ToString();
                        lblStep11Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep11_DisplayName"].ToString() + " Target Date";
                        lblStep12Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep12_DisplayName"].ToString();
                        lblStep12Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep12_DisplayName"].ToString() + " Target Date";
                    }
                    else if (dsFlow.Tables[0].Rows[0]["Steps_Count"].ToString() == "13")
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
                        S6Display.Visible = true;
                        S6Date.Visible = true;
                        S7Display.Visible = true;
                        S7Date.Visible = true;
                        S8Display.Visible = true;
                        S8Date.Visible = true;
                        S9Display.Visible = true;
                        S9Date.Visible = true;
                        S10Display.Visible = true;
                        S10Date.Visible = true;
                        S11Display.Visible = true;
                        S11Date.Visible = true;
                        S12Display.Visible = true;
                        S12Date.Visible = true;
                        S13Display.Visible = true;
                        S13Date.Visible = true;
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
                        lblStep6Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep6_DisplayName"].ToString();
                        lblStep6Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep6_DisplayName"].ToString() + " Target Date";
                        lblStep7Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep7_DisplayName"].ToString();
                        lblStep7Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep7_DisplayName"].ToString() + " Target Date";
                        lblStep8Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep8_DisplayName"].ToString();
                        lblStep8Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep8_DisplayName"].ToString() + " Target Date";
                        lblStep9Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep9_DisplayName"].ToString();
                        lblStep9Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep9_DisplayName"].ToString() + " Target Date";
                        lblStep10Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep10_DisplayName"].ToString();
                        lblStep10Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep10_DisplayName"].ToString() + " Target Date";
                        lblStep11Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep11_DisplayName"].ToString();
                        lblStep11Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep11_DisplayName"].ToString() + " Target Date";
                        lblStep12Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep12_DisplayName"].ToString();
                        lblStep12Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep12_DisplayName"].ToString() + " Target Date";
                        lblStep13Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep13_DisplayName"].ToString();
                        lblStep13Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep13_DisplayName"].ToString() + " Target Date";
                    }
                    else if (dsFlow.Tables[0].Rows[0]["Steps_Count"].ToString() == "14")
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
                        S6Display.Visible = true;
                        S6Date.Visible = true;
                        S7Display.Visible = true;
                        S7Date.Visible = true;
                        S8Display.Visible = true;
                        S8Date.Visible = true;
                        S9Display.Visible = true;
                        S9Date.Visible = true;
                        S10Display.Visible = true;
                        S10Date.Visible = true;
                        S11Display.Visible = true;
                        S11Date.Visible = true;
                        S12Display.Visible = true;
                        S12Date.Visible = true;
                        S13Display.Visible = true;
                        S13Date.Visible = true;
                        S14Display.Visible = true;
                        S14Date.Visible = true;
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
                        lblStep6Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep6_DisplayName"].ToString();
                        lblStep6Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep6_DisplayName"].ToString() + " Target Date";
                        lblStep7Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep7_DisplayName"].ToString();
                        lblStep7Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep7_DisplayName"].ToString() + " Target Date";
                        lblStep8Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep8_DisplayName"].ToString();
                        lblStep8Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep8_DisplayName"].ToString() + " Target Date";
                        lblStep9Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep9_DisplayName"].ToString();
                        lblStep9Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep9_DisplayName"].ToString() + " Target Date";
                        lblStep10Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep10_DisplayName"].ToString();
                        lblStep10Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep10_DisplayName"].ToString() + " Target Date";
                        lblStep11Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep11_DisplayName"].ToString();
                        lblStep11Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep11_DisplayName"].ToString() + " Target Date";
                        lblStep12Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep12_DisplayName"].ToString();
                        lblStep12Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep12_DisplayName"].ToString() + " Target Date";
                        lblStep13Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep13_DisplayName"].ToString();
                        lblStep13Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep13_DisplayName"].ToString() + " Target Date";
                        lblStep14Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep14_DisplayName"].ToString();
                        lblStep14Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep14_DisplayName"].ToString() + " Target Date";
                    }
                    else if (dsFlow.Tables[0].Rows[0]["Steps_Count"].ToString() == "15")
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
                        S6Display.Visible = true;
                        S6Date.Visible = true;
                        S7Display.Visible = true;
                        S7Date.Visible = true;
                        S8Display.Visible = true;
                        S8Date.Visible = true;
                        S9Display.Visible = true;
                        S9Date.Visible = true;
                        S10Display.Visible = true;
                        S10Date.Visible = true;
                        S11Display.Visible = true;
                        S11Date.Visible = true;
                        S12Display.Visible = true;
                        S12Date.Visible = true;
                        S13Display.Visible = true;
                        S13Date.Visible = true;
                        S14Display.Visible = true;
                        S14Date.Visible = true;
                        S15Display.Visible = true;
                        S15Date.Visible = true;
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
                        lblStep6Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep6_DisplayName"].ToString();
                        lblStep6Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep6_DisplayName"].ToString() + " Target Date";
                        lblStep7Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep7_DisplayName"].ToString();
                        lblStep7Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep7_DisplayName"].ToString() + " Target Date";
                        lblStep8Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep8_DisplayName"].ToString();
                        lblStep8Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep8_DisplayName"].ToString() + " Target Date";
                        lblStep9Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep9_DisplayName"].ToString();
                        lblStep9Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep9_DisplayName"].ToString() + " Target Date";
                        lblStep10Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep10_DisplayName"].ToString();
                        lblStep10Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep10_DisplayName"].ToString() + " Target Date";
                        lblStep11Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep11_DisplayName"].ToString();
                        lblStep11Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep11_DisplayName"].ToString() + " Target Date";
                        lblStep12Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep12_DisplayName"].ToString();
                        lblStep12Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep12_DisplayName"].ToString() + " Target Date";
                        lblStep13Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep13_DisplayName"].ToString();
                        lblStep13Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep13_DisplayName"].ToString() + " Target Date";
                        lblStep14Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep14_DisplayName"].ToString();
                        lblStep14Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep14_DisplayName"].ToString() + " Target Date";
                        lblStep15Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep15_DisplayName"].ToString();
                        lblStep15Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep15_DisplayName"].ToString() + " Target Date";
                    }
                    else if (dsFlow.Tables[0].Rows[0]["Steps_Count"].ToString() == "16")
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
                        S6Display.Visible = true;
                        S6Date.Visible = true;
                        S7Display.Visible = true;
                        S7Date.Visible = true;
                        S8Display.Visible = true;
                        S8Date.Visible = true;
                        S9Display.Visible = true;
                        S9Date.Visible = true;
                        S10Display.Visible = true;
                        S10Date.Visible = true;
                        S11Display.Visible = true;
                        S11Date.Visible = true;
                        S12Display.Visible = true;
                        S12Date.Visible = true;
                        S13Display.Visible = true;
                        S13Date.Visible = true;
                        S14Display.Visible = true;
                        S14Date.Visible = true;
                        S15Display.Visible = true;
                        S15Date.Visible = true;
                        S16Display.Visible = true;
                        S16Date.Visible = true;
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
                        lblStep6Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep6_DisplayName"].ToString();
                        lblStep6Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep6_DisplayName"].ToString() + " Target Date";
                        lblStep7Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep7_DisplayName"].ToString();
                        lblStep7Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep7_DisplayName"].ToString() + " Target Date";
                        lblStep8Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep8_DisplayName"].ToString();
                        lblStep8Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep8_DisplayName"].ToString() + " Target Date";
                        lblStep9Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep9_DisplayName"].ToString();
                        lblStep9Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep9_DisplayName"].ToString() + " Target Date";
                        lblStep10Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep10_DisplayName"].ToString();
                        lblStep10Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep10_DisplayName"].ToString() + " Target Date";
                        lblStep11Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep11_DisplayName"].ToString();
                        lblStep11Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep11_DisplayName"].ToString() + " Target Date";
                        lblStep12Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep12_DisplayName"].ToString();
                        lblStep12Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep12_DisplayName"].ToString() + " Target Date";
                        lblStep13Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep13_DisplayName"].ToString();
                        lblStep13Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep13_DisplayName"].ToString() + " Target Date";
                        lblStep14Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep14_DisplayName"].ToString();
                        lblStep14Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep14_DisplayName"].ToString() + " Target Date";
                        lblStep15Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep15_DisplayName"].ToString();
                        lblStep15Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep15_DisplayName"].ToString() + " Target Date";
                        lblStep16Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep16_DisplayName"].ToString();
                        lblStep16Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep16_DisplayName"].ToString() + " Target Date";
                    }
                    else if (dsFlow.Tables[0].Rows[0]["Steps_Count"].ToString() == "17")
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
                        S6Display.Visible = true;
                        S6Date.Visible = true;
                        S7Display.Visible = true;
                        S7Date.Visible = true;
                        S8Display.Visible = true;
                        S8Date.Visible = true;
                        S9Display.Visible = true;
                        S9Date.Visible = true;
                        S10Display.Visible = true;
                        S10Date.Visible = true;
                        S11Display.Visible = true;
                        S11Date.Visible = true;
                        S12Display.Visible = true;
                        S12Date.Visible = true;
                        S13Display.Visible = true;
                        S13Date.Visible = true;
                        S14Display.Visible = true;
                        S14Date.Visible = true;
                        S15Display.Visible = true;
                        S15Date.Visible = true;
                        S16Display.Visible = true;
                        S16Date.Visible = true;
                        S17Display.Visible = true;
                        S17Date.Visible = true;
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
                        lblStep6Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep6_DisplayName"].ToString();
                        lblStep6Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep6_DisplayName"].ToString() + " Target Date";
                        lblStep7Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep7_DisplayName"].ToString();
                        lblStep7Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep7_DisplayName"].ToString() + " Target Date";
                        lblStep8Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep8_DisplayName"].ToString();
                        lblStep8Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep8_DisplayName"].ToString() + " Target Date";
                        lblStep9Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep9_DisplayName"].ToString();
                        lblStep9Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep9_DisplayName"].ToString() + " Target Date";
                        lblStep10Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep10_DisplayName"].ToString();
                        lblStep10Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep10_DisplayName"].ToString() + " Target Date";
                        lblStep11Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep11_DisplayName"].ToString();
                        lblStep11Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep11_DisplayName"].ToString() + " Target Date";
                        lblStep12Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep12_DisplayName"].ToString();
                        lblStep12Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep12_DisplayName"].ToString() + " Target Date";
                        lblStep13Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep13_DisplayName"].ToString();
                        lblStep13Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep13_DisplayName"].ToString() + " Target Date";
                        lblStep14Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep14_DisplayName"].ToString();
                        lblStep14Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep14_DisplayName"].ToString() + " Target Date";
                        lblStep15Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep15_DisplayName"].ToString();
                        lblStep15Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep15_DisplayName"].ToString() + " Target Date";
                        lblStep16Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep16_DisplayName"].ToString();
                        lblStep16Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep16_DisplayName"].ToString() + " Target Date";
                        lblStep17Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep17_DisplayName"].ToString();
                        lblStep17Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep17_DisplayName"].ToString() + " Target Date";
                    }
                    else if (dsFlow.Tables[0].Rows[0]["Steps_Count"].ToString() == "18")
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
                        S6Display.Visible = true;
                        S6Date.Visible = true;
                        S7Display.Visible = true;
                        S7Date.Visible = true;
                        S8Display.Visible = true;
                        S8Date.Visible = true;
                        S9Display.Visible = true;
                        S9Date.Visible = true;
                        S10Display.Visible = true;
                        S10Date.Visible = true;
                        S11Display.Visible = true;
                        S11Date.Visible = true;
                        S12Display.Visible = true;
                        S12Date.Visible = true;
                        S13Display.Visible = true;
                        S13Date.Visible = true;
                        S14Display.Visible = true;
                        S14Date.Visible = true;
                        S15Display.Visible = true;
                        S15Date.Visible = true;
                        S16Display.Visible = true;
                        S16Date.Visible = true;
                        S17Display.Visible = true;
                        S17Date.Visible = true;
                        S18Display.Visible = true;
                        S18Date.Visible = true;
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
                        lblStep6Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep6_DisplayName"].ToString();
                        lblStep6Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep6_DisplayName"].ToString() + " Target Date";
                        lblStep7Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep7_DisplayName"].ToString();
                        lblStep7Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep7_DisplayName"].ToString() + " Target Date";
                        lblStep8Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep8_DisplayName"].ToString();
                        lblStep8Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep8_DisplayName"].ToString() + " Target Date";
                        lblStep9Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep9_DisplayName"].ToString();
                        lblStep9Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep9_DisplayName"].ToString() + " Target Date";
                        lblStep10Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep10_DisplayName"].ToString();
                        lblStep10Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep10_DisplayName"].ToString() + " Target Date";
                        lblStep11Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep11_DisplayName"].ToString();
                        lblStep11Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep11_DisplayName"].ToString() + " Target Date";
                        lblStep12Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep12_DisplayName"].ToString();
                        lblStep12Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep12_DisplayName"].ToString() + " Target Date";
                        lblStep13Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep13_DisplayName"].ToString();
                        lblStep13Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep13_DisplayName"].ToString() + " Target Date";
                        lblStep14Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep14_DisplayName"].ToString();
                        lblStep14Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep14_DisplayName"].ToString() + " Target Date";
                        lblStep15Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep15_DisplayName"].ToString();
                        lblStep15Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep15_DisplayName"].ToString() + " Target Date";
                        lblStep16Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep16_DisplayName"].ToString();
                        lblStep16Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep16_DisplayName"].ToString() + " Target Date";
                        lblStep17Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep17_DisplayName"].ToString();
                        lblStep17Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep17_DisplayName"].ToString() + " Target Date";
                        lblStep18Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep18_DisplayName"].ToString();
                        lblStep18Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep18_DisplayName"].ToString() + " Target Date";
                    }
                    else if (dsFlow.Tables[0].Rows[0]["Steps_Count"].ToString() == "19")
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
                        S6Display.Visible = true;
                        S6Date.Visible = true;
                        S7Display.Visible = true;
                        S7Date.Visible = true;
                        S8Display.Visible = true;
                        S8Date.Visible = true;
                        S9Display.Visible = true;
                        S9Date.Visible = true;
                        S10Display.Visible = true;
                        S10Date.Visible = true;
                        S11Display.Visible = true;
                        S11Date.Visible = true;
                        S12Display.Visible = true;
                        S12Date.Visible = true;
                        S13Display.Visible = true;
                        S13Date.Visible = true;
                        S14Display.Visible = true;
                        S14Date.Visible = true;
                        S15Display.Visible = true;
                        S15Date.Visible = true;
                        S16Display.Visible = true;
                        S16Date.Visible = true;
                        S17Display.Visible = true;
                        S17Date.Visible = true;
                        S18Display.Visible = true;
                        S18Date.Visible = true;
                        S19Display.Visible = true;
                        S19Date.Visible = true;
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
                        lblStep6Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep6_DisplayName"].ToString();
                        lblStep6Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep6_DisplayName"].ToString() + " Target Date";
                        lblStep7Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep7_DisplayName"].ToString();
                        lblStep7Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep7_DisplayName"].ToString() + " Target Date";
                        lblStep8Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep8_DisplayName"].ToString();
                        lblStep8Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep8_DisplayName"].ToString() + " Target Date";
                        lblStep9Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep9_DisplayName"].ToString();
                        lblStep9Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep9_DisplayName"].ToString() + " Target Date";
                        lblStep10Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep10_DisplayName"].ToString();
                        lblStep10Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep10_DisplayName"].ToString() + " Target Date";
                        lblStep11Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep11_DisplayName"].ToString();
                        lblStep11Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep11_DisplayName"].ToString() + " Target Date";
                        lblStep12Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep12_DisplayName"].ToString();
                        lblStep12Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep12_DisplayName"].ToString() + " Target Date";
                        lblStep13Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep13_DisplayName"].ToString();
                        lblStep13Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep13_DisplayName"].ToString() + " Target Date";
                        lblStep14Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep14_DisplayName"].ToString();
                        lblStep14Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep14_DisplayName"].ToString() + " Target Date";
                        lblStep15Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep15_DisplayName"].ToString();
                        lblStep15Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep15_DisplayName"].ToString() + " Target Date";
                        lblStep16Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep16_DisplayName"].ToString();
                        lblStep16Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep16_DisplayName"].ToString() + " Target Date";
                        lblStep17Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep17_DisplayName"].ToString();
                        lblStep17Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep17_DisplayName"].ToString() + " Target Date";
                        lblStep18Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep18_DisplayName"].ToString();
                        lblStep18Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep18_DisplayName"].ToString() + " Target Date";
                        lblStep19Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep19_DisplayName"].ToString();
                        lblStep19Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep19_DisplayName"].ToString() + " Target Date";
                    }
                    else if (dsFlow.Tables[0].Rows[0]["Steps_Count"].ToString() == "20")
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
                        S6Display.Visible = true;
                        S6Date.Visible = true;
                        S7Display.Visible = true;
                        S7Date.Visible = true;
                        S8Display.Visible = true;
                        S8Date.Visible = true;
                        S9Display.Visible = true;
                        S9Date.Visible = true;
                        S10Display.Visible = true;
                        S10Date.Visible = true;
                        S11Display.Visible = true;
                        S11Date.Visible = true;
                        S12Display.Visible = true;
                        S12Date.Visible = true;
                        S13Display.Visible = true;
                        S13Date.Visible = true;
                        S14Display.Visible = true;
                        S14Date.Visible = true;
                        S15Display.Visible = true;
                        S15Date.Visible = true;
                        S16Display.Visible = true;
                        S16Date.Visible = true;
                        S17Display.Visible = true;
                        S17Date.Visible = true;
                        S18Display.Visible = true;
                        S18Date.Visible = true;
                        S19Display.Visible = true;
                        S19Date.Visible = true;
                        S20Display.Visible = true;
                        S20Date.Visible = true;
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
                        lblStep6Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep6_DisplayName"].ToString();
                        lblStep6Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep6_DisplayName"].ToString() + " Target Date";
                        lblStep7Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep7_DisplayName"].ToString();
                        lblStep7Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep7_DisplayName"].ToString() + " Target Date";
                        lblStep8Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep8_DisplayName"].ToString();
                        lblStep8Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep8_DisplayName"].ToString() + " Target Date";
                        lblStep9Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep9_DisplayName"].ToString();
                        lblStep9Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep9_DisplayName"].ToString() + " Target Date";
                        lblStep10Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep10_DisplayName"].ToString();
                        lblStep10Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep10_DisplayName"].ToString() + " Target Date";
                        lblStep11Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep11_DisplayName"].ToString();
                        lblStep11Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep11_DisplayName"].ToString() + " Target Date";
                        lblStep12Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep12_DisplayName"].ToString();
                        lblStep12Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep12_DisplayName"].ToString() + " Target Date";
                        lblStep13Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep13_DisplayName"].ToString();
                        lblStep13Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep13_DisplayName"].ToString() + " Target Date";
                        lblStep14Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep14_DisplayName"].ToString();
                        lblStep14Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep14_DisplayName"].ToString() + " Target Date";
                        lblStep15Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep15_DisplayName"].ToString();
                        lblStep15Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep15_DisplayName"].ToString() + " Target Date";
                        lblStep16Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep16_DisplayName"].ToString();
                        lblStep16Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep16_DisplayName"].ToString() + " Target Date";
                        lblStep17Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep17_DisplayName"].ToString();
                        lblStep17Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep17_DisplayName"].ToString() + " Target Date";
                        lblStep18Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep18_DisplayName"].ToString();
                        lblStep18Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep18_DisplayName"].ToString() + " Target Date";
                        lblStep19Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep19_DisplayName"].ToString();
                        lblStep19Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep19_DisplayName"].ToString() + " Target Date";
                        lblStep20Display.Text = dsFlow.Tables[0].Rows[0]["FlowStep20_DisplayName"].ToString();
                        lblStep20Date.Text = dsFlow.Tables[0].Rows[0]["FlowStep20_DisplayName"].ToString() + " Target Date";
                    }
                    //

                    if (dsFlow.Tables[0].Rows[0]["Steps_Count"].ToString() != "-1")
                    {
                        ddlSubmissionUSer.SelectedValue = ds.Tables[0].Rows[0]["FlowStep1_UserUID"].ToString();
                       // ddlQualityEngg.SelectedValue = ds.Tables[0].Rows[0]["FlowStep2_UserUID"].ToString();
                        // step 2
                        DataSet dsMUSers = getdata.GetMultipleUsers_By_DocumentUID(new Guid(Request.QueryString["DocID"].ToString()), 2);
                        foreach (ListItem listItem in ddlQualityEngg.Items)
                        {
                            listItem.Selected = false;
                            foreach (DataRow dr in dsMUSers.Tables[0].Rows)
                            {
                                if (dr["Approver"].ToString() == listItem.Value)
                                {
                                    listItem.Selected = true;
                                }
                            }
                        }
                       // ddlReviewer_B.SelectedValue = ds.Tables[0].Rows[0]["FlowStep3_UserUID"].ToString();
                        // step 3
                     dsMUSers = getdata.GetMultipleUsers_By_DocumentUID(new Guid(Request.QueryString["DocID"].ToString()), 3);
                        foreach (ListItem listItem in ddlReviewer_B.Items)
                        {
                            listItem.Selected = false;
                            foreach (DataRow dr in dsMUSers.Tables[0].Rows)
                            {
                                if (dr["Approver"].ToString() == listItem.Value)
                                {
                                    listItem.Selected = true;
                                }
                            }
                        }
                       // ddlReviewer.SelectedValue = ds.Tables[0].Rows[0]["FlowStep4_UserUID"].ToString();
                        // step 4
                        dsMUSers = getdata.GetMultipleUsers_By_DocumentUID(new Guid(Request.QueryString["DocID"].ToString()), 4);
                        foreach (ListItem listItem in ddlReviewer.Items)
                        {
                            listItem.Selected = false;
                            foreach (DataRow dr in dsMUSers.Tables[0].Rows)
                            {
                                if (dr["Approver"].ToString() == listItem.Value)
                                {
                                    listItem.Selected = true;
                                }
                            }
                        }
                       // ddlApproval.SelectedValue = ds.Tables[0].Rows[0]["FlowStep5_UserUID"].ToString();
                        // step 5
                        dsMUSers = getdata.GetMultipleUsers_By_DocumentUID(new Guid(Request.QueryString["DocID"].ToString()), 5);
                        foreach (ListItem listItem in ddlApproval.Items)
                        {
                            listItem.Selected = false;
                            foreach (DataRow dr in dsMUSers.Tables[0].Rows)
                            {
                                if (dr["Approver"].ToString() == listItem.Value)
                                {
                                    listItem.Selected = true;
                                }
                            }
                        }
                        //
                       // dlUser6.SelectedValue = ds.Tables[0].Rows[0]["FlowStep6_UserUID"].ToString();
                        // step 6
                        dsMUSers = getdata.GetMultipleUsers_By_DocumentUID(new Guid(Request.QueryString["DocID"].ToString()), 6);
                        foreach (ListItem listItem in dlUser6.Items)
                        {
                            listItem.Selected = false;
                            foreach (DataRow dr in dsMUSers.Tables[0].Rows)
                            {
                                if (dr["Approver"].ToString() == listItem.Value)
                                {
                                    listItem.Selected = true;
                                }
                            }
                        }
                       // dlUser7.SelectedValue = ds.Tables[0].Rows[0]["FlowStep7_UserUID"].ToString();
                        // step 7
                        dsMUSers = getdata.GetMultipleUsers_By_DocumentUID(new Guid(Request.QueryString["DocID"].ToString()), 7);
                        foreach (ListItem listItem in dlUser7.Items)
                        {
                            listItem.Selected = false;
                            foreach (DataRow dr in dsMUSers.Tables[0].Rows)
                            {
                                if (dr["Approver"].ToString() == listItem.Value)
                                {
                                    listItem.Selected = true;
                                }
                            }
                        }
                       // dlUser8.SelectedValue = ds.Tables[0].Rows[0]["FlowStep8_UserUID"].ToString();
                        // step 8
                        dsMUSers = getdata.GetMultipleUsers_By_DocumentUID(new Guid(Request.QueryString["DocID"].ToString()), 8);
                        foreach (ListItem listItem in dlUser8.Items)
                        {
                            listItem.Selected = false;
                            foreach (DataRow dr in dsMUSers.Tables[0].Rows)
                            {
                                if (dr["Approver"].ToString() == listItem.Value)
                                {
                                    listItem.Selected = true;
                                }
                            }
                        }
                      //  dlUser9.SelectedValue = ds.Tables[0].Rows[0]["FlowStep9_UserUID"].ToString();
                        // step 9
                        dsMUSers = getdata.GetMultipleUsers_By_DocumentUID(new Guid(Request.QueryString["DocID"].ToString()), 9);
                        foreach (ListItem listItem in dlUser9.Items)
                        {
                            listItem.Selected = false;
                            foreach (DataRow dr in dsMUSers.Tables[0].Rows)
                            {
                                if (dr["Approver"].ToString() == listItem.Value)
                                {
                                    listItem.Selected = true;
                                }
                            }
                        }
                      //  dlUser10.SelectedValue = ds.Tables[0].Rows[0]["FlowStep10_UserUID"].ToString();
                        // step 10
                        dsMUSers = getdata.GetMultipleUsers_By_DocumentUID(new Guid(Request.QueryString["DocID"].ToString()), 10);
                        foreach (ListItem listItem in dlUser10.Items)
                        {
                            listItem.Selected = false;
                            foreach (DataRow dr in dsMUSers.Tables[0].Rows)
                            {
                                if (dr["Approver"].ToString() == listItem.Value)
                                {
                                    listItem.Selected = true;
                                }
                            }
                        }
                      //  dlUser11.SelectedValue = ds.Tables[0].Rows[0]["FlowStep11_UserUID"].ToString();
                        // step 11
                        dsMUSers = getdata.GetMultipleUsers_By_DocumentUID(new Guid(Request.QueryString["DocID"].ToString()), 11);
                        foreach (ListItem listItem in dlUser11.Items)
                        {
                            listItem.Selected = false;
                            foreach (DataRow dr in dsMUSers.Tables[0].Rows)
                            {
                                if (dr["Approver"].ToString() == listItem.Value)
                                {
                                    listItem.Selected = true;
                                }
                            }
                        }
                     //   dlUser12.SelectedValue = ds.Tables[0].Rows[0]["FlowStep12_UserUID"].ToString();
                        // step 6
                        dsMUSers = getdata.GetMultipleUsers_By_DocumentUID(new Guid(Request.QueryString["DocID"].ToString()), 12);
                        foreach (ListItem listItem in dlUser12.Items)
                        {
                            listItem.Selected = false;
                            foreach (DataRow dr in dsMUSers.Tables[0].Rows)
                            {
                                if (dr["Approver"].ToString() == listItem.Value)
                                {
                                    listItem.Selected = true;
                                }
                            }
                        }
                       // dlUser13.SelectedValue = ds.Tables[0].Rows[0]["FlowStep13_UserUID"].ToString();
                        // step 13
                        dsMUSers = getdata.GetMultipleUsers_By_DocumentUID(new Guid(Request.QueryString["DocID"].ToString()), 13);
                        foreach (ListItem listItem in dlUser13.Items)
                        {
                            listItem.Selected = false;
                            foreach (DataRow dr in dsMUSers.Tables[0].Rows)
                            {
                                if (dr["Approver"].ToString() == listItem.Value)
                                {
                                    listItem.Selected = true;
                                }
                            }
                        }
                       // dlUser14.SelectedValue = ds.Tables[0].Rows[0]["FlowStep14_UserUID"].ToString();
                        // step 14
                        dsMUSers = getdata.GetMultipleUsers_By_DocumentUID(new Guid(Request.QueryString["DocID"].ToString()), 14);
                        foreach (ListItem listItem in dlUser14.Items)
                        {
                            listItem.Selected = false;
                            foreach (DataRow dr in dsMUSers.Tables[0].Rows)
                            {
                                if (dr["Approver"].ToString() == listItem.Value)
                                {
                                    listItem.Selected = true;
                                }
                            }
                        }
                       // dlUser15.SelectedValue = ds.Tables[0].Rows[0]["FlowStep15_UserUID"].ToString();
                        // step 15
                        dsMUSers = getdata.GetMultipleUsers_By_DocumentUID(new Guid(Request.QueryString["DocID"].ToString()), 15);
                        foreach (ListItem listItem in dlUser15.Items)
                        {
                            listItem.Selected = false;
                            foreach (DataRow dr in dsMUSers.Tables[0].Rows)
                            {
                                if (dr["Approver"].ToString() == listItem.Value)
                                {
                                    listItem.Selected = true;
                                }
                            }
                        }
                     //   dlUser16.SelectedValue = ds.Tables[0].Rows[0]["FlowStep16_UserUID"].ToString();
                        // step 16
                        dsMUSers = getdata.GetMultipleUsers_By_DocumentUID(new Guid(Request.QueryString["DocID"].ToString()), 16);
                        foreach (ListItem listItem in dlUser16.Items)
                        {
                            listItem.Selected = false;
                            foreach (DataRow dr in dsMUSers.Tables[0].Rows)
                            {
                                if (dr["Approver"].ToString() == listItem.Value)
                                {
                                    listItem.Selected = true;
                                }
                            }
                        }
                     //   dlUser17.SelectedValue = ds.Tables[0].Rows[0]["FlowStep17_UserUID"].ToString();
                        // step 17
                        dsMUSers = getdata.GetMultipleUsers_By_DocumentUID(new Guid(Request.QueryString["DocID"].ToString()), 17);
                        foreach (ListItem listItem in dlUser17.Items)
                        {
                            listItem.Selected = false;
                            foreach (DataRow dr in dsMUSers.Tables[0].Rows)
                            {
                                if (dr["Approver"].ToString() == listItem.Value)
                                {
                                    listItem.Selected = true;
                                }
                            }
                        }
                       // dlUser18.SelectedValue = ds.Tables[0].Rows[0]["FlowStep18_UserUID"].ToString();
                        // step 18
                        dsMUSers = getdata.GetMultipleUsers_By_DocumentUID(new Guid(Request.QueryString["DocID"].ToString()), 18);
                        foreach (ListItem listItem in dlUser18.Items)
                        {
                            listItem.Selected = false;
                            foreach (DataRow dr in dsMUSers.Tables[0].Rows)
                            {
                                if (dr["Approver"].ToString() == listItem.Value)
                                {
                                    listItem.Selected = true;
                                }
                            }
                        }
                        //dlUser19.SelectedValue = ds.Tables[0].Rows[0]["FlowStep19_UserUID"].ToString();
                        // step 19
                        dsMUSers = getdata.GetMultipleUsers_By_DocumentUID(new Guid(Request.QueryString["DocID"].ToString()), 19);
                        foreach (ListItem listItem in dlUser19.Items)
                        {
                            listItem.Selected = false;
                            foreach (DataRow dr in dsMUSers.Tables[0].Rows)
                            {
                                if (dr["Approver"].ToString() == listItem.Value)
                                {
                                    listItem.Selected = true;
                                }
                            }
                        }
                       // dlUser20.SelectedValue = ds.Tables[0].Rows[0]["FlowStep20_UserUID"].ToString();
                        // step 20
                        dsMUSers = getdata.GetMultipleUsers_By_DocumentUID(new Guid(Request.QueryString["DocID"].ToString()), 20);
                        foreach (ListItem listItem in dlUser20.Items)
                        {
                            listItem.Selected = false;
                            foreach (DataRow dr in dsMUSers.Tables[0].Rows)
                            {
                                if (dr["Approver"].ToString() == listItem.Value)
                                {
                                    listItem.Selected = true;
                                }
                            }
                        }
                        //
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
                        if (ds.Tables[0].Rows[0]["FlowStep6_TargetDate"].ToString() != null && ds.Tables[0].Rows[0]["FlowStep6_TargetDate"].ToString() != "")
                        {

                            dtTargetDate6.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep6_TargetDate"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                        }
                        if (ds.Tables[0].Rows[0]["FlowStep7_TargetDate"].ToString() != null && ds.Tables[0].Rows[0]["FlowStep7_TargetDate"].ToString() != "")
                        {

                            dtTargetDate7.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep7_TargetDate"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                        }
                        if (ds.Tables[0].Rows[0]["FlowStep8_TargetDate"].ToString() != null && ds.Tables[0].Rows[0]["FlowStep8_TargetDate"].ToString() != "")
                        {

                            dtTargetDate8.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep8_TargetDate"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                        }
                        if (ds.Tables[0].Rows[0]["FlowStep9_TargetDate"].ToString() != null && ds.Tables[0].Rows[0]["FlowStep9_TargetDate"].ToString() != "")
                        {

                            dtTargetDate9.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep9_TargetDate"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                        }
                        if (ds.Tables[0].Rows[0]["FlowStep10_TargetDate"].ToString() != null && ds.Tables[0].Rows[0]["FlowStep10_TargetDate"].ToString() != "")
                        {

                            dtTargetDate10.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep10_TargetDate"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                        }
                        if (ds.Tables[0].Rows[0]["FlowStep11_TargetDate"].ToString() != null && ds.Tables[0].Rows[0]["FlowStep11_TargetDate"].ToString() != "")
                        {

                            dtTargetDate11.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep11_TargetDate"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

                        }
                        if (ds.Tables[0].Rows[0]["FlowStep12_TargetDate"].ToString() != null && ds.Tables[0].Rows[0]["FlowStep12_TargetDate"].ToString() != "")
                        {

                            dtTargetDate12.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep12_TargetDate"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                        }
                        if (ds.Tables[0].Rows[0]["FlowStep13_TargetDate"].ToString() != null && ds.Tables[0].Rows[0]["FlowStep13_TargetDate"].ToString() != "")
                        {

                            dtTargetDate13.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep13_TargetDate"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                        }
                        if (ds.Tables[0].Rows[0]["FlowStep14_TargetDate"].ToString() != null && ds.Tables[0].Rows[0]["FlowStep14_TargetDate"].ToString() != "")
                        {

                            dtTargetDate14.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep14_TargetDate"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                        }
                        if (ds.Tables[0].Rows[0]["FlowStep15_TargetDate"].ToString() != null && ds.Tables[0].Rows[0]["FlowStep15_TargetDate"].ToString() != "")
                        {

                            dtTargetDate15.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep15_TargetDate"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                        }
                        if (ds.Tables[0].Rows[0]["FlowStep16_TargetDate"].ToString() != null && ds.Tables[0].Rows[0]["FlowStep16_TargetDate"].ToString() != "")
                        {

                            dtTargetDate16.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep16_TargetDate"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                        }
                        if (ds.Tables[0].Rows[0]["FlowStep17_TargetDate"].ToString() != null && ds.Tables[0].Rows[0]["FlowStep17_TargetDate"].ToString() != "")
                        {

                            dtTargetDate17.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep17_TargetDate"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                        }
                        if (ds.Tables[0].Rows[0]["FlowStep18_TargetDate"].ToString() != null && ds.Tables[0].Rows[0]["FlowStep18_TargetDate"].ToString() != "")
                        {

                            dtTargetDate18.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep18_TargetDate"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                        }
                        if (ds.Tables[0].Rows[0]["FlowStep19_TargetDate"].ToString() != null && ds.Tables[0].Rows[0]["FlowStep19_TargetDate"].ToString() != "")
                        {

                            dtTargetDate19.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep19_TargetDate"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                        }
                        if (ds.Tables[0].Rows[0]["FlowStep20_TargetDate"].ToString() != null && ds.Tables[0].Rows[0]["FlowStep20_TargetDate"].ToString() != "")
                        {

                            dtTargetDate20.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep20_TargetDate"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
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

        protected void DDLDocumentCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (DDLDocumentFlow.SelectedIndex > 0)
                {
                    //
                    if (Request.QueryString["type"] == "add")
                    {
                        LoadDropDowns();
                    }
                    else
                    {

                        BindDocument();
                    }
                       
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}