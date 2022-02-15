using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagementTool._content_pages.measurement_book
{
    public partial class _default : System.Web.UI.Page
    {
        DataSet ds = new DataSet();
        DBGetData getdata = new DBGetData();
        TaskUpdate TKUpdate = new TaskUpdate();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Username"] == null)
            {
                Response.Redirect("~/Login.aspx");
            }
            if (!IsPostBack)
            {
                BindProject();
                DDlProject_SelectedIndexChanged(sender, e);
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

        }

        public void PopulateTreeView(DataSet dtParent, TreeNode treeNode, string ParentUID, int Level)
        {
            foreach (DataRow row in dtParent.Tables[0].Rows)
            {
                TreeNode child = new TreeNode
                {
                    Text = LimitCharts(row["Name"].ToString()),
                    Value = row["TaskUID"].ToString(),
                    Target = "Tasks",
                    ToolTip = row["Name"].ToString()
                };

                //if (ParentUID == "")
                //{
                //    //treeNode.ChildNodes.Add(child);
                //    TreeView1.Nodes.Add(child);
                //    DataSet dschild = getdata.GetTasksForWorkPackages(child.Value);
                //    //DataTable dtChild = TreeViewBAL.BL.TreeViewBL.GetData("Select ID,Name from Module where ProjID=" + child.Value);
                //    if (dschild.Tables[0].Rows.Count > 0)
                //    {
                //        PopulateTreeView(dschild, child, child.Value, 1);
                //    }

                //}
                if (ParentUID == "")
                {
                    TreeView1.Nodes.Add(child);
                    DataSet dssubchild = getdata.GetSubTasksForWorkPackages(child.Value);
                    if (dssubchild.Tables[0].Rows.Count > 0)
                    {
                        PopulateTreeView(dssubchild, child, child.Value, 1);
                    }
                }
                else if (Level == 1)
                {
                    treeNode.ChildNodes.Add(child);
                    DataSet dssubtosubchild = getdata.GetSubtoSubTasksForWorkPackages(child.Value);
                    if (dssubtosubchild.Tables[0].Rows.Count > 0)
                    {
                        PopulateTreeView(dssubtosubchild, child, child.Value, 2);
                    }
                }
                else if (Level == 2)
                {
                    treeNode.ChildNodes.Add(child);
                    DataSet lastchild = getdata.GetSubtoSubtoSubTasksForWorkPackages(child.Value);
                    if (lastchild.Tables[0].Rows.Count > 0)
                    {
                        PopulateTreeView(lastchild, child, child.Value, 3);
                    }
                }
                else if (Level == 3)
                {
                    treeNode.ChildNodes.Add(child);
                    DataSet lastchild = getdata.GetSubtoSubtoSubtoSubTasksForWorkPackages(child.Value);
                    if (lastchild.Tables[0].Rows.Count > 0)
                    {
                        PopulateTreeView(lastchild, child, child.Value, 4);
                    }
                }
                else
                {
                    treeNode.ChildNodes.Add(child);
                    DataSet ds = getdata.GetTask_by_ParentTaskUID(new Guid(child.Value));
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        PopulateTreeView(ds, child, child.Value, 5);
                    }
                }
            }

        }

        public string LimitCharts(string Desc)
        {
            if (Desc.Length > 80)
            {
                return Desc.Substring(0, 80) + "  . . .";
            }
            else
            {
                return Desc;
            }
        }
        public string GetTaskName(string TaskUID)
        {
            return getdata.getTaskNameby_TaskUID(new Guid(TaskUID));
        }

        private void BindMeasurementBook(string TaskUID)
        {

            try
            {
                ActivityHeading.Text = "Task : " + TreeView1.SelectedNode.Text;
                DataSet ds = getdata.GetTaskMeasurementBook(new Guid(TaskUID));
                grdMeasurementbook.DataSource = ds;
                grdMeasurementbook.DataBind();
            }
            catch (Exception ex)
            {

            }
            
        }

        protected void DDlProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDlProject.SelectedValue != "")
            {
                //DataSet ds = getdata.GetWorkPackages_By_ProjectUID(new Guid(DDlProject.SelectedValue));
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
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DDLWorkPackage.DataTextField = "Name";
                    DDLWorkPackage.DataValueField = "WorkPackageUID";
                    DDLWorkPackage.DataSource = ds;
                    DDLWorkPackage.DataBind();

                    DataSet dschild = getdata.GetTasksForWorkPackages(DDLWorkPackage.SelectedValue);
                    if (dschild.Tables[0].Rows.Count > 0)
                    {
                        TreeView1.Nodes.Clear();
                        PopulateTreeView(dschild, null, "", 0);
                        TreeView1.Nodes[0].Selected = true;
                        TreeView1.CollapseAll();
                        TreeView1.Nodes[0].Expand();

                        BindMeasurementBook(TreeView1.SelectedNode.Value);
                    }
                    //Loadtasks();
                    //DDLTask_SelectedIndexChanged(sender, e);
                }
            }
            
        }

        protected void DDLWorkPackage_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataSet dschild = getdata.GetTasksForWorkPackages(DDLWorkPackage.SelectedValue);
            if (dschild.Tables[0].Rows.Count > 0)
            {
                TreeView1.Nodes.Clear();
                PopulateTreeView(dschild, null, "", 0);
                TreeView1.Nodes[0].Selected = true;
                TreeView1.CollapseAll();
                TreeView1.Nodes[0].Expand();

                BindMeasurementBook(TreeView1.SelectedNode.Value);
            }
        }

        protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
        {
            BindMeasurementBook(TreeView1.SelectedNode.Value);
        }

        protected void grdMeasurementbook_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string UID = e.CommandArgument.ToString();
            if (e.CommandName == "download")
            {
                DataSet ds = getdata.GetMeasurementBook_By_UID(new Guid(UID));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string path = Server.MapPath(ds.Tables[0].Rows[0]["Upload_File"].ToString());
                    //   File.Decrypt(path);
                    string getExtension = System.IO.Path.GetExtension(path);
                    string sFileName = Path.GetFileName(path);
                    //string outPath = path.Replace(getExtension, "") + "_download" + getExtension;
                    string outPath = "/_PreviewLoad/" + sFileName;
                    getdata.DecryptFile(path, Server.MapPath(outPath));
                    System.IO.FileInfo file = new System.IO.FileInfo(Server.MapPath(outPath));
                    if (file.Exists)
                    {
                        Response.Clear();

                        Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);

                        Response.AddHeader("Content-Length", file.Length.ToString());

                        Response.ContentType = "application/octet-stream";

                        Response.WriteFile(file.FullName);

                        Response.End();
                    }
                    else
                    {

                        //Response.Write("This file does not exist.");
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('File does not exist.');</script>");

                    }
                }
            }
        }

        protected void grdMeasurementbook_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[5].Text == "&nbsp;" || e.Row.Cells[5].Text == "")
                {
                    LinkButton lnk = (LinkButton)e.Row.FindControl("lnkDownload");
                    lnk.Enabled = false;
                    lnk.Text = "No File";
                }
            }
        }
    }
}