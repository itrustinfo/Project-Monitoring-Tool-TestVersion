using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagementTool._modal_pages
{
    public partial class add_sitephotographs : System.Web.UI.Page
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
                    BindProject();
                    BindMeetingMaster();
                    if (Request.QueryString["PrjUID"] != null)
                    {
                        DDlProject.SelectedValue = Request.QueryString["PrjUID"];
                    }
                    if (Request.QueryString["MeetingUID"] != null)
                    {
                        DDlMeeting.SelectedValue = Request.QueryString["MeetingUID"];
                    }
                }
               
            }
        }

        private void BindProject()
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

            DDlProject.DataTextField = "ProjectName";
            DDlProject.DataValueField = "ProjectUID";
            DDlProject.DataSource = ds;
            DDlProject.DataBind();

        }

        private void BindMeetingMaster()
        {
            DataSet ds = getdata.GetMeetingMasters();
            DDlMeeting.DataTextField = "Meeting_Description";
            DDlMeeting.DataValueField = "Meeting_UID";
            DDlMeeting.DataSource = ds;
            DDlMeeting.DataBind();
        }


        private void BindSitePhotographs()
        {
            DataSet ds = getdata.MeetingSitePhotoGraphs_Selectby_ProjectUID_Meeting_UID_Empty_Desc(new Guid(DDlProject.SelectedValue), new Guid(DDlMeeting.SelectedValue));
            GrdSitePhotograph.DataSource = ds;
            GrdSitePhotograph.DataBind();
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < GrdSitePhotograph.Items.Count; i++)
                {
                    TextBox txtdesc = (TextBox)GrdSitePhotograph.Items[i].FindControl("txtdesc");
                    Label SitePhotoGraph_UID = (Label)GrdSitePhotograph.Items[i].FindControl("LblSitePhotoGraph_UID");

                    int cnt = getdata.MeetingSitePhotoGraphs_Desc_Updte(new Guid(SitePhotoGraph_UID.Text), txtdesc.Text);
                    if (cnt <= 0)
                    {
                        
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code UpdateDesc-01 there is a problem with this feature. Please contact system admin.');</script>");
                    }

                }

                Session["SelectedMeeting"] = DDlProject.SelectedValue + "_" + DDlMeeting.SelectedValue;
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code : UploadImage-01 There is a problem with these feature. Please contact system admin.');</script>");
            }
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                string sFileDirectory = "~/SitePhotographs";

                if (!Directory.Exists(Server.MapPath(sFileDirectory)))
                {
                    Directory.CreateDirectory(Server.MapPath(sFileDirectory));

                }

                //Hidden1.Value = DDlMeeting.SelectedValue;
                int NotSupportedImageCount = 0;
                //int cntdelete = getdata.MeetingSitePhotoGraphs_Delete_by_Meeting_UID(new Guid(DDlMeeting.SelectedValue));
                //if (cntdelete > 0)
                //{
                    foreach (HttpPostedFile uploadedFile in ImageUpload.PostedFiles)
                    {
                        if (uploadedFile.ContentLength > 0 && !String.IsNullOrEmpty(uploadedFile.FileName))
                        {
                            string sFileName = DateTime.Now.Ticks +"_"+ Path.GetFileName(uploadedFile.FileName);
                            string FileExtn = Path.GetExtension(uploadedFile.FileName);
                            if (FileExtn.ToUpper() == ".JPG" || FileExtn.ToUpper() == ".JPEG" || FileExtn.ToUpper() == ".PNG" || FileExtn.ToUpper() == ".GIF" || FileExtn.ToUpper() == ".TIFF")
                            {
                                uploadedFile.SaveAs(Server.MapPath(sFileDirectory + "/" + sFileName));
                                //int Cnt = getdata.SitePhotograph_InsertorUpdate(Guid.NewGuid(), new Guid(Request.QueryString["PrjUID"]), new Guid(Request.QueryString["WorkPackage"]), (sFileDirectory + "/" + sFileName), "", DateTime.Now);
                                int Cnt = getdata.MeetingSitePhotoGraphs_InsertorUpdate(Guid.NewGuid(), new Guid(DDlProject.SelectedValue), new Guid(DDlMeeting.SelectedValue), "", (sFileDirectory + "/" + sFileName));
                                if (Cnt <= 0)
                                {
                                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code ADDSP-01 there is a problem with this feature. Please contact system admin.');</script>");
                                }
                            }
                            else
                            {
                                NotSupportedImageCount += 1;
                            }
                        }
                    }
                    if (NotSupportedImageCount > 0)
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Some of the uploded image formats are not supported. Please contact system admin.');</script>");
                    }
                    btnAdd.Visible = true;
                    //DDlMeeting.SelectedValue = Hidden1.Value;
                    BindSitePhotographs();
                //}
                
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code : ADD_SP-02 There is a problem with these feature. Please contact system admin.');</script>");
            }
        }
    }
}