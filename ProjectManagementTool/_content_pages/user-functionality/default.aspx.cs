using ProjectManagementTool.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagementTool._content_pages.user_functionality
{
    public partial class _default : System.Web.UI.Page
    {
        UserFunctionalityMapping ufp = new UserFunctionalityMapping();
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
                    BindMasterPageNames();
                }
            }
        }

        private void BindMasterPageNames()
        {
            DataSet ds = ufp.GetMasterPages();
            GrdMasterPages.DataSource = ds;
            GrdMasterPages.DataBind();
        }

        protected void GrdMasterPages_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string MasterPageUID = e.CommandArgument.ToString();
            if (e.CommandName == "delete")
            {
                int cnt = ufp.MasterPageName_Delete(new Guid(MasterPageUID));
                if (cnt > 0)
                {
                    BindMasterPageNames();
                }
            }
        }

        protected void GrdMasterPages_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void GrdMasterPages_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdMasterPages.PageIndex = e.NewPageIndex;
            BindMasterPageNames();
        }
    }
}