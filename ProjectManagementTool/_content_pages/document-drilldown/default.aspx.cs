using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagementTool._content_pages.document_drilldown
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
                    if (Request.QueryString["DocumentType"] != null)
                    {
                        string[] DocType = Request.QueryString["DocumentType"].ToString().Split('_');
                        

                        BindDocuments(DocType[0].ToString(), DocType[1].ToString());
                    }
                    
                }
            }
        }

        private void BindDocuments(string ColLabel,string MyData)
        {
            if (ColLabel == "Ontime" && MyData == "Tot. Documents")
            {
                DataSet ds = getdt.ActualDocuments_SelectBy_WorkpackageUID(new Guid(Request.QueryString["WorkPackageUID"]));
                GrdDocuments.DataSource = ds;
                GrdDocuments.DataBind();
                GrdActualSubmittedDocuments.Visible = false;
                GrdDocuments.Visible = true;
                GrdReviewedDocuments.Visible = false;
                GrdApprovedDocuments.Visible = false;
                GrdClientApprovedDocuments.Visible = false;
                LblDocumentHeading.Text = "Total Documents";
            }
            else if (ColLabel == "Ontime" && MyData == "Submitted")
            {
                GrdActualSubmittedDocuments.Visible = true;
                GrdDocuments.Visible = false;
                GrdReviewedDocuments.Visible = false;
                GrdApprovedDocuments.Visible = false;
                GrdClientApprovedDocuments.Visible = false;
                DataSet ds = getdt.Submitted_ActualDocuments_SelectBy_WorkPackageUID_NotDelayed(new Guid(Request.QueryString["WorkPackageUID"]));
                GrdActualSubmittedDocuments.DataSource = ds;
                GrdActualSubmittedDocuments.DataBind();
                LblDocumentHeading.Text = "Submitted Documents";
            }
            else if (ColLabel == "Delayed" && MyData == "Submitted")
            {
                LblDocumentHeading.Text = "Delayed on Submitting Documents";
                GrdActualSubmittedDocuments.Visible = true;
                GrdDocuments.Visible = false;
                GrdReviewedDocuments.Visible = false;
                GrdApprovedDocuments.Visible = false;
                GrdClientApprovedDocuments.Visible = false;
                DataSet ds = getdt.Submitted_ActualDocuments_SelectBy_WorkPackageUID_Delayed(new Guid(Request.QueryString["WorkPackageUID"]));
                GrdActualSubmittedDocuments.DataSource = ds;
                GrdActualSubmittedDocuments.DataBind();
            }
            else if (ColLabel == "Ontime" && MyData == "Code A")
            {
                LblDocumentHeading.Text = "Code A Documents";
                GrdActualSubmittedDocuments.Visible = false;
                GrdDocuments.Visible = false;
                GrdReviewedDocuments.Visible = false;
                GrdApprovedDocuments.Visible = true;
                GrdClientApprovedDocuments.Visible = false;
                DataSet ds = getdt.Approved_ActualDocuments_SelectBy_WorkPackageUID_NotDelayed(new Guid(Request.QueryString["WorkPackageUID"]));
                GrdApprovedDocuments.DataSource = ds;
                GrdApprovedDocuments.DataBind();
            }
            else if (ColLabel == "Delayed" && MyData == "Code A")
            {
                LblDocumentHeading.Text = "Delayed Code A Documents";
                GrdActualSubmittedDocuments.Visible = false;
                GrdDocuments.Visible = false;
                GrdReviewedDocuments.Visible = false;
                GrdApprovedDocuments.Visible = true;
                GrdClientApprovedDocuments.Visible = false;
                DataSet ds = getdt.Approved_ActualDocuments_SelectBy_WorkPackageUID_Delayed(new Guid(Request.QueryString["WorkPackageUID"]));
                GrdApprovedDocuments.DataSource = ds;
                GrdApprovedDocuments.DataBind();
            }
            else if (ColLabel == "Ontime" && MyData == "Client Approved")
            {
                LblDocumentHeading.Text = "Client Approved Documents";
                GrdActualSubmittedDocuments.Visible = false;
                GrdDocuments.Visible = false;
                GrdReviewedDocuments.Visible = false;
                GrdClientApprovedDocuments.Visible = true;
                DataSet ds = getdt.ClientApproved_ActualDocuments_SelectBy_WorkPackageUID_NotDelayed(new Guid(Request.QueryString["WorkPackageUID"]));
                GrdClientApprovedDocuments.DataSource = ds;
                GrdClientApprovedDocuments.DataBind();
            }
            else if (ColLabel == "Delayed" && MyData == "Client Approved")
            {
                LblDocumentHeading.Text = "Client Delayed Approved Documents";
                GrdActualSubmittedDocuments.Visible = false;
                GrdDocuments.Visible = false;
                GrdReviewedDocuments.Visible = false;
                GrdClientApprovedDocuments.Visible = true;
                DataSet ds = getdt.ClientApproved_ActualDocuments_SelectBy_WorkPackageUID_Delayed(new Guid(Request.QueryString["WorkPackageUID"]));
                GrdClientApprovedDocuments.DataSource = ds;
                GrdClientApprovedDocuments.DataBind();
            }
            else if (ColLabel == "FlowAll")
            {
                LblDocumentHeading.Text = MyData;
                GrdActualSubmittedDocuments.Visible = false;
                GrdDocuments.Visible = false;
                GrdReviewedDocuments.Visible = false;
                GrdClientApprovedDocuments.Visible = true;
                DataSet ds = getdt.ClientAllDocuments(new Guid(Request.QueryString["WorkPackageUID"]), MyData);
                GrdClientApprovedDocuments.DataSource = ds;
                GrdClientApprovedDocuments.DataBind();
            }
            else
            {
                if (ColLabel == "Ontime")
                {
                    LblDocumentHeading.Text = MyData + "Documents";
                    GrdActualSubmittedDocuments.Visible = false;
                    GrdDocuments.Visible = false;
                    GrdReviewedDocuments.Visible = true;
                    GrdApprovedDocuments.Visible = false;
                    GrdClientApprovedDocuments.Visible = false;
                    DataSet ds = getdt.Reviewed_ActualDocuments_SelectBy_WorkPackageUID_NotDelayed(new Guid(Request.QueryString["WorkPackageUID"]), MyData);
                    
                    
                    GrdReviewedDocuments.DataSource = ds;
                    GrdReviewedDocuments.DataBind();
                }
                else
                {
                    LblDocumentHeading.Text = "Delayed " + MyData + " Documents";
                    GrdActualSubmittedDocuments.Visible = false;
                    GrdDocuments.Visible = false;
                    GrdReviewedDocuments.Visible = true;
                    GrdApprovedDocuments.Visible = false;
                    GrdClientApprovedDocuments.Visible = false;
                    DataSet ds = getdt.Reviewed_ActualDocuments_SelectBy_WorkPackageUID_Delayed(new Guid(Request.QueryString["WorkPackageUID"]), MyData);
                    GrdReviewedDocuments.DataSource = ds;
                    GrdReviewedDocuments.DataBind();
                }
            }
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

        protected void GrdDocuments_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdDocuments.PageIndex = e.NewPageIndex;
            string[] DocType = Request.QueryString["DocumentType"].ToString().Split('_');
            BindDocuments(DocType[0].ToString(), DocType[1].ToString());
        }

        protected void GrdDocuments_RowDataBound(object sender, GridViewRowEventArgs e)
        {

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

        protected void GrdActualSubmittedDocuments_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[4].Text != "&nbsp;")
                {
                    DateTime dt = getdt.GetDocumentReviewedDateByID(new Guid(e.Row.Cells[4].Text), "Submitted");
                    e.Row.Cells[4].Text = dt.ToString("dd MMM yyyy");
                }
            }
        }

        protected void GrdActualSubmittedDocuments_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdActualSubmittedDocuments.PageIndex = e.NewPageIndex;
            string[] DocType = Request.QueryString["DocumentType"].ToString().Split('_');
            BindDocuments(DocType[0].ToString(), DocType[1].ToString());
        }

        protected void GrdActualSubmittedDocuments_RowCommand(object sender, GridViewCommandEventArgs e)
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

        protected void GrdReviewedDocuments_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[4].Text != "&nbsp;")
                {
                    DateTime dt = getdt.GetDocumentReviewedDateByID(new Guid(e.Row.Cells[4].Text), "Code B");
                    e.Row.Cells[4].Text = dt.ToString("dd MMM yyyy");
                }
                //if (e.Row.Cells[5].Text != "&nbsp;")
                //{
                //    int Cnt = getdt.GetDocumentReviewedinDays(new Guid(e.Row.Cells[5].Text), "Code B");
                //    e.Row.Cells[5].Text = Cnt.ToString();
                //}
            }
        }

        protected void GrdReviewedDocuments_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdReviewedDocuments.PageIndex = e.NewPageIndex;
            string[] DocType = Request.QueryString["DocumentType"].ToString().Split('_');
            BindDocuments(DocType[0].ToString(), DocType[1].ToString());
        }

        protected void GrdReviewedDocuments_RowCommand(object sender, GridViewCommandEventArgs e)
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

        protected void GrdApprovedDocuments_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                if (e.Row.Cells[4].Text != "&nbsp;")
                {
                    DateTime dt = getdt.GetDocumentReviewedDateByID(new Guid(e.Row.Cells[4].Text), "Code A");
                    e.Row.Cells[4].Text = dt.ToString("dd MMM yyyy");
                }
                //if (e.Row.Cells[5].Text != "&nbsp;")
                //{
                //    int Cnt = getdt.GetDocumentReviewedinDays(new Guid(e.Row.Cells[5].Text), "Code A");
                //    e.Row.Cells[5].Text = Cnt.ToString();
                //}
            }
        }

        protected void GrdApprovedDocuments_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdApprovedDocuments.PageIndex = e.NewPageIndex;
            string[] DocType = Request.QueryString["DocumentType"].ToString().Split('_');
            BindDocuments(DocType[0].ToString(), DocType[1].ToString());
        }

        protected void GrdApprovedDocuments_RowCommand(object sender, GridViewCommandEventArgs e)
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

        protected void GrdClientApprovedDocuments_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdClientApprovedDocuments.PageIndex = e.NewPageIndex;
            string[] DocType = Request.QueryString["DocumentType"].ToString().Split('_');
            BindDocuments(DocType[0].ToString(), DocType[1].ToString());
        }

        protected void GrdClientApprovedDocuments_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                if (e.Row.Cells[4].Text != "&nbsp;")
                {
                    DateTime dt = getdt.GetDocumentReviewedDateByID(new Guid(e.Row.Cells[4].Text), "Client Approved");
                    e.Row.Cells[4].Text = dt.ToString("dd MMM yyyy");
                }
            }
        }

        protected void GrdClientApprovedDocuments_RowCommand(object sender, GridViewCommandEventArgs e)
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
        }

        
    }
}