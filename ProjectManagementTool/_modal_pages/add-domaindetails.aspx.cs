using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagementTool._modal_pages
{
    public partial class add_domaindetails : System.Web.UI.Page
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
                if (!IsPostBack)
                {
                    if (Request.QueryString["UID"] != null)
                    {
                        BindDomainDetails(Request.QueryString["UID"].ToString());
                    }
                }
            }
        }
        protected void BindDomainDetails(string UID)
        {
            DataSet ds = getdata.GetDomainDetails_by_UID(new Guid(UID));
            if (ds.Tables[0].Rows.Count > 0)
            {
                txttitle.Text = ds.Tables[0].Rows[0]["Title"].ToString();
                txtdesc.Text = ds.Tables[0].Rows[0]["Description"].ToString();
                txtURL.Text = ds.Tables[0].Rows[0]["URL"].ToString();
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                Guid UID;
                if (Request.QueryString["UID"] != null)
                {
                    UID = new Guid(Request.QueryString["UID"]);
                }
                else
                {
                    UID = Guid.NewGuid();
                }
                string sLogo = "";
                if (LogoUpload.HasFile)
                {
                    sLogo = DateTime.Now.Ticks + "_" + Path.GetFileName(LogoUpload.FileName);
                    LogoUpload.SaveAs(Server.MapPath("~/_assets/Logos/" + sLogo));
                }
                int cnt = getdata.InsertorUpdateDomainDetails(UID, txttitle.Text, txtdesc.Text, sLogo, txtURL.Text);
                if (cnt > 0)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
                }
                else if (cnt == -1)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Title already exists.');</script>");
                }
                
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('There is a problem with this feature. Please contact system admin. Description : " + ex.Message + "');</script>");
            }
        }
    }
}