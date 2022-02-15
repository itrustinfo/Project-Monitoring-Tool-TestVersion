using Newtonsoft.Json;
using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagementTool._content_pages.engineering_status_update
{
    public partial class _default : System.Web.UI.Page
    {
        TaskUpdate TKUpdate = new TaskUpdate();
        DataSet ds = new DataSet();
        DBGetData getdata = new DBGetData();
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
                       "BindEvents();DateText();",
                       true);
                if (!IsPostBack)
                {
                    LoadProjects();
                    SelectedProjectWorkpackage("Project");
                    ddlProject_SelectedIndexChanged(sender, e);
                    LoadStatus();

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

                ddlProject.Items.Insert(0, new ListItem("--Select--", ""));
                ddlworkpackage.Items.Insert(0, new ListItem("--Select--", ""));
            }
            catch (Exception ex)
            {
            }
        }

        private void LoadStatus()
        {
            try
            {
                DataTable dtStatus = TKUpdate.GetStatus();
                ddlpStatus.DataSource = dtStatus;
                ddlpStatus.DataTextField = "Value";
                ddlpStatus.DataValueField = "Status";
                ddlpStatus.DataBind();
            }
            catch (Exception ex)
            {
            }
        }

        private void LoadWorkPackages(string ProjectID)
        {
            try
            {
                DataTable dtWorkPackage = TKUpdate.GetWorkPackage(ProjectID);
                ddlworkpackage.DataSource = dtWorkPackage;
                ddlworkpackage.DataTextField = "Name";
                ddlworkpackage.DataValueField = "WorkPackageUID";
                ddlworkpackage.DataBind();

                ddlworkpackage.Items.Insert(0, new ListItem("--Select--", ""));
            }
            catch (Exception ex)
            {
            }
        }

        protected void ddlProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlProject.SelectedValue != "")
            {
                //DataSet ds = getdata.GetWorkPackages_By_ProjectUID(new Guid(ddlProject.SelectedValue));
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

                    ddlworkpackage.Items.Insert(0, new ListItem("--Select--", ""));

                    SelectedProjectWorkpackage("Workpackage");
                    if (Session["Project_Workpackage"] != null)
                    {
                        ddlworkpackage_SelectedIndexChanged(sender, e);
                    }
                    Session["Project_Workpackage"] = ddlProject.SelectedValue + "_" + ddlworkpackage.SelectedValue;
                    //Bind_Year(ddlworkpackage.SelectedValue);

                    //DataSet dschild = getdata.GetTasksForWorkPackages(ddlworkpackage.SelectedValue);
                    //if (dschild.Tables[0].Rows.Count > 0)
                    //{
                    //    TreeView1.Nodes.Clear();
                    //    PopulateTreeView(dschild, null, "", 0);
                    //    if (Session["SelectedActivity"] != null)
                    //    {
                    //        TreeView1.CollapseAll();
                    //        string UID = Session["SelectedActivity"].ToString();
                    //        for (int i = 0; i < TreeView1.Nodes.Count; i++)
                    //        {
                    //            retrieveNodes(TreeView1.Nodes[i], UID);
                    //        }
                    //        Session["SelectedActivity"] = null;
                    //        ActivityHeading.Text = "Task : " + TreeView1.SelectedNode.Text;
                    //        divStatus.Visible = true;
                    //    }
                    //    else
                    //    {
                    //        //TreeView1.Nodes[0].Selected = true;
                    //        TreeView1.CollapseAll();
                    //        TreeView1.Nodes[0].Expand();
                    //    }

                    //    if (TreeView1.SelectedNode != null)
                    //    {
                    //        BindTaskUpdates();
                    //    }
                    //}
                }

                RDBStatusUpdateView.ClearSelection();
                DivEngineering.Visible = false;
                DivResourceDeployment.Visible = false;
            }
            
        }
        private void BindTaskUpdates()
        {
            DataSet dsTask = getdata.GetTaskDetails(TreeView1.SelectedNode.Value);
            if (dsTask.Tables[0].Rows.Count > 0)
            {
                if (dsTask.Tables[0].Rows[0]["GroupBOQItems"].ToString() == "1")
                {
                    divStatus.Visible = false;
                    DailyUpdate.Visible = true;
                    SubTasksBind(TreeView1.SelectedNode.Value,"1");
                }
                else if (dsTask.Tables[0].Rows[0]["GroupBOQItems"].ToString() == "2")
                {
                    divStatus.Visible = false;
                    DailyUpdate.Visible = true;
                    SubTasksBind(TreeView1.SelectedNode.Value,"2");
                }
                else
                {
                    divStatus.Visible = true;
                    DailyUpdate.Visible = false;
                    LoadStatusRefersh(TreeView1.SelectedNode.Value);
                    LoadTaskResource(TreeView1.SelectedNode.Value);
                    LoadTaskMeasurementBook(TreeView1.SelectedNode.Value);
                    LoadMileStones(TreeView1.SelectedNode.Value);
                    LoadTaskSchedule(TreeView1.SelectedNode.Value);
                    YearBind(TreeView1.SelectedNode.Value);
                }
            }
        }
        protected void ddlworkpackage_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Loadtasks(ddlworkpackage.SelectedItem.Value);
            if (ddlworkpackage.SelectedValue != "")
            {
                DataSet dschild = getdata.GetTasksForWorkPackages(ddlworkpackage.SelectedValue);
                if (dschild.Tables[0].Rows.Count > 0)
                {
                    TreeView1.Nodes.Clear();
                    PopulateTreeView(dschild, null, "", 0);
                    if (TreeView1.SelectedNode != null)
                    {
                        TreeView1.Nodes[0].Selected = true;
                        divStatus.Visible = true;
                        ActivityHeading.Text = "Task : " + TreeView1.SelectedNode.Text;
                        BindTaskUpdates();
                    }
                    TreeView1.CollapseAll();
                    TreeView1.Nodes[0].Expand();
                }
                Bind_Year(ddlworkpackage.SelectedValue);
                DivEngineering.Visible = false;
                DivResourceDeployment.Visible = false;

                Session["Project_Workpackage"] = ddlProject.SelectedValue + "_" + ddlworkpackage.SelectedValue;
            }
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
            //LoadStatusRefersh(TreeView1.SelectedNode.Value);
            //LoadTaskResource(TreeView1.SelectedNode.Value);
            //LoadTaskMeasurementBook(TreeView1.SelectedNode.Value);
            //LoadMileStones(TreeView1.SelectedNode.Value);
            //LoadTaskSchedule(TreeView1.SelectedNode.Value);
            //YearBind(TreeView1.SelectedNode.Value);
            BindTaskUpdates();

            TaskScheduleHistory1.HRef = "/_modal_pages/view-measurement.aspx?TaskUID=" + TreeView1.SelectedNode.Value;
        }

        public string LimitCharts(string Desc)
        {
            if (Desc.Length > 80)
            {
                return Desc.Substring(0, 80) + "  . . .";
            }
            else
            {
                return Desc;
            }
        }

        //private void LoadSSTask(string STaskID)
        //{
        //    try
        //    {
        //        DataTable dtSSTaks = TKUpdate.GetSSTask(STaskID);
        //        ddlSSTask.DataSource = dtSSTaks;
        //        ddlSSTask.DataTextField = "Name";
        //        ddlSSTask.DataValueField = "TaskUID";
        //        ddlSSTask.DataBind();
        //        ddlSSTask.Items.Insert(0, "-Select-");
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}

        //private void LoadSTask(string TaskID)
        //{
        //    try
        //    {
        //        DataTable dtSTaks = TKUpdate.GetSTask(TaskID);
        //        ddlsTask.DataSource = dtSTaks;
        //        ddlsTask.DataTextField = "Name";
        //        ddlsTask.DataValueField = "TaskUID";
        //        ddlsTask.DataBind();
        //        ddlsTask.Items.Insert(0, "-Select-");
        //    }
        //    catch (Exception ex)
        //    {
        //    }

        //}
        //private void Loadtasks(string WorkPackID)
        //{
        //    try
        //    {
        //        DataTable dtTasks = TKUpdate.GetTasks(WorkPackID);
        //        ddlTask.DataSource = dtTasks;
        //        ddlTask.DataTextField = "Name";
        //        ddlTask.DataValueField = "TaskUID";
        //        ddlTask.DataBind();
        //        ddlTask.Items.Insert(0, "-Select-");
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}


        private void LoadMileStones(string TaskUID)
        {
            grdMileStones.DataSource = getdata.getTaskMileStones(new Guid(TaskUID));
            grdMileStones.DataBind();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                string TaskUID = string.Empty;
                //if (ddlSSTask.Items.Count > 0 && ddlSSTask.SelectedIndex > 0)
                //{
                //    TaskUID = ddlSSTask.SelectedItem.Value;
                //}
                //else if (ddlsTask.Items.Count > 0 && ddlsTask.SelectedIndex > 0)
                //{
                //    TaskUID = ddlsTask.SelectedItem.Value;
                //}
                //else if (ddlTask.Items.Count > 0 && ddlTask.SelectedIndex > 0)
                //{
                //    TaskUID = ddlTask.SelectedItem.Value;

                //}
                TaskUID = TreeView1.SelectedNode.Value;
                if (TaskUID != string.Empty)
                {
                    //string sDate1 = "";
                    DateTime CDate1 = DateTime.Now;
                    //sDate1 = (dtTargetDate.FindControl("txtDate") as TextBox).Text;
                    //if (sDate1 != "")
                    //{
                    //    sDate1 = sDate1.Split('/')[1] + "/" + sDate1.Split('/')[0] + "/" + sDate1.Split('/')[2];
                    //    CDate1 = Convert.ToDateTime(sDate1);
                    //}
                    double Percentage = 0;
                    if (ddlpStatus.SelectedItem.Text == "In-Progress")
                    {
                        //Percentage = Convert.ToDouble(txtpercentage.Text);
                        Percentage = 0;
                    }
                    else if (ddlpStatus.SelectedItem.Text == "Not Started")
                    {
                        Percentage = 0;
                    }
                    else
                    {
                        Percentage = 100;
                    }

                    int k = TKUpdate.UpdateTaskStatus(ddlworkpackage.SelectedItem.Value, TaskUID, ddlpStatus.SelectedItem.Value, CDate1, Percentage, "");
                }
            }
            catch (Exception ex)
            {
            }

        }

        private void LoadStatusRefersh(string sTaskUID)
        {
            ds = getdata.GetTaskDetails(sTaskUID);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlpStatus.SelectedValue = ds.Tables[0].Rows[0]["Status"].ToString();
                //txtpercentage.Text = ds.Tables[0].Rows[0]["StatusPer"].ToString();

                lblLastStatus.Text = ds.Tables[0].Rows[0]["Status"].ToString() == "P" ? "Not Started" : ds.Tables[0].Rows[0]["Status"].ToString() == "I" ? "In-Progress" : "Completed";

                //if (ds.Tables[0].Rows[0]["Status"].ToString() == "C")
                //{
                //    //btnSubmit.Enabled = false;
                //    txtpercentage.Enabled = false;
                //    txtcomments.Enabled = false;
                //}
                //else
                //{
                //    //btnSubmit.Enabled = true;
                //    txtpercentage.Enabled = true;
                //    txtcomments.Enabled = true;
                //}
            }
            ds.Clear();
            ds = getdata.GetTaskStatus(new Guid(sTaskUID));
            if (ds.Tables[0].Rows.Count > 0)
            {
                //double Percentage = getdata.GetTaskPercentage(new Guid(sTaskUID));
                //if (Percentage > 0)
                //{
                //    txtpercentage.Text = Percentage.ToString();
                //}
                //lblLastStatus.Text = ds.Tables[0].Rows[0]["Value"].ToString();
                lblDateL.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["CDate"].ToString()).ToString("dd MMM yyyy",CultureInfo.InvariantCulture);
                //ddlpStatus.SelectedValue = ds.Tables[0].Rows[0]["Status"].ToString();

                
            }
            else
            {
                lblLastStatus.Text = "-";
                lblDateL.Text = "-";
            }
        }

        private void LoadTaskSchedule(string TaskUID)
        {
            txtachieved.Text = "";
            LblTragetValue.Text = "";
            DataSet ds = getdata.GetTaskSchedule_By_TaskUID(new Guid(TaskUID));
            if (ds.Tables[0].Rows.Count > 0)
            {
                ActivitySchedule.Visible = true;
                TaskScheduleHistory.Visible = true;
                LblNotDataTaskSchedule.Text = "";
                TaskScheduleHistory.HRef = "/_modal_pages/view-measurement.aspx?TaskUID=" + TreeView1.SelectedNode.Value;
                //TaskScheduleHistory.HRef = "/_modal_pages/task-schedulehistory.aspx?TaskUID=" + TaskUID;

                //TaskScheduleHistory1.HRef= "/_modal_pages/task-schedulehistory.aspx?TaskUID=" + TaskUID;
            }
            else
            {
                ActivitySchedule.Visible = false;
                LblNotDataTaskSchedule.Text = "No Records Found.";
                TaskScheduleHistory.Visible = false;
                //TaskScheduleHistory1.HRef = "/_modal_pages/task-schedulehistory.aspx?TaskUID=" + TaskUID;
            }
            //GrdTaskSchedule.DataSource = ds;
            //GrdTaskSchedule.DataBind();
        }
        private void LoadTaskResource(string sTaskUID)
        {

            grdResources.DataSource = getdata.getTaskResourceAllocated(new Guid(sTaskUID));
            grdResources.DataBind();

        }

        protected void btnUpdateStatus_Click(object sender, EventArgs e)
        {
            try
            {
                string TaskUID = string.Empty;
                //if (ddlSSTask.Items.Count > 0 && ddlSSTask.SelectedIndex > 0)
                //{
                //    TaskUID = ddlSSTask.SelectedItem.Value;
                //}
                //else if (ddlsTask.Items.Count > 0 && ddlsTask.SelectedIndex > 0)
                //{
                //    TaskUID = ddlsTask.SelectedItem.Value;
                //}
                //else if (ddlTask.Items.Count > 0 && ddlTask.SelectedIndex > 0)
                //{
                //    TaskUID = ddlTask.SelectedItem.Value;

                //}
                TaskUID = TreeView1.SelectedNode.Value;
                if (TaskUID != string.Empty)
                {
                    //string sDate1 = "";
                    DateTime CDate1 = DateTime.Now;
                    //sDate1 = (dtTargetDate.FindControl("txtDate") as TextBox).Text;
                    //if (sDate1 != "")
                    //{
                    //    sDate1 = sDate1.Split('/')[1] + "/" + sDate1.Split('/')[0] + "/" + sDate1.Split('/')[2];
                    //    CDate1 = Convert.ToDateTime(sDate1);
                    //}
                    double Percentage = 0;
                    if (ddlpStatus.SelectedItem.Text == "In-Progress")
                    {
                        //Percentage = Convert.ToDouble(txtpercentage.Text);
                        Percentage = 0;
                    }
                    else if (ddlpStatus.SelectedItem.Text == "Not Started")
                    {
                        Percentage = 0;
                    }
                    else
                    {
                        Percentage = 100;
                    }
                    int k = TKUpdate.UpdateTaskStatus(ddlworkpackage.SelectedItem.Value, TaskUID, ddlpStatus.SelectedItem.Value, CDate1, Percentage, "");
                    if (k > 0)
                    {
                        LoadStatusRefersh(TaskUID);
                        LoadStatus();
                        //txtcomments.Text = "";
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Status updated successfully.');</script>");
                    }
                    
                }
            }
            catch (Exception ex)
            {
            }
        }

        protected void grdResources_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "cmdEdit")
            {

                Button lb = (Button)e.CommandSource;
                int index = Convert.ToInt32(lb.CommandArgument);
                Guid UID = Guid.NewGuid();
                //
                //grdList.Rows[index].Cells[1].Text;
                double TotalUsed = 0;
                double TotalUsage = Convert.ToDouble(grdResources.Rows[index].Cells[5].Text);
                string UsedUsage = grdResources.Rows[index].Cells[6].Text;
                
                TextBox txtUSage = (TextBox)grdResources.Rows[index].FindControl("txtCurrentUsage");
                Label lblwarning = (Label)grdResources.Rows[index].FindControl("LblWarning");
                if (UsedUsage != "")
                {
                    TotalUsed = TotalUsage - Convert.ToDouble(grdResources.Rows[index].Cells[6].Text);
                }
                if (Convert.ToDouble(txtUSage.Text) > TotalUsed)
                {

                    lblwarning.Text = "* Current usage exceeded";
                }
                else
                {
                    lblwarning.Text = string.Empty;
                    bool result = getdata.InsertorUpdateResourceUsage(UID, new Guid(grdResources.Rows[index].Cells[0].Text), new Guid(grdResources.Rows[index].Cells[2].Text), new Guid(grdResources.Rows[index].Cells[1].Text), new Guid(grdResources.Rows[index].Cells[3].Text), Convert.ToDouble(txtUSage.Text), DateTime.Now, DateTime.Now, new Guid(Session["UserUID"].ToString()));
                    if (result)
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Data updated successfully.');</script>");
                        LoadTaskResource(grdResources.Rows[index].Cells[3].Text);
                    }
                }
            }
        }

        protected void grdResources_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                ds.Clear();
                string TaskUID = string.Empty;
                //if (ddlSSTask.Items.Count > 0 && ddlSSTask.SelectedIndex > 0)
                //{
                //    TaskUID = ddlSSTask.SelectedItem.Value;
                //}
                //else if (ddlsTask.Items.Count > 0 && ddlsTask.SelectedIndex > 0)
                //{
                //    TaskUID = ddlsTask.SelectedItem.Value;
                //}
                //else if (ddlTask.Items.Count > 0 && ddlTask.SelectedIndex > 0)
                //{
                //    TaskUID = ddlTask.SelectedItem.Value;

                //}
                TaskUID = TreeView1.SelectedNode.Value;
                ds = getdata.getTotalResourceUsage(new Guid(e.Row.Cells[0].Text), new Guid(TaskUID));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    e.Row.Cells[6].Text = ds.Tables[0].Rows[0]["TotalUsage"].ToString();
                }
                //
                if (e.Row.Cells[6].Text == "")
                {
                    e.Row.Cells[6].Text = "0";
                }


            }
        }

        private void LoadTaskMeasurementBook(string sTaskUID)
        {
            ds.Clear();
            ds = getdata.GetTaskDetails(sTaskUID);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["UnitforProgress"].ToString() != "")
                {
                    MeasurementUpdate.Visible = false;
                    lblUnit.Text = ds.Tables[0].Rows[0]["UnitforProgress"].ToString();

                    DataSet ds1 = getdata.GetTaskMeasurementBook(new Guid(sTaskUID));
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        double CumulativeQuan = getdata.GetMeasurementCumulativeQuantity(new Guid(sTaskUID));
                        txtmaxQuantity.Text = (Convert.ToDouble(ds.Tables[0].Rows[0]["UnitQuantity"].ToString()) - CumulativeQuan).ToString();
                        lblLastQuantity.Text = ds1.Tables[0].Rows[0]["Quantity"].ToString();
                        lblLastUpdate.Text = Convert.ToDateTime(ds1.Tables[0].Rows[0]["CreatedDate"].ToString()).ToString("dd MMM yyyy",CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        txtmaxQuantity.Text = ds.Tables[0].Rows[0]["UnitQuantity"].ToString();
                        lblLastQuantity.Text = "-";
                        lblLastUpdate.Text = "-";
                    }
                    txtQuantity.Text = "0";
                    MeasurementUpdateText.Visible=false;
                }
                else
                {
                    MeasurementUpdate.Visible = false;
                    MeasurementUpdateText.Visible = false;
                }
                
            }
        }

        protected void btnupdateBook_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtQuantity.Text == "")
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Current Quantity cannot be empty.');</script>");
                }
                else if (txtQuantity.Text == "0")
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Current Quantity cannot be zero.');</script>");
                }
                else if (Convert.ToDouble(txtQuantity.Text) > Convert.ToDouble(txtmaxQuantity.Text))
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Current Quantity should be less than max Quantity.');</script>");
                }
                else
                {
                    string TaskUID = string.Empty;
                    //if (ddlSSTask.Items.Count > 0 && ddlSSTask.SelectedIndex > 0)
                    //{
                    //    TaskUID = ddlSSTask.SelectedItem.Value;
                    //}
                    //else if (ddlsTask.Items.Count > 0 && ddlsTask.SelectedIndex > 0)
                    //{
                    //    TaskUID = ddlsTask.SelectedItem.Value;
                    //}
                    //else if (ddlTask.Items.Count > 0 && ddlTask.SelectedIndex > 0)
                    //{
                    //    TaskUID = ddlTask.SelectedItem.Value;

                    //}
                    TaskUID = TreeView1.SelectedNode.Value;
                    if (TaskUID != string.Empty)
                    {
                        //string sDate1 = "";
                        DateTime CDate1 = DateTime.Now;
                        //sDate1 = (dtTargetDate.FindControl("txtDate") as TextBox).Text;
                        //if (sDate1 != "")
                        //{
                        //    sDate1 = sDate1.Split('/')[1] + "/" + sDate1.Split('/')[0] + "/" + sDate1.Split('/')[2];
                        //    CDate1 = Convert.ToDateTime(sDate1);
                        //}
                        string DocPath = "";
                        if (FileUpload1.HasFile)
                        {
                            FileUpload1.SaveAs(Server.MapPath("~/Documents/" + FileUpload1.FileName));
                            DocPath = "~/Documents/" + FileUpload1.FileName;
                        }
                        string AchievedDate = DDLDay.SelectedItem.Text + "/" + DDLMonth.SelectedValue + "/" + DDLYear.SelectedValue;
                        AchievedDate = getdata.ConvertDateFormat(AchievedDate);
                        DateTime CDate2 = Convert.ToDateTime(AchievedDate);


                        int rs = getdata.InsertorUpdateTaskMeasurementBook(Guid.NewGuid(), new Guid(TaskUID), lblUnit.Text, txtQuantity.Text, "", CDate1, DocPath, new Guid(Session["UserUID"].ToString()), txtMeasurementRemarks.Text, CDate2);
                        if (rs > 0)
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Data updated successfully.');</script>");
                            LoadTaskMeasurementBook(TaskUID);
                        }

                    }
                }
                
            }
            catch (Exception ex)
            {
            }
        }

        protected void ddlpStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (ddlpStatus.SelectedItem.Text == "In-Progress")
            //{
            //    txtpercentage.Enabled = true;

            //}
            //else
            //{
            //    txtpercentage.Enabled = false;
            //    txtpercentage.Text = "";
            //}
        }

        protected void grdMileStones_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            if (e.CommandName == "edit")
            {
                string TaskUID = string.Empty;
                //if (ddlSSTask.Items.Count > 0 && ddlSSTask.SelectedIndex > 0)
                //{
                //    TaskUID = ddlSSTask.SelectedItem.Value;
                //}
                //else if (ddlsTask.Items.Count > 0 && ddlsTask.SelectedIndex > 0)
                //{
                //    TaskUID = ddlsTask.SelectedItem.Value;
                //}
                //else if (ddlTask.Items.Count > 0 && ddlTask.SelectedIndex > 0)
                //{
                //    TaskUID = ddlTask.SelectedItem.Value;

                //}
                TaskUID = TreeView1.SelectedNode.Value;

                Button lb = (Button)e.CommandSource;
                int index = Convert.ToInt32(lb.CommandArgument);

                string M_UID = grdMileStones.Rows[index].Cells[0].Text;

                string sDate1 = "", sDate2 = "";
                DateTime CDate1 = DateTime.Now, CDate2 = DateTime.Now;

                if (grdMileStones.Rows[index].Cells[2].Text != "" && grdMileStones.Rows[index].Cells[2].Text != "DD/MM/YYYY")
                {
                    sDate1 = grdMileStones.Rows[index].Cells[2].Text;
                    //sDate1 = sDate1.Split('/')[1] + "/" + sDate1.Split('/')[0] + "/" + sDate1.Split('/')[2];
                    sDate1 = getdata.ConvertDateFormat(sDate1);
                    CDate1 = Convert.ToDateTime(sDate1);
                }
                TextBox dtprojectDate = (TextBox)grdMileStones.Rows[index].FindControl("dtprojectDate");
                //ProjectManager.usercontrols.CalendeR ttx = (ProjectManager.usercontrols.CalendeR)grdMileStones.Rows[index].FindControl("dtprojectDate");

                if (dtprojectDate.Text != "" && dtprojectDate.Text != "DD/MM/YYYY")
                {
                    sDate2 = dtprojectDate.Text;
                    //sDate2 = sDate2.Split('/')[1] + "/" + sDate2.Split('/')[0] + "/" + sDate2.Split('/')[2];
                    sDate2 = getdata.ConvertDateFormat(sDate2);
                    CDate2 = Convert.ToDateTime(sDate2);
                }

                DropDownList ddlstatus = (DropDownList)grdMileStones.Rows[index].FindControl("DDLStatus");

                bool result = getdata.InsertorUpdateMileStone(new Guid(M_UID), new Guid(TaskUID), grdMileStones.Rows[index].Cells[1].Text, CDate1, ddlstatus.SelectedValue, DateTime.Now, CDate2, new Guid(Session["UserUID"].ToString()));
                if (result)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Data updated successfully.');</script>");
                    LoadMileStones(TaskUID);
                }
            }
        }

        protected void grdMileStones_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[6].Text != "&nbsp;")
                {
                    TextBox dtprojectDate = (TextBox)e.Row.FindControl("dtprojectDate");
                    //ProjectManager.usercontrols.CalendeR ttx = (ProjectManager.usercontrols.CalendeR)e.Row.FindControl("dtprojectDate");
                    //(ttx.FindControl("txtDate") as TextBox).Text = e.Row.Cells[6].Text;
                    dtprojectDate.Text= e.Row.Cells[6].Text;

                }
                if (e.Row.Cells[7].Text != "")
                {
                    DropDownList ddlstatus = (DropDownList)e.Row.FindControl("DDLStatus");
                    ddlstatus.SelectedValue = e.Row.Cells[7].Text;
                    if (ddlstatus.SelectedValue == "Completed")
                    {
                        ddlstatus.Enabled = false;
                    }
                    else
                    {
                        ddlstatus.Enabled = true;
                    }
                }
            }
        }

        protected void grdMileStones_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }

        //protected void GrdTaskSchedule_RowCommand(object sender, GridViewCommandEventArgs e)
        //{
        //    if (e.CommandName == "TaskEdit")
        //    {
        //        string TaskUID = TreeView1.SelectedNode.Value;

        //        Button lb = (Button)e.CommandSource;
        //        int index = Convert.ToInt32(lb.CommandArgument);
        //        float Targetvalue = float.Parse(GrdTaskSchedule.Rows[index].Cells[2].Text);
        //        TextBox Achieved = (TextBox)GrdTaskSchedule.Rows[index].FindControl("txtachieved");
        //        Label LblWarningNew = (Label)GrdTaskSchedule.Rows[index].FindControl("LblWarningNew");

        //        if (float.Parse(Achieved.Text) > Targetvalue)
        //        {
        //            LblWarningNew.Text = "* Achieved value exceeded";
        //            Achieved.Text = "0";
        //        }
        //        else
        //        {
        //            LblWarningNew.Text = string.Empty;
        //            string TaskScheduleUID = GrdTaskSchedule.Rows[index].Cells[5].Text;
        //            int cnt = getdata.TaskSchedule_Target_Update(new Guid(TaskScheduleUID), float.Parse(Achieved.Text), DateTime.Now);
        //            if (cnt > 0)
        //            {
        //                LoadTaskSchedule(TaskUID);
        //            }
        //        }
               
        //    }
        //}

        //protected void GrdTaskSchedule_RowEditing(object sender, GridViewEditEventArgs e)
        //{

        //}

        //protected void GrdTaskSchedule_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    if (e.Row.RowType == DataControlRowType.DataRow)
        //    {
        //        string TaskScheduleUID = GrdTaskSchedule.DataKeys[e.Row.RowIndex].Values[0].ToString();

        //        DataSet ds = getdata.GetTaskSchedule_by_TaskScheduleUID(new Guid(TaskScheduleUID));
        //        if (ds.Tables[0].Rows.Count > 0)
        //        {
        //            TextBox txtachieved = (TextBox)e.Row.FindControl("txtachieved");
        //            if (ds.Tables[0].Rows[0]["Achieved_Value"].ToString() != "")
        //            {
        //                txtachieved.Text = ds.Tables[0].Rows[0]["Achieved_Value"].ToString();
        //            }
        //        }
        //    }
        //}

        protected void btnScheduleUpdate_Click(object sender, EventArgs e)
        {
            if (DDLYear.SelectedValue != "" && DDLMonth.SelectedValue != "")
            {
                try
                {
                    float Targetvalue = float.Parse(LblTragetValue.Text);
                    string TaskUID = TreeView1.SelectedNode.Value;
                    string sUnit = "";
                    if (LblTaskUnitofProgress.Text == "")
                    {
                        sUnit = "RM";
                    }
                    else
                    {
                        sUnit = LblTaskUnitofProgress.Text;
                    }

                    if (txtachieved.Text == "")
                    {
                        txtachieved.Text = LblTragetValue.Text;
                    }
                    if (LblTragetValue.Text == "")
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Current Quantity cannot be empty.');</script>");
                    }
                    else if (txtachieved.Text == "0")
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Current Quantity cannot be zero.');</script>");
                    }
                    else if (Convert.ToDouble(txtAchievedCurrent.Text) > Convert.ToDouble(txtachieved.Text))
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Current Quantity should be less than max Quantity.');</script>");
                    }
                    else if (TaskUID == string.Empty)
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Please select a task to update measurement.');</script>");
                    }
                    else
                    {
                        string sDate1 = "";
                        DateTime CDate1 = new DateTime();

                        sDate1 = DDLDay.SelectedValue + "/" + DDLMonth.SelectedValue + "/" + DDLYear.SelectedValue;
                        sDate1 = getdata.ConvertDateFormat(sDate1);
                        CDate1 = Convert.ToDateTime(sDate1);

                        Guid MeasurementUID = Guid.NewGuid();
                        int result = getdata.InsertMeasurementBookWithoutTaskGrouping(MeasurementUID, new Guid(TaskUID), sUnit, txtAchievedCurrent.Text, "", CDate1, "", new Guid(Session["UserUID"].ToString()), txtMeasurementRemarks.Text, DateTime.Now);
                        if (result > 0)
                        {
                            if (WebConfigurationManager.AppSettings["Dbsync"] == "Yes")
                            {
                                DataSet copysite = getdata.GetDataCopySiteDetails_by_ProjectUID(new Guid(ddlProject.SelectedValue));
                                if (copysite.Tables[0].Rows.Count > 0)
                                {
                                    int TaskLevel = getdata.getTaskLevel_By_TaskUID(new Guid(TaskUID));

                                    string Tasks = getdata.getTaskNameby_TaskUID(new Guid(TaskUID)) + ",";
                                    string ParentTaskUID = "";
                                    for (int i = 1; i < TaskLevel; i++)
                                    {
                                        if (i == 1)
                                        {
                                            string TName = getdata.GetParentTaskName_by_TaskUID(new Guid(TaskUID));
                                            Tasks += TName.Split('{')[1] + ",";
                                            ParentTaskUID = TName.Split('{')[0];
                                        }
                                        else
                                        {
                                            string TName = getdata.GetParentTaskUID_TaskName_by_TaskUID(new Guid(ParentTaskUID));
                                            Tasks += TName.Split('{')[1] + ",";
                                            ParentTaskUID = TName.Split('{')[0];
                                        }

                                    }
                                    Tasks = Tasks.TrimEnd(',');
                                    var strArr = Tasks.Split(',').Select(p => p.Trim()).ToArray();
                                    var output = string.Join(",", strArr.Reverse());

                                    string WebAPIURL = copysite.Tables[0].Rows[0]["DataCopySiteURL"].ToString();
                                    WebAPIURL = WebAPIURL + "Activity/TaskMeasurementUpdatewithoutGrouping";

                                    string postData = "MeasurementUID=" + MeasurementUID + "&ProjectName=" + ddlProject.SelectedItem.Text + "&WorkpackageName=" + ddlworkpackage.SelectedItem.Text + "&Tasks=" + output + "&Quantity=" + txtAchievedCurrent.Text + "&Remarks=" + txtMeasurementRemarks.Text + "&UserEmail=" + getdata.GetUserEmail_By_UserUID_New(new Guid(Session["UserUID"].ToString())) + "&TLevel=" + TaskLevel + "&UnitforProgress=" + sUnit + "&SelectedDate=" + CDate1;
                                    string sReturnStatus = webPostMethod(postData, WebAPIURL);
                                    if (!sReturnStatus.StartsWith("Error:"))
                                    {
                                        dynamic DynamicData = JsonConvert.DeserializeObject(sReturnStatus);
                                        string RetStatus = DynamicData.Status;
                                        if (!RetStatus.StartsWith("Error:"))
                                        {
                                            //Update Server tag to Y
                                            int cnt = getdata.MeasurementServerFlags_Update(MeasurementUID, 1);
                                            if (cnt > 0)
                                            {
                                                cnt = getdata.MeasurementServerFlags_Update(MeasurementUID, 2);
                                                if (cnt > 0)
                                                {
                                                    cnt = getdata.TaskSchedule_ServerCopiedUpdate(new Guid(TaskUID), CDate1);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            string ErrorMessage = DynamicData.Message;
                                            WebAPIStatusInsert(Guid.NewGuid(), WebAPIURL, postData, ErrorMessage, "Failure", "Measurement Book Update", "MeasurementUpdate", MeasurementUID);
                                            //Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error: DBSync =" + ErrorMessage + "');</script>");
                                        }
                                    }
                                    else
                                    {
                                        WebAPIStatusInsert(Guid.NewGuid(), WebAPIURL, postData, sReturnStatus, "Failure", "Measurement Book Update", "MeasurementUpdate", MeasurementUID);
                                    }
                                }
                            }
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Measurement updated successfully');</script>");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('There is a problem with this feature.Please contact system admin. Description : " + ex.Message + "');</script>");
                }

                txtMeasurementRemarks.Text = "";
                txtAchievedCurrent.Text = "";
                ActivityTargetBind();                //LblWarningNew.Text = string.Empty;
                //int cnt = getdata.TaskSchedule_Target_Update(new Guid(HiddenTaskScheduleUID.Value), float.Parse(txtachieved.Text), DateTime.Now);
                //if (cnt > 0)
                //{
                //    string sFileDirectory = "~/SitePhotographs";

                //    if (!Directory.Exists(Server.MapPath(sFileDirectory)))
                //    {
                //        Directory.CreateDirectory(Server.MapPath(sFileDirectory));

                //    }
                //    string sDate1 = "";
                //    DateTime CDate1 = DateTime.Now;

                //    sDate1 = "01/" + DDLMonth.SelectedValue + "/" + DDLYear.SelectedValue;
                //    sDate1 = getdata.ConvertDateFormat(sDate1);
                //    CDate1 = Convert.ToDateTime(sDate1);

                //    int NotSupportedImageCount = 0;
                //    foreach (HttpPostedFile uploadedFile in SiteUploadPhotograph.PostedFiles)
                //    {
                //        if (uploadedFile.ContentLength > 0 && !String.IsNullOrEmpty(uploadedFile.FileName))
                //        {
                //            string sFileName = Path.GetFileName(uploadedFile.FileName);
                //            string FileExtn = Path.GetExtension(uploadedFile.FileName);
                //            if (FileExtn.ToUpper() == ".JPG" || FileExtn.ToUpper() == ".JPEG" || FileExtn.ToUpper() == ".PNG" || FileExtn.ToUpper() == ".GIF" || FileExtn.ToUpper() == ".TIFF")
                //            {
                //                uploadedFile.SaveAs(Server.MapPath(sFileDirectory + "/" + sFileName));
                //                bool res = getdata.InsertorUpdateTaskSitePhotoGraph(Guid.NewGuid(), new Guid(ddlworkpackage.SelectedValue), new Guid(TreeView1.SelectedNode.Value), new Guid(HiddenTaskScheduleUID.Value), (sFileDirectory + "/" + sFileName), CDate1);
                //                if (!res)
                //                {
                //                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code TSUP-01 there is a problem with this feature. Please contact system admin.');</script>");
                //                }
                //            }
                //            else
                //            {
                //                NotSupportedImageCount += 1;
                //            }
                //        }
                //    }

                //    if (NotSupportedImageCount > 0)
                //    {
                //        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Some of the uploded image formats are not supported. Please contact system admin.');</script>");
                //    }
                //}

                //if (float.Parse(txtachieved.Text) > Targetvalue)
                //{
                //    LblWarningNew.Text = "* Achieved value exceeded";
                //    txtachieved.Text = "";
                //}
                //else
                //{


                //}
            }
        }

        private void YearBind(string TaskUID)
        {
            DDLYear.Items.Clear();
            DDLMonth.Items.Clear();
            DDLDay.Items.Clear();
            try
            {
                DataSet ds = getdata.GetTaskDetails(TaskUID);
                btnScheduleUpdate.Visible = false;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["PlannedStartDate"].ToString() != "" && ds.Tables[0].Rows[0]["PlannedEndDate"].ToString() != "")
                    {
                        LblTaskUnitofProgress.Text = ds.Tables[0].Rows[0]["UnitforProgress"].ToString();
                        DDLYear.Items.Add(new ListItem("--Year--", ""));
                        DDLMonth.Items.Add(new ListItem("--Month--", ""));
                        DDLDay.Items.Add(new ListItem("--Day--", ""));
                        int StartYear = 0;
                        int EndDateYear = 0;
                        int count = getdata.CheckTaskProgressedateExceeds_TaskUID(new Guid(TaskUID));
                        if (count > 0)
                        {


                            DataSet dsworkpackage = getdata.GetWorkPackages_By_WorkPackageUID(new Guid(ddlworkpackage.SelectedValue));
                            if (dsworkpackage.Tables[0].Rows.Count > 0)
                            {
                                if (dsworkpackage.Tables[0].Rows[0]["StartDate"].ToString() != "" && dsworkpackage.Tables[0].Rows[0]["PlannedEndDate"].ToString() != "")
                                {
                                    StartYear = Convert.ToDateTime(dsworkpackage.Tables[0].Rows[0]["StartDate"].ToString()).Year;
                                    EndDateYear = Convert.ToDateTime(dsworkpackage.Tables[0].Rows[0]["PlannedEndDate"].ToString()).Year;
                                }
                                else
                                {
                                    StartYear = Convert.ToDateTime(ds.Tables[0].Rows[0]["PlannedStartDate"].ToString()).Year;
                                    EndDateYear = Convert.ToDateTime(ds.Tables[0].Rows[0]["PlannedEndDate"].ToString()).Year;
                                }
                            }
                        }
                        else
                        {
                            StartYear = Convert.ToDateTime(ds.Tables[0].Rows[0]["PlannedStartDate"].ToString()).Year;
                            EndDateYear = Convert.ToDateTime(ds.Tables[0].Rows[0]["PlannedEndDate"].ToString()).Year;
                        }

                        for (int i = StartYear; i <= EndDateYear; i++)
                        {
                            DDLYear.Items.Add(new ListItem(i.ToString(), i.ToString()));
                        }
                        //HiddenProgressExceeds.Value = count.ToString();
                        HiddenProgressExceeds.Value = "1";
                    }
                    else
                    {
                        ActivitySchedule.Visible = false;
                        LblNotDataTaskSchedule.Text = "No Records Found.";
                        TaskScheduleHistory.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error in Month Selection: " + ex.Message + "');</script>");
            }
            
        }

        protected void DDLYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            DDLMonth.Items.Clear();
            DDLDay.Items.Clear();
            if (DDLYear.SelectedValue != "")
            {
                DataSet ds = new DataSet();
                if (HiddenProgressExceeds.Value == "1")
                {
                    ds = getdata.GetworkpackageMonths_by_WorkpackageUID_Year(new Guid(ddlworkpackage.SelectedValue), Convert.ToInt32(DDLYear.SelectedValue),new Guid(TreeView1.SelectedNode.Value));
                }
                else
                {
                    ds = getdata.GetTaskScheduleMonths_by_TaskUID_Year(new Guid(TreeView1.SelectedNode.Value), Convert.ToInt32(DDLYear.SelectedValue));
                    
                }
                DDLMonth.DataTextField = "ScheduleMonthString";
                DDLMonth.DataValueField = "ScheduleMonthInt";
                DDLMonth.DataSource = ds;
                DDLMonth.DataBind();
                DDLMonth_SelectedIndexChanged(sender, e);
                ActivityTargetBind();
            }
            else
            {
                txtachieved.Text = "";
                LblTragetValue.Text = "";
            }
        }

        protected void DDLMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDLMonth.SelectedValue != "")
            {
                ActivityTargetBind();
                //int days = DateTime.DaysInMonth(Convert.ToInt32(DDLYear.SelectedValue), Convert.ToInt32(DDLMonth.SelectedValue));
                //List<DateTime> daysOfMonth = Enumerable.Range(1, DateTime.DaysInMonth(Convert.ToInt32(DDLYear.SelectedValue), Convert.ToInt32(DDLMonth.SelectedValue)))  // Days: 1, 2 ... 31 etc.
                //             .Select(day => new DateTime(Convert.ToInt32(DDLYear.SelectedValue), Convert.ToInt32(DDLMonth.SelectedValue), day)) // Map each day to a date
                //             .ToList();

                DDLDay.DataSource = Enumerable.Range(1, DateTime.DaysInMonth(Convert.ToInt32(DDLYear.SelectedValue), Convert.ToInt32(DDLMonth.SelectedValue)));
                DDLDay.DataBind();
            }
        }

        private void ActivityTargetBind()
        {
            if (DDLMonth.SelectedValue != "" && DDLYear.SelectedValue != "")
            {
                DataSet ds = getdata.GetTaskSchedule_by_TaksUID_Month_Year(new Guid(TreeView1.SelectedNode.Value), Convert.ToInt32(DDLMonth.SelectedValue), Convert.ToInt32(DDLYear.SelectedValue));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    LblTragetValue.Text = Convert.ToDouble(ds.Tables[0].Rows[0]["Schedule_Value"].ToString()).ToString("0.###");
                    if (ds.Tables[0].Rows[0]["Achieved_Value"].ToString() != "")
                    {
                        txtachieved.Text = Convert.ToDouble(ds.Tables[0].Rows[0]["Achieved_Value"].ToString()).ToString("0.###");
                    }
                    else
                    {
                        txtachieved.Text = "";
                    }
                    HiddenTaskScheduleUID.Value = ds.Tables[0].Rows[0]["TaskScheduleUID"].ToString();
                    btnScheduleUpdate.Visible = true;
                }
                else
                {
                    LblTragetValue.Text = "0";
                }
            }
        }

        private void SubTasksBind(string ParentUID,string GroupBOQItems)
        {
            try
            {

                HiddenGroupBOQItems.Value = GroupBOQItems;
                if (GroupBOQItems == "1")
                {
                    DataSet ds = getdata.GetTask_by_ParentTaskUID(new Guid(ParentUID));
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        GrdMeasurmentUpdate.DataSource = ds;
                        GrdMeasurmentUpdate.DataBind();
                        TaskScheduleHistory1.Visible = false;
                    }
                    else
                    {
                        ds = getdata.GetTaskDetails(ParentUID);
                        GrdMeasurmentUpdate.DataSource = ds;
                        GrdMeasurmentUpdate.DataBind();
                        TaskScheduleHistory1.Visible = true;
                    }
                    
                }
                else
                {
                    DataSet ds = getdata.GetTaskDetails(ParentUID);
                    GrdMeasurmentUpdate.DataSource = ds;
                    GrdMeasurmentUpdate.DataBind();
                    TaskScheduleHistory1.Visible = true;
                }
            }
            catch (Exception ex)
            {

            }
            
        }
        protected void retrieveNodes(TreeNode node, string SelectedVal)
        {
            if (node.Value == SelectedVal)
            {
                node.Selected = true;
                node.Expand();
                if (node.Parent != null)
                {
                    node.Parent.Expand();
                }
            }
            else
            {
                if (node.ChildNodes.Count != 0)
                {
                    foreach (TreeNode tn in node.ChildNodes)
                    {
                        if (tn.Value == SelectedVal)
                        {
                            tn.Selected = true;
                            tn.Expand();
                            if (tn.Parent != null)
                            {
                                tn.Parent.Expand();
                                if (tn.Parent.Parent != null)
                                {
                                    tn.Parent.Parent.Expand();

                                    if (tn.Parent.Parent.Parent != null)
                                    {
                                        tn.Parent.Parent.Parent.Expand();

                                        if (tn.Parent.Parent.Parent.Parent != null)
                                        {
                                            tn.Parent.Parent.Parent.Parent.Expand();

                                            if (tn.Parent.Parent.Parent.Parent.Parent != null)
                                            {
                                                tn.Parent.Parent.Parent.Parent.Parent.Expand();

                                                if (tn.Parent.Parent.Parent.Parent.Parent.Parent != null)
                                                {
                                                    tn.Parent.Parent.Parent.Parent.Parent.Parent.Expand();
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (tn.ChildNodes.Count != 0)
                            {
                                retrieveNodes(tn, SelectedVal);
                            }
                        }
                    }
                }
            }
        }

        public string GetTaskName(string TaskUID)
        {
            return getdata.getTaskNameby_TaskUID(new Guid(TaskUID));
        }

        protected void BtnMeasurementUpdate_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in GrdMeasurmentUpdate.Rows)
            {
                string TaskUID = GrdMeasurmentUpdate.DataKeys[row.RowIndex].Value.ToString();

                try
                {
                    TextBox txtTodayQuantity = (TextBox)row.FindControl("txttodayQuantity");
                    TextBox txtRemarks = (TextBox)row.FindControl("txtremarks");
                    Label LblBOQQuantity = (Label)row.FindControl("LblBOQQuantity");
                    TextBox txtDate = (TextBox)row.FindControl("dtDate");

                    if (txtTodayQuantity.Text != "" && txtDate.Text != "")
                    {
                        string sUnit = "";
                        Label LblUnitforProgress = (Label)row.FindControl("LblUnitforProgress");
                        if (LblUnitforProgress == null)
                        {
                            sUnit = "RM";
                        }
                        else
                        {
                            sUnit = LblUnitforProgress.Text;
                        }
                        if (txtTodayQuantity.Text == "")
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Current Quantity cannot be empty.');</script>");
                        }
                        else if (txtTodayQuantity.Text == "0")
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Current Quantity cannot be zero for activity : " + GetTaskName(TaskUID) + "');</script>");
                        }
                        else if (Convert.ToDouble(txtTodayQuantity.Text) > Convert.ToDouble(LblBOQQuantity.Text))
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Current Quantity should be less than max Quantity for activity: " + GetTaskName(TaskUID) + "');</script>");
                        }
                        else if (txtDate.Text == "")
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Achieved Date cannnot be empty for activity: " + GetTaskName(TaskUID) + "');</script>");
                        }
                        else
                        {

                            if (TaskUID != string.Empty)
                            {
                                string sDate1 = "";
                                DateTime CDate1 = new DateTime();
                                if (txtDate.Text != "")
                                {
                                    //DateTime CDate1 = DateTime.Now;
                                    sDate1 = txtDate.Text;
                                    //sDate2 = sDate2.Split('/')[1] + "/" + sDate2.Split('/')[0] + "/" + sDate2.Split('/')[2];
                                    sDate1 = getdata.ConvertDateFormat(sDate1);
                                    CDate1 = Convert.ToDateTime(sDate1);
                                }
                                else
                                {
                                    CDate1 = DateTime.Now;
                                }


                                string DocPath = "";
                                if (FileUpload1.HasFile)
                                {
                                    FileUpload1.SaveAs(Server.MapPath("~/Documents/" + FileUpload1.FileName));
                                    DocPath = "~/Documents/" + FileUpload1.FileName;
                                }

                                Guid MeasurementUID = Guid.NewGuid();
                                int rs = getdata.InsertorUpdateTaskMeasurementBook(MeasurementUID, new Guid(TaskUID), sUnit, txtTodayQuantity.Text, "", DateTime.Now, DocPath, new Guid(Session["UserUID"].ToString()), txtRemarks.Text, CDate1);
                                if (rs > 0)
                                {
                                    if (WebConfigurationManager.AppSettings["Dbsync"] == "Yes")
                                    {
                                        DataSet copysite = getdata.GetDataCopySiteDetails_by_ProjectUID(new Guid(ddlProject.SelectedValue));
                                        if (copysite.Tables[0].Rows.Count > 0)
                                        {
                                            int TaskLevel = getdata.getTaskLevel_By_TaskUID(new Guid(TaskUID));

                                            string Tasks = getdata.getTaskNameby_TaskUID(new Guid(TaskUID)) + ",";
                                            string ParentTaskUID = "";
                                            for (int i = 1; i < TaskLevel; i++)
                                            {
                                                if (i == 1)
                                                {
                                                    string TName = getdata.GetParentTaskName_by_TaskUID(new Guid(TaskUID));
                                                    Tasks += TName.Split('{')[1] + ",";
                                                    ParentTaskUID = TName.Split('{')[0];
                                                }
                                                else
                                                {
                                                    string TName = getdata.GetParentTaskUID_TaskName_by_TaskUID(new Guid(ParentTaskUID));
                                                    Tasks += TName.Split('{')[1] + ",";
                                                    ParentTaskUID = TName.Split('{')[0];
                                                }

                                            }
                                            Tasks = Tasks.TrimEnd(',');
                                            var strArr = Tasks.Split(',').Select(p => p.Trim()).ToArray();
                                            var output = string.Join(",", strArr.Reverse());
                                            //string WebAPIURL = WebConfigurationManager.AppSettings["DbsyncWebApiURL"];
                                            string WebAPIURL = copysite.Tables[0].Rows[0]["DataCopySiteURL"].ToString();
                                            WebAPIURL = WebAPIURL + "Activity/TaskMeasurementUpdate";

                                            string postData = "MeasurementUID=" + MeasurementUID + "&ProjectName=" + ddlProject.SelectedItem.Text + "&WorkpackageName=" + ddlworkpackage.SelectedItem.Text + "&Tasks=" + output + "&Quantity=" + txtTodayQuantity.Text + "&Remarks=" + txtRemarks.Text + "&UserEmail=" + getdata.GetUserEmail_By_UserUID_New(new Guid(Session["UserUID"].ToString())) + "&TLevel=" + TaskLevel + "&UnitofProgress=" + sUnit + "&AchievedDate=" + CDate1;
                                            string sReturnStatus = webPostMethod(postData, WebAPIURL);
                                            if (!sReturnStatus.StartsWith("Error:"))
                                            {
                                                dynamic DynamicData = JsonConvert.DeserializeObject(sReturnStatus);
                                                string RetStatus = DynamicData.Status;
                                                if (!RetStatus.StartsWith("Error:"))
                                                {
                                                    //Update Server tag to Y
                                                    int cnt = getdata.MeasurementServerFlags_Update(MeasurementUID, 1);
                                                    if (cnt > 0)
                                                    {
                                                        cnt = getdata.MeasurementServerFlags_Update(MeasurementUID, 2);
                                                        if (cnt > 0)
                                                        {
                                                            cnt = getdata.TaskSchedule_ServerCopiedUpdate(new Guid(TaskUID), CDate1);
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    string ErrorMessage = DynamicData.Message;
                                                    WebAPIStatusInsert(Guid.NewGuid(), WebAPIURL, postData, ErrorMessage, "Failure", "Measurement Book Update", "MeasurementUpdate", MeasurementUID);
                                                    //Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error: DBSync =" + ErrorMessage + "');</script>");
                                                }
                                            }
                                            else
                                            {
                                                WebAPIStatusInsert(Guid.NewGuid(), WebAPIURL, postData, sReturnStatus, "Failure", "Measurement Book Update", "MeasurementUpdate", MeasurementUID);
                                            }
                                        }
                                    }
                                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Data updated successfully.');</script>");

                                }
                                else if (rs == -1)
                                {
                                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Achieved Date not valid for task : " + GetTaskName(TaskUID) + ".');</script>");
                                }
                                else if (rs == -2)
                                {
                                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Achieved Date not valid for task : " + GetTaskName(TaskUID) + ".');</script>");
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }

            SubTasksBind(TreeView1.SelectedNode.Value, HiddenGroupBOQItems.Value);
        }

        public string webPostMethod(string postData, string URL)
        {
            try
            {
                string responseFromServer = "";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
                request.Method = "POST";
                request.Credentials = CredentialCache.DefaultCredentials;
                ((HttpWebRequest)request).UserAgent =
                                  "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 7.1; Trident/5.0)";
                request.Accept = "/";
                request.UseDefaultCredentials = true;
                request.Proxy.Credentials = System.Net.CredentialCache.DefaultCredentials;
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();

                WebResponse response = request.GetResponse();
                dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                responseFromServer = reader.ReadToEnd();
                reader.Close();
                dataStream.Close();
                response.Close();
                return responseFromServer;
            }
            catch (Exception ex)
            {
                return "Error:" + ex.Message;
            }
        }

        protected void GrdMeasurmentUpdate_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string TaskUID = GrdMeasurmentUpdate.DataKeys[e.Row.RowIndex].Values[0].ToString();
                Label lblCumulativeQuan = (Label)e.Row.FindControl("LblCumulativeQuantity");
                Label LblLastUpdatedDate = (Label)e.Row.FindControl("LblLastUpdatedDate");
                Label LblUnit = (Label)e.Row.FindControl("LblUnitforProgress");
                if (e.Row.Cells[0].Text != "" && (e.Row.Cells[2].Text.ToUpper() == "SQM" || e.Row.Cells[2].Text.ToUpper() == "SQ M"))
                {
                    string DiaofPipe_in_mm = getdata.GetBOQDiaofPipe_by_BOQDetailsUID(new Guid(e.Row.Cells[0].Text));
                    if (DiaofPipe_in_mm == "Error1:" || DiaofPipe_in_mm == "")
                    {
                        DiaofPipe_in_mm = "0";
                    }

                    Label BOQQuantity = (Label)e.Row.FindControl("LblBOQQuantity");
                    double BOQinMeters = (Convert.ToDouble(BOQQuantity.Text) / (3.1415 * Convert.ToDouble(DiaofPipe_in_mm)));
                    BOQQuantity.Text = Math.Round(BOQinMeters).ToString();
                }
                double CumulativeQuan = getdata.GetMeasurementCumulativeQuantity(new Guid(TaskUID));
                lblCumulativeQuan.Text = CumulativeQuan.ToString();
                LblLastUpdatedDate.Text = getdata.GetMeasurementLastUpdate_Date(new Guid(TaskUID));
            }
        }

        private void Bind_Year(string WorkpackageUID)
        {
            DataSet ds = getdata.GetWorkPackages_By_WorkPackageUID(new Guid(WorkpackageUID));
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["StartDate"].ToString() != "" && ds.Tables[0].Rows[0]["PlannedEndDate"].ToString() != "")
                {
                    try
                    {
                        DDLYear.Items.Clear();
                        //DDLMonth.Items.Clear();
                        DDLYear.Items.Add(new ListItem("-- Year --", ""));
                        DDLMonth.Items.Add(new ListItem("-- Month --", ""));
                        DDLDay.Items.Add(new ListItem("-- Day --", ""));
                        int StartYear = Convert.ToDateTime(ds.Tables[0].Rows[0]["StartDate"].ToString()).Year;
                        int EndDateYear = Convert.ToDateTime(ds.Tables[0].Rows[0]["PlannedEndDate"].ToString()).Year;
                        for (int i = StartYear; i <= EndDateYear; i++)
                        {
                            DDLResourceYear.Items.Add(new ListItem(i.ToString(), i.ToString()));
                        }
                    }
                    catch (Exception ex)
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error : " + ex.Message + "');</script>");
                    }
                }
            }

        }

        protected void BtnResourceDeploymentSubmit_Click(object sender, EventArgs e)
        {
            if (DDLResourceMonth.SelectedValue != "" && DDLResourceYear.SelectedValue != "")
            {
                string sDate1 = "";
                DateTime CDate1 = DateTime.Now;

                sDate1 = "01/" + DDLResourceMonth.SelectedValue + "/" + DDLResourceYear.SelectedValue;
                sDate1 = getdata.ConvertDateFormat(sDate1);
                CDate1 = Convert.ToDateTime(sDate1);

                DataSet ds = getdata.GetResourceDeployment_by_WorkpackageUID_Month(new Guid(ddlworkpackage.SelectedValue), CDate1);
                GrdResourceDeployment.DataSource = ds;
                GrdResourceDeployment.DataBind();
                DivEngineering.Visible = false;
                DivResourceDeployment.Visible = true;

            }
            else
            {
                
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Please select month or year.');</script>");
            }
        }

        protected void GrdResourceDeployment_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string DeploymentUID = e.CommandArgument.ToString();
            if (e.CommandName == "edit")
            {
                string sDate1 = "";
                DateTime CDate1 = new DateTime();
                GridViewRow row = (GridViewRow)(((Button)e.CommandSource).NamingContainer);
                TextBox txtDeployed = (TextBox)row.FindControl("txtdeployedResource");
                TextBox txtremarks = (TextBox)row.FindControl("txtremarks");
                TextBox txtdeployDate = (TextBox)row.FindControl("txtdeployeddate");
                Label LblPlanned = (Label)row.FindControl("LblPlanned");
                Label LblCumulativeDepolyed = (Label)row.FindControl("LblDeployed");
                Label lblmsg = (Label)row.FindControl("LblDateNotEntered");
                Label lblDeployedMsg = (Label)row.FindControl("LbldeployedMsg");
                double RemainingDeploy = Convert.ToDouble(LblPlanned.Text) - Convert.ToDouble(LblCumulativeDepolyed.Text);
                if (txtDeployed.Text == "")
                {
                    lblDeployedMsg.Text = "* Required";
                }
                else if (txtdeployDate.Text == "")
                {
                    //Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Select the Deploy date');</script>");

                    lblmsg.Text = "* Required";
                }
                else if (Convert.ToDouble(txtDeployed.Text) > RemainingDeploy)
                {
                    //Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Deployed value cannot be greater than Planned');</script>");
                    lblDeployedMsg.Text = "* Cannot be greater than Planned";
                    txtDeployed.Text = "";
                }
                else
                {
                    sDate1 = txtdeployDate.Text;
                    //sDate2 = sDate2.Split('/')[1] + "/" + sDate2.Split('/')[0] + "/" + sDate2.Split('/')[2];
                    sDate1 = getdata.ConvertDateFormat(sDate1);
                    CDate1 = Convert.ToDateTime(sDate1);
                    Guid UID = Guid.NewGuid();

                    int cnt = getdata.ResourceDeployment_Update(UID,new Guid(DeploymentUID), float.Parse(txtDeployed.Text), CDate1, txtremarks.Text);
                    if (cnt > 0)
                    {
                        lblmsg.Text = "";
                        lblDeployedMsg.Text = "";
                        if (WebConfigurationManager.AppSettings["Dbsync"] == "Yes")
                        {
                            DataSet copysite = getdata.GetDataCopySiteDetails_by_ProjectUID(new Guid(ddlProject.SelectedValue));
                            if (copysite.Tables[0].Rows.Count > 0)
                            {
                                //string WebAPIURL = WebConfigurationManager.AppSettings["DbsyncWebApiURL"];
                                string WebAPIURL = copysite.Tables[0].Rows[0]["DataCopySiteURL"].ToString();
                                WebAPIURL = WebAPIURL + "Activity/ResourceDeploymentUpdate";
                                string ResourceName = getdata.GetResourceName_by_ReourceDeploymentUID(new Guid(DeploymentUID));
                                string postData = "UID=" + UID + "&ProjectName=" + ddlProject.SelectedItem.Text + "&WorkpackageName=" + ddlworkpackage.SelectedItem.Text + "&ResourceName=" + ResourceName + "&Deployed=" + txtDeployed.Text + "&Remarks=" + txtremarks.Text + "&DeployedDate=" + CDate1;
                                try
                                {
                                    string sReturnStatus = webPostMethod(postData, WebAPIURL);
                                    if (!sReturnStatus.StartsWith("Error:"))
                                    {
                                        dynamic DynamicData = JsonConvert.DeserializeObject(sReturnStatus);

                                        string RetStatus = DynamicData.Status;
                                        if (!RetStatus.StartsWith("Error:"))
                                        {
                                            int rCnt = getdata.ServerFlagsUpdate(UID.ToString(), 1, "ResourceDeploymentUpdate", "Y", "UID");
                                            if (rCnt > 0)
                                            {
                                                rCnt = getdata.ServerFlagsUpdate(UID.ToString(), 2, "ResourceDeploymentUpdate", "Y", "UID");
                                                if (rCnt > 0)
                                                {
                                                    rCnt = getdata.ServerFlagsUpdate(DeploymentUID.ToString(), 2, "ResourceDeployment", "Y", "ReourceDeploymentUID");
                                                    //rCnt = getdata.ResourceDeploymentUpdateServerFlags_Update(UID, 2);
                                                }
                                            }

                                        }
                                        else
                                        {
                                            string ErrorMessage = DynamicData.Message;
                                            WebAPIStatusInsert(Guid.NewGuid(), WebAPIURL, postData, ErrorMessage, "Failure", "Resource Deployment Update", "ResourceDeploymentUpdate", UID);
                                            //Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error: DBSync =" + ErrorMessage + "');</script>");
                                        }
                                    }
                                    else
                                    {
                                        WebAPIStatusInsert(Guid.NewGuid(), WebAPIURL, postData, sReturnStatus, "Failure", "Resource Deployment Update", "ResourceDeploymentUpdate", UID);
                                    }

                                }
                                catch (Exception ex)
                                {
                                    WebAPIStatusInsert(Guid.NewGuid(), WebAPIURL, postData, ex.Message, "Failure", "Resource Deployment Update", "ResourceDeploymentUpdate", UID);
                                }
                            }
                                
                        }
                            BtnResourceDeploymentSubmit_Click(sender, e);
                        //Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Updated Successfully.');</script>");
                    }
                }

            }
        }

        protected void GrdResourceDeployment_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }

        public string WebAPIStatusInsert(Guid WebAPIUID, string url, string WebAPIParameters, string WebAPI_Error, string WebAPIStatus, string WebAPIType, string WebAPIFunction,Guid WebAPI_PrimaryKey)
        {
            string Retval = "";

            int cnt = getdata.WebAPIStatusInsert(WebAPIUID, url, WebAPIParameters, WebAPI_Error, WebAPIStatus, WebAPIType, WebAPIFunction, WebAPI_PrimaryKey);
            if (cnt <= 0)
            {
                Retval = "Insertion Failed for WebAPIStaus table";
            }
            return Retval;
        }

        protected void RDBStatusUpdateView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RDBStatusUpdateView.SelectedValue == "Engineering")
            {
                DivEngineering.Visible = true;
                DivResourceDeployment.Visible = false;
                if (ddlworkpackage.SelectedValue != "")
                {
                    DataSet dschild = getdata.GetTasksForWorkPackages(ddlworkpackage.SelectedValue);
                    if (dschild.Tables[0].Rows.Count > 0)
                    {
                        TreeView1.Nodes.Clear();
                        PopulateTreeView(dschild, null, "", 0);
                        if (TreeView1.SelectedNode != null)
                        {
                            TreeView1.Nodes[0].Selected = true;
                            divStatus.Visible = true;
                            ActivityHeading.Text = "Task : " + TreeView1.SelectedNode.Text;
                            BindTaskUpdates();
                        }
                        TreeView1.CollapseAll();
                        TreeView1.Nodes[0].Expand();
                    }
                    Bind_Year(ddlworkpackage.SelectedValue);
                }
            }
            else
            {
                DivEngineering.Visible = false;
                DivResourceDeployment.Visible = true;
                Bind_Year(ddlworkpackage.SelectedValue);
            }
        }
    }
}