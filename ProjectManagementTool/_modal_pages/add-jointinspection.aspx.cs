using Newtonsoft.Json;
using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagementTool._modal_pages
{
    public partial class add_jointinspection : System.Web.UI.Page
    {
        DBGetData getdata = new DBGetData();
        TaskUpdate TKUpdate = new TaskUpdate();
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
                    if (Request.QueryString["ProjectUID"] != null)
                    {
                        BindBOQByProjectUID();
                    }
                    if (Request.QueryString["type"] == "edit")
                    {
                        BindJointInspection(Request.QueryString["inspectionUid"]);
                    }
                    //if (Session["BOQData"] != null)
                    //{
                    //    lblActivityName.Visible = true;
                    //    LinkBOQData.Visible = false;
                    //    lblActivityName.Text = getdata.GetBOQDesc_by_BOQDetailsUID(new Guid(Session["BOQData"].ToString()));
                    //}
                    //else
                    //{
                    //    lblActivityName.Visible = false;
                    //    LinkBOQData.Visible = true;
                    //}
                }
            }
        }

        private void BindBOQByProjectUID()
        {
            //LinkBOQData.HRef = "/_modal_pages/boq-treeview.aspx?ProjectUID=" + Request.QueryString["ProjectUID"];
            //DataSet ds = getdata.GetBOQDetails_by_projectuid(new Guid(Request.QueryString["ProjectUID"]));
            //DDLBOQDesc.DataTextField = "Description";
            //DDLBOQDesc.DataValueField = "BOQDetailsUID";
            //DDLBOQDesc.DataSource = ds;
            //DDLBOQDesc.DataBind();
        }
        private void BindJointInspection(string inspectionUid)
        {
            DataTable dt = getdata.getJointInspection_by_inspectionUid(inspectionUid);
            if (dt.Rows.Count > 0)
            {
                //DDLBOQDesc.SelectedValue = dt.Rows[0]["BOQUid"].ToString();
                //lblActivityName.Text = getdata.GetBOQDesc_by_BOQDetailsUID(new Guid(dt.Rows[0]["BOQUid"].ToString()));
                //lblActivityName.Visible = true;
                //LinkBOQData.Visible = false;
                txtdiaofpipe.Text= dt.Rows[0]["DiaPipe"].ToString();
                txtinvoicenumber.Text = dt.Rows[0]["invoice_number"].ToString();
                txtquantity.Text = dt.Rows[0]["quantity"].ToString();
                txtunit.Text = dt.Rows[0]["unit"].ToString();
                
                if (dt.Rows[0]["Inspection_Type"].ToString() == "Laying & Jointing")
                {
                    DDLInspectionType.SelectedValue = "Laying & Jointing";
                    ChainageNum.Visible = true;
                    ChainageDesc.Visible = true;
                    ChainageStartingPoint.Visible = true;
                    ChainageLength.Visible = true;
                    SupplierNumber.Visible = false;
                    SupplierDate.Visible = false;
                    PipeNumber.Visible = false;
                    txtchainagenumber.Text = dt.Rows[0]["Chainage_Number"].ToString();
                    txtchainagedesc.Text= dt.Rows[0]["Chainage_Desc"].ToString();
                    txtstartingpoint.Text= dt.Rows[0]["Chainage_StartPoint"].ToString();
                    txtlength.Text= dt.Rows[0]["Chainage_Length"].ToString();
                }
                else if (dt.Rows[0]["Inspection_Type"].ToString() == "Guniting" || dt.Rows[0]["Inspection_Type"].ToString() == "Epoxy")
                {
                    DDLInspectionType.SelectedValue = dt.Rows[0]["Inspection_Type"].ToString();
                    ChainageNum.Visible = false;
                    ChainageDesc.Visible = false;
                    ChainageStartingPoint.Visible = false;
                    ChainageLength.Visible = false;
                    SupplierNumber.Visible = false;
                    SupplierDate.Visible = true;
                    LblDate.InnerText = "Inspection Date";
                    Guniting_QtyinRMT.Visible = true;
                    txtQtyinRMT.Text = dt.Rows[0]["Qty_in_RMT"].ToString();
                    PipeNumber.Visible = true;
                    txtpipenumber.Text = dt.Rows[0]["PipeNumber"].ToString();
                }
                else
                {
                    DDLInspectionType.SelectedValue = "Supply";
                    ChainageNum.Visible = false;
                    ChainageDesc.Visible = false;
                    ChainageStartingPoint.Visible = false;
                    ChainageLength.Visible = false;
                    SupplierNumber.Visible = true;
                    SupplierDate.Visible = true;
                    PipeNumber.Visible = false;
                }
                if (dt.Rows[0]["invoicedate"].ToString() != null && dt.Rows[0]["invoicedate"].ToString() != "")
                {
                    dtInvoiceDate.Text = Convert.ToDateTime(dt.Rows[0]["invoicedate"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                txtRemarks.Text = dt.Rows[0]["Remarks"].ToString();
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                string sDate1 = "";
                DateTime CDate1 = DateTime.Now;
                sDate1 = dtInvoiceDate.Text;
                //sDate1 = sDate1.Split('/')[1] + "/" + sDate1.Split('/')[0] + "/" + sDate1.Split('/')[2];
                sDate1 = getdata.ConvertDateFormat(sDate1);
                CDate1 = Convert.ToDateTime(sDate1);
                Guid inspectionUid;
                if (Request.QueryString["inspectionUid"] != null)
                {
                    inspectionUid = new Guid(Request.QueryString["inspectionUid"]);
                }
                else
                {
                    inspectionUid = Guid.NewGuid();
                }
                float StartingPoint = 0;
                float Length = 0;
                float Qty_in_RMT = 0;
                double Qty_for_Unit = 0;
                double Deductions = 0;
                if (DDLInspectionType.SelectedValue == "Laying & Jointing")
                {
                    StartingPoint = float.Parse(txtstartingpoint.Text);
                    Length = float.Parse(txtlength.Text);
                }
                else if (DDLInspectionType.SelectedValue == "Guniting" || DDLInspectionType.SelectedValue == "Epoxy")
                {
                    string DiaofPipe_in_mm = getdata.GetBOQDiaofPipe_by_BOQDetailsUID(new Guid(Request.QueryString["BOQUID"]));
                    if (DiaofPipe_in_mm == "Error1:" || DiaofPipe_in_mm =="")
                    {
                        DiaofPipe_in_mm = "0";
                    }

                    Qty_in_RMT = float.Parse(txtQtyinRMT.Text);
                    Qty_for_Unit = Math.Round(Convert.ToDouble(txtQtyinRMT.Text) * Convert.ToDouble(3.14159) * Convert.ToDouble(DiaofPipe_in_mm), 2);
                    Deductions = float.Parse(txtquantity.Text) * 0.3;
                    
                }
                
                if (Request.QueryString["type"] == "edit")
                {
                    int cnt = getdata.InsertjointInspection(inspectionUid, new Guid(Request.QueryString["BOQUID"]), txtdiaofpipe.Text, txtunit.Text, txtinvoicenumber.Text, CDate1.ToString(), txtquantity.Text,DDLInspectionType.SelectedValue,txtchainagenumber.Text,txtchainagedesc.Text, StartingPoint,Length, Qty_in_RMT, Qty_for_Unit, Deductions, txtRemarks.Text, new Guid(Request.QueryString["ProjectUID"]),txtpipenumber.Text);
                    if (cnt > 0)
                    {
                        if (WebConfigurationManager.AppSettings["Dbsync"] == "Yes")
                        {
                            DataSet copysite = getdata.GetDataCopySiteDetails_by_ProjectUID(new Guid(Request.QueryString["ProjectUID"]));
                            if (copysite.Tables[0].Rows.Count > 0)
                            {
                                //string WebAPIURL = WebConfigurationManager.AppSettings["DbsyncWebApiURL"];
                                string WebAPIURL = copysite.Tables[0].Rows[0]["DataCopySiteURL"].ToString();
                                WebAPIURL = WebAPIURL + "Activity/EditJointInspectionReport";

                                string postData = "inspectionUid=" + inspectionUid + "&ProjectName=" + getdata.GetProjectName_by_BOQUID(new Guid(Request.QueryString["BOQUID"])) + "&BOQUID=" + Request.QueryString["BOQUID"] + "&DiaPipe=" + txtdiaofpipe.Text + "&PipeNumber=" + txtpipenumber.Text + "&invoice_number=" + txtinvoicenumber.Text + "&invoicedate=" + CDate1.ToString() + "&unit=" + txtunit.Text + "&quantity=" + txtquantity.Text + "&InspectionType=" + DDLInspectionType.SelectedValue + "&StartingPoint=" + StartingPoint + "&Lenght=" + Length + "&Chainage_Number=" + txtchainagenumber.Text + "&Chainage_Desc=" + txtchainagedesc.Text + "&Qty_in_RMT=" + Qty_in_RMT + "&Qty_for_Unit=" + Qty_for_Unit + "&Deductions=" + Deductions + "&Remarks=" + txtRemarks.Text;
                                string sReturnStatus = webPostMethod(postData, WebAPIURL);
                                if (!sReturnStatus.StartsWith("Error:"))
                                {
                                    dynamic DynamicData = JsonConvert.DeserializeObject(sReturnStatus);
                                    string RetStatus = DynamicData.Status;
                                    if (!RetStatus.StartsWith("Error:"))
                                    {
                                        int rCnt = getdata.ServerFlagsUpdate(inspectionUid.ToString(), 2, "JointInspection", "Y", "inspectionUid");
                                    }
                                    else
                                    {
                                        string ErrorMessage = DynamicData.Message;
                                        WebAPIStatusInsert(Guid.NewGuid(), WebAPIURL, postData, ErrorMessage, "Failure", "Edit Joint Inspection", "EditJointInspectionReport", inspectionUid);
                                        //Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error: DBSync =" + ErrorMessage + "');</script>");
                                    }
                                }
                                else
                                {
                                    WebAPIStatusInsert(Guid.NewGuid(), WebAPIURL, postData, sReturnStatus, "Failure", "Edit Joint Inspection", "EditJointInspectionReport", inspectionUid);
                                }
                            }
                        }
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
                    }
                }
                else
                {
                    int cnt = getdata.InsertjointInspection(inspectionUid, new Guid(Request.QueryString["BOQUID"]), txtdiaofpipe.Text, txtunit.Text, txtinvoicenumber.Text, CDate1.ToString(), txtquantity.Text, DDLInspectionType.SelectedValue, txtchainagenumber.Text, txtchainagedesc.Text, StartingPoint, Length, Qty_in_RMT, Qty_for_Unit, Deductions, txtRemarks.Text, new Guid(Request.QueryString["ProjectUID"]), txtpipenumber.Text);
                    if (cnt > 0)
                    {
                        //Session["BOQData"] = null;
                        bool DbSyc = false;
                        string WebAPIURL = "";
                        string WebAPIURLDocuments = "";
                        if (WebConfigurationManager.AppSettings["Dbsync"] == "Yes")
                        {
                            DataSet copysite = getdata.GetDataCopySiteDetails_by_ProjectUID(new Guid(Request.QueryString["ProjectUID"]));
                            if (copysite.Tables[0].Rows.Count > 0)
                            {
                                DbSyc = true;
                                //string WebAPIURL = WebConfigurationManager.AppSettings["DbsyncWebApiURL"];
                                WebAPIURL = copysite.Tables[0].Rows[0]["DataCopySiteURL"].ToString();
                                WebAPIURLDocuments = copysite.Tables[0].Rows[0]["DataCopySiteURL"].ToString();
                                WebAPIURL = WebAPIURL + "Activity/AddJointInspectionReport";

                                string postData = "inspectionUid=" + inspectionUid + "&ProjectName=" + getdata.GetProjectName_by_BOQUID(new Guid(Request.QueryString["BOQUID"])) + "&BOQUID=" + Request.QueryString["BOQUID"] + "&DiaPipe=" + txtdiaofpipe.Text + "&PipeNumber=" + txtpipenumber.Text + "&invoice_number=" + txtinvoicenumber.Text + "&invoicedate=" + CDate1.ToString() + "&unit=" + txtunit.Text + "&quantity=" + txtquantity.Text + "&InspectionType=" + DDLInspectionType.SelectedValue + "&StartingPoint=" + StartingPoint + "&Lenght=" + Length + "&Chainage_Number=" + txtchainagenumber.Text + "&Chainage_Desc=" + txtchainagedesc.Text + "&Qty_in_RMT=" + Qty_in_RMT + "&Qty_for_Unit=" + Qty_for_Unit + "&Deductions=" + Deductions + "&Remarks=" + txtRemarks.Text;
                                string sReturnStatus = webPostMethod(postData, WebAPIURL);
                                if (!sReturnStatus.StartsWith("Error:"))
                                {
                                    dynamic DynamicData = JsonConvert.DeserializeObject(sReturnStatus);
                                    string RetStatus = DynamicData.Status;
                                    if (!RetStatus.StartsWith("Error:"))
                                    {
                                        int rCnt = getdata.ServerFlagsUpdate(inspectionUid.ToString(), 1, "JointInspection", "Y", "inspectionUid");
                                        if (rCnt > 0)
                                        {
                                            rCnt = getdata.ServerFlagsUpdate(inspectionUid.ToString(), 2, "JointInspection", "Y", "inspectionUid");
                                        }
                                    }
                                    else
                                    {
                                        string ErrorMessage = DynamicData.Message;
                                        WebAPIStatusInsert(Guid.NewGuid(), WebAPIURL, postData, ErrorMessage, "Failure", "Add Joint Inspection", "AddJointInspectionReport", inspectionUid);
                                        //Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error: DBSync =" + ErrorMessage + "');</script>");
                                    }
                                }
                                else
                                {
                                    WebAPIStatusInsert(Guid.NewGuid(), WebAPIURL, postData, sReturnStatus, "Failure", "Add Joint Inspection", "AddJointInspectionReport", inspectionUid);
                                }
                            }
                                
                        }

                        string DocumentFor = "";
                        string sDocumentPath = "";
                        string savedPath = string.Empty;
                        string CoverPagePath = "";
                        string Flow1DisplayName = "";
                        string CoverLetterUID = "";
                        string cStatus = "Submitted";

                        string sDocumentUID = "";
                        string FlowID = "";
                        string FlowUserUID = "";
                        string tUID = getdata.GetTaskUID_By_WorkPackageID_TName(new Guid(Request.QueryString["WorkpackageUID"]), "Construction Programme");
                        if (tUID != "")
                        {
                            DataSet dsSubmittal = getdata.GetSubmittalData_by_TaskUID_DocName(new Guid(tUID), "Inspection Report");
                            if (dsSubmittal.Tables[0].Rows.Count > 0)
                            {
                                sDocumentUID = dsSubmittal.Tables[0].Rows[0]["DocumentUID"].ToString();
                                FlowID = dsSubmittal.Tables[0].Rows[0]["FlowUID"].ToString();
                                FlowUserUID = dsSubmittal.Tables[0].Rows[0]["FlowStep1_UserUID"].ToString();
                                
                                foreach (HttpPostedFile uploadedFile in InspectionDocs.PostedFiles)
                                {
                                    Guid InspectionDocumentUID = Guid.NewGuid();
                                    string Extn = System.IO.Path.GetExtension(uploadedFile.FileName);

                                    if (Extn.ToLower() != ".exe" && Extn.ToLower() != ".msi" && Extn.ToLower() != ".db" && Extn!="")
                                    {
                                        DocumentFor = "General Document";
                                        sDocumentPath = "~/" + Request.QueryString["ProjectUID"] + "/Documents";

                                        if (!Directory.Exists(Server.MapPath(sDocumentPath)))
                                        {
                                            Directory.CreateDirectory(Server.MapPath(sDocumentPath));
                                        }
                                        string sFileName = Path.GetFileNameWithoutExtension(uploadedFile.FileName);
                                        savedPath = sDocumentPath + "/" + DateTime.Now.Ticks.ToString() + Path.GetFileName(uploadedFile.FileName);
                                        uploadedFile.SaveAs(Server.MapPath(savedPath));
                                        CoverPagePath = sDocumentPath + "/" + sFileName + "_1" + Extn;
                                        getdata.EncryptFile(Server.MapPath(savedPath), Server.MapPath(CoverPagePath));

                                        Guid ActualDocumentUID = Guid.NewGuid();

                                        string UploadFilePhysicalpath = Server.MapPath(CoverPagePath);
                                        DataSet dsFlow = getdata.GetDocumentFlows_by_UID(new Guid(FlowID));
                                        if (dsFlow.Tables[0].Rows.Count > 0)
                                        {
                                            Flow1DisplayName = dsFlow.Tables[0].Rows[0]["FlowStep1_DisplayName"].ToString();

                                            sDate1 = DateTime.Now.ToString("dd/MM/yyyy");
                                            //sDate1 = sDate1.Split('/')[1] + "/" + sDate1.Split('/')[0] + "/" + sDate1.Split('/')[2];
                                            sDate1 = getdata.ConvertDateFormat(sDate1);
                                            CDate1 = Convert.ToDateTime(sDate1);

                                            int Count = getdata.Document_Insert_or_Update_with_RelativePath_Flow1(ActualDocumentUID, new Guid(Request.QueryString["ProjectUID"]), new Guid(Request.QueryString["WorkpackageUID"]), new Guid(sDocumentUID), "-", "",
                                                        DocumentFor, DateTime.Now, new Guid(FlowID), sFileName, "", 1, Extn,
                                                        "false", Extn.ToUpper() == ".PDF" ? "true" : "false", (Extn.ToUpper() == ".DOC" || Extn.ToUpper() == ".DOCX") ? "true" : "false", "false",
                                                        "false", "false", savedPath, "", "", cStatus,
                                                        new Guid(FlowUserUID), CDate1, Flow1DisplayName, "Contractor", DateTime.Now, "", "", UploadFilePhysicalpath, CoverLetterUID, "Submission");
                                            if (Count > 0)
                                            {
                                                Count = getdata.InsertJointInspectionDocuments(InspectionDocumentUID, inspectionUid, sFileName, Extn, UploadFilePhysicalpath);
                                                if (Count > 0)
                                                {
                                                    if (DbSyc)
                                                    {
                                                        WebAPIURLDocuments = WebAPIURLDocuments + "Activity/AddJointInspectionDocuments";

                                                        using (var form = new MultipartFormDataContent())
                                                        {
                                                            var Content = new ByteArrayContent(File.ReadAllBytes(UploadFilePhysicalpath));
                                                            Content.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
                                                            form.Add(Content, "file", Path.GetFileName(UploadFilePhysicalpath));
                                                            form.Add(new StringContent(Request.QueryString["ProjectUID"] + "/Documents/"), "RelativePath");
                                                            form.Add(new StringContent(UploadFilePhysicalpath), "UploadFilePhysicalpath");
                                                            form.Add(new StringContent(Extn), "Extension");
                                                            form.Add(new StringContent(sFileName), "FileName");
                                                            form.Add(new StringContent(inspectionUid.ToString()), "inspectionUid");
                                                            form.Add(new StringContent(InspectionDocumentUID.ToString()), "InspectionDocumentUID");
                                                            form.Add(new StringContent(Request.QueryString["ProjectUID"]), "ProjectUID");

                                                            string postData = "RelativePath=" + sDocumentPath + "/" + sFileName + "_1" + Extn + "&UploadFilePhysicalpath=" + UploadFilePhysicalpath + "&Extension=" + Extn + "&FileName=" + sFileName + "&inspectionUid=" + inspectionUid + "&InspectionDocumentUID=" + InspectionDocumentUID + "&ProjectUID=" + Request.QueryString["ProjectUID"];


                                                            using (HttpClient client = new HttpClient())
                                                            {
                                                                var response = client.PostAsync(WebAPIURLDocuments, form);
                                                                response.Wait();

                                                                if (response.Result.IsSuccessStatusCode)
                                                                {
                                                                    int rCnt = getdata.ServerFlagsUpdate(InspectionDocumentUID.ToString(), 1, "JointInspectionDocuments", "Y", "InspectionDocumentUID");
                                                                }
                                                                else
                                                                {
                                                                    WebAPIStatusInsert(Guid.NewGuid(), WebAPIURL, postData, response.Result.ToString(), "Failure", "Add Joint Inspection Documents", "AddJointInspectionDocuments", InspectionDocumentUID);
                                                                    //Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error: DBSync =" + ErrorMessage + "');</script>");
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    if (File.Exists(Server.MapPath(savedPath)))
                                    {
                                        File.Delete(Server.MapPath(savedPath));
                                    }
                                }
                            }

                        }

                    }
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");

                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");

               // Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code : AJIR-01. Description :" + ex.Message + "');</script>");
            }
        }

        public string webPostMethod(string postData, string URL)
        {
            try
            {
                string responseFromServer = "";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
                request.Method = "POST";
                request.Credentials = CredentialCache.DefaultCredentials;
                ((HttpWebRequest)request).UserAgent =
                                  "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 7.1; Trident/5.0)";
                request.Accept = "/";
                request.UseDefaultCredentials = true;
                request.Proxy.Credentials = System.Net.CredentialCache.DefaultCredentials;
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();

                WebResponse response = request.GetResponse();
                dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                responseFromServer = reader.ReadToEnd();
                reader.Close();
                dataStream.Close();
                response.Close();
                return responseFromServer;
            }
            catch (Exception ex)
            {
                return "Error:" + ex.Message;
            }
        }

        public string WebAPIStatusInsert(Guid WebAPIUID, string url, string WebAPIParameters, string WebAPI_Error, string WebAPIStatus,string WebAPIType,string WebAPIFunction,Guid WebAPI_PrimaryKey)
        {
            string Retval = "";

            int cnt = getdata.WebAPIStatusInsert(WebAPIUID, url, WebAPIParameters, WebAPI_Error, WebAPIStatus, WebAPIType, WebAPIFunction, WebAPI_PrimaryKey);
            if (cnt <= 0)
            {
                Retval = "Insertion Failed for WebAPIStaus table";
            }
            return Retval;
        }

        protected void DDLInspectionType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDLInspectionType.SelectedValue == "Laying & Jointing")
            {
                ChainageNum.Visible = true;
                ChainageDesc.Visible = true;
                ChainageStartingPoint.Visible = true;
                ChainageLength.Visible = true;
                SupplierNumber.Visible = false;
                SupplierDate.Visible = false;
                Guniting_QtyinRMT.Visible = false;
                PipeNumber.Visible = false;
            }
            else if (DDLInspectionType.SelectedValue == "Guniting" || DDLInspectionType.SelectedValue== "Epoxy")
            {
                ChainageNum.Visible = false;
                ChainageDesc.Visible = false;
                ChainageStartingPoint.Visible = false;
                ChainageLength.Visible = false;
                SupplierNumber.Visible = false;
                SupplierDate.Visible = true;
                LblDate.InnerText = "Inspection Date";
                Guniting_QtyinRMT.Visible = true;
                PipeNumber.Visible = true;
                
            }
            else
            {
                Guniting_QtyinRMT.Visible = false;
                ChainageNum.Visible = false;
                ChainageDesc.Visible = false;
                ChainageStartingPoint.Visible = false;
                ChainageLength.Visible = false;
                SupplierNumber.Visible = true;
                SupplierDate.Visible = true;
                PipeNumber.Visible = false;
            }
        }
    }
}