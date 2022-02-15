using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProjectManager.DAL;

namespace ProjectManagementTool
{
    public partial class CashFlowExcelUpdate : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        //protected void btnSubmit_Click(object sender, EventArgs e) //Template
        //{
        //    try
        //    {
        //        string connectionString = "";

        //        string fileName = Path.GetFileName(FileUpload1.PostedFile.FileName);
        //        string fileExtension = Path.GetExtension(FileUpload1.PostedFile.FileName);
        //        string fileLocation = Server.MapPath("~/RegExcel/" + fileName);
        //        FileUpload1.SaveAs(fileLocation);

        //        //Check whether file extension is xls or xslx

        //        if (fileExtension == ".xls")
        //        {
        //            connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileLocation + @";Extended Properties=" + Convert.ToChar(34).ToString() + @"Excel 8.0;Imex=1;HDR=Yes;" + Convert.ToChar(34).ToString();
        //        }
        //        else if (fileExtension == ".xlsx")
        //        {
        //            connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileLocation + @";Extended Properties=" + Convert.ToChar(34).ToString() + @"Excel 8.0;Imex=1;HDR=Yes;" + Convert.ToChar(34).ToString();
        //        }

        //        //Create OleDB Connection and OleDb Command

        //        OleDbConnection con = new OleDbConnection(connectionString);
        //        OleDbCommand cmd = new OleDbCommand();
        //        cmd.CommandType = System.Data.CommandType.Text;
        //        cmd.Connection = con;
        //        OleDbDataAdapter dAdapter = new OleDbDataAdapter(cmd);
        //        DataTable dtExcelRecords = new DataTable();
        //        con.Open();
        //        DataTable dtExcelSheetName = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
        //        string getExcelSheetName = dtExcelSheetName.Rows[0]["Table_Name"].ToString();
        //        cmd.CommandText = "SELECT * FROM [" + getExcelSheetName + "]";
        //        dAdapter.SelectCommand = cmd;
        //        dAdapter.Fill(dtExcelRecords);
        //        con.Close();
        //        grdExcelData.DataSource = dtExcelRecords;
        //        grdExcelData.DataBind();
        //        //

        //        string Description = string.Empty;
        //        string Quantity = string.Empty;
        //        string[] Month = new string[30];
        //        Guid UID = new Guid();
        //        float AllowedPayment = 0.0f;
        //        int[] Year = new int[50];
        //        int OrderBy = 0;
        //        Guid WorkPackageUID = new Guid("28a6a63b-2573-40a8-bc89-e396c31ce516");

        //        //
        //        for (int i = 3; i < 4; i++)
        //        {
        //            for (int j = 2; j < dtExcelRecords.Columns.Count; j++)
        //            {
        //                Month[j] = dtExcelRecords.Rows[i - 1][j].ToString().Split('-')[1];
        //                Year[j] = int.Parse(dtExcelRecords.Rows[i - 1][j].ToString().Split('-')[0]);

        //            }

        //        }



        //            for (int i = 3; i < dtExcelRecords.Rows.Count; i++)
        //        {
        //            OrderBy = 0;
        //            if (!string.IsNullOrEmpty(dtExcelRecords.Rows[i][1].ToString()))
        //            {
        //                Description = dtExcelRecords.Rows[i][1].ToString();

        //            }
        //            // This is Financial_MileStoneUID
        //            UID = Guid.NewGuid();
        //            DBGetData getdata = new DBGetData();
        //            // for updating Financial Milestone.......
        //            int result = getdata.InsertFinMilestoneExcel(UID, WorkPackageUID, Description);
        //            // loop thru all columns for updating months data to financialMilestoneMonth table
        //            for(int j=2;j<dtExcelRecords.Columns.Count;j++)
        //            {
        //                OrderBy = OrderBy + 1;
        //                if (!string.IsNullOrEmpty(dtExcelRecords.Rows[i][j].ToString()))
        //                {
        //                    AllowedPayment = float.Parse(dtExcelRecords.Rows[i][j].ToString());

        //                    // 
        //                    if (AllowedPayment > 0)
        //                    {
        //                        result = getdata.InsertFinMilestoneMonthExcel(Guid.NewGuid(), UID, AllowedPayment, Month[j], Year[j],WorkPackageUID,OrderBy);
        //                    }
        //                }
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Response.Write("Error " + ex.Message);
        //    }
        //}

        protected void btnSubmit_Click(object sender, EventArgs e) //Template for CP-10 reading
        {
            try
            {
                string connectionString = "";

                string fileName = Path.GetFileName(FileUpload1.PostedFile.FileName);
                string fileExtension = Path.GetExtension(FileUpload1.PostedFile.FileName);
                string fileLocation = Server.MapPath("~/RegExcel/" + fileName);
                FileUpload1.SaveAs(fileLocation);

                //Check whether file extension is xls or xslx

                if (fileExtension == ".xls")
                {
                    connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileLocation + @";Extended Properties=" + Convert.ToChar(34).ToString() + @"Excel 8.0;Imex=1;HDR=Yes;" + Convert.ToChar(34).ToString();
                }
                else if (fileExtension == ".xlsx")
                {
                    connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileLocation + @";Extended Properties=" + Convert.ToChar(34).ToString() + @"Excel 8.0;Imex=1;HDR=Yes;" + Convert.ToChar(34).ToString();
                }

                //Create OleDB Connection and OleDb Command

                OleDbConnection con = new OleDbConnection(connectionString);
                OleDbCommand cmd = new OleDbCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = con;
                OleDbDataAdapter dAdapter = new OleDbDataAdapter(cmd);
                DataTable dtExcelRecords = new DataTable();
                con.Open();
                DataTable dtExcelSheetName = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                string getExcelSheetName = "Statement - 2$"; dtExcelSheetName.Rows[0]["Table_Name"].ToString();
                cmd.CommandText = "SELECT * FROM [" + getExcelSheetName + "]";
                dAdapter.SelectCommand = cmd;
                dAdapter.Fill(dtExcelRecords);
                con.Close();
                grdExcelData.DataSource = dtExcelRecords;
                grdExcelData.DataBind();
                //

                string Description = string.Empty;
                string Quantity = string.Empty;
                string[] Month = new string[60];
                Guid UID = new Guid();
                float AllowedPayment = 0.0f;
                int[] Year = new int[50];
                int OrderBy = 0;
                Guid WorkPackageUID = new Guid(WebConfigurationManager.AppSettings["CashFlowUID"]);

                //
                for (int i = 3; i < 4; i++)
                {
                    for (int j = 4; j < dtExcelRecords.Columns.Count; j++)
                    {
                        if (!string.IsNullOrEmpty(dtExcelRecords.Rows[i - 1][j].ToString()))
                        {
                            Month[j] = dtExcelRecords.Rows[i - 1][j].ToString().Replace("'", "-").Split('-')[0];
                            Year[j] = int.Parse(dtExcelRecords.Rows[i - 1][j].ToString().Replace("'", "-").Split('-')[1]);
                        }
                        //string str = dtExcelRecords.Rows[i - 1][j].ToString();
                    }

                }



                for (int i = 3; i < dtExcelRecords.Rows.Count; i++)
                {
                    OrderBy = 0;
                    if (!string.IsNullOrEmpty(dtExcelRecords.Rows[i][2].ToString()) && dtExcelRecords.Rows[i][2].ToString().Trim()== "CASH FLOW - PAYMENT")
                    {
                        Description = dtExcelRecords.Rows[i][2].ToString();


                        // This is Financial_MileStoneUID
                        UID = Guid.NewGuid();
                        DBGetData getdata = new DBGetData();
                        // for updating Financial Milestone.......
                        int result = getdata.InsertFinMilestoneExcel(UID, WorkPackageUID, Description);
                        // loop thru all columns for updating months data to financialMilestoneMonth table
                        for (int j = 4; j < dtExcelRecords.Columns.Count; j++)
                        {
                            OrderBy = OrderBy + 1;
                            if (!string.IsNullOrEmpty(dtExcelRecords.Rows[i][j].ToString()))
                            {
                                AllowedPayment = float.Parse(dtExcelRecords.Rows[i][j].ToString());

                                // 
                                //if (AllowedPayment > 0)
                                //{
                                     result = getdata.InsertFinMilestoneMonthExcel(Guid.NewGuid(), UID, AllowedPayment, Month[j], Year[j], WorkPackageUID, OrderBy);
                               // }
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Response.Write("Error " + ex.Message);
            }
        }

        protected void btnSubmit09_Click(object sender, EventArgs e) //Template for CP-09 reading
        {
            try
            {
                string connectionString = "";

                string fileName = Path.GetFileName(FileUpload1.PostedFile.FileName);
                string fileExtension = Path.GetExtension(FileUpload1.PostedFile.FileName);
                string fileLocation = Server.MapPath("~/RegExcel/" + fileName);
                FileUpload1.SaveAs(fileLocation);

                //Check whether file extension is xls or xslx

                if (fileExtension == ".xls")
                {
                    connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileLocation + @";Extended Properties=" + Convert.ToChar(34).ToString() + @"Excel 8.0;Imex=1;HDR=Yes;" + Convert.ToChar(34).ToString();
                }
                else if (fileExtension == ".xlsx")
                {
                    connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileLocation + @";Extended Properties=" + Convert.ToChar(34).ToString() + @"Excel 8.0;Imex=1;HDR=Yes;" + Convert.ToChar(34).ToString();
                }

                //Create OleDB Connection and OleDb Command

                OleDbConnection con = new OleDbConnection(connectionString);
                OleDbCommand cmd = new OleDbCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = con;
                OleDbDataAdapter dAdapter = new OleDbDataAdapter(cmd);
                DataTable dtExcelRecords = new DataTable();
                con.Open();
                DataTable dtExcelSheetName = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                string getExcelSheetName = dtExcelSheetName.Rows[0]["Table_Name"].ToString();
                cmd.CommandText = "SELECT * FROM [" + getExcelSheetName + "]";
                dAdapter.SelectCommand = cmd;
                dAdapter.Fill(dtExcelRecords);
                con.Close();
                grdExcelData.DataSource = dtExcelRecords;
                grdExcelData.DataBind();
                //

                string Description = string.Empty;
                string Quantity = string.Empty;
                string[] Month = new string[50];
                Guid UID = new Guid();
                float AllowedPayment = 0.0f;
                int[] Year = new int[50];
                int OrderBy = 0;
                Guid WorkPackageUID = new Guid(WebConfigurationManager.AppSettings["CashFlowUID"]);

                //
                for (int i = 7; i < 8; i++)
                {
                    for (int j = 2; j < dtExcelRecords.Columns.Count-5; j++)
                    {
                        Month[j] = dtExcelRecords.Rows[i - 1][j].ToString().Split('-')[0];
                        Year[j] = int.Parse(dtExcelRecords.Rows[i - 1][j].ToString().Split('-')[1]);

                    }

                }



                for (int i = 7; i < dtExcelRecords.Rows.Count; i++)
                {
                    OrderBy = 0;
                    if (!string.IsNullOrEmpty(dtExcelRecords.Rows[i][1].ToString()) && dtExcelRecords.Rows[i][1].ToString().Trim() == "Invoice - FTM")
                    {
                        if (!string.IsNullOrEmpty(dtExcelRecords.Rows[i][1].ToString()))
                        {
                            Description = dtExcelRecords.Rows[i][1].ToString();

                        }
                        // This is Financial_MileStoneUID
                        UID = Guid.NewGuid();
                        DBGetData getdata = new DBGetData();
                        // for updating Financial Milestone.......
                        int result = getdata.InsertFinMilestoneExcel(UID, WorkPackageUID, Description);
                        // loop thru all columns for updating months data to financialMilestoneMonth table
                        for (int j = 2; j < dtExcelRecords.Columns.Count; j++)
                        {
                            OrderBy = OrderBy + 1;
                            if (!string.IsNullOrEmpty(dtExcelRecords.Rows[i][j].ToString()))
                            {
                                AllowedPayment = float.Parse(dtExcelRecords.Rows[i][j].ToString()) * 0.01f;

                                // 
                                //  if (AllowedPayment > 0)
                                //  {
                                result = getdata.InsertFinMilestoneMonthExcel(Guid.NewGuid(), UID, AllowedPayment, Month[j], Year[j], WorkPackageUID, OrderBy);
                                // }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write("Error " + ex.Message);
            }
        }

        protected void btnSubmit02_Click(object sender, EventArgs e)
        {
            try
            {
                string connectionString = "";

                string fileName = Path.GetFileName(FileUpload1.PostedFile.FileName);
                string fileExtension = Path.GetExtension(FileUpload1.PostedFile.FileName);
                string fileLocation = Server.MapPath("~/RegExcel/" + fileName);
                FileUpload1.SaveAs(fileLocation);

                //Check whether file extension is xls or xslx

                if (fileExtension == ".xls")
                {
                    connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileLocation + @";Extended Properties=" + Convert.ToChar(34).ToString() + @"Excel 8.0;Imex=1;HDR=Yes;" + Convert.ToChar(34).ToString();
                }
                else if (fileExtension == ".xlsx")
                {
                    connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileLocation + @";Extended Properties=" + Convert.ToChar(34).ToString() + @"Excel 8.0;Imex=1;HDR=Yes;" + Convert.ToChar(34).ToString();
                }

                //Create OleDB Connection and OleDb Command

                OleDbConnection con = new OleDbConnection(connectionString);
                OleDbCommand cmd = new OleDbCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = con;
                OleDbDataAdapter dAdapter = new OleDbDataAdapter(cmd);
                DataTable dtExcelRecords = new DataTable();
                con.Open();
                DataTable dtExcelSheetName = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                string getExcelSheetName = "Statement - 2$"; dtExcelSheetName.Rows[0]["Table_Name"].ToString();
                cmd.CommandText = "SELECT * FROM [" + getExcelSheetName + "]";
                dAdapter.SelectCommand = cmd;
                dAdapter.Fill(dtExcelRecords);
                con.Close();
                grdExcelData.DataSource = dtExcelRecords;
                grdExcelData.DataBind();
                //

                string Description = string.Empty;
                string Quantity = string.Empty;
                string[] Month = new string[60];
                Guid UID = new Guid();
                float AllowedPayment = 0.0f;
                int[] Year = new int[50];
                int OrderBy = 0;
                Guid WorkPackageUID = new Guid(WebConfigurationManager.AppSettings["CashFlowUID"]);

                //
                for (int i = 3; i < 4; i++)
                {
                    for (int j = 4; j < dtExcelRecords.Columns.Count; j++)
                    {
                        if (!string.IsNullOrEmpty(dtExcelRecords.Rows[i - 1][j].ToString()))
                        {
                            Month[j] = dtExcelRecords.Rows[i - 1][j].ToString().Replace("'", "-").Split('-')[0];
                            Year[j] = int.Parse(dtExcelRecords.Rows[i - 1][j].ToString().Replace("'", "-").Split('-')[1]);
                        }
                        //string str = dtExcelRecords.Rows[i - 1][j].ToString();
                    }

                }



                for (int i = 3; i < dtExcelRecords.Rows.Count; i++)
                {
                    OrderBy = 0;
                    if (!string.IsNullOrEmpty(dtExcelRecords.Rows[i][2].ToString()) && dtExcelRecords.Rows[i][2].ToString().Trim() == "CASH FLOW - PAYMENT")
                    {
                        Description = dtExcelRecords.Rows[i][2].ToString();


                        // This is Financial_MileStoneUID
                        UID = Guid.NewGuid();
                        DBGetData getdata = new DBGetData();
                        // for updating Financial Milestone.......
                        int result = getdata.InsertFinMilestoneExcel(UID, WorkPackageUID, Description);
                        // loop thru all columns for updating months data to financialMilestoneMonth table
                        for (int j = 4; j < dtExcelRecords.Columns.Count; j++)
                        {
                            OrderBy = OrderBy + 1;
                            if (!string.IsNullOrEmpty(dtExcelRecords.Rows[i][j].ToString()))
                            {
                                AllowedPayment = float.Parse(dtExcelRecords.Rows[i][j].ToString());

                                // 
                                //if (AllowedPayment > 0)
                                //{
                                result = getdata.InsertFinMilestoneMonthExcel(Guid.NewGuid(), UID, AllowedPayment, Month[j], Year[j], WorkPackageUID, OrderBy);
                                // }
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Response.Write("Error " + ex.Message);
            }
        }
    }
}