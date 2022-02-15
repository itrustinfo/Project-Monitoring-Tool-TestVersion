using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using ProjectManager.DAL;

namespace ProjectManagementTool._modal_pages
{
    public partial class add_conactivity : System.Web.UI.Page
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
                    BindMeetings();
                    
                    if (Request.QueryString["meetinguid"] != null)
                    {
                        ddlMeeting.SelectedValue = Request.QueryString["meetinguid"];
                        
                    }
                    if (Request.QueryString["Uid"] != null)
                    {
                        GetConsActivity(new Guid(Request.QueryString["Uid"].ToString()));
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

        private void GetConsActivity(Guid guid)
        {
            DataTable dtCaa = getdt.GetConsActivity(guid);
            if (dtCaa.Rows.Count > 0)
            {
                txtStatus.Text = dtCaa.Rows[0]["status"].ToString().Replace("<br/>", Environment.NewLine);
                txtActivity.Text = dtCaa.Rows[0]["Activity"].ToString();
                //dtPaymentDate.Text =Convert.ToDateTime(dtCaa.Rows[0]["PaymentDate"]).ToString("dd/MM/yyyy");
                ddlContractPackage.SelectedValue =new Guid( dtCaa.Rows[0]["ProjectUid"].ToString()).ToString();
                hiduid.Value = dtCaa.Rows[0]["uid"].ToString();
                ddlMeeting.SelectedValue =new Guid(dtCaa.Rows[0]["meetingid"].ToString()).ToString();
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
                string sDate1 = "";
                DateTime CDate1 = DateTime.Now;
                //sDate1 = dtPaymentDate.Text;
                sDate1 = DateTime.Now.ToString("dd/MM/yyyy");
                //sDate1 = sDate1.Split('/')[1] + "/" + sDate1.Split('/')[0] + "/" + sDate1.Split('/')[2];
                sDate1 = getdt.ConvertDateFormat(sDate1);
                CDate1 = Convert.ToDateTime(sDate1);

                int cnt = getdt.InsertConsActivity(ddlContractPackage.SelectedValue, txtActivity.Text, Server.HtmlEncode(txtStatus.Text.Trim()).Replace("\r\n", "<br/>"), CDate1, new Guid(ddlMeeting.SelectedValue));
                //GetConsActivity();
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
                string sDate1 = "";
                DateTime CDate1 = DateTime.Now;
                //sDate1 = dtPaymentDate.Text;
                sDate1 = DateTime.Now.ToString("dd/MM/yyyy");
                //sDate1 = sDate1.Split('/')[1] + "/" + sDate1.Split('/')[0] + "/" + sDate1.Split('/')[2];
                sDate1 = getdt.ConvertDateFormat(sDate1);
                CDate1 = Convert.ToDateTime(sDate1);

                int cnt = getdt.updateConsActivity(ddlContractPackage.SelectedValue, txtActivity.Text, Server.HtmlEncode(txtStatus.Text.Trim()).Replace("\r\n", "<br/>"), CDate1, new Guid(hiduid.Value), new Guid(ddlMeeting.SelectedValue));
                if (cnt > 0)
                {
                    Session["SelectedMeeting"] = ddlContractPackage.SelectedValue + "_" + ddlMeeting.SelectedValue;
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
                }
                //GetConsActivity();
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
            }
        }
        }
}