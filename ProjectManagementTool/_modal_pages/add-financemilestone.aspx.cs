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
    public partial class add_financemilestone : System.Web.UI.Page
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
                    if (Request.QueryString["TaskUID"] != null)
                    {
                        BindCurrency();
                        GetTaskBudget(Request.QueryString["TaskUID"].ToString());
                    }
                    if (Request.QueryString["FinanceMileStoneUID"] != null)
                    {
                        BindMileStone(Request.QueryString["FinanceMileStoneUID"].ToString());
                    }
                }
            }
        }
        private void GetTaskBudget(string TaskUID)
        {
            DataSet ds = getdata.GetTaskDetails(TaskUID);
            if (ds.Tables[0].Rows.Count > 0)
            {
                TaskBudget.Value = ds.Tables[0].Rows[0]["Total_Budget"].ToString();
            }
            else
            {
                TaskBudget.Value = "0";
            }
        }
        private void BindCurrency()
        {
            DataSet ds = getdata.GetTaskDetails(Request.QueryString["TaskUID"].ToString());
            if (ds.Tables[0].Rows.Count > 0)
            {
                string CurrencySymbol = "";
                HiddenCurrency_CultureInfo.Value = ds.Tables[0].Rows[0]["Currency_CultureInfo"].ToString();
                if (ds.Tables[0].Rows[0]["Currency"].ToString() == "&#x20B9;")
                {
                    CurrencySymbol = "₹";
                }
                else if (ds.Tables[0].Rows[0]["Currency"].ToString() == "&#36;")
                {                    
                    CurrencySymbol = "$";
                }
                else
                {
                    CurrencySymbol = "¥";
                }
                HiddenVal.Value = CurrencySymbol;
            }
            else
            {
                HiddenVal.Value = "₹";
            }
        }
        private void BindMileStone(string FinanceUID)
        {
            DataSet ds = getdata.GetFinance_MileStonesDetails_By_Finance_MileStoneUID(new Guid(FinanceUID));
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtMileStone.Text = ds.Tables[0].Rows[0]["Finance_MileStoneName"].ToString();
                
                if (ds.Tables[0].Rows[0]["Finance_PlannedDate"].ToString() != "" && ds.Tables[0].Rows[0]["Finance_PlannedDate"].ToString() != null)
                {
                    dtPlannedDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["Finance_PlannedDate"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                
                if (string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Finance_AllowedPayment"].ToString()))
                {
                    txtallowedpayment.Text = HiddenVal.Value + " 0";
                }
                else
                {
                    txtallowedpayment.Text = HiddenVal.Value + " " + Convert.ToDouble(ds.Tables[0].Rows[0]["Finance_AllowedPayment"].ToString()).ToString("#,##.##", CultureInfo.CreateSpecificCulture(HiddenCurrency_CultureInfo.Value));
                }
                txtGST.Text = ds.Tables[0].Rows[0]["Finance_GST"].ToString();
                double TotalPayment = Convert.ToDouble(ds.Tables[0].Rows[0]["Finance_AllowedPayment"].ToString()) + ((Convert.ToDouble(ds.Tables[0].Rows[0]["Finance_AllowedPayment"].ToString()) * Convert.ToDouble(txtGST.Text)) / 100);
                
                if (string.IsNullOrEmpty(TotalPayment.ToString()))
                {
                    txttotalpayment.Text = HiddenVal.Value + " 0";
                }
                else
                {
                    txttotalpayment.Text = HiddenVal.Value + " " + TotalPayment.ToString("#,##.##", CultureInfo.CreateSpecificCulture(HiddenCurrency_CultureInfo.Value));
                }
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                Guid FinanceMileStoneUID = Guid.NewGuid();

                double AE = getdata.GetTask_ActualExpenditure_by_TaskUID(new Guid(Request.QueryString["TaskUID"]));

                double allowedpayment = 0;
                if (txtallowedpayment.Text != "")
                {
                    allowedpayment = Convert.ToDouble(txtallowedpayment.Text.Replace(",", "").Replace("₹ ", "").Replace("$ ", "").Replace("¥ ", "").Replace("₹", "").Replace("$", "").Replace("¥", "").Trim());
                }
                if ((AE + allowedpayment) > Convert.ToDouble(TaskBudget.Value))
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Allowed payment should be less than total task budget.');</script>");
                }
                else
                {
                    if (Request.QueryString["FinanceMileStoneUID"] != null)
                    {
                        FinanceMileStoneUID = new Guid(Request.QueryString["FinanceMileStoneUID"].ToString());
                    }
                    string sDate1 = "";
                    DateTime CDate1 = DateTime.Now, CDate2 = DateTime.Now;
                    if (dtPlannedDate.Text != "")
                    {
                        sDate1 = dtPlannedDate.Text;
                        //sDate1 = sDate1.Split('/')[1] + "/" + sDate1.Split('/')[0] + "/" + sDate1.Split('/')[2];
                        sDate1 = getdata.ConvertDateFormat(sDate1);
                        CDate1 = Convert.ToDateTime(sDate1);
                    }
                    double GST = 0;
                    if (txtGST.Text != "")
                    {
                        GST = Convert.ToDouble(txtGST.Text);
                    }

                    bool result = getdata.InsertorUpdateFinanceMileStone(FinanceMileStoneUID, new Guid(Request.QueryString["TaskUID"].ToString()), txtMileStone.Text, allowedpayment, GST, CDate1, new Guid(Session["UserUID"].ToString()));
                    if (result)
                    {
                        Session["SelectedActivity"] = Request.QueryString["TaskUID"].ToString();
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
                    }
                }
                
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('There is a problem with this feature. Please contact system admin');</script>");
            }
        }
    }
}