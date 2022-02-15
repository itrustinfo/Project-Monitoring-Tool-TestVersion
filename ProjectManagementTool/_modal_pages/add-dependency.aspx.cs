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
    public partial class add_dependency : System.Web.UI.Page
    {
        DBGetData getdata = new DBGetData();
        TaskUpdate TKUpdate = new TaskUpdate();
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
                    if (Session["ActivityUID"] != null)
                    {
                        lblActivityName.Visible = true;
                        string ActivityType = Session["ActivityUID"].ToString().Split('*')[0];
                        if (ActivityType == "WkPkg")
                        {
                            lblActivityName.Text = getdata.getWorkPackageNameby_WorkPackageUID(new Guid(Session["ActivityUID"].ToString().Split('*')[1]));
                        }
                        else
                        {
                            lblActivityName.Text = getdata.getTaskNameby_TaskUID(new Guid(Session["ActivityUID"].ToString().Split('*')[1]));
                        }

                        LinkActivity.Visible = false;
                    }
                    else
                    {
                        lblActivityName.Visible = false;
                        LinkActivity.Visible = true;
                    }
                    if (Request.QueryString["type"] == "add")
                    {
                        EditActivity.Visible = false;
                        LinkActivity.HRef = "/_modal_pages/choose-activity.aspx?WorkUID=" + Request.QueryString["WorkUID"] + "&TaskUID=" + Request.QueryString["TaskUID"];
                    }
                    else
                    {
                        BindDependency(Request.QueryString["Dependency_UID"]);
                    }
                }
            }
        }
        private void BindDependency(string Dependency_UID)
        {
            DataSet ds = getdata.getDependencies_by_UID(new Guid(Dependency_UID));
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtdependency.Text = ds.Tables[0].Rows[0]["Dependency_Name"].ToString();
                txtdesc.Text= ds.Tables[0].Rows[0]["Dependency_Desc"].ToString();
                DDLType.SelectedValue = ds.Tables[0].Rows[0]["Dependency_Type"].ToString();
                if (ds.Tables[0].Rows[0]["Dependency_StartDate"].ToString() != "")
                {
                    dtStartDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["Dependency_StartDate"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                if (ds.Tables[0].Rows[0]["Dependency_PlannedEndDate"].ToString() != "")
                {
                    dtEndDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["Dependency_PlannedEndDate"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                LinkActivity.Visible = false;
                lblActivityName.Visible = true;
                EditActivity.Visible = true;
                string DependentTask = getdata.getTaskNameby_TaskUID(new Guid(ds.Tables[0].Rows[0]["Dependent_TaskUID"].ToString()));
                lblActivityName.Text = DependentTask;
                Hidden1.Value = ds.Tables[0].Rows[0]["Dependent_TaskUID"].ToString();
                EditActivity.HRef= "/_modal_pages/choose-activity.aspx?WorkUID=" + ds.Tables[0].Rows[0]["WorkPackageUID"].ToString() + "&TaskUID=" + ds.Tables[0].Rows[0]["TaskUID"].ToString();
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["ActivityUID"] == null && Request.QueryString["type"]=="add")
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Please choose an Activity to Link');</script>");
                }
                else
                {
                    Guid Dependency_UID = new Guid();
                    string DependentTask = "";
                    if (Request.QueryString["type"] == "add")
                    {
                        Dependency_UID = Guid.NewGuid();
                        DependentTask = Session["ActivityUID"].ToString().Split('*')[1];
                    }
                    else
                    {
                        Dependency_UID = new Guid(Request.QueryString["Dependency_UID"].ToString());
                        if (Session["ActivityUID"] != null)
                        {
                            DependentTask = Session["ActivityUID"].ToString().Split('*')[1];
                        }
                        else
                        {
                            DependentTask = Hidden1.Value;
                        }
                    }

                    string sDate1 = "", sDate2 = "";
                    DateTime CDate1 = DateTime.Now, CDate2 = DateTime.Now;
                    if (dtStartDate.Text != "")
                    {
                        sDate1 = dtStartDate.Text;
                        //sDate1 = sDate1.Split('/')[1] + "/" + sDate1.Split('/')[0] + "/" + sDate1.Split('/')[2];
                        sDate1 = getdata.ConvertDateFormat(sDate1);
                        CDate1 = Convert.ToDateTime(sDate1);
                    }
                    //
                    if (dtEndDate.Text != "")
                    {
                        sDate2 = dtEndDate.Text;
                        //sDate2 = sDate2.Split('/')[1] + "/" + sDate2.Split('/')[0] + "/" + sDate2.Split('/')[2];
                        sDate2 = getdata.ConvertDateFormat(sDate2);
                        CDate2 = Convert.ToDateTime(sDate2);
                    }
                    
                                     
                    int Cnt = getdata.InsertorUpdateDependency(Dependency_UID, new Guid(Request.QueryString["WorkUID"]), new Guid(Request.QueryString["TaskUID"]), new Guid(DependentTask), txtdependency.Text, CDate1, CDate2, txtdesc.Text, DDLType.SelectedValue, 0);
                    if (Cnt > 0)
                    {
                        Session["SelectedActivity"] = Request.QueryString["TaskUID"].ToString();
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
                        Session["ActivityUID"] = null;
                    }
                    else
                    {
                        Session["ActivityUID"] = null;
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code : ATD:01 There is a problem with this feature. Please contact system admin.');</script>");
                    }
                }
                
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error:" + ex.Message + "');</script>");
            }
        }
    }
}