using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagementTool._content_pages.resources
{
    public partial class _default : System.Web.UI.Page
    {
        DBGetData getdata = new DBGetData();
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
                    BindProject();
                    SelectedProjectWorkpackage("Project");
                    HLAdd.Visible = false;
                    DDlProject_SelectedIndexChanged(sender, e);
                    //if (Session["SelectedWorkpakage"] != null)
                    //{
                    //    DDlProject.SelectedValue = Session["SelectedWorkpakage"].ToString().Split('_')[0];
                    //    DDlProject_SelectedIndexChanged(sender, e);
                    //}
                }
            }
        }
        private void BindProject()
        {
            DataTable ds = new DataTable();
            if (Session["TypeOfUser"].ToString() == "U" || Session["TypeOfUser"].ToString() =="MD" || Session["TypeOfUser"].ToString() == "VP")
            {
                ds = gettk.GetProjects();
            }
            else if (Session["TypeOfUser"].ToString() == "PA")
            {
                //ds = gettk.GetProjects_by_UserUID(new Guid(Session["UserUID"].ToString()));
                ds = gettk.GetAssignedProjects_by_UserUID(new Guid(Session["UserUID"].ToString()));
            }
            else
            {
                //ds = gettk.GetProjects();
                ds = gettk.GetAssignedProjects_by_UserUID(new Guid(Session["UserUID"].ToString()));
            }
            DDlProject.DataTextField = "ProjectName";
            DDlProject.DataValueField = "ProjectUID";
            DDlProject.DataSource = ds;
            DDlProject.DataBind();
            DDlProject.Items.Insert(0, new ListItem("-- Select Project --", ""));
            DDLWorkPackage.Items.Insert(0, new ListItem("-- Select Workpackage --", ""));

            

        }

        protected void DDlProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDlProject.SelectedValue != "")
            {
                //DataSet ds = getdata.GetWorkPackages_By_ProjectUID(new Guid(DDlProject.SelectedValue));
                DataSet ds = new DataSet();
                if (Session["TypeOfUser"].ToString() == "U" || Session["TypeOfUser"].ToString() == "MD" || Session["TypeOfUser"].ToString() == "VP")
                {
                    ds = getdata.GetWorkPackages_By_ProjectUID(new Guid(DDlProject.SelectedValue));
                }
                else if (Session["TypeOfUser"].ToString() == "PA")
                {
                    ds = getdata.GetWorkPackages_ForUser_by_ProjectUID(new Guid(Session["UserUID"].ToString()), new Guid(DDlProject.SelectedValue));
                }
                else
                {
                    ds = getdata.GetWorkPackages_ForUser_by_ProjectUID(new Guid(Session["UserUID"].ToString()), new Guid(DDlProject.SelectedValue));
                }
                if (ds.Tables[0].Rows.Count > 0)
                {

                    DDLWorkPackage.DataTextField = "Name";
                    DDLWorkPackage.DataValueField = "WorkPackageUID";
                    DDLWorkPackage.DataSource = ds;
                    DDLWorkPackage.DataBind();

                    SelectedProjectWorkpackage("Workpackage");

                    //if (Session["SelectedWorkpakage"] != null)
                    //{
                    //    DDLWorkPackage.SelectedValue = Session["SelectedWorkpakage"].ToString().Split('_')[1];
                    //}
                    HLAdd.Visible = true;
                    BindResources();
                    HLAdd.HRef = "/_modal_pages/add-resourcemaster.aspx?ProjectUID=" + DDlProject.SelectedValue + "&WorkPackageUID=" + DDLWorkPackage.SelectedValue;
                    Session["Project_Workpackage"] = DDlProject.SelectedValue + "_" + DDLWorkPackage.SelectedValue;
                    DDLWorkPackage_SelectedIndexChanged(sender, e);
                    
                }
                else
                {
                    HLAdd.Visible = false;
                    DDLWorkPackage.DataSource = null;
                    DDLWorkPackage.DataBind();
                }
            }
        }
        private void BindResources()
        {
            //WorkPackageUID
            grdResources.DataSource = getdata.getResourceMaster(new Guid(DDLWorkPackage.SelectedValue));
            grdResources.DataBind();
        }
        protected void DDLWorkPackage_SelectedIndexChanged(object sender, EventArgs e)
        {
            //WorkPackageUID
            if (DDLWorkPackage.SelectedValue != "")
            {
                BindResources();
                HLAdd.HRef = "/_modal_pages/add-resourcemaster.aspx?ProjectUID=" + DDlProject.SelectedValue + "&WorkPackageUID=" + DDLWorkPackage.SelectedValue;
                Session["SelectedWorkpakage"] = DDlProject.SelectedValue + "_" + DDLWorkPackage.SelectedValue;
                Session["Project_Workpackage"] = DDlProject.SelectedValue + "_" + DDLWorkPackage.SelectedValue;
            }
        }

        internal void SelectedProjectWorkpackage(string pType)
        {
            if (!IsPostBack && Session["Project_Workpackage"] != null)
            {
                string[] selectedValue = Session["Project_Workpackage"].ToString().Split('_');
                if (selectedValue.Length > 1)
                {
                    if (pType == "Project")
                    {
                        DDlProject.SelectedValue = selectedValue[0];
                    }
                    else
                    {
                        DDLWorkPackage.SelectedValue = selectedValue[1];
                    }
                }
                else
                {
                    if (pType == "Project")
                    {
                        DDlProject.SelectedValue = Session["Project_Workpackage"].ToString();
                    }
                }
            }

        }

        protected void grdResources_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdResources.PageIndex = e.NewPageIndex;
            BindResources();

        }
    }
}