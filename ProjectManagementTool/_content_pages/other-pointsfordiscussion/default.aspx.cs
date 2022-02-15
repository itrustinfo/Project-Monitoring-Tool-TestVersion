
using System;
using System.Web;
using System.Web.UI;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System.Text;
using System.Web.Configuration;
using System.IO;
using ProjectManager.DAL;
using System.Data;
using System.Web.UI.WebControls;

namespace ProjectManagementTool._content_pages.other_pointsfordiscussion
{
    public partial class _default : System.Web.UI.Page
    {
        DBGetData dbg = new DBGetData();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                BindMeetingMaster();
                GetOtherPointsDiscussion();
            }
        }

       
        private void BindMeetingMaster()
        {
            DataSet ds = dbg.GetMeetingMasters();
            DDLMeetingMaster.DataTextField = "Meeting_Description";
            DDLMeetingMaster.DataValueField = "Meeting_UID";
            DDLMeetingMaster.DataSource = ds;
            DDLMeetingMaster.DataBind();
        }
        private void GetOtherPointsDiscussion()
        {
            try
            {
                DataTable dtotherpoints = dbg.GetOtherPoints_Discussion(new Guid(DDLMeetingMaster.SelectedValue));
                grdOtherPoins.DataSource = dtotherpoints;
                grdOtherPoins.DataBind();
                if (dtotherpoints.Rows.Count > 0)
                {
                    btnexport.Visible = true;
                }
                else
                {
                    btnexport.Visible = false;
                }
            }
            catch (Exception ex)
            {

            }
        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            //required to avoid the runtime error "  
            //Control 'GridView1' of type 'GridView' must be placed inside a form tag with runat=server."  
        }

        protected void DDLMeetingMaster_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetOtherPointsDiscussion();
           
        }

        protected void btnexport_Click(object sender, EventArgs e)
        {
            try
            {
                grdOtherPoins.HeaderStyle.ForeColor = System.Drawing.Color.Black;
                grdOtherPoins.BorderColor = System.Drawing.Color.Black;
                grdOtherPoins.Font.Name = "Arial";
                grdOtherPoins.DataSource = dbg.GetOtherPoints_Discussion(new Guid(DDLMeetingMaster.SelectedValue));
                grdOtherPoins.AllowPaging = true;
                //GrdProjectProgress.HorizontalAlign = 0;
                //GrdDocumentMaster.Columns[0].Visible = false; //export won't work if there's a link in the gridview
                grdOtherPoins.DataBind();

                StringWriter stw = new StringWriter();
                HtmlTextWriter htextw = new HtmlTextWriter(stw);
                htextw.AddStyleAttribute("font-size", "9pt");
                htextw.AddStyleAttribute("color", "Black");

                grdOtherPoins.RenderControl(htextw); //Name of the Panel

                //var sb1 = new StringBuilder();
                //GridDiv.RenderControl(new HtmlTextWriter(new StringWriter(sb1)));

                string s = htextw.InnerWriter.ToString();
                string HTMLstring = "<html><head><meta http-equiv='Content-Type' content='text/html; charset=utf-8' /></head> <body>" +
                    "<div style='width:100%; margin:auto;'><div style='width:100%; float:left;' align='center'>";
                HTMLstring += "<h2>" + WebConfigurationManager.AppSettings["Domain"] + "</h2>";
                HTMLstring += "<h3>Other points for discussion</h3><br/>" +
                "</div>";
                HTMLstring += "<div style='width:100%; float:left;'><h4>Other points for discussion "+DDLMeetingMaster.SelectedItem.ToString()+" :</h4><br/></div>";
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
                        htmlparser.Parse(sr);
                        pdfDoc.Close();
                        Response.ContentType = "application/pdf";
                        Response.AddHeader("content-disposition", "attachment;filename=Other_Points_for_Discussion_" + DateTime.Now.Ticks + ".pdf");
                        Response.Cache.SetCacheability(HttpCacheability.NoCache);
                        Response.Write(pdfDoc);
                        Response.End();

                    }
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code: 201-A There is a problem with this feature. Please contact system admin.');</script>");
            }
        }

        protected void grdOtherPoins_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Text = (e.Row.RowIndex + 1).ToString();
            }
        }
    }
}