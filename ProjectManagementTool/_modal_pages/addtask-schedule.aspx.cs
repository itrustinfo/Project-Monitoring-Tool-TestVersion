using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagementTool._modal_pages
{
    public partial class addtask_schedule : System.Web.UI.Page
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
                        CreateControls();
                    }
                    else
                    {
                        CreateControls_Datewise();
                    }
                    
                    HiddenAction.Value = "Add";
                }
                HideSaveButton();
            }
        }

        internal void HideSaveButton()
        {
            btnSave.Visible = false;
            btnAdd.Visible = false;
            btnRemove.Visible = false;
            btnaddDatewise.Visible = false;
            DataSet dscheck = new DataSet();
            dscheck = getdata.GetUsertypeFunctionality_Mapping(Session["TypeOfUser"].ToString());
            if (dscheck.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dscheck.Tables[0].Rows)
                {
                    if (dr["Code"].ToString() == "ACPA" && HiddenAction.Value !="Update")
                    {
                        btnSave.Visible = true;
                        btnAdd.Visible = true;
                        btnRemove.Visible = true;
                        btnaddDatewise.Visible = true;
                    }

                    if (dr["Code"].ToString() == "ACPE" && HiddenAction.Value == "Update")
                    {
                        btnSave.Visible = true;
                        btnAdd.Visible = true;
                        btnRemove.Visible = true;
                        btnaddDatewise.Visible = true;
                    }
                }
            }
        }

        private void BindTaskSchedule(string TaskUID,float Vesrion,string ScheduleType)
        {
            DataSet ds = getdata.GetTaskSchedule_By_TaskUID_TaskScheduleVersion(new Guid(TaskUID), Vesrion);
            if (ds.Tables[0].Rows.Count > 0)
            {
                int count = 0;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (ScheduleType == "Month")
                    {
                        if (ViewState["count"] != null)
                        {
                            count = (int)ViewState["count"];
                            count++; count++;
                            if (IsPostBack)
                            {
                                count--; count--;
                            }
                            ViewState["count"] = count;
                        }                       
                        string Month = Convert.ToDateTime(ds.Tables[0].Rows[i]["StartDate"].ToString()).Month.ToString();
                        string Year = Convert.ToDateTime(ds.Tables[0].Rows[i]["StartDate"].ToString()).Year.ToString();
                        string Val = Convert.ToDouble(ds.Tables[0].Rows[i]["Schedule_Value"].ToString()).ToString("0.###");

                        CreateControls_with_Data(Month.Length == 1 ? ("0" + Month) : Month, Year, Val);
                    }
                    else
                    {
                        if (ViewState["Datewisecount"] != null)
                        {
                            count = (int)ViewState["Datewisecount"];
                            count++; count++;
                            if (IsPostBack)
                            {
                                count--; count--;
                            }
                            ViewState["Datewisecount"] = count;
                        }

                        string StartDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["StartDate"].ToString()).ToString("dd/MM/yyyy");
                        string EndDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["EndDate"].ToString()).ToString("dd/MM/yyyy");
                        string Val = ds.Tables[0].Rows[i]["Schedule_Value"].ToString();

                        CreateControls_Datewise_with_Data(StartDate, EndDate, Val);
                    }
                }
            }
        }
        protected void btnAdd_Click(object sender, EventArgs e)
        {
         
            int count = 0;
            if (ViewState["count"] != null)
            {
                count = (int)ViewState["count"];
            }
            count++;
            count++;
            ViewState["count"] = count;
            CreateControls();
        }

        protected void CreateControls()
        {
            int count = 0;
            if (ViewState["count"] != null)
            {
                count = (int)ViewState["count"];
            }
            else
            {
                //count = 1;
                ViewState["count"] = count;
            }


            while (PlaceHolder1.Controls.Count <= count)
            {
                

                DropDownList ddlmonth = new DropDownList();
                ddlmonth.ID = "DDLMonth_" + PlaceHolder1.Controls.Count.ToString();
                ddlmonth.CssClass = "form-control";
                ddlmonth.Items.Add(new ListItem("--Select Month--", ""));
                ddlmonth.Items.Add(new ListItem("Jan", "01"));
                ddlmonth.Items.Add(new ListItem("Feb", "02"));
                ddlmonth.Items.Add(new ListItem("Mar", "03"));
                ddlmonth.Items.Add(new ListItem("Apr", "04"));
                ddlmonth.Items.Add(new ListItem("May", "05"));
                ddlmonth.Items.Add(new ListItem("Jun", "06"));
                ddlmonth.Items.Add(new ListItem("Jul", "07"));
                ddlmonth.Items.Add(new ListItem("Aug", "08"));
                ddlmonth.Items.Add(new ListItem("Sep", "09"));
                ddlmonth.Items.Add(new ListItem("Oct", "10"));
                ddlmonth.Items.Add(new ListItem("Nov", "11"));
                ddlmonth.Items.Add(new ListItem("Dec", "12"));
                PlaceHolder1.Controls.Add(ddlmonth);

                RequiredFieldValidator rfv = new RequiredFieldValidator();
                rfv.ID = "DDLMonthValidate_" + PlaceHolder1.Controls.Count.ToString();
                rfv.ErrorMessage = "* Required";
                rfv.ForeColor = Color.Red;
                rfv.ControlToValidate = ddlmonth.ID;
                PlaceHolder1.Controls.Add(rfv);
                
            }

            while (PlaceHolder2.Controls.Count <= count)
            {
                DropDownList ddlyear = new DropDownList();
                ddlyear.ID = "DDLYear_" + PlaceHolder2.Controls.Count.ToString();
                ddlyear.Items.Add(new ListItem("--Select Year--", ""));
                ddlyear.CssClass = "form-control";
                int year = DateTime.Now.Year - 5;
                for (int i = 1; i < 25; i++)
                {
                    year = year + 1;
                    ddlyear.Items.Add(new ListItem(year.ToString(), year.ToString()));
                }
                PlaceHolder2.Controls.Add(ddlyear);

                RequiredFieldValidator rfv = new RequiredFieldValidator();
                rfv.ID = "DDLYearValidate_" + PlaceHolder2.Controls.Count.ToString();
                rfv.ErrorMessage = "* Required";
                rfv.ForeColor = Color.Red;
                rfv.ControlToValidate = ddlyear.ID;
                PlaceHolder2.Controls.Add(rfv);
                //rfv.Validate();
            }

            while (PlaceHolder3.Controls.Count <= count)
            {
                TextBox txtvalue = new TextBox();
                txtvalue.CssClass = "form-control";
                txtvalue.Attributes.Add("placeholder", "Target Value");
                txtvalue.ID = "txt_" + PlaceHolder3.Controls.Count.ToString();
                PlaceHolder3.Controls.Add(txtvalue);

                RequiredFieldValidator rfv = new RequiredFieldValidator();
                rfv.ID="txtValidate_"+ PlaceHolder3.Controls.Count.ToString();
                rfv.ErrorMessage = "* Required";
                
                rfv.ForeColor = Color.Red;
                rfv.ControlToValidate = txtvalue.ID;
                PlaceHolder3.Controls.Add(rfv);
                //rfv.Validate();
            }

            //while (PnlDelete.Controls.Count <= count)
            //{
            //    Button btn = new Button();
            //    btn.ID = "btn_" + PnlDelete.Controls.Count.ToString();
            //    btn.Text = "Delete";
            //    btn.CausesValidation = false;
            //    btn.CssClass = "btn btn-primary";
            //    btn.Click += new EventHandler(btn_Click);
            //    PnlDelete.Controls.Add(btn);

            //    Label lblempty = new Label();
            //    lblempty.ID = "lbl_"+ PnlDelete.Controls.Count.ToString();
            //    lblempty.Text = "";
            //    PnlDelete.Controls.Add(lblempty);
            //}
        }


        protected void btnRemove_Click(object sender, EventArgs e)
        {
            int count = (int)ViewState["count"];
            if (count > 0)
            {
                count--; count--;
            }
            ViewState["count"] = count;
        }


        private void btn_Click(object sender, EventArgs e)
        {
            //Button bt = (Button)sender;

            //string[] btnID = bt.ID.Split('_');
            //PlaceHolder1.Controls.Remove(PlaceHolder1.FindControl("DDLMonth_" + btnID[1]));
            //PlaceHolder2.Controls.Remove(PlaceHolder2.FindControl("DDLYear_" + btnID[1]));
            //PlaceHolder3.Controls.Remove(PlaceHolder3.FindControl("txt_" + btnID[1]));
            //PnlDelete.Controls.Remove(PnlDelete.FindControl(bt.ID));

            int count = (int)ViewState["count"];
            count--;
            ViewState["count"] = count;
            
        }

        protected void CreateControls_with_Data(string Month,string Year,string Value)
        {
            int count = 0;
            if (ViewState["count"] != null)
            {
                count = (int)ViewState["count"];
            }
            else
            {
                //count = 1;
                ViewState["count"] = count;
            }


            while (PlaceHolder1.Controls.Count <= count)
            {
                DropDownList ddlmonth = new DropDownList();
                ddlmonth.ID = "DDLMonth" + PlaceHolder1.Controls.Count.ToString();
                ddlmonth.CssClass = "form-control";
                ddlmonth.Items.Add(new ListItem("--Select Month--", ""));
                ddlmonth.Items.Add(new ListItem("Jan", "01"));
                ddlmonth.Items.Add(new ListItem("Feb", "02"));
                ddlmonth.Items.Add(new ListItem("Mar", "03"));
                ddlmonth.Items.Add(new ListItem("Apr", "04"));
                ddlmonth.Items.Add(new ListItem("May", "05"));
                ddlmonth.Items.Add(new ListItem("Jun", "06"));
                ddlmonth.Items.Add(new ListItem("Jul", "07"));
                ddlmonth.Items.Add(new ListItem("Aug", "08"));
                ddlmonth.Items.Add(new ListItem("Sep", "09"));
                ddlmonth.Items.Add(new ListItem("Oct", "10"));
                ddlmonth.Items.Add(new ListItem("Nov", "11"));
                ddlmonth.Items.Add(new ListItem("Dec", "12"));
                PlaceHolder1.Controls.Add(ddlmonth);

                RequiredFieldValidator rfv = new RequiredFieldValidator();
                rfv.ID = "DDLMonthValidate" + PlaceHolder1.Controls.Count.ToString();
                rfv.ErrorMessage = "* Required";
                rfv.ForeColor = Color.Red;
                rfv.ControlToValidate = ddlmonth.ID;
                PlaceHolder1.Controls.Add(rfv);

                ddlmonth.SelectedValue = Month;
            }

            while (PlaceHolder2.Controls.Count <= count)
            {
                DropDownList ddlyear = new DropDownList();
                ddlyear.ID = "DDLYear" + PlaceHolder2.Controls.Count.ToString();
                ddlyear.Items.Add(new ListItem("--Select Year--", ""));
                ddlyear.CssClass = "form-control";
                int year = DateTime.Now.Year - 5;
                for (int i = 1; i < 25; i++)
                {
                    year = year + 1;
                    ddlyear.Items.Add(new ListItem(year.ToString(), year.ToString()));
                }
                PlaceHolder2.Controls.Add(ddlyear);

                RequiredFieldValidator rfv = new RequiredFieldValidator();
                rfv.ID = "DDLYearValidate" + PlaceHolder2.Controls.Count.ToString();
                rfv.ErrorMessage = "* Required";
                rfv.ForeColor = Color.Red;
                rfv.ControlToValidate = ddlyear.ID;
                PlaceHolder2.Controls.Add(rfv);
                //rfv.Validate();
                ddlyear.SelectedValue = Year;
            }

            while (PlaceHolder3.Controls.Count <= count)
            {
                TextBox txtvalue = new TextBox();
                txtvalue.CssClass = "form-control";
                txtvalue.Attributes.Add("placeholder", "Target Value");
                txtvalue.ID = "txt" + PlaceHolder3.Controls.Count.ToString();                
                PlaceHolder3.Controls.Add(txtvalue);

                RequiredFieldValidator rfv = new RequiredFieldValidator();
                rfv.ID = "txtValidate" + PlaceHolder3.Controls.Count.ToString();
                rfv.ErrorMessage = "* Required";

                rfv.ForeColor = Color.Red;
                rfv.ControlToValidate = txtvalue.ID;
                PlaceHolder3.Controls.Add(rfv);
                //rfv.Validate();
                txtvalue.Text = Value;
            }
        }

        protected void CreateControls_Datewise()
        {
            int count = 0;
            if (ViewState["Datewisecount"] != null)
            {
                count = (int)ViewState["Datewisecount"];
            }
            else
            {
                //count = 1;
                ViewState["Datewisecount"] = count;
            }
            while (PlaceHolder4.Controls.Count <= count)
            {
                TextBox txtstartdate = new TextBox();
                txtstartdate.CssClass = "TheDateTimePicker";
                txtstartdate.Attributes.Add("placeholder","StartDate");
                txtstartdate.ID = "dtStartDate" + PlaceHolder4.Controls.Count.ToString();
                PlaceHolder4.Controls.Add(txtstartdate);

                RequiredFieldValidator rfv = new RequiredFieldValidator();
                rfv.ID = "txtStartDateValidate" + PlaceHolder4.Controls.Count.ToString();
                rfv.ErrorMessage = "* Required";
                rfv.ForeColor = Color.Red;
                rfv.ControlToValidate = txtstartdate.ID;
                PlaceHolder4.Controls.Add(rfv);
            }

            while (PlaceHolder5.Controls.Count <= count)
            {
                TextBox txtenddate = new TextBox();
                txtenddate.CssClass = "TheDateTimePicker";
                txtenddate.Attributes.Add("placeholder", "EndDate");
                txtenddate.ID = "dtEndDate" + PlaceHolder5.Controls.Count.ToString();
                PlaceHolder5.Controls.Add(txtenddate);

                RequiredFieldValidator rfv = new RequiredFieldValidator();
                rfv.ID = "txtEndDateValidate" + PlaceHolder5.Controls.Count.ToString();
                rfv.ErrorMessage = "* Required";
                rfv.ForeColor = Color.Red;
                rfv.ControlToValidate = txtenddate.ID;
                PlaceHolder5.Controls.Add(rfv);
            }

            while (PlaceHolder6.Controls.Count <= count)
            {
                TextBox txtvalue = new TextBox();
                txtvalue.CssClass = "form-control";
                txtvalue.Attributes.Add("placeholder", "Target Value");
                txtvalue.ID = "txtval" + PlaceHolder6.Controls.Count.ToString();
                PlaceHolder6.Controls.Add(txtvalue);

                RequiredFieldValidator rfv = new RequiredFieldValidator();
                rfv.ID = "txtValueValidate" + PlaceHolder6.Controls.Count.ToString();
                rfv.ErrorMessage = "* Required";
                rfv.ForeColor = Color.Red;
                rfv.ControlToValidate = txtvalue.ID;
                PlaceHolder6.Controls.Add(rfv);
            }
        }

        protected void CreateControls_Datewise_with_Data(string StartDate,string EndDate,string Value)
        {
            int count = 0;
            if (ViewState["Datewisecount"] != null)
            {
                count = (int)ViewState["Datewisecount"];
            }
            else
            {
                //count = 1;
                ViewState["Datewisecount"] = count;
            }
            while (PlaceHolder4.Controls.Count <= count)
            {
                TextBox txtstartdate = new TextBox();
                txtstartdate.CssClass = "TheDateTimePicker";
                txtstartdate.Attributes.Add("placeholder", "StartDate");
                txtstartdate.ID = "dtStartDate" + PlaceHolder4.Controls.Count.ToString();
                PlaceHolder4.Controls.Add(txtstartdate);

                RequiredFieldValidator rfv = new RequiredFieldValidator();
                rfv.ID = "txtStartDateValidate" + PlaceHolder4.Controls.Count.ToString();
                rfv.ErrorMessage = "* Required";
                rfv.ForeColor = Color.Red;
                rfv.ControlToValidate = txtstartdate.ID;
                PlaceHolder4.Controls.Add(rfv);

                txtstartdate.Text = StartDate;
            }

            while (PlaceHolder5.Controls.Count <= count)
            {
                TextBox txtenddate = new TextBox();
                txtenddate.CssClass = "TheDateTimePicker";
                txtenddate.Attributes.Add("placeholder", "EndDate");
                txtenddate.ID = "dtEndDate" + PlaceHolder5.Controls.Count.ToString();
                PlaceHolder5.Controls.Add(txtenddate);

                RequiredFieldValidator rfv = new RequiredFieldValidator();
                rfv.ID = "txtEndDateValidate" + PlaceHolder5.Controls.Count.ToString();
                rfv.ErrorMessage = "* Required";
                rfv.ForeColor = Color.Red;
                rfv.ControlToValidate = txtenddate.ID;
                PlaceHolder5.Controls.Add(rfv);
                txtenddate.Text = EndDate;
            }

            while (PlaceHolder6.Controls.Count <= count)
            {
                TextBox txtvalue = new TextBox();
                txtvalue.CssClass = "form-control";
                txtvalue.Attributes.Add("placeholder", "Target Value");
                txtvalue.ID = "txtval" + PlaceHolder6.Controls.Count.ToString();
                PlaceHolder6.Controls.Add(txtvalue);

                RequiredFieldValidator rfv = new RequiredFieldValidator();
                rfv.ID = "txtValueValidate" + PlaceHolder6.Controls.Count.ToString();
                rfv.ErrorMessage = "* Required";
                rfv.ForeColor = Color.Red;
                rfv.ControlToValidate = txtvalue.ID;
                PlaceHolder6.Controls.Add(rfv);
                txtvalue.Text = Value;
            }
        }

        protected void RBScheduleTye_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RBScheduleTye.SelectedValue == "Month")
            {
                ByMonth.Visible = true;
                ByDate.Visible = false;
                CreateControls();
            }
            else
            {
                ByMonth.Visible = false;
                ByDate.Visible = true;
                CreateControls_Datewise();
               
            }
        }

        protected void btnaddDatewise_Click(object sender, EventArgs e)
        {
            int count = 0;
            if (ViewState["Datewisecount"] != null)
            {
                count = (int)ViewState["Datewisecount"];
            }
            count++;
            count++;
            ViewState["Datewisecount"] = count;
            CreateControls_Datewise();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (RBScheduleTye.SelectedValue == "Month")
                {
                    List<string> sMonth = new List<string>();
                    List<string> sYear = new List<string>();
                    List<string> sValue = new List<string>();
                    foreach (DropDownList ddlmonth in PlaceHolder1.Controls.OfType<DropDownList>())
                    {
                        sMonth.Add(ddlmonth.SelectedValue);
                    }

                    foreach (DropDownList ddlyear in PlaceHolder2.Controls.OfType<DropDownList>())
                    {
                        sYear.Add(ddlyear.SelectedValue);
                    }

                    foreach (TextBox txtval in PlaceHolder3.Controls.OfType<TextBox>())
                    {
                        sValue.Add(txtval.Text);
                    }
                    if (HiddenAction.Value == "Add")
                    {
                        bool ver = getdata.InsertorUpdateTaskScheduleVesrion(Guid.NewGuid(), new Guid(Request.QueryString["TaskUID"]), "Month");
                        if (ver)
                        {
                            for (int i = 0; i < sMonth.Count; i++)
                            {
                                string sDate1 = "", sDate2 = "";
                                DateTime CDate1 = DateTime.Now, CDate2 = DateTime.Now;
                                sDate1 = "01/" + sMonth[i].ToString() + "/" + sYear[i].ToString();
                                sDate1 = getdata.ConvertDateFormat(sDate1);
                                CDate1 = Convert.ToDateTime(sDate1);

                                int Days = DateTime.DaysInMonth(Convert.ToInt32(sYear[i]), Convert.ToInt32(sMonth[i])) - 1;
                                CDate2 = CDate1.AddDays(Days);

                                float sVal = float.Parse(sValue[i].ToString());
                                //message += sMonth[i].ToString() + "/" + sYear[i].ToString() + "-" + sValue[i].ToString();

                                bool sresult = getdata.InsertorUpdateTaskSchedule(Guid.NewGuid(), new Guid(Request.QueryString["WorkUID"]), new Guid(Request.QueryString["TaskUID"]), CDate1, CDate2, sVal, "Month");
                                if (sresult)
                                {

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
                    else
                    {
                        string confirmValue = Request.Form["confirm_value"];
                        if (confirmValue == "Yes")
                        {
                            bool ver = getdata.InsertorUpdateTaskScheduleVesrion(Guid.NewGuid(), new Guid(Request.QueryString["TaskUID"]), "Month");
                            if (ver)
                            {
                                for (int i = 0; i < sMonth.Count; i++)
                                {
                                    string sDate1 = "", sDate2 = "";
                                    DateTime CDate1 = DateTime.Now, CDate2 = DateTime.Now;
                                    sDate1 = "01/" + sMonth[i].ToString() + "/" + sYear[i].ToString();
                                    sDate1 = getdata.ConvertDateFormat(sDate1);
                                    CDate1 = Convert.ToDateTime(sDate1);

                                    int Days = DateTime.DaysInMonth(Convert.ToInt32(sYear[i]), Convert.ToInt32(sMonth[i])) - 1;
                                    CDate2 = CDate1.AddDays(Days);

                                    float sVal = float.Parse(sValue[i].ToString());
                                    //message += sMonth[i].ToString() + "/" + sYear[i].ToString() + "-" + sValue[i].ToString();

                                    bool sresult = getdata.InsertorUpdateTaskSchedule(Guid.NewGuid(), new Guid(Request.QueryString["WorkUID"]), new Guid(Request.QueryString["TaskUID"]), CDate1, CDate2, sVal, "Month");
                                    if (sresult)
                                    {

                                    }
                                }
                                Session["SelectedActivity"] = Request.QueryString["TaskUID"].ToString();
                                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
                            }
                        }
                        else
                        {
                            int cnt = getdata.TaskSchedule_Delete_by_TaskUID(new Guid(Request.QueryString["TaskUID"]));
                            if (cnt > 0)
                            {
                                for (int i = 0; i < sMonth.Count; i++)
                                {
                                    string sDate1 = "", sDate2 = "";
                                    DateTime CDate1 = DateTime.Now, CDate2 = DateTime.Now;
                                    sDate1 = "01/" + sMonth[i].ToString() + "/" + sYear[i].ToString();
                                    sDate1 = getdata.ConvertDateFormat(sDate1);
                                    CDate1 = Convert.ToDateTime(sDate1);

                                    int Days = DateTime.DaysInMonth(Convert.ToInt32(sYear[i]), Convert.ToInt32(sMonth[i])) - 1;
                                    CDate2 = CDate1.AddDays(Days);

                                    float sVal = float.Parse(sValue[i].ToString());
                                    //message += sMonth[i].ToString() + "/" + sYear[i].ToString() + "-" + sValue[i].ToString();

                                    bool sresult = getdata.InsertorUpdateTaskSchedule(Guid.NewGuid(), new Guid(Request.QueryString["WorkUID"]), new Guid(Request.QueryString["TaskUID"]), CDate1, CDate2, sVal, "Month");
                                    if (sresult)
                                    {

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
                    List<string> sStartDate = new List<string>();
                    List<string> sEndDate = new List<string>();
                    List<string> sValue = new List<string>();

                    foreach (TextBox dtStartDate in PlaceHolder4.Controls.OfType<TextBox>())
                    {
                        sStartDate.Add(dtStartDate.Text);
                    }

                    foreach (TextBox dtEndDate in PlaceHolder5.Controls.OfType<TextBox>())
                    {
                        sEndDate.Add(dtEndDate.Text);
                    }

                    foreach (TextBox txtval in PlaceHolder6.Controls.OfType<TextBox>())
                    {
                        sValue.Add(txtval.Text);
                    }

                    if (HiddenAction.Value == "Add")
                    {
                        bool ver = getdata.InsertorUpdateTaskScheduleVesrion(Guid.NewGuid(), new Guid(Request.QueryString["TaskUID"]), "Date");
                        if (ver)
                        {
                            for (int i = 0; i < sStartDate.Count; i++)
                            {
                                string sDate1 = "", sDate2 = "";
                                DateTime CDate1 = DateTime.Now, CDate2 = DateTime.Now;

                                sDate1 = sStartDate[i].ToString();
                                sDate1 = getdata.ConvertDateFormat(sDate1);
                                CDate1 = Convert.ToDateTime(sDate1);

                                sDate2 = sEndDate[i].ToString();
                                sDate2 = getdata.ConvertDateFormat(sDate2);
                                CDate2 = Convert.ToDateTime(sDate2);

                                float sVal = float.Parse(sValue[i].ToString());

                                bool sresult = getdata.InsertorUpdateTaskSchedule(Guid.NewGuid(), new Guid(Request.QueryString["WorkUID"]), new Guid(Request.QueryString["TaskUID"]), CDate1, CDate2, sVal, "Date");
                                if (sresult)
                                {

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
                    else
                    {
                        string confirmValue = Request.Form["confirm_value"];
                        if (confirmValue == "Yes")
                        {
                            bool ver = getdata.InsertorUpdateTaskScheduleVesrion(Guid.NewGuid(), new Guid(Request.QueryString["TaskUID"]), "Date");
                            if (ver)
                            {
                                for (int i = 0; i < sStartDate.Count; i++)
                                {
                                    string sDate1 = "", sDate2 = "";
                                    DateTime CDate1 = DateTime.Now, CDate2 = DateTime.Now;

                                    sDate1 = sStartDate[i].ToString();
                                    sDate1 = getdata.ConvertDateFormat(sDate1);
                                    CDate1 = Convert.ToDateTime(sDate1);

                                    sDate2 = sEndDate[i].ToString();
                                    sDate2 = getdata.ConvertDateFormat(sDate2);
                                    CDate2 = Convert.ToDateTime(sDate2);

                                    float sVal = float.Parse(sValue[i].ToString());

                                    bool sresult = getdata.InsertorUpdateTaskSchedule(Guid.NewGuid(), new Guid(Request.QueryString["WorkUID"]), new Guid(Request.QueryString["TaskUID"]), CDate1, CDate2, sVal, "Date");
                                    if (sresult)
                                    {

                                    }
                                }
                                Session["SelectedActivity"] = Request.QueryString["TaskUID"].ToString();
                                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
                            }
                                
                        }
                        else
                        {
                            int cnt = getdata.TaskSchedule_Delete_by_TaskUID(new Guid(Request.QueryString["TaskUID"]));
                            if (cnt > 0)
                            {
                                for (int i = 0; i < sStartDate.Count; i++)
                                {
                                    string sDate1 = "", sDate2 = "";
                                    DateTime CDate1 = DateTime.Now, CDate2 = DateTime.Now;

                                    sDate1 = sStartDate[i].ToString();
                                    sDate1 = getdata.ConvertDateFormat(sDate1);
                                    CDate1 = Convert.ToDateTime(sDate1);

                                    sDate2 = sEndDate[i].ToString();
                                    sDate2 = getdata.ConvertDateFormat(sDate2);
                                    CDate2 = Convert.ToDateTime(sDate2);

                                    float sVal = float.Parse(sValue[i].ToString());

                                    bool sresult = getdata.InsertorUpdateTaskSchedule(Guid.NewGuid(), new Guid(Request.QueryString["WorkUID"]), new Guid(Request.QueryString["TaskUID"]), CDate1, CDate2, sVal, "Date");
                                    if (sresult)
                                    {

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
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code ATS-01 there is a problem with this feature. Please contact system admin. Description : " + ex.Message + "');</script>");
            }
        }

       
    }
}