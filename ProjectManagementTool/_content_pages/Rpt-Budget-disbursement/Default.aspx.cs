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

namespace ProjectManagementTool._content_pages.Rpt_Budget_disbursement
{
    public partial class Default : System.Web.UI.Page
    {
        DBGetData getdata = new DBGetData();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlmeeting.DataSource = getdata.GetMeetingMaster();
                ddlmeeting.DataTextField = "Meeting_Description";
                ddlmeeting.DataValueField = "Meeting_UID";
                ddlmeeting.DataBind();
                LoadGridDataBudget();
            }
        }

        private void LoadGridDataBudget()
        {
            try
            {
                //display heading
                DataSet dsheadimg = getdata.GetMeetingMaster();
                foreach(DataRow drh in dsheadimg.Tables[0].Rows)
                {
                    if(drh["Meeting_UID"].ToString() == ddlmeeting.SelectedValue)
                    {
                        heading.InnerHtml = "Status of Budget Vs Disbursement as on " + Convert.ToDateTime(drh["CreatedDate"].ToString()).ToString("dd.MM.yyyy");
                    }
                }

                GrdBudgetbsDisbursemnt.DataSource = getdata.GetBudgetvsDisbursement(new Guid(ddlmeeting.SelectedValue));
                GrdBudgetbsDisbursemnt.DataBind();

                //
                DataTable dt = new DataTable();
                DataRow dr;


                // Add Columns to datatablse
                dt.Columns.Add(new DataColumn("ProjectName")); //'ColumnName1' represents name of datafield in grid
                dt.Columns.Add(new DataColumn("ContractorName"));
                dt.Columns.Add(new DataColumn("AwardedCost"));
                dt.Columns.Add(new DataColumn("Disbursement_Amount"));
                dt.Columns.Add(new DataColumn("Disbursement_Amount_2021"));
                dt.Columns.Add(new DataColumn("Q1_Budget_Amount"));
                dt.Columns.Add(new DataColumn("Q2_Budget_Amount"));
                dt.Columns.Add(new DataColumn("Q3_Budget_Amount"));
                dt.Columns.Add(new DataColumn("Q4_Budget_Amount"));
                dt.Columns.Add(new DataColumn("Q1_Actual_Amount"));
                dt.Columns.Add(new DataColumn("Q2_Actual_Amount"));
                dt.Columns.Add(new DataColumn("Q3_Actual_Amount"));
                dt.Columns.Add(new DataColumn("Q4_Actual_Amount"));
                dt.Columns.Add(new DataColumn("TotalDisburseAmnt"));


                // Add empty row first to DataTable to show as first row in gridview


                float AwardedCost = 0.0f;
                float Disbursement_Amount = 0.0f;
                float Disbursement_Amount_2021 = 0.0f;
                float Q1_Budget_Amount = 0.0f;
                float Q2_Budget_Amount = 0.0f;
                float Q3_Budget_Amount = 0.0f;
                float Q4_Budget_Amount = 0.0f;
                float Q1_Actual_Amount = 0.0f;
                float Q2_Actual_Amount = 0.0f;
                float Q3_Actual_Amount = 0.0f;
                float Q4_Actual_Amount = 0.0f;
                float TotalDisburseAmnt = 0.0f;
                // Get each row from gridview and add it to DataTable
                float convertion = float.Parse(WebConfigurationManager.AppSettings["MJPYtoCrores2"]);
                foreach (DataRow gvr in getdata.GetBudgetvsDisbursement(new Guid(ddlmeeting.SelectedValue)).Tables[0].Rows)
                {
                    dr = dt.NewRow();
                    dr["ProjectName"] = gvr["ProjectName"].ToString();
                    dr["ContractorName"] = gvr["ContractorName"].ToString();
                    dr["AwardedCost"] = gvr["AwardedCost"].ToString();
                    dr["Disbursement_Amount"] = gvr["Disbursement_Amount"].ToString();
                    dr["Disbursement_Amount_2021"] = gvr["Disbursement_Amount_2021"].ToString();
                    dr["Q1_Budget_Amount"] = gvr["Q1_Budget_Amount"].ToString();
                    dr["Q2_Budget_Amount"] = gvr["Q2_Budget_Amount"].ToString();
                    dr["Q3_Budget_Amount"] = gvr["Q3_Budget_Amount"].ToString();
                    dr["Q4_Budget_Amount"] = gvr["Q4_Budget_Amount"].ToString();
                    dr["Q1_Actual_Amount"] = gvr["Q1_Actual_Amount"].ToString();
                    dr["Q2_Actual_Amount"] = gvr["Q2_Actual_Amount"].ToString();
                    dr["Q3_Actual_Amount"] = gvr["Q3_Actual_Amount"].ToString();
                    dr["Q4_Actual_Amount"] = gvr["Q4_Actual_Amount"].ToString();
                    dr["TotalDisburseAmnt"] = gvr["TotalDisburseAmnt"].ToString();
                    //
                    AwardedCost += float.Parse(gvr["AwardedCost"].ToString());
                    Disbursement_Amount += float.Parse(gvr["Disbursement_Amount"].ToString());
                    Disbursement_Amount_2021 += float.Parse(gvr["Disbursement_Amount_2021"].ToString());
                    Q1_Budget_Amount += float.Parse(gvr["Q1_Budget_Amount"].ToString());
                    Q2_Budget_Amount += float.Parse(gvr["Q2_Budget_Amount"].ToString());
                    Q3_Budget_Amount += float.Parse(gvr["Q3_Budget_Amount"].ToString());
                    Q4_Budget_Amount += float.Parse(gvr["Q4_Budget_Amount"].ToString());
                    Q1_Actual_Amount += float.Parse(gvr["Q1_Actual_Amount"].ToString());
                    Q2_Actual_Amount += float.Parse(gvr["Q2_Actual_Amount"].ToString());
                    Q3_Actual_Amount += float.Parse(gvr["Q3_Actual_Amount"].ToString());
                    Q4_Actual_Amount += float.Parse(gvr["Q4_Actual_Amount"].ToString());
                    TotalDisburseAmnt += float.Parse(gvr["TotalDisburseAmnt"].ToString());
                    dt.Rows.Add(dr);
                }
                
                    dr = dt.NewRow();
                    dr["ProjectName"] = "";
                    dr["ContractorName"] = "";
                dr["AwardedCost"] = AwardedCost.ToString("n2");
                dr["Disbursement_Amount"] = Disbursement_Amount.ToString("n2");
                dr["Disbursement_Amount_2021"] = Disbursement_Amount_2021.ToString("n2");
                dr["Q1_Budget_Amount"] = Q1_Budget_Amount.ToString("n2");
                dr["Q2_Budget_Amount"] = Q2_Budget_Amount.ToString("n2");
                dr["Q3_Budget_Amount"] = Q3_Budget_Amount.ToString("n2");
                dr["Q4_Budget_Amount"] = Q4_Budget_Amount.ToString("n2");
                dr["Q1_Actual_Amount"] = Q1_Actual_Amount.ToString("n2");
                dr["Q2_Actual_Amount"] = Q2_Actual_Amount.ToString("n2");
                dr["Q3_Actual_Amount"] = Q3_Actual_Amount.ToString("n2");
                dr["Q4_Actual_Amount"] = Q4_Actual_Amount.ToString("n2");
                dr["TotalDisburseAmnt"] = TotalDisburseAmnt.ToString("n2");
                dt.Rows.Add(dr);

                // Hard coded for JICA Meeting as convertion was not available....
                dr = dt.NewRow();
                dr["ProjectName"] = "";
                dr["ContractorName"] = "";
                dr["AwardedCost"] = WebConfigurationManager.AppSettings["BudgetVsDisbursement"] + " (Total All Quarters)";
                dr["Disbursement_Amount"] = Disbursement_Amount.ToString("n2");
                dr["Disbursement_Amount_2021"] = Disbursement_Amount_2021.ToString("n2");
                dr["Q1_Budget_Amount"] = ""; //Q1_Budget_Amount.ToString();
                dr["Q2_Budget_Amount"] = "";// Q2_Budget_Amount.ToString();
                dr["Q3_Budget_Amount"] = "";// Q3_Budget_Amount.ToString();
                dr["Q4_Budget_Amount"] = (Q1_Budget_Amount + Q2_Budget_Amount + Q3_Budget_Amount + Q4_Budget_Amount).ToString("n2"); ;
                dr["Q1_Actual_Amount"] = "";// Q1_Actual_Amount.ToString();
                dr["Q2_Actual_Amount"] = "";// Q2_Actual_Amount.ToString();
                dr["Q3_Actual_Amount"] = "";// Q3_Actual_Amount.ToString();
                dr["Q4_Actual_Amount"] = (Q1_Actual_Amount + Q2_Actual_Amount + Q3_Actual_Amount + Q4_Actual_Amount).ToString("n2"); ;
                dr["TotalDisburseAmnt"] = TotalDisburseAmnt.ToString("n2");
                dt.Rows.Add(dr);

                //dr = dt.NewRow();
                //dr["ProjectName"] = "";
                //dr["ContractorName"] = "";
                //dr["AwardedCost"] = "Rs. Crores (Total All Quarters)";
                //dr["Disbursement_Amount"] = "62.38";// "";
                //dr["Q1_Budget_Amount"] = "";// Math.Round(Q1_Budget_Amount * convertion, 2);
                //dr["Q2_Budget_Amount"] = "";// Math.Round(Q2_Budget_Amount * convertion, 2);
                //dr["Q3_Budget_Amount"] = "";// Math.Round(Q3_Budget_Amount * convertion, 2);
                //dr["Q4_Budget_Amount"] = "510.33";// Math.Round(Q4_Budget_Amount * convertion, 2);
                //dr["Q1_Actual_Amount"] = "";// Math.Round(Q1_Actual_Amount * convertion, 2);
                //dr["Q2_Actual_Amount"] = "";// Math.Round(Q2_Actual_Amount * convertion, 2);
                //dr["Q3_Actual_Amount"] = "";// Math.Round(Q3_Actual_Amount * convertion, 2);
                //dr["Q4_Actual_Amount"] = "317.15";// Math.Round(Q4_Actual_Amount * convertion, 2);
                //dr["TotalDisburseAmnt"] = "379.53";// Math.Round(TotalDisburseAmnt * convertion, 2);
                //dt.Rows.Add(dr);


                // Add DataTable back to grid
                GrdBudgetbsDisbursemnt.DataSource = dt;
                GrdBudgetbsDisbursemnt.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void GrdBudgetbsDisbursemnt_DataBound(object sender, EventArgs e)
        {

            DateTime rDate = new DateTime();
            string FY = "";
            if (ReviewMeetingDate.Value != "")
            {
                rDate = Convert.ToDateTime(ReviewMeetingDate.Value);

                int sMonth = rDate.Month;
                int sYear = rDate.Year;

                if (sMonth >= 4)
                {
                    FY = sYear + "-" + (sYear + 1).ToString().Substring(2, 2);
                }
                else
                {
                    FY = (sYear - 1) + "-" + sYear.ToString().Substring(2, 2);
                }
            }

            GridViewRow row = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
            TableHeaderCell cell = new TableHeaderCell();
            cell.Text = "";
            cell.ColumnSpan = 5;
            row.Controls.Add(cell);

            cell = new TableHeaderCell();
            cell.ColumnSpan = 4;
            cell.Text = FY != "" ? "Budget for FY " + FY + " " + WebConfigurationManager.AppSettings["BudgetVsDisbursement"] : "Budget for FY 2020-21 MJPY";
            row.Controls.Add(cell);

            cell = new TableHeaderCell();
            cell.ColumnSpan = 4;
            cell.Text = FY != "" ? "Actual Disbursement for FY " + FY + " " + WebConfigurationManager.AppSettings["BudgetVsDisbursement"] : "Actual Disbursement for FY 2020-21 MJPY";
            row.Controls.Add(cell);

            cell = new TableHeaderCell();
            cell.ColumnSpan = 1;
            cell.Text = "";
            row.Controls.Add(cell);

            //row.BackColor = ColorTranslator.FromHtml("#3AC0F2");
            GrdBudgetbsDisbursemnt.HeaderRow.Parent.Controls.AddAt(0, row);
        }

        protected void GrdBudgetbsDisbursemnt_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                

            }
        }

        protected void ddlmeeting_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlmeeting.SelectedValue != "")
            {
                DataSet ds = getdata.GetMeetingMaster_by_Meeting_UID(new Guid(ddlmeeting.SelectedValue));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ReviewMeetingDate.Value = ds.Tables[0].Rows[0]["CreatedDate"].ToString();
                }
                LoadGridDataBudget();
            }
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            //required to avoid the runtime error "  
            //Control 'GridView1' of type 'GridView' must be placed inside a form tag with runat=server."  
        }

        protected void btnexport_Click(object sender, EventArgs e)
        {
            try
            {
                string sDate1 = "";
                DateTime CDate1 = DateTime.Now;

                sDate1 = CDate1.ToString("dd/MM/yyyy");
                sDate1 = getdata.ConvertDateFormat(sDate1);
                CDate1 = Convert.ToDateTime(sDate1);

               
                //to allow paging=false & change style.
                GrdBudgetbsDisbursemnt.HeaderStyle.ForeColor = System.Drawing.Color.Black;
                GrdBudgetbsDisbursemnt.BorderColor = System.Drawing.Color.Black;
                GrdBudgetbsDisbursemnt.Font.Name = "Arial";
             
                LoadGridDataBudget();

                StringWriter stw = new StringWriter();
                HtmlTextWriter htextw = new HtmlTextWriter(stw);
                htextw.AddStyleAttribute("font-size", "9pt");
                htextw.AddStyleAttribute("color", "Black");

                GrdBudgetbsDisbursemnt.RenderControl(htextw); //Name of the Panel

                //var sb1 = new StringBuilder();
                //GridDiv.RenderControl(new HtmlTextWriter(new StringWriter(sb1)));

                string s = htextw.InnerWriter.ToString();
                string HTMLstring = "<html><head><meta http-equiv='Content-Type' content='text/html; charset=utf-8' /></head> <body>" +
                    "<div style='width:100%; margin:auto;'><div style='width:100%; float:left;' align='center'>";
                HTMLstring += "<h2>" + WebConfigurationManager.AppSettings["Domain"] + "</h2>";
                HTMLstring += "<h3>Report Name: " + heading.InnerHtml  + "</h3>" +
                "</div> <div style='width:100%; float:left;'></div>"+
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
                        // Document pdfDoc = new Document(PageSize.A4, 25f, 25f, 35f, 25f);
                        Document pdfDoc = new Document(PageSize.A4.Rotate(), 5, 5, 0, 0);

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
                        htmlparser.Parse(sr);
                        pdfDoc.Close();
                        Response.ContentType = "application/pdf";
                        Response.AddHeader("content-disposition", "attachment;filename=Budget_Disbursement_" + DateTime.Now.Ticks + ".pdf");
                        Response.Cache.SetCacheability(HttpCacheability.NoCache);
                        Response.Write(pdfDoc);
                        Response.End();

                    }


                }
            }
            catch(Exception ex)
                {
                throw ex;
            }
        }

        private void ExporttoPDF()
        {
            try
            {

            }
            catch(Exception ex)
            {

            }
        }
    }
}