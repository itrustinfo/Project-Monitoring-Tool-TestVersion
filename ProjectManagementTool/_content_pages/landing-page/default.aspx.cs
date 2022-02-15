using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProjectManager.DAL;

namespace ProjectManagementTool._content_pages.landing_page
{
    public partial class _default : System.Web.UI.Page
    {
        DBGetData getdt = new DBGetData();
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                ddlmeeting.DataSource = getdt.GetMeetingMaster();
                ddlmeeting.DataTextField = "Meeting_Description";
                ddlmeeting.DataValueField = "Meeting_UID";
                ddlmeeting.DataBind();
                if (Request.QueryString["UID"] != null)
                {
                    ddlmeeting.SelectedValue = Request.QueryString["UID"];
                    ddlmeeting_SelectedIndexChanged(sender, e);
                }
                else
                {
                    heading.InnerHtml = ddlmeeting.SelectedItem.ToString();
                }
            }
        }

        protected void ddlmeeting_SelectedIndexChanged(object sender, EventArgs e)
        {
            heading.InnerHtml = ddlmeeting.SelectedItem.ToString();
        }
    }
}