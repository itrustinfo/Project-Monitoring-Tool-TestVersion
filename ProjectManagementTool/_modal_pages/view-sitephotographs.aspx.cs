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
            DataSet ds = new DataSet();
            if(string.IsNullOrEmpty(txtFromDate.Text) && string.IsNullOrEmpty(txtToDate.Text) && string.IsNullOrEmpty(txtDescription.Text) )
                ds = getdata.GetSitePhotographs_by_WorkpackageUID(new Guid(Request.QueryString["WorkPackage"]));
            else
            {
                DateTime fromDate = Convert.ToDateTime("01-Jan-1900");
                DateTime toDate = Convert.ToDateTime("01-Jan-2099");

                if (!string.IsNullOrEmpty(txtFromDate.Text))
                {
                    if(!DateTime.TryParse(txtFromDate.Text, out fromDate))
                    {
                        fromDate = Convert.ToDateTime("01-Jan-1900");
                    }
                }
                if (!string.IsNullOrEmpty(txtToDate.Text))
                {
                    if(!DateTime.TryParse(txtToDate.Text, out toDate))
                        toDate = Convert.ToDateTime("01-Jan-2099");
                }

                ds = getdata.GetSitePhotographs_by_WorkpackageUID_BetweenDate(new Guid(Request.QueryString["WorkPackage"]), fromDate, toDate, "%" + txtDescription.Text + "%");
            }
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

        protected void GrdSitePhotograph_UpdateCommand(object source, DataListCommandEventArgs e)
        {
            string UID = GrdSitePhotograph.DataKeys[e.Item.ItemIndex].ToString();
            DataTable data = getdata.GetSitePhotographs_by_SitePhotographUID(new Guid(UID));
            if(data != null && data.Rows.Count > 0)
            {
                string description = data.Rows[0].Field<string>("Description");
                DateTime uploadedDate = data.Rows[0].Field<DateTime>("Uploaded_Date");
                string siteImage = data.Rows[0].Field<string>("Site_Image");
                if(siteImage.StartsWith("~/"))
                {
                    siteImage = siteImage.Replace("~/", "../");
                }
                txtDesc.Text = description;
                txtUploadDate.Text = uploadedDate.ToString("MM/dd/yyyy hh:mm:ss tt");
                ClientScript.RegisterStartupScript(this.GetType(), "LoadDiv", "LoadDiv('" + siteImage + "');", true);
            }
        }

        protected void btnSearch_Click(object source, EventArgs e)
        {
            DateTime fromDate = Convert.ToDateTime("01-Jan-1900");
            DateTime toDate = Convert.ToDateTime("01-Jan-2099");

            if (!string.IsNullOrEmpty(txtFromDate.Text))
            {
                if (!DateTime.TryParse(txtFromDate.Text, out fromDate))
                {
                    Response.Write("<script>alert('From date is not correct');</script>");
                    return;
                }
            }
            if (!string.IsNullOrEmpty(txtToDate.Text))
            {
                if (!DateTime.TryParse(txtToDate.Text, out toDate))
                {
                    Response.Write("<script>alert('To date is not correct');</script>");
                    return;
                }
            }
            BindSitePhotographs();

        }
    }
}