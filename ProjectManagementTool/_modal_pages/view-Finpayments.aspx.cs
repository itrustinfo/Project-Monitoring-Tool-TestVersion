using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using ProjectManager.DAL;

namespace ProjectManagementTool._modal_pages
{
    public partial class view_Finpayments : System.Web.UI.Page
    {
        DBGetData getdata = new DBGetData();
        TaskUpdate TKUpdate = new TaskUpdate();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Username"] == null)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
            }
            if (!IsPostBack)
            {
                if (Request.QueryString["MonthID"] != null)
                {
                    AddPayments.HRef = "~/_modal_pages/add-RABillPayments.aspx?monthUID=" + Request.QueryString["MonthID"].ToString() + "&WkpgUID=" + Request.QueryString["WkpgUID"].ToString();
                    LoadRABillPayments();
                }
            }
        }

        private void LoadRABillPayments()
        {
            DataSet ds = getdata.GetRABillPaymentsbyMonth(new Guid(Request.QueryString["MonthID"].ToString()));
            GrdRABills.DataSource = ds;
            GrdRABills.DataBind();
        }
    }
}