using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagementTool._modal_pages
{
    public partial class add_contractor : System.Web.UI.Page
    {
        DBGetData getdt = new DBGetData();
        TaskUpdate gettk = new TaskUpdate();
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
                    LblProjectNumber.InnerText = WebConfigurationManager.AppSettings["Domain"] + " Project Number";
                    if (Request.QueryString["type"] == "edit")
                    {
                        BindContractor(Request.QueryString["ContractID"]);
                        
                    }
                }
            }
        }

        private void BindContractor(string Contractor_UID)
        {
            DataSet ds = getdt.GetContractors_by_ContractorUID(new Guid(Contractor_UID));
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtcontractorname.Text = ds.Tables[0].Rows[0]["Contractor_Name"].ToString();
                txttype.Text = ds.Tables[0].Rows[0]["Type_of_Contract"].ToString();
                txtcontractorcode.Text= ds.Tables[0].Rows[0]["Contractor_Code"].ToString();
                txtrepresentatives.Text = ds.Tables[0].Rows[0]["Contractor_Representatives"].ToString();
                txtrepresentativesdetails.Text = ds.Tables[0].Rows[0]["Contractor_Representatives_Details"].ToString();
                txtCompanyDetails.Text = ds.Tables[0].Rows[0]["Company_Details"].ToString();
                txtduration.Text = ds.Tables[0].Rows[0]["Contract_Duration"].ToString();
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

                if (ds.Tables[0].Rows[0]["Contract_Value"].ToString() == "0")
                {
                    txtcontractvalue.Value = CurrencySymbol + " 0";
                }
                else
                {
                    txtcontractvalue.Value = CurrencySymbol + " " + Convert.ToDouble(ds.Tables[0].Rows[0]["Contract_Value"].ToString()).ToString("#,##.##", CultureInfo.CreateSpecificCulture(ds.Tables[0].Rows[0]["Currency_CultureInfo"].ToString()));
                }
                
                txtnjseinumber.Text = ds.Tables[0].Rows[0]["NJSEI_Number"].ToString();
                txtProjectSpecificNumber.Text = ds.Tables[0].Rows[0]["ProjectSpecific_Number"].ToString();
                if (ds.Tables[0].Rows[0]["Letter_of_Acceptance"].ToString() != "")
                {
                    dtAcceptanceDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["Letter_of_Acceptance"].ToString()).ToString("dd/MM/yyyy",CultureInfo.InvariantCulture);
                }
                if (ds.Tables[0].Rows[0]["Contract_Completion_Date"].ToString() != "")
                {
                    dtCompletionDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["Contract_Completion_Date"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                if (ds.Tables[0].Rows[0]["Contract_StartDate"].ToString() != "")
                {
                    dtStartDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["Contract_StartDate"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                if (ds.Tables[0].Rows[0]["Contract_Agreement_Date"].ToString() != "")
                {
                    dtAgreementDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["Contract_Agreement_Date"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                Guid Contractor_UID;
                if (Request.QueryString["type"] == "Add")
                {
                    Contractor_UID = Guid.NewGuid();
                }
                else
                {
                    Contractor_UID = new Guid(Request.QueryString["ContractID"]);
                }
                string sDate1 = "", sDate2 = "", sDate3 = "", sDate4 = "";
                DateTime CDate1 = DateTime.Now, CDate2 = DateTime.Now, CDate3 = DateTime.Now, CDate4 = DateTime.Now;
                //
                if (dtAcceptanceDate.Text !="")
                {
                    sDate1 = dtAcceptanceDate.Text;
                    //sDate1 = sDate1.Split('/')[1] + "/" + sDate1.Split('/')[0] + "/" + sDate1.Split('/')[2];
                    sDate1 = getdt.ConvertDateFormat(sDate1);
                    CDate1 = Convert.ToDateTime(sDate1);
                }
                //
                if (dtCompletionDate.Text != "")
                {
                    sDate2 = dtCompletionDate.Text;
                    //sDate2 = sDate2.Split('/')[1] + "/" + sDate2.Split('/')[0] + "/" + sDate2.Split('/')[2];
                    sDate2 = getdt.ConvertDateFormat(sDate2);
                    CDate2 = Convert.ToDateTime(sDate2);
                }
                //
                if (dtStartDate.Text != "")
                {
                    sDate3 = dtStartDate.Text;
                    //sDate3 = sDate3.Split('/')[1] + "/" + sDate3.Split('/')[0] + "/" + sDate3.Split('/')[2];
                    sDate3 = getdt.ConvertDateFormat(sDate3);
                    CDate3 = Convert.ToDateTime(sDate3);
                }

                if (dtAgreementDate.Text != "")
                {
                    sDate4 = dtAgreementDate.Text;
                    //sDate4 = sDate4.Split('/')[1] + "/" + sDate4.Split('/')[0] + "/" + sDate4.Split('/')[2];
                    sDate4 = getdt.ConvertDateFormat(sDate4);
                    CDate4 = Convert.ToDateTime(sDate4);
                }

                txtcontractvalue.Value = txtcontractvalue.Value.Replace(",", "").Replace("₹ ", "").Replace("$ ", "").Replace("¥ ", "").Replace("₹", "").Replace("$", "").Replace("¥", "").Trim();
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
                int cnt = getdt.InsertorUpdateContractor(Contractor_UID, txtcontractorname.Text, txtrepresentatives.Text, txttype.Text, Convert.ToDouble(txtcontractvalue.Value), Convert.ToInt32(txtduration.Text),
                    CDate1, CDate2, CDate3, CDate4, (DDLCurrency.SelectedItem.Text == "₹ (RUPEE)") ? "&#x20B9;" : (DDLCurrency.SelectedItem.Text == "$ (USD)") ? "&#36;" : "&#165;", Currecncy_CultureInfo, txtcontractorcode.Text, txtnjseinumber.Text, txtProjectSpecificNumber.Text,txtrepresentativesdetails.Text,txtCompanyDetails.Text);
                if (cnt > 0)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
                }
                else if (cnt == -1)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('NJSEI Project Number already exists. Please try with different name.');</script>");
                }
                else if (cnt == -2)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Project Specific Package Number already exists. Please try with different name.');</script>");
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code AC-01 there is a problem with this feature. Please contact system admin.');</script>");
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code AC-01 there is a problem with this feature. Please contact system admin.');</script>");
            }
            
        }
    }
}