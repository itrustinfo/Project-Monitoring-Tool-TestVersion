using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagementTool._modal_pages
{
    public partial class view_documentsentdetails : System.Web.UI.Page
    {
        DBGetData getdt = new DBGetData();
        GeneralDocuments GD = new GeneralDocuments();
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
                    if (Request.QueryString["DocumentUID"] != null)
                    {
                        BindDocumentSentGrid();
                    }
                    
                }
            }
        }
        private void BindDocumentSentGrid()
        {
            DataSet ds = getdt.GetDocumentSent_by_DocumentUID(new Guid(Request.QueryString["DocumentUID"]));
            GrdDocumentSentDetails.DataSource = ds;
            GrdDocumentSentDetails.DataBind();
        }
        protected void GrdDocumentSentDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdDocumentSentDetails.PageIndex = e.NewPageIndex;
            BindDocumentSentGrid();
        }

        public string GetDocumentName(string DocumentUID)
        {
            string DocName= getdt.ActualDocumentName_By_ActualDocumentUID(new Guid(DocumentUID));
            if (DocName == "")
            {
                DocName= GD.GetGeneralDocumentNameByUID(new Guid(DocumentUID));
            }
            return DocName;
        }

        public string GetUserName(string UserUID)
        {
            return getdt.getUserNameby_UID(new Guid(UserUID));
        }
    }
}