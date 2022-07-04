using ProjectManagementTool.DAL;
using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManager._content_pages
{
    public partial class dashboard : System.Web.UI.Page
    {
        DBGetData getdt = new DBGetData();
        TaskUpdate gettk = new TaskUpdate();
        Invoice invoice = new Invoice();
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
                    
                    Session["ActivityUID"] = null;
                    BindProject();
                    SelectedProjectWorkpackage("Project");
                    DDlProject_SelectedIndexChanged(sender, e);
                    
                    // added on 17/11/2020
                    DataSet dscheck = new DataSet();
                    dscheck = getdt.GetUsertypeFunctionality_Mapping(Session["TypeOfUser"].ToString());
                    // RdList.Items[1].Attributes.CssStyle.Add("display", "none");
                    //   rdSelect.Items[1].Attributes.CssStyle.Add("display", "none");
                    // rdSelect.Items[2].Attributes.CssStyle.Add("display", "none");
                   // RdList.Items[1].Enabled = false;
                    rdSelect.Items[1].Enabled = false;
                    rdSelect.Items[2].Enabled = false;
                     divCamera.Visible = false;
                    if (dscheck.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dscheck.Tables[0].Rows)
                        {

                            if (dr["Code"].ToString() == "FS" || dr["Code"].ToString() == "FT" || dr["Code"].ToString() == "FZ") // VIEW FINANCIAL PROGRESS OF PROJECT-INDIVIDUAL REGIONS // ALL // INDIVIDUAL PROJECT //
                            {
                                // RdList.Items[1].Attributes.CssStyle.Add("display", "block");
                                //  rdSelect.Items[2].Attributes.CssStyle.Add("display", "block");
                              //  RdList.Items[1].Enabled = true;
                                rdSelect.Items[2].Enabled = true;

                            }
                            if (dr["Code"].ToString() == "FX" || Session["TypeOfUser"].ToString() == "U") //Project progress tracking
                            {

                                // rdSelect.Items[1].Attributes.CssStyle.Add("display", "block");
                                rdSelect.Items[1].Enabled = true;
                            }
                            if (Session["TypeOfUser"].ToString() != "U")
                            {
                                if (dr["Code"].ToString() == "DC") //Project progress tracking
                                {

                                    divCamera.Visible = true;
                                }
                               
                            }
                            else
                            {
                                divCamera.Visible = true;
                            }

                        }
                    }

                 
                }
            }
        }

        private void DbSyncStatusCount(string WorkpackageUID)
        {
            if (WebConfigurationManager.AppSettings["Dbsync"] == "Yes")
            {
                DataSet ds = getdt.GetDbsync_Status_Count_by_WorkPackageUID(new Guid(WorkpackageUID));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DivSyncedData.Visible = true;
                    LblLastSyncedDate.Text = ds.Tables[0].Rows[0]["CreatedDate"].ToString() != "" ? Convert.ToDateTime(ds.Tables[0].Rows[0]["CreatedDate"].ToString()).ToString("dd MMM yyyy hh:mm tt") : "NA";
                    // LblTotalSourceDocuments.Text = ds.Tables[0].Rows[0]["SourceDocCount"].ToString() != "" ? ds.Tables[0].Rows[0]["DestDocCount"].ToString() : "NA";
                    // LblTotalDestinationDocuments.Text = ds.Tables[0].Rows[0]["DestDocCount"].ToString() != "" ? ds.Tables[0].Rows[0]["SourceDocCount"].ToString() : "NA";

                    LblSourceHeading.Text = DDlProject.SelectedItem.Text + "(" + DDLWorkPackage.SelectedItem.Text + ")";// +" :- "+ WebConfigurationManager.AppSettings["SourceSite"];
                   // LblDestinationHeading.Text = WebConfigurationManager.AppSettings["DestinationSite"];
                    divsyncdetails.Visible = true;
                    //if (WebConfigurationManager.AppSettings["Domain"] == "LNT")
                    //{
                    //    lblONTBTo_No.Text = ds.Tables[0].Rows[0]["DestDocCount"].ToString() != "" ? ds.Tables[0].Rows[0]["DestDocCount"].ToString() : "0";
                    //}
                    //else
                    //{
                    //    lblONTBTo_No.Text = ds.Tables[0].Rows[0]["SourceDocCount"].ToString() != "" ? ds.Tables[0].Rows[0]["SourceDocCount"].ToString() : "0";

                    //}
                    lblReconDocsNo.Text = getdt.GetDashboardReconciliationDocs(new Guid(DDlProject.SelectedValue)).ToString();
                    lblContractorToNo.Text = getdt.GetDashboardContractotDocsSubmitted(new Guid(DDlProject.SelectedValue)).ToString();
                    lblONTBTo_No.Text = getdt.GetDashboardONTBtoContractorDocs(new Guid(DDlProject.SelectedValue)).ToString();
                    //lblONTBTo_No.Text = (int.Parse(lblONTBTo_No.Text) - int.Parse(lblContractorToNo.Text)) > 0  ? (int.Parse(lblONTBTo_No.Text) - int.Parse(lblContractorToNo.Text)).ToString() : "0";
                    lblRABills.Text = getdt.GetInvoiceDetails_by_WorkpackageUID(new Guid(WorkpackageUID)).Rows.Count.ToString();
                    lblInvoices.Text = invoice.GetInvoiceMaster_by_WorkpackageUID(new Guid(WorkpackageUID)).Tables[0].Rows.Count.ToString();
                    lblBankG.Text = getdt.GetBankGuarantee_by_Bank_WorkPackageUID(new Guid(WorkpackageUID)).Tables[0].Rows.Count.ToString();
                    lblInsurance.Text = getdt.GetInsuranceSelect_by_WorkPackageUID(new Guid(WorkpackageUID)).Tables[0].Rows.Count.ToString();
                    lblMeasurements.Text = getdt.GetTaskMeasurementBookForDashboard(new Guid(WorkpackageUID)).Tables[0].Rows.Count.ToString();

                    // make hyper links
                    hlRABills.HRef = "~/_content_pages/rabill-summary/?&PrjUID=" + DDlProject.SelectedValue;
                    hlInvoices.HRef = "~/_content_pages/invoice/?&PrjUID=" + DDlProject.SelectedValue;
                    hlBankGuarantee.HRef = "~/_content_pages/bank-guarantee/?&PrjUID=" + DDlProject.SelectedValue;
                    hlInsurance.HRef = "~/_content_pages/insurance/?&PrjUID=" + DDlProject.SelectedValue;
                    hlContractor.HRef = "~/_content_pages/documents-contractor/?&type=Contractor&PrjUID=" + DDlProject.SelectedValue;
                    hlReconciliationdocs.HRef = "~/_content_pages/documents-contractor/?&type=Recon&PrjUID=" + DDlProject.SelectedValue;
                    hlONTB.HRef = "~/_content_pages/documents-contractor/?&type=Ontb&PrjUID=" + DDlProject.SelectedValue;
                    hlMeasurement.HRef = "~/_content_pages/dashboard-measurment/?&WorkPackageUID=" + DDLWorkPackage.SelectedValue;

                    UploadSitePhotograph.HRef = "/_modal_pages/upload-sitephotograph.aspx?PrjUID=" + DDlProject.SelectedValue + "&WorkPackage=" + DDLWorkPackage.SelectedValue;
                    ViewSitePhotograph.HRef = "/_modal_pages/view-sitephotographs.aspx?PrjUID=" + DDlProject.SelectedValue + "&WorkPackage=" + DDLWorkPackage.SelectedValue;

                }
                else
                {
                    DivSyncedData.Visible = false;
                    divsyncdetails.Visible = false;
                    //LblLastSyncedDate.Text = "NA";
                    //LblTotalSourceDocuments.Text = "NA";
                    //LblTotalDestinationDocuments.Text = "NA";
                }
                
            }
            else
            {
                DivSyncedData.Visible = false;
            }
        }

        private void getDashboardImages(string WorkpackageUID)
        {
            if (DDLWorkPackage.SelectedValue != "--Select--")
            {
                string ImageName = getdt.GetDashboardImages(new Guid(WorkpackageUID));
                if (!string.IsNullOrEmpty(ImageName))
                {
                    divdashboardimage.Attributes.Add("style", "background-image:url('/_assets/images/" + ImageName + "');width:100% !important; height:100% !important;background-size:100% 100%;background-position: center center;margin: 0px 0px 0px 0px; opacity: 0.9 !important");
                }
                else
                {
                    divdashboardimage.Attributes.Remove("style");
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
            DDlProject.Items.Insert(0, "--Select--");
            DDLWorkPackage.Items.Insert(0, "--Select--");

        }

        protected void DDlProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDlProject.SelectedValue != "--Select--")
            {
                divdashboardimage.Visible = true;
                dummyNJSEIdashboard.Visible = false;
                divdummydashboard.Visible = false;
                dummyONTBdashboard.Visible = false;
                DataSet ds = new DataSet();
                //ds = getdt.GetWorkPackages_By_ProjectUID(new Guid(DDlProject.SelectedValue));
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
                //ds = getdt.GetWorkPackages_By_ProjectUID(new Guid(DDlProject.SelectedValue));
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DDLWorkPackage.DataTextField = "Name";
                        DDLWorkPackage.DataValueField = "WorkPackageUID";
                        DDLWorkPackage.DataSource = ds;
                        DDLWorkPackage.DataBind();
                    SelectedProjectWorkpackage("Workpackage");
                    //  DDLWorkPackage.Items.Insert(0, "--Select--");
                    BindResourceMaster();
                        Bind_DocumentsChart();
                        BindAlerts("WorkPackage");
                        BindActivityPie_Chart("Work Package", DDLWorkPackage.SelectedValue);
                        Bind_ResourceChart("Work Package", DDLWorkPackage.SelectedValue);
                    // Bind_ProgressChart("Work Package", DDLWorkPackage.SelectedValue);
                    Bind_CostChart("Work Package", DDLWorkPackage.SelectedValue);
                    if (rdSelect.SelectedValue == "1")
                    {
                        LoadGraph(); //Physical progress chart
                    }
                    else if (rdSelect.SelectedValue == "2")
                    {
                        LoadFinancialGraph();
                    }
                        BindCamera(DDLWorkPackage.SelectedValue);
                        heading.InnerHtml = "Physical Progress Chart - " + DDlProject.SelectedItem.Text + " (" + DDLWorkPackage.SelectedItem.Text + ")";
                        headingF.InnerHtml = "Financial Progress Chart - " + DDlProject.SelectedItem.Text + " (" + DDLWorkPackage.SelectedItem.Text + ")";

                    DbSyncStatusCount(DDLWorkPackage.SelectedValue);
                    getDashboardImages(DDLWorkPackage.SelectedValue);

                    Session["Project_Workpackage"] = DDlProject.SelectedValue + "_" + DDLWorkPackage.SelectedValue;
                    // added on 10/01/2022 for docs to act on for the user

                    if (Session["TypeOfUser"].ToString() != "U" && Session["TypeOfUser"].ToString() != "VP" && Session["TypeOfUser"].ToString() != "MD")
                    {
                        divUsersdocs.Visible = true;
                        //if (getUserDocsNo() == 0)
                        //{
                        //    Hluserdocs.HRef = "#";
                        //    Hluserdocs.InnerText = "no documents";
                        //}
                        //else
                        //{
                        //    Hluserdocs.InnerText = getUserDocsNo() + " documents";
                        //    Hluserdocs.HRef = "~/_content_pages/documents-contractor/?&type=Ontb&PrjUID=" + DDlProject.SelectedValue + "&UserUID=" + Session["UserUID"].ToString() + "&WkpgUID=" + DDLWorkPackage.SelectedValue;
                        //}
                    }
                    else
                    {
                        divUsersdocs.Visible = false;
                    }
                }
                    else
                    {
                        DDLWorkPackage.DataSource = null;
                        DDLWorkPackage.DataBind();
                        ltScripts_piechart.Text = "<h4>No data</h4>";
                        ltScript_Progress.Text = "<h4>No data</h4>";
                        ltScript_Document.Text = "<h4>No data</h4>";
                        ltScript_Resource.Text = "<h4>No data</h4>";
                        ltScript_PhysicalProgress.Text= "<h4>No data</h4>";
                        ltScript_FinProgress.Text = "<h4>No data</h4>";
                        divtable.InnerHtml = "";
                        btnPrint.Visible = false;
                }
                //}
            }
            else
            {
                ltScripts_piechart.Text = "<h4>No data</h4>";
                ltScript_Progress.Text = "<h4>No data</h4>";
                ltScript_Document.Text = "<h4>No data</h4>";
                ltScript_Resource.Text = "<h4>No data</h4>";
                ltScript_PhysicalProgress.Text = "<h4>No data</h4>";
                ltScript_FinProgress.Text = "<h4>No data</h4>";
                divtable.InnerHtml = ""; 
                btnPrint.Visible = false;
                DDLWorkPackage.Items.Clear();
                DDLWorkPackage.Items.Insert(0, "--Select--");
                divdashboardimage.Visible = false;
                divUsersdocs.Visible = false;
                if (WebConfigurationManager.AppSettings["Domain"] == "NJSEI")
                {
                    divdummydashboard.Visible = false;
                    dummyNJSEIdashboard.Visible = true;
                    divNJSEIMIS.Visible = false;
                    dummyONTBdashboard.Visible = false;
                }
                else
                {
                    divdummydashboard.Visible = false;
                    dummyONTBdashboard.Visible = true;
                    dummyNJSEIdashboard.Visible = false;
                    divNJSEIMIS.Visible = false;
                   // rdSelect.Items[3].Text = "";
                   // rdSelect.Items[3].Enabled = false;
                }
            }
        }

        protected void DDLWorkPackage_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDLWorkPackage.SelectedValue != "--Select--")
            {
                getDashboardImages(DDLWorkPackage.SelectedValue);
                BindResourceMaster();
                Bind_DocumentsChart();
                BindAlerts("WorkPackage");
                BindActivityPie_Chart("Work Package", DDLWorkPackage.SelectedValue);
                Bind_ResourceChart("Work Package", DDLWorkPackage.SelectedValue);
                //Bind_ProgressChart("Work Package", DDLWorkPackage.SelectedValue);
                Bind_CostChart("Work Package", DDLWorkPackage.SelectedValue);
                BindCamera(DDLWorkPackage.SelectedValue);
                heading.InnerHtml = "Physical Progress Chart - " + DDlProject.SelectedItem.Text + " (" + DDLWorkPackage.SelectedItem.Text + ")";
                headingF.InnerHtml = "Financial Progress Chart - " + DDlProject.SelectedItem.Text + " (" + DDLWorkPackage.SelectedItem.Text + ")";
                if (rdSelect.SelectedValue == "1")
                {
                    LoadGraph(); //Physical progress chart
                }
                else if (rdSelect.SelectedValue == "2")
                {
                    LoadFinancialGraph();
                }
                Session["Project_Workpackage"] = DDlProject.SelectedValue + "_" + DDLWorkPackage.SelectedValue;
                //----------------------
                if (Session["TypeOfUser"].ToString() != "U" && Session["TypeOfUser"].ToString() != "VP" && Session["TypeOfUser"].ToString() != "MD")
                {
                    divUsersdocs.Visible = true;
                    //if (getUserDocsNo() == 0)
                    //{
                    //    Hluserdocs.HRef = "#";
                    //    Hluserdocs.InnerText = "no documents";
                    //}
                    //else
                    //{
                    //    Hluserdocs.InnerText = getUserDocsNo() + " documents";
                    //    Hluserdocs.HRef = "~/_content_pages/documents-contractor/?&type=Ontb&PrjUID=" + DDlProject.SelectedValue + "&UserUID=" + Session["UserUID"].ToString() + "&WkpgUID=" + DDLWorkPackage.SelectedValue;
                    //}
                }
                else
                {
                    divUsersdocs.Visible = false;
                }
                //-----------------------
            }
            
        }

        private void SelectedProjectWorkpackage(string pType)
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

        private void BindResourceMaster()
        {
            if (DDLWorkPackage.SelectedValue != "--Select--")
            {
                DataSet ds = getdt.getResourceMaster(new Guid(DDLWorkPackage.SelectedValue));
                DDlResource.DataTextField = "ResourceName";
                DDlResource.DataValueField = "ResourceUID";
                DDlResource.DataSource = ds;
                DDlResource.DataBind();
            }
        }

        private void BindCamera(string WorkpackageUID)
        {
            if (DDLWorkPackage.SelectedValue != "--Select--")
            {
                DataSet ds = getdt.Camera_Selectby_WorkpackageUID(new Guid(WorkpackageUID));
                DDLCamera.DataTextField = "Camera_Name";
                DDLCamera.DataValueField = "Camera_UID";
                DDLCamera.DataSource = ds;
                DDLCamera.DataBind();
            }
        }
        private void Bind_DocumentsChart()
        {
            if (DDLWorkPackage.SelectedValue != "--Select--")
            {
                DataSet ds = getdt.getDocumentCount_by_ProjectUID_WorkPackageUID(new Guid(DDlProject.SelectedValue), new Guid(DDLWorkPackage.SelectedValue));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    StringBuilder strScript = new StringBuilder();
                    strScript.Append(@"<script type='text/javascript'>
                google.charts.load('current', { packages: ['corechart', 'bar'] });
                google.charts.setOnLoadCallback(drawBasic);

                function drawBasic() {
                    var data = google.visualization.arrayToDataTable([
                      ['Document', 'Ontime','Delayed', { role: 'annotation' }],");
                    strScript.Append("['Tot. Documents', " + ds.Tables[0].Rows[0]["DocCount"].ToString() + ", 0,'" + ds.Tables[0].Rows[0]["DocCount"].ToString() + "'],");
                    strScript.Append("['Submitted', " + (Convert.ToInt32(ds.Tables[0].Rows[0]["Status1"].ToString()) - Convert.ToInt32(ds.Tables[0].Rows[0]["Status1Delay"].ToString())) + ", " + ds.Tables[0].Rows[0]["Status1Delay"].ToString() + ",'" + ds.Tables[0].Rows[0]["Status1"].ToString() + "'],");
                    strScript.Append("['Code A', " + (Convert.ToInt32(ds.Tables[0].Rows[0]["Status3"].ToString()) - Convert.ToInt32(ds.Tables[0].Rows[0]["Status3Delay"].ToString())) + ", " + ds.Tables[0].Rows[0]["Status3Delay"].ToString() + ",'" + ds.Tables[0].Rows[0]["Status3"].ToString() + "'],");
                    strScript.Append("['Code B', " + (Convert.ToInt32(ds.Tables[0].Rows[0]["Status2"].ToString()) - Convert.ToInt32(ds.Tables[0].Rows[0]["Status2Delay"].ToString())) + ", " + ds.Tables[0].Rows[0]["Status2Delay"].ToString() + ",'" + ds.Tables[0].Rows[0]["Status2"].ToString() + "'],");
                    strScript.Append("['Code C', " + (Convert.ToInt32(ds.Tables[0].Rows[0]["CodeC"].ToString()) - Convert.ToInt32(ds.Tables[0].Rows[0]["CodeCDelay"].ToString())) + ", " + ds.Tables[0].Rows[0]["CodeCDelay"].ToString() + ",'" + ds.Tables[0].Rows[0]["CodeC"].ToString() + "'],");
                    strScript.Append("['Code D', " + (Convert.ToInt32(ds.Tables[0].Rows[0]["CodeD"].ToString()) - Convert.ToInt32(ds.Tables[0].Rows[0]["CodeDDelay"].ToString())) + ", " + ds.Tables[0].Rows[0]["CodeDDelay"].ToString() + ",'" + ds.Tables[0].Rows[0]["CodeD"].ToString() + "'],");
                    strScript.Append("['Code E', " + (Convert.ToInt32(ds.Tables[0].Rows[0]["CodeE"].ToString()) - Convert.ToInt32(ds.Tables[0].Rows[0]["CodeEDelay"].ToString())) + ", " + ds.Tables[0].Rows[0]["CodeEDelay"].ToString() + ",'" + ds.Tables[0].Rows[0]["CodeE"].ToString() + "'],");
                    strScript.Append("['Code F', " + (Convert.ToInt32(ds.Tables[0].Rows[0]["CodeF"].ToString()) - Convert.ToInt32(ds.Tables[0].Rows[0]["CodeFDelay"].ToString())) + ", " + ds.Tables[0].Rows[0]["CodeFDelay"].ToString() + ",'" + ds.Tables[0].Rows[0]["CodeF"].ToString() + "'],");
                    strScript.Append("['Code G', " + (Convert.ToInt32(ds.Tables[0].Rows[0]["CodeG"].ToString()) - Convert.ToInt32(ds.Tables[0].Rows[0]["CodeGDelay"].ToString())) + ", " + ds.Tables[0].Rows[0]["CodeGDelay"].ToString() + ",'" + ds.Tables[0].Rows[0]["CodeG"].ToString() + "'],");
                    strScript.Append("['Code H', " + (Convert.ToInt32(ds.Tables[0].Rows[0]["CodeH"].ToString()) - Convert.ToInt32(ds.Tables[0].Rows[0]["CodeHDelay"].ToString())) + ", " + ds.Tables[0].Rows[0]["CodeHDelay"].ToString() + ",'" + ds.Tables[0].Rows[0]["CodeH"].ToString() + "'],");
                    strScript.Append("['Client Approved', " + (Convert.ToInt32(ds.Tables[0].Rows[0]["Status4"].ToString()) - Convert.ToInt32(ds.Tables[0].Rows[0]["Status4Delay"].ToString())) + "," + ds.Tables[0].Rows[0]["Status4Delay"].ToString() + ",'" + ds.Tables[0].Rows[0]["Status4"].ToString() + "'],");
                    strScript.Remove(strScript.Length - 1, 1);
                    strScript.Append("]);");
                    strScript.Append(@"var options = {
                        is3D: true,
                        legend: { position: 'none' },
                        fontSize: 13,
                        isStacked: true,
                        chartArea: {
                            left: '25%',
                            top: '5%',
                            height: '88%',
                            width: '61%'
                        },
                        bars: 'horizontal',
                        annotations: {
                        alwaysOutside:true,
                        },
                        axes: {
                            x: {
                                0: { side: 'top', label: 'Percentage' } // Top x-axis.
                            }
                        },
                        hAxis: {
                            minValue: 0
                        }
                    };
                    function selectHandler()
                    {
                        var selection = chart.getSelection();
                        if (selection.length > 0)
                        {
                            var colLabel = data.getColumnLabel(selection[0].column);
                            var mydata = data.getValue(selection[0].row,0);
                            ");
                    strScript.Append("window.open('/_content_pages/document-drilldown/default.aspx?DocumentType=' + (colLabel + '_' + mydata) + '&ProjectUID=" + DDlProject.SelectedValue + "&WorkPackageUID=" + DDLWorkPackage.SelectedValue + "', '_self', true);");
                    //alert('The user selected ' + topping);
                    strScript.Append(@"}
                    }
                    
                    var chart = new google.visualization.BarChart(document.getElementById('DocChart_Div'));
                    google.visualization.events.addListener(chart, 'select', selectHandler);
                    chart.draw(data, options);
                }
            </script>");
                    ltScript_Document.Text = strScript.ToString();
                }
                else
                {
                    ltScript_Document.Text = "<h4>No data</h4>";
                }
            }
        }

        private void BindAlerts(string By)
        {
            if (DDLWorkPackage.SelectedValue != "--Select--")
            {
                DataSet ds = new DataSet();
                if (By == "Project")
                {
                    ds = getdt.getAlerts_by_ProjectUID(new Guid(DDlProject.SelectedValue));
                }
                else if (By == "WorkPackage")
                {
                    ds = getdt.getAlerts_by_WorkPackageUID(new Guid(DDlProject.SelectedValue), new Guid(DDLWorkPackage.SelectedValue));
                }
                //else
                //{
                //    ds = getdt.getAlerts_by_TaskUID(new Guid(DDLTask.SelectedValue));
                //}
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string s1;
                    s1 = "<table class='table table-borderless'>";

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        //s1 += "<tr><td style='width:80%; color:#006699; font-size:larger;'>" + ds.Tables[0].Rows[i]["Alert_Text"].ToString() + "</td>" + "<td style='width:20%; text-align:right;'>" + Convert.ToDateTime(ds.Tables[0].Rows[i]["Alert_Date"].ToString()).ToString("dd/MM/yyyy") + "</td></tr>";
                        s1 += "<tr style='border-bottom:1px dotted Gray; margin-left:0px;'><td>" + ds.Tables[0].Rows[i]["Alert_Text"].ToString() + "</td></tr>";
                    }
                    s1 += "</table>";
                    lt1.Text = s1.ToString();
                }
                else
                {
                    lt1.Text = "<h4>No Alerts Found</h4>";
                }
            }
        }

        private void BindActivityPie_Chart(string ActivityType, string Activity_ID)
        {
            if (DDLWorkPackage.SelectedValue != "--Select--")
            {
                DataSet ds = getdt.Get_Open_Closed_Rejected_Issues_by_WorkPackageUID(new Guid(Activity_ID));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    StringBuilder strScript = new StringBuilder();
                    strScript.Append(@"<script type='text/javascript'>        
                google.charts.load('current', { packages: ['corechart'] });
                google.charts.setOnLoadCallback(drawChart);

                 function drawChart() {
                     var data = google.visualization.arrayToDataTable([
                       ['Issues', 'Count'],");
                    strScript.Append("['Open Issues', " + ds.Tables[0].Rows[0]["OpenIssues"].ToString() + "], ['In-Progress Issues', " + ds.Tables[0].Rows[0]["InProgressIssues"].ToString() + "], ['Closed Issues', " + ds.Tables[0].Rows[0]["ClosedIssues"].ToString() + "], ['Rejected Issues', " + ds.Tables[0].Rows[0]["RejectedIssues"].ToString() + "]]);");
                    strScript.Append(@"var options = {
                         is3D: true,
                         legend: { position: 'labeled', textStyle: { color: 'black', fontSize: 13 } },
                         colors: ['#3366CC', '#FF9900', '#109618', '#DC3912'],
                         pieSliceText: 'value',
                         pieSliceTextStyle: { bold: true, fontSize: 13 },
                         chartArea: {                        
                             height: '92%',
                             width: '92%'
                         }
                     };
                        function selectHandler() {
                        var selection = chart.getSelection(); 
                        if (selection.length > 0) {
                        var selectedItem = chart.getSelection()[0];
                        if (selectedItem) {
                           window.open('/_content_pages/issues', '_self', true);
                         }
                        }
                     }
                     var chart = new google.visualization.PieChart(document.getElementById('piechart_3d'));
                      google.visualization.events.addListener(chart, 'select', selectHandler);
                     chart.draw(data, options);}
                    </script>");
                    ltScripts_piechart.Text = strScript.ToString();
                }
                else
                {
                    ltScripts_piechart.Text = "<h4>No data</h4>";
                }

                //DataSet ds = getdt.GetTask_Status_Count(Session["UserUID"].ToString(), ActivityType, Activity_ID);
                //if (ds.Tables[0].Rows.Count > 0)
                //{

                //    StringBuilder strScript = new StringBuilder();
                //    strScript.Append(@"<script type='text/javascript'>        
                //google.charts.load('current', { packages: ['corechart'] });
                //google.charts.setOnLoadCallback(drawChart);

                // function drawChart() {
                //     var data = google.visualization.arrayToDataTable([
                //       ['Task', 'Hours per Day'],");
                //    strScript.Append("['Not Started', " + ds.Tables[0].Rows[0]["Pending"].ToString() + "], ['Completed', " + ds.Tables[0].Rows[0]["Completed"].ToString() + "], ['In Progress', " + ds.Tables[0].Rows[0]["Inprogress"].ToString() + "]]);");
                //    strScript.Append(@"var options = {
                //         is3D: true,
                //         legend: { position: 'labeled', textStyle: { color: 'black', fontSize: 13 } },
                //         pieSliceText: 'value',
                //         pieSliceTextStyle: { bold: true, fontSize: 13 },
                //         chartArea: {                        
                //             height: '92%',
                //             width: '92%'
                //         }
                //     };
                //     var chart = new google.visualization.PieChart(document.getElementById('piechart_3d'));  
                //     chart.draw(data, options);}
                //    </script>");
                //    ltScripts_piechart.Text = strScript.ToString();


                //    //function selectHandler()
                //    //{
                //    //    var selectedItem = chart.getSelection()[0];
                //    //    if (selectedItem)
                //    //    {
                //    //        var topping = data.getValue(selectedItem.row, 0); ");
                //    //  strScript.Append("window.open('WorkPackages.aspx?TaskType=' + topping + '&ProjectUID=" + DDlProject.SelectedValue + "&WorkPackageUID=" + DDLWorkPackage.SelectedValue + "', '_self', true);");
                //    //        //alert('The user selected ' + topping);
                //    //        strScript.Append(@"}
                //    //}
                //    //google.visualization.events.addListener(chart, 'select', selectHandler);

                //    //}
                //    //else
                //    //{
                //    //    ltScripts_piechart.Text = " < h4>No data</h4>";
                //    //}
                //}
                //else
                //{
                //    ltScripts_piechart.Text = "<h4>No data</h4>";
                //}
            }

        }

        private void Bind_ProgressChart(string ActivityType, string Activity_ID)
        {
            if (DDLWorkPackage.SelectedValue != "--Select--")
            {
                DataSet ds = getdt.GetConstructionProgramme_Tasks(new Guid(Activity_ID));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    StringBuilder strScript = new StringBuilder();
                    strScript.Append(@"<script type='text/javascript'>
                google.charts.load('current', { packages: ['corechart', 'bar'] });
                google.charts.setOnLoadCallback(drawBasic);
                function drawBasic() {
                var data = google.visualization.arrayToDataTable([
                ['Task', 'Target', 'Cumulative achieved'],");
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        string tName = ds.Tables[0].Rows[i]["Name"].ToString();
                        if (tName.Length > 20)
                        {
                            tName = ds.Tables[0].Rows[i]["Name"].ToString().Substring(0, 20) + "..";
                        }
                        else
                        {
                            tName = ds.Tables[0].Rows[i]["Name"].ToString();
                        }


                        string Target = "0";
                        string Cumulative = "0";
                        DataSet dsVal = getdt.GetTask_Target_Cumulative(new Guid(ds.Tables[0].Rows[i]["TaskUID"].ToString()));
                        if (dsVal.Tables[0].Rows.Count > 0)
                        {
                            Target = dsVal.Tables[0].Rows[0]["TargetValue"].ToString();
                            Cumulative = dsVal.Tables[0].Rows[0]["Culumative"].ToString();
                        }
                        strScript.Append("['" + tName + "', " + Target + ", " + Cumulative + "],");
                    }
                    strScript.Remove(strScript.Length - 1, 1);
                    strScript.Append("]);");
                    strScript.Append(@"var options = {
                            is3D: true,
                            legend: { position: 'none' },
                            fontSize: 13,
                            bar: { groupWidth:'50%' },
                            chartArea: {
                                left: '35%',
                                top: '10%',
                                height: '80%',
                                width: '60%'
                            },
                            height: 300,
                            bars: 'horizontal',
                            vAxis: { 
                             gridlines: { count: 10 } 
                              },
                            hAxis: {
                                minValue: 0,
                                gridlines: {count: 10}
                            }
                        };
                var chart = new google.visualization.BarChart(document.getElementById('chart_div'));
                chart.draw(data, options);
                }
                </script>");

                    ltScript_Progress.Text = strScript.ToString();
                }
                else
                {
                    ltScript_Progress.Text = "<h4>No data</h4>";
                }
                //DataSet ds = getdt.GetTask_Status_Percentage(Session["UserUID"].ToString(), ActivityType, Activity_ID);
                //if (ds.Tables[0].Rows.Count > 0)
                //{
                //    StringBuilder strScript = new StringBuilder();
                //    strScript.Append(@"<script type='text/javascript'>
                //    google.charts.load('current', { packages: ['corechart', 'bar'] });
                //    google.charts.setOnLoadCallback(drawBasic);

                //    function drawBasic() {
                //        var data = google.visualization.arrayToDataTable([
                //          ['Task', 'Completion(in %)', { role: 'style' }, { role: 'annotation' }],");
                //    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                //    {
                //        string name = ds.Tables[0].Rows[i]["Name"].ToString();
                //        if (name.Length > 10)
                //        {
                //            name= ds.Tables[0].Rows[i]["Name"].ToString().Substring(0, 10) + "..";
                //        }
                //        else
                //        {
                //            name = ds.Tables[0].Rows[i]["Name"].ToString();
                //        }
                //        if (ds.Tables[0].Rows[i]["StatusPer"].ToString() != "")
                //        {
                //            string colorCode = string.Empty;
                //            if (i == 0)
                //            {
                //                colorCode = "#3366CC";
                //            }
                //            else if (i == 1)
                //            {
                //                colorCode = "#990099";
                //            }
                //            else if (i == 2)
                //            {
                //                colorCode = "#109618";
                //            }
                //            else if (i == 3)
                //            {
                //                colorCode = "#DC3912";
                //            }
                //            else if (i == 4)
                //            {
                //                colorCode = "#DC3912";
                //            }
                //            else
                //            {
                //                colorCode = "#3366CC";
                //            }
                //            strScript.Append("['" + name + "', " + ds.Tables[0].Rows[i]["StatusPer"].ToString() + ", '" + colorCode + "', '" + ds.Tables[0].Rows[i]["StatusPer"].ToString() + "%'],");
                //        }
                //    }
                //    strScript.Remove(strScript.Length - 1, 1);
                //    strScript.Append("]);");
                //    strScript.Append(@"var options = {
                //            is3D: true,
                //            legend: { position: 'none' },
                //            fontSize: 13,
                //            bar: { groupWidth:'50%' },
                //            chartArea: {
                //                left: '25%',
                //                top: '10%',
                //                height: '80%',
                //                width: '50%'
                //            },
                //            height: 300,
                //            bars: 'horizontal',
                //            axes: {
                //                x: {
                //                    0: { side: 'top', label: 'Percentage' } // Top x-axis.
                //                }
                //            },
                //            hAxis: {
                //                minValue: 0,
                //                format: '#\'%\''
                //            }
                //        };

                //      function selectHandler() {
                //      var selectedItem = chart.getSelection()[0];
                //      if (selectedItem) {
                //        var topping = data.getValue(selectedItem.row, 0);");
                //    strScript.Append("window.open('WorkPackages.aspx?TaskName=' + topping + '&ProjectUID=" + DDlProject.SelectedValue + "&WorkPackageUID=" + DDLWorkPackage.SelectedValue + "', '_self', true);");
                //    //alert('The user selected ' + topping);
                //    strScript.Append(@"}
                //    }

                //        var chart = new google.visualization.BarChart(document.getElementById('chart_div'));

                //        chart.draw(data, options);
                //    }
                //</script>");
                //    ltScript_Progress.Text = strScript.ToString();
                //    //google.visualization.events.addListener(chart, 'select', selectHandler);
                //}
                //else
                //{
                //    ltScript_Progress.Text = "<h4>No data</h4>";
                //}
            }
        }

        //private void Bind_CostChart(string ActivityType, string Activity_ID)
        //{
        //    if (DDLWorkPackage.SelectedValue != "--Select--")
        //    {
        //        ltScript_Progress.Text = string.Empty;
        //        DataSet ds = getdt.Get_WorkPackage_Budget(Session["UserUID"].ToString(), ActivityType, Activity_ID);
        //        if (ds.Tables[0].Rows.Count > 0)
        //        {
        //            StringBuilder strScript = new StringBuilder();
        //            strScript.Append(@" <script type='text/javascript'>

        //        google.charts.load('current', { packages: ['corechart', 'bar'] });
        //        google.charts.setOnLoadCallback(drawBasic);

        //        function drawBasic() {

        //        var data = google.visualization.arrayToDataTable([
        //             ['Element', 'Cost', { role: 'style' }, { role: 'annotation' }],");
        //            string CurrencySymbol = "";
        //            if (ds.Tables[0].Rows[0]["Currency"].ToString() == "&#x20B9;")
        //            {
        //                CurrencySymbol = "₹";
        //            }
        //            else if (ds.Tables[0].Rows[0]["Currency"].ToString() == "&#36;")
        //            {
        //                CurrencySymbol = "$";
        //            }
        //            else
        //            {

        //                CurrencySymbol = "¥";
        //            }
        //            strScript.Append("['Actual', " + ds.Tables[0].Rows[0]["Actual"].ToString() + ", '#3366CC', '" + CurrencySymbol + ' ' + ds.Tables[0].Rows[0]["Actual"].ToString() + "'],['Planned', " + ds.Tables[0].Rows[0]["Planned"].ToString() + ", '#DC3912', '" + CurrencySymbol + ' ' + ds.Tables[0].Rows[0]["Planned"].ToString() + "'],['Budget', " + ds.Tables[0].Rows[0]["Budget"].ToString() + ", '#109618', '" + CurrencySymbol + ' ' + ds.Tables[0].Rows[0]["Budget"].ToString() + "']]);");
        //            strScript.Append(@"var options = {
        //            is3D: true,
        //            legend: { position: 'none' },
        //            fontSize: 14,
        //            chartArea: {
        //                left: '10%',
        //                top: '10%',
        //                height: '75%',
        //                width: '80%'
        //            },
        //            height: 300
        //        };

        //        var chart = new google.visualization.ColumnChart(
        //          document.getElementById('chart_div'));
        //         chart.draw(data, options);

        //    }</script>");
        //            //ltScript_Cost.Text = strScript.ToString();
        //            ltScript_Progress.Text = strScript.ToString();
        //        }
        //        else
        //        {
        //            //ltScript_Cost.Text = "<h3>No data</h3>";
        //            ltScript_Progress.Text = "<h3>No data</h3>";

        //        }
        //    }
        //}


        private void Bind_CostChart(string ActivityType, string Activity_ID)
        {
            if (DDLWorkPackage.SelectedValue != "--Select--")
            {
                ltScript_Progress.Text = string.Empty;
                // DataSet ds = getdt.Get_WorkPackage_Budget(Session["UserUID"].ToString(), ActivityType, Activity_ID);

                DataSet ds = getdt.GetCostGraphData(Activity_ID); // changed by saji augustin dated 13 may 2022

                if (ds.Tables[0].Rows.Count > 0)
                {
                    StringBuilder strScript = new StringBuilder();
                    strScript.Append(@" <script type='text/javascript'>
                
                google.charts.load('current', { packages: ['corechart', 'bar'] });
                google.charts.setOnLoadCallback(drawBasic);

                function drawBasic() {

                var data = google.visualization.arrayToDataTable([
                     ['Element', 'Cost', { role: 'style' }, { role: 'annotation' }],");
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
                    strScript.Append("['Actual', " + ds.Tables[0].Rows[0]["Actual"].ToString() + ", '#3366CC', '" + CurrencySymbol + ' ' + ds.Tables[0].Rows[0]["Actual"].ToString() + "'],['Planned', " + ds.Tables[0].Rows[0]["Planned"].ToString() + ", '#DC3912', '" + CurrencySymbol + ' ' + ds.Tables[0].Rows[0]["Planned"].ToString() + "'],['Budget', " + ds.Tables[0].Rows[0]["Budget"].ToString() + ", '#109618', '" + CurrencySymbol + ' ' + ds.Tables[0].Rows[0]["Budget"].ToString() + "']]);");
                    strScript.Append(@"var options = {
                    title : 'Cost in Crores of Rupees',
                    is3D: true, 
                    legend: { position: 'none' },
                    fontSize: 14,
                    chartArea: {
                        left: '10%',
                        top: '10%',
                        height: '75%',
                        width: '80%'
                    },
                    height: 300
                };

                var chart = new google.visualization.ColumnChart(
                  document.getElementById('chart_div'));
                 chart.draw(data, options);
                
            }</script>");
                    //ltScript_Cost.Text = strScript.ToString();
                    ltScript_Progress.Text = strScript.ToString();
                }
                else
                {
                    //ltScript_Cost.Text = "<h3>No data</h3>";
                    ltScript_Progress.Text = "<h3>No data</h3>";

                }
            }
        }

        private void Bind_ResourceChart(string ActivityType, string Activity_ID)
        {
            if (DDLWorkPackage.SelectedValue != "--Select--")
            {

                if (DDlResource.Items.Count > 0)
                {
                    DataSet ds = getdt.Get_WorkPackage_WorkLoad(Session["UserUID"].ToString(), ActivityType, Activity_ID, DDlResource.SelectedValue);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        StringBuilder strScript = new StringBuilder();
                        strScript.Append(@"<script type='text/javascript'>
                google.charts.load('current', { packages: ['corechart', 'bar'] });
                google.charts.setOnLoadCallback(drawStacked);

                function drawStacked() {
                    var data = google.visualization.arrayToDataTable([
                      ['WorkLoad', 'Used', 'Remaining' , { role: 'annotation' }],");
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            strScript.Append("['" + getdt.getTaskNameby_TaskUID(new Guid(ds.Tables[0].Rows[i]["TName"].ToString())) + "', " + ds.Tables[0].Rows[i]["Completed"].ToString() + ", " + ds.Tables[0].Rows[i]["Remaining"].ToString() + "," + ds.Tables[0].Rows[i]["AllocatedUnits"].ToString() + "],");
                        }
                        strScript.Remove(strScript.Length - 1, 1);
                        strScript.Append("]);");

                        strScript.Append(@"var options = {
                        legend:'none',
                        annotations: {alwaysOutside: true},
                        fontSize: 13,
                        height: 300,
                        chartArea: {
                            left: '20%',
                            top: '10%',
                            height: '75%',
                            width: '70%'
                        },
                        isStacked: true
                    };
                    var chart = new google.visualization.BarChart(document.getElementById('Resource_div'));
                    chart.draw(data, options);
                }
            </script>");
                        //LblTitle.Text = DDlResource.SelectedItem.Text;
                        ltScript_Resource.Text = strScript.ToString();
                    }
                    else
                    {
                        ltScript_Resource.Text = "<h4>No data</h4>";
                    }
                }
                else
                {
                    ltScript_Resource.Text = "<h4>No data</h4>";
                }
            }
        }

        protected void DDlResource_SelectedIndexChanged(object sender, EventArgs e)
        {
            Bind_ResourceChart("Work Package", DDLWorkPackage.SelectedValue);
        }

        protected void RdList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDLWorkPackage.SelectedValue != "")
            {
                if (RdList.SelectedValue == "Progress")
                {
                    Bind_ProgressChart("Work Package", DDLWorkPackage.SelectedValue);
                }
                else
                {
                    Bind_CostChart("Work Package", DDLWorkPackage.SelectedValue);
                }
            }
           
        }

        private void LoadGraph()
        {
            try
            {
                if (DDLWorkPackage.SelectedValue != "--Select--")
                {
                    ltScript_PhysicalProgress.Text = string.Empty;

                    DataSet ds = getdt.GetTaskScheduleDatesforGraph(new Guid(DDLWorkPackage.SelectedValue));
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        StringBuilder strScript = new StringBuilder();
                        string tablemonths = "<td>&nbsp;</td>";
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
                            dsvalues = getdt.GetTaskScheduleValuesForGraph(new Guid(DDLWorkPackage.SelectedValue), Convert.ToDateTime(dr["StartDate"].ToString()), Convert.ToDateTime(dr["StartDate"].ToString()).AddMonths(1));
                            if (dsvalues.Tables[0].Rows.Count > 0)
                            {
                                planvalue = decimal.Parse(dsvalues.Tables[0].Rows[0]["TotalSchValue"].ToString());
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
                                strScript.Append("['" + Convert.ToDateTime(dr["StartDate"].ToString()).ToString("MMMyy") + "'," + planvalue + "," + actualvalue + "," + cumplanvalue + "," + cumactualvalue + "]]);");
                            }
                            //
                            tablemonths += "<td>" + Convert.ToDateTime(dr["StartDate"].ToString()).ToString("MMM-yy") + "</td>";
                            tmonthlyplan += "<td>" + decimal.Round(planvalue, 2) + "</td>";
                            tmonthlyactual += "<td>" + decimal.Round(actualvalue, 2) + "</td>";

                            tcumulativeplan += "<td>" + decimal.Round(cumplanvalue, 2) + "</td>";
                            tcumulativeactual += "<td>" + decimal.Round(cumactualvalue, 2) + "</td>";


                            //
                            count++;
                        }

                        strScript.Append(@"var options = {
          title : 'Plan vs Achieved Progress Curve',
          
          hAxis: {title: 'MONTH',titleTextStyle: {
        bold:'true',
      }},
          seriesType: 'bars',
          series: {2: {type: 'line',targetAxisIndex: 1},3: {type: 'line',targetAxisIndex: 1}},
vAxes: {
            // Adds titles to each axis.
          
            0: {title: 'Monthly Plan (%)',titleTextStyle: {
        bold:'true',
      }},
            1: {title: 'Cumulative Plan (%)',titleTextStyle: {
        bold:'true',
      }}
          }
        };
                var chart = new google.visualization.ComboChart(
                  document.getElementById('chart_divProgress'));
                 chart.draw(data, options);
                
            }</script>");
                        //ltScript_Cost.Text = strScript.ToString();
                        ltScript_PhysicalProgress.Text = strScript.ToString();
                        divtable.InnerHtml = "<table border=\"1\" style=\"text-align:center;font-size:11px;padding-left:0px;\">" +
                                          "<tr> " + tablemonths + "</tr>" +
                                           "<tr> " + tmonthlyplan + "</tr>" +
                                            "<tr> " + tmonthlyactual + "</tr>" +
                                             "<tr> " + tcumulativeplan + "</tr>" +
                                              "<tr> " + tcumulativeactual + "</tr>" +
                                                  "</table>";
                        btnPrint.Visible = true;
                    }
                    else
                    {
                        ltScript_PhysicalProgress.Text = "<h3>No data</h3>";
                        divtable.InnerHtml = "";
                        btnPrint.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void rdSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rdSelect.SelectedIndex == 0)
            {
                divProgresschart.Visible = false;
                divFinProgressChart.Visible = false;
                divNJSEIMIS.Visible = false;
                divdashboardimage.Visible = true;
                divMainblocks.Visible = true;
            }
            else if (rdSelect.SelectedIndex == 1)
            {
                divProgresschart.Visible = true;
                divFinProgressChart.Visible = false;
                divNJSEIMIS.Visible = false;
                divdashboardimage.Visible = true;
                divMainblocks.Visible = true;
                LoadGraph();
            }
            else if (rdSelect.SelectedIndex == 2)
            {
                divProgresschart.Visible = false;
                divFinProgressChart.Visible = true;
                divNJSEIMIS.Visible = false;
                divdashboardimage.Visible = true;
                divMainblocks.Visible = true;
                LoadFinancialGraph();
            }
            //else if (rdSelect.SelectedIndex == 3)
            //{
            //    divProgresschart.Visible = false;
            //    divFinProgressChart.Visible = false;
            //    divNJSEIMIS.Visible = true;
            //    divdashboardimage.Visible = true;
            //    divMainblocks.Visible = false;
            //}
        }
        protected void RBLPhotographs_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void LoadFinancialGraph()
        {
            try
            {
                if (DDLWorkPackage.SelectedValue != "--Select--")
                {
                    ltScript_FinProgress.Text = string.Empty;

                    DataSet ds = getdt.GetFinancialScheduleDatesforGraph(new Guid(DDLWorkPackage.SelectedValue));
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        StringBuilder strScript = new StringBuilder();
                        string tablemonths = "<td style=\"width:200px\">&nbsp;</td>";
                        string tmonthlyplan = "<td style=\"padding:3px\">Monthly Plan</td>";
                        string tmonthlyactual = "<td style=\"padding:3px\">Monthly Actual</td>";
                        string tcumulativeplan = "<td style=\"padding:3px\">Cumulative Plan</td>";
                        string tcumulativeactual = "<td style=\"padding:3px\">Cumulative Actual</td>";
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
                            //dsvalues.Clear();
                            //dsvalues = getdt.GetTaskScheduleValuesForGraph(new Guid(DDLWorkPackage.SelectedValue), Convert.ToDateTime(dr["StartDate"].ToString()), Convert.ToDateTime(dr["StartDate"].ToString()).AddMonths(1));
                            //if (dsvalues.Tables[0].Rows.Count > 0)
                            //{
                            //    planvalue = decimal.Parse(dsvalues.Tables[0].Rows[0]["TotalSchValue"].ToString());
                            //    actualvalue = decimal.Parse(dsvalues.Tables[0].Rows[0]["TotalAchValue"].ToString());
                            //    cumplanvalue += planvalue;
                            //    cumactualvalue += actualvalue;
                            //}
                            dsvalues = getdt.GetFinMonthsPaymentTotal(new Guid(dr["FinMileStoneMonthUID"].ToString()));
                            planvalue = decimal.Parse(dr["AllowedPayment"].ToString());
                            actualvalue = 0;
                            if (dsvalues.Tables[0].Rows.Count > 0)
                            {
                                if (dsvalues.Tables[0].Rows[0]["TotalAmnt"].ToString() != "&nbsp;" && dsvalues.Tables[0].Rows[0]["TotalAmnt"].ToString() != "")
                                {
                                    // e.Row.Cells[2].Text = (decimal.Parse(ds.Tables[0].Rows[0]["TotalAmnt"].ToString()) / 10000000).ToString("n2");
                                    actualvalue = (decimal.Parse(dsvalues.Tables[0].Rows[0]["TotalAmnt"].ToString()) / 10000000);
                                }
                            }
                            // comment this code..used only for demo since actual values are not available....1
                            //Random random = new Random();
                            //if (planvalue > 0)
                            //{
                            //    System.Threading.Thread.Sleep(1000);
                            //    actualvalue = planvalue - random.Next(2,5);
                            //}

                            //
                            cumplanvalue += planvalue;
                            cumactualvalue += actualvalue;
                            if (count < ds.Tables[0].Rows.Count)
                            {

                                strScript.Append("['" + dr["MonthYear"].ToString() + "'," + planvalue + "," + actualvalue + "," + cumplanvalue + "," + cumactualvalue + "],");
                            }
                            else
                            {
                                strScript.Append("['" + dr["MonthYear"].ToString() + "'," + planvalue + "," + actualvalue + "," + cumplanvalue + "," + cumactualvalue + "]]);");
                            }
                            //
                            tablemonths += "<td style=\"padding:3px\">" + dr["MonthYear"].ToString() + "</td>";
                            tmonthlyplan += "<td style=\"padding:3px\">" + decimal.Round(planvalue, 2) + "</td>";
                            tmonthlyactual += "<td style=\"padding:3px\">" + decimal.Round(actualvalue, 2) + "</td>";

                            tcumulativeplan += "<td style=\"padding:3px\">" + decimal.Round(cumplanvalue, 2) + "</td>";
                            tcumulativeactual += "<td style=\"padding:3px\">" + decimal.Round(cumactualvalue, 2) + "</td>";


                            //
                            count++;
                        }

                        strScript.Append(@"var options = {
          title : 'Plan vs Achieved Progress Curve',
          
          hAxis: {title: 'MONTH',titleTextStyle: {
        bold:'true',
      }},
          seriesType: 'bars',
          series: {2: {type: 'line',targetAxisIndex: 1},3: {type: 'line',targetAxisIndex: 1}},
vAxes: {
            // Adds titles to each axis.
          
            0: {title: 'Monthly Plan (Crores.)',titleTextStyle: {
        bold:'true',
      }},
            1: {title: 'Cumulative Plan (Crores.)',titleTextStyle: {
        bold:'true',
      }}
          }
        };
                var chart = new google.visualization.ComboChart(
                  document.getElementById('chart_divProgressFin'));
                 chart.draw(data, options);
                
            }</script>");
                        //ltScript_Cost.Text = strScript.ToString();
                        ltScript_FinProgress.Text = strScript.ToString();
                        divtableFin.InnerHtml = "<table border=\"1\" style=\"text-align:center;font-size:11px;padding-left:10px;\">" +
                                          "<tr> " + tablemonths + "</tr>" +
                                           "<tr> " + tmonthlyplan + "</tr>" +
                                            "<tr> " + tmonthlyactual + "</tr>" +
                                             "<tr> " + tcumulativeplan + "</tr>" +
                                              "<tr> " + tcumulativeactual + "</tr>" +
                                                  "</table>";
                        btnPrint.Visible = true;
                    }
                    else
                    {
                        ltScript_FinProgress.Text = "<h3>No data</h3>";
                        divtableFin.InnerHtml = "";
                        btnPrint.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void RadioButtonList2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(RadioButtonList2.SelectedIndex == 0)
            {
                divdummyblock1.Visible = true;
                divdummyblock1_1.Visible = false;
            }
            else
            {
                divdummyblock1_1.Visible = true;
                divdummyblock1.Visible = false;
            }
        }

        private static int getUserDocsNo()// this is for getting no of docs user has to act on
        {
            DBGetData dbget = new DBGetData();
            int docscount = 0;
            string[] selectedValue = HttpContext.Current.Session["Project_Workpackage"].ToString().Split('_');
            
            DataSet ds = dbget.GetNextUserDocuments(new Guid(selectedValue[0]), new Guid(selectedValue[1]));
            DataSet dsNxtUser = new DataSet();
            foreach (DataRow drnext in ds.Tables[0].Rows)
            {
                DataSet dsTop = dbget.getTop1_DocumentStatusSelect(new Guid(drnext["ActualDocumentUID"].ToString()));
                DataSet dsNext = dbget.GetNextStep_By_DocumentUID(new Guid(drnext["ActualDocumentUID"].ToString()), dsTop.Tables[0].Rows[0]["ActivityType"].ToString());

                foreach (DataRow dr in dsNext.Tables[0].Rows)
                {
                    dsNxtUser = new DataSet();
                    dsNxtUser = dbget.GetNextUser_By_DocumentUID(new Guid(drnext["ActualDocumentUID"].ToString()), int.Parse(dr["ForFlow_Step"].ToString()));
                    if (dsNxtUser.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow druser in dsNxtUser.Tables[0].Rows)
                        {
                            if (HttpContext.Current.Session["UserUID"].ToString().ToUpper() == druser["Approver"].ToString().ToUpper())
                            {
                                if (dsTop.Tables[0].Rows[0]["ActivityType"].ToString() == "Accepted")
                                {
                                    if (dbget.checkUserAddedDocumentstatus(new Guid(drnext["ActualDocumentUID"].ToString()), new Guid(HttpContext.Current.Session["UserUID"].ToString()), dsTop.Tables[0].Rows[0]["ActivityType"].ToString()) == 0)
                                    {
                                        docscount = docscount + 1;
                                    }
                                }
                                else
                                {
                                    docscount = docscount + 1;
                                }
                                goto afterloop;
                            }
                            else
                            {


                            }
                        }
                    }
                   
                }

                afterloop:
                Console.WriteLine("/Done");
            }
            return docscount;
        }

        [WebMethod(EnableSession = true)]
        public static string  GetDetails(string Id)
        {
            string[] selectedValue = HttpContext.Current.Session["Project_Workpackage"].ToString().Split('_');
            return getUserDocsNo().ToString() + "$" +  selectedValue[1];
        }
    }
}