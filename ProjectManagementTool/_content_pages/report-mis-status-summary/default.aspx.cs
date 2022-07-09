using iTextSharp.text;
using iTextSharp.text.pdf;
using ProjectManagementTool.DAL;
using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagementTool._content_pages.report_mis_status_summary
{
    public partial class _default : System.Web.UI.Page
    {
        private readonly Dictionary<string, List<string>> projectFlows = new Dictionary<string, List<string>>()
        {
            { "CP-25", new List<string>(){ "Works A", "Works B", "Vendor Approval"}},
            { "CP-26", new List<string>(){ "Works A", "Works B", "Mahadevpura", "Bommanahalli", "Vendor Approval" }},
            { "CP-27", new List<string>(){ "Works A", "Works B", "Vendor Approval"}}
        };

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
                BindData();
            }
        }

        private void BindData()
        {
            
            DateTime startDate = Convert.ToDateTime("01-" + DateTime.Now.ToString("MMM-yyyy"));
            DataTable dataTable = ConstructDataTable();
            //DataTable dtSubmitted = getdt.DocumentSubmittedSummary_ByProject(startDate);
            DataTable dtStatus = getdt.DocumentStatusSummary_ByDate(startDate);

            ReportName.InnerHtml = "MIS Status  Summary Report from June-01 to " + DateTime.Now.ToString("MMMM-dd");
            ViewState["Export"] = "1";
            foreach (string projectName in this.projectFlows.Keys)
            {
                int counter = 0;
                foreach(string flowName in this.projectFlows[projectName])
                {
                    string projectNameDisplay = projectName;
                    if (counter > 0)
                        projectNameDisplay = "";

                    counter++;
                    string submitted = "-", reconciliationPending = "-", reconciliationAccepted = "-", reconciliationRejected = "";
                    string pmcReview = "-", projectCoordinator = "-", meetingWithEE = "-", review = "-";
                    
                    string dtlRejected = "-", dtlInternalMeeting = "-", dtlInternalBackTo = "-", aeApproval = "-", aeeApproval = "-", eeApproval = "-", aceApproval = "-", ceApproval = "-", backToContractorStage = "-";
                    string codeA = "-", codeB = "-", codeC = "-", codeD = "-", clientApproved = "-";
                    submitted = dtStatus.AsEnumerable().Where(r =>
                                                            r.Field<string>("ProjectName") == projectName &&
                                                            r.Field<string>("Flow_Name") == flowName).ToList().Count().ToString();


                    //Reconciliation pending: Status is 'Reconciliation','Contractor Submitted 9 Copies'
                    //Reconciliation accepted: All if its not in pending and Rejected state
                    //Reconciliation rejected: Rejected, Rejected 9 Copies

                    reconciliationPending = dtStatus.AsEnumerable().Where(r => 
                                                            r.Field<string>("ProjectName") == projectName && 
                                                            r.Field<string>("Flow_Name") == flowName &&
                                                            Constants.ReconciliationPendingStatus.Contains(r.Field<string>("ActualDocument_CurrentStatus"))).ToList().Count.ToString();

                    reconciliationAccepted = dtStatus.AsEnumerable().Where(r =>
                                                            r.Field<string>("ProjectName") == projectName &&
                                                            r.Field<string>("Flow_Name") == flowName &&
                                                            !Constants.ReconciliationPendingStatus.Contains(r.Field<string>("ActualDocument_CurrentStatus")) &&
                                                            !r.Field<string>("ActualDocument_CurrentStatus").Contains("Rejected")
                                                            ).ToList().Count.ToString();

                    reconciliationRejected = dtStatus.AsEnumerable().Where(r =>
                                                            r.Field<string>("ProjectName") == projectName &&
                                                            r.Field<string>("Flow_Name") == flowName &&
                                                            !Constants.ReconciliationPendingStatus.Contains(r.Field<string>("ActualDocument_CurrentStatus")) &&
                                                            Constants.ReconciliationRejectedStatus.Contains(r.Field<string>("ActualDocument_CurrentStatus"))
                                                            ).ToList().Count.ToString();

                    //Under PMC review = Accepted, Accepted 9 Copies
                    //Under project coordinator review = Accepted-PMC Comments (Works A), Network Design by ONTB (Works B), Review By ONTB (Vendor approval) stats
                    //Meeting with EE= only Meeting With EE or CE Currect_Status

                    pmcReview = dtStatus.AsEnumerable().Where(r =>
                                                            r.Field<string>("ProjectName") == projectName &&
                                                            r.Field<string>("Flow_Name") == flowName &&
                                                            Constants.PmcReview.Contains(r.Field<string>("ActualDocument_CurrentStatus"))).ToList().Count.ToString();

                    projectCoordinator = dtStatus.AsEnumerable().Where(r =>
                                                            r.Field<string>("ProjectName") == projectName &&
                                                            r.Field<string>("Flow_Name") == flowName &&
                                                            Constants.underProjectCoordinatorReview.Contains(r.Field<string>("ActualDocument_CurrentStatus"))).ToList().Count.ToString();


                    meetingWithEE = dtStatus.AsEnumerable().Where(r =>
                                                            r.Field<string>("ProjectName") == projectName &&
                                                            r.Field<string>("Flow_Name") == flowName &&
                                                            r.Field<string>("ActualDocument_CurrentStatus") == "Meeting With EE or CE").ToList().Count.ToString();

                    //Dtl_review = Recommended-Code A, Recommended-Code B, Recommended-Code C, Recommended-Code D (Works A, B), Network Design Reviewed by Project Co-ordinator
                    //Dtl-rejected =  Rejected by Client, Rejected by PMC = If contains Recommend-Rejected
                    //Dtl-internal meeting = Internal Meeting called by DTL (Works A, B, Vendor approval)

                    review = dtStatus.AsEnumerable().Where(r =>
                                                            r.Field<string>("ProjectName") == projectName &&
                                                            r.Field<string>("Flow_Name") == flowName &&
                                                            Constants.DtlReview.Contains(r.Field<string>("ActualDocument_CurrentStatus"))).ToList().Count.ToString();
                    
                    dtlRejected = dtStatus.AsEnumerable().Where(r =>
                                                            r.Field<string>("ProjectName") == projectName &&
                                                            r.Field<string>("Flow_Name") == flowName &&
                                                            Constants.DtlRejected.Contains(r.Field<string>("ActualDocument_CurrentStatus"))).ToList().Count.ToString();

                    dtlInternalMeeting = dtStatus.AsEnumerable().Where(r =>
                                                            r.Field<string>("ProjectName") == projectName &&
                                                            r.Field<string>("Flow_Name") == flowName &&
                                                            r.Field<string>("ActualDocument_CurrentStatus") == "Internal Meeting called by DTL").ToList().Count.ToString();
                    dtlInternalBackTo = dtStatus.AsEnumerable().Where(r =>
                                                           r.Field<string>("ProjectName") == projectName &&
                                                           r.Field<string>("Flow_Name") == flowName &&
                                                           Constants.DTLBacktoContractor.Contains(r.Field<string>("ActualDocument_CurrentStatus"))).ToList().Count.ToString();
                    
                    //AEE Approval = Code A, Code B(WOrks A), Network Design DTL Reviewed, ONTB DTL Verified
                    //EE Approval = Contains AEE Approval (Works A), 
                    //ACE Approval = Contains EE Approval
                    //CE Approval = Contains ACE approval
                    //Back to contractor stage 1: Network Design EE Approval
                    aeeApproval = dtStatus.AsEnumerable().Where(r =>
                                                            r.Field<string>("ProjectName") == projectName &&
                                                            r.Field<string>("Flow_Name") == flowName &&
                                                            Constants.AeeApproval.Contains(r.Field<string>("ActualDocument_CurrentStatus"))).ToList().Count.ToString();
                    eeApproval = dtStatus.AsEnumerable().Where(r =>
                                                            r.Field<string>("ProjectName") == projectName &&
                                                            r.Field<string>("Flow_Name") == flowName &&

                                                            r.Field<string>("ActualDocument_CurrentStatus").Contains("AEE Approval")).ToList().Count.ToString();
                    aceApproval = dtStatus.AsEnumerable().Where(r =>
                                                            r.Field<string>("ProjectName") == projectName &&
                                                            r.Field<string>("Flow_Name") == flowName &&
                                                            r.Field<string>("ActualDocument_CurrentStatus") != "Network Design EE Approval" &&
                                                            r.Field<string>("ActualDocument_CurrentStatus").Contains(" EE Approval")).ToList().Count.ToString();
                    ceApproval = dtStatus.AsEnumerable().Where(r =>
                                                            r.Field<string>("ProjectName") == projectName &&
                                                            r.Field<string>("Flow_Name") == flowName &&
                                                            r.Field<string>("ActualDocument_CurrentStatus").Contains("ACE Approval")).ToList().Count.ToString();
                    backToContractorStage = dtStatus.AsEnumerable().Where(r =>
                                                            r.Field<string>("ProjectName") == projectName &&
                                                            r.Field<string>("Flow_Name") == flowName &&
                                                            r.Field<string>("ActualDocument_CurrentStatus") == "Network Design EE Approval").ToList().Count.ToString();



                    //Code A = Code A-CE Approval (Works A, vendor)
                    //Code B = Code B-CE Approval (Works A, vendor)
                    //Code C = Code C-CE Approval (Works A, vendor)
                    //Code D = Code D-CE Approval (Works A, vendor)
                    //Client Approved = Client CE GFC Approval (Works B)


                    codeA = dtStatus.AsEnumerable().Where(r =>
                                                            r.Field<string>("ProjectName") == projectName &&
                                                            r.Field<string>("Flow_Name") == flowName &&
                                                            r.Field<string>("ActualDocument_CurrentStatus") == "Code A-CE Approval").ToList().Count.ToString();
                    codeB = dtStatus.AsEnumerable().Where(r =>
                                                            r.Field<string>("ProjectName") == projectName &&
                                                            r.Field<string>("Flow_Name") == flowName &&
                                                            r.Field<string>("ActualDocument_CurrentStatus") == "Code B-CE Approval").ToList().Count.ToString();
                    codeC = dtStatus.AsEnumerable().Where(r =>
                                                            r.Field<string>("ProjectName") == projectName &&
                                                            r.Field<string>("Flow_Name") == flowName &&
                                                            r.Field<string>("ActualDocument_CurrentStatus") == "Code C-CE Approval").ToList().Count.ToString();
                    codeD = dtStatus.AsEnumerable().Where(r =>
                                                            r.Field<string>("ProjectName") == projectName &&
                                                            r.Field<string>("Flow_Name") == flowName &&
                                                            r.Field<string>("ActualDocument_CurrentStatus") == "Code D-CE Approval").ToList().Count.ToString();
                    clientApproved = dtStatus.AsEnumerable().Where(r =>
                                                            r.Field<string>("ProjectName") == projectName &&
                                                            r.Field<string>("Flow_Name") == flowName &&
                                                            r.Field<string>("ActualDocument_CurrentStatus") == "Client CE GFC Approval").ToList().Count.ToString();



                    dataTable.Rows.Add(projectName, projectNameDisplay, flowName, submitted, 
                        reconciliationPending, reconciliationRejected, reconciliationAccepted, pmcReview, projectCoordinator, meetingWithEE, 
                        review, dtlRejected, dtlInternalMeeting,dtlInternalBackTo,aeApproval,aeeApproval, eeApproval, aceApproval, ceApproval, backToContractorStage,
                        clientApproved, codeA, codeB, codeC, codeD);
                }
            }
            grdDataList.DataSource = dataTable;
            grdDataList.DataBind();
        }

        private DataTable ConstructDataTable()
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Project");
            dataTable.Columns.Add("ProjectNameDisplay");
            dataTable.Columns.Add("DocumentType");
            dataTable.Columns.Add("Total Submission");
            dataTable.Columns.Add("Reconciliation Pending");
            dataTable.Columns.Add("Reconciliation Rejected");
            dataTable.Columns.Add("Reconciliation Accepted");
            dataTable.Columns.Add("PMC Review");
            dataTable.Columns.Add("Project Co-ordinator");
            dataTable.Columns.Add("Meeting with EE");
            dataTable.Columns.Add("Review");
            dataTable.Columns.Add("DTL Rejected");
            dataTable.Columns.Add("DTL Internal Meeting");
            dataTable.Columns.Add("DTL Back To Contractor");
            dataTable.Columns.Add("AE Approval");
            dataTable.Columns.Add("AEE Approval");
            dataTable.Columns.Add("EE Approval");
            dataTable.Columns.Add("ACE Approval");
            dataTable.Columns.Add("CE Approval");
            dataTable.Columns.Add("Back to Contractor Stage 1");
            
            dataTable.Columns.Add("Client Approved");
            dataTable.Columns.Add("Code A");
            dataTable.Columns.Add("Code B");
            dataTable.Columns.Add("Code C");
            dataTable.Columns.Add("Code D");
            return dataTable;
        }


        protected void grdDataList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            
        }

        protected void GrdDataList_DataBound(object sender, EventArgs e)
        {
            if (ViewState["Export"].ToString() == "1")
            {
                return;
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
                cell.Text = "Project Co-ordinator";
                cell.ColumnSpan = 2;
                row.Controls.Add(cell);

                cell = new TableHeaderCell();
                cell.Text = "DTL";
                cell.ColumnSpan = 3;
                row.Controls.Add(cell);

                cell = new TableHeaderCell();
                cell.Text = "BWSSB";
                cell.ColumnSpan = 4;
                row.Controls.Add(cell);

                cell = new TableHeaderCell();
                cell.Text = "Works B";
                cell.ColumnSpan = 1;
                row.Controls.Add(cell);

                cell = new TableHeaderCell();
                cell.Text = "CE Approved Till Date";
                cell.ColumnSpan = 5;
                row.Controls.Add(cell);

                //row.BackColor = ColorTranslator.FromHtml("#3AC0F2");
                grdDataList.HeaderRow.Parent.Controls.AddAt(0, row);
            }
        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                StringBuilder StrExport = new StringBuilder();

                StringWriter stw = new StringWriter();
                HtmlTextWriter htextw = new HtmlTextWriter(stw);
                htextw.AddStyleAttribute("font-size", "9pt");
                htextw.AddStyleAttribute("color", "Black");


                grdDataList.RenderControl(htextw); //Name of the Panel

                //var sb1 = new StringBuilder();
                //GridDiv.RenderControl(new HtmlTextWriter(new StringWriter(sb1)));

                string s = htextw.InnerWriter.ToString();

                var splitted = s.Split(new string[] { "default_master_body_grdDataList\">" }, StringSplitOptions.None);

                if (splitted.Length == 2)
                {
                    string header = "<tr style=\"color: White; background-color:#666666;\"><th colspan = \"1\"></th><th colspan = \"1\"></th><th colspan = \"1\"></th><th colspan = \"1\"></th><th colspan = \"1\"></th><th colspan = \"1\"></th><th colspan = \"1\"></th><th colspan = \"2\" >Project Co-ordinator</th><th colspan = \"4\" >DTL</th><th colspan = \"4\" >BWSSB</th><th colspan = \"1\" >Works B</th><th colspan = \"5\" >CE Approved Till Date</th></tr>";

                    string finalOne = splitted[0] + "default_master_body_grdDataList\">" + header + splitted[1];
                    s = finalOne;
                }

                DataTable dt = ConstructDataTable();

                foreach(string projName in this.projectFlows.Keys)
                {
                    foreach(string flowName in this.projectFlows[projName])
                    {
                        for (int counter = 0; counter < dt.Columns.Count; counter++)
                        {
                            s = s.Replace("<a class=\"showModalDocumentView\" href=\"/_content_pages/report-mis-details?ProjectName=" + projName + "&FlowName=" + flowName + "&type=" + dt.Columns[counter].ColumnName + "\">", "");
                        }
                    }
                }

                
                string HTMLstring = "<html><body>" +
                    "<div style='width:100%; margin:auto;'><div style='width:100%; float:left; line-height:25px; font-size:12pt;' align='center'>" +
                    "<asp:Label ID='Lbl1' runat='server' Font-Bold='true'> Report Name: MIS Status  Summary Report from 01-June" + " to " + DateTime.Now.ToString("dd-MMM") + "</asp:Label><br />" +
                    "</div> <div style='width:100%; float:left; height:10px;'>&nbsp;&nbsp;&nbsp;</div>" +
                    "<div style='width:100%; float:left;'>" +
                    s +
                    "</div>" +
                    "</div></body></html>";

                string strFile = "Report_MIS_Status_" + DateTime.Now.Ticks + ".xls";
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
        public override void VerifyRenderingInServerForm(Control control)
        {
            //required to avoid the runtime error "  
            //Control 'GridView1' of type 'GridView' must be placed inside a form tag with runat=server."  
        }

        protected void btnPDF_Click(object sender, EventArgs e)
        {
            ExporttoPDF(grdDataList, 2, "No");
        }

        private void ExporttoPDF(GridView gd, int type, string isPrint)
        {

            DateTime CDate1 = DateTime.Now;
            GridView gdRp = new GridView();


            gdRp = gd;

            int noOfColumns = 0, noOfRows = 0;
            DataTable tbl = null;


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
            string ProjectName = "";


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
            Phrase phApplicationName = new Phrase(Environment.NewLine + WebConfigurationManager.AppSettings["Domain"] + " Report Name: MIS Details report from June 1 to " + DateTime.Now.ToString("MMMM dd"), FontFactory.GetFont("Arial", ApplicationNameSize, iTextSharp.text.Font.BOLD));

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
            Phrase phHeader = new Phrase("");
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
                            if (gdRp.Columns[columnNo] is TemplateField)
                            {
                                DataBoundLiteralControl lc = gdRp.Rows[rowNo].Cells[columnNo].Controls[0] as DataBoundLiteralControl;
                                string s = lc.Text.Trim();
                                var splittedValues = s.Split('>');
                                if(splittedValues.Length == 3)
                                {
                                    s = splittedValues[1].Replace("</a", "").Trim();
                                }
                                else
                                    s = Regex.Replace(s, "(<a [^>]*>).*?(</a>)", string.Empty);
                                Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                                PdfPCell cl = new PdfPCell(ph);
                                cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                mainTable.AddCell(cl);
                            }
                            else
                            {
                                string s = gdRp.Rows[rowNo].Cells[columnNo].Text.Trim();
                                s = s.Replace("&nbsp;", "");
                                s = Regex.Replace(s, "(<a [^>]*>).*?(</a>)", string.Empty);
                                Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                                PdfPCell cl = new PdfPCell(ph);
                                cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                mainTable.AddCell(cl);
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
                Response.AddHeader("content-disposition", "attachment;filename=Report_MIS_Status_" + DateTime.Now.Ticks + ".pdf");
                Response.End();
            }
        }

        protected void tnPrint_Click(object sender, EventArgs e)
        {
            ExporttoPDF(grdDataList, 2, "Yes");
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            Response.Redirect("default.aspx");
        }
    }
}