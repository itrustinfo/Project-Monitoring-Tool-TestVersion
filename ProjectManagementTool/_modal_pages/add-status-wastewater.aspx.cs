using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProjectManager.DAL;

namespace ProjectManagementTool._modal_pages
{
    public partial class add_status_wastewater : System.Web.UI.Page
    {
        DBGetData getdata = new DBGetData();
        TaskUpdate gettk = new TaskUpdate();
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
                    BindProject();
                    //
                    if (Request.QueryString["type"] != null) //it is edit
                    {
                        DataSet ds = getdata.GetStatusWasteWaterUID(new Guid(Request.QueryString["UID"].ToString()));
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            DDlProject.SelectedValue = ds.Tables[0].Rows[0]["ProjectUID"].ToString();
                            txtProjectComponents.Text = ds.Tables[0].Rows[0]["ProjectComponent"].ToString();
                            txtPackageDescription.Text = ds.Tables[0].Rows[0]["PackageDescription"].ToString().Replace("<br/>",Environment.NewLine);
                            txtAwardedCost.Text = ds.Tables[0].Rows[0]["AwardedCost"].ToString();
                            txtPresentStatus.Text = ds.Tables[0].Rows[0]["PresentStatus"].ToString().Replace("<br/>", Environment.NewLine);
                            ddlComponenttype.SelectedValue = ds.Tables[0].Rows[0]["Componenttype"].ToString();
                        }
                    }

                }
            }
        }

        private void BindProject()
        {
            DataTable ds = new DataTable();
            ds = gettk.GetProjects();
            DDlProject.DataTextField = "ProjectName";
            DDlProject.DataValueField = "ProjectUID";
            DDlProject.DataSource = ds;
            DDlProject.DataBind();

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                Guid UId = Guid.NewGuid();
                int result = 0;
                if (Request.QueryString["type"] != null) //it is edit
                {
                    result = getdata.UpdateStatusWasteWater(new Guid(Request.QueryString["UID"].ToString()), new Guid(DDlProject.SelectedValue), DDlProject.SelectedItem.ToString(), Server.HtmlEncode(txtPackageDescription.Text.Trim()).Replace("\r\n", "<br/>"), float.Parse(txtAwardedCost.Text), Server.HtmlEncode(txtProjectComponents.Text.Trim()).Replace("\r\n", "<br/>"), Server.HtmlEncode(txtPresentStatus.Text.Trim()).Replace("\r\n", "<br/>"), ddlComponenttype.SelectedItem.ToString());

                }
                else
                {
                    Session["SelectedMeeting"] = Session["sMeetingUID"].ToString();
                    result = getdata.InsertStatusWasteWater(UId, new Guid(DDlProject.SelectedValue), DDlProject.SelectedItem.ToString(), Server.HtmlEncode(txtPackageDescription.Text.Trim()).Replace("\r\n", "<br/>"), float.Parse(txtAwardedCost.Text), Server.HtmlEncode(txtProjectComponents.Text.Trim()).Replace("\r\n", "<br/>"), Server.HtmlEncode(txtPresentStatus.Text.Trim()).Replace("\r\n", "<br/>"), new Guid(Session["sMeetingUID"].ToString()), ddlComponenttype.SelectedItem.ToString());
                }
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");

            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}