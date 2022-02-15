using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace ProjectManagementTool._modal_pages
{
    public partial class copied_documents : System.Web.UI.Page
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
                    if (Session["copydocument"] != null)
                    {
                        BindCopiedDocuments();
                    }
                    else
                    {
                        GrdCopiedDocumentList.DataSource = null;
                        GrdCopiedDocumentList.DataBind();
                    }
                }
            }
        }

        private void BindCopiedDocuments()
        {
            getdata.sfinallist = (List<CopyDocumentFile>)Session["copydocument"];
            GrdCopiedDocumentList.DataSource = getdata.sfinallist;
            GrdCopiedDocumentList.DataBind();
            if (Session["CopiedActivity"] != null)
            {
                Session["SelectedTaskUID1"] = Session["CopiedActivity"];
                Session["ViewDocBy"] = "Activity";
            }
            if (getdata.sfinallist.Count<=0)
            {
                Session["copydocument"] = null;
                
                
                //Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "HideCopyFiles()", true);
                //HtmlAnchor LnkCopiedFiles= ((HtmlAnchor)PreviousPage.FindControl("CopyDocument"));
                //LnkCopiedFiles.Visible = false;
                //Label LblCopiedFiles= ((Label)PreviousPage.FindControl("LblcopyFileCount"));
                //LblCopiedFiles.Visible = false;
            }
        }

        protected void GrdCopiedDocumentList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdCopiedDocumentList.PageIndex = e.NewPageIndex;
            BindCopiedDocuments();
        }
        public string GetDocumentName(string DocumentUID)
        {
            return getdata.GetActualDocumentName_by_ActualDocumentUID(new Guid(DocumentUID));
        }

        public string GetSubmittalName(string DocumentUID)
        {
            return getdata.GetSubmittalName_by_ActualDocumentUID(new Guid(DocumentUID));
        }

        protected void GrdCopiedDocumentList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string UID = e.CommandArgument.ToString();
            if (e.CommandName == "delete")
            {
                getdata.sfinallist = (List<CopyDocumentFile>)Session["copydocument"];
                var itemToRemove = getdata.sfinallist.Single(r => r.DocumentUID == new Guid(UID));
                if (itemToRemove != null)
                {
                    getdata.sfinallist.Remove(itemToRemove);
                    Session["copydocument"]= getdata.sfinallist;
                    BindCopiedDocuments();
                }
            }
        }

        protected void GrdCopiedDocumentList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }
    }
}