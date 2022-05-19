using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagementTool._content_pages.import_data
{
    public partial class _default : System.Web.UI.Page
    {
        DBGetData getdt = new DBGetData();
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
                    DDlProject_SelectedIndexChanged(sender, e);
                    BindDocumentFlows();
                    DDLDocumentFlow_SelectedIndexChanged(sender, e);
                    BindUsers();
                    BindWorkpackageOption();
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
            //DataSet ds = getdt.GetWorkPackages_By_ProjectUID(new Guid(DDlProject.SelectedValue));
            DataSet ds = new DataSet();
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
            if (ds.Tables[0].Rows.Count > 0)
            {
                DDLWorkPackage.DataTextField = "Name";
                DDLWorkPackage.DataValueField = "WorkPackageUID";
                DDLWorkPackage.DataSource = ds;
                DDLWorkPackage.DataBind();
            }
        }

        private void BindDocumentFlows()
        {
            DataSet ds = getdt.GetDocumentFlows();
            DDLDocumentFlow.DataTextField = "Flow_Name";
            DDLDocumentFlow.DataValueField = "FlowMasterUID";
            DDLDocumentFlow.DataSource = ds;
            DDLDocumentFlow.DataBind();
            DDLDocumentFlow.Items.Insert(0, "--Select--");
            DDLDocumentFlow.SelectedIndex = 2;

        }

        protected void DDLDocumentFlow_SelectedIndexChanged(object sender, EventArgs e)
        {
            DocumentFlowChanged();
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
                //ds = getdata.getUsers_by_ProjectUnder(new Guid(DDlProject.SelectedValue));
                ds = getdt.GetUsers_under_ProjectUID(new Guid(DDlProject.SelectedValue));
            }
            else
            {
                ds = getdt.GetUsers_under_ProjectUID(new Guid(DDlProject.SelectedValue));
            }

            DDLUsers.DataTextField = "UserName";
            DDLUsers.DataValueField = "UserUID";
            DDLUsers.DataSource = ds;
            DDLUsers.DataBind();
        }

        public void DocumentFlowChanged()
        {
            DataSet ds = getdt.GetDocumentFlows_by_UID(new Guid(DDLDocumentFlow.SelectedValue));
            if (ds.Tables[0].Rows.Count > 0)
            {
                //dFlow.Visible = true;
                LoadDropDowns();

                if (ds.Tables[0].Rows[0]["Steps_Count"].ToString() == "2")
                {
                    lblStep1Display.Text = ds.Tables[0].Rows[0]["FlowStep1_DisplayName"].ToString();
                    lblStep1Date.Text = ds.Tables[0].Rows[0]["FlowStep1_DisplayName"].ToString() + " Target Date";
                    lblStep2Display.Text = ds.Tables[0].Rows[0]["FlowStep2_DisplayName"].ToString();
                    lblStep2Date.Text = ds.Tables[0].Rows[0]["FlowStep2_DisplayName"].ToString() + " Target Date";
                    S1Display.Visible = true;
                    S1Date.Visible = true;
                    S2Display.Visible = true;
                    S2Date.Visible = true;
                    S3Display.Visible = false;
                    S3Date.Visible = false;
                }
                else if (ds.Tables[0].Rows[0]["Steps_Count"].ToString() == "3")
                {
                    lblStep1Display.Text = ds.Tables[0].Rows[0]["FlowStep1_DisplayName"].ToString();
                    lblStep1Date.Text = ds.Tables[0].Rows[0]["FlowStep1_DisplayName"].ToString() + " Target Date";
                    lblStep2Display.Text = ds.Tables[0].Rows[0]["FlowStep2_DisplayName"].ToString();
                    lblStep2Date.Text = ds.Tables[0].Rows[0]["FlowStep2_DisplayName"].ToString() + " Target Date";
                    lblStep3Display.Text = ds.Tables[0].Rows[0]["FlowStep3_DisplayName"].ToString();
                    lblStep3Date.Text = ds.Tables[0].Rows[0]["FlowStep3_DisplayName"].ToString() + " Target Date";
                    S1Display.Visible = true;
                    S1Date.Visible = true;
                    S2Display.Visible = true;
                    S2Date.Visible = true;
                    S3Display.Visible = true;
                    S3Date.Visible = true;
                }
                else if (ds.Tables[0].Rows[0]["Steps_Count"].ToString() == "4")
                {
                    lblStep1Display.Text = ds.Tables[0].Rows[0]["FlowStep1_DisplayName"].ToString();
                    lblStep1Date.Text = ds.Tables[0].Rows[0]["FlowStep1_DisplayName"].ToString() + " Target Date";
                    lblStep2Display.Text = ds.Tables[0].Rows[0]["FlowStep2_DisplayName"].ToString();
                    lblStep2Date.Text = ds.Tables[0].Rows[0]["FlowStep2_DisplayName"].ToString() + " Target Date";
                    lblStep3Display.Text = ds.Tables[0].Rows[0]["FlowStep3_DisplayName"].ToString();
                    lblStep3Date.Text = ds.Tables[0].Rows[0]["FlowStep3_DisplayName"].ToString() + " Target Date";
                    S1Display.Visible = true;
                    S1Date.Visible = true;
                    S2Display.Visible = true;
                    S2Date.Visible = true;
                    S3Display.Visible = true;
                    S3Date.Visible = true;
                }
                else
                {
                    lblStep1Display.Text = ds.Tables[0].Rows[0]["FlowStep1_DisplayName"].ToString();
                    lblStep1Date.Text = ds.Tables[0].Rows[0]["FlowStep1_DisplayName"].ToString() + " Target Date";
                    lblStep2Display.Text = ds.Tables[0].Rows[0]["FlowStep2_DisplayName"].ToString();
                    lblStep2Date.Text = ds.Tables[0].Rows[0]["FlowStep2_DisplayName"].ToString() + " Target Date";
                    lblStep3Display.Text = ds.Tables[0].Rows[0]["FlowStep3_DisplayName"].ToString();
                    lblStep3Date.Text = ds.Tables[0].Rows[0]["FlowStep3_DisplayName"].ToString() + " Target Date";
                }
                string sDate1 = "";
                DateTime CDate1 = DateTime.Now;

                //sDate1 = (dtStartDate.FindControl("txtDate") as TextBox).Text;
                sDate1 = DateTime.Now.ToString("dd/MM/yyyy");
                //sDate1 = sDate1.Split('/')[1] + "/" + sDate1.Split('/')[0] + "/" + sDate1.Split('/')[2];
                sDate1 = getdt.ConvertDateFormat(sDate1);
                CDate1 = Convert.ToDateTime(sDate1);

                //string dt = Convert.ToDateTime((dtSubTargetDate.FindControl("txtDate") as TextBox).Text).ToString("dd/MM/yyyy");
                //string dt = CDate1;
                if (ds.Tables[0].Rows[0]["Steps_Count"].ToString() == "2")
                {
                    dtSubTargetDate.Text = CDate1.AddDays(-1).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    //dtSubTargetDate.Text = CDate1.ToString("dd/MM/yyyy");

                    dtQualTargetDate.Text = CDate1.AddDays(Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep2_Duration"].ToString())).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                else if (ds.Tables[0].Rows[0]["Steps_Count"].ToString() == "3")
                {
                    //dtSubTargetDate.Text = CDate1.AddDays(Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep1_Duration"].ToString())).ToString("dd/MM/yyyy");
                    dtSubTargetDate.Text = CDate1.AddDays(-1).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

                    dtQualTargetDate.Text = CDate1.AddDays(Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep2_Duration"].ToString())).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

                    dtRev_B_TargetDate.Text = CDate1.AddDays(Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep3_Duration"].ToString())).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                else if (ds.Tables[0].Rows[0]["Steps_Count"].ToString() == "4")
                {
                    //dtSubTargetDate.Text = CDate1.AddDays(Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep1_Duration"].ToString())).ToString("dd/MM/yyyy");
                    dtSubTargetDate.Text = CDate1.AddDays(-1).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

                    dtQualTargetDate.Text = CDate1.AddDays(Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep2_Duration"].ToString())).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);


                    dtRev_B_TargetDate.Text = CDate1.AddDays(Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep3_Duration"].ToString())).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                else
                {
                    //dtSubTargetDate.Text = CDate1.AddDays(Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep1_Duration"].ToString())).ToString("dd/MM/yyyy");
                    dtSubTargetDate.Text = CDate1.AddDays(-1).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

                    dtQualTargetDate.Text = CDate1.AddDays(Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep2_Duration"].ToString())).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

                    dtRev_B_TargetDate.Text = CDate1.AddDays(Convert.ToInt32(ds.Tables[0].Rows[0]["FlowStep3_Duration"].ToString())).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

                }

            }
        }

        private void LoadDropDowns()
        {
            try
            {

                DataSet ds = new DataSet();
                if (Session["TypeOfUser"].ToString() == "U" || Session["TypeOfUser"].ToString() =="MD" || Session["TypeOfUser"].ToString() == "VP")
                {
                    ds = getdt.getAllUsers();
                }
                else if (Session["TypeOfUser"].ToString() == "PA")
                {
                    //ds = getdata.getUsers_by_ProjectUnder(new Guid(DDlProject.SelectedValue));
                    ds = getdt.GetUsers_under_ProjectUID(new Guid(DDlProject.SelectedValue));
                }
                else
                {
                    ds = getdt.getUsers_by_ProjectUnder(new Guid(DDlProject.SelectedValue));
                }

                //ddlSubmissionUSer.DataSource = getdata.getUsers("S");
                ddlSubmissionUSer.DataSource = ds;
                ddlSubmissionUSer.DataTextField = "UserName";
                ddlSubmissionUSer.DataValueField = "UserUID";
                ddlSubmissionUSer.DataBind();
                //
                //ddlQualityEngg.DataSource = getdata.getUsers("C");
                ddlQualityEngg.DataSource = ds;
                ddlQualityEngg.DataTextField = "UserName";
                ddlQualityEngg.DataValueField = "UserUID";
                ddlQualityEngg.DataBind();
                //
                //ddlReviewer.DataSource = getdata.getUsers("R");
                //ddlReviewer.DataSource = ds;
                //ddlReviewer.DataTextField = "UserName";
                //ddlReviewer.DataValueField = "UserUID";
                //ddlReviewer.DataBind();

                //
                //ddlReviewer_B.DataSource = getdata.getUsers("R");
                ddlReviewer_B.DataSource = ds;
                ddlReviewer_B.DataTextField = "UserName";
                ddlReviewer_B.DataValueField = "UserUID";
                ddlReviewer_B.DataBind();

                //
                //ddlApproval.DataSource = getdata.getUsers("A");
                //ddlApproval.DataSource = ds;
                //ddlApproval.DataTextField = "UserName";
                //ddlApproval.DataValueField = "UserUID";
                //ddlApproval.DataBind();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void BindWorkpackageOption()
        {
            string Domain = WebConfigurationManager.AppSettings["Domain"];

            DataSet ds = getdt.Workpackageoption_SelectBy_OptionFor(Domain);
            DDLOptions.DataTextField = "Workpackage_OptionName";
            DDLOptions.DataValueField = "Workpackage_OptionUID";
            DDLOptions.DataSource = ds;
            DDLOptions.DataBind();
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (FileUpload1.HasFile)
            {
                string ConStr = "";
                string path = Server.MapPath("~/Documents/" + FileUpload1.FileName);
                string ext = Path.GetExtension(FileUpload1.FileName).ToLower();
                FileUpload1.SaveAs(path);
                if (ext.Trim() == ".xls")
                {
                    ConStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                }
                else if (ext.Trim() == ".xlsx")
                {
                    //connection string for that file which extantion is .xlsx  
                    ConStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                    //ConStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties=\"Excel 12.0;\"";
                }
                string query = string.Empty;
                if (RBList.SelectedValue == "Activity")
                {
                    query = "SELECT * FROM [Sheet1$]";
                }
                else if (RBList.SelectedValue == "Activity Puttenahalli")
                {
                    query = "SELECT * FROM [Zuber_changed$]";
                }
                else if (RBList.SelectedValue == "Activity New")
                {
                    query = "SELECT * FROM [Sheet1$]";
                }
                else if (RBList.SelectedValue == "Document")
                {
                    query = "SELECT * FROM [30 MLD PICKET NALLAH$]";
                }
                else
                {
                    query = "SELECT * FROM [Sheet1$]";
                }
                OleDbConnection conn = new OleDbConnection(ConStr);
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                //create command object  
                OleDbCommand cmd = new OleDbCommand(query, conn);
                // create a data adapter and get the data into dataadapter  
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                DataSet ds = new DataSet();
                //fill the Excel data to data set  
                da.Fill(ds);
                conn.Close();

                if (RBList.SelectedValue == "Activity")
                {
                    try
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {

                            //string OptionUID = "E4D01ABA-2ED2-4B0F-B4CB-2620B980EDAC";
                            string OptionUID = DDLOptions.SelectedValue;
                            string MainTask = string.Empty;
                            string MainTaskGUID = string.Empty;
                            string FirstTask = string.Empty;
                            string FirstTaskGUID = string.Empty;
                            string SecondTask = string.Empty;
                            string SecondTaskGUID = string.Empty;
                            string ThirdTask = string.Empty;
                            string ThirdTaskGUID = string.Empty;
                            string FourthTask = string.Empty;
                            string FourthTaskGUID = string.Empty;
                            string sDate1 = "", sDate2 = "";
                            DateTime CDate1 = new DateTime(), CDate2 = new DateTime();

                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                try
                                {
                                    if (ds.Tables[0].Rows[i][7].ToString() != "")
                                    {
                                        Guid BOQDetailsUID = new Guid(ds.Tables[0].Rows[i][11].ToString().Trim());
                                        MainTask = ds.Tables[0].Rows[i][7].ToString().Trim();
                                        sDate1 = ds.Tables[0].Rows[i][8].ToString().Replace("-", "/");
                                        sDate2 = ds.Tables[0].Rows[i][9].ToString().Replace("-", "/");
                                        //DateTime StartDate = Convert.ToDateTime(sDate1);
                                        //DateTime EndDate = Convert.ToDateTime(sDate2);

                                        sDate1 = sDate1.ToString();
                                        //sDate1 = sDate1.Split('/')[1] + "/" + sDate1.Split('/')[0] + "/" + sDate1.Split('/')[2];
                                        sDate1 = getdt.ConvertDateFormat(sDate1);
                                        CDate1 = Convert.ToDateTime(sDate1);

                                        sDate2 = sDate2.ToString();
                                        //sDate2 = sDate2.Split('/')[1] + "/" + sDate2.Split('/')[0] + "/" + sDate2.Split('/')[2];
                                        sDate2 = getdt.ConvertDateFormat(sDate2);
                                        CDate2 = Convert.ToDateTime(sDate2);
                                        string Status = string.Empty;
                                        if (CDate1.Date > DateTime.Now.Date)
                                        {
                                            Status = "P";
                                        }
                                        else if (CDate1.Date < DateTime.Now.Date && CDate2 > DateTime.Now.Date)
                                        {
                                            Status = "I";
                                        }
                                        else
                                        {
                                            Status = "C";
                                        }
                                        MainTaskGUID = getdt.GetTaskUID_By_WorkPackageID_TName(new Guid(DDLWorkPackage.SelectedValue), MainTask);
                                        if (MainTaskGUID == null)
                                        {
                                            MainTaskGUID = Guid.NewGuid().ToString();
                                            bool result = getdt.InsertorUpdateMainTask(new Guid(MainTaskGUID), new Guid(DDLWorkPackage.SelectedValue), new Guid(DDlProject.SelectedValue), DDLUsers.SelectedValue, MainTask, MainTask, "", "", CDate1 == DateTime.MinValue ? "" : CDate1.ToString(), CDate2 == DateTime.MinValue ? "" : CDate2.ToString(), CDate2 == DateTime.MinValue ? "" : CDate2.ToString(), CDate1 == DateTime.MinValue ? "" : CDate1.ToString(), CDate1 == DateTime.MinValue ? "" : CDate1.ToString(), CDate2 == DateTime.MinValue ? "" : CDate2.ToString(), Status, 0, 0, "", 1, 0, 0, 0, "Engineering", "", "&#x20B9;", "en-IN", 0, "", new Guid(OptionUID), 0, Guid.Empty,"0");
                                            if (result)
                                            {

                                            }
                                        }



                                    }
                                    else if (ds.Tables[0].Rows[i][8].ToString() != "" && MainTaskGUID != string.Empty)
                                    {
                                        FirstTask = ds.Tables[0].Rows[i][8].ToString().Trim();

                                        sDate1 = ds.Tables[0].Rows[i][9].ToString().Replace("-", "/");
                                        sDate2 = ds.Tables[0].Rows[i][10].ToString().Replace("-", "/");
                                        //DateTime StartDate = Convert.ToDateTime(sDate1);
                                        //DateTime EndDate = Convert.ToDateTime(sDate2);

                                        sDate1 = sDate1.ToString();
                                        //sDate1 = sDate1.Split('/')[1] + "/" + sDate1.Split('/')[0] + "/" + sDate1.Split('/')[2];
                                        sDate1 = getdt.ConvertDateFormat(sDate1);
                                        CDate1 = Convert.ToDateTime(sDate1);

                                        sDate2 = sDate2.ToString();
                                        //sDate2 = sDate2.Split('/')[1] + "/" + sDate2.Split('/')[0] + "/" + sDate2.Split('/')[2];
                                        sDate2 = getdt.ConvertDateFormat(sDate2);
                                        CDate2 = Convert.ToDateTime(sDate2);
                                        string Status = string.Empty;
                                        if (CDate1.Date > DateTime.Now.Date)
                                        {
                                            Status = "P";
                                        }
                                        else if (CDate1.Date < DateTime.Now.Date && CDate2 > DateTime.Now.Date)
                                        {
                                            Status = "I";
                                        }
                                        else
                                        {
                                            Status = "C";
                                        }

                                        FirstTaskGUID = getdt.GetTaskUID_By_ParentID_TName(new Guid(DDLWorkPackage.SelectedValue), new Guid(MainTaskGUID), FirstTask);
                                        if (FirstTaskGUID == null)
                                        {
                                            Guid BOQDetailsUID = new Guid(ds.Tables[0].Rows[i][12].ToString().Trim());
                                            FirstTaskGUID = Guid.NewGuid().ToString();
                                            bool result = getdt.InsertorUpdateSubTask(new Guid(FirstTaskGUID), new Guid(DDLWorkPackage.SelectedValue), new Guid(DDlProject.SelectedValue), DDLUsers.SelectedValue, FirstTask, FirstTask, "", "", CDate1 == DateTime.MinValue ? "" : CDate1.ToString(), CDate2 == DateTime.MinValue ? "" : CDate2.ToString(), CDate2 == DateTime.MinValue ? "" : CDate2.ToString(), CDate1 == DateTime.MinValue ? "" : CDate1.ToString(), CDate1 == DateTime.MinValue ? "" : CDate1.ToString(), CDate2 == DateTime.MinValue ? "" : CDate2.ToString(), Status, 0, 0, "", 2, MainTaskGUID, 0, 0, 0, "Engineering", "", "&#x20B9;", "en-IN", 0, "", new Guid(OptionUID), 0, Guid.Empty,"0",true);
                                            if (result)
                                            {

                                            }
                                        }
                                    }
                                    else if (ds.Tables[0].Rows[i][9].ToString() != "" && FirstTaskGUID != string.Empty)
                                    {
                                        SecondTask = ds.Tables[0].Rows[i][9].ToString().Trim();

                                        sDate1 = ds.Tables[0].Rows[i][10].ToString().Replace("-", "/");
                                        sDate2 = ds.Tables[0].Rows[i][11].ToString().Replace("-", "/");
                                        //DateTime StartDate = Convert.ToDateTime(sDate1);
                                        //DateTime EndDate = Convert.ToDateTime(sDate2);

                                        sDate1 = sDate1.ToString();
                                        //sDate1 = sDate1.Split('/')[1] + "/" + sDate1.Split('/')[0] + "/" + sDate1.Split('/')[2];
                                        sDate1 = getdt.ConvertDateFormat(sDate1);
                                        CDate1 = Convert.ToDateTime(sDate1);

                                        sDate2 = sDate2.ToString();
                                        //sDate2 = sDate2.Split('/')[1] + "/" + sDate2.Split('/')[0] + "/" + sDate2.Split('/')[2];
                                        sDate2 = getdt.ConvertDateFormat(sDate2);
                                        CDate2 = Convert.ToDateTime(sDate2);

                                        string Status = string.Empty;

                                        if (CDate1.Date > DateTime.Now.Date)
                                        {
                                            Status = "P";
                                        }
                                        else if (CDate1.Date < DateTime.Now.Date && CDate2 > DateTime.Now.Date)
                                        {
                                            Status = "I";
                                        }
                                        else
                                        {
                                            Status = "C";
                                        }

                                        SecondTaskGUID = getdt.GetTaskUID_By_ParentID_TName(new Guid(DDLWorkPackage.SelectedValue), new Guid(FirstTaskGUID), SecondTask);
                                        if (SecondTaskGUID == null)
                                        {
                                            SecondTaskGUID = Guid.NewGuid().ToString();
                                            Guid BOQDetailsUID = new Guid(ds.Tables[0].Rows[i][13].ToString().Trim());
                                            bool result = getdt.InsertorUpdateSubTask(new Guid(SecondTaskGUID), new Guid(DDLWorkPackage.SelectedValue), new Guid(DDlProject.SelectedValue), DDLUsers.SelectedValue, SecondTask, SecondTask, "", "", CDate1 == DateTime.MinValue ? "" : CDate1.ToString(), CDate2 == DateTime.MinValue ? "" : CDate2.ToString(), CDate2 == DateTime.MinValue ? "" : CDate2.ToString(), CDate1 == DateTime.MinValue ? "" : CDate1.ToString(), CDate1 == DateTime.MinValue ? "" : CDate1.ToString(), CDate2 == DateTime.MinValue ? "" : CDate2.ToString(), Status, 0, 0, "", 3, FirstTaskGUID, 0, 0, 0, "Engineering", "", "&#x20B9;", "en-IN", 0, "", new Guid(OptionUID), 0, Guid.Empty,"0",true);
                                            if (result)
                                            {

                                            }
                                        }


                                    }
                                    else if (ds.Tables[0].Rows[i][10].ToString() != "" && SecondTaskGUID != string.Empty)
                                    {
                                        ThirdTask = ds.Tables[0].Rows[i][10].ToString().Trim();

                                        sDate1 = ds.Tables[0].Rows[i][11].ToString().Replace("-", "/");
                                        sDate2 = ds.Tables[0].Rows[i][12].ToString().Replace("-", "/");
                                        //DateTime StartDate = Convert.ToDateTime(sDate1);
                                        //DateTime EndDate = Convert.ToDateTime(sDate2);

                                        sDate1 = sDate1.ToString();
                                        //sDate1 = sDate1.Split('/')[1] + "/" + sDate1.Split('/')[0] + "/" + sDate1.Split('/')[2];
                                        sDate1 = getdt.ConvertDateFormat(sDate1);
                                        CDate1 = Convert.ToDateTime(sDate1);

                                        sDate2 = sDate2.ToString();
                                        //sDate2 = sDate2.Split('/')[1] + "/" + sDate2.Split('/')[0] + "/" + sDate2.Split('/')[2];
                                        sDate2 = getdt.ConvertDateFormat(sDate2);
                                        CDate2 = Convert.ToDateTime(sDate2);
                                        string Status = string.Empty;
                                        if (CDate1.Date > DateTime.Now.Date)
                                        {
                                            Status = "P";
                                        }
                                        else if (CDate1.Date < DateTime.Now.Date && CDate2 > DateTime.Now.Date)
                                        {
                                            Status = "I";
                                        }
                                        else
                                        {
                                            Status = "C";
                                        }
                                        ThirdTaskGUID = getdt.GetTaskUID_By_ParentID_TName(new Guid(DDLWorkPackage.SelectedValue), new Guid(SecondTaskGUID), ThirdTask);
                                        if (ThirdTaskGUID == null)
                                        {
                                            ThirdTaskGUID = Guid.NewGuid().ToString();
                                            Guid BOQDetailsUID = new Guid(ds.Tables[0].Rows[i][14].ToString().Trim());
                                            bool result = getdt.InsertorUpdateSubTask(new Guid(ThirdTaskGUID), new Guid(DDLWorkPackage.SelectedValue), new Guid(DDlProject.SelectedValue), DDLUsers.SelectedValue, ThirdTask, ThirdTask, "", "", CDate1 == DateTime.MinValue ? "" : CDate1.ToString(), CDate2 == DateTime.MinValue ? "" : CDate2.ToString(), CDate2 == DateTime.MinValue ? "" : CDate2.ToString(), CDate1 == DateTime.MinValue ? "" : CDate1.ToString(), CDate1 == DateTime.MinValue ? "" : CDate1.ToString(), CDate2 == DateTime.MinValue ? "" : CDate2.ToString(), Status, 0, 0, "", 4, SecondTaskGUID, 0, 0, 0, "Engineering", "", "&#x20B9;", "en-IN", 0, "", new Guid(OptionUID), 0, Guid.Empty,"0",true);
                                            if (result)
                                            {

                                            }
                                        }


                                    }
                                    else if (ds.Tables[0].Rows[i][11].ToString() != "" && ThirdTaskGUID != string.Empty)
                                    {
                                        FourthTask = ds.Tables[0].Rows[i][11].ToString().Trim();
                                        sDate1 = ds.Tables[0].Rows[i][12].ToString().Replace("-", "/");
                                        sDate2 = ds.Tables[0].Rows[i][13].ToString().Replace("-", "/");
                                        //DateTime StartDate = Convert.ToDateTime(sDate1);
                                        //DateTime EndDate = Convert.ToDateTime(sDate2);

                                        sDate1 = sDate1.ToString();
                                        //sDate1 = sDate1.Split('/')[1] + "/" + sDate1.Split('/')[0] + "/" + sDate1.Split('/')[2];
                                        sDate1 = getdt.ConvertDateFormat(sDate1);
                                        CDate1 = Convert.ToDateTime(sDate1);

                                        sDate2 = sDate2.ToString();
                                        //sDate2 = sDate2.Split('/')[1] + "/" + sDate2.Split('/')[0] + "/" + sDate2.Split('/')[2];
                                        sDate2 = getdt.ConvertDateFormat(sDate2);
                                        CDate2 = Convert.ToDateTime(sDate2);
                                        string Status = string.Empty;
                                        if (CDate1.Date > DateTime.Now.Date)
                                        {
                                            Status = "P";
                                        }
                                        else if (CDate1.Date < DateTime.Now.Date && CDate2 > DateTime.Now.Date)
                                        {
                                            Status = "I";
                                        }
                                        else
                                        {
                                            Status = "C";
                                        }
                                        FourthTaskGUID = getdt.GetTaskUID_By_ParentID_TName(new Guid(DDLWorkPackage.SelectedValue), new Guid(ThirdTaskGUID), FourthTask);
                                        if (FourthTaskGUID == null)
                                        {
                                            Guid BOQDetailsUID = new Guid(ds.Tables[0].Rows[i][15].ToString().Trim());
                                            FourthTaskGUID = Guid.NewGuid().ToString();
                                            bool result = getdt.InsertorUpdateSubTask(new Guid(FourthTaskGUID), new Guid(DDLWorkPackage.SelectedValue), new Guid(DDlProject.SelectedValue), DDLUsers.SelectedValue, FourthTask, FourthTask, "", "", CDate1 == DateTime.MinValue ? "" : CDate1.ToString(), CDate2 == DateTime.MinValue ? "" : CDate2.ToString(), CDate2 == DateTime.MinValue ? "" : CDate2.ToString(), CDate1 == DateTime.MinValue ? "" : CDate1.ToString(), CDate1 == DateTime.MinValue ? "" : CDate1.ToString(), CDate2 == DateTime.MinValue ? "" : CDate2.ToString(), Status, 0, 0, "", 5, ThirdTaskGUID, 0, 0, 0, "Engineering", "", "&#x20B9;", "en-IN", 0, "", new Guid(OptionUID), 0, Guid.Empty,"0",true);
                                            if (result)
                                            {

                                            }
                                        }


                                    }
                                }

                                catch (Exception ex)
                                {
                                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error : Row - " + i + " Main : " + MainTask + FirstTask + SecondTask + ThirdTask + FourthTask + ex.Message + "');</script>");
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error : " + ex.Message + "');</script>");
                    }
                }
                else if (RBList.SelectedValue == "Activity Puttenahalli")
                {
                    try
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {

                            //string OptionUID = "E4D01ABA-2ED2-4B0F-B4CB-2620B980EDAC";
                            string OptionUID = DDLOptions.SelectedValue;
                            string MainTask = string.Empty;
                            string MainTaskGUID = string.Empty;
                            string FirstTask = string.Empty;
                            string FirstTaskGUID = string.Empty;
                            int TaskLevel = 0;
                            string sDate1 = "", sDate2 = "";
                            DateTime CDate1 = DateTime.Now, CDate2 = DateTime.Now;
                            int TotalsuccessCount = 0;
                            int TotalFailureCount = 0;
                            string Status = string.Empty;
                            string FilePath = AppDomain.CurrentDomain.BaseDirectory + "ActivityLogs.txt";
                            System.IO.StreamWriter testfile = null;
                            testfile = new System.IO.StreamWriter(FilePath, false);
                            testfile.WriteLine("Date======================.." + DateTime.Now);
                            int taskOrder = 0;
                            #region old code
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                try
                                {
                                    if (ds.Tables[0].Rows[i][9].ToString() == "Y")
                                    {
                                        if (ds.Tables[0].Rows[i][4].ToString() != "" && (ds.Tables[0].Rows[i][4].ToString() != "&nbsp;"))
                                        {
                                            FirstTask = ds.Tables[0].Rows[i][4].ToString().Trim();
                                            //MainTask = ds.Tables[0].Rows[i][6].ToString().Trim();

                                            sDate1 = ds.Tables[0].Rows[i][7].ToString().Replace("-", "/");
                                            sDate2 = ds.Tables[0].Rows[i][8].ToString().Replace("-", "/");
                                            //DateTime StartDate = Convert.ToDateTime(sDate1);
                                            //DateTime EndDate = Convert.ToDateTime(sDate2);
                                            if (!string.IsNullOrEmpty(sDate1) && !string.IsNullOrEmpty(sDate2))
                                            {
                                                sDate1 = sDate1.ToString();
                                                //sDate1 = sDate1.Split('/')[1] + "/" + sDate1.Split('/')[0] + "/" + sDate1.Split('/')[2];
                                                //sDate1 = getdt.ConvertDateFormat(sDate1);
                                                CDate1 = Convert.ToDateTime(sDate1);

                                                sDate2 = sDate2.ToString();
                                                //sDate2 = sDate2.Split('/')[1] + "/" + sDate2.Split('/')[0] + "/" + sDate2.Split('/')[2];
                                                //sDate2 = getdt.ConvertDateFormat(sDate2);
                                                CDate2 = Convert.ToDateTime(sDate2);
                                               
                                                if (CDate1.Date > DateTime.Now.Date)
                                                {
                                                    Status = "P";
                                                }
                                                else if (CDate1.Date < DateTime.Now.Date && CDate2 > DateTime.Now.Date)
                                                {
                                                    Status = "I";
                                                }
                                                else
                                                {
                                                    Status = "C";
                                                }
                                            }
                                            else
                                            {
                                                Status = "P";
                                            }
                                            int k = 10;
                                            int level = 1;
                                            bool FirstTaskfound = true;
                                            bool taskFound = true;
                                            MainTask = ds.Tables[0].Rows[i][k].ToString().Trim();
                                            MainTaskGUID = getdt.GetTaskUID_By_WorkPackageID_TName(new Guid(DDLWorkPackage.SelectedValue), MainTask);
                                            TaskLevel = level + 1;
                                            level += 1;
                                            while (taskFound)
                                            {
                                                k = k + 1;


                                                if (MainTaskGUID != null)
                                                {
                                                    MainTask = ds.Tables[0].Rows[i][k].ToString().Trim();
                                                    if (!string.IsNullOrEmpty(MainTask))
                                                    {
                                                        FirstTaskGUID = getdt.GetTaskUID_By_ParentID_TName(new Guid(DDLWorkPackage.SelectedValue), new Guid(MainTaskGUID), MainTask);
                                                        //TaskLevel = getdt.GetTaskLevel_By_WorkPackageID_TName(new Guid(DDLWorkPackage.SelectedValue), MainTask);
                                                        if (FirstTaskGUID != null)
                                                        {
                                                            level += 1;
                                                            TaskLevel = level;
                                                            MainTaskGUID = FirstTaskGUID;
                                                            taskFound = true;
                                                            FirstTaskfound = true;
                                                        }
                                                        else
                                                        {
                                                            FirstTaskfound = false;
                                                            taskFound = false;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        taskFound = false;
                                                    }


                                                }
                                                else
                                                {
                                                    taskFound = false;
                                                }

                                            }
                                            if (MainTaskGUID != null)
                                            {

                                                if (FirstTaskfound == true)
                                                {
                                                    taskOrder = taskOrder + 1;
                                                    FirstTaskGUID = Guid.NewGuid().ToString();
                                                    bool result = false;
                                                    if (!string.IsNullOrEmpty(sDate1) && !string.IsNullOrEmpty(sDate2))
                                                    {
                                                        result = getdt.InsertorUpdateSubTaskPuttenahhalli(new Guid(FirstTaskGUID), new Guid(DDLWorkPackage.SelectedValue), new Guid(DDlProject.SelectedValue), DDLUsers.SelectedValue, FirstTask, FirstTask, "", "", CDate1, CDate2, CDate2, CDate1, CDate1, CDate2, Status, 0, 0, "", TaskLevel, MainTaskGUID, 0, 0, 0, "Engineering (From Excel)", "", "&#x20B9;", "en-IN", 0, "", new Guid(OptionUID), 0,taskOrder);
                                                    }
                                                    else
                                                    {
                                                        result = getdt.InsertorUpdateSubTaskPuttenahhalli_withoutdates(new Guid(FirstTaskGUID), new Guid(DDLWorkPackage.SelectedValue), new Guid(DDlProject.SelectedValue), DDLUsers.SelectedValue, FirstTask, FirstTask, "", "", Status, 0, 0, "", TaskLevel, MainTaskGUID, 0, 0, 0, "Engineering (From Excel)", "", "&#x20B9;", "en-IN", 0, "", new Guid(OptionUID), 0,taskOrder);
                                                    }
                                                    if (result)
                                                    {
                                                        TotalsuccessCount += 1;
                                                    }
                                                }
                                                else
                                                {
                                                    TotalFailureCount += 1;
                                                    testfile.WriteLine("Failure.." + "task :- " + FirstTask + ":- Sub TaskUID not found ");
                                                }
                                            }
                                            else
                                            {
                                                TotalFailureCount += 1;
                                                testfile.WriteLine("Failure.." + "task :- " + FirstTask + ":- Main TaskUID not found ");
                                            }
                                        }


                                    }
                                }

                                catch (Exception ex)
                                {

                                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error : Row - " + i + " Main : " + FirstTask + ex.Message + "');</script>");
                                }
                            }

                            #endregion old code

                            #region new Code
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                try
                                {

                                }
                                catch (Exception ex)
                                {

                                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error : Row - " + i + " Main : " + FirstTask + ex.Message + "');</script>");
                                }


                            }

                                # endregion
                                testfile.Close();
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Result : Sucess - " + TotalsuccessCount + " Failure : " + TotalFailureCount + "');</script>");
                        }
                    }
                    catch (Exception ex)
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error : " + ex.Message + "');</script>");
                    }
                }
                else if (RBList.SelectedValue == "Activity New")
                {
                    try
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            string OptionUID = DDLOptions.SelectedValue;
                            string MainTask = string.Empty;
                            string MainTaskGUID = string.Empty;
                            string FirstTask = string.Empty;
                            string FirstTaskGUID = string.Empty;
                            string SecondTask = string.Empty;
                            string SecondTaskGUID = string.Empty;
                            string ThirdTask = string.Empty;
                            string ThirdTaskGUID = string.Empty;
                            string FourthTask = string.Empty;
                            string FourthTaskGUID = string.Empty;
                            string sDate1 = "", sDate2 = "", sDate3 = "", sDate4 = "";
                            DateTime CDate1 = DateTime.Now, CDate2 = DateTime.Now, CDate3 = DateTime.Now, CDate4 = DateTime.Now;

                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                try
                                {
                                    if (ds.Tables[0].Rows[i][0].ToString() != "" && ds.Tables[0].Rows[i][1].ToString() != "")
                                    {
                                        MainTask = ds.Tables[0].Rows[i][1].ToString().Trim();
                                        sDate1 = ds.Tables[0].Rows[i][2].ToString().Replace("-", "/");
                                        sDate2 = ds.Tables[0].Rows[i][3].ToString().Replace("-", "/");
                                        sDate3 = ds.Tables[0].Rows[i][4].ToString().Replace("-", "/");
                                        sDate4 = ds.Tables[0].Rows[i][5].ToString().Replace("-", "/");
                                        //DateTime StartDate = Convert.ToDateTime(sDate1);
                                        //DateTime EndDate = Convert.ToDateTime(sDate2);

                                        sDate1 = sDate1.ToString();
                                        //sDate1 = sDate1.Split('/')[1] + "/" + sDate1.Split('/')[0] + "/" + sDate1.Split('/')[2];
                                        sDate1 = getdt.ConvertDateFormat(sDate1);
                                        CDate1 = Convert.ToDateTime(sDate1);

                                        sDate2 = sDate2.ToString();
                                        //sDate2 = sDate2.Split('/')[1] + "/" + sDate2.Split('/')[0] + "/" + sDate2.Split('/')[2];
                                        sDate2 = getdt.ConvertDateFormat(sDate2);
                                        CDate2 = Convert.ToDateTime(sDate2);

                                        sDate3 = sDate3.ToString();
                                        //sDate1 = sDate1.Split('/')[1] + "/" + sDate1.Split('/')[0] + "/" + sDate1.Split('/')[2];
                                        sDate3 = getdt.ConvertDateFormat(sDate3);
                                        CDate3 = Convert.ToDateTime(sDate3);

                                        sDate4 = sDate4.ToString();
                                        //sDate2 = sDate2.Split('/')[1] + "/" + sDate2.Split('/')[0] + "/" + sDate2.Split('/')[2];
                                        sDate4 = getdt.ConvertDateFormat(sDate4);
                                        CDate4 = Convert.ToDateTime(sDate4);

                                        string Status = string.Empty;
                                        if (CDate1.Date > DateTime.Now.Date)
                                        {
                                            Status = "P";
                                        }
                                        else if (CDate1.Date < DateTime.Now.Date && CDate2 > DateTime.Now.Date)
                                        {
                                            Status = "I";
                                        }
                                        else
                                        {
                                            Status = "C";
                                        }
                                        MainTaskGUID = getdt.GetTaskUID_By_WorkPackageID_TName(new Guid(DDLWorkPackage.SelectedValue), MainTask);
                                        if (MainTaskGUID == null)
                                        {
                                            MainTaskGUID = Guid.NewGuid().ToString();
                                            bool result = getdt.InsertorUpdateMainTask_From_Excel(new Guid(MainTaskGUID), new Guid(DDLWorkPackage.SelectedValue), new Guid(DDlProject.SelectedValue), new Guid(OptionUID), Session["UserUID"].ToString(), MainTask, MainTask, Status, 0, 0, 1, 0, 0, 0, "&#x20B9;", "en-IN", 0, ds.Tables[0].Rows[i][0].ToString().Trim(), CDate1, CDate2, CDate3, CDate4);
                                            if (result)
                                            {

                                            }
                                        }



                                    }
                                    else if (ds.Tables[0].Rows[i][1].ToString() != "" && ds.Tables[0].Rows[i][2].ToString() != "" && MainTaskGUID != string.Empty)
                                    {
                                        FirstTask = ds.Tables[0].Rows[i][2].ToString().Trim();

                                        sDate1 = ds.Tables[0].Rows[i][3].ToString().Replace("-", "/");
                                        sDate2 = ds.Tables[0].Rows[i][4].ToString().Replace("-", "/");
                                        sDate3 = ds.Tables[0].Rows[i][5].ToString().Replace("-", "/");
                                        sDate4 = ds.Tables[0].Rows[i][6].ToString().Replace("-", "/");
                                        //DateTime StartDate = Convert.ToDateTime(sDate1);
                                        //DateTime EndDate = Convert.ToDateTime(sDate2);

                                        sDate1 = sDate1.ToString();
                                        //sDate1 = sDate1.Split('/')[1] + "/" + sDate1.Split('/')[0] + "/" + sDate1.Split('/')[2];
                                        sDate1 = getdt.ConvertDateFormat(sDate1);
                                        CDate1 = Convert.ToDateTime(sDate1);

                                        sDate2 = sDate2.ToString();
                                        //sDate2 = sDate2.Split('/')[1] + "/" + sDate2.Split('/')[0] + "/" + sDate2.Split('/')[2];
                                        sDate2 = getdt.ConvertDateFormat(sDate2);
                                        CDate2 = Convert.ToDateTime(sDate2);

                                        sDate3 = sDate3.ToString();
                                        //sDate1 = sDate1.Split('/')[1] + "/" + sDate1.Split('/')[0] + "/" + sDate1.Split('/')[2];
                                        sDate3 = getdt.ConvertDateFormat(sDate3);
                                        CDate3 = Convert.ToDateTime(sDate3);

                                        sDate4 = sDate4.ToString();
                                        //sDate2 = sDate2.Split('/')[1] + "/" + sDate2.Split('/')[0] + "/" + sDate2.Split('/')[2];
                                        sDate4 = getdt.ConvertDateFormat(sDate4);
                                        CDate4 = Convert.ToDateTime(sDate4);


                                        string Status = string.Empty;
                                        if (CDate1.Date > DateTime.Now.Date)
                                        {
                                            Status = "P";
                                        }
                                        else if (CDate1.Date < DateTime.Now.Date && CDate2 > DateTime.Now.Date)
                                        {
                                            Status = "I";
                                        }
                                        else
                                        {
                                            Status = "C";
                                        }

                                        FirstTaskGUID = getdt.GetTaskUID_By_ParentID_TName(new Guid(DDLWorkPackage.SelectedValue), new Guid(MainTaskGUID), FirstTask);
                                        if (FirstTaskGUID == null)
                                        {
                                            FirstTaskGUID = Guid.NewGuid().ToString();
                                            bool result = getdt.InsertorUpdateSubTask_From_Excel(new Guid(FirstTaskGUID), new Guid(DDLWorkPackage.SelectedValue), new Guid(DDlProject.SelectedValue), new Guid(OptionUID), Session["UserUID"].ToString(), FirstTask, FirstTask, Status, 0, 0, 2, 0, 0, 0, "&#x20B9;", "en-IN", 0, MainTaskGUID, ds.Tables[0].Rows[i][1].ToString().Trim(), CDate1, CDate2, CDate3, CDate4);
                                            if (result)
                                            {

                                            }
                                        }
                                    }
                                    else if (ds.Tables[0].Rows[i][2].ToString() != "" && ds.Tables[0].Rows[i][3].ToString() != "" && FirstTaskGUID != string.Empty)
                                    {
                                        SecondTask = ds.Tables[0].Rows[i][3].ToString().Trim();

                                        sDate1 = ds.Tables[0].Rows[i][4].ToString().Replace("-", "/");
                                        sDate2 = ds.Tables[0].Rows[i][5].ToString().Replace("-", "/");
                                        sDate3 = ds.Tables[0].Rows[i][6].ToString().Replace("-", "/");
                                        sDate4 = ds.Tables[0].Rows[i][7].ToString().Replace("-", "/");
                                        //DateTime StartDate = Convert.ToDateTime(sDate1);
                                        //DateTime EndDate = Convert.ToDateTime(sDate2);

                                        sDate1 = sDate1.ToString();
                                        //sDate1 = sDate1.Split('/')[1] + "/" + sDate1.Split('/')[0] + "/" + sDate1.Split('/')[2];
                                        sDate1 = getdt.ConvertDateFormat(sDate1);
                                        CDate1 = Convert.ToDateTime(sDate1);

                                        sDate2 = sDate2.ToString();
                                        //sDate2 = sDate2.Split('/')[1] + "/" + sDate2.Split('/')[0] + "/" + sDate2.Split('/')[2];
                                        sDate2 = getdt.ConvertDateFormat(sDate2);
                                        CDate2 = Convert.ToDateTime(sDate2);

                                        sDate3 = sDate3.ToString();
                                        //sDate1 = sDate1.Split('/')[1] + "/" + sDate1.Split('/')[0] + "/" + sDate1.Split('/')[2];
                                        sDate3 = getdt.ConvertDateFormat(sDate3);
                                        CDate3 = Convert.ToDateTime(sDate3);

                                        sDate4 = sDate4.ToString();
                                        //sDate2 = sDate2.Split('/')[1] + "/" + sDate2.Split('/')[0] + "/" + sDate2.Split('/')[2];
                                        sDate4 = getdt.ConvertDateFormat(sDate4);
                                        CDate4 = Convert.ToDateTime(sDate4);

                                        string Status = string.Empty;

                                        if (CDate1.Date > DateTime.Now.Date)
                                        {
                                            Status = "P";
                                        }
                                        else if (CDate1.Date < DateTime.Now.Date && CDate2 > DateTime.Now.Date)
                                        {
                                            Status = "I";
                                        }
                                        else
                                        {
                                            Status = "C";
                                        }

                                        SecondTaskGUID = getdt.GetTaskUID_By_ParentID_TName(new Guid(DDLWorkPackage.SelectedValue), new Guid(FirstTaskGUID), SecondTask);
                                        if (SecondTaskGUID == null)
                                        {
                                            SecondTaskGUID = Guid.NewGuid().ToString();

                                            bool result = getdt.InsertorUpdateSubTask_From_Excel(new Guid(SecondTaskGUID), new Guid(DDLWorkPackage.SelectedValue), new Guid(DDlProject.SelectedValue), new Guid(OptionUID), Session["UserUID"].ToString(), SecondTask, SecondTask, Status, 0, 0, 3, 0, 0, 0, "&#x20B9;", "en-IN", 0, FirstTaskGUID, ds.Tables[0].Rows[i][2].ToString().Trim(), CDate1, CDate2, CDate3, CDate4);
                                            if (result)
                                            {

                                            }
                                        }


                                    }
                                    else if (ds.Tables[0].Rows[i][3].ToString() != "" && ds.Tables[0].Rows[i][4].ToString() != "" && SecondTaskGUID != string.Empty)
                                    {
                                        ThirdTask = ds.Tables[0].Rows[i][4].ToString().Trim();

                                        sDate1 = ds.Tables[0].Rows[i][5].ToString().Replace("-", "/");
                                        sDate2 = ds.Tables[0].Rows[i][6].ToString().Replace("-", "/");
                                        sDate3 = ds.Tables[0].Rows[i][7].ToString().Replace("-", "/");
                                        sDate4 = ds.Tables[0].Rows[i][8].ToString().Replace("-", "/");
                                        //DateTime StartDate = Convert.ToDateTime(sDate1);
                                        //DateTime EndDate = Convert.ToDateTime(sDate2);

                                        sDate1 = sDate1.ToString();
                                        //sDate1 = sDate1.Split('/')[1] + "/" + sDate1.Split('/')[0] + "/" + sDate1.Split('/')[2];
                                        sDate1 = getdt.ConvertDateFormat(sDate1);
                                        CDate1 = Convert.ToDateTime(sDate1);

                                        sDate2 = sDate2.ToString();
                                        //sDate2 = sDate2.Split('/')[1] + "/" + sDate2.Split('/')[0] + "/" + sDate2.Split('/')[2];
                                        sDate2 = getdt.ConvertDateFormat(sDate2);
                                        CDate2 = Convert.ToDateTime(sDate2);

                                        sDate3 = sDate3.ToString();
                                        //sDate1 = sDate1.Split('/')[1] + "/" + sDate1.Split('/')[0] + "/" + sDate1.Split('/')[2];
                                        sDate3 = getdt.ConvertDateFormat(sDate3);
                                        CDate3 = Convert.ToDateTime(sDate3);

                                        sDate4 = sDate4.ToString();
                                        //sDate2 = sDate2.Split('/')[1] + "/" + sDate2.Split('/')[0] + "/" + sDate2.Split('/')[2];
                                        sDate4 = getdt.ConvertDateFormat(sDate4);
                                        CDate4 = Convert.ToDateTime(sDate4);


                                        string Status = string.Empty;
                                        if (CDate1.Date > DateTime.Now.Date)
                                        {
                                            Status = "P";
                                        }
                                        else if (CDate1.Date < DateTime.Now.Date && CDate2 > DateTime.Now.Date)
                                        {
                                            Status = "I";
                                        }
                                        else
                                        {
                                            Status = "C";
                                        }
                                        ThirdTaskGUID = getdt.GetTaskUID_By_ParentID_TName(new Guid(DDLWorkPackage.SelectedValue), new Guid(SecondTaskGUID), ThirdTask);
                                        if (ThirdTaskGUID == null)
                                        {
                                            ThirdTaskGUID = Guid.NewGuid().ToString();
                                            bool result = getdt.InsertorUpdateSubTask_From_Excel(new Guid(ThirdTaskGUID), new Guid(DDLWorkPackage.SelectedValue), new Guid(DDlProject.SelectedValue), new Guid(OptionUID), Session["UserUID"].ToString(), ThirdTask, ThirdTask, Status, 0, 0, 4, 0, 0, 0, "&#x20B9;", "en-IN", 0, SecondTaskGUID, ds.Tables[0].Rows[i][3].ToString().Trim(), CDate1, CDate2, CDate3, CDate4);
                                            if (result)
                                            {

                                            }
                                        }


                                    }
                                    else if (ds.Tables[0].Rows[i][5].ToString() != "" && ds.Tables[0].Rows[i][6].ToString() != "" && ThirdTaskGUID != string.Empty)
                                    {
                                        FourthTask = ds.Tables[0].Rows[i][6].ToString().Trim();
                                        sDate1 = ds.Tables[0].Rows[i][7].ToString().Replace("-", "/");
                                        sDate2 = ds.Tables[0].Rows[i][8].ToString().Replace("-", "/");
                                        sDate3 = ds.Tables[0].Rows[i][9].ToString().Replace("-", "/");
                                        sDate4 = ds.Tables[0].Rows[i][10].ToString().Replace("-", "/");
                                        //DateTime StartDate = Convert.ToDateTime(sDate1);
                                        //DateTime EndDate = Convert.ToDateTime(sDate2);

                                        sDate1 = sDate1.ToString();
                                        //sDate1 = sDate1.Split('/')[1] + "/" + sDate1.Split('/')[0] + "/" + sDate1.Split('/')[2];
                                        sDate1 = getdt.ConvertDateFormat(sDate1);
                                        CDate1 = Convert.ToDateTime(sDate1);

                                        sDate2 = sDate2.ToString();
                                        //sDate2 = sDate2.Split('/')[1] + "/" + sDate2.Split('/')[0] + "/" + sDate2.Split('/')[2];
                                        sDate2 = getdt.ConvertDateFormat(sDate2);
                                        CDate2 = Convert.ToDateTime(sDate2);


                                        sDate3 = sDate3.ToString();
                                        //sDate1 = sDate1.Split('/')[1] + "/" + sDate1.Split('/')[0] + "/" + sDate1.Split('/')[2];
                                        sDate3 = getdt.ConvertDateFormat(sDate3);
                                        CDate3 = Convert.ToDateTime(sDate3);

                                        sDate4 = sDate4.ToString();
                                        //sDate2 = sDate2.Split('/')[1] + "/" + sDate2.Split('/')[0] + "/" + sDate2.Split('/')[2];
                                        sDate4 = getdt.ConvertDateFormat(sDate4);
                                        CDate4 = Convert.ToDateTime(sDate4);

                                        string Status = string.Empty;
                                        if (CDate1.Date > DateTime.Now.Date)
                                        {
                                            Status = "P";
                                        }
                                        else if (CDate1.Date < DateTime.Now.Date && CDate2 > DateTime.Now.Date)
                                        {
                                            Status = "I";
                                        }
                                        else
                                        {
                                            Status = "C";
                                        }
                                        FourthTaskGUID = getdt.GetTaskUID_By_ParentID_TName(new Guid(DDLWorkPackage.SelectedValue), new Guid(ThirdTaskGUID), FourthTask);
                                        if (FourthTaskGUID == null)
                                        {

                                            FourthTaskGUID = Guid.NewGuid().ToString();
                                            bool result = getdt.InsertorUpdateSubTask_From_Excel(new Guid(FourthTaskGUID), new Guid(DDLWorkPackage.SelectedValue), new Guid(DDlProject.SelectedValue), new Guid(OptionUID), Session["UserUID"].ToString(), FourthTask, FourthTask, Status, 0, 0, 5, 0, 0, 0, "&#x20B9;", "en-IN", 0, ThirdTaskGUID, ds.Tables[0].Rows[i][4].ToString().Trim(), CDate1, CDate2, CDate3, CDate4);
                                            if (result)
                                            {

                                            }
                                        }


                                    }
                                }

                                catch (Exception ex)
                                {
                                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error : Row - " + i + " Main : " + MainTask + FirstTask + SecondTask + ThirdTask + FourthTask + ex.Message + "');</script>");
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error : " + ex.Message + "');</script>");
                    }
                }
                else if (RBList.SelectedValue == "Document")
                {
                    try
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            string sDate1 = "", sDate2 = "", sDate3 = "", DocStartString = "";
                            DateTime CDate1 = DateTime.Now, CDate2 = DateTime.Now, CDate3 = DateTime.Now, DocStartDate = DateTime.Now;

                            DocStartString = DateTime.Now.ToString("dd/MM/yyyy");
                            //DocStartString = DocStartString.Split('/')[1] + "/" + DocStartString.Split('/')[0] + "/" + DocStartString.Split('/')[2];
                            DocStartString = getdt.ConvertDateFormat(DocStartString);
                            DocStartDate = Convert.ToDateTime(DocStartString);

                            //
                            sDate1 = dtSubTargetDate.Text;
                            //sDate1 = sDate1.Split('/')[1] + "/" + sDate1.Split('/')[0] + "/" + sDate1.Split('/')[2];
                            sDate1 = getdt.ConvertDateFormat(sDate1);
                            CDate1 = Convert.ToDateTime(sDate1);
                            //

                            sDate2 = dtQualTargetDate.Text;
                            //sDate2 = sDate2.Split('/')[1] + "/" + sDate2.Split('/')[0] + "/" + sDate2.Split('/')[2];
                            sDate2 = getdt.ConvertDateFormat(sDate2);
                            CDate2 = Convert.ToDateTime(sDate2);

                            sDate3 = dtRev_B_TargetDate.Text;
                            //sDate3 = sDate3.Split('/')[1] + "/" + sDate3.Split('/')[0] + "/" + sDate3.Split('/')[2];
                            sDate3 = getdt.ConvertDateFormat(sDate3);
                            CDate3 = Convert.ToDateTime(sDate3);

                            int InsertedData = 0;
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                if (ds.Tables[0].Rows[i][1].ToString() != "" && ds.Tables[0].Rows[i][2].ToString() != "")
                                {
                                    string PRefNumber = getdt.GetDocumentSubmittleRef_Number(new Guid(DDlProject.SelectedValue));
                                    string Category = getdt.GetWorkPackageCategoryUID_By_Name(new Guid(DDLWorkPackage.SelectedValue), ds.Tables[0].Rows[i][1].ToString());
                                    if (Category == null || Category == "")
                                    {
                                        Guid wCategory = Guid.NewGuid();
                                        int cnt = getdt.WorkPackageCategory_Insert_or_Update(wCategory, new Guid(DDLWorkPackage.SelectedValue), ds.Tables[0].Rows[i][1].ToString());
                                        if (cnt > 0)
                                        {
                                            int result = getdt.DoumentMaster_Insert_or_Update_Flow_2(Guid.NewGuid(), new Guid(DDLWorkPackage.SelectedValue), new Guid(DDlProject.SelectedValue), Guid.Empty, ds.Tables[0].Rows[i][2].ToString(), wCategory,
                                                PRefNumber, "Submittle Document", 0.0, new Guid(DDLDocumentFlow.SelectedValue), DocStartDate, new Guid(ddlSubmissionUSer.SelectedValue), CDate1,
                                                new Guid(ddlQualityEngg.SelectedValue), CDate2, new Guid(ddlReviewer_B.SelectedValue), CDate3,0,"","","N");
                                            if (result>0)
                                            {
                                                InsertedData = InsertedData + 1;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        string DocumentName = ds.Tables[0].Rows[i][2].ToString();
                                        int result = getdt.DoumentMaster_Insert_or_Update_Flow_2(Guid.NewGuid(), new Guid(DDLWorkPackage.SelectedValue), new Guid(DDlProject.SelectedValue), Guid.Empty, DocumentName, new Guid(Category),
                                                PRefNumber, "Submittle Document", 0.0, new Guid(DDLDocumentFlow.SelectedValue), DocStartDate, new Guid(ddlSubmissionUSer.SelectedValue), CDate1,
                                                new Guid(ddlQualityEngg.SelectedValue), CDate2, new Guid(ddlReviewer_B.SelectedValue), CDate3,0,"","","N");
                                        if (result>0)
                                        {
                                            InsertedData = InsertedData + 1;
                                        }
                                    }
                                }
                            }

                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Total Records Inserted " + InsertedData + "');</script>");
                        }
                    }
                    catch (Exception ex)
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error : " + ex.Message + "');</script>");
                    }
                }
                else if (RBList.SelectedValue == "Construction Program")
                {
                    try
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            string OptionUID = DDLOptions.SelectedValue;
                            string TaskName = "Construction Program";
                            Guid MainTaskUID = Guid.NewGuid();
                            string Task_UID = "";
                            string FirstTaksUID = "";
                            string SecondTaskUID = "";
                            string ThirdTaskUID = "";
                            string TaskSection = "";
                            string UnitofProgress = "";
                            bool result = getdt.InsertorUpdateMainTask_From_Master(MainTaskUID, new Guid(DDLWorkPackage.SelectedValue), new Guid(DDlProject.SelectedValue), new Guid(OptionUID), Session["UserUID"].ToString(), TaskName, TaskName, "I", 0, 0, 1, 0, 0, 0, "&#x20B9;", "en-IN", 0);
                            if (result)
                            {

                                for (int i = 1; i < ds.Tables[0].Rows.Count; i++)
                                {
                                    bool InsertSchedule = false;
                                    if (ds.Tables[0].Rows[i][0].ToString() != "" && ds.Tables[0].Rows[i][1].ToString() != "")
                                    {
                                        FirstTaksUID = Guid.NewGuid().ToString();
                                        Task_UID = FirstTaksUID;
                                        TaskName = ds.Tables[0].Rows[i][1].ToString();
                                        TaskSection = ds.Tables[0].Rows[i][0].ToString();
                                        UnitofProgress = ds.Tables[0].Rows[i][2].ToString();
                                        result = getdt.InsertorUpdateSubTask_From_Excel_For_ConstructionProgram(new Guid(FirstTaksUID), new Guid(DDLWorkPackage.SelectedValue), new Guid(DDlProject.SelectedValue), new Guid(OptionUID), Session["UserUID"].ToString(), TaskName, TaskName, "I", 0, 0, 2, 0, 0, 0, "&#x20B9;", "en-IN", 0, MainTaskUID.ToString(), TaskSection, UnitofProgress);
                                        if (result)
                                        {
                                            if (ds.Tables[0].Rows[i][11].ToString() != "" || ds.Tables[0].Rows[i][12].ToString() != "" || ds.Tables[0].Rows[i][13].ToString() != "" || ds.Tables[0].Rows[i][14].ToString() != "" || ds.Tables[0].Rows[i][15].ToString() != "" || ds.Tables[0].Rows[i][16].ToString() != "" || ds.Tables[0].Rows[i][17].ToString() != "" || ds.Tables[0].Rows[i][18].ToString() != "" || ds.Tables[0].Rows[i][19].ToString() != "" || ds.Tables[0].Rows[i][20].ToString() != "" || ds.Tables[0].Rows[i][21].ToString() != "" || ds.Tables[0].Rows[i][22].ToString() != "" || ds.Tables[0].Rows[i][23].ToString() != "")
                                            {
                                                InsertSchedule = true;
                                            }
                                        }
                                    }
                                    else if (FirstTaksUID != string.Empty && ds.Tables[0].Rows[i][1].ToString() != "" && ds.Tables[0].Rows[i][2].ToString() != "")
                                    {
                                        SecondTaskUID = Guid.NewGuid().ToString();
                                        Task_UID = SecondTaskUID;
                                        TaskName = ds.Tables[0].Rows[i][2].ToString();
                                        TaskSection = ds.Tables[0].Rows[i][1].ToString();
                                        UnitofProgress = ds.Tables[0].Rows[i][3].ToString();

                                        result = getdt.InsertorUpdateSubTask_From_Excel_For_ConstructionProgram(new Guid(SecondTaskUID), new Guid(DDLWorkPackage.SelectedValue), new Guid(DDlProject.SelectedValue), new Guid(OptionUID), Session["UserUID"].ToString(), TaskName, TaskName, "I", 0, 0, 3, 0, 0, 0, "&#x20B9;", "en-IN", 0, FirstTaksUID.ToString(), TaskSection, UnitofProgress);
                                        if (result)
                                        {
                                            if (ds.Tables[0].Rows[i][11].ToString() != "" || ds.Tables[0].Rows[i][12].ToString() != "" || ds.Tables[0].Rows[i][13].ToString() != "" || ds.Tables[0].Rows[i][14].ToString() != "" || ds.Tables[0].Rows[i][15].ToString() != "" || ds.Tables[0].Rows[i][16].ToString() != "" || ds.Tables[0].Rows[i][17].ToString() != "" || ds.Tables[0].Rows[i][18].ToString() != "" || ds.Tables[0].Rows[i][19].ToString() != "" || ds.Tables[0].Rows[i][20].ToString() != "" || ds.Tables[0].Rows[i][21].ToString() != "" || ds.Tables[0].Rows[i][22].ToString() != "" || ds.Tables[0].Rows[i][23].ToString() != "")
                                            {
                                                InsertSchedule = true;
                                            }
                                        }
                                    }
                                    else if (SecondTaskUID !=string.Empty && ds.Tables[0].Rows[i][2].ToString() != "" && ds.Tables[0].Rows[i][3].ToString() != "" && ds.Tables[0].Rows[i][4].ToString() == "" && ds.Tables[0].Rows[i][5].ToString() == "" && ds.Tables[0].Rows[i][6].ToString() == "" && ds.Tables[0].Rows[i][7].ToString() == "")
                                    {
                                        ThirdTaskUID = Guid.NewGuid().ToString();
                                        Task_UID = ThirdTaskUID;
                                        TaskName = ds.Tables[0].Rows[i][3].ToString();
                                        TaskSection = ds.Tables[0].Rows[i][2].ToString();
                                        UnitofProgress = ds.Tables[0].Rows[i][4].ToString();

                                        result = getdt.InsertorUpdateSubTask_From_Excel_For_ConstructionProgram(new Guid(ThirdTaskUID), new Guid(DDLWorkPackage.SelectedValue), new Guid(DDlProject.SelectedValue), new Guid(OptionUID), Session["UserUID"].ToString(), TaskName, TaskName, "I", 0, 0, 4, 0, 0, 0, "&#x20B9;", "en-IN", 0, SecondTaskUID.ToString(), TaskSection, UnitofProgress);
                                        if (result)
                                        {
                                            if (ds.Tables[0].Rows[i][11].ToString() != "" || ds.Tables[0].Rows[i][12].ToString() != "" || ds.Tables[0].Rows[i][13].ToString() != "" || ds.Tables[0].Rows[i][14].ToString() != "" || ds.Tables[0].Rows[i][15].ToString() != "" || ds.Tables[0].Rows[i][16].ToString() != "" || ds.Tables[0].Rows[i][17].ToString() != "" || ds.Tables[0].Rows[i][18].ToString() != "" || ds.Tables[0].Rows[i][19].ToString() != "" || ds.Tables[0].Rows[i][20].ToString() != "" || ds.Tables[0].Rows[i][21].ToString() != "" || ds.Tables[0].Rows[i][22].ToString() != "" || ds.Tables[0].Rows[i][23].ToString() != "")
                                            {
                                                InsertSchedule = true;
                                            }
                                        }
                                    }

                                    if (InsertSchedule)
                                    {
                                        bool ver = getdt.InsertorUpdateTaskScheduleVesrion(Guid.NewGuid(), new Guid(Task_UID), "Month");
                                        if (ver)
                                        {
                                            string sDate1 = "", sDate2 = "";
                                            DateTime CDate1 = DateTime.Now, CDate2 = DateTime.Now;
                                            for(int j=11;j<ds.Tables[0].Rows[i].Table.Columns.Count;j++)
                                            {
                                                if (ds.Tables[0].Columns[j].ToString() != "")
                                                {
                                                    sDate1 = ds.Tables[0].Rows[0][j].ToString().Replace("-", "/");
                                                    //sDate1 = sDate1.Split('/')[1] + "/" + sDate1.Split('/')[0] + "/" + sDate1.Split('/')[2];
                                                    sDate1 = getdt.ConvertDateFormat(sDate1);
                                                    CDate1 = Convert.ToDateTime(sDate1);

                                                    int Days = DateTime.DaysInMonth(CDate1.Year, CDate1.Month);
                                                    CDate2 = CDate1.AddDays(Days);

                                                    float Val = float.Parse(ds.Tables[0].Columns[j].ToString());
                                                    bool sresult = getdt.InsertorUpdateTaskSchedule(Guid.NewGuid(), new Guid(DDLWorkPackage.SelectedValue), new Guid(Task_UID), CDate1, CDate2, Val, "Month");
                                                    if (sresult)
                                                    {

                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            
                        }
                    }
                    catch (Exception ex)
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error : " + ex.Message + "');</script>");
                    }
                }
                else
                {
                    try
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            string WorkpackageUID = DDLWorkPackage.SelectedValue;
                            string MainTask = string.Empty;
                            string MainTaskGUID = string.Empty;
                            string SubTask = string.Empty;
                            string SubTaskUID = string.Empty;
                            double Quantity = 0;
                            string Unit = "";
                            double Price = 0;
                            string Currency = "";
                            string Currency_CultureInfo = "";
                            float GST = 0;
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                try
                                {
                                    if (ds.Tables[0].Rows[i][0].ToString() != "" && ds.Tables[0].Rows[i][1].ToString() != "")
                                    {
                                        MainTaskGUID = getdt.GetBOQDetails_by_Name_WorkpackageUID_ItemNumber(new Guid(WorkpackageUID), ds.Tables[0].Rows[i][1].ToString(), ds.Tables[0].Rows[i][0].ToString());
                                        if (MainTaskGUID == null)
                                        {
                                            MainTaskGUID = Guid.NewGuid().ToString();
                                            if (ds.Tables[0].Rows[i][2].ToString() != "")
                                            {
                                                Quantity = Convert.ToDouble(ds.Tables[0].Rows[i][2].ToString());
                                            }
                                            if (ds.Tables[0].Rows[i][3].ToString() != "")
                                            {
                                                GST = float.Parse(ds.Tables[0].Rows[i][3].ToString());
                                            }
                                            if (ds.Tables[0].Rows[i][4].ToString() != "")
                                            {
                                                Unit = ds.Tables[0].Rows[i][4].ToString();
                                            }
                                            if (ds.Tables[0].Rows[i][5].ToString() != "")
                                            {
                                                Price = Convert.ToDouble(ds.Tables[0].Rows[i][5].ToString());
                                            }
                                            if (ds.Tables[0].Rows[i][6].ToString() == "INR")
                                            {
                                                Currency = "&#x20B9;";
                                                Currency_CultureInfo = "en-IN";
                                            }
                                            else if (ds.Tables[0].Rows[i][6].ToString() == "USD")
                                            {
                                                Currency = "&#36;";
                                                Currency_CultureInfo = "en-US";
                                            }
                                            else
                                            {
                                                Currency = "&#165;";
                                                Currency_CultureInfo = "ja-JP";
                                            }
                                            int cnt = getdt.InsertorUpdateBOQDetails_Main(new Guid(MainTaskGUID), new Guid(WorkpackageUID), ds.Tables[0].Rows[i][0].ToString(), ds.Tables[0].Rows[i][1].ToString(), Quantity, GST, Price, Unit, Currency, Currency_CultureInfo);
                                            if (cnt > 0)
                                            {
                                            }
                                        }
                                    }
                                    else if (ds.Tables[0].Rows[i][1].ToString() != "" && ds.Tables[0].Rows[i][2].ToString() != "" && MainTaskGUID != string.Empty)
                                    {
                                        SubTaskUID = getdt.GetBOQDetails_by_Name_WorkpackageUID_ItemNumber(new Guid(WorkpackageUID), ds.Tables[0].Rows[i][2].ToString(), ds.Tables[0].Rows[i][1].ToString());
                                        if (SubTaskUID == null)
                                        {
                                            SubTaskUID = Guid.NewGuid().ToString();
                                            if (ds.Tables[0].Rows[i][3].ToString() != "")
                                            {
                                                Quantity = Convert.ToDouble(ds.Tables[0].Rows[i][3].ToString());
                                            }
                                            if (ds.Tables[0].Rows[i][4].ToString() != "")
                                            {
                                                GST = float.Parse(ds.Tables[0].Rows[i][4].ToString());
                                            }
                                            if (ds.Tables[0].Rows[i][5].ToString() != "")
                                            {
                                                Unit = ds.Tables[0].Rows[i][5].ToString();
                                            }
                                            if (ds.Tables[0].Rows[i][6].ToString() != "")
                                            {
                                                Price = Convert.ToDouble(ds.Tables[0].Rows[i][6].ToString());
                                            }
                                            if (ds.Tables[0].Rows[i][7].ToString() == "INR")
                                            {
                                                Currency = "&#x20B9;";
                                                Currency_CultureInfo = "en-IN";
                                            }
                                            else if (ds.Tables[0].Rows[i][7].ToString() == "USD")
                                            {
                                                Currency = "&#36;";
                                                Currency_CultureInfo = "en-US";
                                            }
                                            else
                                            {
                                                Currency = "&#165;";
                                                Currency_CultureInfo = "ja-JP";
                                            }
                                            int cnt = getdt.InsertorUpdateBOQDetails_Sub(new Guid(SubTaskUID), new Guid(WorkpackageUID), ds.Tables[0].Rows[i][1].ToString(), ds.Tables[0].Rows[i][2].ToString(), Quantity, GST, Price, Unit, Currency, Currency_CultureInfo, new Guid(MainTaskGUID));
                                            if (cnt > 0)
                                            {
                                            }
                                        }
                                    }
                                    else if (ds.Tables[0].Rows[i][2].ToString() != "" && ds.Tables[0].Rows[i][3].ToString() != "" && SubTaskUID != string.Empty)
                                    {
                                        SubTaskUID = getdt.GetBOQDetails_by_Name_WorkpackageUID_ItemNumber(new Guid(WorkpackageUID), ds.Tables[0].Rows[i][3].ToString(), ds.Tables[0].Rows[i][2].ToString());
                                        if (SubTaskUID == null)
                                        {
                                            SubTaskUID = Guid.NewGuid().ToString();
                                            if (ds.Tables[0].Rows[i][4].ToString() != "")
                                            {
                                                Quantity = Convert.ToDouble(ds.Tables[0].Rows[i][4].ToString());
                                            }
                                            if (ds.Tables[0].Rows[i][5].ToString() != "")
                                            {
                                                GST = float.Parse(ds.Tables[0].Rows[i][5].ToString());
                                            }
                                            if (ds.Tables[0].Rows[i][6].ToString() != "")
                                            {
                                                Unit = ds.Tables[0].Rows[i][6].ToString();
                                            }
                                            if (ds.Tables[0].Rows[i][7].ToString() != "")
                                            {
                                                Price = Convert.ToDouble(ds.Tables[0].Rows[i][7].ToString());
                                            }
                                            if (ds.Tables[0].Rows[i][8].ToString() == "INR")
                                            {
                                                Currency = "&#x20B9;";
                                                Currency_CultureInfo = "en-IN";
                                            }
                                            else if (ds.Tables[0].Rows[i][8].ToString() == "USD")
                                            {
                                                Currency = "&#36;";
                                                Currency_CultureInfo = "en-US";
                                            }
                                            else
                                            {
                                                Currency = "&#165;";
                                                Currency_CultureInfo = "ja-JP";
                                            }
                                            int cnt = getdt.InsertorUpdateBOQDetails_Sub(new Guid(SubTaskUID), new Guid(WorkpackageUID), ds.Tables[0].Rows[i][2].ToString(), ds.Tables[0].Rows[i][3].ToString(), Quantity, GST, Price, Unit, Currency, Currency_CultureInfo, new Guid(MainTaskGUID));
                                            if (cnt > 0)
                                            {
                                            }
                                        }
                                    }
                                    else if (ds.Tables[0].Rows[i][4].ToString() != "" && ds.Tables[0].Rows[i][5].ToString() != "" && SubTaskUID != string.Empty)
                                    {
                                        SubTaskUID = getdt.GetBOQDetails_by_Name_WorkpackageUID_ItemNumber(new Guid(WorkpackageUID), ds.Tables[0].Rows[i][5].ToString(), ds.Tables[0].Rows[i][4].ToString());
                                        if (SubTaskUID == null)
                                        {
                                            SubTaskUID = Guid.NewGuid().ToString();
                                            if (ds.Tables[0].Rows[i][6].ToString() != "")
                                            {
                                                Quantity = Convert.ToDouble(ds.Tables[0].Rows[i][6].ToString());
                                            }
                                            if (ds.Tables[0].Rows[i][7].ToString() != "")
                                            {
                                                GST = float.Parse(ds.Tables[0].Rows[i][7].ToString());
                                            }
                                            if (ds.Tables[0].Rows[i][8].ToString() != "")
                                            {
                                                Unit = ds.Tables[0].Rows[i][8].ToString();
                                            }
                                            if (ds.Tables[0].Rows[i][9].ToString() != "")
                                            {
                                                Price = Convert.ToDouble(ds.Tables[0].Rows[i][9].ToString());
                                            }
                                            if (ds.Tables[0].Rows[i][10].ToString() == "INR")
                                            {
                                                Currency = "&#x20B9;";
                                                Currency_CultureInfo = "en-IN";
                                            }
                                            else if (ds.Tables[0].Rows[i][10].ToString() == "USD")
                                            {
                                                Currency = "&#36;";
                                                Currency_CultureInfo = "en-US";
                                            }
                                            else
                                            {
                                                Currency = "&#165;";
                                                Currency_CultureInfo = "ja-JP";
                                            }
                                            int cnt = getdt.InsertorUpdateBOQDetails_Sub(new Guid(SubTaskUID), new Guid(WorkpackageUID), ds.Tables[0].Rows[i][4].ToString(), ds.Tables[0].Rows[i][5].ToString(), Quantity, GST, Price, Unit, Currency, Currency_CultureInfo, new Guid(MainTaskGUID));
                                            if (cnt > 0)
                                            {
                                            }
                                        }
                                    }
                                    else if (ds.Tables[0].Rows[i][6].ToString() != "" && ds.Tables[0].Rows[i][7].ToString() != "" && SubTaskUID != string.Empty)
                                    {

                                    }
                                }
                                catch (Exception ex)
                                {

                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error : " + ex.Message + "');</script>");
                    }
                }

            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Please upload Excel file');</script>");
            }
        }

        public DataTable ReadExcel(string fileName, string fileExt)
        {
            DataTable dtData = new DataTable();
            try
            {
                

                string conn = string.Empty;
                DataTable dtexcel = new DataTable();
                if (fileExt.CompareTo(".xls") == 0)
                    conn = @"provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileName + ";Extended Properties='Excel 8.0;HRD=Yes;IMEX=1';"; //for below excel 2007  
                else
                    conn = String.Format(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileName + ";Extended Properties='Excel 12.0 Xml;HDR=YES'");
                using (OleDbConnection con = new OleDbConnection(conn))
                {
                    try
                    {
                        OleDbDataAdapter oleAdpt = new OleDbDataAdapter("select * from [Sheet1$]", con); //here we read data from sheet1  
                        oleAdpt.Fill(dtexcel); //fill excel data into dataTable
                        Boolean datafound = false;
                        dtData.Columns.Add("Page");
                        dtData.Columns.Add("Bill No.");
                        dtData.Columns.Add("Description");
                        dtData.Columns.Add("Local(INR)-Amount");
                        dtData.Columns.Add("Foreign(JPY)-Amount");
                        dtData.Columns.Add("Foreign(USD)-Amount");
                        dtData.Columns.Add("uid");
                        foreach (DataRow dr in dtexcel.Rows)
                        {
                            if (datafound && dr.ItemArray[0].ToString() != "")
                            {
                                DataRow dtcolumns = dtData.NewRow();
                                for (int i = 0; i < dr.ItemArray.Length; i++)
                                {
                                    dtcolumns[i] = dr.ItemArray[i];
                                }
                                dtcolumns["uid"] = Guid.NewGuid();
                                dtData.Rows.Add(dtcolumns);
                            }
                            else if (dr.ItemArray[0].ToString() == "Page")
                            {
                                datafound = true;

                            }

                        }
                        foreach (DataRow dr in dtData.Rows)
                        {

                            //dbobj.UpdateBOQDetails(dr["uid"].ToString(),
                            //           workPackageId,
                            //           dr["Bill No."].ToString(),
                            //           dr["Description"].ToString(),
                            //          "",
                            //           "",
                            //          "",
                            //           "",
                            //           "",
                            //           dr["Local(INR)-Amount"].ToString(),
                            //           dr["Foreign(JPY)-Amount"].ToString(),
                            //           dr["Foreign(USD)-Amount"].ToString(),
                            //           "",
                            //           projectId, "Grand Summary");
                            //boqObj.processPageData(dr["Page"].ToString(), dr["Uid"].ToString(), fileName, fileExt);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return dtData;
        }

        protected void RBList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RBList.SelectedValue == "Activity" || RBList.SelectedValue == "Activity New" || RBList.SelectedValue == "Construction Program" || RBList.SelectedValue == "Activity Puttenahalli")
            {
                TaskOption.Visible = true;
            }
            else
            {
                TaskOption.Visible = false;
            }

        }
    }
}