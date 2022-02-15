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
    public partial class add_OtherPointsforDiscussion : System.Web.UI.Page
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
                    
                    BindMeetings();

                  if (Request.QueryString["Uid"] != null)
                    {
                        BindOtherpoints_Uid(new Guid(Request.QueryString["Uid"].ToString()));
                        btnUpdate.Visible = true;
                        btnAdd.Visible = false;
                    }
                    else
                    {
                        btnUpdate.Visible = false;
                        btnAdd.Visible = true;
                    }

                }
            }
        }

        private void BindOtherpoints_Uid(Guid guid)
        {
            DataTable dtPoints = getdt.GetOtherPoints_uid(guid);
            if(dtPoints.Rows.Count>0)
            {
                txtdesc.Text = dtPoints.Rows[0]["points"].ToString();
                ddlMeeting.SelectedValue = new Guid(dtPoints.Rows[0]["meetingid"].ToString()).ToString();
                hiduid.Value = dtPoints.Rows[0]["uid"].ToString();
                ddlMeeting.Enabled = false;
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
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                int cnt = getdt.InsertpointsForDiscussion(ddlMeeting.SelectedValue, txtdesc.Text);
                //GetConsActivity();
                if (cnt > 0)
                {
                    Session["SelectedMeeting"] = ddlMeeting.SelectedValue;
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
                }

            }
            catch(Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");

            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                int cnt = getdt.UpdatepointsForDiscussion(ddlMeeting.SelectedValue, txtdesc.Text, new Guid(hiduid.Value));
                //GetConsActivity();
                if (cnt > 0)
                {
                    Session["SelectedMeeting"] = ddlMeeting.SelectedValue;
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");

            }
        }

    }
}