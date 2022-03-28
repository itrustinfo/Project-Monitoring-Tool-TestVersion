using ProjectManager;
using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using static ProjectManagementTool.DAL.Constants;

namespace ProjectManagementTool
{
    public partial class Login : System.Web.UI.Page
    {
        DBGetData getdt = new DBGetData();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["ChangePassword"] != null)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Password changed Successfully. Please login.');</script>");
                Session["ChangePassword"] = null;
            }
            if (!IsPostBack)
            {
                DisplayLogo();
            }
            
        }

        private void DisplayLogo()
        {
            string Domain = WebConfigurationManager.AppSettings["Domain"];
            string host = Request.Url.Host.ToLower();

            DataSet ds = getdt.GetDomainDetails_by_URL(host.Replace("www.", ""));
            if (ds.Tables[0].Rows.Count > 0)
            {
                LblTitle.Text = ds.Tables[0].Rows[0]["Title"].ToString();
                LblDescription.Text = ds.Tables[0].Rows[0]["Description"].ToString();
                if (ds.Tables[0].Rows[0]["Logo"].ToString() != "")
                {
                    sLogo.ImageUrl = "/_assets/Logos/" + ds.Tables[0].Rows[0]["Logo"].ToString();
                }
                else
                {
                    sLogo.Visible = false;
                }
                
            }
            //if (Domain == "NJSEI")
            //{
            //    NJSEI.Visible = true;
            //    ONTB.Visible = false;
            //}
            //else
            //{
            //    NJSEI.Visible = false;
            //    ONTB.Visible = true;
            //}
        }
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                DataSet ds = new DataSet();
                //string strEncrypt = Security.Encrypt(txtpassword.Value);
                //strEncrypt = strEncrypt + "";
                //string decrypt = Security.Decrypt("bS4yC0dl7KkvWZhoHN8ExA==");
                //decrypt = decrypt + "";
                string Domain = WebConfigurationManager.AppSettings["Domain"];
                string host = Request.Url.Host.ToLower();
                int Maxusers = 0;
                DataSet dsdomain = getdt.GetDomainDetails_by_URL(host.Replace("www.", ""));
                if (dsdomain.Tables[0].Rows.Count > 0)
                {
                    Maxusers = int.Parse(dsdomain.Tables[0].Rows[0]["MaxUsers"].ToString());
                }
                    if ((int)Application["UsersCount"] <= Maxusers)
                {
                    ds = getdt.CheckLogin(txtusername.Value, Security.Encrypt(txtpassword.Value));
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        bool login = false;
                        if (ds.Tables[0].Rows[0]["TypeOfUser"].ToString() != "U" && ds.Tables[0].Rows[0]["TypeOfUser"].ToString() != "PA" && ds.Tables[0].Rows[0]["TypeOfUser"].ToString() != "MD" && ds.Tables[0].Rows[0]["TypeOfUser"].ToString() != "VP")
                        {
                            DataSet prj = getdt.GetAssignedProjects_by_UserUID(new Guid(ds.Tables[0].Rows[0]["UserUID"].ToString()));
                            if (prj.Tables[0].Rows.Count > 0)
                            {
                                login = true;
                            }
                        }
                        else
                        {
                            login = true;
                        }
                        if (login)
                        {
                            Session["Username"] = ds.Tables[0].Rows[0]["FirstName"].ToString() + " " + ds.Tables[0].Rows[0]["LastName"].ToString();
                            Session["UserID"] = ds.Tables[0].Rows[0]["UserName"].ToString();
                            Session["UserUID"] = ds.Tables[0].Rows[0]["UserUID"].ToString();
                            Session["TypeOfUser"] = ds.Tables[0].Rows[0]["TypeOfUser"].ToString();
                            Session["IsContractor"] = ds.Tables[0].Rows[0]["IsContractor"].ToString();
                            // added on 25/03/2022 for nakib
                            string userTypeID = ds.Tables[0].Rows[0]["UserTypeID"].ToString();
                            if (!string.IsNullOrEmpty(userTypeID))
                            {
                                Session["IsClient"] = string.Empty;
                                Session["IsONTB"] = string.Empty;
                                Session["IsNJSEI"] = string.Empty;
                                switch (Convert.ToInt32(userTypeID))
                                {
                                    case (int)UserTypeEnum.Client:
                                        Session["IsClient"] = "Y";
                                        break;
                                    case (int)UserTypeEnum.ONTB:
                                        Session["IsONTB"] = "Y";
                                        break;
                                    case (int)UserTypeEnum.NJSEI:
                                        Session["IsNJSEI"] = "Y";
                                        break;
                                    default:
                                        break;
                                }
                            }
                            else
                            {
                                Session["IsClient"] = string.Empty;
                                Session["IsONTB"] = string.Empty;
                                Session["IsNJSEI"] = string.Empty;
                            }
                           //-----------------------------------------
                            string sessionId = HttpContext.Current.Session.SessionID;
                            getdt.UsersLogInStatus(txtusername.Value, HttpContext.Current.Session.SessionID, "Success");
                            if (Application["UsersCount"] != null)
                            {
                                Application.Lock();
                                Application["UsersCount"] = ((int)Application["UsersCount"]) + 1;
                                Application.UnLock();
                            }
                            if (WebConfigurationManager.AppSettings["Domain"] == "NJSEI")
                            {
                                Response.Redirect("_content_pages/dashboard/default.aspx?type=" + ds.Tables[0].Rows[0]["TypeOfUser"].ToString());
                            }
                            else
                            {
                                if (WebConfigurationManager.AppSettings["LandingPage"] == "0")
                                {
                                    Response.Redirect("_content_pages/dashboard/default.aspx?type=" + ds.Tables[0].Rows[0]["TypeOfUser"].ToString());
                                }
                                else
                                {
                                    Response.Redirect("_content_pages/landing-page/");
                                }
                            }
                        }
                        else
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "Error", "<script language='javascript'>alert('No Project assigned. Ask System admin to assign, then try again');</script>");
                        }
                        
                    }
                    else
                    {
                        lblMsg.Visible = true;
                        lblMsg.Text = "Username/Password is incorrect !"; 
                    }
                }
                else
                {
                    lblMsg.Visible = true;
                    lblMsg.Text = "Maximum simultineous usage count (as per licence exceded).Please try later";
                }
                    
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code : LG-01. Description : " + ex.Message + "');</script>");
            }
        }
    }
}