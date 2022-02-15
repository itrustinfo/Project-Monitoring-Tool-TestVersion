
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using ProjectManager.DAL;
using System.IO;
using System.Web.Configuration;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;

namespace ProjectManagementTool._content_pages.other_points_discussion_insert
{
 
    public partial class _default : System.Web.UI.Page
    {
        DBGetData dbg = new DBGetData();
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!Page.IsPostBack)
            {
               
                BindMeetingMaster();
                if (Session["SelectedMeeting"] != null)
                {
                    DDLMeetingMaster.SelectedValue = Session["SelectedMeeting"].ToString();
                    Session["SelectedMeeting"] = null;
                }
                GetOtherPointsDiscussion();
            }
        }
        protected void btncopy_Click(object sender, EventArgs e)
        {
            try
            {
                int index = DDLMeetingMaster.SelectedIndex - 1;
                Guid sourcemeetingUID = new Guid(DDLMeetingMaster.Items[index].Value);
                int result = dbg.CopyOtherPointsDiscussion(sourcemeetingUID, new Guid(DDLMeetingMaster.SelectedValue));
                DDLMeetingMaster_SelectedIndexChanged(sender, e);
            }
            catch (Exception ex)
            {

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
               
            }
            catch(Exception ex)
            {

            }
        }

        protected void DDLMeetingMaster_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetOtherPointsDiscussion();
            if (DDLMeetingMaster.SelectedIndex == 0)
            {
                btncopy.Visible = false;
            }
            else
            {
                btncopy.Visible = true;
            }
        }
    
        protected void btnexport_Click(object sender, EventArgs e)
        {
            string sDate1 = "";
            DateTime CDate1 = DateTime.Now;

            sDate1 = CDate1.ToString("dd/MM/yyyy");
            sDate1 = dbg.ConvertDateFormat(sDate1);
            CDate1 = Convert.ToDateTime(sDate1);

            //to allow paging=false & change style.
            grdOtherPoins.HeaderStyle.ForeColor = System.Drawing.Color.Black;
            grdOtherPoins.BorderColor = System.Drawing.Color.Black;
            grdOtherPoins.Font.Name = "Arial";
            grdOtherPoins.DataSource = dbg.GetOtherPoints_Discussion(new Guid(DDLMeetingMaster.SelectedValue));
            grdOtherPoins.AllowPaging = true;
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
                "<div style='width:100%; margin:auto;'><div style='width:100%; float:left; line-height:25px;' align='center'>";
            HTMLstring += "<h2>" + WebConfigurationManager.AppSettings["Domain"] + "</h2>";
            HTMLstring += "<h3>Report Name: Physical Progress Achieved</h3>" +
            "</div> <div style='width:100%; float:left;'><br/><br/></div>";
            HTMLstring += "<div style='width:100%; float:left;'><h4>Physical Progress achieved upto " + CDate1.ToString("MMM yyyy") + "</h4><br/><br/></div>";
            HTMLstring += "<div style='width:100%; float:left;'>" +
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
                    Document pdfDoc = new Document(PageSize.A4, 25f, 25f, 35f, 25f);

                    iTextSharp.text.Font foot = new iTextSharp.text.Font();
                    foot.Size = 10;
                    HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                    pdfDoc.Footer = new HeaderFooter(new Phrase("Date : " + DateTime.Now.ToString("dd/MM/yyyy") + "                      Page: ", foot), true);
                    pdfDoc.Open();
                    htmlparser.Parse(sr);
                    pdfDoc.Close();
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "attachment;filename=Report_PhysicalProgress_" + DateTime.Now.Ticks + ".pdf");
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.Write(pdfDoc);
                    Response.End();

                }


            }
        }

        protected void grdOtherPoins_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            string UID = e.CommandArgument.ToString();
            if (e.CommandName == "delete")
            {
                int cnt = dbg.OtherPoints_Discussion_Delete(new Guid(UID));
                if (cnt > 0)
                {
                    GetOtherPointsDiscussion();
                }
            }
        }

        protected void grdOtherPoins_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            
        }
    }
}