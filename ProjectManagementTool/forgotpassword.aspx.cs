using ProjectManager;
using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagementTool
{
    public partial class forgotpassword : System.Web.UI.Page
    {
        DBGetData getdt = new DBGetData();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(Request.QueryString["email"] == null) & !(Request.QueryString["UName"] == null))
            {
                Validate_Date();
            }
            else
            {
                resetPass.Visible = false;
                Forgot.Visible = true;
            }
        }

        protected void BtnSubmit_Click(object sender, EventArgs e)
        {
            if (txtEmialID.Value.Length == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Project Manager", "alert('Enter your Email');", true);
            }
            else
            {
                DataSet ds = getdt.getUserDetails_by_EmailID(txtEmialID.Value);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    SendMail(ds.Tables[0].Rows[0]["EmailID"].ToString(), ds.Tables[0].Rows[0]["sName"].ToString(), "India Standard Time");
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Project Manager", "alert('Invalid Email');", true);
                }
            }
        }

        protected void Validate_Date()
        {
            DataSet ds = getdt.getUserDetails_by_EmailID(Request.QueryString["email"]);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DateTime MailSentDate = Convert.ToDateTime(Request.QueryString["CDate"]);
                DateTime scurrenttime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));
                if (scurrenttime < MailSentDate.AddDays(2))
                {
                    resetPass.Visible = true;
                    Forgot.Visible = false;
                }
                else
                {
                    resetPass.Visible = false;
                    NotAvailabel.Visible = true;
                    Forgot.Visible = false;
                }
            }
        }

        protected void btnchange_Click(object sender, EventArgs e)
        {
            if (txtnewpasswd.Text == txtconfirmpasswd.Text)
            {
                string retstr = getdt.ForgotPasswordChange(Request.QueryString["email"], Security.Encrypt(txtnewpasswd.Text));
                if (retstr == "Success")
                {
                    Response.Redirect("~/Login.aspx?Msg=show", false);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Project Monitoring Tool", "alert('Error: " + retstr + "');", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Project Monitoring Tool", "alert('Password does not Match');", true);
            }
        }

        private void SendMail(string toEmailID, string Name, string sTimezone)
        {
            try
            {

                DateTime scurrenttime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById(sTimezone));
                string sResetLink = WebConfigurationManager.AppSettings["SiteName"] + "ForgotPassword.aspx?cei=verify&key=HFdfhgTURFVagfgfcHYFDAD415FFHGgfaJKHFGbafgsgHGLPOAXVdkdbgfsbdbbgjghkGFhgKHGBAsfdnmnSKBSKH&email=" + toEmailID + "&UName=" + Name + "&CDate=" + scurrenttime.ToString("MM-dd-yyyy");
                string sHtmlString = "<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>" + "<html xmlns='http://www.w3.org/1999/xhtml'>" +
                       "<head>" + "<meta http-equiv='Content-Type' content='text/html; charset=utf-8' />" + "</head>" +
                          "<body style='font-family:Verdana, Arial, sans-serif; font-size:12px; font-style:normal;'>" +
                        "<div style='float:left; width:100%; height:30px;'>" +
                            "Dear, " + Name +
                        "</div>" +
                        "<div style='width:100%; float:left;'><div style='width:100%; float:left;'><h2>Create a new password</h2></div><div style='width:100%; float:left;'>Forgot your password, No big deal.<br/>To create a new password, just follow this link: <br/><br/> <a href='" + sResetLink + "' target='_blank'>Create a New Password</a> <br/><br/> You received this email, because it was requested by a Project Manager user. This is part of the procedure to create a new password on the system. If you DID NOT request a new password then please ignore this email and your password will remain the same. <br/><br/></div><div style='width:100%; float:left;'>Regards,<br/> Project Manager</div></div></body></html>";
                //MailMessage mm = new MailMessage();
                //DataTable dtemailCred = getdt.GetEmailCredentials();
                //mm.To.Add(toEmailID);
                //mm.From = new MailAddress(dtemailCred.Rows[0][0].ToString(), "Project Monitoring Tool");
                //mm.Subject = "Forgot your password on Project Monitoring Tool?";
                //System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
                //client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                //client.EnableSsl = true;
                //client.Host = "smtp.gmail.com";
                //client.Port = 587;
                //System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(dtemailCred.Rows[0][0].ToString(), dtemailCred.Rows[0][1].ToString());
                //client.UseDefaultCredentials = false;
                //client.Credentials = credentials;
                //mm.IsBodyHtml = true;
                //mm.Body = string.Format(sHtmlString);
                //client.Send(mm);
                txtEmialID.Value = "";
                DataTable dtemailCred = getdt.GetEmailCredentials();
                Guid MailUID = Guid.NewGuid();
                getdt.StoreEmaildataToMailQueue(MailUID, Guid.NewGuid(), dtemailCred.Rows[0][0].ToString(), toEmailID, "Forgot your password on Project Monitoring Tool ? !", sHtmlString, "", "");
                ScriptManager.RegisterStartupScript(this, GetType(), "Project Monitoring Tool", "alert('Forgot Password link sent to your EmailID');", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Project Monitoring Tool", "alert('Error : " + ex.Message + "');", true);
            }
        }
    }
}