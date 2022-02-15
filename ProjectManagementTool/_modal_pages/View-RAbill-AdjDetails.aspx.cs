using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProjectManagementTool.DAL;
using ProjectManager.DAL;

namespace ProjectManagementTool._modal_pages
{
    public partial class View_RAbill_AdjDetails : System.Web.UI.Page
    {
        DBGetData dbgetdata = new DBGetData();
        TaskUpdate gettk = new TaskUpdate();
        Invoice invoice = new Invoice();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadGridData();
                AddDetails.HRef = "/_modal_pages/add-rabill-priceadj-details.aspx?MasterUID=" + Request.QueryString["MasterUID"].ToString() + "&RABillUID=" + Request.QueryString["RABillUID"].ToString() + "&WorkPackageUID= " + Request.QueryString["WorkPackageUID"].ToString();
                Session["RABillUID"] = Request.QueryString["RABillUID"].ToString();
            }
        }

        private void LoadGridData()
        {
            try
            {
                
                GrdRABillAdj.DataSource = invoice.GetRABillPriceAdj_Details(new Guid(Request.QueryString["MasterUID"].ToString()));
                GrdRABillAdj.DataBind();
                //
                tdTotalcf.InnerText = invoice.GetPriceAdjWieghting(new Guid(Request.QueryString["MasterUID"].ToString())).ToString();
                tdTotalf.InnerText = invoice.GetPriceAdjFactor(new Guid(Request.QueryString["MasterUID"].ToString())).ToString();

            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        protected void GrdRABillAdj_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            HiddenField hidRAbuilluid = GrdRABillAdj.Rows[e.RowIndex].FindControl("hidrabillDeleteuid") as HiddenField;
            int result = invoice.DeleteRABillPriceAdjDetails(new Guid(hidRAbuilluid.Value), new Guid(Session["UserUID"].ToString()));
            //
            decimal RABillValue = invoice.GetRAbillPresentTotalAmount_by_RABill_UID(new Guid(Session["RABillUID"].ToString()));
            decimal AdjFactor = invoice.GetPriceAdjFactor(new Guid(Request.QueryString["MasterUID"].ToString()));
            decimal PriceAdjValue = RABillValue * AdjFactor;
            decimal RecievedAmount = RABillValue;
            decimal BalanceAmount = PriceAdjValue - RABillValue;
            result = invoice.UpdateRABillPriceAdjMasterAmnt(new Guid(Request.QueryString["MasterUID"].ToString()), AdjFactor, PriceAdjValue, RecievedAmount, BalanceAmount);
            //
            LoadGridData();
        }
    }
}