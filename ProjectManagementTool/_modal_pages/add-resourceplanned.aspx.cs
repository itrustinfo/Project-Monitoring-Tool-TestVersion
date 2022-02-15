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
    public partial class add_resourceplanned : System.Web.UI.Page
    {
        DBGetData getdata = new DBGetData();
        TaskUpdate TKUpdate = new TaskUpdate();
        public int StartYear = 0;
        public int EndYear = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Username"] == null)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
            }
            else
            {
                DataSet dsyear = getdata.GetWorkPackages_By_WorkPackageUID(new Guid(Request.QueryString["WorkpackageUID"]));
                if (dsyear.Tables[0].Rows.Count > 0)
                {
                    if (dsyear.Tables[0].Rows[0]["StartDate"].ToString() != "" && dsyear.Tables[0].Rows[0]["PlannedEndDate"].ToString() != "")
                    {
                        StartYear = Convert.ToDateTime(dsyear.Tables[0].Rows[0]["StartDate"].ToString()).Year;
                        EndYear = Convert.ToDateTime(dsyear.Tables[0].Rows[0]["PlannedEndDate"].ToString()).Year;
                    }
                }
                DataSet ds = getdata.GetResourecDeployment_by_ResourceUID(new Guid(Request.QueryString["ResourceUID"]));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    RBScheduleTye.Enabled = false;
                    HiddenAction.Value = "Update";
                    ByDate.Visible = false;
                    ByMonth.Visible = true;
                    BindResourceDeployment(ds, RBScheduleTye.SelectedValue);
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
            }
        }

        protected void BindResourceDeployment(DataSet ds, string ScheduleType)
        {
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
                        string Val = Convert.ToDouble(ds.Tables[0].Rows[i]["Planned"].ToString()).ToString("0.###");

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
                        string Val = ds.Tables[0].Rows[i]["Planned"].ToString();

                        CreateControls_Datewise_with_Data(StartDate, EndDate, Val);
                    }
                }
            }
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
                ddlmonth.Items.Insert(0,new ListItem("--Select Month--", ""));
                ddlmonth.Items.Insert(1,new ListItem("Jan", "01"));
                ddlmonth.Items.Insert(2,new ListItem("Feb", "02"));
                ddlmonth.Items.Insert(3,new ListItem("Mar", "03"));
                ddlmonth.Items.Insert(4,new ListItem("Apr", "04"));
                ddlmonth.Items.Insert(5,new ListItem("May", "05"));
                ddlmonth.Items.Insert(6,new ListItem("Jun", "06"));
                ddlmonth.Items.Insert(7,new ListItem("Jul", "07"));
                ddlmonth.Items.Insert(8,new ListItem("Aug", "08"));
                ddlmonth.Items.Insert(9,new ListItem("Sep", "09"));
                ddlmonth.Items.Insert(10,new ListItem("Oct", "10"));
                ddlmonth.Items.Insert(11,new ListItem("Nov", "11"));
                ddlmonth.Items.Insert(12,new ListItem("Dec", "12"));
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

                if (StartYear > 0 && EndYear > 0)
                {
                    for (int i = StartYear; i <= EndYear; i++)
                    {
                        ddlyear.Items.Add(new ListItem(i.ToString(), i.ToString()));
                    }
                }
                else
                {
                    int year = DateTime.Now.Year - 5;
                    for (int i = 1; i < 7; i++)
                    {
                        year = year + 1;
                        ddlyear.Items.Add(new ListItem(year.ToString(), year.ToString()));
                    }
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
                txtvalue.Attributes.Add("placeholder", "Planned Value");
                txtvalue.ID = "txt_" + PlaceHolder3.Controls.Count.ToString();
                PlaceHolder3.Controls.Add(txtvalue);

                RequiredFieldValidator rfv = new RequiredFieldValidator();
                rfv.ID = "txtValidate_" + PlaceHolder3.Controls.Count.ToString();
                rfv.ErrorMessage = "* Required";

                rfv.ForeColor = Color.Red;
                rfv.ControlToValidate = txtvalue.ID;
                PlaceHolder3.Controls.Add(rfv);
                //rfv.Validate();
            }

        }

        protected void CreateControls_with_Data(string Month, string Year, string Value)
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
                if (StartYear > 0 && EndYear > 0)
                {
                    for (int i = StartYear; i <= EndYear; i++)
                    {
                        ddlyear.Items.Add(new ListItem(i.ToString(), i.ToString()));
                    }
                }
                else
                {
                    int year = DateTime.Now.Year - 5;
                    for (int i = 1; i < 7; i++)
                    {
                        year = year + 1;
                        ddlyear.Items.Add(new ListItem(year.ToString(), year.ToString()));
                    }
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
                txtvalue.Attributes.Add("placeholder", "Planned Value");
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
                txtstartdate.Attributes.Add("placeholder", "StartDate");
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
                txtvalue.Attributes.Add("placeholder", "Planned Value");
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

        protected void CreateControls_Datewise_with_Data(string StartDate, string EndDate, string Value)
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
                txtvalue.Attributes.Add("placeholder", "Planned Value");
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
            int count = (int)ViewState["count"];
            count--;
            ViewState["count"] = count;

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

                            int sresult = getdata.InsertorUpdateResourceDeploymentPlanned(Guid.NewGuid(), new Guid(Request.QueryString["WorkpackageUID"]), new Guid(Request.QueryString["ResourceUID"]), CDate1, CDate2, "Month", sVal, DateTime.Now);
                            if (sresult>0)
                            {

                            }
                        }

                        Session["SelectedActivity"] = Request.QueryString["WorkpackageUID"].ToString();
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
                    }
                    else
                    {
                        string confirmValue = Request.Form["confirm_value"];
                        if (confirmValue == "Yes")
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

                                int sresult = getdata.InsertorUpdateResourceDeploymentPlanned(Guid.NewGuid(), new Guid(Request.QueryString["WorkpackageUID"]), new Guid(Request.QueryString["ResourceUID"]), CDate1, CDate2, "Month", sVal, DateTime.Now);
                                if (sresult > 0)
                                {

                                }
                            }
                            Session["SelectedActivity"] = Request.QueryString["WorkpackageUID"].ToString();
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");

                        }
                        else
                        {
                            int cnt = getdata.ResourceDeploymentPlan_Delete_by_ResourceUID(new Guid(Request.QueryString["ResourceUID"]));
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

                                    int sresult = getdata.InsertorUpdateResourceDeploymentPlanned(Guid.NewGuid(), new Guid(Request.QueryString["WorkpackageUID"]), new Guid(Request.QueryString["ResourceUID"]), CDate1, CDate2, "Month", sVal, DateTime.Now);
                                    if (sresult > 0)
                                    {

                                    }
                                }

                                Session["SelectedActivity"] = Request.QueryString["WorkpackageUID"].ToString();
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

                            int sresult = getdata.InsertorUpdateResourceDeploymentPlanned(Guid.NewGuid(), new Guid(Request.QueryString["WorkpackageUID"]), new Guid(Request.QueryString["ResourceUID"]), CDate1, CDate2, "Date", sVal, DateTime.Now);
                            if (sresult > 0)
                            {

                            }
                        }

                        Session["SelectedActivity"] = Request.QueryString["WorkpackageUID"].ToString();
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");

                    }
                    else
                    {
                        string confirmValue = Request.Form["confirm_value"];
                        if (confirmValue == "Yes")
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

                                int sresult = getdata.InsertorUpdateResourceDeploymentPlanned(Guid.NewGuid(), new Guid(Request.QueryString["WorkpackageUID"]), new Guid(Request.QueryString["ResourceUID"]), CDate1, CDate2, "Date", sVal, DateTime.Now);
                                if (sresult > 0)
                                {

                                }
                            }
                            Session["SelectedActivity"] = Request.QueryString["WorkpackageUID"].ToString();
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
                        }
                        else
                        {
                            int cnt = getdata.ResourceDeploymentPlan_Delete_by_ResourceUID(new Guid(Request.QueryString["ResourceUID"]));
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

                                    int sresult = getdata.InsertorUpdateResourceDeploymentPlanned(Guid.NewGuid(), new Guid(Request.QueryString["WorkpackageUID"]), new Guid(Request.QueryString["ResourceUID"]), CDate1, CDate2, "Date", sVal, DateTime.Now);
                                    if (sresult > 0)
                                    {

                                    }
                                }

                                Session["SelectedActivity"] = Request.QueryString["WorkpackageUID"].ToString();
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