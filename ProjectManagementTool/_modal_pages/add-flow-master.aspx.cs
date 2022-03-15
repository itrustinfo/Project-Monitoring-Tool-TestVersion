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
    public partial class add_flow_master : System.Web.UI.Page
    {
        DBGetData getData = new DBGetData();
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
                    BindFlow();
                    BindWorkPackage();
                    BindDocumentMaster();
                    BindSteps();
                }
            }
        }

        protected void DDlProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindWorkPackage();
            BindDocumentMaster();
            LoadUsers();
        }

        protected void DDLWorkPackage_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDocumentMaster();
        }

        protected void DDLFlow_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindSteps();
            LoadUsers();
        }
        protected void DDLWorkPackageCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadUsers();
        }

        protected void DDLSteps_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadUsers();
        }
        
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if(DDlProject.SelectedValue == "--Select--")
            {
                Response.Write("<script>alert('Please select a Project');</script>");
                return;
            }
            else if (DDLWorkPackage.SelectedValue == "--Select--")
            {
                Response.Write("<script>alert('Please select a Workpackage');</script>");
                return;
            }
            else if (DDLWorkPackageCategory.SelectedValue == "--Select--")
            {
                Response.Write("<script>alert('Please select a workpackage category');</script>");
                return;
            }
            else if (DDLFlow.SelectedValue == "--Select--")
            {
                Response.Write("<script>alert('Please select a Flow');</script>");
                return;
            }
            else if (DDLSteps.SelectedValue == "--Select--")
            {
                Response.Write("<script>alert('Please select a step');</script>");
                return;
            }


            List<ListItem> selectedItems = chkUserList.Items.Cast<ListItem>()
                                    .Where(li => li.Selected)
                                    .ToList();

            if(selectedItems.Count == 0)
            {
                Response.Write("<script>alert('Please check atleast one user');</script>");
                return;
            }

            var selectedUsersUID = selectedItems.Select(r => new Guid(r.Value)).ToList();

            DataTable dtAddedUsers = getData.FlowMasterUser_Select(new Guid(DDLFlow.SelectedValue), new Guid(DDLWorkPackage.SelectedValue), new Guid(DDLWorkPackageCategory.SelectedValue), Convert.ToInt32(DDLSteps.SelectedValue));


            var toBeDeleted = dtAddedUsers.AsEnumerable().Where(r => !selectedUsersUID.Contains(r.Field<Guid>("UserUID")));
            DataTable dtToBeDeleted = new DataTable();
            if (toBeDeleted.FirstOrDefault() != null)
            {
                dtToBeDeleted = toBeDeleted.CopyToDataTable();
            }

            foreach (var eachSelectedItems in selectedItems)
            {
                int cnt = getData.FlowMasterUser_InsertUpdate(Guid.NewGuid(), new Guid(DDLFlow.SelectedValue), new Guid(DDlProject.SelectedValue), 
                    new Guid(DDLWorkPackage.SelectedValue), new Guid(DDLWorkPackageCategory.SelectedValue), Convert.ToInt32(DDLSteps.SelectedValue), new Guid(eachSelectedItems.Value));

            }
            if(dtToBeDeleted.Rows.Count > 0)
            {
                foreach(DataRow eachRow in dtToBeDeleted.Rows)
                {
                    int cnt = getData.FlowMasterUser_Delete(eachRow.Field<Guid>("UID"));
                }
            }
            Response.Write("<script>alert('User flow saved successfully.');</script>");

        }

        private void BindWorkPackage()
        {
            DataSet ds = new DataSet();
            if (DDlProject.SelectedValue == "--Select--")
            {
                DDLWorkPackage.Items.Clear();
            }
            else
            {
                
                ds = getData.GetWorkPackages_By_ProjectUID(new Guid(DDlProject.SelectedValue));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DDLWorkPackage.DataTextField = "Name";
                    DDLWorkPackage.DataValueField = "WorkPackageUID";
                    DDLWorkPackage.DataSource = ds;
                    DDLWorkPackage.DataBind();
                    
                }
            }
            DDLWorkPackage.Items.Insert(0, "--Select--");
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
        void BindFlow()
        {
            DataTable ds = getData.GetDocumentFlow();
            if (ds != null && ds.Rows.Count > 0)
            {
                DDLFlow.DataTextField = "Flow_Name";
                DDLFlow.DataValueField = "FlowMasterUID";
                DDLFlow.DataSource = ds;
                DDLFlow.DataBind();
                DDLFlow.Items.Insert(0, "--Select--");
                ViewState["Flow"] = ds;
            }
        }

        void BindDocumentMaster()
        {
            DataTable ds = new DataTable();
            if (DDLWorkPackage.SelectedValue == "--Select--")
            {
                DDLWorkPackageCategory.Items.Clear();
            }
            else
            {
                ds = getData.GetWorkpackageCategory(new Guid(DDLWorkPackage.SelectedValue));
                if (ds != null && ds.Rows.Count > 0)
                {
                    DDLWorkPackageCategory.DataTextField = "WorkPackageCategory_Name";
                    DDLWorkPackageCategory.DataValueField = "WorkPackageCategory_UID";
                    DDLWorkPackageCategory.DataSource = ds;
                    DDLWorkPackageCategory.DataBind();
                }
            }
            DDLWorkPackageCategory.Items.Insert(0, "--Select--");
        }

        private void BindSteps()
        {
            DDLSteps.Items.Clear();
            int totalSteps = 0;
            if (DDLFlow.SelectedValue != "--Select--")
            {
                DataTable dt = (DataTable)ViewState["Flow"];
                totalSteps = dt.AsEnumerable().Where(r => r.Field<Guid>("FlowMasterUID") == new Guid(DDLFlow.SelectedValue)).Select(r => r.Field<int>("Steps_Count")).FirstOrDefault();
               
                for (int counter = 1; counter <= totalSteps; counter++)
                {
                    string colName = "FlowStep" + counter.ToString() + "_DisplayName";
                    var val = dt.AsEnumerable().Where(r => r.Field<Guid>("FlowMasterUID") == new Guid(DDLFlow.SelectedValue))
                        .Select(r => r.Field<string>(colName)).FirstOrDefault();


                    DDLSteps.Items.Add(new ListItem { Value = counter.ToString(), Text = val });
                }
            }

            
            DDLSteps.Items.Insert(0, "--Select--");
            
        }

        private void LoadUsers()
        {
            chkUserList.Items.Clear();

            if (DDLWorkPackageCategory.SelectedValue != "--Select--")
            {
                DataTable dt = getData.GetUserDetails_By_WorkpackageCategory(new Guid(DDLWorkPackageCategory.SelectedValue));

                if (dt != null && dt.Rows.Count > 0)
                {
                    chkUserList.DataSource = dt;
                    chkUserList.DataTextField = "Name";
                    chkUserList.DataValueField = "UserUID";
                    chkUserList.DataBind();
                    chkUserList.Enabled = true;
                    LoadUserSelected();
                }
                else
                {
                    chkUserList.Enabled = false;
                }
            }
        }
        private void LoadUserSelected()
        {
            int steps = 0;
            int.TryParse(DDLSteps.SelectedValue, out steps);
            DataTable dtUsers = new DataTable();
            
            if(DDLFlow.SelectedValue != "--Select--" && DDLWorkPackage.SelectedValue != "--Select--" && DDLWorkPackageCategory.SelectedValue != "--Select--" && DDLSteps.SelectedValue != "--Select--")
            {
                dtUsers = getData.FlowMasterUser_Select(new Guid(DDLFlow.SelectedValue), new Guid(DDLWorkPackage.SelectedValue), new Guid(DDLWorkPackageCategory.SelectedValue), Convert.ToInt32(DDLSteps.SelectedValue));

            }

            if (dtUsers != null && dtUsers.Rows.Count > 0)
            {
                foreach (ListItem listItem in chkUserList.Items)
                {
                    listItem.Selected = false;

                    if (dtUsers.AsEnumerable().Where(r => r.Field<Guid>("UserUID") == new Guid(listItem.Value)).FirstOrDefault() != null)
                    {
                        listItem.Selected = true;
                    }
                }
            }
        }

    }
}