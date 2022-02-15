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
    public partial class add_resourceproperty : System.Web.UI.Page
    {
        DBGetData getdata = new DBGetData();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["ProjectUID"] != null)
                {
                    BindUsers();
                }
                if (Request.QueryString["ResourceUID"] != null)
                {
                    BindProperties();

                }
            }
        }
        private void BindUsers()
        {
            DataSet ds = new DataSet();
            if (Session["TypeOfUser"].ToString() == "U" || Session["TypeOfUser"].ToString() =="MD" || Session["TypeOfUser"].ToString() == "VP")
            {
                ds = getdata.getAllUsers();
            }
            else if (Session["TypeOfUser"].ToString() == "PA")
            {
                //ds = getdata.getUsers_by_ProjectUnder(new Guid(DDlProject.SelectedValue));
                ds = getdata.GetUsers_under_ProjectUID(new Guid(Request.QueryString["ProjectUID"]));
            }
            else
            {
                ds = getdata.getUsers_by_ProjectUnder(new Guid(Request.QueryString["ProjectUID"]));
            }
            DDLUser.DataTextField = "UserName";
            DDLUser.DataValueField = "UserUID";
            DDLUser.DataSource = ds;
            DDLUser.DataBind();
        }
        private void BindProperties()
        {
            grdResourceProperty.DataSource = getdata.getResourceProperties_by_ResourceUID(new Guid(Request.QueryString["ResourceUID"]));
            grdResourceProperty.DataBind();
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            int cnt = getdata.InsertorUpdateResourceProperty(Guid.NewGuid(), new Guid(Request.QueryString["ResourceUID"]), new Guid(Request.QueryString["ResourceType_UID"]), txtpropertyname.Text, DDLPropertyTableName.SelectedItem.Text, new Guid(DDLUser.SelectedValue));
            if (cnt > 0)
            {
                txtpropertyname.Text = "";
                BindProperties();
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Inserted Successfully');</script>");
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code : ARP-01 There is a problem with this feature. Please contact system admin.');</script>");
            }
        }
    }
}