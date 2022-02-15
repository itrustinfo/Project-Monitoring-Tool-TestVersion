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
    public partial class add_paymentbreakupterms : System.Web.UI.Page
    {
        DBGetData getdata = new DBGetData();
        TaskUpdate TKUpdate = new TaskUpdate();
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
                    BindPaymentBreakupTypes();
                    //DDLBOQItem.Items.Insert(0, new ListItem(getdata.GetBOQDesc_by_BOQDetailsUID(new Guid(Request.QueryString["BOQDetailsUID"])), ""));
                }
            }
        }

        private void BindPaymentBreakupTypes()
        {
            DataSet ds = getdata.GetPaymentBreakupTypes();
            DDLPaymentBreakupType.DataTextField = "Breakup_Description";
            DDLPaymentBreakupType.DataValueField = "Breakup_UID";
            DDLPaymentBreakupType.DataSource = ds;
            DDLPaymentBreakupType.DataBind();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                Guid BreakupTerms_UID;
                if (Request.QueryString["BreakupTerms_UID"] != null)
                {
                    BreakupTerms_UID = new Guid(Request.QueryString["BreakupTerms_UID"]);
                }
                else
                {
                    BreakupTerms_UID = Guid.NewGuid();
                }

                //string ProjectUID = getdata.getProjectUIDby_WorkpackgeUID(new Guid(Request.QueryString["WorkpackageUID"]));
                int cnt = getdata.InsertorUpdatePaymentBreakupTerms(BreakupTerms_UID, new Guid(Request.QueryString["projectuid"]), new Guid(DDLPaymentBreakupType.SelectedValue), new Guid(Request.QueryString["BOQDetailsUID"]),float.Parse(txtpercentage.Text), txtdesc.Text);
                if (cnt > 0)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
                }
                else if (cnt == -1)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Breakup type already exists..');</script>");
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('There is a problem with this feature. Please contact system admin.');</script>");
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('There is a problem with this feature. Please contact system admin. Description : " + ex.Message + "');</script>");
            }
        }
    }
}