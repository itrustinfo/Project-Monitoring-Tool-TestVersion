using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProjectManager;

using ProjectManager.DAL;

namespace ProjectManagementTool._modal_pages
{
    public partial class reset_password : System.Web.UI.Page
    {
        DBGetData getdata = new DBGetData();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Username"] == null)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
            }
            else
            {
                if(!IsPostBack)
                {

                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtnewpassword.Text.ToString().Length <= 6)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Password must be atleast 6 characters long!');</script>");
                    return;
                }
                if (txtnewpassword.Text != txtconfirmpassword.Text)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('New Password and Confirm Password do not match!');</script>");
                    return;
                }
                else // update the new password.....
                {
                    int result = getdata.UpdatePassword(new Guid(Request.QueryString["UserUID"]), Security.Encrypt(txtnewpassword.Text));
                    if (result != 0)
                    {
                        //Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Password Reset was success !');</script>");
                        //store email for the reset password to be sent to user
                        string CC = string.Empty;
                        string ToEmailID = getdata.GetUserEmail_By_UserUID_New(new Guid(Request.QueryString["UserUID"]));
                        string sUserName = getdata.getUserNameby_UID(new Guid(Request.QueryString["UserUID"]));
                        string Subject = "Password was reset by admin !";
                        string sHtmlString = string.Empty;
                        sHtmlString = "<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>" + "<html xmlns='http://www.w3.org/1999/xhtml'>" +
                          "<head>" + "<meta http-equiv='Content-Type' content='text/html; charset=utf-8' />" + "<style>table, th, td {border: 1px solid black; padding:6px;}</style></head>" +
                             "<body style='font-family:Verdana, Arial, sans-serif; font-size:12px; font-style:normal;'>";
                        sHtmlString += "<div style='width:80%; float:left; padding:1%; border:2px solid #011496; border-radius:5px;'>" +
                                           "<div style='float:left; width:100%; border-bottom:2px solid #011496;'>";
                        if (WebConfigurationManager.AppSettings["Domain"] == "NJSEI")
                        {
                            sHtmlString += "<div style='float:left; width:7%;'><img src='https://dm.njsei.com/_assets/images/NJSEI%20Logo.jpg' width='50' /></div>";
                        }
                        else
                        {
                            sHtmlString += "<div style='float:left; width:7%;'><h2>" + WebConfigurationManager.AppSettings["Domain"] + "</h2></div>";
                        }
                        sHtmlString += "<div style='float:left; width:70%;'><h2 style='margin-top:10px;'>Project Monitoring Tool</h2></div>" +
                                   "</div>";
                        sHtmlString += "<div style='width:100%; float:left;'><br/>Dear " + sUserName + ",<br/><br/><span style='font-weight:bold;'>Admin has reset your passord.</span> <br/><br/></div>";
                        sHtmlString += "<div style='width:100%; float:left;'><table style='width:100%;'>" +
                                        "<tr><td><b>Your new passowrd is : </b>" +txtnewpassword.Text + "</td></tr>";
                        sHtmlString += "</table></div>";
                        sHtmlString += "<div style='width:100%; float:left;'><br/><br/>Sincerely, <br/> Project Monitoring Tool.</div></div></body></html>";

                        DataTable dtemailCred = getdata.GetEmailCredentials();
                        Guid MailUID = Guid.NewGuid();
                        getdata.StoreEmaildataToMailQueue(MailUID, new Guid(Request.QueryString["UserUID"]), dtemailCred.Rows[0][0].ToString(), ToEmailID, Subject, sHtmlString, CC, "");
                        //
                        Session["reset"] = 1;
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");

                    }
                }
            }
            catch(Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error occured.please contact system admin!');</script>");

            }
        }
    }
}