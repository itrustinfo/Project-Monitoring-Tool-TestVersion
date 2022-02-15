using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProjectManager.DAL;

namespace ProjectManagementTool._modal_pages
{
    public partial class add_location : System.Web.UI.Page
    {
        GeneralDocuments GD = new GeneralDocuments();
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
                    if (Request.QueryString["type"] == "edit")
                    {
                        BindLocation(Request.QueryString["lUID"]);
                    }
                }
            }
        }

        private void BindLocation(string LocationMaster_UID)
        {
            DataSet ds = GD.Location_SelectBy_LocationMaster_UID(new Guid(LocationMaster_UID));
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtlocation.Text = ds.Tables[0].Rows[0]["Location_Name"].ToString();
                
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                Guid LocationMaster_UID;
                if (Request.QueryString["lUID"] != null)
                {
                    LocationMaster_UID = new Guid(Request.QueryString["lUID"]);
                }
                else
                {
                    LocationMaster_UID = Guid.NewGuid();
                }

                int cnt = GD.Location_InsertorUpdate(LocationMaster_UID, txtlocation.Text);
                if (cnt > 0)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
                }
                else if (cnt == -1)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Location Name already exists try with different name');</script>");
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code AddLoc-01 there is a problem with this feature. Please contact system admin.');</script>");
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code AddLoc-01 there is a problem with this feature. Please contact system admin.');</script>");
            }
        }
    }
}