using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProjectManager.DAL;

namespace ProjectManagementTool._content_pages.dashboard_measurment
{
    public partial class _default : System.Web.UI.Page
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
                    if (Request.QueryString["WorkPackageUID"] != null)
                    {
                        BindMeasurementBook(Request.QueryString["WorkPackageUID"]);
                        lblTotalcount.Text = "Total Count : " + grdMeasurementbook.Rows.Count.ToString();
                    }
                }
            }
        }

        public string GetTaskName(string TaskUID)
        {
            return getdata.getTaskNameby_TaskUID(new Guid(TaskUID));
        }

        private void BindMeasurementBook(string WorkPackageUID)
        {

            try
            {
                //ActivityHeading.Text = "Task : " + TreeView1.SelectedNode.Text;
                DataSet ds = getdata.GetTaskMeasurementBookForDashboard(new Guid(WorkPackageUID));
                grdMeasurementbook.DataSource = ds;
                grdMeasurementbook.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        protected void grdMeasurementbook_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                

            }
        }
    }
}