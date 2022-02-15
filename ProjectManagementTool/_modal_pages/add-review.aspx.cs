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
    public partial class add_review : System.Web.UI.Page
    {
        DBGetData getdata = new DBGetData();
        TaskUpdate gettk = new TaskUpdate();
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
                    BindProject();
                    DDlProject_SelectedIndexChanged(sender, e);
                    BindUsers();
                    BindAttendies();
                    if (Request.QueryString["Reviews_UID"] != null)
                    {
                        BindReviews();
                    }
                }
            }
        }

        private void BindProject()
        {
            DataTable ds = new DataTable();
            if (Session["TypeOfUser"].ToString() == "U" || Session["TypeOfUser"].ToString() =="MD" || Session["TypeOfUser"].ToString() == "VP")
            {
                ds = gettk.GetProjects();
            }
            else if (Session["TypeOfUser"].ToString() == "PA")
            {
                //ds = gettk.GetProjects_by_UserUID(new Guid(Session["UserUID"].ToString()));
                ds = gettk.GetAssignedProjects_by_UserUID(new Guid(Session["UserUID"].ToString()));
            }
            else
            {
                //ds = gettk.GetProjects();
                ds = gettk.GetAssignedProjects_by_UserUID(new Guid(Session["UserUID"].ToString()));
            }
            DDlProject.DataTextField = "ProjectName";
            DDlProject.DataValueField = "ProjectUID";
            DDlProject.DataSource = ds;
            DDlProject.DataBind();

        }

        private void BindAttendies()
        {

            DataSet ds = new DataSet();
            if (Session["TypeOfUser"].ToString() == "U" || Session["TypeOfUser"].ToString() =="MD" || Session["TypeOfUser"].ToString() == "VP")
            {
                ds = getdata.getAllUsers();
            }
            else if (Session["TypeOfUser"].ToString() == "PA")
            {
                ds = getdata.GetUsers_under_ProjectUID(new Guid(DDlProject.SelectedValue));
            }
            else
            {
                ds = getdata.GetUsers_under_ProjectUID(new Guid(DDlProject.SelectedValue));
            }
            lstUsers.DataTextField = "UserName";
            lstUsers.DataValueField = "UserUID";
            lstUsers.DataSource = ds;
            lstUsers.DataBind();
        }

        private void BindReviews()
        {
            DataSet ds = getdata.getReviewList_by_Reviews_UID(new Guid(Request.QueryString["Reviews_UID"]));
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtdesc.Text = ds.Tables[0].Rows[0]["Review_Description"].ToString();
                DDLWorkPackage.SelectedValue = ds.Tables[0].Rows[0]["WorkPackageUID"].ToString();
                DDlProject.SelectedValue = ds.Tables[0].Rows[0]["ProjectUID"].ToString();
                DDLUsers.SelectedValue = ds.Tables[0].Rows[0]["User_UID"].ToString();
                rdList.SelectedItem.Text = ds.Tables[0].Rows[0]["Review_Type"].ToString();
                if (ds.Tables[0].Rows[0]["Review_Type"].ToString() == "One Time")
                {
                    freq.Visible = false;
                    ReviewOneType.Visible = true;
                    if (ds.Tables[0].Rows[0]["Review_Date"].ToString() != null && ds.Tables[0].Rows[0]["Review_Date"].ToString() != "")
                    {
                        dtReviewDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["Review_Date"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    }
                }
                else
                {
                    freq.Visible = true;
                    ReviewOneType.Visible = false;
                    DDLFreq.SelectedValue = ds.Tables[0].Rows[0]["Review_freq"].ToString();
                }
                DataSet ds1 = getdata.getReviewAttendies_by_Reviews_UID(new Guid(ds.Tables[0].Rows[0]["Reviews_UID"].ToString()));
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                    {
                        foreach (ListItem item in lstUsers.Items)
                        {
                            if (ds1.Tables[0].Rows[i]["User_UID"].ToString() == item.Value)
                            {
                                item.Selected = true;
                            }
                        }
                    }
                }
            }
        }

        private void BindUsers()
        {
            DataSet ds = new DataSet();
            if (Session["TypeOfUser"].ToString() == "U" || Session["TypeOfUser"].ToString() =="MD" || Session["TypeOfUser"].ToString() == "VP")
            {
                ds = getdata.getAllUsers();
            }
            else if (Session["TypeOfUser"].ToString() == "PA")
            {
                //ds = getdata.getUsers_by_ProjectUnder(new Guid(DDlProject.SelectedValue));
                ds = getdata.GetUsers_under_ProjectUID(new Guid(DDlProject.SelectedValue));
            }
            else
            {
                ds = getdata.getUsers_by_ProjectUnder(new Guid(DDlProject.SelectedValue));
            }

            DDLUsers.DataTextField = "UserName";
            DDLUsers.DataValueField = "UserUID";
            DDLUsers.DataSource = ds;
            DDLUsers.DataBind();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                int ItemsCount = (from ListItem li in lstUsers.Items
                                  where li.Selected == true
                                  select li).Count();
                if (ItemsCount == 0)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Please choose atleast one user for this Review..');</script>");
                }
                else
                {
                    Guid Reviews_UID = new Guid();
                    if (Request.QueryString["Reviews_UID"] != null)
                    {
                        Reviews_UID = new Guid(Request.QueryString["Reviews_UID"]);
                    }
                    else
                    {
                        Reviews_UID = Guid.NewGuid();
                    }
                    string Freq = string.Empty;
                    string sDate1 = "";
                    string CC = string.Empty;
                    string ToEmailID = "";
                    string sUserName = "";
                    string sHtmlString = string.Empty;
                    DateTime CDate1 = DateTime.Now;
                    DataSet dsUser = new DataSet();
                    string EmailDate = "";
                    if (rdList.SelectedItem.Text == "Periodic")
                    {
                        Freq = DDLFreq.SelectedItem.Text;
                        EmailDate = Freq;
                    }
                    else
                    {
                        sDate1 = dtReviewDate.Text;
                        //sDate1 = sDate1.Split('/')[1] + "/" + sDate1.Split('/')[0] + "/" + sDate1.Split('/')[2];
                        sDate1 = getdata.ConvertDateFormat(sDate1);
                        CDate1 = Convert.ToDateTime(sDate1);
                        EmailDate = CDate1.ToString("dd/MM/yyyy");
                    }
                    int cnt = getdata.InsertorUpdateReviews(Reviews_UID, new Guid(DDLWorkPackage.SelectedValue), new Guid(DDLUsers.SelectedValue), rdList.SelectedValue, Freq, txtdesc.Text, new Guid(DDlProject.SelectedValue), CDate1);
                    dsUser = getdata.getUserDetails(new Guid(DDLUsers.SelectedValue));
                    if (dsUser.Tables[0].Rows.Count > 0)
                    {
                        sUserName = dsUser.Tables[0].Rows[0]["UserName"].ToString();
                        ToEmailID = dsUser.Tables[0].Rows[0]["EmailID"].ToString();
                    }
                        if (cnt > 0)
                     {
                        foreach (ListItem item in lstUsers.Items)
                        {
                            if (item.Selected)
                            {
                                int cnt1 = getdata.InsertReview_Attendies(Guid.NewGuid(), Reviews_UID, new Guid(item.Value));
                                dsUser.Clear();
                                dsUser = getdata.getUserDetails(new Guid(item.Value));
                                if(dsUser.Tables[0].Rows.Count > 0)
                                {
                                    CC += dsUser.Tables[0].Rows[0]["EmailID"].ToString()  + ",";
                                }
                            }
                        }
                        //
                       

                        // store email for Review 
                        CC = CC.TrimEnd(',');
                        sHtmlString = "<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>" + "<html xmlns='http://www.w3.org/1999/xhtml'>" +
                          "<head>" + "<meta http-equiv='Content-Type' content='text/html; charset=utf-8' />" + "<style>table, th, td {border: 1px solid black; padding:6px;}</style></head>" +
                             "<body style='font-family:Verdana, Arial, sans-serif; font-size:12px; font-style:normal;'>";
                        sHtmlString += "<div style='width:80%; float:left; padding:1%; border:2px solid #011496; border-radius:5px;'>" +
                                           "<div style='float:left; width:100%; border-bottom:2px solid #011496;'>" +
                                           "<div style='float:left; width:7%;'><img src='http://njspm.itrustinfo.com/_assets/images/NJSEI%20Logo.jpg' width='50' /></div>" +
                                           "<div style='float:left; width:70%;'><h2 style='margin-top:10px;'>Project Monitoring Tool</h2></div>" +
                                           "</div>";
                        sHtmlString += "<div style='width:100%; float:left;'><br/>Dear User,<br/><br/><span style='font-weight:bold;'>" + sUserName + " has invited you for the review meeting.Below are the details.</span> <br/><br/></div>";
                        sHtmlString += "<div style='width:100%; float:left;'><table style='width:100%;'>" +
                                        "<tr><td><b>Project Name </b></td><td style='text-align:center;'><b>:</b></td><td>" + DDlProject.SelectedItem.ToString() + "</td></tr>" +
                                        "<tr><td><b>WorkPackage Name </b></td><td style='text-align:center;'><b>:</b></td><td>" + DDLWorkPackage.SelectedItem.ToString() + "</td></tr>" +
                                        "<tr><td><b>Review Type </b></td><td style='text-align:center;'><b>:</b></td><td>" + rdList.SelectedItem.ToString() + "</td></tr>" +
                                        "<tr><td><b>Review Date </b></td><td style='text-align:center;'><b>:</b></td><td>" + EmailDate + "</td></tr>" +
                                        "<tr><td><b>Review Description </b></td><td style='text-align:center;'><b>:</b></td><td>" + txtdesc.Text + "</td></tr>";
                        sHtmlString += "</table></div>";
                        sHtmlString += "<div style='width:100%; float:left;'><br/><br/>Sincerely, <br/> Project Monitoring Tool.</div></div></body></html>";
                        //
                        DataTable dtemailCred = getdata.GetEmailCredentials();
                        Guid MailUID = Guid.NewGuid();
                        getdata.StoreEmaildataToMailQueue(MailUID, new Guid(Session["UserUID"].ToString()), dtemailCred.Rows[0][0].ToString(), ToEmailID, "Review Meeting !", sHtmlString, CC, "");
                        //
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
                    }
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code : ARS-01 there is problem with this feature. please contact system admin.');</script>");
            }
            
        }

        protected void rdList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rdList.SelectedValue == "Periodic")
            {
                freq.Visible = true;
                ReviewOneType.Visible = false;
            }
            else
            {
                freq.Visible = false;
                ReviewOneType.Visible = true;
            }
        }

        protected void DDlProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataSet ds = getdata.GetWorkPackages_By_ProjectUID(new Guid(DDlProject.SelectedValue));
            if (ds.Tables[0].Rows.Count > 0)
            {

                DDLWorkPackage.DataTextField = "Name";
                DDLWorkPackage.DataValueField = "WorkPackageUID";
                DDLWorkPackage.DataSource = ds;
                DDLWorkPackage.DataBind();

            }
        }
    }
}