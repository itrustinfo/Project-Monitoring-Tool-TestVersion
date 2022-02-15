using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagementTool._modal_pages
{
    public partial class add_boq : System.Web.UI.Page
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
                    BindUnit();
                    if (Request.QueryString["BOQDetailsUID"] != null)
                    {
                        BindBOQ(Request.QueryString["BOQDetailsUID"]);
                    }
                }
            }
        }

        private void BindUnit()
        {
            DDLUnit.DataTextField = "Unit_Name";
            DDLUnit.DataValueField = "Unit_UID";
            DDLUnit.DataSource = getdata.getUnitMaster_List();
            DDLUnit.DataBind();
        }

        private void BindBOQ(string BOQDetailsUID)
        {
            DataSet ds = getdata.GetBOQDetails_by_BOQDetailsUID(new Guid(BOQDetailsUID));
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtitemnumber.Text = ds.Tables[0].Rows[0]["Item_Number"].ToString();
                txtdesc.Text = ds.Tables[0].Rows[0]["Description"].ToString();
                txtquantity.Value = ds.Tables[0].Rows[0]["Quantity"].ToString();

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

                if (ds.Tables[0].Rows[0]["Price"].ToString() == "0")
                {
                    txtprice.Value = CurrencySymbol + " " + "0";
                }
                else
                {
                    txtprice.Value = CurrencySymbol + " " + Convert.ToDouble(ds.Tables[0].Rows[0]["Price"].ToString()).ToString("#,##.##", CultureInfo.CreateSpecificCulture(ds.Tables[0].Rows[0]["Currency_CultureInfo"].ToString()));
                }
                txtGST.Value = ds.Tables[0].Rows[0]["GST"].ToString();
                DDLUnit.SelectedItem.Text = ds.Tables[0].Rows[0]["Unit"].ToString();
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                Guid BOQDetailsUID;
                if (Request.QueryString["BOQDetailsUID"] != null)
                {
                    BOQDetailsUID = new Guid(Request.QueryString["BOQDetailsUID"]);
                }
                else
                {
                    BOQDetailsUID = Guid.NewGuid();
                }
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

                txtprice.Value = txtprice.Value.Replace(",", "").Replace("₹ ", "").Replace("$ ", "").Replace("¥ ", "").Replace("₹", "").Replace("$", "").Replace("¥", "").Trim();
                txtGST.Value = txtGST.Value.Replace(",", "");

                int cnt = getdata.InsertorUpdateBOQDetails_Main(BOQDetailsUID, new Guid(Request.QueryString["WorkPackageUID"]), txtitemnumber.Text, txtdesc.Text, Convert.ToDouble(txtquantity.Value), float.Parse(txtGST.Value), Convert.ToDouble(txtprice.Value), DDLUnit.SelectedItem.Text,
                    (DDLCurrency.SelectedItem.Text == "₹ (RUPEE)") ? "&#x20B9;" : (DDLCurrency.SelectedItem.Text == "$ (USD)") ? "&#36;" : "&#165;", Currecncy_CultureInfo);
                if (cnt > 0)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Add BOQ : There is a problem with this feature. Please contact system admin');</script>");
            }
        }
    }
}