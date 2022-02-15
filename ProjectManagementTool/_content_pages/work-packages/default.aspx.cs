using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace ProjectManagementTool._content_pages.work_packages
{
    public partial class _default : System.Web.UI.Page
    {
        DBGetData getdt = new DBGetData();
        DataSet ds = new DataSet();
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
                    BindWorkPackages();
                    HideActions();
                }
            }
        }

        private void BindWorkPackages()
        {
            if (DDLProject.SelectedValue != "")
            {
                DataSet dsworkPackage = new DataSet();
                if (Session["TypeOfUser"].ToString() == "U" || Session["TypeOfUser"].ToString() == "MD" || Session["TypeOfUser"].ToString() == "VP")
                {
                    dsworkPackage = getdt.GetWorkPackages_By_ProjectUID(new Guid(DDLProject.SelectedValue));
                }
                else if (Session["TypeOfUser"].ToString() == "PA")
                {
                    dsworkPackage = getdt.GetWorkPackages_ForUser_by_ProjectUID(new Guid(Session["UserUID"].ToString()), new Guid(DDLProject.SelectedValue));
                }
                else
                {
                    dsworkPackage = getdt.GetWorkPackages_ForUser_by_ProjectUID(new Guid(Session["UserUID"].ToString()), new Guid(DDLProject.SelectedValue));
                }

                //GrdWorkPackage.DataSource = getdt.GetWorkPackages_By_ProjectUID(new Guid(DDLProject.SelectedValue));
                GrdWorkPackage.DataSource = dsworkPackage;
                GrdWorkPackage.DataBind();

                if (GrdWorkPackage.Rows.Count > 0)
                {
                    LinkButton lnkUp = (GrdWorkPackage.Rows[0].FindControl("lnkUp") as LinkButton);
                    LinkButton lnkDown = (GrdWorkPackage.Rows[GrdWorkPackage.Rows.Count - 1].FindControl("lnkDown") as LinkButton);
                    lnkUp.Enabled = false;
                    lnkUp.CssClass = "disabled";
                    lnkDown.Enabled = false;
                    lnkDown.CssClass = "disabled";
                }
            }
        }

        private void HideActions()
        {
            if (Session["TypeOfUser"].ToString() == "U" || Session["TypeOfUser"].ToString() == "PA")
            {
                Workpkg.Visible = true;
                GrdWorkPackage.Columns[8].Visible = true;
                GrdWorkPackage.Columns[9].Visible = true;
                GrdWorkPackage.Columns[10].Visible = true;
                btnaddmail.Visible = true;
            }
            else
            {
                Workpkg.Visible = false;
                GrdWorkPackage.Columns[8].Visible = false;
                GrdWorkPackage.Columns[9].Visible = false;
                GrdWorkPackage.Columns[10].Visible = false;
                btnaddmail.Visible = false;
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
                ds = gettk.GetAssignedProjects_by_UserUID(new Guid(Session["UserUID"].ToString()));
            }
            DDLProject.DataTextField = "ProjectName";
            DDLProject.DataValueField = "ProjectUID";
            DDLProject.DataSource = ds;
            DDLProject.DataBind();
            SelectedProject();
            if (ds.Rows.Count > 0)
            {
                Workpkg.HRef = "/_modal_pages/add-workpackage.aspx?ProjectUID=" + DDLProject.SelectedValue;
            }
            
        }

        protected void DDLProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindWorkPackages();
            Workpkg.HRef = "/_modal_pages/add-workpackage.aspx?ProjectUID=" + DDLProject.SelectedValue;
            Session["Project_Workpackage"] = DDLProject.SelectedValue;
        }

        internal void SelectedProject()
        {
            if (!IsPostBack)
            {
                if (Session["Project_Workpackage"] != null)
                {
                    string[] selectedValue = Session["Project_Workpackage"].ToString().Split('_');
                    if (selectedValue.Length > 1)
                    {
                        DDLProject.SelectedValue = selectedValue[0];
                    }
                    else
                    {
                        DDLProject.SelectedValue = Session["Project_Workpackage"].ToString();
                    }

                }
            }
                
        }

        public string GetProjectName(string ProjectUID)
        {
            return getdt.getProjectNameby_ProjectUID(new Guid(ProjectUID));
        }

        protected void GrdWorkPackage_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdWorkPackage.PageIndex = e.NewPageIndex;
            BindWorkPackages();
        }

        public string GetStatus(string sts)
        {
            if (sts == "I")
            {
                return "In-Progress";
            }
            else if (sts == "P")
            {
                return "Pending";
            }
            else
            {
                return "Completed";
            }
        }

        protected void GrdWorkPackage_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string UID = e.CommandArgument.ToString();
            int Cnt = 0;
            if (e.CommandName == "delete")
            {
                Cnt = getdt.Workpackage_Delete(new Guid(UID),new Guid(Session["UserUID"].ToString()));
                if (Cnt > 0)
                {
                    BindWorkPackages();
                }
            }
            else
            {
                if (e.CommandName == "up")
                {
                    Cnt = getdt.Workpackge_Order_Update(new Guid(UID), "Up");
                }
                else
                {
                    Cnt = getdt.Workpackge_Order_Update(new Guid(UID), "Down");

                }
                if (Cnt > 0)
                {
                    BindWorkPackages();
                }
            }
        }

        protected void GrdWorkPackage_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void GrdWorkPackage_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HtmlGenericControl divBudgetlinkshow = e.Row.FindControl("divB") as HtmlGenericControl;
                HtmlGenericControl divBudgetlnkHide = e.Row.FindControl("divB2") as HtmlGenericControl;

                HtmlGenericControl divactuallinkshow = e.Row.FindControl("divR") as HtmlGenericControl;
                HtmlGenericControl divactuallnkHide = e.Row.FindControl("divR2") as HtmlGenericControl;

                HtmlGenericControl divendatelinkshow = e.Row.FindControl("divE") as HtmlGenericControl;
                HtmlGenericControl divenddatelnkHide = e.Row.FindControl("divE2") as HtmlGenericControl;

              //  e.Row.Style.Add("text-decoration", "blink");
              //  e.Row.Attributes.Add("onmouseover", "this.originalcolor=this.style.backgroundColor;" + " this.style.backgroundColor='#FDCB0A';");
              //  e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalcolor;");

                bool show = false;
                DataSet ds = getdt.GetPrjMasterMailSettings(new Guid(DDLProject.SelectedValue), new Guid(e.Row.Cells[12].Text));
                if ((Session["TypeOfUser"].ToString() != "U"))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (dr["UserUID"].ToString() == Session["UserUID"].ToString())
                        {

                            divBudgetlinkshow.Visible = true;
                            divBudgetlnkHide.Visible = false;
                            //
                            divactuallinkshow.Visible = true;
                            divactuallnkHide.Visible = false;
                            //
                            divendatelinkshow.Visible = true;
                            divenddatelnkHide.Visible = false;
                            show = true;

                        }
                    }
                }
                else
                {
                    divBudgetlinkshow.Visible = true;
                    divBudgetlnkHide.Visible = false;
                    //
                    divactuallinkshow.Visible = true;
                    divactuallnkHide.Visible = false;
                    //
                    divendatelinkshow.Visible = true;
                    divenddatelnkHide.Visible = false;
                    show = true;
                }

                DataSet dsdata = new DataSet(); 
                if(show)
                { 
                if (ds.Tables[0].Rows.Count > 0)
                {
                        if (ds.Tables[1].Rows[0]["Frequency"].ToString() == "Monthly")
                        {
                            dsdata = getdt.GetWorkPackageDataHistory(new Guid(e.Row.Cells[12].Text), "budget");
                            if (dsdata.Tables[0].Rows.Count > 0)
                            {
                                if (Convert.ToDateTime(dsdata.Tables[0].Rows[0]["CreatedDate"]).Month != DateTime.Now.Month && Convert.ToDateTime(dsdata.Tables[0].Rows[0]["CreatedDate"]).Year != DateTime.Now.Year)
                                {
                                    e.Row.Cells[2].BackColor = Color.Orange;
                                }
                            }
                            else
                            {
                                e.Row.Cells[2].BackColor = Color.Orange;
                            }
                            dsdata = getdt.GetWorkPackageDataHistory(new Guid(e.Row.Cells[12].Text), "expenditure");
                            if (dsdata.Tables[0].Rows.Count > 0)
                            {
                                if (Convert.ToDateTime(dsdata.Tables[0].Rows[0]["CreatedDate"]).Month != DateTime.Now.Month && Convert.ToDateTime(dsdata.Tables[0].Rows[0]["CreatedDate"]).Year != DateTime.Now.Year)
                                {
                                    e.Row.Cells[3].BackColor = Color.Orange;
                                }
                            }
                            else
                            {
                                e.Row.Cells[3].BackColor = Color.Orange;
                            }
                            dsdata = getdt.GetWorkPackageDataHistory(new Guid(e.Row.Cells[12].Text), "enddate");
                            if (dsdata.Tables[0].Rows.Count > 0)
                            {
                                if (Convert.ToDateTime(dsdata.Tables[0].Rows[0]["CreatedDate"]).Month != DateTime.Now.Month && Convert.ToDateTime(dsdata.Tables[0].Rows[0]["CreatedDate"]).Year != DateTime.Now.Year)
                                {
                                    e.Row.Cells[6].BackColor = Color.Orange;
                                }
                            }
                            else
                            {
                                e.Row.Cells[6].BackColor = Color.Orange;
                            }
                        }
                        else if (ds.Tables[1].Rows[0]["Frequency"].ToString() == "Quarterly")
                        {
                            if (DateTime.Now.Month == 1 || DateTime.Now.Month == 4 || DateTime.Now.Month == 8 || DateTime.Now.Month == 12)
                            {
                                dsdata = getdt.GetWorkPackageDataHistory(new Guid(e.Row.Cells[12].Text), "budget");
                                if (dsdata.Tables[0].Rows.Count > 0)
                                {
                                    if (Convert.ToDateTime(dsdata.Tables[0].Rows[0]["CreatedDate"]).Month != DateTime.Now.Month && Convert.ToDateTime(dsdata.Tables[0].Rows[0]["CreatedDate"]).Year != DateTime.Now.Year)
                                    {
                                        e.Row.Cells[2].BackColor = Color.Orange;
                                    }
                                }
                                else
                                {
                                    e.Row.Cells[2].BackColor = Color.Orange;
                                }
                                dsdata = getdt.GetWorkPackageDataHistory(new Guid(e.Row.Cells[12].Text), "expenditure");
                                if (dsdata.Tables[0].Rows.Count > 0)
                                {
                                    if (Convert.ToDateTime(dsdata.Tables[0].Rows[0]["CreatedDate"]).Month != DateTime.Now.Month && Convert.ToDateTime(dsdata.Tables[0].Rows[0]["CreatedDate"]).Year != DateTime.Now.Year)
                                    {
                                        e.Row.Cells[3].BackColor = Color.Orange;
                                    }
                                }
                                else
                                {
                                    e.Row.Cells[3].BackColor = Color.Orange;
                                }
                                dsdata = getdt.GetWorkPackageDataHistory(new Guid(e.Row.Cells[12].Text), "enddate");
                                if (dsdata.Tables[0].Rows.Count > 0)
                                {
                                    if (Convert.ToDateTime(dsdata.Tables[0].Rows[0]["CreatedDate"]).Month != DateTime.Now.Month && Convert.ToDateTime(dsdata.Tables[0].Rows[0]["CreatedDate"]).Year != DateTime.Now.Year)
                                    {
                                        e.Row.Cells[6].BackColor = Color.Orange;
                                    }
                                }
                                else
                                {
                                    e.Row.Cells[6].BackColor = Color.Orange;
                                }
                            }
                        }
                    }

                }

               
            }
        }
    }
}