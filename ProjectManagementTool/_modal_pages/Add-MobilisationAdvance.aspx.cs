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
    public partial class Add_MobilisationAdvance : System.Web.UI.Page
    {
        DBGetData getdata = new DBGetData();
        DataSet mobilisation_advance = null;
        static Boolean edit_mobilisation_advance;

        protected void Page_Load(object sender, EventArgs e)
        {
                     
            if (!IsPostBack)
            {
                edit_mobilisation_advance = false;

                    if (Request.QueryString["MobilizationAdvanceUID"] != null)
                    {
                        mobilisation_advance = getdata.GetMobilisationAdvanceByID(new Guid(Request.QueryString["MobilizationAdvanceUID"]));

                        if (mobilisation_advance.Tables[0].Rows.Count > 0)
                        {
                            edit_mobilisation_advance = true;
                            hdnMobilizationAdvanceUID.Value = mobilisation_advance.Tables[0].Rows[0]["MobilizationAdvanceUID"].ToString();
                            txtGivenDate.Text = Convert.ToDateTime(mobilisation_advance.Tables[0].Rows[0]["DateGiven"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                            txtInvoiceNo.Text = mobilisation_advance.Tables[0].Rows[0]["InvoiceNo"].ToString();
                            txtAdvance.Text = mobilisation_advance.Tables[0].Rows[0]["AdvanceAmount"].ToString();
                        }
                    }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                int result = 0;

                
                if (edit_mobilisation_advance)
                {
                    result = getdata.InsertMobilisationAdvance(new Guid(hdnMobilizationAdvanceUID.Value), new Guid(Session["ProjectUID"].ToString()), new Guid(Session["WorkPackageUID"].ToString()), txtInvoiceNo.Text, Convert.ToDateTime(getdata.ConvertDateFormat(txtGivenDate.Text)), Convert.ToDecimal(txtAdvance.Text), "Credit", true);
                }
                else
                {
                    result = getdata.InsertMobilisationAdvance(Guid.NewGuid(), new Guid(Session["ProjectUID"].ToString()), new Guid(Session["WorkPackageUID"].ToString()), txtInvoiceNo.Text, Convert.ToDateTime(getdata.ConvertDateFormat(txtGivenDate.Text)), Convert.ToDecimal(txtAdvance.Text), "Credit", false);
                }

                if (result != 0)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>"); 
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "Warning", "<script language='javascript'>alert('There was some issue with the insert.Please contact system admin!.');</script>");
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "Warning", "<script language='javascript'>alert(" +  ex.Message +  ");</script>");
            }

        }
        
    }
}