using ProjectManager.DAL;
using System;
using System.Data;
using System.Data.OleDb;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Office.Interop.Word;
using System.Text.RegularExpressions;
using System.Web.Configuration;

namespace ProjectManagementTool._modal_pages
{
    public partial class preview_document : System.Web.UI.Page
    {
        DBGetData getdata = new DBGetData();
        GeneralDocuments GD = new GeneralDocuments();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Username"] == null)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
            }
            else
            {
                if (Request.QueryString["previewpath"] != null && Request.QueryString["ActualDocumentUID"] != null)
                {
                    DataSet ds = getdata.getLatest_DocumentVerisonSelect(new Guid(Request.QueryString["ActualDocumentUID"]));
                    //DataSet ds = getdata.ActualDocuments_SelectBy_ActualDocumentUID(new Guid(Request.QueryString["ActualDocumentUID"]));
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        //string path = Request.QueryString["previewpath"].Replace('!', '&');
                        string path = ds.Tables[0].Rows[0]["Doc_FileName"].ToString();
                        string FilePath = Server.MapPath(path);
                        if (File.Exists(FilePath))
                        {
                            int Cnt = getdata.DocumentHistroy_InsertorUpdate(Guid.NewGuid(), new Guid(Request.QueryString["ActualDocumentUID"]), new Guid(Session["UserUID"].ToString()), "Viewed", "Documents");
                            if (Cnt <= 0)
                            {
                                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code: DDH-01. there is a problem with updating histroy. Please contact system admin.');</script>");
                            }

                            string getExtension = System.IO.Path.GetExtension(FilePath);
                            //string outPath = FilePath.Replace(getExtension, "") + "_download" + getExtension;
                            string sFileName = Path.GetFileName(FilePath).Replace(getExtension, "") + "_download" + getExtension;
                            string outPath = "/_PreviewLoad/" + sFileName;
                            getdata.DecryptFile(FilePath, Server.MapPath(outPath));

                            if (getExtension == ".pdf" || getExtension == ".PDF")
                            {
                                //iframe1.Src = "http://localhost:50162/" + outPath + "#toolbar=0";
                                //iframe1.Src = "https://68.169.60.139:451/" + outPath + "#toolbar=0";
                                //iframe1.Src = "https://148.66.128.123:452/" + outPath + "#toolbar=0";
                                iframe1.Src = WebConfigurationManager.AppSettings["SiteName"] + outPath + "#toolbar=0";
                                PDFPreview.Visible = true;
                                divExcel.Visible = false;
                                divWorddoc.Visible = false;
                            }
                            else if (getExtension == ".xls" || getExtension == ".xlsx")
                            {
                                PDFPreview.Visible = false;
                                divExcel.Visible = true;
                                divWorddoc.Visible = false;
                                ReadExcel(Server.MapPath(outPath));
                            }
                            else
                            {
                                PDFPreview.Visible = false;
                                divExcel.Visible = false;
                                divWorddoc.Visible = true;
                                //ReadWordoc(Server.MapPath(outPath));
                                ReadWordDocument(Request.QueryString["ActualDocumentUID"].ToString());
                            }
                        }
                        else
                        {
                            iframe1.InnerHtml = "File doesn't exists.";
                        }
                    }
                }
                else if (Request.QueryString["previewpath"] != null && Request.QueryString["GeneralDocumentUID"] != null)
                {
                    DataSet ds = GD.GeneralDocuments_SelectBy_GeneralDocumentUID(new Guid(Request.QueryString["GeneralDocumentUID"]));
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        //string path = Request.QueryString["previewpath"].Replace('!', '&');
                        string path= ds.Tables[0].Rows[0]["GeneralDocument_Path"].ToString();
                        string FilePath = Server.MapPath(path);
                        if (File.Exists(FilePath))
                        {
                            int Cnt = getdata.DocumentHistroy_InsertorUpdate(Guid.NewGuid(), new Guid(Request.QueryString["GeneralDocumentUID"]), new Guid(Session["UserUID"].ToString()), "Viewed", "General Documents");
                            if (Cnt <= 0)
                            {
                                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code: DDH-01. there is a problem with updating histroy. Please contact system admin.');</script>");
                            }

                            string getExtension = System.IO.Path.GetExtension(FilePath);
                            //string outPath = FilePath.Replace(getExtension, "") + "_download" + getExtension;
                            string sFileName = Path.GetFileName(FilePath).Replace(getExtension, "") + "_download" + getExtension;
                            string outPath = "/_PreviewLoad/" + sFileName;
                            getdata.DecryptFile(FilePath, Server.MapPath(outPath));

                            if (getExtension == ".pdf" || getExtension == ".PDF")
                            {
                                //iframe1.Src = "http://localhost:50162/" + outPath + "#toolbar=0";
                                //iframe1.Src = "https://68.169.60.139:451/" + outPath + "#toolbar=0";
                                //iframe1.Src = "https://148.66.128.123:452/" + outPath + "#toolbar=0";
                                iframe1.Src = WebConfigurationManager.AppSettings["SiteName"] + outPath + "#toolbar=0";
                                PDFPreview.Visible = true;
                                divExcel.Visible = false;
                                divWorddoc.Visible = false;
                            }
                            else if (getExtension == ".xls" || getExtension == ".xlsx")
                            {
                                PDFPreview.Visible = false;
                                divExcel.Visible = true;
                                divWorddoc.Visible = false;
                                ReadExcel(Server.MapPath(outPath));
                            }
                            else
                            {
                                PDFPreview.Visible = false;
                                divExcel.Visible = false;
                                divWorddoc.Visible = true;
                                //ReadWordoc(Server.MapPath(outPath));
                                ReadWordDocument(Request.QueryString["GeneralDocumentUID"].ToString());
                            }
                        }
                        else
                        {
                            iframe1.InnerHtml = "File doesn't exists.";
                        }
                    }
                        
                }
                
            }
        }

        private void ReadExcel(string sFilePath)
        {
            if (Request.QueryString["previewpath"] == null)
            {
                Response.Write("No file found !");
                return;
            }
            string connectionString = string.Empty;
            try
            {
                if (Request.QueryString["previewpath"] != null)
                {
                    string fileName = Request.QueryString["previewpath"];// Path.GetFileName(FileUpload1.PostedFile.FileName);
                                                                         // string fileExtension = Path.GetExtension(FileUpload1.PostedFile.FileName);
                                                                         //string fileLocation = Server.MapPath("~") + Request.QueryString["previewpath"];// Request.QueryString["file_path"];// Server.MapPath("~/RegExcel/" + fileName);
                                                                         //  FileUpload1.SaveAs(fileLocation);


                    //Check whether file extension is xls or xslx
                    string fileLocation = sFilePath;
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

        private void ReadWordoc(string sFilePath)
        {
            if (Request.QueryString["previewpath"] == null)
            {
                Response.Write("No file found !");
                return;
            }
            object documentFormat = 8;
            string randomName = DateTime.Now.Ticks.ToString();
            object htmlFilePath = Server.MapPath("~/_modal_pages/Temp/") + randomName + ".htm";
            string directoryPath = Server.MapPath("~/_modal_pages/Temp/") + randomName + "_files";
            //object fileSavePath = Server.MapPath("~") + Request.QueryString["previewpath"];// Server.MapPath("~/Temp/") + Path.GetFileName(FileUpload1.PostedFile.FileName);
            object fileSavePath = sFilePath;

            //If Directory not present, create it.
            if (!Directory.Exists(Server.MapPath("~/_modal_pages/Temp/")))
            {
                Directory.CreateDirectory(Server.MapPath("~/_modal_pages/Temp/"));
            }

            //Upload the word document and save to Temp folder.


            //Open the word document in background.
            _Application applicationclass = new Application();
            applicationclass.Documents.Open(ref fileSavePath, ReadOnly: true);
            applicationclass.Visible = false;
            Document document = applicationclass.ActiveDocument;

            //Save the word document as HTML file.
            document.SaveAs(ref htmlFilePath, ref documentFormat);

            //Close the word document.
            document.Close();

            //Read the saved Html File.
            string wordHTML = System.IO.File.ReadAllText(htmlFilePath.ToString(), System.Text.Encoding.Default);

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

        private void ReadWordDocument(string DocumentUID)
        {
            DataSet ds = getdata.GetWordDocumentFilePath_DocumentUID(new Guid(DocumentUID));
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["Status"].ToString() == "Pending")
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Document conversion is under progress. Please try again after some time');</script>");
                }
                else if (ds.Tables[0].Rows[0]["Status"].ToString() == "Failed")
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('There is a problem with viewing this word document. Please try agian after some time.');</script>");
                }
                else
                {
                    string wordHTML = System.IO.File.ReadAllText(Server.MapPath(ds.Tables[0].Rows[0]["HTML_Text"].ToString()));
                    divWorddoc.InnerHtml = wordHTML;
                }
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('No File Found.');</script>");
            }
        }
    }
}