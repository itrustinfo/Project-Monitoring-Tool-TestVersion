using ProjectManagementTool.DAL;
using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagementTool._modal_pages
{
    public partial class add_invoicemaster : System.Web.UI.Page
    {
        Invoice invoice = new Invoice();
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
                    if (Request.QueryString["InvoiceMaster_UID"] != null)
                    {
                        BindInvoiceMaster(Request.QueryString["InvoiceMaster_UID"]);
                    }
                }
            }
        }

        private void BindInvoiceMaster(string InvoiceMaster_UID)
        {
            DataSet ds = invoice.GetInvoiceMaster_by_InvoiceMaster_UID(new Guid(InvoiceMaster_UID));
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtinvoicedesc.Text = ds.Tables[0].Rows[0]["Invoice_Desc"].ToString();
                txtinvoicenumber.Text = ds.Tables[0].Rows[0]["Invoice_Number"].ToString();
                if (ds.Tables[0].Rows[0]["Invoice_Date"].ToString() != null && ds.Tables[0].Rows[0]["Invoice_Date"].ToString() != "")
                {
                    dtInvoiceDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["Invoice_Date"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                }

                string CurrencySymbol = "";
                if (ds.Tables[0].Rows[0]["Currency"].ToString() == "&#x20B9;")
                {
                    DDLCurrency.SelectedIndex = 0;
                    CurrencySymbol = "₹";
                }
                else if (ds.Tables[0].Rows[0]["Currency"].ToString() == "&#36;")
                {
                    DDLCurrency.SelectedIndex = 1;
                    CurrencySymbol = "$";
                }
                else
                {
                    DDLCurrency.SelectedIndex = 2;
                    CurrencySymbol = "¥";
                }
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                string sDate1 = "";
                DateTime CDate1 = DateTime.Now;

                if (dtInvoiceDate.Text != "")
                {
                    sDate1 = dtInvoiceDate.Text;
                }
                else
                {
                    sDate1 = DateTime.Now.ToString("dd/MM/yyyy");
                }
                
                sDate1 = getdata.ConvertDateFormat(sDate1);
                CDate1 = Convert.ToDateTime(sDate1);

                Guid InvoiceMaster_UID;
                if (Request.QueryString["InvoiceMaster_UID"] != null)
                {
                    InvoiceMaster_UID = new Guid(Request.QueryString["InvoiceMaster_UID"]);
                }
                else
                {
                    InvoiceMaster_UID = Guid.NewGuid();
                }
                string Currecncy_CultureInfo = "";
                if (DDLCurrency.SelectedItem.Text == "₹ (RUPEE)")
                {
                    Currecncy_CultureInfo = "en-IN";
                }
                else if (DDLCurrency.SelectedItem.Text == "$ (USD)")
                {
                    Currecncy_CultureInfo = "en-US";
                }
                else
                {
                    Currecncy_CultureInfo = "ja-JP";
                }
                int cnt = invoice.InvoiceMaster_InsertorUpdate(InvoiceMaster_UID, new Guid(Request.QueryString["PrjUID"]), new Guid(Request.QueryString["WorkUID"]), txtinvoicenumber.Text, txtinvoicedesc.Text, CDate1, (DDLCurrency.SelectedItem.Text == "₹ (RUPEE)") ? "&#x20B9;" : (DDLCurrency.SelectedItem.Text == "$ (USD)") ? "&#36;" : "&#165;", Currecncy_CultureInfo);
                if (cnt > 0)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
                }
                else if (cnt == -1)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Invoice Number alreday exists. Try with different Invoice Number');</script>");
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code : AIM-01 there is a problem with these feature. please contact system admin.');</script>");
            }
        }
    }
}