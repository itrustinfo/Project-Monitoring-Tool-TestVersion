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
using iTextSharp.text.pdf;
using ProjectManager.DAL;

namespace ProjectManagementTool._content_pages.report_const_drawing_status
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

        private void LoadTabularData()
        {
            DataTable dt = new DataTable();
            DataRow dr;
            DataSet dsvalues = new DataSet();
            int dococunt = 0;
            int totaldocprp = 0;
            int totaldocsubm = 0;
            int totaldocapproved = 0;
            int count = 0;
            // Add Columns to datatablse
            dt.Columns.Add(new DataColumn("Sl_No"));
            dt.Columns.Add(new DataColumn("DocName")); //'ColumnName1' represents name of datafield in grid
            dt.Columns.Add(new DataColumn("PendingDocuments"));
            dt.Columns.Add(new DataColumn("SubmittedDocuments"));
            dt.Columns.Add(new DataColumn("ApprovedDocuments"));
            dt.Columns.Add(new DataColumn("Remarks"));
            // Get each row from gridview and add it to DataTable
            foreach (DataRow gvr in getdt.GetSubmittalDocDrawings(new Guid(DDLWorkPackage.SelectedValue)).Tables[0].Rows)
            {
                dr = dt.NewRow();
                count += 1;
                if (int.Parse(gvr["EstimatedDocuments"].ToString()) != 0)
                {
                    dococunt = int.Parse(gvr["EstimatedDocuments"].ToString()) - (int.Parse(gvr["Submitted"].ToString()) - int.Parse(gvr["Approved"].ToString()));
                }
                dr["DocName"] = gvr["DocName"].ToString();
                dr["PendingDocuments"] = dococunt;
                dr["SubmittedDocuments"] = gvr["Submitted"].ToString();
                dr["ApprovedDocuments"] = gvr["Approved"].ToString();
                dr["Remarks"] = gvr["Remarks"].ToString();
                dr["Sl_No"] = count ;
                dt.Rows.Add(dr);
                totaldocprp += dococunt;
                totaldocsubm += int.Parse(gvr["Submitted"].ToString());
                totaldocapproved += int.Parse(gvr["Approved"].ToString());
            }
            dr = dt.NewRow();

            dr["Sl_No"] = "";
            dr["DocName"] = "Total";
            dr["PendingDocuments"] = totaldocprp;
            ViewState["grandtotalcount"] = totaldocprp;
            dr["SubmittedDocuments"] = totaldocsubm;
            ViewState["grandtotalsubmitted"] = totaldocsubm;
            dr["ApprovedDocuments"] = totaldocapproved;
            ViewState["grandtotalapproved"] = totaldocapproved;
            dr["Remarks"] = "";
            dt.Rows.Add(dr);
            GrdPhysicalData.DataSource = dt;
            GrdPhysicalData.DataBind();

            if (GrdPhysicalData.Rows.Count > 0)
            {
                GrdPhysicalData.Rows[GrdPhysicalData.Rows.Count - 1].Style["font-weight"] = "bold";
            }
                
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (DDlProject.SelectedIndex == 0)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Please select project !');</script>");
                return;
            }
            LoadTabularData();
            divTabular.Visible = true;
            headingTb.InnerHtml = "Status of Construction Drawings - " + DDlProject.SelectedItem.Text + " (" + DDLWorkPackage.SelectedItem.Text + ")";
        }

        protected void btnexcelexport_Click(object sender, EventArgs e) // Excel Report
        {
            try
            {
                StringBuilder StrExport = new StringBuilder();

                StringWriter stw = new StringWriter();
                HtmlTextWriter htextw = new HtmlTextWriter(stw);
                htextw.AddStyleAttribute("font-size", "9pt");
                htextw.AddStyleAttribute("color", "Black");
               

                GrdPhysicalData.RenderControl(htextw); //Name of the Panel

                //var sb1 = new StringBuilder();
                //GridDiv.RenderControl(new HtmlTextWriter(new StringWriter(sb1)));

                string s = htextw.InnerWriter.ToString();

                string HTMLstring = "<html><body>" +
                    "<div style='width:100%; margin:auto;'><div style='width:100%; float:left; line-height:25px; font-size:12pt;' align='center'>" +
                    "<asp:Label ID='Lbl1' runat='server' Font-Bold='true'>" + WebConfigurationManager.AppSettings["Domain"] + " Report Name: Status of Construction Drawings</asp:Label><br />" +
                    "<asp:Label ID='Lbl4' runat='server'>Project Name : " + DDlProject.SelectedItem.Text + " (" + DDLWorkPackage.SelectedItem.Text + ")</asp:Label><br />" +
                    "</div> <div style='width:100%; float:left; height:10px;'>&nbsp;&nbsp;&nbsp;</div>" +
                    "<div style='width:100%; float:left;'>" +
                    s +
                    "</div>" +
                    "</div></body></html>";

                string strFile = "Report__ConstructionDrawingStatus_" + DateTime.Now.Ticks + ".xls";
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

        protected void btnexport_Click(object sender, EventArgs e) // PDF Export
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

        protected void btnPrintPDF_Click(object sender, EventArgs e)// Print
        {
            try
            {
                PDFConvert("Yes");
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

        private void PDFConvert(string isPrint)
        {
            int type = 2;
            DateTime CDate1 = DateTime.Now;
            GridView gdRp = new GridView();

           
            gdRp = GrdPhysicalData;
          

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

            float HeaderTextSize = 10;
            float ReportNameSize = 10;
            float ReportTextSize = 10;
            float ApplicationNameSize = 14;
            string ProjectName = DDlProject.SelectedItem.ToString() + " (" + DDLWorkPackage.SelectedItem.ToString() + ")";

            //if (ProjectName.Length > 30)
            //{
            //    ProjectName = ProjectName.Substring(0, 29) + "..";
            //}

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

            mainTable.SetWidths(new float[] { 6, 29, 15, 15, 15, 20 });
            // Creates a phrase to hold the application name at the left hand side of the header.
            Phrase phApplicationName = new Phrase(Environment.NewLine + WebConfigurationManager.AppSettings["Domain"] + " Report Name: Status of Construction Drawings", FontFactory.GetFont("Arial", ApplicationNameSize, iTextSharp.text.Font.BOLD));

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
                    if (i == 3)
                    {
                        string pCode = getdt.GetClientCodebyWorkpackageUID(new Guid(DDLWorkPackage.SelectedValue));
                        if (pCode.StartsWith("Error:"))
                        {
                            ph = new Phrase(gdRp.Columns[i].HeaderText.Replace("<br/>", Environment.NewLine), FontFactory.GetFont("Arial", HeaderTextSize, iTextSharp.text.Font.BOLD));
                        }
                        else
                        {
                            ph = new Phrase(("No. of Drawings submitted to " + pCode).Replace("<br/>", Environment.NewLine), FontFactory.GetFont("Arial", HeaderTextSize, iTextSharp.text.Font.BOLD));
                        }
                    }
                    else
                    {
                        ph = new Phrase(gdRp.Columns[i].HeaderText.Replace("<br/>", Environment.NewLine), FontFactory.GetFont("Arial", HeaderTextSize, iTextSharp.text.Font.BOLD));
                    }
                    
                }
                PdfPCell cl = new PdfPCell(ph);
                if (i == 1)
                {
                    cl.HorizontalAlignment = Element.ALIGN_LEFT;
                    mainTable.AddCell(cl);
                }
                else if (i == 5)
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
                            if ((columnNo == 1 || columnNo ==2 || columnNo == 3 || columnNo == 4) && rowNo == (noOfRows - 1))
                            {
                                if (gdRp.Columns[columnNo] is TemplateField)
                                {
                                    DataBoundLiteralControl lc = gdRp.Rows[rowNo].Cells[columnNo].Controls[0] as DataBoundLiteralControl;
                                    string s = lc.Text.Trim();
                                    Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.BOLD));
                                    PdfPCell cl = new PdfPCell(ph);
                                    cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                    mainTable.AddCell(cl);
                                }
                                else
                                {
                                    string s = gdRp.Rows[rowNo].Cells[columnNo].Text.Trim();
                                    s = s.Replace("&nbsp;", "");
                                    Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.BOLD));
                                    PdfPCell cl = new PdfPCell(ph);
                                    cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                    mainTable.AddCell(cl);
                                }
                            }
                            else if (columnNo == 1 || columnNo == 5)
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
                            string s = "Total";
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
                                s = ViewState["grandtotalsubmitted"].ToString();// Session["ExpenditureOverallTotal"].ToString();
                                Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.BOLD));
                                PdfPCell cl = new PdfPCell(ph);
                                cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                mainTable.AddCell(cl);
                            }
                            else if (columnNo == 4)
                            {
                                s = ViewState["grandtotalapproved"].ToString();// Session["TargetedOverallTotal"].ToString() + "%";
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
            int len = 174;
            System.Text.StringBuilder time = new System.Text.StringBuilder();
            time.Append(DateTime.Now.ToString("hh:mm tt"));
            time.Append("".PadLeft(len, ' ').Replace(" ", " "));

            iTextSharp.text.Font foot = new iTextSharp.text.Font();
            foot.Size = 10;
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
                Response.AddHeader("content-disposition", "attachment;filename=Report_Status_of_Construction_Drawings_" + DateTime.Now.Ticks + ".pdf");
                Response.End();
            }
        }

        protected void GrdPhysicalData_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                //string pCode = getdt.GetClientCodebyWorkpackageUID(new Guid(DDLWorkPackage.SelectedValue));
                //if (pCode.StartsWith("Error:"))
                //{
                //    pCode = "No. of Drawings submitted to BWSSB";
                //}
                //else
                //{
                //    e.Row.Cells[3].Text = "No. of Drawings submitted to " + pCode;
                //}
            }
        }
    }
}