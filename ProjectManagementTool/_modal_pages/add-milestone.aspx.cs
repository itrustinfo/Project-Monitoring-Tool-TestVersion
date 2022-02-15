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
    public partial class add_milestone : System.Web.UI.Page
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
                this.UnobtrusiveValidationMode = System.Web.UI.UnobtrusiveValidationMode.None;
                if (!IsPostBack)
                {
                    dtTargetDate.Attributes.Add("onkeyup", "javascript:return CopyDate(this)");
                    if (Request.QueryString["type"] != null)
                    {
                        DataSet ds = new DataSet();
                        ds = getdata.getMileStonesDetails(new Guid(Request.QueryString["MileStoneUID"].ToString()));
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            txtMileStone.Text = ds.Tables[0].Rows[0]["Description"].ToString();
                            if (ds.Tables[0].Rows[0]["MileStoneDate"].ToString() != "" && ds.Tables[0].Rows[0]["MileStoneDate"].ToString() != null)
                            {
                                dtTargetDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["MileStoneDate"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                            }
                            if (ds.Tables[0].Rows[0]["ProjectedDate"].ToString() != "" && ds.Tables[0].Rows[0]["ProjectedDate"].ToString() != null)
                            {
                                dtprojectDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["ProjectedDate"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                            }
                            ddlStatus.SelectedValue = ds.Tables[0].Rows[0]["Status"].ToString();

                        }
                    }
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                Guid MileStoneUID = Guid.NewGuid();
                if (Request.QueryString["type"] != null)
                {
                    MileStoneUID = new Guid(Request.QueryString["MileStoneUID"].ToString());
                }
                string sDate1 = "", sDate2 = "";
                DateTime CDate1 = DateTime.Now, CDate2 = DateTime.Now;
                //
                if (dtTargetDate.Text != "")
                {
                    sDate1 = dtTargetDate.Text;
                    //sDate1 = sDate1.Split('/')[1] + "/" + sDate1.Split('/')[0] + "/" + sDate1.Split('/')[2];
                    sDate1 = getdata.ConvertDateFormat(sDate1);
                    CDate1 = Convert.ToDateTime(sDate1);
                }

                if (dtprojectDate.Text != "")
                {
                    sDate2 = dtprojectDate.Text;
                    //sDate2 = sDate2.Split('/')[1] + "/" + sDate2.Split('/')[0] + "/" + sDate2.Split('/')[2];
                    sDate2 = getdata.ConvertDateFormat(sDate2);
                    CDate2 = Convert.ToDateTime(sDate2);
                }

                bool result = getdata.InsertorUpdateMileStone(MileStoneUID, new Guid(Request.QueryString["TaskUID"].ToString()), txtMileStone.Text, CDate1, ddlStatus.SelectedValue, DateTime.Now, CDate2, new Guid(Session["UserUID"].ToString()));
                if (result)
                {
                    Session["SelectedActivity"] = Request.QueryString["TaskUID"].ToString();
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