using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using ProjectManager.DAL;
using System.Globalization;

namespace ProjectManagementTool._modal_pages
{
    public partial class add_wkpgmaster_data : System.Web.UI.Page
    {
        DBGetData getdt = new DBGetData();
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
                    if (Request.QueryString["WorkPackageUID"] != null)
                    {
                       DataSet ds=getdt.GetWorkPackages_By_WorkPackageUID(new Guid(Request.QueryString["WorkPackageUID"]));
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            if (Request.QueryString["type"] == "budget")
                            {
                                txtoldvalue.Text = ds.Tables[0].Rows[0]["Budget"].ToString();
                                divMain.Visible = true;
                                divDate.Visible = false;
                            }
                            else if (Request.QueryString["type"] == "expenditure")
                            {
                                txtoldvalue.Text = ds.Tables[0].Rows[0]["ActualExpenditure"].ToString();
                                divMain.Visible = true;
                                divDate.Visible = false;
                            }
                            else if (Request.QueryString["type"] == "enddate")
                            {
                                dtoldDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["ProjectedEndDate"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                                divMain.Visible = false;
                                divDate.Visible = true;
                            }
                        }
                    }
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (Request.QueryString["type"] == "budget")
                {
                    getdt.InsertWkpgDataHistory(new Guid(Request.QueryString["WorkPackageUID"]), new Guid(Session["UserUID"].ToString()), Convert.ToDouble(txtoldvalue.Text), Convert.ToDouble(txtnewvalue.Text), 0, 0, DateTime.Now, DateTime.Now, 1);
                }
                else if (Request.QueryString["type"] == "expenditure")
                {
                    getdt.InsertWkpgDataHistory(new Guid(Request.QueryString["WorkPackageUID"]), new Guid(Session["UserUID"].ToString()),0,0, Convert.ToDouble(txtoldvalue.Text), Convert.ToDouble(txtnewvalue.Text),DateTime.Now, DateTime.Now, 2);
                }
                else if (Request.QueryString["type"] == "enddate")
                {
                    string sDate1 = "", sDate2 = "";
                    DateTime CDate1 = DateTime.Now, CDate2 = DateTime.Now;
                    sDate1 = dtoldDate.Text;
                    //sDate2 = sDate2.Split('/')[1] + "/" + sDate2.Split('/')[0] + "/" + sDate2.Split('/')[2];
                    sDate1 = getdt.ConvertDateFormat(sDate1);
                    CDate1 = Convert.ToDateTime(sDate1);

                    sDate2 = dtnewDate.Text;
                    //sDate2 = sDate2.Split('/')[1] + "/" + sDate2.Split('/')[0] + "/" + sDate2.Split('/')[2];
                    sDate2 = getdt.ConvertDateFormat(sDate2);
                    CDate2 = Convert.ToDateTime(sDate2);

                    getdt.InsertWkpgDataHistory(new Guid(Request.QueryString["WorkPackageUID"]), new Guid(Session["UserUID"].ToString()), 0, 0, 0, 0, CDate1, CDate2, 3);
                }
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");

            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Occurred while updating data !');</script>");
            }
        }
    }
}