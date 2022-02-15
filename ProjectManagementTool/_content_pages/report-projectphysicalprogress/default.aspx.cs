using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System.IO;
using System.Web.Configuration;
using System.Text;
using System.Globalization;

namespace ProjectManagementTool._content_pages.report_projectphysicalprogress
{
    public partial class _default : System.Web.UI.Page
    {
        DBGetData getdt = new DBGetData();
        TaskUpdate gettk = new TaskUpdate();
        float TargetedOverallTotal = 0;
        float AchievedOverallTotal = 0;
        float ExpenditureOverallTotal = 0;
        float AwardedValue = 0;
        float ExpenditureCost = 0;
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
                    BindMeetingMaster();
                    DDLMeetingMaster_SelectedIndexChanged(sender, e);
                }
            }
        }

        private void BindMeetingMaster()
        {
            DataSet ds = getdt.GetMeetingMasters();
            DDLMeetingMaster.DataTextField = "Meeting_Description";
            DDLMeetingMaster.DataValueField = "Meeting_UID";
            DDLMeetingMaster.DataSource = ds;
            DDLMeetingMaster.DataBind();
        }

        private void BindPhysicalProgress(string MeetingUID)
        {
            DataSet ds = new DataSet();
            ds = getdt.GetProjectPhysicalProgress_by_Meeting_UID(new Guid(MeetingUID));
            GrdPhysicalProgress.DataSource = ds;
            GrdPhysicalProgress.DataBind();
            if (ds.Tables[0].Rows.Count > 0)
            {
                // btnexport.Visible = true;
                btnExportNew.Visible = true;
            }
            else
            {
                btnexport.Visible = false;
                btnExportNew.Visible = false;
            }
        }

        public string GetProjectName(string ProjectUID)
        {
            return getdt.getProjectNameby_ProjectUID(new Guid(ProjectUID));
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            //required to avoid the runtime error "  
            //Control 'GridView1' of type 'GridView' must be placed inside a form tag with runat=server."  
        }

        protected void btnexport_Click(object sender, EventArgs e)
        {
            string sDate1 = "";
            DateTime CDate1 = DateTime.Now;

            sDate1 = CDate1.ToString("dd/MM/yyyy");
            sDate1 = getdt.ConvertDateFormat(sDate1);
            CDate1 = Convert.ToDateTime(sDate1);

            //to allow paging=false & change style.
            GrdPhysicalProgress.HeaderStyle.ForeColor = System.Drawing.Color.Black;
            GrdPhysicalProgress.BorderColor = System.Drawing.Color.Black;
            GrdPhysicalProgress.Font.Name = "Arial";
            GrdPhysicalProgress.DataSource = getdt.GetProjectPhysicalProgress_by_Meeting_UID(new Guid(DDLMeetingMaster.SelectedValue));
            GrdPhysicalProgress.AllowPaging = true;
            //GrdDocumentMaster.Columns[0].Visible = false; //export won't work if there's a link in the gridview
            GrdPhysicalProgress.DataBind();

            StringWriter stw = new StringWriter();
            HtmlTextWriter htextw = new HtmlTextWriter(stw);
            htextw.AddStyleAttribute("font-size", "9pt");
            htextw.AddStyleAttribute("color", "Black");

            GrdPhysicalProgress.RenderControl(htextw); //Name of the Panel

            //var sb1 = new StringBuilder();
            //GridDiv.RenderControl(new HtmlTextWriter(new StringWriter(sb1)));

            string s = htextw.InnerWriter.ToString();
            string HTMLstring = "<html><head><meta http-equiv='Content-Type' content='text/html; charset=utf-8' /></head> <body>" +
                "<div style='width:100%; margin:auto;'><div style='width:100%; float:left; line-height:25px;' align='center'>";
            HTMLstring += "<h2>" + WebConfigurationManager.AppSettings["Domain"] + "</h2>";
            HTMLstring += "<h3>Report Name: Physical Progress Achieved</h3>" +
            "</div> <div style='width:100%; float:left;'><br/><br/></div>";
            HTMLstring += "<div style='width:100%; float:left;'><h4>Physical Progress achieved upto " + CDate1.ToString("MMM yyyy") + "</h4><br/><br/></div>";
            HTMLstring += "<div style='width:100%; float:left;'>" +
            s +
            "</div>";
            HTMLstring += "</div></body></html>";


            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                {
                    StringBuilder sb = new StringBuilder();

                    //Export HTML String as PDF.
                    StringReader sr = new StringReader(HTMLstring);
                    Document pdfDoc = new Document(PageSize.A4.Rotate(), 15f, 15f, 10f, 10f);

                    iTextSharp.text.Font foot = new iTextSharp.text.Font();
                    foot.Size = 9;
                    HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                    pdfDoc.Footer = new HeaderFooter(new Phrase("Date : " + DateTime.Now.ToString("dd/MM/yyyy") + "                      Page: ", foot), true);
                    
                    pdfDoc.Open();
                    htmlparser.Parse(sr);
                    pdfDoc.Close();
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "attachment;filename=Report_PhysicalProgress_" + DateTime.Now.Ticks + ".pdf");
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.Write(pdfDoc);
                    Response.End();

                }
            }
        }

        protected void GrdPhysicalProgress_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblTargeted = (Label)e.Row.FindControl("LblTargetedProgress");
                Label lblAchieved = (Label)e.Row.FindControl("LblAchievedProgress");
                Label lblAwardedValue = (Label)e.Row.FindControl("lblAwardedValue");
                Label lblExpenditure = (Label)e.Row.FindControl("LblExpenditure");
                if (lblTargeted.Text != "")
                {
                    TargetedOverallTotal += float.Parse(lblTargeted.Text);
                }

                if (lblAchieved.Text != "")
                {
                    AchievedOverallTotal += float.Parse(lblAchieved.Text);
                }
                if (lblAwardedValue.Text != "")
                {
                    AwardedValue += float.Parse(lblAwardedValue.Text);
                }
                if (lblExpenditure.Text != "")
                {
                    ExpenditureCost += float.Parse(lblExpenditure.Text);
                }
                lblAchieved.Visible = false;
                lblTargeted.Visible = false;
                lblAwardedValue.Visible = false;
                lblExpenditure.Visible = false;
                Session["AwardedValue"] = AwardedValue.ToString("N2") ;
                Session["AchievedOverallTotal"] = AchievedOverallTotal.ToString("N2");
                Session["TargetedOverallTotal"] = TargetedOverallTotal.ToString("N2");
                Session["ExpenditureOverallTotal"] = ExpenditureCost.ToString("N2");
            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblTargeted = (Label)e.Row.FindControl("LblTargetedOverallTotal");
                Label lblAchieved = (Label)e.Row.FindControl("LblAchievedOverallTotal");
                Label lblAwardedtotal = (Label)e.Row.FindControl("lblAwardedtotal");
                Label lblExpenditureTotal = (Label)e.Row.FindControl("LblExpenditure_Total");
                lblTargeted.Text = TargetedOverallTotal.ToString("N2");
                lblAchieved.Text = AchievedOverallTotal.ToString("N2");
                lblAwardedtotal.Text = AwardedValue.ToString("N2");
                lblExpenditureTotal.Text = ExpenditureCost.ToString("N2");
            }
        }

        protected void DDLMeetingMaster_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDLMeetingMaster.SelectedValue != "")
            {
                BindPhysicalProgress(DDLMeetingMaster.SelectedValue);
            }
        }


        //private void ExportToPDF()
        //{
        //    this.BindGrid();
        //    //Set the Size of PDF document.
        //    Rectangle rect = new Rectangle(500, 300);
        //    Document pdfDoc = new Document(rect, 10f, 10f, 10f, 0f);

        //    //Initialize the PDF document object.
        //    PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
        //    pdfDoc.Open();
        //    //Loop through GridView Pages.
        //    for (int i = 0; i < GridView1.PageCount; i++)
        //    {
        //        //Set the Page Index.
        //        GridView1.PageIndex = i;

        //        //Hide Page as not needed in PDF.
        //        GridView1.PagerSettings.Visible = false;

        //        //Populate the GridView with records for the Page Index.
        //        this.BindGrid();

        //        //Render the GridView as HTML and add to PDF.
        //        using (StringWriter sw = new StringWriter())
        //        {
        //            using (HtmlTextWriter hw = new HtmlTextWriter(sw))
        //            {
        //                GridView1.RenderControl(hw);
        //                HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
        //                StringReader sr = new StringReader(sw.ToString());
        //                htmlparser.Parse(sr);
        //            }
        //        }

        //        //Add a new Page to PDF document.
        //        pdfDoc.NewPage();
        //    }
        //    //Close the PDF document.
        //    pdfDoc.Close();

        //    //Download the PDF file.
        //    Response.ContentType = "application/pdf";
        //    Response.AddHeader("content-disposition", "attachment;filename=GridViewExport.pdf");
        //    Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //    Response.Write(pdfDoc);
        //    Response.End();
        //}

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblTargeted = (Label)e.Row.FindControl("LblTargetedProgress");
                Label lblAchieved = (Label)e.Row.FindControl("LblAchievedProgress");
                Label lblAwardedValue = (Label)e.Row.FindControl("lblAwardedValue");
                if (lblTargeted.Text != "")
                {
                    TargetedOverallTotal += float.Parse(lblTargeted.Text);
                }

                if (lblAchieved.Text != "")
                {
                    AchievedOverallTotal += float.Parse(lblAchieved.Text);
                }
                if (lblAwardedValue.Text != "")
                {
                    AwardedValue += float.Parse(lblAwardedValue.Text);
                }
            
            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblTargeted = (Label)e.Row.FindControl("LblTargetedOverallTotal");
                Label lblAchieved = (Label)e.Row.FindControl("LblAchievedOverallTotal");
                Label lblAwardedtotal = (Label)e.Row.FindControl("lblAwardedtotal");
                lblTargeted.Text = TargetedOverallTotal.ToString("N2");
                lblAchieved.Text = AchievedOverallTotal.ToString("N2");
                lblAwardedtotal.Text = AwardedValue.ToString("N2");
            }
        }

        private void ExporttoPDF()
        {
            try
            {
                string sDate1 = "";
                DateTime CDate1 = DateTime.Now;

                //sDate1 = CDate1.ToString("dd/MM/yyyy");
                DataSet dsmeeting = getdt.GetMeetingMaster_by_Meeting_UID(new Guid(DDLMeetingMaster.SelectedValue));
                if (dsmeeting.Tables[0].Rows.Count > 0)
                {
                    sDate1 = Convert.ToDateTime(dsmeeting.Tables[0].Rows[0]["CreatedDate"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                sDate1 = getdt.ConvertDateFormat(sDate1);
                CDate1 = Convert.ToDateTime(sDate1);
                int noOfColumns = 0, noOfRows = 0;
                DataTable tbl = null;

                if (GrdPhysicalProgress.AutoGenerateColumns)
                {
                    tbl = GrdPhysicalProgress.DataSource as DataTable; // Gets the DataSource of the GridView Control.
                    noOfColumns = tbl.Columns.Count;
                    noOfRows = tbl.Rows.Count;
                }
                else
                {
                    noOfColumns = GrdPhysicalProgress.Columns.Count;
                    noOfRows = GrdPhysicalProgress.Rows.Count;
                }

                float HeaderTextSize = 9;
                float ReportNameSize = 14;
                float ReportTextSize =9;
                float ApplicationNameSize = 16;

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
                Phrase phApplicationName = new Phrase(WebConfigurationManager.AppSettings["Domain"], FontFactory.GetFont("Arial", ApplicationNameSize, iTextSharp.text.Font.BOLD));

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
                Phrase phHeader = new Phrase("Report Name : Physical Progress achieved upto " + CDate1.ToString("MMM yyyy"), FontFactory.GetFont("Arial", ReportNameSize, iTextSharp.text.Font.NORMAL));
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

                    if (GrdPhysicalProgress.AutoGenerateColumns)
                    {
                        ph = new Phrase(tbl.Columns[i].ColumnName.Replace("<br/>", Environment.NewLine), FontFactory.GetFont("Arial", HeaderTextSize, iTextSharp.text.Font.BOLD));
                    }
                    else
                    {
                        ph = new Phrase(GrdPhysicalProgress.Columns[i].HeaderText.Replace("<br/>", Environment.NewLine), FontFactory.GetFont("Arial", HeaderTextSize, iTextSharp.text.Font.BOLD));
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
                            if (GrdPhysicalProgress.AutoGenerateColumns)
                            {
                                string s = GrdPhysicalProgress.Rows[rowNo].Cells[columnNo].Text.Trim();
                                Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                                PdfPCell cl = new PdfPCell(ph);
                                cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                mainTable.AddCell(cl);
                            }
                            else
                            {
                                if (columnNo == 3)
                                {
                                    if (GrdPhysicalProgress.Columns[columnNo] is TemplateField)
                                    {
                                        DataBoundLiteralControl lc = GrdPhysicalProgress.Rows[rowNo].Cells[columnNo].Controls[0] as DataBoundLiteralControl;
                                        string s = lc.Text.Trim();
                                        Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                                        PdfPCell cl = new PdfPCell(ph);
                                        cl.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        mainTable.AddCell(cl);
                                    }
                                    else
                                    {
                                        string s = GrdPhysicalProgress.Rows[rowNo].Cells[columnNo].Text.Trim();
                                        s = s.Replace("&nbsp;", "");
                                        Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                                        PdfPCell cl = new PdfPCell(ph);
                                        cl.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        mainTable.AddCell(cl);
                                    }
                                }
                                else if (columnNo == 5)
                                {
                                    if (GrdPhysicalProgress.Columns[columnNo] is TemplateField)
                                    {
                                        DataBoundLiteralControl lc = GrdPhysicalProgress.Rows[rowNo].Cells[columnNo].Controls[0] as DataBoundLiteralControl;
                                        string s = lc.Text.Trim();
                                        Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                                        PdfPCell cl = new PdfPCell(ph);
                                        cl.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        mainTable.AddCell(cl);
                                    }
                                    else
                                    {
                                        string s = GrdPhysicalProgress.Rows[rowNo].Cells[columnNo].Text.Trim();
                                        s = s.Replace("&nbsp;", "");
                                        Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                                        PdfPCell cl = new PdfPCell(ph);
                                        cl.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        mainTable.AddCell(cl);
                                    }
                                }
                                else
                                {
                                    if (GrdPhysicalProgress.Columns[columnNo] is TemplateField)
                                    {
                                        DataBoundLiteralControl lc = GrdPhysicalProgress.Rows[rowNo].Cells[columnNo].Controls[0] as DataBoundLiteralControl;
                                        string s = lc.Text.Trim();
                                        Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                                        PdfPCell cl = new PdfPCell(ph);
                                        cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                        mainTable.AddCell(cl);
                                    }
                                    else
                                    {
                                        string s = GrdPhysicalProgress.Rows[rowNo].Cells[columnNo].Text.Trim();
                                        s = s.Replace("&nbsp;", "");
                                        Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                                        PdfPCell cl = new PdfPCell(ph);
                                        cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                        mainTable.AddCell(cl);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int columnNo = 0; columnNo < noOfColumns; columnNo++)
                        {
                            string s = "Total";
                            if (columnNo == 2)
                            {
                                Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.BOLD));
                                PdfPCell cl = new PdfPCell(ph);
                                cl.HorizontalAlignment = Element.ALIGN_RIGHT;
                                mainTable.AddCell(cl);
                            }
                            else if (columnNo == 3)
                            {
                                s = Session["AwardedValue"].ToString();
                                Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.BOLD));
                                PdfPCell cl = new PdfPCell(ph);
                                cl.HorizontalAlignment = Element.ALIGN_RIGHT;
                                mainTable.AddCell(cl);
                            }
                            else if (columnNo == 5)
                            {
                                s = Session["ExpenditureOverallTotal"].ToString();
                                Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.BOLD));
                                PdfPCell cl = new PdfPCell(ph);
                                cl.HorizontalAlignment = Element.ALIGN_RIGHT;
                                mainTable.AddCell(cl);
                            }
                            else if (columnNo == 7)
                            {
                                s = Session["TargetedOverallTotal"].ToString() + "%";
                                Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.BOLD));
                                PdfPCell cl = new PdfPCell(ph);
                                cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                mainTable.AddCell(cl);
                            }
                            else if (columnNo == 9)
                            {
                                s = Session["AchievedOverallTotal"].ToString() + "%";
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

                    // Tells the mainTable to complete the row even if any cell is left incomplete.
                    mainTable.CompleteRow();
                }

                // Gets the instance of the document created and writes it to the output stream of the Response object.
                PdfWriter.GetInstance(document, Response.OutputStream);

                // Creates a footer for the PDF document.
                //HeaderFooter pdfFooter = new HeaderFooter(new Phrase(), true);
                //pdfFooter.Alignment = Element.ALIGN_CENTER;
                //pdfFooter.Border = iTextSharp.text.Rectangle.NO_BORDER;

                //// Sets the document footer to pdfFooter.
                //document.Footer = pdfFooter;
                int len = 174;
                System.Text.StringBuilder time = new System.Text.StringBuilder();
                time.Append(DateTime.Now.ToString("hh:mm tt"));
                time.Append("".PadLeft(len, ' ').Replace(" ", " "));

                iTextSharp.text.Font foot = new iTextSharp.text.Font();
                foot.Size = 10;
                HeaderFooter pdfFooter = new HeaderFooter(new Phrase("Date : " + DateTime.Now.ToString("dd/MM/yyyy") + " Time " + time + "  Page: ", foot), new Phrase(Environment.NewLine + " ", foot));
                pdfFooter.Alignment = Element.ALIGN_CENTER;
                document.Footer = pdfFooter;

                // Opens the document.
                document.Open();
                // Adds the mainTable to the document.
                document.Add(mainTable);
                // Closes the document.
                document.Close();

                Response.ContentType = "application/pdf";
                //Response.AddHeader("content-disposition", "attachment; filename= SampleExport.pdf");
                Response.AddHeader("content-disposition", "attachment;filename=Report_PhysicalProgress_achieved_upto_" + CDate1.ToString("MMM-yyyy") + "_" + DateTime.Now.Ticks + ".pdf");
                Response.End();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnExportNew_Click(object sender, EventArgs e)
        {
            ExporttoPDF();
        }

        //protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        //{
        //    GridView1.PageIndex = e.NewPageIndex;
        //}

        //public void BindGrid()
        //{
        //    DataSet ds = new DataSet();
        //    ds = getdt.GetProjectPhysicalProgress_by_Meeting_UID(new Guid(DDLMeetingMaster.SelectedValue));
        //    GridView1.DataSource = ds;
        //    GridView1.DataBind();
        //}
    }
}