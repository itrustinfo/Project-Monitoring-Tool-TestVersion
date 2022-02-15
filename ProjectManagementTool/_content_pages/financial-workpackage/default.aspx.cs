using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagementTool._content_pages.financial_workpackage
{
    public partial class _default : System.Web.UI.Page
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
                    BindTreeview();
                }
            }
        }

        public void BindTreeview()
        {
            TreeView1.Nodes.Clear();
            DataSet ds = new DataSet();
            if (Session["TypeOfUser"].ToString() == "U" || Session["TypeOfUser"].ToString() =="MD" || Session["TypeOfUser"].ToString() == "VP")
            {
                //ds = gettk.GetAllProjects();
                ds = dbgetdata.ProjectClass_Select_All();
            }
            else if (Session["TypeOfUser"].ToString() == "PA")
            {
                //ds = dbgetdata.GetAssignedProjects_by_UserUID(new Guid(Session["UserUID"].ToString()));
                ds = dbgetdata.ProjectClass_Select_By_UserUID(new Guid(Session["UserUID"].ToString()));
            }
            else
            {
                //ds = dbgetdata.GetAssignedProjects_by_UserUID(new Guid(Session["UserUID"].ToString()));
                ds = dbgetdata.ProjectClass_Select_By_UserUID(new Guid(Session["UserUID"].ToString()));
            }
            if (ds.Tables[0].Rows.Count > 0)
            {
                TreeView1.Nodes.Clear();
                PopulateTreeView(ds, null, "", 0);

                if (Session["SelectedActivity"] != null)
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
                    TreeView1.CollapseAll();
                    TreeView1.Nodes[0].Expand();
                    //LoadProjectDetails(TreeView1.SelectedNode.Value);
                }
                BindActivities();
            }
            else
            {
                ProjectDetails.Visible = false;
                Contractors.Visible = false;
                WorkPackageDetils.Visible = false;
                TaskDetails.Visible = false;
                AddFinanceTask.Visible = false;
                AddFinanceSubTask.Visible = false;
            }
            //DataSet ds = getdata.GetTasksForWorkPackages(customerId);
            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    PopulateTreeView(ds, null, "", 0);
            //}
            //TreeView1.CollapseAll();
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
                //    Text = Level == 0 ? row["ProjectClass_Name"].ToString() : Level == 1 ? LimitCharts(dbgetdata.getProjectNameby_ProjectUID(new Guid(row["ProjectUID"].ToString()))) : Level == 2 ? row["Name"].ToString() : Level == 3 ? dbgetdata.WorkpackageoptionName_SelectBy_UID(new Guid(row["Workpackage_OptionUID"].ToString())) : row["Name"].ToString(),
                //    Value = Level == 0 ? row["ProjectClass_UID"].ToString() : Level == 1 ? row["ProjectUID"].ToString() : Level == 2 ? row["WorkPackageUID"].ToString() : Level == 3 ? row["WorkPackageUID"].ToString() : row["TaskUID"].ToString(),
                //    Target = Level == 0 ? "Class" : Level == 1 ? "Project" : Level == 2 ? "WorkPackage" : Level == 3 ? "WorkPackage" : "Tasks",
                //    ToolTip = Level == 0 ? row["ProjectClass_Name"].ToString() : Level == 1 ? dbgetdata.getProjectNameby_ProjectUID(new Guid(row["ProjectUID"].ToString())) : Level == 2 ? row["Name"].ToString() : Level == 3 ? dbgetdata.WorkpackageoptionName_SelectBy_UID(new Guid(row["Workpackage_OptionUID"].ToString())) : row["Name"].ToString()
                //};
                if (ParentUID == "")
                {
                    TreeView1.Nodes.Add(child);
                    DataSet dsProject = new DataSet();
                    if (Session["TypeOfUser"].ToString() == "U" || Session["TypeOfUser"].ToString() =="MD" || Session["TypeOfUser"].ToString() == "VP")
                    {
                        dsProject = gettk.GetProjects_by_ClassUID(new Guid(child.Value));
                    }
                    else
                    {
                        dsProject = gettk.GetUserProjects_by_ClassUID(new Guid(child.Value), new Guid(Session["UserUID"].ToString()));
                    }

                    //DataSet dsProject = gettk.GetUserProjects_by_ClassUID(new Guid(child.Value), new Guid(Session["UserUID"].ToString()));
                    if (dsProject.Tables[0].Rows.Count > 0)
                    {
                        PopulateTreeView(dsProject, child, child.Value, 1);
                    }
                }
                else if (Level == 1)
                {
                    treeNode.ChildNodes.Add(child);
                    //DataSet dsworkPackage = dbgetdata.GetWorkPackages_By_ProjectUID(new Guid(child.Value));
                    DataSet dsworkPackage = new DataSet();
                    if (Session["TypeOfUser"].ToString() == "U" || Session["TypeOfUser"].ToString() == "MD" || Session["TypeOfUser"].ToString() == "VP")
                    {
                        dsworkPackage = dbgetdata.GetWorkPackages_By_ProjectUID(new Guid(child.Value));
                    }
                    else if (Session["TypeOfUser"].ToString() == "PA")
                    {
                        dsworkPackage = dbgetdata.GetWorkPackages_ForUser_by_ProjectUID(new Guid(Session["UserUID"].ToString()), new Guid(child.Value));
                    }
                    else
                    {
                        dsworkPackage = dbgetdata.GetWorkPackages_ForUser_by_ProjectUID(new Guid(Session["UserUID"].ToString()), new Guid(child.Value));
                    }
                    if (dsworkPackage.Tables[0].Rows.Count > 0)
                    {
                        PopulateTreeView(dsworkPackage, child, child.Value, 2);
                    }
                }
                else if (Level == 2)
                {
                    treeNode.ChildNodes.Add(child);
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
                //else if (Level == 2)
                //{
                //    treeNode.ChildNodes.Add(child);
                //    DataSet dsoption = dbgetdata.GetWorkPackages_By_WorkPackageUID(new Guid(child.Value));
                //    if (dsoption.Tables[0].Rows.Count > 0)
                //    {
                //        PopulateTreeView(dsoption, child, child.Value, 3);
                //    }
                //}
                //else if (Level == 3)
                //{
                //    treeNode.ChildNodes.Add(child);
                //    DataSet dschild = dbgetdata.GetTasksForWorkPackages(child.Value);
                //    //DataTable dtChild = TreeViewBAL.BL.TreeViewBL.GetData("Select ID,Name from Module where ProjID=" + child.Value);
                //    //DataSet dschild=dbgetdata.
                //    if (dschild.Tables[0].Rows.Count > 0)
                //    {
                //        PopulateTreeView(dschild, child, child.Value, 4);
                //    }

                //}
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

        protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
        {
            BindActivities();
            
        }

        public void BindActivities()
        {
            if (TreeView1.SelectedNode.Target == "Class")
            {
                ActivityHeading.Text = "Projects";
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
                ProjectDetails.Visible = false;
                WorkPackageDetils.Visible = false;
                TaskDetails.Visible = false;
                Contractors.Visible = false;
                AddFinanceTask.Visible = false;
                AddFinanceSubTask.Visible = false;
                AddFinanceMileStone.Visible = false;
                FinanceMileStone.Visible = false;
                GrdOptions.Visible = false;
                GrdTreeView.Visible = true;
            }
            else if (TreeView1.SelectedNode.Target == "Project")
            {
                LoadProjectDetails(TreeView1.SelectedNode.Value);
                ActivityHeading.Text = "Work Packages";
                DataSet ds = dbgetdata.GetWorkPackage_By_ProjectUID(new Guid(TreeView1.SelectedNode.Value));
                GrdTreeView.DataSource = ds;
                GrdTreeView.DataBind();
                ProjectDetails.Visible = true;
                WorkPackageDetils.Visible = false;
                TaskDetails.Visible = false;
                Contractors.Visible = false;
                AddFinanceTask.Visible = false;
                AddFinanceSubTask.Visible = false;
                AddFinanceMileStone.Visible = false;
                FinanceMileStone.Visible = false;
                GrdOptions.Visible = false;
                GrdTreeView.Visible = true;
            }
            else if (TreeView1.SelectedNode.Target == "WorkPackage")
            {
                ActivityHeading.Text = "Option";
                //DataSet ds = gettk.GetTasks_by_WorkPackage(TreeView1.SelectedNode.Value);
                //GrdTreeView.DataSource = ds;
                //GrdTreeView.DataBind();
                GrdOptions.Visible = true;
                DataSet ds = dbgetdata.GetSelectedOption_By_WorkpackageUID(new Guid(TreeView1.SelectedNode.Value));
                //DataSet ds = dbgetdata.GetWorkPackages_By_WorkPackageUID(new Guid(TreeView1.SelectedNode.Value));
                GrdOptions.DataSource = ds;
                GrdOptions.DataBind();
                GrdTreeView.Visible = false;
                ProjectDetails.Visible = false;
                WorkPackageDetils.Visible = false;
                TaskDetails.Visible = false;
                Contractors.Visible = false;
                AddFinanceTask.Visible = false;
                //AddFinanceTask.HRef = "/_modal_pages/add-finance-task.aspx?type=Add&PrjUID=" + TreeView1.SelectedNode.Parent.Value + "&wUID=" + TreeView1.SelectedNode.Value;
                AddFinanceSubTask.Visible = false;
                AddFinanceMileStone.Visible = false;
                FinanceMileStone.Visible = false;
            }
            else if (TreeView1.SelectedNode.Target == "Option")
            {

                GrdOptions.Visible = false;
                GrdTreeView.Visible = true;
                ActivityHeading.Text = "Activities";
                //DataSet ds = gettk.GetTasks_by_WorkPackage(TreeView1.SelectedNode.Parent.Value);
                try
                {
                    DataSet ds = dbgetdata.GetTasks_by_WorkpackageOptionUID(new Guid(TreeView1.SelectedNode.Parent.Value),new Guid(TreeView1.SelectedNode.Value));
                    GrdTreeView.DataSource = ds;
                    GrdTreeView.DataBind();
                }
                catch (Exception ex)
                {

                }
                ProjectDetails.Visible = false;
                WorkPackageDetils.Visible = false;
               
                TaskDetails.Visible = false;
                Contractors.Visible = false;
            }
            else
            {
                
                ActivityHeading.Text = "Sub Activities";
                DataSet ds = dbgetdata.GetTask_by_ParentTaskUID(new Guid(TreeView1.SelectedNode.Value));
                GrdTreeView.DataSource = ds;
                GrdTreeView.DataBind();
                ProjectDetails.Visible = false;
                WorkPackageDetils.Visible = false;
                TaskDetails.Visible = true;
                GetTask_by_UID(TreeView1.SelectedNode.Value);
                Contractors.Visible = false;
                AddFinanceTask.Visible = false;
                AddFinanceSubTask.Visible = false;
                //FinancePaymentBind(TreeView1.SelectedNode.Value);
                AddFinanceMileStone.Visible = true;
                FinanceMileStone.Visible = true;
                AddFinanceMileStone.HRef = "/_modal_pages/add-financemilestone.aspx?type=Add&TaskUID=" + TreeView1.SelectedNode.Value + "";
                PaymentHistroy.HRef = "/_modal_pages/view-taskpaymenthistroy.aspx?TaskUID=" + TreeView1.SelectedNode.Value + "";
                FinanceMileStoneBind(TreeView1.SelectedNode.Value);
            }
        }

        public void GetContractors(Guid ContractUID)
        {
            DataSet ds = dbgetdata.GetContractors_by_ContractorUID(ContractUID);
            if (ds.Tables[0].Rows.Count > 0)
            {
                LblContractor.Text = ds.Tables[0].Rows[0]["Contractor_Name"].ToString();
                LblContract_Type.Text = ds.Tables[0].Rows[0]["Type_of_Contract"].ToString();
                //LblContract_Value.Text = Convert.ToDouble(ds.Tables[0].Rows[0]["Contract_Value"].ToString()).ToString("#,##0");
                LblContract_Duration.Text = GetStatus(ds.Tables[0].Rows[0]["Contract_Duration"].ToString()) + " Months";


            }
        }

        public void GetWorkPackage_by_UID(Guid WorkPackageID)
        {
            DataSet ds = dbgetdata.GetWorkPackages_By_WorkPackageUID(WorkPackageID);
            if (ds.Tables[0].Rows.Count > 0)
            {
                LblWorkPackageName.Text = ds.Tables[0].Rows[0]["Name"].ToString();
                LblWorkPackageLocation.Text = ds.Tables[0].Rows[0]["WorkPackage_Location"].ToString();
                LblWorkPackageBudgetCurrency.Text = ds.Tables[0].Rows[0]["Currency"].ToString();
                LblWorkPackageBudget.Text = Convert.ToDouble(ds.Tables[0].Rows[0]["Budget"].ToString()).ToString("#,##.00", CultureInfo.CreateSpecificCulture(ds.Tables[0].Rows[0]["Currency_CultureInfo"].ToString()));
                lblWorkpackageActualExpenditureCurrency.Text= ds.Tables[0].Rows[0]["Currency"].ToString();
                lblWorkpackageActualExpenditure.Text = Convert.ToDouble(ds.Tables[0].Rows[0]["ActualExpenditure"].ToString()).ToString("#,##.00", CultureInfo.CreateSpecificCulture(ds.Tables[0].Rows[0]["Currency_CultureInfo"].ToString()));
                //LblWorkPackageBudget.Text = ds.Tables[0].Rows[0]["Budget"].ToString();
                //LblWorkPackageActualExpenditure.Text = ds.Tables[0].Rows[0]["ActualExpenditure"].ToString();
                if (ds.Tables[0].Rows[0]["Contractor_UID"].ToString() != "")
                {
                    GetContractors(new Guid(ds.Tables[0].Rows[0]["Contractor_UID"].ToString()));
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
                LblTaskBudgetCurrency.Text = ds.Rows[0]["Currency"].ToString();
                LblTaskGST.Text = ds.Rows[0]["GST"].ToString();
                LblTaskBudget.Text = Convert.ToDouble(ds.Rows[0]["Total_Budget"].ToString()).ToString("#,##.00", CultureInfo.CreateSpecificCulture(ds.Rows[0]["Currency_CultureInfo"].ToString()));
                LblTaskActualExpenditureCurrency.Text= ds.Rows[0]["Currency"].ToString();
                double AE = dbgetdata.GetTask_ActualExpenditure_by_TaskUID(new Guid(TaskUID));
                if (AE > 0)
                {
                    LblTaskActualExpenditure.Text = AE.ToString("#,##.00", CultureInfo.CreateSpecificCulture(ds.Rows[0]["Currency_CultureInfo"].ToString()));
                }
                else
                {
                    LblTaskActualExpenditure.Text = Convert.ToDouble(ds.Rows[0]["ActualExpenditure"].ToString()).ToString("#,##.00", CultureInfo.CreateSpecificCulture(ds.Rows[0]["Currency_CultureInfo"].ToString()));
                }

                AddFinanceSubTask.HRef = "/_modal_pages/add-finance-subtask.aspx?type=add&ParentTaskUID=" + TaskUID + "&PrjUID=" + ds.Rows[0]["ProjectUID"].ToString() + "&WrkUID=" + ds.Rows[0]["WorkPackageUID"].ToString();
            }
        }

        protected void retrieveNodes(TreeNode node, string SelectedVal)
        {
            if (node.Value == SelectedVal)
            {
                node.Selected = true;
                node.Expand();
                node.Parent.Expand();
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
                int cnt = dbgetdata.Tasks_Delete_by_TaskUID(new Guid(UID),new Guid(Session["UserUID"].ToString()));
                if (cnt > 0)
                {
                    BindActivities();
                }
            }
        }

        protected void GrdTreeView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdTreeView.PageIndex = e.NewPageIndex;
            DataSet ds = dbgetdata.GetTasks_by_ProjectUID(new Guid(TreeView1.SelectedNode.Value));
            GrdTreeView.DataSource = ds;
            GrdTreeView.DataBind();

        }

        private void LoadProjectDetails(string ProjectUID)
        {
            try
            {
                DataSet ds = new DataSet();
                ds = dbgetdata.GetProject_by_ProjectUID(new Guid(ProjectUID));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    lblPrjName.Text = ds.Tables[0].Rows[0]["ProjectName"].ToString();
                    //lblPrjOwner.Text = ds.Tables[0].Rows[0]["OwnerName"].ToString();
                    LblPrjBudgetCurrency.Text = ds.Tables[0].Rows[0]["Currency"].ToString();
                    lblPrjBudget.Text = Convert.ToDouble(ds.Tables[0].Rows[0]["Budget"].ToString()).ToString("#,##.00", CultureInfo.CreateSpecificCulture(ds.Tables[0].Rows[0]["Currency_CultureInfo"].ToString()));
                    lblPrjActaulExpenditureCurrency.Text= ds.Tables[0].Rows[0]["Currency"].ToString();
                    lblPrjActaulExpenditure.Text = Convert.ToDouble(ds.Tables[0].Rows[0]["ActualExpenditure"].ToString()).ToString("#,##.00", CultureInfo.CreateSpecificCulture(ds.Tables[0].Rows[0]["Currency_CultureInfo"].ToString()));
                    LblFundingAgency.Text = ds.Tables[0].Rows[0]["Funding_Agency"].ToString();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void FinanceMileStoneBind(string TaskUID)
        {
            try
            {
                DataSet ds = dbgetdata.GetFinance_MileStonesDetails_By_TaskUID(new Guid(TaskUID));
                GrdFinanceMileStone.DataSource = ds;
                GrdFinanceMileStone.DataBind();

                if (ds.Tables[0].Rows.Count > 0)
                {
                    PaymentHistroy.Visible = false;
                }
                else
                {
                    PaymentHistroy.Visible = false;
                }
            }
            catch (Exception ex)
            {

            }
            
        }

        public string GetOwnerName(string UserUID)
        {
            if (UserUID != "")
            {
                return dbgetdata.getUserNameby_UID(new Guid(UserUID));
            }
            else
            {
                return "";
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
               
        public string LimitCharts(string Desc)
        {
            if (Desc.Length > 100)
            {
                return Desc.Substring(0, 100) + "  . . .";
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

        TreeNode SearchNode(TreeNode node, string SearchText = null)
        {
            if (node.Text == SearchText) return node;

            TreeNode tn = null;
            foreach (TreeNode childNode in node.ChildNodes)
            {
                tn = SearchNode(childNode);
                if (tn != null)
                {
                    break;
                }
                else
                {
                    tn = SearchNode(childNode);
                    if (tn != null) break;
                }
            }

            if (tn != null) node.Expand();
            return tn;
        }

        protected void btnTreeviewSearch_Click(object sender, EventArgs e)
        {
            if (txtTreeviewSearch.Text == "")
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Please enter activity name..');</script>");
            }
            else
            {
                if (!string.IsNullOrEmpty(txtTreeviewSearch.Text))
                {
                    BindTreeview();
                    for (int i = 0; i < TreeView1.Nodes.Count; i++)
                    {
                        retrieveNodes_Search(TreeView1.Nodes[i], txtTreeviewSearch.Text);
                    }
                    if (SearchResultCount == 0)
                    {
                        
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('" + txtTreeviewSearch.Text + " not found.');</script>");
                        txtTreeviewSearch.Text = "";
                    }
                   
                }
            }
        }


        protected void GrdTreeView_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (TreeView1.SelectedNode.Target == "Class")
                {
                    e.Row.Cells[4].Visible = false;
                    GrdTreeView.HeaderRow.Cells[4].Visible = false;
                    //e.Row.Cells[5].Visible = false;
                    //GrdTreeView.HeaderRow.Cells[5].Visible = false;
                }
                else if (TreeView1.SelectedNode.Target == "Project")
                {
                    e.Row.Cells[4].Visible = false;
                    //e.Row.Cells[5].Visible = false;
                    //e.Row.Cells[7].Visible = false;
                    GrdTreeView.HeaderRow.Cells[4].Visible = false;
                    //GrdTreeView.HeaderRow.Cells[5].Visible = false;
                    //GrdTreeView.HeaderRow.Cells[7].Visible = false;
                }
                else if (TreeView1.SelectedNode.Target == "WorkPackage")
                {
                    string TaskUID = GrdTreeView.DataKeys[e.Row.RowIndex].Values[0].ToString();
                    string CurrencySymbol = "";
                    if (e.Row.Cells[5].Text == "&#x20B9;" || e.Row.Cells[5].Text == "&amp;#x20B9;")
                    {
                        CurrencySymbol = "₹";
                    }
                    else if (e.Row.Cells[5].Text == "&#36;" || e.Row.Cells[5].Text == "&amp;#36;")
                    {
                        CurrencySymbol = "$";
                    }
                    else
                    {
                        CurrencySymbol = "¥";
                    }
                    double actualexpenditure = dbgetdata.GetTask_ActualExpenditure_by_TaskUID(new Guid(TaskUID));
                    if (actualexpenditure > 0)
                    {
                        e.Row.Cells[3].Text = CurrencySymbol + " " + actualexpenditure.ToString("#,##.##", CultureInfo.CreateSpecificCulture(e.Row.Cells[6].Text)); ;
                    }
                    else
                    {
                        e.Row.Cells[3].Text = CurrencySymbol + " " + "0";
                    }
                }
                else if (TreeView1.SelectedNode.Target == "Option")
                {
                    string TaskUID = GrdTreeView.DataKeys[e.Row.RowIndex].Values[0].ToString();
                    string CurrencySymbol = "";
                    if (e.Row.Cells[5].Text == "&#x20B9;" || e.Row.Cells[5].Text == "&amp;#x20B9;")
                    {
                        CurrencySymbol = "₹";
                    }
                    else if (e.Row.Cells[5].Text == "&#36;" || e.Row.Cells[5].Text == "&amp;#36;")
                    {
                        CurrencySymbol = "$";
                    }
                    else
                    {
                        CurrencySymbol = "¥";
                    }
                    double actualexpenditure = dbgetdata.GetTask_ActualExpenditure_by_TaskUID(new Guid(TaskUID));
                    if (actualexpenditure > 0)
                    {
                        e.Row.Cells[3].Text = CurrencySymbol + " " + actualexpenditure.ToString("#,##.##", CultureInfo.CreateSpecificCulture(e.Row.Cells[6].Text)); ;
                    }
                    else
                    {
                        e.Row.Cells[3].Text = CurrencySymbol + " " + "0";
                    }
                }
                if (TreeView1.SelectedNode.Target == "Tasks")
                {
                    //e.Row.Cells[6].Visible = false;
                    //GrdTreeView.HeaderRow.Cells[6].Visible = false;
                    string TaskUID = GrdTreeView.DataKeys[e.Row.RowIndex].Values[0].ToString();
                    string CurrencySymbol = "";
                    if (e.Row.Cells[5].Text == "&#x20B9;" || e.Row.Cells[5].Text == "&amp;#x20B9;")
                    {
                        CurrencySymbol = "₹";
                    }
                    else if (e.Row.Cells[5].Text == "&#36;" || e.Row.Cells[5].Text == "&amp;#36;")
                    {
                        CurrencySymbol = "$";
                    }
                    else
                    {
                        CurrencySymbol = "¥";
                    }
                    double actualexpenditure = dbgetdata.GetTask_ActualExpenditure_by_TaskUID(new Guid(TaskUID));
                    if (actualexpenditure > 0)
                    {
                        e.Row.Cells[3].Text = CurrencySymbol + " " + actualexpenditure.ToString("#,##.##", CultureInfo.CreateSpecificCulture(e.Row.Cells[6].Text)); ;
                    }
                    else
                    {
                        e.Row.Cells[3].Text = CurrencySymbol + " " + "0";
                    }
                }
            }
        }

        protected void GrdTreeView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }


        //private void FinancePaymentBind(string TaskUID)
        //{
        //    grdTaskPayment.DataSource = dbgetdata.getTaskPayments(new Guid(TaskUID));
        //    grdTaskPayment.DataBind();
        //}
        protected void GrdFinanceMileStone_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[3].Text != "&nbsp;")
                {
                    string CurrencySymbol = "";
                    if (e.Row.Cells[7].Text == "&#x20B9;" || e.Row.Cells[7].Text == "&amp;#x20B9;")
                    {
                        CurrencySymbol = "₹";
                    }
                    else if (e.Row.Cells[7].Text == "&#36;" || e.Row.Cells[7].Text == "&amp;#36;")
                    {
                        CurrencySymbol = "$";
                    }
                    else
                    {
                        CurrencySymbol = "¥";
                    }
                    double cumulative = dbgetdata.FinanceMileStone_CulumativeAmount(new Guid(e.Row.Cells[3].Text));
                    if (cumulative > 0)
                    {
                        e.Row.Cells[3].Text = CurrencySymbol + " " + cumulative.ToString("#,##.##", CultureInfo.CreateSpecificCulture(e.Row.Cells[8].Text)); ;
                    }
                    else
                    {
                        e.Row.Cells[3].Text = CurrencySymbol + " " + "0";
                    }
                }
            }
        }

        protected void GrdFinanceMileStone_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string UID = e.CommandArgument.ToString();
            if (e.CommandName == "delete")
            {
                int Cnt = dbgetdata.FinanceMileStone_Delete(new Guid(UID),new Guid(Session["UserUID"].ToString()));
                if (Cnt > 0)
                {
                    FinanceMileStoneBind(TreeView1.SelectedNode.Value);
                }
            }
        }

        protected void GrdFinanceMileStone_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            
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
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (tn.ChildNodes.Count != 0)
                            {
                                retrieveNodes_Search(tn, SearchText);
                            }
                        }
                    }
                }
            }
        }

        protected void retrieveNodes_Options(TreeNode node, string SelectedVal, string WorkpackageUID)
        {
            if (node.Value == SelectedVal && node.Parent.Value == WorkpackageUID)
            {
                node.Selected = true;
                node.Expand();
                node.Parent.Expand();
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
    }
}