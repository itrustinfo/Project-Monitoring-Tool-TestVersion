using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProjectManager.DAL;

namespace ProjectManagementTool._content_pages.bwssb_jica
{
    public partial class Default : System.Web.UI.Page
    {
        DBGetData getdata = new DBGetData();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
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
                GrdBudgetbsDisbursemnt.DataSource = getdata.GetBWSSB_VS_JICA_Disbursement(new Guid(ddlmeeting.SelectedValue));
                GrdBudgetbsDisbursemnt.DataBind();
            }
            catch (Exception ex)
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
                int result = getdata.CopyBWSSB_JICA_Disbursement(sourcemeetingUID, new Guid(ddlmeeting.SelectedValue));
                LoadGridDataBudget();
            }
            catch (Exception ex)
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
            cell.ColumnSpan = 4;
            row.Controls.Add(cell);

            cell = new TableHeaderCell();
            cell.ColumnSpan = 4;
            cell.Text = FY != "" ? "Payment Done by BWSSB for FY " + FY + " Crores" : "Payment Done by BWSSB for FY 2020-21 Crores";
            row.Controls.Add(cell);

            cell = new TableHeaderCell();
            cell.ColumnSpan = 4;
            cell.Text = FY != "" ? "Actual Disbursement by JICA for FY " + FY + " Crores" : "Actual Disbursement by JICA for FY 2020-21 MJPY";
            row.Controls.Add(cell);

            //row.BackColor = ColorTranslator.FromHtml("#3AC0F2");
            GrdBudgetbsDisbursemnt.HeaderRow.Parent.Controls.AddAt(0, row);
        }
    }
}