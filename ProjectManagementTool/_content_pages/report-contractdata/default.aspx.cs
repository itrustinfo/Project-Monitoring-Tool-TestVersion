using iTextSharp.text;
using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System.Web.Configuration;


namespace ProjectManagementTool._content_pages.report_contractdata
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
                    ContractData.Visible = false;
                    btnExportReportPDF.Visible = false;
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
                    ContractData.Visible = true;
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
                headingReport.InnerHtml = "Report Name : Contract Details";
                headingProject.InnerHtml = "Project Name : " + DDlProject.SelectedItem.Text + " (" + DDLWorkPackage.SelectedItem.Text + ")";
                DataSet ds = getdt.GetWorkpackge_Contractor_Data_by_WorkpackageUID(new Guid(DDLWorkPackage.SelectedValue));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    btnExportReportPDF.Visible = true;
                    DataTable dt = new DataTable();
                    dt.Columns.Add("Description");
                    dt.Columns.Add("Value");
                    string PrjtName = "";
                    for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
                    {
                        DataRow dr = dt.NewRow();
                        if (ds.Tables[0].Columns[i].ToString() == "ProjectUID")
                        {
                            PrjtName= getdt.getProjectNameby_ProjectUID(new Guid(ds.Tables[0].Rows[0]["ProjectUID"].ToString()));
                            dr["Description"] = "Name of the Project";
                            dr["value"] = PrjtName;
                        }
                        else if (ds.Tables[0].Columns[i].ToString() == "ProjectEndDate")
                        {
                            dr["Description"] = "Project End Date";
                            
                            dr["value"] = ds.Tables[0].Rows[0]["ProjectEndDate"].ToString() != "" ? Convert.ToDateTime(ds.Tables[0].Rows[0]["ProjectEndDate"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : "-";
                        }
                        else if (ds.Tables[0].Columns[i].ToString() == "FundingAgency")
                        {                            
                            dr["Description"] = "Funding Agency";
                            dr["value"] = ds.Tables[0].Rows[0]["FundingAgency"].ToString();
                        }
                        else if (ds.Tables[0].Columns[i].ToString() == "Name")
                        {
                            dr["Description"] = "Title of the work";
                            dr["value"] = ds.Tables[0].Rows[0]["Name"].ToString() + " - " + PrjtName;
                        }
                        else if (ds.Tables[0].Columns[i].ToString() == "WorkPackage_Location")
                        {
                            dr["Description"] = "Location";
                            dr["value"] = ds.Tables[0].Rows[0]["WorkPackage_Location"].ToString();
                        }
                        else if (ds.Tables[0].Columns[i].ToString() == "WorkPackage_Client")
                        {
                            dr["Description"] = "Client";
                            dr["value"] = ds.Tables[0].Rows[0]["WorkPackage_Client"].ToString();
                        }
                        else if (ds.Tables[0].Columns[i].ToString() == "Contractor_Name")
                        {
                            dr["Description"] = "Contractor";
                            dr["value"] = ds.Tables[0].Rows[0]["Contractor_Name"].ToString();
                        }
                        else if (ds.Tables[0].Columns[i].ToString() == "Contractor_Representatives")
                        {
                            dr["Description"] = "Contractor Representatives";
                            dr["value"] = ds.Tables[0].Rows[0]["Contractor_Representatives"].ToString();
                        }
                        else if (ds.Tables[0].Columns[i].ToString() == "Contractor_Representatives_Details")
                        {
                            dr["Description"] = "Contractor Representatives Details";
                            dr["value"] = ds.Tables[0].Rows[0]["Contractor_Representatives_Details"].ToString();
                        }
                        else if (ds.Tables[0].Columns[i].ToString() == "Company_Details")
                        {
                            dr["Description"] = "Company Details";
                            dr["value"] = ds.Tables[0].Rows[0]["Company_Details"].ToString();
                        }
                        else if (ds.Tables[0].Columns[i].ToString() == "Type_of_Contract")
                        {
                            dr["Description"] = "Type of Contract";
                            dr["value"] = ds.Tables[0].Rows[0]["Type_of_Contract"].ToString();
                        }
                        else if (ds.Tables[0].Columns[i].ToString() == "Contract_Value")
                        {
                            string CurrencySymbol = "";
                            if (ds.Tables[0].Rows[0]["Currency"].ToString() == "&#x20B9;")
                            {
                                CurrencySymbol = "INR";
                            }
                            else if (ds.Tables[0].Rows[0]["Currency"].ToString() == "&#36;")
                            {
                                CurrencySymbol = "USD";
                            }
                            else
                            {
                                CurrencySymbol = "YEN";
                            }

                            dr["Description"] = "Contract Value";
                            dr["value"] = CurrencySymbol + " " + Convert.ToDouble(ds.Tables[0].Rows[0]["Contract_Value"].ToString()).ToString("#,##.##", CultureInfo.CreateSpecificCulture(ds.Tables[0].Rows[0]["Currency_CultureInfo"].ToString()));
                        }
                        else if (ds.Tables[0].Columns[i].ToString() == "Letter_of_Acceptance")
                        {
                            dr["Description"] = "Letter of Acceptance";
                            dr["value"] = ds.Tables[0].Rows[0]["Letter_of_Acceptance"].ToString() != "" ? Convert.ToDateTime(ds.Tables[0].Rows[0]["Letter_of_Acceptance"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : "-";
                        }
                        else if (ds.Tables[0].Columns[i].ToString() == "Contract_Agreement_Date")
                        {
                            dr["Description"] = "Contract Agreement Date";
                            dr["value"] = ds.Tables[0].Rows[0]["Contract_Agreement_Date"].ToString() != "" ? Convert.ToDateTime(ds.Tables[0].Rows[0]["Contract_Agreement_Date"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : "-";
                        }
                        else if (ds.Tables[0].Columns[i].ToString() == "Contract_Duration")
                        {
                            dr["Description"] = "Contract Duration";
                            dr["value"] = ds.Tables[0].Rows[0]["Contract_Duration"].ToString() + " Months";
                        }
                        else if (ds.Tables[0].Columns[i].ToString() == "Contract_StartDate")
                        {
                            dr["Description"] = "Contract StartDate";
                            dr["value"] = ds.Tables[0].Rows[0]["Contract_StartDate"].ToString() != "" ? Convert.ToDateTime(ds.Tables[0].Rows[0]["Contract_StartDate"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : "-";
                        }
                        else if (ds.Tables[0].Columns[i].ToString() == "Contract_Completion_Date")
                        {
                            dr["Description"] = "Contract Completion Date";
                            dr["value"] = ds.Tables[0].Rows[0]["Contract_Completion_Date"].ToString() != "" ? Convert.ToDateTime(ds.Tables[0].Rows[0]["Contract_Completion_Date"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : "-";
                        }
                        else if (ds.Tables[0].Columns[i].ToString() == "Defects_Liability_Period")
                        {
                            dr["Description"] = "Defects Liability Period";
                            dr["value"] = ds.Tables[0].Rows[0]["Defects_Liability_Period"].ToString() + " Days";
                        }
                        if (ds.Tables[0].Columns[i].ToString() != "Currency" && ds.Tables[0].Columns[i].ToString() != "Currency_CultureInfo")
                        {
                            dt.Rows.Add(dr);
                        }
                        
                    }
                    GrdContractData.DataSource = dt;
                    GrdContractData.DataBind();
                    //string ProjectName = getdt.getProjectNameby_ProjectUID(new Guid(ds.Tables[0].Rows[0]["ProjectUID"].ToString()));
                    //LblNameoftheProject.Text = ProjectName;
                    //LblFundingAgency.Text= ds.Tables[0].Rows[0]["FundingAgency"].ToString();
                    //LblTitleoftheWork.Text = ds.Tables[0].Rows[0]["Name"].ToString() + " - " + ProjectName;
                    //LblLocation.Text = ds.Tables[0].Rows[0]["WorkPackage_Location"].ToString();
                    //LblClient.Text = ds.Tables[0].Rows[0]["WorkPackage_Client"].ToString();
                    ////LblEmployer.Text = ds.Tables[0].Rows[0][""].ToString();
                    ////LblEngineer.Text = ds.Tables[0].Rows[0][""].ToString();
                    //LblContractor.Text = ds.Tables[0].Rows[0]["Contractor_Name"].ToString();
                    //LblContractorRepresentatives.Text = ds.Tables[0].Rows[0]["Contractor_Representatives"].ToString();
                    //LblTypeofContract.Text = ds.Tables[0].Rows[0]["Type_of_Contract"].ToString();

                    //string CurrencySymbol = "";
                    //if (ds.Tables[0].Rows[0]["Currency"].ToString() == "&#x20B9;")
                    //{
                    //    CurrencySymbol = "INR";
                    //}
                    //else if (ds.Tables[0].Rows[0]["Currency"].ToString() == "&#36;")
                    //{
                    //    CurrencySymbol = "USD";
                    //}
                    //else
                    //{
                    //    CurrencySymbol = "YEN";
                    //}
                    //LblContractValue.Text = CurrencySymbol + " " + Convert.ToDouble(ds.Tables[0].Rows[0]["Contract_Value"].ToString()).ToString("#,##.##", CultureInfo.CreateSpecificCulture(ds.Tables[0].Rows[0]["Currency_CultureInfo"].ToString()));
                    //if (ds.Tables[0].Rows[0]["Letter_of_Acceptance"].ToString() != "")
                    //{
                    //    LblLetterofAcceptance.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["Letter_of_Acceptance"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    //}

                    //if (ds.Tables[0].Rows[0]["Contract_Agreement_Date"].ToString() != "")
                    //{
                    //    LblContractAgressmentDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["Contract_Agreement_Date"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    //}

                    //LblContractDuration.Text = ds.Tables[0].Rows[0]["Contract_Duration"].ToString() + " Months";

                    //if (ds.Tables[0].Rows[0]["Contract_StartDate"].ToString() != "")
                    //{
                    //    LblContractStartDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["Contract_StartDate"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    //}

                    //if (ds.Tables[0].Rows[0]["Contract_Completion_Date"].ToString() != "")
                    //{
                    //    LblContractCompletionDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["Contract_Completion_Date"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    //}

                    //LblDefectsLiabilityPeriod.Text = ds.Tables[0].Rows[0]["Defects_Liability_Period"].ToString() + " Days";
                }
            }
        }

        protected void btnExportReportPDF_Click(object sender, EventArgs e)
        {
            ExporttoPDF(GrdContractData, 2, "No");
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            //required to avoid the runtime error "  
            //Control 'GridView1' of type 'GridView' must be placed inside a form tag with runat=server."  
        }

        protected void btnexcelexport_Click(object sender, EventArgs e)
        {
            try
            {
                StringBuilder StrExport = new StringBuilder();

                StringWriter stw = new StringWriter();
                HtmlTextWriter htextw = new HtmlTextWriter(stw);
                htextw.AddStyleAttribute("font-size", "11pt");
                htextw.AddStyleAttribute("color", "Black");
                


                GrdContractData.RenderControl(htextw); //Name of the Panel

                //var sb1 = new StringBuilder();
                //GridDiv.RenderControl(new HtmlTextWriter(new StringWriter(sb1)));

                string s = htextw.InnerWriter.ToString();

                string HTMLstring = "<html><body>" +
                    "<div style='width:100%; margin:auto;'><div style='width:100%; float:left; line-height:25px; font-size:12pt;' align='center'>" +
                    "<asp:Label ID='Lbl1' runat='server' Font-Bold='true'>" + WebConfigurationManager.AppSettings["Domain"] + " Report Name: Contract Details</asp:Label><br />" +
                    "<asp:Label ID='Lbl4' runat='server'>Project Name : " + DDlProject.SelectedItem.Text + " (" + DDLWorkPackage.SelectedItem.Text + ")</asp:Label><br />" +
                    "</div> <div style='width:100%; float:left; height:10px;'>&nbsp;&nbsp;&nbsp;</div>" +
                    "<div style='width:100%;font-size:11pt float:left;' align='left'>" +
                    s +
                    "</div>" +
                    "</div></body></html>";

                string strFile = "Report__ContractData_" + DateTime.Now.Ticks + ".xls";
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

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            ExporttoPDF(GrdContractData, 2, "Yes");
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

                float HeaderTextSize = 11;
                float ReportNameSize = 11;
                float ReportTextSize = 11;
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
                phApplicationName = new Phrase(Environment.NewLine + WebConfigurationManager.AppSettings["Domain"] + " Report Name: Contract Details", FontFactory.GetFont("Arial", ApplicationNameSize, iTextSharp.text.Font.BOLD));
                ExportFileName = "Report_Contract_Details_" + DateTime.Now.Ticks + ".pdf";
                mainTable.SetWidths(new float[] { 40, 60 });


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
                                cl.HorizontalAlignment = Element.ALIGN_LEFT;
                                mainTable.AddCell(cl);
                            }
                            else
                            {
                                if (columnNo == 0)
                                {
                                    if (gdRp.Columns[columnNo] is TemplateField)
                                    {
                                        DataBoundLiteralControl lc = gdRp.Rows[rowNo].Cells[columnNo].Controls[0] as DataBoundLiteralControl;
                                        string s = "  " + lc.Text.Trim();
                                        Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.BOLD));
                                        PdfPCell cl = new PdfPCell(ph);
                                        cl.HorizontalAlignment = Element.ALIGN_LEFT;
                                        cl.VerticalAlignment = Element.ALIGN_MIDDLE;
                                        cl.FixedHeight = 25;
                                        mainTable.AddCell(cl);
                                    }
                                    else
                                    {
                                        string s = gdRp.Rows[rowNo].Cells[columnNo].Text.Trim();
                                        s = "  " + s.Replace("&nbsp;", "");
                                        Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.BOLD));
                                        PdfPCell cl = new PdfPCell(ph);
                                        cl.HorizontalAlignment = Element.ALIGN_LEFT;
                                        cl.VerticalAlignment = Element.ALIGN_MIDDLE;
                                        cl.FixedHeight = 25;
                                        mainTable.AddCell(cl);
                                    }
                                }
                                else
                                {
                                    if (gdRp.Columns[columnNo] is TemplateField)
                                    {
                                        DataBoundLiteralControl lc = gdRp.Rows[rowNo].Cells[columnNo].Controls[0] as DataBoundLiteralControl;
                                        string s = "  " + lc.Text.Trim();
                                        Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                                        PdfPCell cl = new PdfPCell(ph);
                                        cl.HorizontalAlignment = Element.ALIGN_LEFT;
                                        cl.VerticalAlignment = Element.ALIGN_MIDDLE;
                                        cl.FixedHeight = 25;
                                        mainTable.AddCell(cl);
                                    }
                                    else
                                    {
                                        string s = gdRp.Rows[rowNo].Cells[columnNo].Text.Trim();
                                        s = "  " + s.Replace("&nbsp;", "");
                                        Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                                        PdfPCell cl = new PdfPCell(ph);
                                        cl.HorizontalAlignment = Element.ALIGN_LEFT;
                                        cl.VerticalAlignment = Element.ALIGN_MIDDLE;
                                        cl.FixedHeight = 25;
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
    }
}