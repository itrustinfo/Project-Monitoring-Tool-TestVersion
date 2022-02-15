using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagementTool._content_pages.report_projectworkprogress
{
    public partial class _default : System.Web.UI.Page
    {
        TaskUpdate TKUpdate = new TaskUpdate();
        DataSet ds = new DataSet();
        DBGetData getdata = new DBGetData();
        int CumulativeCount = 0;
        double PreviousTarget = 0;
        double PreviousActual = 0;
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
                    LoadProjects();
                    ddlProject_SelectedIndexChanged(sender, e);
                }
            }
        }

        private void LoadProjects()
        {
            try
            {
                DataTable ds = new DataTable();
                if (Session["TypeOfUser"].ToString() == "U" || Session["TypeOfUser"].ToString() == "MD" || Session["TypeOfUser"].ToString() == "VP")
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
                ddlProject.Items.Insert(0, new ListItem("--Select Project--", ""));
                ddlworkpackage.Items.Insert(0, new ListItem("--Select Workpackage--", ""));
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
                    ddlworkpackage.Items.Insert(0, new ListItem("--Select Workpackage--", ""));
                    //DataSet dschild = getdata.GetConstructionProgramme_TasksForWorkPackages(ddlworkpackage.SelectedValue);
                    //if (dschild.Tables[0].Rows.Count > 0)
                    //{
                    //    TreeView1.Nodes.Clear();
                    //    PopulateTreeView(dschild, null, "", 0);
                    //    TreeView1.Nodes[0].Selected = true;
                    //    TreeView1.CollapseAll();
                    //    TreeView1.Nodes[0].Expand();
                    //    ActivityHeading.Text = "Task : " + TreeView1.SelectedNode.Text;

                    //}
                }
            }

        }

        protected void ddlworkpackage_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Loadtasks(ddlworkpackage.SelectedItem.Value);
            if (ddlworkpackage.SelectedValue != "")
            {
                DataSet dschild = getdata.GetConstructionProgramme_TasksForWorkPackages(ddlworkpackage.SelectedValue);
                if (dschild.Tables[0].Rows.Count > 0)
                {
                    TreeView1.Nodes.Clear();
                    PopulateTreeView(dschild, null, "", 0);
                    TreeView1.Nodes[0].Selected = true;
                    TreeView1.CollapseAll();
                    TreeView1.Nodes[0].Expand();
                    ActivityHeading.Text = "Task : " + TreeView1.SelectedNode.Text;
                    BindTaskSchedule(TreeView1.SelectedNode.Value);
                    Workprogress_Chart(TreeView1.SelectedNode.Value);
                }
                DivData.Visible = true;
            }
            else
            {
                DivData.Visible = false;
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
            ActivityHeading.Text = "Task : " + TreeView1.SelectedNode.Text;
            BindTaskSchedule(TreeView1.SelectedNode.Value);
            Workprogress_Chart(TreeView1.SelectedNode.Value);
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

        private void BindTaskSchedule(string TaskUID)
        {
            DataSet ds = getdata.GetTaskSchedule_By_TaskUID(new Guid(TaskUID));
            if (ds.Tables[0].Rows.Count > 0)
            {
                GrdProjectProgress.DataSource = ds;
                GrdProjectProgress.DataBind();
               
            }
            else
            {
                GrdProjectProgress.DataSource = null;
                GrdProjectProgress.DataBind();
                
            }
        }

        public string GetTaskName(string TaskUID)
        {
            return getdata.getTaskNameby_TaskUID(new Guid(TaskUID));
        }

        protected void GrdProjectProgress_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.Header)
            //{
            //    e.Row.Cells[2].Text = "Target as on " + HiddenDate.Value + " Submitted Construction Programme";
            //    e.Row.Cells[3].Text = "Achieved as on " + HiddenDate.Value;
            //}
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[1].ToString() != "" && e.Row.Cells[2].ToString() != "")
                {
                    string TargetValue = e.Row.Cells[1].Text;
                    string AchievedValue = e.Row.Cells[2].Text;
                    if (AchievedValue != "&nbsp;" && TargetValue != "&nbsp;")
                    {
                        //float Cummulative = float.Parse(TargetValue) + float.Parse(AchievedValue);
                        //float Percentage = (float.Parse(AchievedValue) / float.Parse(TargetValue)) * 100;
                        
                        //e.Row.Cells[4].Text = Math.Round(Percentage).ToString() + "%";

                        if (CumulativeCount == 0)
                        {
                            e.Row.Cells[3].Text = TargetValue;
                            e.Row.Cells[4].Text = AchievedValue;
                            PreviousTarget = Convert.ToDouble(TargetValue);
                            PreviousActual = Convert.ToDouble(AchievedValue);
                            CumulativeCount += 1;
                        }
                        else
                        {
                            e.Row.Cells[3].Text = (Convert.ToDouble(TargetValue) + PreviousTarget).ToString();
                            PreviousTarget = (Convert.ToDouble(TargetValue) + PreviousTarget);
                            e.Row.Cells[4].Text = (Convert.ToDouble(AchievedValue) + PreviousActual).ToString();
                            PreviousActual = (Convert.ToDouble(AchievedValue) + PreviousActual);
                        }
                    }
                    else
                    {
                        e.Row.Cells[3].Text = "0";
                        e.Row.Cells[4].Text = "0";
                    }
                    
                }

            }
        }

        private void Workprogress_Chart(string TaskUID)
        {
            double PrevCumulativePlan = 0;
            double PrevCumulativeActual = 0;
            DataSet ds = getdata.GetTaskSchedule_By_TaskUID(new Guid(TaskUID));
            if (ds.Tables[0].Rows.Count > 0)
            {
                StringBuilder strScript = new StringBuilder();
                strScript.Append(@"<script type='text/javascript'>
              google.charts.load('current', { 'packages':['corechart']
            });
              if (typeof google.charts.visualization == 'undefined') {
                google.charts.setOnLoadCallback(drawVisualization);
            }
            else {
                drawVisualization();
            }
                function drawVisualization()
                {
                    // Some raw data (not necessarily accurate)
                    var data = google.visualization.arrayToDataTable([
                      ['Month', 'Plan', 'Actual', 'Cumulative Plan', 'Cumulative Actual'],");
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            string Plan = ds.Tables[0].Rows[i]["Schedule_Value"].ToString();
                            string Actual = ds.Tables[0].Rows[i]["Achieved_Value"].ToString();
                            if (Plan != "" && Actual != "")
                            {
                                if (i == 0)
                                {
                                    PrevCumulativePlan = Convert.ToDouble(Plan);
                                    PrevCumulativeActual = Convert.ToDouble(Actual);
                                    strScript.Append("['" + Convert.ToDateTime(ds.Tables[0].Rows[i]["EndDate"].ToString()).ToString("MMM-yy") + "'," + Plan + "," + Actual + "," + Plan + "," + Actual + "],");
                                }
                                else
                                {
                                    strScript.Append("['" + Convert.ToDateTime(ds.Tables[0].Rows[i]["EndDate"].ToString()).ToString("MMM-yy") + "'," + Plan + "," + Actual + "," + (Convert.ToDouble(Plan) + PrevCumulativePlan) + "," + (Convert.ToDouble(Actual) + PrevCumulativeActual) + "],");
                                    //e.Row.Cells[3].Text = (Convert.ToDouble(TargetValue) + PreviousTarget).ToString();
                                    PrevCumulativePlan = (Convert.ToDouble(Plan) + PrevCumulativePlan);
                                    //e.Row.Cells[4].Text = (Convert.ToDouble(AchievedValue) + PreviousActual).ToString();
                                    PrevCumulativeActual = (Convert.ToDouble(Actual) + PrevCumulativeActual);
                                }
                            }
                        }
                    strScript.Remove(strScript.Length - 1, 1);
                    strScript.Append("]);");
                    strScript.Append(@"var options = {
                      title : 'Monthly Work Progress',
                      vAxis: { title: 'Monthly Plan'},
                      hAxis: { title: 'Month'},
                      seriesType: 'bars',
                        series: { 2: { type: 'line' },3: { type: 'line' } }

                };

                var chart = new google.visualization.ComboChart(document.getElementById('chart_div'));
                chart.draw(data, options);
                  }
                </script>");
                ltScript_Progress.Text = strScript.ToString();
            }
            else
            {
                ltScript_Progress.Text = "<h4>No data</h4>";
            }
            
        }
    }
}