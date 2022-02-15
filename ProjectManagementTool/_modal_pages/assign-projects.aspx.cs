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
    public partial class assign_projects : System.Web.UI.Page
    {
        DBGetData getdata = new DBGetData();
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
                    BindProject();
                    BindUsers();
                    BindUserRoleMaster();
                    if (Request.QueryString["AssignID"] != null)
                    {
                        GetAssignProject_By_AssignUID(new Guid(Request.QueryString["AssignID"]));
                    }
                }
            }
        }

        private void GetAssignProject_By_AssignUID(Guid AssignID)
        {
            DataSet ds = getdata.GetAssignedProjects_by_AssignUID(AssignID);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DDLProject.SelectedValue = ds.Tables[0].Rows[0]["ProjectUID"].ToString();
                DDLUser.SelectedValue = ds.Tables[0].Rows[0]["UserUID"].ToString();
                DDlUserType.SelectedValue = ds.Tables[0].Rows[0]["UserRole"].ToString();
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

                //ds = gettk.GetProjects_by_UserUID(new Guid(Session["UserUID"].ToString()));
                ds = gettk.GetAssignedProject_by_UserUID(new Guid(Session["UserUID"].ToString()));
            }
            else
            {

                ds = gettk.GetAssignedProject_by_UserUID(new Guid(Session["UserUID"].ToString()));
            }
            DDLProject.DataTextField = "ProjectName";
            DDLProject.DataValueField = "ProjectUID";
            DDLProject.DataSource = ds;
            DDLProject.DataBind();

        }
        private void BindUsers()
        {
            DataSet ds = new DataSet();
            if (Session["TypeOfUser"].ToString() == "U" || Session["TypeOfUser"].ToString() =="MD" || Session["TypeOfUser"].ToString() == "VP")
            {
                ds = getdata.getAllUsers();
            }
            else if (Session["TypeOfUser"].ToString() == "PA")
            {
                //ds = getdata.getUsers_by_ProjectUnder(new Guid(DDLProject.SelectedValue));
                //ds = getdata.GetUsers_under_ProjectUID(new Guid(DDLProject.SelectedValue));
                ds = getdata.getUsers_by_AdminUnder(new Guid(Session["UserUID"].ToString()));
            }
            else
            {
                ds = getdata.getUsers_by_AdminUnder(new Guid(Session["UserUID"].ToString()));
            }
            DDLUser.DataSource = ds;
            DDLUser.DataTextField = "UserName";
            DDLUser.DataValueField = "UserUID";
            DDLUser.DataBind();
            DDLUser.Items.Insert(0, new ListItem("--Select--", ""));
        }
        public void BindUserRoleMaster()
        {
            DataSet ds = getdata.GetUserRolesMaster();
            DDlUserType.DataSource = ds;
            DDlUserType.DataTextField = "UserRole_Desc";
            DDlUserType.DataValueField = "UserRole_ID";
            DDlUserType.DataBind();
            DDlUserType.Items.Insert(0, new ListItem("--Select--", ""));
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                Guid AssignUID;
                if (Request.QueryString["AssignID"] != null)
                {
                    AssignUID = new Guid(Request.QueryString["AssignID"]);
                }
                else
                {
                    AssignUID = Guid.NewGuid();
                }

                int ret = getdata.InsertorUpdateAssignProjects(AssignUID, new Guid(DDLUser.SelectedValue), new Guid(DDLProject.SelectedValue), new Guid(DDlUserType.SelectedValue));
                if (ret > 0)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code APU-01 there is a problem with these feature. Please contact system admin.');</script>");
            }
        }

        protected void DDLUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDLUser.SelectedValue != "")
            {
                string UserRoleUID = getdata.GetUserAssignedRoleUID_by_UserUID(new Guid(DDLUser.SelectedValue));
                if (UserRoleUID != "")
                {
                    DDlUserType.SelectedValue = new Guid(UserRoleUID).ToString();
                }
            }
            else
            {
                DDlUserType.SelectedValue = "";
            }
        }
    }
}