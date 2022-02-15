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
    public partial class add_workpackagemaster : System.Web.UI.Page
    {
        DBGetData getdt = new DBGetData();
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
                    BindProject();
                    if (Request.QueryString["type"] == "edit")
                    {
                        BindWorkpackageMaster(Request.QueryString["wmUID"]);
                    }
                }
            }
        }

        private void BindWorkpackageMaster(string wmUID)
        {
            DataSet ds = getdt.MasterWorkpackage_selectBy_MasterWorkPackageUID(new Guid(wmUID));
            if (ds.Tables[0].Rows.Count > 0)
            {
                DDlProject.SelectedValue = new Guid(ds.Tables[0].Rows[0]["ProjectUID"].ToString()).ToString();
                txtworkpackagename.Text = ds.Tables[0].Rows[0]["MasterWorkPackageName"].ToString();
                txtcode.Text = ds.Tables[0].Rows[0]["MasterWorkPackageCode"].ToString();
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
            DDlProject.DataTextField = "ProjectName";
            DDlProject.DataValueField = "ProjectUID";
            DDlProject.DataSource = ds;
            DDlProject.DataBind();

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                Guid masteruid;
                if (Request.QueryString["type"]== "Add")
                {
                    masteruid = Guid.NewGuid();
                }
                else
                {
                    masteruid = new Guid(Request.QueryString["wmUID"]);
                }

                int cnt = getdt.MasterWorkpackages_InsertorUpdate(masteruid, new Guid(DDlProject.SelectedValue), txtworkpackagename.Text, txtcode.Text);
                if (cnt > 0)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code AWPM-01 there is a problem with this feature. Please contact system admin.');</script>");
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code AWPM-01 there is a problem with this feature. Please contact system admin.');</script>");
            }
        }
    }
}