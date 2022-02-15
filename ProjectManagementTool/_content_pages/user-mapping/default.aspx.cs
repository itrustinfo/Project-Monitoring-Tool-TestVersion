using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProjectManager.DAL;
using System.Data;

namespace ProjectManagementTool._content_pages.user_mapping
{
    public partial class deafult : System.Web.UI.Page
    {
        DBGetData getdata = new DBGetData();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Username"] == null)
            {
                Response.Redirect("~/Login.aspx");
            }
            if(!IsPostBack)
            {
                LoadUsertype();
                LoadUserFunctionalityMaster();
                LoadSelectedUserFunctionality();
                btnSubmit.Visible = false;
                btnEdit.Visible = true;
                btnCancel.Visible = false;
                chkUserFunctionality.Enabled = false;
            }

        }

        private void LoadUsertype()
        {
            try
            {
                ddlUserType.DataSource = getdata.getAllUsers_Roles();
                ddlUserType.DataTextField = "UserRole_Desc";
                ddlUserType.DataValueField = "UserRole_Name";
                ddlUserType.DataBind();
            }
            catch(Exception ex)
            {

            }
        }

        private void LoadUserFunctionalityMaster()
        {
            try
            {
                chkUserFunctionality.DataSource = getdata.GetAllUserTypeFunctionality_Master();
                chkUserFunctionality.DataTextField = "Functionality";
                chkUserFunctionality.DataValueField = "UID";
                chkUserFunctionality.DataBind();
            }
            catch (Exception ex)
            {

            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                getdata.DeleteUsertypeFunctionality_Mapping(ddlUserType.SelectedValue);
                foreach (ListItem listItem in chkUserFunctionality.Items)
                {
                    if (listItem.Selected)
                    {
                        //do some work 
                        getdata.InsertIntoUsertypeFunctionality_Mapping(ddlUserType.SelectedValue, new Guid(listItem.Value));
                    }
                    else
                    {
                        //do something else 
                    }
                }

                btnSubmit.Visible = false;
                btnEdit.Visible = true;
                btnCancel.Visible = false;
                chkUserFunctionality.Enabled = false;

                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Data Updated Successfully');</script>");
            }
            catch(Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('There is a problem with this feature. Please contact sysytem admin');</script>");
            }
        }

        private void LoadSelectedUserFunctionality()
        {
            try
            {
                DataSet ds = new DataSet();
                ds = getdata.GetUsertypeFunctionality_Mapping(ddlUserType.SelectedValue);

                foreach (ListItem listItem in chkUserFunctionality.Items)
                {
                    listItem.Selected = false;
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (dr["FunctionalityUID"].ToString() == listItem.Value)
                        {
                            listItem.Selected = true;
                        }
                    }
                }

                DisableFunctionality();
            }
            catch(Exception ex)
            {

            }
        }

        protected void ddlUserType_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadSelectedUserFunctionality();
            Enable_Funcatiality();
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            btnSubmit.Visible = true;
            btnEdit.Visible = false;
            btnCancel.Visible = true;
            chkUserFunctionality.Enabled = true;

            Enable_Funcatiality();
        }

        private void Enable_Funcatiality()
        {
            DataSet ds = new DataSet();
            ds = getdata.GetUsertypeFunctionality_Mapping(ddlUserType.SelectedValue);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (dr["Code"].ToString() == "BOQM")
                {
                    chkUserFunctionality.Items.FindByText("BOQ(ADD)").Enabled = true;
                    chkUserFunctionality.Items.FindByText("BOQ(EDIT)").Enabled = true;
                    chkUserFunctionality.Items.FindByText("BOQ(DELETE)").Enabled = true;

                    chkUserFunctionality.Items.FindByText("Joint Inspection(ADD)").Enabled = true;
                    chkUserFunctionality.Items.FindByText("Joint Inspection(EDIT)").Enabled = true;
                    chkUserFunctionality.Items.FindByText("Joint Inspection(DELETE)").Enabled = true;
                }
                else if (dr["Code"].ToString() == "M-RA")
                {
                    chkUserFunctionality.Items.FindByText("RA Bill(ADD)").Enabled = true;
                    chkUserFunctionality.Items.FindByText("RA Bill(EDIT)").Enabled = true;
                    chkUserFunctionality.Items.FindByText("RA Bill(DELETE)").Enabled = true;
                }
                else if (dr["Code"].ToString() == "M-I")
                {
                    chkUserFunctionality.Items.FindByText("Invoice(ADD)").Enabled = true;
                    chkUserFunctionality.Items.FindByText("Invoice(EDIT)").Enabled = true;
                    chkUserFunctionality.Items.FindByText("Invoice(DELETE)").Enabled = true;
                    chkUserFunctionality.Items.FindByText("Add RA Bill to Invoice").Enabled = true;
                    chkUserFunctionality.Items.FindByText("Invoice Deduction(ADD)").Enabled = true;
                    chkUserFunctionality.Items.FindByText("Invoice Deduction(EDIT)").Enabled = true;
                    chkUserFunctionality.Items.FindByText("Invoice Deduction(DELETE)").Enabled = true;
                }
                else if (dr["Code"].ToString() == "M-ISS")
                {
                    chkUserFunctionality.Items.FindByText("Issues(ADD)").Enabled = true;
                    chkUserFunctionality.Items.FindByText("Issues(EDIT)").Enabled = true;
                    chkUserFunctionality.Items.FindByText("Issues(DELETE)").Enabled = true;
                }
            }
        }

        private void DisableFunctionality()
        {
            chkUserFunctionality.Items.FindByText("BOQ(ADD)").Enabled = false;
            chkUserFunctionality.Items.FindByText("BOQ(EDIT)").Enabled = false;
            chkUserFunctionality.Items.FindByText("BOQ(DELETE)").Enabled = false;
            chkUserFunctionality.Items.FindByText("Joint Inspection(ADD)").Enabled = false;
            chkUserFunctionality.Items.FindByText("Joint Inspection(EDIT)").Enabled = false;
            chkUserFunctionality.Items.FindByText("Joint Inspection(DELETE)").Enabled = false;

            chkUserFunctionality.Items.FindByText("RA Bill(ADD)").Enabled = false;
            chkUserFunctionality.Items.FindByText("RA Bill(EDIT)").Enabled = false;
            chkUserFunctionality.Items.FindByText("RA Bill(DELETE)").Enabled = false;

            chkUserFunctionality.Items.FindByText("Invoice(ADD)").Enabled = false;
            chkUserFunctionality.Items.FindByText("Invoice(EDIT)").Enabled = false;
            chkUserFunctionality.Items.FindByText("Invoice(DELETE)").Enabled = false;
            chkUserFunctionality.Items.FindByText("Add RA Bill to Invoice").Enabled = false;
            chkUserFunctionality.Items.FindByText("Invoice Deduction(ADD)").Enabled = false;
            chkUserFunctionality.Items.FindByText("Invoice Deduction(EDIT)").Enabled = false;
            chkUserFunctionality.Items.FindByText("Invoice Deduction(DELETE)").Enabled = false;

            chkUserFunctionality.Items.FindByText("Issues(ADD)").Enabled = false;
            chkUserFunctionality.Items.FindByText("Issues(EDIT)").Enabled = false;
            chkUserFunctionality.Items.FindByText("Issues(DELETE)").Enabled = false;

            chkUserFunctionality.Items.FindByText("Add - Project Status of Work Progress").Enabled = false;
            chkUserFunctionality.Items.FindByText("Add/View - Consolidated Activities").Enabled = false;
            chkUserFunctionality.Items.FindByText("Add/View - Site Photographs").Enabled = false;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            btnSubmit.Visible = false;
            btnEdit.Visible = true;
            btnCancel.Visible = false;
            chkUserFunctionality.Enabled = false;
        }

        protected void chkUserFunctionality_SelectedIndexChanged(object sender, EventArgs e)
        {
            //string eventTarget = Request.Form.Get("__EVENTTARGET");

            //int index = Convert.ToInt32(eventTarget.Substring(eventTarget.Length - 1));
            string result = Request.Form["__EVENTTARGET"];

            string[] checkedBox = result.Split('$');

            int index = int.Parse(checkedBox[checkedBox.Length - 1]);

            string Functionality = chkUserFunctionality.Items[index].Text;
            if (Functionality == "Menu - BOQ")
            {
                if (chkUserFunctionality.Items[index].Selected == true)
                {
                    chkUserFunctionality.Items.FindByText("BOQ(ADD)").Enabled = true;
                    chkUserFunctionality.Items.FindByText("BOQ(EDIT)").Enabled = true;
                    chkUserFunctionality.Items.FindByText("BOQ(DELETE)").Enabled = true;

                    chkUserFunctionality.Items.FindByText("Joint Inspection(ADD)").Enabled = true;
                    chkUserFunctionality.Items.FindByText("Joint Inspection(EDIT)").Enabled = true;
                    chkUserFunctionality.Items.FindByText("Joint Inspection(DELETE)").Enabled = true;
                }
                else
                {
                    chkUserFunctionality.Items.FindByText("BOQ(ADD)").Enabled = false;
                    chkUserFunctionality.Items.FindByText("BOQ(EDIT)").Enabled = false;
                    chkUserFunctionality.Items.FindByText("BOQ(DELETE)").Enabled = false;

                    chkUserFunctionality.Items.FindByText("Joint Inspection(ADD)").Enabled = false;
                    chkUserFunctionality.Items.FindByText("Joint Inspection(EDIT)").Enabled = false;
                    chkUserFunctionality.Items.FindByText("Joint Inspection(DELETE)").Enabled = false;
                }

            }
            else if (Functionality == "Menu - RABills")
            {
                if (chkUserFunctionality.Items[index].Selected == true)
                {
                    chkUserFunctionality.Items.FindByText("RA Bill(ADD)").Enabled = true;
                    chkUserFunctionality.Items.FindByText("RA Bill(EDIT)").Enabled = true;
                    chkUserFunctionality.Items.FindByText("RA Bill(DELETE)").Enabled = true;
                }
                else
                {
                    chkUserFunctionality.Items.FindByText("RA Bill(ADD)").Enabled = false;
                    chkUserFunctionality.Items.FindByText("RA Bill(EDIT)").Enabled = false;
                    chkUserFunctionality.Items.FindByText("RA Bill(DELETE)").Enabled = false;
                }
            }
            else if (Functionality == "Menu - Invoice")
            {
                if (chkUserFunctionality.Items[index].Selected == true)
                {
                    chkUserFunctionality.Items.FindByText("Invoice(ADD)").Enabled = true;
                    chkUserFunctionality.Items.FindByText("Invoice(EDIT)").Enabled = true;
                    chkUserFunctionality.Items.FindByText("Invoice(DELETE)").Enabled = true;

                    chkUserFunctionality.Items.FindByText("Add RA Bill to Invoice").Enabled = true;
                    chkUserFunctionality.Items.FindByText("Invoice Deduction(ADD)").Enabled = true;
                    chkUserFunctionality.Items.FindByText("Invoice Deduction(EDIT)").Enabled = true;
                    chkUserFunctionality.Items.FindByText("Invoice Deduction(DELETE)").Enabled = true;
                }
                else
                {
                    chkUserFunctionality.Items.FindByText("Invoice(ADD)").Enabled = false;
                    chkUserFunctionality.Items.FindByText("Invoice(EDIT)").Enabled = false;
                    chkUserFunctionality.Items.FindByText("Invoice(DELETE)").Enabled = false;

                    chkUserFunctionality.Items.FindByText("Add RA Bill to Invoice").Enabled = false;
                    chkUserFunctionality.Items.FindByText("Invoice Deduction(ADD)").Enabled = false;
                    chkUserFunctionality.Items.FindByText("Invoice Deduction(EDIT)").Enabled = false;
                    chkUserFunctionality.Items.FindByText("Invoice Deduction(DELETE)").Enabled = false;
                }
            }
            else if (Functionality == "Menu - Issues")
            {
                if (chkUserFunctionality.Items[index].Selected == true)
                {
                    chkUserFunctionality.Items.FindByText("Issues(ADD)").Enabled = true;
                    chkUserFunctionality.Items.FindByText("Issues(EDIT)").Enabled = true;
                    chkUserFunctionality.Items.FindByText("Issues(DELETE)").Enabled = true;
                }
                else
                {
                    chkUserFunctionality.Items.FindByText("Issues(ADD)").Enabled = false;
                    chkUserFunctionality.Items.FindByText("Issues(EDIT)").Enabled = false;
                    chkUserFunctionality.Items.FindByText("Issues(DELETE)").Enabled = false;
                }
            }
            else if (Functionality == "Menu - Review Reports")
            {
                if (chkUserFunctionality.Items[index].Selected == true)
                {
                    //chkUserFunctionality.Items.FindByText("Review Report - Project Status of Work Progress").Enabled = true;
                    chkUserFunctionality.Items.FindByText("Add - Project Status of Work Progress").Enabled = true;
                    chkUserFunctionality.Items.FindByText("Add/View - Consolidated Activities").Enabled = true;
                    chkUserFunctionality.Items.FindByText("Add/View - Site Photographs").Enabled = true;
                }
                else
                {
                    //chkUserFunctionality.Items.FindByText("Review Report - Project Status of Work Progress").Enabled = false;
                    chkUserFunctionality.Items.FindByText("Add - Project Status of Work Progress").Enabled = false;
                    chkUserFunctionality.Items.FindByText("Add/View - Consolidated Activities").Enabled = false;
                    chkUserFunctionality.Items.FindByText("Add/View - Site Photographs").Enabled = false;
                }
            }

        }
    }
}