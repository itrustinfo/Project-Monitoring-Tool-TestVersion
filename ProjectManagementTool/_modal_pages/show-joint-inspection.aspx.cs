using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Configuration;
using System.IO;
using Ionic.Zip;

namespace ProjectManagementTool._modal_pages
{
    public partial class show_joint_inspection : System.Web.UI.Page
    {
        DBGetData dbgetdata = new DBGetData();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Username"] == null)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
            }
            else
            {
                if (!IsPostBack)
                {
                   
                    if (Request.QueryString["boqUid"] != null)
                    {
                        HideActionButtons();
                        BindInspectionReports();
                        AddJointInspection.HRef = "/_modal_pages/add-jointinspection.aspx?BOQUID=" + Request.QueryString["boqUid"] + "&ProjectUID=" + Request.QueryString["ProjectUID"] + "&WorkpackageUID=" + Request.QueryString["WorkpackageUID"];
                    }
                    //if (Request.QueryString["View"] != null)
                    //{
                    //    additems.Visible = false;
                    //}
                    //else
                    //{
                    //    additems.Visible = true;
                    //}
                }

            }
        }

        private void BindInspectionReports()
        {
            try
            {
                DataTable dtInspectionReports = dbgetdata.getInspectionReports(Request.QueryString["boqUid"].ToString());
                grdinspectionReport.DataSource = dtInspectionReports;
                grdinspectionReport.DataBind();
                
            }
            catch(Exception ex)
            {

            }
        }

        internal void HideActionButtons()
        {
            AddJointInspection.Visible = false;
            ViewState["isEdit"] = "false";
            ViewState["isDelete"] = "false";
            grdinspectionReport.Columns[5].Visible = false;
            grdinspectionReport.Columns[6].Visible = false;
            DataSet dscheck = new DataSet();
            dscheck = dbgetdata.GetUsertypeFunctionality_Mapping(Session["TypeOfUser"].ToString());
            if (dscheck.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dscheck.Tables[0].Rows)
                {
                    if (dr["Code"].ToString() == "JIA")
                    {
                        AddJointInspection.Visible = true;
                    }

                    if (dr["Code"].ToString() == "JIE")
                    {
                        grdinspectionReport.Columns[5].Visible = true;
                        ViewState["isEdit"] = "true";
                    }

                    if (dr["Code"].ToString() == "JID")
                    {
                        grdinspectionReport.Columns[6].Visible = true;
                        ViewState["isDelete"] = "true";
                    }
                }
            }
        }

        protected void grdRaBills_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdinspectionReport.PageIndex = e.NewPageIndex;
            BindInspectionReports();
        }

        protected void grdRaBills_RowEditing(object sender, GridViewEditEventArgs e)
        {
            grdinspectionReport.EditIndex = e.NewEditIndex;
            BindInspectionReports();
        }

        protected void grdRaBills_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            grdinspectionReport.EditIndex = -1;
            BindInspectionReports();

        }

        protected void grdRaBills_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            //Finding the controls from Gridview for the row which is going to update  
            HiddenField itemUId = grdinspectionReport.Rows[e.RowIndex].FindControl("hidUid") as HiddenField;
           
            TextBox txtquantity = grdinspectionReport.Rows[e.RowIndex].FindControl("txtQuantity") as TextBox;
            TextBox txtUnit = grdinspectionReport.Rows[e.RowIndex].FindControl("txtUnits") as TextBox;
            if (double.TryParse(txtquantity.Text, out double quantity))
            {
               
                dbgetdata.UpdateJointInspectionReport(itemUId.Value, quantity,txtUnit.Text);
            }
            grdinspectionReport.EditIndex = -1;
            BindInspectionReports();
        }

        //protected void btnAdd_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (Request.QueryString["boqUid"] != null)
        //        {
        //            int cnt = dbgetdata.InsertjointInspection(Request.QueryString["boqUid"], txtdiapipe.Text, txtunit.Text,txtinvoiceNumber.Text, txtDate.Text, txtQuantity.Text);
        //            if (cnt > 0)
        //            {
        //                BindInspectionReports();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

        protected void grdinspectionReport_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            HiddenField hidInspectionUid = grdinspectionReport.Rows[e.RowIndex].FindControl("hidUid") as HiddenField;
            dbgetdata.deleteInspectionReport(hidInspectionUid.Value);
            BindInspectionReports();
        }

        protected void grdinspectionReport_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (ViewState["isEdit"].ToString() == "false")
                {
                    e.Row.Cells[5].Visible = false;
                }
                if (ViewState["isDelete"].ToString() == "false")
                {
                    e.Row.Cells[6].Visible = false;
                }
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Check for Documents Count
                string inspectionUid= grdinspectionReport.DataKeys[e.Row.RowIndex].Values[0].ToString();
                int cnt = dbgetdata.GetJointInspectionDocuments_Count_by_inspectionUid(new Guid(inspectionUid));
                LinkButton LnkDownloadnew = (LinkButton)e.Row.FindControl("LnkDownloadnew");
                if (cnt > 0)
                {
                    LnkDownloadnew.Enabled = true;
                }
                else
                {
                    LnkDownloadnew.Enabled = false;
                }

                if (ViewState["isEdit"].ToString() == "false")
                {
                    e.Row.Cells[5].Visible = false;
                }
                if (ViewState["isDelete"].ToString() == "false")
                {
                    e.Row.Cells[6].Visible = false;
                }

                if (WebConfigurationManager.AppSettings["Dbsync"] == "Yes")
                {
                    if (e.Row.Cells[7].Text != "")
                    {
                        if (e.Row.Cells[7].Text == "N" || e.Row.Cells[8].Text == "N")
                        {
                            e.Row.BackColor = System.Drawing.Color.White;
                        }
                        else
                        {
                            e.Row.BackColor = System.Drawing.Color.White;
                            //e.Row.ForeColor = System.Drawing.Color.White;
                        }

                    }
                }
            }
        }

        protected void grdinspectionReport_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string UID = e.CommandArgument.ToString();
            if (e.CommandName == "download")
            {
                DataSet ds = dbgetdata.GetJointInspectionDocuments_by_inspectionUid(new Guid(UID));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    
                    if (ds.Tables[0].Rows.Count == 1)
                    {
                        string path = ds.Tables[0].Rows[0]["InspectionDocument_FilePath"].ToString();
                        string getExtension = System.IO.Path.GetExtension(path);
                        string outPath = path.Replace(getExtension, "") + "_download" + getExtension;
                        dbgetdata.DecryptFile(path, outPath);
                        System.IO.FileInfo file = new System.IO.FileInfo(outPath);

                        if (file.Exists)
                        {

                            Response.Clear();

                            Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);

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
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('File does not exists.');</script>");

                        }
                    }
                    else
                    {
                        
                        string ZipFileDir = Server.MapPath("~/ZipFiles/" + DateTime.Now.Ticks);

                        if (!Directory.Exists(ZipFileDir))
                        {
                            Directory.CreateDirectory(ZipFileDir);
                        }

                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            string path = ds.Tables[0].Rows[i]["InspectionDocument_FilePath"].ToString();
                            string sFileName = Path.GetFileName(path);
                            string getExtension = System.IO.Path.GetExtension(path);
                            string outPath = path.Replace(getExtension, "") + "_download" + getExtension;
                            dbgetdata.DecryptFile(path, outPath);
                            File.Copy(outPath, ZipFileDir + "/" + sFileName);
                        }

                        string[] Filenames = Directory.GetFiles(ZipFileDir);
                        using (ZipFile zip = new ZipFile())
                        {
                            zip.AddFiles(Filenames, "InspectionReport"); //Location for inside InspectionReport Folder
                            zip.Save(Server.MapPath("~/ZipFiles/InspectionReport.zip"));//location and name for creating zip file  
                        }

                        System.IO.FileInfo file = new System.IO.FileInfo(Server.MapPath("~/ZipFiles/InspectionReport.zip"));

                        if (file.Exists)
                        {

                            Response.Clear();

                            Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);

                            Response.AddHeader("Content-Length", file.Length.ToString());

                            Response.ContentType = "application/octet-stream";

                            Response.WriteFile(file.FullName);

                            Response.Flush();

                            try
                            {
                                if (File.Exists(Server.MapPath("~/ZipFiles/InspectionReport.zip")))
                                {
                                    File.Delete(Server.MapPath("~/ZipFiles/InspectionReport.zip"));
                                }
                                DeleteDirectory(ZipFileDir);
                            }
                            catch (Exception ex)
                            {
                                //throw
                            }

                            Response.End();
                        }
                        else
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('File does not exists.');</script>");

                        }
                    }
                }
            }
        }

        private void DeleteDirectory(string path)
        {
            if (Directory.Exists(path))
            {
                //Delete all files from the Directory
                foreach (string file in Directory.GetFiles(path))
                {
                    File.Delete(file);
                }
                //Delete a Directory
                Directory.Delete(path);
            }
        }
    }
}