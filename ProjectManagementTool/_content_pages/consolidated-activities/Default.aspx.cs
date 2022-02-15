using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using ProjectManager.DAL;

namespace ProjectManagementTool._content_pages.consolidated_activities
{
    public partial class Default : System.Web.UI.Page
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
                    
                    //BindProject();
                    BindProject();
                    RemoveProjects();
                    BindMeetings();
                    if (Session["SelectedMeeting"] != null)
                    {
                        string[] select_Meeting = Session["SelectedMeeting"].ToString().Split('_');
                        DDLContractPackage.SelectedValue = select_Meeting[0];
                        ddlMeeting.SelectedValue = select_Meeting[1];

                        GetConsActivity();
                        Session["SelectedMeeting"] = null;
                    }
                    //txtStatus.Text = "";
                    //txtActivity.Text = "";
                    //dtPaymentDate.Text = "";
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

            DDLContractPackage.DataTextField = "ProjectName";
            DDLContractPackage.DataValueField = "ProjectUID";
            DDLContractPackage.DataSource = ds;
            DDLContractPackage.DataBind();

        }

        private void RemoveProjects()
        {
            DDLContractPackage.Items.Remove(DDLContractPackage.Items.FindByText("CP-07"));
            DDLContractPackage.Items.Remove(DDLContractPackage.Items.FindByText("CP-08"));
            DDLContractPackage.Items.Remove(DDLContractPackage.Items.FindByText("CP-09"));
            DDLContractPackage.Items.Remove(DDLContractPackage.Items.FindByText("CP-10"));
        }

        private void BindMeetings()
        {
            DataSet ds = new DataSet();
            ds = getdt.GetMeetingMasters();

            ddlMeeting.DataTextField = "Meeting_Description";
            ddlMeeting.DataValueField = "Meeting_UID";
            ddlMeeting.DataSource = ds;
            ddlMeeting.DataBind();

        }
        private void GetConsActivity()
        {
            try
            {
                //DataTable ds = getdt.GetConsActivity_Meeting(new Guid( ddlMeeting.SelectedValue));
                DataTable ds = getdt.GetConsActivity_Meeting_Project(new Guid(ddlMeeting.SelectedValue), new Guid(DDLContractPackage.SelectedValue));
                //DataSet ds = getdt.GetConsActivity_by_ProjectUID_meetingid(DDLContractPackage.SelectedValue, ddlMeeting.SelectedValue);
                gdConsActivity.DataSource = ds;
                gdConsActivity.DataBind();
            }
            catch (Exception ex)
            {

            }
        }

        protected void ddlMeeting_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlMeeting.SelectedIndex > 0)
            {
                btncopy.Visible = true;
            }
            else
            {
                btncopy.Visible = false;
            }
            
        }
        protected void btncopy_Click(object sender, EventArgs e)
        {
            try
            {
                int index = ddlMeeting.SelectedIndex - 1;
                Guid sourcemeetingUID = new Guid(ddlMeeting.Items[index].Value);
                int result = getdt.CopyConsolidateActivities(sourcemeetingUID, new Guid(ddlMeeting.SelectedValue));
                GetConsActivity();
            }
            catch (Exception ex)
            {

            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            GetConsActivity();
        }

        protected void gdConsActivity_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string UID = e.CommandArgument.ToString();
            if (e.CommandName == "delete")
            {
                int cnt = getdt.ConsActivity_Delete(new Guid(UID));
                if (cnt > 0)
                {
                    GetConsActivity();
                }
            }
        }

        protected void gdConsActivity_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }
    }
}