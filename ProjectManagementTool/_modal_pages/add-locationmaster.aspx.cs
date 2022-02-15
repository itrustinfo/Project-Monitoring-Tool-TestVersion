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
    public partial class add_locationmaster : System.Web.UI.Page
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
                        BindLocationMaster(Request.QueryString["LocationUID"]);
                    }
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

                ds = gettk.GetAssignedProjects_by_UserUID(new Guid(Session["UserUID"].ToString()));
            }
            DDlProject.DataTextField = "ProjectName";
            DDlProject.DataValueField = "ProjectUID";
            DDlProject.DataSource = ds;
            DDlProject.DataBind();

        }

        private void BindLocationMaster(string LocationMasterUID)
        {
            DataSet ds = getdt.MasterLocation_selectBy_LocationMasterUID(new Guid(LocationMasterUID));
            if (ds.Tables[0].Rows.Count > 0)
            {
                DDlProject.SelectedValue = new Guid(ds.Tables[0].Rows[0]["ProjectUID"].ToString()).ToString();
                txtlocation.Text = ds.Tables[0].Rows[0]["LocationMaster_Name"].ToString();
                txtcode.Text = ds.Tables[0].Rows[0]["LocationMaster_Code"].ToString();
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                Guid LocationUID;
                if (Request.QueryString["type"] == "Add")
                {
                    LocationUID = Guid.NewGuid();
                }
                else
                {
                    LocationUID = new Guid(Request.QueryString["LocationUID"]);
                }

                int cnt = getdt.MasterLocation_InsertorUpdate(LocationUID, new Guid(DDlProject.SelectedValue), txtlocation.Text, txtcode.Text);
                if (cnt > 0)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
                }
                else if (cnt == -1)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Location Name already exists. Try with different Location Name.');</script>");
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code ALM-01 there is a problem with this feature. Please contact system admin.');</script>");
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code ALM-01 there is a problem with this feature. Please contact system admin.');</script>");
            }
        }
    }
}