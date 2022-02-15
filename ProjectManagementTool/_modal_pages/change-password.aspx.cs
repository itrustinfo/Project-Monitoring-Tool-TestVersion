using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProjectManager;
using ProjectManager.DAL;

namespace ProjectManagementTool._modal_pages
{
    public partial class change_password : System.Web.UI.Page
    {
        DBGetData getdt = new DBGetData();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Username"] == null)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                DataSet ds = getdt.getUserDetails(new Guid(Session["UserUID"].ToString()));
                string oldPassword = string.Empty;
                if(ds.Tables[0].Rows.Count > 0)
                {
                    oldPassword = Security.Decrypt(ds.Tables[0].Rows[0]["password"].ToString());
                }
                if(oldPassword != txtOldPassword.Text)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Old Password is incorrect !');</script>");
                    return;
                }
                if (txtNewPassword.Text.ToString().Length <= 6)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Password must be atleast 6 characters long!');</script>");
                    return;
                }
                if (txtNewPassword.Text != txtConfirmPassword.Text)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('New Password and Confirm Password do not match!');</script>");
                    return;
                }
                else // update the new password.....
                {
                    int result = getdt.UpdatePassword(new Guid(Session["UserUID"].ToString()),Security.Encrypt(txtNewPassword.Text));
                    if(result != 0)
                    {
                        Session["Username"] = null;
                        Session["UserUID"] = null;
                        Session["ChangePassword"] = "true";
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");

                    }
                }
            }
            catch(Exception ex)
            {

            }

        }
    }
}