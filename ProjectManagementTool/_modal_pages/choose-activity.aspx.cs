using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagementTool._modal_pages
{
    public partial class choose_activity : System.Web.UI.Page
    {
        DBGetData getdt = new DBGetData();
        TaskUpdate gettk = new TaskUpdate();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Username"] == null)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
            }
            else
            {
                if (!IsPostBack)
                {
                    Session["ActivityUID"] = null;
                    if (Request.QueryString["WorkUID"] != null)
                    {
                        BindActivities();
                        TreeView1.Nodes[0].SelectAction = TreeNodeSelectAction.None;
                    }
                    if (Request.QueryString["TaskUID"] != null)
                    {
                        TreeNode tn = SearchNode(TreeView1.Nodes[0], Request.QueryString["TaskUID"]);
                        if (tn != null)
                        {
                            tn.SelectAction = TreeNodeSelectAction.None;
                            TreeView1.Nodes[0].SelectAction= TreeNodeSelectAction.None;
                        }
                    }
                }
            }
        }

        private void BindActivities()
        {
            DataSet ds = getdt.GetWorkPackages_By_WorkPackageUID(new Guid(Request.QueryString["WorkUID"]));
            if (ds.Tables[0].Rows.Count > 0)
            {
                PopulateTreeView(ds, null, "", 0);
                if (Request.QueryString["TaskUID"] == null)
                {
                    //TreeView1.Nodes[0].Selected = true;
                    
                }
                TreeView1.Nodes[0].Expand();
            }
                
        }
        public void PopulateTreeView(DataSet dtParent, TreeNode treeNode, string ParentUID, int Level)
        {
            foreach (DataRow row in dtParent.Tables[0].Rows)
            {
                TreeNode child = new TreeNode
                {
                    Text = LimitCharts(row["Name"].ToString()),
                    Value = Level == 0 ? row["WorkPackageUID"].ToString() : row["TaskUID"].ToString(),
                    Target = Level == 0 ? "WorkPackage" : "Tasks",
                    ToolTip = row["Name"].ToString()
                };

                //if (ParentUID == "")
                //{
                //    TreeView1.Nodes.Add(child);
                //    DataSet dsworkPackage = getdt.GetWorkPackages_By_ProjectUID(new Guid(child.Value));
                //    if (dsworkPackage.Tables[0].Rows.Count > 0)
                //    {
                //        PopulateTreeView(dsworkPackage, child, child.Value, 1);
                //    }
                //}
                if (ParentUID == "")
                {
                    //treeNode.ChildNodes.Add(child);
                    TreeView1.Nodes.Add(child);
                    DataSet dschild = getdt.GetTasksForWorkPackages(child.Value);
                    //DataTable dtChild = TreeViewBAL.BL.TreeViewBL.GetData("Select ID,Name from Module where ProjID=" + child.Value);
                    if (dschild.Tables[0].Rows.Count > 0)
                    {
                        PopulateTreeView(dschild, child, child.Value, 1);
                    }

                }
                else if (Level == 1)
                {
                    treeNode.ChildNodes.Add(child);
                    DataSet dssubchild = getdt.GetSubTasksForWorkPackages(child.Value);
                    if (dssubchild.Tables[0].Rows.Count > 0)
                    {
                        PopulateTreeView(dssubchild, child, child.Value, 2);
                    }
                }
                else if (Level == 2)
                {
                    treeNode.ChildNodes.Add(child);
                    DataSet dssubtosubchild = getdt.GetSubtoSubTasksForWorkPackages(child.Value);
                    if (dssubtosubchild.Tables[0].Rows.Count > 0)
                    {
                        PopulateTreeView(dssubtosubchild, child, child.Value, 3);
                    }
                }
                else if (Level == 3)
                {
                    treeNode.ChildNodes.Add(child);
                    DataSet lastchild = getdt.GetSubtoSubtoSubTasksForWorkPackages(child.Value);
                    if (lastchild.Tables[0].Rows.Count > 0)
                    {
                        PopulateTreeView(lastchild, child, child.Value, 4);
                    }
                }
                else if (Level == 4)
                {
                    treeNode.ChildNodes.Add(child);
                    DataSet lastchild = getdt.GetSubtoSubtoSubtoSubTasksForWorkPackages(child.Value);
                    if (lastchild.Tables[0].Rows.Count > 0)
                    {
                        PopulateTreeView(lastchild, child, child.Value, 5);
                    }
                }
                else
                {
                    treeNode.ChildNodes.Add(child);
                    DataSet ds = getdt.GetTask_by_ParentTaskUID(new Guid(child.Value));
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        PopulateTreeView(ds, child, child.Value, 6);
                    }
                }
            }

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

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (TreeView1.SelectedNode.Value != null)
            {
                if (TreeView1.Target == "WorkPackage")
                {
                    Session["ActivityUID"] = "WkPkg*" + TreeView1.SelectedNode.Value;
                }
                else
                {
                    Session["ActivityUID"] = "TskNm*" + TreeView1.SelectedNode.Value;
                }
                
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Please choose an activity to link')</script>");
            }
        }

        TreeNode SearchNode(TreeNode node, string searchValue)
        {
            if (node.Value == searchValue) return node;

            TreeNode tn = null;
            foreach (TreeNode childNode in node.ChildNodes)
            {
                tn = SearchNode(childNode, searchValue);
                if (tn != null) break;
            }

            if (tn != null) node.Expand();
            return tn;
        }
    }
}