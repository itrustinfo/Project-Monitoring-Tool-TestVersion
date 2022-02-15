using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagementTool._content_pages.document_history
{
    public partial class _default : System.Web.UI.Page
    {
        DBGetData getdt = new DBGetData();
        TaskUpdate gettk = new TaskUpdate();
        GeneralDocuments GD = new GeneralDocuments();
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
                    DivActualDocuments.Visible = false;
                    DivGeneralDocuments.Visible = false;
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
                ds = gettk.GetAssignedProjects_by_UserUID(new Guid(Session["UserUID"].ToString()));
            }
            else
            {
                ds = gettk.GetAssignedProjects_by_UserUID(new Guid(Session["UserUID"].ToString()));
            }

            DDlProject.DataTextField = "ProjectName";
            DDlProject.DataValueField = "ProjectUID";
            DDlProject.DataSource = ds;
            DDlProject.DataBind();
            DDlProject.Items.Insert(0, new ListItem("--Select Project--", ""));
            DDlProject.Items.Add("General Documents");

        }
        protected void DDlProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDlProject.SelectedValue != "")
            {
                //DataSet ds = getdt.GetWorkPackages_By_ProjectUID(new Guid(DDlProject.SelectedValue));
                if (DDlProject.SelectedItem.ToString() == "General Documents")
                {
                    WorkPackageDropdown.Visible = false;
                    DivActualDocuments.Visible = false;
                    DivGeneralDocuments.Visible = true;
                    BindGeneralDocuments();
                }
                else
                {
                    WorkPackageDropdown.Visible = true;
                    //DivActualDocuments.Visible = true;
                    //DivGeneralDocuments.Visible = false;
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
                        DDLWorkPackage.Items.Insert(0, new ListItem("--Select Workpackage--", ""));
                        SelectedProjectWorkpackage("Workpackage");

                        if (Session["Project_Workpackage"] != null)
                        {
                            DDLWorkPackage_SelectedIndexChanged(sender, e);
                        }
                        Session["Project_Workpackage"] = DDlProject.SelectedValue + "_" + DDLWorkPackage.SelectedValue;
                    }
                }
            }
            else
            {
                DDLWorkPackage.Items.Insert(0, new ListItem("--Select Workpackage--", ""));
            }
        }
        protected void DDLWorkPackage_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDLWorkPackage.SelectedValue != "")
            {
                DivGeneralDocuments.Visible = false;
                DivActualDocuments.Visible = true;
                BindDocumentHistoryBy_WorkpackgeUID(DDLWorkPackage.SelectedValue);
                Session["Project_Workpackage"] = DDlProject.SelectedValue + "_" + DDLWorkPackage.SelectedValue;
            }
        }

        private void SelectedProjectWorkpackage(string pType)
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

        private void BindDocumentHistoryBy_WorkpackgeUID(string WorkPackgeUID)
        {
            DataSet ds = getdt.GetDocumentHistoryBy_WorkpackgeUID(new Guid(WorkPackgeUID));
            GrdDcumentHsitroy.DataSource = ds;
            GrdDcumentHsitroy.DataBind();
        }
        private void BindGeneralDocuments()
        {
            DataSet ds = getdt.GetGeneralDocumentHistory();
            GrdGeneralDocuments.DataSource = ds;
            GrdGeneralDocuments.DataBind();
        }
        public string GetActivityName(string ActivityUID)
        {
            if (ActivityUID != Guid.Empty.ToString())
            {
                return getdt.getTaskNameby_TaskUID(new Guid(ActivityUID));
            }
            else
            {
                return getdt.getWorkPackageNameby_WorkPackageUID(new Guid(DDLWorkPackage.SelectedValue));
            }
            //return getdt.getWorkPackageNameby_WorkPackageUID(new Guid(DocumentUID));
        }
        public string GetDocumentname(string DocumentUID)
        {
            return getdt.ActualDocumentName_By_ActualDocumentUID(new Guid(DocumentUID));
        }
        protected void GrdDcumentHsitroy_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdDcumentHsitroy.PageIndex = e.NewPageIndex;
            BindDocumentHistoryBy_WorkpackgeUID(DDLWorkPackage.SelectedValue);
        }

        public string GetGeneraDocumentName(string DocumentUID)
        {
            return GD.GetGeneralDocumentNameByUID(new Guid(DocumentUID));
        }
        protected void GrdDcumentHsitroy_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[2].Text != "&nbsp;")
                {
                    DataSet ds = getdt.GetDoucmentHistory_by_DoucmentUID(new Guid(e.Row.Cells[2].Text));
                    if (ds != null)
                    {
                        e.Row.Cells[2].Text = ds.Tables[0].Rows[0]["Downloaded"].ToString() != "" ? ds.Tables[0].Rows[0]["Downloaded"].ToString() : "0";
                        e.Row.Cells[3].Text = ds.Tables[0].Rows[0]["Viewed"].ToString() != "" ? ds.Tables[0].Rows[0]["Viewed"].ToString() : "0";
                    }
                }
            }
        }

        protected void GrdGeneralDocuments_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdGeneralDocuments.PageIndex = e.NewPageIndex;
            BindGeneralDocuments();
        }

        protected void GrdGeneralDocuments_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[1].Text != "&nbsp;")
                {
                    DataSet ds = getdt.GetDoucmentHistory_by_DoucmentUID(new Guid(e.Row.Cells[1].Text));
                    if (ds != null)
                    {
                        e.Row.Cells[1].Text = ds.Tables[0].Rows[0]["Downloaded"].ToString() != "" ? ds.Tables[0].Rows[0]["Downloaded"].ToString() : "0";
                        e.Row.Cells[2].Text = ds.Tables[0].Rows[0]["Viewed"].ToString() != "" ? ds.Tables[0].Rows[0]["Viewed"].ToString() : "0";
                    }
                }
            }
        }
    }
}