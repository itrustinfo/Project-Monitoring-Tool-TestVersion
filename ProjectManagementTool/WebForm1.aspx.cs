using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iTextSharp.text.pdf.draw;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using iTextSharp.text.xml;
using System.Text;
using System.Data;
using System.Threading;
using ProjectManager.DAL;
using System.Web.UI.HtmlControls;

namespace ProjectManagementTool
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        //int maxId = 41;
        //static int lastIndex;
        //protected void Page_PreInit(object sender, EventArgs e)
        //{
        //    var tools = new List<string> {
        //        "TextBox",
        //        "DropDownList",
        //        "RadioButton",
        //        "Button",
        //        "Panel"
        //    };
        //    tools.Sort();

        //    List<string> keys = Request.Form.AllKeys.Where(key => key.Contains("txtDynamic")).ToList();
        //    int i = 1;
        //    foreach (string key in keys)
        //    {
        //        this.CreateLabel("lblDynamic" + i, maxId + i);
        //        this.CreateTextBox("txtDynamic" + i);
        //        this.CreateLabelTools("Tools : ", i.ToString());
        //        this.CreateToolsDdl("ddlToolsDynamic" + i, tools);
        //        this.CreateGroupIdLabel("Group ID Value : ", i.ToString());
        //        this.CreateGroupIdTextBox("txtGroupIdDynamic" + i);
        //        this.CreateParentQnIdLabel("Parent Qn Id : ", i.ToString());
        //        this.CreateParentQnIdDdl("ddlParentQnIdDynamic" + i, CreateDDLItem(i));
        //        i++;
        //    }
        //    lastIndex = i - 1;
        //}
        DBGetData getdt = new DBGetData();
        TaskUpdate tkupdat = new TaskUpdate();
        //ITextEvents Common = new ITextEvents();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //string Path = @"D:\Arun\SampleDoc_1.docx";
                //string outpath = Server.MapPath("~/"+ "SampleDoc_1_new.docx");
                //getdt.DecryptFile(Path, outpath);

                string ProjectName = "CP-09";

                int Diff = 100 - ProjectName.Length;
                //if (ProjectName.Length > 30)
                //{
                //    ProjectName = ProjectName.Substring(0, 29) + "..";
                //}

                if (Diff > 0)
                {
                    ProjectName = ProjectName + string.Concat(Enumerable.Repeat("&nbsp;", Diff));
                }
                LblProjectname.Text = ProjectName + "_Arun";
            }
        }
        protected void btnexporttoPDF_Click(object sender, EventArgs e)
        {
            //ExporttoPDF();
            NewExport();
        }

        

        public static string ConvertDataTableToHTML(DataTable dt)
        {
            string html = "<table>";
            //add header row
            html += "<tr>";
            for (int i = 0; i < dt.Columns.Count; i++)
                html += "<td>" + dt.Columns[i].ColumnName + "</td>";
            html += "</tr>";
            //add rows
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                html += "<tr>";
                for (int j = 0; j < dt.Columns.Count; j++)
                    html += "<td>" + dt.Rows[i][j].ToString() + "</td>";
                html += "</tr>";
            }
            html += "</table>";
            return html;
        }

        
        public void NewExport()
        {
            string fileName = string.Empty;
            DateTime fileCreationDatetime = DateTime.Now;
            fileName = string.Format("{0}.pdf", fileCreationDatetime.ToString(@"yyyyMMdd") + "_" + fileCreationDatetime.ToString(@"HHmmss"));
            string pdfPath = Server.MapPath(@"~\Documents\") + fileName;

            using (FileStream msReport = new FileStream(pdfPath, FileMode.Create))
            {
                //step 1
                Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 140f, 10f);
                
                    try
                    {
                        // step 2
                        PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, msReport);
                        pdfWriter.PageEvent = new ITextEvents();

                        //open the stream 
                        pdfDoc.Open();

                        for (int i = 0; i < 10; i++)
                        {
                            Paragraph para = new Paragraph("Hello world. Checking Header Footer", new Font(Font.HELVETICA, 22));
                            para.Alignment = Element.ALIGN_CENTER;
                            pdfDoc.Add(para);
                            pdfDoc.NewPage();
                        }

                        pdfDoc.Close();
                    }
                    catch (Exception ex)
                    {
                        //handle exception
                    }
                    finally
                    {
                    }
                
            }
        }
        public void ExporttoPDF()
        {
            string HTMLstring = "<html><body><div style='width:100%; margin:auto;'><div style='width:100%; float:left; line-height:25px;' align='center'><h2>NJSEI</h2><h3>Report Name: Project Status of Work Progress</h3><h3>Contract Package : Workpackage 1</h3><h4>Month : Jan 2021</h4></div> <div style='width:100%; float:left;'><br/><br/><br/><br/></div><div style='float:left; width:100%; font-size:9pt;'>"+
                "<table border='1' cellpadding='4' cellspacing='4' style='width:100%;'><tr><td width='10%'>1</td><td style='font-weight:bold;'>Name of the Contractor</td><td>Test Contractor1</td></tr><tr><td width='10%'>2</td><td width='45%' style='font-weight:bold;'>Total Cost</td><td>₹ 2,50,00,00,000</td></tr><tr><td width='10%'>3</td><td width='45%' style='font-weight:bold;'>Date of issue of LOA</td><td>25/10/2020</td></tr><tr><td width='10%'>4</td><td width='45%' style='font-weight:bold;'>Date of signing of Agreement</td><td>01/08/2020</td></tr><tr><td width='10%'>5</td><td width='45%' style='font-weight:bold;'>Date of Commencement</td><td>01/11/2020</td></tr><tr><td width='10%'>6</td><td style='font-weight:bold;' width='45%'>Date of Completion</td><td>31/12/2020</td></tr><tr><td width='10%'>7</td><td style='font-weight:bold;' width='45%'>Period of Completion</td><td>36 Months</td></tr></table></div<div style='width:100%; float:left;'><br/><br/><br/><br/></div><div style='page-break-after:always;'>Content before page breaks</div><div style='width:100%; float:left;'><h4>Project Status of Work Progress :</h4><br/><br/></div><div style='width:100%; float:left;'><div style='font - size:9pt; color: Black;'>" +
            "<table class='table table-bordered' cellspacing='0' rules='all' border='1' id='default_master_body_GrdProjectProgress' style='border-color:Black;font-family:Tahoma;width:100%;border-collapse:collapse;'>"+
		    "<tr style = 'color:Black;' >"+
            "<th scope='col'>Sl.No.</th><th scope = 'col' > Activities </ th >< th scope='col'>Target as on 31/01/2021 Submitted Construction Programme</th><th scope = 'col' > Achieved as on 31/01/2021</th><th scope = 'col' >% Percentage </ th >"+
                    "</tr ><tr>"+
                        "<td align='left'> 1 </td><td>MS Bare Pipe Supply</td><td>15500</td><td>13200</td><td>85%</td></tr>"+
                        "<tr><td align = 'left' >2</ td >< td >MS Coated Pipes Supply</td><td>13000</td><td>10550</td><td>81%</td></tr>" +
                        "</table>"+
            "</div></div></div></body></html>";
            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                {
                    StringBuilder sb = new StringBuilder();

                    //Export HTML String as PDF.
                    StringReader sr = new StringReader(HTMLstring);
                    Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 15f, 15f);

                    iTextSharp.text.Font foot = new iTextSharp.text.Font();
                    foot.Size = 8;
                    HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                    pdfDoc.Footer = new HeaderFooter(new Phrase("Date : " + DateTime.Now.ToString("dd/MM/yyyy") + "                      Page: ", foot), true);
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

        protected void btnSubmit1_Click(object sender, EventArgs e)
        {
            LblDateTime1.Text = DateTime.Now.ToLongDateString();
        }

        protected void btnSubmit2_Click(object sender, EventArgs e)
        {
            LblTime.Text = DateTime.Now.ToShortTimeString();
        }
        //protected void AddTextBox(object sender, EventArgs e)
        //{
        //    var tools = new List<string> {
        //        "TextBox",
        //        "DropDownList",
        //        "RadioButton",
        //        "Button",
        //        "Panel"
        //    };
        //    tools.Sort();
        //    int index = (txtPanel.Controls.OfType<TextBox>().ToList().Count / 2) + 1;
        //    int id = maxId + index;
        //    this.CreateLabel("lblDynamic" + index, id);
        //    this.CreateTextBox("txtDynamic" + index);
        //    this.CreateLabelTools("Tools : ", index.ToString());
        //    this.CreateToolsDdl("ddlToolsDynamic" + index, tools);
        //    this.CreateGroupIdLabel("Group ID Value : ", index.ToString());
        //    this.CreateGroupIdTextBox("txtGroupIdDynamic" + index);
        //    this.CreateParentQnIdLabel("Parent Qn Id : ", index.ToString());
        //    this.CreateParentQnIdDdl("ddlParentQnIdDynamic" + index, CreateDDLItem(index));
        //}

        //private Array CreateDDLItem(int index)
        //{
        //    int it = maxId;
        //    List<int> item = new List<int>();
        //    for (int i = 0; i < index; i++)
        //    {
        //        item.Add(it);
        //        it++;
        //    }

        //    return item.ToArray();
        //}

        //private void CreateTextBox(string id)
        //{
        //    TextBox txt = new TextBox();
        //    txt.ID = id;
        //    txtPanel.Controls.Add(txt);
        //}

        //private void CreateLabel(string id, int qnId)
        //{
        //    string value = qnId.ToString();
        //    Label lbl = new Label();
        //    lbl.ID = id;
        //    lbl.Text = value;
        //    txtPanel.Controls.Add(lbl);
        //}

        //private void CreateLabelTools(string text, string id)
        //{
        //    Label lbl = new Label();
        //    lbl.Text = text;
        //    lbl.ID = "lblTools" + id;
        //    txtPanel.Controls.Add(lbl);
        //}

        //private void CreateToolsDdl(string id, List<string> data)
        //{
        //    DropDownList ddl = new DropDownList();
        //    ddl.ID = id;
        //    ddl.DataSource = data;
        //    ddl.DataBind();
        //    txtPanel.Controls.Add(ddl);
        //}

        //private void CreateGroupIdLabel(string text, string id)
        //{
        //    Label lbl = new Label();
        //    lbl.Text = text;
        //    lbl.ID = "lblGroup" + id;
        //    txtPanel.Controls.Add(lbl);
        //}

        //private void CreateGroupIdTextBox(string id)
        //{
        //    TextBox txt = new TextBox();
        //    txt.ID = id;
        //    txtPanel.Controls.Add(txt);
        //}

        //private void CreateParentQnIdLabel(string text, string id)
        //{
        //    Label lbl = new Label();
        //    lbl.Text = text;
        //    lbl.ID = "lblParentQn" + id;
        //    txtPanel.Controls.Add(lbl);
        //}

        //private void CreateParentQnIdDdl(string id, Array data)
        //{
        //    DropDownList ddl = new DropDownList();
        //    ddl.ID = id;
        //    ddl.DataSource = data;
        //    ddl.DataBind();
        //    txtPanel.Controls.Add(ddl);
        //    Literal lt = new Literal();
        //    lt.Text = "<br />";
        //    txtPanel.Controls.Add(lt);
        //}

        // Remove

        //protected void RemoveTextBox(object sender, EventArgs e)
        //{
        //    int lastId = lastIndex;
        //    this.RemoveLabel("lblDynamic" + (lastId));
        //    this.RemoveTextBox("txtDynamic" + (lastId));
        //    this.RemoveLabelTools("lblTools" + lastId);
        //    this.RemoveToolsDdl("ddlToolsDynamic" + (lastId));
        //    this.RemoveGroupIdLabel("lblGroup" + lastId);
        //    this.RemoveGroupIdTextBox("txtGroupIdDynamic" + (lastId));
        //    this.RemoveParentQnIdLabel("lblParentQn" + lastId);
        //    this.RemoveParentQnIdDdl("ddlParentQnIdDynamic" + (lastId));
        //}

        //private void RemoveTextBox(string id)
        //{
        //    RemoveControl(id);
        //}

        //private void RemoveLabel(string id)
        //{
        //    RemoveControl(id);
        //}

        //private void RemoveLabelTools(string id)
        //{
        //    RemoveControl(id);
        //}

        //private void RemoveToolsDdl(string id)
        //{
        //    RemoveControl(id);
        //}

        //private void RemoveGroupIdLabel(string id)
        //{
        //    RemoveControl(id);
        //}

        //private void RemoveGroupIdTextBox(string id)
        //{
        //    RemoveControl(id);
        //}

        //private void RemoveParentQnIdLabel(string id)
        //{
        //    RemoveControl(id);
        //}

        //private void RemoveParentQnIdDdl(string id)
        //{
        //    RemoveControl(id);

        //}

        //public void RemoveControl(string id)
        //{
        //    Control cntrl = txtPanel.FindControl(id) as Control;
        //    if (txtPanel.Controls.Contains(cntrl))
        //    {
        //        txtPanel.Controls.Remove(cntrl);
        //    }
        //}









        //    protected void btnProgress_Click(object sender, EventArgs e)
        //    {
        //        Thread.Sleep(5000);
        //    }

        //    protected void btnexport_Click(object sender, EventArgs e)
        //    {
        //        DataTable dt = new DataTable();
        //        dt.Columns.AddRange(new DataColumn[3] {
        //new DataColumn("Id"),
        //new DataColumn("Name"),
        //new DataColumn("Country")});
        //        dt.Rows.Add(1, "John Hammond", "United States");
        //        dt.Rows.Add(2, "Mudassar Khan", "India");
        //        dt.Rows.Add(3, "Suzanne Mathews", "France");
        //        dt.Rows.Add(4, "Robert Schidner", "Russia");

        //        StringBuilder sb = new StringBuilder();
        //        sb.Append("<table width='100%' cellspacing='0' cellpadding='2'>");
        //        sb.Append("<tr><td align='center' style='background-color: #18B5F0' colspan = '2'><b>Order Sheet</b></td></tr>");
        //        sb.Append("<tr><td colspan = '2'></td></tr>");
        //        sb.Append("<tr><td><b>Order No:</b> 100</td><td><b>Date: </b>" + DateTime.Now + " </td></tr>");
        //        sb.Append("<tr><td><b>From :</b> " + "Company Name" + " </td><td><b>To: </b>" + " Some Company " + " </td></tr>");
        //        sb.Append("</table>");

        //        sb.Append("<table border = '1'>");
        //        sb.Append("<tr>");
        //        foreach (DataColumn column in dt.Columns)
        //        {
        //            sb.Append("<th style = 'background-color: #D20B0C;color:#ffffff'>");
        //            sb.Append(column.ColumnName);
        //            sb.Append("</th>");
        //        }
        //        sb.Append("</tr>");
        //        foreach (DataRow row in dt.Rows)
        //        {
        //            sb.Append("<tr>");
        //            foreach (DataColumn column in dt.Columns)
        //            {
        //                sb.Append("<td>");
        //                sb.Append(row[column]);
        //                sb.Append("</td>");
        //            }
        //            sb.Append("</tr>");
        //        }
        //        sb.Append("</table>");
        //        Response.Clear();
        //        Response.Buffer = true;
        //        Response.AddHeader("content-disposition", "attachment;filename=ReceiptExport.xls");
        //        Response.Charset = "";
        //        Response.ContentType = "application/vnd.ms-excel";
        //        string style = @"<style> .textmode { } </style>";
        //        Response.Write(style);
        //        Response.Output.Write(sb.ToString());
        //        Response.Flush();
        //        Response.End();
        //    }
    }
}