using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagementTool._modal_pages
{
    public partial class add_workpackage : System.Web.UI.Page
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
                this.UnobtrusiveValidationMode = System.Web.UI.UnobtrusiveValidationMode.None;
                if (!IsPostBack)
                {
                    //txtbudget.Attributes.Add("onkeyup", "javascript: ReplaceNumberWithCommas(this.value);");
                    LblProjectNumber.InnerText = WebConfigurationManager.AppSettings["Domain"] + " Project Number";
                    BindProject();
                    DDLProject_SelectedIndexChanged(sender, e);
                    BindContractors();
                    BindStatusMaster();
                    BindUsers();
                    BindWorkpackageOption();
                    if (Request.QueryString["WorkPackageUID"] != null)
                    {
                        BindWorkPackage();
                        BindAssignedUser();
                    }
                    else
                    {
                        dtStartDate.Text = DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    }
                    if (Request.QueryString["ProjectUID"] != null)
                    {
                        DDLProject.SelectedValue = Request.QueryString["ProjectUID"];
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

                ds = gettk.GetAssignedProjects_by_UserUID(new Guid(Session["UserUID"].ToString()));
            }
            DDLProject.DataTextField = "ProjectName";
            DDLProject.DataValueField = "ProjectUID";
            DDLProject.DataSource = ds;
            DDLProject.DataBind();

            if (Request.QueryString["ProjectUID"] != null)
            {
                DDLProject.SelectedValue = Request.QueryString["ProjectUID"];
            }
        }

        private void BindWorkpackageOption()
        {
            string Domain = WebConfigurationManager.AppSettings["Domain"];

            DataSet ds = getdt.Workpackageoption_SelectBy_OptionFor(Domain);
            DDLWorkpackageOption.DataTextField = "Workpackage_OptionName";
            DDLWorkpackageOption.DataValueField = "Workpackage_OptionUID";
            DDLWorkpackageOption.DataSource = ds;
            DDLWorkpackageOption.DataBind();

            if (Domain == "NJSEI")
            {
                WorkpackageoptionDiv.Visible = true;
                DDLWorkpackageOption.Items.Insert(0, new ListItem("--Select--", ""));
            }
            else
            {
                WorkpackageoptionDiv.Visible = false;
            }
        }
        private void BindContractors()
        {
            DataSet ds = getdt.GetContractors();
            DDLContractor.DataTextField = "Contractor_Name";
            DDLContractor.DataValueField = "Contractor_UID";
            DDLContractor.DataSource = ds;
            DDLContractor.DataBind();
            DDLContractor.Items.Insert(0, new ListItem("--Select--", ""));

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (DDLContractor.SelectedValue != "")
                {
                    DataSet dscontractor = getdt.GetContractors_by_ContractorUID(new Guid(DDLContractor.SelectedValue));
                    if (dscontractor.Tables[0].Rows.Count > 0)
                    {
                        txtnjseinumber.Text = dscontractor.Tables[0].Rows[0]["NJSEI_Number"].ToString();
                        txtprojectspecificnumber.Text = dscontractor.Tables[0].Rows[0]["ProjectSpecific_Number"].ToString();
                    }
                }
                
            }
        }
        private void BindWorkpackageMasters(string ProjectUID)
        {
            DataSet ds = getdt.MasterWorkpackage_selectBy_ProjectUID(new Guid(ProjectUID));
            if (ds.Tables[0].Rows.Count > 0)
            {
                DDLWorkPackage.DataTextField = "MasterWorkPackageName";
                DDLWorkPackage.DataValueField= "MasterWorkPackageUID";
                DDLWorkPackage.DataSource = ds;
                DDLWorkPackage.DataBind();
            }
        }
        private void BindLocationMaster(string ProjectUID)
        {
            DataSet ds = getdt.MasterLocation_selectBy_ProjectUID(new Guid(ProjectUID));
            if (ds.Tables[0].Rows.Count > 0)
            {
                DDLLocation.DataTextField = "LocationMaster_Name";
                DDLLocation.DataValueField = "LocationMasterUID";
                DDLLocation.DataSource = ds;
                DDLLocation.DataBind();
            }
        }
        private void BindClientMaster(string ProjectUID)
        {
            DataSet ds = getdt.MasterClient_selectBy_ProjectUID(new Guid(ProjectUID));
            if (ds.Tables[0].Rows.Count > 0)
            {
                DDLClient.DataTextField = "ClientMaster_Name";
                DDLClient.DataValueField = "ClientMasterUID";
                DDLClient.DataSource = ds;
                DDLClient.DataBind();
            }
        }

        private void BindUsers()
        {
            DataSet ds = new DataSet();

            if (Session["TypeOfUser"].ToString() == "U" || Session["TypeOfUser"].ToString() =="MD" || Session["TypeOfUser"].ToString() == "VP")
            {
                ds = getdt.getAllUsers();
            }
            else if (Session["TypeOfUser"].ToString() == "PA")
            {
                //ds = getdt.getUsers_by_ProjectUnder(new Guid(DDLProject.SelectedValue));
                ds = getdt.GetUsers_under_ProjectUID(new Guid(DDLProject.SelectedValue));

            }
            else
            {
                ds = getdt.GetUsers_under_ProjectUID(new Guid(DDLProject.SelectedValue));

            }
            lstUsers.DataTextField = "UserName";
            lstUsers.DataValueField = "UserUID";
            lstUsers.DataSource = ds;
            lstUsers.DataBind();
        }

        private void BindWorkPackage()
        {
            DataSet ds = getdt.GetWorkPackages_By_WorkPackageUID(new Guid(Request.QueryString["WorkPackageUID"]));
            if (ds.Tables[0].Rows.Count > 0)
            {
                DDLWorkPackage.SelectedItem.Text = ds.Tables[0].Rows[0]["Name"].ToString();
                string CurrencySymbol = "";
                if (ds.Tables[0].Rows[0]["Currency"].ToString() == "&#x20B9;")
                {
                    DDLCurrency.SelectedIndex = 0;
                    CurrencySymbol = "₹";
                }
                else if (ds.Tables[0].Rows[0]["Currency"].ToString() == "&#36;")
                {
                    DDLCurrency.SelectedIndex = 1;
                    CurrencySymbol = "$";
                }
                else
                {
                    DDLCurrency.SelectedIndex = 2;
                    CurrencySymbol = "¥";
                }
                if (ds.Tables[0].Rows[0]["Budget"].ToString() != "0")
                {
                    txtbudget.Value = CurrencySymbol + " " + Convert.ToDouble(ds.Tables[0].Rows[0]["Budget"].ToString()).ToString("#,##.##", CultureInfo.CreateSpecificCulture(ds.Tables[0].Rows[0]["Currency_CultureInfo"].ToString()));
                  
                }
                else
                {
                    txtbudget.Value = CurrencySymbol + " " + "0";
                }

                if (ds.Tables[0].Rows[0]["ActualExpenditure"].ToString() != "0")
                {
                    txtActualExpenditure.Value = CurrencySymbol + " " + Convert.ToDouble(ds.Tables[0].Rows[0]["ActualExpenditure"].ToString()).ToString("#,##.##", CultureInfo.CreateSpecificCulture(ds.Tables[0].Rows[0]["Currency_CultureInfo"].ToString()));
                }
                else
                {
                    txtActualExpenditure.Value = CurrencySymbol + " " + "0";
                }
                
                ddlstatus.SelectedValue = ds.Tables[0].Rows[0]["Status"].ToString();
                DDLProject.SelectedValue = ds.Tables[0].Rows[0]["ProjectUID"].ToString();
                DDLContractor.SelectedValue = ds.Tables[0].Rows[0]["Contractor_UID"].ToString();
                ContractInfoBind();
                DDLLocation.SelectedItem.Text = ds.Tables[0].Rows[0]["WorkPackage_Location"].ToString();
                DDLClient.SelectedItem.Text = ds.Tables[0].Rows[0]["WorkPackage_Client"].ToString();
                if (ds.Tables[0].Rows[0]["Workpackage_OptionUID"].ToString() != "")
                {
                    DDLWorkpackageOption.SelectedValue = ds.Tables[0].Rows[0]["Workpackage_OptionUID"].ToString();
                    DDLWorkpackageOption.Enabled = false;
                }
                else
                {
                    DDLWorkpackageOption.Enabled = true;
                }
                if (ds.Tables[0].Rows[0]["StartDate"].ToString() != null && ds.Tables[0].Rows[0]["StartDate"].ToString() != "")
                {
                    dtStartDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["StartDate"].ToString()).ToString("dd/MM/yyyy",CultureInfo.InvariantCulture);
                }
                if (ds.Tables[0].Rows[0]["PlannedEndDate"].ToString() != null && ds.Tables[0].Rows[0]["PlannedEndDate"].ToString() != "")
                {
                    dtPlannedEndDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["PlannedEndDate"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                if (ds.Tables[0].Rows[0]["ProjectedEndDate"].ToString() != null && ds.Tables[0].Rows[0]["ProjectedEndDate"].ToString() != "")
                {
                    dtProjectedEndDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["ProjectedEndDate"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                // added on 25/01/2022
                ViewState["Budget"] = ds.Tables[0].Rows[0]["Budget"].ToString();
                ViewState["ActualExpenditure"] = ds.Tables[0].Rows[0]["ActualExpenditure"].ToString();
                ViewState["ProjectedEndDate"] = ds.Tables[0].Rows[0]["ProjectedEndDate"];

            }
        }

        private void BindAssignedUser()
        {
            DataSet ds = getdt.GetAssignedUsers_for_WorkPackage(new Guid(DDLProject.SelectedValue), new Guid(Request.QueryString["WorkPackageUID"]));
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    foreach (ListItem item in lstUsers.Items)
                    {
                        if (ds.Tables[0].Rows[i]["UserUID"].ToString() == item.Value)
                        {
                            item.Selected = true;
                        }
                    }
                }
            }
        }

        private void BindStatusMaster()
        {
            ddlstatus.DataSource = getdt.getStatusMaster();
            ddlstatus.DataTextField = "Value";
            ddlstatus.DataValueField = "Status";
            ddlstatus.DataBind();
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsValid)
                {
                    //Page.Validate("add");
                    int ItemsCount = (from ListItem li in lstUsers.Items
                                 where li.Selected == true
                                 select li).Count();
                    Guid WorkPackageUID = new Guid();
                   
                    if (ItemsCount == 0)
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Please assign atleast one user for this workpackage..');</script>");
                    }
                    else
                    {
                        if (Request.QueryString["WorkPackageUID"] != null)
                        {
                            WorkPackageUID = new Guid(Request.QueryString["WorkPackageUID"]);
                          
                        }
                        else
                        {
                            WorkPackageUID = Guid.NewGuid();
                        }

                        string sDate1 = "", sDate2 = "", sDate3 = "";
                        DateTime CDate1 = DateTime.Now, CDate2 = DateTime.Now, CDate3 = DateTime.Now;
                        //
                        if (dtStartDate.Text != "")
                        {
                            sDate1 = dtStartDate.Text;
                            //sDate1 = sDate1.Split('/')[1] + "/" + sDate1.Split('/')[0] + "/" + sDate1.Split('/')[2];
                            sDate1 = getdt.ConvertDateFormat(sDate1);
                            CDate1 = Convert.ToDateTime(sDate1);
                        }
                        //
                        if (dtPlannedEndDate.Text != "")
                        {
                            sDate2 = dtPlannedEndDate.Text;
                            //sDate2 = sDate2.Split('/')[1] + "/" + sDate2.Split('/')[0] + "/" + sDate2.Split('/')[2];
                            sDate2 = getdt.ConvertDateFormat(sDate2);
                            CDate2 = Convert.ToDateTime(sDate2);
                        }
                        //
                        if (dtProjectedEndDate.Text != "")
                        {
                            sDate3 = dtProjectedEndDate.Text;
                            //sDate3 = sDate3.Split('/')[1] + "/" + sDate3.Split('/')[0] + "/" + sDate3.Split('/')[2];
                            sDate3 = getdt.ConvertDateFormat(sDate3);
                            CDate3 = Convert.ToDateTime(sDate3);
                        }
                        txtbudget.Value = txtbudget.Value.Replace(",", "").Replace("₹ ", "").Replace("$ ", "").Replace("¥ ", "").Replace("₹", "").Replace("$", "").Replace("¥", "").Trim();
                        txtActualExpenditure.Value = txtActualExpenditure.Value.Replace(",", "").Replace("₹ ", "").Replace("$ ", "").Replace("¥ ", "").Replace("₹", "").Replace("$", "").Replace("¥", "").Trim();
                        string Currecncy_CultureInfo = "";
                        if (DDLCurrency.SelectedItem.Text == "₹ (RUPEE)")
                        {
                            Currecncy_CultureInfo = "en-IN";
                        }
                        else if (DDLCurrency.SelectedItem.Text == "$ (USD)")
                        {
                            Currecncy_CultureInfo = "en-US";
                        }
                        else
                        {
                            Currecncy_CultureInfo = "ja-JP";
                        }
                        Guid Workpackageoption;
                        if (WebConfigurationManager.AppSettings["Domain"] == "NJSEI")
                        {
                            Workpackageoption = new Guid(DDLWorkpackageOption.SelectedValue);
                        }
                        else
                        {
                            Workpackageoption = new Guid(DDLWorkpackageOption.SelectedValue);
                        }


                        int Cnt = getdt.InsertorUpdateWorkPackage(WorkPackageUID, new Guid(DDLProject.SelectedValue), new Guid(DDLWorkpackageOption.SelectedValue), new Guid(DDLContractor.SelectedValue), DDLWorkPackage.SelectedItem.Text, DDLLocation.SelectedItem.Text, DDLClient.SelectedItem.Text, CDate1, CDate2, CDate3,
                            ddlstatus.SelectedValue, Convert.ToDouble(txtbudget.Value), Convert.ToDouble(txtActualExpenditure.Value), (DDLCurrency.SelectedItem.Text == "₹ (RUPEE)") ? "&#x20B9;" : (DDLCurrency.SelectedItem.Text == "$ (USD)") ? "&#36;" : "&#165;", Currecncy_CultureInfo);
                        if (Cnt > 0)
                        {

                            foreach (ListItem item in lstUsers.Items)
                            {
                                if (item.Selected)
                                {
                                    int count = getdt.Insert_UserWorkPackages(new Guid(DDLProject.SelectedValue), new Guid(item.Value), WorkPackageUID, "A");
                                    if (count > 0)
                                    {
                                        //Inserted
                                    }
                                }
                            }
                            if (Request.QueryString["WorkPackageUID"] == null)
                            {
                                if (WebConfigurationManager.AppSettings["Domain"] == "NJSEI")
                                {
                                    if (getdt.GetWorkpackageOption_Order(new Guid(DDLWorkpackageOption.SelectedValue)) != 4)
                                    {
                                        int result = getdt.WorkpackageSelectedOptions_Insert(Guid.NewGuid(), WorkPackageUID, new Guid(DDLWorkpackageOption.SelectedValue));
                                        if (result > 0)
                                        {
                                            CopyMasterActivityData(WorkPackageUID, new Guid(DDLWorkpackageOption.SelectedValue));
                                        }
                                    }
                                    AddCategoryForWorkpackage(WorkPackageUID.ToString());
                                }
                                else
                                {
                                    DataSet ds = getdt.Workpackageoption_SelectBy_OptionFor(WebConfigurationManager.AppSettings["Domain"]);
                                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                    {
                                        int result = getdt.WorkpackageSelectedOptions_Insert(Guid.NewGuid(), WorkPackageUID, new Guid(ds.Tables[0].Rows[i]["Workpackage_OptionUID"].ToString()));
                                        if (result > 0)
                                        {
                                            CopyMasterActivityData(WorkPackageUID, new Guid(ds.Tables[0].Rows[i]["Workpackage_OptionUID"].ToString()));
                                        }
                                        
                                    }
                                    AddCategoryForWorkpackage(WorkPackageUID.ToString());
                                }
                                    
                            }

                            // added for maintaining data history for yogesh changes for budget,ActualExpendtiture,Projected End Date
                            if (Request.QueryString["WorkPackageUID"] != null)
                            {
                                if(Convert.ToDouble(txtbudget.Value) != Convert.ToDouble(ViewState["Budget"]))
                                {
                                    getdt.InsertWkpgDataHistory(WorkPackageUID, new Guid(Session["UserUID"].ToString()), Convert.ToDouble(ViewState["Budget"]), Convert.ToDouble(txtbudget.Value), Convert.ToDouble(ViewState["ActualExpenditure"]), Convert.ToDouble(txtActualExpenditure.Value), DateTime.Now, CDate3, 1);
                                }
                                if (Convert.ToDouble(txtActualExpenditure.Value) != Convert.ToDouble(ViewState["ActualExpenditure"]))
                                {
                                    getdt.InsertWkpgDataHistory(WorkPackageUID, new Guid(Session["UserUID"].ToString()), Convert.ToDouble(ViewState["Budget"]), Convert.ToDouble(txtbudget.Value), Convert.ToDouble(ViewState["ActualExpenditure"]), Convert.ToDouble(txtActualExpenditure.Value), DateTime.Now, CDate3, 2);
                                }
                                if(CDate3 !=Convert.ToDateTime(ViewState["ProjectedEndDate"]))
                                {
                                    getdt.InsertWkpgDataHistory(WorkPackageUID, new Guid(Session["UserUID"].ToString()), Convert.ToDouble(ViewState["Budget"]), Convert.ToDouble(txtbudget.Value), Convert.ToDouble(ViewState["ActualExpenditure"]), Convert.ToDouble(txtActualExpenditure.Value), Convert.ToDateTime(ViewState["ProjectedEndDate"]), CDate3, 3);

                                }

                            }
                            //
                             Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
                        }
                        else if (Cnt == -1)
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Workpackage Name already exists for project " + DDLProject.SelectedItem.Text + "');</script>");
                        }
                        else
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code : AWP-01 there is a problem with these feature. Please contact system admin.);</script>");
                        }
                    }
                    
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code : AWP-01 there is a problem with these feature. Please contact system admin. Desc : " + ex.Message + "');</script>");
            }
            
        }

        protected void DDLProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindWorkpackageMasters(DDLProject.SelectedValue);
            BindLocationMaster(DDLProject.SelectedValue);
            BindClientMaster(DDLProject.SelectedValue);

            if (DDLWorkPackage.SelectedValue == "")
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Workpackage Name is empty. Please add it in Masters First.');</script>");
            }
            else if (DDLLocation.SelectedValue == "")
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Location is empty. Please add it in Masters First.');</script>");
            }
            else if (DDLClient.SelectedValue == "")
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Client is empty. Please add it in Masters First.');</script>");
            }
        }

        protected void DDLContractor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDLContractor.SelectedValue != "")
            {
                ContractInfoBind();
            }
            
        }
        private void ContractInfoBind()
        {
            DataSet ds = getdt.GetContractors_by_ContractorUID(new Guid(DDLContractor.SelectedValue));
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtnjseinumber.Text = ds.Tables[0].Rows[0]["NJSEI_Number"].ToString();
                txtprojectspecificnumber.Text = ds.Tables[0].Rows[0]["ProjectSpecific_Number"].ToString();
            }
        }
        private void CopyMasterActivityData(Guid WorkpackageUID,Guid OptionUID)
        {
            DataSet ds = getdt.WorkpackageMainActivityMaster_SelectBy_OptionUID(OptionUID);
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Guid TaskUID = Guid.NewGuid();
                    try
                    {

                        bool result = getdt.InsertorUpdateMainTask_From_Master(TaskUID, WorkpackageUID, new Guid(DDLProject.SelectedValue), OptionUID, Session["UserUID"].ToString(), ds.Tables[0].Rows[i]["WorkpakageActivity_Name"].ToString(), ds.Tables[0].Rows[i]["WorkpakageActivity_Name"].ToString(), "P", 0, 0, 1, 0, 0, 0, "&#x20B9;", "en-IN", Convert.ToInt32(ds.Tables[0].Rows[i]["WorkpakageActivity_Order"].ToString()));
                        if (result)
                        {
                            TaskDataInsert(WorkpackageUID, new Guid(ds.Tables[0].Rows[i]["WorkpakageActivity_MasterUID"].ToString()), TaskUID, OptionUID);
                        }
                        else
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code AWPM-01 there is a problem with this feature. Please contact system admin.');</script>");
                        }
                    }
                    catch (Exception ex)
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code AWPM-01 there is a problem with this feature. Please contact system admin.');</script>");
                    }
                    
                }
            }
        }
        private void TaskDataInsert(Guid WorkpackageUID, Guid FromParentTaskUID, Guid ToParentTaskUID,Guid OptionUID)
        {
            DataSet ds = getdt.WorkpackageActivityMaster_SelectBy_ParentUID(FromParentTaskUID);
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Guid sSubTaskUID = Guid.NewGuid();
                    try
                    {
                        
                        bool result = getdt.InsertorUpdateSubTask_From_Master(sSubTaskUID, WorkpackageUID, new Guid(DDLProject.SelectedValue), OptionUID, Session["UserUID"].ToString(), ds.Tables[0].Rows[i]["WorkpakageActivity_Name"].ToString(), ds.Tables[0].Rows[i]["WorkpakageActivity_Name"].ToString(), "P", 0, 0, Convert.ToInt32(ds.Tables[0].Rows[i]["WorkpakageActivity_Level"].ToString()), 0, 0, 0, "&#x20B9;", "en-IN", Convert.ToInt32(ds.Tables[0].Rows[i]["WorkpakageActivity_Order"].ToString()), ToParentTaskUID.ToString());
                        if (result)
                        {
                            TaskDataInsert(WorkpackageUID, new Guid(ds.Tables[0].Rows[i]["WorkpakageActivity_MasterUID"].ToString()), sSubTaskUID, OptionUID);
                        }
                    }
                    catch (Exception ex)
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code SAWPM-01 there is a problem with this feature. Please contact system admin.');</script>");
                    }
                   
                }
            }
        }

        private void AddCategoryForWorkpackage(string WorkpackageUID)
        {
            DataSet ds = getdt.GetActualDocument_Type_Master();
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    int cnt = getdt.WorkPackageCategory_Insert_or_Update(Guid.NewGuid(), new Guid(WorkpackageUID), ds.Tables[0].Rows[i]["ActualDocumentType"].ToString());
                    if (cnt <= 0)
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code AWPC-01 there is a problem with this feature. Please contact system admin.');</script>");
                    }
                }
                
            }
        }
    }
}