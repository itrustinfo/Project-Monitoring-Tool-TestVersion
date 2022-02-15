using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagementTool._content_pages.choose_sitephotographs
{
    public partial class _default : System.Web.UI.Page
    {
        DBGetData getdt = new DBGetData();
        TaskUpdate gettk = new TaskUpdate();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Username"] == null)
            {
                Response.Redirect("~/Login.aspx");
            }
            else
            {
                if (!IsPostBack)
                {
                    BindProject();
                    BindMeetingMaster();
                    SitePhotGraphs.Visible = false;
                    
                }
                else
                {
                    SitePhotGraphs.Visible = true;
                }
                if (Session["SelectedMeeting"] != null)
                {
                    string[] SelectedVal = Session["SelectedMeeting"].ToString().Split('_');
                    DDlProject.SelectedValue = SelectedVal[0];
                    ddlMeeting.SelectedValue = SelectedVal[1];
                    Session["SelectedMeeting"] = null;
                    btnView_Click(sender, e);
                }
            }
        }

        private void BindProject()
        {
            DataTable ds = new DataTable();
            if (Session["TypeOfUser"].ToString() == "U" || Session["TypeOfUser"].ToString() == "MD" || Session["TypeOfUser"].ToString() == "VP")
            {
                ds = gettk.GetProjects();
            }
            else if (Session["TypeOfUser"].ToString() == "PA")
            {
                ds = gettk.GetAssignedProjects_by_UserUID(new Guid(Session["UserUID"].ToString()));
            }
            else
            {
                ds = gettk.GetAssignedProjects_by_UserUID(new Guid(Session["UserUID"].ToString()));
            }

            DDlProject.DataTextField = "ProjectName";
            DDlProject.DataValueField = "ProjectUID";
            DDlProject.DataSource = ds;
            DDlProject.DataBind();

        }

        //protected void ddlMeeting_SelectedIndexChanged(object sender, EventArgs e)
        //{
            
                
        //}

        //protected void DDlProject_SelectedIndexChanged(object sender, EventArgs e)
        //{

        //    if (DDlProject.SelectedValue != "")
        //    {
        //        BindSitePhotographs();
        //    }
        //}
        private void BindSitePhotographs()
        {
            DataSet ds = getdt.MeetingSitePhotoGraphs_Selectby_ProjectUID_Meeting_UID(new Guid(DDlProject.SelectedValue), new Guid(ddlMeeting.SelectedValue));
            if (ds.Tables[0].Rows.Count > 0)
            {
                //btnSubmit.Visible = true;
                LblMessage.Visible = false;
                GrdSitePhotograph.DataSource = ds;
                GrdSitePhotograph.DataBind();
            }
            else
            {
                GrdSitePhotograph.DataSource = null;
                GrdSitePhotograph.DataBind();
                LblMessage.Visible = true;
                //btnSubmit.Visible = false;
            }
        }
        private void BindMeetingMaster()
        {
            DataSet ds = getdt.GetMeetingMasters();
            ddlMeeting.DataTextField = "Meeting_Description";
            ddlMeeting.DataValueField = "Meeting_UID";
            ddlMeeting.DataSource = ds;
            ddlMeeting.DataBind();
        }

        //protected void btnSubmit_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        int Error = 0;
        //        int cntdelete = getdt.MeetingSitePhotoGraphs_Delete_by_ProjectUID(new Guid(DDlProject.SelectedValue));
        //        if (cntdelete > 0)
        //        {
        //            for (int i = 0; i < GrdSitePhotograph.Items.Count; i++)
        //            {
        //                DataListItem item = GrdSitePhotograph.Items[i];

        //                bool isChecked = ((CheckBox)item.FindControl("ChkPhoto")).Checked;
        //                if (isChecked)
        //                {
        //                    string Description = ((Label)item.FindControl("LblDescription")).Text;
        //                    string imgURL = ((Image)item.FindControl("imgEmp")).ImageUrl;
        //                    int cnt = getdt.MeetingSitePhotoGraphs_InsertorUpdate(Guid.NewGuid(), new Guid(DDlProject.SelectedValue), new Guid(ddlMeeting.SelectedValue), Description, imgURL);
        //                    if (cnt <= 0)
        //                    {
        //                        Error += 1;
        //                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code : MSP-01 There is a problem with this feature. Please contact system admin.');</script>");
        //                    }
        //                }
        //            }
        //            if (Error == 0)
        //            {
        //                DDlProject_SelectedIndexChanged(sender, e);
        //                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Site Photographs updated to meeting " + ddlMeeting.SelectedItem.Text + "');</script>");
        //            }
        //        }
                
        //    }
        //    catch (Exception ex)
        //    {
        //        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code : MSP-01 There is a problem with this feature. Please contact system admin.');</script>");
        //    }
            
        //}

        protected void GrdSitePhotograph_DeleteCommand(object source, DataListCommandEventArgs e)
        {
            string UID = GrdSitePhotograph.DataKeys[e.Item.ItemIndex].ToString();
            int cnt = getdt.MeetingSitePhotoGraphs_Delete(new Guid(UID));
            if (cnt > 0)
            {
                BindSitePhotographs();
            }
            
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            if (DDlProject.SelectedValue != "" && ddlMeeting.SelectedValue != "")
            {
                addPhoto.HRef = "/_modal_pages/add-sitephotographs.aspx?PrjUID=" + DDlProject.SelectedValue + "&MeetingUID=" + ddlMeeting.SelectedValue;
                BindSitePhotographs();
                SitePhotGraphs.Visible = true;
            }
        }
    }
}