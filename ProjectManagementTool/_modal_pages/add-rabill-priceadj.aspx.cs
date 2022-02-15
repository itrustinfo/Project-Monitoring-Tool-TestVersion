using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProjectManagementTool.DAL;
using ProjectManager.DAL;
using System.Data;

namespace ProjectManagementTool._modal_pages
{
    public partial class add_rabill_priceadj : System.Web.UI.Page
    {
        DBGetData dbgetdata = new DBGetData();
        TaskUpdate gettk = new TaskUpdate();
        Invoice invoice = new Invoice();
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                LoadRABills();
                if(Request.QueryString["type"]!= null)
                {
                    DataSet ds = invoice.GetRABillPriceADjMAster_UID(new Guid(Request.QueryString["MasterUID"].ToString()));
                    if(ds.Tables[0].Rows.Count > 0)
                    {
                        DDLInvoice.SelectedValue = ds.Tables[0].Rows[0]["RABillUID"].ToString();
                        txtDescription.Text = ds.Tables[0].Rows[0]["Description"].ToString();
                        dtInitialDate.Text =Convert.ToDateTime(ds.Tables[0].Rows[0]["InitialIndicesDate"].ToString()).ToString("dd/MM/yyyy");
                        dtLatestDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["LatestIndicesDate"].ToString()).ToString("dd/MM/yyyy");
                        DDLInvoice.Enabled = false;
                    }
                }
            }
        }

        private void LoadRABills()
        {
            DDLInvoice.DataSource = dbgetdata.GetInvoiceDetails_by_WorkpackageUID(new Guid(Request.QueryString["WorkpackageUID"].ToString()));
            DDLInvoice.DataTextField = "RABillNumber";
            DDLInvoice.DataValueField = "RABillUid";
            DDLInvoice.DataBind();
            DDLInvoice.Items.Insert(0, "--Select RABill--");
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (DDLInvoice.SelectedIndex == 0)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "Warning", "<script language='javascript'>alert('Please select RA Bill.');</script>");
                    return;
                }
                string sDate1 = dtInitialDate.Text;
                //sDate2 = sDate2.Split('/')[1] + "/" + sDate2.Split('/')[0] + "/" + sDate2.Split('/')[2];
                sDate1 = dbgetdata.ConvertDateFormat(sDate1);
                DateTime CDate1 = Convert.ToDateTime(sDate1);
                string sDate2 = dtLatestDate.Text;
                //sDate2 = sDate2.Split('/')[1] + "/" + sDate2.Split('/')[0] + "/" + sDate2.Split('/')[2];
                sDate2 = dbgetdata.ConvertDateFormat(sDate2);
                DateTime CDate2 = Convert.ToDateTime(sDate2);
                decimal RABillValue = invoice.GetRAbillPresentTotalAmount_by_RABill_UID(new Guid(DDLInvoice.SelectedValue));
                decimal PriceAdjFactor = 1;
                decimal PriceAdjValue = RABillValue * PriceAdjFactor;
                decimal RecievedAmount = RABillValue;
                decimal BalanceAmount = PriceAdjValue - RABillValue;
                Guid MasterUID = Guid.NewGuid();
                if (Request.QueryString["type"] != null)
                {
                    MasterUID = new Guid(Request.QueryString["MasterUID"].ToString());
                }
                    int result = invoice.InsertRABillPriceAdj_Master(MasterUID, new Guid(Request.QueryString["WorkpackageUID"].ToString()), new Guid(DDLInvoice.SelectedValue), txtDescription.Text, CDate1, CDate2,RABillValue,PriceAdjFactor,PriceAdjValue,RecievedAmount,BalanceAmount);
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");

            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}