using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagementTool._modal_pages
{
    public partial class mail_documentlink : System.Web.UI.Page
    {
        DBGetData getdata = new DBGetData();
        GeneralDocuments GD = new GeneralDocuments();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Username"] == null)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
            }
            else
            {
                if (Request.QueryString["DocID"] == null && Request.QueryString["GeneralDocumentUID"] ==null)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (Request.QueryString["DocID"] != null)
                {
                    MailActualDocuments();
                }
                else if (Request.QueryString["GeneralDocumentUID"] != null)
                {
                    MailGeneralDocuments();
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "Error", "<script language='javascript'>alert('There is a Problem with this feature. Please contact systtem admin. Desc : " + ex.Message + "');</script>");
            }
        }
        private void MailActualDocuments()
        {
            DataSet ds = getdata.ActualDocuments_SelectBy_ActualDocumentUID(new Guid(Request.QueryString["DocID"]));
            if (ds.Tables[0].Rows.Count > 0)
            {
                string CC = txtemail.Text;

                string sHtmlString = string.Empty;
                CC = CC.TrimEnd(',');

                DataSet dsUser = getdata.getUserDetails(new Guid(Session["UserUID"].ToString()));
                if (dsUser.Tables[0].Rows.Count > 0)
                {
                    string Subject = dsUser.Tables[0].Rows[0]["UserName"].ToString() + " shared a document";
                    string ToEmailID = CC;// dsUser.Tables[0].Rows[0]["EmailID"].ToString();
                    CC = dsUser.Tables[0].Rows[0]["EmailID"].ToString();
                    sHtmlString = "<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>" + "<html xmlns='http://www.w3.org/1999/xhtml'>" +
                                       "<head>" + "<meta http-equiv='Content-Type' content='text/html; charset=utf-8' />" + "<style>table, th, td {border: 1px solid black; padding:6px;}</style></head>" +
                                          "<body style='font-family:Verdana, Arial, sans-serif; font-size:12px; font-style:normal;'>";
                    sHtmlString += "<div style='width:80%; float:left; padding:1%; border:2px solid #011496; border-radius:5px;'>" +
                                       "<div style='float:left; width:100%; border-bottom:2px solid #011496;'>";
                    if (WebConfigurationManager.AppSettings["Domain"] == "NJSEI")
                    {
                        sHtmlString += "<div style='float:left; width:7%;'><img src='https://njspm.itrustinfo.com/_assets/images/NJSEI%20Logo.jpg' width='50' /></div>";
                    }
                    else
                    {
                        sHtmlString += "<div style='float:left; width:7%;'><h2>ONTB</h2></div>";
                    }
                    sHtmlString += "<div style='float:left; width:70%;'><h2 style='margin-top:10px;'>Project Monitoring Tool</h2></div>" +
                                       "</div>";
                    sHtmlString += "<div style='width:100%; float:left;'><br/>Dear All,<br/><br/><span style='font-weight:bold;'>" + dsUser.Tables[0].Rows[0]["UserName"].ToString() + " has shared a document with you. Click on below link to download the document.</span> <br/><br/></div>";
                    sHtmlString += "<div style='width:100%; float:left;'>";
                    sHtmlString += "<a href='" + WebConfigurationManager.AppSettings["SiteName"] + "download-document.aspx?key=GAAHAJKHGSHHDH237HSBHJFFLMNVASERWYWB242VFSGDD8765NJMADWEWDJDJDJFNB12GAHSBMLKOUFFHFXDGZ&DocumentUID=" + Request.QueryString["DocID"] + "&Ticks=" + DateTime.Now.Ticks + "' target='_blank'>Download Here</a>";
                    sHtmlString += "</div>";
                    sHtmlString += "<div style='width:100%; float:left;'><br/><br/>Sincerely, <br/> MIS System.</div></div></body></html>";
                    DataTable dtemailCred = getdata.GetEmailCredentials();
                    Guid MailUID = Guid.NewGuid();
                    getdata.StoreEmaildataToMailQueue(MailUID, new Guid(Session["UserUID"].ToString()), dtemailCred.Rows[0][0].ToString(), ToEmailID, Subject, sHtmlString, CC, "");
                    int cntinsert = getdata.DocumentSent_Insert(Guid.NewGuid(), new Guid(Request.QueryString["DocID"]), new Guid(Session["UserUID"].ToString()), CC);
                    if (cntinsert > 0)
                    {
                        Session["SelectedTaskUID1"] = ds.Tables[0].Rows[0]["DocumentUID"].ToString();
                        Session["ViewDocBy"] = "Activity";

                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
                    }
                    else
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "Error", "<script language='javascript'>alert('There is a Problem with this feature. Please contact systtem admin.');</script>");
                    }
                }
            }
        }

        private void MailGeneralDocuments()
        {
            DataSet ds = GD.GeneralDocuments_SelectBy_GeneralDocumentUID(new Guid(Request.QueryString["GeneralDocumentUID"]));
            if (ds.Tables[0].Rows.Count > 0)
            {
                string CC = txtemail.Text;

                string sHtmlString = string.Empty;
                CC = CC.TrimEnd(',');

                DataSet dsUser = getdata.getUserDetails(new Guid(Session["UserUID"].ToString()));
                if (dsUser.Tables[0].Rows.Count > 0)
                {
                    string Subject = dsUser.Tables[0].Rows[0]["UserName"].ToString() + " shared a document";
                    string ToEmailID = CC;// dsUser.Tables[0].Rows[0]["EmailID"].ToString();
                    CC = dsUser.Tables[0].Rows[0]["EmailID"].ToString();
                    sHtmlString = "<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>" + "<html xmlns='http://www.w3.org/1999/xhtml'>" +
                                       "<head>" + "<meta http-equiv='Content-Type' content='text/html; charset=utf-8' />" + "<style>table, th, td {border: 1px solid black; padding:6px;}</style></head>" +
                                          "<body style='font-family:Verdana, Arial, sans-serif; font-size:12px; font-style:normal;'>";
                    sHtmlString += "<div style='width:80%; float:left; padding:1%; border:2px solid #011496; border-radius:5px;'>" +
                                       "<div style='float:left; width:100%; border-bottom:2px solid #011496;'>";
                    if (WebConfigurationManager.AppSettings["Domain"] == "NJSEI")
                    {
                        sHtmlString += "<div style='float:left; width:7%;'><img src='https://njspm.itrustinfo.com/_assets/images/NJSEI%20Logo.jpg' width='50' /></div>";
                    }
                    else
                    {
                        sHtmlString += "<div style='float:left; width:7%;'><h2>ONTB</h2></div>";
                    }
                    sHtmlString += "<div style='float:left; width:70%;'><h2 style='margin-top:10px;'>Project Monitoring Tool</h2></div>" +
                                       "</div>";
                    sHtmlString += "<div style='width:100%; float:left;'><br/>Dear All,<br/><br/><span style='font-weight:bold;'>" + dsUser.Tables[0].Rows[0]["UserName"].ToString() + " has shared a document with you. Click on below link to download the document.</span> <br/><br/></div>";
                    sHtmlString += "<div style='width:100%; float:left;'>";
                    sHtmlString += "<a href='" + WebConfigurationManager.AppSettings["SiteName"] + "download-document.aspx?key=GAAHAJKHGSHHDH237HSBHJFFLMNVASERWYWB242VFSGDD8765NJMADWEWDJDJDJFNB12GAHSBMLKOUFFHFXDGZ&GeneralDocumentUID=" + Request.QueryString["GeneralDocumentUID"] + "&Ticks=" + DateTime.Now.Ticks + "' target='_blank'>Download Here</a>";
                    sHtmlString += "</div>";
                    sHtmlString += "<div style='width:100%; float:left;'><br/><br/>Sincerely, <br/> MIS System.</div></div></body></html>";
                    DataTable dtemailCred = getdata.GetEmailCredentials();
                    Guid MailUID = Guid.NewGuid();
                    getdata.StoreEmaildataToMailQueue(MailUID, new Guid(Session["UserUID"].ToString()), dtemailCred.Rows[0][0].ToString(), ToEmailID, Subject, sHtmlString, CC, "");

                    int cntinsert = getdata.DocumentSent_Insert(Guid.NewGuid(), new Guid(Request.QueryString["GeneralDocumentUID"]), new Guid(Session["UserUID"].ToString()), CC);
                    if (cntinsert > 0)
                    {
                        Session["GeneralDocumentStructureUID"] = ds.Tables[0].Rows[0]["StructureUID"].ToString();

                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
                    }
                    else
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "Error", "<script language='javascript'>alert('There is a Problem with this feature. Please contact systtem admin.');</script>");
                    }
                    
                }
            }
        }
    }
}