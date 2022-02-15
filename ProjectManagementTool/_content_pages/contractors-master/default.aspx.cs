using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagementTool._content_pages.contractors
{
    public partial class _default : System.Web.UI.Page
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
                    BindContractors();
                }
            }
        }

        private void BindContractors()
        {
            GrdContractors.DataSource = getdt.GetContractors();
            GrdContractors.DataBind();
        }

        protected void GrdContractors_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdContractors.PageIndex = e.NewPageIndex;
            BindContractors();
        }

        protected void GrdContractors_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string id = e.CommandArgument.ToString();
            if (e.CommandName == "delete")
            {
                int cnt = getdt.Contractor_Delete(new Guid(id));
                if (cnt > 0)
                {
                    BindContractors();
                }
            }
        }

        protected void GrdContractors_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void GrdContractors_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[5].Text = WebConfigurationManager.AppSettings["Domain"] + " Project Number";
            }
        }
    }
}