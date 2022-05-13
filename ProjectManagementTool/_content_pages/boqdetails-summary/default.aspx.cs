using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagementTool._content_pages.boqdetails_summary
{
    public partial class _default : System.Web.UI.Page
    {
        DBGetData dbgetdata = new DBGetData();
        TaskUpdate gettk = new TaskUpdate();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Session["Username"] == null)
                {
                    Response.Redirect("~/Login.aspx");
                }
                else
                {
                    if (!IsPostBack)
                    {
                        HideActionButtons();
                        
                        BindProject();
                        DDlProject_SelectedIndexChanged(sender, e);
                    }
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
            string RetUID = string.Empty;
            if (Session["BOQSelectedActivity"] != null)
            {
                RetUID = dbgetdata.GetProjectUIDFromBOQUID(new Guid(Session["BOQSelectedActivity"].ToString()));
                if (RetUID != "")
                {
                    DDlProject.SelectedValue = new Guid(RetUID).ToString();
                }

            }

            if (DDlProject.SelectedValue != "")
            {
                DataSet ds = new DataSet();
                //ds = getdt.GetWorkPackages_By_ProjectUID(new Guid(DDlProject.SelectedValue));
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
                    DDLWorkPackage.DataTextField = "Name";
                    DDLWorkPackage.DataValueField = "WorkPackageUID";
                    DDLWorkPackage.DataSource = ds;
                    DDLWorkPackage.DataBind();

                    if (Session["BOQSelectedActivity"] != null)
                    {
                        RetUID = dbgetdata.GetWorkpackageUIDFromBOQUID(new Guid(Session["BOQSelectedActivity"].ToString()));
                        if (RetUID != "")
                        {
                            DDLWorkPackage.SelectedValue = new Guid(RetUID).ToString();
                        }

                    }
                    DDLWorkPackage_SelectedIndexChanged(sender, e);
                }
            }
        }

        protected void DDLWorkPackage_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDLWorkPackage.SelectedValue != "")
            {
                BindTreeview();
            }
        }
        protected void HideActionButtons()
        {
            DataSet dscheck = new DataSet();
            dscheck = dbgetdata.GetUsertypeFunctionality_Mapping(Session["TypeOfUser"].ToString());
            ViewState["isEdit"] = "false";
            ViewState["isDelete"] = "false";
            AddDependency.Visible = false;
            GrdTreeView.Columns[7].Visible = false;
            GrdTreeView.Columns[8].Visible = false;
            if (dscheck.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dscheck.Tables[0].Rows)
                {
                    if (dr["Code"].ToString() == "BOQA")
                    {
                        AddDependency.Visible = true;
                    }
                    if (dr["Code"].ToString() == "BOQE")
                    {
                        ViewState["isEdit"] = "true";
                        GrdTreeView.Columns[7].Visible = true;
                    }
                    if (dr["Code"].ToString() == "BOQD")
                    {
                        ViewState["isDelete"] = "true";
                        GrdTreeView.Columns[8].Visible = true;
                    }
                }
            }
        }
        protected void retrieveNodes(TreeNode node, string SelectedVal)
        {
            if (node.Value == SelectedVal)
            {
                node.Selected = true;
                node.Expand();
                BindTreeview();
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
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            //BindActivities();
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

        public void BindTreeview()
        {
            TreeView1.Nodes.Clear();
            
            
            DataTable ds = new DataTable();
            //if (Session["TypeOfUser"].ToString() == "U" || Session["TypeOfUser"].ToString() == "MD" || Session["TypeOfUser"].ToString() == "VP")
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
            ds = dbgetdata.getBOQParent_Details(new Guid(DDLWorkPackage.SelectedValue), "Workpackage");
            if (ds.Rows.Count > 0)
            {
                PopulateTreeView(ds, null, "", 3);
                if (Session["BOQSelectedActivity"] != null)
                {
                    TreeView1.CollapseAll();
                    string UID = Session["BOQSelectedActivity"].ToString();
                    for (int i = 0; i < TreeView1.Nodes.Count; i++)
                    {
                        retrieveNodes(TreeView1.Nodes[i], UID);
                       
                    }
                   // Session["BOQSelectedActivity"] = null;
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

                // BindActivities();
               
                if (TreeView1.SelectedNode != null)
                {
                    lblBOQName.Text = "Item Description : " + TreeView1.SelectedNode.ToolTip;
                    BindDataforProject();
                }
               
            }
            else
            {
                GrdTreeView.DataSource = null;
                GrdTreeView.DataBind();
                lblBOQName.Text = "";
            }

        }
        //public void BindTreeview()
        //{
        //    try
        //    {

        //        DataSet ds = new DataSet();
        //        if (Session["TypeOfUser"].ToString() == "U" || Session["TypeOfUser"].ToString() == "MD" || Session["TypeOfUser"].ToString() == "VP")
        //        {
        //            //ds = gettk.GetAllProjects();
        //            ds = dbgetdata.ProjectClass_Select_All();
        //        }
        //        else if (Session["TypeOfUser"].ToString() == "PA")
        //        {
        //            //ds = dbgetdata.GetAssignedProjects_by_UserUID(new Guid(Session["UserUID"].ToString()));
        //            ds = dbgetdata.ProjectClass_Select_By_UserUID(new Guid(Session["UserUID"].ToString()));
        //        }
        //        else
        //        {
        //            //ds = dbgetdata.GetAssignedProjects_by_UserUID(new Guid(Session["UserUID"].ToString()));
        //            ds = dbgetdata.ProjectClass_Select_By_UserUID(new Guid(Session["UserUID"].ToString()));
        //        }
        //        if (ds.Tables[0].Rows.Count > 0)
        //        {
        //            PopulateTreeView(ds.Tables[0], null, "", 0);
        //        }

        //        // DataTable ds = dbgetdata.getBOQParent_Details();
        //        //  PopulateTreeView(ds, null, "");
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

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
        public void PopulateTreeView(DataTable dtParent, TreeNode treeNode, string ParentUID, int Level)
        {
            foreach (DataRow row in dtParent.Rows)
            {

                TreeNode child = new TreeNode
                {

                    Text = Level == 0 ? row["ProjectClass_Name"].ToString() : Level == 1 ? LimitCharts(dbgetdata.getProjectNameby_ProjectUID(new Guid(row["ProjectUID"].ToString()))) : row["Item_number"].ToString(),
                    Value = Level == 0 ? row["ProjectClass_UID"].ToString() : Level == 1 ? row["ProjectUID"].ToString() : row["BOQDetailsUID"].ToString(),
                    Target = Level == 0 ? "Class" : Level == 1 ? "Project" : "Item_number",
                    ToolTip = Level == 0 ? row["ProjectClass_Name"].ToString() : Level == 1 ? dbgetdata.getProjectNameby_ProjectUID(new Guid(row["ProjectUID"].ToString())) : row["Description"].ToString(),

                    //Text = row["Item_number"].ToString(),
                    //    Value = row["BOQDetailsUID"].ToString(),
                    //    Target = row["Item_number"].ToString(),
                    //    ToolTip = row["Description"].ToString(),
                };

                //if (ParentUID == "")
                //{
                //    TreeView1.Nodes.Add(child);
                //    DataSet dsProject = new DataSet();
                //    if (Session["TypeOfUser"].ToString() == "U" || Session["TypeOfUser"].ToString() == "MD" || Session["TypeOfUser"].ToString() == "VP")
                //    {
                //        dsProject = gettk.GetProjects_by_ClassUID(new Guid(child.Value));
                //    }
                //    else
                //    {
                //        dsProject = gettk.GetUserProjects_by_ClassUID(new Guid(child.Value), new Guid(Session["UserUID"].ToString()));
                //    }
                //    if (dsProject.Tables[0].Rows.Count > 0)
                //    {
                //        PopulateTreeView(dsProject.Tables[0], child, child.Value, 1);
                //    }
                //}
                //else 
                if (ParentUID == "")
                {
                    TreeView1.Nodes.Add(child);
                    DataTable ds = dbgetdata.getBoq_Details(new Guid(child.Value));
                    if (ds.Rows.Count > 0)
                    {
                        PopulateTreeView(ds, child, child.Value, 3);
                    }
                  
                }
                else
                {
                    treeNode.ChildNodes.Add(child);
                    DataTable ds = dbgetdata.getBoq_Details(new Guid(child.Value));
                    if (ds.Rows.Count > 0)
                    {
                        PopulateTreeView(ds, child, child.Value, 3);
                    }
                   
                }
                //if (Level == 1)
                //{
                //    //treeNode.ChildNodes.Add(child);
                //    TreeView1.Nodes.Add(child);
                //    DataTable ds = dbgetdata.getBOQParent_Details(new Guid(child.Value), "Project");
                //    if (ds.Rows.Count > 0)
                //    {
                //        PopulateTreeView(ds, child, child.Value, 2);
                //    }
                //    TreeView1.CollapseAll();
                //    TreeView1.Nodes[0].Expand();
                //}
                //else
                //{
                //    //treeNode.ChildNodes.Add(child);
                //    TreeView1.Nodes.Add(child);
                //    DataTable ds = dbgetdata.getBoq_Details(new Guid(child.Value));
                //    if (ds.Rows.Count > 0)
                //    {
                //        PopulateTreeView(ds, child, child.Value, 3);
                //    }
                //    TreeView1.CollapseAll();
                //    TreeView1.Nodes[0].Expand();
                //}


            }

        }

        protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
        {
            if (TreeView1.SelectedNode != null)
            {
                Session["BOQSelectedActivity"] = TreeView1.SelectedNode.Value;
                if (TreeView1.SelectedNode.Target == "Project")
                {
                    lblBOQName.Text = "Project Name:" + TreeView1.SelectedNode.Text;

                }
                else
                {
                    lblBOQName.Text = "Item Description : " + TreeView1.SelectedNode.ToolTip;
                }
                BindDataforProject();
            }
        }

        protected void GrdProject_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string UID = e.CommandArgument.ToString();
            if (e.CommandName == "delete")
            {
                int Cnt = dbgetdata.BOQ_Delete(new Guid(UID), new Guid(Session["UserUID"].ToString()));
                if (Cnt > 0)
                {
                    BindDataforProject();
                }
            }
        }

        private void BindDataforProject()
        {
            try
            {
                AddDependency.HRef = " /_modal_pages/add-boqdetails.aspx?projectuid=" + DDlProject.SelectedValue + "&WorkpackageUID=" + DDLWorkPackage.SelectedValue + "&parameterType=" + TreeView1.SelectedNode.Target + "&ParentUID=" + TreeView1.SelectedNode.Value;
                DataTable dtBOQSummary = dbgetdata.getBOQParent_Details(new Guid(TreeView1.SelectedNode.Value), TreeView1.SelectedNode.Target);
                GrdTreeView.DataSource = dtBOQSummary;
                GrdTreeView.DataBind();
                if(dtBOQSummary.Rows.Count == 0)
                {
                    DataSet ds = dbgetdata.GetBOQDetails_by_BOQDetailsUID(new Guid(TreeView1.SelectedNode.Value));

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        lblBOQDetails.Visible = true;
                        
                        lblBOQDetails.Text = " Approved Quantity : " + ds.Tables[0].Rows[0]["Quantity"] + " , Unit : " + ds.Tables[0].Rows[0]["Unit"] + " , Approved Rate : INR " + ds.Tables[0].Rows[0]["INR-Rate"] + " , Amount : INR  " +  ds.Tables[0].Rows[0]["INR-Amount"];
                    }
                }
                else
                {
                    lblBOQDetails.Visible = false;
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void GrdTreeView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (TreeView1.SelectedNode.Target == "Project")
                {
                    e.Row.Cells[2].Visible = false;
                    e.Row.Cells[3].Visible = false;
                    e.Row.Cells[4].Visible = false;
                    e.Row.Cells[5].Visible = false;
                    e.Row.Cells[6].Visible = false;
                }

                if (ViewState["isEdit"].ToString() == "false")
                {
                    e.Row.Cells[7].Visible = false;
                }
                if (ViewState["isDelete"].ToString() == "false")
                {
                    e.Row.Cells[8].Visible = false;
                }
            }
                if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (TreeView1.SelectedNode.Target == "Project")
                {
                    e.Row.Cells[2].Visible = false;
                    e.Row.Cells[3].Visible = false;
                    e.Row.Cells[4].Visible = false;
                    e.Row.Cells[5].Visible = false;
                    e.Row.Cells[6].Visible = false;

                }

                if (ViewState["isEdit"].ToString() == "false")
                {
                    e.Row.Cells[7].Visible = false;
                }
                if (ViewState["isDelete"].ToString() == "false")
                {
                    e.Row.Cells[8].Visible = false;
                }
            }
        }

        protected void GrdTreeView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdTreeView.PageIndex = e.NewPageIndex;
            BindDataforProject();
        }

        

        protected void GrdTreeView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string UID = e.CommandArgument.ToString();
            if (e.CommandName == "delete")
            {
                int Cnt = dbgetdata.BOQ_Delete(new Guid(UID), new Guid(Session["UserUID"].ToString()));
                if (Cnt > 0)
                {
                    BindDataforProject();
                }
            }
        }

        protected void GrdTreeView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }
    }

}