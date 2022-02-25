using ProjectManagementTool.DAL;
using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;

namespace ProjectManagementTool._content_pages.invoice
{
    public partial class _default : System.Web.UI.Page
    {
        DBGetData getdt = new DBGetData();
        TaskUpdate gettk = new TaskUpdate();
        Invoice invoice = new Invoice();
        string RetHTML = "";
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
                    HideActionButtons();
                    BindProject();
                    SelectedProject();
                    RetHTML = "";
                    //Commented by Arun 03 Jna 2022
                    if (Request.QueryString["PrjUID"] != null)
                    {
                        // DDlProject.SelectedValue = Request.QueryString["PrjUID"].ToString();
                        btnback.Visible = true;

                    }
                    DDlProject_SelectedIndexChanged(sender, e);
                }
              
            }
        }

        private void BindProject()
        {
            DataSet ds = new DataSet();
            if (Session["TypeOfUser"].ToString() == "U" || Session["TypeOfUser"].ToString() == "MD" || Session["TypeOfUser"].ToString() == "VP")
            {
                ds = gettk.GetAllProjects();
            }
            else if (Session["TypeOfUser"].ToString() == "PA")
            {
                ds = getdt.GetAssignedProjects_by_UserUID(new Guid(Session["UserUID"].ToString()));
            }
            else
            {
                ds = getdt.GetAssignedProjects_by_UserUID(new Guid(Session["UserUID"].ToString()));
            }
            if (ds.Tables[0].Rows.Count > 0)
            {
                DDlProject.DataTextField = "ProjectName";
                DDlProject.DataValueField = "ProjectUID";
                DDlProject.DataSource = ds;
                DDlProject.DataBind();
            }
        }

        protected void DDlProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDlProject.SelectedValue != "")
            {
                DataSet ds = new DataSet();
                if (Session["TypeOfUser"].ToString() == "U" || Session["TypeOfUser"].ToString() == "MD" || Session["TypeOfUser"].ToString() == "VP")
                {
                    ds = getdt.GetWorkPackages_By_ProjectUID(new Guid(DDlProject.SelectedValue));
                }
                else if (Session["TypeOfUser"].ToString() == "PA")
                {
                    ds = getdt.GetWorkPackages_ForUser_by_ProjectUID(new Guid(Session["UserUID"].ToString()), new Guid(DDlProject.SelectedValue));
                }
                else
                {
                    ds = getdt.GetWorkPackages_ForUser_by_ProjectUID(new Guid(Session["UserUID"].ToString()), new Guid(DDlProject.SelectedValue));
                }
                if (ds.Tables[0].Rows.Count > 0)
                {
                    InvoiceDiv.Visible = true;
                    TreeView1.Nodes.Clear();
                    PopulateTreeView(ds, null, "", 0);
                    TreeView1.Nodes[0].Selected = true;
                    TreeView1.CollapseAll();
                    TreeView1.Nodes[0].Expand();
                    TreeView1_SelectedNodeChanged(sender, e);
                }
                else
                {
                    TreeView1.Nodes.Clear();
                    InvoiceDiv.Visible = false;
                    
                }
                Session["Project_Workpackage"] = DDlProject.SelectedValue;
            }
            else
            {
                AddInvoice.Visible = false;
            }

        }

        private void SelectedProject()
        {
            if (!IsPostBack)
            {
                if (Session["Project_Workpackage"] != null)
                {
                    string[] selectedValue = Session["Project_Workpackage"].ToString().Split('_');
                    if (selectedValue.Length > 1)
                    {
                        DDlProject.SelectedValue = selectedValue[0];
                    }
                    else
                    {
                        DDlProject.SelectedValue = Session["Project_Workpackage"].ToString();
                    }

                }
            }

        }

        internal void HideActionButtons()
        {
            AddInvoice.Visible = false;
            AddRAbill.Visible = false;
            AddInvoiceDeductions.Visible = false;
            ViewState["isEdit"] = "false";
            ViewState["isDelete"] = "false";
            GrdInvoice.Columns[4].Visible = false;
            GrdInvoice.Columns[5].Visible = false;
            ViewState["isDeductionEdit"] = "false";
            ViewState["isDeductionDelete"] = "false";
            GrdInvoiceDeductions.Columns[3].Visible = false;
            GrdInvoiceDeductions.Columns[4].Visible = false;

            DataSet dscheck = new DataSet();
            dscheck = getdt.GetUsertypeFunctionality_Mapping(Session["TypeOfUser"].ToString());
            if (dscheck.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dscheck.Tables[0].Rows)
                {
                    if (dr["Code"].ToString() == "INA")
                    {
                        AddInvoice.Visible = true;
                    }

                    if (dr["Code"].ToString() == "INE")
                    {
                        GrdInvoice.Columns[4].Visible = true;
                        ViewState["isEdit"] = "true";
                    }

                    if (dr["Code"].ToString() == "IND")
                    {
                        GrdInvoice.Columns[5].Visible = true;
                        ViewState["isDelete"] = "true";
                    }
                    if (dr["Code"].ToString() == "ARABI")
                    {
                        AddRAbill.Visible = true;
                    }
                    if (dr["Code"].ToString() == "IDA")
                    {
                        AddInvoiceDeductions.Visible = true;
                    }

                    if (dr["Code"].ToString() == "IDE")
                    {
                        GrdInvoiceDeductions.Columns[3].Visible = true;
                        ViewState["isDeductionEdit"] = "true";
                    }

                    if (dr["Code"].ToString() == "IDD")
                    {
                        GrdInvoiceDeductions.Columns[4].Visible = true;
                        ViewState["isDeductionDelete"] = "true";
                    }
                }
            }
        }

        public void PopulateTreeView(DataSet dtParent, TreeNode treeNode, string ParentUID, int Level)
        {
            foreach (DataRow row in dtParent.Tables[0].Rows)
            {
                string RABillNumber = "";
                if (Level > 1)
                {
                    RABillNumber = invoice.GetRABillNo_by_InvoiceRABill_UID(new Guid(row["InvoiceRABill_UID"].ToString()));
                }
                TreeNode child = new TreeNode
                {
                    Text = Level == 0 ? LimitCharts(row["Name"].ToString()) : Level == 1 ? LimitCharts(row["Invoice_Number"].ToString()) : RABillNumber,
                    Value = Level == 0 ? row["WorkPackageUID"].ToString() : Level == 1 ? row["InvoiceMaster_UID"].ToString() : row["InvoiceRABill_UID"].ToString(),
                    Target = Level == 0 ? "WorkPackage" : Level == 1 ? "Invoice" : "RA Bill",
                    ToolTip = Level == 0 ? LimitCharts(row["Name"].ToString()) : Level == 1 ? LimitCharts(row["Invoice_Number"].ToString()) : RABillNumber,
                };

                if (ParentUID == "")
                {                    
                    TreeView1.Nodes.Add(child);
                    DataSet dschild = invoice.GetInvoiceMaster_by_WorkpackageUID(new Guid(child.Value));
                    if (dschild.Tables[0].Rows.Count > 0)
                    {
                        PopulateTreeView(dschild, child, child.Value, 1);
                    }

                }
                else if (Level == 1)
                {
                    treeNode.ChildNodes.Add(child);
                    DataSet dssubchild = invoice.GetInvoiceRABills_by_InvoiceMaster_UID(new Guid(child.Value));
                    if (dssubchild.Tables[0].Rows.Count > 0)
                    {
                        PopulateTreeView(dssubchild, child, child.Value, 2);
                    }
                }
                else
                {
                    treeNode.ChildNodes.Add(child);
                   
                }
            }

        }

        protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
        {
            BindData();
        }
        private void BindData()
        {
            if (TreeView1.SelectedNode.Target == "WorkPackage")
            {
                InvoiceDiv.Visible = true;
                InvoiceDeductions.Visible = false;
                RABillsDiv.Visible = false;
                InvoiceDetails.Visible = false;
                DataSet ds = invoice.GetInvoiceMaster_by_WorkpackageUID(new Guid(TreeView1.SelectedNode.Value));
                GrdInvoice.DataSource = ds;
                GrdInvoice.DataBind();
                AddInvoice.HRef = "/_modal_pages/add-invoicemaster.aspx?PrjUID=" + DDlProject.SelectedValue + "&WorkUID=" + TreeView1.SelectedNode.Value;
                ActivityHeading.Text = "Invoice List for " + TreeView1.SelectedNode.Text;
                BindAllInvoiceTotal(TreeView1.SelectedNode.Value);
                btnRABillPrint.Visible = false;
            }
            else if (TreeView1.SelectedNode.Target == "Invoice")
            {
                InvoiceDeductions.Visible = true;
                RABillsDiv.Visible = true;
                InvoiceDiv.Visible = false;
                InvoiceDetails.Visible = true;
                DataSet ds = invoice.GetInvoiceDeduction_by_InvoiceMaster_UID(new Guid(TreeView1.SelectedNode.Value));
                GrdInvoiceDeductions.DataSource = ds;
                GrdInvoiceDeductions.DataBind();
                BindInvoiceMaster(TreeView1.SelectedNode.Value);
                BindRAbills(TreeView1.SelectedNode.Value);
                //AddRAbill.Visible = true;
                AddRAbill.HRef = "/_modal_pages/add-rabill-rabillitem-invoice.aspx?InvoiceMaster_UID=" + TreeView1.SelectedNode.Value + "&WorkUID=" + TreeView1.SelectedNode.Parent.Value;
                if (GrdInvoiceDeductions.Rows.Count == 0)
                {
                    AddInvoiceDeductions.HRef = "/_modal_pages/add-invoicededuction.aspx?InvoiceMaster_UID=" + TreeView1.SelectedNode.Value + "&WorkUID=" + TreeView1.SelectedNode.Parent.Value + "&Mobilization=true";
                }
                else
                {
                    AddInvoiceDeductions.HRef = "/_modal_pages/add-invoicededuction.aspx?InvoiceMaster_UID=" + TreeView1.SelectedNode.Value + "&WorkUID=" + TreeView1.SelectedNode.Parent.Value + "&Mobilization=false";
                }
                BindInvoiceMasterPrevBillTotal(TreeView1.SelectedNode.Parent.Value, TreeView1.SelectedNode.Value);
                btnRABillPrint.Visible = false;
            }
            else
            {
                InvoiceDeductions.Visible = false;
                RABillsDiv.Visible = true;
                InvoiceDiv.Visible = false;
                InvoiceDetails.Visible = false;
                AddRAbill.Visible = false;
                DataSet ds = invoice.GetInvoiceRAbills_by_InvoiceRABill_UID(new Guid(TreeView1.SelectedNode.Value));
                GrdRABillItems.DataSource = ds;
                GrdRABillItems.DataBind();

                if (ds.Tables[0].Rows.Count > 0)
                {
                    btnRABillPrint.Visible = true;
                }
            }
        }

        private void BindAllInvoiceTotal(string WorkpackageUID)
        {
            DataSet ds = invoice.GetAllInvoiceTotalAmount_by_WorkpackageUID(new Guid(WorkpackageUID));
            if (ds.Tables[0].Rows.Count > 0)
            {
                LblAllInvoiceTotal.Text = ds.Tables[0].Rows[0]["Currency"].ToString() + " " + Convert.ToDouble(ds.Tables[0].Rows[0]["TotalAmount"].ToString()).ToString("#,##.00", CultureInfo.CreateSpecificCulture(ds.Tables[0].Rows[0]["Currency_CultureInfo"].ToString()));
                LblAllInvoiceDeductionTotal.Text = ds.Tables[0].Rows[0]["Currency"].ToString() + " " + Convert.ToDouble(ds.Tables[0].Rows[0]["TotalDeductionAmount"].ToString()).ToString("#,##.00", CultureInfo.CreateSpecificCulture(ds.Tables[0].Rows[0]["Currency_CultureInfo"].ToString()));
                LblAllInvoiceNetTotal.Text = ds.Tables[0].Rows[0]["Currency"].ToString() + " " + Convert.ToDouble(ds.Tables[0].Rows[0]["TotalNetAmount"].ToString()).ToString("#,##.00", CultureInfo.CreateSpecificCulture(ds.Tables[0].Rows[0]["Currency_CultureInfo"].ToString()));
            }
            else
            {
                LblAllInvoiceTotal.Text = "-";
                LblAllInvoiceDeductionTotal.Text = "-";
                LblAllInvoiceNetTotal.Text = "-";
            }
        }
        public string LimitCharts(string Desc)
        {
            if (Desc.Length > 100)
            {
                return Desc.Substring(0, 100) + "  . . .";
            }
            else
            {
                return Desc;
            }
        }

        //public string GetInvoiceNumber(string InvoiceMaster_UID)
        //{
        //    return invoice.GetInvoiceNumber_by_InvoiceMaster_UID(new Guid(InvoiceMaster_UID));
        //}
        private void BindRAbills(string InvoiceMaster_UID)
        {
            DataSet ds = invoice.GetInvoiceRABills_by_InvoiceMaster_UID(new Guid(InvoiceMaster_UID));
            GrdRABillItems.DataSource = ds;
            GrdRABillItems.DataBind();
        }
        private void BindInvoiceMaster(string InvoiceMaster_UID)
        {
            DataSet ds = invoice.GetInvoiceMaster_by_InvoiceMaster_UID(new Guid(InvoiceMaster_UID));
            if (ds.Tables[0].Rows.Count > 0)
            {
                LblInvoiceNumber.Text = ds.Tables[0].Rows[0]["Invoice_Number"].ToString();
                if (ds.Tables[0].Rows[0]["Invoice_Date"].ToString() != "")
                {
                    LblInvoiceDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["Invoice_Date"].ToString()).ToString("dd MMM yyyy");
                }
                if (ds.Tables[0].Rows[0]["Currency"].ToString() == "&#x20B9;")
                {
                    HiddenCurrencySymbol.Value = "Rs.";
                }
                else if (ds.Tables[0].Rows[0]["Currency"].ToString() == "&#36;")
                {
                    HiddenCurrencySymbol.Value = "$";
                }
                else
                {
                    HiddenCurrencySymbol.Value = "¥";
                }

                    HiddenCulturalInfo.Value = ds.Tables[0].Rows[0]["Currency_CultureInfo"].ToString();
                Hidden1InvoiceTotal.Value = ds.Tables[0].Rows[0]["Invoice_TotalAmount"].ToString();
                LblInvoiceTotalAmount.Text = ds.Tables[0].Rows[0]["Currency"].ToString() +" "+ Convert.ToDouble(ds.Tables[0].Rows[0]["Invoice_TotalAmount"].ToString()).ToString("#,##.00", CultureInfo.CreateSpecificCulture(ds.Tables[0].Rows[0]["Currency_CultureInfo"].ToString()));
                LblInvoiceDeductionAmount.Text = ds.Tables[0].Rows[0]["Currency"].ToString() + " " + Convert.ToDouble(ds.Tables[0].Rows[0]["Invoice_DeductionAmount"].ToString()).ToString("#,##.00", CultureInfo.CreateSpecificCulture(ds.Tables[0].Rows[0]["Currency_CultureInfo"].ToString()));

                LblNetAmount.Text= ds.Tables[0].Rows[0]["Currency"].ToString() + " " + (Convert.ToDouble(ds.Tables[0].Rows[0]["Invoice_TotalAmount"].ToString()) - Convert.ToDouble(ds.Tables[0].Rows[0]["Invoice_DeductionAmount"].ToString())).ToString("#,##.00", CultureInfo.CreateSpecificCulture(ds.Tables[0].Rows[0]["Currency_CultureInfo"].ToString()));
            }
        }

        private void BindInvoiceMasterPrevBillTotal(string WorkpackageUID,string InvoiceMaster_UID)
        {
            DataSet ds = invoice.GetInvoiceTotalAmountUptoPrev_Invoice(new Guid(WorkpackageUID), new Guid(InvoiceMaster_UID));
            if (ds.Tables[0].Rows.Count > 0)
            {
                Hidden2UptoPrevInvoiceTotal.Value = ds.Tables[0].Rows[0]["TotalAmount"].ToString();
                LblUptoPrevTotal.Text = ds.Tables[0].Rows[0]["Currency"].ToString() + " " + Convert.ToDouble(ds.Tables[0].Rows[0]["TotalAmount"].ToString()).ToString("#,##.00", CultureInfo.CreateSpecificCulture(ds.Tables[0].Rows[0]["Currency_CultureInfo"].ToString()));
                LblUptoPrevDeeductionTotal.Text = ds.Tables[0].Rows[0]["Currency"].ToString() + " " + Convert.ToDouble(ds.Tables[0].Rows[0]["TotalDeductionAmount"].ToString()).ToString("#,##.00", CultureInfo.CreateSpecificCulture(ds.Tables[0].Rows[0]["Currency_CultureInfo"].ToString()));
                LblUptoPrevNetTotal.Text = ds.Tables[0].Rows[0]["Currency"].ToString() + " " + Convert.ToDouble(ds.Tables[0].Rows[0]["TotalNetAmount"].ToString()).ToString("#,##.00", CultureInfo.CreateSpecificCulture(ds.Tables[0].Rows[0]["Currency_CultureInfo"].ToString()));
            }
            else
            {
                LblUptoPrevTotal.Text = "-";
                LblUptoPrevDeeductionTotal.Text = "-";
                LblUptoPrevNetTotal.Text = "-";
            }
        }
        public string GetDeductionMaster(string UID)
        {
            return getdt.GetDeductionMasterName_by_UID(new Guid(UID));
        }

        protected void GrdInvoice_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (ViewState["isEdit"].ToString() == "false")
                {
                    e.Row.Cells[4].Visible = false;
                }
                if (ViewState["isDelete"].ToString() == "false")
                {
                    e.Row.Cells[5].Visible = false;
                }
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (ViewState["isEdit"].ToString() == "false")
                {
                    e.Row.Cells[4].Visible = false;
                }
                if (ViewState["isDelete"].ToString() == "false")
                {
                    e.Row.Cells[5].Visible = false;
                }

                //
                //for db sync check
                if (System.Web.Configuration.WebConfigurationManager.AppSettings["Dbsync"] == "Yes")
                {
                    if (!string.IsNullOrEmpty(e.Row.Cells[6].Text))
                    {
                        if (getdt.checkInvoiceMasterSynced(new Guid(e.Row.Cells[6].Text)) > 0)
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

        protected void GrdInvoice_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string UID = e.CommandArgument.ToString();
            if (e.CommandName == "delete")
            {
                int cnt = invoice.InvoiceMaster_Delete(new Guid(UID), new Guid(Session["UserUID"].ToString()));
                if (cnt > 0)
                {
                    BindData();
                }
            }
        }

        protected void GrdInvoice_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void GrdInvoice_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdInvoice.PageIndex = e.NewPageIndex;
            BindData();
        }

        protected void GrdInvoiceDeductions_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (ViewState["isDeductionEdit"].ToString() == "false")
                {
                    e.Row.Cells[3].Visible = false;
                }
                if (ViewState["isDeductionDelete"].ToString() == "false")
                {
                    e.Row.Cells[4].Visible = false;
                }
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (ViewState["isDeductionEdit"].ToString() == "false")
                {
                    e.Row.Cells[3].Visible = false;
                }
                if (ViewState["isDeductionDelete"].ToString() == "false")
                {
                    e.Row.Cells[4].Visible = false;
                }
                //for db sync check
                if (System.Web.Configuration.WebConfigurationManager.AppSettings["Dbsync"] == "Yes")
                {
                    if (!string.IsNullOrEmpty(e.Row.Cells[5].Text))
                    {
                        if (getdt.checkInvoice_DeductionSynced(new Guid(e.Row.Cells[5].Text)) > 0)
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

        protected void GrdInvoiceDeductions_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string UID = e.CommandArgument.ToString();
            if (e.CommandName == "delete")
            {
                int cnt = invoice.InvoiceDeduction_Delete(new Guid(UID), new Guid(Session["UserUID"].ToString()));
                if (cnt > 0)
                {
                    BindData();
                }
            }
        }

        protected void GrdInvoiceDeductions_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void GrdRABillItems_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label BillValue = (Label)e.Row.FindControl("LblBillValue");
                string RABillUid = GrdRABillItems.DataKeys[e.Row.RowIndex].Values[0].ToString();
                decimal bValue = invoice.GetRAbillPresentTotalAmount_by_RABill_UID(new Guid(RABillUid));
                BillValue.Text= bValue.ToString("#,##.00", CultureInfo.CreateSpecificCulture("en-IN"));
                //for db sync check
                if (System.Web.Configuration.WebConfigurationManager.AppSettings["Dbsync"] == "Yes")
                {
                    if (!string.IsNullOrEmpty(e.Row.Cells[3].Text))
                    {
                        if (getdt.checkInvoiceRABillSynced(new Guid(e.Row.Cells[3].Text)) > 0)
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

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                DataSet ds = getdt.GetContractor_By_WorkpackageUID(new Guid(TreeView1.SelectedNode.Parent.Value));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    //LblContractorName.Text = ds.Tables[0].Rows[0]["Contractor_Name"].ToString();
                    //LblContractorAddress.Text = ds.Tables[0].Rows[0]["Contractor_Address"].ToString();
                    //LblContractValue.Text = ds.Tables[0].Rows[0]["Currency"].ToString() + " " + Convert.ToDouble(ds.Tables[0].Rows[0]["Contract_Value"].ToString()).ToString("#,##.00", CultureInfo.CreateSpecificCulture(ds.Tables[0].Rows[0]["Currency_CultureInfo"].ToString()));
                    //LblContractDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["Contract_Agreement_Date"].ToString()).ToString("dd MMM yyyy");
                    //LblPrintInvoiceNumber.Text = LblInvoiceNumber.Text;
                    //LblPrintInvoiceDate.Text = LblInvoiceDate.Text;

                    DataSet ds1 = getdt.GetWorkPackages_By_WorkPackageUID(new Guid(TreeView1.SelectedNode.Parent.Value));
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        //LblNameoftheEmployer.Text = ds1.Tables[0].Rows[0]["WorkPackage_Client"].ToString();
                        //LblNameoftheWork.Text = ds1.Tables[0].Rows[0]["Name"].ToString();
                    }

                    double PrevBillAmount = 0;
                    if (Hidden2UptoPrevInvoiceTotal.Value != "")
                    {
                        PrevBillAmount = Convert.ToDouble(Hidden2UptoPrevInvoiceTotal.Value);
                    }
                    string HTMLstring = "<html><head><meta http-equiv='Content-Type' content='text/html; charset=utf-8' /></head> <body>" +
                    "<div style='width:100%; margin:auto;'><div style='width:100%; float:left;'>";
                    HTMLstring += "<div style='float:left; width:100%;text-align:center;'><h3>Memo of Payment</h3><br /><br /></div>";
                    HTMLstring += "<div style = 'float:left; width:100%;'>";
                    HTMLstring += "<table border='1'><tr><td colspan='2'><h4>Name & Address of the Contractor</h4></td></tr><tr><td colspan='2'><h5>" + ds.Tables[0].Rows[0]["Contractor_Name"].ToString() + "</h5><h5>" + ds.Tables[0].Rows[0]["Contractor_Address"].ToString() + "</h5></td></tr><tr><td><h5>Invoice  :  " + LblInvoiceNumber.Text + "</h5></td><td><h5>Date : " + LblInvoiceDate.Text + "</h5></td></tr></table>";
                    HTMLstring += "<table border='1'><tr><td><h5>Name of Employer</h5></td><td><h5>" + ds1.Tables[0].Rows[0]["WorkPackage_Client"].ToString() + "</h5></td></tr><tr><td><h5>Name of the Work</h5></td><td><h5>" + ds1.Tables[0].Rows[0]["Name"].ToString() + "</h5></td></tr><tr><td><h5>Contract Date</h5></td><td><h5>" + Convert.ToDateTime(ds.Tables[0].Rows[0]["Contract_Agreement_Date"].ToString()).ToString("dd MMM yyyy") + "</h5></td></tr><tr><td><h5>Contract Value</h5></td><td><h5>" + HiddenCurrencySymbol.Value + " " + Convert.ToDouble(ds.Tables[0].Rows[0]["Contract_Value"].ToString()).ToString("#,##.00", CultureInfo.CreateSpecificCulture(ds.Tables[0].Rows[0]["Currency_CultureInfo"].ToString())) + "</h5></td></tr></table>";
                    HTMLstring += "<table border='1'><tr><td></td><td><h5>Upto Previous Bill Amount</h5></td><td><h5>This Bill Amount</h5></td><td><h5>Total Amount</h5></td></tr>" +
                        "<tr><td></td><td><h5>" + HiddenCurrencySymbol.Value + " " + PrevBillAmount.ToString("#,##.00", CultureInfo.CreateSpecificCulture(HiddenCulturalInfo.Value)) + "</h5></td><td><h5>" + HiddenCurrencySymbol.Value + " " + Convert.ToDouble(Hidden1InvoiceTotal.Value).ToString("#,##.00", CultureInfo.CreateSpecificCulture(HiddenCulturalInfo.Value)) + "</h5></td><td><h5>" + HiddenCurrencySymbol.Value + " " + Convert.ToDouble(Convert.ToDouble(Hidden1InvoiceTotal.Value) + PrevBillAmount).ToString("#,##.00", CultureInfo.CreateSpecificCulture(HiddenCulturalInfo.Value)) + "</h5></td></tr>" +
                        "<tr><td><h5>Recoveries / Deductions : -</h5></td><td></td><td></td><td></td></tr>";
                    double TotPrevDeduction = 0;
                    double TotCurrentDedcution = 0;
                    double TotNetDeduction = 0;
                    DataSet dsDeduction = invoice.GetPrev_CurrentInvoiceDeduction(new Guid(TreeView1.SelectedNode.Parent.Value), new Guid(TreeView1.SelectedNode.Value));
                    if (dsDeduction.Tables.Count > 0)
                    {
                        for (int i = 0; i < dsDeduction.Tables[0].Rows.Count; i++)
                        {
                            TotPrevDeduction += Convert.ToDouble(dsDeduction.Tables[1].Rows[i]["Amount"].ToString());
                            TotCurrentDedcution += Convert.ToDouble(dsDeduction.Tables[0].Rows[i]["Amount"].ToString());
                            TotNetDeduction += Convert.ToDouble(dsDeduction.Tables[1].Rows[i]["Amount"].ToString()) + Convert.ToDouble(dsDeduction.Tables[0].Rows[i]["Amount"].ToString());
                            HTMLstring += "<tr><td><h5>" + dsDeduction.Tables[0].Rows[i]["DeductionsDescription"].ToString() + "</h5></td><td>"+ Convert.ToDouble(dsDeduction.Tables[1].Rows[i]["Amount"].ToString()).ToString("#,##.00", CultureInfo.CreateSpecificCulture(HiddenCulturalInfo.Value)) + "</td><td>"+ Convert.ToDouble(dsDeduction.Tables[0].Rows[i]["Amount"].ToString()).ToString("#,##.00", CultureInfo.CreateSpecificCulture(HiddenCulturalInfo.Value)) + "</td><td>"+ (Convert.ToDouble(dsDeduction.Tables[1].Rows[i]["Amount"].ToString()) + Convert.ToDouble(dsDeduction.Tables[0].Rows[i]["Amount"].ToString())).ToString("#,##.00", CultureInfo.CreateSpecificCulture(HiddenCulturalInfo.Value)) + "</td></tr>";
                        }
                        HTMLstring += "<tr><td><h5>Total Recoveries</h5></td><td>" + TotPrevDeduction.ToString("#,##.00", CultureInfo.CreateSpecificCulture(HiddenCulturalInfo.Value)) + "</td><td>" + TotCurrentDedcution.ToString("#,##.00", CultureInfo.CreateSpecificCulture(HiddenCulturalInfo.Value)) + "</td><td>"+ TotNetDeduction.ToString("#,##.00", CultureInfo.CreateSpecificCulture(HiddenCulturalInfo.Value)) + "</td></tr>";
                        HTMLstring += "<tr><td></td><td></td><td></td><td></td></tr>";
                        HTMLstring += "<tr><td><h5>Net after Recoveries</h5></td><td>"+ (PrevBillAmount - TotPrevDeduction).ToString("#,##.00", CultureInfo.CreateSpecificCulture(HiddenCulturalInfo.Value)) + "</td><td>"+ (Convert.ToDouble(Hidden1InvoiceTotal.Value) - TotCurrentDedcution).ToString("#,##.00", CultureInfo.CreateSpecificCulture(HiddenCulturalInfo.Value)) + "</td><td>"+ ((Convert.ToDouble(Hidden1InvoiceTotal.Value) + PrevBillAmount) - TotNetDeduction).ToString("#,##.00", CultureInfo.CreateSpecificCulture(HiddenCulturalInfo.Value)) + "</td></tr>";
                        HTMLstring += "<tr><td></td><td></td><td></td><td></td></tr>";
                        HTMLstring += "<tr style='text-align:right;'><td colspan='4'><h5>for "+ ds.Tables[0].Rows[0]["Contractor_Name"].ToString() + "</h5> <br/><br/><br/><h5>Authorised Signatory</h5></td></tr>";

                    }
                    HTMLstring += "</table>";
                    HTMLstring += "</div>";
                    HTMLstring += "</div></body></html>";
                    using (StringWriter sw = new StringWriter())
                    {
                        using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                        {
                            StringBuilder sb = new StringBuilder();

                            //Export HTML String as PDF.
                            StringReader sr = new StringReader(HTMLstring);
                            Document pdfDoc = new Document(PageSize.A4, 25f, 25f, 35f, 25f);

                            iTextSharp.text.Font foot = new iTextSharp.text.Font();
                            foot.Size = 10;

                            
                            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                            //PdfWriter writer = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                            PdfWriter.GetInstance(pdfDoc, new FileStream(Server.MapPath("~") + "/RegExcel/printpdf.pdf", FileMode.Create));
                            HeaderFooter pdfFooter = new HeaderFooter(new Phrase("Date : " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + "   Page: ", foot), new Phrase(Environment.NewLine + " ", foot));
                            pdfFooter.Alignment = Element.ALIGN_CENTER;
                            pdfDoc.Footer = pdfFooter;
                            pdfDoc.Open();
                            htmlparser.Parse(sr);
                            pdfDoc.Close();
                            //Response.ContentType = "application/pdf";
                            //Response.AddHeader("content-disposition", "attachment;filename=Report_ProjectProgress_" + DateTime.Now.Ticks + ".pdf");
                            //Response.Cache.SetCacheability(HttpCacheability.NoCache);
                            //Response.Write(pdfDoc);
                            //Response.End();

                        }
                    }
                    Session["Print"] = true;
                    Response.Write("<script>window.open ('/PDFPRint.aspx','_blank');</script>");
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        protected void btnRABillPrint_Click(object sender, EventArgs e)
        {
            try
            {
                DataSet ds = getdt.GetContractor_By_WorkpackageUID(new Guid(TreeView1.SelectedNode.Parent.Parent.Value));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataSet ds1 = getdt.GetWorkPackages_By_WorkPackageUID(new Guid(TreeView1.SelectedNode.Parent.Parent.Value));
                    string HTMLstring = "<html><head><meta http-equiv='Content-Type' content='text/html; charset=utf-8' /></head> <body>" +
                    "<div style='width:100%; margin:auto;'><div style='width:100%; float:left;'>";
                    HTMLstring += "<div style='float:left; width:100%;text-align:center;'><h3>COST ABSTRACT</h3><br /><br /></div>";
                    HTMLstring += "<div style = 'float:left; width:100%;'>";
                    HTMLstring += "<table border='1'><tr><td><h5>Name of the Work</h5></td><td><h5>" + ds1.Tables[0].Rows[0]["WorkPackage_Client"].ToString() + "</h5></td></tr><tr><td><h5>Package No:</h5></td><td><h5>" + ds.Tables[0].Rows[0]["ProjectSpecific_Number"].ToString() + "</h5></td></tr><tr><td><h5>Contractor</h5></td><td><h5>" + ds.Tables[0].Rows[0]["Contractor_Name"].ToString() + "</h5></td></tr><tr><td><h5>LOA No & Date</h5></td><td><h5>" + Convert.ToDateTime(ds.Tables[0].Rows[0]["Letter_of_Acceptance"].ToString()).ToString("dd MMM yyyy") + "</h5></td></tr><tr><td><h5>Date of Commencement</h5></td><td><h5>" + Convert.ToDateTime(ds.Tables[0].Rows[0]["Contract_StartDate"].ToString()).ToString("dd MMM yyyy") + "</h5></td></tr><tr><td><h5>Date of Measurment</h5></td><td><h5>-</h5></td></tr><tr><td><h5>R.A Bill No</h5></td><td><h5>" + TreeView1.SelectedNode.Text + "</h5></td></tr></table>";
                    HTMLstring += "<br/><br/></div>";
                    HTMLstring += "<div style = 'float:left; width:100%;'>";
                    HTMLstring += "<table border='1'>";
                    HTMLstring += "<tr><td style='text-align:center;'><h4>Item No.</h4></td><td><h4>Description</h4></td><td><h4 style='text-align:center'>Unit</h4></td><td style='text-align:center'><h4>Quantity</h4></td><td style='text-align:center'><h4>Approved Rate</h4></td><td><h4>Prev. Quantity</h4></td><td><h4>Prev. Amount</h4></td><td><h4>Current Quantity</h4></td><td><h4>Current.Amount</h4></td></tr>";

                    DataTable BOQParent = getdt.getBOQParent_Details(new Guid(DDlProject.SelectedValue), "Project");
                    for (int i = 0; i < BOQParent.Rows.Count; i++)
                    {
                        RetHTML = "";

                        HTMLstring += BindRABillItems(BOQParent.Rows[i]["BOQDetailsUID"].ToString(), TreeView1.SelectedNode.Parent.Parent.Value, TreeView1.SelectedNode.Value);
                    }
                    //    DataTable dtRaBills = getdt.GetRABills(TreeView1.SelectedNode.Value);
                    //if (dtRaBills.Rows.Count > 0)
                    //{
                        
                    //    for (int i = 0; i < dtRaBills.Rows.Count; i++)
                    //    {
                    //        DataSet BoqData = getdt.GetBOQDetails_by_BOQDetailsUID(new Guid(dtRaBills.Rows[i]["itemUId"].ToString()));
                    //        if (BoqData.Tables[0].Rows.Count > 0)
                    //        {
                    //            HTMLstring += "<tr><td><h5>" + dtRaBills.Rows[i]["item_number"].ToString() + "</h5></td><td><h5>" + dtRaBills.Rows[i]["item_desc"].ToString() + "</h5></td><td><h5>" + BoqData.Tables[0].Rows[0]["Unit"].ToString() + "</h5></td><td><h5>" + BoqData.Tables[0].Rows[0]["Quantity"].ToString() + "</h5></td><td><h5>" + Convert.ToDouble(BoqData.Tables[0].Rows[0]["INR-Rate"].ToString()).ToString("#,##.00", CultureInfo.CreateSpecificCulture("en-IN")) + "</h5></td><td><h5>Rs. " + (Convert.ToDouble(BoqData.Tables[0].Rows[0]["INR-Rate"].ToString()) * Convert.ToDouble(BoqData.Tables[0].Rows[0]["Quantity"].ToString())).ToString("#,##.00", CultureInfo.CreateSpecificCulture("en-IN")) + "</h5></td></tr>";
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    //    HTMLstring += "<tr><td><h5>No Data Found</td></tr>";
                    //}

                    HTMLstring += "</table>";
                    HTMLstring += "</div>";
                    HTMLstring += "</div></body></html>";
                    using (StringWriter sw = new StringWriter())
                    {
                        using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                        {
                            StringBuilder sb = new StringBuilder();

                            //Export HTML String as PDF.
                            StringReader sr = new StringReader(HTMLstring);
                            Document pdfDoc = new Document(PageSize.A4.Rotate(), 10f, 10f, 10f, 10f);

                            iTextSharp.text.Font foot = new iTextSharp.text.Font();
                            foot.Size = 10;


                            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                            //PdfWriter writer = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                            PdfWriter.GetInstance(pdfDoc, new FileStream(Server.MapPath("~") + "/RegExcel/printpdf.pdf", FileMode.Create));
                            HeaderFooter pdfFooter = new HeaderFooter(new Phrase("Date : " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + "   Page: ", foot), new Phrase(Environment.NewLine + " ", foot));
                            pdfFooter.Alignment = Element.ALIGN_CENTER;
                            pdfDoc.Footer = pdfFooter;
                            pdfDoc.Open();
                            htmlparser.Parse(sr);
                            pdfDoc.Close();
                            //Response.ContentType = "application/pdf";
                            //Response.AddHeader("content-disposition", "attachment;filename=Report_ProjectProgress_" + DateTime.Now.Ticks + ".pdf");
                            //Response.Cache.SetCacheability(HttpCacheability.NoCache);
                            //Response.Write(pdfDoc);
                            //Response.End();

                        }
                    }
                    Session["Print"] = true;
                    Response.Write("<script>window.open ('/PDFPRint.aspx','_blank');</script>");
                }

            }
            catch (Exception ex)
            {

            }
        }

        private string BindRABillItems(string ParentUID, string WorkpackageUID, string RAbillUID)
        {
            DataTable dschild = getdt.getBoq_Details(new Guid(ParentUID));
            for (int j = 0; j < dschild.Rows.Count; j++)
            {
                string CurrencySymbol = "";
                if (dschild.Rows[j]["Currency"].ToString() == "&#x20B9;")
                {

                    CurrencySymbol = "Rs.";
                }
                else if (dschild.Rows[j]["Currency"].ToString() == "&#36;")
                {

                    CurrencySymbol = "USD";
                }
                else
                {

                    CurrencySymbol = "YEN";
                }
                string Cul_Info = "";
                if (dschild.Rows[j]["Currency_CultureInfo"].ToString() != "")
                {
                    Cul_Info = dschild.Rows[j]["Currency_CultureInfo"].ToString();
                }
                else
                {
                    Cul_Info = "en-IN";
                }
                if (dschild.Rows[j]["Quantity"].ToString() == "0")
                {
                    RetHTML += "<tr><td><h5>" + dschild.Rows[j]["Item_Number"].ToString() + "</h5></td><td><h5>" + dschild.Rows[j]["Description"].ToString() + "</h5></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>";
                }
                else
                {
                    double TR = 0;
                    double CurrentQuantity = invoice.GetTotalJointInspectionQuantity_by_RAbillItem(new Guid(dschild.Rows[j]["BOQDetailsUID"].ToString()), new Guid(invoice.GetRABillUID_by_InvoiceRABill_UID(new Guid(RAbillUID))));
                    if (CurrentQuantity > 0)
                    {
                        DataSet dsTerms = getdt.GetPaymentBreakupTerms_BOQDetailsUID(new Guid(dschild.Rows[j]["BOQDetailsUID"].ToString()));
                        if (dsTerms.Tables[0].Rows.Count > 0)
                        {
                            TR = (Convert.ToDouble(dschild.Rows[j]["INR-Rate"].ToString()) - ((Convert.ToDouble(dschild.Rows[j]["INR-Rate"].ToString()) * float.Parse(dsTerms.Tables[0].Rows[0]["Percentage"].ToString())) / 100));
                            string PaymentBreakup = dsTerms.Tables[0].Rows[0]["Percentage"].ToString() + "% " + dsTerms.Tables[0].Rows[0]["Terms_Desc"].ToString();
                            string BOQRate = CurrencySymbol + " " + (Convert.ToDouble(dschild.Rows[j]["INR-Rate"].ToString()) - ((Convert.ToDouble(dschild.Rows[j]["INR-Rate"].ToString()) * float.Parse(dsTerms.Tables[0].Rows[0]["Percentage"].ToString())) / 100)).ToString("#,##.##", CultureInfo.CreateSpecificCulture(Cul_Info));
                            string CurrentAmount= CurrencySymbol + " " + (CurrentQuantity * TR).ToString("#,##.##", CultureInfo.CreateSpecificCulture(Cul_Info));
                            RetHTML += "<tr><td><h5>" + dschild.Rows[j]["Item_Number"].ToString() + "</h5></td><td><h5>" + dschild.Rows[j]["Description"].ToString() + "</h5></td><td><h5>" + dschild.Rows[j]["Unit"].ToString() + "</h5></td><td><h5>" + dschild.Rows[j]["Quantity"].ToString() + "</h5></td><td><h5>" + Convert.ToDouble(dschild.Rows[j]["INR-Rate"].ToString()).ToString("#,##.00", CultureInfo.CreateSpecificCulture("en-IN")) + "</h5></td><td><h5>-</h5></td><td><h5>-</h5></td><td></td><td></td></tr>";
                            RetHTML += "<tr><td></td><td>" + PaymentBreakup + "</td><td></td><td></td><td>" + BOQRate + "</td><td><h5>-</h5></td><td><h5>-</h5></td><td><h5>" + CurrentQuantity + "</h5></td><td><h5>" + CurrentAmount + "</h5></td></tr>";
                        }
                        else
                        {
                            string CurrentAmount = CurrencySymbol + " " + (CurrentQuantity * Convert.ToDouble(dschild.Rows[j]["INR-Rate"].ToString())).ToString("#,##.##", CultureInfo.CreateSpecificCulture(Cul_Info));
                            RetHTML += "<tr><td><h5>" + dschild.Rows[j]["Item_Number"].ToString() + "</h5></td><td><h5>" + dschild.Rows[j]["Description"].ToString() + "</h5></td><td><h5>" + dschild.Rows[j]["Unit"].ToString() + "</h5></td><td><h5>" + dschild.Rows[j]["Quantity"].ToString() + "</h5></td><td><h5>" + Convert.ToDouble(dschild.Rows[j]["INR-Rate"].ToString()).ToString("#,##.00", CultureInfo.CreateSpecificCulture("en-IN")) + "</h5></td><td><h5>-</h5></td><td><h5>-</h5></td><td><h5>" + CurrentQuantity + "</h5></td><td><h5>" + CurrentAmount + "</h5></td></tr>";
                        }
                        
                    }
                    else
                    {
                        RetHTML += "<tr><td><h5>" + dschild.Rows[j]["Item_Number"].ToString() + "</h5></td><td><h5>" + dschild.Rows[j]["Description"].ToString() + "</h5></td><td><h5>" + dschild.Rows[j]["Unit"].ToString() + "</h5></td><td><h5>" + dschild.Rows[j]["Quantity"].ToString() + "</h5></td><td><h5>" + Convert.ToDouble(dschild.Rows[j]["INR-Rate"].ToString()).ToString("#,##.00", CultureInfo.CreateSpecificCulture("en-IN")) + "</h5></td><td><h5>-</h5></td><td><h5>-</h5></td><td><h5>0</h5></td><td><h5>0</h5></td></tr>";
                    }
                }

                    
                BindRABillItems(dschild.Rows[j]["BOQDetailsUID"].ToString(), WorkpackageUID, RAbillUID);
            }
            return RetHTML;
        }

        
    }
}