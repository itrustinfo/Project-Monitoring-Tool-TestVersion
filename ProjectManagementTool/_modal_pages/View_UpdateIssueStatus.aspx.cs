using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagementTool._modal_pages
{
    public partial class View_UpdateIssueStatus : System.Web.UI.Page
    {
        DBGetData getdata = new DBGetData();
        TaskUpdate TKUpdate = new TaskUpdate();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Username"] == null)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
            }
            else
            {
                if (Request.QueryString["Issue_Uid"] != null)
                {
                    IssueBind();
                    BindIssueStatus();
                }
            }
        }

        private void IssueBind()
        {
            DataSet ds = getdata.getIssuesList_by_UID(new Guid(Request.QueryString["Issue_Uid"]));
            if (ds.Tables[0].Rows.Count > 0)
            {
                LblIssue.Text = ds.Tables[0].Rows[0]["Issue_Description"].ToString();
                if (ds.Tables[0].Rows[0]["Issue_Status"].ToString() == "Close")
                {
                    AddStatus.Visible = false;
                }
                else
                {
                    AddStatus.Visible = true;
                }
                AddStatus.HRef = "/_modal_pages/add-issuestatus.aspx?Issue_Uid=" + Request.QueryString["Issue_Uid"];
            }
        }
        private void BindIssueStatus()
        {
            DataSet ds = getdata.GetIssueStatus_by_Issue_Uid(new Guid(Request.QueryString["Issue_Uid"]));
            GrdIssueStatus.DataSource = ds;
            GrdIssueStatus.DataBind();
        }

        protected void GrdIssueStatus_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    if (e.Row.Cells[3].Text == "&nbsp;")
            //    {
            //        LinkButton lnk = (LinkButton)e.Row.FindControl("lnkdown");
            //        lnk.Enabled = false;
            //        lnk.Text = "No File";
            //    }
            //}

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[5].Controls.Count == 0)
                {
                    GridViewRow grd_row = e.Row;

                    GridView obj_grdview = new GridView();
                    obj_grdview.Visible = true;

                    DataSet ds = getdata.GetUploadedDocuments(grd_row.Cells[4].Text);

                    ds.Tables[0].Columns.Add("action-1");
                    ds.Tables[0].Columns.Add("action-2");

                    int count = ds.Tables[0].Rows.Count;

                    if (count == 0)
                    {
                        LinkButton new_link = new LinkButton();

                        new_link.ID = "No File";
                        new_link.Text = "No File";
                        new_link.Enabled = false;
                        grd_row.Cells[5].Controls.Add(new_link);
                    }
                    else
                    {

                        obj_grdview.DataSource = ds.Tables[0];
                        obj_grdview.DataBind();

                        foreach (GridViewRow grd_view_row in obj_grdview.Rows)
                        {

                            grd_view_row.Cells[0].Visible = false;
                            grd_view_row.Cells[2].Visible = false;
                            LinkButton new_link1 = new LinkButton();

                            new_link1.ID = "upload_" + grd_view_row.Cells[1].Text;
                            new_link1.CommandName = "download";
                            new_link1.Text = "Download";
                            new_link1.CommandArgument = grd_view_row.Cells[2].Text + "/" + grd_view_row.Cells[1].Text;
                            new_link1.Click += New_link1_Click;

                            grd_view_row.Cells[3].Controls.Add(new_link1);

                            LinkButton new_link2 = new LinkButton();

                            new_link2.ID = "delete_" + grd_view_row.Cells[1].Text;
                            new_link2.CommandName = grd_view_row.Cells[0].Text;
                            new_link2.Text = "Delete";
                            new_link2.CommandArgument = grd_view_row.Cells[2].Text + "/" + grd_view_row.Cells[1].Text;
                            new_link2.Click += New_link2_Click;

                            grd_view_row.Cells[4].Controls.Add(new_link2);


                        }

                        obj_grdview.HeaderRow.Visible = false;


                        grd_row.Cells[5].Controls.Add(obj_grdview);
                    }


                }

            }
        }

        private void New_link2_Click(object sender, EventArgs e)
        {
            LinkButton new_link = (LinkButton)sender;

            int file_count = getdata.DeleteUploadedDoc(Convert.ToInt32(new_link.CommandName));

            if (file_count != 0)
            {
                //string fileName = Server.MapPath(new_link.CommandArgument);

                //if (fileName != null || fileName != string.Empty)
                //{
                //    if ((System.IO.File.Exists(fileName)))
                //    {
                //        System.IO.File.Delete(fileName);
                //    }
                //}
            }

            if (Request.QueryString["Issue_Uid"] != null)
            {
                IssueBind();
                BindIssueStatus();
            }
        }


        private void New_link1_Click(object sender, EventArgs e)
        {
            // for downloading a doc, click on its name, it will be downloaded to system download folder.
            // here decryption is not taking place and while uploading doc encryption is also not taking place.

            LinkButton new_link = (LinkButton)sender;

            //  string path = Server.MapPath(new_link.CommandArgument);
            //  System.IO.FileInfo file = new System.IO.FileInfo(path);

            string path = Server.MapPath(new_link.CommandArgument);

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

                Response.TransmitFile(outPath);

                Response.End();

            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('File does not exist.');</script>");
            }
        }

        protected void GrdIssueStatus_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string IssueStatusUID = e.CommandArgument.ToString();
            if (e.CommandName == "download")
            {
                DataSet ds = getdata.GetIssueStatus_by_IssueRemarksUID(new Guid(IssueStatusUID));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string path = Server.MapPath(ds.Tables[0].Rows[0]["Issue_Document"].ToString());

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

                        Response.WriteFile(file.FullName);

                        Response.End();

                    }

                    else
                    {

                        //Response.Write("This file does not exist.");
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('File does not exist.');</script>");

                    }
                }
            }

            if (e.CommandName == "delete")
            {
                int cnt = getdata.Issues_Remarks_Delete(new Guid(IssueStatusUID), new Guid(Session["UserUID"].ToString()));
                if (cnt > 0)
                {
                    BindIssueStatus();
                }
            }
        }

        protected void GrdIssueStatus_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void GrdIssueStatus_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdIssueStatus.PageIndex = e.NewPageIndex;
            BindIssueStatus();
        }
    }
}