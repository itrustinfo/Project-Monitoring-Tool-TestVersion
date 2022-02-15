using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProjectManager.DAL;

namespace ProjectManagementTool._modal_pages
{
    public partial class view_documentdetails : System.Web.UI.Page
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
                    BindDocument();
                }
            }
        }

        protected void BindDocument()
        {
            DataSet ds = getdata.ActualDocuments_SelectBy_ActualDocumentUID(new Guid(Request.QueryString["DocID"].ToString()));
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtDocumentName.Text = ds.Tables[0].Rows[0]["ActualDocument_Name"].ToString();
                txtDescription.Text = ds.Tables[0].Rows[0]["Description"].ToString();
                lblOrgRefNo.Text = ds.Tables[0].Rows[0]["Ref_Number"].ToString();
                lblReferenceNo.Text = ds.Tables[0].Rows[0]["ProjectRef_Number"].ToString();

                if (ds.Tables[0].Rows[0]["IncomingRec_Date"].ToString() != null && ds.Tables[0].Rows[0]["IncomingRec_Date"].ToString() != "")
                {
                    lblInDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["IncomingRec_Date"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                DDLDocumentMedia.Items[0].Selected = ds.Tables[0].Rows[0]["Media_HC"].ToString() == "true" ? true : false;
                DDLDocumentMedia.Items[1].Selected = ds.Tables[0].Rows[0]["Media_SC"].ToString() == "true" ? true : false;
                DDLDocumentMedia.Items[2].Selected = ds.Tables[0].Rows[0]["Media_SCEF"].ToString() == "true" ? true : false;
                DDLDocumentMedia.Items[3].Selected = ds.Tables[0].Rows[0]["Media_HCR"].ToString() == "true" ? true : false;
                DDLDocumentMedia.Items[4].Selected = ds.Tables[0].Rows[0]["Media_SCR"].ToString() == "true" ? true : false;
                DDLDocumentMedia.Items[5].Selected = ds.Tables[0].Rows[0]["Media_NA"].ToString() == "true" ? true : false;
                lblfileRefNo.Text = ds.Tables[0].Rows[0]["FileRef_Number"].ToString();
                txtRemarks.Text = ds.Tables[0].Rows[0]["Remarks"].ToString();
                if (ds.Tables[0].Rows[0]["Doc_Type"].ToString() == "Cover Letter")
                {
                    Originator.Visible = true;
                    OriginatorNumber.Visible = true;
                    DocumentDate.Visible = true;
                   
                    lblOriginator.Text= ds.Tables[0].Rows[0]["ActualDocument_Originator"].ToString();
                    if (ds.Tables[0].Rows[0]["Document_Date"].ToString() != null && ds.Tables[0].Rows[0]["Document_Date"].ToString() != "")
                    {
                        lblCoverDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["Document_Date"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    }
                }
                else if (ds.Tables[0].Rows[0]["Doc_Type"].ToString() == "Document")
                {

                    lblOriginator.Text = ds.Tables[0].Rows[0]["ActualDocument_Originator"].ToString();
                    Originator.Visible = true;
                    DocumentDate.Visible = false;
                    OriginatorNumber.Visible = true;
                }
                else
                {
                    Originator.Visible = false;
                    OriginatorNumber.Visible = false;
                    DocumentDate.Visible = false;
                }

                if (lblOriginator.Text == "Contractor")
                {
                    LblDateText.Text = "Incoming Recv. Date";
                }
                else if (lblOriginator.Text == "JUIDCo")
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
}