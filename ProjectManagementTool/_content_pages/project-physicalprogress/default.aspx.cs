using ProjectManager.DAL;
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

namespace ProjectManagementTool._content_pages.project_physicalprogress
{
    public partial class _default : System.Web.UI.Page
    {
        DBGetData getdt = new DBGetData();
        TaskUpdate gettk = new TaskUpdate();
        float AwardedCost = 0;
        float ExpenditureCost = 0;
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
                    BindMeetingMaster();
                    if (Session["SelectedMeeting"] != null)
                    {
                        DDLMeetingMaster.SelectedValue = Session["SelectedMeeting"].ToString();
                        Session["SelectedMeeting"] = null;
                    }
                    DDLMeetingMaster_SelectedIndexChanged(sender, e);
                }
            }
        }

        private void BindPhysicalProgress(string MeetingUID)
        {
            DataSet ds = new DataSet();
            ds = getdt.GetProjectPhysicalProgress_by_Meeting_UID(new Guid(MeetingUID));
            GrdPhysicalProgress.DataSource = ds;
            GrdPhysicalProgress.DataBind();
            if (ds.Tables[0].Rows.Count > 0)
            {
                btnexport.Visible = false;
            }
            else
            {
                btnexport.Visible = false;
            }
        }
        public string GetProjectName(string ProjectUID)
        {
            return getdt.getProjectNameby_ProjectUID(new Guid(ProjectUID));
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            //required to avoid the runtime error "  
            //Control 'GridView1' of type 'GridView' must be placed inside a form tag with runat=server."  
        }

        private void BindMeetingMaster()
        {
            DataSet ds = getdt.GetMeetingMasters();
            DDLMeetingMaster.DataTextField = "Meeting_Description";
            DDLMeetingMaster.DataValueField = "Meeting_UID";
            DDLMeetingMaster.DataSource = ds;
            DDLMeetingMaster.DataBind();
        }
        protected void btncopy_Click(object sender, EventArgs e)
        {
            try
            {
                int index = DDLMeetingMaster.SelectedIndex - 1;
                Guid sourcemeetingUID = new Guid(DDLMeetingMaster.Items[index].Value);
                int result = getdt.CopyPhysicalProgress(sourcemeetingUID, new Guid(DDLMeetingMaster.SelectedValue));
                DDLMeetingMaster_SelectedIndexChanged(sender, e);
            }
            catch (Exception ex)
            {

            }
        }
        protected void btnexport_Click(object sender, EventArgs e)
        {
            string sDate1 = "";
            DateTime CDate1 = DateTime.Now;

            sDate1 = CDate1.ToString("dd/MM/yyyy");
            sDate1 = getdt.ConvertDateFormat(sDate1);
            CDate1 = Convert.ToDateTime(sDate1);

            //to allow paging=false & change style.
            GrdPhysicalProgress.HeaderStyle.ForeColor = System.Drawing.Color.Black;
            GrdPhysicalProgress.BorderColor = System.Drawing.Color.Black;
            GrdPhysicalProgress.Font.Name = "Arial";
            GrdPhysicalProgress.DataSource = getdt.GetAllProjectPhysicalProgress();
            GrdPhysicalProgress.AllowPaging = true;
            //GrdDocumentMaster.Columns[0].Visible = false; //export won't work if there's a link in the gridview
            GrdPhysicalProgress.DataBind();

            StringWriter stw = new StringWriter();
            HtmlTextWriter htextw = new HtmlTextWriter(stw);
            htextw.AddStyleAttribute("font-size", "9pt");
            htextw.AddStyleAttribute("color", "Black");

            GrdPhysicalProgress.RenderControl(htextw); //Name of the Panel

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

        protected void DDLMeetingMaster_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDLMeetingMaster.SelectedIndex > 0)
            {
                btncopy.Visible = true;
            }
            else
            {
                btncopy.Visible = false;
            }
            if (DDLMeetingMaster.SelectedValue != "")
            {
                BindPhysicalProgress(DDLMeetingMaster.SelectedValue);
            }
        }

        protected void GrdPhysicalProgress_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblAwarded = (Label)e.Row.FindControl("LblAward");
                if (lblAwarded.Text != "")
                {
                    AwardedCost += float.Parse(lblAwarded.Text);
                }
                Label lblExpenditure = (Label)e.Row.FindControl("LblExpenditure");
                if (lblExpenditure.Text != "")
                {
                    ExpenditureCost += float.Parse(lblExpenditure.Text);
                }
            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[3].Text = AwardedCost.ToString("N2");
                e.Row.Cells[5].Text = ExpenditureCost.ToString("N2");
            }
        }
    }
}