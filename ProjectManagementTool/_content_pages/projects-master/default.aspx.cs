using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagementTool._content_pages.projects
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
                if (!IsPostBack)
                {
                    BindProjects();
                    ProjectCategoryBind();
                }
            }
        }
        private void BindProjects()
        {
            DataTable ds = new DataTable();
            if (Session["TypeOfUser"].ToString() == "U" || Session["TypeOfUser"].ToString() =="MD" || Session["TypeOfUser"].ToString() == "VP")
            {

                ds = gettk.GetProjects();
            }
            else if (Session["TypeOfUser"].ToString() == "PA")
            {

                //ds = gettk.GetProjects_by_UserUID(new Guid(Session["UserUID"].ToString()));
                ds = gettk.GetProjectsData_By_UserUID(new Guid(Session["UserUID"].ToString()));
            }
            else
            {

                ds = gettk.GetProjectsData_By_UserUID(new Guid(Session["UserUID"].ToString()));
            }
            GrdProject.DataSource = ds;
            GrdProject.DataBind();
        }

        protected void GrdProject_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdProject.PageIndex = e.NewPageIndex;
            BindProjects();
        }

        private void ProjectCategoryBind()
        {
            DataSet ds = getdt.ProjectClass_Select_All();
            GrdProjectClass.DataSource = ds;
            GrdProjectClass.DataBind();
        }
        protected void GrdProjectClass_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdProjectClass.PageIndex = e.NewPageIndex;
            ProjectCategoryBind();
        }

        protected void GrdProject_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string UID = e.CommandArgument.ToString();
            if (e.CommandName == "delete")
            {
                int Cnt = getdt.Project_Delete(new Guid(UID), new Guid(Session["UserUID"].ToString()));
                if (Cnt > 0)
                {
                    BindProjects();
                }
            }
        }

        protected void GrdProject_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void GrdProjectClass_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string UID = e.CommandArgument.ToString();
            if (e.CommandName == "delete")
            {
                int Cnt = getdt.ProjectClass_Delete(new Guid(UID), new Guid(Session["UserUID"].ToString()));
                if (Cnt > 0)
                {
                    ProjectCategoryBind();
                }
            }
        }

        protected void GrdProjectClass_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }
    }
}