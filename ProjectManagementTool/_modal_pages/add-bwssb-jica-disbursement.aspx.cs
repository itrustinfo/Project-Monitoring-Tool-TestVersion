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
    public partial class add_bwssb_jica_disbursement : System.Web.UI.Page
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
                    if (Request.QueryString["type"] != null) //it is edit
                    {
                        DataSet ds = getdata.GetbwssbvsJICA_UID(new Guid(Request.QueryString["UID"].ToString()));
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            Session["SelectedMeeting"] = ds.Tables[0].Rows[0]["Meeting_UID"].ToString();
                            DDlProject.SelectedValue = ds.Tables[0].Rows[0]["ProjectUID"].ToString();
                            txtContractorName.Text = ds.Tables[0].Rows[0]["ContractorName"].ToString();
                            txtAwardedCost.Text = ds.Tables[0].Rows[0]["AwardedCost"].ToString();
                            txtBudgetQ1.Text = ds.Tables[0].Rows[0]["Q1_Payment_Amount"].ToString();
                            txtBudgetQ2.Text = ds.Tables[0].Rows[0]["Q2_Payment_Amount"].ToString();
                            txtBudgetQ3.Text = ds.Tables[0].Rows[0]["Q3_Payment_Amount"].ToString();
                            txtBudgetQ4.Text = ds.Tables[0].Rows[0]["Q4_Payment_Amount"].ToString();
                            txtActualQ1.Text = ds.Tables[0].Rows[0]["Q1_Actual_Amount"].ToString();
                            txtActualQ2.Text = ds.Tables[0].Rows[0]["Q2_Actual_Amount"].ToString();
                            txtActualQ3.Text = ds.Tables[0].Rows[0]["Q3_Actual_Amount"].ToString();
                            txtActualQ4.Text = ds.Tables[0].Rows[0]["Q4_Actual_Amount"].ToString();
                            //  DDlProject.Enabled = false;
                            DDlProject.Visible = false;
                            txtProject.Text = ds.Tables[0].Rows[0]["ProjectName"].ToString();
                            txtProject.Visible = true;
                            txtProject.Enabled = false;
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
                string disbursementYear = "2019-20";
                string BudgetYear = "2020-21";
                string ActualYear = "2020-21";
                Guid uid = Guid.NewGuid();
                float awardedcost = float.Parse(txtAwardedCost.Text);
              
                float Q1_BudgetAmnt = float.Parse(txtBudgetQ1.Text);
                float Q2_BudgetAmnt = float.Parse(txtBudgetQ2.Text);
                float Q3_BudgetAmnt = float.Parse(txtBudgetQ3.Text);
                float Q4_BudgetAmnt = float.Parse(txtBudgetQ4.Text);
                //
                float Q1_ActualAmnt = float.Parse(txtActualQ1.Text);
                float Q2_ActualAmnt = float.Parse(txtActualQ2.Text);
                float Q3_ActualAmnt = float.Parse(txtActualQ3.Text);
                float Q4_ActualAmnt = float.Parse(txtActualQ4.Text);

                int result = 0;
                if (Request.QueryString["type"] != null) //it is edit
                {
                    result = getdata.UpdateBWSSB_JICA_Disbursement(new Guid(Request.QueryString["UID"].ToString()), txtContractorName.Text, awardedcost, BudgetYear, Q1_BudgetAmnt, Q2_BudgetAmnt, Q3_BudgetAmnt, Q4_BudgetAmnt, ActualYear, Q1_ActualAmnt, Q2_ActualAmnt, Q3_ActualAmnt, Q4_ActualAmnt);
                }
                else
                {
                    Session["SelectedMeeting"] = Session["sMeetingUID"].ToString();
                    result = getdata.InsertorUpdateBWSSB_JICA_Disbursement(uid, new Guid(DDlProject.SelectedValue), txtContractorName.Text, awardedcost, BudgetYear, Q1_BudgetAmnt, Q2_BudgetAmnt, Q3_BudgetAmnt, Q4_BudgetAmnt, ActualYear, Q1_ActualAmnt, Q2_ActualAmnt, Q3_ActualAmnt, Q4_ActualAmnt, DDlProject.SelectedItem.ToString(), new Guid(Session["sMeetingUID"].ToString()));

                }
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
            }
            catch (Exception ex)
            {
                //Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code : ABG-01 there is a problem with these feature. please contact system admin.');</script>");
                throw;
            }
        }
    }
}