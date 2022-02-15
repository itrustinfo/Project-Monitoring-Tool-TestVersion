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
using System.Text;
using System.Web.Configuration;
using System.IO;
using System.Globalization;
using System.Threading;

namespace ProjectManagementTool._content_pages.project_progress
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
                    BindMeetingMaster();
                    DDlProject_SelectedIndexChanged(sender, e);
                    //YearBind();
                    ProjectProgress.Visible = false;
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

        }

        //private void YearBind()
        //{
        //    DDLYear.Items.Add(new System.Web.UI.WebControls.ListItem("--Year--", ""));
        //    int year = DateTime.Now.Year - 3;
        //    for (int i = 1; i < 25; i++)
        //    {
        //        year = year + 1;
        //        DDLYear.Items.Add(new System.Web.UI.WebControls.ListItem(year.ToString(), year.ToString()));
        //    }
        //    DDLYear.SelectedValue = "2021";
        //    DDLMonth.SelectedValue = "01";
        //}
        protected void DDlProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDlProject.SelectedValue != "")
            {
                //DataSet ds = getdt.GetWorkPackages_By_ProjectUID(new Guid(DDlProject.SelectedValue));
                //DataSet ds = new DataSet();
                //if (Session["TypeOfUser"].ToString() == "U" || Session["TypeOfUser"].ToString() == "MD" || Session["TypeOfUser"].ToString() == "VP")
                //{
                //    ds = getdt.GetWorkPackages_By_ProjectUID(new Guid(DDlProject.SelectedValue));
                //}
                //else if (Session["TypeOfUser"].ToString() == "PA")
                //{
                //    ds = getdt.GetWorkPackages_ForUser_by_ProjectUID(new Guid(Session["UserUID"].ToString()), new Guid(DDlProject.SelectedValue));
                //}
                //else
                //{
                //    ds = getdt.GetWorkPackages_ForUser_by_ProjectUID(new Guid(Session["UserUID"].ToString()), new Guid(DDlProject.SelectedValue));
                //}
                //if (ds.Tables[0].Rows.Count > 0)
                //{
                //    DDLWorkPackage.DataTextField = "Name";
                //    DDLWorkPackage.DataValueField = "WorkPackageUID";
                //    DDLWorkPackage.DataSource = ds;
                //    DDLWorkPackage.DataBind();
                //}
            }

        }

        private void BindTaskSchuldeConsolidated(string ProjectUID,string MeetingUID)
        {
            ProjectProgress.Visible = true;
            DataSet ds = getdt.GetConsMonthActivity_by_ProjectUID_MeetingID(ProjectUID, MeetingUID);
            if (ds.Tables[0].Rows.Count > 0)
            {
                GrdProjectProgress.DataSource = ds;
                GrdProjectProgress.DataBind();
                //btnExportPDF.Visible = true;
                ConstructionProgrammeDiv.Visible = true;
            }
            else
            {
                GrdProjectProgress.DataSource = null;
                GrdProjectProgress.DataBind();
                //btnExportPDF.Visible = false;
                ConstructionProgrammeDiv.Visible = false;
            }
        }

        private void BindWorkprogress(string ProjectUID, string MeetingUID)
        {
            ProjectProgress.Visible = true;
            DataSet ds = getdt.GetConsActivity_by_ProjectUID_meetingid(ProjectUID, MeetingUID);
            if (ds.Tables[0].Rows.Count > 0)
            {
                GrdWorkProgress.DataSource = ds;
                GrdWorkProgress.DataBind();
                //btnExportPDF.Visible = true;
                ProjectWorkProgressDiv.Visible = true;
            }
            else
            {
                GrdWorkProgress.DataSource = null;
                GrdWorkProgress.DataBind();
                //btnExportPDF.Visible = false;
                ProjectWorkProgressDiv.Visible = false;
            }
        }
        //private void BindTaskSchedule(string Workpackage,DateTime sdate)
        //{
            //ProjectProgress.Visible = true;
            
            //DataSet ds = getdt.GetTaskSchedule_By_Workpackage_Date(new Guid(Workpackage), sdate);
            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    GrdProjectProgress.DataSource = ds;
            //    GrdProjectProgress.DataBind();
            //    btnExportPDF.Visible = true;
            //}
            //else
            //{
            //    GrdProjectProgress.DataSource = null;
            //    GrdProjectProgress.DataBind();
            //    btnExportPDF.Visible = false;
            //}
        //}
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                string sDate1 = "", sDate2 = "";
                DateTime CDate1 = DateTime.Now, CDate2 = DateTime.Now;
                DataSet dsmeeting = getdt.GetMeetingMaster_by_Meeting_UID(new Guid(DDLMeetingMaster.SelectedValue));
                if(dsmeeting.Tables[0].Rows.Count>0)
                {
                    sDate1 = Convert.ToDateTime(dsmeeting.Tables[0].Rows[0]["CreatedDate"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                //sDate1 = "01/" + DDLMonth.SelectedValue + "/" + DDLYear.SelectedValue;
                //sDate1 = dtReviewDate.Text;
                sDate1 = getdt.ConvertDateFormat(sDate1);
                CDate1 = Convert.ToDateTime(sDate1);

                //int Days = DateTime.DaysInMonth(Convert.ToInt32(DDLYear.SelectedValue), Convert.ToInt32(DDLMonth.SelectedValue)) - 1;
                //CDate2 = CDate1.AddDays(Days);

                HiddenDate.Value = CDate1.ToString("dd/MM/yyyy");
                BindTaskSchuldeConsolidated(DDlProject.SelectedValue,DDLMeetingMaster.SelectedValue);
                BindWorkprogress(DDlProject.SelectedValue,DDLMeetingMaster.SelectedValue);
                //BindTaskSchedule(DDLWorkPackage.SelectedValue, CDate2);
                GetSitePhotographs(DDlProject.SelectedValue, DDLMeetingMaster.SelectedValue);
                BindContractor(DDlProject.SelectedValue);
                LblPprojectprogress.Text = ("Project Status of Work Progress as on " + HiddenDate.Value).ToUpper();

                if (GrdProjectProgress.Rows.Count > 0 || GrdWorkProgress.Rows.Count > 0)
                {
                    btnExportPDF.Visible = true;
                    LblEmptyWorkProgress.Visible = false;
                }
                else
                {
                    btnExportPDF.Visible = false;
                    LblEmptyWorkProgress.Visible = true;
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code : " + ex.Message + "');</script>");
            }
            
        }

        protected void GrdProjectProgress_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[2].Text = "Target as on " + HiddenDate.Value + " Submitted Construction Programme";
                e.Row.Cells[3].Text = "Achieved as on " + HiddenDate.Value;
                //e.Row.Cells[2].Text = "Target as on 11/01/2021 Submitted Construction Programme";
                //e.Row.Cells[3].Text = "Achieved as on 11/01/2021";
            }
        }

        //protected void GrdProjectProgress_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    if (e.Row.RowType == DataControlRowType.Header)
        //    {
        //        e.Row.Cells[2].Text = "Target as on " + HiddenDate.Value + " Submitted Construction Programme";
        //        e.Row.Cells[3].Text = "Achieved as on " + HiddenDate.Value;
        //    }
        //    if (e.Row.RowType == DataControlRowType.DataRow)
        //    {


        //        if (e.Row.Cells[2].ToString() != "" && e.Row.Cells[3].ToString() != "")
        //        {
        //            string TargetValue = e.Row.Cells[2].Text;
        //            string AchievedValue = e.Row.Cells[3].Text;

        //            float Percentage = (float.Parse(AchievedValue) / float.Parse(TargetValue)) * 100;
        //            e.Row.Cells[4].Text = Math.Round(Percentage).ToString() + "%";
        //        }

        //    }
        //}

        public string GetTaskName(string TaskUID)
        {
            return getdt.getTaskNameby_TaskUID(new Guid(TaskUID));
        }

        protected void btnExportPDF_Click(object sender, EventArgs e)
        {
            try
            {
                if (ConstructionProgrammeDiv.Visible == true)
                {
                    ExportConstructionProgramme();
                }
                else
                {
                    ExportWorkProgress();
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code : Export Work Progress:01 There is a problem with this feature. Please contact system admin.');</script>");
            }
        }

        private void ExportConstructionProgramme()
        {
            //string sDate1 = "", sDate2 = "";
            //DateTime CDate1 = DateTime.Now, CDate2 = DateTime.Now;

            //sDate1 = dtReviewDate.Text;
            //sDate1 = getdt.ConvertDateFormat(sDate1);
            //CDate1 = Convert.ToDateTime(sDate1);

            //int Days = DateTime.DaysInMonth(Convert.ToInt32(DDLYear.SelectedValue), Convert.ToInt32(DDLMonth.SelectedValue)) - 1;
            //CDate2 = CDate1.AddDays(Days);


            //to allow paging=false & change style.
            GrdProjectProgress.HeaderStyle.ForeColor = System.Drawing.Color.Black;
            GrdProjectProgress.BorderColor = System.Drawing.Color.Black;
            GrdProjectProgress.Font.Name = "Arial";
            GrdProjectProgress.DataSource = getdt.GetConsMonthActivity_by_ProjectUID_MeetingID(DDlProject.SelectedValue,DDLMeetingMaster.SelectedValue);
            GrdProjectProgress.AllowPaging = true;
            //GrdDocumentMaster.Columns[0].Visible = false; //export won't work if there's a link in the gridview
            GrdProjectProgress.DataBind();

            StringWriter stw = new StringWriter();
            HtmlTextWriter htextw = new HtmlTextWriter(stw);
            htextw.AddStyleAttribute("font-size", "9pt");
            htextw.AddStyleAttribute("color", "Black");

            GrdProjectProgress.RenderControl(htextw); //Name of the Panel

            //var sb1 = new StringBuilder();
            //GridDiv.RenderControl(new HtmlTextWriter(new StringWriter(sb1)));

            string s = htextw.InnerWriter.ToString();
            string HTMLstring = "<html><head><meta http-equiv='Content-Type' content='text/html; charset=utf-8' /></head> <body>" +
                "<div style='width:100%; margin:auto;'><div style='width:100%; float:left; line-height:25px;' align='center'>";
            HTMLstring += "<h2>" + WebConfigurationManager.AppSettings["Domain"] + "</h2>";
            HTMLstring += "<h3>Project Status of Work Progress as on "+ HiddenDate.Value + "</h3>" +
            "<h3>Contract Package : " + DDlProject.SelectedItem.Text + "</h3>" +
            "</div> <div style='width:100%; float:left;'><br/><br/></div>";
            HTMLstring += GetContractorDetails_by_ProjectUID(DDlProject.SelectedValue);
            HTMLstring += "<div style='width:100%; float:left;'><br/><br/></div>";
            HTMLstring += "<div style='page-break-before:always'>&nbsp;</div>";
            HTMLstring += "<div style='width:100%; float:left;'><h4>Project Status of Work Progress :</h4><br/><br/></div>";
            HTMLstring += "<div style='width:100%; float:left;'>" +
            s +
            "</div>";
            HTMLstring += "<div style='width:100%; float:left;'><br/><br/></div>";
            //HTMLstring += "<div style='width:100%; float:left;'><br/><br/></div>";
            HTMLstring += "<div style='float:left; width:100%;'><div style='float:left; width:100%;'><h4>Site Photographs :</h4></div>";
            HTMLstring += GetSitePhotoGraphsForExport(DDlProject.SelectedValue,DDLMeetingMaster.SelectedValue);
            HTMLstring += "</div></body></html>";


            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                {
                    StringBuilder sb = new StringBuilder();

                    //Export HTML String as PDF.
                    StringReader sr = new StringReader(HTMLstring);
                    Document pdfDoc = new Document(PageSize.A4.Rotate(), 10f, 10f, 10f, 0);

                    
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
                    pdfDoc.Open();

                    //              BaseFont bf =
                    //BaseFont.CreateFont("c:/windows/fonts/arial.ttf",
                    //                    BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                    //              Font font = new Font(bf, 12);
                    //Chunk chunkRupee = new Chunk(" \u20B9 5410", font);
                    //pdfDoc.Add(chunkRupee);
                    //FontSelector selector = new FontSelector();
                    //selector.AddFont(FontFactory.GetFont(FontFactory.TIMES_ROMAN, 12));
                    //selector.AddFont(FontFactory.GetFont("MSung-Light", "UniCNS-UCS2-H", BaseFont.NOT_EMBEDDED));
                    //Phrase ph = selector.Process(HTMLstring);
                    //BaseFont helvetica = BaseFont.CreateFont("Helvetica", BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                    //Font font = new Font(helvetica, 12, Font.NORMAL);
                    //Chunk chunk = new Chunk("Euro symbol:  20\u20B9.", font);
                    //pdfDoc.Add(chunk);
                    //                BaseFont bf = 
                    //BaseFont.CreateFont(Server.MapPath("~/_assets/webfonts/ARIALUNICODEMS.TTF"),
                    //                    BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                    //                Font font = new Font(bf, 12);
                    //                Chunk chunkRupee = new Chunk(" \u20B9 5410", font);
                    //                pdfDoc.Add(chunkRupee);
                    htmlparser.Parse(sr);
                    pdfDoc.Close();
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "attachment;filename=Report_ProjectProgress_" + DateTime.Now.Ticks + ".pdf");
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.Write(pdfDoc);
                    Response.End();

                }


            }
        }

        private void ExportWorkProgress()
        {
            //string sDate1 = "", sDate2 = "";
            //DateTime CDate1 = DateTime.Now, CDate2 = DateTime.Now;

            //sDate1 = "01/" + DDLMonth.SelectedValue + "/" + DDLYear.SelectedValue;
            //sDate1 = getdt.ConvertDateFormat(sDate1);
            //CDate1 = Convert.ToDateTime(sDate1);

            //int Days = DateTime.DaysInMonth(Convert.ToInt32(DDLYear.SelectedValue), Convert.ToInt32(DDLMonth.SelectedValue)) - 1;
            //CDate2 = CDate1.AddDays(Days);


            //to allow paging=false & change style.
            GrdWorkProgress.HeaderStyle.ForeColor = System.Drawing.Color.Black;
            GrdWorkProgress.BorderColor = System.Drawing.Color.Black;
            GrdWorkProgress.Font.Name = "Arial";
            GrdWorkProgress.DataSource = getdt.GetConsActivity_by_ProjectUID_meetingid(DDlProject.SelectedValue,DDLMeetingMaster.SelectedValue);
            GrdWorkProgress.AllowPaging = true;
            //GrdDocumentMaster.Columns[0].Visible = false; //export won't work if there's a link in the gridview
            GrdWorkProgress.DataBind();

            StringWriter stw = new StringWriter();
            HtmlTextWriter htextw = new HtmlTextWriter(stw);
            htextw.AddStyleAttribute("font-size", "9pt");
            htextw.AddStyleAttribute("color", "Black");

            GrdWorkProgress.RenderControl(htextw); //Name of the Panel

            //var sb1 = new StringBuilder();
            //GridDiv.RenderControl(new HtmlTextWriter(new StringWriter(sb1)));

            string s = htextw.InnerWriter.ToString();
            string HTMLstring = "<html><head><meta http-equiv='Content-Type' content='text/html; charset=utf-8' /></head> <body>" +
                "<div style='width:100%; margin:auto;'><div style='width:100%; float:left; line-height:25px;' align='center'>";
            HTMLstring += "<h2>" + WebConfigurationManager.AppSettings["Domain"] + "</h2>";
            HTMLstring += "<h3>Project Status of Work Progress as on "+HiddenDate.Value+"</h3>" +
            "<h3>Contract Package : " + DDlProject.SelectedItem.Text + "</h3>" +
            "</div> <div style='width:100%; float:left;'><br/><br/></div>";
            HTMLstring += GetContractorDetails_by_ProjectUID(DDlProject.SelectedValue);
            HTMLstring += "<div style='width:100%; float:left;'><br/><br/></div>";
            HTMLstring += "<div style='page-break-before:always'>&nbsp;</div>";
            HTMLstring += "<div style='width:100%; float:left;'><h4>Project Status of Work Progress :</h4><br/><br/></div>";
            HTMLstring += "<div style='width:100%; float:left;'>" +
            s +
            "</div>";
            HTMLstring += "<div style='width:100%; float:left;'><br/><br/></div>";
            HTMLstring += "<div style='float:left; width:100%;'><div style='float:left; width:100%;'><h4>Site Photographs :</h4></div>";
            HTMLstring += GetSitePhotoGraphsForExport(DDlProject.SelectedValue, DDLMeetingMaster.SelectedValue);
            HTMLstring += "</div></body></html>";


            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                {
                    StringBuilder sb = new StringBuilder();

                    //Export HTML String as PDF.
                    StringReader sr = new StringReader(HTMLstring);
                    Document pdfDoc = new Document(PageSize.A4.Rotate(), 10f, 10f, 10f, 0);

                    //iTextSharp.text.Font foot = new iTextSharp.text.Font();
                   // foot.Size = 10;
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
                    htmlparser.Parse(sr);
                    pdfDoc.Close();
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "attachment;filename=Report_ProjectProgress_" + DateTime.Now.Ticks + ".pdf");
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.Write(pdfDoc);
                    Response.End();

                }


            }
        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            //required to avoid the runtime error "  
            //Control 'GridView1' of type 'GridView' must be placed inside a form tag with runat=server."  
        }
        private void BindContractor(string ProjectUID)
        {
            DataSet ds = getdt.GetContractor_By_ProjectUID(new Guid(ProjectUID));
            if (ds.Tables[0].Rows.Count > 0)
            {
                LblNameoftheContractor.Text = ds.Tables[0].Rows[0]["Contractor_Name"].ToString();
                string CurrencySymbol = "";
                if (ds.Tables[0].Rows[0]["Currency"].ToString() == "&#x20B9;")
                {
                    CurrencySymbol = "Rs. ";
                }
                else if (ds.Tables[0].Rows[0]["Currency"].ToString() == "&#36;")
                {
                    CurrencySymbol = "$";
                }
                else
                {
                    CurrencySymbol = "¥";
                }
                if (GrdProjectProgress.Rows.Count > 0 || GrdWorkProgress.Rows.Count > 0)
                {
                    LblCostName.Text = "Awarded Cost (Rs.)";
                    LblDateofLOA.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["Letter_of_Acceptance"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    LblDateofAgreement.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["Contract_Agreement_Date"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    LblDateofCommencement.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["Contract_StartDate"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    LblDateofCompletion.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["Contract_Completion_Date"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                else
                {
                    LblCostName.Text = "Sactioned Cost (Rs.)";
                    LblDateofLOA.Text = "NA";
                    LblDateofAgreement.Text = "NA";
                    LblDateofCommencement.Text = "NA";
                    LblDateofCompletion.Text = "NA";
                }
               
                //LblTotalCost.Text = CurrencySymbol + Convert.ToDouble(ds.Tables[0].Rows[0]["Contract_Value"].ToString()).ToString("#,##.##", CultureInfo.CreateSpecificCulture(ds.Tables[0].Rows[0]["Currency_CultureInfo"].ToString()));
                LblTotalCost.Text = ds.Tables[0].Rows[0]["Contractor_Representatives"].ToString();
              
                LblPeriodofCompletion.Text = ds.Tables[0].Rows[0]["Contract_Duration"].ToString() + " Months";

            }
        }
        public string GetContractorDetails_by_ProjectUID(string ProjectUID)
        {
            string HTMLString = "";
            //DataSet ds = getdt.GetContractor_By_WorkpackageUID(new Guid(WorkpackageUID));
            DataSet ds = getdt.GetContractor_By_ProjectUID(new Guid(ProjectUID));
            if (ds.Tables[0].Rows.Count > 0)
            {
                string CurrencySymbol = "";
                if (ds.Tables[0].Rows[0]["Currency"].ToString() == "&#x20B9;")
                {
                    CurrencySymbol = "Rs. ";
                }
                else if (ds.Tables[0].Rows[0]["Currency"].ToString() == "&#36;")
                {
                    CurrencySymbol = "$";
                }
                else
                {
                    CurrencySymbol = "¥";
                }

                //BaseFont bf =
                //            BaseFont.CreateFont("c:/windows/fonts/arial.ttf",
                //          BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                //Font font = new Font(bf, 12);
                //Chunk chunkRupee = new Chunk(" \u20B9 ", font);

                HTMLString += "<div style='float:left; width:100%; font-size:10pt;'>" +
                    "<table border='1' cellpadding='4' cellspacing='4' style='width:100%;'>" +
                    "<tr><td style='font-weight:bold;'>Name of the Contractor</td><td width='45%'>" + ds.Tables[0].Rows[0]["Contractor_Name"].ToString() + "</td></tr>" +
                    "<tr><td style='font-weight:bold; font-family:Arial;'>Awarded Cost (Rs.)</td><td width='45%'>" + ds.Tables[0].Rows[0]["Contractor_Representatives"].ToString()  + "</td></tr>" +
                    "<tr><td style='font-weight:bold;'>Date of issue of LOA</td><td width='45%'>" + Convert.ToDateTime(ds.Tables[0].Rows[0]["Letter_of_Acceptance"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + "</td></tr>" +
                    "<tr><td style='font-weight:bold;'>Date of signing of Agreement</td><td width='45%'>" + Convert.ToDateTime(ds.Tables[0].Rows[0]["Contract_Agreement_Date"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + "</td></tr>" +
                    "<tr><td style='font-weight:bold;'>Date of Commencement</td><td width='45%'>" + Convert.ToDateTime(ds.Tables[0].Rows[0]["Contract_StartDate"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + "</td></tr>" +
                    "<tr><td style='font-weight:bold;'>Date of Completion</td><td width='45%'>" + Convert.ToDateTime(ds.Tables[0].Rows[0]["Contract_Completion_Date"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + "</td></tr>" +
                    "<tr><td style='font-weight:bold;'>Period of Completion</td><td width='45%'>" + ds.Tables[0].Rows[0]["Contract_Duration"].ToString() + " Months</td></tr>" +
                    "</table></div";
            }
            return HTMLString;
        }

        public string GetSitePhotoGraphsForExport(string ProjectUID, string MeetingUID)
        {
            string HTMLString = "";

            //GrdSitePhotograph.HeaderStyle.ForeColor = System.Drawing.Color.Black;
            //GrdSitePhotograph.BorderColor = System.Drawing.Color.Black;
            //GrdSitePhotograph.Font.Name = "Arial";
            //GrdSitePhotograph.DataSource = getdt.GetSiteLatestPhotograph_by_ProjectUID(new Guid(DDlProject.SelectedValue));

            //GrdSitePhotograph.DataBind();

            //StringWriter stw = new StringWriter();
            //HtmlTextWriter htextw = new HtmlTextWriter(stw);
            //htextw.AddStyleAttribute("font-size", "9pt");
            //htextw.AddStyleAttribute("color", "Black");

            //GrdSitePhotograph.RenderControl(htextw); //Name of the Panel


            //string s = htextw.InnerWriter.ToString();
            //HTMLString = s;
            //DataSet ds = getdt.GetTaskSitePhotoGraphs_by_Workpackage_Date(new Guid(WorkpackageUID), SelectedDate);
            //DataSet ds = getdt.GetSiteLatestPhotograph_by_ProjectUID(new Guid(ProjectUID));
            DataSet ds = getdt.MeetingSitePhotoGraphs_Selectby_ProjectUID_Meeting_UID(new Guid(ProjectUID), new Guid(MeetingUID));
            if (ds.Tables[0].Rows.Count > 0)
            {
                HTMLString += "<table class='table table-borderless'>";
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string sImagePath = ds.Tables[0].Rows[i]["Site_Image"].ToString().Replace("~/", "");
                    if (i % 2 == 0)
                    {
                        HTMLString += "<tr><td><img src='"+ WebConfigurationManager.AppSettings["SiteName"]  + sImagePath + "' width='200' alt='' /><br/>"+ ds.Tables[0].Rows[i]["Description"].ToString() + "</td>";
                    }
                    else
                    {
                        HTMLString += "<td><img src='" + WebConfigurationManager.AppSettings["SiteName"]  + sImagePath + "' width='200' alt='' /><br/>" + ds.Tables[0].Rows[i]["Description"].ToString() + "</td></tr>";
                    }
                    
                    //HTMLString += "<div style='width:250; float:left; border:1px solid Gray; text-align:center; background-color:#f2f2f2; text-align:center;'>" +
                    //    "<div style='padding:10px;'>" +
                    //        "<img src='http://localhost:50162" + sImagePath + "' width='200' alt='' /><br/>" +
                    //        ds.Tables[0].Rows[i]["Description"].ToString() +
                    //"</div>" +
                    //    "</div>";
                }
                if (ds.Tables[0].Rows.Count % 2 != 0)
                {
                    HTMLString += "<td></td></tr>";
                }
                HTMLString += "</table>";
            }

            return HTMLString;
        }
        private void GetSitePhotographs(string ProjectUID,string MeetingUID)
        {
            //DataSet ds = getdt.GetTaskSitePhotoGraphs_by_Workpackage_Date(new Guid(WorkpackageUID), SelectedDate);
            //DataSet ds = getdt.GetSiteLatestPhotograph_by_ProjectUID(new Guid(ProjectUID));
            DataSet ds = getdt.MeetingSitePhotoGraphs_Selectby_ProjectUID_Meeting_UID(new Guid(ProjectUID),new Guid(MeetingUID));
            GrdSitePhotograph.DataSource = ds;
            GrdSitePhotograph.DataBind();
            if (ds.Tables[0].Rows.Count > 0)
            {
                lblEmptyData.Visible = false;
            }
            else
            {
                lblEmptyData.Visible = true;
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

        //protected void GrdWorkProgress_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    if (e.Row.RowType == DataControlRowType.DataRow)
        //    {
        //        e.Row.Cells[2].Text = e.Row.Cells[2].Text.Replace("\n", "<br />");
        //    }
        //}
    }
}