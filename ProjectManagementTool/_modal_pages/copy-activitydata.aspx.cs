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
    public partial class copy_activitydata : System.Web.UI.Page
    {
        DBGetData getdt = new DBGetData();
        TaskUpdate gettk = new TaskUpdate();
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
                    if (Request.QueryString["WorkPackage"] != null)
                    {
                        BindProject();
                        txtWorkpackagename.Text = getdt.getWorkPackageNameby_WorkPackageUID(new Guid(Request.QueryString["WorkPackage"]));
                        //BindWorkpackage();
                        DDLProject_SelectedIndexChanged(sender, e);
                    }
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
            DDLProject.DataTextField = "ProjectName";
            DDLProject.DataValueField = "ProjectUID";
            DDLProject.DataSource = ds;
            DDLProject.DataBind();
        }

        protected void DDLProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDLProject.Items.Count > 0)
            {
                BindWorkpackage(); 
            }
        }

        private void BindWorkpackage()
        {
            DataSet ds = getdt.GetWorkPackages_By_ProjectUID_witout_Selected(new Guid(DDLProject.SelectedValue), new Guid(Request.QueryString["WorkPackage"]));
            //DataSet ds = new DataSet();
            //if (Session["TypeOfUser"].ToString() == "U" || Session["TypeOfUser"].ToString() =="MD" || Session["TypeOfUser"].ToString() == "VP")
            //{
            //    ds = getdt.GetAllWorkPackages();
            //}
            //else if (Session["TypeOfUser"].ToString() == "PA")
            //{
            //    ds = getdt.GetWorkPackages_by_UserUID_Without_Selected(new Guid(Session["UserUID"].ToString()),new Guid(Request.QueryString["WorkPackage"]));
            //}
            //else
            //{
            //    ds = getdt.GetWorkPackages_by_UserUID_Without_Selected(new Guid(Session["UserUID"].ToString()), new Guid(Request.QueryString["WorkPackage"]));
            //}
            DDLWorkpackage.DataTextField = "Name";
            DDLWorkpackage.DataValueField = "WorkPackageUID";
            DDLWorkpackage.DataSource = ds;
            DDLWorkpackage.DataBind();
        }
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                //DataSet ds = getdt.GetAllTasks_by_WorkPackagesUID(new Guid(DDLWorkpackage.SelectedValue));
                DataSet ds = getdt.GetAllTasks_by_WorkPackagesUID(new Guid(DDLWorkpackage.SelectedValue));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "Progress Bar", "ShowProgressBar('true')", true);

                    DataSet dscopyworkpackage = getdt.GetWorkPackages_By_WorkPackageUID(new Guid(Request.QueryString["WorkPackage"]));
                    if (dscopyworkpackage.Tables[0].Rows.Count > 0)
                    {
                        int wOptionInsert = getdt.WorkpackageSelectedOptions_Insert(Guid.NewGuid(), new Guid(Request.QueryString["WorkPackage"]), new Guid(ds.Tables[0].Rows[0]["Workpackage_Option"].ToString()));
                        if (wOptionInsert > 0)
                        {
                            //DateTime StartDate = dscopyworkpackage.Tables[0].Rows[0]["StartDate"].ToString() != "" ? Convert.ToDateTime(dscopyworkpackage.Tables[0].Rows[0]["StartDate"].ToString()) : DateTime.Now;
                            //DateTime PlannedEndDate = dscopyworkpackage.Tables[0].Rows[0]["PlannedEndDate"].ToString() != "" ? Convert.ToDateTime(dscopyworkpackage.Tables[0].Rows[0]["PlannedEndDate"].ToString()) : DateTime.Now;
                            //DateTime ProjectedEndDate = dscopyworkpackage.Tables[0].Rows[0]["ProjectedEndDate"].ToString() != "" ? Convert.ToDateTime(dscopyworkpackage.Tables[0].Rows[0]["ProjectedEndDate"].ToString()) : DateTime.Now;

                            //int StartDateDiff = 0, EndDateDiff = 0, PlannedStartDateDiff = 0, PlannedEndDateDiff = 0,
                            //    ProjectedStartDateDiff = 0, ProjectedEndDateDiff = 0;
                            DataSet dsSelected = getdt.GetWorkPackages_By_WorkPackageUID(new Guid(DDLWorkpackage.SelectedValue));

                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                //StartDateDiff = (Convert.ToDateTime(dsSelected.Tables[0].Rows[0]["StartDate"].ToString()) - Convert.ToDateTime(ds.Tables[0].Rows[i]["StartDate"].ToString())).Days;
                                //EndDateDiff = (Convert.ToDateTime(dsSelected.Tables[0].Rows[0]["PlannedEndDate"].ToString()) - Convert.ToDateTime(ds.Tables[0].Rows[i]["ActualEndDate"].ToString())).Days;
                                //PlannedStartDateDiff = (Convert.ToDateTime(dsSelected.Tables[0].Rows[0]["StartDate"].ToString()) - Convert.ToDateTime(ds.Tables[0].Rows[i]["PlannedStartDate"].ToString())).Days;
                                //PlannedEndDateDiff = (Convert.ToDateTime(dsSelected.Tables[0].Rows[0]["PlannedEndDate"].ToString()) - Convert.ToDateTime(ds.Tables[0].Rows[i]["PlannedEndDate"].ToString())).Days;
                                //ProjectedStartDateDiff = (Convert.ToDateTime(dsSelected.Tables[0].Rows[0]["StartDate"].ToString()) - Convert.ToDateTime(ds.Tables[0].Rows[i]["ProjectedStartDate"].ToString())).Days;
                                //ProjectedEndDateDiff = (Convert.ToDateTime(dsSelected.Tables[0].Rows[0]["ProjectedEndDate"].ToString()) - Convert.ToDateTime(ds.Tables[0].Rows[i]["ProjectedEndDate"].ToString())).Days;

                                Guid sTaskUID = Guid.NewGuid();
                                int TaskLevel = Convert.ToInt32(ds.Tables[0].Rows[i]["TaskLevel"].ToString());

                                string sDate1 = "", sDate2 = "", sDate3 = "", sDate4 = "", sDate5 = "", sDate6 = "";
                                DateTime CDate1 = new DateTime(), CDate2 = new DateTime(), CDate3 = new DateTime(), CDate4 = new DateTime(), CDate5 = new DateTime(), CDate6 = new DateTime();
                                //
                                if (ds.Tables[0].Rows[0]["StartDate"].ToString() != "")
                                {
                                    //sDate1 = Convert.ToDateTime(dscopyworkpackage.Tables[0].Rows[0]["StartDate"].ToString()).AddDays(StartDateDiff).ToString("dd/MM/yyyy");
                                    sDate1 = Convert.ToDateTime(ds.Tables[0].Rows[i]["StartDate"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                                    //sDate1 = sDate1.Split('/')[1] + "/" + sDate1.Split('/')[0] + "/" + sDate1.Split('/')[2];
                                    sDate1 = getdt.ConvertDateFormat(sDate1);
                                    CDate1 = Convert.ToDateTime(sDate1);
                                }
                                //
                                if (ds.Tables[0].Rows[0]["PlannedEndDate"].ToString() != "")
                                {
                                    //sDate2 = Convert.ToDateTime(dscopyworkpackage.Tables[0].Rows[0]["PlannedEndDate"].ToString()).AddDays(PlannedEndDateDiff).ToString("dd/MM/yyyy");
                                    sDate2 = Convert.ToDateTime(ds.Tables[0].Rows[i]["PlannedEndDate"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                                    //sDate2 = sDate2.Split('/')[1] + "/" + sDate2.Split('/')[0] + "/" + sDate2.Split('/')[2];
                                    sDate2 = getdt.ConvertDateFormat(sDate2);
                                    CDate2 = Convert.ToDateTime(sDate2);
                                }
                                //
                                if (ds.Tables[0].Rows[0]["ProjectedEndDate"].ToString() != "")
                                {
                                    //sDate3 = Convert.ToDateTime(dscopyworkpackage.Tables[0].Rows[0]["ProjectedEndDate"].ToString()).AddDays(ProjectedEndDateDiff).ToString("dd/MM/yyyy");
                                    sDate3 = Convert.ToDateTime(ds.Tables[0].Rows[i]["ProjectedEndDate"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                                    //sDate3 = sDate3.Split('/')[1] + "/" + sDate3.Split('/')[0] + "/" + sDate3.Split('/')[2];
                                    sDate3 = getdt.ConvertDateFormat(sDate3);
                                    CDate3 = Convert.ToDateTime(sDate3);
                                }

                                // Actual EndDate
                                if (ds.Tables[0].Rows[0]["ActualEndDate"].ToString() != "")
                                {
                                    //sDate4 = Convert.ToDateTime(dscopyworkpackage.Tables[0].Rows[0]["PlannedEndDate"].ToString()).AddDays(EndDateDiff).ToString("dd/MM/yyyy");
                                    sDate4 = Convert.ToDateTime(ds.Tables[0].Rows[i]["ActualEndDate"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                                    //sDate4 = sDate4.Split('/')[1] + "/" + sDate4.Split('/')[0] + "/" + sDate4.Split('/')[2];
                                    sDate4 = getdt.ConvertDateFormat(sDate4);
                                    CDate4 = Convert.ToDateTime(sDate4);
                                }

                                // Planned StartDate
                                if (ds.Tables[0].Rows[0]["PlannedStartDate"].ToString() != "")
                                {
                                    //sDate5 = Convert.ToDateTime(dscopyworkpackage.Tables[0].Rows[0]["StartDate"].ToString()).AddDays(PlannedStartDateDiff).ToString("dd/MM/yyyy");
                                    sDate5 = Convert.ToDateTime(ds.Tables[0].Rows[i]["PlannedStartDate"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                                    //sDate5 = sDate5.Split('/')[1] + "/" + sDate5.Split('/')[0] + "/" + sDate5.Split('/')[2];
                                    sDate5 = getdt.ConvertDateFormat(sDate5);
                                    CDate5 = Convert.ToDateTime(sDate5);
                                }

                                // Projected StartDate
                                if (ds.Tables[0].Rows[0]["ProjectedStartDate"].ToString() != "")
                                {
                                    //sDate6 = Convert.ToDateTime(dscopyworkpackage.Tables[0].Rows[0]["StartDate"].ToString()).AddDays(ProjectedStartDateDiff).ToString("dd/MM/yyyy");
                                    sDate6 = Convert.ToDateTime(ds.Tables[0].Rows[i]["ProjectedStartDate"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                                    //sDate6 = sDate6.Split('/')[1] + "/" + sDate6.Split('/')[0] + "/" + sDate6.Split('/')[2];
                                    sDate6 = getdt.ConvertDateFormat(sDate6);
                                    CDate6 = Convert.ToDateTime(sDate6);
                                }

                                //if (string.IsNullOrEmpty(txtStatusPer.Text))
                                //{
                                //    txtStatusPer.Text = "0";
                                //}
                                string Currecncy_CultureInfo = ds.Tables[0].Rows[i]["Currency_CultureInfo"].ToString();

                                bool result = getdt.InsertorUpdateMainTask_CopyData(sTaskUID, new Guid(Request.QueryString["WorkPackage"]), new Guid(dscopyworkpackage.Tables[0].Rows[0]["ProjectUID"].ToString()), ds.Tables[0].Rows[i]["Owner"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), "", "", CDate1 == DateTime.MinValue ? "" : CDate1.ToString(), CDate2 == DateTime.MinValue ? "" : CDate2.ToString(), CDate3 == DateTime.MinValue ? "" : CDate3.ToString(), CDate5 == DateTime.MinValue ? "" : CDate5.ToString(), CDate6 == DateTime.MinValue ? "" : CDate6.ToString(), CDate4 == DateTime.MinValue ? "" : CDate4.ToString(),
                                    ds.Tables[0].Rows[i]["Status"].ToString(), Convert.ToDouble(ds.Tables[0].Rows[i]["Basic_Budget"].ToString()), Convert.ToDouble(ds.Tables[0].Rows[i]["ActualExpenditure"].ToString()), "", TaskLevel, Convert.ToDouble(ds.Tables[0].Rows[i]["GST"].ToString()), Convert.ToDouble(ds.Tables[0].Rows[i]["Total_Budget"].ToString()), Convert.ToDouble(0), ds.Tables[0].Rows[i]["Discipline"].ToString(), "", ds.Tables[0].Rows[i]["Currency"].ToString(), Currecncy_CultureInfo, ds.Tables[0].Rows[i]["Task_Order"].ToString() != "" ? ds.Tables[0].Rows[i]["Task_Order"].ToString() : "0",!string.IsNullOrEmpty(ds.Tables[0].Rows[i]["Task_Weightage"].ToString()) ? float.Parse(ds.Tables[0].Rows[i]["Task_Weightage"].ToString()) : 0,
                                    ds.Tables[0].Rows[i]["Task_Type"].ToString(),new Guid(ds.Tables[0].Rows[i]["Workpackage_Option"].ToString()), !string.IsNullOrEmpty(ds.Tables[0].Rows[i]["UnitQuantity"].ToString()) ? double.Parse(ds.Tables[0].Rows[i]["UnitQuantity"].ToString()) : 0, ds.Tables[0].Rows[i]["BOQDetailsUID"].ToString(), ds.Tables[0].Rows[i]["GroupBOQItems"].ToString());
                                if (result == true)
                                {
                                    TaskDataInsert(dsSelected, dscopyworkpackage, new Guid(ds.Tables[0].Rows[i]["TaskUID"].ToString()), sTaskUID);
                                }
                                else
                                {
                                    Page.ClientScript.RegisterStartupScript(this.GetType(), "Progress Bar", "ShowProgressBar('false')", true);
                                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code CAD-01 there is a problem with this feature. Please contact system admin.');</script>");

                                }
                            }

                            //Update Workpackge Option
                            int updateUID = getdt.WorkpackageOptionUID_Update(new Guid(Request.QueryString["WorkPackage"]), new Guid(dsSelected.Tables[0].Rows[0]["Workpackage_OptionUID"].ToString())); 
                            Session["SelectedActivity"] = Request.QueryString["WorkPackage"].ToString();
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
                        }
                        
                    }
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code CAD-01 there is a problem with this feature. Please contact system admin.');</script>");
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Progress Bar", "ShowProgressBar('false')", true);
            }
        }

        private void TaskDataInsert(DataSet dsFrom, DataSet dsTo,Guid FromParentTaskUID, Guid ToParentTaskUID)
        {
            
            DataSet ds = getdt.GetTask_by_ParentTaskUID(FromParentTaskUID);
            if (ds.Tables[0].Rows.Count > 0)
            {
                //int StartDateDiff = 0, EndDateDiff = 0, PlannedStartDateDiff = 0, PlannedEndDateDiff = 0,
                //        ProjectedStartDateDiff = 0, ProjectedEndDateDiff = 0;
                string sDate1 = "", sDate2 = "", sDate3 = "", sDate4 = "", sDate5 = "", sDate6 = "";
                DateTime CDate1 = new DateTime(), CDate2 = new DateTime(), CDate3 = new DateTime(), CDate4 = new DateTime(), CDate5 = new DateTime(), CDate6 = new DateTime();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Guid sSubTaskUID = Guid.NewGuid();

                    //StartDateDiff = (Convert.ToDateTime(dsFrom.Tables[0].Rows[0]["StartDate"].ToString()) - Convert.ToDateTime(ds.Tables[0].Rows[i]["StartDate"].ToString())).Days;
                    //EndDateDiff = (Convert.ToDateTime(dsFrom.Tables[0].Rows[0]["PlannedEndDate"].ToString()) - Convert.ToDateTime(ds.Tables[0].Rows[i]["ActualEndDate"].ToString())).Days;
                    //PlannedStartDateDiff = (Convert.ToDateTime(dsFrom.Tables[0].Rows[0]["StartDate"].ToString()) - Convert.ToDateTime(ds.Tables[0].Rows[i]["PlannedStartDate"].ToString())).Days;
                    //PlannedEndDateDiff = (Convert.ToDateTime(dsFrom.Tables[0].Rows[0]["PlannedEndDate"].ToString()) - Convert.ToDateTime(ds.Tables[0].Rows[i]["PlannedEndDate"].ToString())).Days;
                    //ProjectedStartDateDiff = (Convert.ToDateTime(dsFrom.Tables[0].Rows[0]["StartDate"].ToString()) - Convert.ToDateTime(ds.Tables[0].Rows[i]["ProjectedStartDate"].ToString())).Days;
                    //ProjectedEndDateDiff = (Convert.ToDateTime(dsFrom.Tables[0].Rows[0]["ProjectedEndDate"].ToString()) - Convert.ToDateTime(ds.Tables[0].Rows[i]["ProjectedEndDate"].ToString())).Days;
                    //
                    if (ds.Tables[0].Rows[0]["StartDate"].ToString() != "")
                    {
                        //sDate1 = Convert.ToDateTime(dsTo.Tables[0].Rows[0]["StartDate"].ToString()).AddDays(StartDateDiff).ToString("dd/MM/yyyy");
                        sDate1 = Convert.ToDateTime(ds.Tables[0].Rows[i]["StartDate"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                        //sDate1 = sDate1.Split('/')[1] + "/" + sDate1.Split('/')[0] + "/" + sDate1.Split('/')[2];
                        sDate1 = getdt.ConvertDateFormat(sDate1);
                        CDate1 = Convert.ToDateTime(sDate1);
                    }
                    //
                    if (ds.Tables[0].Rows[0]["PlannedEndDate"].ToString() != "")
                    {
                        //sDate2 = Convert.ToDateTime(dsTo.Tables[0].Rows[0]["PlannedEndDate"].ToString()).AddDays(PlannedEndDateDiff).ToString("dd/MM/yyyy");
                        sDate2 = Convert.ToDateTime(ds.Tables[0].Rows[i]["PlannedEndDate"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                        //sDate2 = sDate2.Split('/')[1] + "/" + sDate2.Split('/')[0] + "/" + sDate2.Split('/')[2];
                        sDate2 = getdt.ConvertDateFormat(sDate2);
                        CDate2 = Convert.ToDateTime(sDate2);
                    }
                    //
                    if (ds.Tables[0].Rows[0]["ProjectedEndDate"].ToString() != "")
                    {
                        //sDate3 = Convert.ToDateTime(dsTo.Tables[0].Rows[0]["ProjectedEndDate"].ToString()).AddDays(ProjectedEndDateDiff).ToString("dd/MM/yyyy");
                        sDate3 = Convert.ToDateTime(ds.Tables[0].Rows[i]["ProjectedEndDate"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                        //sDate3 = sDate3.Split('/')[1] + "/" + sDate3.Split('/')[0] + "/" + sDate3.Split('/')[2];
                        sDate3 = getdt.ConvertDateFormat(sDate3);
                        CDate3 = Convert.ToDateTime(sDate3);
                    }

                    // Actual EndDate
                    if (ds.Tables[0].Rows[0]["ActualEndDate"].ToString() != "")
                    {
                        //sDate4 = Convert.ToDateTime(dsTo.Tables[0].Rows[0]["PlannedEndDate"].ToString()).AddDays(EndDateDiff).ToString("dd/MM/yyyy");
                        sDate4 = Convert.ToDateTime(ds.Tables[0].Rows[i]["ActualEndDate"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                        //sDate4 = sDate4.Split('/')[1] + "/" + sDate4.Split('/')[0] + "/" + sDate4.Split('/')[2];
                        sDate4 = getdt.ConvertDateFormat(sDate4);
                        CDate4 = Convert.ToDateTime(sDate4);
                    }

                    // Planned StartDate
                    if (ds.Tables[0].Rows[0]["PlannedStartDate"].ToString() != "")
                    {
                        //sDate5 = Convert.ToDateTime(dsTo.Tables[0].Rows[0]["StartDate"].ToString()).AddDays(PlannedStartDateDiff).ToString("dd/MM/yyyy");
                        sDate5 = Convert.ToDateTime(ds.Tables[0].Rows[i]["PlannedStartDate"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                        //sDate5 = sDate5.Split('/')[1] + "/" + sDate5.Split('/')[0] + "/" + sDate5.Split('/')[2];
                        sDate5 = getdt.ConvertDateFormat(sDate5);
                        CDate5 = Convert.ToDateTime(sDate5);
                    }

                    // Projected StartDate
                    if (ds.Tables[0].Rows[0]["ProjectedStartDate"].ToString() != "")
                    {
                        //sDate6 = Convert.ToDateTime(dsTo.Tables[0].Rows[0]["StartDate"].ToString()).AddDays(ProjectedStartDateDiff).ToString("dd/MM/yyyy");
                        sDate6 = Convert.ToDateTime(ds.Tables[0].Rows[i]["ProjectedStartDate"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                        //sDate6 = sDate6.Split('/')[1] + "/" + sDate6.Split('/')[0] + "/" + sDate6.Split('/')[2];
                        sDate6 = getdt.ConvertDateFormat(sDate6);
                        CDate6 = Convert.ToDateTime(sDate6);
                    }

                    bool result1 = getdt.InsertorUpdateSubTask_CopyData(sSubTaskUID, new Guid(Request.QueryString["WorkPackage"]), new Guid(dsTo.Tables[0].Rows[0]["ProjectUID"].ToString()), ds.Tables[0].Rows[i]["Owner"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), "", "", CDate1 == DateTime.MinValue ? "" : CDate1.ToString(), CDate2 == DateTime.MinValue ? "" : CDate2.ToString(), CDate3 == DateTime.MinValue ? "" : CDate3.ToString(), CDate5 == DateTime.MinValue ? "" : CDate5.ToString(), CDate6 == DateTime.MinValue ? "" : CDate6.ToString(), CDate4 == DateTime.MinValue ? "" : CDate4.ToString(),
                    ds.Tables[0].Rows[i]["Status"].ToString(), Convert.ToDouble(ds.Tables[0].Rows[i]["Basic_Budget"].ToString()), Convert.ToDouble(ds.Tables[0].Rows[i]["ActualExpenditure"].ToString()), "", Convert.ToInt32(ds.Tables[0].Rows[i]["TaskLevel"].ToString()), ToParentTaskUID.ToString(), Convert.ToDouble(ds.Tables[0].Rows[i]["GST"].ToString()), Convert.ToDouble(ds.Tables[0].Rows[i]["Total_Budget"].ToString()), Convert.ToDouble(0), ds.Tables[0].Rows[i]["Discipline"].ToString(), "", ds.Tables[0].Rows[i]["Currency"].ToString(), ds.Tables[0].Rows[i]["Currency_CultureInfo"].ToString(), ds.Tables[0].Rows[i]["Task_Order"].ToString() != "" ? ds.Tables[0].Rows[i]["Task_Order"].ToString() : "0",
                    !string.IsNullOrEmpty(ds.Tables[0].Rows[i]["Task_Weightage"].ToString()) ? float.Parse(ds.Tables[0].Rows[i]["Task_Weightage"].ToString()) : 0, ds.Tables[0].Rows[i]["Task_Type"].ToString(), new Guid(ds.Tables[0].Rows[i]["Workpackage_Option"].ToString()), !string.IsNullOrEmpty(ds.Tables[0].Rows[i]["UnitQuantity"].ToString()) ? double.Parse(ds.Tables[0].Rows[i]["UnitQuantity"].ToString()) : 0, ds.Tables[0].Rows[i]["BOQDetailsUID"].ToString(), ds.Tables[0].Rows[i]["GroupBOQItems"].ToString());
                    if (result1 == true)
                    {
                        TaskDataInsert(dsFrom, dsTo, new Guid(ds.Tables[0].Rows[i]["TaskUID"].ToString()), sSubTaskUID);
                    }
                }
            }
        }

        
    }
}