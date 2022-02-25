using ProjectManagementTool.DAL;
using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagementTool._content_pages.rabill_summary
{
    public partial class _default : System.Web.UI.Page
    {
        DBGetData dbgetdata = new DBGetData();
        TaskUpdate gettk = new TaskUpdate();
        Invoice invoice = new Invoice();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Session["Username"] == null)
                {
                    Response.Redirect("~/Login.aspx");
                }
                else
                {
                    if (!IsPostBack)
                    {
                        BindProject();
                        SelectedProjectWorkpackage("Project");
                        //DDlProject_SelectedIndexChanged(sender, e);
                        //BindDataforInvoice_RABills();
                        Session["BOQData"] = null;
                        Session["RABillUID"] = null;
                        Session["RABillNumber"] = null;
                        boqsummary.Visible = false;

                        HideActionButtons();

                        if (Session["Project_Workpackage"] != null)
                        {
                            DDlProject_SelectedIndexChanged(sender, e);
                        }
                        //Commented by Arun 03 Jan 2022
                        //if (Session["RABillWorkpackgeUID"] != null)
                        //{
                        //    string[] sData = Session["RABillWorkpackgeUID"].ToString().Split('\\');
                        //    DDlProject.SelectedValue = sData[0];
                        //    DDlProject_SelectedIndexChanged(sender, e);
                        //    DDLWorkPackage.SelectedValue = sData[1];
                        //    DDLWorkPackage_SelectedIndexChanged(sender, e);
                        //}

                        //Commented by Arun 03 Jan 2022
                        if (Request.QueryString["PrjUID"] != null)
                        {
                            //DDlProject.SelectedValue = Request.QueryString["PrjUID"].ToString();
                            //DDlProject_SelectedIndexChanged(sender, e);

                            //DDLWorkPackage_SelectedIndexChanged(sender, e);
                            btnback.Visible = true;
                        }
                    }
                }

            }
        }

        protected void HideActionButtons()
        {
            DataSet dscheck = new DataSet();
            dscheck = dbgetdata.GetUsertypeFunctionality_Mapping(Session["TypeOfUser"].ToString());
            ViewState["isEdit"] = "false";
            ViewState["isDelete"] = "false";
            AddRABill.Visible = false;
            GrdTreeView.Columns[3].Visible = false;
            GrdTreeView.Columns[4].Visible = false;
            if (dscheck.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dscheck.Tables[0].Rows)
                {
                    if (dr["Code"].ToString() == "RA-A")
                    {
                        AddRABill.Visible = true;
                    }
                    if (dr["Code"].ToString() == "RA-E")
                    {
                        ViewState["isEdit"] = "true";
                        GrdTreeView.Columns[3].Visible = true;
                    }
                    if (dr["Code"].ToString() == "RA-D")
                    {
                        ViewState["isDelete"] = "true";
                        GrdTreeView.Columns[4].Visible = true;
                    }
                }
            }
        }

        private void BindProject()
        {
            DataTable ds = new DataTable();
            if (Session["TypeOfUser"].ToString() == "U" || Session["TypeOfUser"].ToString() == "MD" || Session["TypeOfUser"].ToString() == "VP")
            {
                ds = gettk.GetProjects();
            }
            else if (Session["TypeOfUser"].ToString() == "PA")
            {
                ds = gettk.GetAssignedProjects_by_UserUID(new Guid(Session["UserUID"].ToString()));
            }
            else
            {
                ds = gettk.GetAssignedProjects_by_UserUID(new Guid(Session["UserUID"].ToString()));
            }

            DDlProject.DataTextField = "ProjectName";
            DDlProject.DataValueField = "ProjectUID";
            DDlProject.DataSource = ds;
            DDlProject.DataBind();

            DDlProject.Items.Insert(0, new ListItem("-- Select --", ""));
            DDLWorkPackage.Items.Insert(0, new ListItem("-- Select --", ""));
        }

        protected void DDlProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDlProject.SelectedValue != "")
            {
                DataSet ds = new DataSet();               
                //ds = getdt.GetWorkPackages_By_ProjectUID(new Guid(DDlProject.SelectedValue));
                if (Session["TypeOfUser"].ToString() == "U" || Session["TypeOfUser"].ToString() == "MD" || Session["TypeOfUser"].ToString() == "VP")
                {
                    ds = dbgetdata.GetWorkPackages_By_ProjectUID(new Guid(DDlProject.SelectedValue));
                }
                else if (Session["TypeOfUser"].ToString() == "PA")
                {
                    ds = dbgetdata.GetWorkPackages_ForUser_by_ProjectUID(new Guid(Session["UserUID"].ToString()), new Guid(DDlProject.SelectedValue));
                }
                else
                {
                    ds = dbgetdata.GetWorkPackages_ForUser_by_ProjectUID(new Guid(Session["UserUID"].ToString()), new Guid(DDlProject.SelectedValue));
                }
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DDLWorkPackage.DataTextField = "Name";
                    DDLWorkPackage.DataValueField = "WorkPackageUID";
                    DDLWorkPackage.DataSource = ds;
                    DDLWorkPackage.DataBind();
                    SelectedProjectWorkpackage("Workpackage");
                    DDLWorkPackage_SelectedIndexChanged(sender, e);
                    //DDLWorkPackage.Items.Insert(0, new ListItem("-- Select --", ""));
                    Session["Project_Workpackage"] = DDlProject.SelectedValue + "_" + DDLWorkPackage.SelectedValue;
                }
                else
                {
                    boqsummary.Visible = false;
                    DDLWorkPackage.DataSource = ds;
                    DDLWorkPackage.DataBind();
                }
            }
        }

        protected void DDLWorkPackage_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDLWorkPackage.SelectedValue != "")
            {
                boqsummary.Visible = true;
                BindDataforInvoice_RABills(DDLWorkPackage.SelectedValue);
                AddRABill.HRef = "/_modal_pages/add-rabill-rabillitem-invoice.aspx?Type=RABillAdd&ProjectUID=" + DDlProject.SelectedValue + "&WorkpackageUID=" + DDLWorkPackage.SelectedValue;
                Session["Project_Workpackage"] = DDlProject.SelectedValue + "_" + DDLWorkPackage.SelectedValue;
            }
        }

        private void SelectedProjectWorkpackage(string pType)
        {
            if (!IsPostBack && Session["Project_Workpackage"] != null)
            {
                string[] selectedValue = Session["Project_Workpackage"].ToString().Split('_');
                if (selectedValue.Length > 1)
                {
                    if (pType == "Project")
                    {
                        DDlProject.SelectedValue = selectedValue[0];
                    }
                    else
                    {
                        DDLWorkPackage.SelectedValue = selectedValue[1];
                    }
                }
                else
                {
                    if (pType == "Project")
                    {
                        DDlProject.SelectedValue = Session["Project_Workpackage"].ToString();
                    }
                }
            }

        }

        private void BindDataforInvoice_RABills(string WorkpackageUID)
        {
            //DataTable dt = dbgetdata.GetInvoiceDetails();
            DataTable dt = dbgetdata.GetInvoiceDetails_by_WorkpackageUID(new Guid(WorkpackageUID));
            GrdTreeView.DataSource = dt;
            GrdTreeView.DataBind();
        }

        protected void GrdTreeView_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GrdTreeView.EditIndex = e.NewEditIndex;
            BindDataforInvoice_RABills(DDLWorkPackage.SelectedValue);

        }

        protected void GrdTreeView_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            HiddenField hidRAbuilluid = GrdTreeView.Rows[e.RowIndex].FindControl("hidrabilluid") as HiddenField;
            TextBox  txtrabillnumber = GrdTreeView.Rows[e.RowIndex].FindControl("txtRabill") as TextBox;
          //  TextBox txtdate = GrdTreeView.Rows[e.RowIndex].FindControl("txtdate") as TextBox;
            dbgetdata.udpateRABillDetilas(hidRAbuilluid.Value, txtrabillnumber.Text);
            GrdTreeView.EditIndex = -1;
            BindDataforInvoice_RABills(DDLWorkPackage.SelectedValue);
        }

        protected void GrdTreeView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            HiddenField hidRAbuilluid = GrdTreeView.Rows[e.RowIndex].FindControl("hidrabillDeleteuid") as HiddenField;
            dbgetdata.deleteRABIlls(hidRAbuilluid.Value, new Guid(Session["UserUID"].ToString()));
            BindDataforInvoice_RABills(DDLWorkPackage.SelectedValue);
        }

        protected void GrdTreeView_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GrdTreeView.EditIndex = -1;
            BindDataforInvoice_RABills(DDLWorkPackage.SelectedValue);
        }

        protected void GrdTreeView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (ViewState["isEdit"].ToString() == "false")
                {
                    e.Row.Cells[3].Visible = false;
                }
                if (ViewState["isDelete"].ToString() == "false")
                {
                    e.Row.Cells[4].Visible = false;
                }
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label LblBillValue = (Label)e.Row.FindControl("LblBillValue");
                string RABillUID= GrdTreeView.DataKeys[e.Row.RowIndex].Values[0].ToString();
                //double BillValue = dbgetdata.GetRAbillValue_by_RABillUid(new Guid(RABillUID));
                decimal BillValue = invoice.GetRAbillPresentTotalAmount_by_RABill_UID(new Guid(RABillUID));
                if (BillValue == 0)
                {
                    LblBillValue.Text = "NIL";
                }
                else
                {
                    LblBillValue.Text = BillValue.ToString("#,##.##", CultureInfo.CreateSpecificCulture("en-IN"));
                }

                if (ViewState["isEdit"].ToString() == "false")
                {
                    e.Row.Cells[3].Visible = false;
                }
                if (ViewState["isDelete"].ToString() == "false")
                {
                    e.Row.Cells[4].Visible = false;
                }

                //for db sync check
                if (System.Web.Configuration.WebConfigurationManager.AppSettings["Dbsync"] == "Yes")
                {
                    if (!string.IsNullOrEmpty(RABillUID))
                    {
                        if (dbgetdata.checkRABillsSynced(new Guid(RABillUID)) > 0)
                        {
                            e.Row.BackColor = System.Drawing.Color.White;
                        }
                        else
                        {
                            //  e.Row.BackColor = System.Drawing.Color.Green;
                            // e.Row.ForeColor = System.Drawing.Color.White;
                        }
                    }
                }
            }
        }
    }
}