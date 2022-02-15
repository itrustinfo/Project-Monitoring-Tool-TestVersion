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
    public partial class view_financemilestonehistory : System.Web.UI.Page
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
                    if (Request.QueryString["Finance_MileStoneUID"] != null)
                    {
                        PaymentHistroyBind(Request.QueryString["Finance_MileStoneUID"]);
                    }
                }
            }
        }

        private void PaymentHistroyBind(string FinanceMileStoneUID)
        {
            DataSet ds = getdata.FinanceMileStonePaymentUpdate_Selectby_Finance_MileStoneUID(new Guid(FinanceMileStoneUID));
            GrdPaymentHistory.DataSource = ds;
            GrdPaymentHistory.DataBind();
        }

        protected void GrdPaymentHistory_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdPaymentHistory.PageIndex = e.NewPageIndex;
            PaymentHistroyBind(Request.QueryString["Finance_MileStoneUID"]);
        }

        protected void GrdPaymentHistory_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string FM_UID = e.CommandArgument.ToString();
            if (e.CommandName == "delete")
            {
                int cnt = getdata.FinanceMileStonePaymentUpdate_Delete(new Guid(FM_UID), new Guid(Session["UserUID"].ToString()));
                if (cnt > 0)
                {
                    PaymentHistroyBind(Request.QueryString["Finance_MileStoneUID"]);
                }
            }
        }

        protected void GrdPaymentHistory_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }
    }
}