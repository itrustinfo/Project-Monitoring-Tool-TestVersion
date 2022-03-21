using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace ProjectManager._content_pages.documents
{
    public partial class Default : System.Web.UI.Page
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
                ScriptManager.RegisterStartupScript(
                        UpdatePanel1,
                        this.GetType(),
                        "MyAction",
                        "BindEvents();",
                        true);
                if (!IsPostBack)
                {
                    BindProject();
                    SelectedProject();
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
                    GrdActualDocuments_new.Columns[7].Visible = false;
                    GrdDocumentByCategory.Columns[6].Visible = false;
                    GrdNewDocument.Columns[6].Visible = false;
                    //
                    GrdActualDocuments_new.Columns[6].Visible = false;
                    GrdDocumentByCategory.Columns[5].Visible = false;
                    GrdNewDocument.Columns[5].Visible = false;
                    //
                    GrdDocumentByCategory.Columns[4].Visible = false;
                    GrdNewDocument.Columns[4].Visible = false;
                    //
                    btnSubmittal.Visible = false;
                    btnaddsubmittalforcategory.Visible = false;
                    if (dscheck.Tables[0].Rows.Count > 0)
                    {
                       foreach(DataRow dr in dscheck.Tables[0].Rows)
                        {
                            if(dr["Code"].ToString() == "FP" || dr["Code"].ToString() == "FO") // Delete submittal and documents
                            {
                                ViewState["isDelete"] = "true";
                                //
                                GrdActualDocuments_new.Columns[7].Visible = true;
                                GrdDocumentByCategory.Columns[6].Visible = true;
                                GrdNewDocument.Columns[6].Visible = true;
                            }
                            if (dr["Code"].ToString() == "FH") // upload documents/ edit / add submittal
                            {
                                ViewState["isUpload"] = "true";
                                btnSubmittal.Visible = true;
                                btnaddsubmittalforcategory.Visible = true;
                                //
                                GrdActualDocuments_new.Columns[6].Visible = true;
                                GrdDocumentByCategory.Columns[5].Visible = true;
                                GrdNewDocument.Columns[5].Visible = true;

                                GrdDocumentByCategory.Columns[4].Visible = true;
                                GrdNewDocument.Columns[4].Visible = true;
                            }
                            if (dr["Code"].ToString() == "FU") // upload documents/ edit 
                            {
                                ViewState["isUpdateStatus"] = "true";
                            }
                            if (dr["Code"].ToString() == "FI") // View Documents
                            {
                                ViewState["isView"] = "true";
                            }
                            if (dr["Code"].ToString() == "FN") // DOWNLOAD APPROVED DOCUMENTS (FINAL APPROVED BY CLIENT)
                            {
                                ViewState["isDownloadClient"] = "true";
                                Session["isDownloadClient"] = "true";
                            }
                            if (dr["Code"].ToString() == "FM") // DOWNLOAD APPROVED DOCUMENTS (FINAL APPROVED BY NJSEI)
                            {
                                ViewState["isDownloadNJSE"] = "true";
                                Session["isDownloadNJSE"] = "true";
                            }
                        }
                    }
                    //----------------------------------------
                    if (Session["SelectedTaskUID"] != null)
                    {
                        //string ProjectUID = getdata.GetProjectUIDFromTaskUID(new Guid(Session["SelectedTaskUID"].ToString()));
                        //if (ProjectUID != "")
                        //{
                        //    DDlProject.SelectedValue = new Guid(ProjectUID).ToString();
                        //}

                        if (Session["ViewDocBy"] != null)
                        {
                            if (Session["ViewDocBy"].ToString() == "Activity")
                            {
                                //ByCategory.Visible = false;
                                //ByActivity.Visible = true;

                                RDBDocumentView.SelectedIndex = 0;
                                RDBDocumentView_SelectedIndexChanged(sender, e);
                            }
                            else
                            {
                                //ByCategory.Visible = true;
                                //ByActivity.Visible = false;
                                RDBDocumentView.SelectedIndex = 1;
                                RDBDocumentView_SelectedIndexChanged(sender, e);
                            }
                        }
                        else
                        {
                            RDBDocumentView.SelectedIndex = 0;
                            RDBDocumentView_SelectedIndexChanged(sender, e);
                        }

                    }
                    else if (Session["SelectedTaskUID1"] != null)
                    {
                        //string ProjectUID = getdata.GetProjectUIDFromSubmittalUID(new Guid(Session["SelectedTaskUID1"].ToString()));
                        //if (ProjectUID != "")
                        //{
                        //    DDlProject.SelectedValue = new Guid(ProjectUID).ToString();
                        //}
                        if (Session["ViewDocBy"] != null)
                        {
                            if (Session["ViewDocBy"].ToString() == "Activity")
                            {
                                //ByCategory.Visible = false;
                                //ByActivity.Visible = true;
                                RDBDocumentView.SelectedIndex = 0;
                                RDBDocumentView_SelectedIndexChanged(sender, e);
                            }
                            else
                            {
                                //ByCategory.Visible = true;
                                //ByActivity.Visible = false;
                                RDBDocumentView.SelectedIndex = 1;
                                RDBDocumentView_SelectedIndexChanged(sender, e);
                            }
                        }
                        else
                        {
                            RDBDocumentView.SelectedIndex = 0;
                            RDBDocumentView_SelectedIndexChanged(sender, e);
                        }
                    }
                    else if (Request.QueryString["SubmittalUID"] != null)
                    {
                        //string ProjectUID = getdata.GetProjectUIDFromSubmittalUID(new Guid(Request.QueryString["SubmittalUID"].ToString()));
                        //if (ProjectUID != "")
                        //{
                        //    DDlProject.SelectedValue = new Guid(ProjectUID).ToString();
                        //}
                        DDlProject_SelectedIndexChanged(sender, e);
                    }
                    else
                    {
                        DDlProject_SelectedIndexChanged(sender, e);
                    }
                }
                //added on 12/01/2022 for treview scroll stay----------------
                if (Page.IsPostBack)
                {
                    string s2 = @"var elem = document.getElementById('{0}_SelectedNode');
                          if(elem != null )
                          {
                                var node = document.getElementById(elem.value);
                                if(node != null)
                                {
                                     node.scrollIntoView(true);
                                }
                          }
                        ";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "myscript", s2.Replace("{0}", TreeView1.ClientID), true);
                }
                //----------------------------------------
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

            //DDlProject.Items.Insert(0, new ListItem("--Select--", ""));
        }

        protected void DDlProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDlProject.SelectedValue != "")
            {
                //BindTreeview();
                DivDocumentView.Visible = true;
                ByActivity.Visible = true;
                ByCategory.Visible = false;

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
                    //DDLWorkPackage.DataTextField = "Name";
                    //DDLWorkPackage.DataValueField = "WorkPackageUID";
                    //DDLWorkPackage.DataSource = ds;
                    //DDLWorkPackage.DataBind();
                    //BindTreeview();
                    DivDocumentView.Visible = true;
                    ByActivity.Visible = true;
                    ByCategory.Visible = false;
                    RDBDocumentView_SelectedIndexChanged(sender, e);
                    Session["Project_Workpackage"] = DDlProject.SelectedValue;
                }
                else
                {
                    //DDLWorkPackage.DataSource = null;
                    //DDLWorkPackage.DataBind();
                    ByActivity.Visible = false;
                    ByCategory.Visible = false;
                    DivDocumentView.Visible = false;
                }
            }
        }

        private void SelectedProject()
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
        public void BindTreeview()
        {
            TreeView1.Nodes.Clear();
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
                PopulateTreeView(ds, null, "", 2);

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
                else if (Request.QueryString["TaskUID"] != null)
                {
                    TreeView1.CollapseAll();
                    string UID = Request.QueryString["TaskUID"];
                    for (int i = 0; i < TreeView1.Nodes.Count; i++)
                    {
                        retrieveNodes(TreeView1.Nodes[i], UID);
                    }
                    if(TreeView1.SelectedNode.Value != null)
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
                BindTreeView_GeneralDocuments();
            }
            else
            {
                AddDocument.Visible = false;
            }
        }

        public void BindTreeview_by_Category()
        {
            TreeView2.Nodes.Clear();
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
                PopulateTreeView_by_Category(ds, null, "", 2);
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
                    //BindDocuments(TreeView1.SelectedNode.Value);
                    Session["SelectedTaskUID1"] = null;
                    Session["ViewDocBy"] = null;
                }
                else
                {
                    TreeView2.Nodes[0].Selected = true;
                    //TreeView2.Nodes[0].Expand();
                    //TreeView2.ExpandAll();
                    TreeView2.CollapseAll();
                    TreeView2.Nodes[0].Expand();
                }
                    
                //BindDocuments(TreeView1.SelectedNode.Value);
                //BindDocuments_By_Category(TreeView2.SelectedNode.Value);
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

                    if (Level == 1)
                    {
                        //TreeView1.Nodes.Add(child);
                        //treeNode.ChildNodes.Add(child);
                        
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
                        //treeNode.ChildNodes.Add(child);
                        TreeView1.Nodes.Add(child);
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
                       
                        if (Session["IsContractor"].ToString() == "Y")
                        {
                            if (row["WorkpackageSelectedOption_Name"].ToString() != "Design")
                            {
                                if (row["WorkpackageSelectedOption_Name"].ToString() == "PMC")
                                {
                                    child.Text = "Construction & Execution";
                                }
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
                        
                        }
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

        public void PopulateTreeView_by_Category(DataSet dtParent, TreeNode treeNode, string ParentUID, int Level)
        {
            foreach (DataRow row in dtParent.Tables[0].Rows)
            {
                TreeNode child = new TreeNode
                {
                    Text = Level == 0 ? row["ProjectClass_Name"].ToString() : Level == 1 ? LimitCharts(getdata.getProjectNameby_ProjectUID(new Guid(row["ProjectUID"].ToString()))) : Level == 2 ? LimitCharts(row["Name"].ToString()) : LimitCharts(row["Name"].ToString()),
                    Value = Level == 0 ? row["ProjectClass_UID"].ToString() : Level == 1 ? row["ProjectUID"].ToString() : Level == 2 ? row["WorkPackageUID"].ToString() : row["TaskUID"].ToString(),
                    Target = Level == 0 ? "Class" : Level == 1 ? "Project" : Level == 2 ? "WorkPackage" : "Category",
                    ToolTip = Level == 0 ? row["ProjectClass_Name"].ToString() : Level == 1 ? getdata.getProjectNameby_ProjectUID(new Guid(row["ProjectUID"].ToString())) : Level == 2 ? row["Name"].ToString() : row["Name"].ToString()
                };

                if (Level == 1)
                {
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
                        PopulateTreeView_by_Category(dsworkPackage, child, child.Value, 2);
                    }
                }
                else if (Level == 2)
                {
                    TreeView2.Nodes.Add(child);
                    //treeNode.ChildNodes.Add(child);
                    //DataSet ds = getdata.GetDocuments_For_WorkPackage(new Guid(row["WorkPackageUID"].ToString()));
                    //if (ds.Tables[0].Rows.Count > 0)
                    //{
                    DataSet cat = getdata.WorkPackageCategory_Selectby_WorkPackageUID(new Guid(row["WorkPackageUID"].ToString()));
                    if (cat.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < cat.Tables[0].Rows.Count; i++)
                        {
                            TreeNode cate = new TreeNode
                            {
                                Text = cat.Tables[0].Rows[i]["WorkPackageCategory_Name"].ToString(),
                                Value = cat.Tables[0].Rows[i]["WorkPackageCategory_UID"].ToString(),
                                Target = "Category",
                                ToolTip = cat.Tables[0].Rows[i]["WorkPackageCategory_Name"].ToString()
                            };
                            child.ChildNodes.Add(cate);

                            DataSet submittal = getdata.GetSubmittals_For_Category(cat.Tables[0].Rows[i]["WorkPackageCategory_UID"].ToString());
                            if (submittal.Tables[0].Rows.Count > 0)
                            {
                                for (int j = 0; j < submittal.Tables[0].Rows.Count; j++)
                                {
                                    TreeNode subdoc = new TreeNode
                                    {
                                        Text = "<div style='color:#007bff;'>" + submittal.Tables[0].Rows[j]["DocName"].ToString() +"</div>",
                                        Value = submittal.Tables[0].Rows[j]["DocumentUID"].ToString(),
                                        Target = "Submittal",
                                        ToolTip = submittal.Tables[0].Rows[j]["DocName"].ToString(),
                                        ImageUrl= "~/_assets/images/submittal.png"
                                    };
                                    cate.ChildNodes.Add(subdoc);
                                    string Dir_Name = "";
                                    int SplitNodeCount = 0;
                                    DataSet documents = getdata.ActualDocuments_SelectBy_DocumentUID(new Guid(submittal.Tables[0].Rows[j]["DocumentUID"].ToString()));
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
                        }

                    }
                    //}   
                }
            }

        }
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

            if (TreeView1.SelectedNode.Target == "Class")
            {
                ActivityHeading.Text = "Projects";
                ProjectDetails.Visible = false;
                WorkPackageDetils.Visible = false;
                AddDocument.Visible = false;
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
                DocumentGrid.Visible = false;
                GrdNewDocument.Visible = true;
                GrdActualDocuments_new.Visible = false;
                GrdTreeView.Visible = true;
                GrdOptions.Visible = false;
                
            }
            else if (TreeView1.SelectedNode.Target == "Project")
            {
                AddDocument.Visible = false;
                AddDocument.HRef = "/_modal_pages/add-submittal.aspx?type=add&ProjectUID=" + DDlProject.SelectedValue;
                ActivityHeading.Text = "Project : " + getdata.getProjectNameby_ProjectUID(new Guid(TreeView1.SelectedNode.Value));
                DataSet ds = getdata.GetWorkPackage_By_ProjectUID(new Guid(TreeView1.SelectedNode.Value));
                GrdTreeView.DataSource = ds;
                GrdTreeView.DataBind();
                DocumentGrid.Visible = false;
                GrdNewDocument.Visible = true;
                GrdActualDocuments_new.Visible = false;
                GrdTreeView.Visible = true;
                GrdOptions.Visible = false;
               
            }
            else if (TreeView1.SelectedNode.Target == "WorkPackage")
            {
                AddDocument.Visible = false;
                AddDocument.HRef = "/_modal_pages/add-submittal.aspx?type=add&WorkPackageUID=" + TreeView1.SelectedNode.Value + "&ProjectUID=" + DDlProject.SelectedValue;
                ActivityHeading.Text = "WorkPackage : " + getdata.getWorkPackageNameby_WorkPackageUID(new Guid(TreeView1.SelectedNode.Value));
                //DataSet ds = gettk.GetTasks_by_WorkPackage(TreeView1.SelectedNode.Value);
                //GrdTreeView.DataSource = ds;
                //GrdTreeView.DataBind();
                //DataSet ds = getdata.GetWorkPackages_By_WorkPackageUID(new Guid(TreeView1.SelectedNode.Value));
               // DataSet ds = getdata.GetSelectedOption_By_WorkpackageUID(new Guid(TreeView1.SelectedNode.Value));
              //  GrdOptions.DataSource = ds;
              //  GrdOptions.DataBind();
                DocumentGrid.Visible = false;
                GrdNewDocument.Visible = true;
                GrdActualDocuments_new.Visible = false;
                GrdTreeView.Visible = false;
                GrdOptions.Visible = true;
                
            }
            else if (TreeView1.SelectedNode.Target == "Option")
            {
                AddDocument.Visible = false;
                AddDocument.HRef = "/_modal_pages/add-submittal.aspx?type=add&WorkPackageUID=" + TreeView1.SelectedNode.Parent.Value + "&ProjectUID=" + DDlProject.SelectedValue;
                ActivityHeading.Text = "Option : " + TreeView1.SelectedNode.Text;
                GrdTreeView.Visible = true;
                GrdOptions.Visible = false;
                DocumentGrid.Visible = false;
                GrdNewDocument.Visible = true;
                GrdActualDocuments_new.Visible = false;
                //DataSet ds = gettk.GetTasks_by_WorkPackage(TreeView1.SelectedNode.Parent.Value);
                DataSet ds = getdata.GetTasks_by_WorkpackageOptionUID(new Guid(TreeView1.SelectedNode.Parent.Value),new Guid(TreeView1.SelectedNode.Value));
                GrdTreeView.DataSource = ds;
                GrdTreeView.DataBind();
            }
            else if (TreeView1.SelectedNode.Target == "Category")
            {
                AddDocument.Visible = false;
                ActivityHeading.Text = "Category : " + TreeView1.SelectedNode.Text;
                GrdTreeView.DataSource = null;
                GrdTreeView.DataBind();
                DocumentGrid.Visible = true;
                GrdNewDocument.Visible = true;
                GrdActualDocuments_new.Visible = false;
                GrdTreeView.Visible = true;
                GrdOptions.Visible = false;
            }
            else if (TreeView1.SelectedNode.Target == "Tasks")
            {
                //if (TreeView1.SelectedNode.ChildNodes.Count > 0)
                //{
                //    AddDocument.Visible = false;
                //}
                //else
                //{
                //    AddDocument.Visible = true;
                //}
                
                AddDocument.Visible = true;
                //AddDocument.HRef = "/_modal_pages/add-submittal.aspx?type=add&TaskUID=" + TreeView1.SelectedNode.Value + "&ViewDocumentBy=" + RDBDocumentView.SelectedValue + "&PrjUID=" + TreeView1.SelectedNode.Parent.Parent.Parent.Value + "&WorkPackageUID=" + TreeView1.SelectedNode.Parent.Parent.Value;
                AddDocument.HRef = "/_modal_pages/add-submittal.aspx?type=add&TaskUID=" + TreeView1.SelectedNode.Value + "&ViewDocumentBy=" + RDBDocumentView.SelectedValue + "&PrjUID=" + DDlProject.SelectedValue + "&WorkPackageUID=" + TreeView1.SelectedNode.Parent.Parent.Value;
                ActivityHeading.Text = "Activity : " + TreeView1.SelectedNode.Text;
                DataSet ds = getdata.GetTask_by_ParentTaskUID(new Guid(TreeView1.SelectedNode.Value));
                GrdTreeView.DataSource = ds;
                GrdTreeView.DataBind();
                DocumentGrid.Visible = true;
                GrdNewDocument.Visible = true;
                GrdActualDocuments_new.Visible = false;
                GrdTreeView.Visible = true;
                GrdOptions.Visible = false;
            }
            else if (TreeView1.SelectedNode.Target == "Submittal")
            {
                
                ActivityHeading.Text = "Submittal : " + TreeView1.SelectedNode.Text;
                // added on 23/10/2020
                ViewState["Submittal"] = "Submittal : " + TreeView1.SelectedNode.Text;
                AddDocument.Visible = false;
                DocumentGrid.Visible = true;
                GrdNewDocument.Visible = true;
                GrdActualDocuments_new.Visible = false;
                GrdTreeView.DataSource = null;
                GrdTreeView.DataBind();
                GrdTreeView.Visible = true;
                GrdOptions.Visible = false;
            }
            else if (TreeView1.SelectedNode.Target == "Folder")
            {
                ActivityHeading.Text = "Folder : " + TreeView1.SelectedNode.Text;
                AddDocument.Visible = false;
                DocumentGrid.Visible = true;
                GrdTreeView.DataSource = null;
                GrdTreeView.DataBind();
                GrdNewDocument.Visible = false;
                GrdActualDocuments_new.Visible = true;
                GrdTreeView.Visible = true;
                GrdOptions.Visible = false;
            }
            else if (TreeView1.SelectedNode.Target == "SubFolder")
            {
                ActivityHeading.Text = "SubFolder : " + TreeView1.SelectedNode.Text;
                AddDocument.Visible = false;
                DocumentGrid.Visible = true;
                GrdTreeView.DataSource = null;
                GrdTreeView.DataBind();
                GrdNewDocument.Visible = false;
                GrdActualDocuments_new.Visible = true;
                GrdTreeView.Visible = true;
                GrdOptions.Visible = false;
            }
            else
            {
                ActivityHeading.Text = "Category List";
                AddDocument.Visible = false;
                GrdTreeView.DataSource = null;
                GrdTreeView.DataBind();
                DocumentGrid.Visible = false;
            }
        }

        private void DbSyncStatusCount(string WorkpackageUID)
        {
            if (WebConfigurationManager.AppSettings["Dbsync"] == "Yes")
            {
                DataSet ds = getdata.GetDbsync_Status_Count_by_WorkPackageUID(new Guid(WorkpackageUID));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DivDocumentsSyncedCount.Visible = true;
                    LblLastSyncedDate.Text = ds.Tables[0].Rows[0]["CreatedDate"].ToString() != "" ? Convert.ToDateTime(ds.Tables[0].Rows[0]["CreatedDate"].ToString()).ToString("dd MMM yyyy hh:mm tt") : "NA";
                    LblTotalSourceDocuments.Text = ds.Tables[0].Rows[0]["DestDocCount"].ToString() != "" ? ds.Tables[0].Rows[0]["DestDocCount"].ToString() : "NA";
                    LblTotalDestinationDocuments.Text = ds.Tables[0].Rows[0]["SourceDocCount"].ToString() != "" ? ds.Tables[0].Rows[0]["SourceDocCount"].ToString() : "NA";

                    //  LblSourceHeading.Text = TreeView1.SelectedNode.Parent.Text + "(" + TreeView1.SelectedNode.Text + ")" + " :- " + WebConfigurationManager.AppSettings["SourceSite"];
                    LblSourceHeading.Text =  TreeView1.SelectedNode.Text  + " :- " + WebConfigurationManager.AppSettings["SourceSite"];
                    LblDestinationHeading.Text = WebConfigurationManager.AppSettings["DestinationSite"];
                }
                else
                {
                    DivDocumentsSyncedCount.Visible = false;
                }
            }
            else
            {
                DivDocumentsSyncedCount.Visible = false;
            }
        }

        public void BindDocuments(string UID)
        {
            DataSet ds = new DataSet();
            Session["CopiedActivity"] = UID;
            if (TreeView1.SelectedNode.Target == "Project")
            {
                ds = getdata.GetDocuments_For_Project(new Guid(UID));
                GrdNewDocument.DataSource = ds;
                GrdNewDocument.DataBind();
                DivDocumentsSyncedCount.Visible = false;
            }
            else if (TreeView1.SelectedNode.Target == "WorkPackage")
            {
                ds = getdata.GetDocuments_For_WorkPackage(new Guid(UID));
                GrdNewDocument.DataSource = ds;
                GrdNewDocument.DataBind();

                DbSyncStatusCount(UID);
            }
            else if (TreeView1.SelectedNode.Target == "Category")
            {
                if (TreeView1.SelectedNode.Parent.Target == "WorkPackage Document")
                {
                    ds = getdata.GetWorkpackageDocuments_For_Category(new Guid(TreeView1.SelectedNode.Parent.Value), UID);
                }
                else if (TreeView1.SelectedNode.Parent.Target == "Document")
                {
                    ds = getdata.GetTaskDocuments_For_Category(new Guid(TreeView1.SelectedNode.Parent.Value), UID);
                }
                GrdNewDocument.DataSource = ds;
                GrdNewDocument.DataBind();
                DivDocumentsSyncedCount.Visible = false;
            }
            else if (TreeView1.SelectedNode.Target == "Tasks")
            {
                ds = getdata.getDocumentsForTasks(new Guid(UID));
                GrdNewDocument.DataSource = ds;
                GrdNewDocument.DataBind();
                DivDocumentsSyncedCount.Visible = false;
            }
            else if (TreeView1.SelectedNode.Target == "Submittal")
            {
                ds = getdata.getDocumentsbyDocID(new Guid(UID));
                GrdNewDocument.DataSource = ds;
                GrdNewDocument.DataBind();
                DivDocumentsSyncedCount.Visible = false;
            }
            else if (TreeView1.SelectedNode.Target == "Folder" || TreeView1.SelectedNode.Target == "SubFolder")
            {
                //  TreeView1.SelectedNode.Text
                ViewState["folder"] = TreeView1.SelectedNode.Text;
                string CheckFile = TreeView1.SelectedNode.Text;
                string NodeParentText = TreeView1.SelectedNode.ToolTip;
                if (!string.IsNullOrEmpty(NodeParentText))
                {
                    NodeParentText = NodeParentText.Split('/')[0];
                }
                ds = new DataSet();
                if (CheckFile.EndsWith(".pdf") || CheckFile.EndsWith(".PDF") || CheckFile.EndsWith(".doc") || CheckFile.EndsWith(".docx") || CheckFile.EndsWith(".dwg") || CheckFile.EndsWith(".xlsx") || CheckFile.EndsWith(".xls") || CheckFile.EndsWith(".txt") || CheckFile.EndsWith(".pptx") || CheckFile.EndsWith(".log") || CheckFile.EndsWith(".zip"))
                {
                    ActivityHeading.Text = "Document : " + TreeView1.SelectedNode.Text;
                    //ds = getdata.ActualDocuments_SelectBy_DocID_FileName(new Guid(UID), TreeView2.SelectedNode.Text.Split('.')[0]);
                    ds = getdata.ActualDocuments_SelectBy_ActualDocumentUID(new Guid(UID));
                }
                else
                {
                    ds = getdata.ActualDocuments_SelectBy_DirectoryName_New(new Guid(UID), NodeParentText);

                }
                //ds = getdata.ActualDocuments_SelectBy_DocumentUID(new Guid(UID));
                GrdActualDocuments_new.DataSource = ds;
                GrdActualDocuments_new.DataBind();
                DivDocumentsSyncedCount.Visible = false;
            }
            //else if (TreeView1.SelectedNode.Target == "Folder")
            //{
            //    ds = getdata.ActualDocuments_SelectBy_DirectoryName(new Guid(UID), TreeView1.SelectedNode.Text);
            //    GrdActualDocuments_new.DataSource = ds;
            //    GrdActualDocuments_new.DataBind();
            //}
            else if (TreeView1.SelectedNode.Target == "SubFolder")
            {                
                string CheckFile = TreeView1.SelectedNode.Text;
                string tooltip1 = TreeView1.SelectedNode.ToolTip;
                if (CheckFile.EndsWith(".pdf") || CheckFile.EndsWith(".PDF") || CheckFile.EndsWith(".doc") || CheckFile.EndsWith(".docx") || CheckFile.EndsWith(".dwg") || CheckFile.EndsWith(".xlsx") || CheckFile.EndsWith(".xls") || CheckFile.EndsWith(".txt"))
                {
                    ActivityHeading.Text = "Document : " + TreeView1.SelectedNode.Text;
                    //ds = getdata.ActualDocuments_SelectBy_DocID_FileName(new Guid(UID), TreeView2.SelectedNode.Text.Split('.')[0]);
                    ds = getdata.ActualDocuments_SelectBy_ActualDocumentUID(new Guid(UID));
                }
                else
                {
                    ds = getdata.ActualDocuments_SelectBy_DirectoryName(new Guid(UID), TreeView1.SelectedNode.Text);
                }

                GrdActualDocuments_new.DataSource = ds;
                GrdActualDocuments_new.DataBind();
                DivDocumentsSyncedCount.Visible = false;
            }
            else
            {
                ds = null;
                GrdNewDocument.DataSource = ds;
                GrdNewDocument.DataBind();
                DivDocumentsSyncedCount.Visible = false;
            }
            

        }

        public void BindDocuments_By_Category(string UID)
        {
            DataSet ds = new DataSet();
            if (TreeView2.SelectedNode.Target == "Class")
            {
                CategoryDocumentGrid.Visible = false;
                CategoryDocumentGridHeading.Visible = false;
                ProjectDetails.Visible = false;
                WorkPackageDetils.Visible = false;
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
                ProjectDetails.Visible = true;
                WorkPackageDetils.Visible = false;
                LoadProjectDetails(UID);
                GrdDocumentByCategory.DataSource = ds;
                GrdDocumentByCategory.DataBind();
                GrdActualDocuments1.Visible = false;
            }
            else if (TreeView2.SelectedNode.Target == "WorkPackage")
            {
                ds = getdata.GetDocuments_For_WorkPackage(new Guid(UID));
                CategoryDocumentGridHeading.Visible = false;
                CategoryDocumentGrid.Visible = false;
                ProjectDetails.Visible = false;
                WorkPackageDetils.Visible = true;
                GetWorkPackage_by_UID(new Guid(UID));
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
                ProjectDetails.Visible = false;
                WorkPackageDetils.Visible = false;
                ActivityHeadingCategory.Text = "Category : " + TreeView2.SelectedNode.Text;
                AddSubmittalCategory.HRef = "/_modal_pages/add-submittalsforcategory.aspx?type=add&CategoryID=" + TreeView2.SelectedNode.Value + "&ViewDocumentBy=" + RDBDocumentView.SelectedValue + "&PrjUID=" + DDlProject.SelectedValue + "&WorkPackageUID=" + TreeView2.SelectedNode.Parent.Value;
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
                ProjectDetails.Visible = false;
                WorkPackageDetils.Visible = false;
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
                ProjectDetails.Visible = false;
                WorkPackageDetils.Visible = false;
               
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
                ProjectDetails.Visible = false;
                WorkPackageDetils.Visible = false;
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

        protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
        {
            BindActivities();
            BindDocuments(TreeView1.SelectedNode.Value);
            GeneralDocumentsDiv.Visible = false;
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

        protected void GrdTreeView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdTreeView.PageIndex = e.NewPageIndex;
            DataSet ds = getdata.GetTasks_by_ProjectUID(new Guid(TreeView1.SelectedNode.Value));
            GrdTreeView.DataSource = ds;
            GrdTreeView.DataBind();

        }

        protected void GrdTreeView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string UID = e.CommandArgument.ToString();
            for (int i = 0; i < TreeView1.Nodes.Count; i++)
            {
                retrieveNodes(TreeView1.Nodes[i], UID);
            }
            BindDocuments(UID);
        }

        protected void GrdNewDocument_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string DocumentUID = GrdNewDocument.DataKeys[e.Row.RowIndex].Values[0].ToString();
                //GridView grdActualDocuments = (GridView)e.Row.FindControl("GrdActualDocuments");

                //DataSet ds = getdata.ActualDocuments_SelectBy_DocumentUID(new Guid(DocumentUID));
                //grdActualDocuments.DataSource = ds;
                //grdActualDocuments.DataBind();

                string Submitter = getdata.GetSubmittal_Submitter_By_DocumentUID(new Guid(DocumentUID));
                if (Submitter != "")
                {

                    if (Session["UserUID"].ToString().ToUpper() != Submitter.ToUpper())
                    {
                        //e.Row.Cells[4].Controls[0].Visible = false;
                        //e.Row.Cells[4].Text = "--";
                        //e.Row.Cells[4].Enabled = false;
                        
                        e.Row.Cells[4].CssClass = "hideItem";
                        GrdNewDocument.HeaderRow.Cells[4].Visible = false;
                        //HtmlAnchor HA = (HtmlAnchor)e.Row.FindControl("AddDoc");
                        //HA.InnerText = "NA";

                    }
                }
                // for db sync checking
                if (WebConfigurationManager.AppSettings["Dbsync"] == "Yes")
                {
                    bool isset = getdata.checkdbsyncflag(new Guid(DocumentUID), "Documents", "DocumentUID");
                    if (isset == true)
                    {
                        e.Row.BackColor = System.Drawing.Color.LightYellow;
                    }
                    else
                    {
                        //  e.Row.BackColor = System.Drawing.Color.Green;
                        // e.Row.ForeColor = System.Drawing.Color.White;
                    }
                }
            }
        }

        protected void GrdNewDocument_RowCommand(object sender, GridViewCommandEventArgs e)
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
                try
                {
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
                catch (Exception ex)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Document not uploaded for this flow.Please Contact Document controller!');</script>");
                }
            }

            if (e.CommandName == "delete")
            {
                int cnt = getdata.Documents_Delete_by_DocID(new Guid(UID), new Guid(Session["UserUID"].ToString()));
                if (TreeView1.SelectedNode.Target == "Submittal")
                {
                    Response.Redirect("~/_content_pages/documents/?TaskUID=" + TreeView1.SelectedNode.Parent.Value);
                }
                else
                {
                    Response.Redirect("~/_content_pages/documents/?TaskUID=" + TreeView1.SelectedNode.Value);
                }
                // if (cnt > 0)
                //{
               // ViewState["TaskUID"] = TreeView1.SelectedNode.Value;
                   // BindTreeview();
                ////TreeView1.CollapseAll();

                //for (int i = 0; i < TreeView1.Nodes.Count; i++)
                //{
                //    retrieveNodes(TreeView1.Nodes[i], TreeView1.SelectedNode.Value);
                //}

                //// 
               // BindActivities();
               // BindDocuments(TreeView1.SelectedNode.Value);
                //BindDocuments(TreeView1.SelectedNode.Value);
                //}

            }
        }

        protected void GrdNewDocument_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdNewDocument.PageIndex = e.NewPageIndex;
            BindDocuments(TreeView1.SelectedNode.Value);
        }

        protected void GrdNewDocument_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

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

        public string GetDocumentTypeIcon(string DocumentExtn,string ActualDocumentUID,string dType)
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

        protected void Show_Hide_DocumentsGrid(object sender, EventArgs e)
        {
            try
            {
                Page currentPage = (Page)HttpContext.Current.Handler;
                ImageButton imgShowHide = (sender as ImageButton);
                GridViewRow row = (imgShowHide.NamingContainer as GridViewRow);
                if (imgShowHide.CommandArgument == "Show")
                {
                    row.FindControl("pnlDocuments").Visible = true;
                    imgShowHide.CommandArgument = "Hide";
                    imgShowHide.ImageUrl = "~/_assets/images/minus.png";
                    string orderId = (row.NamingContainer as GridView).DataKeys[row.RowIndex].Value.ToString();
                    GridView GrdActualDocuments = row.FindControl("GrdActualDocuments") as GridView;
                    BindActualDocuments(orderId, GrdActualDocuments);
                    ScriptManager.RegisterStartupScript(currentPage, typeof(string), "Script" + Guid.NewGuid(), "expand1();", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(currentPage, typeof(string), "Script" + Guid.NewGuid(), "expand1();", true);
                    row.FindControl("pnlDocuments").Visible = false;
                    imgShowHide.CommandArgument = "Show";
                    imgShowHide.ImageUrl = "~/_assets/images/plus.png";
                }
                //string DocumentUID = GrdNewDocument.DataKeys[row.RowIndex].Values[0].ToString();
                string DocumentUID = (row.NamingContainer as GridView).DataKeys[row.RowIndex].Value.ToString();
                string Submitter = getdata.GetSubmittal_Submitter_By_DocumentUID(new Guid(DocumentUID));
                if (Submitter != "")
                {
                    string sessionuser = Session["UserUID"].ToString();
                    if (sessionuser.ToUpper() != Submitter.ToUpper())
                    {
                        //TableCell cell = row.Cells[4];
                        //row.Cells[4].Enabled = false;
                        row.Cells[4].CssClass = "hideItem";
                        //row.Cells[4].Controls[0].Visible = false;
                        //row.Cells[4].Text = "--";

                    }
                }
            }
            catch (Exception ex)
            {

            }
            
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
                //GrdActualDocumentsCategory.ToolTip = DocumentID.ToString();
                DataSet ds = getdata.ActualDocuments_SelectBy_DocumentUID(new Guid(DocumentID));
                GrdActualDocumentsCategory.DataSource = ds;
                GrdActualDocumentsCategory.DataBind();
            }
            catch (Exception ex)
            {
                throw;
            }
            
        }

        protected void GrdActualDocuments_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (ViewState["isDelete"].ToString() == "false")
                {
                    e.Row.Cells[8].Visible = false;
                }
                if (ViewState["isUpload"].ToString() == "false")
                {
                    e.Row.Cells[6].Visible = false;
                }

               
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // added on 05/11/2020
                try
                {
                    string ActualDocumentUID = "";
                    if (ViewState["isDelete"].ToString() == "false")
                    {
                        e.Row.Cells[8].Visible = false;
                    }
                    if (ViewState["isUpload"].ToString() == "false")
                    {
                        e.Row.Cells[6].Visible = false;
                    }
                    //-------------------------------------------
                    if (e.Row.Cells[9].Text == "&nbsp;") // for files 
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
                                Label lblDocumentName = (Label)e.Row.FindControl("lblDocumentName");
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
                                        string newVersionFileName = Path.GetFileNameWithoutExtension(Server.MapPath(ds.Tables[0].Rows[0]["Doc_Path"].ToString()));
                                        lblDocumentName.Text = newVersionFileName.Substring(0, (newVersionFileName.Length - 2));
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
                            //for db sync check
                            if (WebConfigurationManager.AppSettings["Dbsync"] == "Yes")
                            {
                                if (!string.IsNullOrEmpty(ActualDocumentUID))
                                {
                                    if (getdata.checkDocumentsFlagdbsync(new Guid(ActualDocumentUID)) == 1)
                                    {
                                        e.Row.BackColor = System.Drawing.Color.LightYellow;
                                    }
                                    else
                                    {
                                        //  e.Row.BackColor = System.Drawing.Color.Green;
                                        // e.Row.ForeColor = System.Drawing.Color.White;
                                    }
                                }
                            }
                            //--------------------------------------------
                            if (Session["IsContractor"].ToString() == "Y")
                            {
                                string SubmittalUID = getdata.GetSubmittalUID_By_ActualDocumentUID(new Guid(ActualDocumentUID));
                                string phase = getdata.GetPhaseforStatus(new Guid(getdata.GetFlowUIDBySubmittalUID(new Guid(SubmittalUID))), e.Row.Cells[2].Text);
                                if (string.IsNullOrEmpty(phase))
                                {
                                    if (e.Row.Cells[2].Text == "Code A-CE Approval" || e.Row.Cells[2].Text == "Client CE GFC Approval")
                                    {
                                        e.Row.Cells[2].Text = "Approved";

                                    }
                                    if (e.Row.Cells[2].Text == "Code B-CE Approval" || e.Row.Cells[3].Text == "Code C-CE Approval")
                                    {
                                        e.Row.Cells[2].Text = "Under Client Approval Process";
                                    }
                                }
                                else
                                {
                                    e.Row.Cells[2].Text = phase;
                                }


                            }

                            //

                        }
                    }
                    else // for folder view
                    {
                        if (folder != e.Row.Cells[9].Text.Split('/')[0])
                        {
                            Label lblVer = (Label)e.Row.FindControl("LblVersion");
                            lblVer.Visible = false;
                            //  HtmlImage lnkVoucher = e.Row.FindControl("imgpdf") as HtmlImage;
                            // lnkVoucher.Visible = false;
                            HtmlGenericControl htmlDivControl = e.Row.FindControl("divPDF") as HtmlGenericControl;
                            htmlDivControl.Visible = false;
                            LinkButton lnkfolder = (LinkButton)e.Row.FindControl("LinkButton2");
                            lnkfolder.Text = e.Row.Cells[9].Text.Split('/')[0];
                            e.Row.Cells[3].Text = "";
                            // e.Row.Cells[0].Text = "<a href='#' onclick='someFunction()'><img src='/_assets/images/folder.jpg' height='15px' width='20px'>  " + e.Row.Cells[7].Text.Split('/')[0] + "</a>";
                            //e.Row.Cells[0].Text = "<asp:LinkButton ID = \"LinkButton3\" runat = \"server\" CausesValidation = \"False\" CommandName = \"Edit\" Text = \"Edit\" ></ asp:LinkButton >";
                            e.Row.Cells[1].Text = "";
                            e.Row.Cells[2].Text = "";
                            e.Row.Cells[4].Text = "";
                            e.Row.Cells[5].Text = "";
                            e.Row.Cells[6].Text = "";
                            e.Row.Cells[7].Text = "";
                            e.Row.Cells[8].Text = "";
                            folder = e.Row.Cells[9].Text.Split('/')[0];
                        }
                        else
                        {
                            e.Row.Attributes["style"] = "display:none";
                        }
                    }

                    if (ActualDocumentUID != "")
                    {
                        if (Session["copydocument"] != null)
                        {
                            getdata.sfinallist = (List<CopyDocumentFile>)Session["copydocument"];
                            if (getdata.sfinallist.Any(x => x.DocumentUID == new Guid(ActualDocumentUID)))
                            {
                                LinkButton lnkcopy = (LinkButton)e.Row.FindControl("lnkcopy");
                                lnkcopy.Enabled = false;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                

                //-------------------------------------------


                //if (e.Row.Cells[1].Text == "Cover Letter" || e.Row.Cells[1].Text == "General Document")
                //{
                //    HtmlAnchor lnkVoucher = e.Row.FindControl("ViewDoc") as HtmlAnchor;
                //    //HtmlAnchor link = e.Row.Cells[0].Controls[0] as HtmlAnchor;
                //    if (lnkVoucher != null)
                //    {
                //        lnkVoucher.HRef = "javascript:void(0)";
                //    }


                //    //HyperLink myHyperLink = e.Row.FindControl("ViewDoc") as HyperLink;
                //    //myHyperLink.hr
                //}
            }
            //if (e.Row.Cells[2].Text != "&nbsp;")
            //{
            //    DataSet ds = getdata.getTop1_DocumentStatusSelect(new Guid(e.Row.Cells[2].Text));
            //    if (ds != null)
            //    {
            //        int delay = getdata.GetDelayed_Actual_Documents(new Guid(e.Row.Cells[2].Text), ds.Tables[0].Rows[0]["ActivityType"].ToString());
            //        if (ds.Tables[0].Rows[0]["DocumentType"].ToString() == "General Document")
            //        {
            //            Label lblVer = (Label)e.Row.FindControl("LblVersion");
            //            lblVer.Text = "[ Ver. 1 ]";
            //            e.Row.Cells[2].Text = "Submitted";
            //            e.Row.Cells[4].Text = "No History";                           
            //        }
            //        else
            //        {
            //            Label lblVer = (Label)e.Row.FindControl("LblVersion");
            //            e.Row.Cells[2].Text = ds.Tables[0].Rows[0]["ActivityType"].ToString();
            //            if (ds.Tables[0].Rows[0]["ActivityType"].ToString() != "" && ds.Tables[0].Rows[0]["TopVersion"].ToString() != "")
            //            {
            //                //e.Row.Cells[1].Text = ds.Tables[0].Rows[0]["TopVersion"].ToString();

            //                lblVer.Text = "[ Ver. " + ds.Tables[0].Rows[0]["TopVersion"].ToString() + " ]";
            //            }
            //            else
            //            {
            //                lblVer.Text = "[ Ver. 1 ]";
            //            }
            //        }
            //        if (e.Row.Cells[2].Text == "BWSSB Approved" || e.Row.Cells[2].Text == "Approved" || e.Row.Cells[2].Text == "BWSSB Approve")
            //        {
            //            //e.Row.Cells[13].BackColor = System.Drawing.Color.Green;
            //            e.Row.Cells[2].ForeColor = System.Drawing.Color.Green;
            //            e.Row.Cells[2].Font.Bold = true;
            //        }
            //        if (delay == 1)
            //        {
            //            e.Row.Cells[2].BackColor = System.Drawing.Color.Red;
            //            e.Row.Cells[2].ForeColor = System.Drawing.Color.White;
            //        }
            //    }

            //}


        }

        protected void GrdActualDocuments_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string UID = e.CommandArgument.ToString();
            if (e.CommandName == "download")
            {
                string path = string.Empty;
                string filename = string.Empty;
                DataSet ds1 = null;
                DataSet ds = getdata.getLatest_DocumentVerisonSelect(new Guid(UID));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    path = Server.MapPath(ds.Tables[0].Rows[0]["Doc_FileName"].ToString());
                    //File.Decrypt(path);
                    filename = Path.GetFileName(ds.Tables[0].Rows[0]["Doc_FileName"].ToString());
                }
                else
                {
                    ds1 = getdata.ActualDocuments_SelectBy_ActualDocumentUID(new Guid(UID));
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        if (ds1.Tables[0].Rows[0]["ActualDocument_Path"].ToString() != "")
                        {
                            path = Server.MapPath(ds1.Tables[0].Rows[0]["ActualDocument_Path"].ToString());
                            filename = ds1.Tables[0].Rows[0]["ActualDocument_Name"].ToString() + ds1.Tables[0].Rows[0]["ActualDocument_Type"].ToString();
                        }
                    }
                }
                // added on  20/10/2020
                //ds.Clear();
                //ds = getdata.ActualDocuments_SelectBy_ActualDocumentUID(new Guid(UID));
                //if (ds.Tables[0].Rows.Count > 0)
                //{
                //    if (ds.Tables[0].Rows[0]["ActualDocument_Name"].ToString() != "")
                //    {
                //        filename = ds.Tables[0].Rows[0]["ActualDocument_Name"].ToString() + ds.Tables[0].Rows[0]["ActualDocument_Type"].ToString();
                //    }
                //}
                //
                try
                {
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

                        // Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
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
               catch(Exception ex)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Document not uploaded for this flow.Please Contact Document controller!');</script>");
                }
            }

            if (e.CommandName == "delete")
            {
                int cnt = getdata.ActualDocuments_Delete_by_DocID(new Guid(UID), new Guid(Session["UserUID"].ToString()));
                if (cnt > 0)
                {
                    GridViewRow gvRowParent = (((GridView)sender)).DataItemContainer as GridViewRow;
                    string orderId = (gvRowParent.NamingContainer as GridView).DataKeys[gvRowParent.RowIndex].Value.ToString();
                    GridView gvrdChild = ((GridView)sender);
                    BindActualDocuments(orderId, gvrdChild);
                    //
                    //GridView gvwChild = ((GridView)sender);
                    //BindDocuments(TreeView1.SelectedNode.Value);
                    //gvRowParent.FindControl("pnlDocuments").Visible = true;
                    //imgShowHide.CommandArgument = "Hide";
                    //imgShowHide.ImageUrl = "~/_assets/images/minus.png";

                    //ImageButton imgbtn = gvRowParent.FindControl("pnlDocuments") as ImageButton;
                    //GridViewRow row = (sender as LinkButton).NamingContainer as GridViewRow;
                    // string id = "imgProductsShow";
                    //string script = @"$(document).ready(function () {$('#" + id + "').trigger('click');});";
                    ScriptManager.RegisterClientScriptBlock(sender as Control, this.GetType(), "alert", "expand1()", true);


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
                for (int i = 0; i < TreeView1.Nodes.Count; i++)
                {
                    retrieveNodes(TreeView1.Nodes[i], UID);
                }

                BindDocuments(UID);
            }

            if (e.CommandName == "copyfile")
            {
                LinkButton ctrl = e.CommandSource as LinkButton;
                if (ctrl != null)
                {
                    GridViewRow row = ctrl.Parent.NamingContainer as GridViewRow;
                    LinkButton lnkcopy = (LinkButton)row.FindControl("lnkcopy");
                    lnkcopy.Enabled = false;
                }
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
                LblcopyFileCount.Text = "Copied File/s (" + getdata.sfinallist.Count + ")";
                LblcopyFileCount.Visible = true;
            }
            //string UID = e.CommandArgument.ToString();
            //if (e.CommandName == "download")
            //{
            //    string path = string.Empty;
            //    DataSet ds = getdata.getLatest_DocumentVerisonSelect(new Guid(UID));
            //    if (ds.Tables[0].Rows.Count > 0)
            //    {
            //        path = Server.MapPath(ds.Tables[0].Rows[0]["Doc_FileName"].ToString());
            //        //File.Decrypt(path);
            //    }
            //    else
            //    {
            //        DataSet ds1 = getdata.ActualDocuments_SelectBy_ActualDocumentUID(new Guid(UID));
            //        if (ds1.Tables[0].Rows.Count > 0)
            //        {
            //            if (ds1.Tables[0].Rows[0]["ActualDocument_Path"].ToString() != "")
            //            {
            //                path = Server.MapPath(ds1.Tables[0].Rows[0]["ActualDocument_Path"].ToString());
            //            }
            //        }
            //    }
            //    string getExtension = System.IO.Path.GetExtension(path);
            //    string outPath = path.Replace(getExtension, "") + "_download" + getExtension;
            //    getdata.DecryptFile(path, outPath);
            //    System.IO.FileInfo file = new System.IO.FileInfo(outPath);

            //    if (file.Exists)
            //    {
            //        int Cnt = getdata.DocumentHistroy_InsertorUpdate(Guid.NewGuid(), new Guid(UID), new Guid(Session["UserUID"].ToString()), "Downloaded");
            //        if (Cnt <= 0)
            //        {
            //            Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code: DDH-01. there is a problem with updating histroy. Please contact system admin.');</script>");
            //        }
            //        Response.Clear();

            //        Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);

            //        Response.AddHeader("Content-Length", file.Length.ToString());

            //        Response.ContentType = "application/octet-stream";

            //        Response.WriteFile(file.FullName);

            //        Response.End();

            //    }
            //    else
            //    {

            //        //Response.Write("This file does not exist.");
            //        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('File does not exists.');</script>");

            //    }
            //}

            //if (e.CommandName == "delete")
            //{
            //    int cnt = getdata.ActualDocuments_Delete_by_DocID(new Guid(UID));
            //    if (cnt > 0)
            //    {
            //        BindDocuments(TreeView1.SelectedNode.Value);
            //    }
            //}
            //if (e.CommandName == "ViewDoc")
            //{
            //    string FilePath = Server.MapPath(UID);
            //    if (File.Exists(FilePath))
            //    {
            //        string getExtension = System.IO.Path.GetExtension(FilePath);
            //        string outPath = FilePath.Replace(getExtension, "") + "_download" + getExtension;
            //        getdata.DecryptFile(FilePath, outPath);
            //        //Response.Write("<script>window.open('" + outPath + "','_blank')</script>");
            //        WebClient User = new WebClient();
            //        Byte[] FileBuffer = User.DownloadData(outPath);
            //        if (FileBuffer != null)
            //        {
            //            Response.ContentType = "application/pdf";
            //            Response.AddHeader("content-length", FileBuffer.Length.ToString());
            //            Response.BinaryWrite(FileBuffer);
            //        }
            //    }
            //    else
            //    {
            //        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('File not found');</script>");
            //    }

            //}
        }

        protected void GrdActualDocuments_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            
        }


        protected void GrdActualDocuments_new_RowDataBound(object sender, GridViewRowEventArgs e)
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
                                Label lblDocumentName = (Label)e.Row.FindControl("lblDocumentName");
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
                                        string newVersionFileName = Path.GetFileNameWithoutExtension(Server.MapPath(ds.Tables[0].Rows[0]["Doc_Path"].ToString()));
                                        lblDocumentName.Text = newVersionFileName.Substring(0, (newVersionFileName.Length - 2));
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

                        

                        string[] foldernames = e.Row.Cells[9].Text.Split('/');
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
                        if (folder != e.Row.Cells[9].Text.Split('/')[correctindex].Trim().Replace("&amp;", "&"))
                        {
                            if (e.Row.Cells[10].Text.Trim().Replace("amp;", "") == ViewState["folder"].ToString())
                            {
                                if (e.Row.Cells[2].Text != "&nbsp;")
                                {
                                    ActualDocumentUID = e.Row.Cells[2].Text;
                                    DataSet ds = getdata.getTop1_DocumentStatusSelect(new Guid(e.Row.Cells[2].Text));
                                    if (ds != null)
                                    {
                                        int delay = 0;
                                        Label lblVer = (Label)e.Row.FindControl("LblVersion");
                                        Label lblDocumentName = (Label)e.Row.FindControl("lblDocumentName");
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
                                                string newVersionFileName = Path.GetFileNameWithoutExtension(Server.MapPath(ds.Tables[0].Rows[0]["Doc_Path"].ToString()));
                                                lblDocumentName.Text = newVersionFileName.Substring(0, (newVersionFileName.Length - 2));
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

                                //for db sync check
                                if (WebConfigurationManager.AppSettings["Dbsync"] == "Yes")
                                {
                                    if (!string.IsNullOrEmpty(ActualDocumentUID))
                                    {
                                        if (getdata.checkDocumentsFlagdbsync(new Guid(ActualDocumentUID)) == 1)
                                        {
                                            e.Row.BackColor = System.Drawing.Color.LightYellow;
                                        }
                                        else
                                        {
                                            //  e.Row.BackColor = System.Drawing.Color.Green;
                                            // e.Row.ForeColor = System.Drawing.Color.White;
                                        }
                                    }
                                }
                                //--------------------------------------------
                            }
                            else
                            {
                                //e.Row.Attributes["style"] = "display:none";
                                if (correctindex != 0)
                                {
                                    if ((e.Row.Cells[9].Text.Trim().Replace("amp;", "").Contains(ViewState["folder"].ToString())))
                                    {
                                        HtmlGenericControl htmlDivControl = e.Row.FindControl("divPDF") as HtmlGenericControl;
                                        htmlDivControl.Visible = false;
                                        LinkButton lnkfolder = (LinkButton)e.Row.FindControl("LinkButton2");
                                        lnkfolder.Text = e.Row.Cells[9].Text.Split('/')[correctindex];
                                        e.Row.Cells[3].Text = "";
                                        // e.Row.Cells[0].Text = "<a href='#' onclick='someFunction()'><img src='/_assets/images/folder.jpg' height='15px' width='20px'>  " + e.Row.Cells[7].Text.Split('/')[0] + "</a>";
                                        //e.Row.Cells[0].Text = "<asp:LinkButton ID = \"LinkButton3\" runat = \"server\" CausesValidation = \"False\" CommandName = \"Edit\" Text = \"Edit\" ></ asp:LinkButton >";
                                        e.Row.Cells[1].Text = "";
                                        e.Row.Cells[2].Text = "";
                                        e.Row.Cells[4].Text = "";
                                        e.Row.Cells[5].Text = "";
                                        e.Row.Cells[6].Text = "";
                                        e.Row.Cells[7].Text = "";
                                        e.Row.Cells[8].Text = "";
                                        folder = e.Row.Cells[9].Text.Trim().Split('/')[correctindex].Replace("amp;", ""); 
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

                    if (ActualDocumentUID != "")
                    {
                        if (Session["copydocument"] != null)
                        {
                            getdata.sfinallist = (List<CopyDocumentFile>)Session["copydocument"];
                            if (getdata.sfinallist.Any(x => x.DocumentUID == new Guid(ActualDocumentUID)))
                            {
                                LinkButton lnkcopy = (LinkButton)e.Row.FindControl("lnkcopy1");
                                lnkcopy.Enabled = false;
                            }
                        }
                    }
                }



            }
            catch (Exception ex)
            {
                string Error = ex.Message;
            }
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    if (e.Row.Cells[2].Text != "&nbsp;")
            //    {
            //        DataSet ds = getdata.getTop1_DocumentStatusSelect(new Guid(e.Row.Cells[2].Text));
            //        if (ds != null)
            //        {
            //            int delay = getdata.GetDelayed_Actual_Documents(new Guid(e.Row.Cells[2].Text), ds.Tables[0].Rows[0]["ActivityType"].ToString());

            //            if (ds.Tables[0].Rows[0]["DocumentType"].ToString() == "General Document")
            //            {
            //                Label lblVer = (Label)e.Row.FindControl("LblVersion");
            //                lblVer.Text = "[ Ver. 1 ]";
            //                e.Row.Cells[2].Text = "Submitted";
            //                e.Row.Cells[4].Text = "No History";
            //            }
            //            else
            //            {
            //                Label lblVer = (Label)e.Row.FindControl("LblVersion");
            //                e.Row.Cells[2].Text = ds.Tables[0].Rows[0]["ActivityType"].ToString();
            //                if (ds.Tables[0].Rows[0]["ActivityType"].ToString() != "" && ds.Tables[0].Rows[0]["TopVersion"].ToString() != "")
            //                {
            //                    //e.Row.Cells[1].Text = ds.Tables[0].Rows[0]["TopVersion"].ToString();

            //                    lblVer.Text = "[ Ver. " + ds.Tables[0].Rows[0]["TopVersion"].ToString() + " ]";
            //                }
            //                else
            //                {
            //                    lblVer.Text = "[ Ver. 1 ]";
            //                }
            //            }
            //            if (e.Row.Cells[2].Text == "BWSSB Approved" || e.Row.Cells[2].Text == "Approved" || e.Row.Cells[2].Text == "BWSSB Approve")
            //            {
            //                //e.Row.Cells[13].BackColor = System.Drawing.Color.Green;
            //                e.Row.Cells[2].ForeColor = System.Drawing.Color.Green;
            //                e.Row.Cells[2].Font.Bold = true;
            //            }
            //            if (delay == 1)
            //            {
            //                e.Row.Cells[2].BackColor = System.Drawing.Color.Red;
            //                e.Row.Cells[2].ForeColor = System.Drawing.Color.White;
            //            }
            //        }

            //    }
            //    //if (e.Row.Cells[1].Text == "Cover Letter" || e.Row.Cells[1].Text == "General Document")
            //    //{
            //    //    HtmlAnchor lnkVoucher = e.Row.FindControl("ViewDoc") as HtmlAnchor;
            //    //    //HtmlAnchor link = e.Row.Cells[0].Controls[0] as HtmlAnchor;
            //    //    if (lnkVoucher != null)
            //    //    {
            //    //        lnkVoucher.HRef = "javascript:void(0)";
            //    //    }


            //    //    //HyperLink myHyperLink = e.Row.FindControl("ViewDoc") as HyperLink;
            //    //    //myHyperLink.hr
            //    //}
            //}
        }

        protected void GrdActualDocuments_new_RowCommand(object sender, GridViewCommandEventArgs e)
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
                    filename = Path.GetFileName(ds.Tables[0].Rows[0]["Doc_FileName"].ToString());
                }
                else
                {
                    ds1 = getdata.ActualDocuments_SelectBy_ActualDocumentUID(new Guid(UID));
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        if (ds1.Tables[0].Rows[0]["ActualDocument_Path"].ToString() != "")
                        {
                            path = Server.MapPath(ds1.Tables[0].Rows[0]["ActualDocument_Path"].ToString());
                            filename = ds1.Tables[0].Rows[0]["ActualDocument_Name"].ToString() + ds1.Tables[0].Rows[0]["ActualDocument_Type"].ToString();
                        }
                    }
                }
                // added on 20/10/2020
                //ds.Clear();
                //ds = getdata.ActualDocuments_SelectBy_ActualDocumentUID(new Guid(UID));
                //if (ds.Tables[0].Rows.Count > 0)
                //{
                //    if (ds.Tables[0].Rows[0]["ActualDocument_Name"].ToString() != "")
                //    {
                //        filename = ds.Tables[0].Rows[0]["ActualDocument_Name"].ToString() + ds.Tables[0].Rows[0]["ActualDocument_Type"].ToString();
                //    }
                //}
                //
                try
                {
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
                catch (Exception ex)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Document not uploaded for this flow.Please Contact Document controller!');</script>");
                }
            }

            if (e.CommandName == "delete")
            {
                int cnt = getdata.ActualDocuments_Delete_by_DocID(new Guid(UID), new Guid(Session["UserUID"].ToString()));
                if (cnt > 0)
                {
                    BindDocuments(TreeView1.SelectedNode.Value);

                    //GridViewRow gvRowParent = (((GridView)sender)).DataItemContainer as GridViewRow;
                    //string orderId = (gvRowParent.NamingContainer as GridView).DataKeys[gvRowParent.RowIndex].Value.ToString();
                    //GridView gvrdChild = ((GridView)sender);
                    //BindActualDocuments(TreeView1.SelectedNode.Value, gvrdChild);

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
                GridViewRow row = GrdActualDocuments_new.Rows[rowIndex];
                LinkButton lnkfolder = (LinkButton)row.FindControl("LinkButton2");
                string name = lnkfolder.Text.Trim().Replace("amp;", "");
                UID = row.Cells[11].Text;
                for (int i = 0; i < TreeView1.Nodes.Count; i++)
                {
                    retrieveNodes_2(TreeView1.Nodes[i], UID, name);
                }

                BindDocuments(UID);
                //string folder = 
            }

            if (e.CommandName == "copyfile")
            {
                LinkButton ctrl = e.CommandSource as LinkButton;
                if (ctrl != null)
                {
                    GridViewRow row = ctrl.Parent.NamingContainer as GridViewRow;
                    LinkButton lnkcopy = (LinkButton)row.FindControl("lnkcopy1");
                    lnkcopy.Enabled = false;
                }

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

                LblcopyFileCount.Text = "Copied File/s (" + getdata.sfinallist.Count + ")";
                LblcopyFileCount.Visible = true;
            }
            //string UID = e.CommandArgument.ToString();
            //if (e.CommandName == "download")
            //{
            //    string path = string.Empty;
            //    DataSet ds = getdata.getLatest_DocumentVerisonSelect(new Guid(UID));
            //    if (ds.Tables[0].Rows.Count > 0)
            //    {
            //        path = Server.MapPath(ds.Tables[0].Rows[0]["Doc_FileName"].ToString());
            //        //File.Decrypt(path);
            //    }
            //    else
            //    {
            //        DataSet ds1 = getdata.ActualDocuments_SelectBy_ActualDocumentUID(new Guid(UID));
            //        if (ds1.Tables[0].Rows.Count > 0)
            //        {
            //            if (ds1.Tables[0].Rows[0]["ActualDocument_Path"].ToString() != "")
            //            {
            //                path = Server.MapPath(ds1.Tables[0].Rows[0]["ActualDocument_Path"].ToString());
            //            }
            //        }
            //    }
            //    string getExtension = System.IO.Path.GetExtension(path);
            //    string outPath = path.Replace(getExtension, "") + "_download" + getExtension;
            //    getdata.DecryptFile(path, outPath);
            //    System.IO.FileInfo file = new System.IO.FileInfo(outPath);

            //    if (file.Exists)
            //    {
            //        int Cnt = getdata.DocumentHistroy_InsertorUpdate(Guid.NewGuid(), new Guid(UID), new Guid(Session["UserUID"].ToString()), "Downloaded");
            //        if (Cnt <= 0)
            //        {
            //            Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code: DDH-01. there is a problem with updating histroy. Please contact system admin.');</script>");
            //        }
            //        Response.Clear();

            //        Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);

            //        Response.AddHeader("Content-Length", file.Length.ToString());

            //        Response.ContentType = "application/octet-stream";

            //        Response.WriteFile(file.FullName);

            //        Response.End();

            //    }
            //    else
            //    {

            //        //Response.Write("This file does not exist.");
            //        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('File does not exists.');</script>");

            //    }
            //}

            //if (e.CommandName == "delete")
            //{
            //    int cnt = getdata.ActualDocuments_Delete_by_DocID(new Guid(UID));
            //    if (cnt > 0)
            //    {
            //        BindDocuments(TreeView1.SelectedNode.Value);
            //    }
            //}
            //if (e.CommandName == "ViewDoc")
            //{
            //    string FilePath = Server.MapPath(UID);
            //    if (File.Exists(FilePath))
            //    {
            //        string getExtension = System.IO.Path.GetExtension(FilePath);
            //        string outPath = FilePath.Replace(getExtension, "") + "_download" + getExtension;
            //        getdata.DecryptFile(FilePath, outPath);
            //        //Response.Write("<script>window.open('" + outPath + "','_blank')</script>");
            //        WebClient User = new WebClient();
            //        Byte[] FileBuffer = User.DownloadData(outPath);
            //        if (FileBuffer != null)
            //        {
            //            Response.ContentType = "application/pdf";
            //            Response.AddHeader("content-length", FileBuffer.Length.ToString());
            //            Response.BinaryWrite(FileBuffer);
            //        }
            //    }
            //    else
            //    {
            //        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('File not found');</script>");
            //    }

            //}
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
                    //GridView gvrdChild = ((GridView)sender);
                    //BindActualDocuments(TreeView2.SelectedNode.Value, gvrdChild);
                    //BindDocuments(TreeView2.SelectedNode.Value);
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

                LblcopyFileCount.Text = "Copied File/s (" + getdata.sfinallist.Count + ")";
                LblcopyFileCount.Visible = true;
            }
        }

        protected void GrdActualDocuments_new_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

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
                            Label lblDocumentName = (Label)e.Row.FindControl("lblDocumentName");
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
                                    string newVersionFileName = Path.GetFileNameWithoutExtension(Server.MapPath(ds.Tables[0].Rows[0]["Doc_Path"].ToString()));
                                    lblDocumentName.Text = newVersionFileName.Substring(0, (newVersionFileName.Length - 2));
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

                        //for db sync check
                        if (WebConfigurationManager.AppSettings["Dbsync"] == "Yes")
                        {
                            if (!string.IsNullOrEmpty(ActualDocumentUID))
                            {
                                if (getdata.checkDocumentsFlagdbsync(new Guid(ActualDocumentUID)) == 1)
                                {
                                    e.Row.BackColor = System.Drawing.Color.LightYellow;
                                }
                                else
                                {
                                    //  e.Row.BackColor = System.Drawing.Color.Green;
                                    // e.Row.ForeColor = System.Drawing.Color.White;
                                }
                            }
                        }
                        //--------------------------------------------
                        if (Session["IsContractor"].ToString() == "Y")
                        {
                            string SubmittalUID = getdata.GetSubmittalUID_By_ActualDocumentUID(new Guid(ActualDocumentUID));
                            string phase = getdata.GetPhaseforStatus(new Guid(getdata.GetFlowUIDBySubmittalUID(new Guid(SubmittalUID))), e.Row.Cells[2].Text);

                            if (string.IsNullOrEmpty(phase))
                            {
                                
                                if (e.Row.Cells[2].Text == "Code A-CE Approval")
                                {
                                    e.Row.Cells[2].Text = "Approved";

                                }
                                if (e.Row.Cells[2].Text == "Code B-CE Approval" || e.Row.Cells[3].Text == "Code C-CE Approval")
                                {
                                    e.Row.Cells[2].Text = "Client Approval";
                                }
                            }
                            else
                            {
                                e.Row.Cells[2].Text = phase;
                            }


                        }

                        //
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
                //if (e.Row.Cells[1].Text == "Cover Letter" || e.Row.Cells[1].Text == "General Document")
                //{
                //    HtmlAnchor lnkVoucher = e.Row.FindControl("ViewDoc") as HtmlAnchor;
                //    //HtmlAnchor link = e.Row.Cells[0].Controls[0] as HtmlAnchor;
                //    if (lnkVoucher != null)
                //    {
                //        lnkVoucher.HRef = "javascript:void(0)";
                //    }


                //    //HyperLink myHyperLink = e.Row.FindControl("ViewDoc") as HyperLink;
                //    //myHyperLink.hr
                //}
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
                int cnt = getdata.ActualDocuments_Delete_by_DocID(new Guid(UID),new Guid(Session["UserUID"].ToString()));
                if (cnt > 0)
                {
                    //BindDocuments_By_Category(TreeView2.SelectedNode.Value);

                    GridViewRow gvRowParent = (((GridView)sender)).DataItemContainer as GridViewRow;
                    string orderId = (gvRowParent.NamingContainer as GridView).DataKeys[gvRowParent.RowIndex].Value.ToString();
                    GridView gvrdChild = ((GridView)sender);
                    BindActualDocuments(orderId, gvrdChild);

                    ScriptManager.RegisterClientScriptBlock(sender as Control, this.GetType(), "alert", "expand2()", true);
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
                //string folder = 
            }
        }

        protected void GrdActualDocumentsCategory_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        public string GetStatus(string sStatus)
        {
            if (sStatus == "P")
            {
                return "Not Started";
            }
            else if (sStatus == "I")
            {
                return "In-Progress";
            }
            else
            {
                return "Completed";
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

        public string ShoworHide(string Desc)
        {
            if (Desc.Length > 50)
            {
                return "More";
            }
            else
            {
                return string.Empty;
            }
        }

        protected void RDBDocumentView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RDBDocumentView.SelectedValue == "Category")
            {
                ByCategory.Visible = true;
                ByActivity.Visible = false;
                BindTreeview_by_Category();
                if (TreeView2.SelectedNode !=null)
                {
                    BindDocuments_By_Category(TreeView2.SelectedNode.Value);
                }
            }
            else
            {   
                BindTreeview();
                if (TreeView1.SelectedNode !=null)
                {
                    BindDocuments(TreeView1.SelectedNode.Value);
                }
                ByCategory.Visible = false;
                ByActivity.Visible = true;
            }
        }

        protected void TreeView2_SelectedNodeChanged(object sender, EventArgs e)
        {
            BindDocuments_By_Category(TreeView2.SelectedNode.Value);
        }

        public void GetWorkPackage_by_UID(Guid WorkPackageID)
        {
            DataSet ds = getdata.GetWorkPackages_By_WorkPackageUID(WorkPackageID);
            if (ds.Tables[0].Rows.Count > 0)
            {
                LblWorkPackageName.Text = ds.Tables[0].Rows[0]["Name"].ToString();
                LblWorkPackageLocation.Text = ds.Tables[0].Rows[0]["WorkPackage_Location"].ToString();
                LblWorkPackageClient.Text = ds.Tables[0].Rows[0]["WorkPackage_Client"].ToString();
                LblWorkPackageStatus.Text = GetStatus(ds.Tables[0].Rows[0]["Status"].ToString());
                LblWorkPackageCurrency.Text = ds.Tables[0].Rows[0]["Currency"].ToString();
                LblWorkPackageBudget.Text = Convert.ToDouble(ds.Tables[0].Rows[0]["Budget"].ToString()).ToString("#,##.00", CultureInfo.CreateSpecificCulture(ds.Tables[0].Rows[0]["Currency_CultureInfo"].ToString()));
               
            }
        }

        public string GetActivityName(string ActivityID)
        {
            string aName = getdata.getTaskNameby_TaskUID(new Guid(ActivityID));
            return aName;
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
        public string getCategoryName(string CategoryUID)
        {
            return getdata.GetWorkpackageCategory_By_UID(new Guid(CategoryUID));
        }
        private void LoadProjectDetails(string ProjectUID)
        {
            DataSet ds = new DataSet();
            ds = getdata.GetProject_by_ProjectUID(new Guid(ProjectUID));
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
            }
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
                    if (SearchResultCount == 0)
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('" + TxtSearchDocuments.Value + " not found.');</script>");
                        TxtSearchDocuments.Value = "";
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

        public string GetWorkpackageOptionName(string Workpackage_OptionUID)
        {
            return getdata.WorkpackageoptionName_SelectBy_UID(new Guid(Workpackage_OptionUID));
        }

        protected void GrdOptions_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string UID = e.CommandArgument.ToString();
            if (e.CommandName == "view")
            {
                TreeView1.CollapseAll();
                DataSet ds = getdata.GetWorkpackage_SelectOption_by_UID(new Guid(UID));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < TreeView1.Nodes.Count; i++)
                    {
                        retrieveNodes_Options(TreeView1.Nodes[i], ds.Tables[0].Rows[0]["Workpackage_OptionUID"].ToString(), ds.Tables[0].Rows[0]["WorkPackageUID"].ToString());
                    }
                }
                //DataSet ds = getdata.GetWorkPackages_By_WorkPackageUID(new Guid(UID));
                //if (ds.Tables[0].Rows.Count > 0)
                //{
                //    for (int i = 0; i < TreeView1.Nodes.Count; i++)
                //    {
                //        retrieveNodes_by_Parent(TreeView1.Nodes[i], ds.Tables[0].Rows[0]["Workpackage_OptionUID"].ToString(), UID);
                //    }
                //}

            }
        }

        protected void retrieveNodes_by_Parent(TreeNode node, string SelectedVal, string WorkpackageValue)
        {
            if (node.Value == SelectedVal && node.Parent.Value == WorkpackageValue)
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
                        if (tn.Value == SelectedVal && tn.Parent.Value == WorkpackageValue)
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
                                retrieveNodes_by_Parent(tn, SelectedVal, WorkpackageValue);
                            }
                        }
                    }
                }
            }
        }
        //protected void TreeView1_TreeNodeDataBound(object sender, TreeNodeEventArgs e)
        //{
        //    e.Node.ImageUrl = "~/_assets/images/tab_right_active.gif";
        //}

        // added on 23/02/2020 by zuber

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
        protected void retrieveNodes_2(TreeNode node, string SelectedVal, string name)
        {
            if (node.Value == SelectedVal && node.Text.ToLower() == name.ToLower())
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
                                retrieveNodes_2(tn, SelectedVal, name.ToLower());
                            }
                        }
                    }
                }
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
                            BindDocuments_By_Category(TreeView2.SelectedNode.Value);
                        }
                        else
                        {
                            if (tn.ChildNodes.Count != 0)
                            {
                                retrieveNodesCategory_2(tn, SelectedVal,name.ToLower());
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

        private void BindTreeView_GeneralDocuments()
        {
            if(WebConfigurationManager.AppSettings["Domain"] == "ONTB" && Session["IsContractor"].ToString() != "Y")
            { 
            TreeView3.Nodes.Clear();
            DataSet ds = GD.GetGeneralDocumentStructure_TopLevel();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    Populate_GeneralDocumentsTreeview(ds, null, "");

                    if (Session["GeneralDocumentStructureUID"] != null)
                    {
                        if (TreeView1.SelectedNode != null)
                        {
                            TreeView1.SelectedNode.Selected = false;
                        }
                        TreeView3.CollapseAll();

                        string UID = Session["GeneralDocumentStructureUID"].ToString();
                        for (int i = 0; i < TreeView3.Nodes.Count; i++)
                        {
                            retrieveNodes_For_GeneralDocument(TreeView3.Nodes[i], UID);
                        }

                        DocumentGrid.Visible = false;
                        TreeGrid.Visible = false;
                        GeneralDocumentsDiv.Visible = true;
                        Session["GeneralDocumentStructureUID"] = null;
                    }
                    else
                    {
                        TreeView3.CollapseAll();
                        TreeView3.Nodes[0].Expand();
                    }
                }
            }
            else if (WebConfigurationManager.AppSettings["Domain"] == "NJSEI")
            { 
                if(DDlProject.SelectedItem.ToString() == "Bangalore Projects")
                { 
                TreeView3.Nodes.Clear();
            DataSet ds = GD.GetGeneralDocumentStructure_TopLevel();
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        Populate_GeneralDocumentsTreeview(ds, null, "");

                        if (Session["GeneralDocumentStructureUID"] != null)
                        {
                            if (TreeView1.SelectedNode != null)
                            {
                                TreeView1.SelectedNode.Selected = false;
                            }
                            TreeView3.CollapseAll();

                            string UID = Session["GeneralDocumentStructureUID"].ToString();
                            for (int i = 0; i < TreeView3.Nodes.Count; i++)
                            {
                                retrieveNodes_For_GeneralDocument(TreeView3.Nodes[i], UID);
                            }

                            DocumentGrid.Visible = false;
                            TreeGrid.Visible = false;
                            GeneralDocumentsDiv.Visible = true;
                            Session["GeneralDocumentStructureUID"] = null;
                        }
                        else
                        {
                            TreeView3.CollapseAll();
                            TreeView3.Nodes[0].Expand();
                        }
                    }
                }
            }
        }
        protected void TreeView3_SelectedNodeChanged(object sender, EventArgs e)
        {
            DocumentGrid.Visible = false;
            TreeGrid.Visible = false;
            GeneralDocumentsDiv.Visible = true;
            if (TreeView1.SelectedNode != null)
            {
                TreeView1.SelectedNode.Selected = false;
            }
            if (TreeView3.SelectedNode.Parent == null)
            {
                AddGeneralDocument.Visible = false;
            }
            else
            {
                AddGeneralDocument.Visible = true;
            }
            BindGeneralDocumentFolderStructure(TreeView3.SelectedNode.Value);
            BindGeneralDocuments(TreeView3.SelectedNode.Value);
        }

        private void Populate_GeneralDocumentsTreeview(DataSet dtParent, TreeNode treeNode, string ParentUID)
        {
            foreach (DataRow row in dtParent.Tables[0].Rows)
            {
                TreeNode child = new TreeNode
                {
                    Text = row["Structure_Name"].ToString(),
                    Value = row["StructureUID"].ToString(),
                    Target = "Sub Folder",
                    ToolTip = row["Structure_Name"].ToString(),
                };

                if (ParentUID == "")
                {
                    LblGeneralDocumentHeading.Text = child.Text;
                    TreeView3.Nodes.Add(child);
                    DataSet dsSubFolder = GD.GetGeneralDocumentStructure_By_ParentStructureUID(new Guid(child.Value));
                    if (dsSubFolder.Tables[0].Rows.Count > 0)
                    {
                        Populate_GeneralDocumentsTreeview(dsSubFolder, child, child.Value);
                    }
                }
                else
                {
                    LblGeneralDocumentHeading.Text = child.Text;
                    treeNode.ChildNodes.Add(child);
                    DataSet dsSubFolder = GD.GetGeneralDocumentStructure_By_ParentStructureUID(new Guid(child.Value));
                    if (dsSubFolder.Tables[0].Rows.Count > 0)
                    {
                        Populate_GeneralDocumentsTreeview(dsSubFolder, child, child.Value);
                    }
                }
            }
        }
        private void BindGeneralDocumentFolderStructure(string StructureUID)
        {
            LblGeneralDocumentHeading.Text = TreeView3.SelectedNode.Text;
            DataSet ds = GD.GetGeneralDocumentStructure_By_ParentStructureUID(new Guid(StructureUID));
            GrdGeneralDocumentStructure.DataSource = ds;
            GrdGeneralDocumentStructure.DataBind();
            AddGeneralDocumentStructure.HRef = "/_modal_pages/add-generaldocumentstructure.aspx?ParentUID=" + StructureUID;
            AddGeneralDocument.HRef = "/_modal_pages/add-general-document.aspx?StructureUID=" + StructureUID + "&PrJUID=" + DDlProject.SelectedValue;
        }
        private void BindGeneralDocuments(string StructureUID)
        {
            DataSet ds = GD.GeneralDocuments_SelectBy_StructureUID(new Guid(StructureUID));
            GrdGeneralDocuments.DataSource = ds;
            GrdGeneralDocuments.DataBind();
        }

        protected void retrieveNodes_For_GeneralDocument(TreeNode node, string SelectedVal)
        {
            if (node.Value == SelectedVal)
            {
                node.Selected = true;
                node.Expand();
                node.Parent.Expand();
                BindGeneralDocumentFolderStructure(SelectedVal);
                BindGeneralDocuments(SelectedVal);
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
                            BindGeneralDocumentFolderStructure(SelectedVal);
                            BindGeneralDocuments(SelectedVal);
                        }
                        else
                        {
                            if (tn.ChildNodes.Count != 0)
                            {
                                retrieveNodes_For_GeneralDocument(tn, SelectedVal);
                            }
                        }
                    }
                }
            }
        }

        protected void GrdGeneralDocumentStructure_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string UID = e.CommandArgument.ToString();
            if (e.CommandName == "delete")
            {
                int cnt = GD.GeneralDocumentStructure_Delete(new Guid(UID), new Guid(Session["UserUID"].ToString()));
                if (cnt > 0)
                {
                    BindGeneralDocumentFolderStructure(TreeView3.SelectedNode.Value);
                }
            }
            if (e.CommandName == "view")
            {
                TreeView3.CollapseAll();
                for (int i = 0; i < TreeView3.Nodes.Count; i++)
                {
                    retrieveNodes_For_GeneralDocument(TreeView3.Nodes[i], UID);
                }
            }
        }

        protected void GrdGeneralDocumentStructure_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void GrdGeneralDocuments_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string UID = e.CommandArgument.ToString();
            if (e.CommandName == "delete")
            {
                int cnt = GD.GeneralDocument_Delete(new Guid(UID), new Guid(Session["UserUID"].ToString()));
                if (cnt > 0)
                {
                    BindGeneralDocuments(TreeView3.SelectedNode.Value);
                }
            }
            if (e.CommandName == "download")
            {
                string path = string.Empty;
                DataSet ds = GD.GeneralDocuments_SelectBy_GeneralDocumentUID(new Guid(UID));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    path = Server.MapPath(ds.Tables[0].Rows[0]["GeneralDocument_Path"].ToString());

                    string getExtension = System.IO.Path.GetExtension(path);
                    string outPath = path.Replace(getExtension, "") + "_download" + getExtension;
                    getdata.DecryptFile(path, outPath);
                    System.IO.FileInfo file = new System.IO.FileInfo(outPath);

                    if (file.Exists)
                    {

                        int Cnt = getdata.DocumentHistroy_InsertorUpdate(Guid.NewGuid(), new Guid(UID), new Guid(Session["UserUID"].ToString()), "Downloaded", "General Documents");
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
            }
        }

        protected void GrdGeneralDocuments_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

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

                                //for db sync check
                                if (WebConfigurationManager.AppSettings["Dbsync"] == "Yes")
                                {
                                    if (!string.IsNullOrEmpty(ActualDocumentUID))
                                    {
                                        if (getdata.checkDocumentsFlagdbsync(new Guid(ActualDocumentUID)) == 1)
                                        {
                                            e.Row.BackColor = System.Drawing.Color.LightYellow;
                                        }
                                        else
                                        {
                                            //  e.Row.BackColor = System.Drawing.Color.Green;
                                            // e.Row.ForeColor = System.Drawing.Color.White;
                                        }
                                    }
                                }
                                //--------------------------------------------
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

        protected void GrdDocumentByCategory_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string DocumentUID = GrdDocumentByCategory.DataKeys[e.Row.RowIndex].Values[0].ToString();

                // for db sync checking
                if (WebConfigurationManager.AppSettings["Dbsync"] == "Yes")
                {
                    bool isset = getdata.checkdbsyncflag(new Guid(DocumentUID), "Documents", "DocumentUID");
                    if (isset == true)
                    {
                        e.Row.BackColor = System.Drawing.Color.LightYellow;
                    }
                    else
                    {
                        //  e.Row.BackColor = System.Drawing.Color.Green;
                        // e.Row.ForeColor = System.Drawing.Color.White;
                    }
                }
            }
        }

        
      
    }
}