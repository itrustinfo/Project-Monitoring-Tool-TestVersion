using ProjectManager;
using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static ProjectManagementTool.DAL.Constants;

namespace ProjectManagementTool._modal_pages
{
    public partial class add_user : System.Web.UI.Page
    {
        DBGetData getdt = new DBGetData();
        TaskUpdate gettk = new TaskUpdate();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Username"] == null)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
            }
            this.UnobtrusiveValidationMode = System.Web.UI.UnobtrusiveValidationMode.None;
            if (!IsPostBack)
            {
                BindUserType();
                BindType();
                if (Request.QueryString["UserUID"] != null)
                {
                    BindUser(new Guid(Request.QueryString["UserUID"]));
                    //
                    hlresetpassowrd.Visible = true;
                    hlresetpassowrd.HRef = "reset-password.aspx?UserUID=" + Request.QueryString["UserUID"];
                    if (Session["reset"]!=null)
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Password Reset was success !');</script>");
                        Session["reset"] = null;
                    }
                }
                else if (Request.QueryString["type"] != null)
                {
                    BindUser(new Guid(Session["UserUID"].ToString()));
                    divPassword.Visible = false;
                    divUsername.Visible = false;
                    divUserType.Visible = false;
                    divMailSettings.Visible = false;
                }
            }
        }

        private void BindUser(Guid UserUID)
        {
            DataSet ds = getdt.get_UserDetails(UserUID);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ViewState["EditUser"] = "true";
                txtfirstname.Text = ds.Tables[0].Rows[0]["FirstName"].ToString();
                txtlastname.Text = ds.Tables[0].Rows[0]["LastName"].ToString();
                txtemailid.Text = ds.Tables[0].Rows[0]["EmailID"].ToString();
                txtmobile.Text = ds.Tables[0].Rows[0]["Mobilenumber"].ToString();
                txtaddress1.Text = ds.Tables[0].Rows[0]["Address1"].ToString();
                txtaddress2.Text = ds.Tables[0].Rows[0]["Address2"].ToString();
                txtloginusername.Text = ds.Tables[0].Rows[0]["Username"].ToString();
                //txtloginpassword.Text = Security.Decrypt(ds.Tables[0].Rows[0]["password"].ToString());
                DDlUserType.SelectedValue = ds.Tables[0].Rows[0]["TypeOfUser"].ToString();
                if (ds.Tables[0].Rows[0]["Profile_Pic"] != DBNull.Value)
                {
                    if (ds.Tables[0].Rows[0]["Profile_Pic"].ToString() != "")
                    {
                        ViewState["ProfilePic"] = ds.Tables[0].Rows[0]["Profile_Pic"].ToString();// Profile_Pic
                        ImgPicure.ImageUrl = ds.Tables[0].Rows[0]["Profile_Pic"].ToString();
                    }
                    else
                    {
                        ImgPicure.ImageUrl = "~/_assets/images/Photo_mb.png";
                    }
                }
                else
                {
                    ImgPicure.ImageUrl = "~/_assets/images/Photo_mb.png";
                }
                txtloginpassword.Attributes["value"] = "******";
               // txtloginpassword.Text = "******";
                txtloginpassword.Enabled = false;
                txtemailid.Enabled = false;
                if (getdt.GetUserMailAccess(UserUID, "documentmail") != 0)
                {
                    chkboxlstMailSettings.Items[0].Selected = true;
                }
                if (getdt.GetUserMailAccess(UserUID, "projectmastermail") != 0)
                {
                    chkboxlstMailSettings.Items[1].Selected = true;
                }

                string userTypeID = ds.Tables[0].Rows[0]["UserTypeID"].ToString();
                if(!string.IsNullOrEmpty(userTypeID))
                {
                    RBLType.SelectedValue = userTypeID;
                }
                else
                {
                    if( ds.Tables[0].Rows[0]["IsContractor"].ToString() == "Y")
                    {
                        RBLType.SelectedValue = ((int)UserTypeEnum.Contractor).ToString();
                    }
                }
            }
        }

        private void BindUserType()
        {
            DataSet ds = getdt.getAllUsers_Roles();
            DDlUserType.DataTextField = "UserRole_Desc";
            DDlUserType.DataValueField = "UserRole_Name";
            DDlUserType.DataSource = ds;
            DDlUserType.DataBind();
        }

        private void BindType()
        {
            DataSet ds = getdt.getUserType();
            RBLType.DataTextField = "Name";
            RBLType.DataValueField = "ID";
            RBLType.DataSource = ds;
            RBLType.DataBind();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    Guid UserUID = Guid.NewGuid();
                    string docmail = "N";
                    string projecmastermail ="N";
                    if (Request.QueryString["UserUID"] != null)
                    {
                        UserUID = new Guid(Request.QueryString["UserUID"]);
                    }
                    else if (Request.QueryString["type"] != null)
                    {
                        UserUID = new Guid(Session["UserUID"].ToString());
                    }
                    string PicPath = string.Empty;
                    if (ProfileUpload.HasFile)
                    {
                        ProfileUpload.SaveAs(Server.MapPath("~/_assets/images/ProfilePics/" + UserUID + ProfileUpload.FileName));
                        //FileUploadDoc.SaveAs(Server.MapPath("~/Documents/Encrypted/" + sDocumentUID + "_" + txtDocName.Text + "_1"  + "_enp" + InputFile));
                        PicPath = "~/_assets/images/ProfilePics/" + UserUID + ProfileUpload.FileName;

                    }
                    else
                    {
                        if(ViewState["ProfilePic"] != null)
                        {
                            PicPath = ViewState["ProfilePic"].ToString();
                        }
                    }

                    //string ProjectUnder = string.Empty;
                    //ProjectUnder = DDLProject.SelectedValue;
                    txtloginusername.Text = txtemailid.Text;

                    if (ViewState["EditUser"] == null)
                    {
                        if (getdt.CheckUserName_Exists(txtloginusername.Text) != 0)
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Login Username already exists.Please try different Username');</script>");
                            return;
                        }
                    }
                    if(chkboxlstMailSettings.Items[0].Selected == true)
                    {
                        docmail = "Y";
                    }
                    if (chkboxlstMailSettings.Items[1].Selected == true)
                    {
                        projecmastermail = "Y";
                    }
                    string userType = RBLType.SelectedValue;
                    string IsContractor = string.Empty;
                    if(!string.IsNullOrEmpty(userType))
                    {
                        if (Convert.ToInt32(userType) == (int)UserTypeEnum.Contractor)
                            IsContractor = "Y";
                    }

                    bool ret = getdt.InsertorUpdateUsers(UserUID, txtfirstname.Text, txtlastname.Text, txtemailid.Text, txtmobile.Text, txtaddress1.Text, txtaddress2.Text, txtloginusername.Text, txtloginpassword.Text, DDlUserType.SelectedValue, new Guid(Session["UserUID"].ToString()), PicPath,docmail,projecmastermail, IsContractor, userType);
                    if (ret)
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
                    }
                }
                catch (Exception ex)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code : AU-01 there is a problem with this feature. please contact system admin.');</script>");
                }
               
            }
        }
    }
}