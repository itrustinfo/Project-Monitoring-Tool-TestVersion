using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProjectManager.DAL;
using System.Data;
using ProjectManagementTool.DAL;
using System.IO;

namespace ProjectManagementTool._modal_pages
{
    public partial class add_rabill_rabillitem_invoice : System.Web.UI.Page
    {
        DBGetData dbObj = new DBGetData();
        Invoice invoice = new Invoice();
        protected void Page_Load(object sender, EventArgs e)
        {
           
            if (!Page.IsPostBack)
            {
                if (Request.QueryString["InvoiceMaster_UID"] != null)
                {
                    hidInvoiceUId.Value = Request.QueryString["InvoiceMaster_UID"];
                    dvrabill.Visible = true;
                  
                    GetRABills(Request.QueryString["WorkUID"]);
                    txtInvoiceNumber.Text = invoice.GetInvoiceNumber_by_InvoiceMaster_UID(new Guid(Request.QueryString["InvoiceMaster_UID"]));
                }

                if (Request.QueryString["type"] != null)
                {
                    invoice_RABill.Visible = true;
                    LinkBOQData.HRef = "/_modal_pages/boq-treeview.aspx?ProjectUID=" + Request.QueryString["ProjectUID"];
                }

                if (Session["BOQData"] != null)
                {
                    lblActivityName.Visible = true;
                    LinkBOQData.Visible = false;
                    lblActivityName.Text = dbObj.GetBOQItemNumber_by_BOQDetailsUID(new Guid(Session["BOQData"].ToString()));
                    AddRABillItem.Visible = true;
                    invoice_RABill.Visible = false;
                    LblRABillNumber.Text = Session["RABillNumber"].ToString();
                    HiddenRABillUID.Value = Session["RABillUID"].ToString();

                    Session["RABillUID"] = null;
                    Session["RABillNumber"] = null;
                    //txtaddrabillnumber.Text = Session["RABillNumber"].ToString();
                    //txtDate.Text = Session["RABillDate"].ToString();
                    //Session["RABillNumber"] = null;
                    //Session["RABillDate"] = null;
                }
                else
                {
                    Session["RABillUID"] = null;
                    Session["RABillNumber"] = null;
                    Session["BOQData"] = null;
                    lblActivityName.Visible = false;
                    LinkBOQData.Visible = true;
                    //Session["RABillNumber"] = null;
                    //Session["RABillDate"] = null;
                }
            }
        }

        private void GetInvoices()
        {
            DataTable dtinvoice = dbObj.getInvoiceList();
            for(int i=0;i<dtinvoice.Rows.Count;i++)
            {
                if(dtinvoice.Rows[i]["InvoiceUId"].ToString() == hidInvoiceUId.Value)
                {
                    txtInvoiceNumber.Text = dtinvoice.Rows[i]["Invoice_Number"].ToString();
                }
            }
        }

        private void GetRABills(string WorkpackageUID)
        {
            DataSet dtRABillsAbstarct = invoice.GetRAbillAbstract_by_WorkpackageUID(new Guid(WorkpackageUID));
            ddlRabillNumber.DataSource = dtRABillsAbstarct;
            ddlRabillNumber.DataTextField = "RABillNumber";
            ddlRabillNumber.DataValueField = "RABillUid";
            ddlRabillNumber.DataBind();
            ddlRabillNumber.Items.Insert(0, new ListItem("--Select--", ""));
        }

        protected void btnaddrabill_Click(object sender, EventArgs e)
        {
            try
            {

                string sDate1 = "";
                DateTime CDate1 = DateTime.Now;

                sDate1 = txtDate.Text;
                //sDate1 = sDate1.Split('/')[1] + "/" + sDate1.Split('/')[0] + "/" + sDate1.Split('/')[2];
                sDate1 = dbObj.ConvertDateFormat(sDate1);
                CDate1 = Convert.ToDateTime(sDate1);

                string rabillUid = dbObj.AddRABillNumber(txtaddrabillnumber.Text, new Guid(Request.QueryString["WorkpackageUID"]), CDate1);
                if (rabillUid == "Exists")
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('RA Bill Number already exists.');</script>");
                }
                else if (rabillUid == "Error1")
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('There is a problem with this feature. Please contact system admin.');</script>");
                }
                else
                {
                    // file upload
                    string sFileDirectory = "~/Documents/RABills/" + rabillUid;

                    if (!Directory.Exists(Server.MapPath(sFileDirectory)))
                    {
                        Directory.CreateDirectory(Server.MapPath(sFileDirectory));
                    }
                    
                    foreach (HttpPostedFile uploadedFile in ImageUpload.PostedFiles)
                    {
                        if (uploadedFile.ContentLength > 0 && !String.IsNullOrEmpty(uploadedFile.FileName))
                        {
                            string sFileName = Path.GetFileNameWithoutExtension(uploadedFile.FileName);
                            string Extn = Path.GetExtension(uploadedFile.FileName);
                            uploadedFile.SaveAs(Server.MapPath(sFileDirectory + "/" + sFileName + Extn));
                            string savedPath = sFileDirectory + "/" + sFileName + Extn;
                            string DecryptPagePath = sFileDirectory + "/" + sFileName + "_DE" + Extn;
                            dbObj.EncryptFile(Server.MapPath(savedPath), Server.MapPath(DecryptPagePath));


                            int Cnt = dbObj.RABill_Document_InsertUpdate(Guid.NewGuid(), new Guid(rabillUid), new Guid(Request.QueryString["WorkpackageUID"]), savedPath, txtInvoiceNumber.Text, new Guid(Session["UserUID"].ToString()));
                            
                        }
                    }


                    int ErrorCount = 0;
                    int ItemCount = 0;
                    double totamount = 0;
                    DataSet ds = dbObj.GetBOQDetails_by_projectuid(new Guid(Request.QueryString["ProjectUID"]));
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            string sDate2 = "";
                            DateTime CDate2 = DateTime.Now;

                            sDate2 = DateTime.Now.ToString("dd/MM/yyyy");
                            //sDate1 = sDate1.Split('/')[1] + "/" + sDate1.Split('/')[0] + "/" + sDate1.Split('/')[2];
                            sDate2 = dbObj.ConvertDateFormat(sDate2);
                            CDate2 = Convert.ToDateTime(sDate1);

                            int cnt = dbObj.InsertRABillsItems(rabillUid, ds.Tables[0].Rows[i]["Item_Number"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), CDate1.ToString(), "0", new Guid(Request.QueryString["ProjectUID"]), new Guid(Request.QueryString["WorkpackageUID"]), new Guid(ds.Tables[0].Rows[i]["BOQDetailsUID"].ToString()));
                            if (cnt <= 0)
                            {
                                ErrorCount += 1;
                            }
                            else {
                                ItemCount += 1;
                                totamount += ds.Tables[0].Rows[i]["INR-Amount"].ToString() == "" ? 0 : Convert.ToDouble(ds.Tables[0].Rows[i]["INR-Amount"].ToString());
                            }
                        }
                    }

                    if (ErrorCount > 0)
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('There is a problem linking BOQ details to this RABill. Please contact system admin');</script>");
                    }
                    
                    
                    
                    //AddRABillItem.Visible = true;
                    //invoice_RABill.Visible = false;
                    Session["RABillWorkpackgeUID"] = Request.QueryString["ProjectUID"] + "\\" + Request.QueryString["WorkpackageUID"];
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
                    //HiddenRABillUID.Value = rabillUid;
                    //Session["RABillUID"] = rabillUid;
                    //Session["RABillNumber"] = txtaddrabillnumber.Text;
                    //LblRABillNumber.Text = txtaddrabillnumber.Text;
                }
                    
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('There is a problem with this feature. Please contact system admin.');</script>");
            }

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                Guid InvoiceRABill_UID;
                if (Request.QueryString["InvoiceRABill_UID"] != null)
                {
                    InvoiceRABill_UID = new Guid(Request.QueryString["InvoiceRABill_UID"]);
                }
                else
                {
                    InvoiceRABill_UID = Guid.NewGuid();
                }

                string sDate1 = "";
                DateTime CDate1 = DateTime.Now;

                if (dtRABillDate.Text != "")
                {
                    sDate1 = dtRABillDate.Text;
                }
                else
                {
                    sDate1 = DateTime.Now.ToString("dd/MM/yyyy");
                }

                sDate1 = dbObj.ConvertDateFormat(sDate1);
                CDate1 = Convert.ToDateTime(sDate1);

                int cnt = invoice.Invoice_RABills_Insert(InvoiceRABill_UID, new Guid(Request.QueryString["InvoiceMaster_UID"]), new Guid(ddlRabillNumber.SelectedValue), CDate1);
                //int cnt= dbObj.AddRABillNumber_Invoice(hidInvoiceUId.Value,ddlRabillNumber.SelectedValue.ToString());
                if (cnt > 0)
                {
                    DataSet ds = invoice.GetInvoiceDeduction_by_InvoiceMaster_UID_With_Name(new Guid(Request.QueryString["InvoiceMaster_UID"]));
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        float Mobilization = 0;
                        DataSet dsInvoice = invoice.GetInvoiceMaster_by_InvoiceMaster_UID(new Guid(Request.QueryString["InvoiceMaster_UID"]));
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            float Percent = float.Parse(ds.Tables[0].Rows[i]["Percentage"].ToString());
                            float InvoiceAmount = float.Parse(dsInvoice.Tables[0].Rows[0]["Invoice_TotalAmount"].ToString());
                            if (i == 0)
                            {
                                if (Percent > 0)
                                {
                                    float finalamount = (InvoiceAmount * Percent) / 100;
                                    Mobilization = finalamount; 
                                }
                                else
                                {
                                    Mobilization = InvoiceAmount;
                                }

                                int cnt1 = invoice.InvoiceDeduction_Amount_Update(new Guid(ds.Tables[0].Rows[i]["Invoice_DeductionUID"].ToString()), new Guid(ds.Tables[0].Rows[i]["InvoiceMaster_UID"].ToString()), Mobilization);
                                if (cnt1 <= 0)
                                {
                                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('There is a problem with this feature. Please contact system admin.');</script>");
                                }

                            }
                            else
                            {
                                float finalamount = 0;
                                if (ds.Tables[0].Rows[i]["DeductionsDescription"].ToString() == "SGST" || ds.Tables[0].Rows[i]["DeductionsDescription"].ToString() == "CGST" || ds.Tables[0].Rows[i]["DeductionsDescription"].ToString() == "GST")
                                {
                                    string sVal = invoice.GetGST_Calculation_Value("GST Calculation");
                                    if (sVal != "" && !sVal.StartsWith("Error"))
                                    {
                                        finalamount = (Mobilization / float.Parse(sVal));
                                        finalamount = (finalamount * Percent) / 100;
                                    }
                                }
                                else
                                {
                                    finalamount = (Mobilization * Percent) / 100;
                                }

                                int cnt1 = invoice.InvoiceDeduction_Amount_Update(new Guid(ds.Tables[0].Rows[i]["Invoice_DeductionUID"].ToString()), new Guid(ds.Tables[0].Rows[i]["InvoiceMaster_UID"].ToString()), finalamount);
                                if (cnt1 <= 0)
                                {
                                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('There is a problem with this feature. Please contact system admin.');</script>");
                                }
                            }
                        }
                    }
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
                }
                else if (cnt == -1)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('RA Bill already exists for the invoice. Try with different RA Bill No.');</script>");
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('There is a problem with this feature. Please contact system admin.');</script>");
            }

        }

        protected void btnAddRaBillItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["BOQData"] == null)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Please choose BOQ Item...');</script>");
                }
                else
                {
                    int cnt = dbObj.InsertRABillsItems(HiddenRABillUID.Value, lblActivityName.Text, txtradescription.Text, DateTime.Now.ToString(), "0", new Guid(Request.QueryString["ProjectUID"]), new Guid(Request.QueryString["WorkpackageUID"]), new Guid(Session["BOQData"].ToString()));
                    if (cnt > 0)
                    {
                        Session["BOQData"] = null;
                        //Session["RABillUID"] = null;
                        //Session["RABillNumber"] = null;
                        
                        txtradescription.Text = "";
                        lblActivityName.Visible = false;
                        LinkBOQData.Visible = true;
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Item added Successfully.');</script>");
                    }
                }
            }
            catch (Exception ex)
            {
                Session["BOQData"] = null;
                Session["RABillUID"] = null;
                Session["RABillNumber"] = null;
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error 1: There is a problem with this feature. Please contact system admin.');</script>");
            }
        }
    }
}