using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProjectManagementTool.DAL;
using ProjectManager.DAL;
using System.Data;

namespace ProjectManagementTool._modal_pages
{
    public partial class add_rabill_priceadj_details : System.Web.UI.Page
    {
        DBGetData dbgetdata = new DBGetData();
        TaskUpdate gettk = new TaskUpdate();
        Invoice invoice = new Invoice();
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                if (Request.QueryString["type"] != null)
                {
                    DataSet ds = invoice.GetRABillPriceADjDetails_UID(new Guid(Request.QueryString["UID"].ToString()));
                    if(ds.Tables[0].Rows.Count > 0)
                    {
                        ddlItem.Items.Insert(0, ds.Tables[0].Rows[0]["ItemDescription"].ToString());
                        ddlItem.Enabled = false;
                        txtIndex.Text = ds.Tables[0].Rows[0]["SourceIndex"].ToString();
                        txtweighting.Text = ds.Tables[0].Rows[0]["ProposedWeighting"].ToString();
                        txtCoefficient.Text = ds.Tables[0].Rows[0]["Coefficient"].ToString();
                        txtInitialIndices.Text = ds.Tables[0].Rows[0]["InitialIndiceValue"].ToString();
                        txtLatestIndices.Text = ds.Tables[0].Rows[0]["LatestIndiceValue"].ToString();
                        ViewState["oldvalue"] = txtweighting.Text;
                    }
                    
                }
                else
                {
                    ddlItem.DataSource = invoice.GetPriceAdjMaster(new Guid(Request.QueryString["WorkPackageUID"].ToString()), new Guid(Request.QueryString["MasterUID"].ToString()));
                    ddlItem.DataValueField = "UID";
                    ddlItem.DataTextField = "IndexName";
                    ddlItem.DataBind();
                    ddlItem.Items.Insert(0, "--Select Item--");
                }
                //ddlItem.Items.Insert(0, "--Select Item--");
                //ddlItem.Items.Insert(1, new ListItem("Non Adjustable", Guid.NewGuid().ToString()));
                //ddlItem.Items.Insert(2, new ListItem("Labour",Guid.NewGuid().ToString()));
                //ddlItem.Items.Insert(3, new ListItem("Cement", Guid.NewGuid().ToString()));
                //ddlItem.Items.Insert(4, new ListItem("Steel", Guid.NewGuid().ToString()));
                //ddlItem.Items.Insert(5, new ListItem("Fuel", Guid.NewGuid().ToString()));
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                decimal oldvalue = 0.0m; ;
                decimal totalwt = 0.0m;
                totalwt = invoice.GetPriceAdjWieghting(new Guid(Request.QueryString["MasterUID"].ToString()));
                if (Request.QueryString["type"] == null)
                {
                    if (ddlItem.SelectedIndex == 0)
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "Warning", "<script language='javascript'>alert('Please select Item .');</script>");
                        return;
                    }
                }
                else
                {
                    oldvalue = decimal.Parse(ViewState["oldvalue"].ToString());
                    totalwt = totalwt - oldvalue;
                }
                //check the total weightage cannot be greater than 100%
               
                
                if(totalwt  + decimal.Parse(txtweighting.Text) > 100.0m)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "Warning", "<script language='javascript'>alert('The Total Weightage for Bill Adjustment cannot be greater than 100.Please check and try again.');</script>");
                    return;
                }


                //if(ddlItem.SelectedItem.ToString() == "Non Adjustable")
                //{
                //    txtCoefficient.Text = (decimal.Parse(txtweighting.Text) new Guid(ddlItem.SelectedValue) 0.01m).ToString();
                //}
                Guid DetailsUID = Guid.NewGuid();
                Guid ItemUID = Guid.NewGuid(); 
                Guid RABillUID = new Guid(Session["RABillUID"].ToString());
                if (Request.QueryString["type"] != null) //Edit
                {
                    DetailsUID = new Guid(Request.QueryString["UID"].ToString());
                    ItemUID = Guid.NewGuid();
                }
                else
                {
                    ItemUID =  new Guid(ddlItem.SelectedValue);
                }
                    decimal PriceAdjFactor = invoice.CalculatePriceAdjFactor(decimal.Parse(txtCoefficient.Text), decimal.Parse(txtInitialIndices.Text), decimal.Parse(txtLatestIndices.Text));
                int result = invoice.InsertRABillPriceAdj_Details(DetailsUID, new Guid(Request.QueryString["MasterUID"].ToString()), ItemUID, ddlItem.SelectedItem.ToString(), txtIndex.Text, decimal.Parse(txtweighting.Text), decimal.Parse(txtCoefficient.Text), decimal.Parse(txtInitialIndices.Text), decimal.Parse(txtLatestIndices.Text),PriceAdjFactor);
                if(result !=0)
                {
                    decimal RABillValue = invoice.GetRAbillPresentTotalAmount_by_RABill_UID(RABillUID);
                    decimal AdjFactor = invoice.GetPriceAdjFactor(new Guid(Request.QueryString["MasterUID"].ToString()));
                    decimal PriceAdjValue = RABillValue * AdjFactor;
                    decimal RecievedAmount = RABillValue;
                    decimal BalanceAmount = PriceAdjValue - RecievedAmount;
                    //if(BalanceAmount < 0)
                    //{
                    //    BalanceAmount = 0;
                    //}
                    result = invoice.UpdateRABillPriceAdjMasterAmnt(new Guid(Request.QueryString["MasterUID"].ToString()), AdjFactor, PriceAdjValue, RecievedAmount, BalanceAmount);
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "Warning", "<script language='javascript'>alert('There was some issue with the insert.Please contact system admin!.');</script>");
                }

            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        
    }
}