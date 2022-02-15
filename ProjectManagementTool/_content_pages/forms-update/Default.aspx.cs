using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProjectManager.DAL;

namespace ProjectManagementTool._content_pages.forms_update
{
    public partial class Default : System.Web.UI.Page
    {
        DBGetData getdata = new DBGetData();
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                ddlmeeting.DataSource = getdata.GetMeetingMaster();
                ddlmeeting.DataTextField = "Meeting_Description";
                ddlmeeting.DataValueField = "Meeting_UID";
                ddlmeeting.DataBind();
                if (Session["SelectedMeeting"] != null)
                {
                    ddlmeeting.SelectedValue = Session["SelectedMeeting"].ToString();
                    Session["SelectedMeeting"] = null;
                }
                LoadGridDataBudget();
                Session["sMeetingUID"] = ddlmeeting.SelectedValue;
            }
        }

        private void LoadGridDataBudget()
        {
            try
            {
                GrdBudgetbsDisbursemnt.DataSource = getdata.GetBudgetvsDisbursement(new Guid(ddlmeeting.SelectedValue));
                GrdBudgetbsDisbursemnt.DataBind();

                //
                DataTable dt = new DataTable();
                DataRow dr;


                // Add Columns to datatablse
                dt.Columns.Add(new DataColumn("UID"));
                dt.Columns.Add(new DataColumn("ProjectName")); //'ColumnName1' represents name of datafield in grid
                dt.Columns.Add(new DataColumn("ContractorName"));
                dt.Columns.Add(new DataColumn("AwardedCost"));
                dt.Columns.Add(new DataColumn("Disbursement_Amount"));
                dt.Columns.Add(new DataColumn("Disbursement_Amount_2021"));
                dt.Columns.Add(new DataColumn("Q1_Budget_Amount"));
                dt.Columns.Add(new DataColumn("Q2_Budget_Amount"));
                dt.Columns.Add(new DataColumn("Q3_Budget_Amount"));
                dt.Columns.Add(new DataColumn("Q4_Budget_Amount"));
                dt.Columns.Add(new DataColumn("Q1_Actual_Amount"));
                dt.Columns.Add(new DataColumn("Q2_Actual_Amount"));
                dt.Columns.Add(new DataColumn("Q3_Actual_Amount"));
                dt.Columns.Add(new DataColumn("Q4_Actual_Amount"));
                //dt.Columns.Add(new DataColumn("TotalDisburseAmnt"));


                // Add empty row first to DataTable to show as first row in gridview


                float AwardedCost = 0.0f;
                float Disbursement_Amount = 0.0f;
                float Disbursement_Amount_2021 = 0.0f;
                float Q1_Budget_Amount = 0.0f;
                float Q2_Budget_Amount = 0.0f;
                float Q3_Budget_Amount = 0.0f;
                float Q4_Budget_Amount = 0.0f;
                float Q1_Actual_Amount = 0.0f;
                float Q2_Actual_Amount = 0.0f;
                float Q3_Actual_Amount = 0.0f;
                float Q4_Actual_Amount = 0.0f;
                //float TotalDisburseAmnt = 0.0f;
                // Get each row from gridview and add it to DataTable
                float convertion = float.Parse(WebConfigurationManager.AppSettings["MJPYtoCrores2"]);
                foreach (DataRow gvr in getdata.GetBudgetvsDisbursement(new Guid(ddlmeeting.SelectedValue)).Tables[0].Rows)
                {
                    dr = dt.NewRow();
                    dr["ProjectName"] = gvr["UID"].ToString();
                    dr["ProjectName"] = gvr["ProjectName"].ToString();
                    dr["ContractorName"] = gvr["ContractorName"].ToString();
                    dr["AwardedCost"] = gvr["AwardedCost"].ToString();
                    dr["Disbursement_Amount"] = gvr["Disbursement_Amount"].ToString();
                    dr["Disbursement_Amount_2021"] = gvr["Disbursement_Amount_2021"].ToString();
                    dr["Q1_Budget_Amount"] = gvr["Q1_Budget_Amount"].ToString();
                    dr["Q2_Budget_Amount"] = gvr["Q2_Budget_Amount"].ToString();
                    dr["Q3_Budget_Amount"] = gvr["Q3_Budget_Amount"].ToString();
                    dr["Q4_Budget_Amount"] = gvr["Q4_Budget_Amount"].ToString();
                    dr["Q1_Actual_Amount"] = gvr["Q1_Actual_Amount"].ToString();
                    dr["Q2_Actual_Amount"] = gvr["Q2_Actual_Amount"].ToString();
                    dr["Q3_Actual_Amount"] = gvr["Q3_Actual_Amount"].ToString();
                    dr["Q4_Actual_Amount"] = gvr["Q4_Actual_Amount"].ToString();
                    //dr["TotalDisburseAmnt"] = gvr["TotalDisburseAmnt"].ToString();
                    //
                    AwardedCost += float.Parse(gvr["AwardedCost"].ToString());
                    Disbursement_Amount += float.Parse(gvr["Disbursement_Amount"].ToString());
                    Disbursement_Amount_2021 += float.Parse(gvr["Disbursement_Amount_2021"].ToString());
                    Q1_Budget_Amount += float.Parse(gvr["Q1_Budget_Amount"].ToString());
                    Q2_Budget_Amount += float.Parse(gvr["Q2_Budget_Amount"].ToString());
                    Q3_Budget_Amount += float.Parse(gvr["Q3_Budget_Amount"].ToString());
                    Q4_Budget_Amount += float.Parse(gvr["Q4_Budget_Amount"].ToString());
                    Q1_Actual_Amount += float.Parse(gvr["Q1_Actual_Amount"].ToString());
                    Q2_Actual_Amount += float.Parse(gvr["Q2_Actual_Amount"].ToString());
                    Q3_Actual_Amount += float.Parse(gvr["Q3_Actual_Amount"].ToString());
                    Q4_Actual_Amount += float.Parse(gvr["Q4_Actual_Amount"].ToString());
                    //TotalDisburseAmnt += float.Parse(gvr["TotalDisburseAmnt"].ToString());
                    dt.Rows.Add(dr);
                }

                dr = dt.NewRow();
                dr["UID"] = "";
                dr["ProjectName"] = "";
                dr["ContractorName"] = "";
                dr["AwardedCost"] = AwardedCost.ToString("n2");
                dr["Disbursement_Amount"] = Disbursement_Amount.ToString("n2");
                dr["Disbursement_Amount_2021"] = Disbursement_Amount_2021.ToString("n2");
                dr["Q1_Budget_Amount"] = Q1_Budget_Amount.ToString("n2");
                dr["Q2_Budget_Amount"] = Q2_Budget_Amount.ToString("n2");
                dr["Q3_Budget_Amount"] = Q3_Budget_Amount.ToString("n2");
                dr["Q4_Budget_Amount"] = Q4_Budget_Amount.ToString("n2");
                dr["Q1_Actual_Amount"] = Q1_Actual_Amount.ToString("n2");
                dr["Q2_Actual_Amount"] = Q2_Actual_Amount.ToString("n2");
                dr["Q3_Actual_Amount"] = Q3_Actual_Amount.ToString("n2");
                dr["Q4_Actual_Amount"] = Q4_Actual_Amount.ToString("n2");
                //dr["TotalDisburseAmnt"] = TotalDisburseAmnt.ToString("n2");
                dt.Rows.Add(dr);

                // Hard coded for JICA Meeting as convertion was not available....
                dr = dt.NewRow();
                dr["UID"] = "";
                dr["ProjectName"] = "";
                dr["ContractorName"] = "";
                dr["AwardedCost"] = WebConfigurationManager.AppSettings["BudgetVsDisbursement"] + " (Total All Quarters)";
                dr["Disbursement_Amount"] = Disbursement_Amount.ToString("n2");
                dr["Disbursement_Amount_2021"] = Disbursement_Amount_2021.ToString("n2");
                dr["Q1_Budget_Amount"] = ""; //Q1_Budget_Amount.ToString();
                dr["Q2_Budget_Amount"] = "";// Q2_Budget_Amount.ToString();
                dr["Q3_Budget_Amount"] = "";// Q3_Budget_Amount.ToString();
                dr["Q4_Budget_Amount"] = (Q1_Budget_Amount + Q2_Budget_Amount + Q3_Budget_Amount + Q4_Budget_Amount).ToString("n2"); ;
                dr["Q1_Actual_Amount"] = "";// Q1_Actual_Amount.ToString();
                dr["Q2_Actual_Amount"] = "";// Q2_Actual_Amount.ToString();
                dr["Q3_Actual_Amount"] = "";// Q3_Actual_Amount.ToString();
                dr["Q4_Actual_Amount"] = (Q1_Actual_Amount + Q2_Actual_Amount + Q3_Actual_Amount + Q4_Actual_Amount).ToString("n2"); ;
                //dr["TotalDisburseAmnt"] = TotalDisburseAmnt.ToString("n2");
                dt.Rows.Add(dr);

                // Add DataTable back to grid
                GrdBudgetbsDisbursemnt.DataSource = dt;
                GrdBudgetbsDisbursemnt.DataBind();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        protected void ddlmeeting_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlmeeting.SelectedValue != "")
            {
                DataSet ds = getdata.GetMeetingMaster_by_Meeting_UID(new Guid(ddlmeeting.SelectedValue));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ReviewMeetingDate.Value = ds.Tables[0].Rows[0]["CreatedDate"].ToString();
                }

                LoadGridDataBudget();
                Session["sMeetingUID"] = ddlmeeting.SelectedValue;
                if (ddlmeeting.SelectedIndex > 0)
                {
                    btncopy.Visible = true;
                }
                else
                {
                    btncopy.Visible = false;
                }
            }
        }

        protected void btncopy_Click(object sender, EventArgs e)
        {
            try
            {
                int index = ddlmeeting.SelectedIndex - 1;
                Guid sourcemeetingUID = new Guid(ddlmeeting.Items[index].Value);
                int result = getdata.CopyStatusvsDisbursementData(sourcemeetingUID, new Guid(ddlmeeting.SelectedValue));
                LoadGridDataBudget();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        protected void GrdBudgetbsDisbursemnt_DataBound(object sender, EventArgs e)
        {
            DateTime rDate = new DateTime();
            string FY = "";
            if (ReviewMeetingDate.Value != "")
            {
                rDate = Convert.ToDateTime(ReviewMeetingDate.Value);

                int sMonth = rDate.Month;
                int sYear = rDate.Year;

                if (sMonth >= 4)
                {
                    FY = sYear + "-" + (sYear + 1).ToString().Substring(2, 2);
                }
                else
                {
                    FY = (sYear - 1) + "-" + sYear.ToString().Substring(2, 2);
                }
            }

            GridViewRow row = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
            TableHeaderCell cell = new TableHeaderCell();
            cell.Text = "";
            cell.ColumnSpan = 6;
            row.Controls.Add(cell);

            cell = new TableHeaderCell();
            cell.ColumnSpan = 4;
            cell.Text = FY != "" ? "Budget for FY " + FY + " " + WebConfigurationManager.AppSettings["BudgetVsDisbursement"] : "Budget for FY 2020-21 MJPY";
            row.Controls.Add(cell);

            cell = new TableHeaderCell();
            cell.ColumnSpan = 4;
            cell.Text = FY != "" ? "Actual Disbursement for FY " + FY + " " + WebConfigurationManager.AppSettings["BudgetVsDisbursement"] : "Actual Disbursement for FY 2020-21 MJPY";
            row.Controls.Add(cell);

           

            //row.BackColor = ColorTranslator.FromHtml("#3AC0F2");
            GrdBudgetbsDisbursemnt.HeaderRow.Parent.Controls.AddAt(0, row);
        }
    }
}