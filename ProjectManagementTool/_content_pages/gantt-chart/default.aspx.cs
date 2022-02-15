using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagementTool._content_pages.gantt_chart
{
    public partial class _default : System.Web.UI.Page
    {
        DBGetData dbgetdata = new DBGetData();
        TaskUpdate gettk = new TaskUpdate();
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
                    BindProject();
                    SelectedProjectWorkpackage("Project");
                    DDlProject_SelectedIndexChanged(sender, e);

                }
            }
        }

        private void BindProject()
        {
            DataTable ds = new DataTable();
            if (Session["TypeOfUser"].ToString() == "U" || Session["TypeOfUser"].ToString() =="MD" || Session["TypeOfUser"].ToString() == "VP")
            {
                ds = gettk.GetProjects();
            }
            else if (Session["TypeOfUser"].ToString() == "PA")
            {
                //ds = gettk.GetProjects_by_UserUID(new Guid(Session["UserUID"].ToString()));
                ds = gettk.GetAssignedProjects_by_UserUID(new Guid(Session["UserUID"].ToString()));
            }
            else
            {
                //ds = gettk.GetProjects();
                ds = gettk.GetAssignedProjects_by_UserUID(new Guid(Session["UserUID"].ToString()));
            }
            DDlProject.DataTextField = "ProjectName";
            DDlProject.DataValueField = "ProjectUID";
            DDlProject.DataSource = ds;
            DDlProject.DataBind();

        }

        protected void DDlProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDlProject.SelectedValue != "")
            {
                //DataSet ds = dbgetdata.GetWorkPackages_By_ProjectUID(new Guid(DDlProject.SelectedValue));
                DataSet ds = new DataSet();
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

                    LoadActivity(DDLWorkPackage.SelectedValue);

                    Session["Project_Workpackage"] = DDlProject.SelectedValue + "_" + DDLWorkPackage.SelectedValue;
                }
            }
                
        }

        protected void DDLWorkPackage_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDLWorkPackage.SelectedValue != "")
            {
                LoadActivity(DDLWorkPackage.SelectedValue);
                Session["Project_Workpackage"] = DDlProject.SelectedValue + "_" + DDLWorkPackage.SelectedValue;
            }
        }

        internal void SelectedProjectWorkpackage(string pType)
        {
            if (!IsPostBack)
            {
                if (Session["Project_Workpackage"] != null)
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
            
        }

        private void LoadActivity(string WorkPackageUID)
        {
            DataSet ds = dbgetdata.GetTasksForWorkPackages(WorkPackageUID);
            if (ds.Tables[0].Rows.Count > 0)
            {
                StringBuilder strScript = new StringBuilder();
                strScript.Append(@"<script type='text/javascript'> 
                    var g = new JSGantt.GanttChart(document.getElementById('GanttChartDIV'), 'month');
                    g.setOptions({
                      vCaptionType: 'Complete', 
                      vQuarterColWidth: 36,
                      vDateTaskDisplayFormat: 'day dd month yyyy',
                      vDayMajorDateDisplayFormat: 'mon yyyy - Week ww',
                      vWeekMinorDateDisplayFormat: 'dd mon',
                      vLang: 'en',
                      vShowTaskInfoLink: 1,
                      vShowEndWeekDate: 0,
                      vShowRes: (0),
                      vShowComp:(0),
                      vDayColWidth:36,
                      vWeekColWidth:64,
                      vUseSingleCell: 10000,
                      vFormatArr: ['Day', 'Week', 'Month', 'Quarter'],
                    });");
                int pgroup = 0;
                string pClass = string.Empty;
                //int mParent = 0;
                DataSet dsworkpackage = dbgetdata.GetWorkPackages_By_WorkPackageUID(new Guid(WorkPackageUID));
                if (dsworkpackage.Tables[0].Rows.Count > 0)
                {
                    pgroup = ds.Tables[0].Rows.Count > 0 ? 1 : 0;
                    strScript.Append(@"g.AddTaskItemObject({pID: '" + dsworkpackage.Tables[0].Rows[0]["WorkPackageUID"].ToString() + "',pName: '" + dsworkpackage.Tables[0].Rows[0]["Name"].ToString().Replace("'", "") + "',pStart: '" + Convert.ToDateTime(dsworkpackage.Tables[0].Rows[0]["StartDate"].ToString()).ToString("yyyy-MM-dd") + "',pEnd: '" + Convert.ToDateTime(dsworkpackage.Tables[0].Rows[0]["PlannedEndDate"].ToString()).ToString("yyyy-MM-dd") + "',");
                    strScript.Append(@"pClass: 'ggroupblack',pLink: '',pMile: 0,pRes: '',pComp: 0,pGroup: " + pgroup + ",pParent: 0,pOpen: 1,pDepend: '',pCaption: '',pCost: 1000,pNotes: '" + dsworkpackage.Tables[0].Rows[0]["Status"].ToString() + "'});");
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        int TaskCritical = 0;
                        if (ds.Tables[0].Rows[i]["StartDate"].ToString() != "" && ds.Tables[0].Rows[i]["PlannedEndDate"].ToString() != "")
                        {
                            bool DelayedTask = false;
                            if (ds.Tables[0].Rows[i]["Status"].ToString() == "In-Progress")
                            {
                                DelayedTask = dbgetdata.Check_Task_is_Delayed(new Guid(ds.Tables[0].Rows[i]["TaskUID"].ToString())) > 0 ? true : false;
                            }
                            pgroup = dbgetdata.GetTaskCount_by_ParentTaskID(new Guid(ds.Tables[0].Rows[i]["TaskUID"].ToString())) > 0 ? 1 : 0;
                            TaskCritical = dbgetdata.Is_Task_Critical(new Guid(ds.Tables[0].Rows[i]["TaskUID"].ToString()));
                            if (TaskCritical > 0)
                            {
                                pClass = "gtaskpurple";
                            }
                            else
                            {
                                pClass = ds.Tables[0].Rows[i]["Status"].ToString() == "Completed" ? "gtaskgreen" : (ds.Tables[0].Rows[i]["Status"].ToString() == "In-Progress" && !DelayedTask) ? "gtaskblue" : (ds.Tables[0].Rows[i]["Status"].ToString() == "In-Progress" && DelayedTask) ? "gtaskred" : "gtaskyellow";
                            }

                            DataSet Milsestones = dbgetdata.getTaskMileStones(new Guid(ds.Tables[0].Rows[i]["TaskUID"].ToString()));
                            if (Milsestones.Tables[0].Rows.Count > 0)
                            {
                                //mParent = mParent + 1;
                                //strScript.Append(@"g.AddTaskItemObject({pID: '"+ mParent + "',pName: 'MileStones for (" + ds.Tables[0].Rows[i]["Name"].ToString().Replace("'", "") + ")',pStart: '" + Convert.ToDateTime(Milsestones.Tables[0].Rows[0]["MileStoneDate"].ToString()).ToString("yyyy-MM-dd") + "',pEnd: '" + Convert.ToDateTime(Milsestones.Tables[0].Rows[0]["ProjectedDate"].ToString()).ToString("yyyy-MM-dd") + "',");
                                //strScript.Append(@"pClass: 'gtaskblack',pLink: '',pMile: 0,pRes: '',pComp: 0,pGroup: 1,pParent: '" + ds.Tables[0].Rows[i]["TaskUID"].ToString() + "',pOpen: 1,pDepend: '',pCaption: '',pCost: 0,pNotes: '" + Milsestones.Tables[0].Rows[0]["Description"].ToString().Replace("'", "")+" MileStones" + "'});");

                                for (int mi = 0; mi < Milsestones.Tables[0].Rows.Count; mi++)
                                {
                                    strScript.Append(@"g.AddTaskItemObject({pID: '" + Milsestones.Tables[0].Rows[mi]["MileStoneUID"].ToString() + "',pName: '" + Milsestones.Tables[0].Rows[mi]["Description"].ToString().Replace("'", "") + "',pStart: '" + Convert.ToDateTime(Milsestones.Tables[0].Rows[mi]["MileStoneDate"].ToString()).ToString("yyyy-MM-dd") + "',pEnd: '" + Convert.ToDateTime(Milsestones.Tables[0].Rows[mi]["ProjectedDate"].ToString()).ToString("yyyy-MM-dd") + "',");
                                    strScript.Append(@"pClass: 'gtaskblack',pLink: '',pMile: 1,pRes: '',pComp: 0,pGroup: 0,pParent: '" + ds.Tables[0].Rows[i]["TaskUID"].ToString() + "',pOpen: 1,pDepend: '',pCaption: '',pCost: 0,pNotes: '" + Milsestones.Tables[0].Rows[mi]["Description"].ToString().Replace("'", "") + "'});");
                                }

                            }

                            string dependencyvalues = string.Empty;
                            DataSet dependency = dbgetdata.getDependencies(new Guid(ds.Tables[0].Rows[i]["TaskUID"].ToString()));
                            if (dependency.Tables[0].Rows.Count > 0)
                            {
                                for (int dp = 0; dp < dependency.Tables[0].Rows.Count; dp++)
                                {
                                    dependencyvalues += dependency.Tables[0].Rows[dp]["Dependent_TaskUID"].ToString() + dependency.Tables[0].Rows[dp]["Dependency_Type"].ToString() + ",";
                                }
                                dependencyvalues = dependencyvalues.TrimEnd(',');
                            }
                            string tStatus = ds.Tables[0].Rows[i]["Status"].ToString();

                            strScript.Append(@"g.AddTaskItemObject({pID: '" + ds.Tables[0].Rows[i]["TaskUID"].ToString() + "',pName: '" + ds.Tables[0].Rows[i]["Name"].ToString().Replace("'", "") + "',pStart: '" + Convert.ToDateTime(ds.Tables[0].Rows[i]["StartDate"].ToString()).ToString("yyyy-MM-dd") + "',pEnd: '" + Convert.ToDateTime(ds.Tables[0].Rows[i]["PlannedEndDate"].ToString()).ToString("yyyy-MM-dd") + "',");
                            strScript.Append(@"pClass: '" + pClass + "',pLink: '',pMile: 0,pRes: '',pComp: " + ds.Tables[0].Rows[i]["StatusPer"].ToString() + ",pGroup: " + pgroup + ",pParent: '" + dsworkpackage.Tables[0].Rows[0]["WorkPackageUID"].ToString() + "',pOpen: 1,pDepend: '" + dependencyvalues + "',pCaption: '',pCost: 1000,pNotes: '" + tStatus + "'});");

                            DataSet dsSecond = dbgetdata.GetTask_by_ParentTaskUID(new Guid(ds.Tables[0].Rows[i]["TaskUID"].ToString()));
                            for (int j = 0; j < dsSecond.Tables[0].Rows.Count; j++)
                            {
                                if (dsSecond.Tables[0].Rows[j]["StartDate"].ToString() != "" && dsSecond.Tables[0].Rows[j]["PlannedEndDate"].ToString() != "")
                                {
                                    if (dsSecond.Tables[0].Rows[j]["Status"].ToString() == "I")
                                    {
                                        DelayedTask = dbgetdata.Check_Task_is_Delayed(new Guid(dsSecond.Tables[0].Rows[j]["TaskUID"].ToString())) > 0 ? true : false;
                                    }
                                    pgroup = dbgetdata.GetTaskCount_by_ParentTaskID(new Guid(dsSecond.Tables[0].Rows[j]["TaskUID"].ToString())) > 0 ? 1 : 0;
                                    TaskCritical = dbgetdata.Is_Task_Critical(new Guid(dsSecond.Tables[0].Rows[j]["TaskUID"].ToString()));
                                    if (TaskCritical > 0)
                                    {
                                        pClass = "gtaskpurple";
                                    }
                                    else
                                    {
                                        pClass = dsSecond.Tables[0].Rows[j]["Status"].ToString() == "C" ? "gtaskgreen" : (dsSecond.Tables[0].Rows[j]["Status"].ToString() == "I" && !DelayedTask) ? "gtaskblue" : (dsSecond.Tables[0].Rows[j]["Status"].ToString() == "I" && DelayedTask) ? "gtaskred" : "gtaskyellow";
                                    }
                                    DataSet Milsestones1 = dbgetdata.getTaskMileStones(new Guid(dsSecond.Tables[0].Rows[j]["TaskUID"].ToString()));
                                    if (Milsestones1.Tables[0].Rows.Count > 0)
                                    {
                                        //mParent = mParent + 1;
                                        //strScript.Append(@"g.AddTaskItemObject({pID: '" + mParent + "',pName: 'MileStones for (" + dsSecond.Tables[0].Rows[j]["Name"].ToString().Replace("'", "") + ")',pStart: '" + Convert.ToDateTime(Milsestones1.Tables[0].Rows[0]["MileStoneDate"].ToString()).ToString("yyyy-MM-dd") + "',pEnd: '" + Convert.ToDateTime(Milsestones1.Tables[0].Rows[0]["ProjectedDate"].ToString()).ToString("yyyy-MM-dd") + "',");
                                        //strScript.Append(@"pClass: 'gtaskblack',pLink: '',pMile: 0,pRes: '',pComp: 0,pGroup: 1,pParent: '" + dsSecond.Tables[0].Rows[j]["TaskUID"].ToString() + "',pOpen: 1,pDepend: '',pCaption: '',pCost: 0,pNotes: '" + Milsestones.Tables[0].Rows[0]["Description"].ToString().Replace("'", "") + " MileStones" + "'});");
                                        for (int mi = 0; mi < Milsestones1.Tables[0].Rows.Count; mi++)
                                        {
                                            strScript.Append(@"g.AddTaskItemObject({pID: '" + Milsestones1.Tables[0].Rows[mi]["MileStoneUID"].ToString() + "',pName: '" + Milsestones1.Tables[0].Rows[mi]["Description"].ToString().Replace("'", "") + "',pStart: '" + Convert.ToDateTime(Milsestones1.Tables[0].Rows[mi]["MileStoneDate"].ToString()).ToString("yyyy-MM-dd") + "',pEnd: '" + Convert.ToDateTime(Milsestones1.Tables[0].Rows[mi]["ProjectedDate"].ToString()).ToString("yyyy-MM-dd") + "',");
                                            strScript.Append(@"pClass: 'gtaskblack',pLink: '',pMile: 1,pRes: '',pComp: 0,pGroup: 0,pParent: '" + dsSecond.Tables[0].Rows[j]["TaskUID"].ToString() + "',pOpen: 1,pDepend: '',pCaption: '',pCost: 0,pNotes: '" + Milsestones1.Tables[0].Rows[mi]["Description"].ToString().Replace("'", "") + "'});");
                                        }

                                    }


                                    string dependencyvalues1 = string.Empty;
                                    DataSet dependency1 = dbgetdata.getDependencies(new Guid(dsSecond.Tables[0].Rows[j]["TaskUID"].ToString()));
                                    if (dependency1.Tables[0].Rows.Count > 0)
                                    {
                                        for (int dp = 0; dp < dependency1.Tables[0].Rows.Count; dp++)
                                        {
                                            dependencyvalues1 += dependency1.Tables[0].Rows[dp]["Dependent_TaskUID"].ToString() + dependency1.Tables[0].Rows[dp]["Dependency_Type"].ToString() + ",";
                                        }
                                        dependencyvalues1 = dependencyvalues1.TrimEnd(',');
                                    }

                                    tStatus = dsSecond.Tables[0].Rows[j]["Status"].ToString();

                                    strScript.Append(@"g.AddTaskItemObject({pID: '" + dsSecond.Tables[0].Rows[j]["TaskUID"].ToString() + "',pName: '" + dsSecond.Tables[0].Rows[j]["Name"].ToString().Replace("'", "") + "',pStart: '" + Convert.ToDateTime(dsSecond.Tables[0].Rows[j]["StartDate"].ToString()).ToString("yyyy-MM-dd") + "',pEnd: '" + Convert.ToDateTime(dsSecond.Tables[0].Rows[j]["PlannedEndDate"].ToString()).ToString("yyyy-MM-dd") + "',");
                                    strScript.Append(@"pClass: '" + pClass + "',pLink: '',pMile: 0,pRes: '',pComp: " + dsSecond.Tables[0].Rows[j]["StatusPer"].ToString() + ",pGroup:" + pgroup + ",pParent: '" + ds.Tables[0].Rows[i]["TaskUID"].ToString() + "',pOpen: 1,pDepend: '" + dependencyvalues1 + "',pCaption: '',pCost: 1000,pNotes: '" + tStatus + "'});");

                                    DataSet dsThird = dbgetdata.GetTask_by_ParentTaskUID(new Guid(dsSecond.Tables[0].Rows[j]["TaskUID"].ToString()));
                                    for (int k = 0; k < dsThird.Tables[0].Rows.Count; k++)
                                    {
                                        if (dsThird.Tables[0].Rows[k]["StartDate"].ToString() != "" && dsThird.Tables[0].Rows[k]["PlannedEndDate"].ToString() != "")
                                        {
                                            if (dsThird.Tables[0].Rows[k]["Status"].ToString() == "I")
                                            {
                                                DelayedTask = dbgetdata.Check_Task_is_Delayed(new Guid(dsThird.Tables[0].Rows[k]["TaskUID"].ToString())) > 0 ? true : false;
                                            }

                                            pgroup = dbgetdata.GetTaskCount_by_ParentTaskID(new Guid(dsThird.Tables[0].Rows[k]["TaskUID"].ToString())) > 0 ? 1 : 0;

                                            TaskCritical = dbgetdata.Is_Task_Critical(new Guid(dsThird.Tables[0].Rows[k]["TaskUID"].ToString()));
                                            if (TaskCritical > 0)
                                            {
                                                pClass = "gtaskpurple";
                                            }
                                            else
                                            {
                                                pClass = dsThird.Tables[0].Rows[k]["Status"].ToString() == "C" ? "gtaskgreen" : (dsThird.Tables[0].Rows[k]["Status"].ToString() == "I" && !DelayedTask) ? "gtaskblue" : (dsThird.Tables[0].Rows[k]["Status"].ToString() == "I" && DelayedTask) ? "gtaskred" : "gtaskyellow";
                                            }



                                            DataSet Milsestones2 = dbgetdata.getTaskMileStones(new Guid(dsThird.Tables[0].Rows[k]["TaskUID"].ToString()));
                                            if (Milsestones2.Tables[0].Rows.Count > 0)
                                            {
                                                for (int mi = 0; mi < Milsestones2.Tables[0].Rows.Count; mi++)
                                                {
                                                    strScript.Append(@"g.AddTaskItemObject({pID: '" + Milsestones2.Tables[0].Rows[mi]["MileStoneUID"].ToString() + "',pName: '" + Milsestones2.Tables[0].Rows[mi]["Description"].ToString().Replace("'", "") + "',pStart: '" + Convert.ToDateTime(Milsestones2.Tables[0].Rows[mi]["MileStoneDate"].ToString()).ToString("yyyy-MM-dd") + "',pEnd: '" + Convert.ToDateTime(Milsestones2.Tables[0].Rows[mi]["ProjectedDate"].ToString()).ToString("yyyy-MM-dd") + "',");
                                                    strScript.Append(@"pClass: 'gtaskblack',pLink: '',pMile: 1,pRes: '',pComp: 0,pGroup: 0,pParent: '" + dsThird.Tables[0].Rows[k]["TaskUID"].ToString() + "',pOpen: 1,pDepend: '',pCaption: '',pCost: 0,pNotes: '" + Milsestones2.Tables[0].Rows[mi]["Description"].ToString().Replace("'", "") + "'});");
                                                }

                                            }

                                            string dependencyvalues2 = string.Empty;
                                            DataSet dependency2 = dbgetdata.getDependencies(new Guid(dsThird.Tables[0].Rows[k]["TaskUID"].ToString()));
                                            if (dependency2.Tables[0].Rows.Count > 0)
                                            {
                                                for (int dp = 0; dp < dependency2.Tables[0].Rows.Count; dp++)
                                                {
                                                    dependencyvalues2 += dependency2.Tables[0].Rows[dp]["Dependent_TaskUID"].ToString() + dependency2.Tables[0].Rows[dp]["Dependency_Type"].ToString() + ",";
                                                }
                                                dependencyvalues2 = dependencyvalues2.TrimEnd(',');
                                            }

                                            tStatus = dsThird.Tables[0].Rows[k]["Status"].ToString();

                                            strScript.Append(@"g.AddTaskItemObject({pID: '" + dsThird.Tables[0].Rows[k]["TaskUID"].ToString() + "',pName: '" + dsThird.Tables[0].Rows[k]["Name"].ToString().Replace("'", "") + "',pStart: '" + Convert.ToDateTime(dsThird.Tables[0].Rows[k]["StartDate"].ToString()).ToString("yyyy-MM-dd") + "',pEnd: '" + Convert.ToDateTime(dsThird.Tables[0].Rows[k]["PlannedEndDate"].ToString()).ToString("yyyy-MM-dd") + "',");
                                            strScript.Append(@"pClass: '" + pClass + "',pLink: '',pMile: 0,pRes: '',pComp: " + dsThird.Tables[0].Rows[k]["StatusPer"].ToString() + ",pGroup: " + pgroup + ",pParent: '" + dsSecond.Tables[0].Rows[j]["TaskUID"].ToString() + "',pOpen: 1,pDepend: '" + dependencyvalues2 + "',pCaption: '',pCost: 1000,pNotes: ''});");
                                            DataSet dsFourth = dbgetdata.GetTask_by_ParentTaskUID(new Guid(dsThird.Tables[0].Rows[k]["TaskUID"].ToString()));
                                            for (int l = 0; l < dsFourth.Tables[0].Rows.Count; l++)
                                            {
                                                if (dsFourth.Tables[0].Rows[l]["StartDate"].ToString() != "" && dsFourth.Tables[0].Rows[l]["PlannedEndDate"].ToString() != "")
                                                {
                                                    if (dsFourth.Tables[0].Rows[l]["Status"].ToString() == "I")
                                                    {
                                                        DelayedTask = dbgetdata.Check_Task_is_Delayed(new Guid(dsFourth.Tables[0].Rows[l]["TaskUID"].ToString())) > 0 ? true : false;
                                                    }

                                                    pgroup = dbgetdata.GetTaskCount_by_ParentTaskID(new Guid(dsFourth.Tables[0].Rows[l]["TaskUID"].ToString())) > 0 ? 1 : 0;

                                                    TaskCritical = dbgetdata.Is_Task_Critical(new Guid(dsFourth.Tables[0].Rows[l]["TaskUID"].ToString()));
                                                    if (TaskCritical > 0)
                                                    {
                                                        pClass = "gtaskpurple";
                                                    }
                                                    else
                                                    {
                                                        pClass = dsFourth.Tables[0].Rows[l]["Status"].ToString() == "C" ? "gtaskgreen" : (dsFourth.Tables[0].Rows[l]["Status"].ToString() == "I" && !DelayedTask) ? "gtaskblue" : (dsFourth.Tables[0].Rows[l]["Status"].ToString() == "I" && DelayedTask) ? "gtaskred" : "gtaskyellow";
                                                    }
                                                    DataSet Milsestones3 = dbgetdata.getTaskMileStones(new Guid(dsFourth.Tables[0].Rows[l]["TaskUID"].ToString()));
                                                    if (Milsestones3.Tables[0].Rows.Count > 0)
                                                    {
                                                        for (int mi = 0; mi < Milsestones3.Tables[0].Rows.Count; mi++)
                                                        {
                                                            strScript.Append(@"g.AddTaskItemObject({pID: '" + Milsestones3.Tables[0].Rows[mi]["MileStoneUID"].ToString() + "',pName: '" + Milsestones3.Tables[0].Rows[mi]["Description"].ToString().Replace("'", "") + "',pStart: '" + Convert.ToDateTime(Milsestones3.Tables[0].Rows[mi]["MileStoneDate"].ToString()).ToString("yyyy-MM-dd") + "',pEnd: '" + Convert.ToDateTime(Milsestones3.Tables[0].Rows[mi]["ProjectedDate"].ToString()).ToString("yyyy-MM-dd") + "',");
                                                            strScript.Append(@"pClass: 'gtaskblack',pLink: '',pMile: 1,pRes: '',pComp: 0,pGroup: 0,pParent: '" + dsFourth.Tables[0].Rows[l]["TaskUID"].ToString() + "',pOpen: 1,pDepend: '',pCaption: '',pCost: 0,pNotes: '" + Milsestones3.Tables[0].Rows[mi]["Description"].ToString().Replace("'", "") + "'});");
                                                        }

                                                    }

                                                    string dependencyvalues3 = string.Empty;
                                                    DataSet dependency3 = dbgetdata.getDependencies(new Guid(dsFourth.Tables[0].Rows[l]["TaskUID"].ToString()));
                                                    if (dependency3.Tables[0].Rows.Count > 0)
                                                    {
                                                        for (int dp = 0; dp < dependency3.Tables[0].Rows.Count; dp++)
                                                        {
                                                            dependencyvalues3 += dependency3.Tables[0].Rows[dp]["Dependent_TaskUID"].ToString() + dependency3.Tables[0].Rows[dp]["Dependency_Type"].ToString() + ",";
                                                        }
                                                        dependencyvalues3 = dependencyvalues3.TrimEnd(',');
                                                    }

                                                    tStatus = dsFourth.Tables[0].Rows[l]["Status"].ToString();

                                                    strScript.Append(@"g.AddTaskItemObject({pID: '" + dsFourth.Tables[0].Rows[l]["TaskUID"].ToString() + "',pName: '" + dsFourth.Tables[0].Rows[l]["Name"].ToString().Replace("'", "") + "',pStart: '" + Convert.ToDateTime(dsFourth.Tables[0].Rows[l]["StartDate"].ToString()).ToString("yyyy-MM-dd") + "',pEnd: '" + Convert.ToDateTime(dsFourth.Tables[0].Rows[l]["PlannedEndDate"].ToString()).ToString("yyyy-MM-dd") + "',");
                                                    strScript.Append(@"pClass: '" + pClass + "',pLink: '',pMile: 0,pRes: '',pComp: " + dsFourth.Tables[0].Rows[l]["StatusPer"].ToString() + ",pGroup: " + pgroup + ",pParent: '" + dsThird.Tables[0].Rows[k]["TaskUID"].ToString() + "',pOpen: 1,pDepend: '" + dependencyvalues3 + "',pCaption: '',pCost: 1000,pNotes: '" + tStatus + "'});");

                                                    DataSet dsFifth = dbgetdata.GetTask_by_ParentTaskUID(new Guid(dsFourth.Tables[0].Rows[l]["TaskUID"].ToString()));
                                                    for (int m = 0; m < dsFifth.Tables[0].Rows.Count; m++)
                                                    {
                                                        if (dsFifth.Tables[0].Rows[m]["StartDate"].ToString() != "" && dsFifth.Tables[0].Rows[m]["PlannedEndDate"].ToString() != "")
                                                        {
                                                            if (dsFifth.Tables[0].Rows[m]["Status"].ToString() == "I")
                                                            {
                                                                DelayedTask = dbgetdata.Check_Task_is_Delayed(new Guid(dsFifth.Tables[0].Rows[m]["TaskUID"].ToString())) > 0 ? true : false;
                                                            }

                                                            pgroup = dbgetdata.GetTaskCount_by_ParentTaskID(new Guid(dsFifth.Tables[0].Rows[m]["TaskUID"].ToString())) > 0 ? 1 : 0;
                                                            TaskCritical = dbgetdata.Is_Task_Critical(new Guid(dsFifth.Tables[0].Rows[m]["TaskUID"].ToString()));
                                                            if (TaskCritical > 0)
                                                            {
                                                                pClass = "gtaskpurple";
                                                            }
                                                            else
                                                            {
                                                                pClass = dsFifth.Tables[0].Rows[m]["Status"].ToString() == "C" ? "gtaskgreen" : (dsFifth.Tables[0].Rows[m]["Status"].ToString() == "I" && !DelayedTask) ? "gtaskblue" : (dsFifth.Tables[0].Rows[m]["Status"].ToString() == "I" && DelayedTask) ? "gtaskred" : "gtaskyellow";
                                                            }


                                                            DataSet Milsestones4 = dbgetdata.getTaskMileStones(new Guid(dsFifth.Tables[0].Rows[m]["TaskUID"].ToString()));
                                                            if (Milsestones4.Tables[0].Rows.Count > 0)
                                                            {
                                                                for (int mi = 0; mi < Milsestones4.Tables[0].Rows.Count; mi++)
                                                                {
                                                                    strScript.Append(@"g.AddTaskItemObject({pID: '" + Milsestones4.Tables[0].Rows[mi]["MileStoneUID"].ToString() + "',pName: '" + Milsestones4.Tables[0].Rows[mi]["Description"].ToString().Replace("'", "") + "',pStart: '" + Convert.ToDateTime(Milsestones4.Tables[0].Rows[mi]["MileStoneDate"].ToString()).ToString("yyyy-MM-dd") + "',pEnd: '" + Convert.ToDateTime(Milsestones4.Tables[0].Rows[mi]["ProjectedDate"].ToString()).ToString("yyyy-MM-dd") + "',");
                                                                    strScript.Append(@"pClass: 'gtaskblack',pLink: '',pMile: 1,pRes: '',pComp: 0,pGroup: 0,pParent: '" + dsFifth.Tables[0].Rows[m]["TaskUID"].ToString() + "',pOpen: 1,pDepend: '',pCaption: '',pCost: 0,pNotes: '" + Milsestones4.Tables[0].Rows[mi]["Description"].ToString().Replace("'", "") + "'});");
                                                                }

                                                            }

                                                            string dependencyvalues4 = string.Empty;
                                                            DataSet dependency4 = dbgetdata.getDependencies(new Guid(dsFifth.Tables[0].Rows[m]["TaskUID"].ToString()));
                                                            if (dependency4.Tables[0].Rows.Count > 0)
                                                            {
                                                                for (int dp = 0; dp < dependency4.Tables[0].Rows.Count; dp++)
                                                                {
                                                                    dependencyvalues4 += dependency4.Tables[0].Rows[dp]["Dependent_TaskUID"].ToString() + dependency4.Tables[0].Rows[dp]["Dependency_Type"].ToString() + ",";
                                                                }
                                                                dependencyvalues4 = dependencyvalues4.TrimEnd(',');
                                                            }

                                                            tStatus = dsFifth.Tables[0].Rows[m]["Status"].ToString();

                                                            strScript.Append(@"g.AddTaskItemObject({pID: '" + dsFifth.Tables[0].Rows[m]["TaskUID"].ToString() + "',pName: '" + dsFifth.Tables[0].Rows[m]["Name"].ToString().Replace("'", "") + "',pStart: '" + Convert.ToDateTime(dsFifth.Tables[0].Rows[m]["StartDate"].ToString()).ToString("yyyy-MM-dd") + "',pEnd: '" + Convert.ToDateTime(dsFifth.Tables[0].Rows[m]["PlannedEndDate"].ToString()).ToString("yyyy-MM-dd") + "',");
                                                            strScript.Append(@"pClass: '" + pClass + "',pLink: '',pMile: 0,pRes: '',pComp:" + dsFifth.Tables[0].Rows[m]["StatusPer"].ToString() + ",pGroup: " + pgroup + ",pParent: '" + dsFourth.Tables[0].Rows[l]["TaskUID"].ToString() + "',pOpen: 1,pDepend: '" + dependencyvalues4 + "',pCaption: '',pCost: 1000,pNotes: '" + tStatus + "'});");
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    strScript.Append(@"g.Draw();
                    </script>");
                    LtGantt.Text = strScript.ToString();
                }
                else
                {
                    LtGantt.Text = "No Data Found.";
                }
            }
            else
            {
                LtGantt.Text = "No Data Found.";
            }
        }
    }
}