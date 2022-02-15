using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace ProjectManagementTool._content_pages.categorywise_documents
{
    public partial class _default : System.Web.UI.Page
    {
        DBGetData getdata = new DBGetData();
        TaskUpdate gettk = new TaskUpdate();
        GeneralDocuments GD = new GeneralDocuments();
        string folder = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Username"] == null)
            {
                Response.Redirect("~/Login.aspx");
            }
            else
            {
                ScriptManager.RegisterStartupScript(
                        UpdatePanel1,
                        this.GetType(),
                        "MyAction",
                        "BindEvents();",
                        true);
                if (!IsPostBack)
                {
                    BindProject();
                    DDlProject_SelectedIndexChanged(sender, e);
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
                DataSet ds = new DataSet();
                ds = getdata.GetWorkPackages_By_ProjectUID(new Guid(DDlProject.SelectedValue));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DDLWorkPackage.DataTextField = "Name";
                    DDLWorkPackage.DataValueField = "WorkPackageUID";
                    DDLWorkPackage.DataSource = ds;
                    DDLWorkPackage.DataBind();
                    BindTreeview_by_Category(DDLWorkPackage.SelectedValue);
                    BindDocuments_By_Category(TreeView2.SelectedNode.Value);
                }
                else
                {
                    DDLWorkPackage.DataSource = null;
                    DDLWorkPackage.DataBind();
                }
            }
        }

        protected void DDLWorkPackage_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDLWorkPackage.SelectedValue != "")
            {
                BindTreeview_by_Category(DDLWorkPackage.SelectedValue);

                BindDocuments_By_Category(TreeView2.SelectedNode.Value);
            }
        }

        public void BindTreeview_by_Category(string WorkpackageUID)
        {
            TreeView2.Nodes.Clear();
            DataSet ds = new DataSet();
            ds=getdata.WorkPackageCategory_Selectby_WorkPackageUID(new Guid(WorkpackageUID));
            if (ds.Tables[0].Rows.Count > 0)
            {
                PopulateTreeView_by_Category(ds, null, "", 0);
                if (Session["SelectedTaskUID"] != null)
                {
                    TreeView2.CollapseAll();
                    string UID = Session["SelectedTaskUID"].ToString();
                    for (int i = 0; i < TreeView2.Nodes.Count; i++)
                    {
                        retrieveNodesCategory(TreeView2.Nodes[i], UID);
                    }
                    Session["SelectedTaskUID"] = null;
                    Session["ViewDocBy"] = null;
                }
                else if (Session["SelectedTaskUID1"] != null)
                {
                    TreeView2.CollapseAll();
                    string UID = Session["SelectedTaskUID1"].ToString();
                    for (int i = 0; i < TreeView2.Nodes.Count; i++)
                    {
                        retrieveNodesCategory(TreeView2.Nodes[i], UID);
                    }
                    Session["SelectedTaskUID1"] = null;
                    Session["ViewDocBy"] = null;
                }
                else
                {
                    TreeView2.Nodes[0].Selected = true;
                    TreeView2.CollapseAll();
                    TreeView2.Nodes[0].Expand();
                }
            }
        }

        public void PopulateTreeView_by_Category(DataSet dtParent, TreeNode treeNode, string ParentUID, int Level)
        {
            foreach (DataRow row in dtParent.Tables[0].Rows)
            {
                TreeNode child = new TreeNode
                {
                    Text = LimitCharts(row["WorkPackageCategory_Name"].ToString()),
                    Value = row["WorkPackageCategory_UID"].ToString(),
                    Target = "Category",
                    ToolTip = row["WorkPackageCategory_Name"].ToString()
                };

                if (ParentUID == "")
                {
                    TreeView2.Nodes.Add(child);
                    DataSet submittal = getdata.GetSubmittals_For_Category(row["WorkPackageCategory_UID"].ToString());
                    if (submittal.Tables[0].Rows.Count > 0)
                    {
                        for (int j = 0; j < submittal.Tables[0].Rows.Count; j++)
                        {
                            TreeNode subdoc = new TreeNode
                            {
                                Text = "<div style='color:#007bff;'>" + submittal.Tables[0].Rows[j]["DocName"].ToString() + "</div>",
                                Value = submittal.Tables[0].Rows[j]["DocumentUID"].ToString(),
                                Target = "Submittal",
                                ToolTip = submittal.Tables[0].Rows[j]["DocName"].ToString(),
                                ImageUrl = "~/_assets/images/submittal.png"
                            };
                            child.ChildNodes.Add(subdoc);
                            //string Dir_Name = "";
                            //int SplitNodeCount = 0;
                            //DataSet documents = getdata.ActualDocuments_SelectBy_DocumentUID(new Guid(submittal.Tables[0].Rows[j]["DocumentUID"].ToString()));
                            //for (int k = 0; k < documents.Tables[0].Rows.Count; k++)
                            //{
                            //    if (documents.Tables[0].Rows[k]["ActualDocument_RelativePath"].ToString() != "")
                            //    {
                            //        SplitNodeCount = 0;
                            //        string[] relativepath = documents.Tables[0].Rows[k]["ActualDocument_RelativePath"].ToString().Split('/');
                            //        if (relativepath.Length > 0)
                            //        {
                            //            List<string> list = new List<string>(relativepath);
                            //            TreeNode SubFolder = new TreeNode();
                            //            TreeNode SubFolder1 = new TreeNode();
                            //            TreeNode SubFolder2 = new TreeNode();
                            //            TreeNode SubFolder3 = new TreeNode();
                            //            TreeNode SubFolder4 = new TreeNode();
                            //            TreeNode SubFolder5 = new TreeNode();
                            //            TreeNode SubFolder6 = new TreeNode();
                            //            TreeNode SubFolder7 = new TreeNode();
                            //            if (SplitNodeCount < list.Count)
                            //            {

                            //                TreeNode trnode = SearchNode(subdoc, list[SplitNodeCount].ToString());
                            //                if (trnode != null)
                            //                {
                            //                    SubFolder = trnode;
                            //                }
                            //                else
                            //                {
                            //                    SubFolder = new TreeNode();
                            //                    SubFolder.Text = list[SplitNodeCount].ToString();
                            //                    SubFolder.Target = "Folder";
                            //                    SubFolder.Value = documents.Tables[0].Rows[k]["ActualDocumentUID"].ToString();
                            //                    SubFolder.ToolTip = documents.Tables[0].Rows[k]["ActualDocument_RelativePath"].ToString();// list[SplitNodeCount].ToString();

                            //                    subdoc.ChildNodes.Add(SubFolder);
                            //                }

                            //            }
                            //            SplitNodeCount = SplitNodeCount + 1;

                            //            if (SplitNodeCount < list.Count)
                            //            {


                            //                TreeNode trnode1 = SearchNode(SubFolder, list[SplitNodeCount].ToString());
                            //                if (trnode1 != null)
                            //                {
                            //                    SubFolder1 = trnode1;
                            //                }
                            //                else
                            //                {
                            //                    SubFolder1 = new TreeNode();
                            //                    SubFolder1.Text = list[SplitNodeCount].ToString();
                            //                    SubFolder1.Target = "SubFolder";
                            //                    SubFolder1.Value = documents.Tables[0].Rows[k]["ActualDocumentUID"].ToString();
                            //                    SubFolder1.ToolTip = documents.Tables[0].Rows[k]["ActualDocument_RelativePath"].ToString();// list[SplitNodeCount].ToString();
                            //                    SubFolder.ChildNodes.Add(SubFolder1);
                            //                }
                            //            }
                            //            SplitNodeCount = SplitNodeCount + 1;

                            //            if (SplitNodeCount < list.Count)
                            //            {

                            //                TreeNode trnode2 = SearchNode(SubFolder1, list[SplitNodeCount].ToString());
                            //                if (trnode2 != null)
                            //                {
                            //                    SubFolder2 = trnode2;
                            //                }
                            //                else
                            //                {
                            //                    SubFolder2 = new TreeNode();
                            //                    SubFolder2.Text = list[SplitNodeCount].ToString();
                            //                    SubFolder2.Target = "SubFolder";
                            //                    SubFolder2.Value = documents.Tables[0].Rows[k]["ActualDocumentUID"].ToString();
                            //                    SubFolder2.ToolTip = documents.Tables[0].Rows[k]["ActualDocument_RelativePath"].ToString();// list[SplitNodeCount].ToString();
                            //                    SubFolder1.ChildNodes.Add(SubFolder2);
                            //                }


                            //            }
                            //            SplitNodeCount = SplitNodeCount + 1;

                            //            if (SplitNodeCount < list.Count)
                            //            {

                            //                TreeNode trnode3 = SearchNode(SubFolder2, list[SplitNodeCount].ToString());
                            //                if (trnode3 != null)
                            //                {
                            //                    SubFolder3 = trnode3;
                            //                }
                            //                else
                            //                {
                            //                    SubFolder3 = new TreeNode();
                            //                    SubFolder3.Text = list[SplitNodeCount].ToString();
                            //                    SubFolder3.Target = "SubFolder";
                            //                    SubFolder3.Value = documents.Tables[0].Rows[k]["ActualDocumentUID"].ToString();
                            //                    SubFolder3.ToolTip = documents.Tables[0].Rows[k]["ActualDocument_RelativePath"].ToString();// list[SplitNodeCount].ToString();
                            //                    SubFolder2.ChildNodes.Add(SubFolder3);
                            //                }


                            //            }
                            //            SplitNodeCount = SplitNodeCount + 1;

                            //            if (SplitNodeCount < list.Count)
                            //            {

                            //                TreeNode trnode4 = SearchNode(SubFolder3, list[SplitNodeCount].ToString());
                            //                if (trnode4 != null)
                            //                {
                            //                    SubFolder4 = trnode4;
                            //                }
                            //                else
                            //                {
                            //                    SubFolder4 = new TreeNode();
                            //                    SubFolder4.Text = list[SplitNodeCount].ToString();
                            //                    SubFolder4.Target = "SubFolder";
                            //                    SubFolder4.Value = documents.Tables[0].Rows[k]["ActualDocumentUID"].ToString();
                            //                    SubFolder4.ToolTip = documents.Tables[0].Rows[k]["ActualDocument_RelativePath"].ToString();// list[SplitNodeCount].ToString();
                            //                    SubFolder3.ChildNodes.Add(SubFolder4);
                            //                }


                            //            }
                            //            SplitNodeCount = SplitNodeCount + 1;

                            //            if (SplitNodeCount < list.Count)
                            //            {

                            //                TreeNode trnode = SearchNode(SubFolder4, list[SplitNodeCount].ToString());
                            //                if (trnode != null)
                            //                {
                            //                    SubFolder5 = trnode;
                            //                }
                            //                else
                            //                {
                            //                    SubFolder5 = new TreeNode();
                            //                    SubFolder5.Text = list[SplitNodeCount].ToString();
                            //                    SubFolder5.Target = "SubFolder";
                            //                    SubFolder5.Value = documents.Tables[0].Rows[k]["ActualDocumentUID"].ToString();
                            //                    SubFolder5.ToolTip = documents.Tables[0].Rows[k]["ActualDocument_RelativePath"].ToString();// list[SplitNodeCount].ToString();
                            //                    SubFolder4.ChildNodes.Add(SubFolder5);
                            //                }
                            //            }
                            //            SplitNodeCount = SplitNodeCount + 1;

                            //            if (SplitNodeCount < list.Count)
                            //            {

                            //                TreeNode trnode = SearchNode(SubFolder5, list[SplitNodeCount].ToString());
                            //                if (trnode != null)
                            //                {
                            //                    SubFolder6 = trnode;
                            //                }
                            //                else
                            //                {
                            //                    SubFolder6 = new TreeNode();
                            //                    SubFolder6.Text = list[SplitNodeCount].ToString();
                            //                    SubFolder6.Target = "SubFolder";
                            //                    SubFolder6.Value = documents.Tables[0].Rows[k]["ActualDocumentUID"].ToString();
                            //                    SubFolder6.ToolTip = documents.Tables[0].Rows[k]["ActualDocument_RelativePath"].ToString();// list[SplitNodeCount].ToString();
                            //                    SubFolder5.ChildNodes.Add(SubFolder6);
                            //                }
                            //            }

                            //            SplitNodeCount = SplitNodeCount + 1;

                            //            if (SplitNodeCount < list.Count)
                            //            {

                            //                TreeNode trnode = SearchNode(SubFolder6, list[SplitNodeCount].ToString());
                            //                if (trnode != null)
                            //                {
                            //                    SubFolder7 = trnode;
                            //                }
                            //                else
                            //                {
                            //                    SubFolder7 = new TreeNode();
                            //                    SubFolder7.Text = list[SplitNodeCount].ToString();
                            //                    SubFolder7.Target = "SubFolder";
                            //                    SubFolder7.Value = documents.Tables[0].Rows[k]["ActualDocumentUID"].ToString();
                            //                    SubFolder7.ToolTip = documents.Tables[0].Rows[k]["ActualDocument_RelativePath"].ToString();// list[SplitNodeCount].ToString();
                            //                    SubFolder6.ChildNodes.Add(SubFolder7);
                            //                }
                            //            }

                            //        }
                            //    }
                            //}
                        }
                    }
                }
            }

        }

        public void BindDocuments_By_Category(string UID)
        {
            DataSet ds = new DataSet();
            if (TreeView2.SelectedNode.Target == "Class")
            {
                CategoryDocumentGrid.Visible = false;
                CategoryDocumentGridHeading.Visible = false;
                ds = null;
                GrdDocumentByCategory.DataSource = ds;
                GrdDocumentByCategory.DataBind();
                GrdActualDocuments1.Visible = false;
            }
            else if (TreeView2.SelectedNode.Target == "Project")
            {
                ds = getdata.GetDocuments_For_Project(new Guid(UID));
                CategoryDocumentGrid.Visible = false;
                CategoryDocumentGridHeading.Visible = false;
                GrdDocumentByCategory.DataSource = ds;
                GrdDocumentByCategory.DataBind();
                GrdActualDocuments1.Visible = false;
            }
            else if (TreeView2.SelectedNode.Target == "WorkPackage")
            {
                ds = getdata.GetDocuments_For_WorkPackage(new Guid(UID));
                CategoryDocumentGridHeading.Visible = false;
                CategoryDocumentGrid.Visible = false;
                GrdDocumentByCategory.DataSource = ds;
                GrdDocumentByCategory.DataBind();
                GrdDocumentByCategory.Visible = true;
                GrdActualDocuments1.Visible = false;
            }
            else if (TreeView2.SelectedNode.Target == "Category")
            {
                AddSubmittalCategory.Visible = true;
                ds = getdata.GetSubmittals_For_Category(UID);
                CategoryDocumentGrid.Visible = true;
                CategoryDocumentGridHeading.Visible = true;
                ActivityHeadingCategory.Text = "Category : " + TreeView2.SelectedNode.Text;
                AddSubmittalCategory.HRef = "/_modal_pages/add-submittalsforcategory.aspx?type=add&CategoryID=" + TreeView2.SelectedNode.Value + "&ViewDocumentBy=Category";
                GrdDocumentByCategory.DataSource = ds;
                GrdDocumentByCategory.DataBind();
                GrdDocumentByCategory.Visible = true;
                GrdActualDocuments1.Visible = false;
            }
            else if (TreeView2.SelectedNode.Target == "Submittal")
            {
                ds = getdata.getDocumentsbyDocID(new Guid(UID));
                CategoryDocumentGrid.Visible = true;
                CategoryDocumentGridHeading.Visible = true;
                ActivityHeadingCategory.Text = "Submittal : " + TreeView2.SelectedNode.Text;
                AddSubmittalCategory.Visible = false;
                GrdDocumentByCategory.DataSource = ds;
                GrdDocumentByCategory.DataBind();
                GrdDocumentByCategory.Visible = true;
                GrdActualDocuments1.Visible = false;
            }
            else if (TreeView2.SelectedNode.Target == "Folder" || TreeView2.SelectedNode.Target == "SubFolder")
            {
                ViewState["folder"] = TreeView2.SelectedNode.Text;
                string CheckFile = TreeView2.SelectedNode.Text;
                string NodeParentText = TreeView2.SelectedNode.ToolTip;
                if (!string.IsNullOrEmpty(NodeParentText))
                {
                    NodeParentText = NodeParentText.Split('/')[0];
                }
                ds = new DataSet();
                if (CheckFile.EndsWith(".pdf") || CheckFile.EndsWith(".PDF") || CheckFile.EndsWith(".doc") || CheckFile.EndsWith(".docx") || CheckFile.EndsWith(".dwg") || CheckFile.EndsWith(".xlsx") || CheckFile.EndsWith(".xls") || CheckFile.EndsWith(".txt") || CheckFile.EndsWith(".pptx") || CheckFile.EndsWith(".log") || CheckFile.EndsWith(".zip"))
                {
                    ActivityHeadingCategory.Text = "Document : " + TreeView2.SelectedNode.Text;
                    //ds = getdata.ActualDocuments_SelectBy_DocID_FileName(new Guid(UID), TreeView2.SelectedNode.Text.Split('.')[0]);
                    ds = getdata.ActualDocuments_SelectBy_ActualDocumentUID(new Guid(UID));
                }
                else
                {
                    ds = getdata.ActualDocuments_SelectBy_DirectoryName_New(new Guid(UID), NodeParentText);
                    ActivityHeadingCategory.Text = "Folder : " + TreeView2.SelectedNode.Text;
                }
                LblDocumentHeading.Text = "Documents";

                CategoryDocumentGrid.Visible = true;
                CategoryDocumentGridHeading.Visible = true;
                AddSubmittalCategory.Visible = false;
                GrdActualDocuments1.DataSource = ds;
                GrdActualDocuments1.DataBind();
                GrdDocumentByCategory.Visible = false;
                GrdActualDocuments1.Visible = true;
            }
            else if (TreeView2.SelectedNode.Target == "SubFolder")
            {
                LblDocumentHeading.Text = "Documents";
                string CheckFile = TreeView2.SelectedNode.Text;
                string tooltip1 = TreeView2.SelectedNode.ToolTip;
                if (CheckFile.EndsWith(".pdf") || CheckFile.EndsWith(".doc") || CheckFile.EndsWith(".docx") || CheckFile.EndsWith(".dwg") || CheckFile.EndsWith(".xlsx") || CheckFile.EndsWith(".xls") || CheckFile.EndsWith(".txt"))
                {
                    //ds = getdata.ActualDocuments_SelectBy_DocID_FileName(new Guid(UID), TreeView2.SelectedNode.Text.Split('.')[0]);
                    ds = getdata.ActualDocuments_SelectBy_ActualDocumentUID(new Guid(UID));
                }
                else
                {
                    ds = getdata.ActualDocuments_SelectBy_DirectoryName(new Guid(UID), TreeView2.SelectedNode.Text);
                }

                CategoryDocumentGrid.Visible = true;
                CategoryDocumentGridHeading.Visible = true;
                ActivityHeadingCategory.Text = "Folder : " + TreeView2.SelectedNode.Text;
                AddSubmittalCategory.Visible = false;
                GrdActualDocuments1.DataSource = ds;
                GrdActualDocuments1.DataBind();
                GrdDocumentByCategory.Visible = false;
                GrdActualDocuments1.Visible = true;
            }
            else
            {
                ds = null;
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

        protected void retrieveNodesCategory(TreeNode node, string SelectedVal)
        {
            if (node.Value == SelectedVal)
            {
                node.Selected = true;
                node.Expand();
                node.Parent.Expand();
                //BindActivities();
                BindDocuments_By_Category(TreeView2.SelectedNode.Value);
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
                            tn.Parent.Expand();
                            //BindActivities();
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
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            BindDocuments_By_Category(TreeView2.SelectedNode.Value);
                        }
                        else
                        {
                            if (tn.ChildNodes.Count != 0)
                            {
                                retrieveNodesCategory(tn, SelectedVal);
                            }
                        }
                    }
                }
            }
        }

        TreeNode SearchNode(TreeNode node, string searchText)
        {
            if (node.Text == searchText) return node;

            TreeNode tn = null;
            foreach (TreeNode childNode in node.ChildNodes)
            {
                tn = SearchNode(childNode, searchText);
                if (tn != null) break;
            }

            if (tn != null) node.Expand();
            return tn;
        }

        protected void TreeView2_SelectedNodeChanged(object sender, EventArgs e)
        {
            BindDocuments_By_Category(TreeView2.SelectedNode.Value);
        }

        protected void GrdDocumentByCategory_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string UID = e.CommandArgument.ToString();
            if (e.CommandName == "download")
            {
                string path = string.Empty;
                DataSet ds = getdata.getLatest_DocumentVerisonSelect(new Guid(UID));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    path = Server.MapPath(ds.Tables[0].Rows[0]["Doc_FileName"].ToString());
                }
                else
                {
                    DataSet ds1 = getdata.getDocumentsbyDocID(new Guid(UID));
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        if (ds1.Tables[0].Rows[0]["DocPath"].ToString() != "")
                        {
                            path = Server.MapPath(ds1.Tables[0].Rows[0]["DocPath"].ToString());
                        }
                    }
                }
                string getExtension = System.IO.Path.GetExtension(path);
                string outPath = path.Replace(getExtension, "") + "_download" + getExtension;
                getdata.DecryptFile(path, outPath);
                System.IO.FileInfo file = new System.IO.FileInfo(outPath);

                if (file.Exists)
                {

                    Response.Clear();

                    Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);

                    Response.AddHeader("Content-Length", file.Length.ToString());

                    Response.ContentType = "application/octet-stream";

                    Response.WriteFile(file.FullName);

                    Response.Flush();

                    try
                    {
                        if (File.Exists(outPath))
                        {
                            File.Delete(outPath);
                        }
                    }
                    catch (Exception ex)
                    {
                        //throw
                    }

                    Response.End();


                }
                else
                {

                    //Response.Write("This file does not exist.");
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('File does not exists.');</script>");

                }
            }

            if (e.CommandName == "delete")
            {
                int cnt = getdata.Documents_Delete_by_DocID(new Guid(UID), new Guid(Session["UserUID"].ToString()));
                if (cnt > 0)
                {
                    BindDocuments_By_Category(TreeView2.SelectedNode.Value);
                }
            }
        }

        protected void GrdDocumentByCategory_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdDocumentByCategory.PageIndex = e.NewPageIndex;
            BindDocuments_By_Category(TreeView2.SelectedNode.Value);
        }

        protected void GrdDocumentByCategory_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void Show_Hide_DocumentsGridCategory(object sender, EventArgs e)
        {
            Page currentPage = (Page)HttpContext.Current.Handler;
            ImageButton imgShowHide = (sender as ImageButton);
            GridViewRow row = (imgShowHide.NamingContainer as GridViewRow);
            if (imgShowHide.CommandArgument == "Show")
            {
                row.FindControl("pnlDocuments1").Visible = true;
                imgShowHide.CommandArgument = "Hide";
                imgShowHide.ImageUrl = "~/_assets/images/minus.png";
                string orderId = (row.NamingContainer as GridView).DataKeys[row.RowIndex].Value.ToString();
                GridView GrdActualDocumentsCategory = row.FindControl("GrdActualDocumentsCategory") as GridView;
                BindActualDocuments(orderId, GrdActualDocumentsCategory);
                ScriptManager.RegisterStartupScript(currentPage, typeof(string), "Script" + Guid.NewGuid(), "expand2();", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(currentPage, typeof(string), "Script" + Guid.NewGuid(), "expand2();", true);
                row.FindControl("pnlDocuments1").Visible = false;
                imgShowHide.CommandArgument = "Show";
                imgShowHide.ImageUrl = "~/_assets/images/plus.png";
            }
        }

        public void BindActualDocuments(string DocumentID, GridView GrdActualDocumentsCategory)
        {
            try
            {
                DataSet ds = getdata.ActualDocuments_SelectBy_DocumentUID(new Guid(DocumentID));
                GrdActualDocumentsCategory.DataSource = ds;
                GrdActualDocumentsCategory.DataBind();
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        protected void GrdActualDocumentsCategory_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // added on 06/11/2020
            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (ViewState["isDelete"].ToString() == "false")
                {
                    e.Row.Cells[6].Visible = false;
                }
                if (ViewState["isUpload"].ToString() == "false")
                {
                    e.Row.Cells[5].Visible = false;
                }
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string ActualDocumentUID = "";
                if (ViewState["isDelete"].ToString() == "false")
                {
                    e.Row.Cells[6].Visible = false;
                }
                if (ViewState["isUpload"].ToString() == "false")
                {
                    e.Row.Cells[5].Visible = false;
                }
                if (e.Row.Cells[7].Text == "&nbsp;")
                {
                    HtmlImage lnkVoucher = e.Row.FindControl("imgfolder") as HtmlImage;
                    lnkVoucher.Visible = false;
                    LinkButton lnkfolder = (LinkButton)e.Row.FindControl("LinkButton2");
                    lnkfolder.Visible = false;
                    if (e.Row.Cells[2].Text != "&nbsp;")
                    {
                        ActualDocumentUID = e.Row.Cells[2].Text;
                        DataSet ds = getdata.getTop1_DocumentStatusSelect(new Guid(e.Row.Cells[2].Text));
                        if (ds != null)
                        {
                            int delay = 0;
                            Label lblVer = (Label)e.Row.FindControl("LblVersion");
                            if (ds.Tables[0].Rows[0]["DocumentType"].ToString() == "General Document")
                            {

                                lblVer.Text = "[ Ver. 1 ]";
                                e.Row.Cells[2].Text = "Submitted";
                                e.Row.Cells[4].Text = "No History";
                            }
                            else
                            {
                                delay = getdata.GetDelayed_Actual_Documents(new Guid(ActualDocumentUID), ds.Tables[0].Rows[0]["ActivityType"].ToString());
                                e.Row.Cells[2].Text = ds.Tables[0].Rows[0]["ActivityType"].ToString();
                                if (ds.Tables[0].Rows[0]["ActivityType"].ToString() != "" && ds.Tables[0].Rows[0]["TopVersion"].ToString() != "")
                                {
                                    //e.Row.Cells[1].Text = ds.Tables[0].Rows[0]["TopVersion"].ToString();

                                    lblVer.Text = "[ Ver. " + ds.Tables[0].Rows[0]["TopVersion"].ToString() + " ]";
                                }
                                else
                                {
                                    lblVer.Text = "[ Ver. 1 ]";
                                }
                                //
                                if (ViewState["isUpdateStatus"].ToString() == "false")
                                {
                                    e.Row.Cells[4].Text = "";
                                }

                            }
                            //
                            if (ViewState["isView"].ToString() == "false")
                            {
                                Label lblName = (Label)e.Row.FindControl("lblName");
                                e.Row.Cells[0].Text = lblName.Text + " " + lblVer.Text;
                            }
                            if (e.Row.Cells[2].Text == "Code A")
                            {
                                if (ViewState["isDownloadNJSE"].ToString() == "false")
                                {
                                    e.Row.Cells[3].Text = "";
                                }
                            }
                            //
                            if (e.Row.Cells[2].Text == "Client Approved" || e.Row.Cells[2].Text == "Approved" || e.Row.Cells[2].Text == "Client Approve")
                            {
                                //e.Row.Cells[13].BackColor = System.Drawing.Color.Green;
                                e.Row.Cells[2].ForeColor = System.Drawing.Color.Green;
                                e.Row.Cells[2].Font.Bold = true;
                                if (ViewState["isDownloadClient"].ToString() == "false")
                                {
                                    e.Row.Cells[3].Text = "";
                                }
                            }
                            if (delay == 1)
                            {
                                e.Row.Cells[2].BackColor = System.Drawing.Color.Red;
                                e.Row.Cells[2].ForeColor = System.Drawing.Color.White;
                            }
                        }
                    }
                }
                else
                {
                    if (folder != e.Row.Cells[7].Text.Split('/')[0])
                    {
                        Label lblVer = (Label)e.Row.FindControl("LblVersion");
                        lblVer.Visible = false;
                        //  HtmlImage lnkVoucher = e.Row.FindControl("imgpdf") as HtmlImage;
                        // lnkVoucher.Visible = false;
                        HtmlGenericControl htmlDivControl = e.Row.FindControl("divPDF") as HtmlGenericControl;
                        htmlDivControl.Visible = false;
                        LinkButton lnkfolder = (LinkButton)e.Row.FindControl("LinkButton2");
                        lnkfolder.Text = e.Row.Cells[7].Text.Split('/')[0];
                        e.Row.Cells[3].Text = "";
                        // e.Row.Cells[0].Text = "<a href='#' onclick='someFunction()'><img src='/_assets/images/folder.jpg' height='15px' width='20px'>  " + e.Row.Cells[7].Text.Split('/')[0] + "</a>";
                        //e.Row.Cells[0].Text = "<asp:LinkButton ID = \"LinkButton3\" runat = \"server\" CausesValidation = \"False\" CommandName = \"Edit\" Text = \"Edit\" ></ asp:LinkButton >";
                        e.Row.Cells[1].Text = "";
                        e.Row.Cells[2].Text = "";
                        e.Row.Cells[4].Text = "";
                        e.Row.Cells[5].Text = "";
                        e.Row.Cells[6].Text = "";
                        folder = e.Row.Cells[7].Text.Split('/')[0];
                    }
                    else
                    {
                        e.Row.Attributes["style"] = "display:none";
                    }

                }
                
            }
        }

        protected void GrdActualDocumentsCategory_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string UID = e.CommandArgument.ToString();
            if (e.CommandName == "download")
            {
                string path = string.Empty;
                DataSet ds = getdata.getLatest_DocumentVerisonSelect(new Guid(UID));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    path = Server.MapPath(ds.Tables[0].Rows[0]["Doc_FileName"].ToString());
                    //File.Decrypt(path);
                }
                else
                {
                    DataSet ds1 = getdata.ActualDocuments_SelectBy_ActualDocumentUID(new Guid(UID));
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        if (ds1.Tables[0].Rows[0]["ActualDocument_Path"].ToString() != "")
                        {
                            path = Server.MapPath(ds1.Tables[0].Rows[0]["ActualDocument_Path"].ToString());
                        }
                    }
                }
                string getExtension = System.IO.Path.GetExtension(path);
                string outPath = path.Replace(getExtension, "") + "_download" + getExtension;
                getdata.DecryptFile(path, outPath);
                System.IO.FileInfo file = new System.IO.FileInfo(outPath);

                if (file.Exists)
                {

                    int Cnt = getdata.DocumentHistroy_InsertorUpdate(Guid.NewGuid(), new Guid(UID), new Guid(Session["UserUID"].ToString()), "Downloaded", "Documents");
                    if (Cnt <= 0)
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code: DDH-01. there is a problem with updating histroy. Please contact system admin.');</script>");
                    }
                    Response.Clear();

                    Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);

                    Response.AddHeader("Content-Length", file.Length.ToString());

                    Response.ContentType = "application/octet-stream";

                    Response.WriteFile(file.FullName);

                    Response.Flush();

                    try
                    {
                        if (File.Exists(outPath))
                        {
                            File.Delete(outPath);
                        }
                    }
                    catch (Exception ex)
                    {
                        //throw
                    }

                    Response.End();


                }
                else
                {

                    //Response.Write("This file does not exist.");
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('File does not exists.');</script>");

                }
            }

            if (e.CommandName == "delete")
            {
                int cnt = getdata.ActualDocuments_Delete_by_DocID(new Guid(UID), new Guid(Session["UserUID"].ToString()));
                if (cnt > 0)
                {
                    BindDocuments_By_Category(TreeView2.SelectedNode.Value);
                }
            }
            if (e.CommandName == "ViewDoc")
            {
                string FilePath = Server.MapPath(UID);
                if (File.Exists(FilePath))
                {
                    string getExtension = System.IO.Path.GetExtension(FilePath);
                    string outPath = FilePath.Replace(getExtension, "") + "_download" + getExtension;
                    getdata.DecryptFile(FilePath, outPath);
                    //Response.Write("<script>window.open('" + outPath + "','_blank')</script>");
                    WebClient User = new WebClient();
                    Byte[] FileBuffer = User.DownloadData(outPath);
                    if (FileBuffer != null)
                    {
                        Response.ContentType = "application/pdf";
                        Response.AddHeader("content-length", FileBuffer.Length.ToString());
                        Response.BinaryWrite(FileBuffer);
                    }
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('File not found');</script>");
                }

            }
            else if (e.CommandName == "Folder_View") // this is for maintaining the folder structure....
            {
                for (int i = 0; i < TreeView2.Nodes.Count; i++)
                {
                    retrieveNodesCategory(TreeView2.Nodes[i], UID);
                }

                BindDocuments_By_Category(UID);                
            }
        }

        protected void GrdActualDocumentsCategory_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        public string GetDocumentName(string DocumentExtn)
        {
            string retval = getdata.GetDocumentMasterType_by_Extension(DocumentExtn);
            if (retval == null || retval == "")
            {
                if (DocumentExtn.ToLower() == ".jpeg" || DocumentExtn.ToLower() == ".jpg" || DocumentExtn.ToLower() == ".png" || DocumentExtn.ToLower() == ".gif" || DocumentExtn.ToLower() == ".bmp")
                {
                    return "Image";
                }
                else
                {
                    return "N/A";
                }

            }
            else
            {
                return retval;
            }
        }

        public string GetDocumentTypeIcon(string DocumentExtn, string ActualDocumentUID, string dType)
        {
            if (DocumentExtn.ToLower() == ".jpeg" || DocumentExtn.ToLower() == ".jpg" || DocumentExtn.ToLower() == ".png" || DocumentExtn.ToLower() == ".gif" || DocumentExtn.ToLower() == ".bmp")
            {
                string pPath = string.Empty;
                if (dType == "General Document")
                {
                    DataSet ds = GD.GeneralDocuments_SelectBy_GeneralDocumentUID(new Guid(ActualDocumentUID));
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        pPath = ds.Tables[0].Rows[0]["GeneralDocument_Path"].ToString();
                    }
                }
                else
                {
                    DataSet ds = getdata.ActualDocuments_SelectBy_ActualDocumentUID(new Guid(ActualDocumentUID));
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        pPath = ds.Tables[0].Rows[0]["ActualDocument_Path"].ToString().Replace("~", "");
                        //pPath = Server.MapPath("~/2bbfa1ef-b427-4e19-add1-97df91390f97/28a6a63b-2573-40a8-bc89-e396c31ce516/Documents/pdf_icon_1.jpg");
                    }
                }


                return pPath;
            }
            else
            {
                return "../../_assets/images/" + getdata.GetDocumentTypeMasterIcon_by_Extension(DocumentExtn);
            }

        }

        protected void GrdActualDocuments1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string filename = string.Empty;
            DataSet ds1 = null;
            string UID = e.CommandArgument.ToString();
            // e.CommandArgument.ToString();
            if (e.CommandName == "download")
            {
                string path = string.Empty;
                DataSet ds = getdata.getLatest_DocumentVerisonSelect(new Guid(UID));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    path = Server.MapPath(ds.Tables[0].Rows[0]["Doc_FileName"].ToString());
                    //File.Decrypt(path);
                }
                else
                {
                    ds1 = getdata.ActualDocuments_SelectBy_ActualDocumentUID(new Guid(UID));
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        if (ds1.Tables[0].Rows[0]["ActualDocument_Path"].ToString() != "")
                        {
                            path = Server.MapPath(ds1.Tables[0].Rows[0]["ActualDocument_Path"].ToString());
                        }
                    }
                }
                // added on 20/10/2020
                ds.Clear();
                ds = getdata.ActualDocuments_SelectBy_ActualDocumentUID(new Guid(UID));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["ActualDocument_Name"].ToString() != "")
                    {
                        filename = ds.Tables[0].Rows[0]["ActualDocument_Name"].ToString() + ds.Tables[0].Rows[0]["ActualDocument_Type"].ToString();
                    }
                }
                //
                string getExtension = System.IO.Path.GetExtension(path);
                string outPath = path.Replace(getExtension, "") + "_download" + getExtension;
                getdata.DecryptFile(path, outPath);
                System.IO.FileInfo file = new System.IO.FileInfo(outPath);

                if (file.Exists)
                {
                    int Cnt = getdata.DocumentHistroy_InsertorUpdate(Guid.NewGuid(), new Guid(UID), new Guid(Session["UserUID"].ToString()), "Downloaded", "Documents");
                    if (Cnt <= 0)
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code: DDH-01. there is a problem with updating histroy. Please contact system admin.');</script>");
                    }
                    Response.Clear();

                    Response.AddHeader("Content-Disposition", "attachment; filename=" + filename);

                    Response.AddHeader("Content-Length", file.Length.ToString());

                    Response.ContentType = "application/octet-stream";

                    Response.WriteFile(file.FullName);

                    Response.Flush();

                    try
                    {
                        if (File.Exists(outPath))
                        {
                            File.Delete(outPath);
                        }
                    }
                    catch (Exception ex)
                    {
                        //throw
                    }

                    Response.End();



                }
                else
                {

                    //Response.Write("This file does not exist.");
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('File does not exists.');</script>");

                }
            }

            if (e.CommandName == "delete")
            {
                int cnt = getdata.ActualDocuments_Delete_by_DocID(new Guid(UID), new Guid(Session["UserUID"].ToString()));
                if (cnt > 0)
                {
                    BindDocuments_By_Category(TreeView2.SelectedNode.Value);
                }
            }
            if (e.CommandName == "ViewDoc")
            {
                string FilePath = Server.MapPath(UID);
                if (File.Exists(FilePath))
                {
                    string getExtension = System.IO.Path.GetExtension(FilePath);
                    string outPath = FilePath.Replace(getExtension, "") + "_download" + getExtension;
                    getdata.DecryptFile(FilePath, outPath);
                    //Response.Write("<script>window.open('" + outPath + "','_blank')</script>");
                    WebClient User = new WebClient();
                    Byte[] FileBuffer = User.DownloadData(outPath);
                    if (FileBuffer != null)
                    {
                        Response.ContentType = "application/pdf";
                        Response.AddHeader("content-length", FileBuffer.Length.ToString());
                        Response.BinaryWrite(FileBuffer);
                    }
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('File not found');</script>");
                }

            }
            else if (e.CommandName == "Folder_View") // this is for maintaining the folder structure....
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                // string name = GrdActualDocuments_new.Rows[rowIndex].Cells[0].Text;
                GridViewRow row = GrdActualDocuments1.Rows[rowIndex];
                LinkButton lnkfolder = (LinkButton)row.FindControl("LinkButton2");
                string name = lnkfolder.Text.Trim().Replace("amp;", "");
                UID = row.Cells[9].Text; ;
                for (int i = 0; i < TreeView2.Nodes.Count; i++)
                {
                    retrieveNodesCategory_2(TreeView2.Nodes[i], UID, name);
                }

                // BindDocuments_By_Category(UID);
                //string folder = 
            }

            if (e.CommandName == "copyfile")
            {
                if (Session["copydocument"] != null)
                {
                    getdata.sfinallist = (List<CopyDocumentFile>)Session["copydocument"];
                    if (!getdata.sfinallist.Any(x => x.DocumentUID == new Guid(UID)))
                    {
                        CopyDocumentFile cFile = new CopyDocumentFile();
                        cFile.DocumentUID = new Guid(UID);
                        cFile.DocumentName = getdata.GetActualDocumentName_by_ActualDocumentUID(new Guid(UID));
                        getdata.sfinallist.Add(cFile);
                        Session["copydocument"] = getdata.sfinallist;
                    }
                }
                else
                {
                    CopyDocumentFile cFile = new CopyDocumentFile();
                    cFile.DocumentUID = new Guid(UID);
                    cFile.DocumentName = getdata.GetActualDocumentName_by_ActualDocumentUID(new Guid(UID));
                    getdata.sfinallist.Add(cFile);
                    Session["copydocument"] = getdata.sfinallist;
                }

                //LblcopyFileCount.Text = "Copied File/s (" + getdata.sfinallist.Count + ")";
                //LblcopyFileCount.Visible = true;
            }
        }

        protected void GrdActualDocuments1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void GrdActualDocuments1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    string ActualDocumentUID = "";
                    if (ViewState["folder"].ToString().EndsWith(".pdf") || ViewState["folder"].ToString().EndsWith(".PDF") || ViewState["folder"].ToString().EndsWith(".doc") || ViewState["folder"].ToString().EndsWith(".docx") || ViewState["folder"].ToString().EndsWith(".dwg") || ViewState["folder"].ToString().EndsWith(".xlsx") || ViewState["folder"].ToString().EndsWith(".xls") || ViewState["folder"].ToString().EndsWith(".txt") || ViewState["folder"].ToString().EndsWith(".pptx") || ViewState["folder"].ToString().EndsWith(".log") || ViewState["folder"].ToString().EndsWith(".zip") || ViewState["folder"].ToString().EndsWith(".bak"))
                    {
                        if (e.Row.Cells[2].Text != "&nbsp;")
                        {
                            ActualDocumentUID = e.Row.Cells[2].Text;
                            DataSet ds = getdata.getTop1_DocumentStatusSelect(new Guid(e.Row.Cells[2].Text));
                            if (ds != null)
                            {
                                int delay = 0;
                                Label lblVer = (Label)e.Row.FindControl("LblVersion");
                                if (ds.Tables[0].Rows[0]["DocumentType"].ToString() == "General Document")
                                {

                                    lblVer.Text = "[ Ver. 1 ]";
                                    e.Row.Cells[2].Text = "Submitted";
                                    e.Row.Cells[4].Text = "No History";
                                }
                                else
                                {
                                    delay = getdata.GetDelayed_Actual_Documents(new Guid(ActualDocumentUID), ds.Tables[0].Rows[0]["ActivityType"].ToString());
                                    e.Row.Cells[2].Text = ds.Tables[0].Rows[0]["ActivityType"].ToString();
                                    if (ds.Tables[0].Rows[0]["ActivityType"].ToString() != "" && ds.Tables[0].Rows[0]["TopVersion"].ToString() != "")
                                    {
                                        //e.Row.Cells[1].Text = ds.Tables[0].Rows[0]["TopVersion"].ToString();

                                        lblVer.Text = "[ Ver. " + ds.Tables[0].Rows[0]["TopVersion"].ToString() + " ]";
                                    }
                                    else
                                    {
                                        lblVer.Text = "[ Ver. 1 ]";
                                    }
                                    if (ViewState["isUpdateStatus"].ToString() == "false")
                                    {
                                        e.Row.Cells[4].Text = "";
                                    }
                                }
                                //
                                if (ViewState["isView"].ToString() == "false")
                                {
                                    Label lblName = (Label)e.Row.FindControl("lblName");
                                    e.Row.Cells[0].Text = lblName.Text + " " + lblVer.Text;
                                }
                                if (e.Row.Cells[2].Text == "Code A")
                                {
                                    if (ViewState["isDownloadNJSE"].ToString() == "false")
                                    {
                                        e.Row.Cells[3].Text = "";
                                    }
                                }
                                //
                                if (e.Row.Cells[2].Text == "Client Approved" || e.Row.Cells[2].Text == "Approved" || e.Row.Cells[2].Text == "Client Approve")
                                {
                                    //e.Row.Cells[13].BackColor = System.Drawing.Color.Green;
                                    e.Row.Cells[2].ForeColor = System.Drawing.Color.Green;
                                    e.Row.Cells[2].Font.Bold = true;
                                    if (ViewState["isDownloadClient"].ToString() == "false")
                                    {
                                        e.Row.Cells[3].Text = "";
                                    }
                                }
                                if (delay == 1)
                                {
                                    e.Row.Cells[2].BackColor = System.Drawing.Color.Red;
                                    e.Row.Cells[2].ForeColor = System.Drawing.Color.White;
                                }
                            }

                        }

                        HtmlImage lnkVoucher = e.Row.FindControl("imgfolder") as HtmlImage;
                        lnkVoucher.Visible = false;
                        LinkButton lnkfolder = (LinkButton)e.Row.FindControl("LinkButton2");
                        lnkfolder.Visible = false;


                    }
                    else
                    {// added for folder structure on 22/10/2020



                        string[] foldernames = e.Row.Cells[7].Text.Split('/');
                        int index = 0;
                        int correctindex = 0;
                        string foldername = "";
                        foreach (string str in foldernames)
                        {
                            foldername = str.Trim().Replace("amp;", "");
                            if (foldername == ViewState["folder"].ToString().Trim())
                            {
                                correctindex = index + 1;
                            }
                            index = index + 1;
                        }
                        if (folder != e.Row.Cells[7].Text.Split('/')[correctindex].Trim().Replace("&amp;", "&"))
                        {
                            if (e.Row.Cells[8].Text.Trim().Replace("amp;", "") == ViewState["folder"].ToString())
                            {
                                if (e.Row.Cells[2].Text != "&nbsp;")
                                {
                                    ActualDocumentUID = e.Row.Cells[2].Text;
                                    DataSet ds = getdata.getTop1_DocumentStatusSelect(new Guid(e.Row.Cells[2].Text));
                                    if (ds != null)
                                    {
                                        int delay = 0;
                                        Label lblVer = (Label)e.Row.FindControl("LblVersion");
                                        if (ds.Tables[0].Rows[0]["DocumentType"].ToString() == "General Document")
                                        {

                                            lblVer.Text = "[ Ver. 1 ]";
                                            e.Row.Cells[2].Text = "Submitted";
                                            e.Row.Cells[4].Text = "No History";
                                        }
                                        else
                                        {
                                            delay = getdata.GetDelayed_Actual_Documents(new Guid(ActualDocumentUID), ds.Tables[0].Rows[0]["ActivityType"].ToString());
                                            e.Row.Cells[2].Text = ds.Tables[0].Rows[0]["ActivityType"].ToString();
                                            if (ds.Tables[0].Rows[0]["ActivityType"].ToString() != "" && ds.Tables[0].Rows[0]["TopVersion"].ToString() != "")
                                            {
                                                //e.Row.Cells[1].Text = ds.Tables[0].Rows[0]["TopVersion"].ToString();

                                                lblVer.Text = "[ Ver. " + ds.Tables[0].Rows[0]["TopVersion"].ToString() + " ]";
                                            }
                                            else
                                            {
                                                lblVer.Text = "[ Ver. 1 ]";
                                            }
                                            if (ViewState["isUpdateStatus"].ToString() == "false")
                                            {
                                                e.Row.Cells[4].Text = "";
                                            }
                                        }
                                        //
                                        if (ViewState["isView"].ToString() == "false")
                                        {
                                            Label lblName = (Label)e.Row.FindControl("lblName");
                                            e.Row.Cells[0].Text = lblName.Text + " " + lblVer.Text;
                                        }
                                        if (e.Row.Cells[2].Text == "Code A")
                                        {
                                            if (ViewState["isDownloadNJSE"].ToString() == "false")
                                            {
                                                e.Row.Cells[3].Text = "";
                                            }
                                        }
                                        //
                                        if (e.Row.Cells[2].Text == "BWSSB Approved" || e.Row.Cells[2].Text == "Approved" || e.Row.Cells[2].Text == "BWSSB Approve")
                                        {
                                            //e.Row.Cells[13].BackColor = System.Drawing.Color.Green;
                                            e.Row.Cells[2].ForeColor = System.Drawing.Color.Green;
                                            e.Row.Cells[2].Font.Bold = true;
                                            if (ViewState["isDownloadClient"].ToString() == "false")
                                            {
                                                e.Row.Cells[3].Text = "";
                                            }
                                        }
                                        if (delay == 1)
                                        {
                                            e.Row.Cells[2].BackColor = System.Drawing.Color.Red;
                                            e.Row.Cells[2].ForeColor = System.Drawing.Color.White;
                                        }
                                    }

                                }

                                HtmlImage lnkVoucher = e.Row.FindControl("imgfolder") as HtmlImage;
                                lnkVoucher.Visible = false;
                                LinkButton lnkfolder = (LinkButton)e.Row.FindControl("LinkButton2");
                                lnkfolder.Visible = false;
                            }
                            else
                            {
                                //e.Row.Attributes["style"] = "display:none";
                                if (correctindex != 0)
                                {
                                    if ((e.Row.Cells[7].Text.Trim().Replace("amp;", "").Contains(ViewState["folder"].ToString())))
                                    {
                                        HtmlGenericControl htmlDivControl = e.Row.FindControl("divPDF") as HtmlGenericControl;
                                        htmlDivControl.Visible = false;
                                        LinkButton lnkfolder = (LinkButton)e.Row.FindControl("LinkButton2");
                                        lnkfolder.Text = e.Row.Cells[7].Text.Split('/')[correctindex];
                                        e.Row.Cells[3].Text = "";
                                        // e.Row.Cells[0].Text = "<a href='#' onclick='someFunction()'><img src='/_assets/images/folder.jpg' height='15px' width='20px'>  " + e.Row.Cells[7].Text.Split('/')[0] + "</a>";
                                        //e.Row.Cells[0].Text = "<asp:LinkButton ID = \"LinkButton3\" runat = \"server\" CausesValidation = \"False\" CommandName = \"Edit\" Text = \"Edit\" ></ asp:LinkButton >";
                                        e.Row.Cells[1].Text = "";
                                        e.Row.Cells[2].Text = "";
                                        e.Row.Cells[4].Text = "";
                                        e.Row.Cells[5].Text = "";
                                        e.Row.Cells[6].Text = "";
                                        //e.Row.Cells[7].Text = "";
                                        //e.Row.Cells[8].Text = "";
                                        folder = e.Row.Cells[7].Text.Trim().Split('/')[correctindex].Replace("amp;", "");
                                    }
                                    else
                                    {
                                        // e.Row.Attributes["style"] = "display:none";
                                        e.Row.Visible = false;
                                    }
                                }
                                else
                                {
                                    e.Row.Visible = false;
                                }
                            }
                        }
                        else
                        {
                            e.Row.Attributes["style"] = "display:none";
                        }
                    }
                }



            }
            catch (Exception ex)
            {
                string Error = ex.Message;
            }
        }

        protected void retrieveNodesCategory_2(TreeNode node, string SelectedVal, string name)
        {
            if (node.Value == SelectedVal && node.Text.ToLower() == name.ToLower())
            {
                node.Selected = true;
                node.Expand();
                node.Parent.Expand();
                //BindActivities();
                BindDocuments_By_Category(TreeView2.SelectedNode.Value);
            }
            else
            {
                if (node.ChildNodes.Count != 0)
                {
                    foreach (TreeNode tn in node.ChildNodes)
                    {
                        if (tn.Value == SelectedVal && tn.Text.ToLower() == name.ToLower())
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
                            BindDocuments_By_Category(TreeView2.SelectedNode.Value);
                        }
                        else
                        {
                            if (tn.ChildNodes.Count != 0)
                            {
                                retrieveNodesCategory_2(tn, SelectedVal, name.ToLower());
                            }
                        }
                    }
                }
            }
        }

        public string GetWorkpackageName(string WorkPackageUID)
        {
            string wName = "";
            string wkUID = WorkPackageUID.Split('*')[0];
            string sTaskUID = WorkPackageUID.Split('*')[1];
            if (sTaskUID == Guid.Empty.ToString())
            {
                wName = getdata.getWorkPackageNameby_WorkPackageUID(new Guid(wkUID));
            }
            else
            {
                wName = getdata.getTaskNameby_TaskUID(new Guid(sTaskUID));
            }
            return wName;
        }
    }
}