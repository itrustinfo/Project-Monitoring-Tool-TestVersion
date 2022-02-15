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
    public partial class add_complianceofMOM : System.Web.UI.Page
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
                    BindMeetings();
                    if (Request.QueryString["ComplianceofMOM_UID"] != null)
                    {
                        DDlMeeting.Enabled = false;
                        BindComplianceofMOM(Request.QueryString["ComplianceofMOM_UID"]);
                    }
                }
            }
        }

        private void BindComplianceofMOM(string ComplianceofMOM_UID)
        {
            DataSet ds = getdata.GetComplianceofMOM_Select_by_ComplianceofMOM_UID(new Guid(ComplianceofMOM_UID));
            if (ds.Tables[0].Rows.Count > 0)
            {
                DDlMeeting.SelectedValue= ds.Tables[0].Rows[0]["Meeting_UID"].ToString();
                txtPoints.Text = ds.Tables[0].Rows[0]["Meeting_Points"].ToString();
                txtStatus.Text = ds.Tables[0].Rows[0]["Meeting_Status"].ToString();
            }
        }
        private void BindMeetings()
        {
            DataSet ds = new DataSet();
            ds = getdata.GetMeetingMasters();
            DDlMeeting.DataTextField = "Meeting_Description";
            DDlMeeting.DataValueField = "Meeting_UID";
            DDlMeeting.DataSource = ds;
            DDlMeeting.DataBind();

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                Guid ComplianceofMOM_UID;
                if (Request.QueryString["ComplianceofMOM_UID"] != null)
                {
                    ComplianceofMOM_UID = new Guid(Request.QueryString["ComplianceofMOM_UID"]);
                }
                else
                {
                    ComplianceofMOM_UID = Guid.NewGuid();
                }
                int cnt = getdata.ComplianceofMOM_InsertorUpdate(ComplianceofMOM_UID, new Guid(DDlMeeting.SelectedValue), txtPoints.Text, txtStatus.Text);
                if (cnt >0)
                {
                    Session["SelectedMeeting"] = DDlMeeting.SelectedValue;
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code : CMOM-01 there is a problem with these feature. please contact system admin.');</script>");
            }
        }
    }
}