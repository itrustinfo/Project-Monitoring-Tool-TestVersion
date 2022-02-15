using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Drawing;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System.Text;
using System.Web.Configuration;

namespace ProjectManager._content_pages.reports_RA
{
    public partial class Default : System.Web.UI.Page
    {
      
        //TaskUpdate gettk = new TaskUpdate();
        DBGetData getdt = new DBGetData();
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
                    GetContractPackages();
                   
                }
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
        private void GetContractPackages()
        {
            try
            {
                DataTable  ds = getdt.GetcontractPackage_Meeting_Details(new Guid(ddlMeeting.SelectedValue));
                gdCP.DataSource = ds;
                gdCP.DataBind();
            }
            catch(Exception ex)
            {

            }
        }

        protected void ddlMeeting_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(ddlMeeting.SelectedIndex>0)
            {
                btncopy.Visible = true;
            }
            else
            {
                btncopy.Visible = false;
            }
            GetContractPackages();
        }

        protected void btncopy_Click(object sender, EventArgs e)
        {
            try
            {
                int index = ddlMeeting.SelectedIndex - 1;
                Guid sourcemeetingUID = new Guid(ddlMeeting.Items[index].Value);
                int result = getdt.CopyCAAJICAData(sourcemeetingUID, new Guid(ddlMeeting.SelectedValue));
                GetContractPackages();
            }
            catch(Exception ex)
            {

            }
        }

        protected void gdCP_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string UID = e.CommandArgument.ToString();
            if (e.CommandName == "delete")
            {
                int cnt = getdt.CAAA_JICA_Delete(new Guid(UID));
                if (cnt > 0)
                {
                    GetContractPackages();
                }
            }
        }

        protected void gdCP_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            
        }

        protected void gdCP_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[1].Text = "Amount (in " + WebConfigurationManager.AppSettings["CliamsSenttoCAAA"] + ")";
            }
        }



        //public override void VerifyRenderingInServerForm(Control control)
        //{
        //    //required to avoid the runtime error "  
        //    //Control 'GridView1' of type 'GridView' must be placed inside a form tag with runat=server."  
        //}
    }
}