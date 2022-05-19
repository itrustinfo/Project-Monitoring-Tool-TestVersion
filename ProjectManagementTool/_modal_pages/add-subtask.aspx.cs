using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagementTool._modal_pages
{
    public partial class add_subtask : System.Web.UI.Page
    {
        TaskUpdate TKUpdate = new TaskUpdate();
        DBGetData getdata = new DBGetData();
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
                    //txtBasicBudget.Attributes.Add("onkeyup", "javascript:return allnumericplusminus(this)");
                    //txtGST.Attributes.Add("onkeyup", "javascript:return allnumericplusminus(this)");
                    if (Request.QueryString["type"] != null)
                    {
                        BindProject();
                        BindMeasurementUnit();
                        ddlStatus.DataSource = getdata.getStatusMaster();
                        ddlStatus.DataTextField = "Value";
                        ddlStatus.DataValueField = "Status";
                        ddlStatus.DataBind();
                        BindUsers();

                        if (Request.QueryString["type"] == "edit")
                        {
                            DDlProject_SelectedIndexChanged(sender, e);
                            LoadTaskDetails();
                            parenttask.Visible = false;
                            LnkChangeItem.Visible = false;
                        }
                        else if (Request.QueryString["type"] == "add")
                        {
                            DDlProject.SelectedValue = Request.QueryString["PrjUID"].ToString();
                            DDlProject_SelectedIndexChanged(sender, e);
                            
                            parenttask.Visible = true;
                            //BindTasks();
                        }

                        if (Session["BOQData"] != null)
                        {
                            lblActivityName.Visible = true;
                            LinkBOQData.Visible = false;
                           // LnkChangeItem.Visible = true;
                            txtquantity.Disabled = true;
                            txtGST.Disabled = true;
                            txtprice.Disabled = true;
                            DDLMeasurementUnit.Enabled = false;
                            //lblActivityName.Text = getdata.GetBOQItemNumber_by_BOQDetailsUID(new Guid(Session["BOQData"].ToString()));
                            DataSet dsBOQ = getdata.GetBOQDetails_by_BOQDetailsUID(new Guid(Session["BOQData"].ToString()));
                            if (dsBOQ.Tables[0].Rows.Count > 0)
                            {
                                lblActivityName.Text = dsBOQ.Tables[0].Rows[0]["Item_Number"].ToString();
                                txtquantity.Value = dsBOQ.Tables[0].Rows[0]["Quantity"].ToString();
                                txtGST.Value = dsBOQ.Tables[0].Rows[0]["GST"].ToString() == "" ? "0" : dsBOQ.Tables[0].Rows[0]["GST"].ToString();
                                if (dsBOQ.Tables[0].Rows[0]["Currency"].ToString() == "&#x20B9;")
                                {
                                    txtprice.Value = "₹ " + dsBOQ.Tables[0].Rows[0]["INR-Rate"].ToString();
                                }
                                else if (dsBOQ.Tables[0].Rows[0]["Currency"].ToString() == "&#36;")
                                {
                                    txtprice.Value = "$ " + dsBOQ.Tables[0].Rows[0]["INR-Rate"].ToString();
                                }
                                else
                                {
                                    txtprice.Value = "¥ " + dsBOQ.Tables[0].Rows[0]["INR-Rate"].ToString();
                                }
                                DDLMeasurementUnit.SelectedItem.Text = dsBOQ.Tables[0].Rows[0]["Unit"].ToString();
                                txtMeasurementQuantity.Text = dsBOQ.Tables[0].Rows[0]["Quantity"].ToString();
                            }
                        }
                        else if (Request.QueryString["type"] != "edit")
                        {
                            lblActivityName.Visible = false;
                            LinkBOQData.Visible = true;
                            LnkChangeItem.Visible = false;
                        }

                        //BtnAddBOQSubItems.Visible = false;
                    }
                }
            }
        }

        private void BindProject()
        {
            DataTable ds = new DataTable();
            if (Session["TypeOfUser"].ToString() == "U" || Session["TypeOfUser"].ToString() =="MD" || Session["TypeOfUser"].ToString() == "VP")
            {

                ds = TKUpdate.GetProjects();
            }
            else if (Session["TypeOfUser"].ToString() == "PA")
            {

                //ds = TKUpdate.GetProjects_by_UserUID(new Guid(Session["UserUID"].ToString()));
                ds = TKUpdate.GetAssignedProjects_by_UserUID(new Guid(Session["UserUID"].ToString()));
            }
            else
            {

                //ds = TKUpdate.GetProjects();
                ds = TKUpdate.GetAssignedProjects_by_UserUID(new Guid(Session["UserUID"].ToString()));
            }
            DDlProject.DataTextField = "ProjectName";
            DDlProject.DataValueField = "ProjectUID";
            DDlProject.DataSource = ds;
            DDlProject.DataBind();
            if (Request.QueryString["PrjUID"] != null)
            {
                DDlProject.SelectedValue = Request.QueryString["PrjUID"].ToString();
            }

        }

        private void BindUsers()
        {
            DataSet ds = new DataSet();
            if (Session["TypeOfUser"].ToString() == "U" || Session["TypeOfUser"].ToString() =="MD" || Session["TypeOfUser"].ToString() == "VP")
            {
                ds = getdata.getAllUsers();
            }
            else if (Session["TypeOfUser"].ToString() == "PA")
            {
                //ds = getdata.getUsers_by_ProjectUnder(new Guid(DDlProject.SelectedValue));
                ds = getdata.GetUsers_under_ProjectUID(new Guid(DDlProject.SelectedValue));
            }
            else
            {
                //ds = getdata.getUsers_by_ProjectUnder(new Guid(DDlProject.SelectedValue));
                ds = getdata.GetUsers_under_ProjectUID(new Guid(DDlProject.SelectedValue));
            }

            DDLUsers.DataTextField = "UserName";
            DDLUsers.DataValueField = "UserUID";
            DDLUsers.DataSource = ds;
            DDLUsers.DataBind();
        }

        private void BindTasks()
        {
            DataSet ds = getdata.GetTasks_by_WorkPackageUID(DDLWorkPackage.SelectedValue);
            DDLParentTask.DataTextField = "Name";
            DDLParentTask.DataValueField = "TaskUID";
            DDLParentTask.DataSource = ds;
            DDLParentTask.DataBind();
            DDLParentTask.Items.Insert(0, "-- Select --");
            if (Request.QueryString["ParentTaskUID"] != null)
            {
                DDLParentTask.SelectedValue = Request.QueryString["ParentTaskUID"].ToString();
            }
        }

        private void BindMeasurementUnit()
        {
            DDLMeasurementUnit.DataTextField = "Unit_Name";
            DDLMeasurementUnit.DataValueField = "Unit_UID";
            DDLMeasurementUnit.DataSource = getdata.getUnitMaster_List();
            DDLMeasurementUnit.DataBind();
            DDLMeasurementUnit.Items.Insert(0, new ListItem("PERCENTAGE", "1"));
        }
        protected void DDlProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            //DataSet ds = getdata.GetWorkPackages_By_ProjectUID(new Guid(DDlProject.SelectedValue));
            DataSet ds = new DataSet();
            if (Session["TypeOfUser"].ToString() == "U" || Session["TypeOfUser"].ToString() == "MD" || Session["TypeOfUser"].ToString() == "VP")
            {
                ds = getdata.GetWorkPackages_By_ProjectUID(new Guid(DDlProject.SelectedValue));
            }
            else if (Session["TypeOfUser"].ToString() == "PA")
            {
                ds = getdata.GetWorkPackages_ForUser_by_ProjectUID(new Guid(Session["UserUID"].ToString()), new Guid(DDlProject.SelectedValue));
            }
            else
            {
                ds = getdata.GetWorkPackages_ForUser_by_ProjectUID(new Guid(Session["UserUID"].ToString()), new Guid(DDlProject.SelectedValue));
            }
            if (ds.Tables[0].Rows.Count > 0)
            {
                DDLWorkPackage.DataTextField = "Name";
                DDLWorkPackage.DataValueField = "WorkPackageUID";
                DDLWorkPackage.DataSource = ds;
                DDLWorkPackage.DataBind();
                if (Request.QueryString["WrkUID"] != null)
                {
                    DDLWorkPackage.SelectedValue = Request.QueryString["WrkUID"].ToString();
                    LinkBOQData.HRef = "/_modal_pages/boq-treeview.aspx?ProjectUID=" + DDlProject.SelectedValue;
                }
                
                BindTasks();
                //BindBOQDetails();
            }
        }

        protected void DDLWorkPackage_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDLWorkPackage.SelectedValue != "")
            {
                BindTasks();
                //BindBOQDetails();
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                //if (Session["BOQData"] == null)
                //{
                //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Choose BOQ Item...');</script>");
                //}
                //else
                //{
                Guid BOQUID;
                if (Session["BOQData"] == null)
                {
                    BOQUID = Guid.Empty;
                }
                else
                {
                    BOQUID = new Guid(Session["BOQData"].ToString());
                }
                Guid sTaskUID = Guid.NewGuid();
                int TaskLevel = 0;
                string ParentTaskID = string.Empty;
                if (Request.QueryString["TaskUID"] != null)
                {
                    sTaskUID = new Guid(Request.QueryString["TaskUID"].ToString());
                    ParentTaskID = Guid.NewGuid().ToString();
                }
                else
                {
                    sTaskUID = Guid.NewGuid();
                    if (DDLParentTask.SelectedItem.Text == "-- Select --")
                    {
                        TaskLevel = 1;
                        ParentTaskID = "00000000-0000-0000-0000-000000000000";
                    }
                    else
                    {
                        TaskLevel = getdata.getTaskLevel_By_TaskUID(new Guid(DDLParentTask.SelectedValue));
                        TaskLevel = TaskLevel + 1;
                        ParentTaskID = DDLParentTask.SelectedValue;
                    }
                }
                string sDate1 = "", sDate2 = "", sDate3 = "", sDate4 = "", sDate5 = "", sDate6 = "";
                DateTime CDate1 = new DateTime(), CDate2 = new DateTime(), CDate3 = new DateTime(), CDate4 = new DateTime(), CDate5 = new DateTime(), CDate6 = new DateTime();
                if (dtStartdate.Text != "" && dtStartdate.Text != "MM/dd/yyyy")
                {
                    sDate1 = dtStartdate.Text;
                    //sDate1 = sDate1.Split('/')[1] + "/" + sDate1.Split('/')[0] + "/" + sDate1.Split('/')[2];
                    sDate1 = getdata.ConvertDateFormat(sDate1);
                    CDate1 = Convert.ToDateTime(sDate1);
                }
                //
                if (dtPlanneddate.Text != "")
                {
                    sDate2 = dtPlanneddate.Text;
                    //sDate2 = sDate2.Split('/')[1] + "/" + sDate2.Split('/')[0] + "/" + sDate2.Split('/')[2];
                    sDate2 = getdata.ConvertDateFormat(sDate2);
                    CDate2 = Convert.ToDateTime(sDate2);
                }
                //
                if (dtProjecteddate.Text != "")
                {
                    sDate3 = dtProjecteddate.Text;
                    //sDate3 = sDate3.Split('/')[1] + "/" + sDate3.Split('/')[0] + "/" + sDate3.Split('/')[2];
                    sDate3 = getdata.ConvertDateFormat(sDate3);
                    CDate3 = Convert.ToDateTime(sDate3);
                }

                // Actual EndDate
                if (dtActualEndDate.Text != "")
                {
                    sDate4 = dtActualEndDate.Text;
                    //sDate4 = sDate4.Split('/')[1] + "/" + sDate4.Split('/')[0] + "/" + sDate4.Split('/')[2];
                    sDate4 = getdata.ConvertDateFormat(sDate4);
                    CDate4 = Convert.ToDateTime(sDate4);
                }

                // Planned StartDate
                if (dtPlannedStartDate.Text != "")
                {
                    sDate5 = dtPlannedStartDate.Text;
                    //sDate5 = sDate5.Split('/')[1] + "/" + sDate5.Split('/')[0] + "/" + sDate5.Split('/')[2];
                    sDate5 = getdata.ConvertDateFormat(sDate5);
                    CDate5 = Convert.ToDateTime(sDate5);
                }

                // Projected StartDate
                if (dtProjectedStartDate.Text != "")
                {
                    sDate6 = dtProjectedStartDate.Text;
                    //sDate6 = sDate6.Split('/')[1] + "/" + sDate6.Split('/')[0] + "/" + sDate6.Split('/')[2];
                    sDate6 = getdata.ConvertDateFormat(sDate6);
                    CDate6 = Convert.ToDateTime(sDate6);
                }
                //if (string.IsNullOrEmpty(txtStatusPer.Text))
                //{
                //    txtStatusPer.Text = "0";
                //}
                //txtBasicBudget.Value = txtBasicBudget.Value.Replace(",", "").Replace("₹ ", "").Replace("$ ", "").Replace("¥ ", "").Replace("₹", "").Replace("$", "").Replace("¥", "").Trim();
                //txtGST.Value = txtGST.Value.Replace(",", "");
                //txtBudget.Value = txtBudget.Value.Replace(",", "").Replace("₹ ", "").Replace("$ ", "").Replace("¥ ", "").Replace("₹", "").Replace("$", "").Replace("¥", "").Trim();
                //txtActualExpenditure.Value = txtActualExpenditure.Value.Replace(",", "").Replace("₹ ", "").Replace("$ ", "").Replace("¥ ", "").Replace("₹", "").Replace("$", "").Replace("¥", "").Trim();

                string Currecncy_CultureInfo = "";
                string Currency = "";
                if (Session["BOQData"] == null)
                {
                    Currecncy_CultureInfo = "en-IN";
                    Currency = "&#x20B9;";
                }
                else
                {
                    DataSet dsBOQ = getdata.GetBOQDetails_by_BOQDetailsUID(new Guid(Session["BOQData"].ToString()));
                    if (dsBOQ.Tables[0].Rows.Count > 0)
                    {
                        Currecncy_CultureInfo = dsBOQ.Tables[0].Rows[0]["Currency_CultureInfo"].ToString();
                        Currency = dsBOQ.Tables[0].Rows[0]["Currency"].ToString();
                    }
                }
                
                //if (DDLCurrency.SelectedItem.Text == "₹ (RUPEE)")
                //{
                //    Currecncy_CultureInfo = "en-IN";
                //}
                //else if (DDLCurrency.SelectedItem.Text == "$ (USD)")
                //{
                //    Currecncy_CultureInfo = "en-US";
                //}
                //else
                //{
                //    Currecncy_CultureInfo = "ja-JP";
                //}

                double TaskWeightage = 0;
                if (txtweightage.Text != "")
                {
                    TaskWeightage = Convert.ToDouble(txtweightage.Text);
                }


                string MeasurementUnit = "";
                double MeasurementQuantity = 0;
                if (DDLMeasurementUnit.SelectedValue != "")
                {
                    MeasurementUnit = DDLMeasurementUnit.SelectedItem.Text;
                    if (txtMeasurementQuantity.Text != "")
                    {
                        MeasurementQuantity = Convert.ToDouble(txtMeasurementQuantity.Text);
                    }
                }
                txtGST.Value = (txtGST.Value == "") ? "0" : txtGST.Value;

                // changed by saji augustin dated 17/05/2022

                bool result = getdata.InsertorUpdateSubTask(sTaskUID, new Guid(DDLWorkPackage.SelectedValue), new Guid(Request.QueryString["PrjUID"].ToString()), DDLUsers.SelectedValue, txtTaskName.Text, txtDescription.Text, "", "", CDate1 == DateTime.MinValue ? "" : CDate1.ToString(), CDate2 == DateTime.MinValue ? "" : CDate2.ToString(), CDate3 == DateTime.MinValue ? "" : CDate3.ToString(), CDate5 == DateTime.MinValue ? "" : CDate5.ToString(), CDate6 == DateTime.MinValue ? "" : CDate6.ToString(), CDate4 == DateTime.MinValue ? "" : CDate4.ToString(), ddlStatus.SelectedValue, 0, 0, "", TaskLevel, ParentTaskID, Convert.ToDouble(txtGST.Value),
                    0, Convert.ToDouble(0), DDlDiscipline.SelectedValue, MeasurementUnit, Currency, Currecncy_CultureInfo, TaskWeightage, DDLTaskType.SelectedValue, new Guid(Request.QueryString["OptionUID"].ToString()), MeasurementQuantity, BOQUID, RBLOption.SelectedValue,chkInGraph.Checked);
                
                
                if (result)
                {
                    if (Request.QueryString["type"] == "edit")
                    {
                        Session["SelectedActivity"] = Request.QueryString["TaskUID"].ToString();
                    }
                    else
                    {
                        Session["SelectedActivity"] = ParentTaskID;
                    }
                    Session["BOQData"] = null;
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
                }
            }
            //}
            catch (Exception ex)
            {
                Session["BOQData"] = null;
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('There is a problem with this feature. Please contact system admin');</script>");
            }
        }

        private void LoadTaskDetails()
        {
            try
            {
                DataSet ds = new DataSet();
                ds = getdata.GetTaskDetails(Request.QueryString["TaskUID"].ToString());
                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtTaskName.Text = ds.Tables[0].Rows[0]["Name"].ToString();
                    if (ds.Tables[0].Rows[0]["Owner"].ToString() != "")
                    {
                        DDLUsers.SelectedValue = ds.Tables[0].Rows[0]["Owner"].ToString();
                    }
                    txtDescription.Text = ds.Tables[0].Rows[0]["Description"].ToString();

                    if (ds.Tables[0].Rows[0]["GroupBOQItems"].ToString() != "")
                    {
                        RBLOption.SelectedValue = ds.Tables[0].Rows[0]["GroupBOQItems"].ToString();
                    }
                    RBLOption.Enabled = false;
                    if (ds.Tables[0].Rows[0]["BOQDetailsUID"].ToString() != "")
                    {
                        //DDLBOQDetails.SelectedValue = ds.Tables[0].Rows[0]["BOQDetailsUID"].ToString();
                        Session["BOQData"] = ds.Tables[0].Rows[0]["BOQDetailsUID"].ToString();
                        lblActivityName.Visible = true;
                        LinkBOQData.Visible = false;
                        LnkChangeItem.Visible = true;
                        txtquantity.Disabled = true;
                        txtGST.Disabled = true;
                        txtprice.Disabled = true;

                        DataSet dsBOQ = getdata.GetBOQDetails_by_BOQDetailsUID(new Guid(ds.Tables[0].Rows[0]["BOQDetailsUID"].ToString()));
                        if (dsBOQ.Tables[0].Rows.Count > 0)
                        {
                            lblActivityName.Text = dsBOQ.Tables[0].Rows[0]["Item_Number"].ToString();
                            txtquantity.Value = dsBOQ.Tables[0].Rows[0]["Quantity"].ToString();
                            txtGST.Value = dsBOQ.Tables[0].Rows[0]["GST"].ToString() == "" ? "" : dsBOQ.Tables[0].Rows[0]["GST"].ToString();

                            if (dsBOQ.Tables[0].Rows[0]["Currency"].ToString() == "&#x20B9;")
                            {
                                txtprice.Value = "₹ " + dsBOQ.Tables[0].Rows[0]["INR-Rate"].ToString();
                            }
                            else if (dsBOQ.Tables[0].Rows[0]["Currency"].ToString() == "&#36;")
                            {
                                txtprice.Value = "$ " + dsBOQ.Tables[0].Rows[0]["INR-Rate"].ToString();
                            }
                            else
                            {
                                txtprice.Value = "¥ " + dsBOQ.Tables[0].Rows[0]["INR-Rate"].ToString();
                            }
                        }
                    }
                    else
                    {
                        lblActivityName.Visible = false;
                        LinkBOQData.Visible = true;
                        LnkChangeItem.Visible = false;
                    }
                    //txtPOReference.Text = ds.Tables[0].Rows[0]["POReference"].ToString();
                    //txtRFPReference.Text = ds.Tables[0].Rows[0]["RFPReference"].ToString();
                    //string CurrencySymbol = "";
                    //if (ds.Tables[0].Rows[0]["Currency"].ToString() == "&#x20B9;")
                    //{
                    //    DDLCurrency.SelectedIndex = 0;
                    //    CurrencySymbol = "₹";
                    //}
                    //else if (ds.Tables[0].Rows[0]["Currency"].ToString() == "&#36;")
                    //{
                    //    DDLCurrency.SelectedIndex = 1;
                    //    CurrencySymbol = "$";
                    //}
                    //else
                    //{
                    //    DDLCurrency.SelectedIndex = 2;
                    //    CurrencySymbol = "¥";
                    //}
                    //txtBasicBudget.Value = Convert.ToDouble(ds.Tables[0].Rows[0]["Basic_Budget"].ToString()).ToString("#,##.##", CultureInfo.CreateSpecificCulture(ds.Tables[0].Rows[0]["Currency_CultureInfo"].ToString()));
                    //if (string.IsNullOrEmpty(txtBasicBudget.Value))
                    //{
                    //    txtBasicBudget.Value = CurrencySymbol + " 0";
                    //}
                    //else
                    //{
                    //    txtBasicBudget.Value = CurrencySymbol + " " + Convert.ToDouble(ds.Tables[0].Rows[0]["Basic_Budget"].ToString()).ToString("#,##.##", CultureInfo.CreateSpecificCulture(ds.Tables[0].Rows[0]["Currency_CultureInfo"].ToString()));
                    //}
                    //txtGST.Value = ds.Tables[0].Rows[0]["GST"].ToString();
                    //if (string.IsNullOrEmpty(txtGST.Value))
                    //{
                    //    txtGST.Value = "0";
                    //}
                    //txtBudget.Value = Convert.ToDouble(ds.Tables[0].Rows[0]["Total_Budget"].ToString()).ToString("#,##.##", CultureInfo.CreateSpecificCulture(ds.Tables[0].Rows[0]["Currency_CultureInfo"].ToString()));
                    //if (string.IsNullOrEmpty(txtBudget.Value))
                    //{
                    //    txtBudget.Value = CurrencySymbol + " 0";
                    //}
                    //else
                    //{
                    //    txtBudget.Value = CurrencySymbol + " " + Convert.ToDouble(ds.Tables[0].Rows[0]["Total_Budget"].ToString()).ToString("#,##.##", CultureInfo.CreateSpecificCulture(ds.Tables[0].Rows[0]["Currency_CultureInfo"].ToString()));
                    //}
                    txtweightage.Text = ds.Tables[0].Rows[0]["Task_Weightage"].ToString();
                    if (string.IsNullOrEmpty(txtweightage.Text))
                    {
                        txtweightage.Text = "0";
                    }
                    DDLTaskType.SelectedValue = ds.Tables[0].Rows[0]["Task_Type"].ToString();
                    //txtStatusPer.Text = ds.Tables[0].Rows[0]["StatusPer"].ToString();
                    //if (string.IsNullOrEmpty(txtStatusPer.Text))
                    //{
                    //    txtStatusPer.Text = "0";
                    //}
                    ddlStatus.SelectedValue = ds.Tables[0].Rows[0]["Status"].ToString();
                    var x = ds.Tables[0].Rows[0]["InGraph"].ToString();

                    // added code by saji augustin dated 17/05/2022
                    if (ds.Tables[0].Rows[0]["InGraph"] != null)
                      chkInGraph.Checked =  ds.Tables[0].Rows[0]["InGraph"].ToString() == "Y" ? true : false;
                    
                    //txtActualExpenditure.Value = Convert.ToDouble(ds.Tables[0].Rows[0]["ActualExpenditure"].ToString()).ToString("#,##.##", CultureInfo.CreateSpecificCulture(ds.Tables[0].Rows[0]["Currency_CultureInfo"].ToString()));
                    //if (string.IsNullOrEmpty(txtActualExpenditure.Value))
                    //{
                    //    txtActualExpenditure.Value = CurrencySymbol + " 0";
                    //}
                    //else
                    //{
                    //    txtActualExpenditure.Value = CurrencySymbol + " " + Convert.ToDouble(ds.Tables[0].Rows[0]["ActualExpenditure"].ToString()).ToString("#,##.##", CultureInfo.CreateSpecificCulture(ds.Tables[0].Rows[0]["Currency_CultureInfo"].ToString()));
                    //}
                    if (ds.Tables[0].Rows[0]["StartDate"].ToString() != null && ds.Tables[0].Rows[0]["StartDate"].ToString() != "")
                    {
                        dtStartdate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["StartDate"].ToString()).ToString("dd/MM/yyyy",CultureInfo.InvariantCulture);
                    }
                    if (ds.Tables[0].Rows[0]["PlannedEndDate"].ToString() != null && ds.Tables[0].Rows[0]["PlannedEndDate"].ToString() != "")
                    {
                        dtPlanneddate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["PlannedEndDate"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    }
                    if (ds.Tables[0].Rows[0]["ProjectedEndDate"].ToString() != null && ds.Tables[0].Rows[0]["ProjectedEndDate"].ToString() != "")
                    {
                        dtProjecteddate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["ProjectedEndDate"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    }

                    if (ds.Tables[0].Rows[0]["PlannedStartDate"].ToString() != null && ds.Tables[0].Rows[0]["PlannedStartDate"].ToString() != "")
                    {
                        dtPlannedStartDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["PlannedStartDate"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    }
                    if (ds.Tables[0].Rows[0]["ProjectedStartDate"].ToString() != null && ds.Tables[0].Rows[0]["ProjectedStartDate"].ToString() != "")
                    {
                        dtProjectedStartDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["ProjectedStartDate"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    }
                    if (ds.Tables[0].Rows[0]["ActualEndDate"].ToString() != null && ds.Tables[0].Rows[0]["ActualEndDate"].ToString() != "")
                    {
                        dtActualEndDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["ActualEndDate"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    }
                    if (ds.Tables[0].Rows[0]["Discipline"].ToString() != null && ds.Tables[0].Rows[0]["Discipline"].ToString() != "")
                    {
                        DDlDiscipline.SelectedValue = ds.Tables[0].Rows[0]["Discipline"].ToString();
                    }
                    if (ds.Tables[0].Rows[0]["UnitforProgress"].ToString() != "")
                    {
                        DDLMeasurementUnit.SelectedItem.Text = ds.Tables[0].Rows[0]["UnitforProgress"].ToString();
                        MeasurementQuan.Visible = true;
                        txtMeasurementQuantity.Text = ds.Tables[0].Rows[0]["UnitQuantity"].ToString();
                        if (DDLMeasurementUnit.SelectedItem.Text != "PERCENTAGE")
                        {
                            txtMeasurementQuantity.Enabled = true;
                        }
                    }
                    else
                    {
                        txtMeasurementQuantity.Text = "100";
                    }
                    
                    //DDLUnit.SelectedItem.Text = ds.Tables[0].Rows[0]["UnitforProgress"].ToString();
                    //DDlProject.SelectedValue = ds.Tables[0].Rows[0]["ProjectUID"].ToString();
                    //DDLWorkPackage.SelectedValue = ds.Tables[0].Rows[0]["WorkPackageUID"].ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void DDLMeasurementUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDLMeasurementUnit.SelectedValue != "")
            {
                if (DDLMeasurementUnit.SelectedItem.Text == "PERCENTAGE")
                {
                    txtMeasurementQuantity.Text = "100";
                    txtMeasurementQuantity.Enabled = false;
                }
                else
                {
                    txtMeasurementQuantity.Text = "";
                    txtMeasurementQuantity.Enabled = true;
                }
                MeasurementQuan.Visible = true;
            }
        }

        protected void LnkChangeItem_Click(object sender, EventArgs e)
        {
            lblActivityName.Visible = false;
            LinkBOQData.Visible = true;
            LnkChangeItem.Visible = false;
            Session["BOQData"] = null;
            txtprice.Value = "";
            txtquantity.Value = "";
            txtGST.Value = "";
            txtquantity.Disabled = false;
            txtGST.Disabled = false;
            txtprice.Disabled = false;
            DDLMeasurementUnit.Enabled = false;
        }

        //protected void RBLOption_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (RBLOption.SelectedValue == "1")
        //    {
        //        if (Request.QueryString["ParentTaskUID"] != null)
        //        {
        //            DataSet ds = getdata.GetTaskDetails(Request.QueryString["ParentTaskUID"]);
        //            if (ds.Tables[0].Rows.Count > 0)
        //            {
        //                if (ds.Tables[0].Rows[0]["BOQDetailsUID"].ToString() == "")
        //                {
        //                    txtTaskName.Enabled = true;
        //                    txtDescription.Enabled = true;
        //                    txtGST.Disabled = false;
        //                    txtMeasurementQuantity.Enabled = true;
        //                    txtprice.Disabled = false;
        //                    txtquantity.Disabled = false;
        //                    txtweightage.Enabled = true;
        //                    DDLTaskType.Enabled = true;
        //                    btnSubmit.Visible = true;
        //                    BtnAddBOQSubItems.Visible = false;
        //                    ChooseBOQ.Visible = true;
        //                    dtActualEndDate.Enabled = true;
        //                    dtPlanneddate.Enabled = true;
        //                    dtPlannedStartDate.Enabled = true;
        //                    dtProjecteddate.Enabled = true;
        //                    dtProjectedStartDate.Enabled = true;
        //                    dtStartdate.Enabled = true;
        //                    DDLMeasurementUnit.Enabled = true;
        //                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Task :" + DDLParentTask.SelectedItem.Text + " is not linked to BOQ Item. Please link BOQ Item first.');</script>");
        //                }
        //                else
        //                {
        //                    txtTaskName.Enabled = false;
        //                    txtDescription.Enabled = false;
        //                    txtGST.Disabled = true;
        //                    txtMeasurementQuantity.Enabled = false;
        //                    txtprice.Disabled = true;
        //                    txtquantity.Disabled = true;
        //                    txtweightage.Enabled = false;
        //                    DDLTaskType.Enabled = false;
        //                    btnSubmit.Visible = false;
        //                    BtnAddBOQSubItems.Visible = true;
        //                    ChooseBOQ.Visible = false;
        //                    dtActualEndDate.Enabled = false;
        //                    dtPlanneddate.Enabled = false;
        //                    dtPlannedStartDate.Enabled = false;
        //                    dtProjecteddate.Enabled = false;
        //                    dtProjectedStartDate.Enabled = false;
        //                    dtStartdate.Enabled = false;
        //                    DDLMeasurementUnit.Enabled = false;
        //                }
        //            }
        //        }
        //    }
        //}

        //protected void BtnAddBOQSubItems_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        string ParentTaskUID = Request.QueryString["ParentTaskUID"];

        //        DataSet ds = new DataSet();
        //        ds = getdata.GetTaskDetails(Request.QueryString["ParentTaskUID"]);
        //        if (ds.Tables[0].Rows.Count > 0)
        //        {
        //            bool result = getdata.InsertBOQItemstoTask(new Guid(ParentTaskUID), new Guid(DDLWorkPackage.SelectedValue), new Guid(Request.QueryString["PrjUID"].ToString()), new Guid(Request.QueryString["OptionUID"].ToString()), DDLUsers.SelectedValue, new Guid(ds.Tables[0].Rows[0]["BOQDetailsUID"].ToString()));
        //            if (result)
        //            {
        //                Session["BOQData"] = null;
        //                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
        //            }

        //        }
                
        //    }
        //    catch (Exception ex)
        //    {
        //        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('There is a problem with this feature. Please contact system admin');</script>");
        //    }
        //}

        //private void BindBOQDetails()
        //{
        //    DataSet ds = getdata.GetBOQDetails_by_WorkpackageUID(new Guid(DDLWorkPackage.SelectedValue));
        //    DDLBOQDetails.DataTextField = "Description";
        //    DDLBOQDetails.DataValueField = "BOQDetailsUID";
        //    DDLBOQDetails.DataSource = ds;
        //    DDLBOQDetails.DataBind();
        //    DDLBOQDetails.Items.Insert(0, new ListItem("--Select--", ""));
        //}

        //protected void DDLBOQDetails_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (DDLBOQDetails.SelectedValue != "--Select--")
        //    {
        //        DataSet dsBOQ = getdata.GetBOQDetails_by_BOQDetailsUID(new Guid(DDLBOQDetails.SelectedValue));
        //        if (dsBOQ.Tables[0].Rows.Count > 0)
        //        {
        //            txtquantity.Value = dsBOQ.Tables[0].Rows[0]["Quantity"].ToString();
        //            txtGST.Value = dsBOQ.Tables[0].Rows[0]["GST"].ToString();


        //            if (dsBOQ.Tables[0].Rows[0]["Currency"].ToString() == "&#x20B9;")
        //            {
        //                txtprice.Value = "₹ " + dsBOQ.Tables[0].Rows[0]["Price"].ToString();
        //            }
        //            else if (dsBOQ.Tables[0].Rows[0]["Currency"].ToString() == "&#36;")
        //            {
        //                txtprice.Value = "$ " + dsBOQ.Tables[0].Rows[0]["Price"].ToString();
        //            }
        //            else
        //            {
        //                txtprice.Value = "¥ " + dsBOQ.Tables[0].Rows[0]["Price"].ToString();
        //            }
        //        }
        //    }

        //}
    }
}