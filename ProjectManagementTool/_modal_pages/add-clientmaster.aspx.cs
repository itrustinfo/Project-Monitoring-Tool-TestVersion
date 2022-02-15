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
    public partial class add_clientmaster : System.Web.UI.Page
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
                        BindClientMaster(Request.QueryString["ClientUID"]);
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

        private void BindClientMaster(string ClientMasterUID)
        {
            DataSet ds = getdt.MasterClient_selectBy_ClientMasterUID(new Guid(ClientMasterUID));
            if (ds.Tables[0].Rows.Count > 0)
            {
                DDlProject.SelectedValue = new Guid(ds.Tables[0].Rows[0]["ProjectUID"].ToString()).ToString();
                txtclientname.Text = ds.Tables[0].Rows[0]["ClientMaster_Name"].ToString();
                txtcode.Text = ds.Tables[0].Rows[0]["ClientMaster_Code"].ToString();
                txtclientdetails.Text = ds.Tables[0].Rows[0]["ClientDetails"].ToString();
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                Guid ClientMasterUID;
                if (Request.QueryString["type"] == "Add")
                {
                    ClientMasterUID = Guid.NewGuid();
                }
                else
                {
                    ClientMasterUID = new Guid(Request.QueryString["ClientUID"]);
                }

                int cnt = getdt.MasterClient_InsertorUpdate(ClientMasterUID, new Guid(DDlProject.SelectedValue), txtclientname.Text, txtcode.Text,txtclientdetails.Text);
                if (cnt > 0)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
                }
                else if (cnt == -1)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Client Name already exists. Try with different Client Name.');</script>");
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code ACM-01 there is a problem with this feature. Please contact system admin.');</script>");
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code ACM-01 there is a problem with this feature. Please contact system admin.');</script>");
            }
        }
    }
}