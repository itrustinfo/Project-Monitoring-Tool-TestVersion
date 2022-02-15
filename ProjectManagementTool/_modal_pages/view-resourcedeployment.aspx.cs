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
    public partial class view_resourcedeployment : System.Web.UI.Page
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
                    if (Request.QueryString["ReourceDeploymentUID"] != null)
                    {
                        BindResourceDeploymentUpdate(Request.QueryString["ReourceDeploymentUID"]);
                    }
                }
            }
        }

        internal void BindResourceDeploymentUpdate(string ReourceDeploymentUID)
        {
            DataSet ds = getdata.GetResourceDeploymentUpdate_by_ReourceDeploymentUID(new Guid(ReourceDeploymentUID));
            GrdResourceDeployment.DataSource = ds;
            GrdResourceDeployment.DataBind();
        }

        protected void GrdResourceDeployment_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string UID = e.CommandArgument.ToString();
            if (e.CommandName == "delete")
            {
                int cnt = getdata.ResourceDeploymentUpdate_Delete(new Guid(UID), new Guid(Session["UserUID"].ToString()));
                if (cnt > 0)
                {
                    if (WebConfigurationManager.AppSettings["Dbsync"] == "Yes")
                    {
                        string WebAPIURL = WebConfigurationManager.AppSettings["DbsyncWebApiURL"];
                        WebAPIURL = WebAPIURL + "Activity/ResourceDeploymentDelete";

                        string postData = "UID=" + UID + "&UserUID=" + Session["UserUID"].ToString();
                        string sReturnStatus = getdata.webPostMethod(postData, WebAPIURL);
                        if (!sReturnStatus.StartsWith("Error:"))
                        {
                            dynamic DynamicData = JsonConvert.DeserializeObject(sReturnStatus);
                            string RetStatus = DynamicData.Status;
                            if (!RetStatus.StartsWith("Error:"))
                            {
                                int rCnt = getdata.ServerFlagsUpdate(UID, 2, "ResourceDeploymentUpdate", "Y", "UID");
                            }
                            else
                            {
                                string ErrorMessage = DynamicData.Message;
                                WebAPIStatusInsert(Guid.NewGuid(), WebAPIURL, postData, ErrorMessage, "Failure", "Resource Deployment Update Delete", "ResourceDeploymentDelete", new Guid(UID));
                                //Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error: DBSync =" + ErrorMessage + "');</script>");
                            }
                        }
                        else
                        {
                            WebAPIStatusInsert(Guid.NewGuid(), WebAPIURL, postData, sReturnStatus, "Failure", "Resource Deployment Update Delete", "ResourceDeploymentDelete", new Guid(UID));
                        }
                    }

                    BindResourceDeploymentUpdate(Request.QueryString["ReourceDeploymentUID"]);
                }
            }
        }

        public string WebAPIStatusInsert(Guid WebAPIUID, string url, string WebAPIParameters, string WebAPI_Error, string WebAPIStatus, string WebAPIType, string WebAPIFunction,Guid WebAPI_PrimaryKey)
        {
            string Retval = "";

            int cnt = getdata.WebAPIStatusInsert(WebAPIUID, url, WebAPIParameters, WebAPI_Error, WebAPIStatus, WebAPIType, WebAPIFunction, WebAPI_PrimaryKey);
            if (cnt <= 0)
            {
                Retval = "Insertion Failed for WebAPIStaus table";
            }
            return Retval;
        }

        protected void GrdResourceDeployment_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void GrdResourceDeployment_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GrdResourceDeployment.EditIndex = -1;
            BindResourceDeploymentUpdate(Request.QueryString["ReourceDeploymentUID"]);
        }

        protected void GrdResourceDeployment_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GrdResourceDeployment.EditIndex = e.NewEditIndex;
            BindResourceDeploymentUpdate(Request.QueryString["ReourceDeploymentUID"]);
        }

        protected void GrdResourceDeployment_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            HiddenField hiddeployeduid = GrdResourceDeployment.Rows[e.RowIndex].FindControl("hiddeployeduid") as HiddenField;
            TextBox txtDeployed = GrdResourceDeployment.Rows[e.RowIndex].FindControl("txtDeployed") as TextBox;
            TextBox txtremarks = GrdResourceDeployment.Rows[e.RowIndex].FindControl("txtremarks") as TextBox;

            //Update code goes here
            int cnt = getdata.ResourceDeploymentUpdate_Edit(new Guid(hiddeployeduid.Value), float.Parse(txtDeployed.Text), txtremarks.Text);
            if (cnt > 0)
            {
                GrdResourceDeployment.EditIndex = -1;
                BindResourceDeploymentUpdate(Request.QueryString["ReourceDeploymentUID"]);
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('There is a problem with these feature. Please contact system admin.');</script>");
            }
        }

        protected void GrdResourceDeployment_RowDataBound(object sender, GridViewRowEventArgs e)
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
    }
}