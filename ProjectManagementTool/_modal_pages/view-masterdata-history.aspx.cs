using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProjectManager.DAL;

namespace ProjectManagementTool._modal_pages
{
    public partial class view_masterdata_history : System.Web.UI.Page
    {
        DBGetData getdata = new DBGetData();
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
                    if (Request.QueryString["WorkPackageUID"] != null)
                    {
                        if (Request.QueryString["type"].ToString() == "budget")
                        {
                            grdHistory.DataSource = getdata.GetWorkPackageDataHistory(new Guid(Request.QueryString["WorkPackageUID"]), "budget");
                        }
                        else if (Request.QueryString["type"].ToString() == "expenditure")
                        {
                            grdHistory.DataSource = getdata.GetWorkPackageDataHistory(new Guid(Request.QueryString["WorkPackageUID"]), "expenditure");
                        }
                        else if (Request.QueryString["type"].ToString() == "enddate")
                        {
                            grdHistory.DataSource = getdata.GetWorkPackageDataHistory(new Guid(Request.QueryString["WorkPackageUID"]), "enddate");
                        }
                        grdHistory.DataBind();
                    }
                }
            }
        }
    }
}