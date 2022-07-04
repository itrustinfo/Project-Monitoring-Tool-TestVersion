using iTextSharp.text;
using iTextSharp.text.pdf;
using ProjectManagementTool.BLL;
using ProjectManager.DAL;
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

namespace ProjectManagementTool._content_pages.report_reconciliation_status
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
                ReportFilter.Visible = false;
                BindStatus();
                BindProject();
                BindFlow();
                DDlProject_SelectedIndexChanged(sender, e);
            }
        }

        private void BindStatus()
        {
            DDLStatus.Items.Add("All");
            DDLStatus.Items.Add("Accepted");
            DDLStatus.Items.Add("Rejected");
        }

        void BindFlow()
        {
            DataTable ds = getdt.GetDocumentFlow().AsEnumerable().Where(r => r.Field<string>("Flow_Name").Contains("Works A") || r.Field<string>("Flow_Name").Contains("Works B") ||  r.Field<string>("Flow_Name").Contains("Vendor Approval")).CopyToDataTable();
            if (ds != null && ds.Rows.Count > 0)
            {
                DDLFlow.DataTextField = "Flow_Name";
                DDLFlow.DataValueField = "FlowMasterUID";
                DDLFlow.DataSource = ds;
                DDLFlow.DataBind();
                DDLFlow.Items.Insert(0, "All");
                ViewState["Flow"] = ds;
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
                //DataSet ds = getdt.GetWorkPackages_By_ProjectUID(new Guid(DDlProject.SelectedValue));

                divTabular.Visible = false;
                ReportFilter.Visible = true;
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

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (DDlProject.SelectedValue != "")
            {
                grdDataList.DataSource = getdt.GetAllDocumentExceptReconciliation(new Guid(DDlProject.SelectedValue), txtOntbReference.Text, txtProjectRefernce.Text, DDLStatus.SelectedValue,DDLFlow.SelectedValue);
                if (getdt.GetAllDocumentExceptReconciliation(new Guid(DDlProject.SelectedValue), txtOntbReference.Text, txtProjectRefernce.Text, DDLStatus.SelectedValue,DDLFlow.SelectedValue).Tables.Count != 0)
                {
                    grdDataList.DataBind();
                    divTabular.Visible = true;
                }
            }


        }
        protected void grdDataList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                if(WebConfigurationManager.AppSettings["Domain"].ToString().ToUpper() == "NJSEI")
                    e.Row.Cells[5+2].Text = "NJSEI Reference #";
                else
                    e.Row.Cells[5+2].Text = "ONTB Reference #";
            }
            else if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (!e.Row.Cells[4+2].Text.Contains("Rejected"))
                {
                    // e.Row.Cells[4].Text = "Accepted";
                    e.Row.Cells[10+2].Text = getdt.GetUSP_DocumentAcceptedRejectedRemarks(new Guid(e.Row.Cells[0+2].Text));
                }
                else
                {
                    e.Row.Cells[10+2].Text = getdt.GetUSP_DocumentAcceptedRejectedRemarks(new Guid(e.Row.Cells[0+2].Text));
                }
                DateTime? acceptedRejDate = getdt.GetDocumentAcceptedRecejtedDate(new Guid(e.Row.Cells[0+2].Text));
                if(acceptedRejDate != null)
                {
                    e.Row.Cells[9+2].Text = Convert.ToDateTime(acceptedRejDate).ToString("dd/MM/yyyy");
                }
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

                grdDataList.Columns[0].Visible = false;
                grdDataList.RenderControl(htextw); //Name of the Panel

                //var sb1 = new StringBuilder();
                //GridDiv.RenderControl(new HtmlTextWriter(new StringWriter(sb1)));

                string s = htextw.InnerWriter.ToString();

                string HTMLstring = "<html><body>" +
                    "<div style='width:100%; margin:auto;'><div style='width:100%; float:left; line-height:25px; font-size:12pt;' align='center'>" +
                    "<asp:Label ID='Lbl1' runat='server' Font-Bold='true'>" + WebConfigurationManager.AppSettings["Domain"] + " Report Name: Reconciliation Approved or Rejected for " + DDlProject.SelectedItem.ToString() + "</asp:Label><br />" +
                    "</div> <div style='width:100%; float:left; height:10px;'>&nbsp;&nbsp;&nbsp;</div>" +
                    "<div style='width:100%; float:left;'>" +
                    s +
                    "</div>" +
                    "</div></body></html>";

                string strFile = "Report_Reconciliation_Status_" + DateTime.Now.Ticks + ".xls";
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
            gd.Columns[0].Visible = false;
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
            Phrase phApplicationName = new Phrase(Environment.NewLine + WebConfigurationManager.AppSettings["Domain"] + " Report Name: Reconciliation Approved or Rejected for "  + DDlProject.SelectedItem.ToString(), FontFactory.GetFont("Arial", ApplicationNameSize, iTextSharp.text.Font.BOLD));

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
                            if (columnNo == 3)
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
                            else if (columnNo == 5)
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
                                        cl.HorizontalAlignment = Element.ALIGN_CENTER;
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
                Response.AddHeader("content-disposition", "attachment;filename=Report_Reconciliation_Status_" + DateTime.Now.Ticks + ".pdf");
                Response.End();
            }
        }

        protected void tnPrint_Click(object sender, EventArgs e)
        {
            ExporttoPDF(grdDataList, 2, "Yes");
        }
    }
}