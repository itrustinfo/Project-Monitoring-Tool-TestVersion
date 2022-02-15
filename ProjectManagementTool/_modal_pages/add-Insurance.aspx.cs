using Newtonsoft.Json;
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
    public partial class add_Insurance : System.Web.UI.Page
    {
        DBGetData getdata = new DBGetData();
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
                    BindProject();
                    DDlProject_SelectedIndexChanged(sender, e);

                    if (Request.QueryString["InsuranceUID"] != null)
                    {
                        BindInsurance(Request.QueryString["InsuranceUID"]);
                    }
                    if (Request.QueryString["View"] != null)
                    {
                        btnSubmit.Visible = false;
                        DDlProject.Enabled = false;
                        DDLWorkPackage.Enabled = false;
                        txtvendorname.Enabled = false;
                        txtvendoraddress.Enabled = false;
                        dtInsuredDate.Enabled = false;
                        dtMaturityDate.Enabled = false;
                        DDLCurrency.Enabled = false;
                        DDLFreqency.Enabled = false;
                        DDLPolicyStatus.Enabled = false;
                        txtPolicy_Number.Disabled = true;
                        txtNominee.Enabled = false;
                        txtamount.Disabled = true;
                        txtpremiumamount.Disabled = true;
                        txtName_of_InsuranceCompany.Enabled = false;
                        txtBranch.Enabled = false;
                            
                    }
                }
            }
        }

        private void BindProject()
        {
            DataTable ds = new DataTable();
            if (Session["TypeOfUser"].ToString() == "U" || Session["TypeOfUser"].ToString() == "MD" || Session["TypeOfUser"].ToString() == "VP")
            {

                ds = gettk.GetProjects();
            }
            else if (Session["TypeOfUser"].ToString() == "PA")
            {

                //ds = gettk.GetProjects_by_UserUID(new Guid(Session["UserUID"].ToString()));
                ds = gettk.GetAssignedProjects_by_UserUID(new Guid(Session["UserUID"].ToString()));
            }
            else
            {

                //ds = gettk.GetProjects();
                ds = gettk.GetAssignedProjects_by_UserUID(new Guid(Session["UserUID"].ToString()));
            }
            DDlProject.DataTextField = "ProjectName";
            DDlProject.DataValueField = "ProjectUID";
            DDlProject.DataSource = ds;
            DDlProject.DataBind();
            if (Request.QueryString["ProjectUID"] != null)
            {
                DDlProject.SelectedValue = Request.QueryString["ProjectUID"];
            }

        }

        private void BindInsurance(string InsuranceUID)
        {
            DataSet ds = getdata.GetInsuranceSelect_by_InsuranceUID(new Guid(InsuranceUID));
            if (ds.Tables[0].Rows.Count > 0)
            {
                DDlProject.SelectedValue = ds.Tables[0].Rows[0]["ProjectUID"].ToString();
                DDLWorkPackage.SelectedValue = ds.Tables[0].Rows[0]["WorkPackageUID"].ToString();
                txtvendorname.Text = ds.Tables[0].Rows[0]["Vendor_Name"].ToString();
                txtvendoraddress.Text = ds.Tables[0].Rows[0]["Vendor_Address"].ToString();
                txtName_of_InsuranceCompany.Text = ds.Tables[0].Rows[0]["Name_of_InsuranceCompany"].ToString();
                txtBranch.Text = ds.Tables[0].Rows[0]["Branch"].ToString();
                txtPolicy_Number.Value = ds.Tables[0].Rows[0]["Policy_Number"].ToString();
                DDLPolicyStatus.SelectedValue = ds.Tables[0].Rows[0]["Policy_Status"].ToString();
                txtNominee.Text = ds.Tables[0].Rows[0]["Nominee"].ToString();

                if (ds.Tables[0].Rows[0]["Maturity_Date"].ToString() != null && ds.Tables[0].Rows[0]["Maturity_Date"].ToString() != "")
                {
                    dtMaturityDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["Maturity_Date"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                }

                if (ds.Tables[0].Rows[0]["Insured_Date"].ToString() != null && ds.Tables[0].Rows[0]["Insured_Date"].ToString() != "")
                {
                    dtInsuredDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["Insured_Date"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                }

                if (ds.Tables[0].Rows[0]["FirstPremium_Duedate"].ToString() != null && ds.Tables[0].Rows[0]["FirstPremium_Duedate"].ToString() != "")
                {
                    dtFirstPremiunDueDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["FirstPremium_Duedate"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
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

                txtamount.Value = CurrencySymbol + " " +  Convert.ToDouble(ds.Tables[0].Rows[0]["Insured_Amount"].ToString()).ToString("#,##.##", CultureInfo.CreateSpecificCulture(ds.Tables[0].Rows[0]["Currency_CultureInfo"].ToString()));
                txtpremiumamount.Value = CurrencySymbol + " " + Convert.ToDouble(ds.Tables[0].Rows[0]["Premium_Amount"].ToString()).ToString("#,##.##", CultureInfo.CreateSpecificCulture(ds.Tables[0].Rows[0]["Currency_CultureInfo"].ToString()));
                DDLFreqency.SelectedValue = ds.Tables[0].Rows[0]["Frequency"].ToString();
            }
        }
        protected void DDlProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDlProject.SelectedValue != "")
            {
                DataSet ds = getdata.GetWorkPackages_By_ProjectUID(new Guid(DDlProject.SelectedValue));
                if (ds.Tables[0].Rows.Count > 0)
                {

                    DDLWorkPackage.DataTextField = "Name";
                    DDLWorkPackage.DataValueField = "WorkPackageUID";
                    DDLWorkPackage.DataSource = ds;
                    DDLWorkPackage.DataBind();

                    if (Request.QueryString["WorkpackgeUID"] != null)
                    {
                        DDLWorkPackage.SelectedValue = Request.QueryString["WorkpackgeUID"];
                    }
                }
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                Guid InsuranceUID = Guid.NewGuid();
                int ActionType = 1;
                string Function = "Add Insurance";
                if (Request.QueryString["InsuranceUID"] != null)
                {
                    InsuranceUID = new Guid(Request.QueryString["InsuranceUID"]);
                    Function = "Edit Insurance";
                    ActionType = 2;
                }
                string sDate1 = "", sDate2 = "", sDate3 = "";
                DateTime CDate1 = DateTime.Now, CDate2 = DateTime.Now;
                sDate1 = dtMaturityDate.Text;
                //sDate1 = sDate1.Split('/')[1] + "/" + sDate1.Split('/')[0] + "/" + sDate1.Split('/')[2];
                sDate1 = getdata.ConvertDateFormat(sDate1);
                CDate1 = Convert.ToDateTime(sDate1);

                sDate2 = dtInsuredDate.Text;
                //sDate1 = sDate1.Split('/')[1] + "/" + sDate1.Split('/')[0] + "/" + sDate1.Split('/')[2];
                sDate2 = getdata.ConvertDateFormat(sDate2);
                CDate2 = Convert.ToDateTime(sDate2);

                if (dtFirstPremiunDueDate.Text != "")
                {
                    sDate3 = dtFirstPremiunDueDate.Text;
                    sDate3 = getdata.ConvertDateFormat(sDate3);
                }

                string Currecncy_CultureInfo = "";
                string Currency = "";
                if (DDLCurrency.SelectedItem.Text == "₹ (RUPEE)")
                {
                    Currecncy_CultureInfo = "en-IN";
                    Currency = "&#x20B9;";
                }
                else if (DDLCurrency.SelectedItem.Text == "$ (USD)")
                {
                    Currecncy_CultureInfo = "en-US";
                    Currency = "&#36;";
                }
                else
                {
                    Currecncy_CultureInfo = "ja-JP";
                    Currency = "&#165;";
                }

                txtamount.Value = txtamount.Value.Replace(",", "").Replace("₹ ", "").Replace("$ ", "").Replace("¥ ", "").Replace("₹", "").Replace("$", "").Replace("¥", "").Trim();
                txtpremiumamount.Value = txtpremiumamount.Value.Replace(",", "").Replace("₹ ", "").Replace("$ ", "").Replace("¥ ", "").Replace("₹", "").Replace("$", "").Replace("¥", "").Trim();
                bool ret = getdata.InsertorUpdateInsurance(InsuranceUID, txtvendorname.Text, txtvendoraddress.Text, txtName_of_InsuranceCompany.Text, txtBranch.Text, txtPolicy_Number.Value, DDLPolicyStatus.SelectedValue, CDate1, txtNominee.Text, new Guid(DDlProject.SelectedValue), new Guid(DDLWorkPackage.SelectedValue),CDate2,
                    decimal.Parse(txtamount.Value), decimal.Parse(txtpremiumamount.Value),Convert.ToInt32(DDLFreqency.SelectedValue), (DDLCurrency.SelectedItem.Text == "₹ (RUPEE)") ? "&#x20B9;" : (DDLCurrency.SelectedItem.Text == "$ (USD)") ? "&#36;" : "&#165;", Currecncy_CultureInfo, sDate3);
                if (ret)
                {
                    if (WebConfigurationManager.AppSettings["Dbsync"] == "Yes")
                    {
                        string WebAPIURL = "";
                        DataSet copysite = getdata.GetDataCopySiteDetails_by_ProjectUID(new Guid(DDlProject.SelectedValue));
                        if (copysite.Tables[0].Rows.Count > 0)
                        {
                            WebAPIURL = copysite.Tables[0].Rows[0]["DataCopySiteURL"].ToString();
                            WebAPIURL = WebAPIURL + "Activity/AddInsurance";
                            string postData = "InsuranceUID=" + InsuranceUID + "&ProjectName=" + DDlProject.SelectedItem.Text + "&WorkpackageName=" + DDLWorkPackage.SelectedItem.Text + "&Vendor_Name=" + txtvendorname.Text + "&Vendor_Address=" + txtvendoraddress.Text + "&Name_of_InsuranceCompany=" + txtName_of_InsuranceCompany.Text + "&Branch="+ txtBranch.Text + "&Policy_Number=" + txtPolicy_Number.Value + "&Policy_Status=" + DDLPolicyStatus.SelectedValue + "&Maturity_Date=" + CDate1 +
                                "&Nominee=" + txtNominee.Text + "&Insured_Date=" + CDate2 + "&Insured_Amount=" + txtamount.Value + "&Premium_Amount=" + txtpremiumamount.Value + "&Frequency=" + DDLFreqency.SelectedValue + "&Currency=" + Currency + "&Currency_CultureInfo=" + Currecncy_CultureInfo + "&FirstPremium_Duedate=" + sDate3;
                            string sReturnStatus = getdata.webPostMethod(postData, WebAPIURL);
                            if (!sReturnStatus.StartsWith("Error:"))
                            {
                                dynamic DynamicData = JsonConvert.DeserializeObject(sReturnStatus);
                                string RetStatus = DynamicData.Status;
                                if (!RetStatus.StartsWith("Error:"))
                                {
                                    int rCnt = getdata.ServerFlagsUpdate(InsuranceUID.ToString(), ActionType, "Insurance", "Y", "InsuranceUID");
                                    if (rCnt > 0)
                                    {
                                    }
                                }
                                else
                                {
                                    string ErrorMessage = DynamicData.Message;
                                    getdata.WebAPIStatusInsert(Guid.NewGuid(), WebAPIURL, postData, ErrorMessage, "Failure", Function, "AddInsurance", InsuranceUID);
                                    //Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error: DBSync =" + ErrorMessage + "');</script>");
                                }
                            }
                            else
                            {
                                getdata.WebAPIStatusInsert(Guid.NewGuid(), WebAPIURL, postData, sReturnStatus, "Failure", Function, "AddInsurance ", InsuranceUID);
                            }
                        }
                    }
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code : AINS-01 there is a problem with these feature. please contact system admin.');</script>");
            }
        }
    }
}