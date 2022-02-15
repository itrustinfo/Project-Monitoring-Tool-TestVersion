using ProjectManagementTool.DAL;
using ProjectManager.DAL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagementTool._modal_pages
{
    public partial class add_jointinspection_to_rabill : System.Web.UI.Page
    {
        DBGetData getdt = new DBGetData();
        Invoice invoice = new Invoice();
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
                    if (Request.QueryString["View"] != null)
                    {
                        btnAdd.Visible = false;
                        BindJointInspection(Request.QueryString["itemUId"]);
                    }
                    else
                    {
                        Session["chkditems"] = null;
                        BindJointInspection(Request.QueryString["WorkpackageUID"]);
                        if (grdinspectionReport.Rows.Count > 0)
                        {
                            btnAdd.Visible = true;
                        }
                        else
                        {
                            btnAdd.Visible = false;
                        }
                    }
                    
                }
            }
        }
        private void BindJointInspection(string WorkpackageUID)
        {
            DataSet ds = new DataSet();
            if (Request.QueryString["View"] != null)
            {
                ds = invoice.GetAssignedJointInspection_by_RABill_ItemUID(new Guid(Request.QueryString["itemUId"]), new Guid(Request.QueryString["RABillUid"]));
            }
            else
            {
                //ds = invoice.GetJointInspection_by_WorkpackageUID(new Guid(WorkpackageUID), new Guid(Request.QueryString["RABillUid"]));
                string ProjectUID = getdt.getProjectUIDby_WorkpackgeUID(new Guid(WorkpackageUID));
                ds = invoice.GetJointInspection_by_ProjectUID_ItemUID(new Guid(ProjectUID), new Guid(Request.QueryString["itemUId"]));
            }
            grdinspectionReport.DataSource = ds;
            grdinspectionReport.DataBind();
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                int i = 0;
                int ErrorCount = 0;
                foreach (GridViewRow row in grdinspectionReport.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        CheckBox c = (CheckBox)row.FindControl("ChkBox");
                        if (c.Checked)
                        {
                            Guid AssignJointInspectionUID;
                            if (Request.QueryString["AssignJointInspectionUID"] != null)
                            {
                                AssignJointInspectionUID = new Guid(Request.QueryString["AssignJointInspectionUID"]);
                            }
                            else
                            {
                                AssignJointInspectionUID = Guid.NewGuid();
                            }
                            string InspectionUID = grdinspectionReport.DataKeys[row.RowIndex].Values[0].ToString();

                            int cnt = invoice.InsertJointInspectiontoRAbill(AssignJointInspectionUID, new Guid(Request.QueryString["RABillUid"]), new Guid(Request.QueryString["itemUId"]), new Guid(InspectionUID));
                            if (cnt <= 0)
                            {
                                ErrorCount += 1;
                            }
                            i += 1;
                        }
                    }
                }
                if (i == 0)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Please check atleast one item in the List');</script>");
                }
                else
                {
                    if (ErrorCount == 0)
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
                        
                    }
                    else
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Some Items are not Assigned. Please contact system admin.');</script>");
                    }
                    
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code : A-JI-to-RA-01 there is a problem with these feature. Please contact system admin. Desc : " + ex.Message + "');</script>");
            }
        }

        protected void grdinspectionReport_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            SaveCheckedItems(sender);
            grdinspectionReport.PageIndex = e.NewPageIndex;
            BindJointInspection(Request.QueryString["WorkpackageUID"]);
            CheckSavedItems(sender);
        }

        private void SaveCheckedItems(object sender)
        {

            ArrayList usercontent = new ArrayList();

            Guid index = Guid.Empty;

            foreach (GridViewRow gvrow in grdinspectionReport.Rows)
            {

                //index = gvrow.RowIndex;
                index = new Guid(grdinspectionReport.DataKeys[gvrow.RowIndex].Value.ToString());
                bool result = ((CheckBox)gvrow.FindControl("ChkBox")).Checked;
                // Check in the Session

                if (Session["chkditems"] != null)

                    usercontent = (ArrayList)Session["chkditems"];

                if (result)
                {

                    if (!usercontent.Contains(index))

                        usercontent.Add(index);

                }

                else

                    usercontent.Remove(index);

            }

            if (usercontent != null && usercontent.Count > 0)

                Session["chkditems"] = usercontent;

        }

        private void CheckSavedItems(object sender)
        {

            ArrayList usercontent = (ArrayList)Session["chkditems"];

            if (usercontent != null && usercontent.Count > 0)
            {

                foreach (GridViewRow gvrow in grdinspectionReport.Rows)
                {
                    //int index = gvrow.RowIndex;

                    Guid index = new Guid(grdinspectionReport.DataKeys[gvrow.RowIndex].Value.ToString());

                    if (usercontent.Contains(index))
                    {

                        CheckBox myCheckBox = (CheckBox)gvrow.FindControl("ChkBox");

                        myCheckBox.Checked = true;

                    }

                }

            }

        }

        protected void grdinspectionReport_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (Request.QueryString["View"] != null)
                {
                    e.Row.Cells[0].Visible = false;
                    e.Row.Cells[7].Visible = true;
                }
                else
                {
                    e.Row.Cells[0].Visible = true;
                    e.Row.Cells[7].Visible = false;
                }
            }
                if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //CheckBox c = (CheckBox)e.Row.FindControl("ChkBox");
                if (Request.QueryString["View"] != null)
                {
                    e.Row.Cells[0].Visible = false;
                    e.Row.Cells[7].Visible = true;
                }
                else
                {
                    e.Row.Cells[0].Visible = true;
                    e.Row.Cells[7].Visible = false;
                }
            }
        }

        protected void grdinspectionReport_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void grdinspectionReport_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string UID = e.CommandArgument.ToString();
            if (e.CommandName == "delete")
            {
                int cnt = invoice.AssignedJointInspection_Delete(new Guid(Request.QueryString["itemUId"]), new Guid(UID), new Guid(Request.QueryString["RABillUid"]));
                if (cnt > 0)
                {
                    BindJointInspection(Request.QueryString["itemUId"]);
                }
            }
        }
    }
}