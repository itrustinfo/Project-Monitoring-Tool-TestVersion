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
    public partial class add_caa_jica_claims : System.Web.UI.Page
    {
        DBGetData getdt = new DBGetData();
        TaskUpdate gettk = new TaskUpdate();
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (Session["Username"] == null)
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
                    
                }
                if (Request.QueryString["Uid"] != null)
                {
                    GetCaajicaclaims(new Guid(Request.QueryString["Uid"].ToString()));
                    btnUpdate.Visible = true;
                    btnAdd.Visible = false;
                }
                else
                {
                    btnUpdate.Visible = false;
                    btnAdd.Visible = true;
                }



            }
           // }
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
        public void GetCaajicaclaims(Guid uid)
        {
            try
            {
                DataTable dtCaa = getdt.GetcontractPackage_Details(uid);
                if(dtCaa.Rows.Count>0)
                {
                    txtAmount.Text = dtCaa.Rows[0]["Amount"].ToString();
                    txtDescription.Text= dtCaa.Rows[0]["Description"].ToString();
                    txtRemarks.Text= dtCaa.Rows[0]["Remarks"].ToString();
                    //dtPaymentDate.Text= Convert.ToDateTime(dtCaa.Rows[0]["PaymentDate"]).ToString("dd/MM/yyyy");
                    //txtCAADate.Text= Convert.ToDateTime( dtCaa.Rows[0]["CAADate"]).ToString("dd/MM/yyyy");
                    ddlContractPackage.SelectedValue = new Guid(dtCaa.Rows[0]["ProjectContractId"].ToString()).ToString();
                    //ddlStatus.SelectedItem.Text= dtCaa.Rows[0]["Status"].ToString();
                    ddlMeeting.SelectedValue= new Guid(dtCaa.Rows[0]["meetingid"].ToString()).ToString();
                    hidUid.Value = dtCaa.Rows[0]["uid"].ToString();
                    ddlMeeting.Enabled = false;
                }
                
            }
            catch(Exception ex)
            {

            }
        }
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                decimal amt = Convert.ToDecimal(txtAmount.Text);
                string sDate1 = "", sDate2 = "";
                DateTime CDate1 = DateTime.Now, CDate2 = DateTime.Now;

                sDate1 = DateTime.Now.ToString("dd/MM/yyyy");
                sDate1 = getdt.ConvertDateFormat(sDate1);
                CDate1 = Convert.ToDateTime(sDate1);

                //sDate2 = txtCAADate.Text;
                sDate2 = DateTime.Now.ToString("dd/MM/yyyy");
                sDate2 = getdt.ConvertDateFormat(sDate2);
                CDate2 = Convert.ToDateTime(sDate2);

                int cnt = getdt.Insert_CPDetails(ddlContractPackage.SelectedValue, amt, txtDescription.Text, txtRemarks.Text, CDate1, CDate2, "Under Process", new Guid(ddlMeeting.SelectedValue));
                if (cnt>0)
                {
                    Session["SelectedMeeting"] = ddlMeeting.SelectedValue;
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
                }

            }
            catch (Exception ex)
            {

                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code : ACJ-01 there is a problem with this feature. please contact system admin.');</script>");
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

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                decimal amt = Convert.ToDecimal(txtAmount.Text);
                string sDate1 = "", sDate2 = "";
                DateTime CDate1 = DateTime.Now, CDate2 = DateTime.Now;

                sDate1 = DateTime.Now.ToString("dd/MM/yyyy");
                sDate1 = getdt.ConvertDateFormat(sDate1);
                CDate1 = Convert.ToDateTime(sDate1);

                sDate2 = DateTime.Now.ToString("dd/MM/yyyy");
                sDate2 = getdt.ConvertDateFormat(sDate2);
                CDate2 = Convert.ToDateTime(sDate2);

                int cnt = getdt.Update_CP_CPDetails(ddlContractPackage.SelectedValue, amt, txtDescription.Text, txtRemarks.Text, CDate1, CDate2, "Under Process", new Guid(hidUid.Value), new Guid(ddlMeeting.SelectedValue));
                if (cnt > 0)
                {
                    Session["SelectedMeeting"] = ddlMeeting.SelectedValue;
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
                }

            }
            catch (Exception ex)
            {

                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code : ACJ-01 there is a problem with this feature. please contact system admin.');</script>");
            }
        }
    }
}