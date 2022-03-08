using ProjectManager.DAL;
using System;
using System.Collections.Generic;
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
    public partial class edit_document : System.Web.UI.Page
    {
        DBGetData getdata = new DBGetData();
        TaskUpdate TKUpdate = new TaskUpdate();
        protected void Page_Load(object sender, EventArgs e)
        {
            this.UnobtrusiveValidationMode = System.Web.UI.UnobtrusiveValidationMode.None;
            if (!IsPostBack)
            {
                if (Request.QueryString["DocID"] != null)
                {
                    BindOriginator();
                    BindDocument();

                    if (Request.QueryString["fID"] != null)
                    {
                        DataSet dsFlow = getdata.GetDocumentFlows_by_UID(new Guid(Request.QueryString["fID"]));
                        if (dsFlow.Tables[0].Rows.Count > 0)
                        {
                            if (dsFlow.Tables[0].Rows[0]["Steps_Count"].ToString() == "-1")
                            {
                                Originator.Visible = false;
                                CoverLetterUpload.Visible = false;
                                OriginatorNumber.Visible = false;
                                DocumentDate.Visible = false;
                                IncomingReceDate.Visible = false;
                                PrjRefNumber.Visible = false;
                                DocumentMedia.Visible = false;
                                FileRefNumber.Visible = false;
                                DataSet dsDoc = getdata.getDocumentsbyDocID(new Guid(Request.QueryString["dUID"]));
                                if (dsDoc.Tables[0].Rows.Count > 0)
                                {
                                    dtDocumentDate.Text = Convert.ToDateTime(dsDoc.Tables[0].Rows[0]["FlowStep1_TargetDate"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                                    dtIncomingRevDate.Text = Convert.ToDateTime(dsDoc.Tables[0].Rows[0]["FlowStep1_TargetDate"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                                }
                            }
                        }
                    }
                }
            }
        }

        protected void BindDocument()
        {
            DataSet ds = getdata.ActualDocuments_SelectBy_ActualDocumentUID(new Guid(Request.QueryString["DocID"].ToString()));
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtDocumentName.Text = ds.Tables[0].Rows[0]["ActualDocument_Name"].ToString();
                txtdesc.Text = ds.Tables[0].Rows[0]["Description"].ToString();
                txtRefNumber.Text = ds.Tables[0].Rows[0]["Ref_Number"].ToString();
                txtprefnumber.Text = ds.Tables[0].Rows[0]["ProjectRef_Number"].ToString();
                HiddenProjectUID.Value = ds.Tables[0].Rows[0]["ProjectUID"].ToString();
                if (ds.Tables[0].Rows[0]["IncomingRec_Date"].ToString() != null && ds.Tables[0].Rows[0]["IncomingRec_Date"].ToString() != "")
                {
                    dtIncomingRevDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["IncomingRec_Date"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                DDLDocumentMedia.Items[0].Selected = ds.Tables[0].Rows[0]["Media_HC"].ToString() == "true" ? true : false;
                DDLDocumentMedia.Items[1].Selected = ds.Tables[0].Rows[0]["Media_SC"].ToString() == "true" ? true : false;
                DDLDocumentMedia.Items[2].Selected = ds.Tables[0].Rows[0]["Media_SCEF"].ToString() == "true" ? true : false;
                DDLDocumentMedia.Items[3].Selected = ds.Tables[0].Rows[0]["Media_HCR"].ToString() == "true" ? true : false;
                DDLDocumentMedia.Items[4].Selected = ds.Tables[0].Rows[0]["Media_SCR"].ToString() == "true" ? true : false;
                DDLDocumentMedia.Items[5].Selected = ds.Tables[0].Rows[0]["Media_NA"].ToString() == "true" ? true : false;
                txtFileRefNumber.Text = ds.Tables[0].Rows[0]["FileRef_Number"].ToString();
                txtremarks.Text = ds.Tables[0].Rows[0]["Remarks"].ToString();
                if (ds.Tables[0].Rows[0]["Document_Date"].ToString() != null && ds.Tables[0].Rows[0]["Document_Date"].ToString() != "")
                {
                    dtDocumentDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["Document_Date"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                if (ds.Tables[0].Rows[0]["Doc_Type"].ToString() == "Cover Letter")
                {
                    Originator.Visible = true;
                    OriginatorNumber.Visible = true;
                    DocumentDate.Visible = true;
                    CoverLetterUpload.Visible = true;
                    HiddenField1.Value = ds.Tables[0].Rows[0]["Doc_Type"].ToString();
                    RBLOriginator.SelectedValue = ds.Tables[0].Rows[0]["ActualDocument_Originator"].ToString();
                    
                }
                else if (ds.Tables[0].Rows[0]["Doc_Type"].ToString() == "Document")
                {
                    HiddenField1.Value = ds.Tables[0].Rows[0]["Doc_Type"].ToString();
                    RBLOriginator.SelectedValue = ds.Tables[0].Rows[0]["ActualDocument_Originator"].ToString();
                    Originator.Visible = true;
                    DocumentDate.Visible = true;
                    OriginatorNumber.Visible = true;
                    CoverLetterUpload.Visible = true;
                    //if (ds.Tables[0].Rows[0]["Document_Date"].ToString() != null && ds.Tables[0].Rows[0]["Document_Date"].ToString() != "")
                    //{
                    //    dtDocumentDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["Document_Date"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    //}
                }
                else
                {
                    Originator.Visible = false;
                    OriginatorNumber.Visible = false;
                    DocumentDate.Visible = true;
                    CoverLetterUpload.Visible = false;
                }

                if (RBLOriginator.SelectedValue == "Contractor")
                {
                    LblDateText.Text = "Incoming Recv. Date";
                }
                else if (RBLOriginator.SelectedValue == "JUIDCo")
                {
                    LblDateText.Text = "Letter Dated";
                }
                else if (RBLOriginator.SelectedValue == "NJSEI")
                {
                    LblDateText.Text = "Reply Date";
                }
                else
                {
                    LblDateText.Text = "Incoming Recv. Date";
                }
            }
        }

        private void BindOriginator()
        {
            if (WebConfigurationManager.AppSettings["Domain"] == "ONTB" || WebConfigurationManager.AppSettings["Domain"] == "LNT" || WebConfigurationManager.AppSettings["Domain"] == "Suez")
            {
                RBLOriginator.Items.Insert(0, new ListItem("Contractor", "Contractor"));
                RBLOriginator.Items.Insert(1, new ListItem("ONTB", "ONTB"));
                RBLOriginator.Items.Insert(2, new ListItem("BWSSB", "BWSSB"));
                RBLOriginator.Items.Insert(3, new ListItem("Others", "Others"));
                
            }
            else
            {
                DataSet ds = getdata.GetOriginatorMaster();
                RBLOriginator.DataTextField = "Originator_Name";
                RBLOriginator.DataValueField = "Originator_Name";
                RBLOriginator.DataSource = ds;
                RBLOriginator.DataBind();
                // added by zuber on 22/02/2022 for KIADB
                if (getdata.GetClientCodebyWorkpackageUID(new Guid(Request.QueryString["wUID"].ToString())) != "")
                {
                    RBLOriginator.Items.Insert(0, new ListItem(getdata.GetClientCodebyWorkpackageUID(new Guid(Request.QueryString["wUID"].ToString())), getdata.GetClientCodebyWorkpackageUID(new Guid(Request.QueryString["wUID"].ToString()))));
                }

                if (ds.Tables[0].Rows.Count > 0)
                {
                    RBLOriginator.Items[0].Selected = true;
                }
            }




        }
        //private void BindOriginator()
        //{
        //    DataSet ds = getdata.GetOriginatorMaster();
        //    RBLOriginator.DataTextField = "Originator_Name";
        //    RBLOriginator.DataValueField = "Originator_Name";
        //    RBLOriginator.DataSource = ds;
        //    RBLOriginator.DataBind();
        //}

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    Guid sDocumentUID = new Guid();
                    if (Request.QueryString["DocID"] != null)
                    {
                        sDocumentUID = new Guid(Request.QueryString["DocID"]);
                    }

                    Guid TaskUID = Guid.Empty;
                    Guid projectId = Guid.Empty;
                    Guid workpackageid = Guid.Empty;

                    string sIncomingRevDate = "";
                    DateTime IRD = DateTime.Now;
                    if (dtIncomingRevDate.Text != "")
                    {
                        sIncomingRevDate = dtIncomingRevDate.Text;
                        //sIncomingRevDate = sIncomingRevDate.Split('/')[1] + "/" + sIncomingRevDate.Split('/')[0] + "/" + sIncomingRevDate.Split('/')[2];
                        sIncomingRevDate = getdata.ConvertDateFormat(sIncomingRevDate);
                        IRD = Convert.ToDateTime(sIncomingRevDate);
                    }

                    int result = 0;
                    string Originator = string.Empty;
                    string DocumentDate = string.Empty;
                    if (HiddenField1.Value == "Cover Letter" || HiddenField1.Value == "Document")
                    {
                        Originator = RBLOriginator.SelectedValue;
                        if (dtDocumentDate.Text != "")
                        {
                            DocumentDate = dtDocumentDate.Text;
                        }
                        else
                        {
                            DocumentDate = DateTime.MinValue.ToString("dd/MM/yyyy");
                        }
                    }
                    else
                    {
                        if (dtDocumentDate.Text != "")
                        {
                            DocumentDate = dtDocumentDate.Text;
                        }
                        else
                        {
                            DocumentDate = DateTime.MinValue.ToString("dd/MM/yyyy");
                        }
                    }

                    //DocumentDate = DocumentDate.Split('/')[1] + "/" + DocumentDate.Split('/')[0] + "/" + DocumentDate.Split('/')[2];
                    DocumentDate = getdata.ConvertDateFormat(DocumentDate);
                    DateTime Document_Date = Convert.ToDateTime(DocumentDate);

                    string sDocumentPath = string.Empty;
                    string CoverPagePath = string.Empty;
                    if (FileUploadCoverPage.HasFile)
                    {
                        string Extn = System.IO.Path.GetExtension(FileUploadCoverPage.FileName);
                        if (Extn.ToLower() != ".exe" && Extn.ToLower() != ".msi" && Extn.ToLower() != ".db")
                        {
                            sDocumentPath = "~/" + HiddenProjectUID.Value + "/CoverLetter";

                            if (!Directory.Exists(Server.MapPath(sDocumentPath)))
                            {
                                Directory.CreateDirectory(Server.MapPath(sDocumentPath));
                            }

                            string sFileName = Path.GetFileNameWithoutExtension(FileUploadCoverPage.FileName);

                            FileUploadCoverPage.SaveAs(Server.MapPath(sDocumentPath + "/" + sFileName + "_1_copy" + Extn));

                            string savedPath = sDocumentPath + "/" + sFileName + "_1_copy" + Extn;
                            CoverPagePath = sDocumentPath + "/" + sFileName + "_1" + Extn;
                            getdata.EncryptFile(Server.MapPath(savedPath), Server.MapPath(CoverPagePath));

                            try
                            {
                                if (File.Exists(Server.MapPath(savedPath)))
                                {
                                    File.Delete(Server.MapPath(savedPath));
                                }
                            }
                            catch (Exception ex)
                            {
                                //throw
                            }
                        }
                    }
                        result = getdata.Document_Update(sDocumentUID, txtprefnumber.Text, txtRefNumber.Text, IRD, txtDocumentName.Text, txtdesc.Text,
                        DDLDocumentMedia.Items[0].Selected == true ? "true" : "false", DDLDocumentMedia.Items[1].Selected == true ? "true" : "false",
                        DDLDocumentMedia.Items[2].Selected == true ? "true" : "false", DDLDocumentMedia.Items[3].Selected == true ? "true" : "false",
                        DDLDocumentMedia.Items[4].Selected == true ? "true" : "false", DDLDocumentMedia.Items[5].Selected == true ? "true" : "false",
                        txtremarks.Text, txtFileRefNumber.Text, Originator, Document_Date, CoverPagePath);
                    if (result > 0)
                    {

                        //Session["SelectedTaskUID1"] = sDocumentUID;
                        Session["SelectedTaskUID1"] = getdata.GetSubmittalUID_By_ActualDocumentUID(sDocumentUID);
                        Session["ViewDocBy"] = "Activity";

                        if (Request.QueryString["type"] != null)
                        {
                            Session["searchedit"] = "true";
                            Session["PUID"] = Request.QueryString["pUID"].ToString();
                            Session["WUID"] = Request.QueryString["wUID"].ToString();
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href + '?type=search';</script>");
                        }
                        else
                        {
                            
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
                        }
                    }
                    else
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('There is problem with this feature. Contact Administrator.');</script>");
                    }
                }
                catch (Exception ex)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "Warning", "<script language='javascript'>alert('Error: " + ex.Message + "');</script>");
                }

            }
        }

        protected void RBLOriginator_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RBLOriginator.SelectedValue == "Contractor")
            {
                LblDateText.Text = "Incoming Recv. Date";
            }
            else if (RBLOriginator.SelectedValue == "BWSSB")
            {
                LblDateText.Text = "Letter Dated";
            }
            else
            {
                LblDateText.Text = "Reply Date";
            }
        }
    }
}