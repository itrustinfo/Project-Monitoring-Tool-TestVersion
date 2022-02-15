using ProjectManagementTool.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagementTool._modal_pages
{
    public partial class add_pagenamemaster : System.Web.UI.Page
    {
        UserFunctionalityMapping ufp = new UserFunctionalityMapping();
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
                    if (Request.QueryString["MasterPageUID"] != null)
                    {
                        BindMasterPageData(Request.QueryString["MasterPageUID"]);
                    }
                }
            }
        }

        private void BindMasterPageData(string MasterPageUID)
        {
            DataSet ds = ufp.GetMasterPage_by_MasterPageUID(new Guid(MasterPageUID));
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtpagename.Text = ds.Tables[0].Rows[0]["MasterPageName"].ToString();
                txtpageurl.Text = ds.Tables[0].Rows[0]["MasterPageURL"].ToString();
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                Guid MasterPageUID;
                if (Request.QueryString["MasterPageUID"] == null)
                {
                    MasterPageUID = Guid.NewGuid();
                }
                else
                {
                    MasterPageUID = new Guid(Request.QueryString["MasterPageUID"]);
                }

                int cnt = ufp.InsertorUpdateMasterPageName(MasterPageUID, txtpagename.Text, txtpageurl.Text);
                if (cnt > 0)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
                }
                else if (cnt == -1)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Page Name already exists.');</script>");
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('There is a problem with this feature. Please contact system admin. Description : " + ex.Message + "');</script>");
            }
        }
    }
}