using ProjectManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Text;

namespace ProjectManagementTool.DAL
{
    public class Invoice
    {
        DBUtility db = new DBUtility();

        public int InvoiceMaster_InsertorUpdate(Guid InvoiceMaster_UID, Guid ProjectUID, Guid WorkpackageUID, string Invoice_Number,string Invoice_Desc,DateTime Invoice_Date,string Currency,string Currency_CultureInfo)
        {
            int cnt = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(db.GetConnectionString()))
                {

                    using (SqlCommand cmd = new SqlCommand("usp_InsertorUpdateInvoiceMaster"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("@InvoiceMaster_UID", InvoiceMaster_UID);
                        cmd.Parameters.AddWithValue("@ProjectUID", ProjectUID);
                        cmd.Parameters.AddWithValue("@WorkpackageUID", WorkpackageUID);
                        cmd.Parameters.AddWithValue("@Invoice_Number", Invoice_Number);
                        cmd.Parameters.AddWithValue("@Invoice_Desc", Invoice_Desc);
                        cmd.Parameters.AddWithValue("@Invoice_Date", Invoice_Date);
                        cmd.Parameters.AddWithValue("@Currency", Currency);
                        cmd.Parameters.AddWithValue("@Currency_CultureInfo", Currency_CultureInfo);
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

        public DataSet GetInvoiceMaster_by_WorkpackageUID(Guid WorkpackageUID)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlConnection con = new SqlConnection(db.GetConnectionString());
                SqlDataAdapter cmd = new SqlDataAdapter("usp_GetInvoiceMaster_by_WorkpackageUID", con);
                cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                cmd.SelectCommand.Parameters.AddWithValue("@WorkpackageUID", WorkpackageUID);
                cmd.Fill(ds);
            }
            catch (Exception ex)
            {
                ds = null;
            }
            return ds;
        }

        public DataSet GetAllInvoiceTotalAmount_by_WorkpackageUID(Guid WorkpackageUID)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlConnection con = new SqlConnection(db.GetConnectionString());
                SqlDataAdapter cmd = new SqlDataAdapter("usp_GetAllInvoiceTotalAmount_by_WorkpackageUID", con);
                cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                cmd.SelectCommand.Parameters.AddWithValue("@WorkpackageUID", WorkpackageUID);
                cmd.Fill(ds);
            }
            catch (Exception ex)
            {
                ds = null;
            }
            return ds;
        }

        public DataSet GetInvoiceTotalAmountUptoPrev_Invoice(Guid WorkpackageUID,Guid InvoiceMaster_UID)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlConnection con = new SqlConnection(db.GetConnectionString());
                SqlDataAdapter cmd = new SqlDataAdapter("usp_GetInvoiceTotalAmountUptoPrev_Invoice", con);
                cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                cmd.SelectCommand.Parameters.AddWithValue("@WorkpackageUID", WorkpackageUID);
                cmd.SelectCommand.Parameters.AddWithValue("@InvoiceMaster_UID", InvoiceMaster_UID);
                cmd.Fill(ds);
            }
            catch (Exception ex)
            {
                ds = null;
            }
            return ds;
        }

        public DataSet GetPrev_CurrentInvoiceDeduction(Guid WorkpackageUID, Guid InvoiceMaster_UID)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlConnection con = new SqlConnection(db.GetConnectionString());
                SqlDataAdapter cmd = new SqlDataAdapter("usp_GetPrev_CurrentInvoiceDeduction", con);
                cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                cmd.SelectCommand.Parameters.AddWithValue("@WorkpackageUID", WorkpackageUID);
                cmd.SelectCommand.Parameters.AddWithValue("@InvoiceMaster_UID", InvoiceMaster_UID);
                cmd.Fill(ds);
            }
            catch (Exception ex)
            {
                ds = null;
            }
            return ds;
        }

        public DataSet GetInvoiceMaster_by_InvoiceMaster_UID(Guid InvoiceMaster_UID)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlConnection con = new SqlConnection(db.GetConnectionString());
                SqlDataAdapter cmd = new SqlDataAdapter("usp_GetInvoiceMaster_by_InvoiceMaster_UID", con);
                cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                cmd.SelectCommand.Parameters.AddWithValue("@InvoiceMaster_UID", InvoiceMaster_UID);
                cmd.Fill(ds);
            }
            catch (Exception ex)
            {
                ds = null;
            }
            return ds;
        }

        public int InvoiceMaster_Delete(Guid InvoiceMaster_UID, Guid UserUID)
        {
            int sresult = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(db.GetConnectionString()))
                {

                    using (SqlCommand cmd = new SqlCommand("usp_InvoiceMaster_Delete"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("@InvoiceMaster_UID", InvoiceMaster_UID);
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

        public int Invoice_RABills_Insert(Guid InvoiceRABill_UID, Guid InvoiceMaster_UID, Guid RABillUid,DateTime InvoiceRABill_Date)
        {
            int cnt = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(db.GetConnectionString()))
                {

                    using (SqlCommand cmd = new SqlCommand("usp_InvoiceRABill_Insert"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("@InvoiceRABill_UID", InvoiceRABill_UID);
                        cmd.Parameters.AddWithValue("@InvoiceMaster_UID", InvoiceMaster_UID);
                        cmd.Parameters.AddWithValue("@RABillUid", RABillUid);
                        cmd.Parameters.AddWithValue("@InvoiceRABill_Date", InvoiceRABill_Date);
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

        public DataSet GetInvoiceRABills_by_InvoiceMaster_UID(Guid InvoiceMaster_UID)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlConnection con = new SqlConnection(db.GetConnectionString());
                SqlDataAdapter cmd = new SqlDataAdapter("usp_GetInvoiceRAbills_by_InvoiceMaster_UID", con);
                cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                cmd.SelectCommand.Parameters.AddWithValue("@InvoiceMaster_UID", InvoiceMaster_UID);
                cmd.Fill(ds);
            }
            catch (Exception ex)
            {
                ds = null;
            }
            return ds;
        }

        public DataSet GetInvoiceRAbills_by_InvoiceRABill_UID(Guid InvoiceRABill_UID)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlConnection con = new SqlConnection(db.GetConnectionString());
                SqlDataAdapter cmd = new SqlDataAdapter("usp_GetInvoiceRAbills_by_InvoiceRABill_UID", con);
                cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                cmd.SelectCommand.Parameters.AddWithValue("@InvoiceRABill_UID", InvoiceRABill_UID);
                cmd.Fill(ds);
            }
            catch (Exception ex)
            {
                ds = null;
            }
            return ds;
        }

        public int InvoiceDeduction_InsertorUpdate(Guid Invoice_DeductionUID, Guid WorkpackageUID, Guid InvoiceMaster_UID, Guid Deduction_UID, float Amount, float Percentage,string Currency,string Currency_CultureInfo,string Deduction_Mode)
        {
            int cnt = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(db.GetConnectionString()))
                {

                    using (SqlCommand cmd = new SqlCommand("usp_InsertorUpdateInvoiceDeductions"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("@Invoice_DeductionUID", Invoice_DeductionUID);
                        cmd.Parameters.AddWithValue("@WorkpackageUID", WorkpackageUID);
                        cmd.Parameters.AddWithValue("@InvoiceMaster_UID", InvoiceMaster_UID);
                        cmd.Parameters.AddWithValue("@Deduction_UID", Deduction_UID);
                        cmd.Parameters.AddWithValue("@Amount", Amount);
                        cmd.Parameters.AddWithValue("@Percentage", Percentage);
                        cmd.Parameters.AddWithValue("@Currency", Currency);
                        cmd.Parameters.AddWithValue("@Currency_CultureInfo", Currency_CultureInfo);
                        cmd.Parameters.AddWithValue("@Deduction_Mode", Deduction_Mode);
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

        public DataSet GetInvoiceDeduction_by_InvoiceMaster_UID(Guid InvoiceMaster_UID)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlConnection con = new SqlConnection(db.GetConnectionString());
                SqlDataAdapter cmd = new SqlDataAdapter("usp_GetInvoiceDeduction_by_InvoiceMaster_UID", con);
                cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                cmd.SelectCommand.Parameters.AddWithValue("@InvoiceMaster_UID", InvoiceMaster_UID);
                cmd.Fill(ds);
            }
            catch (Exception ex)
            {
                ds = null;
            }
            return ds;
        }

        public DataSet GetInvoiceDeduction_by_InvoiceMaster_UID_With_Name(Guid InvoiceMaster_UID)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlConnection con = new SqlConnection(db.GetConnectionString());
                SqlDataAdapter cmd = new SqlDataAdapter("usp_GetInvoiceDeduction_by_InvoiceMaster_UID_With_Name", con);
                cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                cmd.SelectCommand.Parameters.AddWithValue("@InvoiceMaster_UID", InvoiceMaster_UID);
                cmd.Fill(ds);
            }
            catch (Exception ex)
            {
                ds = null;
            }
            return ds;
        }

        public DataSet GetInvoiceDeduction_by_Invoice_DeductionUID(Guid Invoice_DeductionUID)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlConnection con = new SqlConnection(db.GetConnectionString());
                SqlDataAdapter cmd = new SqlDataAdapter("usp_GetInvoiceDeduction_by_Invoice_DeductionUID", con);
                cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                cmd.SelectCommand.Parameters.AddWithValue("@Invoice_DeductionUID", Invoice_DeductionUID);
                cmd.Fill(ds);
            }
            catch (Exception ex)
            {
                ds = null;
            }
            return ds;
        }

        public string GetMobilizationInvoiceDeduction_by_InvoiceMaster_UID(Guid InvoiceMaster_UID)
        {
            string Percent = "";
            try
            {
                SqlConnection con = new SqlConnection(db.GetConnectionString());
                if (con.State == ConnectionState.Closed) con.Open();
                SqlCommand cmd = new SqlCommand("usp_GetMobilizationInvoiceDeduction_by_InvoiceMaster_UID", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@InvoiceMaster_UID", InvoiceMaster_UID);
                Percent = (string)cmd.ExecuteScalar();
                con.Close();
            }
            catch (Exception ex)
            {
                Percent = "0";
            }
            return Percent;
        }

        public string GetInvoiceNumber_by_InvoiceMaster_UID(Guid InvoiceMaster_UID)
        {
            string sUser = "";
            try
            {
                SqlConnection con = new SqlConnection(db.GetConnectionString());
                if (con.State == ConnectionState.Closed) con.Open();
                SqlCommand cmd = new SqlCommand("usp_GetInvoiceNumber_by_InvoiceMaster_UID", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@InvoiceMaster_UID", InvoiceMaster_UID);
                sUser = (string)cmd.ExecuteScalar();
                con.Close();
            }
            catch (Exception ex)
            {
                sUser = "Error : " + ex.Message;
            }
            return sUser;
        }

        public int InvoiceDeduction_Delete(Guid Invoice_DeductionUID, Guid UserUID)
        {
            int sresult = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(db.GetConnectionString()))
                {

                    using (SqlCommand cmd = new SqlCommand("usp_InvoiceDeduction_Delete"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("@Invoice_DeductionUID", Invoice_DeductionUID);
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

        public int InvoiceDeduction_Amount_Update(Guid Invoice_DeductionUID, Guid InvoiceMaster_UID, float Amount)
        {
            int cnt = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(db.GetConnectionString()))
                {

                    using (SqlCommand cmd = new SqlCommand("usp_InvoiceDeduction_Amount_Update"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("@Invoice_DeductionUID", Invoice_DeductionUID);
                        cmd.Parameters.AddWithValue("@InvoiceMaster_UID", InvoiceMaster_UID);
                        cmd.Parameters.AddWithValue("@Amount", Amount);
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

        public double GetTotalRABillValue_by_RABillUid(Guid RABillUid)
        {
            double BillValue = 0;
            try
            {
                SqlConnection con = new SqlConnection(db.GetConnectionString());
                if (con.State == ConnectionState.Closed) con.Open();
                SqlCommand cmd = new SqlCommand("usp_GetTotalRABillValue_by_RABillUid", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RABillUid", RABillUid);
                BillValue = (double)cmd.ExecuteScalar();
                con.Close();
            }
            catch (Exception ex)
            {
                BillValue = 0;
            }
            return BillValue;
        }

        public decimal GetRAbillPresentTotalAmount_by_RABill_UID(Guid RABill_UID)
        {
            decimal BillValue = 0;
            try
            {
                SqlConnection con = new SqlConnection(db.GetConnectionString());
                if (con.State == ConnectionState.Closed) con.Open();
                SqlCommand cmd = new SqlCommand("usp_GetRAbillPresentTotalAmount_by_RABill_UID", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RABill_UID", RABill_UID);
                BillValue = (decimal)cmd.ExecuteScalar();
                con.Close();
            }
            catch (Exception ex)
            {
                BillValue = 0;
            }
            return BillValue;
        }

        public DataSet GetRAbillAbstract_by_WorkpackageUID(Guid WorkpackageUID)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlConnection con = new SqlConnection(db.GetConnectionString());
                SqlDataAdapter cmd = new SqlDataAdapter("usp_GetRAbillAbstract_by_WorkpackageUID", con);
                cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                cmd.SelectCommand.Parameters.AddWithValue("@WorkpackageUID", WorkpackageUID);
                cmd.Fill(ds);
            }
            catch (Exception ex)
            {
                ds = null;
            }
            return ds;
        }

        public string GetRABillNo_by_InvoiceRABill_UID(Guid InvoiceRABill_UID)
        {
            string sUser = "";
            try
            {
                SqlConnection con = new SqlConnection(db.GetConnectionString());
                if (con.State == ConnectionState.Closed) con.Open();
                SqlCommand cmd = new SqlCommand("usp_GetRABillNo_by_InvoiceRABill_UID", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@InvoiceRABill_UID", InvoiceRABill_UID);
                sUser = (string)cmd.ExecuteScalar();
                con.Close();
            }
            catch (Exception ex)
            {
                sUser = "Error : " + ex.Message;
            }
            return sUser;
        }

        public string GetGST_Calculation_Value(string CalculationItem_For)
        {
            string sValue = "";
            try
            {
                SqlConnection con = new SqlConnection(db.GetConnectionString());
                if (con.State == ConnectionState.Closed) con.Open();
                SqlCommand cmd = new SqlCommand("usp_GetGST_Calculation_Value", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CalculationItem_For", CalculationItem_For);
                sValue = (string)cmd.ExecuteScalar();
                con.Close();
            }
            catch (Exception ex)
            {
                sValue = "Error : " + ex.Message;
            }
            return sValue;
        }

        public DataSet GetJointInspection_by_WorkpackageUID(Guid WorkpackgeUID,Guid RABill_UID)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlConnection con = new SqlConnection(db.GetConnectionString());
                SqlDataAdapter cmd = new SqlDataAdapter("usp_GetJointInspection_by_WorkpackageUID", con);
                cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                cmd.SelectCommand.Parameters.AddWithValue("@WorkpackgeUID", WorkpackgeUID);
                cmd.SelectCommand.Parameters.AddWithValue("@RABill_UID", RABill_UID);
                cmd.Fill(ds);
            }
            catch (Exception ex)
            {
                ds = null;
            }
            return ds;
        }

        public int InsertJointInspectiontoRAbill(Guid AssignJointInspectionUID, Guid RABill_UID, Guid RABill_ItemUID, Guid InspectionUID)
        {
            int cnt = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(db.GetConnectionString()))
                {

                    using (SqlCommand cmd = new SqlCommand("usp_InsertJointInspectiontoRAbill"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("@AssignJointInspectionUID", AssignJointInspectionUID);
                        cmd.Parameters.AddWithValue("@RABill_UID", RABill_UID);
                        cmd.Parameters.AddWithValue("@RABill_ItemUID", RABill_ItemUID);
                        cmd.Parameters.AddWithValue("@InspectionUID", InspectionUID);
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

        public int AssignedJointInspection_Delete(Guid RABill_ItemUID, Guid InspectionUID,Guid RABill_UID)
        {
            int sresult = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(db.GetConnectionString()))
                {

                    using (SqlCommand cmd = new SqlCommand("usp_AssignedJointInspection_Delete"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("@RABill_ItemUID", RABill_ItemUID);
                        cmd.Parameters.AddWithValue("@InspectionUID", InspectionUID);
                        cmd.Parameters.AddWithValue("@RABill_UID", RABill_UID);
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

        public DataSet GetAssignedJointInspection_by_RABill_ItemUID(Guid RABill_ItemUID,Guid RABill_UID)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlConnection con = new SqlConnection(db.GetConnectionString());
                SqlDataAdapter cmd = new SqlDataAdapter("usp_GetAssignedJointInspection_by_RABill_ItemUID", con);
                cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                cmd.SelectCommand.Parameters.AddWithValue("@RABill_ItemUID", RABill_ItemUID);
                cmd.SelectCommand.Parameters.AddWithValue("@RABill_UID", RABill_UID);
                cmd.Fill(ds);
            }
            catch (Exception ex)
            {
                ds = null;
            }
            return ds;
        }

        public DataSet GetJointInspection_by_WorkpackageUID_ItemUID(Guid WorkpackgeUID, Guid RABill_ItemUID)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlConnection con = new SqlConnection(db.GetConnectionString());
                SqlDataAdapter cmd = new SqlDataAdapter("usp_JointInspection_by_WorkpackgeUID_BOQItemUID", con);
                cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                cmd.SelectCommand.Parameters.AddWithValue("@RABill_ItemUID", RABill_ItemUID);
                cmd.SelectCommand.Parameters.AddWithValue("@WorkpackgeUID", WorkpackgeUID);
                cmd.Fill(ds);
            }
            catch (Exception ex)
            {
                ds = null;
            }
            return ds;
        }

        public DataSet GetJointInspection_by_ProjectUID_ItemUID(Guid ProjectUID, Guid RABill_ItemUID)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlConnection con = new SqlConnection(db.GetConnectionString());
                SqlDataAdapter cmd = new SqlDataAdapter("usp_JointInspection_by_ProjectUID_BOQItemUID", con);
                cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                cmd.SelectCommand.Parameters.AddWithValue("@RABill_ItemUID", RABill_ItemUID);
                cmd.SelectCommand.Parameters.AddWithValue("@ProjectUID", ProjectUID);
                cmd.Fill(ds);
            }
            catch (Exception ex)
            {
                ds = null;
            }
            return ds;
        }

        public double GetTotalJointInspectionQuantity_by_RAbillItem(Guid RABill_ItemUID,Guid RABill_UID)
        {
            double BillValue = 0;
            SqlConnection con = new SqlConnection(db.GetConnectionString());
            try
            {
                
                if (con.State == ConnectionState.Closed) con.Open();
                SqlCommand cmd = new SqlCommand("usp_GetTotalJointInspectionQuantity_by_RAbillItem", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RABill_ItemUID", RABill_ItemUID);
                cmd.Parameters.AddWithValue("@RABill_UID", RABill_UID);
                BillValue = (double)cmd.ExecuteScalar();
                con.Close();
            }
            catch (Exception ex)
            {
                BillValue = 0;
                con.Close();
                con.Dispose();
            }
            return BillValue;
        }

        public double GetPrevJointInspectionQuantity_by_RAbillItem(Guid RABill_ItemUID, Guid RABill_UID)
        {
            double BillValue = 0;
            SqlConnection con = new SqlConnection(db.GetConnectionString());
            try
            {
                if (con.State == ConnectionState.Closed) con.Open();
                SqlCommand cmd = new SqlCommand("usp_GetPrevJointInspectionQuantity_by_RAbillItem", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RABill_ItemUID", RABill_ItemUID);
                cmd.Parameters.AddWithValue("@RABill_UID", RABill_UID);
                BillValue = (double)cmd.ExecuteScalar();
                con.Close();
            }
            catch (Exception ex)
            {
                BillValue = 0;
                con.Close();
                con.Dispose();
            }
            return BillValue;
        }

        public int PaymentBreakupType_InsertOrUpdate(Guid Breakup_UID, string Breakup_Description)
        {
            int cnt = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(db.GetConnectionString()))
                {

                    using (SqlCommand cmd = new SqlCommand("usp_InsertorUpdatePaymentBreakupTypes"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("@Breakup_UID", Breakup_UID);
                        cmd.Parameters.AddWithValue("@Breakup_Description", Breakup_Description);
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

        public DataSet GetAllPaymentBreakupTypes()
        {
            DataSet ds = new DataSet();
            try
            {
                SqlConnection con = new SqlConnection(db.GetConnectionString());
                SqlDataAdapter cmd = new SqlDataAdapter("usp_GetAllPaymentBreakupTypes", con);
                cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                cmd.Fill(ds);
            }
            catch (Exception ex)
            {
                ds = null;
            }
            return ds;
        }

        public DataSet GetPaymentBreakupTypes_by_Breakup_UID(Guid Breakup_UID)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlConnection con = new SqlConnection(db.GetConnectionString());
                SqlDataAdapter cmd = new SqlDataAdapter("usp_GetPaymentBreakupTypes_by_Breakup_UID", con);
                cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                cmd.SelectCommand.Parameters.AddWithValue("@Breakup_UID", Breakup_UID);
                cmd.Fill(ds);
            }
            catch (Exception ex)
            {
                ds = null;
            }
            return ds;
        }

        public string GetRABillUID_by_InvoiceRABill_UID(Guid InvoiceRABill_UID)
        {
            string sUser = "";
            try
            {
                SqlConnection con = new SqlConnection(db.GetConnectionString());
                if (con.State == ConnectionState.Closed) con.Open();
                SqlCommand cmd = new SqlCommand("usp_GetRABillUID_by_InvoiceRABill_UID", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@InvoiceRABill_UID", InvoiceRABill_UID);
                sUser = (string)cmd.ExecuteScalar();
                con.Close();
            }
            catch (Exception ex)
            {
                sUser = "Error : " + ex.Message;
            }
            return sUser;
        }

        // added on 26/05/2021
        public DataSet GetRABillPriceAdj_Master(Guid WorkpackageUID)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlConnection con = new SqlConnection(db.GetConnectionString());
                SqlDataAdapter cmd = new SqlDataAdapter("usp_GetRABillPriceAdj_Master", con);
                cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                cmd.SelectCommand.Parameters.AddWithValue("@WorkpackageUID", WorkpackageUID);
                cmd.Fill(ds);
            }
            catch (Exception ex)
            {
                ds = null;
            }
            return ds;
        }

        public int InsertRABillPriceAdj_Master(Guid UID, Guid WorkPackageUID, Guid RABillUID, string Description, DateTime InitialIndicesDate, DateTime LatestIndicesDate, Decimal RABillAmount, Decimal PriceAdjFActor, Decimal PriceAdjValue, Decimal RecievedAmount, Decimal BalanceAmount)
        {
            int cnt = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(db.GetConnectionString()))
                {

                    using (SqlCommand cmd = new SqlCommand("usp_RABillPriceAdj_Master_InsertUpdate"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("@UID", UID);
                        cmd.Parameters.AddWithValue("@WorkPackageUID", WorkPackageUID);
                        cmd.Parameters.AddWithValue("@RABillUID", RABillUID);
                        cmd.Parameters.AddWithValue("@Description", Description);
                        cmd.Parameters.AddWithValue("@InitialIndicesDate", InitialIndicesDate);
                        cmd.Parameters.AddWithValue("@LatestIndicesDate", LatestIndicesDate);
                        cmd.Parameters.AddWithValue("@RABillAmount", RABillAmount);
                        cmd.Parameters.AddWithValue("@PriceAdjFActor", PriceAdjFActor);
                        cmd.Parameters.AddWithValue("@PriceAdjValue", PriceAdjValue);
                        cmd.Parameters.AddWithValue("@RecievedAmount", RecievedAmount);
                        cmd.Parameters.AddWithValue("@BalanceAmount", BalanceAmount);
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

        public int InsertRABillPriceAdj_Details(Guid UID, Guid MasterUID, Guid ItemUID, string ItemDescription, string SourceIndex, Decimal ProposedWeighting, Decimal Coefficient, Decimal InitialIndiceValue, Decimal LatestIndiceValue, Decimal PriceAdjFactor)
        {
            int cnt = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(db.GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("usp_RABillPriceAdj_Details_InsertUpdate"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("@UID", UID);
                        cmd.Parameters.AddWithValue("@MasterUID", MasterUID);
                        cmd.Parameters.AddWithValue("@ItemUID", ItemUID);
                        cmd.Parameters.AddWithValue("@ItemDescription", ItemDescription);
                        cmd.Parameters.AddWithValue("@SourceIndex", SourceIndex);
                        cmd.Parameters.AddWithValue("@ProposedWeighting", ProposedWeighting);
                        cmd.Parameters.AddWithValue("@Coefficient", Coefficient);
                        cmd.Parameters.AddWithValue("@InitialIndiceValue", InitialIndiceValue);
                        cmd.Parameters.AddWithValue("@LatestIndiceValue", LatestIndiceValue);
                        cmd.Parameters.AddWithValue("@PriceAdjFactor", PriceAdjFactor);
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

        public DataSet GetRABillPriceAdj_Details(Guid MasterUID)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlConnection con = new SqlConnection(db.GetConnectionString());
                SqlDataAdapter cmd = new SqlDataAdapter("usp_GetRABillsPriceAdjDetails", con);
                cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                cmd.SelectCommand.Parameters.AddWithValue("@MasterUID", MasterUID);
                cmd.Fill(ds);
            }
            catch (Exception ex)
            {
                ds = null;
            }
            return ds;
        }

        public decimal CalculatePriceAdjFactor(decimal Coefficent, decimal InitalIndice, decimal LatestIndice)
        {
            decimal factor = 0.0m;

            if (InitalIndice == 0.0m)
            {
                factor = Coefficent;
            }
            else
            {
                factor = (Coefficent * LatestIndice) / InitalIndice;
            }
            return factor;
        }

        public int UpdateRABillPriceAdjMasterAmnt(Guid UID, decimal PriceAdjFactor, decimal PriceAdjValue, decimal RecievedAmount, decimal BalanceAmount)
        {
            int cnt = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(db.GetConnectionString()))
                {

                    using (SqlCommand cmd = new SqlCommand("usp_UpdateRABillPriceAdjMasterAmnt"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("@UID", UID);
                        cmd.Parameters.AddWithValue("@PriceAdjFactor", PriceAdjFactor);
                        cmd.Parameters.AddWithValue("@PriceAdjValue", PriceAdjValue);
                        cmd.Parameters.AddWithValue("@RecievedAmount", RecievedAmount);
                        cmd.Parameters.AddWithValue("@BalanceAmount", BalanceAmount);
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


        public decimal GetPriceAdjFactor(Guid MasterUID)
        {
            decimal BillValue = 0;
            try
            {
                SqlConnection con = new SqlConnection(db.GetConnectionString());
                if (con.State == ConnectionState.Closed) con.Open();
                SqlCommand cmd = new SqlCommand("usp_GetPriceAdjFactor", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MasterUID", MasterUID);
                BillValue = (decimal)cmd.ExecuteScalar();
                con.Close();
            }
            catch (Exception ex)
            {
                BillValue = 0;
            }
            return BillValue;
        }

        public DataSet GetPriceAdjMaster(Guid WorkPackageUID, Guid MasterUID)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlConnection con = new SqlConnection(db.GetConnectionString());
                SqlDataAdapter cmd = new SqlDataAdapter("usp_GetPriceAdjMaster", con);
                cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                cmd.SelectCommand.Parameters.AddWithValue("@WorkPackageUID", WorkPackageUID);
                cmd.SelectCommand.Parameters.AddWithValue("@MasterUID", MasterUID);
                cmd.Fill(ds);
            }
            catch (Exception ex)
            {
                ds = null;
            }
            return ds;
        }

        public DataSet GetRABillPriceADjMAster_UID(Guid UID)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlConnection con = new SqlConnection(db.GetConnectionString());
                SqlDataAdapter cmd = new SqlDataAdapter("usp_GetPriceADjMAster_UID", con);
                cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                cmd.SelectCommand.Parameters.AddWithValue("@UID", UID);

                cmd.Fill(ds);
            }
            catch (Exception ex)
            {
                ds = null;
            }
            return ds;
        }

        public DataSet GetRABillPriceADjDetails_UID(Guid UID)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlConnection con = new SqlConnection(db.GetConnectionString());
                SqlDataAdapter cmd = new SqlDataAdapter("usp_GetRABillPriceADj_Detials_UID", con);
                cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                cmd.SelectCommand.Parameters.AddWithValue("@UID", UID);

                cmd.Fill(ds);
            }
            catch (Exception ex)
            {
                ds = null;
            }
            return ds;
        }

        public int DeleteRABillPriceAdjMaster(Guid UID, Guid DeleteBy)
        {
            int cnt = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(db.GetConnectionString()))
                {

                    using (SqlCommand cmd = new SqlCommand("usp_DeleteRABillPriceAdjMaster"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("@UID", UID);
                        cmd.Parameters.AddWithValue("@DeleteBy", DeleteBy);
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

        public int DeleteRABillPriceAdjDetails(Guid UID, Guid DeleteBy)
        {
            int cnt = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(db.GetConnectionString()))
                {

                    using (SqlCommand cmd = new SqlCommand("usp_DeleteRABillPriceAdjDetails"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("@UID", UID);
                        cmd.Parameters.AddWithValue("@DeleteBy", DeleteBy);
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

        public decimal GetPriceAdjWieghting(Guid MasterUID)
        {
            decimal BillValue = 0;
            try
            {
                SqlConnection con = new SqlConnection(db.GetConnectionString());
                if (con.State == ConnectionState.Closed) con.Open();
                SqlCommand cmd = new SqlCommand("usp_GetPriceAdjWieghting", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MasterUID", MasterUID);
                BillValue = (decimal)cmd.ExecuteScalar();
                con.Close();
            }
            catch (Exception ex)
            {
                BillValue = 0;
            }
            return BillValue;
        }

    }
}