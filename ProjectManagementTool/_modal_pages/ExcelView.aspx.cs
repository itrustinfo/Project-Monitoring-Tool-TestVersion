using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Office.Interop.Word;
using System.Text.RegularExpressions;

namespace ProjectManagementTool._content_pages
{
    public partial class ExcelView : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["file_path"] == null)
            {
                Response.Write("No file found !");
                return;
            }
            if (Request.QueryString["file_path"].Split('.')[1] == "xls" || Request.QueryString["file_path"].Split('.')[1] == "xlsx")
            {
                ReadExcel();
            }
            else if (Request.QueryString["file_path"].Split('.')[1] == "doc" || Request.QueryString["file_path"].Split('.')[1] == "docx")
            {
                ReadWordoc();
            }
        }

        //protected void btnsubmit_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        string connectionString = string.Empty;
        //        if (FileUpload1.HasFile)
        //        {
        //            string fileName = Path.GetFileName(FileUpload1.PostedFile.FileName);
        //            string fileExtension = Path.GetExtension(FileUpload1.PostedFile.FileName);
        //            string fileLocation = Server.MapPath("~/RegExcel/" + fileName);
        //            FileUpload1.SaveAs(fileLocation);

        //            //Check whether file extension is xls or xslx

        //            if (fileExtension == ".xls")
        //            {
        //                connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileLocation + @";Extended Properties=" + Convert.ToChar(34).ToString() + @"Excel 8.0;Imex=1;HDR=Yes;" + Convert.ToChar(34).ToString();
        //            }
        //            else if (fileExtension == ".xlsx")
        //            {
        //                connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileLocation + @";Extended Properties=" + Convert.ToChar(34).ToString() + @"Excel 8.0;Imex=1;HDR=Yes;" + Convert.ToChar(34).ToString();
        //            }

        //            //Create OleDB Connection and OleDb Command

        //            OleDbConnection con = new OleDbConnection(connectionString);
        //            OleDbCommand cmd = new OleDbCommand();
        //            cmd.CommandType = System.Data.CommandType.Text;
        //            cmd.Connection = con;
        //            OleDbDataAdapter dAdapter = new OleDbDataAdapter(cmd);
        //            DataTable dtExcelRecords = new DataTable();
        //            DataTable dtExcelRecords1 = new DataTable();
        //            con.Open();
        //            DataTable dtExcelSheetName = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
        //            int count = 0;
        //            foreach (DataRow dr in dtExcelSheetName.Rows)
        //            {
        //                dtExcelRecords = null;
        //                dtExcelRecords = new DataTable();
        //                //
        //                string getExcelSheetName = dr["Table_Name"].ToString();
        //                cmd.CommandText = "SELECT * FROM [" + getExcelSheetName + "]";
        //                dAdapter.SelectCommand = cmd;
        //                dAdapter.Fill(dtExcelRecords);
        //                con.Close();

        //                AjaxControlToolkit.TabPanel tabPanel = new AjaxControlToolkit.TabPanel()
        //                {
        //                    ID = count.ToString(),
        //                    HeaderText = getExcelSheetName.Remove(getExcelSheetName.Length - 1)


        //                };
        //                TabContainer1.ActiveTabIndex = 0;

        //                TabContainer1.Controls.Add(tabPanel);
        //                //
        //                GridView grd = new GridView();
        //                grd.HeaderStyle.Wrap = false;
        //                grd.SelectedRowStyle.Wrap = false;
        //                grd.DataSource = dtExcelRecords;
        //                grd.DataBind();
        //                TabContainer1.Tabs[count].Controls.Add(grd);
        //                count++;
        //                //if (count == 0)
        //                //{
        //                //    TabPanel1.HeaderText = getExcelSheetName.Remove(getExcelSheetName.Length - 1);
        //                //    grdExcelView.DataSource = dtExcelRecords;
        //                //    grdExcelView.DataBind();
        //                //}
        //                //else if (count == 1)
        //                //{
        //                //    TabPanel2.Visible = true;
        //                //    TabPanel2.HeaderText = getExcelSheetName.Remove(getExcelSheetName.Length - 1);
        //                //    GridView1.DataSource = dtExcelRecords;
        //                //    GridView1.DataBind();
        //                //}


        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Response.Write("Error :" + ex.Message);
        //    }
        //}

        //protected void btnOpneXML_Click(object sender, EventArgs e)
        //{
        //    //Save the uploaded Excel file.
        //    string filePath = Server.MapPath("~/RegExcel/") + Path.GetFileName(FileUpload1.PostedFile.FileName);
        //    FileUpload1.SaveAs(filePath);
        //    try
        //    {
        //        //Open the Excel file in Read Mode using OpenXml.
        //        using (SpreadsheetDocument doc = SpreadsheetDocument.Open(filePath, false))
        //        {
        //            //Read the first Sheet from Excel file.
        //            Sheet sheet = doc.WorkbookPart.Workbook.Sheets.GetFirstChild<Sheet>();

        //            //Get the Worksheet instance.
        //            Worksheet worksheet = (doc.WorkbookPart.GetPartById(sheet.Id.Value) as WorksheetPart).Worksheet;

        //            //Fetch all the rows present in the Worksheet.
        //            IEnumerable<Row> rows = worksheet.GetFirstChild<SheetData>().Descendants<Row>();

        //            //Create a new DataTable.
        //            DataTable dt = new DataTable();

        //            //Loop through the Worksheet rows.
        //            foreach (Row row in rows)
        //            {
        //                //Use the first row to add columns to DataTable.
        //                if (row.RowIndex.Value == 1)
        //                {
        //                    foreach (Cell cell in row.Descendants<Cell>())
        //                    {
        //                        dt.Columns.Add(GetValue(doc, cell));
        //                    }
        //                }
        //                else
        //                {
        //                    //Add rows to DataTable.
        //                    dt.Rows.Add();
        //                    int i = 0;
        //                    foreach (Cell cell in row.Descendants<Cell>())
        //                    {
        //                        dt.Rows[dt.Rows.Count - 1][i] = GetValue(doc, cell);
        //                        i++;
        //                    }
        //                }
        //            }
        //            //grdExcelView.DataSource = dt;
        //           // grdExcelView.DataBind();
        //            WorkbookPart workBookPart = doc.WorkbookPart;
        //            List<string> lstComments = new List<string>();
        //            foreach (WorksheetPart sheet1 in workBookPart.WorksheetParts)
        //            {
        //                foreach (WorksheetCommentsPart commentsPart in sheet1.GetPartsOfType<WorksheetCommentsPart>())
        //                {
        //                    foreach (Comment comment in commentsPart.Comments.CommentList)
        //                    {
        //                        lstComments.Add(comment.InnerText);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        Response.Write("Error :" + ex.Message);
        //    }
        //}

        //private string GetValue(SpreadsheetDocument doc, Cell cell)
        //{
        //    string value = string.Empty;
        //    if (cell.CellValue != null)
        //    {
        //        value = cell.CellValue.InnerText;
        //        if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
        //        {
        //            return doc.WorkbookPart.SharedStringTablePart.SharedStringTable.ChildElements.GetItem(int.Parse(value)).InnerText;
        //        }
        //    }
        //    return value;
        //}

        private void ReadExcel()
        {
            if(Request.QueryString["file_path"] == null)
            {
                Response.Write("No file found !");
                return;
            }
            string connectionString = string.Empty;
            try
            {
                if (Request.QueryString["file_path"] != null)
                {
                    string fileName = Request.QueryString["file_path"];// Path.GetFileName(FileUpload1.PostedFile.FileName);
                                                                       // string fileExtension = Path.GetExtension(FileUpload1.PostedFile.FileName);
                    string fileLocation = Server.MapPath("~") + Request.QueryString["file_path"];// Request.QueryString["file_path"];// Server.MapPath("~/RegExcel/" + fileName);
                  //  FileUpload1.SaveAs(fileLocation);

                    //Check whether file extension is xls or xslx

                    if (fileName.Split('.')[1] == "xls")
                    {
                        connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileLocation + @";Extended Properties=" + Convert.ToChar(34).ToString() + @"Excel 8.0;Imex=1;HDR=Yes;" + Convert.ToChar(34).ToString();
                    }
                    else if (fileName.Split('.')[1] == "xlsx")
                    {
                        connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileLocation + @";Extended Properties=" + Convert.ToChar(34).ToString() + @"Excel 8.0;Imex=1;HDR=Yes;" + Convert.ToChar(34).ToString();
                    }

                    //Create OleDB Connection and OleDb Command

                    OleDbConnection con = new OleDbConnection(connectionString);
                    OleDbCommand cmd = new OleDbCommand();
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Connection = con;
                    OleDbDataAdapter dAdapter = new OleDbDataAdapter(cmd);
                    System.Data.DataTable dtExcelRecords = new System.Data.DataTable();
                    System.Data.DataTable dtExcelRecords1 = new System.Data.DataTable();
                    con.Open();
                    System.Data.DataTable dtExcelSheetName = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    int count = 0;
                    foreach (DataRow dr in dtExcelSheetName.Rows)
                    {
                        dtExcelRecords = null;
                        dtExcelRecords = new System.Data.DataTable();
                        //
                        string getExcelSheetName = dr["Table_Name"].ToString();
                        cmd.CommandText = "SELECT * FROM [" + getExcelSheetName + "]";
                        dAdapter.SelectCommand = cmd;
                        dAdapter.Fill(dtExcelRecords);
                        con.Close();

                        AjaxControlToolkit.TabPanel tabPanel = new AjaxControlToolkit.TabPanel()
                        {
                            ID = count.ToString(),
                            HeaderText = getExcelSheetName.Remove(getExcelSheetName.Length - 1)


                        };
                        TabContainer1.ActiveTabIndex = 0;
                        tabPanel.CssClass = "tabpanlecss";
                        TabContainer1.Controls.Add(tabPanel);
                        //
                        GridView grd = new GridView();
                        grd.HeaderStyle.Wrap = false;
                        grd.CssClass = "aclass";
                     
                        grd.RowStyle.Wrap = false;
                        grd.DataSource = dtExcelRecords;
                        grd.DataBind();
                        TabContainer1.Tabs[count].Controls.Add(grd);
                        count++;
                        //if (count == 0)
                        //{
                        //    TabPanel1.HeaderText = getExcelSheetName.Remove(getExcelSheetName.Length - 1);
                        //    grdExcelView.DataSource = dtExcelRecords;
                        //    grdExcelView.DataBind();
                        //}
                        //else if (count == 1)
                        //{
                        //    TabPanel2.Visible = true;
                        //    TabPanel2.HeaderText = getExcelSheetName.Remove(getExcelSheetName.Length - 1);
                        //    GridView1.DataSource = dtExcelRecords;
                        //    GridView1.DataBind();
                        //}


                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write("Error :" + ex.Message);
            }
        }

        private void ReadWordoc()
        {
            if (Request.QueryString["file_path"] == null)
            {
                Response.Write("No file found !");
                return;
            }
            object documentFormat = 8;
            string randomName = DateTime.Now.Ticks.ToString();
            object htmlFilePath = Server.MapPath("~/_modal_pages/Temp/") + randomName + ".htm";
            string directoryPath = Server.MapPath("~/_modal_pages/Temp/") + randomName + "_files";
            object fileSavePath = Server.MapPath("~") + Request.QueryString["file_path"];// Server.MapPath("~/Temp/") + Path.GetFileName(FileUpload1.PostedFile.FileName);

            //If Directory not present, create it.
            if (!Directory.Exists(Server.MapPath("~/_modal_pages/Temp/")))
            {
                Directory.CreateDirectory(Server.MapPath("~/_modal_pages/Temp/"));
            }

            //Upload the word document and save to Temp folder.
          

            //Open the word document in background.
            _Application applicationclass = new Application();
            applicationclass.Documents.Open(ref fileSavePath);
            applicationclass.Visible = false;
            Document document = applicationclass.ActiveDocument;

            //Save the word document as HTML file.
            document.SaveAs(ref htmlFilePath, ref documentFormat);

            //Close the word document.
            document.Close();

            //Read the saved Html File.
            string wordHTML = System.IO.File.ReadAllText(htmlFilePath.ToString(),System.Text.Encoding.Default);

            //Loop and replace the Image Path.
            //foreach (Match match in Regex.Matches(wordHTML, "<v:imagedata.+?src=[\"'](.+?)[\"'].*?>", RegexOptions.IgnoreCase))
            //{
            //    wordHTML = Regex.Replace(wordHTML, match.Groups[1].Value, "Temp/" + match.Groups[1].Value);
            //}

            wordHTML = wordHTML.Replace("<v:imagedata", "<img");
            wordHTML = wordHTML.Replace(randomName + "_files", "Temp/" + randomName + "_files");
            wordHTML = wordHTML.Replace("/Temp/Temp/", "/Temp/");
            //Delete the Uploaded Word File.
            // System.IO.File.Delete(fileSavePath.ToString());
            //System.IO.File.
            //Write the file
           //using (System.IO.StreamWriter outfile = new System.IO.StreamWriter(htmlFilePath.ToString()))
           // {
           //     outfile.Write(wordHTML);
           // }
            dvWord.InnerHtml = wordHTML;
          //iframhtml.Attributes.Add("srcdoc", wordHTML);

        }
    }
}