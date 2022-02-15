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
    public partial class view_taskpaymenthistroy : System.Web.UI.Page
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
                        BindPaymentHistroy(Request.QueryString["TaskUID"].ToString());
                    }
                }
            }
        }

        private void BindPaymentHistroy(string TaskUID)
        {
            DataSet ds = getdata.GetFinance_MileStonesDetails_By_TaskUID(new Guid(TaskUID));
            GrdFinanceMileStone.DataSource = ds;
            GrdFinanceMileStone.DataBind();
        }

        protected void GrdFinanceMileStone_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton imgShowHide = (ImageButton)e.Row.FindControl("imgProductsShow");
                Show_Hide_ProductsGrid(imgShowHide, e);
                GridView grdpaymentupdate = (GridView)e.Row.FindControl("GrdPaymentUpdate");
                string Finance_MileStoneUID = GrdFinanceMileStone.DataKeys[e.Row.RowIndex].Values[0].ToString();
                DataSet ds = getdata.FinanceMileStonePaymentUpdate_Selectby_Finance_MileStoneUID(new Guid(Finance_MileStoneUID));
                grdpaymentupdate.DataSource = ds;
                grdpaymentupdate.DataBind();
            }
        }
        protected void Show_Hide_ProductsGrid(object sender, EventArgs e)
        {
            ImageButton imgShowHide = (sender as ImageButton);
            GridViewRow row = (imgShowHide.NamingContainer as GridViewRow);
            if (imgShowHide.CommandArgument == "Show")
            {
                row.FindControl("pnlPaymentHistory").Visible = true;
                imgShowHide.CommandArgument = "Hide";
                imgShowHide.ImageUrl = "~/_assets/images/minus.png";
            }
            else
            {
                row.FindControl("pnlPaymentHistory").Visible = false;
                imgShowHide.CommandArgument = "Show";
                imgShowHide.ImageUrl = "~/_assets/images/plus.png";
            }
        }
        protected void GrdPaymentUpdate_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                //e.Row.Cells[0].Visible = false;
                //e.Row.Cells[0].Attributes.Add("colspan", "4");
            }
        }
    }
}