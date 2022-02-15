using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagementTool._content_pages.reviews
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
                    BindProject();
                    SelectedProjectWorkpackage("Project");
                    DDlProject_SelectedIndexChanged(sender, e);
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

        }

        private void BindReviews()
        {
            GrdReviews.DataSource = getdt.getReviewList_by_WorkPackageUID(new Guid(DDLWorkPackage.SelectedValue));
            GrdReviews.DataBind();
        }

        private void BindUserReviews()
        {
            GrdReviews.DataSource = getdt.getReviewList_by_User_UID(new Guid(Session["UserUID"].ToString()));
            GrdReviews.DataBind();
        }

        protected void DDLWorkPackage_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDLWorkPackage.SelectedValue != "")
            {
                if (Session["TypeOfUser"].ToString() == "U" || Session["TypeOfUser"].ToString() == "PA")
                {
                    AddReviews.Visible = true;
                    BindReviews();
                }
                else
                {
                    BindUserReviews();
                    AddReviews.Visible = false;
                }
                Session["Project_Workpackage"] = DDlProject.SelectedValue + "_" + DDLWorkPackage.SelectedValue;
            }
           
        }

        internal void SelectedProjectWorkpackage(string pType)
        {
            if (!IsPostBack)
            {
                if (Session["Project_Workpackage"] != null)
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

        }

        protected void GrdReviews_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdReviews.PageIndex = e.NewPageIndex;
            if (Session["TypeOfUser"].ToString() == "U" || Session["TypeOfUser"].ToString() == "PA")
            {
                AddReviews.Visible = true;
                BindReviews();
            }
            else
            {
                BindUserReviews();
                AddReviews.Visible = false;
            }
        }

        public string GetUserName(string UserUID)
        {
            return getdt.getUserNameby_UID(new Guid(UserUID));
        }

        protected void DDlProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDlProject.SelectedValue != "")
            {
                //DataSet ds = getdt.GetWorkPackages_By_ProjectUID(new Guid(DDlProject.SelectedValue));
                DataSet ds = new DataSet();
                if (Session["TypeOfUser"].ToString() == "U" || Session["TypeOfUser"].ToString() == "MD" || Session["TypeOfUser"].ToString() == "VP")
                {
                    ds = getdt.GetWorkPackages_By_ProjectUID(new Guid(DDlProject.SelectedValue));
                }
                else if (Session["TypeOfUser"].ToString() == "PA")
                {
                    ds = getdt.GetWorkPackages_ForUser_by_ProjectUID(new Guid(Session["UserUID"].ToString()), new Guid(DDlProject.SelectedValue));
                }
                else
                {
                    ds = getdt.GetWorkPackages_ForUser_by_ProjectUID(new Guid(Session["UserUID"].ToString()), new Guid(DDlProject.SelectedValue));
                }
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DDLWorkPackage.DataTextField = "Name";
                    DDLWorkPackage.DataValueField = "WorkPackageUID";
                    DDLWorkPackage.DataSource = ds;
                    DDLWorkPackage.DataBind();

                    SelectedProjectWorkpackage("Workpackage");

                    if (Session["TypeOfUser"].ToString() == "U" || Session["TypeOfUser"].ToString() == "PA")
                    {
                        AddReviews.Visible = true;
                        BindReviews();
                    }
                    else
                    {
                        //BindUserReviews();
                        BindReviews();
                        AddReviews.Visible = false;
                    }
                    Session["Project_Workpackage"] = DDlProject.SelectedValue + "_" + DDLWorkPackage.SelectedValue;
                }
            }
            
        }

        protected void GrdReviews_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string ReviewUID = e.CommandArgument.ToString();
            if (e.CommandName == "delete")
            {
                int cnt = getdt.Review_Delete(new Guid(ReviewUID), new Guid(Session["UserUID"].ToString()));
                if (cnt > 0)
                {
                    if (Session["TypeOfUser"].ToString() == "U" || Session["TypeOfUser"].ToString() == "PA")
                    {
                        AddReviews.Visible = true;
                        BindReviews();
                    }
                    else
                    {
                        BindUserReviews();
                        AddReviews.Visible = false;
                    }
                }
            }
        }

        protected void GrdReviews_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }
    }
}