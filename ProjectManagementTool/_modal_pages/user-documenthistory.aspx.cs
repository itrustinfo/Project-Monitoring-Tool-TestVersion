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
    public partial class user_documenthistory : System.Web.UI.Page
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
                    BindUserDocumentHistroy(Request.QueryString["DocumentUID"]);
                }
            }
        }

        private void BindUserDocumentHistroy(string DocumentUID)
        {
            DataSet ds = getdata.GetDoucmentHistory_For_Uesr_by_DoucmentUID(new Guid(DocumentUID));
            GrdUserDocumentHsitroy.DataSource = ds;
            GrdUserDocumentHsitroy.DataBind();
        }

        public string GetUserName(string UserUID)
        {
            return getdata.getUserNameby_UID(new Guid(UserUID));
        }
        protected void GrdUserDocumentHsitroy_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdUserDocumentHsitroy.PageIndex = e.NewPageIndex;
            BindUserDocumentHistroy(Request.QueryString["DocumentUID"]);
        }

        protected void GrdUserDocumentHsitroy_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[1].Text != "&nbsp;")
                {
                    DataSet ds = getdata.GetDocumentActionHistory_For_Uesr_by_DoucmentUID(new Guid(Request.QueryString["DocumentUID"]), new Guid(e.Row.Cells[1].Text));
                    if (ds != null)
                    {
                        e.Row.Cells[1].Text = ds.Tables[0].Rows[0]["Downloaded"].ToString() != "" ? ds.Tables[0].Rows[0]["Downloaded"].ToString() : "0";
                        e.Row.Cells[2].Text = ds.Tables[0].Rows[0]["Viewed"].ToString() != "" ? ds.Tables[0].Rows[0]["Viewed"].ToString() : "0";
                    }
                }
            }
        }
    }
}