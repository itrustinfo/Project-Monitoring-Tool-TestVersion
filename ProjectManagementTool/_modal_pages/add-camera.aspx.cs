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
    public partial class add_camera : System.Web.UI.Page
    {
        DBGetData getdata = new DBGetData();
        TaskUpdate TKUpdate = new TaskUpdate();
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
                    BindWorkpackage(Request.QueryString["WorkUID"]);
                    if (Request.QueryString["Camera_UID"] != null)
                    {
                        BindCamera(Request.QueryString["Camera_UID"]);
                    }
                }
            }
        }
        private void BindCamera(string CameraUID)
        {
            DataSet ds = getdata.Camera_Selectby_Camera_UID(new Guid(CameraUID));
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtcameraname.Text = ds.Tables[0].Rows[0]["Camera_Name"].ToString();
                txtipaddress.Text = ds.Tables[0].Rows[0]["Camera_IPAddress"].ToString();
                txtdesc.Text = ds.Tables[0].Rows[0]["Camera_Description"].ToString();
            }
        }
        private void BindWorkpackage(string WorkpackageUID)
        {
            DataSet ds = getdata.GetWorkPackages_By_WorkPackageUID(new Guid(WorkpackageUID));
            DDLWorkPackage.DataTextField = "Name";
            DDLWorkPackage.DataValueField = "WorkPackageUID";
            DDLWorkPackage.DataSource = ds;
            DDLWorkPackage.DataBind();
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                Guid CameraUID;
                if (Request.QueryString["Camera_UID"] == null)
                {
                    CameraUID = Guid.NewGuid();
                }
                else
                {
                    CameraUID = new Guid(Request.QueryString["Camera_UID"]);
                }

                int cnt = getdata.Camera_InsertorUpdate(CameraUID, new Guid(Request.QueryString["PrjUID"]), new Guid(DDLWorkPackage.SelectedValue), txtcameraname.Text, txtipaddress.Text, txtdesc.Text);
                if (cnt > 0)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
                }

            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('There is a problem with this feature. Please contact system admin. Description : " + ex.Message + "');</script>");
            }
            
        }
    }
}