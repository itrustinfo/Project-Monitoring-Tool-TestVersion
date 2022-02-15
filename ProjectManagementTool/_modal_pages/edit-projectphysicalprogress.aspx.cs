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
    public partial class edit_projectphysicalprogress : System.Web.UI.Page
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
                    if (Request.QueryString["PhysicalProgressUID"] != null)
                    {
                        BindMeetingMaster();
                        BindPhysicalProgress(Request.QueryString["PhysicalProgressUID"]);
                    }
                    
                }
            }
        }
        private void BindMeetingMaster()
        {
            DataSet ds = getdata.GetMeetingMasters();
            DDLMeetingMaster.DataTextField = "Meeting_Description";
            DDLMeetingMaster.DataValueField = "Meeting_UID";
            DDLMeetingMaster.DataSource = ds;
            DDLMeetingMaster.DataBind();
        }

        private void BindPhysicalProgress(string PhysicalProgressUID)
        {
            DataSet ds = getdata.GetProjectPhysicalProgress_by_PhysicalProgressUID(new Guid(PhysicalProgressUID));
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtnameofthepackage.Text = ds.Tables[0].Rows[0]["NameofthePackage"].ToString();
                txtTragetedPhysicalprogress.Text = float.Parse(ds.Tables[0].Rows[0]["Targeted_PhysicalProgress"].ToString()).ToString("N2");
                txtTargeted_Overall_WeightedProgress.Text = float.Parse(ds.Tables[0].Rows[0]["Targeted_Overall_WeightedProgress"].ToString()).ToString("N2");
                txtAchieved_PhysicalProgress.Text = float.Parse(ds.Tables[0].Rows[0]["Achieved_PhysicalProgress"].ToString()).ToString("N2");
                txtAchieved_Overall_WeightedProgress.Text = float.Parse(ds.Tables[0].Rows[0]["Achieved_Overall_WeightedProgress"].ToString()).ToString("N2");
                txtawarded_SanctionedValue.Text = float.Parse(ds.Tables[0].Rows[0]["Awarded_Sanctioned_Value"].ToString()).ToString("N2");
                txtStatusofAward.Text = ds.Tables[0].Rows[0]["Award_Status"].ToString();
                DDLMeetingMaster.SelectedValue= ds.Tables[0].Rows[0]["Meeting_UID"].ToString();
                if (ds.Tables[0].Rows[0]["Expenditure_As_On_Date"].ToString() != "")
                {
                    txtexpenditure.Text = float.Parse(ds.Tables[0].Rows[0]["Expenditure_As_On_Date"].ToString()).ToString("N2");
                }   
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                Guid PhysicalProgressUID = new Guid(Request.QueryString["PhysicalProgressUID"]);
                Guid ProjectUID = new Guid(Request.QueryString["ProjectUID"]);
                int cnt = getdata.ProjectPhysicalProgress_Insert(PhysicalProgressUID, ProjectUID, txtnameofthepackage.Text, float.Parse(txtTragetedPhysicalprogress.Text),
                        float.Parse(txtTargeted_Overall_WeightedProgress.Text), float.Parse(txtAchieved_PhysicalProgress.Text), float.Parse(txtAchieved_Overall_WeightedProgress.Text), DateTime.Now, float.Parse(txtawarded_SanctionedValue.Text), txtStatusofAward.Text, new Guid(DDLMeetingMaster.SelectedValue), float.Parse(txtexpenditure.Text));
                if (cnt > 0)
                {
                    Session["SelectedMeeting"] = DDLMeetingMaster.SelectedValue;
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code : EditPPP-01 There is a problem with these feature. Please contact system admin.');</script>");
            }
        }
    }
}