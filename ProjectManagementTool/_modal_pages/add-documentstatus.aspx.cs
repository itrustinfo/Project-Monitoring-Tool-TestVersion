using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagementTool._modal_pages
{
    public partial class add_documentstatus : System.Web.UI.Page
    {
        DBGetData getdata = new DBGetData();
        string cnnString = ConfigurationManager.ConnectionStrings["PMConnectionString"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Username"] == null)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
            }
            else
            {
                this.UnobtrusiveValidationMode = System.Web.UI.UnobtrusiveValidationMode.None;
                if (!IsPostBack)
                {
                    BindDocuments();
                    BindOriginator();
                    if (Request.QueryString["StatusUID"] != null)
                    {
                        BindStatus();
                        BindDocStatus(new Guid(Request.QueryString["StatusUID"]));
                        btnSubmit.Text = "Update";

                    }
                    else
                    {
                        DDLDocument.SelectedValue = Request.QueryString["DocID"];
                        btnSubmit.Text = "Submit";
                        BindStatus();
                    }
                    
                }
            }
        }

        
        private void BindDocuments()
        {
            //DataSet ds = getdata.getDocuments();
            DataSet ds = getdata.ActualDocuments_SelectBy_ActualDocumentUID(new Guid(Request.QueryString["DocID"]));
            DDLDocument.DataTextField = "ActualDocument_Name";
            DDLDocument.DataValueField = "ActualDocumentUID";
            DDLDocument.DataSource = ds;
            DDLDocument.DataBind();
        }
        private void BindStatus()
        {
            DataSet ds1 = getdata.getTop1_DocumentStatusSelect(new Guid(Request.QueryString["DocID"]));
            if (ds1.Tables[0].Rows.Count > 0)
            {
                //int StepCount = getdata.GetFlowStep_by_FlowUID(new Guid(Request.QueryString["FlowUID"]));
                //DataSet ds = getdata.GetStatus_by_UserType(Session["TypeOfUser"].ToString(), ds1.Tables[0].Rows[0]["ActivityType"].ToString(), StepCount);
                DataSet ds = getdata.GetStatus_by_UserType_FlowUID(Session["TypeOfUser"].ToString(), ds1.Tables[0].Rows[0]["ActivityType"].ToString(), new Guid(ds1.Tables[0].Rows[0]["FlowUID"].ToString()));
                DDlStatus.DataTextField = "Update_Status";
                DDlStatus.DataValueField = "Update_Status";
                DDlStatus.DataSource = ds;
                DDlStatus.DataBind();
                
                if (DDlStatus.Items.Count > 0)
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "Tooltip", "getValueFromCodeBehind('" + DDlStatus.SelectedItem.Text + "')", true);
                    btnSubmit.Visible = true;

                }
                else
                {
                    btnSubmit.Visible = false;
                }
                // added on  12/01/2022 for Correspondence ONTB / flow 3 NJSEI

                string Approver = getdata.GetSubmittal_Approver_By_DocumentUID(new Guid(Request.QueryString["DocID"]));
                string Reviewer = getdata.GetSubmittal_Reviewer_By_DocumentUID(new Guid(Request.QueryString["DocID"]));
                if (Session["UserUID"].ToString().ToUpper() == Reviewer.ToUpper())
                {
                    
                }
                else if (Approver != "")
                {
                    if (Session["UserUID"].ToString().ToUpper() == Approver.ToUpper())
                    {
                        //DDlStatus.Items.Clear();
                        //DDlStatus.Items.Insert(0, "Closed");
                        DDlStatus.Items.Remove("Reply");
                        DDlStatus.Items.Remove("Received");
                    }
                    else
                    {
                        DDlStatus.Items.Remove("Closed");
                    }

                }

            }
        }
        private void BindDocStatus(Guid StatusUID)
        {
            DataSet ds = getdata.getDocumentStatusList_by_StatusUID(StatusUID);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DDLDocument.SelectedValue = ds.Tables[0].Rows[0]["DocumentUID"].ToString();
                //txtbudget.Text = ds.Tables[0].Rows[0]["Activity_Budget"].ToString();
                txtcomments.Text = ds.Tables[0].Rows[0]["Status_Comments"].ToString();
                if (DDlStatus.Items.Count > 0)
                {
                    DDlStatus.SelectedItem.Text = ds.Tables[0].Rows[0]["ActivityType"].ToString();
                }
                if (ds.Tables[0].Rows[0]["ActivityDate"].ToString() != null && ds.Tables[0].Rows[0]["ActivityDate"].ToString() != "")
                {
                    dtStartdate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["ActivityDate"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                if (ds.Tables[0].Rows[0]["DocumentDate"].ToString() != null && ds.Tables[0].Rows[0]["DocumentDate"].ToString() != "")
                {
                    dtDocumentDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["DocumentDate"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                if (ds.Tables[0].Rows[0]["LinkToReviewFile"].ToString() != "" && ds.Tables[0].Rows[0]["LinkToReviewFile"].ToString() != null)
                {
                    FileUploadDoc.Enabled = false;
                }
                else
                {
                    FileUploadDoc.Enabled = true;
                }
                if (ds.Tables[0].Rows[0]["Origin"].ToString() != "")
                {
                    RBLOriginator.SelectedValue = ds.Tables[0].Rows[0]["Origin"].ToString();
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e) //final submit
        {
            if (DDlStatus.SelectedItem.ToString() == "Closed")
            {
                try
                {
                    if (dtStartdate.Text == "" || dtStartdate.Text == "dd/MM/YYYY")
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "Warning", "<script language='javascript'>alert('Please enter Actual Date.');</script>");
                        return;
                    }
                    string DocPath = "";
                    string Subject = string.Empty;
                    string sHtmlString = string.Empty;
                    Guid StatusUID = new Guid();
                    string CoverPagePath = string.Empty;
                    if (Request.QueryString["StatusUID"] != null)
                    {
                        StatusUID = new Guid(Request.QueryString["StatusUID"]);
                        Subject = Session["Username"].ToString() + " updated a Status";
                    }
                    else
                    {
                        StatusUID = Guid.NewGuid();
                        Subject = Session["Username"].ToString() + " added a new Status";
                    }

                    string sDate1 = "";
                    DateTime CDate1 = DateTime.Now;
                    //
                    sDate1 = dtStartdate.Text;
                    //sDate1 = sDate1.Split('/')[1] + "/" + sDate1.Split('/')[0] + "/" + sDate1.Split('/')[2];
                    sDate1 = getdata.ConvertDateFormat(sDate1);
                    CDate1 = Convert.ToDateTime(sDate1);

                    DateTime lastUpdated = getdata.GetDocumentMax_ActualDate(new Guid(Request.QueryString["DocID"]));
                    if (lastUpdated.Date > CDate1.Date)
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Actual Date should be greater than previous date.');</script>");
                    }
                    else
                    {

                        string DocumentDate = string.Empty;
                        if (dtDocumentDate.Text != "")
                        {
                            DocumentDate = dtDocumentDate.Text;
                        }
                        else
                        {
                            DocumentDate = dtStartdate.Text;
                        }
                        //DocumentDate = DocumentDate.Split('/')[1] + "/" + DocumentDate.Split('/')[0] + "/" + DocumentDate.Split('/')[2];
                        DocumentDate = getdata.ConvertDateFormat(DocumentDate);
                        DateTime Document_Date = Convert.ToDateTime(DocumentDate);


                        if (FileUploadCoverLetter.HasFile)
                        {
                            string FileDatetime = DateTime.Now.ToString("dd MMM yyyy hh-mm-ss tt");
                            string sDocumentPath = string.Empty;
                            //sDocumentPath = "~/" + Request.QueryString["ProjectUID"] + "/" + Request.QueryString["DocID"] + "/" + StatusUID + "/" + FileDatetime + "/CoverLetter";
                            sDocumentPath = "~/" + Request.QueryString["ProjectUID"] + "/" + StatusUID + "/CoverLetter";

                            if (!Directory.Exists(Server.MapPath(sDocumentPath)))
                            {
                                Directory.CreateDirectory(Server.MapPath(sDocumentPath));
                            }

                            string sFileName = Path.GetFileNameWithoutExtension(FileUploadCoverLetter.FileName);
                            string Extn = System.IO.Path.GetExtension(FileUploadCoverLetter.FileName);
                            FileUploadCoverLetter.SaveAs(Server.MapPath(sDocumentPath + "/" + sFileName + "_1_copy" + Extn));
                            string savedPath = sDocumentPath + "/" + sFileName + "_1_copy" + Extn;
                            CoverPagePath = sDocumentPath + "/" + sFileName + "_1" + Extn;
                            getdata.EncryptFile(Server.MapPath(savedPath), Server.MapPath(CoverPagePath));

                        }


                        //
                        //sDate2 = (dtPlannedDate.FindControl("txtDate") as TextBox).Text;
                        //sDate2 = sDate2.Split('/')[1] + "/" + sDate2.Split('/')[0] + "/" + sDate2.Split('/')[2];
                        //CDate2 = Convert.ToDateTime(sDate2);
                        int Cnt = getdata.InsertorUpdateDocumentStatus(StatusUID, new Guid(Request.QueryString["DocID"].ToString()), 1, DDlStatus.SelectedItem.Text, 0, CDate1, DocPath,
                            new Guid(Session["UserUID"].ToString()), txtcomments.Text, DDlStatus.SelectedItem.Text, txtrefNumber.Text, Document_Date, CoverPagePath, RBLDocumentStatusUpdate.SelectedValue, RBLOriginator.SelectedValue);
                        if (Cnt > 0)
                        {
                            //DataSet ds = getdata.getAllUsers();
                            DataSet ds = new DataSet();
                            //if (Session["TypeOfUser"].ToString() == "U" || Session["TypeOfUser"].ToString() == "MD" || Session["TypeOfUser"].ToString() == "VP")
                            //{
                            //    ds = getdata.getAllUsers();
                            //}
                            //else if (Session["TypeOfUser"].ToString() == "PA")
                            //{
                            //ds = getdata.getUsers_by_ProjectUnder(new Guid(DDlProject.SelectedValue));
                            ds = getdata.GetUsers_under_ProjectUID(new Guid(Request.QueryString["ProjectUID"]));
                            // }
                            //else
                            //{
                            //    ds = getdata.getUsers_by_ProjectUnder(new Guid(Request.QueryString["ProjectUID"]));
                            //}

                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                string CC = string.Empty;
                                string ToEmailID = "";
                                string sUserName = "";
                                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                {
                                    if (ds.Tables[0].Rows[i]["UserUID"].ToString() == Session["UserUID"].ToString())
                                    {
                                        ToEmailID = ds.Tables[0].Rows[i]["EmailID"].ToString();
                                        sUserName = ds.Tables[0].Rows[i]["UserName"].ToString();
                                    }
                                    else
                                    {
                                        if (getdata.GetUserMailAccess(new Guid(ds.Tables[0].Rows[i]["UserUID"].ToString()), "documentmail") != 0)
                                        {
                                            CC += ds.Tables[0].Rows[i]["EmailID"].ToString() + ",";
                                        }
                                    }
                                }
                                CC = CC.TrimEnd(',');
                                sHtmlString = "<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>" + "<html xmlns='http://www.w3.org/1999/xhtml'>" +
                                  "<head>" + "<meta http-equiv='Content-Type' content='text/html; charset=utf-8' />" + "<style>table, th, td {border: 1px solid black; padding:6px;}</style></head>" +
                                     "<body style='font-family:Verdana, Arial, sans-serif; font-size:12px; font-style:normal;'>";
                                sHtmlString += "<div style='width:80%; float:left; padding:1%; border:2px solid #011496; border-radius:5px;'>" +
                                                   "<div style='float:left; width:100%; border-bottom:2px solid #011496;'>";
                                if (WebConfigurationManager.AppSettings["Domain"] == "NJSEI")
                                {
                                    sHtmlString += "<div style='float:left; width:7%;'><img src='https://dm.njsei.com/_assets/images/NJSEI%20Logo.jpg' width='50' /></div>";
                                }
                                else
                                {
                                    sHtmlString += "<div style='float:left; width:7%;'><h2>" + WebConfigurationManager.AppSettings["Domain"] + "</h2></div>";
                                }
                                sHtmlString += "<div style='float:left; width:70%;'><h2 style='margin-top:10px;'>Project Monitoring Tool</h2></div>" +
                                           "</div>";
                                sHtmlString += "<div style='width:100%; float:left;'><br/>Dear Users,<br/><br/><span style='font-weight:bold;'>" + sUserName + " has changed " + DDLDocument.SelectedItem.Text + " status.</span> <br/><br/></div>";
                                sHtmlString += "<div style='width:100%; float:left;'><table style='width:100%;'>" +
                                                "<tr><td><b>Document Name </b></td><td style='text-align:center;'><b>:</b></td><td>" + DDLDocument.SelectedItem.Text + "</td></tr>" +
                                                "<tr><td><b>Status </b></td><td style='text-align:center;'><b>:</b></td><td>" + DDlStatus.SelectedItem.Text + "</td></tr>" +
                                                "<tr><td><b>Ref. Number </b></td><td style='text-align:center;'><b>:</b></td><td>" + txtrefNumber.Text + "</td></tr>" +
                                                "<tr><td><b>Date </b></td><td style='text-align:center;'><b>:</b></td><td>" + CDate1.ToString("dd MMM yyyy") + "</td></tr>" +
                                                "<tr><td><b>Comments </b></td><td style='text-align:center;'><b>:</b></td><td>" + txtcomments.Text + "</td></tr>";
                                sHtmlString += "</table></div>";
                                sHtmlString += "<div style='width:100%; float:left;'><br/><br/>Sincerely, <br/> Project Monitoring Tool.</div></div></body></html>";

                                //sHtmlString = "<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>" + "<html xmlns='http://www.w3.org/1999/xhtml'>" +
                                //       "<head>" + "<meta http-equiv='Content-Type' content='text/html; charset=utf-8' />" + "</head>" +
                                //          "<body style='font-family:Verdana, Arial, sans-serif; font-size:12px; font-style:normal;'>";
                                //sHtmlString += "<div style='float:left; width:100%; height:30px;'>" +
                                //                   "Dear, " + "Users" +
                                //                   "<br/><br/></div>";
                                //sHtmlString += "<div style='width:100%; float:left;'><div style='width:100%; float:left;'>Below are the Status details. <br/><br/></div>";
                                //sHtmlString += "<div style='width:100%; float:left;'><div style='width:100%; float:left;'>Document Name : " + DDLDocument.SelectedItem.Text + "<br/></div>";
                                //sHtmlString += "<div style='width:100%; float:left;'><div style='width:100%; float:left;'>Status : " + DDlStatus.SelectedItem.Text + "<br/></div>";
                                //sHtmlString += "<div style='width:100%; float:left;'><div style='width:100%; float:left;'>Date : " + CDate1.ToShortDateString() + "<br/></div>";
                                //sHtmlString += "<div style='width:100%; float:left;'><br/><br/>Sincerely, <br/> Project Manager.</div></div></body></html>";
                                //string ret = getdata.SendMail(ds.Tables[0].Rows[0]["EmailID"].ToString(), Subject, sHtmlString, CC, Server.MapPath(DocPath));
                                // added on 02/11/2020
                                DataTable dtemailCred = getdata.GetEmailCredentials();
                                Guid MailUID = Guid.NewGuid();
                                getdata.StoreEmaildataToMailQueue(MailUID, new Guid(Session["UserUID"].ToString()), dtemailCred.Rows[0][0].ToString(), ToEmailID, Subject, sHtmlString, CC, "");
                                //
                               
                                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
                            }
                            else
                            {

                                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error: There is a problem with this feature. Please contact system admin');</script>");

                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code : ADS 12 - There is a problem with this feature. Please contact system admin');</script>");
                }

            }
            else
            {
                
                if (txtrefNumber.Text.Trim()=="")
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "Warning", "<script language='javascript'>alert('Please enter Reference Number.');</script>");
                    return;
                }
                if (dtDocumentDate.Text == "" || dtDocumentDate.Text=="dd/MM/YYYY")
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "Warning", "<script language='javascript'>alert('Please enter Cover letter Date.');</script>");
                    return;
                }
                if (dtStartdate.Text == "" || dtStartdate.Text == "dd/MM/YYYY")
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "Warning", "<script language='javascript'>alert('Please enter Actual Date.');</script>");
                    return;
                }

                if (!FileUploadCoverLetter.HasFile)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "Warning", "<script language='javascript'>alert('Please upload a cover letter.');</script>");
                }
                else
                {
                    try
                    {
                        string DocPath = "";
                        string Subject = string.Empty;
                        string sHtmlString = string.Empty;
                        Guid StatusUID = new Guid();
                        string CoverPagePath = string.Empty;
                        if (Request.QueryString["StatusUID"] != null)
                        {
                            StatusUID = new Guid(Request.QueryString["StatusUID"]);
                            Subject = Session["Username"].ToString() + " updated a Status";
                        }
                        else
                        {
                            StatusUID = Guid.NewGuid();
                            Subject = Session["Username"].ToString() + " added a new Status";
                        }

                        string sDate1 = "";
                        DateTime CDate1 = DateTime.Now;
                        //
                        sDate1 = dtStartdate.Text;
                        //sDate1 = sDate1.Split('/')[1] + "/" + sDate1.Split('/')[0] + "/" + sDate1.Split('/')[2];
                        sDate1 = getdata.ConvertDateFormat(sDate1);
                        CDate1 = Convert.ToDateTime(sDate1);

                        DateTime lastUpdated = getdata.GetDocumentMax_ActualDate(new Guid(Request.QueryString["DocID"]));
                        if (lastUpdated.Date > CDate1.Date)
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Actual Date should be greater than previous date.');</script>");
                        }
                        else
                        {
                            if (FileUploadDoc.HasFile)
                            {
                                //string projectName = getdata.getProjectNameby_ProjectUID(new Guid(Request.QueryString["PrjUID"].ToString()));
                                string LinkPath = Request.QueryString["ProjectUID"] + "/" + StatusUID + "/Link Document";
                                if (!Directory.Exists(Server.MapPath(LinkPath)))
                                {
                                    Directory.CreateDirectory(Server.MapPath(LinkPath));
                                }
                                string sFileName = Path.GetFileNameWithoutExtension(FileUploadDoc.FileName);
                                string Extn = System.IO.Path.GetExtension(FileUploadDoc.FileName);

                                FileUploadDoc.SaveAs(Server.MapPath(LinkPath + "/" + sFileName + "_1_copy" + Extn));
                                //FileUploadDoc.SaveAs(Server.MapPath("~/Documents/Encrypted/" + StatusUID + "_" + "1" + "_enp" + InputFile));
                                string savedPath = LinkPath + "/" + sFileName + "_1_copy" + Extn;
                                DocPath = LinkPath + "/" + sFileName + "_1" + Extn;
                                getdata.EncryptFile(Server.MapPath(savedPath), Server.MapPath(DocPath));

                                //DocPath = "~/Documents/" + projectName + "/" + StatusUID + "_" + "1" + InputFile;
                                //EncryptFile(Server.MapPath(savedPath), Server.MapPath(DocPath));
                                //  File.Encrypt(Server.MapPath(DocPath));
                            }
                            string DocumentDate = string.Empty;
                            if (dtDocumentDate.Text != "")
                            {
                                DocumentDate = dtDocumentDate.Text;
                            }
                            else
                            {
                                DocumentDate = DateTime.MinValue.ToString("MM/dd/yyyy");
                            }
                            //DocumentDate = DocumentDate.Split('/')[1] + "/" + DocumentDate.Split('/')[0] + "/" + DocumentDate.Split('/')[2];
                            DocumentDate = getdata.ConvertDateFormat(DocumentDate);
                            DateTime Document_Date = Convert.ToDateTime(DocumentDate);


                            if (FileUploadCoverLetter.HasFile)
                            {
                                string FileDatetime = DateTime.Now.ToString("dd MMM yyyy hh-mm-ss tt");
                                string sDocumentPath = string.Empty;
                                //sDocumentPath = "~/" + Request.QueryString["ProjectUID"] + "/" + Request.QueryString["DocID"] + "/" + StatusUID + "/" + FileDatetime + "/CoverLetter";
                                sDocumentPath = "~/" + Request.QueryString["ProjectUID"] + "/" + StatusUID + "/CoverLetter";

                                if (!Directory.Exists(Server.MapPath(sDocumentPath)))
                                {
                                    Directory.CreateDirectory(Server.MapPath(sDocumentPath));
                                }

                                string sFileName = Path.GetFileNameWithoutExtension(FileUploadCoverLetter.FileName);
                                string Extn = System.IO.Path.GetExtension(FileUploadCoverLetter.FileName);
                                FileUploadCoverLetter.SaveAs(Server.MapPath(sDocumentPath + "/" + sFileName + "_1_copy" + Extn));
                                string savedPath = sDocumentPath + "/" + sFileName + "_1_copy" + Extn;
                                CoverPagePath = sDocumentPath + "/" + sFileName + "_1" + Extn;
                                getdata.EncryptFile(Server.MapPath(savedPath), Server.MapPath(CoverPagePath));

                            }


                            //
                            //sDate2 = (dtPlannedDate.FindControl("txtDate") as TextBox).Text;
                            //sDate2 = sDate2.Split('/')[1] + "/" + sDate2.Split('/')[0] + "/" + sDate2.Split('/')[2];
                            //CDate2 = Convert.ToDateTime(sDate2);
                            int Cnt = getdata.InsertorUpdateDocumentStatus(StatusUID, new Guid(Request.QueryString["DocID"].ToString()), 1, DDlStatus.SelectedItem.Text, 0, CDate1, DocPath,
                                new Guid(Session["UserUID"].ToString()), txtcomments.Text, DDlStatus.SelectedItem.Text, txtrefNumber.Text, Document_Date, CoverPagePath, RBLDocumentStatusUpdate.SelectedValue, RBLOriginator.SelectedValue);
                            if (Cnt > 0)
                            {
                                //DataSet ds = getdata.getAllUsers();
                                DataSet ds = new DataSet();
                                //if (Session["TypeOfUser"].ToString() == "U" || Session["TypeOfUser"].ToString() == "MD" || Session["TypeOfUser"].ToString() == "VP")
                                //{
                                //    ds = getdata.getAllUsers();
                                //}
                                //else if (Session["TypeOfUser"].ToString() == "PA")
                                //{
                                    //ds = getdata.getUsers_by_ProjectUnder(new Guid(DDlProject.SelectedValue));
                                    ds = getdata.GetUsers_under_ProjectUID(new Guid(Request.QueryString["ProjectUID"]));
                                //}
                                //else
                                //{
                                //    ds = getdata.getUsers_by_ProjectUnder(new Guid(Request.QueryString["ProjectUID"]));
                                //}

                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    string CC = string.Empty;
                                    string ToEmailID = "";
                                    string sUserName = "";
                                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                    {
                                        if (ds.Tables[0].Rows[i]["UserUID"].ToString() == Session["UserUID"].ToString())
                                        {
                                            ToEmailID = ds.Tables[0].Rows[i]["EmailID"].ToString();
                                            sUserName = ds.Tables[0].Rows[i]["UserName"].ToString();
                                        }
                                        else
                                        {
                                            if (getdata.GetUserMailAccess(new Guid(ds.Tables[0].Rows[i]["UserUID"].ToString()), "documentmail") != 0)
                                            {
                                                CC += ds.Tables[0].Rows[i]["EmailID"].ToString() + ",";
                                            }
                                        }
                                    }
                                    CC = CC.TrimEnd(',');
                                    sHtmlString = "<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>" + "<html xmlns='http://www.w3.org/1999/xhtml'>" +
                                      "<head>" + "<meta http-equiv='Content-Type' content='text/html; charset=utf-8' />" + "<style>table, th, td {border: 1px solid black; padding:6px;}</style></head>" +
                                         "<body style='font-family:Verdana, Arial, sans-serif; font-size:12px; font-style:normal;'>";
                                    sHtmlString += "<div style='width:80%; float:left; padding:1%; border:2px solid #011496; border-radius:5px;'>" +
                                                       "<div style='float:left; width:100%; border-bottom:2px solid #011496;'>";
                                    if (WebConfigurationManager.AppSettings["Domain"] == "NJSEI")
                                    {
                                        sHtmlString += "<div style='float:left; width:7%;'><img src='https://dm.njsei.com/_assets/images/NJSEI%20Logo.jpg' width='50' /></div>";
                                    }
                                    else
                                    {
                                        sHtmlString += "<div style='float:left; width:7%;'><h2>" + WebConfigurationManager.AppSettings["Domain"] + "</h2></div>";
                                    }
                                    sHtmlString += "<div style='float:left; width:70%;'><h2 style='margin-top:10px;'>Project Monitoring Tool</h2></div>" +
                                               "</div>";
                                    sHtmlString += "<div style='width:100%; float:left;'><br/>Dear Users,<br/><br/><span style='font-weight:bold;'>" + sUserName + " has changed " + DDLDocument.SelectedItem.Text + " status.</span> <br/><br/></div>";
                                    sHtmlString += "<div style='width:100%; float:left;'><table style='width:100%;'>" +
                                                    "<tr><td><b>Document Name </b></td><td style='text-align:center;'><b>:</b></td><td>" + DDLDocument.SelectedItem.Text + "</td></tr>" +
                                                    "<tr><td><b>Status </b></td><td style='text-align:center;'><b>:</b></td><td>" + DDlStatus.SelectedItem.Text + "</td></tr>" +
                                                    "<tr><td><b>Ref. Number </b></td><td style='text-align:center;'><b>:</b></td><td>" + txtrefNumber.Text + "</td></tr>" +
                                                    "<tr><td><b>Date </b></td><td style='text-align:center;'><b>:</b></td><td>" + CDate1.ToString("dd MMM yyyy") + "</td></tr>" +
                                                    "<tr><td><b>Comments </b></td><td style='text-align:center;'><b>:</b></td><td>" + txtcomments.Text + "</td></tr>";
                                    sHtmlString += "</table></div>";
                                    sHtmlString += "<div style='width:100%; float:left;'><br/><br/>Sincerely, <br/> Project Monitoring Tool.</div></div></body></html>";

                                    //sHtmlString = "<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>" + "<html xmlns='http://www.w3.org/1999/xhtml'>" +
                                    //       "<head>" + "<meta http-equiv='Content-Type' content='text/html; charset=utf-8' />" + "</head>" +
                                    //          "<body style='font-family:Verdana, Arial, sans-serif; font-size:12px; font-style:normal;'>";
                                    //sHtmlString += "<div style='float:left; width:100%; height:30px;'>" +
                                    //                   "Dear, " + "Users" +
                                    //                   "<br/><br/></div>";
                                    //sHtmlString += "<div style='width:100%; float:left;'><div style='width:100%; float:left;'>Below are the Status details. <br/><br/></div>";
                                    //sHtmlString += "<div style='width:100%; float:left;'><div style='width:100%; float:left;'>Document Name : " + DDLDocument.SelectedItem.Text + "<br/></div>";
                                    //sHtmlString += "<div style='width:100%; float:left;'><div style='width:100%; float:left;'>Status : " + DDlStatus.SelectedItem.Text + "<br/></div>";
                                    //sHtmlString += "<div style='width:100%; float:left;'><div style='width:100%; float:left;'>Date : " + CDate1.ToShortDateString() + "<br/></div>";
                                    //sHtmlString += "<div style='width:100%; float:left;'><br/><br/>Sincerely, <br/> Project Manager.</div></div></body></html>";
                                    //string ret = getdata.SendMail(ds.Tables[0].Rows[0]["EmailID"].ToString(), Subject, sHtmlString, CC, Server.MapPath(DocPath));
                                    // added on 02/11/2020
                                    DataTable dtemailCred = getdata.GetEmailCredentials();
                                    Guid MailUID = Guid.NewGuid();
                                    getdata.StoreEmaildataToMailQueue(MailUID, new Guid(Session["UserUID"].ToString()), dtemailCred.Rows[0][0].ToString(), ToEmailID, Subject, sHtmlString, CC, "");
                                    //
                                    // added on 07/01/2022 for sending mail to next user in line to change status...
                                    DataSet dsnew = getdata.getTop1_DocumentStatusSelect(new Guid(Request.QueryString["DocID"]));
                                    DataSet dsNext = getdata.GetNextStep_By_DocumentUID(new Guid(Request.QueryString["DocID"]), dsnew.Tables[0].Rows[0]["ActivityType"].ToString());

                                    sHtmlString = "<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>" + "<html xmlns='http://www.w3.org/1999/xhtml'>" +
                                    "<head>" + "<meta http-equiv='Content-Type' content='text/html; charset=utf-8' />" + "<style>table, th, td {border: 1px solid black; padding:6px;}</style></head>" +
                                       "<body style='font-family:Verdana, Arial, sans-serif; font-size:12px; font-style:normal;'>";
                                    sHtmlString += "<div style='width:80%; float:left; padding:1%; border:2px solid #011496; border-radius:5px;'>" +
                                                       "<div style='float:left; width:100%; border-bottom:2px solid #011496;'>";
                                    if (WebConfigurationManager.AppSettings["Domain"] == "NJSEI")
                                    {
                                        sHtmlString += "<div style='float:left; width:7%;'><img src='https://dm.njsei.com/_assets/images/NJSEI%20Logo.jpg' width='50' /></div>";
                                    }
                                    else
                                    {
                                        sHtmlString += "<div style='float:left; width:7%;'><h2>" + WebConfigurationManager.AppSettings["Domain"] + "</h2></div>";
                                    }
                                    sHtmlString += "<div style='float:left; width:70%;'><h2 style='margin-top:10px;'>Project Monitoring Tool</h2></div>" +
                                               "</div>";
                                    sHtmlString += "<div style='width:100%; float:left;'><br/>Dear Users,<br/><br/><span style='font-weight:bold;'>" + sUserName + " has changed " + DDLDocument.SelectedItem.Text + " status.</span> <br/><br/></div>";
                                    sHtmlString += "<div style='width:100%; float:left;'><table style='width:100%;'>" +
                                                    "<tr><td><b>Document Name </b></td><td style='text-align:center;'><b>:</b></td><td>" + DDLDocument.SelectedItem.Text + "</td></tr>" +
                                                    "<tr><td><b>Status </b></td><td style='text-align:center;'><b>:</b></td><td>" + DDlStatus.SelectedItem.Text + "</td></tr>" +
                                                    "<tr><td><b>Ref. Number </b></td><td style='text-align:center;'><b>:</b></td><td>" + txtrefNumber.Text + "</td></tr>" +
                                                    "<tr><td><b>Date </b></td><td style='text-align:center;'><b>:</b></td><td>" + CDate1.ToString("dd MMM yyyy") + "</td></tr>" +
                                                    "<tr><td><b>Comments </b></td><td style='text-align:center;'><b>:</b></td><td>" + txtcomments.Text + "</td></tr>";
                                    sHtmlString += "</table><br /><br /><div style='color: red'>Kindly note that you are to act on this to complete the next step in document flow.</div></div>";
                                    sHtmlString += "<div style='width:100%; float:left;'><br/><br/>Sincerely, <br/> Project Monitoring Tool.</div></div></body></html>";
                                    Subject = Subject + ".Kindly complete the next step !";
                                    string next = string.Empty;
                                    foreach (DataRow dr in dsNext.Tables[0].Rows)
                                    {
                                        string NextUser = getdata.GetNextUser_By_DocumentUID(new Guid(Request.QueryString["DocID"]), int.Parse(dr["ForFlow_Step"].ToString()));
                                        if (!string.IsNullOrEmpty(NextUser))
                                        {
                                            ToEmailID = getdata.GetUserEmail_By_UserUID_New(new Guid(NextUser));
                                            if (ToEmailID != next)
                                            {
                                                
                                                getdata.StoreEmaildataToMailQueue(Guid.NewGuid(), new Guid(Session["UserUID"].ToString()), dtemailCred.Rows[0][0].ToString(), ToEmailID, Subject, sHtmlString, "", "");
                                                next = ToEmailID;
                                            }
                                           
                                        }
                                    }

                                }
                                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
                            }
                            else
                            {

                                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error: There is a problem with this feature. Please contact system admin');</script>");

                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code : ADS 12 - There is a problem with this feature. Please contact system admin');</script>");
                    }
                }
            }
        }

        private void BindOriginator()
        {
            DataSet ds = getdata.GetOriginatorMaster();
            RBLOriginator.DataTextField = "Originator_Name";
            RBLOriginator.DataValueField = "Originator_Name";
            RBLOriginator.DataSource = ds;
            RBLOriginator.DataBind();
            if (ds.Tables[0].Rows.Count > 0)
            {
                RBLOriginator.Items[0].Selected = true;
            }
        }
    }
}