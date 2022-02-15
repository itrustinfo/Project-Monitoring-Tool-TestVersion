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
    public partial class change_activityorder : System.Web.UI.Page
    {
        DBGetData dbgetdata = new DBGetData();
        TaskUpdate gettk = new TaskUpdate();
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
                    if (Request.QueryString["WorkPackage"] != null)
                    {
                        BindTaskbyWorkpackageUID(Request.QueryString["WorkPackage"]);
                    }
                    else if (Request.QueryString["TaskUID"] != null)
                    {
                        BindTaskbyParentTaskUID(Request.QueryString["TaskUID"]);
                    }
                    if (GrdTreeView.Rows.Count > 0)
                    {
                        LinkButton lnkUp = (GrdTreeView.Rows[0].FindControl("lnkUp") as LinkButton);
                        LinkButton lnkDown = (GrdTreeView.Rows[GrdTreeView.Rows.Count - 1].FindControl("lnkDown") as LinkButton);
                        lnkUp.Enabled = false;
                        lnkUp.CssClass = "disabled";
                        lnkDown.Enabled = false;
                        lnkDown.CssClass = "disabled";
                    }
                    
                }
            }
        }

        private void BindTaskbyWorkpackageUID(string WorkpackageUID)
        {
            DataSet ds = new DataSet();
            if (Request.QueryString["OptionUID"] != null)
            {
                ds = dbgetdata.GetTasks_by_WorkpackageOptionUID(new Guid(WorkpackageUID), new Guid(Request.QueryString["OptionUID"]));
            }
            else
            {
                ds = gettk.GetTasks_by_WorkPackage(WorkpackageUID);
            }
            GrdTreeView.DataSource = ds;
            GrdTreeView.DataBind();
        }

        private void BindTaskbyParentTaskUID(string ParentTaskUID)
        {
            DataSet ds = dbgetdata.GetTask_by_ParentTaskUID(new Guid(ParentTaskUID));
            GrdTreeView.DataSource = ds;
            GrdTreeView.DataBind();
        }
        public string ShoworHide(string Desc)
        {
            if (Desc.Length > 60)
            {
                return "More";
            }
            else
            {
                return string.Empty;
            }
        }

        protected void ChangePreference(object sender, EventArgs e)
        {

            LinkButton btnUp = (LinkButton)sender;
            GridViewRow row = (GridViewRow)btnUp.NamingContainer;
            // Get all items except the one selected  
            var rows = GrdTreeView.Rows.Cast<GridViewRow>().Where(a => a != row).ToList();
            switch (btnUp.CommandName)
            {
                case "Up":
                    //If First Item, insert at end (rotating positions)
                    if (row.RowIndex.Equals(0))
                        rows.Add(row);
                    else
                        rows.Insert(row.RowIndex - 1, row);
                    break;
                case "Down":
                    //If Last Item, insert at beginning (rotating positions)
                    if (row.RowIndex.Equals(GrdTreeView.Rows.Count - 1))
                        rows.Insert(0, row);
                    else
                        rows.Insert(row.RowIndex + 1, row);
                    break;
            }
            GrdTreeView.DataSource = rows.Select(a => new
            {
                Name = ((Label)a.FindControl("LblName")).Text,
                Description = ((Label)a.FindControl("LblDesc")).Text,
                StartDate = ((Label)a.FindControl("LblStartDate")).Text,
                PlannedEndDate = ((Label)a.FindControl("LblEndDate")).Text,
                TaskUID= ((Label)a.FindControl("lblTaskUID")).Text
            }).ToList();
            GrdTreeView.DataBind();
            
            LinkButton lnkUp = (GrdTreeView.Rows[0].FindControl("lnkUp") as LinkButton);
            LinkButton lnkDown = (GrdTreeView.Rows[GrdTreeView.Rows.Count - 1].FindControl("lnkDown") as LinkButton);
            lnkUp.Enabled = false;
            lnkUp.CssClass = "disabled";
            lnkDown.Enabled = false;
            lnkDown.CssClass = "disabled";
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                int Order = 1;
                foreach (GridViewRow row in GrdTreeView.Rows)
                {
                    Label lblTaskUID = row.FindControl("lblTaskUID") as Label;
                    string TaskUID = lblTaskUID.Text;

                    Label lblName = row.FindControl("LblName") as Label;
                    string Name = lblName.Text;


                    int Cnt = dbgetdata.Task_Order_Update_by_TaskUID(new Guid(lblTaskUID.Text), Order);
                    if (Cnt <= 0)
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code:AOU:01 there is a problem with these feature. Please contact system admin.');</script>");
                    }
                    Order = Order + 1;

                }
                if (Request.QueryString["WorkPackage"] != null)
                {
                    Session["SelectedActivity"] = Request.QueryString["WorkPackage"].ToString();
                }
                else
                {
                    Session["SelectedActivity"] = Request.QueryString["TaskUID"].ToString();
                }
                    

                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code:AOU:01 there is a problem with these feature. Please contact system admin.');</script>");
            }
        }
    }
}