using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagementTool._modal_pages
{
    public partial class add_activitydatafromexcel : System.Web.UI.Page
    {
        DBGetData getdata = new DBGetData();
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
                    GetOptionName(Request.QueryString["OptionUID"]);
                }
            }
        }
        private void GetOptionName(string OptionUID)
        {
            DataSet ds = getdata.Workpackageoption_SelectBy_UID(new Guid(OptionUID));
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtoptionname.Text = ds.Tables[0].Rows[0]["Workpackage_OptionName"].ToString();
            }
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
                query = "SELECT * FROM [Sheet1$]";
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

                try
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        string MainTask = string.Empty;
                        string MainTaskGUID = string.Empty;
                        int MainTaskOrder = 0;
                        string FirstTask = string.Empty;
                        string FirstTaskGUID = string.Empty;
                        int FirstTaskOrder = 0;
                        string SecondTask = string.Empty;
                        string SecondTaskGUID = string.Empty;
                        int SecondTaskOrder = 0;
                        string ThirdTask = string.Empty;
                        string ThirdTaskGUID = string.Empty;
                        int ThirdTaskOrder = 0;
                        string FourthTask = string.Empty;
                        string FourthTaskGUID = string.Empty;
                        int FourthTaskOrder = 0;

                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            try
                            {
                                if (ds.Tables[0].Rows[i][0].ToString() != "")
                                {
                                    MainTask = ds.Tables[0].Rows[i][0].ToString().Trim();


                                    MainTaskGUID = getdata.WorkpackageActivityMaster_SelectBy_Name(new Guid(Request.QueryString["OptionUID"]), MainTask);
                                    if (MainTaskGUID == null)
                                    {
                                        MainTaskOrder = MainTaskOrder + 1;
                                        MainTaskGUID = Guid.NewGuid().ToString();
                                        //insert

                                        int cnt = getdata.WorkpackageActivityMasterMainActivity_Insert(new Guid(MainTaskGUID), new Guid(Request.QueryString["OptionUID"]), MainTask, MainTaskOrder);
                                        if (cnt <= 0)
                                        {
                                            string error = "Error" + MainTask;
                                        }
                                    }
                                }
                                else if (ds.Tables[0].Rows[i][1].ToString() != "" && MainTaskGUID != string.Empty)
                                {
                                    FirstTask = ds.Tables[0].Rows[i][1].ToString().Trim();

                                    FirstTaskGUID = getdata.WorkpackageActivityMasterParent_SelectBy_Name(new Guid(Request.QueryString["OptionUID"]), new Guid(MainTaskGUID), FirstTask);
                                    if (FirstTaskGUID == null)
                                    {
                                        FirstTaskGUID = Guid.NewGuid().ToString();
                                        FirstTaskOrder = FirstTaskOrder + 1;

                                        int cnt = getdata.WorkpackageActivityMasterSubActivity_Insert(new Guid(FirstTaskGUID), new Guid(Request.QueryString["OptionUID"]), FirstTask, new Guid(MainTaskGUID), FirstTaskOrder,2);
                                        if (cnt <= 0)
                                        {
                                            string error = "Error" + FirstTask;
                                        }

                                    }
                                }
                                else if (ds.Tables[0].Rows[i][2].ToString() != "" && FirstTaskGUID != string.Empty)
                                {
                                    SecondTask = ds.Tables[0].Rows[i][2].ToString().Trim();

                                    SecondTaskGUID = getdata.WorkpackageActivityMasterParent_SelectBy_Name(new Guid(Request.QueryString["OptionUID"]), new Guid(FirstTaskGUID), SecondTask);
                                    if (SecondTaskGUID == null)
                                    {
                                        SecondTaskGUID = Guid.NewGuid().ToString();
                                        SecondTaskOrder = SecondTaskOrder + 1;
                                        int cnt = getdata.WorkpackageActivityMasterSubActivity_Insert(new Guid(SecondTaskGUID), new Guid(Request.QueryString["OptionUID"]), SecondTask, new Guid(FirstTaskGUID), SecondTaskOrder,3);
                                        if (cnt <= 0)
                                        {
                                            string error = "Error" + SecondTask;
                                        }
                                    }


                                }
                                else if (ds.Tables[0].Rows[i][3].ToString() != "" && SecondTaskGUID != string.Empty)
                                {
                                    ThirdTask = ds.Tables[0].Rows[i][3].ToString().Trim();

                                    ThirdTaskGUID = getdata.WorkpackageActivityMasterParent_SelectBy_Name(new Guid(Request.QueryString["OptionUID"]), new Guid(SecondTaskGUID), ThirdTask);
                                    if (ThirdTaskGUID == null)
                                    {
                                        ThirdTaskGUID = Guid.NewGuid().ToString();
                                        ThirdTaskOrder = ThirdTaskOrder + 1;
                                        int cnt = getdata.WorkpackageActivityMasterSubActivity_Insert(new Guid(ThirdTaskGUID), new Guid(Request.QueryString["OptionUID"]), ThirdTask, new Guid(SecondTaskGUID), ThirdTaskOrder,4);
                                        if (cnt <= 0)
                                        {
                                            string error = "Error" + ThirdTask;
                                        }
                                    }


                                }
                                else if (ds.Tables[0].Rows[i][4].ToString() != "" && ThirdTaskGUID != string.Empty)
                                {
                                    FourthTask = ds.Tables[0].Rows[i][4].ToString().Trim();

                                    FourthTaskGUID = getdata.WorkpackageActivityMasterParent_SelectBy_Name(new Guid(Request.QueryString["OptionUID"]), new Guid(ThirdTaskGUID), FourthTask);
                                    if (FourthTaskGUID == null)
                                    {
                                        FourthTaskGUID = Guid.NewGuid().ToString();
                                        FourthTaskOrder = FourthTaskOrder + 1;
                                        int cnt = getdata.WorkpackageActivityMasterSubActivity_Insert(new Guid(FourthTaskGUID), new Guid(Request.QueryString["OptionUID"]), FourthTask, new Guid(ThirdTaskGUID), FourthTaskOrder,5);
                                        if (cnt <= 0)
                                        {
                                            string error = "Error" + FourthTask;
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
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Please upload Excel file');</script>");
            }
        }
    }
}