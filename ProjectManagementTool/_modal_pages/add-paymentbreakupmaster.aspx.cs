using ProjectManagementTool.DAL;
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
    public partial class add_paymentbreakupmaster : System.Web.UI.Page
    {
        DBGetData getdata = new DBGetData();
        TaskUpdate TKUpdate = new TaskUpdate();
        Invoice invoice = new Invoice();
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
                    if (Request.QueryString["Breakup_UID"] != null)
                    {
                        BindPaymentBreakTypes(Request.QueryString["Breakup_UID"]);
                    }
                }
            }
        }
        private void BindPaymentBreakTypes(string Breakup_UID)
        {
            DataSet ds = invoice.GetPaymentBreakupTypes_by_Breakup_UID(new Guid(Breakup_UID));
            if (ds.Tables[0].Rows.Count > 0)
            {
                txttype.Text = ds.Tables[0].Rows[0]["Breakup_Description"].ToString();
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                Guid Breakup_UID;
                if (Request.QueryString["Breakup_UID"] != null)
                {
                    Breakup_UID = new Guid(Request.QueryString["Breakup_UID"]);
                }
                else
                {
                    Breakup_UID = Guid.NewGuid();
                }
                int cnt = invoice.PaymentBreakupType_InsertOrUpdate(Breakup_UID, txttype.Text);
                if (cnt == -1)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Payment breakup type already exists. Try with different type name.');</script>");
                }
                else if (cnt > 0)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('There is a problem with this feature. Please contact system admin.');</script>");
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('There is a problem with this feature. Please contact system admin. Description : " + ex.Message + "');</script>");
            }
        }
    }
}