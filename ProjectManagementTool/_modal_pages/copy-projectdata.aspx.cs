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
    public partial class copy_projectdata : System.Web.UI.Page
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
                    if (Request.QueryString["ProjectUID"] != null)
                    {
                        txtprojectname.Text = getdt.getProjectNameby_ProjectUID(new Guid(Request.QueryString["ProjectUID"]));
                        BindProject(Request.QueryString["ProjectUID"]);
                    }
                }
            }
        }
        private void BindProject(string ProjectUID)
        {
            DataSet ds = new DataSet();
            if (Session["TypeOfUser"].ToString() == "U" || Session["TypeOfUser"].ToString() =="MD" || Session["TypeOfUser"].ToString() == "VP")
            {
                ds = gettk.GetAllProjects_Expect_Selected(new Guid(ProjectUID));
            }
            else if (Session["TypeOfUser"].ToString() == "PA")
            {
                //ds = gettk.GetProjects_by_UserUID(new Guid(Session["UserUID"].ToString()));
                ds = gettk.GetAssignedProject_by_UserUID_Except_Selected(new Guid(Session["UserUID"].ToString()), new Guid(ProjectUID));
            }
            else
            {
                //ds = gettk.GetProjects();
                ds = gettk.GetAssignedProject_by_UserUID_Except_Selected(new Guid(Session["UserUID"].ToString()), new Guid(ProjectUID));
            }
            DDLProject.DataTextField = "ProjectName";
            DDLProject.DataValueField = "ProjectUID";
            DDLProject.DataSource = ds;
            DDLProject.DataBind();
        }
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                DataSet ds = getdt.GetWorkPackages_By_ProjectUID(new Guid(DDLProject.SelectedValue));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "Progress Bar", "ShowProgressBar('true')", true);
                    for (int w = 0; w < ds.Tables[0].Rows.Count; w++)
                    {
                        DataSet dsSelected = getdt.GetWorkPackages_By_WorkPackageUID(new Guid(ds.Tables[0].Rows[w]["WorkPackageUID"].ToString()));
                        Guid WorkpackageUID = Guid.NewGuid();
                        int cnt = getdt.InsertorUpdateWorkPackage(WorkpackageUID, new Guid(Request.QueryString["ProjectUID"]),new Guid(ds.Tables[0].Rows[w]["Workpackage_OptionUID"].ToString()), new Guid(ds.Tables[0].Rows[w]["Contractor_UID"].ToString()), ds.Tables[0].Rows[w]["Name"].ToString(), ds.Tables[0].Rows[w]["WorkPackage_Location"].ToString(), ds.Tables[0].Rows[w]["WorkPackage_Client"].ToString(),
                            Convert.ToDateTime(ds.Tables[0].Rows[w]["StartDate"].ToString()), Convert.ToDateTime(ds.Tables[0].Rows[w]["PlannedEndDate"].ToString()), Convert.ToDateTime(ds.Tables[0].Rows[w]["ProjectedEndDate"].ToString()), ds.Tables[0].Rows[w]["Status"].ToString(), Convert.ToDouble(ds.Tables[0].Rows[w]["Budget"].ToString()), Convert.ToDouble(ds.Tables[0].Rows[w]["ActualExpenditure"].ToString()), ds.Tables[0].Rows[w]["Currency"].ToString(), ds.Tables[0].Rows[w]["Currency_CultureInfo"].ToString());
                        if (cnt > 0)
                        {
                            DataSet dscopyworkpackage = getdt.GetWorkPackages_By_WorkPackageUID(WorkpackageUID);
                            DataSet dstask = getdt.GetAllTasks_by_WorkPackagesUID(new Guid(ds.Tables[0].Rows[w]["WorkPackageUID"].ToString()));
                            if (dstask.Tables[0].Rows.Count > 0)
                            {
                                for (int i = 0; i < dstask.Tables[0].Rows.Count; i++)
                                {
                                    Guid sTaskUID = Guid.NewGuid();
                                    int TaskLevel = Convert.ToInt32(dstask.Tables[0].Rows[i]["TaskLevel"].ToString());
                                    string Currecncy_CultureInfo = dstask.Tables[0].Rows[i]["Currency_CultureInfo"].ToString();

                                    bool result = getdt.InsertorUpdateMainTask_CopyData(sTaskUID, WorkpackageUID, new Guid(Request.QueryString["ProjectUID"]), dstask.Tables[0].Rows[i]["Owner"].ToString(), dstask.Tables[0].Rows[i]["Name"].ToString(), dstask.Tables[0].Rows[i]["Description"].ToString(), "", "", Convert.ToDateTime(dstask.Tables[0].Rows[i]["StartDate"].ToString()), Convert.ToDateTime(dstask.Tables[0].Rows[i]["PlannedEndDate"].ToString()), Convert.ToDateTime(dstask.Tables[0].Rows[i]["ProjectedEndDate"].ToString()), Convert.ToDateTime(dstask.Tables[0].Rows[i]["PlannedStartDate"].ToString()), Convert.ToDateTime(dstask.Tables[0].Rows[i]["ProjectedStartDate"].ToString()), Convert.ToDateTime(dstask.Tables[0].Rows[i]["ActualEndDate"].ToString()),
                                        dstask.Tables[0].Rows[i]["Status"].ToString(), Convert.ToDouble(dstask.Tables[0].Rows[i]["Basic_Budget"].ToString()), Convert.ToDouble(dstask.Tables[0].Rows[i]["ActualExpenditure"].ToString()), "", TaskLevel, Convert.ToDouble(dstask.Tables[0].Rows[i]["GST"].ToString()), Convert.ToDouble(dstask.Tables[0].Rows[i]["Total_Budget"].ToString()), Convert.ToDouble(0), dstask.Tables[0].Rows[i]["Discipline"].ToString(), "", dstask.Tables[0].Rows[i]["Currency"].ToString(), Currecncy_CultureInfo, dstask.Tables[0].Rows[i]["Task_Order"].ToString() != "" ? dstask.Tables[0].Rows[i]["Task_Order"].ToString() : "0");
                                    if (result == true)
                                    {
                                        TaskDataInsert(dsSelected, dscopyworkpackage, new Guid(dstask.Tables[0].Rows[i]["TaskUID"].ToString()), sTaskUID);
                                    }
                                    else
                                    {
                                        Page.ClientScript.RegisterStartupScript(this.GetType(), "Progress Bar", "ShowProgressBar('false')", true);
                                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code CAD-01 there is a problem with this feature. Please contact system admin.');</script>");

                                    }
                                }
                            }
                        }
                        //DataSet dsworkpackage = getdt.GetWorkPackages_By_WorkPackageUID(new Guid(ds.Tables[0].Rows[w]["WorkPackageUID"].ToString()));
                        //if (dsworkpackage.Tables[0].Rows.Count > 0)
                        //{

                        //}
                    }
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('No Data Found.');</script>");
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code CPD-01 there is a problem with this feature. Please contact system admin.');</script>");
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Progress Bar", "ShowProgressBar('false')", true);
            }
        }

        private void TaskDataInsert(DataSet dsFrom, DataSet dsTo, Guid FromParentTaskUID, Guid ToParentTaskUID)
        {
            DataSet ds = getdt.GetTask_by_ParentTaskUID(FromParentTaskUID);
            if (ds.Tables[0].Rows.Count > 0)
            {
                int StartDateDiff = 0, EndDateDiff = 0, PlannedStartDateDiff = 0, PlannedEndDateDiff = 0,
                        ProjectedStartDateDiff = 0, ProjectedEndDateDiff = 0;
                string sDate1 = "", sDate2 = "", sDate3 = "", sDate4 = "", sDate5 = "", sDate6 = "";
                DateTime CDate1 = DateTime.Now, CDate2 = DateTime.Now, CDate3 = DateTime.Now, CDate4 = DateTime.Now, CDate5 = DateTime.Now, CDate6 = DateTime.Now;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Guid sSubTaskUID = Guid.NewGuid();

                    StartDateDiff = (Convert.ToDateTime(dsFrom.Tables[0].Rows[0]["StartDate"].ToString()) - Convert.ToDateTime(ds.Tables[0].Rows[i]["StartDate"].ToString())).Days;
                    EndDateDiff = (Convert.ToDateTime(dsFrom.Tables[0].Rows[0]["PlannedEndDate"].ToString()) - Convert.ToDateTime(ds.Tables[0].Rows[i]["ActualEndDate"].ToString())).Days;
                    PlannedStartDateDiff = (Convert.ToDateTime(dsFrom.Tables[0].Rows[0]["StartDate"].ToString()) - Convert.ToDateTime(ds.Tables[0].Rows[i]["PlannedStartDate"].ToString())).Days;
                    PlannedEndDateDiff = (Convert.ToDateTime(dsFrom.Tables[0].Rows[0]["PlannedEndDate"].ToString()) - Convert.ToDateTime(ds.Tables[0].Rows[i]["PlannedEndDate"].ToString())).Days;
                    ProjectedStartDateDiff = (Convert.ToDateTime(dsFrom.Tables[0].Rows[0]["StartDate"].ToString()) - Convert.ToDateTime(ds.Tables[0].Rows[i]["ProjectedStartDate"].ToString())).Days;
                    ProjectedEndDateDiff = (Convert.ToDateTime(dsFrom.Tables[0].Rows[0]["ProjectedEndDate"].ToString()) - Convert.ToDateTime(ds.Tables[0].Rows[i]["ProjectedEndDate"].ToString())).Days;
                    //
                    if (dsTo.Tables[0].Rows[0]["StartDate"].ToString() != "")
                    {
                        //sDate1 = Convert.ToDateTime(dsTo.Tables[0].Rows[0]["StartDate"].ToString()).AddDays(StartDateDiff).ToString("dd/MM/yyyy");
                        sDate1 = Convert.ToDateTime(ds.Tables[0].Rows[i]["StartDate"].ToString()).ToString("dd/MM/yyyy",CultureInfo.InvariantCulture);
                        //sDate1 = sDate1.Split('/')[1] + "/" + sDate1.Split('/')[0] + "/" + sDate1.Split('/')[2];
                        sDate1 = getdt.ConvertDateFormat(sDate1);
                        CDate1 = Convert.ToDateTime(sDate1);
                    }
                    //
                    if (dsTo.Tables[0].Rows[0]["PlannedEndDate"].ToString() != "")
                    {
                        //sDate2 = Convert.ToDateTime(dsTo.Tables[0].Rows[0]["PlannedEndDate"].ToString()).AddDays(PlannedEndDateDiff).ToString("dd/MM/yyyy");
                        sDate2 = Convert.ToDateTime(ds.Tables[0].Rows[i]["PlannedEndDate"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                        //sDate2 = sDate2.Split('/')[1] + "/" + sDate2.Split('/')[0] + "/" + sDate2.Split('/')[2];
                        sDate2 = getdt.ConvertDateFormat(sDate2);
                        CDate2 = Convert.ToDateTime(sDate2);
                    }
                    //
                    if (dsTo.Tables[0].Rows[0]["ProjectedEndDate"].ToString() != "")
                    {
                        //sDate3 = Convert.ToDateTime(dsTo.Tables[0].Rows[0]["ProjectedEndDate"].ToString()).AddDays(ProjectedEndDateDiff).ToString("dd/MM/yyyy");
                        sDate3 = Convert.ToDateTime(ds.Tables[0].Rows[i]["ProjectedEndDate"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                        //sDate3 = sDate3.Split('/')[1] + "/" + sDate3.Split('/')[0] + "/" + sDate3.Split('/')[2];
                        sDate3 = getdt.ConvertDateFormat(sDate3);
                        CDate3 = Convert.ToDateTime(sDate3);
                    }

                    // Actual EndDate
                    if (dsTo.Tables[0].Rows[0]["PlannedEndDate"].ToString() != "")
                    {
                        //sDate4 = Convert.ToDateTime(dsTo.Tables[0].Rows[0]["PlannedEndDate"].ToString()).AddDays(EndDateDiff).ToString("dd/MM/yyyy");
                        sDate4 = Convert.ToDateTime(ds.Tables[0].Rows[i]["ActualEndDate"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                        //sDate4 = sDate4.Split('/')[1] + "/" + sDate4.Split('/')[0] + "/" + sDate4.Split('/')[2];
                        sDate4 = getdt.ConvertDateFormat(sDate4);
                        CDate4 = Convert.ToDateTime(sDate4);
                    }

                    // Planned StartDate
                    if (dsTo.Tables[0].Rows[0]["StartDate"].ToString() != "")
                    {
                        //sDate5 = Convert.ToDateTime(dsTo.Tables[0].Rows[0]["StartDate"].ToString()).AddDays(PlannedStartDateDiff).ToString("dd/MM/yyyy");
                        sDate5 = Convert.ToDateTime(ds.Tables[0].Rows[i]["PlannedStartDate"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                        //sDate5 = sDate5.Split('/')[1] + "/" + sDate5.Split('/')[0] + "/" + sDate5.Split('/')[2];
                        sDate5 = getdt.ConvertDateFormat(sDate5);
                        CDate5 = Convert.ToDateTime(sDate5);
                    }

                    // Projected StartDate
                    if (dsTo.Tables[0].Rows[0]["StartDate"].ToString() != "")
                    {
                        //sDate6 = Convert.ToDateTime(dsTo.Tables[0].Rows[0]["StartDate"].ToString()).AddDays(ProjectedStartDateDiff).ToString("dd/MM/yyyy");
                        sDate6 = Convert.ToDateTime(ds.Tables[0].Rows[i]["ProjectedStartDate"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                        //sDate6 = sDate6.Split('/')[1] + "/" + sDate6.Split('/')[0] + "/" + sDate6.Split('/')[2];
                        sDate6 = getdt.ConvertDateFormat(sDate6);
                        CDate6 = Convert.ToDateTime(sDate6);
                    }

                    bool result1 = getdt.InsertorUpdateSubTask_CopyData(sSubTaskUID, new Guid(dsTo.Tables[0].Rows[0]["WorkPackageUID"].ToString()), new Guid(dsTo.Tables[0].Rows[0]["ProjectUID"].ToString()), ds.Tables[0].Rows[i]["Owner"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), "", "", CDate1, CDate2, CDate3, CDate5, CDate6, CDate4,
                    ds.Tables[0].Rows[i]["Status"].ToString(), Convert.ToDouble(ds.Tables[0].Rows[i]["Basic_Budget"].ToString()), Convert.ToDouble(ds.Tables[0].Rows[i]["ActualExpenditure"].ToString()), "", Convert.ToInt32(ds.Tables[0].Rows[i]["TaskLevel"].ToString()), ToParentTaskUID.ToString(), Convert.ToDouble(ds.Tables[0].Rows[i]["GST"].ToString()), Convert.ToDouble(ds.Tables[0].Rows[i]["Total_Budget"].ToString()), Convert.ToDouble(0), ds.Tables[0].Rows[i]["Discipline"].ToString(), "", ds.Tables[0].Rows[i]["Currency"].ToString(), ds.Tables[0].Rows[i]["Currency_CultureInfo"].ToString(), ds.Tables[0].Rows[i]["Task_Order"].ToString() != "" ? ds.Tables[0].Rows[i]["Task_Order"].ToString() : "0");
                    if (result1 == true)
                    {
                        TaskDataInsert(dsFrom, dsTo, new Guid(ds.Tables[0].Rows[i]["TaskUID"].ToString()), sSubTaskUID);
                    }
                }
            }
        }
    }
}