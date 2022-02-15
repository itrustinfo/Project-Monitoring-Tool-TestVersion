 using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagementTool._modal_pages
{
    public partial class resubmit_documents : System.Web.UI.Page
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
                    UploadFolder.Visible = false;
                    UploadFile.Visible = true;
                    CoverLetter.Visible = true;
                }
            }
                
        }

        protected void btnSubmit_Click(object sender, EventArgs e) // submit
        {
            if (RBLUploadType.SelectedValue == "Folder" && Request.Files.Count == 0)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "Warning", "<script language='javascript'>alert('Please choose folder to upload documents...');</script>");
            }
            else if (RBLUploadType.SelectedValue == "File" && !FileUploadDoc.HasFile)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "Warning", "<script language='javascript'>alert('Please choose a document.');</script>");
            }
            else
            {
                try
                {
                    string DocPath = string.Empty;
                    string Subject = Session["Username"].ToString() + " Uploaded a new Document";
                    string sHtmlString = string.Empty;
                    if (RBLUploadType.SelectedValue == "File")
                    {
                        Guid DocVersion_UID = Guid.NewGuid();
                        string ConverLetterFilepath = "";
                        int DocVersion = getdata.getDocumentStatusVersion(new Guid(Request.QueryString["StatusUID"]), new Guid(Request.QueryString["DocumentUID"]));
                        if (FileCoverLetterUpload.HasFile)
                        {
                            string FileDatetime = DateTime.Now.ToString("dd MMM yyyy hh-mm-ss tt");
                            string sDocumentPath = "~/" + Request.QueryString["ProjectUID"] + "/" + Request.QueryString["DocumentUID"] + "/" + FileDatetime + "/Document";
                            if (!Directory.Exists(Server.MapPath(sDocumentPath)))
                            {
                                Directory.CreateDirectory(Server.MapPath(sDocumentPath));
                            }

                            string sFileName = Path.GetFileNameWithoutExtension(FileCoverLetterUpload.FileName);
                            string Extn = System.IO.Path.GetExtension(FileCoverLetterUpload.FileName);

                            FileCoverLetterUpload.SaveAs(Server.MapPath(sDocumentPath + "/" + sFileName + "_1_copy" + Extn));
                            string savedPath = sDocumentPath + "/" + sFileName + "_1_copy" + Extn;
                            DocPath = sDocumentPath + "/" + sFileName + "_1" + Extn;

                            getdata.EncryptFile(Server.MapPath(savedPath), Server.MapPath(DocPath));
                            ConverLetterFilepath = sDocumentPath + "/" + sFileName + "_1" + Extn;
                        }
                        if (FileUploadDoc.HasFile)
                        {
                            string FileDatetime = DateTime.Now.ToString("dd MMM yyyy hh-mm-ss tt");
                            string sDocumentPath = "~/" + Request.QueryString["ProjectUID"] + "/" + Request.QueryString["DocumentUID"] + "/" + FileDatetime + "/Document";
                            if (!Directory.Exists(Server.MapPath(sDocumentPath)))
                            {
                                Directory.CreateDirectory(Server.MapPath(sDocumentPath));
                            }

                            string sFileName = Path.GetFileNameWithoutExtension(FileUploadDoc.FileName);
                            string Extn = System.IO.Path.GetExtension(FileUploadDoc.FileName);

                            //string projectName = getdata.getProjectNameby_ProjectUID(new Guid(Request.QueryString["ProjectUID"]));
                            //string InputFile = System.IO.Path.GetExtension(FileUploadDoc.FileName);

                            FileUploadDoc.SaveAs(Server.MapPath(sDocumentPath + "/" + sFileName + "_1_copy" + Extn));
                            string savedPath = sDocumentPath + "/" + sFileName + "_1_copy" + Extn;
                            DocPath = sDocumentPath + "/" + sFileName + "_1" + Extn;

                            getdata.EncryptFile(Server.MapPath(savedPath), Server.MapPath(DocPath));

                            //FileUploadDoc.SaveAs(Server.MapPath("~/Documents/" + Request.QueryString["ProjectUID"] + "/" + DocVersion_UID + "_" + DocVersion + InputFile));
                            //FileUploadDoc.SaveAs(Server.MapPath("~/Documents/Encrypted/" + DocVersion_UID + "_" + DocVersion + "_enp" + InputFile));

                            //DocPath = "~/Documents/" + projectName + "/" + DocVersion_UID + "_" + DocVersion + InputFile;
                            //File.Encrypt(Server.MapPath(DocPath));

                            int cnt = getdata.InsertDocumentVersion(DocVersion_UID, new Guid(Request.QueryString["StatusUID"]), new Guid(Request.QueryString["DocumentUID"]), Extn, DocPath, txtcomments.Text, ConverLetterFilepath);
                            if (cnt > 0)
                            {
                                DataSet ds = getdata.getAllUsers();
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    string CC = string.Empty;
                                    for (int i = 1; i < ds.Tables[0].Rows.Count; i++)
                                    {
                                        CC += ds.Tables[0].Rows[i]["EmailID"].ToString() + ",";
                                    }
                                    CC = CC.TrimEnd(',');

                                    sHtmlString = "<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>" + "<html xmlns='http://www.w3.org/1999/xhtml'>" +
                                           "<head>" + "<meta http-equiv='Content-Type' content='text/html; charset=utf-8' />" + "</head>" +
                                              "<body style='font-family:Verdana, Arial, sans-serif; font-size:12px; font-style:normal;'>";
                                    sHtmlString += "<div style='float:left; width:100%; height:30px;'>" +
                                                       "Dear, " + "Users" +
                                                       "<br/><br/></div>";
                                    sHtmlString += "<div style='width:100%; float:left;'><div style='width:100%; float:left;'>Below are the Status details. <br/><br/></div>";
                                    sHtmlString += "<div style='width:100%; float:left;'><div style='width:100%; float:left;'>Document Name : " + getdata.getDocumentName_by_DocumentUID(new Guid(Request.QueryString["DocumentUID"])) + "<br/></div>";
                                    sHtmlString += "<div style='width:100%; float:left;'><div style='width:100%; float:left;'>Version : " + DocVersion + "<br/></div>";
                                    sHtmlString += "<div style='width:100%; float:left;'><div style='width:100%; float:left;'>Status : " + getdata.getDocumentStatus_by_StatusUID(new Guid(Request.QueryString["StatusUID"])) + "<br/></div>";
                                    sHtmlString += "<div style='width:100%; float:left;'><div style='width:100%; float:left;'>Document Type : " + Extn + "<br/></div>";
                                    sHtmlString += "<div style='width:100%; float:left;'><div style='width:100%; float:left;'>Date : " + DateTime.Now.ToShortDateString() + "<br/></div>";
                                    sHtmlString += "<div style='width:100%; float:left;'><div style='width:100%; float:left;'>Comments : " + txtcomments.Text + "<br/></div>";
                                    sHtmlString += "<div style='width:100%; float:left;'><br/><br/>Sincerely, <br/> Project Manager Tool.</div></div></body></html>";
                                    //string ret = getdata.SendMail(ds.Tables[0].Rows[0]["EmailID"].ToString(), Subject, sHtmlString, CC, Server.MapPath(DocPath));
                                }

                                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
                            }
                        }
                        else
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Please choose a file to upload');</script>");
                        }
                    }
                    else
                    {
                        if (Request.Files.Count > 0)
                        {
                            HttpFileCollection attachments = Request.Files;
                            for (int i = 0; i < attachments.Count; i++)
                            {
                                HttpPostedFile attachment = attachments[i];
                                if (attachment.ContentLength > 0 && !String.IsNullOrEmpty(attachment.FileName))
                                {
                                    Guid DocVersion_UID = Guid.NewGuid();
                                    string sFileName = Path.GetFileNameWithoutExtension(Path.GetFileName(attachment.FileName));
                                    string Extn = System.IO.Path.GetExtension(Path.GetFileName(attachment.FileName));

                                    string SubmittalUID = getdata.GetSubmittalUID_By_ActualDocumentUID(new Guid(Request.QueryString["DocumentUID"]));

                                    string ActualDocumentUID = getdata.ActualDocumentUID_By_ActualDocumentName(sFileName, new Guid(SubmittalUID));

                                    string StatusUID = getdata.GetCurrentDocumentStatus_by_DocumentUID(new Guid(ActualDocumentUID));
                                    //string DocumentUID=getdata.getDocumentName_by_DocumentUID
                                    int DocVersion = getdata.getDocumentStatusVersion(new Guid(StatusUID), new Guid(ActualDocumentUID));
                                    string FileDatetime = DateTime.Now.ToString("dd MMM yyyy hh-mm-ss tt");
                                    string sDocumentPath = "~/" + Request.QueryString["ProjectUID"] + "/" + ActualDocumentUID + "/" + FileDatetime + "/Document";
                                    if (!Directory.Exists(Server.MapPath(sDocumentPath)))
                                    {
                                        Directory.CreateDirectory(Server.MapPath(sDocumentPath));
                                    }

                                    attachment.SaveAs(Server.MapPath(sDocumentPath + "/" + sFileName + "_1_copy" + Extn));
                                    string savedPath = sDocumentPath + "/" + sFileName + "_1_copy" + Extn;
                                    DocPath = sDocumentPath + "/" + sFileName + "_1" + Extn;

                                    getdata.EncryptFile(Server.MapPath(savedPath), Server.MapPath(DocPath));

                                    int cnt = getdata.InsertDocumentVersion(DocVersion_UID, new Guid(StatusUID), new Guid(ActualDocumentUID), Extn, DocPath, txtcomments.Text,"");
                                    if (cnt > 0)
                                    {

                                    }
                                }
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code : RSD 11 - There is a problem with this feature. Please contact system admin');</script>");
                }
            }
        }

        protected void RBLUploadType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RBLUploadType.SelectedValue == "Folder")
            {
                UploadFolder.Visible = true;
                UploadFile.Visible = false;
            }
            else
            {
                UploadFolder.Visible = false;
                UploadFile.Visible = true;
            }
        }
    }
}