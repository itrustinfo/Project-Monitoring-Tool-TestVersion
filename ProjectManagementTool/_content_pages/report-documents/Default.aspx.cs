using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Drawing;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System.Text;
using System.Web.Configuration;
using ProjectManagementTool.DAL;

namespace ProjectManager._content_pages.reports
{
    public partial class Default : System.Web.UI.Page
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
                    //DDlProject_SelectedIndexChanged(sender, e);
                    ByDate.Visible = false;
                    btnExportPDF.Visible = false;
                    btnExportExcel.Visible = false;
                    ByStatusGrid.Visible = false;
                    ByDateGrid.Visible = false;
                    DocumentSummary.Visible = false;
                    ReportFormat.Visible = false;
                    DivByOriginator.Visible = false;
                    ByOriginator.Visible = false;
                    DivDocumentSummary.Visible = false;
                    //
                    hRefNo.InnerHtml = Constants.ProjectReferenceName;
                }
            }
        }

        private void BindProject()
        {
            DataTable ds = new DataTable();
            if (Session["TypeOfUser"].ToString() == "U" || Session["TypeOfUser"].ToString() =="MD" || Session["TypeOfUser"].ToString() == "VP")
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
                    ReportFormat.Visible = true;
                    RBLReportFor.ClearSelection();
                    ByDate.Visible = false;
                    ByOriginator.Visible = false;
                    DocumentSummary.Visible = false;
                    ByDateGrid.Visible = false;
                    ByStatusGrid.Visible = false;
                    DivByOriginator.Visible = false;
                    DivDocumentSummary.Visible = false;
                    //BindDocumentSummary(DDlProject.SelectedValue, DDLWorkPackage.SelectedValue, RBLReportFor.SelectedValue);
                }
            }
            
        }

        protected void DDLWorkPackage_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDLWorkPackage.SelectedValue != "")
            {
                RBLReportFor.ClearSelection();
                ReportFormat.Visible = true;
                ByDate.Visible = false;
                ByOriginator.Visible = false;
                DocumentSummary.Visible = false;
                ByDateGrid.Visible = false;
                ByStatusGrid.Visible = false;
                DivByOriginator.Visible = false;
                DivDocumentSummary.Visible = false;
                //BindDocumentSummary(DDlProject.SelectedValue, DDLWorkPackage.SelectedValue,RBLReportFor.SelectedValue);
            }
            //DataSet ds = getdt.GetTasksForWorkPackages(DDLWorkPackage.SelectedValue);
            //DDLTask.DataTextField = "Name";
            //DDLTask.DataValueField = "TaskUID";
            //DDLTask.DataSource = ds;
            //DDLTask.DataBind();
            //DDLTask_SelectedIndexChanged(sender, e);
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (dtStartDate.Text != "" && dtEndDate.Text != "")
            {
                DocumentByDateReportName.InnerHtml = "Report Name : Document List between Start Date " + dtStartDate.Text + " and End Date" + dtEndDate.Text;
                DocumentByDateProjectName.InnerHtml = "Project Name : " + DDlProject.SelectedItem.Text + " (" + DDLWorkPackage.SelectedItem.Text + ")";
                //HeadingbyDatewise.InnerText = "Document List(From : " + dtStartDate.Text + ", To : " + dtEndDate.Text + ")";
                //lblProject.InnerText = "Project Name : " + DDlProject.SelectedItem.Text;
                //LblProjectName.Text = "Project : " + DDlProject.SelectedItem.Text;
                //LblPwrkPackage.Text = "WorkPackage : " + DDLWorkPackage.SelectedItem.Text;
                //LBlDate.Text = "Start Date : " + dtStartDate.Text + " End Date : " + dtEndDate.Text;
                //LblReportFormat.Text = "Report Fromat : " + RBLReportFor.SelectedValue;
                string sDate1 = "", sDate2 = "";
                DateTime CDate1 = DateTime.Now, CDate2 = DateTime.Now;
                //
                sDate1 = dtStartDate.Text;
                //sDate1 = sDate1.Split('/')[1] + "/" + sDate1.Split('/')[0] + "/" + sDate1.Split('/')[2];
                sDate1 = getdt.ConvertDateFormat(sDate1);
                CDate1 = Convert.ToDateTime(sDate1);
                //

                sDate2 = dtEndDate.Text;
                //sDate2 = sDate2.Split('/')[1] + "/" + sDate2.Split('/')[0] + "/" + sDate2.Split('/')[2];
                sDate2 = getdt.ConvertDateFormat(sDate2);
                CDate2 = Convert.ToDateTime(sDate2);

                DataSet ds = getdt.GetDocumentsBy_Date(new Guid(DDlProject.SelectedValue), new Guid(DDLWorkPackage.SelectedValue), CDate1, CDate2);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    btnExportPDF.Visible = true;
                    btnExportExcel.Visible = true;
                    GrdDocumentMaster.DataSource = ds;
                    GrdDocumentMaster.DataBind();
                    ByDateGrid.Visible = true;
                    DocumentSummary.Visible = true;
                    BindDocumentSummary(DDlProject.SelectedValue, DDLWorkPackage.SelectedValue, RBLReportFor.SelectedValue);
                }
                else
                {
                    ByDateGrid.Visible = true;
                    btnExportPDF.Visible = false;
                    btnExportExcel.Visible = false;
                    GrdDocumentMaster.DataSource = null;
                    GrdDocumentMaster.DataBind();
                }

            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Select Start Date and End Date');</script>");
            }
        }

        public string GetDocumentTypeIcon(string DocumentExtn)
        {
            return getdt.GetDocumentTypeMasterIcon_by_Extension(DocumentExtn);
        }

        private void BindDocumentSummary(string ProjectID, string WorkpackgeUID,string ReportType)
        {            
            DataSet ds = new DataSet();
            if (ReportType == "By Date")
            {
                HeadingSummary.InnerText = "Document Summary by Date";
                string sDate1 = "", sDate2 = "";
                DateTime CDate1 = DateTime.Now, CDate2 = DateTime.Now;

                sDate1 = dtStartDate.Text;
                sDate1 = getdt.ConvertDateFormat(sDate1);
                CDate1 = Convert.ToDateTime(sDate1);

                sDate2 = dtEndDate.Text;
                sDate2 = getdt.ConvertDateFormat(sDate2);
                CDate2 = Convert.ToDateTime(sDate2);

                ds = getdt.getDocumentCount_by_ProjectUID_WorkPackageUID_withDates(new Guid(DDlProject.SelectedValue), new Guid(DDLWorkPackage.SelectedValue), CDate1, CDate2);
            }
            else if (ReportType == "By Originator" && DDLDocumentCategory.SelectedValue == "All")
            {
                HeadingSummary.InnerText = "Document Summary by Originator";
                ds = getdt.getDocumentCount_by_ProjectUID_WorkPackageUID_Orininator(new Guid(DDlProject.SelectedValue), new Guid(DDLWorkPackage.SelectedValue), DDLOriginator.SelectedValue);

            }
            else if (ReportType == "By Originator" && DDLDocumentCategory.SelectedValue != "All")
            {
                HeadingSummary.InnerText = "Document Summary by Originator";
                ds = getdt.GetDocumentCount_by_ProjectUID_WorkPackageUID_Originator_CategoryUID(new Guid(DDlProject.SelectedValue), new Guid(DDLWorkPackage.SelectedValue), DDLOriginator.SelectedValue, DDLDocumentCategory.SelectedValue);
            }
            else
            {
                HeadingSummary.InnerText = "Document Summary by Status";
                ds = getdt.getDocumentCount_by_ProjectUID_WorkPackageUID(new Guid(DDlProject.SelectedValue), new Guid(DDLWorkPackage.SelectedValue));

            }
            if (ds.Tables[0].Rows.Count > 0)
            {
                LblTotalDocuments.Text = ds.Tables[0].Rows[0]["DocCount"].ToString();
                LblSubmitted.Text = ds.Tables[0].Rows[0]["Status1"].ToString();
                LblCodeA.Text = ds.Tables[0].Rows[0]["Status3"].ToString();
                LblCodeB.Text = ds.Tables[0].Rows[0]["Status2"].ToString();
                LblCodeC.Text = ds.Tables[0].Rows[0]["CodeC"].ToString();
                LblCodeD.Text = ds.Tables[0].Rows[0]["CodeD"].ToString();
                LblCodeE.Text = ds.Tables[0].Rows[0]["CodeE"].ToString();
                LblCodeF.Text = ds.Tables[0].Rows[0]["CodeF"].ToString();
                LblCodeG.Text = ds.Tables[0].Rows[0]["CodeG"].ToString();
                LblCodeH.Text = ds.Tables[0].Rows[0]["CodeH"].ToString();
                LblClientApproved.Text = ds.Tables[0].Rows[0]["Status4"].ToString();
            }
        }
        public string GetDocumentName(string DocumentExtn)
        {
            string retval = getdt.GetDocumentMasterType_by_Extension(DocumentExtn);
            if (retval == null || retval == "")
            {
                return "N/A";
            }
            else
            {
                return retval;
            }
        }
        protected void GrdActualDocuments_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[5].Text = Constants.ProjectReferenceName;
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[2].Text != "&nbsp;")
                {
                    e.Row.Cells[2].Text = GetDocumentName(e.Row.Cells[2].Text);
                }
                if (e.Row.Cells[3].Text != "&nbsp;")
                {
                    DataSet ds = getdt.getTop1_DocumentStatusSelect(new Guid(e.Row.Cells[3].Text));
                    if (ds != null)
                    {
                        e.Row.Cells[3].Text = ds.Tables[0].Rows[0]["ActivityType"].ToString();
                        if (ds.Tables[0].Rows[0]["ActivityType"].ToString() != "" && ds.Tables[0].Rows[0]["TopVersion"].ToString() != "")
                        {
                            e.Row.Cells[1].Text = ds.Tables[0].Rows[0]["TopVersion"].ToString();
                        }
                    }
                }
                //if (e.Row.Cells[2].Text == "Cover Letter" || e.Row.Cells[2].Text == "General Document")
                //{
                //    HtmlAnchor lnkVoucher = e.Row.FindControl("ViewDoc") as HtmlAnchor;
                //    //HtmlAnchor link = e.Row.Cells[0].Controls[0] as HtmlAnchor;
                //    if (lnkVoucher != null)
                //    {
                //        lnkVoucher.HRef = "javascript:void(0)";
                //    }


                //    //HyperLink myHyperLink = e.Row.FindControl("ViewDoc") as HyperLink;
                //    //myHyperLink.hr
                //}
            }
        }

        protected void GrdDocumentMaster_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string DocUID = GrdDocumentMaster.DataKeys[e.Row.RowIndex].Values[0].ToString();
                GridView ChidGrid = (GridView)e.Row.FindControl("GrdActualDocuments");
                DataSet ds = getdt.ActualDocuments_SelectBy_DocumentUID(new Guid(DocUID));
                ChidGrid.DataSource = ds;
                ChidGrid.DataBind();
            }
        }

        protected void btnExportPDF_Click(object sender, EventArgs e)
        {
            string sDate1 = "", sDate2 = "";
            DateTime CDate1 = DateTime.Now, CDate2 = DateTime.Now;
            //
            sDate1 = dtStartDate.Text;
            //sDate1 = sDate1.Split('/')[1] + "/" + sDate1.Split('/')[0] + "/" + sDate1.Split('/')[2];
            sDate1 = getdt.ConvertDateFormat(sDate1);
            CDate1 = Convert.ToDateTime(sDate1);
            //

            sDate2 = dtEndDate.Text;
            //sDate2 = sDate2.Split('/')[1] + "/" + sDate2.Split('/')[0] + "/" + sDate2.Split('/')[2];
            sDate2 = getdt.ConvertDateFormat(sDate2);
            CDate2 = Convert.ToDateTime(sDate2);

            //to allow paging=false & change style.
            GrdDocumentMaster.HeaderStyle.ForeColor = System.Drawing.Color.Black;
            GrdDocumentMaster.BorderColor = System.Drawing.Color.Black;
            GrdDocumentMaster.Font.Name = "Tahoma";
            GrdDocumentMaster.DataSource = getdt.GetDocumentsBy_Date(new Guid(DDlProject.SelectedValue), new Guid(DDLWorkPackage.SelectedValue), CDate1, CDate2);
            GrdDocumentMaster.AllowPaging = false;
            //GrdDocumentMaster.Columns[0].Visible = false; //export won't work if there's a link in the gridview
            GrdDocumentMaster.DataBind();

            StringWriter stw = new StringWriter();
            HtmlTextWriter htextw = new HtmlTextWriter(stw);
            htextw.AddStyleAttribute("font-size", "9pt");
            htextw.AddStyleAttribute("color", "Black");
            
            GrdDocumentMaster.RenderControl(htextw); //Name of the Panel

            //var sb1 = new StringBuilder();
            //GridDiv.RenderControl(new HtmlTextWriter(new StringWriter(sb1)));

            string s = htextw.InnerWriter.ToString();
            string HTMLstring = "<html><body>" +
                    "<div style='width:100%; margin:auto;'><div style='width:100%; float:left; line-height:25px; font-size:12pt;' align='center'>" +
                    "<b style='font-size:13pt;'>" + WebConfigurationManager.AppSettings["Domain"] + " Report Name: Document list between Start Date " + dtStartDate.Text + " and End Date " + dtEndDate.Text + "</b><br />" +
                    "<asp:Label ID='Lbl4' runat='server'>Project Name : " + DDlProject.SelectedItem.Text + " (" + DDLWorkPackage.SelectedItem.Text + ")</asp:Label><br />" +
                    "</div> <div style='width:100%; float:left; height:10px;'>&nbsp;&nbsp;&nbsp;</div>" +
                    "<div style='width:100%;font-size:11pt float:left;' align='left'>" +
                    s +
                    "</div>" +
                    "</div></body></html>";

            //string HTMLstring = "<html><body>" +
            //    "<div style='width:100%; margin:auto;'><div style='width:100%; float:left; line-height:25px; font-size:10pt;' align='center'>";
            //    HTMLstring += "<asp:Label ID='Lbl1' runat='server' Font-Bold='true'>"+ WebConfigurationManager.AppSettings["Domain"] + "</asp:Label><br />";
            //    HTMLstring += "<asp:Label ID='Lbl2' runat='server' >Report Name: Document Status Report</asp:Label><br />" +
            //    "<asp:Label ID='Lbl3' runat='server' >Start Date : " + CDate1.ToString("dd MMM yyyy") + "   End Date : " + CDate2.ToString("dd MMM yyyy") + "</asp:Label><br />" +
            //    "<asp:Label ID='Lbl4' runat='server'>Project Name : " + DDlProject.SelectedItem.Text + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Workpackage : " + DDLWorkPackage.SelectedItem.Text + "</asp:Label><br />" +
            //    "<asp:Label ID='Lbl5' runat='server' >Report Format : " + RBLReportFor.SelectedValue + "</asp:Label>" +
            //    "</div> <div style='width:100%; float:left; height:10px;'>&nbsp;&nbsp;&nbsp;</div>" +
            //    "<div style='width:100%; float:left;'>" +
            //    s +
            //    "</div>" +
            //    "</div></body></html>";


            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                {
                    StringBuilder sb = new StringBuilder();

                    //Export HTML String as PDF.
                    StringReader sr = new StringReader(HTMLstring);
                    Document pdfDoc = new Document(PageSize.A4.Rotate(), 10, 10, 0, 0);
                    
                    iTextSharp.text.pdf.PdfPTable mainTable = new iTextSharp.text.pdf.PdfPTable(GrdDocumentMaster.Columns.Count);

                    // Sets the first 4 rows of the table as the header rows which will be repeated in all the pages.
                    mainTable.HeaderRows = 4;

                    iTextSharp.text.pdf.PdfPTable headerTable = new iTextSharp.text.pdf.PdfPTable(1);

                    Phrase phApplicationName = new Phrase();
                    phApplicationName = new Phrase(Environment.NewLine + WebConfigurationManager.AppSettings["Domain"] + " Report Name: Document list between Start Date" + dtStartDate.Text + " and End Date" + dtEndDate.Text + "", FontFactory.GetFont("Arial", 13, iTextSharp.text.Font.BOLD));
                    PdfPCell clApplicationName = new PdfPCell(phApplicationName);
                    // Sets the border of the cell to zero.
                    clApplicationName.Border = PdfPCell.NO_BORDER;
                    // Sets the Horizontal Alignment of the PdfPCell to left.
                    clApplicationName.HorizontalAlignment = Element.ALIGN_CENTER;

                    headerTable.AddCell(clApplicationName);
                    // Adds the cell which holds the date to the headerTable.
                    //  headerTable.AddCell(clDate);
                    // Sets the border of the headerTable to zero.
                    headerTable.DefaultCell.Border = PdfPCell.NO_BORDER;

                    // Creates a phrase which holds the file name.
                    Phrase phHeader = new Phrase("Project Name : " + DDlProject.SelectedItem.Text + " (" + DDLWorkPackage.SelectedItem.Text + ")");
                    PdfPCell clHeader = new PdfPCell(phHeader);
                    clHeader.Border = PdfPCell.NO_BORDER;
                    clHeader.HorizontalAlignment = Element.ALIGN_CENTER;
                    mainTable.AddCell(clHeader);

                    Phrase phSpace = new Phrase("\n");
                    PdfPCell clSpace = new PdfPCell(phSpace);
                    clSpace.Border = PdfPCell.NO_BORDER;
                    //clSpace.Colspan = noOfColumns;
                    mainTable.AddCell(clSpace);

                    HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);


                    int len = 174;
                    System.Text.StringBuilder time = new System.Text.StringBuilder();
                    time.Append(DateTime.Now.ToString("hh:mm tt"));
                    time.Append("".PadLeft(len, ' ').Replace(" ", " "));

                    iTextSharp.text.Font foot = new iTextSharp.text.Font();
                    foot.Size = 10;
                    HeaderFooter pdfFooter = new HeaderFooter(new Phrase("Date : " + DateTime.Now.ToString("dd/MM/yyyy") + " Time " + time + "  Page: ", foot), new Phrase(Environment.NewLine + " ", foot));
                    pdfFooter.Alignment = Element.ALIGN_CENTER;

                    pdfDoc.Footer = pdfFooter;
                    //pdfDoc.Footer = new HeaderFooter(new Phrase("Date : " + DateTime.Now.ToString("dd/MM/yyyy") + "                      Page: ", foot), true);
                    pdfDoc.Open();
                    pdfDoc.Add(mainTable);
                    htmlparser.Parse(sr);
                    pdfDoc.Close();
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "attachment;filename=Report_Document_list_by_Date" + "_" + DateTime.Now.Ticks + ".pdf");
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.Write(pdfDoc);
                    Response.End();

                    //CreatePDFFromHTMLFile(HTMLstring);
                    //StringReader sr = new StringReader(Request.Form[hfGridHtml.UniqueID]);
                    //Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
                    //PdfWriter writer = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                    //pdfDoc.Open();
                    //XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                    //pdfDoc.Close();
                    //Response.ContentType = "application/pdf";
                    //Response.AddHeader("content-disposition", "attachment;filename=HTML.pdf");
                    //Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    //Response.Write(pdfDoc);
                    //Response.End();

                    //Response.ContentType = "application/pdf";
                    //Response.AddHeader("content-disposition", "attachment;filename=" + DateTime.Now.Ticks + "_report.pdf");
                    //Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    //StringWriter sw = new StringWriter();
                    //HtmlTextWriter hw = new HtmlTextWriter(sw);
                    //ExportDiv.RenderControl(hw);
                    //StringReader sr = new StringReader(sw.ToString());
                    //Document pdfDoc = new Document(PageSize.A4, 5, 5, 15, 5);
                    //HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                    //PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                    //pdfDoc.Open();
                    //htmlparser.Parse(sr);
                    //pdfDoc.Close();
                    //Response.Write(pdfDoc);
                    //Response.End();
                }


            }
            //using (StringWriter sw = new StringWriter())
            //{
            //    using (HtmlTextWriter hw = new HtmlTextWriter(sw))
            //    {
            //        //GrdDocumentMaster.RenderControl(hw);
            //        GridDiv.RenderControl(hw);
            //        StringReader sr = new StringReader(sw.ToString());
            //        Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
            //        PdfWriter writer = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            //        pdfDoc.Open();
            //        XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
            //        pdfDoc.Close();
            //        Response.ContentType = "application/pdf";
            //        Response.AddHeader("content-disposition", "attachment;filename=GridViewExport.pdf");
            //        Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //        Response.Write(pdfDoc);
            //        Response.End();
            //    }
            //}

            //Response.ContentType = "application/pdf";
            //Response.AddHeader("content-disposition", "attachment;filename=" + DateTime.Now.Ticks + "_report.pdf");
            //Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //StringWriter sw = new StringWriter();
            //HtmlTextWriter hw = new HtmlTextWriter(sw);
            //GridDiv.RenderControl(hw);
            //StringReader sr = new StringReader(sw.ToString());
            //iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(PageSize.A4, 10f, 10f, 20f, 0f);
            //HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            //PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            //pdfDoc.Open();
            //htmlparser.Parse(sr);
            //pdfDoc.Close();
            //Response.Write(pdfDoc);
            //Response.End();


            //Response.ContentType = "application/pdf";
            //Response.AddHeader("content-disposition", "attachment;filename=UserDetails.pdf");
            //Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //StringWriter sw = new StringWriter();
            //HtmlTextWriter hw = new HtmlTextWriter(sw);
            //GridDiv.RenderControl(hw);
            //StringReader sr = new StringReader(sw.ToString());
            //iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(PageSize.A4, 10f, 10f, 20f, 0.0f);
            //HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            //PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            //pdfDoc.Open();
            //htmlparser.Parse(sr);
            //pdfDoc.Close();
            //Response.Write(pdfDoc);
            //Response.End();

            //string sDate1 = "", sDate2 = "";
            //DateTime CDate1 = DateTime.Now, CDate2 = DateTime.Now;
            ////
            //sDate1 = dtStartDate.Text;
            //sDate1 = sDate1.Split('/')[1] + "/" + sDate1.Split('/')[0] + "/" + sDate1.Split('/')[2];
            //CDate1 = Convert.ToDateTime(sDate1);
            ////

            //sDate2 = dtEndDate.Text;
            //sDate2 = sDate2.Split('/')[1] + "/" + sDate2.Split('/')[0] + "/" + sDate2.Split('/')[2];
            //CDate2 = Convert.ToDateTime(sDate2);

            ////to allow paging=false & change style.
            //GrdDocumentMaster.HeaderStyle.ForeColor = System.Drawing.Color.Black;
            //GrdDocumentMaster.BorderColor = System.Drawing.Color.Black;
            //GrdDocumentMaster.Font.Name = "Tahoma";
            //GrdDocumentMaster.DataSource = getdt.GetDocumentsBy_Date(new Guid(DDlProject.SelectedValue), new Guid(DDLWorkPackage.SelectedValue), CDate1, CDate2);
            //GrdDocumentMaster.AllowPaging = false;
            ////GrdDocumentMaster.Columns[0].Visible = false; //export won't work if there's a link in the gridview
            //GrdDocumentMaster.DataBind();

            ////to PDF code --Sam
            //string attachment = "attachment; filename=report.pdf";
            //Response.ClearContent();
            //Response.AddHeader("content-disposition", attachment);
            //Response.ContentType = "application/pdf";
            //StringWriter stw = new StringWriter();
            //HtmlTextWriter htextw = new HtmlTextWriter(stw);
            //htextw.AddStyleAttribute("font-size", "8pt");
            //htextw.AddStyleAttribute("color", "Black");

            //GrdDocumentMaster.RenderControl(htextw); //Name of the Panel
            //iTextSharp.text.Document document = new iTextSharp.text.Document();
            //document = new iTextSharp.text.Document(PageSize.A4, 5, 5, 15, 5);
            //FontFactory.GetFont("Tahoma", 50, iTextSharp.text.Color.BLUE);
            //PdfWriter.GetInstance(document, Response.OutputStream);
            //document.Open();

            //StringReader str = new StringReader(stw.ToString());
            //HTMLWorker htmlworker = new HTMLWorker(document);
            //htmlworker.Parse(str);

            //document.Close();
            //Response.Write(document);

        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            //required to avoid the runtime error "  
            //Control 'GridView1' of type 'GridView' must be placed inside a form tag with runat=server."  
        }

        protected void btnExportExcel_Click(object sender, EventArgs e)
        {

            StringBuilder StrExport = new StringBuilder();

            StringWriter stw = new StringWriter();
            HtmlTextWriter htextw = new HtmlTextWriter(stw);
            htextw.AddStyleAttribute("font-size", "9pt");
            htextw.AddStyleAttribute("color", "Black");

            GrdDocumentMaster.RenderControl(htextw); //Name of the Panel

            //var sb1 = new StringBuilder();
            //GridDiv.RenderControl(new HtmlTextWriter(new StringWriter(sb1)));

            string s = htextw.InnerWriter.ToString();

            string HTMLstring = "<html><body>" +
                    "<div style='width:100%; margin:auto;'><div style='width:100%; float:left; line-height:25px; font-size:12pt;' align='center'>" +
                    "<asp:Label ID='Lbl1' runat='server' Font-Bold='true'>" + WebConfigurationManager.AppSettings["Domain"] + " Report Name: Document list between " + dtStartDate.Text + " and " + dtEndDate.Text + " date</asp:Label><br />" +
                    "<asp:Label ID='Lbl4' runat='server'>Project Name : " + DDlProject.SelectedItem.Text + " (" + DDLWorkPackage.SelectedItem.Text + ")</asp:Label><br />" +
                    "</div> <div style='width:100%; float:left; height:10px;'>&nbsp;&nbsp;&nbsp;</div>" +
                    "<div style='width:100%;font-size:11pt float:left;' align='left'>" +
                    s +
                    "</div>" +
                    "</div></body></html>";

            //string HTMLstring = "<html><body>" +
            //    "<div style='width:100%; margin:auto;'><div style='width:100%; float:left; line-height:25px; font-size:12pt;' align='center'>" +
            //    "<asp:Label ID='Lbl1' runat='server' Font-Bold='true'>" + WebConfigurationManager.AppSettings["Domain"] + "</asp:Label><br />" +
            //    "<asp:Label ID='Lbl2' runat='server' >Report Name: Document Status Report</asp:Label><br />" +
            //    "<asp:Label ID='Lbl3' runat='server' >Start Date : " + dtStartDate.Text + "   End Date : " + dtEndDate.Text + "</asp:Label><br />" +
            //    "<asp:Label ID='Lbl4' runat='server'>Project Name : " + DDlProject.SelectedItem.Text + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Workpackage : " + DDLWorkPackage.SelectedItem.Text + "</asp:Label><br />" +
            //    "<asp:Label ID='Lbl5' runat='server' >Report Format : " + RBLReportFor.SelectedValue + "</asp:Label>" +
            //    "</div> <div style='width:100%; float:left; height:10px;'>&nbsp;&nbsp;&nbsp;</div>" +
            //    "<div style='width:100%; float:left;'>" +
            //    s +
            //    "</div>" +
            //    "</div></body></html>";

            string strFile = "Report_Document_list_by_date" + DateTime.Now.Ticks + ".xls";
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

            //Response.ClearContent();
            //Response.Buffer = true;
            //Response.Charset = "";
            //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            //Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "Customers.xlsx"));
            //StringWriter sw = new StringWriter();
            //HtmlTextWriter ht = new HtmlTextWriter(sw);
            //GrdDocumentMaster.RenderControl(ht);
            //Response.Write(sw.ToString());
            //Response.End();


            //Response.ClearContent();
            //Response.Buffer = true;
            //Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "Customers.xlsx"));
            //Response.ContentType = "application/vnd.ms-excel";

            //lblProject.InnerText = "Project Name : " + DDlProject.SelectedItem.Text;
            //LblPwrkPackage.InnerText = "WorkPackage Name : " + DDLWorkPackage.SelectedItem.Text;
            //string sDate1 = "", sDate2 = "";
            //DateTime CDate1 = DateTime.Now, CDate2 = DateTime.Now;
            ////
            //sDate1 = (dtStartDate.FindControl("txtDate") as TextBox).Text;
            //sDate1 = sDate1.Split('/')[1] + "/" + sDate1.Split('/')[0] + "/" + sDate1.Split('/')[2];
            //CDate1 = Convert.ToDateTime(sDate1);
            ////

            //sDate2 = (dtEndDate.FindControl("txtDate") as TextBox).Text;
            //sDate2 = sDate2.Split('/')[1] + "/" + sDate2.Split('/')[0] + "/" + sDate2.Split('/')[2];
            //CDate2 = Convert.ToDateTime(sDate2);
            //string str = string.Empty;
            //DataSet ds = getdt.GetDocumentsBy_Date(new Guid(DDlProject.SelectedValue), new Guid(DDLWorkPackage.SelectedValue), CDate1, CDate2);
            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    //DataTable dt = GetDatafromDatabase();
            //    int k = 1;
            //    foreach (DataColumn dtcol in ds.Tables[0].Columns)
            //    {
            //        Response.Write(str + "Column " + k);
            //        str = "\t";
            //        k += 1;
            //    }
            //    Response.Write("\n");
            //    foreach (DataRow dr in ds.Tables[0].Rows)
            //    {
            //        str = "";
            //        for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
            //        {
            //            Response.Write(str + Convert.ToString(dr[j]));
            //            str = "\t";
            //        }
            //        Response.Write("\n");
            //    }
            //    Response.End();
            //    Response.Flush();
            //    Response.Close();
            //    //for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            //    //{
            //    //    str = "";
            //    //    //Response.Write(str + ds.Tables[0].Rows[i]["DocName"].ToString() + " [ " + ds.Tables[0].Rows[i]["DocName"].ToString() + " ]");
            //    //    //str = "\t";
            //    //    //Response.Write("\n");
            //    //    //DataSet dsdoc = getdt.ActualDocuments_SelectBy_DocumentUID(new Guid(ds.Tables[0].Rows[i]["DocumentUID"].ToString()));
            //    //    //if (dsdoc.Tables[0].Rows.Count > 0)
            //    //    //{
            //    //    //    str = "";
            //    //    //    foreach (DataRow dr in dsdoc.Tables[0].Rows)
            //    //    //    {
            //    //    //        for (int j = 0; j < dsdoc.Tables[0].Columns.Count; j++)
            //    //    //        {
            //    //    //            Response.Write(str + Convert.ToString(dr[j]));
            //    //    //            str = "\t";
            //    //    //        }
            //    //    //        Response.Write("\n");
            //    //    //    }
            //    //    //}
            //    //}

            //    //Response.End();

            //}


            //Response.Clear();
            //Response.Buffer = true;
            //Response.AddHeader("content-disposition", "attachment;filename=GridViewExport.xls");
            //Response.Charset = "";
            //Response.ContentType = "application/vnd.ms-excel";
            //using (StringWriter sw = new StringWriter())
            //{
            //    HtmlTextWriter hw = new HtmlTextWriter(sw);

            //    //To Export all pages
            //    GrdDocumentMaster.AllowPaging = false;


            //    GrdDocumentMaster.HeaderRow.BackColor = Color.White;
            //    foreach (TableCell cell in GrdDocumentMaster.HeaderRow.Cells)
            //    {
            //        cell.BackColor = GrdDocumentMaster.HeaderStyle.BackColor;
            //    }
            //    foreach (GridViewRow row in GrdDocumentMaster.Rows)
            //    {
            //        row.BackColor = Color.White;
            //        foreach (TableCell cell in row.Cells)
            //        {
            //            if (row.RowIndex % 2 == 0)
            //            {
            //                cell.BackColor = GrdDocumentMaster.AlternatingRowStyle.BackColor;
            //            }
            //            else
            //            {
            //                cell.BackColor = GrdDocumentMaster.RowStyle.BackColor;
            //            }
            //            cell.CssClass = "textmode";
            //        }
            //    }

            //    GrdDocumentMaster.RenderControl(hw);

            //    //style to format numbers to string
            //    string style = @"<style> .textmode { } </style>";
            //    Response.Write(style);
            //    Response.Output.Write(sw.ToString());
            //    Response.Flush();
            //    Response.End();
            //DataTable dt = new DataTable("GridView_Data");
            //DataSet dsgrid = new DataSet();
            //DataSet ds = getdt.GetDocumentsBy_Date(new Guid(DDlProject.SelectedValue), new Guid(DDLWorkPackage.SelectedValue), CDate1, CDate2);
            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    dsgrid.Tables.Add("MasterDocument");
            //    dsgrid.Tables[0].Columns.Add("Submittle Name");
            //    dsgrid.Tables.Add("ActualDocument");
            //    dsgrid.Tables[1].Columns.Add("Name");
            //    dsgrid.Tables[1].Columns.Add("Version");
            //    dsgrid.Tables[1].Columns.Add("Document For");
            //    dsgrid.Tables[1].Columns.Add("Document Type");
            //    dsgrid.Tables[1].Columns.Add("Status");
            //    dsgrid.Tables[1].Columns.Add("Submit Date");
            //    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            //    {
            //        //dt.Rows.Add(ds.Tables[0].Rows[i]["DocName"].ToString());
            //        dsgrid.Tables[0].Rows.Add(ds.Tables[0].Rows[i]["DocName"].ToString());



            //        DataSet dsdoc = getdt.ActualDocuments_SelectBy_DocumentUID(new Guid(ds.Tables[0].Rows[i]["DocumentUID"].ToString()));
            //        if (dsdoc.Tables[0].Rows.Count > 0)
            //        {
            //            //foreach (DataRow dr in dsdoc.Tables[0].Rows)
            //            //{

            //            //}
            //            for (int j = 0; j < dsdoc.Tables[0].Rows.Count; j++)
            //            {
            //                dsgrid.Tables[1].Rows.Add(dsdoc.Tables[0].Rows[j]["ActualDocument_Name"].ToString());
            //            }

            //        }
            //    }


            //    using (XLWorkbook wb = new XLWorkbook())
            //    {
            //        wb.Worksheets.Add(dsgrid);

            //        Response.Clear();
            //        Response.Buffer = true;
            //        Response.Charset = "";
            //        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            //        Response.AddHeader("content-disposition", "attachment;filename=GridView.xlsx");
            //        using (MemoryStream MyMemoryStream = new MemoryStream())
            //        {
            //            wb.SaveAs(MyMemoryStream);
            //            MyMemoryStream.WriteTo(Response.OutputStream);
            //            Response.Flush();
            //            Response.End();
            //        }
            //    }
            //}
            //int j = 1;

            //foreach (TableCell cell in GrdDocumentMaster.HeaderRow.Cells)
            //{
            //    dt.Columns.Add("Column " + j);
            //    j = j + 1;
            //}
            //foreach (GridViewRow row in GrdDocumentMaster.Rows)
            //{
            //    dt.Rows.Add();
            //    for (int i = 0; i < row.Cells.Count; i++)
            //    {
            //        dt.Rows[dt.Rows.Count - 1][i] = row.Cells[i].Text;
            //    }
            //}




            //Response.Clear();
            //Response.Buffer = true;
            //Response.ClearContent();
            //Response.ClearHeaders();
            //Response.Charset = "";
            //string FileName = DDLWorkPackage.SelectedItem.Text + DateTime.Now + ".xlsx";
            //StringWriter strwritter = new StringWriter();
            //HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
            //Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //Response.ContentType = "application/vnd.ms-excel";
            //Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
            //GrdDocumentMaster.GridLines = GridLines.Both;
            //GrdDocumentMaster.HeaderStyle.Font.Bold = true;
            //GrdDocumentMaster.RenderControl(htmltextwrtter);
            //Response.Write(strwritter.ToString());
            //Response.End();
        }

        protected void RBLReportFor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDLWorkPackage.SelectedValue != "")
            {
                if (RBLReportFor.SelectedValue == "By Date")
                {
                    ByDate.Visible = true;
                    ByDateGrid.Visible = false;
                    ByStatusGrid.Visible = false;
                    DocumentSummary.Visible = false;
                    dtStartDate.Text = "";
                    dtEndDate.Text = "";
                    ByOriginator.Visible = false;
                    DivByOriginator.Visible = false;
                    DivDocumentSummary.Visible = false;
                }
                else if (RBLReportFor.SelectedValue == "By Originator")
                {
                    ByOriginator.Visible = true;
                    ByDate.Visible = false;
                    ByStatusGrid.Visible = false;
                    ByDateGrid.Visible = false;
                    DocumentSummary.Visible = false;
                    DivByOriginator.Visible = false;
                    //DataSet ds = getdt.GetOriginatorMaster();
                    //DDLOriginator.DataTextField = "Originator_Name";
                    //DDLOriginator.DataValueField = "Originator_Name";
                    //DDLOriginator.DataSource = ds;
                    //DDLOriginator.DataBind();
                    DDLOriginator.Items.Clear();
                    DDLOriginator.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", ""));
                    DDLOriginator.Items.Insert(1, new System.Web.UI.WebControls.ListItem("Contractor", "Contractor"));
                    DDLOriginator.Items.Insert(2, new System.Web.UI.WebControls.ListItem("NJSEI", "NJSEI"));
                    string pCode = getdt.GetClientCodebyWorkpackageUID(new Guid(DDLWorkPackage.SelectedValue));
                    if (!pCode.StartsWith("Error:"))
                    {
                        DDLOriginator.Items.Insert(3, new System.Web.UI.WebControls.ListItem(pCode, pCode));
                    }
                    DivDocumentSummary.Visible = false;

                    DataSet dscategory = getdt.WorkPackageCategory_Selectby_WorkPackageUID(new Guid(DDLWorkPackage.SelectedValue));
                    DDLDocumentCategory.DataTextField = "WorkPackageCategory_Name";
                    DDLDocumentCategory.DataValueField = "WorkPackageCategory_UID";
                    DDLDocumentCategory.DataSource = dscategory;
                    DDLDocumentCategory.DataBind();
                    DDLDocumentCategory.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select--", ""));
                    DDLDocumentCategory.Items.Insert(1, new System.Web.UI.WebControls.ListItem("All", "All"));
                }
                else if (RBLReportFor.SelectedValue == "By Summary")
                {
                    ByOriginator.Visible = false;
                    ByDate.Visible = false;
                    ByDateGrid.Visible = false;
                    ByStatusGrid.Visible = false;
                    DocumentSummary.Visible = false;
                    DivByOriginator.Visible = false;
                    DivDocumentSummary.Visible = true;
                    BindDocumentSummary();
                }
                else
                {
                    DivByOriginator.Visible = false;
                    ByDate.Visible = false;
                    ByDateGrid.Visible = false;
                    ByOriginator.Visible = false;
                    ByStatusGrid.Visible = true;
                    BindSubmittedDocuments(DDLWorkPackage.SelectedValue);
                    BindReviewedDocuments(DDLWorkPackage.SelectedValue);
                    BindApprovedDocuments(DDLWorkPackage.SelectedValue);
                    DocumentSummary.Visible = true;
                    BindDocumentSummary(DDlProject.SelectedValue, DDLWorkPackage.SelectedValue, RBLReportFor.SelectedValue);
                    DivDocumentSummary.Visible = false;

                }
            }
        }

        
        private void BindSubmittedDocuments(string WorkpackageID)
        {
            DataSet ds = getdt.Submitted_ActualDocuments_SelectBy_WorkPackageUID(new Guid(WorkpackageID));
            GrdActualSubmittedDocuments.DataSource = ds;
            GrdActualSubmittedDocuments.DataBind();
        }

        private void BindReviewedDocuments(string WorkpackageID)
        {
            DataSet ds = getdt.Reviewed_ActualDocuments_SelectBy_WorkPackageUID(new Guid(WorkpackageID));
            GrdReviewedDocuments.DataSource = ds;
            GrdReviewedDocuments.DataBind();
        }

        private void BindApprovedDocuments(string WorkpackageID)
        {
            DataSet ds = getdt.Approved_ActualDocuments_SelectBy_WorkPackageUID(new Guid(WorkpackageID));
            GrdApprovedDocuments.DataSource = ds;
            GrdApprovedDocuments.DataBind();
        }

        protected void GrdActualSubmittedDocuments_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[4].Text != "&nbsp;")
                {
                    DateTime dt = getdt.GetDocumentReviewedDateByID(new Guid(e.Row.Cells[4].Text), "Submitted");
                    e.Row.Cells[4].Text = dt.ToString("dd MMM yyyy");
                }
            }
        }
        protected void GrdReviewedDocuments_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[4].Text != "&nbsp;")
                {
                    DateTime dt = getdt.GetDocumentReviewedDateByID(new Guid(e.Row.Cells[4].Text), "Code B");
                    e.Row.Cells[4].Text = dt.ToString("dd MMM yyyy");
                }
                if (e.Row.Cells[5].Text != "&nbsp;")
                {
                    int Cnt = getdt.GetDocumentReviewedinDays(new Guid(e.Row.Cells[5].Text), "Code B");
                    e.Row.Cells[5].Text = Cnt.ToString();
                }
            }
        }

        protected void GrdApprovedDocuments_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                if (e.Row.Cells[4].Text != "&nbsp;")
                {
                    DateTime dt = getdt.GetDocumentReviewedDateByID(new Guid(e.Row.Cells[4].Text), "Code A");
                    e.Row.Cells[4].Text = dt.ToString("dd MMM yyyy");
                }
                if (e.Row.Cells[5].Text != "&nbsp;")
                {
                    int Cnt = getdt.GetDocumentReviewedinDays(new Guid(e.Row.Cells[5].Text), "Code A");
                    e.Row.Cells[5].Text = Cnt.ToString();
                }
            }
        }

        

        protected void GrdActualSubmittedDocuments_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdActualSubmittedDocuments.PageIndex = e.NewPageIndex;
            BindSubmittedDocuments(DDLWorkPackage.SelectedValue);
        }

        protected void GrdReviewedDocuments_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdReviewedDocuments.PageIndex = e.NewPageIndex;
            BindReviewedDocuments(DDLWorkPackage.SelectedValue);
        }

        protected void GrdApprovedDocuments_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdApprovedDocuments.PageIndex = e.NewPageIndex;
            BindApprovedDocuments(DDLWorkPackage.SelectedValue);
        }

        protected void DDLOriginator_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDLOriginator.SelectedValue != "")
            {

                //LblOriginatorDocument.Text = "Status of Documents submitted by " + DDLOriginator.SelectedItem.Text;
                OriginatorReportName.InnerHtml = "Report Name : Status of Documents submitted by " + DDLOriginator.SelectedItem.Text;
                OriginatorProjectName.InnerHtml = "Project Name : " + DDlProject.SelectedItem.Text + " (" + DDLWorkPackage.SelectedItem.Text + ")";

                DocumentSummary.Visible = true;
                DivByOriginator.Visible = true;
                BindDocumentSummary(DDlProject.SelectedValue, DDLWorkPackage.SelectedValue, RBLReportFor.SelectedValue);

                DataSet ds = getdt.GetDocuments_by_Workpackage_Orininator(new Guid(DDlProject.SelectedValue), new Guid(DDLWorkPackage.SelectedValue), DDLOriginator.SelectedValue, txtProjectRef.Text, txtOriginator.Text);
                GrdOriginatorDocuments.DataSource = ds;
                GrdOriginatorDocuments.DataBind();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    btnbyOriginatorExportPDF.Visible = true;
                    btnbyOriginatorPrint.Visible = true;
                    btnbyOriginatorExportExcel.Visible = true;
                }
                else
                {
                    btnbyOriginatorExportPDF.Visible = false;
                    btnbyOriginatorPrint.Visible = false;
                    btnbyOriginatorExportExcel.Visible = false;
                }
            }
        }

        protected void BindDocumentSummary()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Sl_No");
            dt.Columns.Add("Documents");
            dt.Columns.Add("Submitted_by_the_Contractor");
            dt.Columns.Add("Recommended_Returned_by_PMC");
            dt.Columns.Add("Approved_by_BWSSB");
            int TotalReturnedbyPMC = 0;
            int TotalSubmitted = 0;
            int TotalApproved = 0;
            DataSet ds = getdt.GetDocumentSummary_by_WorkpackgeUID(new Guid(DDlProject.SelectedValue), new Guid(DDLWorkPackage.SelectedValue));
            if (ds.Tables[0].Rows.Count > 0)
            {
                btnDocumentSummaryExportPDF.Visible = true;
                btnDocumentSummaryPrint.Visible = true;
                btnDocumentSummaryExportExcel.Visible = true;
                DocumentSummaryReportName.InnerHtml = "Report Name : Status of Documents submitted by the Contractor";
                DocumentSummaryProjectName.InnerHtml = "Project Name : " + DDlProject.SelectedItem.Text + " (" + DDLWorkPackage.SelectedItem.Text + ")";
                for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
                {
                    DataRow dr = dt.NewRow();
                    if (i == 0)
                    {
                        dr["Sl_No"] = (i + 1);
                        dr["Documents"] = "No. of Documemts submitted by the Contractor";
                        dr["Submitted_by_the_Contractor"] = ds.Tables[0].Rows[0]["SubmittedDocuments"].ToString();
                        dr["Recommended_Returned_by_PMC"] = "";
                        dr["Approved_by_BWSSB"] = "-";
                        TotalSubmitted += ds.Tables[0].Rows[0]["SubmittedDocuments"].ToString() != "" ? Convert.ToInt32(ds.Tables[0].Rows[0]["SubmittedDocuments"].ToString()) : 0;
                    }
                    else if (i == 1)
                    {
                        dr["Sl_No"] = (i + 1);
                        dr["Documents"] = "No. of Documemts under category A";
                        dr["Submitted_by_the_Contractor"] = "-";
                        dr["Recommended_Returned_by_PMC"] = ds.Tables[0].Rows[0]["CodeA"].ToString();
                        dr["Approved_by_BWSSB"] = "-";
                        TotalReturnedbyPMC += ds.Tables[0].Rows[0]["CodeA"].ToString() != "" ? Convert.ToInt32(ds.Tables[0].Rows[0]["CodeA"].ToString()) : 0;
                    }
                    else if (i == 2)
                    {
                        dr["Sl_No"] = (i + 1);
                        dr["Documents"] = "No. of Documemts under category B";
                        dr["Submitted_by_the_Contractor"] = "-";
                        dr["Recommended_Returned_by_PMC"] = ds.Tables[0].Rows[0]["CodeB"].ToString();
                        dr["Approved_by_BWSSB"] = "-";
                        TotalReturnedbyPMC += ds.Tables[0].Rows[0]["CodeB"].ToString() != "" ? Convert.ToInt32(ds.Tables[0].Rows[0]["CodeB"].ToString()) : 0;
                    }
                    else if (i == 3)
                    {
                        dr["Sl_No"] = (i + 1);
                        dr["Documents"] = "No. of Documemts under category C";
                        dr["Submitted_by_the_Contractor"] = "-";
                        dr["Recommended_Returned_by_PMC"] = ds.Tables[0].Rows[0]["CodeC"].ToString();
                        dr["Approved_by_BWSSB"] = "-";
                        TotalReturnedbyPMC += ds.Tables[0].Rows[0]["CodeC"].ToString() != "" ? Convert.ToInt32(ds.Tables[0].Rows[0]["CodeC"].ToString()) : 0;
                    }
                    else if (i == 4)
                    {
                        dr["Sl_No"] = (i + 1);
                        dr["Documents"] = "No. of Documemts under category D";
                        dr["Submitted_by_the_Contractor"] = "-";
                        dr["Recommended_Returned_by_PMC"] = ds.Tables[0].Rows[0]["CodeD"].ToString();
                        dr["Approved_by_BWSSB"] = "-";
                        TotalReturnedbyPMC += ds.Tables[0].Rows[0]["CodeD"].ToString() != "" ? Convert.ToInt32(ds.Tables[0].Rows[0]["CodeD"].ToString()) : 0;
                    }
                    else if (i == 5)
                    {
                        dr["Sl_No"] = (i + 1);
                        dr["Documents"] = "No. of Documemts under category E";
                        dr["Submitted_by_the_Contractor"] = "-";
                        dr["Recommended_Returned_by_PMC"] = ds.Tables[0].Rows[0]["CodeE"].ToString();
                        dr["Approved_by_BWSSB"] = "-";
                        TotalReturnedbyPMC += ds.Tables[0].Rows[0]["CodeE"].ToString() != "" ? Convert.ToInt32(ds.Tables[0].Rows[0]["CodeE"].ToString()) : 0;
                    }
                    else if (i == 6)
                    {
                        dr["Sl_No"] = (i + 1);
                        dr["Documents"] = "No. of Documemts under category F";
                        dr["Submitted_by_the_Contractor"] = "-";
                        dr["Recommended_Returned_by_PMC"] = ds.Tables[0].Rows[0]["CodeF"].ToString();
                        dr["Approved_by_BWSSB"] = "-";
                        TotalReturnedbyPMC += ds.Tables[0].Rows[0]["CodeF"].ToString() != "" ? Convert.ToInt32(ds.Tables[0].Rows[0]["CodeF"].ToString()) : 0;
                    }
                    else if (i == 7)
                    {
                        dr["Sl_No"] = (i + 1);
                        dr["Documents"] = "No. of Documemts under category G";
                        dr["Submitted_by_the_Contractor"] = "-";
                        dr["Recommended_Returned_by_PMC"] = ds.Tables[0].Rows[0]["CodeG"].ToString();
                        dr["Approved_by_BWSSB"] = "-";
                        TotalReturnedbyPMC += ds.Tables[0].Rows[0]["CodeG"].ToString() != "" ? Convert.ToInt32(ds.Tables[0].Rows[0]["CodeG"].ToString()) : 0;
                    }
                    else if (i == 8)
                    {
                        dr["Sl_No"] = (i + 1);
                        dr["Documents"] = "No. of Documemts under category H";
                        dr["Submitted_by_the_Contractor"] = "-";
                        dr["Recommended_Returned_by_PMC"] = ds.Tables[0].Rows[0]["CodeH"].ToString();
                        dr["Approved_by_BWSSB"] = "-";
                        TotalReturnedbyPMC += ds.Tables[0].Rows[0]["CodeH"].ToString() != "" ? Convert.ToInt32(ds.Tables[0].Rows[0]["CodeH"].ToString()) : 0;
                    }
                    else
                    {
                        dr["Sl_No"] = (i + 1);
                        dr["Documents"] = "Client Approved Documents";
                        dr["Submitted_by_the_Contractor"] = "-";
                        dr["Recommended_Returned_by_PMC"] = "-";
                        dr["Approved_by_BWSSB"] = ds.Tables[0].Rows[0]["ClientApproved"].ToString();
                        TotalApproved+= ds.Tables[0].Rows[0]["ClientApproved"].ToString() != "" ? Convert.ToInt32(ds.Tables[0].Rows[0]["ClientApproved"].ToString()) : 0;
                    }
                    dt.Rows.Add(dr);
                }
                DataRow drtot = dt.NewRow();
                drtot["Sl_No"] = "";
                drtot["Documents"] = "Total No. of Documents";
                drtot["Submitted_by_the_Contractor"] = TotalSubmitted;
                drtot["Recommended_Returned_by_PMC"] = TotalReturnedbyPMC;
                drtot["Approved_by_BWSSB"] = TotalApproved;
                dt.Rows.Add(drtot);

                ViewState["Export"] = "1";
                GrdDocumentSummary.DataSource = dt;
                GrdDocumentSummary.DataBind();

                if (GrdDocumentSummary.Rows.Count > 0)
                {
                    GrdDocumentSummary.Rows[GrdDocumentSummary.Rows.Count - 1].Style["font-weight"] = "bold";
                }
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
                if (RBLReportFor.SelectedValue == "By Summary")
                {
                    phApplicationName = new Phrase(Environment.NewLine + WebConfigurationManager.AppSettings["Domain"] + " Report Name: Status of Documents submitted by the Contractor", FontFactory.GetFont("Arial", ApplicationNameSize, iTextSharp.text.Font.BOLD));
                    ExportFileName = "Report_Status-of-Documents-submitted-by-the-Contractor_" + DateTime.Now.Ticks + ".pdf";
                    mainTable.SetWidths(new float[] { 10, 30, 20, 20, 20 });
                }
                else if (RBLReportFor.SelectedValue == "By Date")
                {
                    phApplicationName = new Phrase(Environment.NewLine + WebConfigurationManager.AppSettings["Domain"] + " Report Name: Status of Documents submitted Datewise(" + dtStartDate.Text + "-" + dtEndDate.Text + ")", FontFactory.GetFont("Arial", ApplicationNameSize, iTextSharp.text.Font.BOLD));
                    ExportFileName = "Report_Status-of-Documents-submitted_datewise(" + dtStartDate.Text + "-" + dtEndDate.Text + ")-" + DateTime.Now.Ticks + ".pdf";
                }
                else if (RBLReportFor.SelectedValue == "By Originator")
                {
                    phApplicationName = new Phrase(Environment.NewLine + WebConfigurationManager.AppSettings["Domain"] + " Report Name: Status of Documents submitted by " + DDLOriginator.SelectedItem.Text, FontFactory.GetFont("Arial", ApplicationNameSize, iTextSharp.text.Font.BOLD));
                    ExportFileName = "Report_Status-of-Documents-submitted-by-" + DDLOriginator.SelectedItem.Text + DateTime.Now.Ticks + ".pdf";

                    mainTable.SetWidths(new float[] { 5, 15, 22, 23, 10, 15, 10 });
                }
                else
                {
                    phApplicationName = new Phrase(Environment.NewLine + WebConfigurationManager.AppSettings["Domain"] + " Report Name: Status of Documents", FontFactory.GetFont("Arial", ApplicationNameSize, iTextSharp.text.Font.BOLD));
                    ExportFileName = "Report_Status-of-Documents_" + DateTime.Now.Ticks + ".pdf";
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
                        if (RBLReportFor.SelectedValue == "By Summary" && i == 4)
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
                        if (RBLReportFor.SelectedValue == "By Originator")
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
                    else if (i == 3)
                    {
                        if (RBLReportFor.SelectedValue == "By Originator")
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
                                        if (RBLReportFor.SelectedValue == "By Summary" && rowNo == 10)
                                        {
                                            ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.BOLD));
                                        }
                                        else
                                        {
                                            ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                                        }
                                        PdfPCell cl = new PdfPCell(ph);
                                        if (RBLReportFor.SelectedValue == "By Originator")
                                        {
                                            cl.HorizontalAlignment = Element.ALIGN_LEFT;
                                        }
                                        else
                                        {
                                            cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                        }
                                            
                                        mainTable.AddCell(cl);
                                    }
                                    else
                                    {
                                        string s = gdRp.Rows[rowNo].Cells[columnNo].Text.Trim();
                                        s = s.Replace("&nbsp;", "");
                                        Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                                        PdfPCell cl = new PdfPCell(ph);
                                        if (RBLReportFor.SelectedValue == "By Originator")
                                        {
                                            cl.HorizontalAlignment = Element.ALIGN_LEFT;
                                        }
                                        else
                                        {
                                            cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                        }
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
                                        if (RBLReportFor.SelectedValue == "By Summary" && rowNo == 10)
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
                                        if (RBLReportFor.SelectedValue == "By Summary" && rowNo == 10)
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
                                        if (RBLReportFor.SelectedValue == "By Summary" && rowNo == 10)
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
                                        
                                        if (RBLReportFor.SelectedValue == "By Summary" && rowNo == 10)
                                        {
                                            ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.BOLD));
                                        }
                                        else if (RBLReportFor.SelectedValue == "By Originator" && columnNo == 2)
                                        {
                                            ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                                            
                                        }
                                        else
                                        {
                                            ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                                            
                                        }

                                        PdfPCell cl = new PdfPCell(ph);
                                        if (RBLReportFor.SelectedValue == "By Originator" && columnNo == 2)
                                        {
                                            cl.HorizontalAlignment = Element.ALIGN_LEFT;
                                        }
                                        else
                                        {
                                            cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                        }
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

        protected void btnbyOriginatorExportPDF_Click(object sender, EventArgs e)
        {
            ExporttoPDF(GrdOriginatorDocuments, 2, "No");
        }

        protected void btnbyOriginatorPrint_Click(object sender, EventArgs e)
        {
            ExporttoPDF(GrdOriginatorDocuments, 2, "Yes");
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

        protected void btnbyOriginatorExportExcel_Click(object sender, EventArgs e)
        {
            try
            {
                StringBuilder StrExport = new StringBuilder();

                StringWriter stw = new StringWriter();
                HtmlTextWriter htextw = new HtmlTextWriter(stw);
                htextw.AddStyleAttribute("font-size", "11pt");
                htextw.AddStyleAttribute("color", "Black");



                GrdOriginatorDocuments.RenderControl(htextw); //Name of the Panel

                //var sb1 = new StringBuilder();
                //GridDiv.RenderControl(new HtmlTextWriter(new StringWriter(sb1)));

                string s = htextw.InnerWriter.ToString();

                string HTMLstring = "<html><body>" +
                    "<div style='width:100%; margin:auto;'><div style='width:100%; float:left; line-height:25px; font-size:12pt;' align='center'>" +
                    "<asp:Label ID='Lbl1' runat='server' Font-Bold='true'>" + WebConfigurationManager.AppSettings["Domain"] + " Report Name: Status of Documents submitted by Originator " + DDLOriginator.SelectedItem.Text +"</asp:Label><br />" +
                    "<asp:Label ID='Lbl4' runat='server'>Project Name : " + DDlProject.SelectedItem.Text + " (" + DDLWorkPackage.SelectedItem.Text + ")</asp:Label><br />" +
                    "</div> <div style='width:100%; float:left; height:10px;'>&nbsp;&nbsp;&nbsp;</div>" +
                    "<div style='width:100%;font-size:11pt float:left;' align='left'>" +
                    s +
                    "</div>" +
                    "</div></body></html>";

                string strFile = "Report_Status-of-Documents-submitted-by-originator_" + DDLOriginator.SelectedItem.Text + DateTime.Now.Ticks + ".xls";
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

        public string GetSubmittalName(string DocumentID)
        {
            return getdt.getDocumentName_by_DocumentUID(new Guid(DocumentID));
        }

        public string GetDocumentCategoryName(string DocumentUID)
        {
            return getdt.GetDocumentCategoryName_by_DocumentUID(new Guid(DocumentUID));
        }

        protected void BtnOriginatorSubmit_Click(object sender, EventArgs e)
        {
            if (DDLOriginator.SelectedValue != "" && DDLDocumentCategory.SelectedValue!="")
            {
                OriginatorReportName.InnerHtml = "Report Name : Status of Documents submitted by " + DDLOriginator.SelectedItem.Text;
                OriginatorProjectName.InnerHtml = "Project Name : " + DDlProject.SelectedItem.Text + " (" + DDLWorkPackage.SelectedItem.Text + ")";
                DocumentSummary.Visible = true;
                DivByOriginator.Visible = true;
                BindDocumentSummary(DDlProject.SelectedValue, DDLWorkPackage.SelectedValue, RBLReportFor.SelectedValue);
                DataSet ds = new DataSet();
                if (DDLDocumentCategory.SelectedValue == "All")
                {
                    ds = getdt.GetDocuments_by_Workpackage_Orininator(new Guid(DDlProject.SelectedValue), new Guid(DDLWorkPackage.SelectedValue), DDLOriginator.SelectedValue, txtProjectRef.Text, txtOriginator.Text);
                }
                else
                {
                    ds = getdt.GetDocuments_by_Workpackage_Orininator_CategoryUID(new Guid(DDlProject.SelectedValue), new Guid(DDLWorkPackage.SelectedValue), DDLOriginator.SelectedValue, DDLDocumentCategory.SelectedValue, txtProjectRef.Text, txtOriginator.Text);
                }
                
                GrdOriginatorDocuments.DataSource = ds;
                GrdOriginatorDocuments.DataBind();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    btnbyOriginatorExportPDF.Visible = true;
                    btnbyOriginatorPrint.Visible = true;
                    btnbyOriginatorExportExcel.Visible = true;
                }
                else
                {
                    btnbyOriginatorExportPDF.Visible = false;
                    btnbyOriginatorPrint.Visible = false;
                    btnbyOriginatorExportExcel.Visible = false;
                }
            }
        }

        protected void GrdOriginatorDocuments_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[5].Text = Constants.ProjectReferenceName;
            }
        }

        protected void GrdDocumentSummary_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                string pCode = getdt.GetClientCodebyWorkpackageUID(new Guid(DDLWorkPackage.SelectedValue));
                if (pCode.StartsWith("Error:"))
                {
                    pCode = "Approved by BWSSB";
                }
                else
                {
                    e.Row.Cells[4].Text = "Approved by " + pCode;
                }
            }
        }
    }
}