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
            Guid IssueRemarksUID = Guid.NewGuid();
            if (Request.QueryString["IssueRemarksUID"] != null)
            {
                DecryptPagePath = ViewState["Document"].ToString();
                IssueRemarksUID = new Guid(Request.QueryString["IssueRemarksUID"]);
            }
            if (FileUploadDoc.HasFile)
            {
                string FileDirectory = "~/Documents/Issues/";
                if (!Directory.Exists(Server.MapPath(FileDirectory)))
                {
                    Directory.CreateDirectory(Server.MapPath(FileDirectory));
                }

                string sFileName = Path.GetFileNameWithoutExtension(FileUploadDoc.FileName);
                string Extn = Path.GetExtension(FileUploadDoc.FileName);
                FileUploadDoc.SaveAs(Server.MapPath(FileDirectory + "/" + sFileName + Extn));
                //FileUploadDoc.SaveAs(Server.MapPath("~/Documents/Encrypted/" + sDocumentUID + "_" + txtDocName.Text + "_1"  + "_enp" + InputFile));
                string savedPath = FileDirectory + "/" + sFileName + Extn;
                DecryptPagePath = FileDirectory + "/" + sFileName + "_DE" + Extn;
                getdata.EncryptFile(Server.MapPath(savedPath), Server.MapPath(DecryptPagePath));
            }
           
                int cnt = getdata.Issues_Status_Remarks_Insert(IssueRemarksUID, new Guid(Request.QueryString["Issue_Uid"]), DDLStatus.SelectedValue, txtremarks.Text, DecryptPagePath);
            if (cnt > 0)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
            }
        }
    }
}