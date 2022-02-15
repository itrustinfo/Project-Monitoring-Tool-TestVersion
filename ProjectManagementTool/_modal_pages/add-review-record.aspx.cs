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
    public partial class add_review_record : System.Web.UI.Page
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
                    BindUsers();
                    if (Request.QueryString["Reviews_UID"] != null)
                    {
                        BindReviewRecords();
                    }
                }
            }
        }

        private void BindReviewRecords()
        {
            GrdReviewRecords.DataSource = getdata.getReviewRecords_by_ReviewUID(new Guid(Request.QueryString["Reviews_UID"]));
            GrdReviewRecords.DataBind();
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
                ds = getdata.GetUsers_under_ProjectUID(new Guid(Request.QueryString["PrjID"]));
            }
            else
            {
                ds = getdata.GetUsers_under_ProjectUID(new Guid(Request.QueryString["PrjID"]));
            }

            lstUsers.DataTextField = "UserName";
            lstUsers.DataValueField = "UserUID";
            lstUsers.DataSource = ds;
            lstUsers.DataBind();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                Guid RecordID = new Guid();
                if (Hidden1.Value != "")
                {
                    RecordID = new Guid(Hidden1.Value);
                }
                else
                {
                    RecordID = Guid.NewGuid();
                }

                int cnt = getdata.InsertorUpdateReviewRecords(RecordID, new Guid(Request.QueryString["Reviews_UID"]), Convert.ToDateTime(Request.QueryString["Review_Date"]), txtrecorddesc.Text, txtxsummary.Text);
                if (cnt > 0)
                {
                    foreach (ListItem item in lstUsers.Items)
                    {
                        if (item.Selected)
                        {
                            int cnt1 = getdata.InsertReviewRecord_Attendies(Guid.NewGuid(), RecordID, new Guid(item.Value));
                        }
                    }
                    txtrecorddesc.Text = "";
                    txtxsummary.Text = "";
                    BindReviewRecords();
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code : ARRec-01 there is problem with this feature. please contact system admin.');</script>");
            }
        }

        protected void GrdReviewRecords_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdReviewRecords.PageIndex = e.NewPageIndex;
            BindReviewRecords();
        }

        protected void GrdReviewRecords_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string ID = e.CommandArgument.ToString();
            if (e.CommandName == "edit")
            {
                DataSet ds = getdata.getReviewRecords_by_Review_RecordUID(new Guid(ID));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    Hidden1.Value = ds.Tables[0].Rows[0]["Review_RecordUID"].ToString();
                    txtrecorddesc.Text = ds.Tables[0].Rows[0]["ReviewRecord_Desc"].ToString();
                    txtxsummary.Text = ds.Tables[0].Rows[0]["ReviewRecord_Summary"].ToString();

                    DataSet ds1 = getdata.getReviewRecord_by_Review_RecordUID(new Guid(ds.Tables[0].Rows[0]["Review_RecordUID"].ToString()));
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
        }

        protected void GrdReviewRecords_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }
    }
}