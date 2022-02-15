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
    public partial class assign_locationtouser : System.Web.UI.Page
    {
        GeneralDocuments GD = new GeneralDocuments();
        DBGetData getdt = new DBGetData();
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
                    BindLocation();
                    BindUser();
                    if (Request.QueryString["uUID"] !=null)
                    {
                        BindUserLocation(Request.QueryString["uUID"]);
                    }
                }
            }
        }
        private void BindLocation()
        {
            DataSet ds = GD.Location_Select();
            LstLocation.DataTextField = "Location_Name";
            LstLocation.DataValueField = "LocationMaster_UID";
            LstLocation.DataSource = ds;
            LstLocation.DataBind();
        }
        private void BindUser()
        {
            DataSet ds= getdt.getAllUsers();
            DDLUser.DataTextField = "UserName";
            DDLUser.DataValueField = "UserUID";
            DDLUser.DataSource = ds;
            DDLUser.DataBind();
            DDLUser.Items.Insert(0, new ListItem("--Select--", ""));
        }

        private void BindUserLocation(string UserUID)
        {
            DDLUser.SelectedValue = UserUID;
            if (DDLUser.SelectedValue != "")
            {
                DataSet dsLoc = GD.UserLocation_SelectBy_UserUID(new Guid(DDLUser.SelectedValue));
                for (int i = 0; i < dsLoc.Tables[0].Rows.Count; i++)
                {
                    foreach (ListItem item in LstLocation.Items)
                    {
                        if (dsLoc.Tables[0].Rows[i]["LocationMaster_UID"].ToString() == item.Value)
                        {
                            item.Selected = true;
                        }
                    }
                }
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                int ItemsCount = (from ListItem li in LstLocation.Items
                                  where li.Selected == true
                                  select li).Count();

                if (ItemsCount == 0)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Please select atleast one location for this User..');</script>");
                }
                else
                {
                    string sAction = string.Empty;
                    if (Request.QueryString["type"] == "edit")
                    {
                        sAction = "edit";

                        int cnt = GD.UserLocation_Delete_by_UserUID(new Guid(DDLUser.SelectedValue));
                        if (cnt <= 0)
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code AddUserLoc-01 there is a problem with this feature. Please contact system admin.');</script>");
                        }
                    }
                    else
                    {
                        sAction = "add";
                    }

                    foreach (ListItem item in LstLocation.Items)
                    {
                        if (item.Selected)
                        {
                            int cnt = GD.UserLocation_InsertorUpdate(Guid.NewGuid(), new Guid(DDLUser.SelectedValue), new Guid(item.Value), sAction);
                            if (cnt <= 0)
                            {
                                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code AddUserLoc-01 there is a problem with this feature. Please contact system admin.');</script>");
                            }
                        }
                    }

                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
                }
                
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code AddUserLoc-01 there is a problem with this feature. Please contact system admin.');</script>");
            }
        }
    }
}