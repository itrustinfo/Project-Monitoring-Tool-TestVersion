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
    public partial class task_schedulehistory : System.Web.UI.Page
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
                        Session["SelectedActivity"] = Request.QueryString["TaskUID"].ToString();
                        LoadTaskSchedule(Request.QueryString["TaskUID"]);
                    }
                }
            }
        }

        private void LoadTaskSchedule(string TaskUID)
        {
            DataSet ds = getdata.GetTaskSchedule_By_TaskUID(new Guid(TaskUID));
            GrdTaskSchedule.DataSource = ds;
            GrdTaskSchedule.DataBind();
        }

        protected void GrdTaskSchedule_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string TaskScheduleUID = GrdTaskSchedule.DataKeys[e.Row.RowIndex].Values[0].ToString();

                DataSet ds = getdata.GetTaskSchedule_by_TaskScheduleUID(new Guid(TaskScheduleUID));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    Label LblAchieved = (Label)e.Row.FindControl("LblAchieved");
                    if (ds.Tables[0].Rows[0]["Achieved_Value"].ToString() != "")
                    {
                        LblAchieved.Text = ds.Tables[0].Rows[0]["Achieved_Value"].ToString();
                    }
                    else
                    {
                        LblAchieved.Text = "-";
                    }
                }
            }
        }
    }
}