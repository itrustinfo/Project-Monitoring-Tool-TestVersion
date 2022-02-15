using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProjectManagementTool.DAL;
using ProjectManager.DAL;
namespace ProjectManagementTool._modal_pages
{
    public partial class add_Fin_Month_data : System.Web.UI.Page
    {
        DBGetData getdata = new DBGetData();
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {

            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                int result = getdata.InsertfinMonthData(Guid.NewGuid(), new Guid(Session["UserUID"].ToString()), new Guid(Request.QueryString["WorkPackageUID"].ToString()), decimal.Parse(txtPayment.Text), ddlItem.SelectedValue, int.Parse(ddlYear.SelectedValue));
                if (result != 0)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "Warning", "<script language='javascript'>alert('There was some issue with the insert.Please contact system admin!.');</script>");
                }
            }
            catch(Exception ex)
            {

            }

        }
    }
}