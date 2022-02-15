using Newtonsoft.Json;
using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Data;
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
    public partial class add_bankdocument : System.Web.UI.Page
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
                    if (Request.QueryString["Bank_GuaranteeUID"] != null)
                    {
                        BindBankDocuments();
                    }
                }
            }
        }

        private void BindBankDocuments()
        {
            DataSet ds = getdata.GetBankDocuments_by_BankGuarantee_UID(new Guid(Request.QueryString["Bank_GuaranteeUID"]));
            grdBankDocuments.DataSource = ds;
            grdBankDocuments.DataBind();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                Guid BankDoc_UID = Guid.NewGuid();
                if (Request.QueryString["BankDoc_UID"] != null)
                {
                    BankDoc_UID = new Guid(Request.QueryString["BankDoc_UID"]);
                }
                string DocPath = "";

                if (FileUpload1.HasFile)
                {
                    string InputFile = System.IO.Path.GetExtension(FileUpload1.FileName);
                    FileUpload1.SaveAs(Server.MapPath("~/Documents/" + FileUpload1.FileName));
                    DocPath = "~/Documents/" + FileUpload1.FileName;
                    string Extn = Path.GetExtension(InputFile);
                    bool ret = getdata.InsertorUpdateBankDocuments(BankDoc_UID, new Guid(Request.QueryString["Bank_GuaranteeUID"]), txtdocumentName.Text, Extn, DocPath);
                    if (ret)
                    {
                        bool DbSyc = false;
                        string WebAPIURL = "";
                        if (WebConfigurationManager.AppSettings["Dbsync"] == "Yes")
                        {
                            DataSet copysite = getdata.GetDataCopySiteDetails_by_ProjectUID(new Guid(Request.QueryString["ProjectUID"]));
                            if (copysite.Tables[0].Rows.Count > 0)
                            {
                                DbSyc = true;
                                WebAPIURL = copysite.Tables[0].Rows[0]["DataCopySiteURL"].ToString();
                                WebAPIURL = WebAPIURL + "Activity/AddBankGuaranteeDocuments";

                                using (var form = new MultipartFormDataContent())
                                {
                                    var Content = new ByteArrayContent(File.ReadAllBytes(Server.MapPath(DocPath)));
                                    Content.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
                                    form.Add(Content, "file", Path.GetFileName(FileUpload1.FileName));
                                    form.Add(new StringContent("/Documents/"), "RelativePath");
                                    form.Add(new StringContent(DocPath), "Document_File");
                                    form.Add(new StringContent(Extn), "Document_Type");
                                    form.Add(new StringContent(txtdocumentName.Text), "Document_Name");
                                    form.Add(new StringContent(Request.QueryString["Bank_GuaranteeUID"]), "Bank_GuaranteeUID");
                                    form.Add(new StringContent(BankDoc_UID.ToString()), "BankDoc_UID");

                                    string postData = "BankDoc_UID=" + BankDoc_UID + "&Bank_GuaranteeUID=" + Request.QueryString["Bank_GuaranteeUID"] + "&Document_Name=" + txtdocumentName.Text + "&Document_Type=" + Extn + "&Document_File=" + DocPath + "&RelativePath=/Documents/";


                                    using (HttpClient client = new HttpClient())
                                    {
                                        var response = client.PostAsync(WebAPIURL, form);
                                        response.Wait();

                                        if (response.Result.IsSuccessStatusCode)
                                        {
                                            int rCnt = getdata.ServerFlagsUpdate(BankDoc_UID.ToString(), 1, "BankDocuments", "Y", "BankDoc_UID");
                                        }
                                        else
                                        {
                                            getdata.WebAPIStatusInsert(Guid.NewGuid(), WebAPIURL, postData, response.Result.ToString(), "Failure", "Add Bank Documents", "AddBankGuaranteeDocuments", BankDoc_UID);
                                            //Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error: DBSync =" + ErrorMessage + "');</script>");
                                        }
                                    }
                                }
                            }
                        }
                                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
                    }
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Please choose a document.');</script>");
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code : ABD-01 there is a problem with this feature. please contact system admin.');</script>");
            }
        }
        protected void grdBankDocuments_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string UID = e.CommandArgument.ToString();
            if (e.CommandName == "download")
            {
                DataSet ds = getdata.GetBankDocuments_by_BankDoc_UID(new Guid(UID));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string path = Server.MapPath(ds.Tables[0].Rows[0]["Document_File"].ToString());

                    System.IO.FileInfo file = new System.IO.FileInfo(path);

                    if (file.Exists)
                    {

                        Response.Clear();

                        Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);

                        Response.AddHeader("Content-Length", file.Length.ToString());

                        Response.ContentType = "application/octet-stream";

                        Response.WriteFile(file.FullName);

                        Response.End();

                    }

                    else
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('File not found.');</script>");
                    }
                }
            }

            if (e.CommandName == "delete")
            {
                int cnt = getdata.BankDocuments_Delete(new Guid(UID), new Guid(Session["UserUID"].ToString()));
                if (cnt > 0)
                {
                    if (WebConfigurationManager.AppSettings["Dbsync"] == "Yes")
                    {
                        string WebAPIURL = "";
                        DataSet copysite = getdata.GetDataCopySiteDetails_by_ProjectUID(new Guid(Request.QueryString["ProjectUID"]));
                        if (copysite.Tables[0].Rows.Count > 0)
                        {
                            WebAPIURL = copysite.Tables[0].Rows[0]["DataCopySiteURL"].ToString();
                            WebAPIURL = WebAPIURL + "Activity/BankGuaranteeDocumentsDelete";
                            string postData = "BankDoc_UID=" + UID + "&UserUID=" + Session["UserUID"].ToString();
                            string sReturnStatus = getdata.webPostMethod(postData, WebAPIURL);
                            if (!sReturnStatus.StartsWith("Error:"))
                            {
                                dynamic DynamicData = JsonConvert.DeserializeObject(sReturnStatus);
                                string RetStatus = DynamicData.Status;
                                if (!RetStatus.StartsWith("Error:"))
                                {
                                    int rCnt = getdata.ServerFlagsUpdate(UID.ToString(), 2, "BankDocuments", "Y", "BankDoc_UID");
                                    if (rCnt > 0)
                                    {
                                    }
                                }
                                else
                                {
                                    string ErrorMessage = DynamicData.Message;
                                    getdata.WebAPIStatusInsert(Guid.NewGuid(), WebAPIURL, postData, ErrorMessage, "Failure", "Bank Guarantee Document Delete", "BankGuaranteeDocumentsDelete", new Guid(UID));
                                    //Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error: DBSync =" + ErrorMessage + "');</script>");
                                }
                            }
                            else
                            {
                                getdata.WebAPIStatusInsert(Guid.NewGuid(), WebAPIURL, postData, sReturnStatus, "Failure", "Bank Guarantee Document Delete", "BankGuaranteeDocumentsDelete", new Guid(UID));
                            }
                        }
                    }

                    BindBankDocuments();
                }
            }
        }

        protected void grdBankDocuments_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }
    }
}