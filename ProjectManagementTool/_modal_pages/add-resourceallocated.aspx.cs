using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagementTool._modal_pages
{
    public partial class add_resourceallocated : System.Web.UI.Page
    {
        DBGetData getdata = new DBGetData();
        DataSet ds = new DataSet();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                //BindResourceMaster();
                //LoadResources();
                BindBOQDetails();
                if (Request.QueryString["ResoureAllocationUID"] != null)
                {
                    DataSet ds = new DataSet();
                    ds = getdata.getTaskResourceAllocatedDetails(new Guid(Request.QueryString["ResoureAllocationUID"].ToString()));
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        txtAllocatedUnits.Text = ds.Tables[0].Rows[0]["AllocatedUnits"].ToString();
                        //ddlResource.SelectedValue = ds.Tables[0].Rows[0]["ResourceUID"].ToString();
                        DDLBOQDetails.SelectedValue = ds.Tables[0].Rows[0]["ResourceUID"].ToString();

                    }
                }

            }
        }
        //private void BindResourceMaster()
        //{
        //    ddlResource.DataSource = getdata.getResourceMaster(new Guid(Request.QueryString["WorkPackageUID"].ToString()));
        //    ddlResource.DataTextField = "ResourceName";
        //    ddlResource.DataValueField = "ResourceUID";
        //    ddlResource.DataBind();
        //}

        protected void ddlResource_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                LoadResources();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void LoadResources()
        {
            //if (ddlResource.SelectedValue != "")
            //{
            //    ds.Clear();
            //    ds = getdata.getResourceMasterDetails(new Guid(ddlResource.SelectedValue));
            //    if (ds.Tables[0].Rows.Count > 0)
            //    {
            //        //txtCostType.Text = ds.Tables[0].Rows[0]["CostType"].ToString();
            //        //txtTotalCost.Text = Convert.ToDouble(ds.Tables[0].Rows[0]["Total_Budget"].ToString()).ToString("#,##0");
            //    }
            //}

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToDouble(txtAllocatedUnits.Text) > Convert.ToDouble(txtquantity.Value))
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Allocated quantity cannot be greater than actual quantity.');</script>");
                }
                else
                {
                    Guid ResourceAllocationUID = Guid.NewGuid();
                    if (Request.QueryString["ResoureAllocationUID"] != null)
                    {
                        ResourceAllocationUID = new Guid(Request.QueryString["ResoureAllocationUID"].ToString());
                    }

                    txtAllocatedUnits.Text = txtAllocatedUnits.Text.Trim();
                    int result = getdata.InsertorUpdateResourceAllocated(ResourceAllocationUID, new Guid(DDLBOQDetails.SelectedValue), new Guid(Request.QueryString["ProjectUID"].ToString()), new Guid(Request.QueryString["WorkPackageUID"].ToString()), new Guid(Request.QueryString["TaskUID"].ToString()), Convert.ToDouble(txtAllocatedUnits.Text), new Guid(Session["UserUID"].ToString()));
                    if (result > 0)
                    {
                        Session["SelectedActivity"] = Request.QueryString["TaskUID"].ToString();
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
                    }
                    else if (result == -1)
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('" + DDLBOQDetails.SelectedItem.Text + " already allocated to this taks.');</script>");
                    }
                    else
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('There is a problem with this feature. Please contact system admin');</script>");
                    }
                }
                
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('There is a problem with this feature. Please contact system admin');</script>");
            }
        }

        private void BindBOQDetails()
        {
            DataSet ds = getdata.GetBOQDetails_by_WorkpackageUID(new Guid(Request.QueryString["WorkPackageUID"]));
            DDLBOQDetails.DataTextField = "Description";
            DDLBOQDetails.DataValueField = "BOQDetailsUID";
            DDLBOQDetails.DataSource = ds;
            DDLBOQDetails.DataBind();
            DDLBOQDetails.Items.Insert(0, new ListItem("--Select--", ""));
        }

        protected void DDLBOQDetails_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDLBOQDetails.SelectedValue != "--Select--")
            {
                DataSet dsBOQ = getdata.GetBOQDetails_by_BOQDetailsUID(new Guid(DDLBOQDetails.SelectedValue));
                if (dsBOQ.Tables[0].Rows.Count > 0)
                {
                    txtquantity.Value = dsBOQ.Tables[0].Rows[0]["Quantity"].ToString();
                    txtGST.Value = dsBOQ.Tables[0].Rows[0]["GST"].ToString();


                    if (dsBOQ.Tables[0].Rows[0]["Currency"].ToString() == "&#x20B9;")
                    {
                        txtprice.Value = "₹ " + dsBOQ.Tables[0].Rows[0]["Price"].ToString();
                    }
                    else if (dsBOQ.Tables[0].Rows[0]["Currency"].ToString() == "&#36;")
                    {
                        txtprice.Value = "$ " + dsBOQ.Tables[0].Rows[0]["Price"].ToString();
                    }
                    else
                    {
                        txtprice.Value = "¥ " + dsBOQ.Tables[0].Rows[0]["Price"].ToString();
                    }
                }
            }

        }
    }
}