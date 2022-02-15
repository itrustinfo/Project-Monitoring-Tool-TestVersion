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
using System.IO;
using System.Web.Configuration;

namespace ProjectManagementTool._content_pages.complaints_mom
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
                    BindMeetings();                    
                    ddlmeeting_SelectedIndexChanged(sender, e);
                }
            }
        }

        private void BindMeetings()
        {
            ddlmeeting.DataSource = getdt.GetMeetingMaster();
            ddlmeeting.DataTextField = "Meeting_Description";
            ddlmeeting.DataValueField = "Meeting_UID";
            ddlmeeting.DataBind();
        }
        private void BindComplianceofMOM()
        {
            DataSet ds = getdt.GetComplianceofMOM_by_Meeting_UID(new Guid(ddlmeeting.SelectedValue));
            GrdCompliance.DataSource = ds;
            GrdCompliance.DataBind();

            DataSet dsheadimg = getdt.GetMeetingMaster();
            foreach (DataRow drh in dsheadimg.Tables[0].Rows)
            {
                if (drh["Meeting_UID"].ToString() == ddlmeeting.SelectedValue)
                {
                    LblHeading.Text = "Compliance of Minutes of Last JICA review meeting on " + Convert.ToDateTime(drh["CreatedDate"].ToString()).ToString("dd.MM.yyyy");
                }
            }
        }

        protected void ddlmeeting_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlmeeting.SelectedValue != "")
            {
                BindComplianceofMOM();
            }
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            //required to avoid the runtime error "  
            //Control 'GridView1' of type 'GridView' must be placed inside a form tag with runat=server."  
        }

        protected void btnExportPDF_Click(object sender, EventArgs e)
        {
            try
            {
                GrdCompliance.HeaderStyle.ForeColor = System.Drawing.Color.Black;
                GrdCompliance.BorderColor = System.Drawing.Color.Black;
                //GrdProjectProgress.Font.Name = "Arial";
                GrdCompliance.DataSource = getdt.GetComplianceofMOM_by_Meeting_UID(new Guid(ddlmeeting.SelectedValue));
                GrdCompliance.AllowPaging = true;
                //GrdProjectProgress.HorizontalAlign = 0;
                //GrdDocumentMaster.Columns[0].Visible = false; //export won't work if there's a link in the gridview
                GrdCompliance.DataBind();

                StringWriter stw = new StringWriter();
                HtmlTextWriter htextw = new HtmlTextWriter(stw);
                htextw.AddStyleAttribute("font-size", "10pt");
                htextw.AddStyleAttribute("color", "Black");

                GrdCompliance.RenderControl(htextw); //Name of the Panel

                //var sb1 = new StringBuilder();
                //GridDiv.RenderControl(new HtmlTextWriter(new StringWriter(sb1)));

                string s = htextw.InnerWriter.ToString();
                string HTMLstring = "<html><head><meta http-equiv='Content-Type' content='text/html; charset=utf-8' /></head> <body>" +
                    "<div style='width:100%; margin:auto;'><div style='width:100%; float:left; line-height:25px;' align='center'>";
                HTMLstring += "<h2>" + WebConfigurationManager.AppSettings["Domain"] + "</h2>";
                HTMLstring += "<h3>"+ LblHeading.Text + "</h3>" +
                "</div> <div style='width:100%; float:left;'><br/><br/></div>";
                HTMLstring += "<div style='width:100%; float:left;'>" +
                s +
                "</div>";
                HTMLstring += "<div style='width:100%; float:left;'><br/></div>";
                HTMLstring += "</div></body></html>";


                using (StringWriter sw = new StringWriter())
                {
                    using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                    {
                        StringBuilder sb = new StringBuilder();

                        //Export HTML String as PDF.
                        StringReader sr = new StringReader(HTMLstring);
                        Document pdfDoc = new Document(PageSize.A4.Rotate(), 15f, 15f, 10f, 0);

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
                        Response.AddHeader("content-disposition", "attachment;filename=Report_Compliance_of_MOM_" + DateTime.Now.Ticks + ".pdf");
                        Response.Cache.SetCacheability(HttpCacheability.NoCache);
                        Response.Write(pdfDoc);
                        Response.End();

                    }
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code: 301-A There is a problem with this feature. Please contact system admin.');</script>");
            }
        }
    }
}