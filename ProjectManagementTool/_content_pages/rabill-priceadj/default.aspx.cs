using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProjectManagementTool.DAL;
using ProjectManager.DAL;

namespace ProjectManagementTool._content_pages.rabill_priceadj
{
    public partial class _default : System.Web.UI.Page
    {
        DBGetData dbgetdata = new DBGetData();
        TaskUpdate gettk = new TaskUpdate();
        Invoice invoice = new Invoice();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
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
                        //BindDataforInvoice_RABills();
                       

                    }
                }

            }
        }


        private void BindProject()
        {
            DataTable ds = new DataTable();
            if (Session["TypeOfUser"].ToString() == "U" || Session["TypeOfUser"].ToString() == "MD" || Session["TypeOfUser"].ToString() == "VP")
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

        }
        protected void DDlProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDlProject.SelectedValue != "")
            {
                DataSet ds = new DataSet();
                ds = dbgetdata.GetWorkPackages_By_ProjectUID(new Guid(DDlProject.SelectedValue));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DDLWorkPackage.DataTextField = "Name";
                    DDLWorkPackage.DataValueField = "WorkPackageUID";
                    DDLWorkPackage.DataSource = ds;
                    DDLWorkPackage.DataBind();
                    grdRABillsMaster.Visible = true;
                    SelectedProjectWorkpackage("Workpackage");
                    BindRABillAdjMaster();
                    AddDependency.HRef = "/_modal_pages/add-rabill-priceadj.aspx?WorkpackageUID=" + DDLWorkPackage.SelectedValue;
                    Session["Project_Workpackage"] = DDlProject.SelectedValue + "_" + DDLWorkPackage.SelectedValue;
                }
                else
                {
                    DDLWorkPackage.DataSource = null;
                    DDLWorkPackage.DataBind();
                    grdRABillsMaster.Visible = false;
                }
            }
        }

        protected void DDLWorkPackage_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindRABillAdjMaster();
            Session["Project_Workpackage"] = DDlProject.SelectedValue + "_" + DDLWorkPackage.SelectedValue;
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

        private void BindRABillAdjMaster()
        {
            try
            {
                grdRABillsMaster.DataSource = invoice.GetRABillPriceAdj_Master(new Guid(DDLWorkPackage.SelectedValue));
                grdRABillsMaster.DataBind();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        protected void grdRABillsMaster_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            HiddenField hidRAbuilluid = grdRABillsMaster.Rows[e.RowIndex].FindControl("hidrabillDeleteuid") as HiddenField;
            int result = invoice.DeleteRABillPriceAdjMaster(new Guid(hidRAbuilluid.Value), new Guid(Session["UserUID"].ToString()));
            BindRABillAdjMaster();

        }
    }
}