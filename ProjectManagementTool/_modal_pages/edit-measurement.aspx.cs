using Newtonsoft.Json;
using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagementTool._modal_pages
{
    public partial class edit_measurement : System.Web.UI.Page
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
                    if (Request.QueryString["UID"] != null)
                    {
                        BindMeasurement(Request.QueryString["UID"].ToString());
                    }
                }
            }
        }

        private void BindMeasurement(string UID)
        {
            DataSet ds = getdata.GetMeasurementBook_By_UID(new Guid(UID));
            if (ds.Tables[0].Rows.Count > 0)
            {
                HiddenTaskID.Value = ds.Tables[0].Rows[0]["TaskUID"].ToString();
                LblUnitforProgress.Text = ds.Tables[0].Rows[0]["UnitforProgress"].ToString();
                txtquantity.Text= ds.Tables[0].Rows[0]["Quantity"].ToString();
                txtremarks.Text = ds.Tables[0].Rows[0]["Remarks"].ToString();
                if (ds.Tables[0].Rows[0]["CreatedDate"].ToString() != "")
                {
                    dtMeasurementDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["CreatedDate"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                string sDate1 = "";
                DateTime CDate1 = DateTime.Now;

                sDate1 = dtMeasurementDate.Text;
                sDate1 = getdata.ConvertDateFormat(sDate1);
                CDate1 = Convert.ToDateTime(sDate1);

                string TaskUID = HiddenTaskID.Value;
                Guid MeasurementUID = new Guid(Request.QueryString["UID"].ToString());
                int rs = getdata.MeasurementBookUpdate(MeasurementUID, new Guid(TaskUID), LblUnitforProgress.Text, txtquantity.Text, "", CDate1, "", new Guid(Session["UserUID"].ToString()), txtremarks.Text);
                if (rs > 0)
                {
                    if (WebConfigurationManager.AppSettings["Dbsync"] == "Yes")
                    {
                        string WebAPIURL = WebConfigurationManager.AppSettings["DbsyncWebApiURL"];
                        WebAPIURL = WebAPIURL + "Activity/MeasurementBookEdit";

                        string postData = "MeasurementUID=" + MeasurementUID + "&Quantity=" + txtquantity.Text + "&Remarks=" + txtremarks.Text + "&UserEmail=" + getdata.GetUserEmail_By_UserUID_New(new Guid(Session["UserUID"].ToString())) + "&MeasurementDate=" + CDate1;
                        string sReturnStatus = getdata.webPostMethod(postData, WebAPIURL);
                        if (!sReturnStatus.StartsWith("Error:"))
                        {
                            dynamic DynamicData = JsonConvert.DeserializeObject(sReturnStatus);
                            string RetStatus = DynamicData.Status;
                            if (!RetStatus.StartsWith("Error:"))
                            {
                                int rCnt = getdata.ServerFlagsUpdate(MeasurementUID.ToString(), 2, "MeasurementBook", "Y", "UID");
                            }
                            else
                            {
                                string ErrorMessage = DynamicData.Message;
                                WebAPIStatusInsert(Guid.NewGuid(), WebAPIURL, postData, ErrorMessage, "Failure","Measurement Book Edit", "MeasurementBookEdit", MeasurementUID);
                                //Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error: DBSync =" + ErrorMessage + "');</script>");
                            }
                        }
                        else
                        {
                            WebAPIStatusInsert(Guid.NewGuid(), WebAPIURL, postData, sReturnStatus, "Failure", "Measurement Book Edit", "MeasurementBookEdit", MeasurementUID);
                        }
                    }
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code : EM-01. Description=" + ex.Message + "');</script>");
            }
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