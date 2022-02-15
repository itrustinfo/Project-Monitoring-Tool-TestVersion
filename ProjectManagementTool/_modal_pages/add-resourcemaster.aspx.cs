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
    public partial class add_resourcemaster : System.Web.UI.Page
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
                    BindResourceCostType();
                    BindUnit();
                    BindResourceType();
                    //txtCost.Attributes.Add("onkeyup", "javascript:return allnumericplusminus(this)");
                    //txtGST.Attributes.Add("onkeyup", "javascript:return allnumericplusminus(this)");
                    if (Request.QueryString["type"] != null)
                    {
                        ResourceBind();
                    }

                }
            }
        }

        private void ResourceBind()
        {
            DataSet ds = new DataSet();
            ds = getdata.getResourceMasterDetails(new Guid(Request.QueryString["ResourceUID"].ToString()));
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtResourceName.Text = ds.Tables[0].Rows[0]["ResourceName"].ToString();
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
                if (ds.Tables[0].Rows[0]["Basic_Budget"].ToString() == "0")
                {
                    //txtCost.Value = CurrencySymbol + " " + "0";
                    txtCost.Value = "";
                }
                else
                {
                    txtCost.Value = CurrencySymbol + " " + Convert.ToDouble(ds.Tables[0].Rows[0]["Basic_Budget"].ToString()).ToString("#,##.##", CultureInfo.CreateSpecificCulture(ds.Tables[0].Rows[0]["Currency_CultureInfo"].ToString()));
                }
                if (ds.Tables[0].Rows[0]["Total_Budget"].ToString() == "0")
                {
                    //txtTotalCost.Value = CurrencySymbol + " " + "0";
                    txtTotalCost.Value = "";
                }
                else
                {
                    txtTotalCost.Value = CurrencySymbol + " " + Convert.ToDouble(ds.Tables[0].Rows[0]["Total_Budget"].ToString()).ToString("#,##.##", CultureInfo.CreateSpecificCulture(ds.Tables[0].Rows[0]["Currency_CultureInfo"].ToString())); ;
                }
                if (ds.Tables[0].Rows[0]["GST"].ToString() == "0")
                {
                    txtGST.Value = "";
                }
                else
                {
                    txtGST.Value = ds.Tables[0].Rows[0]["GST"].ToString();
                }
                if (ds.Tables[0].Rows[0]["CostType"].ToString() != "")
                {
                    ddlCostType.SelectedValue = ds.Tables[0].Rows[0]["CostType"].ToString();
                }
                txtdescription.Text = ds.Tables[0].Rows[0]["Resource_Description"].ToString();
                DDLunit.SelectedItem.Text = ds.Tables[0].Rows[0]["Unit_for_Measurement"].ToString();
                DDLResourceType.SelectedValue = ds.Tables[0].Rows[0]["ResourceType_UID"].ToString();
            }
        }
        private void BindUnit()
        {
            DDLunit.DataTextField = "Unit_Name";
            DDLunit.DataValueField = "Unit_UID";
            DDLunit.DataSource = getdata.getUnitMaster_List();
            DDLunit.DataBind();
        }
        private void BindResourceType()
        {
            DDLResourceType.DataTextField = "ResourceType_Name";
            DDLResourceType.DataValueField = "ResourceType_UID";
            DDLResourceType.DataSource = getdata.getResourceTypeMaster_List();
            DDLResourceType.DataBind();
        }
        private void BindResourceCostType()
        {
            ddlCostType.DataTextField = "ResourceCost_Type";
            ddlCostType.DataValueField = "ResourceCost_Type";
            ddlCostType.DataSource = getdata.getResourceCostType();
            ddlCostType.DataBind();
            ddlCostType.Items.Insert(0, new ListItem("--Select Costtype--", ""));
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                Guid ResourceUID = Guid.NewGuid();
                if (Request.QueryString["type"] != null)
                {
                    ResourceUID = new Guid(Request.QueryString["ResourceUID"].ToString());
                }
                txtCost.Value = txtCost.Value.Replace(",", "").Replace("₹ ", "").Replace("$ ", "").Replace("¥ ", "").Replace("₹", "").Replace("$", "").Replace("¥", "").Trim();
                txtGST.Value= txtGST.Value.Replace(",", "");
                txtTotalCost.Value = txtTotalCost.Value.Replace(",", "").Replace("₹ ", "").Replace("$ ", "").Replace("¥ ", "").Replace("₹", "").Replace("$", "").Replace("¥", "").Trim();

                txtCost.Value = (txtCost.Value == "") ? "0" : txtCost.Value;
                txtGST.Value = (txtGST.Value == "") ? "0" : txtGST.Value;
                txtTotalCost.Value = (txtTotalCost.Value == "") ? "0" : txtTotalCost.Value;

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

                bool result = getdata.InsertorUpdateResourceMaster(ResourceUID, new Guid(Request.QueryString["ProjectUID"].ToString()), new Guid(Request.QueryString["WorkPackageUID"].ToString()), txtResourceName.Text, ddlCostType.SelectedValue, Convert.ToDouble(txtCost.Value), Convert.ToDouble(txtGST.Value), Convert.ToDouble(txtTotalCost.Value), txtdescription.Text, 
                    DDLunit.SelectedItem.Text, new Guid(DDLResourceType.SelectedValue), (DDLCurrency.SelectedItem.Text == "₹ (RUPEE)") ? "&#x20B9;" : (DDLCurrency.SelectedItem.Text == "$ (USD)") ? "&#36;" : "&#165;", Currecncy_CultureInfo);
                if (result)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('There is a problem with this feature. Please contact system admin');</script>");
            }
        }
    }
}