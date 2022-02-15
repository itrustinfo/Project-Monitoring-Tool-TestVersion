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
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[3].Text == "&nbsp;")
                {
                    LinkButton lnk = (LinkButton)e.Row.FindControl("lnkdown");
                    lnk.Enabled = false;
                    lnk.Text = "No File";
                }
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