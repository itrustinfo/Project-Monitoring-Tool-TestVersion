using ProjectManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ProjectManagementTool.DAL
{
    public class UserFunctionalityMapping
    {
        DBUtility db = new DBUtility();

        public int InsertorUpdateMasterPageName(Guid MasterPageUID,string MasterPageName,string MasterPageURL)
        {
            int cnt = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(db.GetConnectionString()))
                {

                    using (SqlCommand cmd = new SqlCommand("usp_InsertorUpdateMasterPage"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("@MasterPageUID", MasterPageUID);
                        cmd.Parameters.AddWithValue("@MasterPageName", MasterPageName);
                        cmd.Parameters.AddWithValue("@MasterPageURL", MasterPageURL);
                        con.Open();
                        cnt = cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
                return cnt;
            }
            catch (Exception ex)
            {
                return cnt;
            }
        }

        public DataSet GetMasterPages()
        {
            DataSet ds = new DataSet();
            try
            {
                SqlConnection con = new SqlConnection(db.GetConnectionString());
                SqlDataAdapter cmd = new SqlDataAdapter("usp_MasterPages_Select", con);
                cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                cmd.Fill(ds);
            }
            catch (Exception ex)
            {
                ds = null;
            }
            return ds;
        }

        public DataSet GetMasterPage_by_MasterPageUID(Guid MasterPageUID)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlConnection con = new SqlConnection(db.GetConnectionString());
                SqlDataAdapter cmd = new SqlDataAdapter("usp_MasterPage_Select_by_MasterPageUID", con);
                cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                cmd.SelectCommand.Parameters.AddWithValue("@MasterPageUID", MasterPageUID);
                cmd.Fill(ds);
            }
            catch (Exception ex)
            {
                ds = null;
            }
            return ds;
        }

        public int MasterPageName_Delete(Guid MasterPageUID)
        {
            int sresult = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(db.GetConnectionString()))
                {

                    using (SqlCommand cmd = new SqlCommand("usp_MasterPage_Delete"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("@MasterPageUID", MasterPageUID);
                        con.Open();
                        sresult = (int)cmd.ExecuteNonQuery();
                        con.Close();

                    }
                }
                return sresult;
            }
            catch (Exception ex)
            {
                return sresult = 0;
            }
        }
    }
}