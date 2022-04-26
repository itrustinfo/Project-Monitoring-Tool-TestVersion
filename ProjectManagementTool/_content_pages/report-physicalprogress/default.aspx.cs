using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Web.Configuration;
using System.IO;
using System.Text;

namespace ProjectManagementTool._content_pages.report_physicalprogress
{
    public partial class _default : System.Web.UI.Page
    {
        DBGetData getdt = new DBGetData();
        TaskUpdate gettk = new TaskUpdate();
        double PreviousTarget = 0;
        double PreviousActual = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Username"] == null)
            {
                Response.Redirect("~/Login.aspx");
            }
            else
            {
                if (!IsPostBack)
                {
                    BindProject();
                    ReportFormat.Visible = false;
                    MonthlyPhysicalProgress.Visible = false;
                    ByMonth.Visible = false;
                    ByWeek.Visible = false;
                    WeeklyProgressReport.Visible = false;
                    AcrossMonthsProgressReport.Visible = false;
                    ByActivity.Visible = false;
                    ActivityProgressReport.Visible = false;
                }
            }
        }

        private void BindProject()
        {
            DataTable ds = new DataTable();
            if (Session["TypeOfUser"].ToString() == "U" || Session["TypeOfUser"].ToString() == "MD" || Session["TypeOfUser"].ToString() == "VP")
            {
                ds = gettk.GetProjects();
            }
            else if (Session["TypeOfUser"].ToString() == "PA")
            {
                ds = gettk.GetAssignedProjects_by_UserUID(new Guid(Session["UserUID"].ToString()));
            }
            else
            {
                ds = gettk.GetAssignedProjects_by_UserUID(new Guid(Session["UserUID"].ToString()));
            }

            DDlProject.DataTextField = "ProjectName";
            DDlProject.DataValueField = "ProjectUID";
            DDlProject.DataSource = ds;
            DDlProject.DataBind();

            DDlProject.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", ""));
            DDLWorkPackage.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", ""));

        }

        protected void DDlProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDlProject.SelectedValue != "")
            {
                DataSet ds = new DataSet();
                if (Session["TypeOfUser"].ToString() == "U" || Session["TypeOfUser"].ToString() == "MD" || Session["TypeOfUser"].ToString() == "VP")
                {
                    ds = getdt.GetWorkPackages_By_ProjectUID(new Guid(DDlProject.SelectedValue));
                }
                else if (Session["TypeOfUser"].ToString() == "PA")
                {
                    ds = getdt.GetWorkPackages_ForUser_by_ProjectUID(new Guid(Session["UserUID"].ToString()), new Guid(DDlProject.SelectedValue));
                }
                else
                {
                    ds = getdt.GetWorkPackages_ForUser_by_ProjectUID(new Guid(Session["UserUID"].ToString()), new Guid(DDlProject.SelectedValue));
                }
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DDLWorkPackage.DataTextField = "Name";
                    DDLWorkPackage.DataValueField = "WorkPackageUID";
                    DDLWorkPackage.DataSource = ds;
                    DDLWorkPackage.DataBind();

                    DDLWorkPackage_SelectedIndexChanged(sender, e);
                }
            }
        }

        protected void DDLWorkPackage_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDLWorkPackage.SelectedValue != "")
            {
                //DataSet ds = getdt.GetConstructionProgramme_Tasks(new Guid(DDLWorkPackage.SelectedValue));
                //GrdMonthPhysicalProgress.DataSource = ds;
                //GrdMonthPhysicalProgress.DataBind();
                //BindMonths(DDLWorkPackage.SelectedValue);
                ReportFormat.Visible = true;
                Bind_Year(DDLWorkPackage.SelectedValue);
                ByMonth.Visible = false;
                ByWeek.Visible = false;
                ByActivity.Visible = false;
                AcrossMonthsProgressReport.Visible = false;
                ActivityProgressReport.Visible = false;
                WeeklyProgressReport.Visible = false;
                MonthlyPhysicalProgress.Visible = false;
                RBLReportFor.ClearSelection();
            }
        }

        private void Bind_Year(string WorkpackageUID)
        {
            DataSet ds = getdt.GetWorkPackages_By_WorkPackageUID(new Guid(WorkpackageUID));
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["StartDate"].ToString() != "" && ds.Tables[0].Rows[0]["PlannedEndDate"].ToString() != "")
                {
                    try
                    {
                        DDLYear.Items.Clear();
                        //DDLMonth.Items.Clear();
                        DDLYear.Items.Add(new System.Web.UI.WebControls.ListItem("--Select Year--", ""));
                        DDLMonth.Items.Add(new System.Web.UI.WebControls.ListItem("--Select Month--", ""));
                        int StartYear = Convert.ToDateTime(ds.Tables[0].Rows[0]["StartDate"].ToString()).Year;
                        int EndDateYear = Convert.ToDateTime(ds.Tables[0].Rows[0]["PlannedEndDate"].ToString()).Year;
                        for (int i = StartYear; i <= EndDateYear; i++)
                        {
                            DDLYear.Items.Add(new System.Web.UI.WebControls.ListItem(i.ToString(), i.ToString()));
                        }
                    }
                    catch (Exception ex)
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error : " + ex.Message + "');</script>");
                    }
                }
            }

        }
        //private void BindMonths(string WorkpackageUID)
        //{
        //    DataSet ds = getdt.GetWorkPackages_By_WorkPackageUID(new Guid(WorkpackageUID));
        //    if (ds.Tables[0].Rows.Count > 0)
        //    {
        //        DateTime StartDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["StartDate"].ToString());
        //        DateTime EndDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["PlannedEndDate"].ToString());
        //        int i = 0;
        //        foreach (DateTime day in EachCalendarDay(StartDate, EndDate))
        //        {
        //            DDLMonth.Items.Insert(i, day.ToString("MMM yyyy"));
        //            i += 1;
        //        }
        //    }
        //}

        //public IEnumerable<DateTime> EachCalendarDay(DateTime startDate, DateTime endDate)
        //{
        //    for (var date = startDate.Date; date.Date <= endDate.Date; date = date.AddDays(1)) yield
        //    return date;
        //}

        protected void RBLReportFor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RBLReportFor.SelectedValue == "By Week")
            {
                ByMonth.Visible = false;
                ByWeek.Visible = true;
                ByActivity.Visible = false;
                AcrossMonthsProgressReport.Visible = false;
                ActivityProgressReport.Visible = false;
                MonthlyPhysicalProgress.Visible = false;
                WeeklyProgressReport.Visible = false;
                DateTime start = DateTime.Today;// Adjust to your start date

                DDLWeek.Items.Clear();
                DateTime startOfWeek = DateTime.Today.AddDays(-1 * (int)(DateTime.Today.DayOfWeek));
                DDLWeek.Items.Add(new System.Web.UI.WebControls.ListItem("-- Select Week --", ""));
                DDLWeek.Items.Add(new System.Web.UI.WebControls.ListItem(startOfWeek.ToString("dd/MM/yyyy") + " - " + DateTime.Now.ToString("dd/MM/yyyy"), startOfWeek.ToString("dd/MM/yyyy") + " - " + DateTime.Now.ToString("dd/MM/yyyy")));
                for (int x = 0; x < 3; x++)
                {
                    DDLWeek.Items.Add(new System.Web.UI.WebControls.ListItem(startOfWeek.AddDays(-7).ToString("dd/MM/yyyy") + " - " + startOfWeek.ToString("dd/MM/yyyy"), startOfWeek.AddDays(-7).ToString("dd/MM/yyyy") + " - " + startOfWeek.ToString("dd/MM/yyyy")));
                    startOfWeek = startOfWeek.AddDays(-7);
                }
            }
            else if (RBLReportFor.SelectedValue == "Across Months")
            {
                ByMonth.Visible = false;
                ByWeek.Visible = false;
                ByActivity.Visible = false;
                AcrossMonthsProgressReport.Visible = true;
                ActivityProgressReport.Visible = false;
                WeeklyProgressReport.Visible = false;
                MonthlyPhysicalProgress.Visible = false;
                BindAcrossMontsPhysicalProgress(DDLWorkPackage.SelectedValue);
            }
            else if (RBLReportFor.SelectedValue == "By Activity")
            {
                DDLActivity.Items.Clear();
                ByMonth.Visible = false;
                ByWeek.Visible = false;
                ByActivity.Visible = true;
                AcrossMonthsProgressReport.Visible = false;
                ActivityProgressReport.Visible = false;
                WeeklyProgressReport.Visible = false;
                MonthlyPhysicalProgress.Visible = false;
                DataTable ds = getdt.GetConstructionProgramme_Tasks(new Guid(DDLWorkPackage.SelectedValue)).Tables[0].AsEnumerable()
                     .OrderBy(r => r.Field<string>("Name"))
                     .CopyToDataTable();

               foreach(DataRow dr in ds.Rows)
                {
                    System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem(dr["Name"].ToString() + " (" + getTaskHeirarchy(new Guid(dr["TaskUID"].ToString())) + ")", dr["TaskUID"].ToString());
                    DDLActivity.Items.Add(lst);
                }

                //DDLActivity.DataTextField = "Name";
                //DDLActivity.DataValueField = "TaskUID";
                //DDLActivity.DataSource = ds;
                //DDLActivity.DataBind();
                DDLActivity.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select Activity--", ""));
            }
            else
            {
                ByMonth.Visible = true;
                ByWeek.Visible = false;
                ByActivity.Visible = false;
                AcrossMonthsProgressReport.Visible = false;
                ActivityProgressReport.Visible = false;
                WeeklyProgressReport.Visible = false;
                MonthlyPhysicalProgress.Visible = false;
            }
        }

        protected void BntSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (DDLYear.SelectedValue != "" && DDLMonth.SelectedValue != "")
                {
                    BindMonthlyProjectProgressReport();
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Please select Month or Year');</script>");
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('There is a problem with this feature. Please contact system admin.');</script>");
            }
        }

        protected void BindMonthlyProjectProgressReport()
        {
            ViewState["Export"] = "1";
            MonthlyProgressReportName.InnerHtml = "Report Name : Physical Progress Achievements in the month " + DDLMonth.SelectedItem.Text + " " + DDLYear.SelectedValue;
            MOnthlyProgressProjectName.InnerHtml = "Project Name : " + DDlProject.SelectedItem.Text + " (" + DDLWorkPackage.SelectedItem.Text + ")";
            DataSet ds = getdt.GetConstructionProgramme_Tasks(new Guid(DDLWorkPackage.SelectedValue));
            GrdMonthPhysicalProgress.DataSource = ds;
            GrdMonthPhysicalProgress.DataBind();

            MonthlyPhysicalProgress.Visible = true;
            if (ds.Tables[0].Rows.Count > 0)
            {
                btnMonthlyProgressExportPDF.Visible = true;
                btnMonthlyProgressPrint.Visible = true;
                btnMonthlyProgressExporttoExcel.Visible = true;
            }
        }

        protected void GrdMonthPhysicalProgress_DataBound(object sender, EventArgs e)
        {
            if (ViewState["Export"].ToString() == "1")
            {
                GridViewRow row = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
                TableHeaderCell cell = new TableHeaderCell();
                cell.Text = "";
                cell.ColumnSpan = 1;
                row.Controls.Add(cell);

                cell = new TableHeaderCell();
                cell.Text = "";
                cell.ColumnSpan = 1;
                row.Controls.Add(cell);

                cell = new TableHeaderCell();
                cell.Text = "";
                cell.ColumnSpan = 1;
                row.Controls.Add(cell);

                cell = new TableHeaderCell();
                cell.Text = "";
                cell.ColumnSpan = 1;
                row.Controls.Add(cell);

                cell = new TableHeaderCell();
                cell.ColumnSpan = 1;
                cell.Text = "Up to previous Month";
                //cell.HorizontalAlign
                row.Controls.Add(cell);

                cell = new TableHeaderCell();
                cell.ColumnSpan = 3;
                cell.Text = "For this Month";
                row.Controls.Add(cell);

                cell = new TableHeaderCell();
                cell.ColumnSpan = 3;
                cell.Text = "Cumulative for the Project";
                row.Controls.Add(cell);

                //row.BackColor = ColorTranslator.FromHtml("#3AC0F2");
                GrdMonthPhysicalProgress.HeaderRow.Parent.Controls.AddAt(0, row);
            }
        }

        protected void GrdMonthPhysicalProgress_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string TaskUID = GrdMonthPhysicalProgress.DataKeys[e.Row.RowIndex].Values[0].ToString();

                string sDate1 = "";
                DateTime CDate1 = DateTime.Now;

                sDate1 = "01/" + DDLMonth.SelectedValue + "/" + DDLYear.SelectedValue;
                sDate1 = getdt.ConvertDateFormat(sDate1);
                CDate1 = Convert.ToDateTime(sDate1);

                if (e.Row.Cells[2].Text != "")
                {

                    if (e.Row.Cells[2].Text.ToUpper() == "METERS")
                    {
                        e.Row.Cells[3].Text = e.Row.Cells[3].Text != "" ? Convert.ToDouble(e.Row.Cells[3].Text).ToString("N2") : "";
                    }
                    else if (e.Row.Cells[2].Text.ToUpper() == "NUMBERS")
                    {
                        e.Row.Cells[3].Text = e.Row.Cells[3].Text;
                    }
                    else if (e.Row.Cells[2].Text.ToUpper() == "RMT")
                    {
                        e.Row.Cells[3].Text = e.Row.Cells[3].Text != "" ? Convert.ToDouble(e.Row.Cells[3].Text).ToString("N2") : "";
                    }
                    else if (e.Row.Cells[2].Text.ToUpper() == "TONS")
                    {
                        e.Row.Cells[3].Text = e.Row.Cells[3].Text != "" ? Convert.ToDouble(e.Row.Cells[3].Text).ToString("N2") : "";
                    }
                    else if (e.Row.Cells[2].Text.ToUpper() == "KM")
                    {
                        e.Row.Cells[3].Text = e.Row.Cells[3].Text != "" ? Convert.ToDecimal(e.Row.Cells[3].Text).ToString("N2") : "";
                    }
                    else if (e.Row.Cells[2].Text.ToUpper() == "LOT")
                    {
                        e.Row.Cells[3].Text = e.Row.Cells[3].Text != "" ? Convert.ToDouble(e.Row.Cells[3].Text).ToString("N2") : "";
                    }
                    else if (e.Row.Cells[2].Text.ToUpper() == "HOUR")
                    {
                        e.Row.Cells[3].Text = e.Row.Cells[3].Text != "" ? Convert.ToDouble(e.Row.Cells[3].Text).ToString("N2") : "";
                    }
                    else
                    {
                        e.Row.Cells[3].Text = e.Row.Cells[3].Text != "" ? Convert.ToDouble(e.Row.Cells[3].Text).ToString("N2") : "";
                    }
                }
                DataSet ds = getdt.GetPhysicalProgress_ForMonth_by_TaskUID(new Guid(TaskUID), CDate1);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    e.Row.Cells[4].Text = ds.Tables[0].Rows[0]["PrevAchieved"].ToString() != "" ? decimal.Round(Convert.ToDecimal(ds.Tables[0].Rows[0]["PrevAchieved"].ToString()), 2).ToString() : "";
                    e.Row.Cells[5].Text = ds.Tables[0].Rows[0]["PlannedThisMonth"].ToString() != "" ? decimal.Round(Convert.ToDecimal(ds.Tables[0].Rows[0]["PlannedThisMonth"].ToString()), 2).ToString() : "";
                    e.Row.Cells[6].Text = ds.Tables[0].Rows[0]["AchievedThisMonth"].ToString() != "" ? decimal.Round(Convert.ToDecimal(ds.Tables[0].Rows[0]["AchievedThisMonth"].ToString()), 2).ToString() : "";
                    e.Row.Cells[7].Text = ds.Tables[0].Rows[0]["AchievedPercentage"].ToString() != "" ? decimal.Round(Convert.ToDecimal(ds.Tables[0].Rows[0]["AchievedPercentage"].ToString()), 2).ToString() : "";
                    e.Row.Cells[8].Text = ds.Tables[0].Rows[0]["CumulativePlanned"].ToString() != "" ? decimal.Round(Convert.ToDecimal(ds.Tables[0].Rows[0]["CumulativePlanned"].ToString()), 2).ToString() : "";
                    e.Row.Cells[9].Text = ds.Tables[0].Rows[0]["CumulativeAchieved"].ToString() != "" ? decimal.Round(Convert.ToDecimal(ds.Tables[0].Rows[0]["CumulativeAchieved"].ToString()), 2).ToString() : "";
                    e.Row.Cells[10].Text = ds.Tables[0].Rows[0]["Balance"].ToString() != "" ? decimal.Round(Convert.ToDecimal(ds.Tables[0].Rows[0]["Balance"].ToString()), 2).ToString() : "";
                    e.Row.Cells[11].Text = ds.Tables[0].Rows[0]["NextMonthPlan"].ToString() != "" ? decimal.Round(Convert.ToDecimal(ds.Tables[0].Rows[0]["NextMonthPlan"].ToString()), 2).ToString() : "";
                    e.Row.Cells[12].Text = ds.Tables[0].Rows[0]["OverAllCompletion"].ToString() != "" ? decimal.Round(Convert.ToDecimal(ds.Tables[0].Rows[0]["OverAllCompletion"].ToString()), 2).ToString() : "";
                }
            }
        }

        protected void btnWeekSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (DDLWeek.SelectedValue != "")
                {
                    BindWeeklyProgressReport();
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Please select Week');</script>");
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('There is a problem with this feature. Please contact system admin.');</script>");
            }
        }

        protected void BindWeeklyProgressReport()
        {
            ViewState["Export"] = "1";
            //LblWeeklyHeading.Text = "Weekly work Progress Status as on " + DDLWeek.SelectedValue.Split('-')[1];
            WeeklyReportNameHeading.InnerHtml = "Report Name : Weekly work Progress Status as on " + DDLWeek.SelectedValue.Split('-')[1];
            WeeklyProjectName.InnerHtml = "Project Name : " + DDlProject.SelectedItem.Text + " (" + DDLWorkPackage.SelectedItem.Text + ")";
            DataSet ds = getdt.GetConstructionProgramme_Tasks(new Guid(DDLWorkPackage.SelectedValue));
            GrdWeeklyprogress.DataSource = ds;
            GrdWeeklyprogress.DataBind();
            MonthlyPhysicalProgress.Visible = false;
            WeeklyProgressReport.Visible = true;
            
            if (ds.Tables[0].Rows.Count > 0)
            {

                btnExportReportPDF.Visible = true;
                btnPrintPDF.Visible = true;
                btnExportReportExcel.Visible = true;
            }
        }

        protected void GrdWeeklyprogress_DataBound(object sender, EventArgs e)
        {
            if (ViewState["Export"].ToString() == "1")
            {
                GridViewRow row = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
                TableHeaderCell cell = new TableHeaderCell();
                cell.Text = "";
                cell.ColumnSpan = 1;
                row.Controls.Add(cell);

                cell = new TableHeaderCell();
                cell.Text = "";
                cell.ColumnSpan = 1;
                row.Controls.Add(cell);

                cell = new TableHeaderCell();
                cell.Text = "";
                cell.ColumnSpan = 1;
                row.Controls.Add(cell);

                cell = new TableHeaderCell();
                cell.ColumnSpan = 2;
                cell.Text = "Target as on " + DDLWeek.SelectedValue.Split('-')[1].Trim();
                //cell.HorizontalAlign
                row.Controls.Add(cell);

                cell = new TableHeaderCell();
                cell.ColumnSpan = 2;
                cell.Text = "Achieved as on " + DDLWeek.SelectedValue.Split('-')[1].Trim();
                row.Controls.Add(cell);

                //row.BackColor = ColorTranslator.FromHtml("#3AC0F2");
                GrdWeeklyprogress.HeaderRow.Parent.Controls.AddAt(0, row);
            }
        }

        protected void GrdWeeklyprogress_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string TaskUID = GrdWeeklyprogress.DataKeys[e.Row.RowIndex].Values[0].ToString();

                string sDate1 = "", sDate2="";
                DateTime CDate1 = DateTime.MinValue, CDate2 = DateTime.MinValue;

                sDate1 = DDLWeek.SelectedValue.Split('-')[0].Trim();
                sDate1 = getdt.ConvertDateFormat(sDate1);
                CDate1 = Convert.ToDateTime(sDate1);

                sDate2 = DDLWeek.SelectedValue.Split('-')[1].Trim();
                sDate2 = getdt.ConvertDateFormat(sDate2);
                CDate2 = Convert.ToDateTime(sDate2);

                DataSet ds = getdt.GetPhysicalProgress_ForWeek__by_TaskUID(new Guid(TaskUID), CDate1, CDate2);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    e.Row.Cells[3].Text = ds.Tables[0].Rows[0]["CumulativePlanned"].ToString() != "" ? decimal.Round(Convert.ToDecimal(ds.Tables[0].Rows[0]["CumulativePlanned"].ToString()), 2).ToString() : "";
                    e.Row.Cells[4].Text = ds.Tables[0].Rows[0]["PlanFortheWeek"].ToString() != "" ? decimal.Round(Convert.ToDecimal(ds.Tables[0].Rows[0]["PlanFortheWeek"].ToString()), 2).ToString() : "";
                    e.Row.Cells[5].Text = ds.Tables[0].Rows[0]["CumulativeAchieved"].ToString() != "" ? decimal.Round(Convert.ToDecimal(ds.Tables[0].Rows[0]["CumulativeAchieved"].ToString()), 2).ToString() : "";
                    e.Row.Cells[6].Text = ds.Tables[0].Rows[0]["AchievedFortheWeek"].ToString() != "" ? decimal.Round(Convert.ToDecimal(ds.Tables[0].Rows[0]["AchievedFortheWeek"].ToString()), 2).ToString() : "";
                    e.Row.Cells[7].Text = ds.Tables[0].Rows[0]["ProgressPercentage"].ToString() != "" ? decimal.Round(Convert.ToDecimal(ds.Tables[0].Rows[0]["ProgressPercentage"].ToString()), 2).ToString() : "";
                }
            }
        }

        protected void BindAcrossMontsPhysicalProgress(string WorkpackageUID)
        {
            AcrossMonthReportName.InnerHtml = "Report Name : Physical Progress Monitoring Sheet";
            AcrossMonthProjectName.InnerHtml = "Project Name : " + DDlProject.SelectedItem.Text + " (" + DDLWorkPackage.SelectedItem.Text + ")";

            DataSet ds = getdt.GetConstructionProgramme_Tasks(new Guid(WorkpackageUID));
            if (ds.Tables[0].Rows.Count > 0)
            {
                btnAcrossMonthsExportExcel.Visible = true;
                string sHTMLString = "<div style='width:100%; float:left; height:500px; overflow-y:auto;'><table class='table table-bordered'>";
                string DescriptionofWork = "<thead style='position:sticky; top:0; background:LightGray;'><tr><th></th>";
                string sUnit = "<tr><td><b>Unit</td></td>";
                string BillOfQuantity= "<tr><td><b>Bill of Quantities</td></td>";
                string Rev_Scope= "<tr><td><b>Surveyed Quantities</td></td>";
                string ActivityData = "";
                string sActivityData = "";
                string Target = "";
                string Achieved = "";
                string PercentageProgress = "";
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DescriptionofWork += "<th style='text-align:center;'><b>" + (i + 1) + "." + ds.Tables[0].Rows[i]["Name"].ToString() + "</b></th>";
                    sUnit += "<td style='text-align:center;'>" + ds.Tables[0].Rows[i]["UnitforProgress"].ToString() + "</td>";
                    BillOfQuantity+= "<td style='text-align:center;'>" + ds.Tables[0].Rows[i]["UnitQuantity"].ToString() + "</td>";
                    Rev_Scope += "<td style='text-align:center;'>" + ds.Tables[0].Rows[i]["UnitQuantity"].ToString() + "</td>";
                }
                DataSet dsMonths = getdt.GetConstructionProgramme_Months_by_WorkpackageUID(new Guid(WorkpackageUID), DateTime.Now);
                for (int j = 0; j < dsMonths.Tables[0].Rows.Count; j++)
                {
                    ActivityData = "<tr style='background-color:LightGray;'><td><b>" + Convert.ToDateTime(dsMonths.Tables[0].Rows[j]["StartDate"].ToString()).ToString("MMM yyyy") + "</b></td>";
                    Target = "<tr><td>Target</td>";
                    Achieved = "<tr><td>Achieved</td>";
                    PercentageProgress = "<tr><td>% age of Progress</td>";

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        try
                        {
                            DataSet cdate = getdt.GetConstructionProgramme_MonthData_by_TaskUID(new Guid(ds.Tables[0].Rows[i]["TaskUID"].ToString()), Convert.ToDateTime(dsMonths.Tables[0].Rows[j]["StartDate"].ToString()));
                            if (cdate.Tables[0].Rows.Count > 0)
                            {
                                decimal TargetValue = 0;
                                decimal AchievedValue = 0;
                                decimal ProgressPercentage = 0;

                                TargetValue = cdate.Tables[0].Rows[0]["Schedule_Value"].ToString() != "" ? Convert.ToDecimal(cdate.Tables[0].Rows[0]["Schedule_Value"].ToString()) : 0;
                                AchievedValue = cdate.Tables[0].Rows[0]["Achieved_Value"].ToString() != "" ? Convert.ToDecimal(cdate.Tables[0].Rows[0]["Achieved_Value"].ToString()) : 0;
                                ProgressPercentage = cdate.Tables[0].Rows[0]["ProgressPercentage"].ToString() != "" ? Convert.ToDecimal(cdate.Tables[0].Rows[0]["ProgressPercentage"].ToString()) : 0;
                                ActivityData += "<td></td>";
                                Target += "<td style='text-align:center;'>" + decimal.Round(TargetValue,2) + "</td>";
                                Achieved += "<td style='text-align:center;'>" + decimal.Round(AchievedValue,2) + "</td>";
                                PercentageProgress += "<td style='text-align:center;'>" + decimal.Round(ProgressPercentage,2) + "</td>";
                            }
                            else
                            {
                                ActivityData += "<td></td>";
                                Target += "<td>-</td>";
                                Achieved += "<td>-</td>";
                                PercentageProgress += "<td>-</td>";
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    ActivityData += "</tr>";
                    Target+= "</tr>";
                    Achieved+= "</tr>";
                    PercentageProgress+= "</tr>";
                    sActivityData += ActivityData + Target + Achieved + PercentageProgress;
                }

                DescriptionofWork += "</tr></thead>";
                sUnit += "</tr>";
                BillOfQuantity += "</tr>";
                Rev_Scope += "</tr>";
                sHTMLString += DescriptionofWork + sUnit + BillOfQuantity + Rev_Scope + sActivityData + "</table></div>";

                DivAcrossMonths.InnerHtml = sHTMLString;
            }
            else
            {
                DivAcrossMonths.InnerHtml = "No Data Found..";
            }
        }

        protected void DDLActivity_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDLActivity.SelectedValue != "")
            {
                ActivityProgressReport.Visible = true;
                //LblActivityPhysicalProgress.Text = "Physical Progress for the Activity : " + DDLActivity.SelectedItem.Text;

                ActivityProgressReportName.InnerHtml = "Report Name : Physical Progress Monitoring for the Activity '" + DDLActivity.SelectedItem.Text + "'";
                ActivityProgressProjectName.InnerHtml = "Project Name : " + DDlProject.SelectedItem.Text + " (" + DDLWorkPackage.SelectedItem.Text + ")";

                decimal PrevCumulativePlan = 0;
                decimal PrevCumulativeActual = 0;
                DataSet ds = getdt.GetTaskSchedule_By_TaskUID(new Guid(DDLActivity.SelectedValue));
                if (ds.Tables[0].Rows.Count > 0)
                {                    
                    btnActivityProgressPrint.Visible = true;
                    bool ShowTable = false;
                    string tablemonths = "<td style=\"width:200px\">&nbsp;</td>";
                    string tmonthlyplan = "<td style=\"padding:3px\">Monthly Plan</td>";
                    string tmonthlyactual = "<td style=\"padding:3px\">Monthly Actual</td>";
                    string tcumulativeplan = "<td style=\"padding:3px\">Cumulative Plan</td>";
                    string tcumulativeactual = "<td style=\"padding:3px\">Cumulative Actual</td>";

                    System.Text.StringBuilder strScript = new System.Text.StringBuilder();
                    strScript.Append(@"<script type='text/javascript'>
                    google.charts.load('current', { 'packages':['corechart']
                    });
                      if (typeof google.charts.visualization == 'undefined') {
                        google.charts.setOnLoadCallback(drawVisualization);
                    }
                    else {
                        drawVisualization();
                    }
                    function drawVisualization()
                    {
                        // Some raw data (not necessarily accurate)
                        var data = google.visualization.arrayToDataTable([
                          ['Month', 'Plan', 'Actual', 'Cumulative Plan', 'Cumulative Actual'],");
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        if (Convert.ToDateTime(ds.Tables[0].Rows[i]["EndDate"].ToString()) <= DateTime.Now)
                        {
                            ShowTable = true;
                            string Plan = ds.Tables[0].Rows[i]["Schedule_Value"].ToString() != "" ? ds.Tables[0].Rows[i]["Schedule_Value"].ToString() : "0";
                            string Actual = ds.Tables[0].Rows[i]["Achieved_Value"].ToString() != "" ? ds.Tables[0].Rows[i]["Achieved_Value"].ToString() : "0";
                            if (Plan != "" && Actual != "")
                            {
                                if (i == 0)
                                {
                                    PrevCumulativePlan = Convert.ToDecimal(Plan);
                                    PrevCumulativeActual = Convert.ToDecimal(Actual);
                                    strScript.Append("['" + Convert.ToDateTime(ds.Tables[0].Rows[i]["EndDate"].ToString()).ToString("MMM-yy") + "'," + Plan + "," + Actual + "," + Plan + "," + Actual + "],");
                                }
                                else
                                {
                                    strScript.Append("['" + Convert.ToDateTime(ds.Tables[0].Rows[i]["EndDate"].ToString()).ToString("MMM-yy") + "'," + Plan + "," + Actual + "," + (Convert.ToDecimal(Plan) + PrevCumulativePlan) + "," + (Convert.ToDecimal(Actual) + PrevCumulativeActual) + "],");
                                    //e.Row.Cells[3].Text = (Convert.ToDouble(TargetValue) + PreviousTarget).ToString();
                                    PrevCumulativePlan = (Convert.ToDecimal(Plan) + PrevCumulativePlan);
                                    //e.Row.Cells[4].Text = (Convert.ToDouble(AchievedValue) + PreviousActual).ToString();
                                    PrevCumulativeActual = (Convert.ToDecimal(Actual) + PrevCumulativeActual);
                                }

                                tablemonths += "<td style=\"padding:3px\">" + Convert.ToDateTime(ds.Tables[0].Rows[i]["EndDate"].ToString()).ToString("MMM-yy") + "</td>";
                                tmonthlyplan += "<td style=\"padding:3px\">" + decimal.Round(Convert.ToDecimal(Plan), 2) + "</td>";
                                tmonthlyactual += "<td style=\"padding:3px\">" + decimal.Round(Convert.ToDecimal(Actual), 2) + "</td>";

                                tcumulativeplan += "<td style=\"padding:3px\">" + decimal.Round(PrevCumulativePlan, 2) + "</td>";
                                tcumulativeactual += "<td style=\"padding:3px\">" + decimal.Round(PrevCumulativeActual, 2) + "</td>";
                            }


                        }
                    }
                    strScript.Remove(strScript.Length - 1, 1);
                    strScript.Append("]);");
                    strScript.Append(@"var options = {
                        legend: { position: 'top' },
                        seriesType: 'bars',
                        series: { 2: { type: 'line',targetAxisIndex: 1 },3: { type: 'line',targetAxisIndex: 1 } },
                        hAxis: { title: 'Month',titleTextStyle: {
                        bold:'true',
                      }},
                      vAxes: {                        
          
                        0: {title: 'Monthly Plan',titleTextStyle: {
                    bold:'true',
                  }},
                        1: {title: 'Cumulative Plan',titleTextStyle: {
                    bold:'true',
                  }}
                      }
                };

                var chart = new google.visualization.ComboChart(document.getElementById('chart_div'));
                chart.draw(data, options);
                  }
                </script>");
                    if (ShowTable)
                    {
                        ltScript_Progress.Text = strScript.ToString();

                        DivActivityProgressTabular.InnerHtml = "<table border=\"1\" style=\"text-align:center;font-size:12px; width:100%; color:black; padding-left:10px;\">" +
                                     "<tr> " + tablemonths + "</tr>" +
                                      "<tr> " + tmonthlyplan + "</tr>" +
                                       "<tr> " + tmonthlyactual + "</tr>" +
                                        "<tr> " + tcumulativeplan + "</tr>" +
                                         "<tr> " + tcumulativeactual + "</tr>" +
                                             "</table>";
                    }
                    else
                    {
                        ltScript_Progress.Text = "<h3>No data</h3>";
                        DivActivityProgressTabular.InnerHtml= "<h3>No data</h3>";
                    }
                    
                }
                else
                {
                    ltScript_Progress.Text = "<h3>No data</h3>";
                }
            }
        }

        protected void btnExportReportPDF_Click(object sender, EventArgs e)
        {
            ViewState["Export"] = "2";
            BindWeeklyProgressReport();
            ExporttoPDF(GrdWeeklyprogress, 2, "No");
        }

        protected void btnPrintPDF_Click(object sender, EventArgs e)
        {
            ViewState["Export"] = "2";
            BindWeeklyProgressReport();
            ExporttoPDF(GrdWeeklyprogress, 2, "Yes");
        }

        private void ExporttoPDF(GridView gd, int type, string isPrint)
        {
            try
            {

                DateTime CDate1 = DateTime.Now;
                GridView gdRp = new GridView();
                gdRp = gd;

                int noOfColumns = 0, noOfRows = 0;
                DataTable tbl = null;

                if (RBLReportFor.SelectedValue == "By Week")
                {
                    gdRp.Columns[0].HeaderText = "Sl.No";
                    gdRp.Columns[1].HeaderText = "Description of Work";
                    gdRp.Columns[2].HeaderText = "UOM";
                    gdRp.Columns[3].HeaderText = "Cumulative Target as on " + DDLWeek.SelectedValue.Split('-')[1].Trim();
                    gdRp.Columns[4].HeaderText = "Target for the Week";
                    gdRp.Columns[5].HeaderText = "Cumulative Achieved as on " + DDLWeek.SelectedValue.Split('-')[1].Trim();
                    gdRp.Columns[6].HeaderText = "Achieved for the Week";
                    gdRp.Columns[7].HeaderText = "% of Progress Cumulative";
                }
                else if (RBLReportFor.SelectedValue == "By Month")
                {
                    int index = 0;
                    if (DDLMonth.SelectedIndex != 0)
                    {
                        index = (DDLMonth.SelectedIndex) - 1;
                    }
                    
                    gdRp.Columns[0].HeaderText = "S.No";
                    gdRp.Columns[1].HeaderText = "Description of Work";
                    gdRp.Columns[2].HeaderText = "UOM";
                    gdRp.Columns[3].HeaderText = "Scope as per BOQ";
                    gdRp.Columns[4].HeaderText = "Achieved up to " + DDLMonth.Items[index].Text + "_" + DDLYear.SelectedValue;
                    gdRp.Columns[5].HeaderText = "Planned for " + DDLMonth.SelectedItem.Text + "_" + DDLYear.SelectedValue + "";
                    gdRp.Columns[6].HeaderText = "Achieved for " + DDLMonth.SelectedItem.Text + "_" + DDLYear.SelectedValue + "";
                    gdRp.Columns[7].HeaderText = "Achieved % " + DDLMonth.SelectedItem.Text + "_" + DDLYear.SelectedValue + "";

                    gdRp.Columns[8].HeaderText = "Cum. Planned";
                    gdRp.Columns[9].HeaderText = "Cum. Achieved";
                    gdRp.Columns[10].HeaderText = "Balance";
                    gdRp.Columns[11].HeaderText = "Plan for the Next Month";
                    gdRp.Columns[12].HeaderText = "Overall % of Completion";
                }
                if (gdRp.AutoGenerateColumns)
                {
                    tbl = gdRp.DataSource as DataTable; // Gets the DataSource of the GridView Control.
                    noOfColumns = tbl.Columns.Count;
                    noOfRows = tbl.Rows.Count;
                }
                else
                {
                    noOfColumns = gdRp.Columns.Count;
                    noOfRows = gdRp.Rows.Count;
                }

                float HeaderTextSize = 9;
                float ReportNameSize = 9;
                float ReportTextSize = 9;
                float ApplicationNameSize = 13;
                string ProjectName = DDlProject.SelectedItem.ToString();

                //if (ProjectName.Length > 30)
                //{
                //    ProjectName = ProjectName.Substring(0, 29) + "..";
                //}

                // Creates a PDF document

                Document document = null;
                //if (LandScape == true)
                //{
                // Sets the document to A4 size and rotates it so that the     orientation of the page is Landscape.
                document = new Document(PageSize.A4.Rotate(), 0, 0, 0, 0);

                //}
                //else
                //{
                //    document = new Document(PageSize.A4, 0, 0, 15, 5);
                //}

                // Creates a PdfPTable with column count of the table equal to no of columns of the gridview or gridview datasource.
                iTextSharp.text.pdf.PdfPTable mainTable = new iTextSharp.text.pdf.PdfPTable(noOfColumns);

                // Sets the first 4 rows of the table as the header rows which will be repeated in all the pages.
                mainTable.HeaderRows = 4;

                // Creates a PdfPTable with 2 columns to hold the header in the exported PDF.
                iTextSharp.text.pdf.PdfPTable headerTable = new iTextSharp.text.pdf.PdfPTable(1);

                // Creates a phrase to hold the application name at the left hand side of the header.
                Phrase phApplicationName = new Phrase();
                string ExportFileName = "";
                if (RBLReportFor.SelectedValue == "By Week")
                {
                    phApplicationName = new Phrase(Environment.NewLine + WebConfigurationManager.AppSettings["Domain"] + " Report Name: Weekly work Progress Status as on  " + DDLWeek.SelectedValue.Split('-')[1].Trim(), FontFactory.GetFont("Arial", ApplicationNameSize, iTextSharp.text.Font.BOLD));
                    ExportFileName = "Report_Weekly_work_Progress_Status_" + DateTime.Now.Ticks + ".pdf";

                    mainTable.SetWidths(new float[] { 6, 32, 12, 10, 10, 10, 10, 10 });
                }
                else if (RBLReportFor.SelectedValue == "By Month")
                {
                    phApplicationName = new Phrase(Environment.NewLine + WebConfigurationManager.AppSettings["Domain"] + " Report Name: Physical Progress Achievements in the month " + DDLMonth.SelectedItem.Text + " " + DDLYear.SelectedValue, FontFactory.GetFont("Arial", ApplicationNameSize, iTextSharp.text.Font.BOLD));
                    ExportFileName = "Report_Physical_Progress_Achievements_in_the_month_" + DDLMonth.SelectedItem.Text + "-" + DDLYear.SelectedValue + "_" + DateTime.Now.Ticks + ".pdf";
                    mainTable.SetWidths(new float[] { 4, 12, 10, 8, 8, 7, 7, 7, 7, 7, 6, 8, 8 });
                }
                else if (RBLReportFor.SelectedValue == "Across Months")
                {
                    phApplicationName = new Phrase(Environment.NewLine + WebConfigurationManager.AppSettings["Domain"] + " Report Name: Physical Progress Monitoring Sheet", FontFactory.GetFont("Arial", ApplicationNameSize, iTextSharp.text.Font.BOLD));
                    ExportFileName = "Report_Physical_Progress_Monitoring_Sheet_" + DateTime.Now.Ticks + ".pdf";
                }
                else
                {
                    phApplicationName = new Phrase(Environment.NewLine + WebConfigurationManager.AppSettings["Domain"] + " Report Name: Physical Progress Monitoring for the Activity " + DDLActivity.SelectedItem.Text, FontFactory.GetFont("Arial", ApplicationNameSize, iTextSharp.text.Font.BOLD));
                    ExportFileName = "Report_Physical_Progress_Monitoring_for_the_Activity_" + DDLActivity.SelectedItem.Text + "_" + DateTime.Now.Ticks + ".pdf";
                }


                // Creates a PdfPCell which accepts a phrase as a parameter.
                PdfPCell clApplicationName = new PdfPCell(phApplicationName);
                // Sets the border of the cell to zero.
                clApplicationName.Border = PdfPCell.NO_BORDER;
                // Sets the Horizontal Alignment of the PdfPCell to left.
                clApplicationName.HorizontalAlignment = Element.ALIGN_CENTER;

                // Creates a phrase to show the current date at the right hand side of the header.
                //Phrase phDate = new Phrase(DateTime.Now.Date.ToString("dd/MM/yyyy"), FontFactory.GetFont("Arial", ApplicationNameSize, iTextSharp.text.Font.NORMAL));

                //// Creates a PdfPCell which accepts the date phrase as a parameter.
                //PdfPCell clDate = new PdfPCell(phDate);
                //// Sets the Horizontal Alignment of the PdfPCell to right.
                //clDate.HorizontalAlignment = Element.ALIGN_RIGHT;
                //// Sets the border of the cell to zero.
                //clDate.Border = PdfPCell.NO_BORDER;

                // Adds the cell which holds the application name to the headerTable.
                headerTable.AddCell(clApplicationName);
                // Adds the cell which holds the date to the headerTable.
                //  headerTable.AddCell(clDate);
                // Sets the border of the headerTable to zero.
                headerTable.DefaultCell.Border = PdfPCell.NO_BORDER;

                // Creates a PdfPCell that accepts the headerTable as a parameter and then adds that cell to the main PdfPTable.
                PdfPCell cellHeader = new PdfPCell(headerTable);
                cellHeader.Border = PdfPCell.NO_BORDER;
                // Sets the column span of the header cell to noOfColumns.
                cellHeader.Colspan = noOfColumns;
                // Adds the above header cell to the table.
                mainTable.AddCell(cellHeader);

                // Creates a phrase which holds the file name.
                Phrase phHeader = new Phrase("Project Name : " + ProjectName + " (" + DDLWorkPackage.SelectedItem.Text + ")");
                PdfPCell clHeader = new PdfPCell(phHeader);
                clHeader.Colspan = noOfColumns;
                clHeader.Border = PdfPCell.NO_BORDER;
                clHeader.HorizontalAlignment = Element.ALIGN_CENTER;
                mainTable.AddCell(clHeader);



                // Creates a phrase for a new line.
                Phrase phSpace = new Phrase("\n");
                PdfPCell clSpace = new PdfPCell(phSpace);
                clSpace.Border = PdfPCell.NO_BORDER;
                clSpace.Colspan = noOfColumns;
                mainTable.AddCell(clSpace);

                // Sets the gridview column names as table headers.
                for (int i = 0; i < noOfColumns; i++)
                {
                    Phrase ph = null;

                    if (gdRp.AutoGenerateColumns)
                    {
                        ph = new Phrase(tbl.Columns[i].ColumnName.Replace("<br/>", Environment.NewLine), FontFactory.GetFont("Arial", HeaderTextSize, iTextSharp.text.Font.BOLD));
                    }
                    else
                    {
                        ph = new Phrase(gdRp.Columns[i].HeaderText.Replace("<br/>", Environment.NewLine), FontFactory.GetFont("Arial", HeaderTextSize, iTextSharp.text.Font.BOLD));
                    }
                    PdfPCell cl = new PdfPCell(ph);
                    if (i == 1)
                    {
                        cl.HorizontalAlignment = Element.ALIGN_LEFT;
                        mainTable.AddCell(cl);
                    }
                    else
                    {
                        cl.HorizontalAlignment = Element.ALIGN_CENTER;
                        mainTable.AddCell(cl);
                    }
                    
                }

                // Reads the gridview rows and adds them to the mainTable
                for (int rowNo = 0; rowNo <= noOfRows; rowNo++)
                {
                    if (rowNo != noOfRows)
                    {
                        for (int columnNo = 0; columnNo < noOfColumns; columnNo++)
                        {
                            if (gdRp.AutoGenerateColumns)
                            {
                                string s = gdRp.Rows[rowNo].Cells[columnNo].Text.Trim();
                                Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                                PdfPCell cl = new PdfPCell(ph);
                                cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                mainTable.AddCell(cl);
                            }
                            else
                            {
                                if (columnNo == 0)
                                {
                                    if (gdRp.Columns[columnNo] is TemplateField)
                                    {
                                        DataBoundLiteralControl lc = gdRp.Rows[rowNo].Cells[columnNo].Controls[0] as DataBoundLiteralControl;
                                        string s = lc.Text.Trim();
                                        Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                                        PdfPCell cl = new PdfPCell(ph);
                                        cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                        mainTable.AddCell(cl);
                                    }
                                    else
                                    {
                                        string s = gdRp.Rows[rowNo].Cells[columnNo].Text.Trim();
                                        s = s.Replace("&nbsp;", "");
                                        Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                                        PdfPCell cl = new PdfPCell(ph);
                                        cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                        mainTable.AddCell(cl);
                                    }
                                }
                                else if (columnNo == 1)
                                {
                                    if (gdRp.Columns[columnNo] is TemplateField)
                                    {
                                        DataBoundLiteralControl lc = gdRp.Rows[rowNo].Cells[columnNo].Controls[0] as DataBoundLiteralControl;
                                        string s = lc.Text.Trim();
                                        Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                                        PdfPCell cl = new PdfPCell(ph);
                                        cl.HorizontalAlignment = Element.ALIGN_LEFT;
                                        mainTable.AddCell(cl);
                                    }
                                    else
                                    {
                                        string s = gdRp.Rows[rowNo].Cells[columnNo].Text.Trim();
                                        s = s.Replace("&nbsp;", "");
                                        Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                                        PdfPCell cl = new PdfPCell(ph);
                                        cl.HorizontalAlignment = Element.ALIGN_LEFT;
                                        mainTable.AddCell(cl);
                                    }
                                }
                                else if (columnNo == 2)
                                {
                                    if (gdRp.Columns[columnNo] is TemplateField)
                                    {
                                        DataBoundLiteralControl lc = gdRp.Rows[rowNo].Cells[columnNo].Controls[0] as DataBoundLiteralControl;
                                        string s = lc.Text.Trim();
                                        Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                                        PdfPCell cl = new PdfPCell(ph);
                                        cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                        mainTable.AddCell(cl);
                                    }
                                    else
                                    {
                                        string s = gdRp.Rows[rowNo].Cells[columnNo].Text.Trim();
                                        s = s.Replace("&nbsp;", "");
                                        Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                                        PdfPCell cl = new PdfPCell(ph);
                                        cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                        mainTable.AddCell(cl);
                                    }
                                }
                                else if (columnNo == 3 || columnNo == 4 || columnNo == 5 || columnNo == 6 || columnNo == 7)
                                {
                                    if (gdRp.Columns[columnNo] is TemplateField)
                                    {
                                        DataBoundLiteralControl lc = gdRp.Rows[rowNo].Cells[columnNo].Controls[0] as DataBoundLiteralControl;
                                        string s = lc.Text.Trim();
                                        Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                                        PdfPCell cl = new PdfPCell(ph);
                                        cl.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        mainTable.AddCell(cl);
                                    }
                                    else
                                    {
                                        string s = gdRp.Rows[rowNo].Cells[columnNo].Text.Trim();
                                        s = s.Replace("&nbsp;", "");
                                        Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                                        PdfPCell cl = new PdfPCell(ph);
                                        cl.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        mainTable.AddCell(cl);
                                    }
                                }
                                else
                                {
                                    if (gdRp.Columns[columnNo] is TemplateField)
                                    {
                                        DataBoundLiteralControl lc = gdRp.Rows[rowNo].Cells[columnNo].Controls[0] as DataBoundLiteralControl;
                                        string s = lc.Text.Trim();
                                        Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                                        PdfPCell cl = new PdfPCell(ph);
                                        cl.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        mainTable.AddCell(cl);
                                    }
                                    else
                                    {
                                        string s = gdRp.Rows[rowNo].Cells[columnNo].Text.Trim();
                                        s = s.Replace("&nbsp;", "");
                                        Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                                        PdfPCell cl = new PdfPCell(ph);
                                        cl.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        mainTable.AddCell(cl);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (type == 1)
                        {
                            for (int columnNo = 0; columnNo < noOfColumns; columnNo++)
                            {
                                string s = "Grand Total";
                                if (columnNo == 1)
                                {
                                    Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.BOLD));
                                    PdfPCell cl = new PdfPCell(ph);
                                    cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                    mainTable.AddCell(cl);
                                }
                                else if (columnNo == 2)
                                {
                                    s = ViewState["grandtotalcount"].ToString();// Session["AwardedValue"].ToString();
                                    Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.BOLD));
                                    PdfPCell cl = new PdfPCell(ph);
                                    cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                    mainTable.AddCell(cl);
                                }
                                else if (columnNo == 3)
                                {
                                    s = ViewState["grandtotaldwnldcount"].ToString();// Session["ExpenditureOverallTotal"].ToString();
                                    Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.BOLD));
                                    PdfPCell cl = new PdfPCell(ph);
                                    cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                    mainTable.AddCell(cl);
                                }
                                else if (columnNo == 4)
                                {
                                    s = ViewState["grandtotalviewcount"].ToString();// Session["TargetedOverallTotal"].ToString() + "%";
                                    Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.BOLD));
                                    PdfPCell cl = new PdfPCell(ph);
                                    cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                    mainTable.AddCell(cl);
                                }
                                else if (columnNo == 5)
                                {
                                    s = ViewState["GrandTotalDocumentLinkSent"].ToString();// Session["AchievedOverallTotal"].ToString() + "%";
                                    Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.BOLD));
                                    PdfPCell cl = new PdfPCell(ph);
                                    cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                    mainTable.AddCell(cl);
                                }
                                else
                                {
                                    Phrase ph = new Phrase("", FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.BOLD));
                                    PdfPCell cl = new PdfPCell(ph);
                                    cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                    mainTable.AddCell(cl);
                                }
                            }
                        }

                    }

                    // Tells the mainTable to complete the row even if any cell is left incomplete.
                    mainTable.CompleteRow();
                }

                // Gets the instance of the document created and writes it to the output stream of the Response object.
                if (isPrint == "Yes")
                {
                    PdfWriter.GetInstance(document, new FileStream(Server.MapPath("~") + "/RegExcel/printpdf.pdf", FileMode.Create));
                }
                else
                {
                    PdfWriter.GetInstance(document, Response.OutputStream);
                }
                //PdfWriter.GetInstance(document, new FileStream(Server.MapPath("~") + "mypdf.pdf", FileMode.Create));
                // Creates a footer for the PDF document.
                int len = 174;
                System.Text.StringBuilder time = new System.Text.StringBuilder();
                time.Append(DateTime.Now.ToString("hh:mm tt"));
                time.Append("".PadLeft(len, ' ').Replace(" ", " "));

                iTextSharp.text.Font foot = new iTextSharp.text.Font();
                foot.Size = 10;
                HeaderFooter pdfFooter = new HeaderFooter(new Phrase("Date : " + DateTime.Now.ToString("dd/MM/yyyy") + " Time " + time + "  Page: ", foot), new Phrase(Environment.NewLine + " ", foot));
                pdfFooter.Alignment = Element.ALIGN_CENTER;
                document.Footer = pdfFooter;
                document.Open();
                // Adds the mainTable to the document.
                document.Add(mainTable);
                // Closes the document.
                document.Close();
                if (isPrint == "Yes")
                {
                    Session["Print"] = true;
                    Response.Write("<script>window.open ('/PDFPRint.aspx','_blank');</script>");
                }
                else
                {
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "attachment;filename=" + ExportFileName + "");
                    Response.End();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnMonthlyProgressExportPDF_Click(object sender, EventArgs e)
        {
            ViewState["Export"] = "2";
            BindMonthlyProjectProgressReport();
            ExporttoPDF(GrdMonthPhysicalProgress, 2, "No");
        }

        protected void btnMonthlyProgressPrint_Click(object sender, EventArgs e)
        {
            ViewState["Export"] = "2";
            BindMonthlyProjectProgressReport();
            ExporttoPDF(GrdMonthPhysicalProgress, 2, "Yes");
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            //required to avoid the runtime error "  
            //Control 'GridView1' of type 'GridView' must be placed inside a form tag with runat=server."  
        }

        protected void btnAcrossMonthsExportExcel_Click(object sender, EventArgs e)
        {
            try
            {
                StringBuilder StrExport = new StringBuilder();

                StringWriter stw = new StringWriter();
                HtmlTextWriter htextw = new HtmlTextWriter(stw);
                htextw.AddStyleAttribute("font-size", "9pt");
                htextw.AddStyleAttribute("color", "Black");



                DivAcrossMonths.RenderControl(htextw); //Name of the Panel

                //var sb1 = new StringBuilder();
                //GridDiv.RenderControl(new HtmlTextWriter(new StringWriter(sb1)));

                string s = htextw.InnerWriter.ToString();

                string HTMLstring = "<html><body>" +
                    "<div style='width:100%; margin:auto;'><div style='width:100%; float:left; line-height:25px; font-size:12pt;' align='center'>" +
                    "<asp:Label ID='Lbl1' runat='server' Font-Bold='true'>" + WebConfigurationManager.AppSettings["Domain"] + " Report Name: Physical Progress Monitoring Sheet</asp:Label><br />" +
                    "<asp:Label ID='Lbl4' runat='server'>Project Name : " + DDlProject.SelectedItem.Text + " (" + DDLWorkPackage.SelectedItem.Text + ")</asp:Label><br />" +
                    "</div> <div style='width:100%; float:left; height:10px;'>&nbsp;&nbsp;&nbsp;</div>" +
                    "<div style='width:100%;font-size:11pt float:left;' align='left'>" +
                    s +
                    "</div>" +
                    "</div></body></html>";

                string strFile = "Report_Report_Physical_Progress_Monitoring_Sheet_" + DateTime.Now.Ticks + ".xls";
                string strcontentType = "application/excel";
                Response.ClearContent();
                Response.ClearHeaders();
                Response.BufferOutput = true;
                Response.ContentType = strcontentType;
                Response.AddHeader("Content-Disposition", "attachment; filename=" + strFile);
                Response.Write(HTMLstring);
                Response.Flush();
                Response.Close();
                Response.End();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnExportReportExcel_Click(object sender, EventArgs e)
        {
            try
            {
                ViewState["Export"] = "2";
                BindWeeklyProgressReport();

                StringBuilder StrExport = new StringBuilder();

                StringWriter stw = new StringWriter();
                HtmlTextWriter htextw = new HtmlTextWriter(stw);
                htextw.AddStyleAttribute("font-size", "9pt");
                htextw.AddStyleAttribute("color", "Black");



                GrdWeeklyprogress.RenderControl(htextw); //Name of the Panel

                //var sb1 = new StringBuilder();
                //GridDiv.RenderControl(new HtmlTextWriter(new StringWriter(sb1)));

                string s = htextw.InnerWriter.ToString();

                string HTMLstring = "<html><body>" +
                    "<div style='width:100%; margin:auto;'><div style='width:100%; float:left; line-height:25px; font-size:12pt;' align='center'>" +
                    "<asp:Label ID='Lbl1' runat='server' Font-Bold='true'>" + WebConfigurationManager.AppSettings["Domain"] + " Report Name: Weekly work Progress Status as on  " + DDLWeek.SelectedValue.Split('-')[1].Trim() +"</asp:Label><br />" +
                    "<asp:Label ID='Lbl4' runat='server'>Project Name : " + DDlProject.SelectedItem.Text + " (" + DDLWorkPackage.SelectedItem.Text + ")</asp:Label><br />" +
                    "</div> <div style='width:100%; float:left; height:10px;'>&nbsp;&nbsp;&nbsp;</div>" +
                    "<div style='width:100%;font-size:11pt float:left;' align='left'>" +
                    s +
                    "</div>" +
                    "</div></body></html>";

                string strFile = "Report_Weekly_work_Progress_Status_" + DateTime.Now.Ticks + ".xls";
                string strcontentType = "application/excel";
                Response.ClearContent();
                Response.ClearHeaders();
                Response.BufferOutput = true;
                Response.ContentType = strcontentType;
                Response.AddHeader("Content-Disposition", "attachment; filename=" + strFile);
                Response.Write(HTMLstring);
                Response.Flush();
                Response.Close();
                Response.End();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnMonthlyProgressExporttoExcel_Click(object sender, EventArgs e)
        {
            ViewState["Export"] = "2";
            BindMonthlyProjectProgressReport();

            StringBuilder StrExport = new StringBuilder();

            StringWriter stw = new StringWriter();
            HtmlTextWriter htextw = new HtmlTextWriter(stw);
            htextw.AddStyleAttribute("font-size", "9pt");
            htextw.AddStyleAttribute("color", "Black");



            GrdMonthPhysicalProgress.RenderControl(htextw); //Name of the Panel

            //var sb1 = new StringBuilder();
            //GridDiv.RenderControl(new HtmlTextWriter(new StringWriter(sb1)));

            string s = htextw.InnerWriter.ToString();

            string HTMLstring = "<html><body>" +
                "<div style='width:100%; margin:auto;'><div style='width:100%; float:left; line-height:25px; font-size:12pt;' align='center'>" +
                "<asp:Label ID='Lbl1' runat='server' Font-Bold='true'>" + WebConfigurationManager.AppSettings["Domain"] + " Report Name: Physical Progress Achievements in the month " + DDLMonth.SelectedItem.Text + " " + DDLYear.SelectedValue + "</asp:Label><br />" +
                "<asp:Label ID='Lbl4' runat='server'>Project Name : " + DDlProject.SelectedItem.Text + " (" + DDLWorkPackage.SelectedItem.Text + ")</asp:Label><br />" +
                "</div> <div style='width:100%; float:left; height:10px;'>&nbsp;&nbsp;&nbsp;</div>" +
                "<div style='width:100%;font-size:11pt float:left;' align='left'>" +
                s +
                "</div>" +
                "</div></body></html>";

            string strFile = "Report_Physical_Progress_Achievements_in_the_month_" + DDLMonth.SelectedItem.Text + "-" + DDLYear.SelectedValue + "_" + DateTime.Now.Ticks + ".xls";
            string strcontentType = "application/excel";
            Response.ClearContent();
            Response.ClearHeaders();
            Response.BufferOutput = true;
            Response.ContentType = strcontentType;
            Response.AddHeader("Content-Disposition", "attachment; filename=" + strFile);
            Response.Write(HTMLstring);
            Response.Flush();
            Response.Close();
            Response.End();
        }

        private string getTaskHeirarchy(Guid TaskUID)
        {
            string TaskList = string.Empty;
            string ParenttaskUID = getdt.GetParentTaskUID_by_TaskUID(TaskUID);
            while(!string.IsNullOrWhiteSpace(ParenttaskUID))
            {
                TaskList += getdt.getTaskNameby_TaskUID(new Guid(ParenttaskUID)) + "->";
                ParenttaskUID = getdt.GetParentTaskUID_by_TaskUID(new Guid(ParenttaskUID));
            }
            if(!string.IsNullOrEmpty(TaskList))
            {
                TaskList = TaskList.Substring(0, TaskList.Length - 2);
            }
            return TaskList;
        }

       
    }
}