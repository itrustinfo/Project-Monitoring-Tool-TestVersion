using ProjectManagementTool.DAL;
using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagementTool._modal_pages
{
    public partial class add_rabillitem : System.Web.UI.Page
    {
        Invoice invoice = new Invoice();
        DBGetData dbObj = new DBGetData();
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
                    if (Request.QueryString["WorkpackageUID"] != null)
                    {
                        LinkBOQData.HRef = "/_modal_pages/boq-treeview.aspx?ProjectUID=" + dbObj.getProjectUIDby_WorkpackgeUID(new Guid(Request.QueryString["WorkpackageUID"]));
                    }
                    
                    if (Session["BOQData"] != null)
                    {
                        lblActivityName.Visible = true;
                        LinkBOQData.Visible = false;
                        lblActivityName.Text = dbObj.GetBOQItemNumber_by_BOQDetailsUID(new Guid(Session["BOQData"].ToString()));
                    }
                    else
                    {
                        lblActivityName.Visible = false;
                        LinkBOQData.Visible = true;
                    }
                }
            }
        }

        protected void btnaddrabillitem_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["BOQData"] == null)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Please choose BOQ Item...');</script>");
                }
                else
                {
                    string sDate1 = "";
                    DateTime CDate1 = DateTime.Now;

                    sDate1 = DateTime.Now.ToString("dd/MM/yyyy");
                    //sDate1 = sDate1.Split('/')[1] + "/" + sDate1.Split('/')[0] + "/" + sDate1.Split('/')[2];
                    sDate1 = dbObj.ConvertDateFormat(sDate1);
                    CDate1 = Convert.ToDateTime(sDate1);

                    string ProjectUID = dbObj.getProjectUIDby_WorkpackgeUID(new Guid(Request.QueryString["WorkpackageUID"]));
                    int cnt = dbObj.InsertRABillsItems(Request.QueryString["RABillUid"], lblActivityName.Text, txtradescription.Text, CDate1.ToString(), "0", new Guid(ProjectUID), new Guid(Request.QueryString["WorkpackageUID"]),new Guid(Session["BOQData"].ToString()));
                    if (cnt > 0)
                    {
                        Session["BOQData"] = null;
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
                    }
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code : ARABI-1, There is a problem with this feature. Please contact system admin.');</script>");
            }
        }
    }
}