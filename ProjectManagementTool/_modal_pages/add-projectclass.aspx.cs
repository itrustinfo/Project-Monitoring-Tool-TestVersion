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
    public partial class add_projectclass : System.Web.UI.Page
    {
        DBGetData getdt = new DBGetData();
        TaskUpdate gettk = new TaskUpdate();
        protected void Page_Load(object sender, EventArgs e)
        {
            if(Session["Username"] == null)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
            }
            else
            {
                if (!IsPostBack)
                {
                    if (Request.QueryString["type"] == "edit")
                    {
                        BindProjectClass(Request.QueryString["pClassUID"]);
                    }
                }
            }
        }

        private void BindProjectClass(string pClassUID)
        {
            DataSet ds = getdt.ProjectClass_SelectBy_pClassUID(new Guid(pClassUID));
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtprojectclass.Text = ds.Tables[0].Rows[0]["ProjectClass_Name"].ToString();
                txtprojectclassdesc.Text= ds.Tables[0].Rows[0]["ProjectClass_Description"].ToString();
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                Guid pClassUID;
                if (Request.QueryString["type"] == "Add")
                {
                    pClassUID = Guid.NewGuid();
                }
                else
                {
                    pClassUID = new Guid(Request.QueryString["pClassUID"]);
                }
                int cnt = getdt.ProjectClass_InsertorUpdate(pClassUID, txtprojectclass.Text, txtprojectclassdesc.Text);
                if (cnt > 0)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
                }
                else if (cnt == -1)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Project Category already exists. Please try with different Project Category.');</script>");
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code APC-01 there is a problem with this feature. Please contact system admin.');</script>");
                }


            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code APC-01 there is a problem with this feature. Please contact system admin.');</script>");
            }
        }
    }
}