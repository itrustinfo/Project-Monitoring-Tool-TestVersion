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
    public partial class add_projectphysicalprogress : System.Web.UI.Page
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
                    BindMeetingMaster();
                    ProjectBind();
                }
            }
        }
        private void BindMeetingMaster()
        {
            DataSet ds = getdata.GetMeetingMasters();
            DDLMeetingMaster.DataTextField = "Meeting_Description";
            DDLMeetingMaster.DataValueField = "Meeting_UID";
            DDLMeetingMaster.DataSource = ds;
            DDLMeetingMaster.DataBind();
        }
        private void ProjectBind()
        {
            DataTable ds = new DataTable();
            if (Session["TypeOfUser"].ToString() == "U" || Session["TypeOfUser"].ToString() == "MD" || Session["TypeOfUser"].ToString() == "VP")
            {
                ds = TKUpdate.GetProjects();
            }
            else if (Session["TypeOfUser"].ToString() == "PA")
            {
                ds = TKUpdate.GetAssignedProjects_by_UserUID(new Guid(Session["UserUID"].ToString()));
            }
            else
            {
                ds = TKUpdate.GetAssignedProjects_by_UserUID(new Guid(Session["UserUID"].ToString()));
            }
            GrdPhysicalProgress.DataSource = ds;
            GrdPhysicalProgress.DataBind();
        }
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                for (int count = 0; count < GrdPhysicalProgress.Rows.Count; count++)
                {
                    Label ProjectName = (Label)GrdPhysicalProgress.Rows[count].FindControl("LblProjectName");
                    string ProjectUID = GrdPhysicalProgress.Rows[count].Cells[6].Text;
                    TextBox NameofthePackage = (TextBox)GrdPhysicalProgress.Rows[count].FindControl("txtnameofthepackage");
                    TextBox Targeted_PhysicalProgress = (TextBox)GrdPhysicalProgress.Rows[count].FindControl("txtTragetedPhysicalprogress");
                    TextBox Targeted_Overall_WeightedProgress = (TextBox)GrdPhysicalProgress.Rows[count].FindControl("txtTargeted_Overall_WeightedProgress");
                    TextBox Achieved_PhysicalProgress = (TextBox)GrdPhysicalProgress.Rows[count].FindControl("txtAchieved_PhysicalProgress");
                    TextBox Achieved_Overall_WeightedProgress = (TextBox)GrdPhysicalProgress.Rows[count].FindControl("txtAchieved_Overall_WeightedProgress");
                    TextBox ExpenditureasonDate = (TextBox)GrdPhysicalProgress.Rows[count].FindControl("txtExpenditureasonDate");
                    TextBox AwardedValue = (TextBox)GrdPhysicalProgress.Rows[count].FindControl("txtawarded_SanctionedValue");
                    TextBox StatusofAward = (TextBox)GrdPhysicalProgress.Rows[count].FindControl("txtStatusofAward");

                    int cnt = getdata.ProjectPhysicalProgress_Insert(Guid.NewGuid(), new Guid(ProjectUID), NameofthePackage.Text, float.Parse(Targeted_PhysicalProgress.Text),
                        float.Parse(Targeted_Overall_WeightedProgress.Text), float.Parse(Achieved_PhysicalProgress.Text), float.Parse(Achieved_Overall_WeightedProgress.Text), DateTime.Now, float.Parse(AwardedValue.Text), StatusofAward.Text, new Guid(DDLMeetingMaster.SelectedValue), float.Parse(ExpenditureasonDate.Text));
                    if (cnt > 0)
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
                    }
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code : APPP-01 There is a problem with these feature. Please contact system admin.');</script>");
            }
        }

        public string GetProjectName(string ProjectUID)
        {
            return getdata.getProjectNameby_ProjectUID(new Guid(ProjectUID));
        }
    }
}