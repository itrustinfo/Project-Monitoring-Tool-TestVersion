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
    public partial class add_invoicededuction : System.Web.UI.Page
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
                    BindInvoiceMaster(Request.QueryString["WorkUID"]);
                    BindDeductions();
                    if (Request.QueryString["InvoiceMaster_UID"] != null && Request.QueryString["Mobilization"] != null)
                    {
                        DDLInvoiceMaster.SelectedValue = Request.QueryString["InvoiceMaster_UID"];
                        var index = DDLDeduction.Items.IndexOf(DDLDeduction.Items.FindByText("MOBILIZATION ADVANCE"));
                        if (Request.QueryString["Mobilization"] == "true")
                        {
                            DDLDeduction.SelectedIndex = index;
                            DDLDeduction.Enabled = false;
                        }
                        else
                        {                            
                            DDLDeduction.Items.RemoveAt(index);
                            DDLDeduction.Enabled = true;
                        }
                        BindInvoiceTotalAmount(Request.QueryString["Mobilization"]);
                    }
                    if (Request.QueryString["Invoice_DeductionUID"] != null)
                    {
                        BindInvoiceDeduction(Request.QueryString["Invoice_DeductionUID"]);
                    }
                    else
                    {
                        DDLMode_SelectedIndexChanged(sender, e);
                    }
                }
            }
        }

        private void BindInvoiceDeduction(string Invoice_DeductionUID)
        {
            DataSet ds = invoice.GetInvoiceDeduction_by_Invoice_DeductionUID(new Guid(Invoice_DeductionUID));
            if (ds.Tables[0].Rows.Count > 0)
            {
                DDLInvoiceMaster.SelectedValue = ds.Tables[0].Rows[0]["InvoiceMaster_UID"].ToString();
                DDLDeduction.SelectedValue = ds.Tables[0].Rows[0]["Deduction_UID"].ToString();
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

                if (ds.Tables[0].Rows[0]["Amount"].ToString() == "0")
                {
                    txtamount.Value = CurrencySymbol + " " + "0";
                }
                else
                {
                    txtamount.Value = CurrencySymbol + " " + Convert.ToDouble(ds.Tables[0].Rows[0]["Amount"].ToString()).ToString("#,##.##", CultureInfo.CreateSpecificCulture(ds.Tables[0].Rows[0]["Currency_CultureInfo"].ToString()));
                }

                if (ds.Tables[0].Rows[0]["Deduction_Mode"].ToString() == "Amount")
                {
                    txtperentage.Disabled = true;
                    txtamount.Disabled = false;
                    DDLMode.SelectedValue = "Amount";
                }
                else
                {
                    txtperentage.Disabled = false;
                    txtamount.Disabled = true;
                    DDLMode.SelectedValue = "Percentage";
                }
                DDLMode.Enabled = false;
                txtperentage.Value = ds.Tables[0].Rows[0]["Percentage"].ToString();
            }
        }
        private void BindInvoiceMaster(string WorkpackageUID)
        {
            DataSet ds = invoice.GetInvoiceMaster_by_WorkpackageUID(new Guid(WorkpackageUID));
            DDLInvoiceMaster.DataTextField = "Invoice_Number";
            DDLInvoiceMaster.DataValueField = "InvoiceMaster_UID";
            DDLInvoiceMaster.DataSource = ds;
            DDLInvoiceMaster.DataBind();
            DDLInvoiceMaster.Items.Insert(0,new ListItem("--Select--", ""));
        }

        private void BindDeductions()
        {
            DataSet ds = getdata.GetDeductionMaster();
            DDLDeduction.DataTextField = "DeductionsDescription";
            DDLDeduction.DataValueField = "UID";
            DDLDeduction.DataSource = ds;
            DDLDeduction.DataBind();
            DDLDeduction.Items.Insert(0,new ListItem("--Select--", ""));
        }
        private void BindInvoiceTotalAmount(string Mobilization)
        {
            DataSet ds = invoice.GetInvoiceMaster_by_InvoiceMaster_UID(new Guid(Request.QueryString["InvoiceMaster_UID"]));
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (Mobilization == "true")
                {
                    txtInvoiceamount.Text = ds.Tables[0].Rows[0]["Invoice_TotalAmount"].ToString();
                }
                else
                {
                    string Percentage = invoice.GetMobilizationInvoiceDeduction_by_InvoiceMaster_UID(new Guid(Request.QueryString["InvoiceMaster_UID"]));
                    if (float.Parse(Percentage) > 0)
                    {
                        double finalmount = Convert.ToDouble(ds.Tables[0].Rows[0]["Invoice_TotalAmount"].ToString()) - ((Convert.ToDouble(ds.Tables[0].Rows[0]["Invoice_TotalAmount"].ToString()) * float.Parse(Percentage)) / 100);
                        txtInvoiceamount.Text = Convert.ToString(finalmount);
                    }
                    else
                    {
                        txtInvoiceamount.Text = ds.Tables[0].Rows[0]["Invoice_TotalAmount"].ToString();
                    }
                }
                
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                Guid Invoice_DeductionUID;
                if (Request.QueryString["Invoice_DeductionUID"] != null)
                {
                    Invoice_DeductionUID = new Guid(Request.QueryString["Invoice_DeductionUID"]);
                }
                else
                {
                    Invoice_DeductionUID = Guid.NewGuid();
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
                if (DDLMode.SelectedValue != "Percentage")
                {
                    txtperentage.Value = "0";
                }
                txtamount.Value = txtamount.Value.Replace(",", "").Replace("₹ ", "").Replace("$ ", "").Replace("¥ ", "").Replace("₹", "").Replace("$", "").Replace("¥", "").Trim();
                if (txtamount.Value == "")
                {
                    txtamount.Value = "0";
                }
                int cnt = invoice.InvoiceDeduction_InsertorUpdate(Invoice_DeductionUID, new Guid(Request.QueryString["WorkUID"]), new Guid(DDLInvoiceMaster.SelectedValue), new Guid(DDLDeduction.SelectedValue), float.Parse(txtamount.Value), float.Parse(txtperentage.Value), (DDLCurrency.SelectedItem.Text == "₹ (RUPEE)") ? "&#x20B9;" : (DDLCurrency.SelectedItem.Text == "$ (USD)") ? "&#36;" : "&#165;", Currecncy_CultureInfo, DDLMode.SelectedValue);
                if (cnt > 0)
                {

                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
                }
                else if (cnt == -1)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Invoice Deduction already exists for the invoice : " + DDLInvoiceMaster.SelectedItem.Text + "');</script>");
                }

            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code : AID-01 there is a problem with these feature. please contact system admin.');</script>");
            }
        }

        protected void DDLDeduction_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDLDeduction.SelectedItem.Text == "SGST" || DDLDeduction.SelectedItem.Text == "CGST" || DDLDeduction.SelectedItem.Text == "GST")
            {
                string sVal = invoice.GetGST_Calculation_Value("GST Calculation");
                if (sVal != "" && !sVal.StartsWith("Error"))
                {
                    Hidden1.Value = sVal;
                }
            }
        }

        protected void DDLMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDLMode.SelectedValue == "Percentage")
            {
                PercentageDiv.Visible = true;
                //txtamount.Disabled = true;
                txtperentage.Value = "";
            }
            else
            {
                PercentageDiv.Visible = false;
                //txtamount.Disabled = false;
                txtperentage.Value = "0";
            }
        }
    }
}