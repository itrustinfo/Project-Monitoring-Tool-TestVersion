using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProjectManager.DAL;

namespace ProjectManagementTool._modal_pages
{
    public partial class addtask_targetschedule : System.Web.UI.Page
    {
        DBGetData getdata = new DBGetData();
        TaskUpdate TKUpdate = new TaskUpdate();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Username"] == null)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
            }
            else
            {
                if (hidSourceID.Value == "")
                {
                    DataSet ds = getdata.GetLatest_TaskScheduleVesrion_By_TaskUID(new Guid(Request.QueryString["TaskUID"]));
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        RBScheduleTye.Enabled = false;
                        HiddenAction.Value = "Update";
                        RBScheduleTye.SelectedValue = ds.Tables[0].Rows[0]["TaskScheduleType"].ToString();
                        if (RBScheduleTye.SelectedValue == "Date")
                        {
                            ByDate.Visible = true;
                            ByMonth.Visible = false;
                        }
                        BindTaskSchedule(ds.Tables[0].Rows[0]["TaskUID"].ToString(), float.Parse(ds.Tables[0].Rows[0]["TaskScheduleVersion"].ToString()), ds.Tables[0].Rows[0]["TaskScheduleType"].ToString());
                    }
                    else
                    {
                        if (RBScheduleTye.SelectedValue == "Month")
                        {
                            AddAndRemoveDynamicControls_Month();
                        }
                        else
                        {
                            AddAndRemoveDynamicControls_Date();
                        }

                        HiddenAction.Value = "Add";
                    }
                }
                else
                {
                    if (RBScheduleTye.SelectedValue == "Month")
                    {
                        AddAndRemoveDynamicControls_Month();
                    }
                    else
                    {
                        AddAndRemoveDynamicControls_Date();
                    }

                    HiddenAction.Value = "Add";
                }
                HideSaveButton();
            }
        }

        private void HideSaveButton()
        {
            btnSave.Visible = false;
            btnAdd.Visible = false;
            btnAddbyDate.Visible = false;
            DataSet dscheck = new DataSet();
            dscheck = getdata.GetUsertypeFunctionality_Mapping(Session["TypeOfUser"].ToString());
            if (dscheck.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dscheck.Tables[0].Rows)
                {
                    if (dr["Code"].ToString() == "ACPA" && HiddenAction.Value != "Update")
                    {
                        btnSave.Visible = true;
                        btnAdd.Visible = true;
                        btnAddbyDate.Visible = true;
                    }

                    if (dr["Code"].ToString() == "ACPE" && HiddenAction.Value == "Update")
                    {
                        btnSave.Visible = true;
                        btnAdd.Visible = true;
                        btnAddbyDate.Visible = true;
                    }
                }
            }
        }

        private void BindYear(DropDownList DDLYear)
        {
            DDLYear.Items.Clear();
            int year = DateTime.Now.Year - 5;
            DDLYear.Items.Add(new ListItem("--Select Year--", ""));
            for (int i = 1; i < 10; i++)
            {
                year = year + 1;
                DDLYear.Items.Add(new ListItem(year.ToString(), year.ToString()));
            }
        }
        protected void RBScheduleTye_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RBScheduleTye.SelectedValue == "Month")
            {
                ByMonth.Visible = true;
                ByDate.Visible = false;
            }
            else
            {
                ByMonth.Visible = false;
                ByDate.Visible = true;
            }
        }

        private void BindTaskSchedule(string TaskUID, float Vesrion, string ScheduleType)
        {
            try
            {
                DataSet ds = getdata.GetTaskSchedule_By_TaskUID_TaskScheduleVersion(new Guid(TaskUID), Vesrion);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (!IsPostBack)
                    {
                        ltlCount.Text = ds.Tables[0].Rows.Count.ToString();
                    }

                    ph1.Controls.Clear();
                    int ControlID = 0;

                    for (int i = 0; i < (Convert.ToInt16(ltlCount.Text)); i++)
                    {
                        string Month = string.Empty;
                        string Year = string.Empty;
                        string Val = string.Empty;
                        if (ScheduleType == "Month")
                        {
                            Month = Convert.ToDateTime(ds.Tables[0].Rows[i]["StartDate"].ToString()).Month.ToString();
                            Year = Convert.ToDateTime(ds.Tables[0].Rows[i]["StartDate"].ToString()).Year.ToString();
                            Val = Convert.ToDouble(ds.Tables[0].Rows[i]["Schedule_Value"].ToString()).ToString("0.##");


                            TaskScheduleUserControl DynamicUserControl11 = (TaskScheduleUserControl)LoadControl("TaskScheduleUserControl.ascx");
                            while (InDeletedList("uc" + ControlID) == true)
                            {
                                ControlID += 1;
                            }

                            DynamicUserControl11.ID = "uc" + ControlID;

                            DynamicUserControl11.RemoveUserControl += this.HandleRemoveUserControl;

                            DropDownList ddlYear = DynamicUserControl11.FindControl("DDLYear") as DropDownList;
                            BindYear(ddlYear);
                            DropDownList ddlMonth = DynamicUserControl11.FindControl("DDLMonth") as DropDownList;
                            TextBox txtTarget = DynamicUserControl11.FindControl("txtTraget") as TextBox;
                            ddlYear.SelectedValue = Year;
                            ddlMonth.SelectedValue = Month.Length == 1 ? ("0" + Month) : Month;
                            txtTarget.Text = Val;
                            ph1.Controls.Add(DynamicUserControl11);
                            ControlID += 1;
                        }
                        else
                        {
                            string StartDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["StartDate"].ToString()).ToString("dd/MM/yyyy");
                            string EndDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["EndDate"].ToString()).ToString("dd/MM/yyyy");
                            Val = Convert.ToDouble(ds.Tables[0].Rows[i]["Schedule_Value"].ToString()).ToString("0.##");

                            TaskScheduleUserControlDatewise DynamicUserControlDatewise = (TaskScheduleUserControlDatewise)LoadControl("TaskScheduleUserControlDatewise.ascx");
                            while (InDeletedList("uc" + ControlID) == true)
                            {
                                ControlID += 1;
                            }

                            DynamicUserControlDatewise.ID = "uc" + ControlID;

                            DynamicUserControlDatewise.RemoveUserControlDatewise += this.HandleRemoveUserControl;

                            TextBox dtStart = DynamicUserControlDatewise.FindControl("dtStartDate") as TextBox;
                            TextBox dtEnd = DynamicUserControlDatewise.FindControl("dtEndDate") as TextBox;
                            TextBox txtTarget = DynamicUserControlDatewise.FindControl("txtTarget") as TextBox;

                            dtStart.Text = StartDate;
                            dtEnd.Text = EndDate;
                            txtTarget.Text = Val;

                            ph2.Controls.Add(DynamicUserControlDatewise);
                            ControlID += 1;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code ATS-01 there is a problem with this feature. Please contact system admin.');</script>");
            }

        }


        private void AddAndRemoveDynamicControls_Month()
        {
            Control c = GetPostBackControl(Page);

            if ((c != null))
            {
                if (c.ID.ToString() == "btnAdd")
                {
                    ltlCount.Text = (Convert.ToInt16(ltlCount.Text) + 1).ToString();
                }
            }

            ph1.Controls.Clear();
            int ControlID = 0;
            for (int i = 0; i <= (Convert.ToInt16(ltlCount.Text) - 1); i++)
            {
                try
                {
                    TaskScheduleUserControl DynamicUserControl11 = (TaskScheduleUserControl)LoadControl("TaskScheduleUserControl.ascx");
                    while (InDeletedList("uc" + ControlID) == true)
                    {
                        ControlID += 1;
                    }
                    DropDownList ddlYear = DynamicUserControl11.FindControl("DDLYear") as DropDownList;
                    BindYear(ddlYear);
                    DynamicUserControl11.ID = "uc" + ControlID;

                    DynamicUserControl11.RemoveUserControl += this.HandleRemoveUserControl;
                    ph1.Controls.Add(DynamicUserControl11);
                    ControlID += 1;
                }
                catch (Exception ex)
                {

                }
            }
        }

        private void AddAndRemoveDynamicControls_Date()
        {
            Control c = GetPostBackControl(Page);

            if ((c != null))
            {
                if (c.ID.ToString() == "btnAddbyDate")
                {
                    ltlCount.Text = (Convert.ToInt16(ltlCount.Text) + 1).ToString();
                }
            }

            ph2.Controls.Clear();
            int ControlID = 0;
            for (int i = 0; i <= (Convert.ToInt16(ltlCount.Text) - 1); i++)
            {
                try
                {
                    TaskScheduleUserControlDatewise DynamicUserControlDatewise = (TaskScheduleUserControlDatewise)LoadControl("TaskScheduleUserControlDatewise.ascx");
                    while (InDeletedList("uc" + ControlID) == true)
                    {
                        ControlID += 1;
                    }

                    DynamicUserControlDatewise.ID = "uc" + ControlID;

                    DynamicUserControlDatewise.RemoveUserControlDatewise += this.HandleRemoveUserControl;
                    ph2.Controls.Add(DynamicUserControlDatewise);
                    ControlID += 1;
                }
                catch (Exception ex)
                {

                }
            }
        }

        private bool InDeletedList(string ControlID)
        {
            string[] DeletedList = ltlRemoved.Text.Split('|');
            for (int i = 0; i <= DeletedList.GetLength(0) - 1; i++)
            {
                if (ControlID.ToLower() == DeletedList[i].ToLower())
                {
                    return true;
                }
            }
            return false;
        }

        public void HandleRemoveUserControl(object sender, EventArgs e)
        {
            Button remove = (sender as Button);
            if (RBScheduleTye.SelectedValue == "Month")
            {
                TaskScheduleUserControl DynamicUserControl = (TaskScheduleUserControl)remove.Parent;
                ph1.Controls.Remove((TaskScheduleUserControl)remove.Parent);
                ltlRemoved.Text += DynamicUserControl.ID + "|";
                ltlCount.Text = (Convert.ToInt16(ltlCount.Text) - 1).ToString();
            }
            else
            {
                TaskScheduleUserControlDatewise DynamicUserControl = (TaskScheduleUserControlDatewise)remove.Parent;
                ph2.Controls.Remove((TaskScheduleUserControlDatewise)remove.Parent);
                ltlRemoved.Text += DynamicUserControl.ID + "|";
                ltlCount.Text = (Convert.ToInt16(ltlCount.Text) - 1).ToString();
            }

        }

        protected void btnAdd_Click(object sender, System.EventArgs e)
        {
            //Handled in page load
        }

        //Find the control that caused the postback.
        public Control GetPostBackControl(Page page)
        {
            Control control = null;

            string ctrlname = page.Request.Params.Get("__EVENTTARGET");
            if ((ctrlname != null) & ctrlname != string.Empty)
            {
                control = page.FindControl(ctrlname);
            }
            else
            {
                foreach (string ctl in page.Request.Form)
                {
                    Control c = page.FindControl(ctl);
                    if (c is System.Web.UI.WebControls.Button)
                    {
                        control = c;
                        break;
                    }
                }
            }
            return control;
        }
        private void ScheduleInsertbyMonth()
        {
            bool ver = getdata.InsertorUpdateTaskScheduleVesrion(Guid.NewGuid(), new Guid(Request.QueryString["TaskUID"]), "Month");
            if (ver)
            {
                foreach (Control c in ph1.Controls)
                {
                    if (c.GetType().Name.ToLower() == "_modal_pages_taskscheduleusercontrol_ascx")
                    {
                        TaskScheduleUserControl uc = (TaskScheduleUserControl)c;
                        DropDownList ddlYear = uc.FindControl("DDLYear") as DropDownList;
                        DropDownList ddlMonth = uc.FindControl("DDLMonth") as DropDownList;
                        TextBox txtTarget = uc.FindControl("txtTraget") as TextBox;

                        string sDate1 = "";
                        DateTime CDate1 = DateTime.Now, CDate2 = DateTime.Now;
                        sDate1 = "01/" + ddlMonth.SelectedValue + "/" + ddlYear.SelectedValue;
                        sDate1 = getdata.ConvertDateFormat(sDate1);
                        CDate1 = Convert.ToDateTime(sDate1);

                        int Days = DateTime.DaysInMonth(Convert.ToInt32(ddlYear.SelectedValue), Convert.ToInt32(ddlMonth.SelectedValue)) - 1;
                        CDate2 = CDate1.AddDays(Days);

                        float sVal = float.Parse(txtTarget.Text);

                        getdata.InsertorUpdateTaskSchedule(Guid.NewGuid(), new Guid(Request.QueryString["WorkUID"]), new Guid(Request.QueryString["TaskUID"]), CDate1, CDate2, sVal, "Month");
                    }
                }
                Session["SelectedActivity"] = Request.QueryString["TaskUID"].ToString();
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code ATS-01 there is a problem with this feature. Please contact system admin.');</script>");
            }
        }

        private void ScheduleInsertbyDate()
        {
            bool ver = getdata.InsertorUpdateTaskScheduleVesrion(Guid.NewGuid(), new Guid(Request.QueryString["TaskUID"]), "Date");
            if (ver)
            {
                foreach (Control c in ph2.Controls)
                {
                    if (c.GetType().Name.ToLower() == "_modal_pages_taskscheduleusercontroldatewise_ascx")
                    {
                        TaskScheduleUserControlDatewise uc = (TaskScheduleUserControlDatewise)c;

                        TextBox dtStart = uc.FindControl("dtStartDate") as TextBox;
                        TextBox dtEnd = uc.FindControl("dtEndDate") as TextBox;
                        TextBox txtTarget = uc.FindControl("txtTarget") as TextBox;

                        string sDate1 = "", sDate2 = "";
                        DateTime CDate1 = DateTime.Now, CDate2 = DateTime.Now;

                        sDate1 = dtStart.Text;
                        sDate1 = getdata.ConvertDateFormat(sDate1);
                        CDate1 = Convert.ToDateTime(sDate1);

                        sDate2 = dtEnd.Text;
                        sDate2 = getdata.ConvertDateFormat(sDate2);
                        CDate2 = Convert.ToDateTime(sDate2);

                        float sVal = float.Parse(txtTarget.Text);

                        getdata.InsertorUpdateTaskSchedule(Guid.NewGuid(), new Guid(Request.QueryString["WorkUID"]), new Guid(Request.QueryString["TaskUID"]), CDate1, CDate2, sVal, "Date");
                    }
                }
                Session["SelectedActivity"] = Request.QueryString["TaskUID"].ToString();
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code ATS-01 there is a problem with this feature. Please contact system admin.');</script>");
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (RBScheduleTye.SelectedValue == "Month")
                {
                    if (HiddenAction.Value == "Add")
                    {
                        ScheduleInsertbyMonth();
                    }
                    else
                    {
                        string confirmValue = Request.Form["confirm_value"];
                        if (confirmValue == "Yes")
                        {
                            ScheduleInsertbyMonth();
                        }
                        else
                        {
                            int cnt = getdata.TaskSchedule_Delete_by_TaskUID(new Guid(Request.QueryString["TaskUID"]));
                            if (cnt > 0)
                            {
                                foreach (Control c in ph1.Controls)
                                {
                                    if (c.GetType().Name.ToLower() == "_modal_pages_taskscheduleusercontrol_ascx")
                                    {
                                        TaskScheduleUserControl uc = (TaskScheduleUserControl)c;
                                        DropDownList ddlYear = uc.FindControl("DDLYear") as DropDownList;
                                        DropDownList ddlMonth = uc.FindControl("DDLMonth") as DropDownList;
                                        TextBox txtTarget = uc.FindControl("txtTraget") as TextBox;

                                        string sDate1 = "";
                                        DateTime CDate1 = DateTime.Now, CDate2 = DateTime.Now;
                                        sDate1 = "01/" + ddlMonth.SelectedValue + "/" + ddlYear.SelectedValue;
                                        sDate1 = getdata.ConvertDateFormat(sDate1);
                                        CDate1 = Convert.ToDateTime(sDate1);

                                        int Days = DateTime.DaysInMonth(Convert.ToInt32(ddlYear.SelectedValue), Convert.ToInt32(ddlMonth.SelectedValue)) - 1;
                                        CDate2 = CDate1.AddDays(Days);

                                        float sVal = float.Parse(txtTarget.Text);

                                        getdata.InsertorUpdateTaskSchedule(Guid.NewGuid(), new Guid(Request.QueryString["WorkUID"]), new Guid(Request.QueryString["TaskUID"]), CDate1, CDate2, sVal, "Month");
                                    }
                                }
                                Session["SelectedActivity"] = Request.QueryString["TaskUID"].ToString();
                                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
                            }
                        }
                    }
                }
                else
                {
                    if (HiddenAction.Value == "Add")
                    {
                        ScheduleInsertbyDate();
                    }
                    else
                    {
                        string confirmValue = Request.Form["confirm_value"];
                        if (confirmValue == "Yes")
                        {
                            ScheduleInsertbyDate();
                        }
                        else
                        {
                            int cnt = getdata.TaskSchedule_Delete_by_TaskUID(new Guid(Request.QueryString["TaskUID"]));
                            if (cnt > 0)
                            {
                                foreach (Control c in ph2.Controls)
                                {
                                    if (c.GetType().Name.ToLower() == "_modal_pages_taskscheduleusercontroldatewise_ascx")
                                    {
                                        TaskScheduleUserControlDatewise uc = (TaskScheduleUserControlDatewise)c;

                                        TextBox dtStart = uc.FindControl("dtStartDate") as TextBox;
                                        TextBox dtEnd = uc.FindControl("dtEndDate") as TextBox;
                                        TextBox txtTarget = uc.FindControl("txtTarget") as TextBox;

                                        string sDate1 = "", sDate2 = "";
                                        DateTime CDate1 = DateTime.Now, CDate2 = DateTime.Now;

                                        sDate1 = dtStart.Text;
                                        sDate1 = getdata.ConvertDateFormat(sDate1);
                                        CDate1 = Convert.ToDateTime(sDate1);

                                        sDate2 = dtEnd.Text;
                                        sDate2 = getdata.ConvertDateFormat(sDate2);
                                        CDate2 = Convert.ToDateTime(sDate2);

                                        float sVal = float.Parse(txtTarget.Text);

                                        getdata.InsertorUpdateTaskSchedule(Guid.NewGuid(), new Guid(Request.QueryString["WorkUID"]), new Guid(Request.QueryString["TaskUID"]), CDate1, CDate2, sVal, "Date");
                                    }
                                }
                                Session["SelectedActivity"] = Request.QueryString["TaskUID"].ToString();
                                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code ATS-01 there is a problem with this feature. Please contact system admin. Description :" + ex.Message + "');</script>");
            }
        }
    }
}