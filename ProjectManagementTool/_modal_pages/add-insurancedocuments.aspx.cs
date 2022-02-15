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
    public partial class add_insurancedocuments : System.Web.UI.Page
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
                    if (Request.QueryString["InsuranceUID"] != null)
                    {
                        BindInsurenceDocuments();
                    }
                }
            }
        }
        private void BindInsurenceDocuments()
        {
            DataSet ds = getdata.GetInsurenceDocuments_by_BankInsuranceUID(new Guid(Request.QueryString["InsuranceUID"]));
            grdInsuranceDocuments.DataSource = ds;
            grdInsuranceDocuments.DataBind();
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                Guid InsuranceDoc_UID = Guid.NewGuid();
                if (Request.QueryString["InsuranceDoc_UID"] != null)
                {
                    InsuranceDoc_UID = new Guid(Request.QueryString["InsuranceDoc_UID"]);
                }
                string DocPath = "";
                if (FileUpload1.HasFile)
                {
                    string InputFile = System.IO.Path.GetExtension(FileUpload1.FileName);
                    FileUpload1.SaveAs(Server.MapPath("~/Documents/" + FileUpload1.FileName));
                    DocPath = "~/Documents/" + FileUpload1.FileName;

                    string Extn = Path.GetExtension(InputFile);

                    bool ret = getdata.InsertorUpdateInsuranceDocuments(InsuranceDoc_UID, new Guid(Request.QueryString["InsuranceUID"]), txtdocumentName.Text, Extn, DocPath);
                    if (ret)
                    {
                        //Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
                        bool DbSyc = false;
                        string WebAPIURL = "";
                        string WebAPIURLDocuments = "";
                        if (WebConfigurationManager.AppSettings["Dbsync"] == "Yes")
                        {
                            DataSet copysite = getdata.GetDataCopySiteDetails_by_ProjectUID(new Guid(Request.QueryString["ProjectUID"]));
                            if (copysite.Tables[0].Rows.Count > 0)
                            {
                                DbSyc = true;
                                WebAPIURL = copysite.Tables[0].Rows[0]["DataCopySiteURL"].ToString();
                                WebAPIURL = WebAPIURL + "Activity/AddInsuranceDocuments";

                                using (var form = new MultipartFormDataContent())
                                {
                                    var Content = new ByteArrayContent(File.ReadAllBytes(Server.MapPath(DocPath)));
                                    Content.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
                                    form.Add(Content, "file", Path.GetFileName(FileUpload1.FileName));
                                    form.Add(new StringContent("/Documents/"), "RelativePath");
                                    form.Add(new StringContent(DocPath), "InsuranceDoc_FilePath");
                                    form.Add(new StringContent(Extn), "InsuranceDoc_Type");
                                    form.Add(new StringContent(txtdocumentName.Text), "InsuranceDoc_Name");
                                    form.Add(new StringContent(Request.QueryString["InsuranceUID"]), "InsuranceUID");
                                    form.Add(new StringContent(InsuranceDoc_UID.ToString()), "InsuranceDoc_UID");

                                    string postData = "InsuranceDoc_UID=" + InsuranceDoc_UID + "&InsuranceUID=" + Request.QueryString["InsuranceUID"] + "&InsuranceDoc_Name=" + txtdocumentName.Text + "&InsuranceDoc_Type=" + Extn + "&InsuranceDoc_FilePath=" + DocPath + "&RelativePath=/Documents/";


                                    using (HttpClient client = new HttpClient())
                                    {
                                        var response = client.PostAsync(WebAPIURLDocuments, form);
                                        response.Wait();

                                        if (response.Result.IsSuccessStatusCode)
                                        {
                                            int rCnt = getdata.ServerFlagsUpdate(InsuranceDoc_UID.ToString(), 1, "InsuranceDocuments", "Y", "InsuranceDoc_UID");
                                        }
                                        else
                                        {
                                            getdata.WebAPIStatusInsert(Guid.NewGuid(), WebAPIURL, postData, response.Result.ToString(), "Failure", "Add Insurance Documents", "AddInsuranceDocuments", InsuranceDoc_UID);
                                            //Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error: DBSync =" + ErrorMessage + "');</script>");
                                        }
                                    }
                                }
                            }
                        }
                        BindInsurenceDocuments();
                    }
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code : AINSDOCS-01 there is a problem with these feature. please contact system admin.');</script>");
            }
            
        }

        protected void grdInsuranceDocuments_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string UID = e.CommandArgument.ToString();
            if (e.CommandName == "download")
            {
                DataSet ds = getdata.GetInsuranceDocuments_by_InsuranceDoc_UID(new Guid(UID));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string path = Server.MapPath(ds.Tables[0].Rows[0]["InsuranceDoc_FilePath"].ToString());

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
                int cnt = getdata.InsuranceDocuments_Delete(new Guid(UID), new Guid(Session["UserUID"].ToString()));
                if (cnt > 0)
                {
                    if (WebConfigurationManager.AppSettings["Dbsync"] == "Yes")
                    {
                        string WebAPIURL = "";
                        DataSet copysite = getdata.GetDataCopySiteDetails_by_ProjectUID(new Guid(Request.QueryString["ProjectUID"]));
                        if (copysite.Tables[0].Rows.Count > 0)
                        {
                            WebAPIURL = copysite.Tables[0].Rows[0]["DataCopySiteURL"].ToString();
                            WebAPIURL = WebAPIURL + "Activity/InsuranceDocumentsDelete";
                            string postData = "InsuranceDoc_UID=" + UID + "&UserUID=" + Session["UserUID"].ToString();
                            string sReturnStatus = getdata.webPostMethod(postData, WebAPIURL);
                            if (!sReturnStatus.StartsWith("Error:"))
                            {
                                dynamic DynamicData = JsonConvert.DeserializeObject(sReturnStatus);
                                string RetStatus = DynamicData.Status;
                                if (!RetStatus.StartsWith("Error:"))
                                {
                                    int rCnt = getdata.ServerFlagsUpdate(UID.ToString(), 2, "InsuranceDocuments", "Y", "InsuranceDoc_UID");
                                    if (rCnt > 0)
                                    {
                                    }
                                }
                                else
                                {
                                    string ErrorMessage = DynamicData.Message;
                                    getdata.WebAPIStatusInsert(Guid.NewGuid(), WebAPIURL, postData, ErrorMessage, "Failure", "Insurance Document Delete", "InsuranceDocumentsDelete", new Guid(UID));
                                    //Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error: DBSync =" + ErrorMessage + "');</script>");
                                }
                            }
                            else
                            {
                                getdata.WebAPIStatusInsert(Guid.NewGuid(), WebAPIURL, postData, sReturnStatus, "Failure", "Insurance Document Delete", "InsuranceDocumentsDelete", new Guid(UID));
                            }
                        }
                    }

                    BindInsurenceDocuments();
                }
            }
        }

        protected void grdInsuranceDocuments_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }
    }
}