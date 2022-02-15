using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagementTool._content_pages.domain_details
{
    public partial class _default : System.Web.UI.Page
    {
        DBGetData getdt = new DBGetData();
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
                    BindDomainDetails();
                }
            }


        }

        protected void BindDomainDetails()
        {
            DataSet ds = getdt.GetDomainDetails();
            GrdDomainDetails.DataSource = ds;
            GrdDomainDetails.DataBind();
        }
    }
}