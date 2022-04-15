using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagementTool._modal_pages
{
    public partial class add_issues : System.Web.UI.Page
    {
        TaskUpdate TKUpdate = new TaskUpdate();
        DBGetData getdata = new DBGetData();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDropDowns();
                if (Request.QueryString["IssueFor"] != null)
                {
                    txtactivityname.Text = Request.QueryString["AName"];
                    AcvitityName.Visible = true;
                    IssueStatus.Visible = false;
                }
                else if (Request.QueryString["Issue_Uid"] != null)
                {
                    BindIssues();
                    AcvitityName.Visible = false;
                    IssueStatus.Visible = false;
                }
            }
        }

        private void BindIssues()
        {
            DataSet ds = getdata.getIssuesList_by_UID(new Guid(Request.QueryString["Issue_Uid"]));
            if (ds.Tables[0].Rows.Count > 0)
            {
                //DDlProject.SelectedValue = ds.Tables[0].Rows[0]["ProjectUID"].ToString();
                ProjectHiddenValue.Value = ds.Tables[0].Rows[0]["ProjectUID"].ToString();
                WorkPackageHiddenValue.Value = ds.Tables[0].Rows[0]["WorkPackagesUID"].ToString();
                TaskHiddenValue.Value = ds.Tables[0].Rows[0]["TaskUID"].ToString();
                //DDLWorkPackage.SelectedValue = ds.Tables[0].Rows[0]["WorkPackagesUID"].ToString();
                txtIssue_Description.Text = ds.Tables[0].Rows[0]["Issue_Description"].ToString();
                txtRemarks.Text = ds.Tables[0].Rows[0]["Issue_Remarks"].ToString();
                //ddlTask.SelectedValue = ds.Tables[0].Rows[0]["TaskUID"].ToString();
                ddlReportingUser.SelectedValue = new Guid(ds.Tables[0].Rows[0]["Issued_User"].ToString()).ToString();
                ddlAssignedUser.SelectedValue = new Guid(ds.Tables[0].Rows[0]["Assigned_User"].ToString()).ToString();
                ddlApprovingUser.SelectedValue = new Guid(ds.Tables[0].Rows[0]["Approving_User"].ToString()).ToString();

                ddlStatus.SelectedItem.Text = ds.Tables[0].Rows[0]["Issue_Status"].ToString();

                if (ds.Tables[0].Rows[0]["Issue_Date"].ToString() != null && ds.Tables[0].Rows[0]["Issue_Date"].ToString() != "")
                {
                    dtReportingDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["Issue_Date"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                if (ds.Tables[0].Rows[0]["Assigned_Date"].ToString() != null && ds.Tables[0].Rows[0]["Assigned_Date"].ToString() != "")
                {
                    dtAssignedDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["Assigned_Date"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                if (ds.Tables[0].Rows[0]["Actual_Closer_Date"].ToString() != null && ds.Tables[0].Rows[0]["Actual_Closer_Date"].ToString() != "")
                {
                    dtApprovingDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["Actual_Closer_Date"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                if (ds.Tables[0].Rows[0]["Issue_ProposedCloser_Date"].ToString() != null && ds.Tables[0].Rows[0]["Issue_ProposedCloser_Date"].ToString() != "")
                {
                    dtProposedCloserDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["Issue_ProposedCloser_Date"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                }

            }
        }
        private void LoadDropDowns()
        {
            try
            {
                DataSet ds = new DataSet();
                if (Session["TypeOfUser"].ToString() == "U" || Session["TypeOfUser"].ToString() =="MD" || Session["TypeOfUser"].ToString() == "VP")
                {
                    ds = getdata.getAllUsers();
                }
                else if (Session["TypeOfUser"].ToString() == "PA")
                {
                    //ds = getdata.getUsers_by_ProjectUnder(new Guid(DDlProject.SelectedValue));
                    ds = getdata.GetUsers_under_ProjectUID(new Guid(Request.QueryString["PrjID"]));
                }
                else
                {
                    //ds = getdata.getUsers_by_ProjectUnder(new Guid(Request.QueryString["PrjID"]));
                    ds = getdata.GetUsers_under_ProjectUID(new Guid(Request.QueryString["PrjID"]));
                }

                ddlReportingUser.DataSource = ds;
                ddlReportingUser.DataTextField = "UserName";
                ddlReportingUser.DataValueField = "UserUID";
                ddlReportingUser.DataBind();
                //
                ddlAssignedUser.DataSource = ds;
                ddlAssignedUser.DataTextField = "UserName";
                ddlAssignedUser.DataValueField = "UserUID";
                ddlAssignedUser.DataBind();

                //
                ddlApprovingUser.DataSource = ds;
                ddlApprovingUser.DataTextField = "UserName";
                ddlApprovingUser.DataValueField = "UserUID";
                ddlApprovingUser.DataBind();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                Guid Issue_Uid = new Guid();
                string ProjectUID = string.Empty;
                string WorkPackageID = string.Empty;
                string TaskUID = string.Empty;
                if (Request.QueryString["Issue_Uid"] != null)
                {
                    Issue_Uid = new Guid(Request.QueryString["Issue_Uid"]);
                    ProjectUID = ProjectHiddenValue.Value;
                    WorkPackageID = WorkPackageHiddenValue.Value;
                    TaskUID = TaskHiddenValue.Value;
                }
                else
                {
                    Issue_Uid = Guid.NewGuid();
                    if (Request.QueryString["ActivityID"] != null)
                    {
                        ProjectUID = Request.QueryString["PrjID"];
                        if (Request.QueryString["IssueFor"] == "WorkPackage")
                        {
                            WorkPackageID = Request.QueryString["ActivityID"].ToString();
                            TaskUID = Guid.Empty.ToString();
                        }
                        else
                        {

                            DataTable dt = getdata.GetTaskDetails_TaskUID(Request.QueryString["ActivityID"].ToString());
                            if (dt.Rows.Count > 0)
                            {
                                WorkPackageID = dt.Rows[0]["WorkPackageUID"].ToString();
                            }
                            else
                            {
                                WorkPackageID = Guid.Empty.ToString();
                            }
                            TaskUID = Request.QueryString["ActivityID"].ToString();
                        }
                    }
                }
                string sDate1 = "", sDate2 = "", sDate3 = "", sDate4 = "";
                DateTime CDReportingDate = DateTime.MinValue;
                DateTime? CDAssignedDate = null, CDApprovingDate = null, CDProposeClosureDate = null;

                //
                if (dtReportingDate.Text != "")
                {
                    sDate1 = dtReportingDate.Text;
                    //sDate1 = sDate1.Split('/')[1] + "/" + sDate1.Split('/')[0] + "/" + sDate1.Split('/')[2];
                    sDate1 = getdata.ConvertDateFormat(sDate1);
                    CDReportingDate = Convert.ToDateTime(sDate1);
                }

                //
                if (dtAssignedDate.Text != "")
                {
                    sDate2 = dtAssignedDate.Text;
                    //sDate2 = sDate2.Split('/')[1] + "/" + sDate2.Split('/')[0] + "/" + sDate2.Split('/')[2];
                    sDate2 = getdata.ConvertDateFormat(sDate2);
                    CDAssignedDate = Convert.ToDateTime(sDate2);
                }

                //
                if (dtApprovingDate.Text != "")
                {
                    sDate3 = dtApprovingDate.Text;
                    //sDate3 = sDate3.Split('/')[1] + "/" + sDate3.Split('/')[0] + "/" + sDate3.Split('/')[2];
                    sDate3 = getdata.ConvertDateFormat(sDate3);
                    CDApprovingDate = Convert.ToDateTime(sDate3);
                }

                //
                if (dtProposedCloserDate.Text != "")
                {
                    sDate4 = dtProposedCloserDate.Text;
                    //sDate4 = sDate4.Split('/')[1] + "/" + sDate4.Split('/')[0] + "/" + sDate4.Split('/')[2];
                    sDate4 = getdata.ConvertDateFormat(sDate4);
                    CDProposeClosureDate = Convert.ToDateTime(sDate4);
                }

                
                string DecryptPagePath = "";
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
                int Cnt = getdata.InsertorUpdateIssues(Issue_Uid, new Guid(TaskUID), txtIssue_Description.Text, CDReportingDate, new Guid(ddlReportingUser.SelectedValue), new Guid(ddlAssignedUser.SelectedValue), CDAssignedDate, CDProposeClosureDate, new Guid(ddlApprovingUser.SelectedValue), CDApprovingDate, ddlStatus.SelectedItem.Text, txtRemarks.Text, new Guid(WorkPackageID), new Guid(ProjectUID), DecryptPagePath);
                if (Cnt > 0)
                {
                    //if (Request.QueryString["Issue_Uid"] == null)
                    //{
                    //    if (Request.QueryString["IssueFor"] == "WorkPackage")
                    //    {
                    //        Session["SelectedActivity"] = WorkPackageID;
                    //    }
                    //    else
                    //    {
                    //        Session["SelectedActivity"] = TaskUID;
                    //    }
                        
                    //}
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
                }

            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code : AIS 01, there is a problem with this feature. please contact system admin.');</script>");
            }
        }
    }
}