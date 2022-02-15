using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProjectManager.DAL;

namespace ProjectManagementTool._content_pages.physical_progress_chart
{
    public partial class Default : System.Web.UI.Page
    {
        TaskUpdate TKUpdate = new TaskUpdate();
        DBGetData getdata = new DBGetData();
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
                //
                LoadGraph();
            }
            catch (Exception ex)
            {
            }
        }

        protected void ddlProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlProject.SelectedValue != "")
            {
               
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

                    //Loadtasks(ddlworkpackage.SelectedValue);
                    //LoadSTask(ddlTask.SelectedItem.Value);
                    LoadGraph();

                }
            }
        }

        //private void LoadGraph()
        //{
        //    try
        //    {
        //        ltScript_Progress.Text = string.Empty;
        //        int test = 165;
        //        StringBuilder strScript = new StringBuilder();
        //        strScript.Append(@" <script type='text/javascript'>

        //        google.charts.load('current', { packages: ['corechart', 'bar'] });
        //        google.charts.setOnLoadCallback(drawBasic);

        //        function drawBasic() {

        //        var data = google.visualization.arrayToDataTable([
        //  ['Month', 'Bolivia', 'Ecuador', 'Madagascar', 'Papua New Guinea', 'Rwanda', 'Average'],");

        //        strScript.Append("['2004/05'," + test + ",938,522,998,450,614.6],['2005/06',135,1120,599,1268,288,682],['2006/07',157,1167,587,807,397,623],['2007/08',139,1110,615,968,215,609.4],['2008/09',136,691,629,1026,366,569.6]]); ");

        //strScript.Append(@"var options = {
        //  title : 'Monthly Coffee Production by Country',
        //  vAxis: {title: 'Cups'},
        //  hAxis: {title: 'Month'},
        //  seriesType: 'bars',
        //  series: {4: {type: 'line'},5: {type: 'line'}}
        //};
        //        var chart = new google.visualization.ColumnChart(
        //          document.getElementById('chart_div'));
        //         chart.draw(data, options);

        //    }</script>");
        //        //ltScript_Cost.Text = strScript.ToString();
        //        ltScript_Progress.Text = strScript.ToString();

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        private void LoadGraph()
        {
            try
            {
                ltScript_Progress.Text = string.Empty;
                int test = 165;
                DataSet ds = getdata.GetTaskScheduleDatesforGraph(new Guid(ddlworkpackage.SelectedValue));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    StringBuilder strScript = new StringBuilder();
                    string tablemonths ="<td>&nbsp;</td>";
                    string tmonthlyplan = "<td>Monthly Plan</td>";
                    string tmonthlyactual = "<td>Monthly Actual</td>";
                    string tcumulativeplan = "<td>Cumulative Plan</td>";
                    string tcumulativeactual = "<td>Cumulative Actual</td>";
                    strScript.Append(@" <script type='text/javascript'>
                
                google.charts.load('current', { packages: ['corechart', 'bar'] });
                google.charts.setOnLoadCallback(drawBasic);

                function drawBasic() {

                var data = google.visualization.arrayToDataTable([
          ['Month', 'Monthly Plan', 'Monthly Actual', 'Cumulative Plan', 'Cumulative Actual'],");
                    int count = 1;
                    DataSet dsvalues = new DataSet();
                    decimal planvalue = 0;
                    decimal actualvalue = 0;
                    decimal cumplanvalue = 0;
                    decimal cumactualvalue = 0;
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        //get the actual and planned values....
                        dsvalues.Clear();
                        dsvalues = getdata.GetTaskScheduleValuesForGraph(new Guid(ddlworkpackage.SelectedValue), Convert.ToDateTime(dr["StartDate"].ToString()), Convert.ToDateTime(dr["StartDate"].ToString()).AddMonths(1));
                        if(dsvalues.Tables[0].Rows.Count > 0)
                        {
                            planvalue =decimal.Parse(dsvalues.Tables[0].Rows[0]["TotalSchValue"].ToString());
                            actualvalue = decimal.Parse(dsvalues.Tables[0].Rows[0]["TotalAchValue"].ToString());
                            cumplanvalue += planvalue;
                            cumactualvalue += actualvalue;
                        }
                        if (count < ds.Tables[0].Rows.Count)
                        {

                            strScript.Append("['" + Convert.ToDateTime(dr["StartDate"].ToString()).ToString("MMMyy") + "'," + planvalue + "," + actualvalue + "," + cumplanvalue + "," + cumactualvalue + "],");
                        }
                        else
                        {
                            strScript.Append("['"  + Convert.ToDateTime(dr["StartDate"].ToString()).ToString("MMMyy") + "'," + planvalue + "," + actualvalue + "," + cumplanvalue + "," + cumactualvalue + "]]);");
                        }
                        //
                        tablemonths += "<td>" + Convert.ToDateTime(dr["StartDate"].ToString()).ToString("MMM-yy") + "</td>";
                        tmonthlyplan += "<td>" + planvalue + "</td>";
                        tmonthlyactual += "<td>" + actualvalue + "</td>";

                        tcumulativeplan += "<td>" + cumplanvalue + "</td>";
                        tcumulativeactual += "<td>" + cumactualvalue + "</td>";


                        //
                        count++;
                    }
                   
                    strScript.Append(@"var options = {
          title : 'Plan vs Achieved Progress Curve',
          
          hAxis: {title: 'MONTH'},
          seriesType: 'bars',
          series: {2: {type: 'line',targetAxisIndex: 1},3: {type: 'line',targetAxisIndex: 1}},
vAxes: {
            // Adds titles to each axis.
          
            0: {title: 'Monthly Plan (%)'},
            1: {title: 'Cummulative Plan (%)'}
          }
        };
                var chart = new google.visualization.ComboChart(
                  document.getElementById('chart_div'));
                 chart.draw(data, options);
                
            }</script>");
                    //ltScript_Cost.Text = strScript.ToString();
                    ltScript_Progress.Text = strScript.ToString();
                    divtable.InnerHtml = "<table border=\"1\" style=\"text-align:center; padding-left:100px\">" + 
                                      "<tr> " + tablemonths + "</tr>" +
                                       "<tr> " + tmonthlyplan + "</tr>" +
                                        "<tr> " + tmonthlyactual + "</tr>" +
                                         "<tr> " + tcumulativeplan + "</tr>" +
                                          "<tr> " + tcumulativeactual + "</tr>" +

                                              "</table>";
                }
                else
                {
                    ltScript_Progress.Text = "<h3>No data</h3>";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void ddlworkpackage_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlworkpackage.SelectedValue != "")
            {
                LoadGraph();
            }
        }
    }
}