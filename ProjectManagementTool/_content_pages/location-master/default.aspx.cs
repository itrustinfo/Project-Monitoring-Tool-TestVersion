using System;
using ProjectManager.DAL;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace ProjectManagementTool._content_pages.location_master
{
    public partial class _default : System.Web.UI.Page
    {
        GeneralDocuments GD = new GeneralDocuments();
        DBGetData getdt = new DBGetData();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Username"] == null)
            {
                Response.Redirect("~/Login.aspx");
            }
            else
            {
                if (!IsPostBack)
                {
                    BindLocation();
                    BindUserLocation();
                }
            }
        }

        private void BindLocation()
        {
            DataSet ds = GD.Location_Select();
            GrdLocation.DataSource = ds;
            GrdLocation.DataBind();
        }

        private void BindUserLocation()
        {
            DataSet ds = GD.Distinct_Users_SelectFromUserLocation();
            //DataSet ds = GD.UserLocation_Select();
            GrdAssignedLocation.DataSource = ds;
            GrdAssignedLocation.DataBind();
        }

        protected void GrdLocation_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdLocation.PageIndex = e.NewPageIndex;
            BindLocation();
        }

        protected void GrdLocation_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string UID = e.CommandArgument.ToString();
            if (e.CommandName == "delete")
            {
                int cnt = GD.Location_Delete(new Guid(UID), new Guid(Session["UserUID"].ToString()));
                if (cnt > 0)
                {
                    BindLocation();
                }
            }
        }

        protected void GrdLocation_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void GrdAssignedLocation_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdAssignedLocation.PageIndex = e.NewPageIndex;
            BindUserLocation();
        }

        protected void GrdAssignedLocation_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string UID = e.CommandArgument.ToString();
            if (e.CommandName == "delete")
            {
                int cnt = GD.UserLocation_Delete(new Guid(UID), new Guid(Session["UserUID"].ToString()));
                if (cnt > 0)
                {
                    BindUserLocation();
                }
            }
        }

        protected void GrdAssignedLocation_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        public string GetUserName(string UserUID)
        {
            return getdt.getUserNameby_UID(new Guid(UserUID));
        }

        protected void GrdAssignedLocation_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataList dl = (DataList)e.Row.FindControl("DL_Location");
                string UserUID= GrdAssignedLocation.DataKeys[e.Row.RowIndex].Values[0].ToString();
                DataSet ds = GD.UserLocation_SelectBy_UserUID(new Guid(UserUID));
                dl.DataSource = ds;
                dl.DataBind();
            }
        }

        public string GetLocationName(string LocationMaster_UID)
        {
            return GD.LocationName_by_LocationMaster_UID(new Guid(LocationMaster_UID));
        }
    }
}