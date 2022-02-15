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

namespace ProjectManagementTool._content_pages.Rpt_status_wastewater
{
    public partial class _default : System.Web.UI.Page
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
                foreach (DataRow drh in dsheadimg.Tables[0].Rows)
                {
                    if (drh["Meeting_UID"].ToString() == ddlmeeting.SelectedValue)
                    {
                        heading.InnerHtml = "Status of Waste Water Contract Packages as on " + Convert.ToDateTime(drh["CreatedDate"].ToString()).ToString("dd.MM.yyyy");
                    }
                }
                GrdStatus.DataSource = getdata.GetStatusWasteWater(new Guid(ddlmeeting.SelectedValue));
                GrdStatus.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
                GrdStatus.HeaderStyle.ForeColor = System.Drawing.Color.Black;
                GrdStatus.BorderColor = System.Drawing.Color.Black;
                GrdStatus.Font.Name = "Arial";

                LoadGridDataBudget();

                StringWriter stw = new StringWriter();
                HtmlTextWriter htextw = new HtmlTextWriter(stw);
                htextw.AddStyleAttribute("font-size", "9pt");
                htextw.AddStyleAttribute("color", "Black");

                GrdStatus.RenderControl(htextw); //Name of the Panel

                //var sb1 = new StringBuilder();
                //GridDiv.RenderControl(new HtmlTextWriter(new StringWriter(sb1)));

                string s = htextw.InnerWriter.ToString();
                string HTMLstring = "<html><head><meta http-equiv='Content-Type' content='text/html; charset=utf-8' /></head> <body>" +
                    "<div style='width:100%; margin:auto;'><div style='width:100%; float:left;' align='center'>";
                HTMLstring += "<h2>" + WebConfigurationManager.AppSettings["Domain"] + "</h2>";
                HTMLstring += "<h3>Report Name:" + heading.InnerHtml + "</h3>" +
                     "</div> <div style='width:100%; float:left;'></div>" +
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
                        Document pdfDoc = new Document(PageSize.A4.Rotate(), 2, 2, 2, 0);

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
                        Response.AddHeader("content-disposition", "attachment;filename=status_wastewater_contractpackage_" + DateTime.Now.Ticks + ".pdf");
                        Response.Cache.SetCacheability(HttpCacheability.NoCache);
                        Response.Write(pdfDoc);
                        Response.End();

                    }


                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void ddlmeeting_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadGridDataBudget();
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            //required to avoid the runtime error "  
            //Control 'GridView1' of type 'GridView' must be placed inside a form tag with runat=server."  
        }


    }
}