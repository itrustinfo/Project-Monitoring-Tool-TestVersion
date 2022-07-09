using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagementTool._modal_pages
{
    public partial class add_issuestatus : System.Web.UI.Page
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
                    if (Request.QueryString["Issue_Uid"] != null)
                    {
                        IssueBind();
                    }
                    if (Request.QueryString["IssueRemarksUID"] != null)
                    {
                        IssueStatusDataBind();
                    }
                }
            }
        }

        private void IssueStatusDataBind()
        {
            DataSet ds = getdata.GetIssueStatus_by_IssueRemarksUID(new Guid(Request.QueryString["IssueRemarksUID"]));
            if (ds.Tables[0].Rows.Count > 0)
            {
                DDLStatus.SelectedValue = ds.Tables[0].Rows[0]["Issue_Status"].ToString();
                txtremarks.Text = ds.Tables[0].Rows[0]["Issue_Remarks"].ToString();
                ViewState["Document"] = ds.Tables[0].Rows[0]["Issue_Document"].ToString();
                //DDLStatus.Enabled = false;
            }
        }

        private void IssueBind()
        {
            DataSet ds = getdata.getIssuesList_by_UID(new Guid(Request.QueryString["Issue_Uid"]));
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["Issue_Status"].ToString() == "Close")
                {
                    DDLStatus.SelectedValue = "Close";
                    DDLStatus.Enabled = false;
                    btnSubmit.Visible = false;
                }
                else
                {
                    DDLStatus.Enabled = true;
                    btnSubmit.Visible = true;
                }

                if (ds.Tables[0].Rows[0]["TaskUID"].ToString() == "00000000-0000-0000-0000-000000000000")
                {
                    HiddenActivity.Value = ds.Tables[0].Rows[0]["WorkPackagesUID"].ToString();
                }
                else
                {
                    HiddenActivity.Value = ds.Tables[0].Rows[0]["TaskUID"].ToString();
                }


            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string DecryptPagePath = "";
            string files_path = "";
            //Guid IssueRemarksUID = Guid.NewGuid();
            //if (Request.QueryString["IssueRemarksUID"] != null)
            //{
            //    DecryptPagePath = ViewState["Document"].ToString();
            //    IssueRemarksUID = new Guid(Request.QueryString["IssueRemarksUID"]);
            //}
            //if (FileUploadDoc.HasFile)
            //{
            //    string FileDirectory = "~/Documents/Issues/";
            //    if (!Directory.Exists(Server.MapPath(FileDirectory)))
            //    {
            //        Directory.CreateDirectory(Server.MapPath(FileDirectory));
            //    }

            //    string sFileName = Path.GetFileNameWithoutExtension(FileUploadDoc.FileName);
            //    string Extn = Path.GetExtension(FileUploadDoc.FileName);
            //    FileUploadDoc.SaveAs(Server.MapPath(FileDirectory + "/" + sFileName + Extn));
            //   // FileUploadDoc.SaveAs(Server.MapPath("~/Documents/Encrypted/" + sDocumentUID + "_" + txtDocName.Text + "_1"  + "_enp" + InputFile));
            //    string savedPath = FileDirectory + "/" + sFileName + Extn;
            //    DecryptPagePath = FileDirectory + "/" + sFileName + "_DE" + Extn;
            //    getdata.EncryptFile(Server.MapPath(savedPath), Server.MapPath(DecryptPagePath));
            //}

            //int cnt = getdata.Issues_Status_Remarks_Insert(IssueRemarksUID, new Guid(Request.QueryString["Issue_Uid"]), DDLStatus.SelectedValue, txtremarks.Text, DecryptPagePath);
            //if (cnt > 0)
            //{
            //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
            //}

            var issue_uid = new Guid(Request.QueryString["Issue_Uid"]);

            var issue_remarks_uid = Guid.NewGuid();
                       
            if (Request.QueryString["IssueRemarksUID"] != null)
            {
               // DecryptPagePath = ViewState["Document"].ToString();
                issue_remarks_uid = new Guid(Request.QueryString["IssueRemarksUID"]);
            }

            int cnt = getdata.Issues_Status_Remarks_Insert(issue_remarks_uid, issue_uid, DDLStatus.SelectedValue, txtremarks.Text, DecryptPagePath,DateTime.Today.Date);
            
           
            if (FileUploadDoc.HasFiles)
            {
                string FileDirectory = "/Documents/Issues/";


                if (!Directory.Exists(Server.MapPath(FileDirectory)))
                {
                    Directory.CreateDirectory(Server.MapPath(FileDirectory));
                }

                foreach (HttpPostedFile postedFile in FileUploadDoc.PostedFiles)
                {
                    string fileName = Path.GetFileName(postedFile.FileName);
                    postedFile.SaveAs(Server.MapPath(FileDirectory) + fileName);

                    string sFileName = Path.GetFileNameWithoutExtension(postedFile.FileName);
                    string Extn = Path.GetExtension(postedFile.FileName);

                    string savedPath = FileDirectory + "/" + fileName;

                    DecryptPagePath = FileDirectory + "/" + sFileName + "_DE" + Extn;

                    files_path = files_path + Server.MapPath(DecryptPagePath) + ",";

                    getdata.EncryptFile(Server.MapPath(savedPath), Server.MapPath(DecryptPagePath));

                    getdata.InsertUploadedDocument(sFileName + "_DE" + Extn, FileDirectory, issue_remarks_uid.ToString());
                }
            }

            DataSet ds_issue = getdata.GetIssueByIssueUid(Request.QueryString["Issue_Uid"]);

           
            string work_package_name = "";
            string issue_description = "";

            if (ds_issue.Tables[0].Rows.Count  >0)
            {
                work_package_name = ds_issue.Tables[0].Rows[0].ItemArray[0].ToString();
                issue_description = ds_issue.Tables[0].Rows[0].ItemArray[1].ToString();
            }

            string sHtmlString = "";
            string RefNostring = "";

            sHtmlString = "<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>" + "<html xmlns='http://www.w3.org/1999/xhtml'>" +
                                      "<head>" + "<meta http-equiv='Content-Type' content='text/html; charset=utf-8' />" + "<style>table, th, td {border: 1px solid black; padding:6px;}</style></head>" +
                                         "<body style='font-family:Verdana, Arial, sans-serif; font-size:12px; font-style:normal;'>";
            sHtmlString += "<div style='width:80%; float:left; padding:1%; border:2px solid #011496; border-radius:5px;'>" +
                               "<div style='float:left; width:100%; border-bottom:2px solid #011496;'>";
            if (WebConfigurationManager.AppSettings["Domain"] == "NJSEI")
            {
                sHtmlString += "<div style='float:left; width:7%;'><img src='https://dm.njsei.com/_assets/images/NJSEI%20Logo.jpg' width='50' /></div>";
                RefNostring = "NJSEI Ref Number";
            }
            else
            {
                sHtmlString += "<div style='float:left; width:7%;'><h2>" + WebConfigurationManager.AppSettings["Domain"] + "</h2></div>";
                RefNostring = "ONTB Ref Number";
            }
            sHtmlString += "<div style='float:left; width:70%;'><h2 style='margin-top:10px;'>Project Monitoring Tool</h2></div>" +
                       "</div>";
            sHtmlString += "<div style='width:100%; float:left;'><br/>Dear Users,<br/><br/><span style='font-weight:bold;'>" + Session["Username"].ToString() + " has changed issue status" + "</span> <br/><br/></div>";
            sHtmlString += "<div style='width:100%; float:left;'><table style='width:100%;'>" +
                            "<tr><td><b>Project Name </b></td><td style='text-align:center;'><b>:</b></td><td>" + Session["ProjectName"].ToString() + "</td></tr>" +
                            "<tr><td><b>Activity Name </b></td><td style='text-align:center;'><b>:</b></td><td>" + Session["ActivityName"].ToString() + "</td></tr>" +
                            "<tr><td><b>Issue </b></td><td style='text-align:center;'><b>:</b></td><td>" + issue_description + "</td></tr>" +
                            "<tr><td><b>Issue Status </b></td><td style='text-align:center;'><b>:</b></td><td>" + DDLStatus.Text + "</td></tr>" +
                            "<tr><td><b>Remarks </b></td><td style='text-align:center;'><b>:</b></td><td>" + txtremarks.Text + "</td></tr>" +
                            "<tr><td><b>Date </b></td><td style='text-align:center;'><b>:</b></td><td>" + DateTime.Today.ToString("dd-MMM-yyyy") + "</td></tr>";
            sHtmlString += "</table></div>";
            sHtmlString += "<div style='width:100%; float:left;'><br/><br/>Sincerely, <br/> MIS System.</div></div></body></html>";

            DataTable dtemailCred = getdata.GetEmailCredentials();

            DataSet ds_wp_emails = getdata.GetWorkPackageEmails(new Guid(Session["ProjectUID"].ToString()), new Guid(Session["WorkPackageUID"].ToString()));

            string to_email_ids = "";

            foreach (DataRow email in ds_wp_emails.Tables[0].Rows)
            {
                to_email_ids = to_email_ids + email.ItemArray[1].ToString() + ",";
            }
 
            if (to_email_ids.Length >0)
               to_email_ids = to_email_ids.Substring(0, to_email_ids.Length - 1);

            if (files_path.Length >0)
               files_path = files_path.Substring(0, files_path.Length - 1);


            Guid MailUID = Guid.NewGuid();

            if (ds_wp_emails.Tables[0].Rows.Count >0)
                getdata.StoreEmaildataToMailQueue(MailUID, new Guid(Session["UserUID"].ToString()), dtemailCred.Rows[0][0].ToString(), to_email_ids, "Issue Status", sHtmlString, "", files_path);


            Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
        }
    }
}