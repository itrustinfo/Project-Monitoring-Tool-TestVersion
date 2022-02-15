using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagementTool._modal_pages
{
    public partial class add_project : System.Web.UI.Page
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
                    BindProjectClass();
                    if (Request.QueryString["type"] == "edit")
                    {
                        BindProjectDetails(Request.QueryString["ProjectUID"]);
                    }
                }
                
            }
        }

        private void BindProjectClass()
        {
            DataSet ds = getdata.ProjectClass_Select_All();
            DDLProjectClass.DataTextField = "ProjectClass_Name";
            DDLProjectClass.DataValueField = "ProjectClass_UID";
            DDLProjectClass.DataSource = ds;
            DDLProjectClass.DataBind();
        }
        private void BindProjectDetails(string ProjectUID)
        {
            DataSet ds = getdata.GetProject_by_ProjectUID(new Guid(ProjectUID));
            if (ds.Tables[0].Rows.Count > 0)
            {
                
                txtprojectName.Text = ds.Tables[0].Rows[0]["ProjectName"].ToString();
                //txtownername.Text = ds.Tables[0].Rows[0]["OwnerName"].ToString();
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
                if (ds.Tables[0].Rows[0]["Budget"].ToString() == "0")
                {
                    txtbudget.Value = CurrencySymbol + " " + "0";
                }
                else
                {
                    txtbudget.Value = CurrencySymbol + " " + Convert.ToDouble(ds.Tables[0].Rows[0]["Budget"].ToString()).ToString("#,##.##", CultureInfo.CreateSpecificCulture(ds.Tables[0].Rows[0]["Currency_CultureInfo"].ToString()));
                }

                if (ds.Tables[0].Rows[0]["ActualExpenditure"].ToString() == "0")
                {
                    txtActualExpenditure.Value = CurrencySymbol + " " + "0";
                }
                else
                {
                    txtActualExpenditure.Value = CurrencySymbol + " " + Convert.ToDouble(ds.Tables[0].Rows[0]["ActualExpenditure"].ToString()).ToString("#,##.##", CultureInfo.CreateSpecificCulture(ds.Tables[0].Rows[0]["Currency_CultureInfo"].ToString()));
                }
                
                txtprojectcode.Text = ds.Tables[0].Rows[0]["ProjectAbbrevation"].ToString();
                txtFundingAgency.Text = ds.Tables[0].Rows[0]["Funding_Agency"].ToString();
                //DDLCurrency.SelectedValue= ds.Tables[0].Rows[0]["Currency"].ToString();
                //string ccc = ds.Tables[0].Rows[0]["Currency"].ToString();

                if (ds.Tables[0].Rows[0]["ProjectClass_UID"].ToString() != "")
                {
                    DDLProjectClass.SelectedValue = ds.Tables[0].Rows[0]["ProjectClass_UID"].ToString();
                }
                
                 if (ds.Tables[0].Rows[0]["StartDate"].ToString() != null && ds.Tables[0].Rows[0]["StartDate"].ToString() != "")
                {
                    dtStartDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["StartDate"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                if (ds.Tables[0].Rows[0]["PlannedEndDate"].ToString() != null && ds.Tables[0].Rows[0]["PlannedEndDate"].ToString() != "")
                {
                    dtPlannedEndDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["PlannedEndDate"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                if (ds.Tables[0].Rows[0]["ProjectedEndDate"].ToString() != null && ds.Tables[0].Rows[0]["ProjectedEndDate"].ToString() != "")
                {
                    dtProjectedEndDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["ProjectedEndDate"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
            }
        }
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                Guid ProjectUID;
                if (Request.QueryString["ProjectUID"] != null)
                {
                    ProjectUID = new Guid(Request.QueryString["ProjectUID"]);
                }
                else
                {
                    ProjectUID = Guid.NewGuid();
                }

                string sDate1 = "", sDate2 = "", sDate3 = "";
                DateTime CDate1 = DateTime.Now, CDate2 = DateTime.Now, CDate3 = DateTime.Now;

                sDate1 = dtStartDate.Text;
                //sDate1 = sDate1.Split('/')[1] + "/" + sDate1.Split('/')[0] + "/" + sDate1.Split('/')[2];
                sDate1 = getdata.ConvertDateFormat(sDate1);
                CDate1 = Convert.ToDateTime(sDate1);
                //
                sDate2 = dtPlannedEndDate.Text;
                //sDate2 = sDate2.Split('/')[1] + "/" + sDate2.Split('/')[0] + "/" + sDate2.Split('/')[2];
                sDate2 = getdata.ConvertDateFormat(sDate2);
                CDate2 = Convert.ToDateTime(sDate2);
                //
                sDate3 = dtProjectedEndDate.Text;
                //sDate3 = sDate3.Split('/')[1] + "/" + sDate3.Split('/')[0] + "/" + sDate3.Split('/')[2];
                sDate3 = getdata.ConvertDateFormat(sDate3);
                CDate3 = Convert.ToDateTime(sDate3);

                txtbudget.Value = txtbudget.Value.Replace(",", "").Replace("₹ ", "").Replace("$ ", "").Replace("¥ ", "").Replace("₹", "").Replace("$", "").Replace("¥", "").Trim();
                txtActualExpenditure.Value = txtActualExpenditure.Value.Replace(",", "").Replace("₹ ", "").Replace("$ ", "").Replace("¥ ", "").Replace("₹", "").Replace("$", "").Replace("¥", "").Trim();
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
                int cnt = getdata.InsertorUpdateProjects(ProjectUID, new Guid(DDLProjectClass.SelectedValue),txtprojectName.Text, txtprojectcode.Text, txtFundingAgency.Text, "", CDate1, CDate2, CDate3, "P", Convert.ToDouble(txtbudget.Value), Convert.ToDouble(txtActualExpenditure.Value), (DDLCurrency.SelectedItem.Text == "₹ (RUPEE)") ? "&#x20B9;" : (DDLCurrency.SelectedItem.Text == "$ (USD)") ? "&#36;" : "&#165;", Currecncy_CultureInfo);
                if (cnt > 0)
                {
                    if (Session["TypeOfUser"].ToString() == "PA" && Request.QueryString["ProjectUID"] == null)
                    {
                        Guid AssignUID= Guid.NewGuid();
                        string UserRole = getdata.GetUserRoleID_by_UserRoleName(Session["TypeOfUser"].ToString());
                        int ret = getdata.InsertorUpdateAssignProjects(AssignUID, new Guid(Session["UserUID"].ToString()), ProjectUID, new Guid(UserRole));
                        if (ret <= 0)
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code : AP-02 there is a problem with this feature. Please contact system admin.');</script>");
                        }
                    }
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
                }
                else if (cnt == -1)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Project Name already exists. Please try with different project name.');</script>");
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code : AP-01 there is a problem with this feature. Please contact system admin.');</script>");
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code : AP-01 there is a problem with this feature. Please contact system admin.');</script>");
            }
        }
    }
}