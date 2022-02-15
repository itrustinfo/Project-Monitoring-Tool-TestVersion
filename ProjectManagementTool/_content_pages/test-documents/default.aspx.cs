using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace ProjectManagementTool._content_pages.test_documents
{
    public partial class _default : System.Web.UI.Page
    {
        DBGetData getdata = new DBGetData();
        TaskUpdate gettk = new TaskUpdate();
        GeneralDocuments GD = new GeneralDocuments();
        public int SearchResultCount = 0;
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
                        UpdatePanel2,
                        this.GetType(),
                        "MyAction",
                        "BindEvents();",
                        true);
                //ScriptManager.RegisterStartupScript(
                //        UpdatePanel1,
                //        this.GetType(),
                //        "MyAction",
                //        "BindEvents();",
                //        true);
                if (!IsPostBack)
                {
                    // added on 05/11/2020
                    DataSet dscheck = new DataSet();
                    dscheck = getdata.GetUsertypeFunctionality_Mapping(Session["TypeOfUser"].ToString());
                    ViewState["isDelete"] = "false";
                    ViewState["isUpload"] = "false";
                    ViewState["isUpdateStatus"] = "false";
                    ViewState["isView"] = "false";
                    ViewState["isDownloadClient"] = "false";
                    ViewState["isDownloadNJSE"] = "false";
                    Session["isDownloadClient"] = "false";
                    Session["isDownloadNJSE"] = "false";
                  
                  
                    //----------------------------------------
                    if (Session["SelectedTaskUID"] != null)
                    {
                        if (Session["ViewDocBy"] != null)
                        {
                            if (Session["ViewDocBy"].ToString() == "Activity")
                            {
                                //ByCategory.Visible = false;
                                //ByActivity.Visible = true;
                                RDBDocumentView.SelectedIndex = 0;
                                
                            }
                            else
                            {
                                //ByCategory.Visible = true;
                                //ByActivity.Visible = false;
                                RDBDocumentView.SelectedIndex = 1;
                                
                            }
                        }
                        else
                        {
                            RDBDocumentView.SelectedIndex = 0;
                            
                        }

                    }
                    else if (Session["SelectedTaskUID1"] != null)
                    {
                        //BindTreeview();
                        //ByCategory.Visible = false;
                        //ByActivity.Visible = true;
                        //Session["SelectedTaskUID1"] = null;
                        if (Session["ViewDocBy"] != null)
                        {
                            if (Session["ViewDocBy"].ToString() == "Activity")
                            {
                                //ByCategory.Visible = false;
                                //ByActivity.Visible = true;
                                RDBDocumentView.SelectedIndex = 0;
                                
                            }
                            else
                            {
                                //ByCategory.Visible = true;
                                //ByActivity.Visible = false;
                                RDBDocumentView.SelectedIndex = 1;
                                
                            }
                        }
                        else
                        {
                            RDBDocumentView.SelectedIndex = 0;
                            
                        }
                    }
                    else
                    {
                        BindTreeview();
                        //ByCategory.Visible = false;
                        ByActivity.Visible = true;

                    }
                }

                if (Session["copydocument"] != null)
                {
                    getdata.sfinallist = (List<CopyDocumentFile>)Session["copydocument"];
                    LblcopyFileCount.Text = "Copied File/s (" + getdata.sfinallist.Count + ")";
                    LblcopyFileCount.Visible = true;
                }
                else
                {
                    LblcopyFileCount.Visible = false;
                }
            }
        }

        public void BindTreeview()
        {
            TreeView1.Nodes.Clear();
            DataSet ds = new DataSet();
            if (Session["TypeOfUser"].ToString() == "U" || Session["TypeOfUser"].ToString() == "MD" || Session["TypeOfUser"].ToString() == "VP")
            {
                //ds = gettk.GetAllProjects();
                ds = getdata.ProjectClass_Select_All();
            }
            else if (Session["TypeOfUser"].ToString() == "PA")
            {
                //ds = getdata.GetAssignedProjects_by_UserUID(new Guid(Session["UserUID"].ToString()));
                ds = getdata.ProjectClass_Select_By_UserUID(new Guid(Session["UserUID"].ToString()));
            }
            else
            {
                //ds = getdata.GetAssignedProjects_by_UserUID(new Guid(Session["UserUID"].ToString()));
                ds = getdata.ProjectClass_Select_By_UserUID(new Guid(Session["UserUID"].ToString()));
            }
            if (ds.Tables[0].Rows.Count > 0)
            {
                PopulateTreeView(ds, null, "", 0);

                if (Session["SelectedTaskUID"] != null)
                {
                    TreeView1.CollapseAll();
                    string UID = Session["SelectedTaskUID"].ToString();
                    for (int i = 0; i < TreeView1.Nodes.Count; i++)
                    {
                        retrieveNodes(TreeView1.Nodes[i], UID);
                    }

                    Session["SelectedTaskUID"] = null;
                    Session["ViewDocBy"] = null;
                }
                else if (Session["SelectedTaskUID1"] != null)
                {
                    TreeView1.CollapseAll();
                    string UID = Session["SelectedTaskUID1"].ToString();
                    for (int i = 0; i < TreeView1.Nodes.Count; i++)
                    {
                        retrieveNodes(TreeView1.Nodes[i], UID);
                    }
                    BindDocuments(TreeView1.SelectedNode.Value);
                    Session["SelectedTaskUID1"] = null;
                    Session["ViewDocBy"] = null;
                }
                else if (Request.QueryString["SubmittalUID"] != null)
                {
                    TreeView1.CollapseAll();
                    string UID = Request.QueryString["SubmittalUID"];
                    for (int i = 0; i < TreeView1.Nodes.Count; i++)
                    {
                        retrieveNodes(TreeView1.Nodes[i], UID);
                    }
                    BindDocuments(TreeView1.SelectedNode.Value);
                }
                else
                {
                    TreeView1.Nodes[0].Selected = true;
                    TreeView1.CollapseAll();
                    TreeView1.Nodes[0].Expand();
                    //TreeView1.ExpandAll();


                }
                BindActivities();
                




                //BindDocuments(TreeView1.SelectedNode.Value);
            }
            else
            {
                //AddDocument.Visible = false;
            }
        }

        //public void BindTreeview_by_Category()
        //{
        //    TreeView2.Nodes.Clear();
        //    DataSet ds = new DataSet();
        //    if (Session["TypeOfUser"].ToString() == "U" || Session["TypeOfUser"].ToString() == "MD" || Session["TypeOfUser"].ToString() == "VP")
        //    {
        //        //ds = gettk.GetAllProjects();
        //        ds = getdata.ProjectClass_Select_All();
        //    }
        //    else if (Session["TypeOfUser"].ToString() == "PA")
        //    {
        //        //ds = getdata.GetAssignedProjects_by_UserUID(new Guid(Session["UserUID"].ToString()));
        //        ds = getdata.ProjectClass_Select_By_UserUID(new Guid(Session["UserUID"].ToString()));
        //    }
        //    else
        //    {
        //        //ds = getdata.GetAssignedProjects_by_UserUID(new Guid(Session["UserUID"].ToString()));
        //        ds = getdata.ProjectClass_Select_By_UserUID(new Guid(Session["UserUID"].ToString()));
        //    }
        //    if (ds.Tables[0].Rows.Count > 0)
        //    {
        //        PopulateTreeView_by_Category(ds, null, "", 0);
        //        if (Session["SelectedTaskUID"] != null)
        //        {
        //            TreeView2.CollapseAll();
        //            string UID = Session["SelectedTaskUID"].ToString();
        //            for (int i = 0; i < TreeView2.Nodes.Count; i++)
        //            {
        //                retrieveNodesCategory(TreeView2.Nodes[i], UID);
        //            }
        //            Session["SelectedTaskUID"] = null;
        //            Session["ViewDocBy"] = null;
        //        }
        //        else if (Session["SelectedTaskUID1"] != null)
        //        {
        //            TreeView2.CollapseAll();
        //            string UID = Session["SelectedTaskUID1"].ToString();
        //            for (int i = 0; i < TreeView2.Nodes.Count; i++)
        //            {
        //                retrieveNodesCategory(TreeView2.Nodes[i], UID);
        //            }
        //            //BindDocuments(TreeView1.SelectedNode.Value);
        //            Session["SelectedTaskUID1"] = null;
        //            Session["ViewDocBy"] = null;
        //        }
        //        else
        //        {
        //            TreeView2.Nodes[0].Selected = true;
        //            //TreeView2.Nodes[0].Expand();
        //            //TreeView2.ExpandAll();
        //            TreeView2.CollapseAll();
        //            TreeView2.Nodes[0].Expand();
        //        }

        //        //BindDocuments(TreeView1.SelectedNode.Value);
        //        //BindDocuments_By_Category(TreeView2.SelectedNode.Value);
        //    }
        //}
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
        public void PopulateTreeView(DataSet dtParent, TreeNode treeNode, string ParentUID, int Level)
        {
            try
            {
                foreach (DataRow row in dtParent.Tables[0].Rows)
                {
                    //TreeNode child = new TreeNode
                    //{
                    //    Text = Level == 0 ? row["ProjectClass_Name"].ToString() : Level == 1 ? LimitCharts(getdata.getProjectNameby_ProjectUID(new Guid(row["ProjectUID"].ToString()))) : Level == 2 ? row["Name"].ToString() : Level == 3 ? getdata.WorkpackageoptionName_SelectBy_UID(new Guid(row["Workpackage_OptionUID"].ToString())) : row["Name"].ToString(),
                    //    Value = Level == 0 ? row["ProjectClass_UID"].ToString() : Level == 1 ? row["ProjectUID"].ToString() : Level == 2 ? row["WorkPackageUID"].ToString() : Level == 3 ? row["Workpackage_OptionUID"].ToString() : row["TaskUID"].ToString(),
                    //    Target = Level == 0 ? "Class" : Level == 1 ? "Project" : Level == 2 ? "WorkPackage" : Level == 3 ? "Option" : "Tasks",
                    //    ToolTip = Level == 0 ? row["ProjectClass_Name"].ToString() : Level == 1 ? getdata.getProjectNameby_ProjectUID(new Guid(row["ProjectUID"].ToString())) : Level == 2 ? row["Name"].ToString() : Level == 3 ? getdata.WorkpackageoptionName_SelectBy_UID(new Guid(row["Workpackage_OptionUID"].ToString())) : row["Name"].ToString(),
                    //};

                    TreeNode child = new TreeNode
                    {
                        Text = Level == 0 ? row["ProjectClass_Name"].ToString() : Level == 1 ? LimitCharts(getdata.getProjectNameby_ProjectUID(new Guid(row["ProjectUID"].ToString()))) : Level == 2 ? row["Name"].ToString() : Level == 3 ? row["WorkpackageSelectedOption_Name"].ToString() : row["Name"].ToString(),
                        Value = Level == 0 ? row["ProjectClass_UID"].ToString() : Level == 1 ? row["ProjectUID"].ToString() : Level == 2 ? row["WorkPackageUID"].ToString() : Level == 3 ? row["Workpackage_OptionUID"].ToString() : row["TaskUID"].ToString(),
                        Target = Level == 0 ? "Class" : Level == 1 ? "Project" : Level == 2 ? "WorkPackage" : Level == 3 ? "Option" : "Tasks",
                        ToolTip = Level == 0 ? row["ProjectClass_Name"].ToString() : Level == 1 ? getdata.getProjectNameby_ProjectUID(new Guid(row["ProjectUID"].ToString())) : Level == 2 ? row["Name"].ToString() : Level == 3 ? row["WorkpackageSelectedOption_Name"].ToString() : row["Name"].ToString(),
                    };

                    if (ParentUID == "")
                    {
                        TreeView1.Nodes.Add(child);
                        DataSet dsProject = new DataSet();
                        if (Session["TypeOfUser"].ToString() == "U" || Session["TypeOfUser"].ToString() == "MD" || Session["TypeOfUser"].ToString() == "VP")
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
                        //TreeView1.Nodes.Add(child);
                        treeNode.ChildNodes.Add(child);
                        //DataSet dsworkPackage = getdata.GetWorkPackages_By_ProjectUID(new Guid(child.Value));
                        DataSet dsworkPackage = new DataSet();
                        if (Session["TypeOfUser"].ToString() == "U" || Session["TypeOfUser"].ToString() == "MD" || Session["TypeOfUser"].ToString() == "VP")
                        {
                            dsworkPackage = getdata.GetWorkPackages_By_ProjectUID(new Guid(child.Value));
                        }
                        else if (Session["TypeOfUser"].ToString() == "PA")
                        {
                            dsworkPackage = getdata.GetWorkPackages_ForUser_by_ProjectUID(new Guid(Session["UserUID"].ToString()), new Guid(child.Value));
                        }
                        else
                        {
                            dsworkPackage = getdata.GetWorkPackages_ForUser_by_ProjectUID(new Guid(Session["UserUID"].ToString()), new Guid(child.Value));
                        }
                        if (dsworkPackage.Tables[0].Rows.Count > 0)
                        {
                            PopulateTreeView(dsworkPackage, child, child.Value, 2);
                        }
                    }
                    else if (Level == 2)
                    {
                        treeNode.ChildNodes.Add(child);
                        DataSet dsoption = getdata.GetSelectedOption_By_WorkpackageUID(new Guid(child.Value));
                        if (dsoption.Tables[0].Rows.Count > 0)
                        {
                            PopulateTreeView(dsoption, child, child.Value, 3);
                        }

                        //DataSet dsoption = getdata.GetWorkPackages_By_WorkPackageUID(new Guid(child.Value));
                        //if (dsoption.Tables[0].Rows.Count > 0)
                        //{
                        //    PopulateTreeView(dsoption, child, child.Value, 3);
                        //}
                        //DataSet dssubmittal = getdata.GetDocuments_For_WorkPackage(new Guid(child.Value));
                        //if (dssubmittal.Tables[0].Rows.Count > 0)
                        //{
                        //    for (int i = 0; i < dssubmittal.Tables[0].Rows.Count; i++)
                        //    {
                        //        TreeNode subdoc = new TreeNode
                        //        {
                        //            Text = dssubmittal.Tables[0].Rows[i]["DocName"].ToString(),
                        //            Value = dssubmittal.Tables[0].Rows[i]["DocumentUID"].ToString(),
                        //            Target = "Submittal",
                        //            ToolTip = dssubmittal.Tables[0].Rows[i]["DocName"].ToString()
                        //        };
                        //        child.ChildNodes.Add(subdoc);

                        //        int SplitNodeCount = 0;
                        //        DataSet documents = getdata.ActualDocuments_SelectBy_DocumentUID(new Guid(dssubmittal.Tables[0].Rows[i]["DocumentUID"].ToString()));
                        //        for (int k = 0; k < documents.Tables[0].Rows.Count; k++)
                        //        {
                        //            if (documents.Tables[0].Rows[k]["ActualDocument_RelativePath"].ToString() != "")
                        //            {
                        //                SplitNodeCount = 0;
                        //                string[] relativepath = documents.Tables[0].Rows[k]["ActualDocument_RelativePath"].ToString().Split('/');
                        //                if (relativepath.Length > 0)
                        //                {
                        //                    List<string> list = new List<string>(relativepath);
                        //                    TreeNode SubFolder = new TreeNode();
                        //                    TreeNode SubFolder1 = new TreeNode();
                        //                    TreeNode SubFolder2 = new TreeNode();
                        //                    TreeNode SubFolder3 = new TreeNode();
                        //                    TreeNode SubFolder4 = new TreeNode();
                        //                    TreeNode SubFolder5 = new TreeNode();
                        //                    TreeNode SubFolder6 = new TreeNode();
                        //                    TreeNode SubFolder7 = new TreeNode();
                        //                    if (SplitNodeCount < list.Count)
                        //                    {

                        //                        //if (!CheckChildNode(subdoc, list[SplitNodeCount].ToString()))
                        //                        //{
                        //                        //    
                        //                        //}

                        //                        TreeNode trnode = SearchNode(subdoc, list[SplitNodeCount].ToString());
                        //                        if (trnode != null)
                        //                        {
                        //                            SubFolder = trnode;
                        //                        }
                        //                        else
                        //                        {
                        //                            SubFolder = new TreeNode();
                        //                            SubFolder.Text = list[SplitNodeCount].ToString();
                        //                            SubFolder.Target = "Folder";
                        //                            SubFolder.Value = documents.Tables[0].Rows[k]["ActualDocumentUID"].ToString();
                        //                            SubFolder.ToolTip = list[SplitNodeCount].ToString();

                        //                            subdoc.ChildNodes.Add(SubFolder);
                        //                        }

                        //                    }
                        //                    SplitNodeCount = SplitNodeCount + 1;

                        //                    if (SplitNodeCount < list.Count)
                        //                    {


                        //                        TreeNode trnode1 = SearchNode(SubFolder, list[SplitNodeCount].ToString());
                        //                        if (trnode1 != null)
                        //                        {
                        //                            SubFolder1 = trnode1;
                        //                        }
                        //                        else
                        //                        {
                        //                            SubFolder1 = new TreeNode();
                        //                            SubFolder1.Text = list[SplitNodeCount].ToString();
                        //                            SubFolder1.Target = "SubFolder";
                        //                            SubFolder1.Value = documents.Tables[0].Rows[k]["ActualDocumentUID"].ToString();
                        //                            SubFolder1.ToolTip = list[SplitNodeCount].ToString();
                        //                            SubFolder.ChildNodes.Add(SubFolder1);
                        //                        }
                        //                    }
                        //                    SplitNodeCount = SplitNodeCount + 1;

                        //                    if (SplitNodeCount < list.Count)
                        //                    {

                        //                        TreeNode trnode2 = SearchNode(SubFolder1, list[SplitNodeCount].ToString());
                        //                        if (trnode2 != null)
                        //                        {
                        //                            SubFolder2 = trnode2;
                        //                        }
                        //                        else
                        //                        {
                        //                            SubFolder2 = new TreeNode();
                        //                            SubFolder2.Text = list[SplitNodeCount].ToString();
                        //                            SubFolder2.Target = "SubFolder";
                        //                            SubFolder2.Value = documents.Tables[0].Rows[k]["ActualDocumentUID"].ToString();
                        //                            SubFolder2.ToolTip = list[SplitNodeCount].ToString();
                        //                            SubFolder1.ChildNodes.Add(SubFolder2);
                        //                        }


                        //                    }
                        //                    SplitNodeCount = SplitNodeCount + 1;

                        //                    if (SplitNodeCount < list.Count)
                        //                    {

                        //                        TreeNode trnode3 = SearchNode(SubFolder2, list[SplitNodeCount].ToString());
                        //                        if (trnode3 != null)
                        //                        {
                        //                            SubFolder3 = trnode3;
                        //                        }
                        //                        else
                        //                        {
                        //                            SubFolder3 = new TreeNode();
                        //                            SubFolder3.Text = list[SplitNodeCount].ToString();
                        //                            SubFolder3.Target = "SubFolder";
                        //                            SubFolder3.Value = documents.Tables[0].Rows[k]["ActualDocumentUID"].ToString();
                        //                            SubFolder3.ToolTip = list[SplitNodeCount].ToString();
                        //                            SubFolder2.ChildNodes.Add(SubFolder3);
                        //                        }


                        //                    }
                        //                    SplitNodeCount = SplitNodeCount + 1;

                        //                    if (SplitNodeCount < list.Count)
                        //                    {

                        //                        TreeNode trnode4 = SearchNode(SubFolder3, list[SplitNodeCount].ToString());
                        //                        if (trnode4 != null)
                        //                        {
                        //                            SubFolder4 = trnode4;
                        //                        }
                        //                        else
                        //                        {
                        //                            SubFolder4 = new TreeNode();
                        //                            SubFolder4.Text = list[SplitNodeCount].ToString();
                        //                            SubFolder4.Target = "SubFolder";
                        //                            SubFolder4.Value = documents.Tables[0].Rows[k]["ActualDocumentUID"].ToString();
                        //                            SubFolder4.ToolTip = list[SplitNodeCount].ToString();
                        //                            SubFolder3.ChildNodes.Add(SubFolder4);
                        //                        }


                        //                    }
                        //                    SplitNodeCount = SplitNodeCount + 1;

                        //                    if (SplitNodeCount < list.Count)
                        //                    {

                        //                        TreeNode trnode = SearchNode(SubFolder4, list[SplitNodeCount].ToString());
                        //                        if (trnode != null)
                        //                        {
                        //                            SubFolder5 = trnode;
                        //                        }
                        //                        else
                        //                        {
                        //                            SubFolder5 = new TreeNode();
                        //                            SubFolder5.Text = list[SplitNodeCount].ToString();
                        //                            SubFolder5.Target = "SubFolder";
                        //                            SubFolder5.Value = documents.Tables[0].Rows[k]["ActualDocumentUID"].ToString();
                        //                            SubFolder5.ToolTip = list[SplitNodeCount].ToString();
                        //                            SubFolder4.ChildNodes.Add(SubFolder5);
                        //                        }
                        //                    }
                        //                    SplitNodeCount = SplitNodeCount + 1;

                        //                    if (SplitNodeCount < list.Count)
                        //                    {

                        //                        TreeNode trnode = SearchNode(SubFolder5, list[SplitNodeCount].ToString());
                        //                        if (trnode != null)
                        //                        {
                        //                            SubFolder6 = trnode;
                        //                        }
                        //                        else
                        //                        {
                        //                            SubFolder6 = new TreeNode();
                        //                            SubFolder6.Text = list[SplitNodeCount].ToString();
                        //                            SubFolder6.Target = "SubFolder";
                        //                            SubFolder6.Value = documents.Tables[0].Rows[k]["ActualDocumentUID"].ToString();
                        //                            SubFolder6.ToolTip = list[SplitNodeCount].ToString();
                        //                            SubFolder5.ChildNodes.Add(SubFolder6);
                        //                        }
                        //                    }

                        //                    SplitNodeCount = SplitNodeCount + 1;

                        //                    if (SplitNodeCount < list.Count)
                        //                    {

                        //                        TreeNode trnode = SearchNode(SubFolder6, list[SplitNodeCount].ToString());
                        //                        if (trnode != null)
                        //                        {
                        //                            SubFolder7 = trnode;
                        //                        }
                        //                        else
                        //                        {
                        //                            SubFolder7 = new TreeNode();
                        //                            SubFolder7.Text = list[SplitNodeCount].ToString();
                        //                            SubFolder7.Target = "SubFolder";
                        //                            SubFolder7.Value = documents.Tables[0].Rows[k]["ActualDocumentUID"].ToString();
                        //                            SubFolder7.ToolTip = list[SplitNodeCount].ToString();
                        //                            SubFolder6.ChildNodes.Add(SubFolder7);
                        //                        }
                        //                    }

                        //                }
                        //            }
                        //        }
                        //    }

                        //}


                        //DataSet ds = getdata.GetDocuments_For_WorkPackage(new Guid(row["WorkPackageUID"].ToString()));
                        //if (ds.Tables[0].Rows.Count > 0)
                        //{
                        //    TreeNode Doc = new TreeNode
                        //    {
                        //        Text = "Documents",
                        //        Value = row["WorkPackageUID"].ToString(),
                        //        Target = "WorkPackage Document",
                        //        ToolTip = "Documents",
                        //        SelectAction = TreeNodeSelectAction.None
                        //    };
                        //    child.ChildNodes.Add(Doc);

                        //    DataSet cat = getdata.WorkPackageCategory_Selectby_WorkPackageUID(new Guid(row["WorkPackageUID"].ToString()));
                        //    if (cat.Tables[0].Rows.Count > 0)
                        //    {
                        //        for (int i = 0; i < cat.Tables[0].Rows.Count; i++)
                        //        {
                        //            TreeNode cate = new TreeNode
                        //            {
                        //                Text = cat.Tables[0].Rows[i]["WorkPackageCategory_Name"].ToString(),
                        //                Value = cat.Tables[0].Rows[i]["WorkPackageCategory_UID"].ToString(),
                        //                Target = "Category",
                        //                ToolTip = cat.Tables[0].Rows[i]["WorkPackageCategory_Name"].ToString()
                        //            };
                        //            Doc.ChildNodes.Add(cate);
                        //        }

                        //    }
                        //}

                        //DataSet dschild = getdata.GetTasksForWorkPackages(child.Value);
                        ////DataTable dtChild = TreeViewBAL.BL.TreeViewBL.GetData("Select ID,Name from Module where ProjID=" + child.Value);
                        //if (dschild.Tables[0].Rows.Count > 0)
                        //{
                        //    PopulateTreeView(dschild, child, child.Value, 3);
                        //}

                    }
                    else if (Level == 3)
                    {
                        treeNode.ChildNodes.Add(child);

                        DataSet dssubmittal = getdata.getDocumentsForTasks(new Guid(child.Value));
                        if (dssubmittal.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < dssubmittal.Tables[0].Rows.Count; i++)
                            {
                                TreeNode subdoc = new TreeNode
                                {
                                    Text = "<div style='color:#007bff;'>" + dssubmittal.Tables[0].Rows[i]["DocName"].ToString() + "</div>",
                                    Value = dssubmittal.Tables[0].Rows[i]["DocumentUID"].ToString(),
                                    Target = "Submittal",
                                    ToolTip = dssubmittal.Tables[0].Rows[i]["DocName"].ToString(),
                                    ImageUrl = "~/_assets/images/submittal.png"
                                };
                                child.ChildNodes.Add(subdoc);


                                int SplitNodeCount = 0;
                                DataSet documents = getdata.ActualDocuments_SelectBy_DocumentUID(new Guid(dssubmittal.Tables[0].Rows[i]["DocumentUID"].ToString()));
                                for (int k = 0; k < documents.Tables[0].Rows.Count; k++)
                                {
                                    if (documents.Tables[0].Rows[k]["ActualDocument_RelativePath"].ToString() != "")
                                    {
                                        SplitNodeCount = 0;
                                        string[] relativepath = documents.Tables[0].Rows[k]["ActualDocument_RelativePath"].ToString().Split('/');
                                        if (relativepath.Length > 0)
                                        {
                                            List<string> list = new List<string>(relativepath);
                                            TreeNode SubFolder = new TreeNode();
                                            TreeNode SubFolder1 = new TreeNode();
                                            TreeNode SubFolder2 = new TreeNode();
                                            TreeNode SubFolder3 = new TreeNode();
                                            TreeNode SubFolder4 = new TreeNode();
                                            TreeNode SubFolder5 = new TreeNode();
                                            TreeNode SubFolder6 = new TreeNode();
                                            TreeNode SubFolder7 = new TreeNode();
                                            if (SplitNodeCount < list.Count)
                                            {

                                                //if (!CheckChildNode(subdoc, list[SplitNodeCount].ToString()))
                                                //{
                                                //    
                                                //}

                                                TreeNode trnode = SearchNode(subdoc, list[SplitNodeCount].ToString());
                                                if (trnode != null)
                                                {
                                                    SubFolder = trnode;
                                                }
                                                else
                                                {
                                                    SubFolder = new TreeNode();
                                                    SubFolder.Text = list[SplitNodeCount].ToString();
                                                    SubFolder.Target = "Folder";
                                                    SubFolder.Value = documents.Tables[0].Rows[k]["ActualDocumentUID"].ToString();
                                                    SubFolder.ToolTip = documents.Tables[0].Rows[k]["ActualDocument_RelativePath"].ToString();// list[SplitNodeCount].ToString();
                                                    SubFolder.ImageUrl = "~/_assets/images/submittal.png";
                                                    subdoc.ChildNodes.Add(SubFolder);
                                                }

                                            }
                                            SplitNodeCount = SplitNodeCount + 1;

                                            if (SplitNodeCount < list.Count)
                                            {


                                                TreeNode trnode1 = SearchNode(SubFolder, list[SplitNodeCount].ToString());
                                                if (trnode1 != null)
                                                {
                                                    SubFolder1 = trnode1;
                                                }
                                                else
                                                {
                                                    SubFolder1 = new TreeNode();
                                                    SubFolder1.Text = list[SplitNodeCount].ToString();
                                                    SubFolder1.Target = "SubFolder";
                                                    SubFolder1.Value = documents.Tables[0].Rows[k]["ActualDocumentUID"].ToString();
                                                    SubFolder1.ToolTip = documents.Tables[0].Rows[k]["ActualDocument_RelativePath"].ToString();// list[SplitNodeCount].ToString();
                                                    SubFolder.ChildNodes.Add(SubFolder1);
                                                }
                                            }
                                            SplitNodeCount = SplitNodeCount + 1;

                                            if (SplitNodeCount < list.Count)
                                            {

                                                TreeNode trnode2 = SearchNode(SubFolder1, list[SplitNodeCount].ToString());
                                                if (trnode2 != null)
                                                {
                                                    SubFolder2 = trnode2;
                                                }
                                                else
                                                {
                                                    SubFolder2 = new TreeNode();
                                                    SubFolder2.Text = list[SplitNodeCount].ToString();
                                                    SubFolder2.Target = "SubFolder";
                                                    SubFolder2.Value = documents.Tables[0].Rows[k]["ActualDocumentUID"].ToString();
                                                    SubFolder2.ToolTip = documents.Tables[0].Rows[k]["ActualDocument_RelativePath"].ToString();// list[SplitNodeCount].ToString();
                                                    SubFolder1.ChildNodes.Add(SubFolder2);
                                                }


                                            }
                                            SplitNodeCount = SplitNodeCount + 1;

                                            if (SplitNodeCount < list.Count)
                                            {

                                                TreeNode trnode3 = SearchNode(SubFolder2, list[SplitNodeCount].ToString());
                                                if (trnode3 != null)
                                                {
                                                    SubFolder3 = trnode3;
                                                }
                                                else
                                                {
                                                    SubFolder3 = new TreeNode();
                                                    SubFolder3.Text = list[SplitNodeCount].ToString();
                                                    SubFolder3.Target = "SubFolder";
                                                    SubFolder3.Value = documents.Tables[0].Rows[k]["ActualDocumentUID"].ToString();
                                                    SubFolder3.ToolTip = documents.Tables[0].Rows[k]["ActualDocument_RelativePath"].ToString();// list[SplitNodeCount].ToString();
                                                    SubFolder2.ChildNodes.Add(SubFolder3);
                                                }


                                            }
                                            SplitNodeCount = SplitNodeCount + 1;

                                            if (SplitNodeCount < list.Count)
                                            {

                                                TreeNode trnode4 = SearchNode(SubFolder3, list[SplitNodeCount].ToString());
                                                if (trnode4 != null)
                                                {
                                                    SubFolder4 = trnode4;
                                                }
                                                else
                                                {
                                                    SubFolder4 = new TreeNode();
                                                    SubFolder4.Text = list[SplitNodeCount].ToString();
                                                    SubFolder4.Target = "SubFolder";
                                                    SubFolder4.Value = documents.Tables[0].Rows[k]["ActualDocumentUID"].ToString();
                                                    SubFolder4.ToolTip = documents.Tables[0].Rows[k]["ActualDocument_RelativePath"].ToString();// list[SplitNodeCount].ToString();
                                                    SubFolder3.ChildNodes.Add(SubFolder4);
                                                }


                                            }
                                            SplitNodeCount = SplitNodeCount + 1;

                                            if (SplitNodeCount < list.Count)
                                            {

                                                TreeNode trnode = SearchNode(SubFolder4, list[SplitNodeCount].ToString());
                                                if (trnode != null)
                                                {
                                                    SubFolder5 = trnode;
                                                }
                                                else
                                                {
                                                    SubFolder5 = new TreeNode();
                                                    SubFolder5.Text = list[SplitNodeCount].ToString();
                                                    SubFolder5.Target = "SubFolder";
                                                    SubFolder5.Value = documents.Tables[0].Rows[k]["ActualDocumentUID"].ToString();
                                                    SubFolder5.ToolTip = documents.Tables[0].Rows[k]["ActualDocument_RelativePath"].ToString();// list[SplitNodeCount].ToString();
                                                    SubFolder4.ChildNodes.Add(SubFolder5);
                                                }
                                            }
                                            SplitNodeCount = SplitNodeCount + 1;

                                            if (SplitNodeCount < list.Count)
                                            {

                                                TreeNode trnode = SearchNode(SubFolder5, list[SplitNodeCount].ToString());
                                                if (trnode != null)
                                                {
                                                    SubFolder6 = trnode;
                                                }
                                                else
                                                {
                                                    SubFolder6 = new TreeNode();
                                                    SubFolder6.Text = list[SplitNodeCount].ToString();
                                                    SubFolder6.Target = "SubFolder";
                                                    SubFolder6.Value = documents.Tables[0].Rows[k]["ActualDocumentUID"].ToString();
                                                    SubFolder6.ToolTip = documents.Tables[0].Rows[k]["ActualDocument_RelativePath"].ToString();// list[SplitNodeCount].ToString();
                                                    SubFolder5.ChildNodes.Add(SubFolder6);
                                                }
                                            }

                                            SplitNodeCount = SplitNodeCount + 1;

                                            if (SplitNodeCount < list.Count)
                                            {

                                                TreeNode trnode = SearchNode(SubFolder6, list[SplitNodeCount].ToString());
                                                if (trnode != null)
                                                {
                                                    SubFolder7 = trnode;
                                                }
                                                else
                                                {
                                                    SubFolder7 = new TreeNode();
                                                    SubFolder7.Text = list[SplitNodeCount].ToString();
                                                    SubFolder7.Target = "SubFolder";
                                                    SubFolder7.Value = documents.Tables[0].Rows[k]["ActualDocumentUID"].ToString();
                                                    SubFolder7.ToolTip = documents.Tables[0].Rows[k]["ActualDocument_RelativePath"].ToString();// list[SplitNodeCount].ToString();
                                                    SubFolder6.ChildNodes.Add(SubFolder7);
                                                }
                                            }

                                        }
                                    }
                                }
                            }

                        }

                        //DataSet dschild = getdata.GetTasksForWorkPackages(child.Parent.Value);
                        DataSet dschild = getdata.GetTasks_by_WorkpackageOptionUID(new Guid(child.Parent.Value), new Guid(child.Value));
                        //DataTable dtChild = TreeViewBAL.BL.TreeViewBL.GetData("Select ID,Name from Module where ProjID=" + child.Value);
                        //DataSet dschild=dbgetdata.
                        if (dschild.Tables[0].Rows.Count > 0)
                        {
                            PopulateTreeView(dschild, child, child.Value, 4);
                        }

                    }
                    else if (Level == 4)
                    {
                        treeNode.ChildNodes.Add(child);
                        DataSet dssubmittal = getdata.getDocumentsForTasks(new Guid(child.Value));
                        if (dssubmittal.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < dssubmittal.Tables[0].Rows.Count; i++)
                            {
                                TreeNode subdoc = new TreeNode
                                {
                                    Text = "<div style='color:#007bff;'>" + dssubmittal.Tables[0].Rows[i]["DocName"].ToString() + "</div>",
                                    Value = dssubmittal.Tables[0].Rows[i]["DocumentUID"].ToString(),
                                    Target = "Submittal",
                                    ToolTip = dssubmittal.Tables[0].Rows[i]["DocName"].ToString(),
                                    ImageUrl = "~/_assets/images/submittal.png"
                                };
                                child.ChildNodes.Add(subdoc);

                                int SplitNodeCount = 0;
                                DataSet documents = getdata.ActualDocuments_SelectBy_DocumentUID(new Guid(dssubmittal.Tables[0].Rows[i]["DocumentUID"].ToString()));
                                for (int k = 0; k < documents.Tables[0].Rows.Count; k++)
                                {
                                    if (documents.Tables[0].Rows[k]["ActualDocument_RelativePath"].ToString() != "")
                                    {
                                        SplitNodeCount = 0;
                                        string[] relativepath = documents.Tables[0].Rows[k]["ActualDocument_RelativePath"].ToString().Split('/');
                                        if (relativepath.Length > 0)
                                        {
                                            List<string> list = new List<string>(relativepath);
                                            TreeNode SubFolder = new TreeNode();
                                            TreeNode SubFolder1 = new TreeNode();
                                            TreeNode SubFolder2 = new TreeNode();
                                            TreeNode SubFolder3 = new TreeNode();
                                            TreeNode SubFolder4 = new TreeNode();
                                            TreeNode SubFolder5 = new TreeNode();
                                            TreeNode SubFolder6 = new TreeNode();
                                            TreeNode SubFolder7 = new TreeNode();
                                            if (SplitNodeCount < list.Count)
                                            {

                                                //if (!CheckChildNode(subdoc, list[SplitNodeCount].ToString()))
                                                //{
                                                //    
                                                //}

                                                TreeNode trnode = SearchNode(subdoc, list[SplitNodeCount].ToString());
                                                if (trnode != null)
                                                {
                                                    SubFolder = trnode;
                                                }
                                                else
                                                {
                                                    SubFolder = new TreeNode();
                                                    SubFolder.Text = list[SplitNodeCount].ToString();
                                                    SubFolder.Target = "Folder";
                                                    SubFolder.Value = documents.Tables[0].Rows[k]["ActualDocumentUID"].ToString();
                                                    SubFolder.ToolTip = documents.Tables[0].Rows[k]["ActualDocument_RelativePath"].ToString();// list[SplitNodeCount].ToString();

                                                    subdoc.ChildNodes.Add(SubFolder);
                                                }

                                            }
                                            SplitNodeCount = SplitNodeCount + 1;

                                            if (SplitNodeCount < list.Count)
                                            {


                                                TreeNode trnode1 = SearchNode(SubFolder, list[SplitNodeCount].ToString());
                                                if (trnode1 != null)
                                                {
                                                    SubFolder1 = trnode1;
                                                }
                                                else
                                                {
                                                    SubFolder1 = new TreeNode();
                                                    SubFolder1.Text = list[SplitNodeCount].ToString();
                                                    SubFolder1.Target = "SubFolder";
                                                    SubFolder1.Value = documents.Tables[0].Rows[k]["ActualDocumentUID"].ToString();
                                                    SubFolder1.ToolTip = documents.Tables[0].Rows[k]["ActualDocument_RelativePath"].ToString();// list[SplitNodeCount].ToString();
                                                    SubFolder.ChildNodes.Add(SubFolder1);
                                                }
                                            }
                                            SplitNodeCount = SplitNodeCount + 1;

                                            if (SplitNodeCount < list.Count)
                                            {

                                                TreeNode trnode2 = SearchNode(SubFolder1, list[SplitNodeCount].ToString());
                                                if (trnode2 != null)
                                                {
                                                    SubFolder2 = trnode2;
                                                }
                                                else
                                                {
                                                    SubFolder2 = new TreeNode();
                                                    SubFolder2.Text = list[SplitNodeCount].ToString();
                                                    SubFolder2.Target = "SubFolder";
                                                    SubFolder2.Value = documents.Tables[0].Rows[k]["ActualDocumentUID"].ToString();
                                                    SubFolder2.ToolTip = documents.Tables[0].Rows[k]["ActualDocument_RelativePath"].ToString();// list[SplitNodeCount].ToString();
                                                    SubFolder1.ChildNodes.Add(SubFolder2);
                                                }


                                            }
                                            SplitNodeCount = SplitNodeCount + 1;

                                            if (SplitNodeCount < list.Count)
                                            {

                                                TreeNode trnode3 = SearchNode(SubFolder2, list[SplitNodeCount].ToString());
                                                if (trnode3 != null)
                                                {
                                                    SubFolder3 = trnode3;
                                                }
                                                else
                                                {
                                                    SubFolder3 = new TreeNode();
                                                    SubFolder3.Text = list[SplitNodeCount].ToString();
                                                    SubFolder3.Target = "SubFolder";
                                                    SubFolder3.Value = documents.Tables[0].Rows[k]["ActualDocumentUID"].ToString();
                                                    SubFolder3.ToolTip = documents.Tables[0].Rows[k]["ActualDocument_RelativePath"].ToString();// list[SplitNodeCount].ToString();
                                                    SubFolder2.ChildNodes.Add(SubFolder3);
                                                }


                                            }
                                            SplitNodeCount = SplitNodeCount + 1;

                                            if (SplitNodeCount < list.Count)
                                            {

                                                TreeNode trnode4 = SearchNode(SubFolder3, list[SplitNodeCount].ToString());
                                                if (trnode4 != null)
                                                {
                                                    SubFolder4 = trnode4;
                                                }
                                                else
                                                {
                                                    SubFolder4 = new TreeNode();
                                                    SubFolder4.Text = list[SplitNodeCount].ToString();
                                                    SubFolder4.Target = "SubFolder";
                                                    SubFolder4.Value = documents.Tables[0].Rows[k]["ActualDocumentUID"].ToString();
                                                    SubFolder4.ToolTip = documents.Tables[0].Rows[k]["ActualDocument_RelativePath"].ToString();// list[SplitNodeCount].ToString();
                                                    SubFolder3.ChildNodes.Add(SubFolder4);
                                                }


                                            }
                                            SplitNodeCount = SplitNodeCount + 1;

                                            if (SplitNodeCount < list.Count)
                                            {

                                                TreeNode trnode = SearchNode(SubFolder4, list[SplitNodeCount].ToString());
                                                if (trnode != null)
                                                {
                                                    SubFolder5 = trnode;
                                                }
                                                else
                                                {
                                                    SubFolder5 = new TreeNode();
                                                    SubFolder5.Text = list[SplitNodeCount].ToString();
                                                    SubFolder5.Target = "SubFolder";
                                                    SubFolder5.Value = documents.Tables[0].Rows[k]["ActualDocumentUID"].ToString();
                                                    SubFolder5.ToolTip = documents.Tables[0].Rows[k]["ActualDocument_RelativePath"].ToString();// list[SplitNodeCount].ToString();
                                                    SubFolder4.ChildNodes.Add(SubFolder5);
                                                }
                                            }
                                            SplitNodeCount = SplitNodeCount + 1;

                                            if (SplitNodeCount < list.Count)
                                            {

                                                TreeNode trnode = SearchNode(SubFolder5, list[SplitNodeCount].ToString());
                                                if (trnode != null)
                                                {
                                                    SubFolder6 = trnode;
                                                }
                                                else
                                                {
                                                    SubFolder6 = new TreeNode();
                                                    SubFolder6.Text = list[SplitNodeCount].ToString();
                                                    SubFolder6.Target = "SubFolder";
                                                    SubFolder6.Value = documents.Tables[0].Rows[k]["ActualDocumentUID"].ToString();
                                                    SubFolder6.ToolTip = documents.Tables[0].Rows[k]["ActualDocument_RelativePath"].ToString();// list[SplitNodeCount].ToString();
                                                    SubFolder5.ChildNodes.Add(SubFolder6);
                                                }
                                            }

                                            SplitNodeCount = SplitNodeCount + 1;

                                            if (SplitNodeCount < list.Count)
                                            {

                                                TreeNode trnode = SearchNode(SubFolder6, list[SplitNodeCount].ToString());
                                                if (trnode != null)
                                                {
                                                    SubFolder7 = trnode;
                                                }
                                                else
                                                {
                                                    SubFolder7 = new TreeNode();
                                                    SubFolder7.Text = list[SplitNodeCount].ToString();
                                                    SubFolder7.Target = "SubFolder";
                                                    SubFolder7.Value = documents.Tables[0].Rows[k]["ActualDocumentUID"].ToString();
                                                    SubFolder7.ToolTip = documents.Tables[0].Rows[k]["ActualDocument_RelativePath"].ToString();// list[SplitNodeCount].ToString();
                                                    SubFolder6.ChildNodes.Add(SubFolder7);
                                                }
                                            }

                                        }
                                    }
                                }
                            }

                        }

                        DataSet dssubchild = getdata.GetSubTasksForWorkPackages(child.Value);
                        if (dssubchild.Tables[0].Rows.Count > 0)
                        {
                            PopulateTreeView(dssubchild, child, child.Value, 5);
                        }

                        //DataSet ds = getdata.getDocumentsForTasks(new Guid(row["TaskUID"].ToString()));
                        //if (ds.Tables[0].Rows.Count > 0)
                        //{
                        //    TreeNode Doc = new TreeNode
                        //    {
                        //        Text = "Documents",
                        //        Value = row["TaskUID"].ToString(),
                        //        Target = "Document",
                        //        ToolTip = "Documents",
                        //        SelectAction = TreeNodeSelectAction.None
                        //    };
                        //    child.ChildNodes.Add(Doc);

                        //    DataSet cat = getdata.WorkPackageCategory_Selectby_WorkPackageUID(new Guid(child.Parent.Value));
                        //    if (cat.Tables[0].Rows.Count > 0)
                        //    {
                        //        for (int i = 0; i < cat.Tables[0].Rows.Count; i++)
                        //        {
                        //            TreeNode cate = new TreeNode
                        //            {
                        //                Text = cat.Tables[0].Rows[i]["WorkPackageCategory_Name"].ToString(),
                        //                Value = cat.Tables[0].Rows[i]["WorkPackageCategory_UID"].ToString(),
                        //                Target = "Category",
                        //                ToolTip = cat.Tables[0].Rows[i]["WorkPackageCategory_Name"].ToString()
                        //            };
                        //            Doc.ChildNodes.Add(cate);
                        //        }

                        //    }
                        //}


                    }
                    //else if (Level == 4)
                    //{
                    //    treeNode.ChildNodes.Add(child);
                    //    DataSet dssubmittal = getdata.getDocumentsForTasks(new Guid(child.Value));
                    //    if (dssubmittal.Tables[0].Rows.Count > 0)
                    //    {
                    //        for (int i = 0; i < dssubmittal.Tables[0].Rows.Count; i++)
                    //        {
                    //            TreeNode sub = new TreeNode
                    //            {
                    //                Text = dssubmittal.Tables[0].Rows[i]["DocName"].ToString(),
                    //                Value = dssubmittal.Tables[0].Rows[i]["DocumentUID"].ToString(),
                    //                Target = "Submittal",
                    //                ToolTip = dssubmittal.Tables[0].Rows[i]["DocName"].ToString()
                    //            };
                    //            child.ChildNodes.Add(sub);
                    //        }

                    //    }
                    //    //DataSet ds = getdata.getDocumentsForTasks(new Guid(row["TaskUID"].ToString()));
                    //    //if (ds.Tables[0].Rows.Count > 0)
                    //    //{
                    //    //    TreeNode Doc = new TreeNode
                    //    //    {
                    //    //        Text = "Documents",
                    //    //        Value = row["TaskUID"].ToString(),
                    //    //        Target = "Document",
                    //    //        ToolTip = "Documents",
                    //    //        SelectAction = TreeNodeSelectAction.None
                    //    //    };
                    //    //    child.ChildNodes.Add(Doc);

                    //    //    DataSet cat = getdata.WorkPackageCategory_Selectby_WorkPackageUID(new Guid(child.Parent.Parent.Value));
                    //    //    if (cat.Tables[0].Rows.Count > 0)
                    //    //    {
                    //    //        for (int i = 0; i < cat.Tables[0].Rows.Count; i++)
                    //    //        {
                    //    //            TreeNode cate = new TreeNode
                    //    //            {
                    //    //                Text = cat.Tables[0].Rows[i]["WorkPackageCategory_Name"].ToString(),
                    //    //                Value = cat.Tables[0].Rows[i]["WorkPackageCategory_UID"].ToString(),
                    //    //                Target = "Category",
                    //    //                ToolTip = cat.Tables[0].Rows[i]["WorkPackageCategory_Name"].ToString()
                    //    //            };
                    //    //            Doc.ChildNodes.Add(cate);
                    //    //        }

                    //    //    }
                    //    //}
                    //    DataSet dssubtosubchild = getdata.GetSubtoSubTasksForWorkPackages(child.Value);
                    //    if (dssubtosubchild.Tables[0].Rows.Count > 0)
                    //    {
                    //        PopulateTreeView(dssubtosubchild, child, child.Value, 5);
                    //    }
                    //}
                    //else if (Level == 5)
                    //{
                    //    treeNode.ChildNodes.Add(child);
                    //    DataSet dssubmittal = getdata.getDocumentsForTasks(new Guid(child.Value));
                    //    if (dssubmittal.Tables[0].Rows.Count > 0)
                    //    {
                    //        for (int i = 0; i < dssubmittal.Tables[0].Rows.Count; i++)
                    //        {
                    //            TreeNode sub = new TreeNode
                    //            {
                    //                Text = dssubmittal.Tables[0].Rows[i]["DocName"].ToString(),
                    //                Value = dssubmittal.Tables[0].Rows[i]["DocumentUID"].ToString(),
                    //                Target = "Submittal",
                    //                ToolTip = dssubmittal.Tables[0].Rows[i]["DocName"].ToString()
                    //            };
                    //            child.ChildNodes.Add(sub);
                    //        }

                    //    }
                    //    //DataSet ds = getdata.getDocumentsForTasks(new Guid(row["TaskUID"].ToString()));
                    //    //if (ds.Tables[0].Rows.Count > 0)
                    //    //{
                    //    //    TreeNode Doc = new TreeNode
                    //    //    {
                    //    //        Text = "Documents",
                    //    //        Value = row["TaskUID"].ToString(),
                    //    //        Target = "Document",
                    //    //        ToolTip = "Documents",
                    //    //        SelectAction = TreeNodeSelectAction.None
                    //    //    };
                    //    //    child.ChildNodes.Add(Doc);

                    //    //    DataSet cat = getdata.WorkPackageCategory_Selectby_WorkPackageUID(new Guid(child.Parent.Parent.Parent.Value));
                    //    //    if (cat.Tables[0].Rows.Count > 0)
                    //    //    {
                    //    //        for (int i = 0; i < cat.Tables[0].Rows.Count; i++)
                    //    //        {
                    //    //            TreeNode cate = new TreeNode
                    //    //            {
                    //    //                Text = cat.Tables[0].Rows[i]["WorkPackageCategory_Name"].ToString(),
                    //    //                Value = cat.Tables[0].Rows[i]["WorkPackageCategory_UID"].ToString(),
                    //    //                Target = "Category",
                    //    //                ToolTip = cat.Tables[0].Rows[i]["WorkPackageCategory_Name"].ToString()
                    //    //            };
                    //    //            Doc.ChildNodes.Add(cate);
                    //    //        }

                    //    //    }
                    //    //}
                    //    DataSet lastchild = getdata.GetSubtoSubtoSubTasksForWorkPackages(child.Value);
                    //    if (lastchild.Tables[0].Rows.Count > 0)
                    //    {
                    //        PopulateTreeView(lastchild, child, child.Value, 6);
                    //    }
                    //}
                    //else if (Level == 6)
                    //{
                    //    treeNode.ChildNodes.Add(child);
                    //    DataSet dssubmittal = getdata.getDocumentsForTasks(new Guid(child.Value));
                    //    if (dssubmittal.Tables[0].Rows.Count > 0)
                    //    {
                    //        for (int i = 0; i < dssubmittal.Tables[0].Rows.Count; i++)
                    //        {
                    //            TreeNode sub = new TreeNode
                    //            {
                    //                Text = dssubmittal.Tables[0].Rows[i]["DocName"].ToString(),
                    //                Value = dssubmittal.Tables[0].Rows[i]["DocumentUID"].ToString(),
                    //                Target = "Submittal",
                    //                ToolTip = dssubmittal.Tables[0].Rows[i]["DocName"].ToString()
                    //            };
                    //            child.ChildNodes.Add(sub);
                    //        }

                    //    }
                    //    //DataSet ds = getdata.getDocumentsForTasks(new Guid(row["TaskUID"].ToString()));
                    //    //if (ds.Tables[0].Rows.Count > 0)
                    //    //{
                    //    //    TreeNode Doc = new TreeNode
                    //    //    {
                    //    //        Text = "Documents",
                    //    //        Value = row["TaskUID"].ToString(),
                    //    //        Target = "Document",
                    //    //        ToolTip = "Documents",
                    //    //        SelectAction = TreeNodeSelectAction.None
                    //    //    };
                    //    //    child.ChildNodes.Add(Doc);

                    //    //    DataSet cat = getdata.WorkPackageCategory_Selectby_WorkPackageUID(new Guid(child.Parent.Parent.Parent.Parent.Value));
                    //    //    if (cat.Tables[0].Rows.Count > 0)
                    //    //    {
                    //    //        for (int i = 0; i < cat.Tables[0].Rows.Count; i++)
                    //    //        {
                    //    //            TreeNode cate = new TreeNode
                    //    //            {
                    //    //                Text = cat.Tables[0].Rows[i]["WorkPackageCategory_Name"].ToString(),
                    //    //                Value = cat.Tables[0].Rows[i]["WorkPackageCategory_UID"].ToString(),
                    //    //                Target = "Category",
                    //    //                ToolTip = cat.Tables[0].Rows[i]["WorkPackageCategory_Name"].ToString()
                    //    //            };
                    //    //            Doc.ChildNodes.Add(cate);
                    //    //        }

                    //    //    }
                    //    //}
                    //    DataSet lastchild = getdata.GetSubtoSubtoSubtoSubTasksForWorkPackages(child.Value);
                    //    if (lastchild.Tables[0].Rows.Count > 0)
                    //    {
                    //        PopulateTreeView(lastchild, child, child.Value, 7);
                    //    }
                    //}
                    else
                    {
                        treeNode.ChildNodes.Add(child);
                        DataSet dssubmittal = getdata.getDocumentsForTasks(new Guid(child.Value));
                        if (dssubmittal.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < dssubmittal.Tables[0].Rows.Count; i++)
                            {
                                TreeNode subdoc = new TreeNode
                                {
                                    Text = "<div style='color:#007bff;'>" + dssubmittal.Tables[0].Rows[i]["DocName"].ToString() + "</div>",
                                    Value = dssubmittal.Tables[0].Rows[i]["DocumentUID"].ToString(),
                                    Target = "Submittal",
                                    ToolTip = dssubmittal.Tables[0].Rows[i]["DocName"].ToString(),
                                    ImageUrl = "~/_assets/images/submittal.png"
                                };
                                child.ChildNodes.Add(subdoc);

                                int SplitNodeCount = 0;
                                DataSet documents = getdata.ActualDocuments_SelectBy_DocumentUID(new Guid(dssubmittal.Tables[0].Rows[i]["DocumentUID"].ToString()));
                                for (int k = 0; k < documents.Tables[0].Rows.Count; k++)
                                {
                                    if (documents.Tables[0].Rows[k]["ActualDocument_RelativePath"].ToString() != "")
                                    {
                                        SplitNodeCount = 0;
                                        string[] relativepath = documents.Tables[0].Rows[k]["ActualDocument_RelativePath"].ToString().Split('/');
                                        if (relativepath.Length > 0)
                                        {
                                            List<string> list = new List<string>(relativepath);
                                            TreeNode SubFolder = new TreeNode();
                                            TreeNode SubFolder1 = new TreeNode();
                                            TreeNode SubFolder2 = new TreeNode();
                                            TreeNode SubFolder3 = new TreeNode();
                                            TreeNode SubFolder4 = new TreeNode();
                                            TreeNode SubFolder5 = new TreeNode();
                                            TreeNode SubFolder6 = new TreeNode();
                                            TreeNode SubFolder7 = new TreeNode();
                                            if (SplitNodeCount < list.Count)
                                            {

                                                //if (!CheckChildNode(subdoc, list[SplitNodeCount].ToString()))
                                                //{
                                                //    
                                                //}

                                                TreeNode trnode = SearchNode(subdoc, list[SplitNodeCount].ToString());
                                                if (trnode != null)
                                                {
                                                    SubFolder = trnode;
                                                }
                                                else
                                                {
                                                    SubFolder = new TreeNode();
                                                    SubFolder.Text = list[SplitNodeCount].ToString();
                                                    SubFolder.Target = "Folder";
                                                    SubFolder.Value = documents.Tables[0].Rows[k]["ActualDocumentUID"].ToString();
                                                    SubFolder.ToolTip = documents.Tables[0].Rows[k]["ActualDocument_RelativePath"].ToString();// list[SplitNodeCount].ToString();

                                                    subdoc.ChildNodes.Add(SubFolder);
                                                }

                                            }
                                            SplitNodeCount = SplitNodeCount + 1;

                                            if (SplitNodeCount < list.Count)
                                            {


                                                TreeNode trnode1 = SearchNode(SubFolder, list[SplitNodeCount].ToString());
                                                if (trnode1 != null)
                                                {
                                                    SubFolder1 = trnode1;
                                                }
                                                else
                                                {
                                                    SubFolder1 = new TreeNode();
                                                    SubFolder1.Text = list[SplitNodeCount].ToString();
                                                    SubFolder1.Target = "SubFolder";
                                                    SubFolder1.Value = documents.Tables[0].Rows[k]["ActualDocumentUID"].ToString();
                                                    SubFolder1.ToolTip = documents.Tables[0].Rows[k]["ActualDocument_RelativePath"].ToString();// list[SplitNodeCount].ToString();
                                                    SubFolder.ChildNodes.Add(SubFolder1);
                                                }
                                            }
                                            SplitNodeCount = SplitNodeCount + 1;

                                            if (SplitNodeCount < list.Count)
                                            {

                                                TreeNode trnode2 = SearchNode(SubFolder1, list[SplitNodeCount].ToString());
                                                if (trnode2 != null)
                                                {
                                                    SubFolder2 = trnode2;
                                                }
                                                else
                                                {
                                                    SubFolder2 = new TreeNode();
                                                    SubFolder2.Text = list[SplitNodeCount].ToString();
                                                    SubFolder2.Target = "SubFolder";
                                                    SubFolder2.Value = documents.Tables[0].Rows[k]["ActualDocumentUID"].ToString();
                                                    SubFolder2.ToolTip = documents.Tables[0].Rows[k]["ActualDocument_RelativePath"].ToString();// list[SplitNodeCount].ToString();
                                                    SubFolder1.ChildNodes.Add(SubFolder2);
                                                }


                                            }
                                            SplitNodeCount = SplitNodeCount + 1;

                                            if (SplitNodeCount < list.Count)
                                            {

                                                TreeNode trnode3 = SearchNode(SubFolder2, list[SplitNodeCount].ToString());
                                                if (trnode3 != null)
                                                {
                                                    SubFolder3 = trnode3;
                                                }
                                                else
                                                {
                                                    SubFolder3 = new TreeNode();
                                                    SubFolder3.Text = list[SplitNodeCount].ToString();
                                                    SubFolder3.Target = "SubFolder";
                                                    SubFolder3.Value = documents.Tables[0].Rows[k]["ActualDocumentUID"].ToString();
                                                    SubFolder3.ToolTip = documents.Tables[0].Rows[k]["ActualDocument_RelativePath"].ToString();// list[SplitNodeCount].ToString();
                                                    SubFolder2.ChildNodes.Add(SubFolder3);
                                                }


                                            }
                                            SplitNodeCount = SplitNodeCount + 1;

                                            if (SplitNodeCount < list.Count)
                                            {

                                                TreeNode trnode4 = SearchNode(SubFolder3, list[SplitNodeCount].ToString());
                                                if (trnode4 != null)
                                                {
                                                    SubFolder4 = trnode4;
                                                }
                                                else
                                                {
                                                    SubFolder4 = new TreeNode();
                                                    SubFolder4.Text = list[SplitNodeCount].ToString();
                                                    SubFolder4.Target = "SubFolder";
                                                    SubFolder4.Value = documents.Tables[0].Rows[k]["ActualDocumentUID"].ToString();
                                                    SubFolder4.ToolTip = documents.Tables[0].Rows[k]["ActualDocument_RelativePath"].ToString();// list[SplitNodeCount].ToString();
                                                    SubFolder3.ChildNodes.Add(SubFolder4);
                                                }


                                            }
                                            SplitNodeCount = SplitNodeCount + 1;

                                            if (SplitNodeCount < list.Count)
                                            {

                                                TreeNode trnode = SearchNode(SubFolder4, list[SplitNodeCount].ToString());
                                                if (trnode != null)
                                                {
                                                    SubFolder5 = trnode;
                                                }
                                                else
                                                {
                                                    SubFolder5 = new TreeNode();
                                                    SubFolder5.Text = list[SplitNodeCount].ToString();
                                                    SubFolder5.Target = "SubFolder";
                                                    SubFolder5.Value = documents.Tables[0].Rows[k]["ActualDocumentUID"].ToString();
                                                    SubFolder5.ToolTip = documents.Tables[0].Rows[k]["ActualDocument_RelativePath"].ToString();// list[SplitNodeCount].ToString();
                                                    SubFolder4.ChildNodes.Add(SubFolder5);
                                                }
                                            }
                                            SplitNodeCount = SplitNodeCount + 1;

                                            if (SplitNodeCount < list.Count)
                                            {

                                                TreeNode trnode = SearchNode(SubFolder5, list[SplitNodeCount].ToString());
                                                if (trnode != null)
                                                {
                                                    SubFolder6 = trnode;
                                                }
                                                else
                                                {
                                                    SubFolder6 = new TreeNode();
                                                    SubFolder6.Text = list[SplitNodeCount].ToString();
                                                    SubFolder6.Target = "SubFolder";
                                                    SubFolder6.Value = documents.Tables[0].Rows[k]["ActualDocumentUID"].ToString();
                                                    SubFolder6.ToolTip = documents.Tables[0].Rows[k]["ActualDocument_RelativePath"].ToString();// list[SplitNodeCount].ToString();
                                                    SubFolder5.ChildNodes.Add(SubFolder6);
                                                }
                                            }

                                            SplitNodeCount = SplitNodeCount + 1;

                                            if (SplitNodeCount < list.Count)
                                            {

                                                TreeNode trnode = SearchNode(SubFolder6, list[SplitNodeCount].ToString());
                                                if (trnode != null)
                                                {
                                                    SubFolder7 = trnode;
                                                }
                                                else
                                                {
                                                    SubFolder7 = new TreeNode();
                                                    SubFolder7.Text = list[SplitNodeCount].ToString();
                                                    SubFolder7.Target = "SubFolder";
                                                    SubFolder7.Value = documents.Tables[0].Rows[k]["ActualDocumentUID"].ToString();
                                                    SubFolder7.ToolTip = documents.Tables[0].Rows[k]["ActualDocument_RelativePath"].ToString();// list[SplitNodeCount].ToString();
                                                    SubFolder6.ChildNodes.Add(SubFolder7);
                                                }
                                            }

                                        }
                                    }
                                }
                            }

                        }
                        //DataSet ds = getdata.getDocumentsForTasks(new Guid(row["TaskUID"].ToString()));
                        //if (ds.Tables[0].Rows.Count > 0)
                        //{
                        //    TreeNode Doc = new TreeNode
                        //    {
                        //        Text = "Documents",
                        //        Value = row["TaskUID"].ToString(),
                        //        Target = "Document",
                        //        ToolTip = "Documents",
                        //        SelectAction = TreeNodeSelectAction.None
                        //    };
                        //    child.ChildNodes.Add(Doc);

                        //    DataSet cat = getdata.WorkPackageCategory_Selectby_WorkPackageUID(new Guid(child.Parent.Parent.Parent.Parent.Parent.Value));
                        //    if (cat.Tables[0].Rows.Count > 0)
                        //    {
                        //        for (int i = 0; i < cat.Tables[0].Rows.Count; i++)
                        //        {
                        //            TreeNode cate = new TreeNode
                        //            {
                        //                Text = cat.Tables[0].Rows[i]["WorkPackageCategory_Name"].ToString(),
                        //                Value = cat.Tables[0].Rows[i]["WorkPackageCategory_UID"].ToString(),
                        //                Target = "Category",
                        //                ToolTip = cat.Tables[0].Rows[i]["WorkPackageCategory_Name"].ToString()
                        //            };
                        //            Doc.ChildNodes.Add(cate);
                        //        }

                        //    }
                        //}
                        DataSet dsfinal = getdata.GetTask_by_ParentTaskUID(new Guid(child.Value));
                        if (dsfinal.Tables[0].Rows.Count > 0)
                        {
                            PopulateTreeView(dsfinal, child, child.Value, 7);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        //public void PopulateTreeView_by_Category(DataSet dtParent, TreeNode treeNode, string ParentUID, int Level)
        //{
        //    foreach (DataRow row in dtParent.Tables[0].Rows)
        //    {
        //        TreeNode child = new TreeNode
        //        {
        //            Text = Level == 0 ? row["ProjectClass_Name"].ToString() : Level == 1 ? LimitCharts(getdata.getProjectNameby_ProjectUID(new Guid(row["ProjectUID"].ToString()))) : Level == 2 ? LimitCharts(row["Name"].ToString()) : LimitCharts(row["Name"].ToString()),
        //            Value = Level == 0 ? row["ProjectClass_UID"].ToString() : Level == 1 ? row["ProjectUID"].ToString() : Level == 2 ? row["WorkPackageUID"].ToString() : row["TaskUID"].ToString(),
        //            Target = Level == 0 ? "Class" : Level == 1 ? "Project" : Level == 2 ? "WorkPackage" : "Category",
        //            ToolTip = Level == 0 ? row["ProjectClass_Name"].ToString() : Level == 1 ? getdata.getProjectNameby_ProjectUID(new Guid(row["ProjectUID"].ToString())) : Level == 2 ? row["Name"].ToString() : row["Name"].ToString()
        //        };

        //        if (ParentUID == "")
        //        {
        //            TreeView2.Nodes.Add(child);
        //            DataSet dsProject = new DataSet();
        //            if (Session["TypeOfUser"].ToString() == "U" || Session["TypeOfUser"].ToString() == "MD" || Session["TypeOfUser"].ToString() == "VP")
        //            {
        //                dsProject = gettk.GetProjects_by_ClassUID(new Guid(child.Value));
        //            }
        //            else
        //            {
        //                dsProject = gettk.GetUserProjects_by_ClassUID(new Guid(child.Value), new Guid(Session["UserUID"].ToString()));
        //            }
        //            //DataSet dsProject = gettk.GetUserProjects_by_ClassUID(new Guid(child.Value), new Guid(Session["UserUID"].ToString()));
        //            if (dsProject.Tables[0].Rows.Count > 0)
        //            {
        //                PopulateTreeView_by_Category(dsProject, child, child.Value, 1);
        //            }
        //        }
        //        else if (Level == 1)
        //        {
        //            treeNode.ChildNodes.Add(child);
        //            //DataSet dsworkPackage = getdata.GetWorkPackages_By_ProjectUID(new Guid(child.Value));
        //            DataSet dsworkPackage = new DataSet();
        //            if (Session["TypeOfUser"].ToString() == "U" || Session["TypeOfUser"].ToString() == "MD" || Session["TypeOfUser"].ToString() == "VP")
        //            {
        //                dsworkPackage = getdata.GetWorkPackages_By_ProjectUID(new Guid(child.Value));
        //            }
        //            else if (Session["TypeOfUser"].ToString() == "PA")
        //            {
        //                dsworkPackage = getdata.GetWorkPackages_ForUser_by_ProjectUID(new Guid(Session["UserUID"].ToString()), new Guid(child.Value));
        //            }
        //            else
        //            {
        //                dsworkPackage = getdata.GetWorkPackages_ForUser_by_ProjectUID(new Guid(Session["UserUID"].ToString()), new Guid(child.Value));
        //            }
        //            if (dsworkPackage.Tables[0].Rows.Count > 0)
        //            {
        //                PopulateTreeView_by_Category(dsworkPackage, child, child.Value, 2);
        //            }
        //        }
        //        else if (Level == 2)
        //        {
        //            treeNode.ChildNodes.Add(child);
        //            //DataSet ds = getdata.GetDocuments_For_WorkPackage(new Guid(row["WorkPackageUID"].ToString()));
        //            //if (ds.Tables[0].Rows.Count > 0)
        //            //{
        //            DataSet cat = getdata.WorkPackageCategory_Selectby_WorkPackageUID(new Guid(row["WorkPackageUID"].ToString()));
        //            if (cat.Tables[0].Rows.Count > 0)
        //            {
        //                for (int i = 0; i < cat.Tables[0].Rows.Count; i++)
        //                {
        //                    TreeNode cate = new TreeNode
        //                    {
        //                        Text = cat.Tables[0].Rows[i]["WorkPackageCategory_Name"].ToString(),
        //                        Value = cat.Tables[0].Rows[i]["WorkPackageCategory_UID"].ToString(),
        //                        Target = "Category",
        //                        ToolTip = cat.Tables[0].Rows[i]["WorkPackageCategory_Name"].ToString()
        //                    };
        //                    child.ChildNodes.Add(cate);

        //                    DataSet submittal = getdata.GetSubmittals_For_Category(cat.Tables[0].Rows[i]["WorkPackageCategory_UID"].ToString());
        //                    if (submittal.Tables[0].Rows.Count > 0)
        //                    {
        //                        for (int j = 0; j < submittal.Tables[0].Rows.Count; j++)
        //                        {
        //                            TreeNode subdoc = new TreeNode
        //                            {
        //                                Text = "<div style='color:#007bff;'>" + submittal.Tables[0].Rows[j]["DocName"].ToString() + "</div>",
        //                                Value = submittal.Tables[0].Rows[j]["DocumentUID"].ToString(),
        //                                Target = "Submittal",
        //                                ToolTip = submittal.Tables[0].Rows[j]["DocName"].ToString(),
        //                                ImageUrl = "~/_assets/images/submittal.png"
        //                            };
        //                            cate.ChildNodes.Add(subdoc);
        //                            string Dir_Name = "";
        //                            int SplitNodeCount = 0;
        //                            DataSet documents = getdata.ActualDocuments_SelectBy_DocumentUID(new Guid(submittal.Tables[0].Rows[j]["DocumentUID"].ToString()));
        //                            for (int k = 0; k < documents.Tables[0].Rows.Count; k++)
        //                            {
        //                                if (documents.Tables[0].Rows[k]["ActualDocument_RelativePath"].ToString() != "")
        //                                {
        //                                    SplitNodeCount = 0;
        //                                    string[] relativepath = documents.Tables[0].Rows[k]["ActualDocument_RelativePath"].ToString().Split('/');
        //                                    if (relativepath.Length > 0)
        //                                    {
        //                                        List<string> list = new List<string>(relativepath);
        //                                        TreeNode SubFolder = new TreeNode();
        //                                        TreeNode SubFolder1 = new TreeNode();
        //                                        TreeNode SubFolder2 = new TreeNode();
        //                                        TreeNode SubFolder3 = new TreeNode();
        //                                        TreeNode SubFolder4 = new TreeNode();
        //                                        TreeNode SubFolder5 = new TreeNode();
        //                                        TreeNode SubFolder6 = new TreeNode();
        //                                        TreeNode SubFolder7 = new TreeNode();
        //                                        if (SplitNodeCount < list.Count)
        //                                        {

        //                                            //if (!CheckChildNode(subdoc, list[SplitNodeCount].ToString()))
        //                                            //{
        //                                            //    
        //                                            //}

        //                                            TreeNode trnode = SearchNode(subdoc, list[SplitNodeCount].ToString());
        //                                            if (trnode != null)
        //                                            {
        //                                                SubFolder = trnode;
        //                                            }
        //                                            else
        //                                            {
        //                                                SubFolder = new TreeNode();
        //                                                SubFolder.Text = list[SplitNodeCount].ToString();
        //                                                SubFolder.Target = "Folder";
        //                                                SubFolder.Value = documents.Tables[0].Rows[k]["ActualDocumentUID"].ToString();
        //                                                SubFolder.ToolTip = documents.Tables[0].Rows[k]["ActualDocument_RelativePath"].ToString();// list[SplitNodeCount].ToString();

        //                                                subdoc.ChildNodes.Add(SubFolder);
        //                                            }

        //                                        }
        //                                        SplitNodeCount = SplitNodeCount + 1;

        //                                        if (SplitNodeCount < list.Count)
        //                                        {


        //                                            TreeNode trnode1 = SearchNode(SubFolder, list[SplitNodeCount].ToString());
        //                                            if (trnode1 != null)
        //                                            {
        //                                                SubFolder1 = trnode1;
        //                                            }
        //                                            else
        //                                            {
        //                                                SubFolder1 = new TreeNode();
        //                                                SubFolder1.Text = list[SplitNodeCount].ToString();
        //                                                SubFolder1.Target = "SubFolder";
        //                                                SubFolder1.Value = documents.Tables[0].Rows[k]["ActualDocumentUID"].ToString();
        //                                                SubFolder1.ToolTip = documents.Tables[0].Rows[k]["ActualDocument_RelativePath"].ToString();// list[SplitNodeCount].ToString();
        //                                                SubFolder.ChildNodes.Add(SubFolder1);
        //                                            }
        //                                        }
        //                                        SplitNodeCount = SplitNodeCount + 1;

        //                                        if (SplitNodeCount < list.Count)
        //                                        {

        //                                            TreeNode trnode2 = SearchNode(SubFolder1, list[SplitNodeCount].ToString());
        //                                            if (trnode2 != null)
        //                                            {
        //                                                SubFolder2 = trnode2;
        //                                            }
        //                                            else
        //                                            {
        //                                                SubFolder2 = new TreeNode();
        //                                                SubFolder2.Text = list[SplitNodeCount].ToString();
        //                                                SubFolder2.Target = "SubFolder";
        //                                                SubFolder2.Value = documents.Tables[0].Rows[k]["ActualDocumentUID"].ToString();
        //                                                SubFolder2.ToolTip = documents.Tables[0].Rows[k]["ActualDocument_RelativePath"].ToString();// list[SplitNodeCount].ToString();
        //                                                SubFolder1.ChildNodes.Add(SubFolder2);
        //                                            }


        //                                        }
        //                                        SplitNodeCount = SplitNodeCount + 1;

        //                                        if (SplitNodeCount < list.Count)
        //                                        {

        //                                            TreeNode trnode3 = SearchNode(SubFolder2, list[SplitNodeCount].ToString());
        //                                            if (trnode3 != null)
        //                                            {
        //                                                SubFolder3 = trnode3;
        //                                            }
        //                                            else
        //                                            {
        //                                                SubFolder3 = new TreeNode();
        //                                                SubFolder3.Text = list[SplitNodeCount].ToString();
        //                                                SubFolder3.Target = "SubFolder";
        //                                                SubFolder3.Value = documents.Tables[0].Rows[k]["ActualDocumentUID"].ToString();
        //                                                SubFolder3.ToolTip = documents.Tables[0].Rows[k]["ActualDocument_RelativePath"].ToString();// list[SplitNodeCount].ToString();
        //                                                SubFolder2.ChildNodes.Add(SubFolder3);
        //                                            }


        //                                        }
        //                                        SplitNodeCount = SplitNodeCount + 1;

        //                                        if (SplitNodeCount < list.Count)
        //                                        {

        //                                            TreeNode trnode4 = SearchNode(SubFolder3, list[SplitNodeCount].ToString());
        //                                            if (trnode4 != null)
        //                                            {
        //                                                SubFolder4 = trnode4;
        //                                            }
        //                                            else
        //                                            {
        //                                                SubFolder4 = new TreeNode();
        //                                                SubFolder4.Text = list[SplitNodeCount].ToString();
        //                                                SubFolder4.Target = "SubFolder";
        //                                                SubFolder4.Value = documents.Tables[0].Rows[k]["ActualDocumentUID"].ToString();
        //                                                SubFolder4.ToolTip = documents.Tables[0].Rows[k]["ActualDocument_RelativePath"].ToString();// list[SplitNodeCount].ToString();
        //                                                SubFolder3.ChildNodes.Add(SubFolder4);
        //                                            }


        //                                        }
        //                                        SplitNodeCount = SplitNodeCount + 1;

        //                                        if (SplitNodeCount < list.Count)
        //                                        {

        //                                            TreeNode trnode = SearchNode(SubFolder4, list[SplitNodeCount].ToString());
        //                                            if (trnode != null)
        //                                            {
        //                                                SubFolder5 = trnode;
        //                                            }
        //                                            else
        //                                            {
        //                                                SubFolder5 = new TreeNode();
        //                                                SubFolder5.Text = list[SplitNodeCount].ToString();
        //                                                SubFolder5.Target = "SubFolder";
        //                                                SubFolder5.Value = documents.Tables[0].Rows[k]["ActualDocumentUID"].ToString();
        //                                                SubFolder5.ToolTip = documents.Tables[0].Rows[k]["ActualDocument_RelativePath"].ToString();// list[SplitNodeCount].ToString();
        //                                                SubFolder4.ChildNodes.Add(SubFolder5);
        //                                            }
        //                                        }
        //                                        SplitNodeCount = SplitNodeCount + 1;

        //                                        if (SplitNodeCount < list.Count)
        //                                        {

        //                                            TreeNode trnode = SearchNode(SubFolder5, list[SplitNodeCount].ToString());
        //                                            if (trnode != null)
        //                                            {
        //                                                SubFolder6 = trnode;
        //                                            }
        //                                            else
        //                                            {
        //                                                SubFolder6 = new TreeNode();
        //                                                SubFolder6.Text = list[SplitNodeCount].ToString();
        //                                                SubFolder6.Target = "SubFolder";
        //                                                SubFolder6.Value = documents.Tables[0].Rows[k]["ActualDocumentUID"].ToString();
        //                                                SubFolder6.ToolTip = documents.Tables[0].Rows[k]["ActualDocument_RelativePath"].ToString();// list[SplitNodeCount].ToString();
        //                                                SubFolder5.ChildNodes.Add(SubFolder6);
        //                                            }
        //                                        }

        //                                        SplitNodeCount = SplitNodeCount + 1;

        //                                        if (SplitNodeCount < list.Count)
        //                                        {

        //                                            TreeNode trnode = SearchNode(SubFolder6, list[SplitNodeCount].ToString());
        //                                            if (trnode != null)
        //                                            {
        //                                                SubFolder7 = trnode;
        //                                            }
        //                                            else
        //                                            {
        //                                                SubFolder7 = new TreeNode();
        //                                                SubFolder7.Text = list[SplitNodeCount].ToString();
        //                                                SubFolder7.Target = "SubFolder";
        //                                                SubFolder7.Value = documents.Tables[0].Rows[k]["ActualDocumentUID"].ToString();
        //                                                SubFolder7.ToolTip = documents.Tables[0].Rows[k]["ActualDocument_RelativePath"].ToString();// list[SplitNodeCount].ToString();
        //                                                SubFolder6.ChildNodes.Add(SubFolder7);
        //                                            }
        //                                        }

        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }
        //                }

        //            }
        //            //}   
        //        }
        //    }

        //}
        private bool CheckChildNode(TreeNode nodes, string FindText)
        {
            bool retVal = false;
            for (int i = 0; i < nodes.ChildNodes.Count; i++)
            {
                if (nodes.ChildNodes[i].ToString() == FindText)
                {
                    retVal = true;

                }
            }
            return retVal;
        }
        public void BindActivities()
        {

            
        }

        public void BindDocuments(string UID)
        {
            

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

        protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
        {
            LblShowMessage.Text = TreeView1.SelectedNode.Text;
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
                            tn.ExpandAll();
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

        //protected void retrieveNodesCategory(TreeNode node, string SelectedVal)
        //{
        //    if (node.Value == SelectedVal)
        //    {
        //        node.Selected = true;
        //        node.Expand();
        //        node.Parent.Expand();
        //        //BindActivities();
        //        BindDocuments_By_Category(TreeView2.SelectedNode.Value);
        //    }
        //    else
        //    {
        //        if (node.ChildNodes.Count != 0)
        //        {
        //            foreach (TreeNode tn in node.ChildNodes)
        //            {
        //                if (tn.Value == SelectedVal)
        //                {
        //                    tn.Selected = true;
        //                    tn.Expand();
        //                    tn.Parent.Expand();
        //                    //BindActivities();
        //                    if (tn.Parent.Parent != null)
        //                    {
        //                        tn.Parent.Parent.Expand();

        //                        if (tn.Parent.Parent.Parent != null)
        //                        {
        //                            tn.Parent.Parent.Parent.Expand();

        //                            if (tn.Parent.Parent.Parent.Parent != null)
        //                            {
        //                                tn.Parent.Parent.Parent.Parent.Expand();

        //                                if (tn.Parent.Parent.Parent.Parent.Parent != null)
        //                                {
        //                                    tn.Parent.Parent.Parent.Parent.Parent.Expand();

        //                                    if (tn.Parent.Parent.Parent.Parent.Parent.Parent != null)
        //                                    {
        //                                        tn.Parent.Parent.Parent.Parent.Parent.Parent.Expand();

        //                                        if (tn.Parent.Parent.Parent.Parent.Parent.Parent.Parent != null)
        //                                        {
        //                                            tn.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Expand();
        //                                        }
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }
        //                    BindDocuments_By_Category(TreeView2.SelectedNode.Value);
        //                }
        //                else
        //                {
        //                    if (tn.ChildNodes.Count != 0)
        //                    {
        //                        retrieveNodesCategory(tn, SelectedVal);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}

        

       

        

        
    }
}