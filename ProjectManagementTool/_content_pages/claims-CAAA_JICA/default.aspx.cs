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

namespace ProjectManagementTool._content_pages.claims_CAAA_JICA
{
    public partial class _default : System.Web.UI.Page
    {
        DBGetData getdt = new DBGetData();
        TaskUpdate gettk = new TaskUpdate();
        float ToatAmount = 0;
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
                    ddlmeeting.DataSource = getdt.GetMeetingMaster();
                    ddlmeeting.DataTextField = "Meeting_Description";
                    ddlmeeting.DataValueField = "Meeting_UID";
                    ddlmeeting.DataBind();
                    BindReports();
                    //DDlProject_SelectedIndexChanged(sender, e);
                    //YearBind();
                    //ProjectProgress.Visible = false;
                }
            }
        }

        private void BindReports()
        {
            DataTable ds = new DataTable();
            ds = getdt.GetcontractPackage_Meeting_Details(new Guid(ddlmeeting.SelectedValue)); 
            GrdProjectProgress.DataSource = ds;
            GrdProjectProgress.DataBind();
        }

        protected void GrdProjectProgress_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Text = (e.Row.RowIndex + 1).ToString();

                if (e.Row.Cells[2].Text != "")
                {
                    ToatAmount += float.Parse(e.Row.Cells[2].Text);
                }
                //Label lblAmt = (Label)e.Row.FindControl("LblAmount");
                //if (lblAmt.Text != "")
                //{
                //    ToatAmount += float.Parse(lblAmt.Text);
                //}
            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[1].Text = "Total";
                e.Row.Cells[1].Font.Bold = true;
                e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[2].Text= ToatAmount.ToString("N2");
                e.Row.Cells[2].Font.Bold = true;
                //Label lblTot = (Label)e.Row.FindControl("lblSummary");
                //lblTot.Text = ToatAmount.ToString("N2");
            }
        }

        protected void btnExportPDF_Click(object sender, EventArgs e)
        {
            try
            {

                //ExporttoPDF();
                GrdProjectProgress.HeaderStyle.ForeColor = System.Drawing.Color.Black;
                GrdProjectProgress.BorderColor = System.Drawing.Color.Black;
                //GrdProjectProgress.Font.Name = "Arial";
                GrdProjectProgress.DataSource = getdt.GetcontractPackage_Meeting_Details(new Guid(ddlmeeting.SelectedValue));
                GrdProjectProgress.AllowPaging = true;
                //GrdDocumentMaster.Columns[0].Visible = false; //export won't work if there's a link in the gridview
                GrdProjectProgress.DataBind();

                StringWriter stw = new StringWriter();
                HtmlTextWriter htextw = new HtmlTextWriter(stw);
                htextw.AddStyleAttribute("font-size", "10pt");
                htextw.AddStyleAttribute("color", "Black");

                GrdProjectProgress.RenderControl(htextw); //Name of the Panel

                //var sb1 = new StringBuilder();
                //GridDiv.RenderControl(new HtmlTextWriter(new StringWriter(sb1)));

                string s = htextw.InnerWriter.ToString();
                string HTMLstring = "<html><head><meta http-equiv='Content-Type' content='text/html; charset=utf-8' /></head> <body>" +
                    "<div style='width:100%; margin:auto;'><div style='width:100%; float:left; line-height:25px;' align='center'>";
                HTMLstring += "<h2>" + WebConfigurationManager.AppSettings["Domain"] + "</h2>";
                HTMLstring += "<h3>Claims sent to CAAA/JICA</h3>" +
                "</div> <div style='width:100%; float:left;'><br/><br/></div>";
                HTMLstring += "<div style='width:100%; float:left;'><h4>Claims sent to CAAA/JICA as " + ddlmeeting.SelectedItem.ToString() + " :</h4><br/><br/></div>";
                HTMLstring += "<div style='width:100%; float:left;'>" +
                s +
                "</div>";
                HTMLstring += "<div style='width:100%; float:left;'><br/><br/></div>";
                HTMLstring += "</div></body></html>";


                using (StringWriter sw = new StringWriter())
                {
                    using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                    {
                        StringBuilder sb = new StringBuilder();

                        //Export HTML String as PDF.
                        StringReader sr = new StringReader(HTMLstring);
                        Document pdfDoc = new Document(PageSize.A4, 25f, 25f, 35f, 25f);

                        iTextSharp.text.Font foot = new iTextSharp.text.Font();
                        foot.Size = 10;
                        HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                        PdfWriter writer = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                        pdfDoc.Footer = new HeaderFooter(new Phrase("Date : " + DateTime.Now.ToString("dd/MM/yyyy") + "                      Page: ", foot), true);
                        pdfDoc.Open();
                        htmlparser.Parse(sr);
                        //XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                        pdfDoc.Close();
                        Response.ContentType = "application/pdf";
                        Response.AddHeader("content-disposition", "attachment;filename=Report_ClaimsSent_to_CAAA_JICA" + DateTime.Now.Ticks + ".pdf");
                        Response.Cache.SetCacheability(HttpCacheability.NoCache);
                        Response.Write(pdfDoc);
                        Response.End();

                    }
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code: 101-A There is a problem with this feature. Please contact system admin.');</script>");
            }
        }
        protected void ddlmeeting_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindReports();
           // LoadGridDataBudget();
        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            //required to avoid the runtime error "  
            //Control 'GridView1' of type 'GridView' must be placed inside a form tag with runat=server."  
        }

        private void ExporttoPDF()
        {

            

            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=GridViewExport.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            StringWriter swr = new StringWriter();
            HtmlTextWriter htmlwr = new HtmlTextWriter(swr);
            //htmlwr.AddStyleAttribute("font-size", "9pt");
            //htmlwr.AddStyleAttribute("color", "Black");

            GrdProjectProgress.AllowPaging = false;
            GrdProjectProgress.RenderControl(htmlwr);
            string s = htmlwr.InnerWriter.ToString();

            string HTMLstring = "<html><head><meta http-equiv='Content-Type' content='text/html; charset=utf-8' /></head> <body>" +
                    "<div style='width:100%; margin:auto;'><div style='width:100%; float:left; line-height:25px;' align='center'>";
            HTMLstring += "<h2>" + WebConfigurationManager.AppSettings["Domain"] + "</h2>";
            HTMLstring += "<h3>Claims sent to CAAA/JICA</h3>" +
            "</div> <div style='width:100%; float:left;'><br/><br/></div>";
            HTMLstring += "<div style='width:100%; float:left;'><h4>Claims sent to CAAA/JICA as " + ddlmeeting.SelectedItem.ToString() + " :</h4><br/><br/></div>";
            HTMLstring += "<div style='width:100%; float:left; font-size:10px;'>" +
            s +
            "</div>";
            HTMLstring += "<div style='width:100%; float:left;'><br/><br/></div>";
            HTMLstring += "</div></body></html>";

            StringReader srr = new StringReader(HTMLstring);
            Document pdfdoc = new Document(PageSize.A4, 25f, 25f, 35f, 25f);
            HTMLWorker htmlparser = new HTMLWorker(pdfdoc);
            PdfWriter.GetInstance(pdfdoc, Response.OutputStream);

            pdfdoc.Open();
            htmlparser.Parse(srr);
            pdfdoc.Close();


            Response.Write(pdfdoc);
            Response.End();
        }
    }
}