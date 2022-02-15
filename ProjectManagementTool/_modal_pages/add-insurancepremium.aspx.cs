using Newtonsoft.Json;
using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagementTool._modal_pages
{
    public partial class add_insurancepremium : System.Web.UI.Page
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
                    if (Request.QueryString["PremiumUID"] != null)
                    {
                        BindPremium(Request.QueryString["PremiumUID"]);
                    }
                    else if (Request.QueryString["InsuranceUID"] != null)
                    {
                        DataSet ds = getdata.GetInsurancePremium_DueDate_NextDueDate(new Guid(Request.QueryString["InsuranceUID"]));
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            if (ds.Tables[0].Rows[0]["DueDate"].ToString() != null && ds.Tables[0].Rows[0]["DueDate"].ToString() != "")
                            {
                                dtDueDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["DueDate"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                            }
                            if (ds.Tables[0].Rows[0]["NextDueDate"].ToString() != null && ds.Tables[0].Rows[0]["NextDueDate"].ToString() != "")
                            {
                                HiddenNextDueDate.Value = Convert.ToDateTime(ds.Tables[0].Rows[0]["NextDueDate"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                            }

                            string CurrencySymbol = "";
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

                            HiddenCurrency.Value = CurrencySymbol;
                            HiddenCulturelInfo.Value = ds.Tables[0].Rows[0]["Currency_CultureInfo"].ToString();
                        }
                    }
                }
            }
        }

        private void BindPremium(string PremiumUID)
        {
            DataSet ds = getdata.GetInsurancePremiumSelect_by_PremiumUID(new Guid(PremiumUID));
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtpremiumamount.Value = HiddenCurrency.Value + " " + Convert.ToDouble(ds.Tables[0].Rows[0]["Premium_Paid"].ToString()).ToString("#,##.##", CultureInfo.CreateSpecificCulture(ds.Tables[0].Rows[0]["Currency_CultureInfo"].ToString()));
                txtinterest.Value = HiddenCurrency.Value + " " + Convert.ToDouble(ds.Tables[0].Rows[0]["Interest"].ToString()).ToString("#,##.##", CultureInfo.CreateSpecificCulture(ds.Tables[0].Rows[0]["Currency_CultureInfo"].ToString()));
                txtpenalty.Value = HiddenCurrency.Value + " " + Convert.ToDouble(ds.Tables[0].Rows[0]["Penalty"].ToString()).ToString("#,##.##", CultureInfo.CreateSpecificCulture(ds.Tables[0].Rows[0]["Currency_CultureInfo"].ToString()));
                if (ds.Tables[0].Rows[0]["Premium_PaidDate"].ToString() != null && ds.Tables[0].Rows[0]["Premium_PaidDate"].ToString() != "")
                {
                    dtPremiumPaidDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["Premium_PaidDate"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                txtrenarks.Text = ds.Tables[0].Rows[0]["Remarks"].ToString();
            }
                
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                Guid PremiumUID = Guid.NewGuid();
                string Function = "Add Insurance Premium";
                int ActionType = 1;
                if (Request.QueryString["PremiumUID"] != null)
                {
                    PremiumUID = new Guid(Request.QueryString["PremiumUID"]);
                    Function = "Edit Insurance Premium";
                    ActionType = 2;
                }

                string DocPath = "";
                if (FileUpload1.HasFile)
                {
                    string InputFile = System.IO.Path.GetExtension(FileUpload1.FileName);
                    FileUpload1.SaveAs(Server.MapPath("~/Documents/" + FileUpload1.FileName));
                    DocPath = "~/Documents/" + FileUpload1.FileName;
                }

                string sDate1 = "", sDate2 = "",sDate3="";
                DateTime CDate1 = DateTime.Now, CDate2 = DateTime.Now, CDate3 = DateTime.Now;

                sDate1 = dtPremiumPaidDate.Text;
                //sDate1 = sDate1.Split('/')[1] + "/" + sDate1.Split('/')[0] + "/" + sDate1.Split('/')[2];
                sDate1 = getdata.ConvertDateFormat(sDate1);
                CDate1 = Convert.ToDateTime(sDate1);

                sDate2 = dtDueDate.Text;
                //sDate1 = sDate1.Split('/')[1] + "/" + sDate1.Split('/')[0] + "/" + sDate1.Split('/')[2];
                sDate2 = getdata.ConvertDateFormat(sDate2);
                CDate2 = Convert.ToDateTime(sDate2);


                sDate3 = HiddenNextDueDate.Value;
                //sDate1 = sDate1.Split('/')[1] + "/" + sDate1.Split('/')[0] + "/" + sDate1.Split('/')[2];
                sDate3 = getdata.ConvertDateFormat(sDate3);
                CDate3 = Convert.ToDateTime(sDate3);

                txtpremiumamount.Value = txtpremiumamount.Value.Replace(",", "").Replace("₹ ", "").Replace("$ ", "").Replace("¥ ", "").Replace("₹", "").Replace("$", "").Replace("¥", "").Trim();
                txtinterest.Value = txtpremiumamount.Value.Replace(",", "").Replace("₹ ", "").Replace("$ ", "").Replace("¥ ", "").Replace("₹", "").Replace("$", "").Replace("¥", "").Trim();
                txtpenalty.Value = txtpremiumamount.Value.Replace(",", "").Replace("₹ ", "").Replace("$ ", "").Replace("¥ ", "").Replace("₹", "").Replace("$", "").Replace("¥", "").Trim();

                bool result = getdata.InsertorUpdateInsurancePremium(PremiumUID, new Guid(Request.QueryString["InsuranceUID"]), float.Parse(txtpremiumamount.Value), float.Parse(txtinterest.Value), float.Parse(txtpenalty.Value), CDate1, CDate2, CDate3, DocPath, txtrenarks.Text);
                if (result)
                {
                    if (WebConfigurationManager.AppSettings["Dbsync"] == "Yes")
                    {
                        string WebAPIURL = "";
                        DataSet copysite = getdata.GetDataCopySiteDetails_by_ProjectUID(new Guid(Request.QueryString["ProjectUID"]));
                        if (copysite.Tables[0].Rows.Count > 0)
                        {
                            WebAPIURL = copysite.Tables[0].Rows[0]["DataCopySiteURL"].ToString();
                            WebAPIURL = WebAPIURL + "Activity/AddInsurancePremium";
                            string postData = "PremiumUID=" + PremiumUID + "&InsuranceUID=" + Request.QueryString["InsuranceUID"] + "&Premium_Paid=" + txtpremiumamount.Value + "&Interest=" + txtinterest.Value + "&Penalty=" + txtpenalty.Value + "&Premium_PaidDate=" + CDate1 + "&Premium_DueDate=" + CDate2 + "&Next_PremiumDate=" + CDate3 + "&Premium_Receipt=" + DocPath + "&Remarks=" + txtrenarks.Text + "&RelativePath=/Documents/";
                            if (FileUpload1.HasFile)
                            {
                                using (var form = new MultipartFormDataContent())
                                {
                                    var Content = new ByteArrayContent(File.ReadAllBytes(Server.MapPath(DocPath)));
                                    Content.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
                                    form.Add(Content, "file", Path.GetFileName(FileUpload1.FileName));
                                    form.Add(new StringContent("/Documents/"), "RelativePath");
                                    form.Add(new StringContent(PremiumUID.ToString()), "PremiumUID");
                                    form.Add(new StringContent(Request.QueryString["InsuranceUID"]), "InsuranceUID");
                                    form.Add(new StringContent(txtpremiumamount.Value), "Premium_Paid");
                                    form.Add(new StringContent(txtinterest.Value), "Interest");
                                    form.Add(new StringContent(txtpenalty.Value), "Penalty");
                                    form.Add(new StringContent(CDate1.ToString()), "Premium_PaidDate");
                                    form.Add(new StringContent(CDate2.ToString()), "Premium_DueDate");
                                    form.Add(new StringContent(CDate3.ToString()), "Next_PremiumDate");
                                    form.Add(new StringContent(DocPath), "Premium_Receipt");
                                    form.Add(new StringContent(txtrenarks.Text), "Remarks");

                                    using (HttpClient client = new HttpClient())
                                    {
                                        var response = client.PostAsync(WebAPIURL, form);
                                        response.Wait();

                                        if (response.Result.IsSuccessStatusCode)
                                        {
                                            int rCnt = getdata.ServerFlagsUpdate(PremiumUID.ToString(), 1, "Insurance_Premiums", "Y", "PremiumUID");
                                        }
                                        else
                                        {
                                            getdata.WebAPIStatusInsert(Guid.NewGuid(), WebAPIURL, postData, response.Result.ToString(), "Failure", "Add Insurance Premium", "AddInsurancePremium", PremiumUID);
                                            //Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error: DBSync =" + ErrorMessage + "');</script>");
                                        }
                                    }
                                }
                            }
                            else
                            {
                                string sReturnStatus = getdata.webPostMethod(postData, WebAPIURL);
                                if (!sReturnStatus.StartsWith("Error:"))
                                {
                                    dynamic DynamicData = JsonConvert.DeserializeObject(sReturnStatus);
                                    string RetStatus = DynamicData.Status;
                                    if (!RetStatus.StartsWith("Error:"))
                                    {
                                        int rCnt = getdata.ServerFlagsUpdate(PremiumUID.ToString(), ActionType, "Insurance_Premiums", "Y", "PremiumUID");
                                        if (rCnt > 0)
                                        {
                                        }
                                    }
                                    else
                                    {
                                        string ErrorMessage = DynamicData.Message;
                                        getdata.WebAPIStatusInsert(Guid.NewGuid(), WebAPIURL, postData, ErrorMessage, "Failure", Function, "AddInsurancePremium", PremiumUID);
                                        //Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error: DBSync =" + ErrorMessage + "');</script>");
                                    }
                                }
                                else
                                {
                                    getdata.WebAPIStatusInsert(Guid.NewGuid(), WebAPIURL, postData, sReturnStatus, "Failure", Function, "AddInsurancePremium", PremiumUID);
                                }
                            }
                                
                        }
                    }
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
                }

            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code : AINSPREM-01 there is a problem with these feature. please contact system admin.');</script>");
            }

        }
    }
}