using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ProjectManager.DAL
{
    public class InventoryCS
    {


        SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["PMConnectionString"].ToString());
        //SqlConnection con = new SqlConnection("Server=ITRUST-PC\\SQLEXPRESS;DataBase=EdifyERP;Uid=sa;Pwd=itrust;");
        SqlDataAdapter da;
        SqlCommand cmd;

        public DataTable GetInvoice_List()
        {

            DataTable dtitems = new DataTable();
            try
            {
                da = new SqlDataAdapter("usp_getInvoiceList", con);
                da.Fill(dtitems);
            }
            catch (Exception ex)
            {
            }
            return dtitems;
        }

        public DataTable GetInvoice_List_by_InvoiceID(Guid InvoiceID)
        {

            DataTable dtInvoice = new DataTable();
            try
            {
                cmd = new SqlCommand("usp_getInvoiceList_by_InvoiceID", con);
                cmd.Parameters.AddWithValue("@InvoiceID", InvoiceID);
                cmd.CommandType = CommandType.StoredProcedure;
                da = new SqlDataAdapter(cmd);
                da.Fill(dtInvoice);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dtInvoice;
        }

        public DataTable GetInvoice_List_by_VendorID(Guid VendorID)
        {

            DataTable dtInvoice = new DataTable();
            try
            {
                cmd = new SqlCommand("usp_getInvoiceList_by_VendorID", con);
                cmd.Parameters.AddWithValue("@VendorID", VendorID);
                cmd.CommandType = CommandType.StoredProcedure;
                da = new SqlDataAdapter(cmd);
                da.Fill(dtInvoice);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dtInvoice;
        }

        public string GetInvoiceNumber_by_InvoiceUID(Guid InvoiceID)
        {
            string ret = string.Empty;
            try
            {
                cmd = new SqlCommand("SP_GetInvoiceNumber_by_InvoiceUID", con);
                cmd.Parameters.AddWithValue("@InvoiceID", InvoiceID);
                cmd.CommandType = CommandType.StoredProcedure;
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                ret = (string)cmd.ExecuteScalar();
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }

            }
            catch (Exception ex)
            {
            }
            return ret;
        }

        public int InvoiceInsertorUpdate(Guid InvoiceID, string InvoiceNumber, Guid VendorID, string PONumber, DateTime InvoiceDate, double Invoice_TotalAmount, string Invoice_File, string Account_Code, Guid TaskUID)
        {
            int cnt = 0;
            try
            {
                // con = new SqlConnection("");
                cmd = new SqlCommand("usp_InsertorUpdateInvoice", con);
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@ItemID", ItemID);
                cmd.Parameters.AddWithValue("@InvoiceID", InvoiceID);
                cmd.Parameters.AddWithValue("@InvoiceNumber", InvoiceNumber);
                cmd.Parameters.AddWithValue("@VendorID", VendorID);
                cmd.Parameters.AddWithValue("@PONumber", PONumber);
                cmd.Parameters.AddWithValue("@InvoiceDate", InvoiceDate);
                cmd.Parameters.AddWithValue("@Invoice_TotalAmount", Invoice_TotalAmount);
                cmd.Parameters.AddWithValue("@Invoice_File", Invoice_File);
                cmd.Parameters.AddWithValue("@Account_Code", Account_Code);
                cmd.Parameters.AddWithValue("@TaskUID", TaskUID);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                cnt = cmd.ExecuteNonQuery();
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }

            }
            catch (Exception ex)
            {
            }
            return cnt;
        }

        public DataTable GetItem_Units()
        {

            DataTable dtitems = new DataTable();
            try
            {
                da = new SqlDataAdapter("usp_getRespurceCostTypeList", con);
                da.Fill(dtitems);
            }
            catch (Exception ex)
            {
            }
            return dtitems;
        }

        public int SaveItems(string GenName, string GenDesc, string GenCnt, DateTime ValidFrom, DateTime ValidTo)
        {
            int cnt = 0;
            try
            {
                // con = new SqlConnection("");
                cmd = new SqlCommand("SP_InsertItems", con);
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@ItemID", ItemID);
                cmd.Parameters.AddWithValue("@GenName", GenName);
                cmd.Parameters.AddWithValue("@GenDesc", GenDesc);
                cmd.Parameters.AddWithValue("@GenCnt", GenCnt);
                cmd.Parameters.AddWithValue("@ValidFrom", ValidFrom);
                cmd.Parameters.AddWithValue("@ValidTo", ValidTo);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                cnt = cmd.ExecuteNonQuery();
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }

            }
            catch (Exception ex)
            {
            }
            return cnt;
        }

        public int SaveItems_WithoutDate(string GenName, string GenDesc, string GenCnt)
        {
            int cnt = 0;
            try
            {
                // con = new SqlConnection("");
                cmd = new SqlCommand("SP_InsertItems_Without_Date", con);
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@ItemID", ItemID);
                cmd.Parameters.AddWithValue("@GenName", GenName);
                cmd.Parameters.AddWithValue("@GenDesc", GenDesc);
                cmd.Parameters.AddWithValue("@GenCnt", GenCnt);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                cnt = cmd.ExecuteNonQuery();
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }

            }
            catch (Exception ex)
            {
            }
            return cnt;
        }

        public int Update_Items(string ItemID, string GenName, string GenDesc, string GenCnt, DateTime ValidFrom, DateTime ValidTo)
        {
            int cnt = 0;
            try
            {
                // con = new SqlConnection("");
                cmd = new SqlCommand("SP_UpdateItems", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ItemID", ItemID);
                cmd.Parameters.AddWithValue("@GenName", GenName);
                cmd.Parameters.AddWithValue("@GenDesc", GenDesc);
                cmd.Parameters.AddWithValue("@GenCnt", GenCnt);
                cmd.Parameters.AddWithValue("@ValidFrom", ValidFrom);
                cmd.Parameters.AddWithValue("@ValidTo", ValidTo);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                cnt = cmd.ExecuteNonQuery();
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }

            }
            catch (Exception ex)
            {
            }
            return cnt;
        }

        public int Update_Items_Without_Date(string ItemID, string GenName, string GenDesc, string GenCnt)
        {
            int cnt = 0;
            try
            {
                // con = new SqlConnection("");
                cmd = new SqlCommand("SP_UpdateItems_Without_Date", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ItemID", ItemID);
                cmd.Parameters.AddWithValue("@GenName", GenName);
                cmd.Parameters.AddWithValue("@GenDesc", GenDesc);
                cmd.Parameters.AddWithValue("@GenCnt", GenCnt);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                cnt = cmd.ExecuteNonQuery();
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }

            }
            catch (Exception ex)
            {
            }
            return cnt;
        }

        public int SaveVenItems(string compName, string Address1, string Address2, string city, string state, string country, string contactnumber, string RegNumber, string PanNumber, string VatNumber, string AccountNo, string BankName, string IFSCCode, string BranchName)
        {
            int cnt = 0;
            try
            {
                // con = new SqlConnection("");
                cmd = new SqlCommand("SP_InsertVenodrsList", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@compName", compName);
                cmd.Parameters.AddWithValue("@Address1", Address1);
                cmd.Parameters.AddWithValue("@Address2", Address2);
                cmd.Parameters.AddWithValue("@city", city);
                cmd.Parameters.AddWithValue("@state", state);
                cmd.Parameters.AddWithValue("@country", country);
                cmd.Parameters.AddWithValue("@contactnumber", contactnumber);
                cmd.Parameters.AddWithValue("@RegNumber", RegNumber);
                cmd.Parameters.AddWithValue("@PanNumber", PanNumber);
                cmd.Parameters.AddWithValue("@VatNumber", VatNumber);
                cmd.Parameters.AddWithValue("@AccountNo", AccountNo);
                cmd.Parameters.AddWithValue("@BankName", BankName);
                cmd.Parameters.AddWithValue("@IFSCCode", IFSCCode);
                cmd.Parameters.AddWithValue("@BranchName", BranchName);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                cnt = cmd.ExecuteNonQuery();
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }

            }
            catch (Exception ex)
            {
            }
            return cnt;
        }

        public int UpdateVenItems(string VendorID, string compName, string Address1, string Address2, string city, string state, string country, string contactnumber, string RegNumber, string PanNumber, string VatNumber, string AccountNo, string BankName, string IFSCCode, string BranchName)
        {
            int cnt = 0;
            try
            {
                // con = new SqlConnection("");
                cmd = new SqlCommand("SP_UpdateVenodrsList", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@VendorID", VendorID);
                cmd.Parameters.AddWithValue("@compName", compName);
                cmd.Parameters.AddWithValue("@Address1", Address1);
                cmd.Parameters.AddWithValue("@Address2", Address2);
                cmd.Parameters.AddWithValue("@city", city);
                cmd.Parameters.AddWithValue("@state", state);
                cmd.Parameters.AddWithValue("@country", country);
                cmd.Parameters.AddWithValue("@contactnumber", contactnumber);
                cmd.Parameters.AddWithValue("@RegNumber", RegNumber);
                cmd.Parameters.AddWithValue("@PanNumber", PanNumber);
                cmd.Parameters.AddWithValue("@VatNumber", VatNumber);
                cmd.Parameters.AddWithValue("@AccountNo", AccountNo);
                cmd.Parameters.AddWithValue("@BankName", BankName);
                cmd.Parameters.AddWithValue("@IFSCCode", IFSCCode);
                cmd.Parameters.AddWithValue("@BranchName", BranchName);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                cnt = cmd.ExecuteNonQuery();
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }

            }
            catch (Exception ex)
            {
            }
            return cnt;
        }

        public int SaveCurrentStockDetails(string ItemID, string ItemName, string ItemDesc, string StoreName, string itemcount)
        {
            int cnt = 0;
            try
            {
                // con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Constring"].ConnectionString);
                cmd = new SqlCommand("SP_InsertCStockDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ItemID", ItemID);
                cmd.Parameters.AddWithValue("@ItemName", ItemName);
                cmd.Parameters.AddWithValue("@ItemDesc", ItemDesc);
                cmd.Parameters.AddWithValue("@StoreName", StoreName);
                cmd.Parameters.AddWithValue("@itemcount", itemcount);

                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                cmd.ExecuteNonQuery();
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }

            }
            catch (Exception ex)
            {
            }
            return cnt;
        }

        public DataTable GetItemsList(string Uid)
        {
            DataTable dtitems = new DataTable();
            try
            {
                cmd = new SqlCommand("SP_GetItemsList", con);
                cmd.Parameters.AddWithValue("@uid", Uid);
                cmd.CommandType = CommandType.StoredProcedure;
                da = new SqlDataAdapter(cmd);
                da.Fill(dtitems);
            }
            catch (Exception ex)
            {
            }
            return dtitems;
        }

        public string GetItem_GName_OR_Desc(string ItemID, string GetFor)
        {
            string ret = string.Empty;
            try
            {
                cmd = new SqlCommand("GetItem_GName_OR_Desc", con);
                cmd.Parameters.AddWithValue("@ItemID", ItemID);
                cmd.Parameters.AddWithValue("@GetFor", GetFor);
                cmd.CommandType = CommandType.StoredProcedure;
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                ret = (string)cmd.ExecuteScalar();
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }

            }
            catch (Exception ex)
            {
            }
            return ret;
        }

        public DataTable GetVendorsList()
        {

            DataTable dtitems = new DataTable();
            try
            {
                da = new SqlDataAdapter("SP_GetVenodrsList", con);
                da.Fill(dtitems);
            }
            catch (Exception ex)
            {
            }
            return dtitems;
        }

        public DataTable GetVendor_by_VendorID(string VendorID)
        {
            DataTable dtVendor = new DataTable();
            try
            {
                cmd = new SqlCommand("Vendor_Select_by_VendorID", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@VendorID", VendorID);
                da = new SqlDataAdapter(cmd);
                da.Fill(dtVendor);
            }
            catch (Exception ex)
            {
            }
            return dtVendor;
        }

        public string Getvendor_CName_by_VendorID(string VendorID)
        {
            string ret = string.Empty;
            try
            {
                cmd = new SqlCommand("Getvendor_CName_by_VendorID", con);
                cmd.Parameters.AddWithValue("@VendorID", VendorID);
                cmd.CommandType = CommandType.StoredProcedure;
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                ret = (string)cmd.ExecuteScalar();
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            catch (Exception ex)
            {
            }
            return ret;
        }

        public int SavePoOrder(string PO_OrderID, string VendorId, string PO_Number, DateTime PO_Date, double PO_TotalAmount, string PO_File, string Account_Code)
        {
            int cnt = 0;
            try
            {
                // con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Constring"].ConnectionString);
                cmd = new SqlCommand("SP_POOrder_Details_Insert", con);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PO_OrderID", PO_OrderID);
                cmd.Parameters.AddWithValue("@VendorId", VendorId);
                cmd.Parameters.AddWithValue("@PO_Number", PO_Number);
                cmd.Parameters.AddWithValue("@PO_Date", PO_Date);
                cmd.Parameters.AddWithValue("@PO_TotalAmount", PO_TotalAmount);
                cmd.Parameters.AddWithValue("@PO_File", PO_File);
                cmd.Parameters.AddWithValue("@Account_Code", Account_Code);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                cnt = cmd.ExecuteNonQuery();
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }

            }
            catch (Exception ex)
            {
            }
            return cnt;
        }

        public int UpdatePoOrder(string PO_OrderID, string VendorId, string PO_Number, DateTime PO_Date, double PO_TotalAmount, string PO_File, string Account_Code)
        {
            int cnt = 0;
            try
            {
                // con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Constring"].ConnectionString);
                cmd = new SqlCommand("SP_POOrder_Details_Update", con);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PO_OrderID", PO_OrderID);
                cmd.Parameters.AddWithValue("@VendorId", VendorId);
                cmd.Parameters.AddWithValue("@PO_Number", PO_Number);
                cmd.Parameters.AddWithValue("@PO_Date", PO_Date);
                cmd.Parameters.AddWithValue("@PO_TotalAmount", PO_TotalAmount);
                cmd.Parameters.AddWithValue("@PO_File", PO_File);
                cmd.Parameters.AddWithValue("@Account_Code", Account_Code);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                cnt = cmd.ExecuteNonQuery();
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }

            }
            catch (Exception ex)
            {
            }
            return cnt;
        }

        public DataTable GetPoOrderDetails()
        {
            DataTable dtPoOrder = new DataTable();
            try
            {
                da = new SqlDataAdapter("SP_Get_POOrder_Details", con);
                da.Fill(dtPoOrder);
            }
            catch (Exception ex)
            {
            }
            return dtPoOrder;
        }

        public DataTable GetPoOrders()
        {
            DataTable dtPoOrder = new DataTable();
            try
            {
                da = new SqlDataAdapter("sp_GetPoOrders", con);
                da.Fill(dtPoOrder);
            }
            catch (Exception ex)
            {
            }
            return dtPoOrder;
        }

        public DataTable GetPoOrderDetails(string PoOrderID)
        {
            DataTable dtItem_PoOrder = new DataTable();
            try
            {
                cmd = new SqlCommand("SP_Get_POOrder_Details_by_PO_OrderNumber", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PO_Number", PoOrderID);
                da = new SqlDataAdapter(cmd);
                da.Fill(dtItem_PoOrder);
            }
            catch (Exception ex)
            {
            }
            return dtItem_PoOrder;
        }

        public string GetPO_Number_by_POOrderID(string PO_OrderID)
        {
            string ret = string.Empty;
            try
            {
                cmd = new SqlCommand("SP_GetPO_Number_by_POOrderID", con);
                cmd.Parameters.AddWithValue("@PO_OrderID", PO_OrderID);
                cmd.CommandType = CommandType.StoredProcedure;
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                ret = (string)cmd.ExecuteScalar();
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }

            }
            catch (Exception ex)
            {
            }
            return ret;
        }

        public int PO_Items_Insert(string PO_OrderID, string Item_ID, int PO_Item_Quantity)
        {
            int cnt = 0;
            try
            {
                // con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Constring"].ConnectionString);
                cmd = new SqlCommand("PO_Items_Insert", con);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PO_OrderID", PO_OrderID);
                cmd.Parameters.AddWithValue("@Item_ID", Item_ID);
                cmd.Parameters.AddWithValue("@PO_Item_Quantity", PO_Item_Quantity);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                cnt = cmd.ExecuteNonQuery();
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }

            }
            catch (Exception ex)
            {
            }
            return cnt;
        }

        public int PO_Items_Update(string PO_ItemID, string PO_OrderID, string Item_ID, int PO_Item_Quantity)
        {
            int cnt = 0;
            try
            {
                // con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Constring"].ConnectionString);
                cmd = new SqlCommand("PO_Items_Update", con);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PO_ItemID", PO_ItemID);
                cmd.Parameters.AddWithValue("@PO_OrderID", PO_OrderID);
                cmd.Parameters.AddWithValue("@Item_ID", Item_ID);
                cmd.Parameters.AddWithValue("@PO_Item_Quantity", PO_Item_Quantity);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                cnt = cmd.ExecuteNonQuery();
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }

            }
            catch (Exception ex)
            {
            }
            return cnt;
        }

        public DataTable Get_Items_by_PO_ID(string PO_OrderID)
        {
            DataTable PO_Items = new DataTable();
            try
            {
                cmd = new SqlCommand("Get_Items_by_PO_ID", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PO_OrderID", PO_OrderID);
                da = new SqlDataAdapter(cmd);
                da.Fill(PO_Items);
            }
            catch (Exception ex)
            {
            }
            return PO_Items;
        }

        public DataTable Get_POItems_by_PO_OrderID(string PO_OrderID)
        {
            DataTable PO_Items = new DataTable();
            try
            {
                cmd = new SqlCommand("Get_POItems_by_PO_OrderID", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PO_OrderID", PO_OrderID);
                da = new SqlDataAdapter(cmd);
                da.Fill(PO_Items);
            }
            catch (Exception ex)
            {
            }
            return PO_Items;
        }

        public int SaveGoodsReceived(string PO_OrderID, string Item_ID, string Received_Qty, DateTime Received_Date)
        {
            int cnt = 0;
            try
            {
                // con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Constring"].ConnectionString);
                cmd = new SqlCommand("SP_InsertGoodsRecived", con);

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@PO_OrderID", PO_OrderID);
                cmd.Parameters.AddWithValue("@Item_ID", Item_ID);
                cmd.Parameters.AddWithValue("@Received_Qty", Received_Qty);
                cmd.Parameters.AddWithValue("@Received_Date", Received_Date);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                cnt = (int)cmd.ExecuteScalar();
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }

            }
            catch (Exception ex)
            {
            }
            return cnt;
        }

        public DataTable GetgoodsRecivedDetails()
        {
            DataTable dtGoodsRecived = new DataTable();
            try
            {
                da = new SqlDataAdapter("sp_GetGoodsRecived", con);
                da.Fill(dtGoodsRecived);
            }
            catch (Exception ex)
            {
            }
            return dtGoodsRecived;
        }

        public string GetGoods_PO_Number_or_Qty(string PO_OrderID, string Item_ID, string GetFor)
        {
            string ret = string.Empty;
            try
            {
                cmd = new SqlCommand("GetGoods_PO_Number_or_Qty", con);
                cmd.Parameters.AddWithValue("@PO_OrderID", PO_OrderID);
                cmd.Parameters.AddWithValue("@Item_ID", Item_ID);
                cmd.Parameters.AddWithValue("@GetFor", GetFor);
                cmd.CommandType = CommandType.StoredProcedure;
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                ret = (string)cmd.ExecuteScalar();
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }

            }
            catch (Exception ex)
            {
            }
            return ret;
        }

        public DataTable GetVendorsList(string VenID)
        {
            DataTable dtitems = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_GetVenodrsList_ID", con);
                cmd.Parameters.Add("@VenID", VenID);
                cmd.CommandType = CommandType.StoredProcedure;
                da = new SqlDataAdapter(cmd);
                da.Fill(dtitems);
            }
            catch (Exception ex)
            {
            }
            return dtitems;
        }


        public int SaveVenItemDetails(string VendorId, string ItemId, string quantity, DateTime QuotationDate, DateTime Validity, string Price)
        {
            int cnt = 0;
            try
            {
                // con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Constring"].ConnectionString);
                cmd = new SqlCommand("SP_InsertVenItemDetails", con);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@VendorID", VendorId);
                cmd.Parameters.AddWithValue("@Validity", Validity);
                cmd.Parameters.AddWithValue("@ItemID", ItemId);
                cmd.Parameters.AddWithValue("@Min_Qty", quantity);
                cmd.Parameters.AddWithValue("@QuotationDate", QuotationDate);
                cmd.Parameters.AddWithValue("@Price_Unit", Price);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                cnt = cmd.ExecuteNonQuery();
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }

            }
            catch (Exception ex)
            {
            }
            return cnt;
        }

        public int UpdateVenItemDetails(string UID, string Min_Qty, DateTime Validity, string Price_Unit)
        {
            int cnt = 0;
            try
            {
                // con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Constring"].ConnectionString);
                cmd = new SqlCommand("SP_UpdateVenItemDetails", con);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UID", UID);
                cmd.Parameters.AddWithValue("@Min_Qty", Min_Qty);
                cmd.Parameters.AddWithValue("@Validity", Validity);
                cmd.Parameters.AddWithValue("@Price_Unit", Price_Unit);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                cnt = cmd.ExecuteNonQuery();
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }

            }
            catch (Exception ex)
            {
            }
            return cnt;
        }

        public DataTable GetVenItemDetails(string VenID)
        {

            DataTable dtVenitems = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_GetVenItems", con);
                cmd.Parameters.Add("@UID", VenID);
                cmd.CommandType = CommandType.StoredProcedure;
                da = new SqlDataAdapter(cmd);
                da.Fill(dtVenitems);
            }
            catch (Exception ex)
            {
            }
            return dtVenitems;
        }

        public DataTable VendorItem_Select_by_VendorID(string VenID)
        {

            DataTable dtVenitems = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("SupplierDetails_Select_by_VendorID", con);
                cmd.Parameters.Add("@VendorID", VenID);
                cmd.CommandType = CommandType.StoredProcedure;
                da = new SqlDataAdapter(cmd);
                da.Fill(dtVenitems);
            }
            catch (Exception ex)
            {
            }
            return dtVenitems;
        }

        public DataTable GetPoOrderDetails_UID(string Uid)
        {
            DataTable dtVenitems = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_Get_POOrder_Details_by_PO_OrderID", con);
                cmd.Parameters.AddWithValue("@PO_OrderID", Uid);
                cmd.CommandType = CommandType.StoredProcedure;
                da = new SqlDataAdapter(cmd);
                da.Fill(dtVenitems);
            }
            catch (Exception ex)
            {
            }
            return dtVenitems;
        }



        internal DataTable GetgoodsRecivedDetails_ID(string Uid)
        {
            DataTable dtVenitems = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("sp_GetGoodsRecived_Item", con);
                cmd.Parameters.AddWithValue("@GoodsRecevied_Guid", Uid);
                cmd.CommandType = CommandType.StoredProcedure;
                da = new SqlDataAdapter(cmd);
                da.Fill(dtVenitems);
            }
            catch (Exception ex)
            {
            }
            return dtVenitems;
        }

        public int UpdateGoodsOrder(string GUID, string Quantity)
        {
            int cnt = 0;
            try
            {
                // con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Constring"].ConnectionString);
                cmd = new SqlCommand("SP_UpdateGSDetails", con);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@GoodsRecevied_Guid", GUID);
                cmd.Parameters.AddWithValue("@Received_Qty", Quantity);

                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                cnt = cmd.ExecuteNonQuery();
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }

            }
            catch (Exception ex)
            {
            }
            return cnt;
        }

        public int GetGoodsReceived_Qty_by_ItemID(string Item_ID)
        {
            int ret = 0;
            try
            {
                cmd = new SqlCommand("Sp_GetReceived_Qty_by_ItemID", con);
                cmd.Parameters.AddWithValue("@Item_ID", Item_ID);
                cmd.CommandType = CommandType.StoredProcedure;
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                ret = (int)cmd.ExecuteScalar();
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }

            }
            catch (Exception ex)
            {
            }
            return ret;
        }
        internal DataTable getStockItems()
        {
            DataTable dtitems = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("getStockItems", con);
                // cmd.Parameters.AddWithValue("@UID", Uid);
                cmd.CommandType = CommandType.StoredProcedure;
                da = new SqlDataAdapter(cmd);
                da.Fill(dtitems);
            }
            catch (Exception ex)
            {
            }
            return dtitems;
        }

        internal int getStockCount(string StockID)
        {
            DataTable dtVenitems = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("sp_GetStockItems", con);
                cmd.Parameters.AddWithValue("@StockID", StockID);
                cmd.CommandType = CommandType.StoredProcedure;
                da = new SqlDataAdapter(cmd);
                da.Fill(dtVenitems);
            }
            catch (Exception ex)
            {
            }
            if (dtVenitems.Rows.Count > 0)
            {
                return Convert.ToInt32(dtVenitems.Rows[0][0]);
            }
            else
            {
                return 0;
            }
        }


        internal int SaveIssuedItems(string EMPID, DateTime issuedDate, string ItemID, string Quantity)
        {
            int cnt = 0;
            try
            {
                // con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Constring"].ConnectionString);
                cmd = new SqlCommand("SP_SaveIssuedDetails", con);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Emp_ID", EMPID);
                cmd.Parameters.AddWithValue("@IssueDate", issuedDate);
                cmd.Parameters.AddWithValue("@ItemID", ItemID);
                cmd.Parameters.AddWithValue("@Issued_Qty", Quantity);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                cnt = cmd.ExecuteNonQuery();
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }

            }
            catch (Exception ex)
            {
            }
            return cnt;
        }

        internal int UpdateIssuedItems(string GUID, string EMPID, string Quantity)
        {
            int cnt = 0;
            try
            {
                // con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Constring"].ConnectionString);
                cmd = new SqlCommand("SP_UpdateIssuedDetails", con);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UID", GUID);
                cmd.Parameters.AddWithValue("@EMPID", EMPID);
                cmd.Parameters.AddWithValue("@Quantity", Quantity);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                cnt = cmd.ExecuteNonQuery();
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }

            }
            catch (Exception ex)
            {
            }
            return cnt;
        }

        internal DataTable GetIssuedItems()
        {
            DataTable dtissuedItems = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_GetIssuedItems", con);
                //cmd.Parameters.AddWithValue("@StockID", StockID);
                cmd.CommandType = CommandType.StoredProcedure;
                da = new SqlDataAdapter(cmd);
                da.Fill(dtissuedItems);
            }
            catch (Exception ex)
            {
            }
            return dtissuedItems;
        }

        internal DataTable GetIssuedItems_by_UID(string UID, string ItemID)
        {
            DataTable dtissuedItems = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_GetIssuedItems_by_UID", con);
                cmd.Parameters.AddWithValue("@UID", UID);
                cmd.Parameters.AddWithValue("@ItemID", ItemID);
                cmd.CommandType = CommandType.StoredProcedure;
                da = new SqlDataAdapter(cmd);
                da.Fill(dtissuedItems);
            }
            catch (Exception ex)
            {
            }
            return dtissuedItems;
        }
        internal DataTable StaffDetails_Name_and_Uid_Select()
        {
            DataTable dtEmp = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("StaffDetails_Name_and_Uid_Select", con);
                //cmd.Parameters.AddWithValue("@StockID", StockID);
                cmd.CommandType = CommandType.StoredProcedure;
                da = new SqlDataAdapter(cmd);
                da.Fill(dtEmp);
            }
            catch (Exception ex)
            {
            }
            return dtEmp;
        }
    }
}