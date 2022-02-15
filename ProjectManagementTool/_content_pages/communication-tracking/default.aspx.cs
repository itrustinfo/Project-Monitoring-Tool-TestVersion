using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagementTool._content_pages.communication_tracking
{
    public partial class _default : System.Web.UI.Page
    {
        DBGetData getdt = new DBGetData();
        TaskUpdate gettk = new TaskUpdate();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Username"] == null)
            {
                Response.Redirect("~/Login.aspx");
            }
            else
            {
                //ScriptManager.RegisterStartupScript(
                //       UpdatePanel2,
                //       this.GetType(),
                //       "MyAction",
                //       "expand1();",
                //       true);
                if (!IsPostBack)
                {
                    BindProject();
                    SelectedProjectWorkpackage("Project");
                    DDlProject_SelectedIndexChanged(sender, e);
                }
            }
        }

        private void BindProject()
        {
            DataTable ds = new DataTable();
            if (Session["TypeOfUser"].ToString() == "U" || Session["TypeOfUser"].ToString() =="MD" || Session["TypeOfUser"].ToString() == "VP")
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
                //ds = getdt.GetWorkPackages_By_ProjectUID(new Guid(DDlProject.SelectedValue));
                if (Session["TypeOfUser"].ToString() == "U" || Session["TypeOfUser"].ToString() == "MD" || Session["TypeOfUser"].ToString() == "VP")
                {
                    ds = getdt.GetWorkPackages_By_ProjectUID(new Guid(DDlProject.SelectedValue));
                }
                else if (Session["TypeOfUser"].ToString() == "PA")
                {
                    ds = getdt.GetWorkPackages_ForUser_by_ProjectUID(new Guid(Session["UserUID"].ToString()), new Guid(DDlProject.SelectedValue));
                }
                else
                {
                    ds = getdt.GetWorkPackages_ForUser_by_ProjectUID(new Guid(Session["UserUID"].ToString()), new Guid(DDlProject.SelectedValue));
                }
                DDLWorkPackage.DataTextField = "Name";
                DDLWorkPackage.DataValueField = "WorkPackageUID";
                DDLWorkPackage.DataSource = ds;
                DDLWorkPackage.DataBind();
                SelectedProjectWorkpackage("Workpackage");
                if (DDLWorkPackage.SelectedValue != "")
                {
                    BindCommunicationDocuments(DDlProject.SelectedValue, DDLWorkPackage.SelectedValue);
                }
                Session["Project_Workpackage"] = DDlProject.SelectedValue + "_" + DDLWorkPackage.SelectedValue;
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
                        DDlProject.SelectedValue = selectedValue[0];
                    }
                    else
                    {
                        DDLWorkPackage.SelectedValue = selectedValue[1];
                    }
                }
                else
                {
                    if (pType == "Project")
                    {
                        DDlProject.SelectedValue = Session["Project_Workpackage"].ToString();
                    }
                }
            }

        }

        protected void DDLWorkPackage_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDLWorkPackage.SelectedValue != "")
            {
                BindCommunicationDocuments(DDlProject.SelectedValue, DDLWorkPackage.SelectedValue);
                Session["Project_Workpackage"] = DDlProject.SelectedValue + "_" + DDLWorkPackage.SelectedValue;
            }
        }
        private void BindCommunicationDocuments(string ProjectID, string WorkPackageID)
        {
            DataSet ds = getdt.GetActualProjectCommunicationDocuments(new Guid(ProjectID), new Guid(WorkPackageID));
            GrdCommunicationDocs.DataSource = ds;
            GrdCommunicationDocs.DataBind();
            if (GrdCommunicationDocs.Rows.Count > 10)
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "Key", "<script>MakeStaticHeaderNew('" + GrdCommunicationDocs.ClientID + "', 600, 1300 , 55 ,false); </script>", false);
            }
        }

        protected void GrdCommunicationDocs_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdCommunicationDocs.PageIndex = e.NewPageIndex;
            BindCommunicationDocuments(DDlProject.SelectedValue, DDLWorkPackage.SelectedValue);
        }

        protected void GrdCommunicationDocs_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string UID = e.CommandArgument.ToString();
            if (e.CommandName == "download")
            {
                string path = string.Empty;
                
                DataSet ds1 = getdt.ActualDocuments_SelectBy_ActualDocumentUID(new Guid(UID));
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    if (ds1.Tables[0].Rows[0]["ActualDocument_Path"].ToString() != "")
                    {
                        path = Server.MapPath(ds1.Tables[0].Rows[0]["ActualDocument_Path"].ToString());
                    }
                }
                string getExtension = System.IO.Path.GetExtension(path);
                string outPath = path.Replace(getExtension, "") + "_download" + getExtension;
                getdt.DecryptFile(path, outPath);
                System.IO.FileInfo file = new System.IO.FileInfo(outPath);

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
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('File not found.');</script>");

                }
            }
        }

        protected void GrdCommunicationDocs_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[3].Text != "")
                {
                    string dsstatus = getdt.GetDocumentStatu_by_CoverLetterUID(new Guid(e.Row.Cells[3].Text));
                    if (dsstatus != "")
                    {
                        e.Row.Cells[3].Text = dsstatus;
                    }
                }
                string DocUID = GrdCommunicationDocs.DataKeys[e.Row.RowIndex].Value.ToString();
                GridView GrdStatus = e.Row.FindControl("GrdStatus") as GridView;
                DataSet ds1 = getdt.GetDocumentStatus_by_CoverLetterUID(new Guid(DocUID));
                GrdStatus.DataSource = ds1;
                GrdStatus.DataBind();
            }
        }

        protected void Show_Hide_StatusGrid(object sender, EventArgs e)
        {
            Page currentPage = (Page)HttpContext.Current.Handler;
            ImageButton imgShowHide = (sender as ImageButton);
            GridViewRow row = (imgShowHide.NamingContainer as GridViewRow);
            if (imgShowHide.CommandArgument == "Show")
            {
                ScriptManager.RegisterStartupScript(currentPage, typeof(string), "Script" + Guid.NewGuid(), "expand1();", true);
                row.FindControl("pnlStatus").Visible = true;
                imgShowHide.CommandArgument = "Hide";
                imgShowHide.ImageUrl = "~/_assets/images/minus.png";
                //string DocUID = (row.NamingContainer as GridView).DataKeys[row.RowIndex].Value.ToString();
                //GridView GrdStatus = row.FindControl("GrdStatus") as GridView;
                //DataSet ds = getdt.getActualDocumentStatusList(new Guid(DocUID));
                //GrdStatus.DataSource = ds;
                //GrdStatus.DataBind();
                
            }
            else
            {
                ScriptManager.RegisterStartupScript(currentPage, typeof(string), "Script" + Guid.NewGuid(), "expand1();", true);
                row.FindControl("pnlStatus").Visible = false;
                imgShowHide.CommandArgument = "Show";
                imgShowHide.ImageUrl = "~/_assets/images/plus.png";
            }
        }

        protected void GrdStatus_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string StatusUID = e.CommandArgument.ToString();
            if (e.CommandName == "CoverLetterDownload")
            {
                DataSet ds = getdt.getDocumentStatusList_by_StatusUID(new Guid(StatusUID));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string path = Server.MapPath(ds.Tables[0].Rows[0]["CoverLetterFile"].ToString());

                    string getExtension = System.IO.Path.GetExtension(path);
                    string outPath = path.Replace(getExtension, "") + "_download" + getExtension;
                    getdt.DecryptFile(path, outPath);
                    System.IO.FileInfo file = new System.IO.FileInfo(outPath);

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
    }
}