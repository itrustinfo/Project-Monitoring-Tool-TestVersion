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
    public partial class add_projectstatus : System.Web.UI.Page
    {
        DBGetData getdt = new DBGetData();
        TaskUpdate gettk = new TaskUpdate();
        protected void Page_Load(object sender, EventArgs e)
        {

           // if (Session["Username"] == null)
            //{
            //    Response.Redirect("~/Login.aspx");
            //}
            //else
            //{
                if (!IsPostBack)
                {
                    BindProject();
                    BindMeetings();
                    if (Request.QueryString["meetingid"] != null)
                    {
                        ddlMeeting.SelectedValue = Request.QueryString["meetingid"];
                        ddlMeeting.Enabled = false;
                    }
                    if (Request.QueryString["Uid"] != null)
                    {
                        GetConsMonthActivity(new Guid(Request.QueryString["Uid"].ToString()));
                        btnUpdate.Visible = true;
                        btnAdd.Visible = false;
                    }
                    else
                    {
                        btnUpdate.Visible = false;
                        btnAdd.Visible = true;
                    }

               // }
            }
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
        private void GetConsMonthActivity(Guid guid)
        {
            DataTable dtCaa = getdt.GetConsMonthActivity(guid);
            if (dtCaa.Rows.Count > 0)
            {
                txtAchieved.Text = dtCaa.Rows[0]["Achieved"].ToString();
                ddlContractPackage.SelectedValue = new Guid(dtCaa.Rows[0]["ProjectUId"].ToString()).ToString();
                txtActivity.Text = dtCaa.Rows[0]["Activity"].ToString();
                txtprogress.Text = dtCaa.Rows[0]["Percentage"].ToString();
                txtTarget.Text = dtCaa.Rows[0]["Target"].ToString();
                ddlMeeting.SelectedValue=new Guid( dtCaa.Rows[0]["meetingid"].ToString()).ToString();
                hiduid.Value = dtCaa.Rows[0]["uid"].ToString();
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

            ddlContractPackage.DataTextField = "ProjectName";
            ddlContractPackage.DataValueField = "ProjectUID";
            ddlContractPackage.DataSource = ds;
            ddlContractPackage.DataBind();

        }
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                int cnt = getdt.InsertConsMonthlyActivity(ddlContractPackage.SelectedValue, txtActivity.Text, txtTarget.Text, txtAchieved.Text, txtprogress.Text, new Guid(ddlMeeting.SelectedValue));
            
                txtAchieved.Text = "";
                txtActivity.Text = "";
                txtTarget.Text = "";
                if (cnt > 0)
                {
                    Session["SelectedMeeting"] = ddlContractPackage.SelectedValue + "_" + ddlMeeting.SelectedValue;
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
                }
            }
            catch (Exception ex)
            {
               
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
                
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                int cnt = getdt.UpdateConsMonthlyActivity(ddlContractPackage.SelectedValue, txtActivity.Text, txtTarget.Text, txtAchieved.Text, txtprogress.Text,new Guid(hiduid.Value), new Guid(ddlMeeting.SelectedValue));

                txtAchieved.Text = "";
                txtActivity.Text = "";
                txtTarget.Text = "";
                if (cnt > 0)
                {
                    Session["SelectedMeeting"] = ddlContractPackage.SelectedValue + "_" + ddlMeeting.SelectedValue;
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
                }
            }
            catch(Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
            }

        }
    }
}