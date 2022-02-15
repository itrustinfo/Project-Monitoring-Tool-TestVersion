using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using System.Net.Mail;
using System.IO;
using System.Security.Cryptography;
using System.Web.Configuration;
using System.Globalization;

namespace ProjectManager.DAL
{
    public class GeneralDocuments
    {
        DBUtility db = new DBUtility();

        public int GeneralDocumentStructure_InsertorUpdate(Guid StructureUID, string Structure_Name,Guid ParentStructureUID, Guid CreatedBy)
        {
            int cnt = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(db.GetConnectionString()))
                {

                    using (SqlCommand cmd = new SqlCommand("usp_InsertorUpdate_GeneralDocumentStructure"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("@StructureUID", StructureUID);
                        cmd.Parameters.AddWithValue("@Structure_Name", Structure_Name);
                        cmd.Parameters.AddWithValue("@ParentStructureUID", ParentStructureUID);
                        cmd.Parameters.AddWithValue("@CreatedBy", CreatedBy);
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

        public int GeneralDocumentStructure_Delete(Guid StructureUID, Guid UserUID)
        {
            int sresult = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(db.GetConnectionString()))
                {

                    using (SqlCommand cmd = new SqlCommand("usp_GeneralDocumentStructure_Delete"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("@StructureUID", StructureUID);
                        cmd.Parameters.AddWithValue("@UserUID", UserUID);
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

        public DataSet GetGeneralDocumentStructure_TopLevel()
        {
            DataSet ds = new DataSet();
            try
            {
                SqlConnection con = new SqlConnection(db.GetConnectionString());
                SqlDataAdapter cmd = new SqlDataAdapter("usp_GetGeneralDocumentStructure_TopLevel", con);
                cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                cmd.Fill(ds);
            }
            catch (Exception ex)
            {
                ds = null;
            }
            return ds;
        }

        public DataSet GetGeneralDocumentStructure_By_ParentStructureUID(Guid ParentStructureUID)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlConnection con = new SqlConnection(db.GetConnectionString());
                SqlDataAdapter cmd = new SqlDataAdapter("usp_GetGeneralDocumentStructure_by_ParentStructureUID", con);
                cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                cmd.SelectCommand.Parameters.AddWithValue("@ParentStructureUID", ParentStructureUID);
                cmd.Fill(ds);
            }
            catch (Exception ex)
            {
                ds = null;
            }
            return ds;
        }

        public DataSet GetGeneralDocumentStructure_By_StructureUID(Guid StructureUID)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlConnection con = new SqlConnection(db.GetConnectionString());
                SqlDataAdapter cmd = new SqlDataAdapter("usp_GetGeneralDocumentStructure_by_StructureUID", con);
                cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                cmd.SelectCommand.Parameters.AddWithValue("@StructureUID", StructureUID);
                cmd.Fill(ds);
            }
            catch (Exception ex)
            {
                ds = null;
            }
            return ds;
        }

        // added on 25/02/2021
        public int InsertIntoGeneralDocuments(Guid GeneralDocumentUID, Guid StructureUID, string Ref_Number,
        DateTime IncomingRec_Date, string GeneralDocument_Name, string Description, double ActualDocument_Version, string ActualDocument_Type,
         string Media_HC, string Media_SC, string Media_SCEF, string Media_HCR, string Media_SCR, string Media_NA, string ActualDocument_Path, string Remarks,
         string FileRef_Number, string ActualDocument_CurrentStatus, DateTime Document_Date, string ActualDocument_RelativePath, string ActualDocument_DirectoryName, string UploadFilePhysicalpath,Guid Created_By)
        {
            int sresult = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(db.GetConnectionString()))
                {

                    using (SqlCommand cmd = new SqlCommand("usp_InsertIntoGeneralDocuments"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("@GeneralDocumentUID", GeneralDocumentUID);
                        cmd.Parameters.AddWithValue("@StructureUID", StructureUID);
                        cmd.Parameters.AddWithValue("@Ref_Number", Ref_Number);
                        cmd.Parameters.AddWithValue("@IncomingRec_Date", IncomingRec_Date);
                        cmd.Parameters.AddWithValue("@GeneralDocument_Name", GeneralDocument_Name);
                        cmd.Parameters.AddWithValue("@GeneralDocument_Description", Description);
                        cmd.Parameters.AddWithValue("@GeneralDocument_Version", ActualDocument_Version);
                        cmd.Parameters.AddWithValue("@GeneralDocument_Type", ActualDocument_Type);
                        cmd.Parameters.AddWithValue("@Media_HC", Media_HC);
                        cmd.Parameters.AddWithValue("@Media_SC", Media_SC);
                        cmd.Parameters.AddWithValue("@Media_SCEF", Media_SCEF);
                        cmd.Parameters.AddWithValue("@Media_HCR", Media_HCR);
                        cmd.Parameters.AddWithValue("@Media_SCR", Media_SCR);
                        cmd.Parameters.AddWithValue("@Media_NA", Media_NA);
                        cmd.Parameters.AddWithValue("@GeneralDocument_Path", ActualDocument_Path);
                        cmd.Parameters.AddWithValue("@Remarks", Remarks);
                        cmd.Parameters.AddWithValue("@FileRef_Number", FileRef_Number);
                        cmd.Parameters.AddWithValue("@GeneralDocument_CurrentStatus", ActualDocument_CurrentStatus);
                        cmd.Parameters.AddWithValue("@Document_Date", Document_Date);
                        cmd.Parameters.AddWithValue("@GeneralDocument_RelativePath", ActualDocument_RelativePath);
                        cmd.Parameters.AddWithValue("@GeneralDocument_DirectoryName", ActualDocument_DirectoryName);
                        cmd.Parameters.AddWithValue("@UploadFilePhysicalpath", UploadFilePhysicalpath);
                        cmd.Parameters.AddWithValue("@Created_By", Created_By);
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


        public int UpdateIntoGeneralDocuments(Guid GeneralDocumentUID, Guid StructureUID, string Ref_Number,
        DateTime IncomingRec_Date, string GeneralDocument_Name, string Description,string Media_HC, string Media_SC, string Media_SCEF, string Media_HCR, string Media_SCR, string Media_NA, string Remarks,
         string FileRef_Number, DateTime Document_Date)
        {
            int sresult = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(db.GetConnectionString()))
                {

                    using (SqlCommand cmd = new SqlCommand("usp_GeneralDocuments_Update"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("@GeneralDocumentUID", GeneralDocumentUID);
                        cmd.Parameters.AddWithValue("@StructureUID", StructureUID);
                        cmd.Parameters.AddWithValue("@Ref_Number", Ref_Number);
                        cmd.Parameters.AddWithValue("@IncomingRec_Date", IncomingRec_Date);
                        cmd.Parameters.AddWithValue("@GeneralDocument_Name", GeneralDocument_Name);
                        cmd.Parameters.AddWithValue("@GeneralDocument_Description", Description);
                        cmd.Parameters.AddWithValue("@Media_HC", Media_HC);
                        cmd.Parameters.AddWithValue("@Media_SC", Media_SC);
                        cmd.Parameters.AddWithValue("@Media_SCEF", Media_SCEF);
                        cmd.Parameters.AddWithValue("@Media_HCR", Media_HCR);
                        cmd.Parameters.AddWithValue("@Media_SCR", Media_SCR);
                        cmd.Parameters.AddWithValue("@Media_NA", Media_NA);
                        cmd.Parameters.AddWithValue("@Remarks", Remarks);
                        cmd.Parameters.AddWithValue("@FileRef_Number", FileRef_Number);
                        cmd.Parameters.AddWithValue("@Document_Date", Document_Date);
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

        public string GetFolderNameByUID(Guid StructureUID)
        {
            //DataSet ds = new DataSet();

            string data = string.Empty;
            try
            {
                SqlConnection con = new SqlConnection(db.GetConnectionString());
                if (con.State == ConnectionState.Closed) con.Open();
                SqlCommand cmd = new SqlCommand("usp_GetFolderNameByUID", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StructureUID", StructureUID);
                data = (string)cmd.ExecuteScalar();
                con.Close();
            }
            catch (Exception ex)
            {
                //ds = null;
            }
            return data;
        }

        public string CheckGeneralDocumentStructure_NameExist_For_StructureUID(Guid StructureUID,string Structure_Name)
        {

            string data = string.Empty;
            try
            {
                SqlConnection con = new SqlConnection(db.GetConnectionString());
                if (con.State == ConnectionState.Closed) con.Open();
                SqlCommand cmd = new SqlCommand("usp_GetGeneralDocumentStructure_Name_For_StructureUID", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StructureUID", StructureUID);
                cmd.Parameters.AddWithValue("@Structure_Name", Structure_Name);
                data = (string)cmd.ExecuteScalar();
                con.Close();
            }
            catch (Exception ex)
            {
                data = string.Empty;
            }
            return data;
        }

        public DataSet GeneralDocuments_SelectBy_StructureUID(Guid StructureUID)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlConnection con = new SqlConnection(db.GetConnectionString());
                SqlDataAdapter cmd = new SqlDataAdapter("GeneralDocuments_SelectBy_StructureUID", con);
                cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                cmd.SelectCommand.Parameters.AddWithValue("@StructureUID ", StructureUID);
                cmd.Fill(ds);
            }
            catch (Exception ex)
            {
                ds = null;
            }
            return ds;
        }

        public DataSet GeneralDocuments_SelectBy_GeneralDocumentUID(Guid GeneralDocumentUID)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlConnection con = new SqlConnection(db.GetConnectionString());
                SqlDataAdapter cmd = new SqlDataAdapter("usp_GetGeneralDocuments_by_GeneralDocuments", con);
                cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                cmd.SelectCommand.Parameters.AddWithValue("@GeneralDocumentUID ", GeneralDocumentUID);
                cmd.Fill(ds);
            }
            catch (Exception ex)
            {
                ds = null;
            }
            return ds;
        }

        public string GetGeneralDocumentNameByUID(Guid GeneralDocumentUID)
        {
            //DataSet ds = new DataSet();

            string data = string.Empty;
            try
            {
                SqlConnection con = new SqlConnection(db.GetConnectionString());
                if (con.State == ConnectionState.Closed) con.Open();
                SqlCommand cmd = new SqlCommand("usp_GetGeneralDocumentName_By_GeneralDocumentUID", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@GeneralDocumentUID", GeneralDocumentUID);
                data = (string)cmd.ExecuteScalar();
                con.Close();
            }
            catch (Exception ex)
            {
                //ds = null;
            }
            return data;
        }


        public int GeneralDocument_Delete(Guid GeneralDocumentUID, Guid UserUID)
        {
            int sresult = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(db.GetConnectionString()))
                {

                    using (SqlCommand cmd = new SqlCommand("usp_GeneralDocuments_Delete"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("@GeneralDocumentUID", GeneralDocumentUID);
                        cmd.Parameters.AddWithValue("@UserUID", UserUID);
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

        // added on 26/02/2021
        public DataSet GetDoctypeForSearchGD()
        {
            DataSet ds = new DataSet();
            try
            {
                SqlConnection con = new SqlConnection(db.GetConnectionString());
                SqlDataAdapter cmd = new SqlDataAdapter("usp_GetDoctypeForSearchGD", con);
                cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                cmd.Fill(ds);
            }
            catch (Exception ex)
            {
                ds = null;
            }
            return ds;
        }

        // added on 26/01/2021
        public DataSet GeneralDocuments_Search(string DocumentName, string Doctype, DateTime DocDate, DateTime DocumentDate, DateTime DocToDate, DateTime DocumentToDate, int Type)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlConnection con = new SqlConnection(db.GetConnectionString());
                SqlDataAdapter cmd = new SqlDataAdapter("GeneralDocuments_Search", con);
                cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                cmd.SelectCommand.Parameters.AddWithValue("@DocumentName", DocumentName);
                cmd.SelectCommand.Parameters.AddWithValue("@Doctype", Doctype);
                cmd.SelectCommand.Parameters.AddWithValue("@Type", Type);
                cmd.SelectCommand.Parameters.AddWithValue("@DocDate", DocDate);// this is incomiung recv date
                cmd.SelectCommand.Parameters.AddWithValue("@DocumentDate", DocumentDate);
                cmd.SelectCommand.Parameters.AddWithValue("@DocDateTo", DocToDate);// this is incomiung recv date
                cmd.SelectCommand.Parameters.AddWithValue("@DocumentDateTo", DocumentToDate);
                cmd.Fill(ds);
            }
            catch (Exception ex)
            {
                ds = null;
            }
            return ds;
        }

        public string CheckGeneralDocumentRelativePathExists(Guid StructureUID, string GeneralDocument_RelativePath)
        {

            string data = string.Empty;
            try
            {
                SqlConnection con = new SqlConnection(db.GetConnectionString());
                if (con.State == ConnectionState.Closed) con.Open();
                SqlCommand cmd = new SqlCommand("usp_CheckGeneralDocumentRelativePathExists", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StructureUID", StructureUID);
                cmd.Parameters.AddWithValue("@GeneralDocument_RelativePath", GeneralDocument_RelativePath);
                data = (string)cmd.ExecuteScalar();
                con.Close();
            }
            catch (Exception ex)
            {
                data = string.Empty;
            }
            return data;
        }


        //added on 03 Mar 2021 Arun

        public int Location_InsertorUpdate(Guid LocationMaster_UID, string Location_Name)
        {
            int cnt = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(db.GetConnectionString()))
                {

                    using (SqlCommand cmd = new SqlCommand("usp_InsertorUpdate_LocationMaster"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("@LocationMaster_UID", LocationMaster_UID);
                        cmd.Parameters.AddWithValue("@Location_Name", Location_Name);
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

        public DataSet Location_Select()
        {
            DataSet ds = new DataSet();
            try
            {
                SqlConnection con = new SqlConnection(db.GetConnectionString());
                SqlDataAdapter cmd = new SqlDataAdapter("usp_Location_Select", con);
                cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                cmd.Fill(ds);
            }
            catch (Exception ex)
            {
                ds = null;
            }
            return ds;
        }

        public DataSet Location_SelectBy_LocationMaster_UID(Guid LocationMaster_UID)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlConnection con = new SqlConnection(db.GetConnectionString());
                SqlDataAdapter cmd = new SqlDataAdapter("usp_Location_SelectBy_LocationMaster_UID", con);
                cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                cmd.SelectCommand.Parameters.AddWithValue("@LocationMaster_UID ", LocationMaster_UID);
                cmd.Fill(ds);
            }
            catch (Exception ex)
            {
                ds = null;
            }
            return ds;
        }

        public int Location_Delete(Guid LocationMaster_UID, Guid UserUID)
        {
            int sresult = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(db.GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("usp_Location_Delete"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("@LocationMaster_UID", LocationMaster_UID);
                        cmd.Parameters.AddWithValue("@UserUID", UserUID);
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

        public string LocationName_by_LocationMaster_UID(Guid LocationMaster_UID)
        {
            //DataSet ds = new DataSet();

            string data = string.Empty;
            try
            {
                SqlConnection con = new SqlConnection(db.GetConnectionString());
                if (con.State == ConnectionState.Closed) con.Open();
                SqlCommand cmd = new SqlCommand("usp_LocationName_by_LocationMaster_UID", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@LocationMaster_UID", LocationMaster_UID);
                data = (string)cmd.ExecuteScalar();
                con.Close();
            }
            catch (Exception ex)
            {
                //ds = null;
            }
            return data;
        }

        public int UserLocation_InsertorUpdate(Guid UserLocation_UID, Guid UserUID,Guid LocationMaster_UID,string Action)
        {
            int cnt = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(db.GetConnectionString()))
                {

                    using (SqlCommand cmd = new SqlCommand("usp_InsertorUpdate_UserLocation"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("@UserLocation_UID", UserLocation_UID);
                        cmd.Parameters.AddWithValue("@UserUID", UserUID);
                        cmd.Parameters.AddWithValue("@LocationMaster_UID", LocationMaster_UID);
                        cmd.Parameters.AddWithValue("@Action", Action);
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

        public DataSet UserLocation_Select()
        {
            DataSet ds = new DataSet();
            try
            {
                SqlConnection con = new SqlConnection(db.GetConnectionString());
                SqlDataAdapter cmd = new SqlDataAdapter("usp_UserLocation_Select", con);
                cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                cmd.Fill(ds);
            }
            catch (Exception ex)
            {
                ds = null;
            }
            return ds;
        }

        public DataSet UserLocation_SelectBy_UserUID(Guid UserUID)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlConnection con = new SqlConnection(db.GetConnectionString());
                SqlDataAdapter cmd = new SqlDataAdapter("usp_UserLocation_Select_by_UserUID", con);
                cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                cmd.SelectCommand.Parameters.AddWithValue("@UserUID ", UserUID);
                cmd.Fill(ds);
            }
            catch (Exception ex)
            {
                ds = null;
            }
            return ds;
        }

        public DataSet UserLocation_SelectBy_UserLocation_UID(Guid UserLocation_UID)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlConnection con = new SqlConnection(db.GetConnectionString());
                SqlDataAdapter cmd = new SqlDataAdapter("usp_UserLocation_Select_by_UserLocation_UID", con);
                cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                cmd.SelectCommand.Parameters.AddWithValue("@UserLocation_UID ", UserLocation_UID);
                cmd.Fill(ds);
            }
            catch (Exception ex)
            {
                ds = null;
            }
            return ds;
        }

        public int UserLocation_Delete(Guid DeletingUser, Guid DeletedBy)
        {
            int sresult = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(db.GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("usp_UserLocation_Delete"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("@DeletingUser", DeletingUser);
                        cmd.Parameters.AddWithValue("@DeletedBy", DeletedBy);
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


        public int UserLocation_Delete_by_UserUID(Guid UserUID)
        {
            int sresult = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(db.GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("usp_UserLocation_Delete_by_UserUID"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("@UserUID", UserUID);
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

        public DataSet Distinct_Users_SelectFromUserLocation()
        {
            DataSet ds = new DataSet();
            try
            {
                SqlConnection con = new SqlConnection(db.GetConnectionString());
                SqlDataAdapter cmd = new SqlDataAdapter("usp_UserLocation_UserUID_Select", con);
                cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                cmd.Fill(ds);
            }
            catch (Exception ex)
            {
                ds = null;
            }
            return ds;
        }
    }
}