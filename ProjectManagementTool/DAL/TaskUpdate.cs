using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ProjectManager.DAL
{
    public class TaskUpdate
    {
        DBUtility db = new DBUtility();

        internal System.Data.DataTable GetSSSTask(string TaskID)
        {
            DataTable ds = new DataTable();
            try
            {
                SqlConnection con = new SqlConnection(db.GetConnectionString());
                SqlDataAdapter cmd = new SqlDataAdapter("usp_GetSTask", con);
                cmd.SelectCommand.Parameters.AddWithValue("@TaskID", TaskID);
                cmd.SelectCommand.Parameters.AddWithValue("@LevelID", 4);
                cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                cmd.Fill(ds);
            }
            catch (Exception ex)
            {
                ds = null;
            }
            return ds;
        }

        internal System.Data.DataTable GetSSTask(string TaskID)
        {
            DataTable ds = new DataTable();
            try
            {
                SqlConnection con = new SqlConnection(db.GetConnectionString());
                SqlDataAdapter cmd = new SqlDataAdapter("usp_GetSTask", con);
                cmd.SelectCommand.Parameters.AddWithValue("@TaskID", TaskID);
                cmd.SelectCommand.Parameters.AddWithValue("@LevelID", 3);
                cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                cmd.Fill(ds);
            }
            catch (Exception ex)
            {
                ds = null;
            }
            return ds;
        }

        internal System.Data.DataTable GetSTask(string TaskID)
        {
            DataTable ds = new DataTable();
            try
            {
                SqlConnection con = new SqlConnection(db.GetConnectionString());
                SqlDataAdapter cmd = new SqlDataAdapter("usp_GetSTask", con);
                cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                cmd.SelectCommand.Parameters.AddWithValue("@TaskID", TaskID);
                cmd.SelectCommand.Parameters.AddWithValue("@LevelID", 2);
                cmd.Fill(ds);
            }
            catch (Exception ex)
            {
                ds = null;
            }
            return ds;
        }

        internal System.Data.DataTable GetWorkPackage(string ProjectID)
        {
            DataTable ds = new DataTable();
            try
            {
                SqlConnection con = new SqlConnection(db.GetConnectionString());
                SqlDataAdapter cmd = new SqlDataAdapter("usp_GetWPackage", con);
                cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                cmd.SelectCommand.Parameters.AddWithValue("@ProjectID", ProjectID);
                cmd.Fill(ds);
            }
            catch (Exception ex)
            {
                ds = null;
            }
            return ds;
        }

        internal System.Data.DataTable GetTasks(string WorkPackID)
        {
            DataTable ds = new DataTable();
            try
            {
                SqlConnection con = new SqlConnection(db.GetConnectionString());
                SqlDataAdapter cmd = new SqlDataAdapter("usp_GetTasks", con);
                cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                cmd.SelectCommand.Parameters.AddWithValue("@WorkPackID", WorkPackID);
                cmd.Fill(ds);
            }
            catch (Exception ex)
            {
                ds = null;
            }
            return ds;
        }

        internal System.Data.DataSet GetTasks_by_WorkPackage(string WorkPackID)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlConnection con = new SqlConnection(db.GetConnectionString());
                SqlDataAdapter cmd = new SqlDataAdapter("usp_GetTasks", con);
                cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                cmd.SelectCommand.Parameters.AddWithValue("@WorkPackID", WorkPackID);
                cmd.Fill(ds);
            }
            catch (Exception ex)
            {
                ds = null;
            }
            return ds;
        }


        internal System.Data.DataSet GetTasks_by_Workpackage_Option(Guid Workpackage_Option,Guid WorkPackageUID)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlConnection con = new SqlConnection(db.GetConnectionString());
                SqlDataAdapter cmd = new SqlDataAdapter("usp_Tasks_SelectBy_OptionUID", con);
                cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                cmd.SelectCommand.Parameters.AddWithValue("@Workpackage_Option", Workpackage_Option);
                cmd.SelectCommand.Parameters.AddWithValue("@WorkPackageUID", WorkPackageUID);
                cmd.Fill(ds);
            }
            catch (Exception ex)
            {
                ds = null;
            }
            return ds;
        }

        internal System.Data.DataSet GetUserFirstLevelTasks_by_WorkPackageID(Guid WorkPackageUID,Guid UserUID)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlConnection con = new SqlConnection(db.GetConnectionString());
                SqlDataAdapter cmd = new SqlDataAdapter("GetUserFirstLevelTasks_by_WorkPackageID", con);
                cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                cmd.SelectCommand.Parameters.AddWithValue("@WorkPackageUID", WorkPackageUID);
                cmd.SelectCommand.Parameters.AddWithValue("@UserUID", UserUID);
                cmd.Fill(ds);
            }
            catch (Exception ex)
            {
                ds = null;
            }
            return ds;
        }

        internal System.Data.DataTable GetProjects()
        {
            DataTable ds = new DataTable();
            try
            {
                SqlConnection con = new SqlConnection(db.GetConnectionString());
                SqlDataAdapter cmd = new SqlDataAdapter("usp_GetProjects", con);
                cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                cmd.Fill(ds);
            }
            catch (Exception ex)
            {
                ds = null;
            }
            return ds;
        }

        internal System.Data.DataSet GetAllProjects()
        {
            DataSet ds = new DataSet();
            try
            {
                SqlConnection con = new SqlConnection(db.GetConnectionString());
                SqlDataAdapter cmd = new SqlDataAdapter("usp_GetProjects", con);
                cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                cmd.Fill(ds);
            }
            catch (Exception ex)
            {
                ds = null;
            }
            return ds;
        }

        internal System.Data.DataSet GetAllProjects_Expect_Selected(Guid ProjectUID)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlConnection con = new SqlConnection(db.GetConnectionString());
                SqlDataAdapter cmd = new SqlDataAdapter("usp_GetAllProjects_Expect_Selected", con);
                cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                cmd.SelectCommand.Parameters.AddWithValue("@ProjectUID", ProjectUID);
                cmd.Fill(ds);
            }
            catch (Exception ex)
            {
                ds = null;
            }
            return ds;
        }

        internal System.Data.DataTable GetProjects_by_UserUID(Guid UserUID)
        {
            DataTable ds = new DataTable();
            try
            {
                SqlConnection con = new SqlConnection(db.GetConnectionString());
                SqlDataAdapter cmd = new SqlDataAdapter("usp_getProjects_by_UserUID", con);
                cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                cmd.SelectCommand.Parameters.AddWithValue("@UserUID", UserUID);
                cmd.Fill(ds);
            }
            catch (Exception ex)
            {
                ds = null;
            }
            return ds;
        }

        internal System.Data.DataSet GetUserProjects_by_ClassUID(Guid ProjectClass_UID,Guid UserUID)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlConnection con = new SqlConnection(db.GetConnectionString());
                SqlDataAdapter cmd = new SqlDataAdapter("usp_GetProject_by_ClassUID", con);
                cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                cmd.SelectCommand.Parameters.AddWithValue("@ProjectClass_UID", ProjectClass_UID);
                cmd.SelectCommand.Parameters.AddWithValue("@UserUID", UserUID);
                cmd.Fill(ds);
            }
            catch (Exception ex)
            {
                ds = null;
            }
            return ds;
        }

        internal System.Data.DataSet GetProjects_by_ClassUID(Guid ProjectClass_UID)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlConnection con = new SqlConnection(db.GetConnectionString());
                SqlDataAdapter cmd = new SqlDataAdapter("usp_GetProjects_by_ClassUID", con);
                cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                cmd.SelectCommand.Parameters.AddWithValue("@ProjectClass_UID", ProjectClass_UID);
                cmd.Fill(ds);
            }
            catch (Exception ex)
            {
                ds = null;
            }
            return ds;
        }

        public DataTable GetAssignedProjects_by_UserUID(Guid UserUID)
        {
            DataTable ds = new DataTable();
            try
            {
                SqlConnection con = new SqlConnection(db.GetConnectionString());
                SqlDataAdapter cmd = new SqlDataAdapter("GetAssignedProject_by_UserUID", con);
                cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                cmd.SelectCommand.Parameters.AddWithValue("@UserUID", UserUID);
                cmd.Fill(ds);
            }
            catch (Exception ex)
            {
                ds = null;
            }
            return ds;
        }

        public DataSet GetAssignedProject_by_UserUID_Except_Selected(Guid UserUID,Guid ProjectUID)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlConnection con = new SqlConnection(db.GetConnectionString());
                SqlDataAdapter cmd = new SqlDataAdapter("GetAssignedProject_by_UserUID_Except_Selected", con);
                cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                cmd.SelectCommand.Parameters.AddWithValue("@UserUID", UserUID);
                cmd.SelectCommand.Parameters.AddWithValue("@ProjectUID", ProjectUID);
                cmd.Fill(ds);
            }
            catch (Exception ex)
            {
                ds = null;
            }
            return ds;
        }

        public DataTable GetProjectsData_By_UserUID(Guid UserUID)
        {
            DataTable ds = new DataTable();
            try
            {
                SqlConnection con = new SqlConnection(db.GetConnectionString());
                SqlDataAdapter cmd = new SqlDataAdapter("GetProjectsData_By_UserUID", con);
                cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                cmd.SelectCommand.Parameters.AddWithValue("@UserUID", UserUID);
                cmd.Fill(ds);
            }
            catch (Exception ex)
            {
                ds = null;
            }
            return ds;
        }

        internal System.Data.DataTable GetStatus()
        {
            DataTable ds = new DataTable();
            try
            {
                SqlConnection con = new SqlConnection(db.GetConnectionString());
                SqlDataAdapter cmd = new SqlDataAdapter("usp_Getstatus", con);
                cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                cmd.Fill(ds);
            }
            catch (Exception ex)
            {
                ds = null;
            }
            return ds;
        }

        internal int UpdateTaskStatus(string WorkPackageUID, string TaskUID, string Status, DateTime CDate1, double StatusPer,string Comment)
        {
            int p = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(db.GetConnectionString()))
                {

                    using (SqlCommand cmd = new SqlCommand("usp_UpdateTaskStatus"))
                    {
                        cmd.Connection = con;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@TaskUID", TaskUID);
                        cmd.Parameters.AddWithValue("@WorkPackageUID", WorkPackageUID);
                        cmd.Parameters.AddWithValue("@Status", Status);
                        cmd.Parameters.AddWithValue("@CDate1", CDate1);
                        cmd.Parameters.AddWithValue("@StatusPer", StatusPer);
                        cmd.Parameters.AddWithValue("@Comments", Comment);
                        con.Open();
                        p = cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return p;
        }

        internal System.Data.DataTable GetAssignedProject_by_UserUID(Guid UserUID)
        {
            DataTable ds = new DataTable();
            try
            {
                SqlConnection con = new SqlConnection(db.GetConnectionString());
                SqlDataAdapter cmd = new SqlDataAdapter("GetAssignedProject_by_UserUID", con);
                cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                cmd.SelectCommand.Parameters.AddWithValue("@UserUID", UserUID);
                cmd.Fill(ds);
            }
            catch (Exception ex)
            {
                ds = null;
            }
            return ds;
        }

        //Venkat Jan 12 2021

        internal DataTable GetcontractPackage_Details()
        {
            DataTable ds = new DataTable();
            try
            {
                SqlConnection con = new SqlConnection(db.GetConnectionString());
                SqlDataAdapter cmd = new SqlDataAdapter("usp_GetContractPackage_Details", con);
                cmd.SelectCommand.CommandType = CommandType.StoredProcedure;

                cmd.Fill(ds);
            }
            catch (Exception ex)
            {
                ds = null;
            }
            return ds;
        }

        internal int Insert_CPDetails(string projectUID, decimal amount, string description, string remarks, DateTime dtPaymentDate, DateTime CAADate,string status)
        {
            int cnt = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(db.GetConnectionString()))
                {

                    using (SqlCommand cmd = new SqlCommand("usp_Insert_CPDetails"))
                    {
                        cmd.Connection = con;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@projectUID", new Guid(projectUID));
                        cmd.Parameters.AddWithValue("@amount", amount);
                        cmd.Parameters.AddWithValue("@description", description);
                        cmd.Parameters.AddWithValue("@dtPaymentDate", dtPaymentDate);
                        cmd.Parameters.AddWithValue("@remarks", remarks);
                        cmd.Parameters.AddWithValue("@CAADate", CAADate);
                        cmd.Parameters.AddWithValue("@status", status);
                        con.Open();
                        cnt = cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                string exnew = ex.Message;
            }
            return cnt;
        }

        internal DataTable GetProjectCPReports()
        {
            DataTable ds = new DataTable();
            try
            {
                SqlConnection con = new SqlConnection(db.GetConnectionString());
                SqlDataAdapter cmd = new SqlDataAdapter("GetProjectCPReports", con);
                cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                cmd.Fill(ds);
            }
            catch (Exception ex)
            {
                ds = null;
            }
            return ds;
        }

        internal DataTable GetProjectCPReports_UnderProcess()
        {
            DataTable ds = new DataTable();
            try
            {
                SqlConnection con = new SqlConnection(db.GetConnectionString());
                SqlDataAdapter cmd = new SqlDataAdapter("GetProjectCPReports_UnderProcessed", con);
                cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                cmd.Fill(ds);
            }
            catch (Exception ex)
            {
                ds = null;
            }
            return ds;
        }



        internal DataTable GetConsMonthActivity()
        {
            DataTable ds = new DataTable();
            try
            {
                SqlConnection con = new SqlConnection(db.GetConnectionString());
                SqlDataAdapter cmd = new SqlDataAdapter("usp_GetConsMonthActivites", con);

                cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                cmd.Fill(ds);
            }
            catch (Exception ex)
            {
                ds = null;
            }
            return ds;
        }

        internal DataTable GetConsActivity()
        {
            DataTable ds = new DataTable();
            try
            {
                SqlConnection con = new SqlConnection(db.GetConnectionString());
                SqlDataAdapter cmd = new SqlDataAdapter("usp_GetConsActivites", con);

                cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                cmd.Fill(ds);
            }
            catch (Exception ex)
            {
                ds = null;
            }
            return ds;
        }

        //internal int UpdateTaskDates(string WID, string TaskUID, DateTime CDate1, DateTime CDate2)
        //{
        //    int p = 0;
        //    try
        //    {
        //        using (SqlConnection con = new SqlConnection(db.GetConnectionString()))
        //        {

        //            using (SqlCommand cmd = new SqlCommand("usp_UpdateTaskStatus"))
        //            {
        //                cmd.Connection = con;
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                cmd.Parameters.AddWithValue("@TaskUID", TaskUID);
        //                cmd.Parameters.AddWithValue("@WorkPackageUID", WorkPackageUID);
        //                cmd.Parameters.AddWithValue("@Status", Status);
        //                cmd.Parameters.AddWithValue("@CDate1", CDate1);
        //                con.Open();
        //                p = cmd.ExecuteNonQuery();
        //                con.Close();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    return p;
        //}
    }
}