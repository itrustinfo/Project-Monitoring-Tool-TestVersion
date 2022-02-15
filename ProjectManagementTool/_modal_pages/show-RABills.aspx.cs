using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Globalization;
using ProjectManagementTool.DAL;

namespace ProjectManagementTool._modal_pages
{
    public partial class show_RABills : System.Web.UI.Page
    {
        DBGetData dbgetdata = new DBGetData();
        Invoice invoice = new Invoice();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Username"] == null)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
            }
            else
            {
                if(! IsPostBack)
                {
                    if (Request.QueryString["type"] != null)
                    {
                        additems.Visible = false;
                    }

                    if (Request.QueryString["RABillUid"] != null)
                    {
                        //Session["rabilluid"] = Request.QueryString["RABillUid"];
                        //string s = Request.QueryString["RABillUid"].ToString();
                        BindRABills();
                        //AddRABillItem.HRef = "/_modal_pages/add-rabillitem.aspx?From=Item&RABillUid=" + Request.QueryString["RABillUid"] + "&WorkpackageUID=" + Request.QueryString["WorkpackageUID"];
                        LinkBOQData.HRef = "/_modal_pages/boq-treeview.aspx?ProjectUID=" + dbgetdata.getProjectUIDby_WorkpackgeUID(new Guid(Request.QueryString["WorkpackageUID"]));
                    }

                    //if (Session["BOQData"] != null)
                    //{
                    //    lblActivityName.Visible = true;
                    //    LinkBOQData.Visible = false;
                    //    LnkChangeItem.Visible = true;
                    //    lblActivityName.Text = dbgetdata.GetBOQItemNumber_by_BOQDetailsUID(new Guid(Session["BOQData"].ToString()));
                    //    txtradescription.Text = dbgetdata.GetBOQDesc_by_BOQDetailsUID(new Guid(Session["BOQData"].ToString()));
                    //    double CurrentQuantity = invoice.GetTotalJointInspectionQuantity_by_RAbillItem(new Guid(Session["BOQData"].ToString()), new Guid(Request.QueryString["RABillUid"]));
                    //    if (CurrentQuantity > 0)
                    //    {
                    //        btnAddItem.Visible = false;
                    //        lblActivityName.Text = "";
                    //        txtradescription.Text = "";
                    //        Session["BOQData"] = null;
                    //        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('"+ lblActivityName.Text + " already added to this RA bill. Please click on Item number in the below table and add joint inspection to that Item.');</script>");
                    //    }
                    //    else
                    //    {
                    //        btnAddItem.Visible = true;
                    //        AddJointInspectionItem.HRef = "/_modal_pages/add-jointinspection-to-rabill.aspx?itemUId=" + Session["BOQData"].ToString() + "&WorkpackageUID=" + Request.QueryString["WorkpackageUID"] + "&RABillUid=" + Request.QueryString["RABillUid"] + "";
                    //    }
                        
                    //}
                    //else
                    //{
                    //    btnAddItem.Visible = false;
                    //    lblActivityName.Visible = false;
                    //    LinkBOQData.Visible = true;
                    //    LnkChangeItem.Visible = false;
                    //}
                }
                
            }
        }

        private void BindRABills()
        {
            //DataTable dtRaBills = dbgetdata.GetRABills(Request.QueryString["RABillUid"]);
            //grdRaBills.DataSource = dtRaBills;
            //grdRaBills.DataBind();

            DataTable dt = new DataTable();
            dt.Columns.Add("WorkpackageUID");
            dt.Columns.Add("itemUId");
            dt.Columns.Add("item_number");
            dt.Columns.Add("item_desc");
            dt.Columns.Add("RABillUid");

            string ProjectUID = dbgetdata.getProjectUIDby_WorkpackgeUID(new Guid(Request.QueryString["WorkpackageUID"]));
            DataTable ds = dbgetdata.getBOQParent_Details(new Guid(ProjectUID), "Project");
            for (int i = 0; i < ds.Rows.Count; i++)
            {
                BindRABillItems(dt, ds.Rows[i]["BOQDetailsUID"].ToString(), Request.QueryString["WorkpackageUID"], Request.QueryString["RABillUid"]);
            }
            grdRaBills.DataSource = dt;
            grdRaBills.DataBind();
        }

        protected void grdRaBills_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdRaBills.PageIndex = e.NewPageIndex;
            BindRABills();
        }

        protected void grdRaBills_RowEditing(object sender, GridViewEditEventArgs e)
        {
            grdRaBills.EditIndex = e.NewEditIndex;
            BindRABills();
        }

        protected void grdRaBills_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            grdRaBills.EditIndex = -1;
            BindRABills();

        }

        protected void grdRaBills_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            //Finding the controls from Gridview for the row which is going to update  
            HiddenField itemUId = grdRaBills.Rows[e.RowIndex].FindControl("hidUid") as HiddenField ;
            HiddenField oldCost = grdRaBills.Rows[e.RowIndex].FindControl("hidCost") as HiddenField;
            TextBox txtCost = grdRaBills.Rows[e.RowIndex].FindControl("txtItemCost") as TextBox;
           if(double.TryParse(txtCost.Text ,out double newCost))
            {
                double costDiff = newCost -Convert.ToDouble(oldCost.Value);
                dbgetdata.UpdateItemCostData(itemUId.Value, newCost, costDiff);
            }
            grdRaBills.EditIndex = -1;
            BindRABills();
        }

        //protected void btnAdd_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (Request.QueryString["RABillUid"] != null)
        //        {
        //            int cnt = dbgetdata.InsertRABillsItems(Request.QueryString["RABillUid"], txtItemNumber.Text, txtDescription.Text, txtDate.Text, txtAddcost.Text, new Guid(Request.QueryString["ProjectUID"]), new Guid(Request.QueryString["WorkpackageUID"]));
        //            if (cnt > 0)
        //            {
        //                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
        //            }
        //        }
        //    }
        //    catch(Exception ex)
        //    {

        //    }
        //}

        public string GetBOQHierarchy_By_itemUId(string itemUId)
        {
            return dbgetdata.GetBOQItemNumberHierarchy_by_BOQDetailsUID(new Guid(itemUId));
        }

        public string GetBOQDescriptionHierarchy_By_itemUId(string itemUId)
        {
            return dbgetdata.GetBOQDescriptionHierarchy_by_BOQDetailsUID(new Guid(itemUId));
        }

        protected void grdRaBills_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblApprovedQuan = (Label)e.Row.FindControl("LblApprovedQuantity");
                Label lblApprovedRate = (Label)e.Row.FindControl("LblApprovedRate");

                string ItemUID= grdRaBills.DataKeys[e.Row.RowIndex].Values[0].ToString();

                DataSet ds = dbgetdata.GetBOQDetails_by_BOQDetailsUID(new Guid(ItemUID));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string CurrencySymbol = "";
                    if (ds.Tables[0].Rows[0]["Currency"].ToString() == "&#x20B9;")
                    {
                       
                        CurrencySymbol = "₹";
                    }
                    else if (ds.Tables[0].Rows[0]["Currency"].ToString() == "&#36;")
                    {
                        
                        CurrencySymbol = "$";
                    }
                    else
                    {
                        
                        CurrencySymbol = "¥";
                    }
                    string Cul_Info = "";
                    if (ds.Tables[0].Rows[0]["Currency_CultureInfo"].ToString() != "")
                    {
                        Cul_Info = ds.Tables[0].Rows[0]["Currency_CultureInfo"].ToString();
                    }
                    else
                    {
                        Cul_Info = "en-IN";
                    }
                    if (ds.Tables[0].Rows[0]["Quantity"].ToString() == "0")
                    {
                        lblApprovedQuan.Text = "";
                        lblApprovedRate.Text = "";
                        e.Row.Cells[8].Visible = false;
                    }
                    else
                    {
                        e.Row.Cells[8].Visible = true;

                        lblApprovedQuan.Text = ds.Tables[0].Rows[0]["Quantity"].ToString();
                        lblApprovedRate.Text = CurrencySymbol + " " + Convert.ToDouble(ds.Tables[0].Rows[0]["INR-Rate"].ToString()).ToString("#,##.##", CultureInfo.CreateSpecificCulture(Cul_Info));

                        double TR = 0;

                        Label lblCurrentQuantity = (Label)e.Row.FindControl("LblCurrentQuantity");
                        Label lblCurrentAmount = (Label)e.Row.FindControl("LblCurrentAmount");
                        double CurrentQuantity = invoice.GetTotalJointInspectionQuantity_by_RAbillItem(new Guid(ItemUID), new Guid(Request.QueryString["RABillUid"]));
                        if (CurrentQuantity > 0)
                        {
                            //DataSet dsTerms = dbgetdata.GetPaymentBreakupTerms_RABillUid_BOQDetailsUID(new Guid(Request.QueryString["RABillUid"]), new Guid(ItemUID));
                            DataSet dsTerms = dbgetdata.GetPaymentBreakupTerms_BOQDetailsUID(new Guid(ItemUID));
                            if (dsTerms.Tables[0].Rows.Count > 0)
                            {
                                Label lblTRDesc = (Label)e.Row.FindControl("LblTRDesc");
                                Label lblTRRate = (Label)e.Row.FindControl("LblTRRate");
                                TR = (Convert.ToDouble(ds.Tables[0].Rows[0]["INR-Rate"].ToString()) - ((Convert.ToDouble(ds.Tables[0].Rows[0]["INR-Rate"].ToString()) * float.Parse(dsTerms.Tables[0].Rows[0]["Percentage"].ToString())) / 100));
                                lblTRDesc.Text = dsTerms.Tables[0].Rows[0]["Percentage"].ToString() + "% " + dsTerms.Tables[0].Rows[0]["Terms_Desc"].ToString();
                                lblTRRate.Text = CurrencySymbol + " " + (Convert.ToDouble(ds.Tables[0].Rows[0]["INR-Rate"].ToString()) - ((Convert.ToDouble(ds.Tables[0].Rows[0]["INR-Rate"].ToString()) * float.Parse(dsTerms.Tables[0].Rows[0]["Percentage"].ToString())) / 100)).ToString("#,##.##", CultureInfo.CreateSpecificCulture(Cul_Info));
                            }

                            lblCurrentQuantity.Text = CurrentQuantity.ToString();

                            if (dsTerms.Tables[0].Rows.Count > 0)
                            {
                                lblCurrentAmount.Text = CurrencySymbol + " " + (CurrentQuantity * TR).ToString("#,##.##", CultureInfo.CreateSpecificCulture(Cul_Info));
                            }
                            else
                            {
                                lblCurrentAmount.Text = CurrencySymbol + " " + (CurrentQuantity * Convert.ToDouble(ds.Tables[0].Rows[0]["INR-Rate"].ToString())).ToString("#,##.##", CultureInfo.CreateSpecificCulture(Cul_Info));
                            }
                        }
                        else
                        {
                            lblCurrentQuantity.Text = "0";
                            lblCurrentAmount.Text = CurrencySymbol + " 0";
                        }

                        Label lblPrevQuantity = (Label)e.Row.FindControl("LblPrevQuantity");
                        Label lblPrevAmount = (Label)e.Row.FindControl("LblPrevAmount");
                        double PrevQuantity = invoice.GetPrevJointInspectionQuantity_by_RAbillItem(new Guid(ItemUID), new Guid(Request.QueryString["RABillUid"]));
                        if (PrevQuantity > 0)
                        {

                            DataSet dsTerms = dbgetdata.GetPaymentBreakupTerms_BOQDetailsUID(new Guid(ItemUID));
                            if (dsTerms.Tables[0].Rows.Count > 0)
                            {
                                TR = (Convert.ToDouble(ds.Tables[0].Rows[0]["INR-Rate"].ToString()) - ((Convert.ToDouble(ds.Tables[0].Rows[0]["INR-Rate"].ToString()) * float.Parse(dsTerms.Tables[0].Rows[0]["Percentage"].ToString())) / 100));
                            }

                            lblPrevQuantity.Text = PrevQuantity.ToString();

                            if (TR > 0)
                            {
                                lblPrevAmount.Text = CurrencySymbol + " " + (PrevQuantity * TR).ToString("#,##.##", CultureInfo.CreateSpecificCulture(Cul_Info));
                            }
                            else
                            {
                                lblPrevAmount.Text = CurrencySymbol + " " + (PrevQuantity * Convert.ToDouble(ds.Tables[0].Rows[0]["INR-Rate"].ToString())).ToString("#,##.##", CultureInfo.CreateSpecificCulture(Cul_Info));
                            }
                        }
                        else
                        {
                            lblPrevQuantity.Text = "0";
                            lblPrevAmount.Text = CurrencySymbol + " 0";
                        }
                    }
                }
            }
        }

        protected void btnAddItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["BOQData"] == null)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Please choose BOQ Item...');</script>");
                }
                else
                {
                    string sDate1 = "";
                    DateTime CDate1 = DateTime.Now;

                    sDate1 = DateTime.Now.ToString("dd/MM/yyyy");
                    //sDate1 = sDate1.Split('/')[1] + "/" + sDate1.Split('/')[0] + "/" + sDate1.Split('/')[2];
                    sDate1 = dbgetdata.ConvertDateFormat(sDate1);
                    CDate1 = Convert.ToDateTime(sDate1);

                    string ProjectUID = dbgetdata.getProjectUIDby_WorkpackgeUID(new Guid(Request.QueryString["WorkpackageUID"]));
                    int cnt = dbgetdata.InsertRABillsItems(Request.QueryString["RABillUid"], lblActivityName.Text, txtradescription.Text, CDate1.ToString(), "0", new Guid(ProjectUID), new Guid(Request.QueryString["WorkpackageUID"]), new Guid(Session["BOQData"].ToString()));
                    if (cnt > 0)
                    {
                        Session["BOQData"] = null;
                        BindRABills();
                        txtradescription.Text = "";
                        lblActivityName.Visible = false;
                        LinkBOQData.Visible = true;
                        LnkChangeItem.Visible = false;
                        //Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
                    }
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code : ARABI-1, There is a problem with this feature. Please contact system admin.');</script>");
            }
        }

        protected void LnkChangeItem_Click(object sender, EventArgs e)
        {
            lblActivityName.Visible = false;
            LinkBOQData.Visible = true;
            LnkChangeItem.Visible = false;
            Session["BOQData"] = null;
            txtradescription.Text = "";
        }

        private void BindRABillItems(DataTable dt,string ParentUID,string WorkpackageUID,string RAbillUID)
        {
            DataTable dschild = dbgetdata.getBoq_Details(new Guid(ParentUID));
            for (int j = 0; j < dschild.Rows.Count; j++)
            {
                DataRow dtrow = dt.NewRow();
                dtrow["itemUId"] = dschild.Rows[j]["BOQDetailsUID"].ToString();
                dtrow["WorkpackageUID"] = WorkpackageUID;
                dtrow["item_number"] = dschild.Rows[j]["Item_Number"].ToString();
                dtrow["RABillUid"] = RAbillUID;
                dtrow["item_desc"] = dschild.Rows[j]["Description"].ToString();
                dt.Rows.Add(dtrow);

                BindRABillItems(dt, dschild.Rows[j]["BOQDetailsUID"].ToString(), WorkpackageUID, RAbillUID);
            }
        }
    }
}