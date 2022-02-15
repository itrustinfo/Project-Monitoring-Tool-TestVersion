using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManager._content_pages.assign_projects
{
    public partial class Default : System.Web.UI.Page
    {
        DBGetData getdata = new DBGetData();
        TaskUpdate gettk = new TaskUpdate();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["Username"] == null)
                {
                    Response.Redirect("~/Login.aspx");
                }
                else
                {
                    GetAssignProjects();
                }

            }
        }

        public void GetAssignProjects()
        {            
            DataSet ds = new DataSet();
            if (Session["TypeOfUser"].ToString() == "U" || Session["TypeOfUser"].ToString() =="MD" || Session["TypeOfUser"].ToString() == "VP")
            {
                ds = getdata.GetAllAssignedProjects();
                GrdProjects.DataSource = ds;
                GrdProjects.DataBind();
            }
            else
            {
                ds = getdata.GetAssignedProjectsData_by_UserUID(new Guid(Session["UserUID"].ToString()));
                GrdProjects.DataSource = ds;
                GrdProjects.DataBind();
            }

        }
        public string GetUserName(string uUID)
        {
            return getdata.getUserNameby_UID(new Guid(uUID));
        }
        public string GetProjectName(string pUID)
        {
            return getdata.getProjectNameby_ProjectUID(new Guid(pUID));
        }
        public string getUserType(string sType)
        {
            return getdata.GetUserRolesMasterDesc_by_UID(new Guid(sType));
        }

        protected void GrdProjects_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdProjects.PageIndex = e.NewPageIndex;
            GetAssignProjects();
        }
    }
}