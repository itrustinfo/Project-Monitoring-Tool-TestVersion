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
    public partial class add_bankguarantee : System.Web.UI.Page
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
                  

                    if (Request.QueryString["Bank_GuaranteeUID"] != null)
                    {
                        BindProject();
                        BindBankGuarantee();
                        DDlProject_SelectedIndexChanged(sender, e);
                    }
                    else
                    {
                        BindProject();
                        DDlProject_SelectedIndexChanged(sender, e);
                    }
                }
            }
            
        }

        private void BindProject()
        {
            DataTable ds = new DataTable();
            if (Session["TypeOfUser"].ToString() == "U" || Session["TypeOfUser"].ToString() =="MD" || Session["TypeOfUser"].ToString() == "VP")
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

        private void BindBankGuarantee()
        {
            DataSet ds = getdata.GetBankGuarantee_by_Bank_GuaranteeUID(new Guid(Request.QueryString["Bank_GuaranteeUID"]));
            if (ds.Tables[0].Rows.Count > 0)
            {
                DDlProject.SelectedValue = ds.Tables[0].Rows[0]["ProjectUID"].ToString();
                DDLWorkPackage.SelectedValue = ds.Tables[0].Rows[0]["WorkPackageUID"].ToString();
                //txtvendorname.Text = ds.Tables[0].Rows[0]["Vendor_Name"].ToString();
                //txtvendoraddress.Text = ds.Tables[0].Rows[0]["Vendor_Address"].ToString();
                //txtvalidity.Text = ds.Tables[0].Rows[0]["Validity"].ToString();
                txtNo_of_Collaterals.Text = ds.Tables[0].Rows[0]["No_of_Collaterals"].ToString();
                txtIFSC_Code.Text = ds.Tables[0].Rows[0]["IFSC_Code"].ToString();
                txtBank_Name.Text = ds.Tables[0].Rows[0]["Bank_Name"].ToString();
                txtBank_Branch.Text = ds.Tables[0].Rows[0]["Bank_Branch"].ToString();
                txtamount.Value = Convert.ToDouble(ds.Tables[0].Rows[0]["Amount"].ToString()).ToString("#,##.##", CultureInfo.CreateSpecificCulture(ds.Tables[0].Rows[0]["Currency_CultureInfo"].ToString()));
                txtBgNumber.Text = ds.Tables[0].Rows[0]["BG_Number"].ToString();
                if (ds.Tables[0].Rows[0]["Currency"].ToString() == "&#x20B9;")
                {
                    DDLCurrency.SelectedIndex = 0;
                }
                else if (ds.Tables[0].Rows[0]["Currency"].ToString() == "&#36;")
                {
                    DDLCurrency.SelectedIndex = 1;
                }
                else
                {
                    DDLCurrency.SelectedIndex = 2;
                }
                txtaddress.Text = ds.Tables[0].Rows[0]["Bank_Address"].ToString();
                if (ds.Tables[0].Rows[0]["Date_of_Guarantee"].ToString() != null && ds.Tables[0].Rows[0]["Date_of_Guarantee"].ToString() != "")
                {
                    dtdate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["Date_of_Guarantee"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                if (ds.Tables[0].Rows[0]["Date_of_Expiry"].ToString() != null && ds.Tables[0].Rows[0]["Date_of_Expiry"].ToString() != "")
                {
                    dtExpiry.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["Date_of_Expiry"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                if (ds.Tables[0].Rows[0]["Cliam_Date"].ToString() != null && ds.Tables[0].Rows[0]["Cliam_Date"].ToString() != "")
                {
                    dtClaimDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["Cliam_Date"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
            }

        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                Guid Bank_GuaranteeUID = Guid.NewGuid();
                string Function = "Add Bank Guarantee";
                int ActionType = 1;
                if (Request.QueryString["Bank_GuaranteeUID"] != null)
                {
                    Bank_GuaranteeUID = new Guid(Request.QueryString["Bank_GuaranteeUID"]);
                    Function = "Edit Bank Guarantee";
                    ActionType = 2;
                }
                string sDate1 = "", sDate2 = "",sDate3="";
                DateTime CDate1 = DateTime.Now, CDate2 = DateTime.Now, CDate3 = DateTime.Now;
                sDate1 = dtdate.Text;
                //sDate1 = sDate1.Split('/')[1] + "/" + sDate1.Split('/')[0] + "/" + sDate1.Split('/')[2];
                sDate1 = getdata.ConvertDateFormat(sDate1);
                CDate1 = Convert.ToDateTime(sDate1);

                sDate2 = dtExpiry.Text;
                //sDate2 = sDate2.Split('/')[1] + "/" + sDate2.Split('/')[0] + "/" + sDate2.Split('/')[2];
                sDate2 = getdata.ConvertDateFormat(sDate2);
                CDate2 = Convert.ToDateTime(sDate2);

                sDate3 = dtClaimDate.Text;
                sDate3 = getdata.ConvertDateFormat(sDate3);
                CDate3 = Convert.ToDateTime(sDate3);

                txtamount.Value = txtamount.Value.Replace(",", "");
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
                bool ret = getdata.InsertorUpdateBankGuarantee(Bank_GuaranteeUID, "", "", Convert.ToDouble(txtamount.Value), "0", CDate1, Convert.ToInt32(txtNo_of_Collaterals.Text), txtBank_Name.Text, txtBank_Branch.Text, txtIFSC_Code.Text, 
                    new Guid(DDLWorkPackage.SelectedValue), new Guid(DDlProject.SelectedValue), txtBgNumber.Text, CDate2, (DDLCurrency.SelectedItem.Text == "₹ (RUPEE)") ? "&#x20B9;" : (DDLCurrency.SelectedItem.Text == "$ (USD)") ? "&#36;" : "&#165;", Currecncy_CultureInfo,txtaddress.Text, CDate3);
                if (ret)
                {
                    if (WebConfigurationManager.AppSettings["Dbsync"] == "Yes")
                    {
                        string WebAPIURL = "";
                        DataSet copysite = getdata.GetDataCopySiteDetails_by_ProjectUID(new Guid(DDlProject.SelectedValue));
                        if (copysite.Tables[0].Rows.Count > 0)
                        {
                            WebAPIURL = copysite.Tables[0].Rows[0]["DataCopySiteURL"].ToString();
                            WebAPIURL = WebAPIURL + "Activity/AddorUpdateBankGuarantee";
                            string postData = "Bank_GuaranteeUID=" + Bank_GuaranteeUID + "&ProjectName=" + DDlProject.SelectedItem.Text + "&WorkpackageName=" + DDLWorkPackage.SelectedItem.Text + "&Vendor_Name=''&Vendor_Address=''&Amount=" + txtamount.Value + "&Validity=0&Date_of_Guarantee=" + CDate1 + "&No_of_Collaterals=" + txtNo_of_Collaterals.Text + "&Bank_Name=" + txtBank_Name.Text +
                                "&Bank_Branch=" + txtBank_Branch.Text + "&IFSC_Code=" + txtIFSC_Code.Text + "&BG_Number=" + txtBgNumber.Text + "&Date_of_Expiry=" + CDate2 + "&Currency=" + Currency + "&Currency_CultureInfo=" + Currecncy_CultureInfo + "&Bank_Address=" + txtaddress.Text + "&Cliam_Date=" + CDate3;
                            string sReturnStatus = getdata.webPostMethod(postData, WebAPIURL);
                            if (!sReturnStatus.StartsWith("Error:"))
                            {
                                dynamic DynamicData = JsonConvert.DeserializeObject(sReturnStatus);
                                string RetStatus = DynamicData.Status;
                                if (!RetStatus.StartsWith("Error:"))
                                {
                                    int rCnt = getdata.ServerFlagsUpdate(Bank_GuaranteeUID.ToString(), ActionType, "BankGuarantee", "Y", "Bank_GuaranteeUID");
                                    if (rCnt > 0)
                                    {
                                    }
                                }
                                else
                                {
                                    string ErrorMessage = DynamicData.Message;
                                    getdata.WebAPIStatusInsert(Guid.NewGuid(), WebAPIURL, postData, ErrorMessage, "Failure", Function, "AddorUpdateBankGuarantee", Bank_GuaranteeUID);
                                    //Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error: DBSync =" + ErrorMessage + "');</script>");
                                }
                            }
                            else
                            {
                                getdata.WebAPIStatusInsert(Guid.NewGuid(), WebAPIURL, postData, sReturnStatus, "Failure", Function, "AddorUpdateBankGuarantee", Bank_GuaranteeUID);
                            }
                        }
                    }
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code : ABG-01 there is a problem with these feature. please contact system admin.');</script>");
            }
        }

        protected void DDlProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataSet ds = getdata.GetWorkPackages_By_ProjectUID(new Guid(DDlProject.SelectedValue));
            if (ds.Tables[0].Rows.Count > 0)
            {

                DDLWorkPackage.DataTextField = "Name";
                DDLWorkPackage.DataValueField = "WorkPackageUID";
                DDLWorkPackage.DataSource = ds;
                DDLWorkPackage.DataBind();

            }
        }
    }
}