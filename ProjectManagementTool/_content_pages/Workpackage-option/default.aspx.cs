using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagementTool._content_pages.Workpackage_option
{
    public partial class _default : System.Web.UI.Page
    {
        DBGetData getdata = new DBGetData();
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
                    BindWorkpackageOptions();
                }
                
            }
        }
        private void BindWorkpackageOptions()
        {
            DataSet ds = getdata.Workpackageoption_Select();
            GrdWorkpackageOptions.DataSource = ds;
            GrdWorkpackageOptions.DataBind();
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                Guid WorkpackageoptionUID;
                if (btnSubmit.Text == "Submit")
                {
                    WorkpackageoptionUID = Guid.NewGuid();
                }
                else
                {
                    WorkpackageoptionUID = new Guid(Hidden1.Value);
                }

                int cnt = getdata.Workpackageoption_InsertorUpdate(WorkpackageoptionUID, txtoption.Text);
                if (cnt > 0)
                {
                    txtoption.Text = "";
                    btnSubmit.Text = "Submit";
                    BindWorkpackageOptions();
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Inserted Successfully.');</script>");
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "Error", "<script language='javascript'>alert('" + ex.Message + "');</script>");
            }
            
        }

        protected void GrdWorkpackageOptions_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string UID = e.CommandArgument.ToString();
            if (e.CommandName == "edit")
            {
                btnSubmit.Text = "Update";
                Hidden1.Value = UID;
                DataSet ds = getdata.Workpackageoption_SelectBy_UID(new Guid(UID));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtoption.Text = ds.Tables[0].Rows[0]["Workpackage_OptionName"].ToString();
                }
            }
        }

        protected void GrdWorkpackageOptions_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }
    }
}