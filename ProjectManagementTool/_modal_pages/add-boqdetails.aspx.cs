using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using ProjectManager.DAL;

namespace ProjectManagementTool._modal_pages
{
    public partial class add_boqdetails : System.Web.UI.Page
    {
        DBGetData dbObj = new DBGetData();
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!Page.IsPostBack)
            {
                if(Request.QueryString["projectUid"] != null )
                {
                    if(Guid.TryParse(Request.QueryString["projectUid"].ToString(),out Guid result))
                    {
                        getBOQItems(result, Request.QueryString["parameterType"].ToString());
                        btnSubmit.Visible = true;
                        btnUpdate.Visible = false;
                    }
                    
                }
                if (Request.QueryString["BOQDetailsUid"] != null)
                {
                    if (Guid.TryParse(Request.QueryString["BOQDetailsUid"].ToString(), out Guid result))
                    {
                   
                        getBOQDetails(Request.QueryString["BOQDetailsUid"].ToString());
                        btnSubmit.Visible = false;
                        btnUpdate.Visible = true;
                    }

                }

            }
        }

        private void getBOQDetails(string uid)
        {
            try
            {
                DataSet dsBOQDetails = dbObj.GetBOQDetails_by_BOQDetailsUID(new Guid(uid));
                if(dsBOQDetails.Tables[0].Rows.Count>0)
                {
                    //ddlItems.SelectedValue = uid;
                    hidUid.Value = uid;
                    //ddlItems.Visible = false;
                   // txtitem.Visible = true;
                    txtitemnumber.Text = dsBOQDetails.Tables[0].Rows[0]["item_number"].ToString();
                    txtdesc.Text = dsBOQDetails.Tables[0].Rows[0]["Description"].ToString();
                    txtforeignJpyAmount.Value= dsBOQDetails.Tables[0].Rows[0]["USD-Amount"].ToString();
                    txtforeignJpyAmount.Disabled = true;
                    txtforeignUsdAmount.Value = dsBOQDetails.Tables[0].Rows[0]["JPY-Amount"].ToString();
                    txtforeignUsdAmount.Disabled = true;
                    txtLocalInrAmount.Value = dsBOQDetails.Tables[0].Rows[0]["INR-Amount"].ToString();
                    txtUnit.Value = dsBOQDetails.Tables[0].Rows[0]["Unit"].ToString();
                    txtquantity.Value = dsBOQDetails.Tables[0].Rows[0]["Quantity"].ToString();
                    txtForgignJpyRate.Value = dsBOQDetails.Tables[0].Rows[0]["JPY-Rate"].ToString();
                    txtForgignJpyRate.Disabled = true;
                    txtForgignUsdRate.Value = dsBOQDetails.Tables[0].Rows[0]["USD-Rate"].ToString();
                    txtForgignUsdRate.Disabled = true;
                    txtInrRate.Value = dsBOQDetails.Tables[0].Rows[0]["INR-Rate"].ToString();

                }
            }
            catch( Exception ex)
            {

            }
        }

        private void getBOQItems(Guid projectUid,string parameterType)
        {
            try
            {
                DataTable dtItems = dbObj.getBOQParent_Details(projectUid,parameterType);
               // ddlItems.DataSource = dtItems;
                //ddlItems.DataValueField = "BOQDetailsUID";
                //ddlItems.DataTextField = "Item_Number";
                //ddlItems.DataBind();
                if(dtItems.Rows.Count>0)
                {
                    txtitemnumber.Text = dtItems.Rows[0]["Item_Number"].ToString();
                }

            }
            catch(Exception ex)
            {

            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
               
                int cnt = dbObj.UpdateBOQDetails(txtitemnumber.Text, txtdesc.Text, txtquantity.Value, txtUnit.Value, txtInrRate.Value, txtLocalInrAmount.Value, new Guid(hidUid.Value));
                if (cnt > 0)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code : ACJ-01 there is a problem with this feature. please contact system admin.');</script>");
            }

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                DataSet dtItems =dbObj.GetBOQDetails_by_BOQDetailsUID(new Guid(hidUid.Value));
                string projectUid = "";
                if(dtItems.Tables[0].Rows.Count>0)
                {
                    projectUid = dtItems.Tables[0].Rows[0]["projectuid"].ToString();
                }
              int cnt=   dbObj.InsertBOQDetails(txtitemnumber.Text, txtdesc.Text, txtquantity.Value, txtUnit.Value, txtInrRate.Value, txtForgignJpyRate.Value,
                    txtForgignUsdRate.Value, txtLocalInrAmount.Value, txtforeignJpyAmount.Value, txtforeignUsdAmount.Value, hidUid.Value,
                    new Guid(projectUid));
                if (cnt > 0)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
                }
            }
            catch(Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code : ACJ-01 there is a problem with this feature. please contact system admin.');</script>");
            }
        }
    }
}