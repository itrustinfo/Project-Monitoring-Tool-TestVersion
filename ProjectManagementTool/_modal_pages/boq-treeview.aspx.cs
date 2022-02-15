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
    public partial class boq_treeview : System.Web.UI.Page
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
                    Session["BOQData"] = null;
                    //TreeView1.Nodes[0].SelectAction = TreeNodeSelectAction.None;
                    if (Request.QueryString["ProjectUID"] != null)
                    {
                        BindTreeview();
                        TreeView1.Nodes[0].SelectAction = TreeNodeSelectAction.None;
                    }
                    
                }
            }
        }

        public void BindTreeview()
        {
            TreeView1.Nodes.Clear();

            DataTable dt = getdt.getBOQParent_Details(new Guid(Request.QueryString["ProjectUID"]), "Project");
            if (dt.Rows.Count > 0)
            {
                PopulateTreeView(dt, null, "", 0);
                //TreeView1.Nodes[0].Selected = true;
                TreeView1.CollapseAll();
                TreeView1.Nodes[0].Expand();
            }

        }

        public void PopulateTreeView(DataTable dtParent, TreeNode treeNode, string ParentUID, int Level)
        {
            foreach (DataRow row in dtParent.Rows)
            {

                TreeNode child = new TreeNode
                {
                    Text = row["Item_number"].ToString(),
                    Value =  row["BOQDetailsUID"].ToString(),
                    Target = "Item_number",
                    ToolTip = row["Description"].ToString(),
                };

                if (ParentUID == "")
                {
                    TreeView1.Nodes.Add(child);
                    DataTable ds = getdt.getBoq_Details(new Guid(child.Value));
                    if (ds.Rows.Count > 0)
                    {
                        PopulateTreeView(ds, child, child.Value, 1);
                    }
                }
                else
                {
                    treeNode.ChildNodes.Add(child);
                    DataTable ds = getdt.getBoq_Details(new Guid(child.Value));
                    if (ds.Rows.Count > 0)
                    {
                        PopulateTreeView(ds, child, child.Value, 1);
                    }
                }
                
            }

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (TreeView1.SelectedNode != null)
            {
                Session["BOQData"] = TreeView1.SelectedNode.Value;
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Please choose an BOQ data to link')</script>");
            }
        }
    }
}