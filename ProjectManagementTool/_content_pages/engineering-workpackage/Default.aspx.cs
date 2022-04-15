using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManager._content_pages.work_packages
{
    public partial class Default : System.Web.UI.Page
    {
        DBGetData dbgetdata = new DBGetData();
        TaskUpdate gettk = new TaskUpdate();
        public int SearchResultCount = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Username"] == null)
            {
                Response.Redirect("~/Login.aspx");
            }
            else
            {
                ScriptManager.RegisterStartupScript(
                        UpdatePanel2,
                        this.GetType(),
                        "MyAction",
                        "BindEvents();",
                        true);
                if (!IsPostBack)
                {
                    BindProject();
                    SelectedProject();
                    DDlProject_SelectedIndexChanged(sender, e);
                    Session["BOQData"] = null;
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

            DDlProject.DataTextField = "ProjectName";
            DDlProject.DataValueField = "ProjectUID";
            DDlProject.DataSource = ds;
            DDlProject.DataBind();
        }

        protected void DDlProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDlProject.SelectedValue != "")
            {
                //string ProjectUID = string.Empty;
                //if (Request.QueryString["ActivityUID"] != null)
                //{
                //    ProjectUID=dbgetdata.GetProjectUIDFromTaskUID(new Guid(Request.QueryString["ActivityUID"].Split('*')[1].ToString()));

                //}
                //else if(Session["SelectedActivity"] != null)
                //{
                //    ProjectUID = dbgetdata.GetProjectUIDFromTaskUID(new Guid(Session["SelectedActivity"].ToString()));
                //}

                //if (ProjectUID != "")
                //{
                //    DDlProject.SelectedValue = new Guid(ProjectUID).ToString();
                //}
                
                BindTreeview();
                // HideButtons();
                Session["Project_Workpackage"] = DDlProject.SelectedValue;
            }
        }

        internal void SelectedProject()
        {
            if (!IsPostBack)
            {
                if (Session["Project_Workpackage"] != null)
                {
                    string[] selectedValue = Session["Project_Workpackage"].ToString().Split('_');
                    if (selectedValue.Length > 1)
                    {
                        DDlProject.SelectedValue = selectedValue[0];
                    }
                    else
                    {
                        DDlProject.SelectedValue = Session["Project_Workpackage"].ToString();
                    }

                }
            }
            
        }

        private void HideButtons()
        {
            // added on 18/11/2020
            DataSet dscheck = new DataSet();
            dscheck = dbgetdata.GetUsertypeFunctionality_Mapping(Session["TypeOfUser"].ToString());
            AddTask.Visible = false;
            AddWorkPackage.Visible = false;
            AddSubTask.Visible = false;
            AddTaskMileStone.Visible = false;
            AddTaskResources.Visible = false;
            AddDependency.Visible = false;
            ViewState["Show"] = "false";
            if (dscheck.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dscheck.Tables[0].Rows)
                {

                    if (dr["Code"].ToString() == "FX") // PROJECT PROGRESS TRACKING
                    {
                        AddTask.Visible = true;
                        AddWorkPackage.Visible = true;
                        AddSubTask.Visible = true;
                        AddTaskMileStone.Visible = true;
                        AddTaskResources.Visible = true;
                        AddDependency.Visible = true;
                        ViewState["Show"] = "true";
                    }


                }
            }
            //
        }

        public void BindTreeview()
        {
            TreeView1.Nodes.Clear();
            //DataSet ds = new DataSet();
            //if (Session["TypeOfUser"].ToString() == "U" || Session["TypeOfUser"].ToString() =="MD" || Session["TypeOfUser"].ToString() == "VP")
            //{
            //    //ds = gettk.GetAllProjects();
            //    ds = dbgetdata.ProjectClass_Select_All();
            //}
            //else if (Session["TypeOfUser"].ToString() == "PA")
            //{
            //    //ds = dbgetdata.GetAssignedProjects_by_UserUID(new Guid(Session["UserUID"].ToString()));
            //    ds = dbgetdata.ProjectClass_Select_By_UserUID(new Guid(Session["UserUID"].ToString()));
            //}
            //else
            //{
            //    //ds = dbgetdata.GetAssignedProjects_by_UserUID(new Guid(Session["UserUID"].ToString()));
            //    ds = dbgetdata.ProjectClass_Select_By_UserUID(new Guid(Session["UserUID"].ToString()));
            //}
            DataSet ds = new DataSet();
            if (Session["TypeOfUser"].ToString() == "U" || Session["TypeOfUser"].ToString() == "MD" || Session["TypeOfUser"].ToString() == "VP")
            {
                ds = dbgetdata.GetWorkPackages_By_ProjectUID(new Guid(DDlProject.SelectedValue));
            }
            else if (Session["TypeOfUser"].ToString() == "PA")
            {
                ds = dbgetdata.GetWorkPackages_ForUser_by_ProjectUID(new Guid(Session["UserUID"].ToString()), new Guid(DDlProject.SelectedValue));
            }
            else
            {
                ds = dbgetdata.GetWorkPackages_ForUser_by_ProjectUID(new Guid(Session["UserUID"].ToString()), new Guid(DDlProject.SelectedValue));
            }
            if (ds.Tables[0].Rows.Count > 0)
            {
                PopulateTreeView(ds, null, "", 2);
                if (Request.QueryString["ActivityUID"] != null)
                {
                    string UID = "";
                    TreeView1.CollapseAll();
                    string wkUID = Request.QueryString["ActivityUID"].Split('*')[0];
                    string sTaskUID = Request.QueryString["ActivityUID"].Split('*')[1];
                    if (sTaskUID == Guid.Empty.ToString())
                    {
                        UID = wkUID;
                    }
                    else
                    {
                        UID = sTaskUID;
                    }
                    for (int i = 0; i < TreeView1.Nodes.Count; i++)
                    {
                        retrieveNodes(TreeView1.Nodes[i], UID);
                    }
                }
                else if (Session["SelectedActivity"] != null)
                {
                    TreeView1.CollapseAll();
                    string UID = Session["SelectedActivity"].ToString();
                    for (int i = 0; i < TreeView1.Nodes.Count; i++)
                    {
                        retrieveNodes(TreeView1.Nodes[i], UID);
                    }
                    Session["SelectedActivity"] = null;
                }
                else
                {
                    TreeView1.Nodes[0].Selected = true;
                    //TreeView1.Nodes[0].Expand();
                    //TreeView1.ExpandAll();
                    TreeView1.CollapseAll();
                    TreeView1.Nodes[0].Expand();
                    //LoadProjectDetails(TreeView1.SelectedNode.Value);
                    //AddTask.HRef = "/_modal_pages/add-task.aspx?type=Add&PrjUID=" + TreeView1.SelectedNode.Value;
                }

                BindActivities();

            }
            else
            {
                ProjectDetails.Visible = false;
                Contractors.Visible = false;
                WorkPackageDetils.Visible = false;
                TaskDetails.Visible = false;
                EnableOption.Visible = false;
                AddTask.Visible = false;
                AddWorkPackage.Visible = false;
                AddSubTask.Visible = false;
                SortActivity.Visible = false;
            }
        }

        public void PopulateTreeView(DataSet dtParent, TreeNode treeNode, string ParentUID, int Level)
        {
            foreach (DataRow row in dtParent.Tables[0].Rows)
            {
                TreeNode child = new TreeNode
                {
                    Text = Level == 0 ? row["ProjectClass_Name"].ToString() : Level == 1 ? LimitCharts(dbgetdata.getProjectNameby_ProjectUID(new Guid(row["ProjectUID"].ToString()))) : Level == 2 ? row["Name"].ToString() : Level == 3 ? row["WorkpackageSelectedOption_Name"].ToString() : row["Name"].ToString(),
                    Value = Level == 0 ? row["ProjectClass_UID"].ToString() : Level == 1 ? row["ProjectUID"].ToString() : Level == 2 ? row["WorkPackageUID"].ToString() : Level == 3 ? row["Workpackage_OptionUID"].ToString() : row["TaskUID"].ToString(),
                    Target = Level == 0 ? "Class" : Level == 1 ? "Project" : Level == 2 ? "WorkPackage" : Level == 3 ? "Option" : "Tasks",
                    ToolTip = Level == 0 ? row["ProjectClass_Name"].ToString() : Level == 1 ? dbgetdata.getProjectNameby_ProjectUID(new Guid(row["ProjectUID"].ToString())) : Level == 2 ? row["Name"].ToString() : Level == 3 ? row["WorkpackageSelectedOption_Name"].ToString() : row["Name"].ToString(),
                };

                //TreeNode child = new TreeNode
                //{
                //    Text = Level == 0 ? row["ProjectClass_Name"].ToString() : Level == 1 ? LimitCharts(dbgetdata.getProjectNameby_ProjectUID(new Guid(row["ProjectUID"].ToString()))) : Level == 2 ? row["Name"].ToString() : Level == 3 ? row["Workpackage_OptionName"].ToString() : row["Name"].ToString(),
                //    Value = Level == 0 ? row["ProjectClass_UID"].ToString() : Level == 1 ? row["ProjectUID"].ToString() : Level == 2 ? row["WorkPackageUID"].ToString() : Level == 3 ? row["SelectedOptionUID"].ToString() : row["TaskUID"].ToString(),
                //    Target = Level == 0 ? "Class" : Level == 1 ? "Project" : Level == 2 ? "WorkPackage" : Level == 3 ? "Option" : "Tasks",
                //    ToolTip = Level == 0 ? row["ProjectClass_Name"].ToString() : Level == 1 ? dbgetdata.getProjectNameby_ProjectUID(new Guid(row["ProjectUID"].ToString())) : Level == 2 ? row["Name"].ToString() : Level == 3 ? row["Workpackage_OptionName"].ToString() : row["Name"].ToString()
                //};

                //if (ParentUID == "")
                //{
                //    TreeView1.Nodes.Add(child);
                //    DataSet dsProject = new DataSet();
                //    if (Session["TypeOfUser"].ToString() == "U" || Session["TypeOfUser"].ToString() =="MD" || Session["TypeOfUser"].ToString() == "VP")
                //    {
                //        dsProject = gettk.GetProjects_by_ClassUID(new Guid(child.Value));
                //    }
                //    else
                //    {
                //        dsProject = gettk.GetUserProjects_by_ClassUID(new Guid(child.Value), new Guid(Session["UserUID"].ToString()));
                //    }

                //    //DataSet dsProject = gettk.GetUserProjects_by_ClassUID(new Guid(child.Value), new Guid(Session["UserUID"].ToString()));
                //    if (dsProject.Tables[0].Rows.Count > 0)
                //    {
                //        PopulateTreeView(dsProject, child, child.Value, 1);
                //    }
                //}
                //else if (Level == 1)
                //{
                //    treeNode.ChildNodes.Add(child);
                //    DataSet dsworkPackage = new DataSet();
                //    //DataSet dsworkPackage = dbgetdata.GetWorkPackages_By_ProjectUID(new Guid(child.Value));
                //    if (Session["TypeOfUser"].ToString() == "U" || Session["TypeOfUser"].ToString() == "MD" || Session["TypeOfUser"].ToString() == "VP")
                //    {
                //        dsworkPackage = dbgetdata.GetWorkPackages_By_ProjectUID(new Guid(child.Value));
                //    }
                //    else if (Session["TypeOfUser"].ToString() == "PA")
                //    {
                //        dsworkPackage = dbgetdata.GetWorkPackages_ForUser_by_ProjectUID(new Guid(Session["UserUID"].ToString()), new Guid(child.Value));
                //    }
                //    else
                //    {
                //        dsworkPackage = dbgetdata.GetWorkPackages_ForUser_by_ProjectUID(new Guid(Session["UserUID"].ToString()), new Guid(child.Value));
                //    }



                //    if (dsworkPackage.Tables[0].Rows.Count > 0)
                //    {
                //        PopulateTreeView(dsworkPackage, child, child.Value, 2);
                //    }
                //}
                //else 
                //child.SelectAction = TreeNodeSelectAction.None;
                if (Level == 2)
                {
                    //treeNode.ChildNodes.Add(child);
                    TreeView1.Nodes.Add(child);
                    //DataSet dsoption = dbgetdata.GetWorkPackages_By_WorkPackageUID(new Guid(child.Value));
                    //DataSet dsoption = dbgetdata.WorkpackageSelectedOptions_by_WorkPackageUID(new Guid(child.Value));
                    DataSet dsoption = dbgetdata.GetSelectedOption_By_WorkpackageUID(new Guid(child.Value));
                    if (dsoption.Tables[0].Rows.Count > 0)
                    {
                        PopulateTreeView(dsoption, child, child.Value, 3);
                    }
                }
                else if (Level == 3)
                {
                    if (Session["IsContractor"].ToString() == "Y")
                    {
                       if( row["WorkpackageSelectedOption_Name"].ToString() != "Design")
                       {
                            if (row["WorkpackageSelectedOption_Name"].ToString() == "PMC")
                            {
                                child.Text = "Construction & Execution";
                            }

                            treeNode.ChildNodes.Add(child);
                            DataSet dschild = dbgetdata.GetTasks_by_WorkpackageOptionUID(new Guid(child.Parent.Value), new Guid(child.Value));
                            //DataSet dschild = dbgetdata.GetTasksForWorkPackages(child.Parent.Value);
                            //DataSet dschild = gettk.GetTasks_by_Workpackage_Option(new Guid(child.Value), new Guid(child.Parent.Value));
                            //DataTable dtChild = TreeViewBAL.BL.TreeViewBL.GetData("Select ID,Name from Module where ProjID=" + child.Value);
                            //DataSet dschild=dbgetdata.
                            if (dschild.Tables[0].Rows.Count > 0)
                            {
                                PopulateTreeView(dschild, child, child.Value, 4);
                            }
                        }
                    }
                    else
                    {
                        treeNode.ChildNodes.Add(child);
                        DataSet dschild = dbgetdata.GetTasks_by_WorkpackageOptionUID(new Guid(child.Parent.Value), new Guid(child.Value));
                        //DataSet dschild = dbgetdata.GetTasksForWorkPackages(child.Parent.Value);
                        //DataSet dschild = gettk.GetTasks_by_Workpackage_Option(new Guid(child.Value), new Guid(child.Parent.Value));
                        //DataTable dtChild = TreeViewBAL.BL.TreeViewBL.GetData("Select ID,Name from Module where ProjID=" + child.Value);
                        //DataSet dschild=dbgetdata.
                        if (dschild.Tables[0].Rows.Count > 0)
                        {
                            PopulateTreeView(dschild, child, child.Value, 4);
                        }
                    }

                }
                else if (Level == 4)
                {
                    treeNode.ChildNodes.Add(child);
                    DataSet dssubchild = dbgetdata.GetSubTasksForWorkPackages(child.Value);
                    if (dssubchild.Tables[0].Rows.Count > 0)
                    {
                        PopulateTreeView(dssubchild, child, child.Value, 5);
                    }
                }
                else if (Level == 5)
                {
                    treeNode.ChildNodes.Add(child);
                    DataSet dssubtosubchild = dbgetdata.GetSubtoSubTasksForWorkPackages(child.Value);
                    if (dssubtosubchild.Tables[0].Rows.Count > 0)
                    {
                        PopulateTreeView(dssubtosubchild, child, child.Value, 6);
                    }
                }
                else if (Level == 6)
                {
                    treeNode.ChildNodes.Add(child);
                    DataSet lastchild = dbgetdata.GetSubtoSubtoSubTasksForWorkPackages(child.Value);
                    if (lastchild.Tables[0].Rows.Count > 0)
                    {
                        PopulateTreeView(lastchild, child, child.Value, 7);
                    }
                }
                else if (Level == 7)
                {
                    treeNode.ChildNodes.Add(child);
                    DataSet lastchild = dbgetdata.GetSubtoSubtoSubtoSubTasksForWorkPackages(child.Value);
                    if (lastchild.Tables[0].Rows.Count > 0)
                    {
                        PopulateTreeView(lastchild, child, child.Value, 8);
                    }
                }
                else
                {
                    treeNode.ChildNodes.Add(child);
                    DataSet ds = dbgetdata.GetTask_by_ParentTaskUID(new Guid(child.Value));
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        PopulateTreeView(ds, child, child.Value, 9);
                    }
                }
            }
           
        }

        private void LoadProjectDetails(string ProjectUID)
        {
            DataSet ds = new DataSet();
            ds = dbgetdata.GetProject_by_ProjectUID(new Guid(ProjectUID));
            if (ds.Tables[0].Rows.Count > 0)
            {
                lblPrjName.Text = ds.Tables[0].Rows[0]["ProjectName"].ToString();
                //lblPrjOwner.Text = ds.Tables[0].Rows[0]["OwnerName"].ToString();
                lblStartDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["StartDate"].ToString()).ToString("dd MMM yyyy");
                lblPlannedEndDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["PlannedEndDate"].ToString()).ToString("dd MMM yyyy");
                lblPrjctedEndDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["ProjectedEndDate"].ToString()).ToString("dd MMM yyyy");
                //lblPrjBudget.Text = ds.Tables[0].Rows[0]["Budget"].ToString();
                //lblPrjActaulExpenditure.Text = ds.Tables[0].Rows[0]["ActualExpenditure"].ToString();
                lblPrjStatus.Text = GetStatus(ds.Tables[0].Rows[0]["Status"].ToString());
                LblFundingAgency.Text = ds.Tables[0].Rows[0]["Funding_Agency"].ToString();
                LblCumulativeProgressProject.Text = dbgetdata.GetCumulativeProgress_Project(new Guid(ProjectUID)).ToString("0.###") + " %";
            }
        }

        protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
        {
            BindActivities();
        }

        public void BindActivities()
        {
            HideButtons();
            if (TreeView1.SelectedNode.Target == "Class")
            {
                ActivityHeading.Text = "Projects";
                ProjectDetails.Visible = false;
                WorkPackageDetils.Visible = false;
                AddTask.Visible = false;
                TaskDetails.Visible = false;
                Contractors.Visible = false;
                AddWorkPackage.Visible = false;
                MileStoneList.Visible = false;
                AddSubTask.Visible = false;
                ResourceAllocatedList.Visible = false;
                Dependencies.Visible = false;
                AddSubTask.Visible = false;
                SortActivity.Visible = false;
                DataSet ds = new DataSet();
                if (Session["TypeOfUser"].ToString() == "U" || Session["TypeOfUser"].ToString() =="MD" || Session["TypeOfUser"].ToString() == "VP")
                {
                    ds = gettk.GetProjects_by_ClassUID(new Guid(TreeView1.SelectedNode.Value));
                }
                else
                {
                    ds = gettk.GetUserProjects_by_ClassUID(new Guid(TreeView1.SelectedNode.Value), new Guid(Session["UserUID"].ToString()));
                }

                //DataSet ds = gettk.GetUserProjects_by_ClassUID(new Guid(TreeView1.SelectedNode.Value), new Guid(Session["UserUID"].ToString()));
                GrdTreeView.DataSource = ds;
                GrdTreeView.DataBind();
                EnableOption.Visible = false;
                GrdOptions.Visible = false;
                GrdTreeView.Visible = true;
                ResourceDeployment.Visible = false;
            }
            else if (TreeView1.SelectedNode.Target == "Project")
            {
                LoadProjectDetails(DDlProject.SelectedValue);
                ActivityHeading.Text = "Work Packages";
                DataSet ds = dbgetdata.GetWorkPackage_By_ProjectUID(new Guid(DDlProject.SelectedValue));
                GrdTreeView.DataSource = ds;
                GrdTreeView.DataBind();
                ProjectDetails.Visible = true;
                WorkPackageDetils.Visible = false;
                AddTask.Visible = false;
                TaskDetails.Visible = false;
                Contractors.Visible = false;
                if (ViewState["Show"].ToString() == "true")
                {
                    AddWorkPackage.Visible = true;
                }
                AddWorkPackage.HRef = "/_modal_pages/add-workpackage.aspx?ProjectUID=" + DDlProject.SelectedValue;
                MileStoneList.Visible = true;
                AddSubTask.Visible = false;
                ResourceAllocatedList.Visible = false;
                LoadTaskMileStones(TreeView1.SelectedNode.Value);
                AddTaskMileStone.HRef = "/_modal_pages/add-milestone.aspx?TaskUID=" + TreeView1.SelectedNode.Value;
                AddSubTask.Visible = false;
                SortActivity.Visible = false;
                Dependencies.Visible = false;
                EnableOption.Visible = false;
                GrdOptions.Visible = false;
                GrdTreeView.Visible = true;
                ResourceDeployment.Visible = false;
                if (ds.Tables[0].Rows.Count == 0)
                {
                    CopyProjectData.Visible = true;
                    CopyProjectData.HRef = "/_modal_pages/copy-projectdata.aspx?ProjectUID=" + DDlProject.SelectedValue;
                }
                else
                {
                    CopyProjectData.Visible = false;
                }

            }
            else if (TreeView1.SelectedNode.Target == "WorkPackage")
            {
               
                AddTask.HRef = "/_modal_pages/add-task.aspx?type=Add&PrjUID=" + DDlProject.SelectedValue + "&wUID=" + TreeView1.SelectedNode.Value;
                UploadSitePhotograph.HRef = "/_modal_pages/upload-sitephotograph.aspx?PrjUID=" + DDlProject.SelectedValue + "&WorkPackage=" + TreeView1.SelectedNode.Value;
                ViewSitePhotograph.HRef = "/_modal_pages/view-sitephotographs.aspx?PrjUID=" + DDlProject.SelectedValue + "&WorkPackage=" + TreeView1.SelectedNode.Value;

                //ActivityHeading.Text = "Option";
                //DataSet ds = gettk.GetTasks_by_WorkPackage(TreeView1.SelectedNode.Value);
                //GrdTreeView.DataSource = ds;
                //GrdTreeView.DataBind();
                GrdOptions.Visible = false;

                DataSet ds = dbgetdata.GetSelectedOption_By_WorkpackageUID(new Guid(TreeView1.SelectedNode.Value));
                //DataSet ds = dbgetdata.GetWorkPackages_By_WorkPackageUID(new Guid(TreeView1.SelectedNode.Value));
                GrdOptions.DataSource = ds;
                GrdOptions.DataBind();

                GrdTreeView.Visible = false;
                ProjectDetails.Visible = false;
                WorkPackageDetils.Visible = true;
                GetWorkPackage_by_UID(new Guid(TreeView1.SelectedNode.Value));
                AddTask.Visible = false;
                TaskDetails.Visible = false;
                Contractors.Visible = true;
                AddWorkPackage.Visible = false;
                AddSubTask.Visible = false;
                MileStoneList.Visible = true;
                ResourceAllocatedList.Visible = false;
                Dependencies.Visible = false;
                LoadTaskMileStones(TreeView1.SelectedNode.Value);
                EnableOption.Visible = false;
                AddTaskMileStone.HRef = "/_modal_pages/add-milestone.aspx?TaskUID=" + TreeView1.SelectedNode.Value;
                SortActivity.Visible = false;
                ResourceDeployment.Visible = true;
                LoadResourceDeployment(TreeView1.SelectedNode.Value);
                //if (ds.Tables[0].Rows.Count > 0)
                //{
                //    SortActivity.Visible = true;
                //    SortActivity.HRef = "/_modal_pages/change-activityorder.aspx?WorkPackage=" + TreeView1.SelectedNode.Value;
                //}
                //else
                //{
                //    SortActivity.Visible = false;
                //}
                //if (ds.Tables[0].Rows.Count == 0)
                //{
                //    CopyActivityData.Visible = true;
                //    CopyActivityData.HRef = "/_modal_pages/copy-activitydata.aspx?WorkPackage=" + TreeView1.SelectedNode.Value;
                    
                //}
                //else
                //{
                //    CopyActivityData.Visible = false;
                    
                //}
            }
            else if (TreeView1.SelectedNode.Target == "Option")
            {
               
                GrdOptions.Visible = false;
                GrdTreeView.Visible = true;
                AddTask.HRef = "/_modal_pages/add-task.aspx?type=Add&PrjUID=" + DDlProject.SelectedValue + "&wUID=" + TreeView1.SelectedNode.Parent.Value + "&OptionUID=" + TreeView1.SelectedNode.Value;
                ActivityHeading.Text = "Activities";
                //DataSet ds = gettk.GetTasks_by_WorkPackage(TreeView1.SelectedNode.Parent.Value);
                DataSet ds = dbgetdata.GetTasks_by_WorkpackageOptionUID(new Guid(TreeView1.SelectedNode.Parent.Value),new Guid(TreeView1.SelectedNode.Value));
                GrdTreeView.DataSource = ds;
                GrdTreeView.DataBind();
                ProjectDetails.Visible = false;
                WorkPackageDetils.Visible = false;
                //GetWorkPackage_by_UID(new Guid(TreeView1.SelectedNode.Value));
                if (ViewState["Show"].ToString() == "true")
                {
                    AddTask.Visible = true;
                }
                TaskDetails.Visible = false;
                Contractors.Visible = false;
                AddWorkPackage.Visible = false;
                AddSubTask.Visible = false;
                MileStoneList.Visible = false;
                ResourceAllocatedList.Visible = false;
                Dependencies.Visible = false;
                LoadTaskMileStones(TreeView1.SelectedNode.Value);
                EnableOption.Visible = false;
                AddTaskMileStone.HRef = "/_modal_pages/add-milestone.aspx?TaskUID=" + TreeView1.SelectedNode.Value;
                CopyActivityData.Visible = false;
                ResourceDeployment.Visible = false;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    SortActivity.Visible = true;
                    SortActivity.HRef = "/_modal_pages/change-activityorder.aspx?WorkPackage=" + TreeView1.SelectedNode.Parent.Value + "&OptionUID=" + TreeView1.SelectedNode.Value;
                }
                else
                {
                    SortActivity.Visible = false;
                }
                //if (ds.Tables[0].Rows.Count == 0)
                //{
                //    CopyActivityData.Visible = true;
                //    CopyActivityData.HRef = "/_modal_pages/copy-activitydata.aspx?WorkPackage=" + TreeView1.SelectedNode.Value;
                //}
                //else
                //{
                //    CopyActivityData.Visible = false;
                //}
            }
            //else if (TreeView1.SelectedNode.Target == "Option")
            //{
            //    DataSet dsoption = dbgetdata.WorkpackageSelectedOptions_by_UID(new Guid(TreeView1.SelectedNode.Value));
            //    if (dsoption.Tables[0].Rows.Count > 0)
            //    {
            //        ProjectDetails.Visible = false;
            //        WorkPackageDetils.Visible = false;
            //        //GetWorkPackage_by_UID(new Guid(TreeView1.SelectedNode.Value));
            //        AddTask.Visible = false;
            //        TaskDetails.Visible = false;
            //        Contractors.Visible = false;
            //        AddWorkPackage.Visible = false;
            //        AddSubTask.Visible = false;
            //        MileStoneList.Visible = false;
            //        ResourceAllocatedList.Visible = false;
            //        Dependencies.Visible = false;
            //        //LoadTaskMileStones(TreeView1.SelectedNode.Value);
            //        //AddTaskMileStone.HRef = "/_modal_pages/add-milestone.aspx?TaskUID=" + TreeView1.SelectedNode.Value;
            //        CopyActivityData.Visible = false;
            //        SortActivity.Visible = false;

            //        if (dsoption.Tables[0].Rows[0]["Workpackage_OptionEnabled"].ToString() == "1")
            //        {
            //            EnableOption.Visible = false;
            //            GrdTreeView.Visible = true;
            //            //AddTask.HRef = "/_modal_pages/add-task.aspx?type=Add&PrjUID=" + TreeView1.SelectedNode.Parent.Value + "&wUID=" + TreeView1.SelectedNode.Value;
            //            ActivityHeading.Text = "Activities";
            //            //DataSet ds = gettk.GetTasks_by_WorkPackage(TreeView1.SelectedNode.Value);
            //            DataSet ds = gettk.GetTasks_by_Workpackage_Option(new Guid(TreeView1.SelectedNode.Value), new Guid(TreeView1.SelectedNode.Parent.Value));
            //            GrdTreeView.DataSource = ds;
            //            GrdTreeView.DataBind();

            //        }
            //        else
            //        {
            //            EnableOption.Visible = true;
            //            ActivityHeading.Text = "Option : " + TreeView1.SelectedNode.Text;
            //            GrdTreeView.Visible = false;
            //        }
            //    }

            //}
            else
            {
               
                GrdOptions.Visible = false;
                ActivityHeading.Text = "Sub Activities";
                DataSet ds = dbgetdata.GetTask_by_ParentTaskUID(new Guid(TreeView1.SelectedNode.Value));
                GrdTreeView.DataSource = ds;
                GrdTreeView.DataBind();
                ProjectDetails.Visible = false;
                WorkPackageDetils.Visible = false;
                AddTask.Visible = false;
                AddWorkPackage.Visible = false;
                TaskDetails.Visible = true;
                GetTask_by_UID(TreeView1.SelectedNode.Value);
                Contractors.Visible = false;
                if (ViewState["Show"].ToString() == "true")
                {
                    AddSubTask.Visible = true;
                }
                MileStoneList.Visible = true;
                ResourceAllocatedList.Visible = true;
                Dependencies.Visible = true;
                CopyActivityData.Visible = false;
                CopyProjectData.Visible = false;
                EnableOption.Visible = false;
                GrdTreeView.Visible = true;
                ResourceDeployment.Visible = false;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    SortActivity.Visible = true;
                    SortActivity.HRef = "/_modal_pages/change-activityorder.aspx?TaskUID=" + TreeView1.SelectedNode.Value;
                }
                else
                {
                    SortActivity.Visible = false;
                }
            }

           
            //if (GrdTreeView.Rows.Count > 0)
            //{
            //    LinkButton lnkUp = (GrdTreeView.Rows[0].FindControl("lnkUp") as LinkButton);
            //    LinkButton lnkDown = (GrdTreeView.Rows[GrdTreeView.Rows.Count - 1].FindControl("lnkDown") as LinkButton);
            //    lnkUp.Enabled = false;
            //    lnkUp.CssClass = "disabled";
            //    lnkDown.Enabled = false;
            //    lnkDown.CssClass = "disabled";
            //}
        }

        protected void retrieveNodes(TreeNode node, string SelectedVal)
        {
            if (node.Value == SelectedVal)
            {
                node.Selected = true;
                node.Expand();
                if (node.Parent != null)
                {
                    node.Parent.Expand();
                }
                BindActivities();
            }
            else
            {
                if (node.ChildNodes.Count != 0)
                {
                    foreach (TreeNode tn in node.ChildNodes)
                    {
                        if (tn.Value == SelectedVal)
                        {
                            tn.Selected = true;
                            tn.Expand();
                            if (tn.Parent != null)
                            {
                                tn.Parent.Expand();
                                if (tn.Parent.Parent != null)
                                {
                                    tn.Parent.Parent.Expand();

                                    if (tn.Parent.Parent.Parent != null)
                                    {
                                        tn.Parent.Parent.Parent.Expand();

                                        if (tn.Parent.Parent.Parent.Parent != null)
                                        {
                                            tn.Parent.Parent.Parent.Parent.Expand();

                                            if (tn.Parent.Parent.Parent.Parent.Parent != null)
                                            {
                                                tn.Parent.Parent.Parent.Parent.Parent.Expand();

                                                if (tn.Parent.Parent.Parent.Parent.Parent.Parent != null)
                                                {
                                                    tn.Parent.Parent.Parent.Parent.Parent.Parent.Expand();

                                                    if (tn.Parent.Parent.Parent.Parent.Parent.Parent.Parent != null)
                                                    {
                                                        tn.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Expand();

                                                        if (tn.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Parent != null)
                                                        {
                                                            tn.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Expand();
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            
                            BindActivities();
                        }
                        else
                        {
                            if (tn.ChildNodes.Count != 0)
                            {
                                retrieveNodes(tn, SelectedVal);
                            }
                        }
                    }
                }
            }
        }

        protected void retrieveNodes_Options(TreeNode node, string SelectedVal,string WorkpackageUID)
        {
            if (node.Value == SelectedVal && node.Parent.Value== WorkpackageUID)
            {
                node.Selected = true;
                node.Expand();
                if (node.Parent != null)
                {
                    node.Parent.Expand();
                }
                
                BindActivities();
            }
            else
            {
                if (node.ChildNodes.Count != 0)
                {
                    foreach (TreeNode tn in node.ChildNodes)
                    {
                        if (tn.Value == SelectedVal && tn.Parent.Value == WorkpackageUID)
                        {
                            tn.Selected = true;
                            tn.Expand();
                            if (tn.Parent != null)
                            {
                                tn.Parent.Expand();
                                if (tn.Parent.Parent != null)
                                {
                                    tn.Parent.Parent.Expand();

                                    if (tn.Parent.Parent.Parent != null)
                                    {
                                        tn.Parent.Parent.Parent.Expand();

                                        if (tn.Parent.Parent.Parent.Parent != null)
                                        {
                                            tn.Parent.Parent.Parent.Parent.Expand();

                                            if (tn.Parent.Parent.Parent.Parent.Parent != null)
                                            {
                                                tn.Parent.Parent.Parent.Parent.Parent.Expand();

                                                if (tn.Parent.Parent.Parent.Parent.Parent.Parent != null)
                                                {
                                                    tn.Parent.Parent.Parent.Parent.Parent.Parent.Expand();

                                                    if (tn.Parent.Parent.Parent.Parent.Parent.Parent.Parent != null)
                                                    {
                                                        tn.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Expand();

                                                        if (tn.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Parent != null)
                                                        {
                                                            tn.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Expand();
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            BindActivities();
                        }
                        else
                        {
                            if (tn.ChildNodes.Count != 0)
                            {
                                retrieveNodes_Options(tn, SelectedVal, WorkpackageUID);
                            }
                        }
                    }
                }
            }
        }

        protected void retrieveNodes_Search(TreeNode node, string SearchText)
        {
            
            if (node.Text.ToUpper().Contains(SearchText.ToUpper()))
            {
                SearchResultCount += 1;
                node.Text = "<div style='background-color:Yellow;'>" + node.Text + "</div>";
                node.Expand();
            }
            else
            {
                if (node.ChildNodes.Count != 0)
                {
                    foreach (TreeNode tn in node.ChildNodes)
                    {
                        if (tn.Text.ToUpper().Contains(SearchText.ToUpper()))
                        {
                            SearchResultCount += 1;
                            tn.Text = "<div style='background-color:Yellow;'>" + tn.Text + "</div>";
                            tn.Expand();
                            if (tn.Parent != null)
                            {
                                tn.Parent.Expand();
                                if (tn.Parent.Parent != null)
                                {
                                    tn.Parent.Parent.Expand();

                                    if (tn.Parent.Parent.Parent != null)
                                    {
                                        tn.Parent.Parent.Parent.Expand();

                                        if (tn.Parent.Parent.Parent.Parent != null)
                                        {
                                            tn.Parent.Parent.Parent.Parent.Expand();

                                            if (tn.Parent.Parent.Parent.Parent.Parent != null)
                                            {
                                                tn.Parent.Parent.Parent.Parent.Parent.Expand();

                                                if (tn.Parent.Parent.Parent.Parent.Parent.Parent != null)
                                                {
                                                    tn.Parent.Parent.Parent.Parent.Parent.Parent.Expand();

                                                    if (tn.Parent.Parent.Parent.Parent.Parent.Parent.Parent != null)
                                                    {
                                                        tn.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Expand();

                                                        if (tn.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Parent != null)
                                                        {
                                                            tn.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Expand();
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                                //tn.Text = "<div style='background-color:white;'>" + tn.Text + "</div>";
                            if (tn.ChildNodes.Count != 0)
                            {
                                retrieveNodes_Search(tn, SearchText);
                            }
                        }
                    }
                }
                //node.Text = "<div style='background-color:white;'>" + node.Text + "</div>";
            }
        }

        protected void GrdTreeView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string UID = e.CommandArgument.ToString();
            if (e.CommandName == "view")
            {
                for (int i = 0; i < TreeView1.Nodes.Count; i++)
                {
                    retrieveNodes(TreeView1.Nodes[i], UID);
                }
            }
            if (e.CommandName == "delete")
            {
                if (TreeView1.SelectedNode.Target == "Class")
                {
                    //Delete whole project data
                }
                else if (TreeView1.SelectedNode.Target == "Project")
                {
                    int cnt = dbgetdata.Workpackage_Delete(new Guid(UID), new Guid(Session["UserUID"].ToString()));
                    if (cnt > 0)
                    {
                        BindActivities();
                    }
                }
                else
                {
                    int cnt = dbgetdata.Tasks_Delete_by_TaskUID(new Guid(UID), new Guid(Session["UserUID"].ToString()));
                    if (cnt > 0)
                    {
                        BindActivities();
                    }
                }
                
            }
            if (e.CommandName == "up")
            {
                if (TreeView1.SelectedNode.Target == "Project")
                {
                    int Cnt = dbgetdata.Workpackge_Order_Update(new Guid(UID), "Up");
                    if (Cnt > 0)
                    {
                        string TreeSelectedValue = TreeView1.SelectedNode.Value;
                        BindTreeview();
                        //for (int i = 0; i < TreeView1.Nodes.Count; i++)
                        //{
                        //    retrieveNodes(TreeView1.Nodes[i], TreeSelectedValue);
                        //}
                        BindActivities();
                    }
                }
                else
                {
                    int Cnt = dbgetdata.Task_Order_Update(new Guid(UID), "Up");
                    if (Cnt > 0)
                    {
                        string TreeSelectedValue = TreeView1.SelectedNode.Value;
                        BindTreeview();
                        for (int i = 0; i < TreeView1.Nodes.Count; i++)
                        {
                            retrieveNodes(TreeView1.Nodes[i], TreeSelectedValue);
                        }
                        BindActivities();
                    }
                }
            }
            if (e.CommandName == "down")
            {
                if (TreeView1.SelectedNode.Target == "Project")
                {
                    int Cnt = dbgetdata.Workpackge_Order_Update(new Guid(UID), "Down");
                    if (Cnt > 0)
                    {
                        string TreeSelectedValue = TreeView1.SelectedNode.Value;
                        BindTreeview();
                        //for (int i = 0; i < TreeView1.Nodes.Count; i++)
                        //{
                        //    retrieveNodes(TreeView1.Nodes[i], TreeSelectedValue);
                        //}
                        BindActivities();
                    }
                }
                else
                {
                    int Cnt = dbgetdata.Task_Order_Update(new Guid(UID), "Down");
                    if (Cnt > 0)
                    {
                        string TreeSelectedValue = TreeView1.SelectedNode.Value;
                        BindTreeview();
                        for (int i = 0; i < TreeView1.Nodes.Count; i++)
                        {
                            retrieveNodes(TreeView1.Nodes[i], TreeSelectedValue);
                        }
                        BindActivities();
                    }
                }
            }
        }

        protected void GrdTreeView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdTreeView.PageIndex = e.NewPageIndex;
            BindActivities();
            //DataSet ds = dbgetdata.GetTasks_by_ProjectUID(new Guid(TreeView1.SelectedNode.Value));
            //GrdTreeView.DataSource = ds;
            //GrdTreeView.DataBind();

        }

        protected void GrdTreeView_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (TreeView1.SelectedNode.Target == "Class")
                {
                    e.Row.Cells[4].Visible = false;
                    GrdTreeView.HeaderRow.Cells[4].Visible = false;
                    e.Row.Cells[5].Visible = false;
                    GrdTreeView.HeaderRow.Cells[5].Visible = false;
                }
                else if (TreeView1.SelectedNode.Target == "Project")
                {
                    e.Row.Cells[4].Visible = false;
                    //e.Row.Cells[5].Visible = false;
                    //e.Row.Cells[6].Visible = false;
                    GrdTreeView.HeaderRow.Cells[4].Visible = false;
                    //GrdTreeView.HeaderRow.Cells[5].Visible = false;
                    //GrdTreeView.HeaderRow.Cells[6].Visible = false;
                }
                if (TreeView1.SelectedNode.Target == "Tasks")
                {
                    //e.Row.Cells[6].Visible = false;
                    //GrdTreeView.HeaderRow.Cells[6].Visible = false;
                }
            }
        }

        protected void GrdTreeView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        public void GetWorkPackage_by_UID(Guid WorkPackageID)
        {
            DataSet ds = dbgetdata.GetWorkPackages_By_WorkPackageUID(WorkPackageID);
            if (ds.Tables[0].Rows.Count > 0)
            {
                LblWorkPackageName.Text = ds.Tables[0].Rows[0]["Name"].ToString();
                LblWorkPackageLocation.Text = ds.Tables[0].Rows[0]["WorkPackage_Location"].ToString();
                LblWorkPackageClient.Text = ds.Tables[0].Rows[0]["WorkPackage_Client"].ToString();
                LblWorkPackageStatus.Text = GetStatus(ds.Tables[0].Rows[0]["Status"].ToString());
                LblCumulativeProgressWorkpackage.Text = dbgetdata.GetCumulativeProgress_Workpackage(WorkPackageID).ToString("0.###") + " %";
                //LblWorkpackageCurrency.Text = ds.Tables[0].Rows[0]["Currency"].ToString();
                //LblWorkPackageBudget.Text =  Convert.ToDouble(ds.Tables[0].Rows[0]["Budget"].ToString()).ToString("#,##.00", CultureInfo.CreateSpecificCulture(ds.Tables[0].Rows[0]["Currency_CultureInfo"].ToString()));
                lblWkpgStartDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["StartDate"].ToString()).ToString("dd MMM yyyy");
                lblWkpgEndDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["PlannedEndDate"].ToString()).ToString("dd MMM yyyy");
                if (ds.Tables[0].Rows[0]["Contractor_UID"].ToString() != "")
                {
                    GetContractors(new Guid(ds.Tables[0].Rows[0]["Contractor_UID"].ToString()));
                }

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Workpackage_OptionUID"].ToString()))
                {
                    if (dbgetdata.GetWorkpackageOption_Order(new Guid(ds.Tables[0].Rows[0]["Workpackage_OptionUID"].ToString())) == 4)
                    {
                        CopyActivityData.Visible = true;
                        CopyActivityData.HRef = "/_modal_pages/copy-activitydata.aspx?WorkPackage=" + TreeView1.SelectedNode.Value;
                    }
                    else
                    {
                        CopyActivityData.Visible = false;
                    }
                }
            }
        }

        public void GetContractors(Guid ContractUID)
        {
            DataSet ds = dbgetdata.GetContractors_by_ContractorUID(ContractUID);
            if (ds.Tables[0].Rows.Count > 0)
            {
                LblContractor.Text = ds.Tables[0].Rows[0]["Contractor_Name"].ToString();
                LblContract_Type.Text = ds.Tables[0].Rows[0]["Type_of_Contract"].ToString();
                //LblContract_Representatives.Text = ds.Tables[0].Rows[0]["Contractor_Representatives"].ToString();
                LblContract_Duration.Text = GetStatus(ds.Tables[0].Rows[0]["Contract_Duration"].ToString()) + " Months";

                LBLProjectNumber.Text = WebConfigurationManager.AppSettings["Domain"] + " Project Number";

                LblNJSEIProjectNumber.Text = ds.Tables[0].Rows[0]["NJSEI_Number"].ToString();
                LblProjectSpecificPackageNumber.Text = ds.Tables[0].Rows[0]["ProjectSpecific_Number"].ToString();
                if (ds.Tables[0].Rows[0]["Contract_StartDate"].ToString() != "")
                {
                    LblContract_StartDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["Contract_StartDate"].ToString()).ToString("dd MMM yyyy");
                }
                if (ds.Tables[0].Rows[0]["Contract_Completion_Date"].ToString() != "")
                {
                    LblContract_CompletionDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["Contract_Completion_Date"].ToString()).ToString("dd MMM yyyy");
                }

            }
        }

        public void GetTask_by_UID(string TaskUID)
        {
            DataTable ds = dbgetdata.GetTaskDetails_TaskUID(TaskUID);
         
            if (ds.Rows.Count > 0)
            {
                LblTaskName.Text = ds.Rows[0]["Name"].ToString();
                LblTaskDescription.Text = ds.Rows[0]["Description"].ToString();
                if (ds.Rows[0]["StartDate"].ToString() != "")
                {
                    LblTaskStartDate.Text = Convert.ToDateTime(ds.Rows[0]["StartDate"].ToString()).ToString("dd MMM yyyy");
                }
                else
                {
                    LblTaskStartDate.Text = "";
                }
                if (ds.Rows[0]["PlannedEndDate"].ToString() != "")
                {
                    LblTaskPlannedEndDate.Text = Convert.ToDateTime(ds.Rows[0]["PlannedEndDate"].ToString()).ToString("dd MMM yyyy");
                }
                else
                {
                    LblTaskPlannedEndDate.Text = "";
                }
                //LblTaskCurrency.Text = ds.Rows[0]["Currency"].ToString();
                //LblTaskBudget.Text = Convert.ToDouble(ds.Rows[0]["Total_Budget"].ToString()).ToString("#,##.00", CultureInfo.CreateSpecificCulture(ds.Rows[0]["Currency_CultureInfo"].ToString()));
                LblTaskStatus.Text = GetStatus(ds.Rows[0]["Status"].ToString());

                if (ds.Rows[0]["UnitforProgress"].ToString() != "")
                {
                    MeasurementDetails.Visible = true;
                    LblMeasurementUnit.Text = ds.Rows[0]["UnitforProgress"].ToString();
                    LblMeasurementTotalQuantity.Text= ds.Rows[0]["UnitQuantity"].ToString();
                    //double CumulativeQuan = dbgetdata.GetMeasurementCumulativeQuantity(new Guid(TaskUID));
                    if (ds.Rows[0]["CumulativeAchvQuantity"] != DBNull.Value)
                    {
                        if (ds.Rows[0]["ParentTaskID"] != DBNull.Value)
                        {
                            if (dbgetdata.GetTaskCount_by_ParentTaskID(new Guid(ds.Rows[0]["ParentTaskID"].ToString())) > 0)
                            {
                                if (ds.Rows[0]["UnitforProgress"].ToString().ToLower() == "percentage")
                                {
                                    LblMeasurementCumulativeQuantity.Text = ds.Rows[0]["Task_CulumativePercentage"].ToString() == "" ? "0 %" : Convert.ToDecimal(ds.Rows[0]["Task_CulumativePercentage"].ToString()).ToString("0.###");
                                }
                                else
                                {
                                    LblMeasurementCumulativeQuantity.Text = ds.Rows[0]["CumulativeAchvQuantity"].ToString();
                                }
                            }
                            else
                            {
                                LblMeasurementCumulativeQuantity.Text = ds.Rows[0]["CumulativeAchvQuantity"].ToString();
                            }
                        }
                        else
                        {
                            LblMeasurementCumulativeQuantity.Text = ds.Rows[0]["CumulativeAchvQuantity"].ToString();
                        }
                    }
                    else
                    {
                        LblMeasurementCumulativeQuantity.Text = "0";
                    }
                }
                else
                {
                    MeasurementDetails.Visible = false;
                }
                LblCumulativePercentage.Text = ds.Rows[0]["Task_CulumativePercentage"].ToString() == "" ? "0 %" : Convert.ToDecimal(ds.Rows[0]["Task_CulumativePercentage"].ToString()).ToString("0.###") + " %";
                LoadTaskMileStones(TaskUID);
                BindDependencies(TaskUID);
                AddTaskMileStone.HRef = "/_modal_pages/add-milestone.aspx?TaskUID=" + TaskUID;
                AddTaskResources.HRef = "/_modal_pages/add-resourceallocated.aspx?TaskUID=" + TaskUID + "&ProjectUID=" + ds.Rows[0]["ProjectUID"].ToString() + "&WorkPackageUID=" + ds.Rows[0]["WorkPackageUID"].ToString();
                AddDependency.HRef = "/_modal_pages/add-dependency.aspx?type=add&TaskUID=" + TaskUID + "&WorkUID=" + ds.Rows[0]["WorkPackageUID"].ToString();
                LoadResourceAllocated(TaskUID);
               AddSubTask.HRef = "/_modal_pages/add-subtask.aspx?type=add&ParentTaskUID=" + TaskUID + "&PrjUID=" + ds.Rows[0]["ProjectUID"].ToString() + "&WrkUID=" + ds.Rows[0]["WorkPackageUID"].ToString() + "&OptionUID="+ ds.Rows[0]["Workpackage_Option"].ToString();
                TaskSchedule.HRef= "/_modal_pages/addtask-targetschedule.aspx?type=add&TaskUID=" + TaskUID + "&WorkUID=" + ds.Rows[0]["WorkPackageUID"].ToString();
            }
        }

        private void LoadTaskMileStones(string TaskUID)
        {
            grdMileStones.DataSource = dbgetdata.getTaskMileStones(new Guid(TaskUID));
            grdMileStones.DataBind();
        }

        private void LoadResourceAllocated(string TaskUID)
        {
            grdResourceAllocated.DataSource = dbgetdata.getTaskResourceAllocated(new Guid(TaskUID));
            grdResourceAllocated.DataBind();
        }

        protected void grdResourceAllocated_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[5].Text != "&nbsp;")
                {
                    DataSet ds = dbgetdata.GetUserResource_by_TaskUID_AllocationUID(new Guid(e.Row.Cells[7].Text), new Guid(e.Row.Cells[5].Text));
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        e.Row.Cells[5].Text = ds.Tables[0].Rows[0]["Usage"].ToString();
                    }
                    else
                    {
                        e.Row.Cells[5].Text = "0";
                    }
                    //Label lblgst = (Label)e.Row.FindControl("LblGST");
                    //DataSet res = dbgetdata.getResourceMasterDetails(new Guid(e.Row.Cells[7].Text));
                    //DataSet res= dbgetdata.GetBOQDetails_by_BOQDetailsUID(new Guid(DDLBOQDetails.SelectedValue));
                    //if (res.Tables[0].Rows.Count > 0)
                    //{
                    //    lblgst.Text = res.Tables[0].Rows[0]["GST"].ToString() + "%";
                    //}
                    //else
                    //{
                    //    lblgst.Text = "0";
                    //}
                }
            }
        }

        protected void grdMileStones_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[6].Text != "&nbsp;")
                {
                    DateTime dt = Convert.ToDateTime(e.Row.Cells[6].Text);
                    if (dt.Date < DateTime.Now.Date && e.Row.Cells[4].Text == "Not Completed")
                    {
                        e.Row.Cells[3].BackColor = System.Drawing.Color.Red;
                        e.Row.Cells[3].ForeColor = System.Drawing.Color.White;
                    }
                }
                if (e.Row.Cells[4].Text == "Completed")
                {
                    e.Row.Cells[4].ForeColor = System.Drawing.Color.Green;
                }
            }
        }

        public string LimitCharts(string Desc)
        {
            if (Desc.Length > 120)
            {
                return Desc.Substring(0, 120) + "  . . .";
            }
            else
            {
                return Desc;
            }
        }

        public string ShoworHide(string Desc)
        {
            if (Desc.Length > 45)
            {
                return "More";
            }
            else
            {
                return string.Empty;
            }
        }
        

        public string GetStatus(string StatusCode)
        {
            string retStatus = StatusCode;
            if (StatusCode == "I")
            {
                retStatus = "In-Progress";
            }
            else if (StatusCode == "P")
            {
                retStatus = "Not Started";
            }
            else if (StatusCode == "C")
            {
                retStatus = "Completed";

            }
            return retStatus;
        }

        protected void BtnSearchDocuments_Click(object sender, EventArgs e)
        {
            if (TxtSearchDocuments.Value == "")
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Please enter activity name..');</script>");
            }
            else
            {
                if (!string.IsNullOrEmpty(TxtSearchDocuments.Value))
                {
                    BindTreeview();
                    for (int i = 0; i < TreeView1.Nodes.Count; i++)
                    {
                        retrieveNodes_Search(TreeView1.Nodes[i], TxtSearchDocuments.Value);
                    }
                    if (SearchResultCount == 0) {
                        
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('" + TxtSearchDocuments.Value + " not found.');</script>");
                        TxtSearchDocuments.Value = "";
                    }
                    //    TreeNode searchedNode = null;
                    //    foreach (TreeNode node in TreeView1.Nodes)
                    //    {
                    //        searchedNode = SearchNode(node, TxtSearchDocuments.Value);
                    //        if (searchedNode == null)
                    //        {
                    //            foreach (TreeNode childNode in node.ChildNodes)
                    //            {
                    //                searchedNode = SearchNode(childNode, TxtSearchDocuments.Value);
                    //                if (searchedNode != null)
                    //                    goto Here;
                    //            }
                    //        }
                    //        else
                    //        {
                    //            break;
                    //        }
                    //    }
                    //Here:
                    //    if (searchedNode != null)
                    //    {
                    //        searchedNode.Select();

                    //        TreeView1.ExpandAll();
                    //        TxtSearchDocuments.Value = "";
                    //        //BindActivities();
                    //    }
                    //    else
                    //    {
                    //        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Activity " + TxtSearchDocuments.Value + " not found');</script>");
                    //    }
                }
            }
        }

        TreeNode SearchNode(TreeNode node, string searchText)
        {
            if (node.Text.Contains(searchText)) return node;

            TreeNode tn = null;
            foreach (TreeNode childNode in node.ChildNodes)
            {
                tn = SearchNode(childNode, searchText);
                if (tn != null) break;
            }

            if (tn != null) node.Expand();
            return tn;
        }

        public string getTaskName(string TaskUID)
        {
            return dbgetdata.getTaskNameby_TaskUID(new Guid(TaskUID));
        }

        protected void BindDependencies(string TaskUID)
        {
            DataSet ds = dbgetdata.getDependencies(new Guid(TaskUID));
            GrdDependency.DataSource = ds;
            GrdDependency.DataBind();
        }
        public string DependencyType(string dCode)
        {
            if (dCode == "FS")
            {
                return "Finish to Start";
            }
            else if (dCode == "SF")
            {
                return "Start to Finish";
            }
            else if (dCode == "SS")
            {
                return "Start to Start";
            }
            else
            {
                return "Finish to Finish";
            }
        }

        protected void btnEnableOption_Click(object sender, EventArgs e)
        {
            DataSet ds = dbgetdata.WorkpackageSelectedOptions_by_UID(new Guid(TreeView1.SelectedNode.Value));
            if (ds.Tables[0].Rows.Count > 0)
            {
                CopyMasterActivityData(new Guid(ds.Tables[0].Rows[0]["WorkPackageUID"].ToString()), new Guid(ds.Tables[0].Rows[0]["Workpackage_OptionUID"].ToString()), new Guid(DDlProject.SelectedValue), new Guid(TreeView1.SelectedNode.Value));
                int cnt = dbgetdata.WorkpackageSelectedOption_Enabled(new Guid(TreeView1.SelectedNode.Value));
                if (cnt > 0)
                {
                    Session["SelectedActivity"] = TreeView1.SelectedNode.Value;
                    Response.Redirect(Request.Url.AbsoluteUri);
                }
            }
        }

        private void CopyMasterActivityData(Guid WorkpackageUID, Guid OptionUID, Guid ProjectUID,Guid SelectedOptionUID)
        {
            DataSet ds = dbgetdata.WorkpackageMainActivityMaster_SelectBy_OptionUID(OptionUID);
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Guid TaskUID = Guid.NewGuid();
                    try
                    {

                        bool result = dbgetdata.InsertorUpdateMainTask_From_Master(TaskUID, WorkpackageUID, ProjectUID, SelectedOptionUID, Session["UserUID"].ToString(), ds.Tables[0].Rows[i]["WorkpakageActivity_Name"].ToString(), ds.Tables[0].Rows[i]["WorkpakageActivity_Name"].ToString(), "P", 0, 0, 1, 0, 0, 0, "&#x20B9;", "en-IN", Convert.ToInt32(ds.Tables[0].Rows[i]["WorkpakageActivity_Order"].ToString()));
                        if (result)
                        {
                            TaskDataInsert(WorkpackageUID, new Guid(ds.Tables[0].Rows[i]["WorkpakageActivity_MasterUID"].ToString()), TaskUID, ProjectUID, SelectedOptionUID);
                        }
                        else
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code AWPM-01 there is a problem with this feature. Please contact system admin.');</script>");
                        }
                    }
                    catch (Exception ex)
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code AWPM-01 there is a problem with this feature. Please contact system admin.');</script>");
                    }

                }
            }
        }
        private void TaskDataInsert(Guid WorkpackageUID, Guid FromParentTaskUID, Guid ToParentTaskUID,Guid ProjectUID,Guid SelectedOptionUID)
        {
            DataSet ds = dbgetdata.WorkpackageActivityMaster_SelectBy_ParentUID(FromParentTaskUID);
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Guid sSubTaskUID = Guid.NewGuid();
                    try
                    {

                        bool result = dbgetdata.InsertorUpdateSubTask_From_Master(sSubTaskUID, WorkpackageUID, ProjectUID, SelectedOptionUID, Session["UserUID"].ToString(), ds.Tables[0].Rows[i]["WorkpakageActivity_Name"].ToString(), ds.Tables[0].Rows[i]["WorkpakageActivity_Name"].ToString(), "P", 0, 0, Convert.ToInt32(ds.Tables[0].Rows[i]["WorkpakageActivity_Level"].ToString()), 0, 0, 0, "&#x20B9;", "en-IN", Convert.ToInt32(ds.Tables[0].Rows[i]["WorkpakageActivity_Order"].ToString()), ToParentTaskUID.ToString());
                        if (result)
                        {
                            TaskDataInsert(WorkpackageUID, new Guid(ds.Tables[0].Rows[i]["WorkpakageActivity_MasterUID"].ToString()), sSubTaskUID, ProjectUID, SelectedOptionUID);
                        }
                    }
                    catch (Exception ex)
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code SAWPM-01 there is a problem with this feature. Please contact system admin.');</script>");
                    }

                }
            }
        }

        public string GetWorkpackageOptionName(string Workpackage_OptionUID)
        {
            return dbgetdata.WorkpackageoptionName_SelectBy_UID(new Guid(Workpackage_OptionUID));
        }

        protected void GrdOptions_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string UID = e.CommandArgument.ToString();
            if (e.CommandName == "view")
            {
                TreeView1.CollapseAll();
                DataSet ds = dbgetdata.GetWorkpackage_SelectOption_by_UID(new Guid(UID));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < TreeView1.Nodes.Count; i++)
                    {
                        retrieveNodes_Options(TreeView1.Nodes[i], ds.Tables[0].Rows[0]["Workpackage_OptionUID"].ToString(), ds.Tables[0].Rows[0]["WorkPackageUID"].ToString());
                    }
                }
                
                //DataSet ds = dbgetdata.GetWorkPackages_By_WorkPackageUID(new Guid(UID));
                //if (ds.Tables[0].Rows.Count > 0)
                //{
                //    for (int i = 0; i < TreeView1.Nodes.Count; i++)
                //    {
                //        retrieveNodes_by_Parent(TreeView1.Nodes[i], ds.Tables[0].Rows[0]["Workpackage_OptionUID"].ToString(), UID);
                //    }
                //}

            }
        }

        protected void retrieveNodes_by_Parent(TreeNode node, string SelectedVal,string WorkpackageValue)
        {
            if (node.Value == SelectedVal && node.Parent.Value == WorkpackageValue)
            {
                node.Selected = true;
                node.Expand();
                if (node.Parent != null)
                {
                    node.Parent.Expand();
                }
                BindActivities();
            }
            else
            {
                if (node.ChildNodes.Count != 0)
                {
                    foreach (TreeNode tn in node.ChildNodes)
                    {
                        if (tn.Value == SelectedVal && tn.Parent.Value== WorkpackageValue)
                        {
                            tn.Selected = true;
                            tn.Expand();
                            if (tn.Parent != null)
                            {
                                tn.Parent.Expand();
                                if (tn.Parent.Parent != null)
                                {
                                    tn.Parent.Parent.Expand();

                                    if (tn.Parent.Parent.Parent != null)
                                    {
                                        tn.Parent.Parent.Parent.Expand();

                                        if (tn.Parent.Parent.Parent.Parent != null)
                                        {
                                            tn.Parent.Parent.Parent.Parent.Expand();

                                            if (tn.Parent.Parent.Parent.Parent.Parent != null)
                                            {
                                                tn.Parent.Parent.Parent.Parent.Parent.Expand();

                                                if (tn.Parent.Parent.Parent.Parent.Parent.Parent != null)
                                                {
                                                    tn.Parent.Parent.Parent.Parent.Parent.Parent.Expand();
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            BindActivities();
                        }
                        else
                        {
                            if (tn.ChildNodes.Count != 0)
                            {
                                retrieveNodes_by_Parent(tn, SelectedVal, WorkpackageValue);
                            }
                        }
                    }
                }
            }
        }

        protected void grdMileStones_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string UID = e.CommandArgument.ToString();
            if (e.CommandName == "delete")
            {
                int cnt = dbgetdata.MileStone_Delete(new Guid(UID), new Guid(Session["UserUID"].ToString()));
                if (cnt > 0)
                {
                    LoadTaskMileStones(TreeView1.SelectedNode.Value);
                }
            }
        }

        protected void grdMileStones_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void grdResourceAllocated_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string UID = e.CommandArgument.ToString();
            if (e.CommandName == "delete")
            {
                int cnt = dbgetdata.ResourceAllocation_Delete(new Guid(UID), new Guid(Session["UserUID"].ToString()));
                if (cnt > 0)
                {
                    LoadResourceAllocated(TreeView1.SelectedNode.Value);
                }
            }
        }

        protected void grdResourceAllocated_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void GrdDependency_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string dUID = e.CommandArgument.ToString();
            if (e.CommandName == "delete")
            {
                int cnt = dbgetdata.Dependency_Delete(new Guid(dUID), new Guid(Session["UserUID"].ToString()));
                if (cnt > 0)
                {
                    BindDependencies(TreeView1.SelectedNode.Value);
                }
            }
        }

        protected void GrdDependency_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        private void LoadResourceDeployment(string WorkpackageUID)
        {
            GrdResourceDeployment.DataSource = dbgetdata.getResourceMaster(new Guid(WorkpackageUID));
            GrdResourceDeployment.DataBind();
        }
    }
}