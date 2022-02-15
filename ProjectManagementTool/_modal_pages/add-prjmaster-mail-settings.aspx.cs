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
    public partial class add_prjmaster_mail_settings : System.Web.UI.Page
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
                    
                }
            }
        }

        private void BindProject()
        {
            DataTable ds = new DataTable();
            ds = gettk.GetProjects();
            DDlProject.DataTextField = "ProjectName";
            DDlProject.DataValueField = "ProjectUID";
            DDlProject.DataSource = ds;
            DDlProject.DataBind();
            DDlProject.Items.Insert(0, "--Select--");

        }

        protected void DDlProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            ds = getdt.GetWorkPackages_By_ProjectUID(new Guid(DDlProject.SelectedValue));
            if (ds.Tables[0].Rows.Count > 0)
            {
                DDLWorkPackage.DataTextField = "Name";
                DDLWorkPackage.DataValueField = "WorkPackageUID";
                DDLWorkPackage.DataSource = ds;
                DDLWorkPackage.DataBind();
                // fill the checkboxlist
                chkUserList.Items.Clear();
                if (getdt.GetProjectMasterReminderUsers(new Guid(DDlProject.SelectedValue), new Guid(DDLWorkPackage.SelectedValue)).Tables[0].Rows.Count > 0)
                {
                    chkUserList.DataSource = getdt.GetProjectMasterReminderUsers(new Guid(DDlProject.SelectedValue), new Guid(DDLWorkPackage.SelectedValue));
                    chkUserList.DataTextField = "Name";
                    chkUserList.DataValueField = "UserUID";
                    chkUserList.DataBind();
                    chkUserList.Enabled = true;
                  
                }
                else
                {
                    //chkUserList.Items.Insert(0, "No Users found !");
                    chkUserList.Enabled = false;
                }
                LoadUserSelected();
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e) //final submit
        {
            try
            {
                string Frequency = rdFreqList.SelectedValue;
                bool sresult = false;
                if(string.IsNullOrEmpty(Frequency))
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Please select frequency');</script>");
                    return;
                }
               
                foreach (ListItem listItem in chkUserList.Items)
                {
                    if (listItem.Selected)
                    {
                        getdt.InsertPrjMasterMailSettings(Guid.NewGuid(), new Guid(DDlProject.SelectedValue), new Guid(DDLWorkPackage.SelectedValue), new Guid(listItem.Value),"Y",Frequency);
                    }
                    else
                    {
                        
                        getdt.InsertPrjMasterMailSettings(Guid.NewGuid(), new Guid(DDlProject.SelectedValue), new Guid(DDLWorkPackage.SelectedValue), new Guid(listItem.Value), "N",Frequency);
                    }
                    sresult = true;
                }

                if (sresult)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Data Updated Successfully');</script>");
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('No User selected !');</script>");

                }
            }
            catch(Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Occurred while updating data !');</script>");
            }
        }

        private void LoadUserSelected()
        {
            try
            {
                rdFreqList.Items.Clear();
                rdFreqList.Items.Add(new ListItem("Monthly"));
                rdFreqList.Items.Add(new ListItem("Quarterly"));

                DataSet ds = new DataSet();
                ds = getdt.GetPrjMasterMailSettings(new Guid(DDlProject.SelectedValue), new Guid(DDLWorkPackage.SelectedValue));

                
                if (ds.Tables[1].Rows.Count > 0)
                {
                    rdFreqList.SelectedValue = ds.Tables[1].Rows[0]["Frequency"].ToString();
                }

                foreach (ListItem listItem in chkUserList.Items)
                {
                    listItem.Selected = false;
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (dr["UserUID"].ToString() == listItem.Value)
                        {
                            listItem.Selected = true;
                        }
                    }
                }

               
            }
            catch (Exception ex)
            {

            }
        }

        protected void DDLWorkPackage_SelectedIndexChanged(object sender, EventArgs e)
        {
            // fill the checkboxlist
            chkUserList.Items.Clear();
            if (getdt.GetProjectMasterReminderUsers(new Guid(DDlProject.SelectedValue), new Guid(DDLWorkPackage.SelectedValue)).Tables[0].Rows.Count > 0)
            {
                chkUserList.DataSource = getdt.GetProjectMasterReminderUsers(new Guid(DDlProject.SelectedValue), new Guid(DDLWorkPackage.SelectedValue));
                chkUserList.DataTextField = "Name";
                chkUserList.DataValueField = "UserUID";
                chkUserList.DataBind();
                chkUserList.Enabled = true;

            }
            else
            {
                //chkUserList.Items.Insert(0, "No Users found !");
                chkUserList.Enabled = false;
            }
            LoadUserSelected();
        }
    }
}