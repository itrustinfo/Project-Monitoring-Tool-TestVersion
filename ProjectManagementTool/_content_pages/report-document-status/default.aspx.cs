using iTextSharp.text;
using iTextSharp.text.pdf;
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

namespace ProjectManagementTool._content_pages.report_document_status
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
            else
            {
                if (!IsPostBack)
                {
                    BindProject();
                }
            }
        }

        private void BindProject()
        {
            List<string> projects = new List<string>() { "CP-25", "CP-26", "CP-27"};

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

            var dsProjects = ds.AsEnumerable().Where(r => projects.Contains(r.Field<string>("ProjectName")));
            if(dsProjects.FirstOrDefault() == null)
            {
                ds = new DataTable();
            }
            else
            {
                ds = dsProjects.CopyToDataTable();
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

                    BindDocumentSummary();
                }
            }

        }

        protected void DDLWorkPackage_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDLWorkPackage.SelectedValue != "")
            {
                BindDocumentSummary();
            }
        }

        private DataTable AddDataRow(DataTable dt, int slNo, string document, string contractor, string bwssb, string ontb)
        {
            DataRow dr = dt.NewRow();
            dr["Sl_No"] = slNo;
            dr["Documents"] = document;
            dr["Contractor"] = contractor;
            dr["BWSSB"] = bwssb;
            dr["ONTB"] = ontb;
            dt.Rows.Add(dr);
            return dt;
        }
        protected void BindDocumentSummary()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Sl_No");
            dt.Columns.Add("Documents");
            dt.Columns.Add("Contractor");
            dt.Columns.Add("BWSSB");
            dt.Columns.Add("ONTB");
            DataSet ds = getdt.GetDocumentSummary_PMC_by_WorkpackgeUID(new Guid(DDlProject.SelectedValue), new Guid(DDLWorkPackage.SelectedValue));
            if (ds.Tables[0].Rows.Count > 0)
            {
                btnDocumentSummaryExportPDF.Visible = true;
                btnDocumentSummaryPrint.Visible = true;
                btnDocumentSummaryExportExcel.Visible = true;
                DocumentSummaryReportName.InnerHtml = "Report Name : Status of Documents";
                DocumentSummaryProjectName.InnerHtml = "Project Name : " + DDlProject.SelectedItem.Text + " (" + DDLWorkPackage.SelectedItem.Text + ")";

                
                dt = AddDataRow(dt, 1, "No. of Document in Submitted status(Flow 1 & Flow 2)", ds.Tables[0].Rows[0]["SubmittedDocumentsContractor"].ToString(), "-", ds.Tables[0].Rows[0]["SubmittedDocumentsOntb"].ToString());
                dt = AddDataRow(dt, 2, "No. of Documemts under Reconciliation status", "-", "-", ds.Tables[0].Rows[0]["ReconciliationONTB"].ToString());
                dt = AddDataRow(dt, 3, "No. of Documents under Rejected status", "-", ds.Tables[0].Rows[0]["RejectedBwssb"].ToString(), ds.Tables[0].Rows[0]["RejectedOntb"].ToString());
                dt = AddDataRow(dt, 4, "No. of Documents submitted by contractror(Works A, Works B & Vendor Approval)", ds.Tables[0].Rows[0]["SubmittedDocumentsContractorWorksA"].ToString(), "-" , "-");
                

                dt = AddDataRow(dt, 5, "No. of Documemts under category A", "-", ds.Tables[0].Rows[0]["CodeABwssb"].ToString(), ds.Tables[0].Rows[0]["CodeAOntb"].ToString());
                dt = AddDataRow(dt, 6, "No. of Documemts under category B", "-", ds.Tables[0].Rows[0]["CodeBBwssb"].ToString(), ds.Tables[0].Rows[0]["CodeBOntb"].ToString());
                dt = AddDataRow(dt, 7, "No. of Documemts under category C", "-", ds.Tables[0].Rows[0]["CodeCBwssb"].ToString(), ds.Tables[0].Rows[0]["CodeCOntb"].ToString());
                dt = AddDataRow(dt, 8, "No. of Documemts under category D", "-", ds.Tables[0].Rows[0]["CodeDBwssb"].ToString(), ds.Tables[0].Rows[0]["CodeDOntb"].ToString());
                dt = AddDataRow(dt, 9, "No. of Documemts under category E", "-", ds.Tables[0].Rows[0]["CodeEBwssb"].ToString(), ds.Tables[0].Rows[0]["CodeEOntb"].ToString());
                dt = AddDataRow(dt, 10, "No. of Documemts under category F", "-", ds.Tables[0].Rows[0]["CodeFBwssb"].ToString(), ds.Tables[0].Rows[0]["CodeFOntb"].ToString());
                dt = AddDataRow(dt, 11, "No. of Documemts under category G", "-", ds.Tables[0].Rows[0]["CodeGBwssb"].ToString(), ds.Tables[0].Rows[0]["CodeGOntb"].ToString());
                dt = AddDataRow(dt, 12, "No. of Documemts under category H", "-", ds.Tables[0].Rows[0]["CodeHBwssb"].ToString(), ds.Tables[0].Rows[0]["CodeHOntb"].ToString());
                dt = AddDataRow(dt, 13, "Client Approved Documents(Flow 2)", "-", ds.Tables[0].Rows[0]["ClientApproved"].ToString(), "-");


                ViewState["Export"] = "1";
                GrdDocumentSummary.DataSource = dt;
                GrdDocumentSummary.DataBind();

            }
        }
        protected void GrdDocumentSummary_DataBound(object sender, EventArgs e)
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
                cell.Text = "Status of Documents";
                cell.ColumnSpan = 3;
                row.Controls.Add(cell);

                //row.BackColor = ColorTranslator.FromHtml("#3AC0F2");
                GrdDocumentSummary.HeaderRow.Parent.Controls.AddAt(0, row);
            }
        }

        protected void GrdDocumentSummary_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.Header)
            //{
            //    string pCode = getdt.GetClientCodebyWorkpackageUID(new Guid(DDLWorkPackage.SelectedValue));
            //    if (pCode.StartsWith("Error:"))
            //    {
            //        pCode = "Approved by BWSSB";
            //    }
            //    else
            //    {
            //        e.Row.Cells[4].Text = "Approved by " + pCode;
            //    }
            //}
        }

        protected void btnDocumentSummaryExportExcel_Click(object sender, EventArgs e)
        {
            try
            {
                ViewState["Export"] = "2";
                BindDocumentSummary();
                StringBuilder StrExport = new StringBuilder();

                StringWriter stw = new StringWriter();
                HtmlTextWriter htextw = new HtmlTextWriter(stw);
                htextw.AddStyleAttribute("font-size", "11pt");
                htextw.AddStyleAttribute("color", "Black");



                GrdDocumentSummary.RenderControl(htextw); //Name of the Panel

                //var sb1 = new StringBuilder();
                //GridDiv.RenderControl(new HtmlTextWriter(new StringWriter(sb1)));

                string s = htextw.InnerWriter.ToString();

                string HTMLstring = "<html><body>" +
                    "<div style='width:100%; margin:auto;'><div style='width:100%; float:left; line-height:25px; font-size:12pt;' align='center'>" +
                    "<asp:Label ID='Lbl1' runat='server' Font-Bold='true'>" + WebConfigurationManager.AppSettings["Domain"] + " Report Name: Status of Documents submitted by the Contractor</asp:Label><br />" +
                    "<asp:Label ID='Lbl4' runat='server'>Project Name : " + DDlProject.SelectedItem.Text + " (" + DDLWorkPackage.SelectedItem.Text + ")</asp:Label><br />" +
                    "</div> <div style='width:100%; float:left; height:10px;'>&nbsp;&nbsp;&nbsp;</div>" +
                    "<div style='width:100%;font-size:11pt float:left;' align='left'>" +
                    s +
                    "</div>" +
                    "</div></body></html>";

                string strFile = "Report_Status-of-Documents-submitted-by-the-Contractor_" + DateTime.Now.Ticks + ".xls";
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
        protected void btnDocumentSummaryExportPDF_Click(object sender, EventArgs e)
        {
            ViewState["Export"] = "2";
            BindDocumentSummary();
            ExporttoPDF(GrdDocumentSummary, 2, "No");
        }
        protected void btnDocumentSummaryPrint_Click(object sender, EventArgs e)
        {
            ViewState["Export"] = "2";
            BindDocumentSummary();
            ExporttoPDF(GrdDocumentSummary, 2, "Yes");
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

                //int Diff = 100 - ProjectName.Length;
                //if (ProjectName.Length > 30)
                //{
                //    ProjectName = ProjectName.Substring(0, 29) + "..";
                //}

                //if (Diff > 0)
                //{
                //    ProjectName = ProjectName + string.Concat(Enumerable.Repeat("&nbsp;", Diff));
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
                phApplicationName = new Phrase(Environment.NewLine + WebConfigurationManager.AppSettings["Domain"] + " Report Name: Status of Documents submitted by the Contractor", FontFactory.GetFont("Arial", ApplicationNameSize, iTextSharp.text.Font.BOLD));
                ExportFileName = "Report_Status-of-Documents-submitted-by-the-Contractor_" + DateTime.Now.Ticks + ".pdf";
                mainTable.SetWidths(new float[] { 10, 30, 20, 20, 20 });


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
                        if ( i == 4)
                        {
                            string pCode = getdt.GetClientCodebyWorkpackageUID(new Guid(DDLWorkPackage.SelectedValue));
                            if (pCode.StartsWith("Error:"))
                            {
                                ph = new Phrase(gdRp.Columns[i].HeaderText.Replace("<br/>", Environment.NewLine), FontFactory.GetFont("Arial", HeaderTextSize, iTextSharp.text.Font.BOLD));
                            }
                            else
                            {
                                ph = new Phrase(("Approved by " + pCode).Replace("<br/>", Environment.NewLine), FontFactory.GetFont("Arial", HeaderTextSize, iTextSharp.text.Font.BOLD));
                            }
                        }
                        else
                        {
                            ph = new Phrase(gdRp.Columns[i].HeaderText.Replace("<br/>", Environment.NewLine), FontFactory.GetFont("Arial", HeaderTextSize, iTextSharp.text.Font.BOLD));
                        }
                    }
                    PdfPCell cl = new PdfPCell(ph);
                    if (i == 0)
                    {
                        cl.HorizontalAlignment = Element.ALIGN_CENTER;
                        mainTable.AddCell(cl);
                    }
                    else if (i == 1)
                    {
                        cl.HorizontalAlignment = Element.ALIGN_LEFT;
                        mainTable.AddCell(cl);
                    }
                    else if (i == 2)
                    {
                        cl.HorizontalAlignment = Element.ALIGN_CENTER;
                        mainTable.AddCell(cl);
                    }
                    else if (i == 3)
                    {
                        cl.HorizontalAlignment = Element.ALIGN_CENTER;
                        mainTable.AddCell(cl);
                    }
                    //else if (i == 3)
                    //{
                    //    if (RBLReportFor.SelectedValue == "By Originator")
                    //    {
                    //        cl.HorizontalAlignment = Element.ALIGN_LEFT;
                    //        mainTable.AddCell(cl);
                    //    }
                    //    else
                    //    {
                    //        cl.HorizontalAlignment = Element.ALIGN_CENTER;
                    //        mainTable.AddCell(cl);
                    //    }

                    //}
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
                                if (columnNo == 3)
                                {
                                    if (gdRp.Columns[columnNo] is TemplateField)
                                    {
                                        DataBoundLiteralControl lc = gdRp.Rows[rowNo].Cells[columnNo].Controls[0] as DataBoundLiteralControl;
                                        string s = lc.Text.Trim();
                                        Phrase ph = new Phrase();
                                        if (rowNo == 10)
                                        {
                                            ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.BOLD));
                                        }
                                        else
                                        {
                                            ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                                        }
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
                                        Phrase ph = new Phrase();
                                        if (rowNo == 10)
                                        {
                                            ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.BOLD));
                                        }
                                        else
                                        {
                                            ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                                        }
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
                                        //Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                                        Phrase ph = new Phrase();
                                        if (rowNo == 10)
                                        {
                                            ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.BOLD));
                                        }
                                        else
                                        {
                                            ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                                        }
                                        PdfPCell cl = new PdfPCell(ph);
                                        cl.HorizontalAlignment = Element.ALIGN_LEFT;
                                        mainTable.AddCell(cl);
                                    }
                                    else
                                    {
                                        string s = gdRp.Rows[rowNo].Cells[columnNo].Text.Trim();
                                        s = s.Replace("&nbsp;", "");
                                        //Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                                        Phrase ph = new Phrase();
                                        if (rowNo == 10)
                                        {
                                            ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.BOLD));
                                        }
                                        else
                                        {
                                            ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                                        }
                                        PdfPCell cl = new PdfPCell(ph);
                                        cl.HorizontalAlignment = Element.ALIGN_LEFT;
                                        mainTable.AddCell(cl);
                                    }
                                }
                                else
                                {
                                    if (gdRp.Columns[columnNo] is TemplateField)
                                    {
                                        DataBoundLiteralControl lc = gdRp.Rows[rowNo].Cells[columnNo].Controls[0] as DataBoundLiteralControl;
                                        string s = lc.Text.Trim();
                                        Phrase ph = new Phrase();
                                        ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.BOLD));

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

        public override void VerifyRenderingInServerForm(Control control)
        {
            //required to avoid the runtime error "  
            //Control 'GridView1' of type 'GridView' must be placed inside a form tag with runat=server."  
        }


    }
}