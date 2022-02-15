using Newtonsoft.Json;
using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagementTool._modal_pages
{
    public partial class view_measurement : System.Web.UI.Page
    {
        DBGetData getdata = new DBGetData();

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
                    if (Request.QueryString["TaskUID"] != null)
                    {
                        BindMeasurementBook(Request.QueryString["TaskUID"]);
                    }
                }
            }
        }

        public string GetTaskName(string TaskUID)
        {
            return getdata.getTaskNameby_TaskUID(new Guid(TaskUID));
        }

        private void BindMeasurementBook(string TaskUID)
        {

            try
            {
                //ActivityHeading.Text = "Task : " + TreeView1.SelectedNode.Text;
                DataSet ds = getdata.GetTaskMeasurementBook(new Guid(TaskUID));
                grdMeasurementbook.DataSource = ds;
                grdMeasurementbook.DataBind();
            }
            catch (Exception ex)
            {

            }

        }

        protected void grdMeasurementbook_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (WebConfigurationManager.AppSettings["Dbsync"] == "Yes")
                {
                    if (e.Row.Cells[5].Text != "")
                    {
                        if (e.Row.Cells[5].Text == "N" || e.Row.Cells[6].Text == "N")
                        {
                            e.Row.BackColor = System.Drawing.Color.Orange;
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

        protected void grdMeasurementbook_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string UID = e.CommandArgument.ToString();
            if (e.CommandName == "delete")
            {
                int cnt = getdata.Measurement_Delete(new Guid(UID), new Guid(Session["UserUID"].ToString()));
                if (cnt > 0)
                {
                    if (WebConfigurationManager.AppSettings["Dbsync"] == "Yes")
                    {
                        string WebAPIURL = WebConfigurationManager.AppSettings["DbsyncWebApiURL"];
                        WebAPIURL = WebAPIURL + "Activity/TaskMeasurementDelete";

                        string postData = "MeasurementUID=" + UID + "&UserUID=" + Session["UserUID"].ToString();
                        string sReturnStatus = getdata.webPostMethod(postData, WebAPIURL);
                        if (!sReturnStatus.StartsWith("Error:"))
                        {
                            dynamic DynamicData = JsonConvert.DeserializeObject(sReturnStatus);
                            string RetStatus = DynamicData.Status;
                            if (!RetStatus.StartsWith("Error:"))
                            {
                                int rCnt = getdata.ServerFlagsUpdate(UID, 2, "MeasurementBook", "Y", "UID");
                            }
                            else
                            {
                                string ErrorMessage = DynamicData.Message;
                                WebAPIStatusInsert(Guid.NewGuid(), WebAPIURL, postData, ErrorMessage, "Failure", "Measurement Book Delete", "MeasurementDelete", new Guid(UID));
                                //Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error: DBSync =" + ErrorMessage + "');</script>");
                            }
                        }
                        else
                        {
                            WebAPIStatusInsert(Guid.NewGuid(), WebAPIURL, postData, sReturnStatus, "Failure", "Measurement Book Delete", "MeasurementDelete", new Guid(UID));
                        }
                    }
                    BindMeasurementBook(Request.QueryString["TaskUID"]);
                }
            }
        }

        protected void grdMeasurementbook_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            
        }

        public string WebAPIStatusInsert(Guid WebAPIUID, string url, string WebAPIParameters, string WebAPI_Error, string WebAPIStatus,string WebAPIType,string WebAPIFunction,Guid WebAPI_PrimaryKey)
        {
            string Retval = "";

            int cnt = getdata.WebAPIStatusInsert(WebAPIUID, url, WebAPIParameters, WebAPI_Error, WebAPIStatus, WebAPIType, WebAPIFunction, WebAPI_PrimaryKey);
            if (cnt <= 0)
            {
                Retval = "Insertion Failed for WebAPIStaus table";
            }
            return Retval;
        }
    }
}