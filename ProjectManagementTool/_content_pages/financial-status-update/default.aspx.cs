using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagementTool._content_pages.financial_status_update
{
    public partial class _default : System.Web.UI.Page
    {
        TaskUpdate TKUpdate = new TaskUpdate();
        DBGetData getdata = new DBGetData();
        DataSet dsPayment = new DataSet();
        InventoryCS invobj = new InventoryCS();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Username"] == null)
            {
                Response.Redirect("~/Login.aspx");
            }
            else
            {
                ScriptManager.RegisterStartupScript(
                              UpdatePanel2,
                              this.GetType(),
                              "MyAction",
                              "DateText();",
                              true);
                if (!IsPostBack)
                {
                    
                    LoadProjects();
                    SelectedProjectWorkpackage("Project");
                    ddlProject_SelectedIndexChanged(sender, e);
                    divStatus.Visible = false;
                    divStatusMonth.Visible = true;
                    AddData.HRef = "~/_modal_pages/add-Fin-Month-data.aspx?WorkPackageUID=" + ddlworkpackage.SelectedValue;
                }
            }
            
        }

        private void LoadProjects()
        {
            try
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
                ddlProject.DataSource = ds;
                ddlProject.DataTextField = "ProjectName";
                ddlProject.DataValueField = "ProjectUID";
                ddlProject.DataBind();
            }
            catch (Exception ex)
            {
            }
        }
        protected void ddlProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlProject.SelectedValue != "")
            {
                divStatus.Visible = false;
                divStatusMonth.Visible = true;
                DataSet ds = new DataSet();
                if (Session["TypeOfUser"].ToString() == "U" || Session["TypeOfUser"].ToString() == "MD" || Session["TypeOfUser"].ToString() == "VP")
                {
                    ds = getdata.GetWorkPackages_By_ProjectUID(new Guid(ddlProject.SelectedValue));
                }
                else if (Session["TypeOfUser"].ToString() == "PA")
                {
                    ds = getdata.GetWorkPackages_ForUser_by_ProjectUID(new Guid(Session["UserUID"].ToString()), new Guid(ddlProject.SelectedValue));
                }
                else
                {
                    ds = getdata.GetWorkPackages_ForUser_by_ProjectUID(new Guid(Session["UserUID"].ToString()), new Guid(ddlProject.SelectedValue));
                }
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlworkpackage.DataTextField = "Name";
                    ddlworkpackage.DataValueField = "WorkPackageUID";
                    ddlworkpackage.DataSource = ds;
                    ddlworkpackage.DataBind();
                    SelectedProjectWorkpackage("Workpackage");

                    AddData.HRef = "~/_modal_pages/add-Fin-Month-data.aspx?WorkPackageUID=" + ddlworkpackage.SelectedValue;
                    //Loadtasks(ddlworkpackage.SelectedValue);
                    //LoadSTask(ddlTask.SelectedItem.Value);

                    DataSet dschild = getdata.GetTasksForWorkPackages(ddlworkpackage.SelectedValue);
                    if (dschild.Tables[0].Rows.Count > 0)
                    {
                        TreeView1.Nodes.Clear();
                        PopulateTreeView(dschild, null, "", 0);
                        //  TreeView1.Nodes[0].Selected = true;
                        TreeView1.CollapseAll();
                        //TreeView1.Nodes[0].Expand();
                        ActivityHeading.Text = "WorkPackage : " + ddlworkpackage.SelectedItem.ToString() + " (Cash Flow Gross Payment)";
                        //divStatus.Visible = true;
                        ////LoadTaskPayments(TreeView1.SelectedNode.Value);
                        ////FillAllowedPayment();
                        //FinanceMileStoneBind(TreeView1.SelectedNode.Value);
                        FinanceMileStoneMonthBind(ddlworkpackage.SelectedValue);
                    }

                    Session["Project_Workpackage"] = ddlProject.SelectedValue + "_" + ddlworkpackage.SelectedValue;
                }
            }
            
        }

        //private void FillAllowedPayment()
        //{
        //    LblTaskName.Text = TreeView1.SelectedNode.Text;

        //    string allowedpayment = getdata.GetAllowedPayment_TaskUID(new Guid(TreeView1.SelectedNode.Value));
        //    txtallowedpayment.Text = allowedpayment;
        //}
        //private void LoadWorkPackages(string ProjectID)
        //{
        //    try
        //    {
        //        DataTable dtWorkPackage = TKUpdate.GetWorkPackage(ProjectID);
        //        ddlworkpackage.DataSource = dtWorkPackage;
        //        ddlworkpackage.DataTextField = "Name";
        //        ddlworkpackage.DataValueField = "WorkPackageUID";
        //        ddlworkpackage.DataBind();
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}

        protected void ddlworkpackage_SelectedIndexChanged(object sender, EventArgs e)
        {
            divStatus.Visible = true;
            ActivityHeading.Text = "WorkPackage : " + ddlworkpackage.SelectedItem.ToString() + " (Cash Flow Gross Payment)";
            AddData.HRef = "~/_modal_pages/add-Fin-Month-data.aspx?WorkPackageUID=" + ddlworkpackage.SelectedValue;
            //LoadTaskPayments(TreeView1.SelectedNode.Value);
            FinanceMileStoneMonthBind(ddlworkpackage.SelectedValue);
            Session["Project_Workpackage"] = ddlProject.SelectedValue + "_" + ddlworkpackage.SelectedValue;
        }

        internal void SelectedProjectWorkpackage(string pType)
        {
            if (!IsPostBack && Session["Project_Workpackage"] != null)
            {
                string[] selectedValue = Session["Project_Workpackage"].ToString().Split('_');
                if (selectedValue.Length > 1)
                {
                    if (pType == "Project")
                    {
                        ddlProject.SelectedValue = selectedValue[0];
                    }
                    else
                    {
                        ddlworkpackage.SelectedValue = selectedValue[1];
                    }
                }
                else
                {
                    if (pType == "Project")
                    {
                        ddlProject.SelectedValue = Session["Project_Workpackage"].ToString();
                    }
                }
            }

        }

        private void FinanceMileStoneMonthBind(string WorkPackageUID)
        {
            DataSet ds = getdata.GetFinance_MileStonesDetails_By_WorkPackageUID(new Guid(WorkPackageUID));
            grdMileStoneMonths.DataSource = ds;
            grdMileStoneMonths.DataBind();
        }

        public void PopulateTreeView(DataSet dtParent, TreeNode treeNode, string ParentUID, int Level)
        {
            foreach (DataRow row in dtParent.Tables[0].Rows)
            {
                TreeNode child = new TreeNode
                {
                    Text = LimitCharts(row["Name"].ToString()),
                    Value = row["TaskUID"].ToString(),
                    Target = "Tasks",
                    ToolTip = row["Name"].ToString()
                };

                //if (ParentUID == "")
                //{
                //    //treeNode.ChildNodes.Add(child);
                //    TreeView1.Nodes.Add(child);
                //    DataSet dschild = getdata.GetTasksForWorkPackages(child.Value);
                //    //DataTable dtChild = TreeViewBAL.BL.TreeViewBL.GetData("Select ID,Name from Module where ProjID=" + child.Value);
                //    if (dschild.Tables[0].Rows.Count > 0)
                //    {
                //        PopulateTreeView(dschild, child, child.Value, 1);
                //    }

                //}
                if (ParentUID == "")
                {
                    TreeView1.Nodes.Add(child);
                    DataSet dssubchild = getdata.GetSubTasksForWorkPackages(child.Value);
                    if (dssubchild.Tables[0].Rows.Count > 0)
                    {
                        PopulateTreeView(dssubchild, child, child.Value, 1);
                    }
                }
                else if (Level == 1)
                {
                    treeNode.ChildNodes.Add(child);
                    DataSet dssubtosubchild = getdata.GetSubtoSubTasksForWorkPackages(child.Value);
                    if (dssubtosubchild.Tables[0].Rows.Count > 0)
                    {
                        PopulateTreeView(dssubtosubchild, child, child.Value, 2);
                    }
                }
                else if (Level == 2)
                {
                    treeNode.ChildNodes.Add(child);
                    DataSet lastchild = getdata.GetSubtoSubtoSubTasksForWorkPackages(child.Value);
                    if (lastchild.Tables[0].Rows.Count > 0)
                    {
                        PopulateTreeView(lastchild, child, child.Value, 3);
                    }
                }
                else if (Level == 3)
                {
                    treeNode.ChildNodes.Add(child);
                    DataSet lastchild = getdata.GetSubtoSubtoSubtoSubTasksForWorkPackages(child.Value);
                    if (lastchild.Tables[0].Rows.Count > 0)
                    {
                        PopulateTreeView(lastchild, child, child.Value, 4);
                    }
                }
                else
                {
                    treeNode.ChildNodes.Add(child);
                    DataSet ds = getdata.GetTask_by_ParentTaskUID(new Guid(child.Value));
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        PopulateTreeView(ds, child, child.Value, 5);
                    }
                }
            }

        }

        protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
        {
            divStatus.Visible = true;
            ActivityHeading.Text = "Task : " + TreeView1.SelectedNode.Text;
            //FillAllowedPayment();
            //LoadTaskPayments(TreeView1.SelectedNode.Value);
            FinanceMileStoneBind(TreeView1.SelectedNode.Value);
        }
        private void FinanceMileStoneBind(string TaskUID)
        {
            divStatus.Visible = true;
            divStatusMonth.Visible = false;
            DataSet ds = getdata.GetFinance_MileStonesDetails_By_TaskUID(new Guid(TaskUID));
            grdFinanceMileStones.DataSource = ds;
            grdFinanceMileStones.DataBind();
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

        protected void grdFinanceMileStones_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string Finance_MileStoneUID = grdFinanceMileStones.DataKeys[e.Row.RowIndex].Values[0].ToString();
                string Allowedpayment = getdata.FinanceMileStoneAllowedPayment_Finance_MileStoneUID(new Guid(Finance_MileStoneUID));
                TextBox txtallowedpayment = (TextBox)e.Row.FindControl("txtallowedpayment");
                txtallowedpayment.Text = Allowedpayment;
            }
        }

        protected void grdFinanceMileStones_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int rowIndex = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName == "edit")
            {
                try
                {
                    Guid FinanceMileStoneUpdate_UID = Guid.NewGuid();
                    Button lb = (Button)e.CommandSource;
                    int index = Convert.ToInt32(lb.CommandArgument);
                    //string Finance_MileStoneUID = grdFinanceMileStones.Rows[index].Cells[0].Text;
                    string Finance_MileStoneUID = grdFinanceMileStones.DataKeys[rowIndex].Values[0].ToString();
                    TextBox allowedpayment = (TextBox)grdFinanceMileStones.Rows[index].FindControl("txtallowedpayment");
                    TextBox actualpayment = (TextBox)grdFinanceMileStones.Rows[index].FindControl("txtactualpayment");
                    TextBox paymentdate = (TextBox)grdFinanceMileStones.Rows[index].FindControl("dtActualPymentDate");
                    Label lblrequired = (Label)grdFinanceMileStones.Rows[index].FindControl("LblPaymentrequired");
                    Label lbldaterequired = (Label)grdFinanceMileStones.Rows[index].FindControl("LblPaymentDaterequired");
                    if (actualpayment.Text == "")
                    {

                        lblrequired.Text = "*";
                        //Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Actual payment cannot be empty.');</script>");
                    }
                    else if (paymentdate.Text == "" || paymentdate.Text == "dd/mm/yyyy")
                    {
                        lblrequired.Text = string.Empty;
                        lbldaterequired.Text = "*";
                    }
                    else if (Convert.ToDouble(actualpayment.Text) > Convert.ToDouble(allowedpayment.Text))
                    {
                        //Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Actual payment should be less than Allowed payment.');</script>");
                        lblrequired.Text = "* Actual Payment Exceeded";
                    }
                    else
                    {
                        lblrequired.Text = string.Empty;
                        lbldaterequired.Text = string.Empty;
                        string sDate1 = "";
                        DateTime CDate1 = DateTime.Now;

                        if (paymentdate.Text != "" && paymentdate.Text != "dd/mm/yyyy")
                        {
                            sDate1 = paymentdate.Text;
                            //sDate1 = sDate1.Split('/')[1] + "/" + sDate1.Split('/')[0] + "/" + sDate1.Split('/')[2];
                            sDate1 = getdata.ConvertDateFormat(sDate1);
                            CDate1 = Convert.ToDateTime(sDate1);
                        }

                        bool cnt = getdata.FinanceMileStonePaymentUpdate_Insert(FinanceMileStoneUpdate_UID, new Guid(Finance_MileStoneUID), Convert.ToDouble(allowedpayment.Text), Convert.ToDouble(actualpayment.Text), CDate1, new Guid(Session["UserUID"].ToString()));
                        if (cnt)
                        {
                            FinanceMileStoneBind(TreeView1.SelectedNode.Value);
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Data updated successfully.');</script>");
                        }
                    }
                }
                catch (Exception ex)
                {

                }

            }
        }

        protected void grdFinanceMileStones_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }

        protected void grdMileStoneMonths_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string Finance_MileStoneMonthUID = grdMileStoneMonths.DataKeys[e.Row.RowIndex].Values[0].ToString();
                DataSet ds = getdata.GetFinMonthsPaymentTotal(new Guid(Finance_MileStoneMonthUID));
                if(ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["TotalAmnt"].ToString() != "&nbsp;" && ds.Tables[0].Rows[0]["TotalAmnt"].ToString() != "")
                    {
                        e.Row.Cells[2].Text =(decimal.Parse(ds.Tables[0].Rows[0]["TotalAmnt"].ToString()) / 10000000).ToString("n2");
                        e.Row.Cells[3].Text = (decimal.Parse(ds.Tables[0].Rows[0]["TotalDeduc"].ToString()) / 10000000).ToString("n2");
                    }
                }

            }

            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    if (e.Row.Cells[0].Text != "&nbsp;")
            //    {
            //        DropDownList ddl = (DropDownList)e.Row.FindControl("ddlMileStoneMonth");
            //        string Finance_MileStoneUID = grdMileStoneMonths.DataKeys[e.Row.RowIndex].Values[0].ToString();
            //        ddl.DataSource = getdata.GetFinMilestoneMonths(new Guid(Finance_MileStoneUID));
            //        ddl.DataTextField = "MonthYear";
            //        ddl.DataValueField = "FinMileStoneMonthUID";
            //        ddl.DataBind();
            //        //
            //        string Allowedpayment = getdata.GetFinMilestoneMonthsData(new Guid(ddl.SelectedValue));
            //        TextBox txtallowedpayment = (TextBox)e.Row.FindControl("txtallowedpayment");
            //        txtallowedpayment.Text = Allowedpayment;

            //        DataSet ds = new DataSet();
            //        ds = getdata.GetFinMilestoneMonthPayment(new Guid(Finance_MileStoneUID), new Guid(ddl.SelectedValue));
            //        if(ds.Tables[0].Rows.Count > 0)
            //        {

            //            TextBox txtactualpayment = (TextBox)e.Row.FindControl("txtactualpayment");
            //            txtactualpayment.Text = ds.Tables[0].Rows[0]["Actual_Payment"].ToString();
            //            txtactualpayment.Enabled = false;
            //            //
            //            TextBox paymentdate = (TextBox)e.Row.FindControl("dtActualPymentDate");
            //            paymentdate.Text =Convert.ToDateTime(ds.Tables[0].Rows[0]["Actual_PaymentDate"]).ToString("dd/MM/yyyy");
            //            paymentdate.Enabled = false;
            //            //
            //            Button btnupdate =(Button)e.Row.FindControl("btnupdate");
            //            btnupdate.Visible =false;
            //        }

            //    }
            // }
        }

        

        protected void grdMileStoneMonths_RowEditing(object sender, GridViewEditEventArgs e)
        {
            grdMileStoneMonths.EditIndex = e.NewEditIndex;
            FinanceMileStoneMonthBind(ddlworkpackage.SelectedValue);
        }

        protected void ddlMileStoneMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            string str = "zuber";
            DropDownList lb = (DropDownList)sender;
            GridViewRow gvRow = (GridViewRow)lb.NamingContainer;
            // string id = gvRow.Cells[0].Text;
            string Finance_MileStoneUID = grdMileStoneMonths.DataKeys[gvRow.RowIndex].Values[0].ToString();
            string Allowedpayment = getdata.GetFinMilestoneMonthsData(new Guid(lb.SelectedValue));
            TextBox txtallowedpayment = (TextBox)gvRow.FindControl("txtallowedpayment");
            txtallowedpayment.Text = Allowedpayment;

            DataSet ds = new DataSet();
            ds = getdata.GetFinMilestoneMonthPayment(new Guid(Finance_MileStoneUID), new Guid(lb.SelectedValue));
            TextBox txtactualpayment = (TextBox)gvRow.FindControl("txtactualpayment");
            TextBox paymentdate = (TextBox)gvRow.FindControl("dtActualPymentDate");
            if (ds.Tables[0].Rows.Count > 0)
            {

                txtactualpayment.Text = ds.Tables[0].Rows[0]["Actual_Payment"].ToString();
                txtactualpayment.Enabled = false;
               
                paymentdate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["Actual_PaymentDate"]).ToString("dd/MM/yyyy");
                paymentdate.Enabled = false;
                //
                Button btnupdate = (Button)gvRow.FindControl("btnupdate");
                btnupdate.Visible = false;

            }
            else
            {
                txtactualpayment.Enabled = true;
                txtactualpayment.Text = "";
                paymentdate.Enabled = true;
                paymentdate.Text = "";
                //
                Button btnupdate = (Button)gvRow.FindControl("btnupdate");
                btnupdate.Visible = true;
            }
        }

        protected void grdMileStoneMonths_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            HiddenField hidRAbuilluid = grdMileStoneMonths.Rows[e.RowIndex].FindControl("hidDeleteuid") as HiddenField;
            int result = getdata.DeleteFinMileStoneMonth(new Guid(hidRAbuilluid.Value), new Guid(Session["UserUID"].ToString()));
            FinanceMileStoneMonthBind(ddlworkpackage.SelectedValue);
        }

        protected void grdMileStoneMonths_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            grdMileStoneMonths.EditIndex = -1;
            FinanceMileStoneMonthBind(ddlworkpackage.SelectedValue);
        }

        protected void grdMileStoneMonths_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            HiddenField hidfinMilestoneMonthuid = grdMileStoneMonths.Rows[e.RowIndex].FindControl("hidrabilluid") as HiddenField;
            HiddenField hidPlannedMonth = grdMileStoneMonths.Rows[e.RowIndex].FindControl("hidPlannedMonth") as HiddenField;
            HiddenField hidoldvalue = grdMileStoneMonths.Rows[e.RowIndex].FindControl("hidoldvalue") as HiddenField;
            string monthyear = grdMileStoneMonths.Rows[e.RowIndex].Cells[1].Text;
            TextBox txtallowedpayment = grdMileStoneMonths.Rows[e.RowIndex].FindControl("txtProjected") as TextBox;
            //  TextBox txtdate = GrdTreeView.Rows[e.RowIndex].FindControl("txtdate") as TextBox;
            if (decimal.Parse(hidoldvalue.Value) != decimal.Parse(txtallowedpayment.Text))
            {
                getdata.InsertFinMilestoneMonthExcel(new Guid(hidfinMilestoneMonthuid.Value), Guid.NewGuid(), float.Parse(txtallowedpayment.Text), hidPlannedMonth.Value.Split(' ')[0], int.Parse(hidPlannedMonth.Value.Split(' ')[1]), Guid.NewGuid(), 0);
                getdata.InsertFinMileStoneMonth_EditedValues(Guid.NewGuid(), new Guid(hidfinMilestoneMonthuid.Value), new Guid(Session["UserUID"].ToString()), decimal.Parse(hidoldvalue.Value), decimal.Parse(txtallowedpayment.Text));
                
            }
            grdMileStoneMonths.EditIndex = -1;
            FinanceMileStoneMonthBind(ddlworkpackage.SelectedValue);
        }



        //protected void btnUpdate_Click(object sender, EventArgs e)
        //{
        //    if (txtactualpayment.Text == "0")
        //    {
        //        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Actual Payment cannot be zero.');</script>");
        //    }
        //    else if (Convert.ToDouble(txtactualpayment.Text) > Convert.ToDouble(txtallowedpayment.Text))
        //    {

        //        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Actual payment should be less than Allowed payment.');</script>");

        //    }
        //    else
        //    {
        //        try
        //        {
        //            string sDate1 = "";
        //            DateTime CDate1 = DateTime.Now;
        //            if (dtActualpayment.Text != "")
        //            {
        //                sDate1 = dtActualpayment.Text;
        //                sDate1 = sDate1.Split('/')[1] + "/" + sDate1.Split('/')[0] + "/" + sDate1.Split('/')[2];
        //                CDate1 = Convert.ToDateTime(sDate1);
        //            }

        //            double TaskBudget = 0;
        //            double Cumulative = 0;
        //            DataSet ds = getdata.GetTaskDetails(TreeView1.SelectedNode.Value);
        //            if (ds.Tables[0].Rows.Count > 0)
        //            {
        //                TaskBudget = Convert.ToDouble(ds.Tables[0].Rows[0]["Total_Budget"].ToString());
        //            }

        //            if (TaskBudget > 0)
        //            {
        //                Cumulative = (TaskBudget - Convert.ToDouble(txtallowedpayment.Text)) + Convert.ToDouble(txtactualpayment.Text);
        //            }
        //            bool result = getdata.InsertorUpdateTaskPayment(Guid.NewGuid(), new Guid(TreeView1.SelectedNode.Value), Guid.Empty, Cumulative, Convert.ToDouble(txtactualpayment.Text), DateTime.Now, CDate1, new Guid(Session["UserUID"].ToString()));
        //            if (result)
        //            {
        //                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Data updated successfully.');</script>");
        //            }

        //        }
        //        catch (Exception ex)
        //        {
        //            Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error : TPU, there is a problem with this feature. Please contact system admin.');</script>");
        //        }
        //    }
        //}

        //private void LoadTaskPayments(string TaskUID)
        //{
        //    divStatus.Visible = true;
        //    grdTaskPayment.DataSource = getdata.getTaskPayments(new Guid(TaskUID));
        //    grdTaskPayment.DataBind();
        //}

        //protected void grdTaskPayment_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    if (e.Row.RowType == DataControlRowType.DataRow)
        //    {
        //        dsPayment.Clear();
        //        if (e.Row.Cells[2].Text != "&nbsp;")
        //        {
        //            //dsPayment = getdata.getMileStonesDetails(new Guid(e.Row.Cells[2].Text));
        //            //if (dsPayment.Tables[0].Rows.Count > 0)
        //            //{
        //            //    e.Row.Cells[2].Text = dsPayment.Tables[0].Rows[0]["Description"].ToString();
        //            //}
        //            //else
        //            //{
        //            //    e.Row.Cells[2].Text = "--";
        //            //}
        //        }

        //        if (e.Row.Cells[4].Text != "&nbsp;")
        //        {
        //            //ProjectManager.usercontrols.CalendeR ttx = (ProjectManager.usercontrols.CalendeR)e.Row.FindControl("dtActualpayment");
        //            TextBox dtActualpayment = (TextBox)e.Row.FindControl("dtActualpayment");
        //            //(ttx.FindControl("txtDate") as TextBox).Text = e.Row.Cells[6].Text;
        //            dtActualpayment.Text = e.Row.Cells[4].Text;

        //        }

        //    }
        //}

        //protected void grdTaskPayment_RowCommand(object sender, GridViewCommandEventArgs e)
        //{
        //    if (e.CommandName == "edit")
        //    {
        //        string TaskUID = string.Empty;
        //        Button lb = (Button)e.CommandSource;
        //        int index = Convert.ToInt32(lb.CommandArgument);
        //        TextBox dtActualpayment = (TextBox)grdTaskPayment.Rows[index].FindControl("dtActualpayment");
        //        TextBox allowedPayment = (TextBox)grdTaskPayment.Rows[index].FindControl("txtallowedpayment");
        //        TextBox actualpayment = (TextBox)grdTaskPayment.Rows[index].FindControl("txtactualpayment");
        //        if (dtActualpayment.Text == "")
        //        {
        //            Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Actual Payment Date cannot be empty.');</script>");
        //        }
        //        else if (allowedPayment.Text =="" && actualpayment.Text =="")
        //        {
        //            Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Allowed Payment &  Actual Payment cannot be empty.');</script>");
        //        }
        //        else if (allowedPayment.Text == "0" && actualpayment.Text == "0")
        //        {
        //            Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Allowed Payment &  Actual Payment cannot be zero.');</script>");
        //        }
        //        else
        //        {
        //            //if (ddlSSTask.Items.Count > 0 && ddlSSTask.SelectedIndex > 0)
        //            //{
        //            //    TaskUID = ddlSSTask.SelectedItem.Value;
        //            //}
        //            //else if (ddlsTask.Items.Count > 0 && ddlsTask.SelectedIndex > 0)
        //            //{
        //            //    TaskUID = ddlsTask.SelectedItem.Value;
        //            //}
        //            //else if (ddlTask.Items.Count > 0 && ddlTask.SelectedIndex > 0)
        //            //{
        //            //    TaskUID = ddlTask.SelectedItem.Value;

        //            //}
        //            TaskUID = TreeView1.SelectedNode.Value;
        //            Guid MileStoneUID = new Guid();



        //            //string P_UID = grdTaskPayment.Rows[index].Cells[0].Text;
        //            string P_UID = Guid.NewGuid().ToString();
        //            //if (grdTaskPayment.Rows[index].Cells[2].Text != "&nbsp;")
        //            //{
        //            //    MileStoneUID = new Guid(grdTaskPayment.Rows[index].Cells[8].Text);
        //            //}
        //            string sDate1 = "";
        //            DateTime CDate1 = DateTime.Now;


        //            //ProjectManager.usercontrols.CalendeR ttx = (ProjectManager.usercontrols.CalendeR)grdTaskPayment.Rows[index].FindControl("dtActualpayment");

        //            if (dtActualpayment.Text != "")
        //            {
        //                sDate1 = dtActualpayment.Text;
        //                sDate1 = sDate1.Split('/')[1] + "/" + sDate1.Split('/')[0] + "/" + sDate1.Split('/')[2];
        //                CDate1 = Convert.ToDateTime(sDate1);
        //            }

        //            Label LblInvoiceNumber = (Label)grdTaskPayment.Rows[index].FindControl("LblInvoiceNumber");
        //            allowedPayment.Text = allowedPayment.Text.Replace(",", "");
        //            actualpayment.Text = actualpayment.Text.Replace(",", "");
        //            //bool result = getdata.InsertorUpdateTaskPayment(new Guid(P_UID), new Guid(TaskUID), MileStoneUID, Convert.ToDouble(allowedPayment.Text), Convert.ToDouble(actualpayment.Text), DateTime.Now, CDate1, new Guid(Session["UserUID"].ToString()), new Guid(LblInvoiceNumber.Text));
        //            bool result = getdata.InsertorUpdateTaskPayment(new Guid(P_UID), new Guid(TaskUID), MileStoneUID, Convert.ToDouble(allowedPayment.Text), Convert.ToDouble(actualpayment.Text), DateTime.Now, CDate1, new Guid(Session["UserUID"].ToString()));
        //            if (result)
        //            {
        //                DataSet ds = getdata.getMilestonePayment_by_UID(new Guid(P_UID));
        //                if (ds.Tables[0].Rows.Count > 0)
        //                {
        //                    result = getdata.InsertorUpdateMileStonePayment(new Guid(P_UID), new Guid(TaskUID), MileStoneUID, ds.Tables[0].Rows[0]["Payment_Type"].ToString(), ds.Tables[0].Rows[0]["Payment_by"].ToString(), Convert.ToInt32(ds.Tables[0].Rows[0]["Payment_Tenture"].ToString()), Convert.ToDouble(allowedPayment.Text), Convert.ToDouble(actualpayment.Text), DateTime.Now, CDate1, new Guid(Session["UserUID"].ToString()), new Guid(LblInvoiceNumber.Text));
        //                }
        //                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Data updated successfully.');</script>");
        //            }
        //        }

        //    }
        //}

        //public string GetInvoiceNumber(string InvoiceUID)
        //{
        //    if (InvoiceUID != "")
        //    {
        //        return invobj.GetInvoiceNumber_by_InvoiceUID(new Guid(InvoiceUID));

        //    }
        //    else
        //    {
        //        return string.Empty;
        //    }
        //}

        //protected void grdTaskPayment_RowEditing(object sender, GridViewEditEventArgs e)
        //{

        //}
    }
}