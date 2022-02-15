using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagementTool._content_pages.view_compliancemom
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
                    BindMeetings();
                    if (Session["SelectedMeeting"] != null)
                    {
                        ddlMeeting.SelectedValue = Session["SelectedMeeting"].ToString();
                        Session["SelectedMeeting"] = null;
                    }
                    ddlMeeting_SelectedIndexChanged(sender, e);
                }
            }
        }
        private void BindMeetings()
        {
            DataSet ds = getdt.GetMeetingMaster();
            ddlMeeting.DataTextField = "Meeting_Description";
            ddlMeeting.DataValueField = "Meeting_UID";
            ddlMeeting.DataSource = ds;
            ddlMeeting.DataBind();
           
        }

        protected void ddlMeeting_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindComplianceofMOM();
            if (ddlMeeting.SelectedIndex > 0)
            {
                btncopy.Visible = true;
            }
            else
            {
                btncopy.Visible = false;
            }
        }
        private void BindComplianceofMOM()
        {
            if (ddlMeeting.SelectedValue != "")
            {
                DataSet ds = getdt.GetComplianceofMOM_by_Meeting_UID(new Guid(ddlMeeting.SelectedValue));
                GrdCompliance.DataSource = ds;
                GrdCompliance.DataBind();
            }
        }
        protected void btncopy_Click(object sender, EventArgs e)
        {
            int index = ddlMeeting.SelectedIndex - 1;
            Guid sourcemeetingUID = new Guid(ddlMeeting.Items[index].Value);
            int result = getdt.CopyComplianceMOM(sourcemeetingUID, new Guid(ddlMeeting.SelectedValue));
            if (result > 0)
            {
                BindComplianceofMOM();
            }
        }

        protected void GrdCompliance_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string UID = e.CommandArgument.ToString();
            if (e.CommandName == "delete")
            {
                int cnt = getdt.ComplianceofMOM_Delete(new Guid(UID));
                if (cnt > 0)
                {
                    BindComplianceofMOM();
                }
            }
        }

        protected void GrdCompliance_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }
    }
}