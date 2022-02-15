using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagementTool
{
    public partial class download_document : System.Web.UI.Page
    {
        DBGetData getdata = new DBGetData();
        GeneralDocuments GD = new GeneralDocuments();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DisplayLogo();
                if (Request.QueryString["DocumentUID"] != null && Request.QueryString["Ticks"] != null)
                {
                    long ticks = long.Parse(Request.QueryString["Ticks"].ToString());
                    DateTime dt = new DateTime(ticks);
                    int Hours = (DateTime.Now - dt).Hours;

                    int ExpiryHours = 0;
                    string ExpityIn = WebConfigurationManager.AppSettings["HoursExpiry"];
                    if (ExpityIn != "")
                    {
                        ExpiryHours = Convert.ToInt32(ExpityIn);
                    }
                    else
                    {
                        ExpiryHours = 24;
                    }


                    if (Hours > ExpiryHours)
                    {
                        LblMessage.InnerText = "Download link as been Expired..";
                    }
                    else
                    {
                        DownloadDcoument(Request.QueryString["DocumentUID"]);
                    }
                }
                else if (Request.QueryString["GeneralDocumentUID"] != null && Request.QueryString["Ticks"] != null)
                {
                    long ticks = long.Parse(Request.QueryString["Ticks"].ToString());
                    DateTime dt = new DateTime(ticks);
                    int Hours = (DateTime.Now - dt).Hours;

                    int ExpiryHours = 0;
                    string ExpityIn = WebConfigurationManager.AppSettings["HoursExpiry"];
                    if (ExpityIn != "")
                    {
                        ExpiryHours = Convert.ToInt32(ExpityIn);
                    }
                    else
                    {
                        ExpiryHours = 24;
                    }

                    if (Hours > ExpiryHours)
                    {
                        LblMessage.InnerText = "Download link as been Expired..";
                    }
                    else
                    {
                        DownloadGeneralDocument(Request.QueryString["GeneralDocumentUID"]);
                    }
                }
                else
                {
                    LblMessage.InnerText = "Invalid Request";
                }
            }
        }
        private void DownloadGeneralDocument(string GeneralDocumentUID)
        {
            string path = string.Empty;
            string filename = string.Empty;
            DataSet ds = GD.GeneralDocuments_SelectBy_GeneralDocumentUID(new Guid(Request.QueryString["GeneralDocumentUID"]));
            if (ds.Tables[0].Rows.Count > 0)
            {
                path = Server.MapPath(ds.Tables[0].Rows[0]["GeneralDocument_Path"].ToString());
                string getExtension = System.IO.Path.GetExtension(path);
                string outPath = path.Replace(getExtension, "") + "_download" + getExtension;
                getdata.DecryptFile(path, outPath);
                System.IO.FileInfo file = new System.IO.FileInfo(outPath);

                if (file.Exists)
                {
                    Response.Clear();

                    Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
                    
                    Response.AddHeader("Content-Length", file.Length.ToString());

                    Response.ContentType = "application/octet-stream";

                    Response.TransmitFile(file.FullName);

                    Response.Flush();

                    try
                    {
                        if (File.Exists(outPath))
                        {
                            File.Delete(outPath);
                        }
                    }
                    catch (Exception ex)
                    {
                        //throw
                    }

                    Response.End();
                    Thread.Sleep(200);
                    this.ClientScript.RegisterClientScriptBlock(this.GetType(), "Close", "window.close()", true);
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('File does not exists.');</script>");
                }
            }
        }
        private void DownloadDcoument(string DocumentUID)
        {
            string path = string.Empty;
            string filename = string.Empty;

            DataSet ds = getdata.getLatest_DocumentVerisonSelect(new Guid(DocumentUID));
            if (ds.Tables[0].Rows.Count > 0)
            {
                path = Server.MapPath(ds.Tables[0].Rows[0]["Doc_FileName"].ToString());
                //File.Decrypt(path);
            }
            else
            {
                ds.Clear();
                ds = getdata.ActualDocuments_SelectBy_ActualDocumentUID(new Guid(DocumentUID));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["ActualDocument_Path"].ToString() != "")
                    {
                        path = Server.MapPath(ds.Tables[0].Rows[0]["ActualDocument_Path"].ToString());
                    }
                }
            }

            string getExtension = System.IO.Path.GetExtension(path);
            string outPath = path.Replace(getExtension, "") + "_download" + getExtension;
            getdata.DecryptFile(path, outPath);
            System.IO.FileInfo file = new System.IO.FileInfo(outPath);
            if (file.Exists)
            {
                Response.Clear();

                 Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
                //Response.AddHeader("Content-Disposition", "attachment; filename=" + filename);
                Response.AddHeader("Content-Length", file.Length.ToString());

                Response.ContentType = "application/octet-stream";

                //Response.WriteFile(file.FullName);

                Response.TransmitFile(file.FullName);

                Response.End();
                Thread.Sleep(200);
                this.ClientScript.RegisterClientScriptBlock(this.GetType(), "Close", "window.close()", true);
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('File does not exists.');</script>");

            }
        }

        private void DisplayLogo()
        {
            string Domain = WebConfigurationManager.AppSettings["Domain"];
            if (Domain == "NJSEI")
            {
                NJSEI.Visible = true;
                ONTB.Visible = false;
                ONTBLogo.Visible = false;
                NJSEILogo.Visible = true;
            }
            else
            {
                NJSEI.Visible = false;
                ONTB.Visible = true;
                ONTBLogo.Visible = true;
                NJSEILogo.Visible = false;
            }
        }
    }
}