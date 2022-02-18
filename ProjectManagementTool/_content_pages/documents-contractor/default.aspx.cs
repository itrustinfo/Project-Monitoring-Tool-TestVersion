using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProjectManager.DAL;

namespace ProjectManagementTool._content_pages.documents_contractor
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
                    if (Request.QueryString["UserUID"] != null)
                    {
                        GrdDocuments.DataSource = getdt.GetNextUserDocuments(new Guid(Request.QueryString["PrjUID"]), new Guid(Request.QueryString["WkpgUID"]));
                        GrdDocuments.DataBind();
                        LblDocumentHeading.Text = "Document List";
                        lblTotalcount.Visible = false;
                      //  lblTotalcount.Text = "Total Count : " + GrdDocuments.Rows.Count.ToString();
                        //if (GrdDocuments.Rows.Count > 15)
                        //{
                        //    ScriptManager.RegisterStartupScript(Page, this.GetType(), "Key", "<script>MakeStaticHeader('" + GrdDocuments.ClientID + "', 700, 1300 , 45 ,true); </script>", false);
                        //}
                    }
                    else if (Request.QueryString["PrjUID"] != null)
                    {
                        if (Request.QueryString["type"].ToString() == "Contractor")
                        {
                            GrdDocuments.DataSource = getdt.GetDashboardContractotDocsSubmitted_Details(new Guid(Request.QueryString["PrjUID"]));
                            GrdDocuments.DataBind();
                            LblDocumentHeading.Text = "Document List for Contractor -> ONTB";
                            lblTotalcount.Text = "Total Count : " + GrdDocuments.Rows.Count.ToString();
                            if (GrdDocuments.Rows.Count > 15)
                            {
                                ScriptManager.RegisterStartupScript(Page, this.GetType(), "Key", "<script>MakeStaticHeader('" + GrdDocuments.ClientID + "', 700, 1300 , 45 ,true); </script>", false);
                            }
                        }
                        else
                        {
                            GrdDocuments.DataSource = getdt.GetDashboardONTBtoContractorDocs_Details(new Guid(Request.QueryString["PrjUID"]));
                            GrdDocuments.DataBind();
                            LblDocumentHeading.Text = "Document List for ONTB -> Contractor";
                            lblTotalcount.Text = "Total Count : " + GrdDocuments.Rows.Count.ToString();
                            if (GrdDocuments.Rows.Count > 15)
                            {
                                ScriptManager.RegisterStartupScript(Page, this.GetType(), "Key", "<script>MakeStaticHeader('" + GrdDocuments.ClientID + "', 700, 1300 , 45 ,true); </script>", false);
                            }
                        }
                    }
                    
                        Session["isDownloadNJSE"] = false;
                    DataSet dscheck = new DataSet();
                    dscheck = getdt.GetUsertypeFunctionality_Mapping(Session["TypeOfUser"].ToString());
                    if (dscheck.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dscheck.Tables[0].Rows)
                        {

                            if (dr["Code"].ToString() == "FN") // DOWNLOAD APPROVED DOCUMENTS (FINAL APPROVED BY CLIENT)
                            {
                                ViewState["isDownloadClient"] = "true";
                                Session["isDownloadClient"] = "true";
                            }
                            if (dr["Code"].ToString() == "FM") // DOWNLOAD APPROVED DOCUMENTS (FINAL APPROVED BY NJSEI)
                            {
                                ViewState["isDownloadNJSE"] = "true";
                                Session["isDownloadNJSE"] = "true";
                            }
                        }
                    }
                }
            }
        }

        public string GetTaskHierarchy_By_DocumentUID(string DocumentUID)
        {
            return getdt.GetTaskHierarchy_By_DocumentUID(new Guid(DocumentUID));
        }

        public string GetSubmittalName(string DocumentID)
        {
            return getdt.getDocumentName_by_DocumentUID(new Guid(DocumentID));
        }
        public string GetDocumentTypeIcon(string DocumentExtn)
        {
            return getdt.GetDocumentTypeMasterIcon_by_Extension(DocumentExtn);
        }

        public string GetDocumentName(string DocumentExtn)
        {
            string retval = getdt.GetDocumentMasterType_by_Extension(DocumentExtn);
            if (retval == null || retval == "")
            {
                return "N/A";
            }
            else
            {
                return retval;
            }
        }

        protected void GrdDocuments_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            //LinkButton lnkbtn;
            //if (e.Row.RowType == DataControlRowType.Header)
            //{
            //    foreach (TableCell cell in e.Row.Cells)
            //    {
            //        lnkbtn = (LinkButton)cell.Controls[0];
            //        if (!string.IsNullOrEmpty(GrdDocuments.SortExpression))
            //        {
            //            if (GrdDocuments.SortExpression.Equals(lnkbtn.Text))
            //            {
            //                cell.BackColor = System.Drawing.Color.Crimson;
            //            }
            //        }
            //    }
            //}
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
               
                DataSet ds = getdt.getTop1_DocumentStatusSelect(new Guid(e.Row.Cells[0].Text));
                Label lblDocumentName = (Label)e.Row.FindControl("lblDocumentName");
                //
                if (Request.QueryString["UserUID"] != null)
                {
                    e.Row.Visible = false;
                    DataSet dsNext = getdt.GetNextStep_By_DocumentUID(new Guid(e.Row.Cells[0].Text), ds.Tables[0].Rows[0]["ActivityType"].ToString());

                    foreach (DataRow dr in dsNext.Tables[0].Rows)
                    {
                        string NextUser = getdt.GetNextUser_By_DocumentUID(new Guid(e.Row.Cells[0].Text), int.Parse(dr["ForFlow_Step"].ToString()));
                        if (!string.IsNullOrEmpty(NextUser))
                        {
                            if (Session["UserUID"].ToString().ToUpper() == NextUser.ToUpper())
                            {
                                e.Row.Visible = true;
                                return;
                            }
                            else
                            {
                                e.Row.Visible = false;

                            }
                        }
                        else
                        {
                            e.Row.Visible = false;
                        }
                    }
                }
                if (ds != null)
                {


                    if (ds.Tables[0].Rows[0]["DocumentType"].ToString() == "General Document")
                    {
                        e.Row.Cells[8].Text = "No History";
                    }
                }
                if (ds.Tables[0].Rows[0]["ActivityType"].ToString() != "" && ds.Tables[0].Rows[0]["TopVersion"].ToString() != "")
                {
                    //e.Row.Cells[1].Text = ds.Tables[0].Rows[0]["TopVersion"].ToString();


                    string newVersionFileName = Path.GetFileNameWithoutExtension(Server.MapPath(ds.Tables[0].Rows[0]["Doc_Path"].ToString()));
                    lblDocumentName.Text = newVersionFileName.Substring(0, (newVersionFileName.Length - 2)) + " [ Ver. " + ds.Tables[0].Rows[0]["TopVersion"].ToString() + " ]";

                }
            }
        }

        protected void GrdDocuments_DataBound(object sender, EventArgs e)
        {
            //int sortedColumnPosition = 0;
            //LinkButton lnkbtn;

            //// Gets position of column whose header text matches SortExpression
            //// of the GridView when column is sorted
            //foreach (TableCell cell in GrdDocuments.HeaderRow.Cells)
            //{
            //    lnkbtn = (LinkButton)cell.Controls[0];
            //    if (lnkbtn.Text == GrdDocuments.SortExpression)
            //    {
            //        break;
            //    }
            //    sortedColumnPosition++;
            //}
            //if (!string.IsNullOrEmpty(GrdDocuments.SortExpression))
            //{
            //    foreach (GridViewRow row in GrdDocuments.Rows)
            //    {
            //        row.Cells[sortedColumnPosition].BackColor = System.Drawing.Color.LavenderBlush;
            //    }
            //}
        }
        protected void GrdDocuments_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string UID = e.CommandArgument.ToString();
            if (e.CommandName == "download")
            {
                string path = string.Empty;
                string filename = string.Empty;
                DataSet ds1 = null;
                DataSet ds = getdt.getLatest_DocumentVerisonSelect(new Guid(UID));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    path = Server.MapPath(ds.Tables[0].Rows[0]["Doc_FileName"].ToString());
                    //File.Decrypt(path);
                }
                else
                {
                    ds1 = getdt.ActualDocuments_SelectBy_ActualDocumentUID(new Guid(UID));
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        if (ds1.Tables[0].Rows[0]["ActualDocument_Path"].ToString() != "")
                        {
                            path = Server.MapPath(ds1.Tables[0].Rows[0]["ActualDocument_Path"].ToString());
                        }
                    }
                }
                // added on  20/10/2020
                ds.Clear();
                ds = getdt.ActualDocuments_SelectBy_ActualDocumentUID(new Guid(UID));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["ActualDocument_Name"].ToString() != "")
                    {
                        filename = ds.Tables[0].Rows[0]["ActualDocument_Name"].ToString() + ds.Tables[0].Rows[0]["ActualDocument_Type"].ToString();
                    }
                }
                //
                try
                {
                    string getExtension = System.IO.Path.GetExtension(path);
                    string outPath = path.Replace(getExtension, "") + "_download" + getExtension;
                    getdt.DecryptFile(path, outPath);
                    System.IO.FileInfo file = new System.IO.FileInfo(outPath);

                    if (file.Exists)
                    {
                        int Cnt = getdt.DocumentHistroy_InsertorUpdate(Guid.NewGuid(), new Guid(UID), new Guid(Session["UserUID"].ToString()), "Downloaded", "Documents");
                        if (Cnt <= 0)
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code: DDH-01. there is a problem with updating histroy. Please contact system admin.');</script>");
                        }
                        Response.Clear();

                        // Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
                        Response.AddHeader("Content-Disposition", "attachment; filename=" + filename);
                        Response.AddHeader("Content-Length", file.Length.ToString());

                        Response.ContentType = "application/octet-stream";

                        Response.WriteFile(file.FullName);

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

                    }
                    else
                    {

                        //Response.Write("This file does not exist.");
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('File does not exists.');</script>");

                    }
                }
                catch (Exception ex)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Document not uploaded for this flow.Please Contact Document controller!');</script>");
                }
            }
        }

    }
}