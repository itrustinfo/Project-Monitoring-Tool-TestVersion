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
    public partial class add_workpackagecategory : System.Web.UI.Page
    {
        DBGetData getdt = new DBGetData();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindMasterCategory();
                BindWorkpackage();
                AddNew.Visible = true;
                CopyFrom.Visible = false;
                if (Request.QueryString["wName"] != null)
                {
                    txtworkpackage.Text = Request.QueryString["wName"];
                }
            }
        }

        private void BindMasterCategory()
        {
            DataSet ds = getdt.GetActualDocument_Type_Master();
            lstCategory.DataTextField = "ActualDocumentType";
            lstCategory.DataValueField = "ActualDocumenTypeUID";
            lstCategory.DataSource = ds; 
            lstCategory.DataBind();
        }
        private void BindWorkpackage()
        {
            DataSet ds = new DataSet();
            if (Session["TypeOfUser"].ToString() == "U" || Session["TypeOfUser"].ToString() =="MD" || Session["TypeOfUser"].ToString() == "VP")
            {
                ds = getdt.GetAllWorkPackages();
            }
            else if (Session["TypeOfUser"].ToString() == "PA")
            {
                ds = getdt.GetWorkPackages_by_UserUID_Without_Selected(new Guid(Session["UserUID"].ToString()), new Guid(Request.QueryString["WorkPackageUID"]));
            }
            else
            {
                ds = getdt.GetWorkPackages_by_UserUID_Without_Selected(new Guid(Session["UserUID"].ToString()), new Guid(Request.QueryString["WorkPackageUID"]));
            }
            DDLWorkpackage.DataTextField = "Name";
            DDLWorkpackage.DataValueField = "WorkPackageUID";
            DDLWorkpackage.DataSource = ds;
            DDLWorkpackage.DataBind();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (RBList.SelectedValue == "Add New")
            {
                int selectedCount = lstCategory.Items.Cast<ListItem>().Count(li => li.Selected);
                if (selectedCount == 0 && txtcategory.Text == "")
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Enter Category Name or Select from List')</script>");
                }
                else
                {
                    if (txtcategory.Text != "")
                    {
                        int cnt = getdt.WorkPackageCategory_Insert_or_Update(Guid.NewGuid(), new Guid(Request.QueryString["WorkPackageUID"]), txtcategory.Text);
                        if (cnt == -1)
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Category already exists')</script>");
                            txtcategory.Text = "";
                        }
                        else
                        {
                            txtcategory.Text = "";
                        }
                    }
                    else
                    {
                        foreach (ListItem item in lstCategory.Items)
                        {
                            if (item.Selected)
                            {
                                int cnt = getdt.WorkPackageCategory_Insert_or_Update(Guid.NewGuid(), new Guid(Request.QueryString["WorkPackageUID"]), item.Text);
                                if (cnt > 0)
                                {

                                }
                            }
                        }
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
                    }
                }
            }
            else
            {
                DataSet ds = getdt.WorkPackageCategory_Selectby_WorkPackageUID(new Guid(DDLWorkpackage.SelectedValue));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        int cnt = getdt.WorkPackageCategory_Insert_or_Update(Guid.NewGuid(), new Guid(Request.QueryString["WorkPackageUID"]), ds.Tables[0].Rows[i]["WorkPackageCategory_Name"].ToString());
                        if (cnt <= 0)
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code: AWC-01 there is a problem with these feature. Please contact system admin.');</script>");
                        }
                    }
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('No Categories found..');</script>");
                }
            }
        }

        protected void RBList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RBList.SelectedValue == "Add New")
            {
                AddNew.Visible = true;
                CopyFrom.Visible = false;
            }
            else
            {
                AddNew.Visible = false;
                CopyFrom.Visible = true;
            }
        }
    }
}