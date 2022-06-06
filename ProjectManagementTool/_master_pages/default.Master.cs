using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManager._master_pages
{
    public partial class _default : System.Web.UI.MasterPage
    {
        DBGetData getdt = new DBGetData();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Username"] == null)
            {
                Response.Redirect("/Login.aspx");
            }
            else
            {
                lblUserGreeting.InnerText = "Hi, " + Session["Username"];
                HideMenus();
                DisplayLogo();
            }
        }

        private void DisplayLogo()
        {
            string host = Request.Url.Host.ToLower();

            DataSet ds = getdt.GetDomainDetails_by_URL(host.Replace("www.", ""));
            if (ds.Tables[0].Rows.Count > 0)
            {
                LblTitle.Text = ds.Tables[0].Rows[0]["Title"].ToString();
                if (ds.Tables[0].Rows[0]["Logo"].ToString() != "")
                {
                    ONTBLogo.Visible = false;
                    sLogo.ImageUrl = "/_assets/Logos/" + ds.Tables[0].Rows[0]["Logo"].ToString();
                }
                else
                {
                    sLogo.Visible = false;
                    ONTBLogo.Visible = true;
                }
            }
            //string Domain = WebConfigurationManager.AppSettings["Domain"];
            //if (Domain == "NJSEI")
            //{
            //    NJSEI.Visible = true;
            //    ONTB.Visible = false;
            //    ONTBLogo.Visible = false;
            //    NJSEILogo.Visible = true;
            //}
            //else
            //{
            //    NJSEI.Visible = false;
            //    ONTB.Visible = true;
            //    ONTBLogo.Visible = true;
            //    NJSEILogo.Visible = false;
            //}
        }

        protected void LnkBtnSignOut_Click(object sender, EventArgs e)
        {
            getdt.UsersLogOutStatus(Session["Username"].ToString(), HttpContext.Current.Session.SessionID);
            Session["Username"] = null;
            Session["UserUID"] = null;
            Session["TypeOfUser"] = null;
            Session["FirstName"] = null;
            Session["LastName"] = null;
            Session["copydocument"] = null;
            Session["CopiedActivity"] = null;
            Session["SelectedWorkpakage"] = null;
            Session["Project_Workpackage"] = null;
            Response.Redirect("/Login.aspx");
        }
        private void HideMenus()
        {

            //
            //if (Session["TypeOfUser"].ToString() == "U")
            //{
                
            //    UserDocumentProgress.Visible = true;                
            //}
            //else if (Session["TypeOfUser"].ToString() == "PA")
            //{
                
            //    UserDocumentProgress.Visible = true;
            //}
            //else if (Session["TypeOfUser"].ToString() == "MD" || Session["TypeOfUser"].ToString() == "RD" || Session["TypeOfUser"].ToString() == "VP" || Session["TypeOfUser"].ToString() == "RM")
            //{
                
            //    UserDocumentProgress.Visible = true;
            //}
            //else if (Session["TypeOfUser"].ToString() == "SPM" || Session["TypeOfUser"].ToString() == "PM" || Session["TypeOfUser"].ToString() == "CM")
            //{
                
            //    UserDocumentProgress.Visible = true;
            //}
            //else
            //{
                
            //    UserDocumentProgress.Visible = false;
            //}
            // added on 14/11/2020

            DataSet dscheck = new DataSet();
            dscheck = getdt.GetUsertypeFunctionality_Mapping(Session["TypeOfUser"].ToString());
            
            
            Master.Visible = false;
            navLinkAssignProject.Visible = false;
            hlprojects.Visible = false;
            ReviewReports.Visible = false;
            AddProjectStatusofWorkProgress.Visible = false;
            AddViewSitePhotographs.Visible = false;
            AddViewConsolidatedActivities.Visible = false;
            ReportProjectStatusofWorkProgress.Visible = false;
            if (Session["TypeOfUser"].ToString() != "U")
            {
                ViewReports.Visible = false;
                ReportContractData.Visible = false;
                ReportDocumentStatus.Visible = false;
                ReportConstructionDrawingStatus.Visible = false;
                ReportUserDocumentProgress.Visible = false;
                ReportProjectPhysicalProgress.Visible = false;
                ReportMonthlyPhysicalProgress.Visible = false;
                ReportMonthlyFinancialProgress.Visible = false;
                
                ReportResourceDeployment.Visible = false;
                ReportIssues.Visible = false;
                ProfileManagement.Visible = false;
                BOQ.Visible = false;
                RAbill.Visible = false;
                Invoice.Visible = false;
                Issues.Visible = false;
                Insurance.Visible = false;
                BankGuarantee.Visible = false;
                navLinkUser_UserManagement.Visible = false;
                ProjectTracking.Visible = false;
                FinanceWorkpackage.Visible = false;
                StatusUpdate.Visible = false;
                // added on 24/09/2021
                AccessedDocsHistory.Visible = false;
                Misc_Camera.Visible = false;
                Misc_Reviews.Visible = false;
                Misc_Measurement.Visible = false;
                Misc_Review_Meeting.Visible = false;
                Misc_Project_PhysicalPrg.Visible = false;
                Misc_Project_Status.Visible = false;
                Misc_Site_Photos.Visible = false;
                Misc_Claims.Visible = false;
                Misc_Budget_Disb.Visible = false;
                Misc_Budget_JICA.Visible = false;
                Misc_MOM.Visible = false;
                Misc_Consolidated.Visible = false;
                Misc_Status_WasteWater.Visible = false;
                Misc_OtherPoints.Visible = false;
                RAbillPriceAdj.Visible = false;
                //
                ReportWkgMasterDataSettings.Visible = false;
                ReportWkgMasterData.Visible = false;
            }

            int ReportExists = 0;
            if (dscheck.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dscheck.Tables[0].Rows)
                {
                    if (dr["Code"].ToString() == "FW" || dr["Code"].ToString() == "FV") // VIEWING OF REPORTS //REPORT GENERATION & DOWNLOAD
                    {
                        ViewReports.Visible = true;
                    }
                    if (dr["Code"].ToString() == "FX") // PROJECT PROGRESS TRACKING
                    {
                        if (Session["TypeOfUser"].ToString() != "EXTO")
                        {
                            ProjectTracking.Visible = true;
                        }
                        StatusUpdate.Visible = true;
                    }
                    if (dr["Code"].ToString() == "FS" || dr["Code"].ToString() == "FT" || dr["Code"].ToString() == "FZ")  // VIEW FINANCIAL PROGRESS OF PROJECT-INDIVIDUAL REGIONS // ALL // INDIVIDUAL PROJECT //
                    {
                        FinanceWorkpackage.Visible = true;

                    }
                    if (dr["Code"].ToString() == "FF") // INPUT PROJECT RELATED DATA IN MASTER FILE
                    {
                        Master.Visible = true;

                    }
                    if (dr["Code"].ToString() == "FD" || dr["Code"].ToString() == "FE") // CREATE Projects
                    {
                        hlprojects.Visible = true;
                        navLinkAssignProject.Visible = true;
                    }
                    if (dr["Code"].ToString() == "UM") // USER Management
                    {

                        navLinkUser_UserManagement.Visible = true;
                    }
                    if (dr["Code"].ToString() == "RR") // Review Reports
                    {
                        ReviewReports.Visible = true;
                    }

                    if (dr["Code"].ToString() == "BG") // Bank Guarantee
                    {
                        BankGuarantee.Visible = true;
                    }

                    if (dr["Code"].ToString() == "APSWP") //Add : Project Status of Work Progress
                    {
                        AddProjectStatusofWorkProgress.Visible = true;
                    }
                    if (dr["Code"].ToString() == "APSWP") //Add/View : Site Photographs
                    {
                        AddViewSitePhotographs.Visible = true;
                    }

                    if (dr["Code"].ToString() == "AVCA") //View : Consolidated Activities
                    {
                        AddViewConsolidatedActivities.Visible = true;
                    }

                    if (dr["Code"].ToString() == "RRPSWP") //Review Report : Project Status of Work Progress
                    {
                        ReportProjectStatusofWorkProgress.Visible = true;
                        ReportExists += 1;
                    }

                    if (dr["Code"].ToString() == "R-CD") //View : Report - Contract Data
                    {
                        ReportContractData.Visible = true;
                        ReportExists += 1;
                    }

                    if (dr["Code"].ToString() == "R-DS") //View : Report - Document Status
                    {
                        ReportDocumentStatus.Visible = true;
                        ReportExists += 1;
                    }

                    if (dr["Code"].ToString() == "R-CDS") //View : Report - Construction Drawing Status
                    {
                        ReportConstructionDrawingStatus.Visible = true;
                        ReportExists += 1;
                    }

                    if (dr["Code"].ToString() == "R-U/PWDU") //View : Report - User/Project Wise Documents Upload
                    {
                        ReportUserDocumentProgress.Visible = true;
                        ReportExists += 1;
                    }

                    if (dr["Code"].ToString() == "R-PPP") //View : Report - Project Physical Progress
                    {
                        ReportProjectPhysicalProgress.Visible = true;
                        ReportExists += 1;
                    }

                    if (dr["Code"].ToString() == "R-MPPP") //View : Report - Monthly Project Physical Progress(%)
                    {
                        ReportMonthlyPhysicalProgress.Visible = true;
                        ReportExists += 1;
                    }
                    if (dr["Code"].ToString() == "R-WMDUS") //Report - WorkPackage Master Data Update Settings
                    {
                        ReportWkgMasterDataSettings.Visible = true;
                        ReportExists += 1;
                    }
                    if (dr["Code"].ToString() == "R-WMDU") //Report - WorkPackage Master Data Update 
                    {
                        ReportWkgMasterData.Visible = true;
                        ReportExists += 1;
                    }

                    if (dr["Code"].ToString() == "R-MFP") //View : Report - Monthly Project Financial Progress
                    {
                        ReportMonthlyFinancialProgress.Visible = true;
                        ReportExists += 1;
                    }

                    if (dr["Code"].ToString() == "R-RD") //View : Report - Resource Deployment
                    {
                        ReportResourceDeployment.Visible = true;
                        ReportExists += 1;
                    }

                    if (dr["Code"].ToString() == "R-I") //View : Report - Issues
                    {
                        ReportIssues.Visible = true;
                        ReportExists += 1;
                    }

                    if (dr["Code"].ToString() == "PM") //View : Profile Management
                    {
                        ProfileManagement.Visible = true;
                    }

                    if (dr["Code"].ToString() == "BOQM") //View : BOQ Menu
                    {
                        BOQ.Visible = true;
                    }

                    if (dr["Code"].ToString() == "M-I") //View : Invoice Menu
                    {
                        Invoice.Visible = true;
                    }

                    if (dr["Code"].ToString() == "M-RA") //View : RA Bill Menu
                    {
                        RAbill.Visible = true;
                    }

                    if (dr["Code"].ToString() == "M-ISS") //View : Issues Menu
                    {
                        Issues.Visible = true;
                    }

                    if (dr["Code"].ToString() == "M-INS") //View : Insurance Menu
                    {
                        Insurance.Visible = true;
                    }
                    // added on 24/09/2021
                    if (dr["Code"].ToString() == "HAD") //View : History of Accessed Documents
                    {
                        AccessedDocsHistory.Visible = true;
                    }
                    if (dr["Code"].ToString() == "MC") //View : Misc Menu camera
                    {
                        Misc_Camera.Visible = true;
                    }
                    if (dr["Code"].ToString() == "MR") //View : Misc Menu Review
                    {
                        Misc_Reviews.Visible = true;
                    }
                    if (dr["Code"].ToString() == "MMB") //View : Misc Menu - Measurement Book
                    {
                        Misc_Measurement.Visible = true;
                    }
                    if (dr["Code"].ToString() == "MRMM") //View : Menu - Review Meeting Master
                    {
                        Misc_Review_Meeting.Visible = true;
                    }
                    if (dr["Code"].ToString() == "MPPP") //View : Menu - Project Physical Progress
                    {
                        Misc_Project_PhysicalPrg.Visible = true;
                    }
                    if (dr["Code"].ToString() == "MPSWP") //View : Menu - Project Status of Work Progress
                    {
                        Misc_Project_Status.Visible = true;
                    }
                    if (dr["Code"].ToString() == "MSP") //View : Menu - Site Photographs
                    {
                        Misc_Site_Photos.Visible = true;
                    }
                    if (dr["Code"].ToString() == "MCJ") //View : Menu - Claims sent to CAAA/JICA
                    {
                        Misc_Claims.Visible = true;
                    }
                    if (dr["Code"].ToString() == "MBD") //View : Menu - Budget vs Disbursement
                    {
                        Misc_Budget_Disb.Visible = true;
                    }
                    if (dr["Code"].ToString() == "MBJ") //View : Menu - BWSSB vs JICA
                    {
                        Misc_Budget_JICA.Visible = true;
                    }
                    if (dr["Code"].ToString() == "MCM") //View : Menu - Compliance of M.O.M
                    {
                        Misc_MOM.Visible = true;
                    }
                    if (dr["Code"].ToString() == "MCA") //View : Menu - Consolidated Activities
                    {
                        Misc_Consolidated.Visible = true;
                    }
                    if (dr["Code"].ToString() == "MSWC") //View : Menu - Status WasteWater ContractPackage
                    {
                        Misc_Status_WasteWater.Visible = true;
                    }
                    if (dr["Code"].ToString() == "MOPD") //View : Menu - Other points for Discussion
                    {
                        Misc_OtherPoints.Visible = true;
                    }
                    if (dr["Code"].ToString() == "RBPA") //View : RA bills Price Adj
                    {
                        RAbillPriceAdj.Visible = true;
                    }
                }

                if (Session["TypeOfUser"].ToString() != "U")
                {
                    if (ReportExists > 0)
                    {
                        ViewReports.Visible = true;
                    }
                    else
                    {
                        ViewReports.Visible = false;
                    }
                }

               
                if (Session["IsContractor"].ToString() == "Y" || Session["TypeOfUser"].ToString() == "U")
                {
                    ReportReconciliationPhase.Visible = true;
                }
                else
                {
                    ReportReconciliationPhase.Visible = false;
                }
                if (WebConfigurationManager.AppSettings["Domain"] == "NJSEI")
                {
                    ReviewReports.Visible = false;
                    ReportConstructionDrawingStatus.Visible = false;
                    ReportProjectPhysicalProgress.Visible = false;
                    ReportProjectStatusofWorkProgress.Visible = false;
                    ReportResourceDeployment.Visible = false;
                    ReportReconciliation.Visible = false;
                    ReportReconciliationStatus.Visible = false;
                    ReportReconciliationPhase.Visible = false;
                }
                //
                if (Session["IsContractor"].ToString() == "Y")
                {
                    ReportReconciliation.Visible = false;
                    ReportReconciliationStatus.Visible = false;
                }
                if(Session["TypeOfUser"].ToString() == "U")
                {
                    navlinkFlowMaster.Visible = true;
                    navlinkDocumentFlowMaster.Visible = true;
                }
                else
                {
                    navlinkFlowMaster.Visible = false;
                    navlinkDocumentFlowMaster.Visible = false;
                }

                //
                if((Session["TypeOfUser"].ToString() == "DDE" || Session["TypeOfUser"].ToString() == "PM") && WebConfigurationManager.AppSettings["Domain"] == "ONTB")
                {
                    ViewReports.Visible = true;
                    ReportReconciliation.Visible = true;
                    ReportReconciliationStatus.Visible = true;
                }
            }
            //
        }
    }
}