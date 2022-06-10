using ProjectManagementTool.Models;
using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagementTool._content_pages.task_report_selection
{
    public partial class _default : System.Web.UI.Page
    {
        TaskUpdate TKUpdate = new TaskUpdate();
        DBGetData getdata = new DBGetData();


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadProjects();
                SelectedProjectWorkpackage("Project");
                ddlProject_SelectedIndexChanged(sender, e);
                TreeView1.Nodes.Clear();
            }
        }

        protected void ddlworkpackage_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (!string.IsNullOrEmpty(ddlworkpackage.SelectedValue))
            {
                Session["ProjectUID"] = ddlProject.SelectedValue;
                Session["WorkPackageUID"] = ddlworkpackage.SelectedValue;
            }
        }

        private void LoadProjects()
        {
            try
            {
                DataTable ds = new DataTable();
                if (Session["TypeOfUser"].ToString() == "U" || Session["TypeOfUser"].ToString() == "MD" || Session["TypeOfUser"].ToString() == "VP")
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
                ddlProject.DataSource = ds;
                ddlProject.DataTextField = "ProjectName";
                ddlProject.DataValueField = "ProjectUID";
                ddlProject.DataBind();
            }
            catch (Exception ex)
            {
            }
        }

        internal void SelectedProjectWorkpackage(string pType)
        {
            if (!IsPostBack && Session["Project_Workpackage"] != null)
            {
                string[] selectedValue = Session["Project_Workpackage"].ToString().Split('_');
                if (selectedValue.Length > 1)
                {
                    if (pType == "Project")
                    {
                        ddlProject.SelectedValue = selectedValue[0];
                    }
                    else
                    {
                        ddlworkpackage.SelectedValue = selectedValue[1];
                    }
                }
                else
                {
                    if (pType == "Project")
                    {
                        ddlProject.SelectedValue = Session["Project_Workpackage"].ToString();
                    }
                }

                if (DropDownList1.SelectedIndex >0 ) BindTreeview();
            }

        }

        protected void ddlProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlProject.SelectedValue != "")
            {

                // divStatus.Visible = false;
                // divStatusMonth.Visible = true;
                DataSet ds = new DataSet();
                if (Session["TypeOfUser"].ToString() == "U" || Session["TypeOfUser"].ToString() == "MD" || Session["TypeOfUser"].ToString() == "VP")
                {
                    ds = getdata.GetWorkPackages_By_ProjectUID(new Guid(ddlProject.SelectedValue));
                }
                else if (Session["TypeOfUser"].ToString() == "PA")
                {
                    ds = getdata.GetWorkPackages_ForUser_by_ProjectUID(new Guid(Session["UserUID"].ToString()), new Guid(ddlProject.SelectedValue));
                }
                else
                {
                    ds = getdata.GetWorkPackages_ForUser_by_ProjectUID(new Guid(Session["UserUID"].ToString()), new Guid(ddlProject.SelectedValue));
                }
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlworkpackage.DataTextField = "Name";
                    ddlworkpackage.DataValueField = "WorkPackageUID";
                    ddlworkpackage.DataSource = ds;
                    ddlworkpackage.DataBind();
                    SelectedProjectWorkpackage("Workpackage");

                    // AddData.HRef = "~/_modal_pages/add-Fin-Month-data.aspx?WorkPackageUID=" + ddlworkpackage.SelectedValue;
                    //Loadtasks(ddlworkpackage.SelectedValue);
                    //LoadSTask(ddlTask.SelectedItem.Value);

                    DataSet dschild = getdata.GetTasksForWorkPackages(ddlworkpackage.SelectedValue);
                    if (dschild.Tables[0].Rows.Count > 0)
                    {
                        //TreeView1.Nodes.Clear();
                        // PopulateTreeView(dschild, null, "", 0);
                        //  TreeView1.Nodes[0].Selected = true;
                        // TreeView1.CollapseAll();
                        //TreeView1.Nodes[0].Expand();
                        ActivityHeading.Text = "WorkPackage : " + ddlworkpackage.SelectedItem.ToString() + "(Task Selection Update)";
                        //divStatus.Visible = true;
                        ////LoadTaskPayments(TreeView1.SelectedNode.Value);
                        ////FillAllowedPayment();
                        //FinanceMileStoneBind(TreeView1.SelectedNode.Value);
                        // FinanceMileStoneMonthBind(ddlworkpackage.SelectedValue);
                    }

                    Session["Project_Workpackage"] = ddlProject.SelectedValue + "_" + ddlworkpackage.SelectedValue;

                    Session["ProjectUID"] = ddlProject.SelectedValue;
                    Session["WorkPackageUID"] = ddlworkpackage.SelectedValue;

                    if (DropDownList1.SelectedIndex > 0 ) BindTreeview();
                }
            }
        }

        protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
        {
           // getdata.UpdateTask(TreeView1.SelectedValue, DropDownList1.SelectedValue);
        }

        public void BindTreeview()
        {
            TreeView1.Nodes.Clear();
            
            DataSet ds = new DataSet();
            if (Session["TypeOfUser"].ToString() == "U" || Session["TypeOfUser"].ToString() == "MD" || Session["TypeOfUser"].ToString() == "VP")
            {
                ds = getdata.GetWorkPackages_By_ProjectUID(new Guid(ddlProject.SelectedValue));
            }
            else if (Session["TypeOfUser"].ToString() == "PA")
            {
                ds = getdata.GetWorkPackages_ForUser_by_ProjectUID(new Guid(Session["UserUID"].ToString()), new Guid(ddlProject.SelectedValue));
            }
            else
            {
                ds = getdata.GetWorkPackages_ForUser_by_ProjectUID(new Guid(Session["UserUID"].ToString()), new Guid(ddlProject.SelectedValue));
            }
            if (ds.Tables[0].Rows.Count > 0)
            {
                PopulateTreeView(ds, null, "", 2);
                TreeView1.Nodes[0].Expand();
                foreach (TreeNode node in TreeView1.Nodes[0].ChildNodes)
                {
                    node.Expand();
                }
            }
        }

        

        public void PopulateTreeView(DataSet dtParent, TreeNode treeNode, string ParentUID, int Level)
        {
            foreach (DataRow row in dtParent.Tables[0].Rows)
            {
                TreeNode child = new TreeNode
                {
                    Text = Level == 0 ? row["ProjectClass_Name"].ToString() : Level == 1 ? LimitCharts(getdata.getProjectNameby_ProjectUID(new Guid(row["ProjectUID"].ToString()))) : Level == 2 ? row["Name"].ToString() : Level == 3 ? row["WorkpackageSelectedOption_Name"].ToString() : row["Name"].ToString(),
                    Value = Level == 0 ? row["ProjectClass_UID"].ToString() : Level == 1 ? row["ProjectUID"].ToString() : Level == 2 ? row["WorkPackageUID"].ToString() : Level == 3 ? row["Workpackage_OptionUID"].ToString() : row["TaskUID"].ToString(),
                    Target = Level == 0 ? "Class" : Level == 1 ? "Project" : Level == 2 ? "WorkPackage" : Level == 3 ? "Option" : "Tasks",
                    ToolTip = Level == 0 ? row["ProjectClass_Name"].ToString() : Level == 1 ? getdata.getProjectNameby_ProjectUID(new Guid(row["ProjectUID"].ToString())) : Level == 2 ? row["Name"].ToString() : Level == 3 ? row["WorkpackageSelectedOption_Name"].ToString() : row["Name"].ToString(),
                };

                if (Level >3)
                {
                    child.ShowCheckBox = true;
                    if (row["Report"].ToString() == "Y")
                        child.Checked = true;
                }
                                                               
                if (Level == 2)
                {
   
                    TreeView1.Nodes.Add(child);
                    DataSet dsoption = getdata.GetSelectedOption_By_WorkpackageUID(new Guid(child.Value));
                    if (dsoption.Tables[0].Rows.Count > 0)
                    {
                        PopulateTreeView(dsoption, child, child.Value, 3);
                    }
                }
                else if (Level == 3)
                {
                   
                    if (Session["IsContractor"].ToString() == "Y")
                    {
                        if (row["WorkpackageSelectedOption_Name"].ToString() != "Design")
                        {
                            if (row["WorkpackageSelectedOption_Name"].ToString() == "PMC")
                            {
                                child.Text = "Construction & Execution";
                            }

                            treeNode.ChildNodes.Add(child);
                                                    
                            DataSet dschild = getdata.GetTasks_by_WorkpackageOptionUID_ForTaskUpdate(new Guid(child.Parent.Value), new Guid(child.Value),DropDownList1.SelectedValue);
                            
                            if (dschild.Tables[0].Rows.Count > 0)
                            {
                                PopulateTreeView(dschild, child, child.Value, 4);
                            }
                        }
                    }
                    else
                    {
                        
                        treeNode.ChildNodes.Add(child);
                        DataSet dschild = getdata.GetTasks_by_WorkpackageOptionUID_ForTaskUpdate(new Guid(child.Parent.Value), new Guid(child.Value),DropDownList1.SelectedValue);
                        
                        if (dschild.Tables[0].Rows.Count > 0)
                        {
                            PopulateTreeView(dschild, child, child.Value, 4);
                        }
                    }

                }
                else if (Level == 4)
                {
                                       
                    treeNode.ChildNodes.Add(child);
                    DataSet dssubchild = getdata.GetSubTasksForWorkPackages_ForTaskUpdate(child.Value,DropDownList1.SelectedValue);
                    if (dssubchild.Tables[0].Rows.Count > 0)
                    {
                        PopulateTreeView(dssubchild, child, child.Value, 5);
                    }
                }
                else if (Level == 5)
                {
                    treeNode.ChildNodes.Add(child);
                    DataSet dssubtosubchild = getdata.GetSubtoSubTasksForWorkPackages_ForTaskUpdate(child.Value, DropDownList1.SelectedValue);
                    if (dssubtosubchild.Tables[0].Rows.Count > 0)
                    {
                        PopulateTreeView(dssubtosubchild, child, child.Value, 6);
                    }
                }
                else if (Level == 6)
                {
                    treeNode.ChildNodes.Add(child);
                    DataSet lastchild = getdata.GetSubtoSubtoSubTasksForWorkPackages_ForTaskUpdate(child.Value, DropDownList1.SelectedValue);
                    if (lastchild.Tables[0].Rows.Count > 0)
                    {
                        PopulateTreeView(lastchild, child, child.Value, 7);
                    }
                }
                else if (Level == 7)
                {
                    treeNode.ChildNodes.Add(child);
                    DataSet lastchild = getdata.GetSubtoSubtoSubtoSubTasksForWorkPackages_ForTaskUpdate(child.Value, DropDownList1.SelectedValue);
                    if (lastchild.Tables[0].Rows.Count > 0)
                    {
                        PopulateTreeView(lastchild, child, child.Value, 8);
                    }
                }
                else
                {
                    treeNode.ChildNodes.Add(child);
                    DataSet ds = getdata.GetTask_by_ParentTaskUID_ForTaskUpdate(new Guid(child.Value), DropDownList1.SelectedValue);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        PopulateTreeView(ds, child, child.Value, 9);
                    }
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


        //public void BindTreeview()
        //{
        //    DataSet ds = new DataSet();
        //    DataSet ds1 = new DataSet();
        //    if (Session["TypeOfUser"].ToString() == "U" || Session["TypeOfUser"].ToString() == "MD" || Session["TypeOfUser"].ToString() == "VP")
        //    {
        //        ds1 = getdata.GetWorkPackageOptions();

        //        TreeView1.Nodes[0].ChildNodes.Clear();

        //        if (ds1.Tables[0].Rows.Count > 0)
        //        {
        //            foreach (DataRow row in ds1.Tables[0].Rows)
        //            {
        //                ds = getdata.GetTasks_by_WorkPackageUID_ByLevel(ddlworkpackage.SelectedValue, row.ItemArray[0].ToString(),DropDownList1.SelectedValue);

        //                if (ds != null)
        //                {
        //                    if (ds.Tables[0].Rows.Count > 0)
        //                    {
        //                        DataTable dt = ds.Tables[0];

        //                        var nodeList = (from DataRow row1 in dt.Rows
        //                                        select new TaskNode
        //                                        {
        //                                            Id = row1["TaskUID"].ToString(),
        //                                            Name = row1["Name"].ToString(),
        //                                            ParentId = row1["ParentTaskID"].ToString(),
        //                                            TaskSelected = row1["Report"].ToString() == "Y" ? true: false
        //                                        }).ToList();

        //                        TreeNode RootNode = TreeView1.Nodes[0];
        //                        TreeNode ParentNode = new TreeNode(row.ItemArray[1].ToString(), "");
        //                        RootNode.ChildNodes.Add(ParentNode);
        //                        PopulateTreeView(nodeList, ParentNode);
        //                    }
        //                }

        //            }

        //        }

        //        TreeView1.Nodes[0].CollapseAll();

        //        TreeView1.Nodes[0].Expand();

        //        foreach(TreeNode node in TreeView1.Nodes[0].ChildNodes)
        //        {
        //            node.Expand();
        //        }

        //        //else if (Session["TypeOfUser"].ToString() == "PA")
        //        //{
        //        //    ds = getdata.GetWorkPackages_ForUser_by_ProjectUID(new Guid(Session["UserUID"].ToString()), new Guid(ddlProject.SelectedValue));
        //        //}
        //        //else
        //        //{
        //        //    ds = getdata.GetWorkPackages_ForUser_by_ProjectUID(new Guid(Session["UserUID"].ToString()), new Guid(ddlProject.SelectedValue));
        //        //}
        //    }
        //}


        //private void PopulateTreeView(IEnumerable<TaskNode> list, TreeNode parentNode)
        //{
        //    var nodes = list.Where(x => parentNode == null ? x.ParentId == null : x.ParentId == parentNode.Value).ToList();

        //    foreach (var node in nodes)
        //    {
        //        TreeNode newNode = new TreeNode(node.Name, node.Id.ToString());

        //        newNode.ShowCheckBox = true;
        //        newNode.Checked = node.TaskSelected;

        //        if (parentNode == null)
        //        {
        //            TreeView1.Nodes.Add(newNode);
        //        }
        //        else
        //        {
        //            parentNode.ChildNodes.Add(newNode);
        //        }

        //        PopulateTreeView(list, newNode);
        //    }
        //}

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (TreeView1.CheckedNodes.Count >0 & DropDownList1.SelectedIndex > 0)
            {
                foreach(TreeNode node in TreeView1.CheckedNodes)
                {
                    getdata.UpdateTask(node.Value, DropDownList1.SelectedValue);
                }

                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Selected Tasks are updated.');</script>");
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Task/Update Field is not selected');</script>");
            }
        }

        protected void btnExpand_Click(object sender, EventArgs e)
        {
            TreeView1.ExpandAll();
        }

        protected void btnCollapse_Click(object sender, EventArgs e)
        {
            if (TreeView1.Nodes.Count >0)
            {
                TreeView1.Nodes[0].CollapseAll();

                TreeView1.Nodes[0].Expand();

                foreach (TreeNode node in TreeView1.Nodes[0].ChildNodes)
                {
                    node.Expand();
                }
            }
                
        }

        protected void chkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (ddlworkpackage.SelectedValue !="" & DropDownList1.SelectedIndex >0 )
            {
                //foreach (TreeNode node in TreeView1.Nodes[0].ChildNodes)
                //{
                //    SelectAll(node,chkBox1.Checked);
                //}

                getdata.UpdateTaskAll(new Guid(ddlworkpackage.SelectedValue), DropDownList1.SelectedValue, chkBox1.Checked ? "Y" : "N");
                BindTreeview();
                
            }
        }

        public void SelectAll(TreeNode node,bool node_checked)
        {
            node.Checked = true;

            foreach(TreeNode nd in node.ChildNodes)
            {
                if (nd.ChildNodes.Count > 0)
                    SelectAll(nd,node_checked);
                else
                    nd.Checked = node_checked;
            }
        }

         protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownList1.SelectedIndex >0 )
            {
                BindTreeview();
                chkBox1.Checked = false;
            }
            else
            {
                TreeView1.Nodes.Clear();
            }
                
        }
    }
}