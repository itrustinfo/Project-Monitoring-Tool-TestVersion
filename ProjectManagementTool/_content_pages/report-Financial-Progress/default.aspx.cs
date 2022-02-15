using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using ProjectManager.DAL;

namespace ProjectManagementTool._content_pages.report_Financial_Progress
{
    public partial class _default : System.Web.UI.Page
    {
        DBGetData getdt = new DBGetData();
        TaskUpdate gettk = new TaskUpdate();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Username"] == null)
            {
                Response.Redirect("~/Login.aspx");
            }
            if (!IsPostBack)
            {
                ViewState["export"] = 2;
                BindProject();
                DDlProject_SelectedIndexChanged(sender, e);
              
            }
        }

        protected void DDlProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDlProject.SelectedValue != "")
            {
                //DataSet ds = getdt.GetWorkPackages_By_ProjectUID(new Guid(DDlProject.SelectedValue));
                //
                divFinProgressChart.Visible = false;
                divTabular.Visible = false;
                //
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

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if(DDlProject.SelectedIndex == 0)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Please select project !');</script>");
                    return;
                }
                if (rdSelect.SelectedValue == "0")
                {
                    divFinProgressChart.Visible = false;
                    divTabular.Visible = true;
                    LoadTabularData();
                    headingTb.InnerHtml = "Monthly Financial Progress Report - " + DDlProject.SelectedItem.Text + " (" + DDLWorkPackage.SelectedItem.Text + ")";
                }
                else if (rdSelect.SelectedValue == "1")
                {
                    divFinProgressChart.Visible = true;
                    divTabular.Visible = false;
                    LoadFinancialGraph();
                    heading.InnerHtml = "Monthly Financial Progress Chart - " + DDlProject.SelectedItem.Text + " (" + DDLWorkPackage.SelectedItem.Text + ")";
                   
                }
                else if (rdSelect.SelectedValue == "2")
                {
                    divFinProgressChart.Visible = true;
                    divTabular.Visible = false;
                    LoadFinancialGraphPer();
                    heading.InnerHtml = "Monthly Financial Progress Chart - " + DDlProject.SelectedItem.Text + " (" + DDLWorkPackage.SelectedItem.Text + ")";

                }


            }
            catch(Exception ex)
            {
                throw ex;
            }

        }

        private void LoadFinancialGraph()
        {
            try
            {
                ltScript_FinProgress.Text = string.Empty;

                DataSet ds = getdt.GetFinancialScheduleDatesforGraph(new Guid(DDLWorkPackage.SelectedValue));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    StringBuilder strScript = new StringBuilder();
                    string tablemonths = "<td style=\"width:200px\">&nbsp;</td>";
                    string tmonthlyplan = "<td style=\"padding:3px\">Monthly Plan</td>";
                    string tmonthlyactual = "<td style=\"padding:3px\">Monthly Actual</td>";
                    string tcumulativeplan = "<td style=\"padding:3px\">Cumulative Plan</td>";
                    string tcumulativeactual = "<td style=\"padding:3px\">Cumulative Actual</td>";
                    strScript.Append(@" <script type='text/javascript'>
                
                google.charts.load('current', { packages: ['corechart', 'bar'] });
                google.charts.setOnLoadCallback(drawBasic);

                function drawBasic() {

                var data = google.visualization.arrayToDataTable([
          ['Month', 'Monthly Plan', 'Monthly Actual', 'Cumulative Plan', 'Cumulative Actual'],");
                    int count = 1;
                    DataSet dsvalues = new DataSet();
                    decimal planvalue = 0;
                    decimal actualvalue = 0;
                    decimal cumplanvalue = 0;
                    decimal cumactualvalue = 0;
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        //get the actual and planned values....
                        //dsvalues.Clear();
                        //dsvalues = getdt.GetTaskScheduleValuesForGraph(new Guid(DDLWorkPackage.SelectedValue), Convert.ToDateTime(dr["StartDate"].ToString()), Convert.ToDateTime(dr["StartDate"].ToString()).AddMonths(1));
                        //if (dsvalues.Tables[0].Rows.Count > 0)
                        //{
                        //    planvalue = decimal.Parse(dsvalues.Tables[0].Rows[0]["TotalSchValue"].ToString());
                        //    actualvalue = decimal.Parse(dsvalues.Tables[0].Rows[0]["TotalAchValue"].ToString());
                        //    cumplanvalue += planvalue;
                        //    cumactualvalue += actualvalue;
                        //}
                        dsvalues = getdt.GetFinMonthsPaymentTotal(new Guid(dr["FinMileStoneMonthUID"].ToString()));
                        planvalue = decimal.Parse(dr["AllowedPayment"].ToString());
                        actualvalue = 0;
                        if (dsvalues.Tables[0].Rows.Count > 0)
                        {
                            if (dsvalues.Tables[0].Rows[0]["TotalAmnt"].ToString() != "&nbsp;" && dsvalues.Tables[0].Rows[0]["TotalAmnt"].ToString() != "")
                            {
                                // e.Row.Cells[2].Text = (decimal.Parse(ds.Tables[0].Rows[0]["TotalAmnt"].ToString()) / 10000000).ToString("n2");
                                actualvalue = (decimal.Parse(dsvalues.Tables[0].Rows[0]["TotalAmnt"].ToString()) / 10000000);
                            }
                        }
                        // comment this code..used only for demo since actual values are not available....1
                        //Random random = new Random();
                        //if (planvalue > 0)
                        //{
                        //    System.Threading.Thread.Sleep(1000);
                        //    actualvalue = planvalue - random.Next(2,5);
                        //}

                        //
                        cumplanvalue += planvalue;
                        cumactualvalue += actualvalue;
                        if (count < ds.Tables[0].Rows.Count)
                        {

                            strScript.Append("['" + dr["MonthYear"].ToString() + "'," + planvalue + "," + actualvalue + "," + cumplanvalue + "," + cumactualvalue + "],");
                        }
                        else
                        {
                            strScript.Append("['" + dr["MonthYear"].ToString() + "'," + planvalue + "," + actualvalue + "," + cumplanvalue + "," + cumactualvalue + "]]);");
                        }
                        //
                        tablemonths += "<td style=\"padding:3px\">" + dr["MonthYear"].ToString() + "</td>";
                        tmonthlyplan += "<td style=\"padding:3px\">" + decimal.Round(planvalue, 2) + "</td>";
                        tmonthlyactual += "<td style=\"padding:3px\">" + decimal.Round(actualvalue, 2) + "</td>";

                        tcumulativeplan += "<td style=\"padding:3px\">" + decimal.Round(cumplanvalue, 2) + "</td>";
                        tcumulativeactual += "<td style=\"padding:3px\">" + decimal.Round(cumactualvalue, 2) + "</td>";


                        //
                        count++;
                    }

                    strScript.Append(@"var options = {
          title : 'Plan vs Achieved Progress Curve',
          
          hAxis: {title: 'MONTH',titleTextStyle: {
        bold:'true',
      }},
          seriesType: 'bars',
          series: {2: {type: 'line',targetAxisIndex: 1},3: {type: 'line',targetAxisIndex: 1}},
vAxes: {
            // Adds titles to each axis.
          
            0: {title: 'Monthly Plan (Crores.)',titleTextStyle: {
        bold:'true',
      }},
            1: {title: 'Cumulative Plan (Crores.)',titleTextStyle: {
        bold:'true',
      }}
          }
        };
                var chart = new google.visualization.ComboChart(
                  document.getElementById('chart_divProgressFin'));
                 chart.draw(data, options);
                
            }</script>");
                    //ltScript_Cost.Text = strScript.ToString();
                    ltScript_FinProgress.Text = strScript.ToString();
                    divtableFin.InnerHtml = "<table border=\"1\" style=\"text-align:center;font-size:11px;padding-left:10px;\">" +
                                      "<tr> " + tablemonths + "</tr>" +
                                       "<tr> " + tmonthlyplan + "</tr>" +
                                        "<tr> " + tmonthlyactual + "</tr>" +
                                         "<tr> " + tcumulativeplan + "</tr>" +
                                          "<tr> " + tcumulativeactual + "</tr>" +
                                              "</table>";
                   // btnPrint.Visible = true;
                }
                else
                {
                    ltScript_FinProgress.Text = "<h3>No data</h3>";
                   // divtable.InnerHtml = "";
                   // btnPrint.Visible = false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void LoadFinancialGraphPer()
        {
            try
            {
                ltScript_FinProgress.Text = string.Empty;

                DataSet ds = getdt.GetFinancialScheduleDatesforGraph(new Guid(DDLWorkPackage.SelectedValue));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    StringBuilder strScript = new StringBuilder();
                    string tablemonths = "<td style=\"width:200px\">&nbsp;</td>";
                    string tmonthlyplan = "<td style=\"padding:3px\">Monthly Plan</td>";
                    string tmonthlyactual = "<td style=\"padding:3px\">Monthly Actual</td>";
                    string tcumulativeplan = "<td style=\"padding:3px\">Cumulative Plan</td>";
                    string tcumulativeactual = "<td style=\"padding:3px\">Cumulative Actual</td>";
                    strScript.Append(@" <script type='text/javascript'>
                
                google.charts.load('current', { packages: ['corechart', 'bar'] });
                google.charts.setOnLoadCallback(drawBasic);

                function drawBasic() {

                var data = google.visualization.arrayToDataTable([
          ['Month', 'Monthly Plan', 'Monthly Actual', 'Cumulative Plan', 'Cumulative Actual'],");
                    int count = 1;
                    DataSet dsvalues = new DataSet();
                    decimal planvalue = 0;
                    decimal actualvalue = 0;
                    decimal cumplanvalue = 0;
                    decimal cumactualvalue = 0;
                    decimal Sumplanvalue = 0;
                    foreach (DataRow gvr in getdt.GetFinancialScheduleDatesforGraph(new Guid(DDLWorkPackage.SelectedValue)).Tables[0].Rows)
                    {

                        planvalue = decimal.Parse(gvr["AllowedPayment"].ToString());
                        Sumplanvalue += planvalue;
                    }
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        //get the actual and planned values....
                        //dsvalues.Clear();
                        //dsvalues = getdt.GetTaskScheduleValuesForGraph(new Guid(DDLWorkPackage.SelectedValue), Convert.ToDateTime(dr["StartDate"].ToString()), Convert.ToDateTime(dr["StartDate"].ToString()).AddMonths(1));
                        //if (dsvalues.Tables[0].Rows.Count > 0)
                        //{
                        //    planvalue = decimal.Parse(dsvalues.Tables[0].Rows[0]["TotalSchValue"].ToString());
                        //    actualvalue = decimal.Parse(dsvalues.Tables[0].Rows[0]["TotalAchValue"].ToString());
                        //    cumplanvalue += planvalue;
                        //    cumactualvalue += actualvalue;
                        //}
                        dsvalues = getdt.GetFinMonthsPaymentTotal(new Guid(dr["FinMileStoneMonthUID"].ToString()));
                        planvalue = decimal.Parse(dr["AllowedPayment"].ToString());
                        actualvalue = 0;
                        if (dsvalues.Tables[0].Rows.Count > 0)
                        {
                            if (dsvalues.Tables[0].Rows[0]["TotalAmnt"].ToString() != "&nbsp;" && dsvalues.Tables[0].Rows[0]["TotalAmnt"].ToString() != "")
                            {
                                // e.Row.Cells[2].Text = (decimal.Parse(ds.Tables[0].Rows[0]["TotalAmnt"].ToString()) / 10000000).ToString("n2");
                                actualvalue = (decimal.Parse(dsvalues.Tables[0].Rows[0]["TotalAmnt"].ToString()) / 10000000);
                            }
                        }
                        // comment this code..used only for demo since actual values are not available....1
                        //Random random = new Random();
                        //if (planvalue > 0)
                        //{
                        //    System.Threading.Thread.Sleep(1000);
                        //    actualvalue = planvalue - random.Next(2,5);
                        //}

                        //
                        cumplanvalue += ((planvalue / Sumplanvalue) * 100);
                        cumactualvalue += ((actualvalue / Sumplanvalue) * 100);
                        if (count < ds.Tables[0].Rows.Count)
                        {

                            strScript.Append("['" + dr["MonthYear"].ToString() + "'," + decimal.Round(((planvalue / Sumplanvalue) * 100), 2) + "," + decimal.Round(((actualvalue / Sumplanvalue) * 100), 2) + "," + decimal.Round(cumplanvalue, 2) + "," + decimal.Round(cumactualvalue, 2) + "],");
                        }
                        else
                        {
                            strScript.Append("['" + dr["MonthYear"].ToString() + "'," + decimal.Round(((planvalue / Sumplanvalue) * 100), 2) + "," + decimal.Round(((actualvalue / Sumplanvalue) * 100), 2) + "," + decimal.Round(cumplanvalue, 2) + "," + decimal.Round(cumactualvalue, 2) + "]]);");
                        }
                        //
                        tablemonths += "<td style=\"padding:3px\">" + dr["MonthYear"].ToString() + "</td>";
                        tmonthlyplan += "<td style=\"padding:3px\">" + decimal.Round(((planvalue / Sumplanvalue) * 100), 2) + "</td>";
                        tmonthlyactual += "<td style=\"padding:3px\">" + decimal.Round(((actualvalue / Sumplanvalue) * 100), 2) + "</td>";

                        tcumulativeplan += "<td style=\"padding:3px\">" + decimal.Round(cumplanvalue, 2) + "</td>";
                        tcumulativeactual += "<td style=\"padding:3px\">" + decimal.Round(cumactualvalue, 2) + "</td>";


                        //
                        count++;
                    }

                    if (rdSelect.SelectedValue == "1")
                    {
                        strScript.Append(@"var options = {
          title : 'Plan vs Achieved Progress Curve',
          
          hAxis: {title: 'MONTH',titleTextStyle: {
        bold:'true',
      }},
          seriesType: 'bars',
          series: {2: {type: 'line',targetAxisIndex: 1},3: {type: 'line',targetAxisIndex: 1}},
vAxes: {
            // Adds titles to each axis.
          
            0: {title: 'Monthly Plan (Crores.)',titleTextStyle: {
        bold:'true',
      }},
            1: {title: 'Cumulative Plan (Crores.)',titleTextStyle: {
        bold:'true',
      }}
          }
        };
                var chart = new google.visualization.ComboChart(
                  document.getElementById('chart_divProgressFin'));
                 chart.draw(data, options);
                
            }</script>");
                    }
                    else
                    {
                        strScript.Append(@"var options = {
          title : 'Plan vs Achieved Progress Curve',
          
          hAxis: {title: 'MONTH',titleTextStyle: {
        bold:'true',
      }},
          seriesType: 'bars',
          series: {2: {type: 'line',targetAxisIndex: 1},3: {type: 'line',targetAxisIndex: 1}},
vAxes: {
            // Adds titles to each axis.
          
            0: {title: 'Monthly Plan (%)',titleTextStyle: {
        bold:'true',
      }},
            1: {title: 'Cummulative Plan (%)',titleTextStyle: {
        bold:'true',
      }}
          }
        };
                var chart = new google.visualization.ComboChart(
                  document.getElementById('chart_divProgressFin'));
                 chart.draw(data, options);
                
            }</script>");
                    }
                    //ltScript_Cost.Text = strScript.ToString();
                    ltScript_FinProgress.Text = strScript.ToString();
                    divtableFin.InnerHtml = "<table border=\"1\" style=\"text-align:center;font-size:11px;padding-left:10px;\">" +
                                      "<tr> " + tablemonths + "</tr>" +
                                       "<tr> " + tmonthlyplan + "</tr>" +
                                        "<tr> " + tmonthlyactual + "</tr>" +
                                         "<tr> " + tcumulativeplan + "</tr>" +
                                          "<tr> " + tcumulativeactual + "</tr>" +
                                              "</table>";
                    // btnPrint.Visible = true;
                }
                else
                {
                    ltScript_FinProgress.Text = "<h3>No data</h3>";
                    // divtable.InnerHtml = "";
                    // btnPrint.Visible = false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void rdSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            divFinProgressChart.Visible = false;
            divTabular.Visible = false;
        }

        private void LoadTabularData()
        {
            DataTable dt = new DataTable();
            DataRow dr;
            int count = 1;
            DataSet dsvalues = new DataSet();
            decimal planvalue = 0;
            decimal actualvalue = 0;
            decimal cumplanvalue = 0;
            decimal cumactualvalue = 0;
            decimal Sumplanvalue = 0;
            foreach (DataRow gvr in getdt.GetFinancialScheduleDatesforGraph(new Guid(DDLWorkPackage.SelectedValue)).Tables[0].Rows)
            {
                
                planvalue = decimal.Parse(gvr["AllowedPayment"].ToString());
                Sumplanvalue += planvalue;
            }
            // Add Columns to datatablse
            dr = dt.NewRow();
            dt.Columns.Add(new DataColumn("Months")); //'ColumnName1' represents name of datafield in grid
            dt.Columns.Add(new DataColumn("ProjectedValue"));
            dt.Columns.Add(new DataColumn("AchievedValue"));

            dt.Columns.Add(new DataColumn("ProjectedPer"));
            dt.Columns.Add(new DataColumn("AchievedPer"));
            dt.Columns.Add(new DataColumn("CumulativeProjected"));
            dt.Columns.Add(new DataColumn("CumulativeAchieved"));
            // Get each row from gridview and add it to DataTable
            foreach (DataRow gvr in getdt.GetFinancialScheduleDatesforGraph(new Guid(DDLWorkPackage.SelectedValue)).Tables[0].Rows)
            {
                dr = dt.NewRow();
               
                dsvalues = getdt.GetFinMonthsPaymentTotal(new Guid(gvr["FinMileStoneMonthUID"].ToString()));
                planvalue = decimal.Parse(gvr["AllowedPayment"].ToString());
                actualvalue = 0;
                if (dsvalues.Tables[0].Rows.Count > 0)
                {
                    if (dsvalues.Tables[0].Rows[0]["TotalAmnt"].ToString() != "&nbsp;" && dsvalues.Tables[0].Rows[0]["TotalAmnt"].ToString() != "")
                    {
                        // e.Row.Cells[2].Text = (decimal.Parse(ds.Tables[0].Rows[0]["TotalAmnt"].ToString()) / 10000000).ToString("n2");
                        actualvalue = (decimal.Parse(dsvalues.Tables[0].Rows[0]["TotalAmnt"].ToString()) / 10000000);
                    }
                }
                // comment this code..used only for demo since actual values are not available....1
                //Random random = new Random();
                //if (planvalue > 0)
                //{
                //    System.Threading.Thread.Sleep(1000);
                //    actualvalue = planvalue - random.Next(2,5);
                //}

                //
                cumplanvalue += ((planvalue / Sumplanvalue) * 100);
                cumactualvalue += ((actualvalue / Sumplanvalue) * 100);
                dr["Months"] = gvr["MonthYear"].ToString();
                dr["ProjectedValue"] = decimal.Round(planvalue, 2);
                dr["AchievedValue"] = decimal.Round(actualvalue, 2);
                dr["ProjectedPer"] = decimal.Round(((planvalue/Sumplanvalue) * 100),2);
                dr["AchievedPer"] = decimal.Round(((actualvalue/Sumplanvalue) * 100),2);
                dr["CumulativeProjected"] = decimal.Round(cumplanvalue,2);
                dr["CumulativeAchieved"] = decimal.Round(cumactualvalue,2);
                dt.Rows.Add(dr);
            }
            if (dt.Rows.Count > 0)
            {
                GrdFinancialData.DataSource = dt;
                GrdFinancialData.DataBind();
            }
        }

        protected void GrdFinancialData_DataBound(object sender, EventArgs e)
        {
            try
            {
                if (ViewState["export"].ToString() == "2")
                {
                    GridViewRow row = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
                    TableHeaderCell cell = new TableHeaderCell();
                    cell.Text = "";
                    cell.ColumnSpan = 1;
                    row.Controls.Add(cell);

                    cell = new TableHeaderCell();
                    cell.ColumnSpan = 2;
                    cell.Text = "Financial Progress in Crores";
                    row.Controls.Add(cell);

                    cell = new TableHeaderCell();
                    cell.ColumnSpan = 4;
                    cell.Text = "% of Financial Progress";
                    row.Controls.Add(cell);

                    //row.BackColor = ColorTranslator.FromHtml("#3AC0F2");
                    GrdFinancialData.HeaderRow.Parent.Controls.AddAt(0, row);
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        protected void btnexport_Click(object sender, EventArgs e)
        {
            try
            {
                PDFConvert("No");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void PDFConvert(string isPrint)
        {
            int type = 2;
            DateTime CDate1 = DateTime.Now;
            GridView gdRp = new GridView();

            ViewState["export"] = 1;
            LoadTabularData();
            gdRp = GrdFinancialData;
            ViewState["export"] = 2;
            LoadTabularData();
            int noOfColumns = 0, noOfRows = 0;
            DataTable tbl = null;
            gdRp.Columns[0].HeaderText = "Months";
            gdRp.Columns[1].HeaderText = "Projected (Crores)";
            gdRp.Columns[2].HeaderText = "Achieved (Crores)";
            gdRp.Columns[3].HeaderText = "Projected (%)";
            gdRp.Columns[4].HeaderText = "Achieved (%)";
            gdRp.Columns[5].HeaderText = "Cumulative Projected (%)";
            gdRp.Columns[6].HeaderText = "Cumulative Achieved (%)";

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
            string ProjectName = DDlProject.SelectedItem.ToString() + " (" + DDLWorkPackage.SelectedItem.ToString() + ")";

            if (ProjectName.Length > 30)
            {
                ProjectName = ProjectName.Substring(0, 29) + "..";
            }

            //float HeaderTextSize = 15;
            //float ReportNameSize = 18;
            //float ReportTextSize = 15;
            //float ApplicationNameSize = 20;


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
            Phrase phApplicationName = new Phrase(Environment.NewLine + WebConfigurationManager.AppSettings["Domain"] + " Report Name: Monthly Financial Progress Report", FontFactory.GetFont("Arial", ApplicationNameSize, iTextSharp.text.Font.BOLD));

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
            Phrase phHeader = new Phrase("Project Name :" + ProjectName);
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
                cl.HorizontalAlignment = Element.ALIGN_CENTER;
                mainTable.AddCell(cl);
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
                            if (columnNo == 3)
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
                            else if (columnNo == 5)
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
                                    cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                    mainTable.AddCell(cl);
                                }
                                else
                                {
                                    string s = gdRp.Rows[rowNo].Cells[columnNo].Text.Trim();
                                    s = s.Replace("&nbsp;", "");
                                    Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                                    PdfPCell cl = new PdfPCell(ph);
                                    if (columnNo == 0)
                                    {
                                        cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                    }
                                    else
                                    {
                                        cl.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    }
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
            // 

            // Creates a footer for the PDF document.
            //HeaderFooter pdfFooter = new HeaderFooter(new Phrase(), true);
            //pdfFooter.Alignment = Element.ALIGN_CENTER;
            //pdfFooter.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //pdfFooter.Bottom = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            //// Sets the document footer to pdfFooter.
            //document.Footer = pdfFooter;
            iTextSharp.text.Font foot = new iTextSharp.text.Font();
            foot.Size = 10;

            int len = 174;
            System.Text.StringBuilder time = new System.Text.StringBuilder();
            time.Append(DateTime.Now.ToString("hh:mm tt"));
            time.Append("".PadLeft(len, ' ').Replace(" ", " "));
            HeaderFooter pdfFooter = new HeaderFooter(new Phrase("Date : " + DateTime.Now.ToString("dd/MM/yyyy") + " Time " + time + "  Page: ", foot), new Phrase(Environment.NewLine + " ", foot));
            pdfFooter.Alignment = Element.ALIGN_CENTER;
            //pdfFooter.Border = iTextSharp.text.Rectangle.NO_BORDER;

            //// Sets the document footer to pdfFooter.
            document.Footer = pdfFooter;
            //document.Footer = new HeaderFooter(new Phrase("        Date : " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + "   Page: ", foot), new Phrase(Environment.NewLine + " ", foot));


            // Opens the document.
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
                ////Response.AddHeader("content-disposition", "attachment; filename= SampleExport.pdf");
                Response.AddHeader("content-disposition", "attachment;filename=Report_FinancialProgress_" + DateTime.Now.Ticks + ".pdf");
                Response.End();
            }
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            //required to avoid the runtime error "  
            //Control 'GridView1' of type 'GridView' must be placed inside a form tag with runat=server."  
        }

        protected void btnprint_Click(object sender, EventArgs e)
        {
            PDFConvert("Yes");
        }

        protected void btnexcelexport_Click(object sender, EventArgs e)
        {
            try
            {
                StringBuilder StrExport = new StringBuilder();

                StringWriter stw = new StringWriter();
                HtmlTextWriter htextw = new HtmlTextWriter(stw);
                htextw.AddStyleAttribute("font-size", "9pt");
                htextw.AddStyleAttribute("color", "Black");
                ViewState["export"] = 2;
                LoadTabularData();
              
                GrdFinancialData.RenderControl(htextw); //Name of the Panel

                //var sb1 = new StringBuilder();
                //GridDiv.RenderControl(new HtmlTextWriter(new StringWriter(sb1)));

                string s = htextw.InnerWriter.ToString();

                string HTMLstring = "<html><body>" +
                    "<div style='width:100%; margin:auto;'><div style='width:100%; float:left; line-height:25px; font-size:12pt;' align='center'>" +
                    "<asp:Label ID='Lbl1' runat='server' Font-Bold='true'>" + WebConfigurationManager.AppSettings["Domain"] + " Report Name: Monthly Financial Progress Report</asp:Label><br />" +
                    "<asp:Label ID='Lbl4' runat='server'>Project Name : " + DDlProject.SelectedItem.Text + " (" + DDLWorkPackage.SelectedItem.Text + ")</asp:Label><br />" +
                    "</div> <div style='width:100%; float:left; height:10px;'>&nbsp;&nbsp;&nbsp;</div>" +
                    "<div style='width:100%; float:left;'>" +
                    s +
                    "</div>" +
                    "</div></body></html>";

                string strFile = "Report__FinancialProgress_" + DateTime.Now.Ticks + ".xls";
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
    }
}