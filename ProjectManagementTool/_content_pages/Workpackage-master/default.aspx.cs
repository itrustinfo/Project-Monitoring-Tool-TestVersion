using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagementTool._content_pages.masters
{
    public partial class _default : System.Web.UI.Page
    {
        DBGetData getdt = new DBGetData();
        TaskUpdate gettk = new TaskUpdate();
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
                    BindWorkpackageMasters();
                    //BindContractors();
                    //BindProjects();
                    BindLocationMasters();
                    BindClientMasters();
                    //ProjectClassBind();
                }
            }
        }

        protected override void InitializeCulture()
        {

            //CultureInfo ci = new CultureInfo("en-IN");
            // assign your custom Rupee symbol of your country
            //ci.NumberFormat.CurrencySymbol = "&#8377;";
            //Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture
                                                   //= ci;

            //CultureInfo ui = new CultureInfo("en-US");
            //// assign your custom Rupee symbol of your country
            ////ci.NumberFormat.CurrencySymbol = "&#8377;";
            //Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture
            //                                       = ui;

            //base.InitializeCulture();
        }

        //private void BindProjects()
        //{
        //    DataTable ds = new DataTable();
        //    if (Session["TypeOfUser"].ToString() == "U")
        //    {

        //        ds = gettk.GetProjects();
        //    }
        //    else if (Session["TypeOfUser"].ToString() == "PA")
        //    {

        //        //ds = gettk.GetProjects_by_UserUID(new Guid(Session["UserUID"].ToString()));
        //        ds = gettk.GetProjectsData_By_UserUID(new Guid(Session["UserUID"].ToString()));
        //    }
        //    else
        //    {

        //        ds = gettk.GetProjectsData_By_UserUID(new Guid(Session["UserUID"].ToString()));
        //    }
        //    GrdProject.DataSource = ds;
        //    GrdProject.DataBind();
        //}

        private void BindWorkpackageMasters()
        {
            DataSet ds = new DataSet();
            if (Session["TypeOfUser"].ToString() == "U" || Session["TypeOfUser"].ToString() =="MD" || Session["TypeOfUser"].ToString() == "VP")
            {
                ds = getdt.MasterWorkpackage_select_All();
            }
            else
            {
                ds= getdt.MasterWorkpackage_SelectBy_UserUID(new Guid(Session["UserUID"].ToString()));
            }
            grdMasters.DataSource = ds;
            grdMasters.DataBind();
        }

        private void BindLocationMasters()
        {
            DataSet ds = new DataSet();
            if (Session["TypeOfUser"].ToString() == "U" || Session["TypeOfUser"].ToString() =="MD" || Session["TypeOfUser"].ToString() == "VP")
            {
                ds = getdt.MasterLocation_Select_All();
            }
            else
            {
                ds = getdt.MasterLocation_SelectBy_UserUID(new Guid(Session["UserUID"].ToString()));
            }
            GrdLocationMaster.DataSource = ds;
            GrdLocationMaster.DataBind();
        }


        private void BindClientMasters()
        {
            DataSet ds = new DataSet();
            if (Session["TypeOfUser"].ToString() == "U" || Session["TypeOfUser"].ToString() =="MD" || Session["TypeOfUser"].ToString() == "VP")
            {
                ds = getdt.MasterClient_Select_All();
            }
            else
            {
                ds = getdt.MasterClient_SelectBy_UserUID(new Guid(Session["UserUID"].ToString()));
            }
            GrdClient.DataSource = ds;
            GrdClient.DataBind();
        }

        //private void BindContractors()
        //{
        //    GrdContractors.DataSource = getdt.GetContractors();
        //    GrdContractors.DataBind();
        //}
        protected void grdMasters_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdMasters.PageIndex = e.NewPageIndex;
            BindWorkpackageMasters();
        }

        protected void grdMasters_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string ID = e.CommandArgument.ToString();
            if (e.CommandName == "delete")
            {
                int cnt = getdt.MasterWorkpackage_Delete(new Guid(ID), new Guid(Session["UserUID"].ToString()));
                if (cnt > 0)
                {
                    BindWorkpackageMasters();
                }
            }
        }
        protected void grdMasters_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        public string GetProjectName(string ProjectUID)
        {
            return getdt.getProjectNameby_ProjectUID(new Guid(ProjectUID));
        }
        //protected void GrdContractors_PageIndexChanging(object sender, GridViewPageEventArgs e)
        //{
        //    GrdContractors.PageIndex = e.NewPageIndex;
        //    BindContractors();
        //}

        //protected void GrdContractors_RowCommand(object sender, GridViewCommandEventArgs e)
        //{
        //    string id = e.CommandArgument.ToString();
        //    if (e.CommandName == "delete")
        //    {
        //        BindContractors();
        //    }
        //}

        //protected void GrdContractors_RowDeleting(object sender, GridViewDeleteEventArgs e)
        //{

        //}

        //protected void GrdProject_PageIndexChanging(object sender, GridViewPageEventArgs e)
        //{
        //    GrdProject.PageIndex = e.NewPageIndex;
        //    BindProjects();
        //}

        protected void GrdLocationMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdLocationMaster.PageIndex = e.NewPageIndex;
            BindLocationMasters();
        }

        protected void GrdLocationMaster_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string ID = e.CommandArgument.ToString();
            if (e.CommandName == "delete")
            {
                int cnt = getdt.MasterLocation_Delete(new Guid(ID), new Guid(Session["UserUID"].ToString()));
                if (cnt > 0)
                {
                    BindLocationMasters();
                }
            }
        }

        protected void GrdLocationMaster_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void GrdClient_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdClient.PageIndex = e.NewPageIndex;
            BindClientMasters();
        }

        protected void GrdClient_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string ID = e.CommandArgument.ToString();
            if (e.CommandName == "delete")
            {
                int cnt = getdt.MasterClient_Delete(new Guid(ID), new Guid(Session["UserUID"].ToString()));
                if (cnt > 0)
                {
                    BindClientMasters();
                }
            }
        }
        protected void GrdClient_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }
        //private void ProjectClassBind()
        //{
        //    DataSet ds = getdt.ProjectClass_Select_All();
        //    GrdProjectClass.DataSource = ds;
        //    GrdProjectClass.DataBind();
        //}
        //protected void GrdProjectClass_PageIndexChanging(object sender, GridViewPageEventArgs e)
        //{
        //    GrdProjectClass.PageIndex = e.NewPageIndex;
        //    ProjectClassBind();
        //}
    }
}