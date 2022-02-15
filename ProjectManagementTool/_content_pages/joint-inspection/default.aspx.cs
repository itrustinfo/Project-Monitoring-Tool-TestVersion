using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagementTool._content_pages.joint_inspection
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
                ds = getdt.GetWorkPackages_By_ProjectUID(new Guid(DDlProject.SelectedValue));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DDLWorkPackage.DataTextField = "Name";
                    DDLWorkPackage.DataValueField = "WorkPackageUID";
                    DDLWorkPackage.DataSource = ds;
                    DDLWorkPackage.DataBind();
                    BindJointInsecption(DDLWorkPackage.SelectedValue);
                    AddJointInspection.HRef = "/_modal_pages/add-jointinspection.aspx?type=Add&ProjectUID=" + DDlProject.SelectedValue;
                }
            }
        }
        protected void DDLWorkPackage_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDLWorkPackage.SelectedValue != "")
            {
                BindJointInsecption(DDLWorkPackage.SelectedValue);
                AddJointInspection.HRef = "/_modal_pages/add-jointinspection.aspx?type=Add&ProjectUID=" + DDlProject.SelectedValue;
            }
        }
        private void BindJointInsecption(string WorkpackgeUID)
        {
            DataTable ds = getdt.getJointInspection_by_WorkpackgeUID(new Guid(WorkpackgeUID));
            GrdJointInspection.DataSource = ds;
            GrdJointInspection.DataBind();
        }

        protected void GrdJointInspection_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdJointInspection.PageIndex = e.NewPageIndex;
            BindJointInsecption(DDLWorkPackage.SelectedValue);
        }

        protected void GrdJointInspection_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string UID = e.CommandArgument.ToString();
            if (e.CommandName == "delete")
            {
                getdt.deleteInspectionReport(UID);
                BindJointInsecption(DDLWorkPackage.SelectedValue);
            }
        }

        protected void GrdJointInspection_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        public string GetBOQDesc(string BOQUid)
        {
            return getdt.GetBOQDesc_by_BOQDetailsUID(new Guid(BOQUid));
        }
    }
}