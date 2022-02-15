using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProjectManager.DAL;

namespace ProjectManagementTool._content_pages.status_wastewater_forms
{
    public partial class _default : System.Web.UI.Page
    {
        DBGetData getdata = new DBGetData();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlmeeting.DataSource = getdata.GetMeetingMaster();
                ddlmeeting.DataTextField = "Meeting_Description";
                ddlmeeting.DataValueField = "Meeting_UID";
                ddlmeeting.DataBind();
                if (Session["SelectedMeeting"] != null)
                {
                    ddlmeeting.SelectedValue = Session["SelectedMeeting"].ToString();
                    Session["SelectedMeeting"] = null;
                }
                LoadGridDataBudget();
                Session["sMeetingUID"] = ddlmeeting.SelectedValue;
            }
        }

        private void LoadGridDataBudget()
        {
            try
            {
                GrdStatus.DataSource = getdata.GetStatusWasteWater(new Guid(ddlmeeting.SelectedValue));
                GrdStatus.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void ddlmeeting_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadGridDataBudget();
            Session["sMeetingUID"] = ddlmeeting.SelectedValue;
            if (ddlmeeting.SelectedIndex > 0)
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
                int index = ddlmeeting.SelectedIndex - 1;
                Guid sourcemeetingUID = new Guid(ddlmeeting.Items[index].Value);
                int result = getdata.CopyStatusWasteWater(sourcemeetingUID, new Guid(ddlmeeting.SelectedValue));
                LoadGridDataBudget();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}