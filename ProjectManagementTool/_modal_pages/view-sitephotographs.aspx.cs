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
    public partial class view_sitephotographs : System.Web.UI.Page
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
                    if (Request.QueryString["WorkPackage"] != null)
                    {
                        BindSitePhotographs();
                    }
                }
            }
        }

        private void BindSitePhotographs()
        {
            DataSet ds = getdata.GetSitePhotographs_by_WorkpackageUID(new Guid(Request.QueryString["WorkPackage"]));
            GrdSitePhotograph.DataSource = ds;
            GrdSitePhotograph.DataBind();

            if (ds.Tables[0].Rows.Count > 0)
            {
                LblMessage.Visible = false;
            }
            else
            {
                LblMessage.Visible = true;
            }
        }

        protected void GrdSitePhotograph_DeleteCommand(object source, DataListCommandEventArgs e)
        {
            string UID = GrdSitePhotograph.DataKeys[e.Item.ItemIndex].ToString();
            int cnt = getdata.SitePhotoGraphs_Delete(new Guid(UID), new Guid(Session["UserUID"].ToString()));
            if (cnt > 0)
            {
                BindSitePhotographs();
            }
        }
    }
}