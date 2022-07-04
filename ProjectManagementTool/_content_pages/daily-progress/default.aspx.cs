using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagementTool._content_pages.daily_progress
{
    public partial class _default : System.Web.UI.Page
    {
        DBGetData getdt = new DBGetData();
        TaskUpdate gettk = new TaskUpdate();
        float AwardedCost = 0;
        float ExpenditureCost = 0;
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
                    string projectUID = string.Empty, workPackageUID = string.Empty, dprUID = string.Empty;
                    if (Session["ProjectUID"] != null)
                        projectUID = Session["ProjectUID"].ToString();
                    if (Session["WorkPackageUID"] != null)
                        workPackageUID = Session["WorkPackageUID"].ToString();
                    if (Session["DPR_UID"] != null)
                        dprUID = Session["DPR_UID"].ToString();

                    BindProject();
                    DDlProject_SelectedIndexChanged(sender, e);


                    BindDailyReportMasterMaster();
                    if (Session["SelectedMeeting"] != null)
                    {
                        DDLDailyReportMaster.SelectedValue = Session["SelectedMeeting"].ToString();
                        Session["SelectedMeeting"] = null;
                    }
                    
                    if(!string.IsNullOrEmpty(projectUID))
                    {
                        DDlProject.SelectedValue = projectUID;
                        DDlProject_SelectedIndexChanged(sender, e);
                    }
                    if (!string.IsNullOrEmpty(workPackageUID))
                    {
                        DDLWorkPackage.SelectedValue = workPackageUID;
                    }
                    if (!string.IsNullOrEmpty(dprUID))
                    {
                        DDLDailyReportMaster.SelectedValue = dprUID;
                    }
                    DDLDailyReportMaster_SelectedIndexChanged(sender, e);

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
            DDlProject.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", ""));
            DDLWorkPackage.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", ""));

        }

        protected void DDlProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDlProject.SelectedValue != "")
            {
                //DataSet ds = getdt.GetWorkPackages_By_ProjectUID(new Guid(DDlProject.SelectedValue));

                //divTabular.Visible = false;
                DataSet ds = new DataSet();
                if (Session["TypeOfUser"].ToString() == "U" || Session["TypeOfUser"].ToString() == "MD" || Session["TypeOfUser"].ToString() == "VP")
                {
                    ds = getdt.GetWorkPackages_By_ProjectUID(new Guid(DDlProject.SelectedValue));
                }
                else if (Session["TypeOfUser"].ToString() == "PA")
                {
                    ds = getdt.GetWorkPackages_ForUser_by_ProjectUID(new Guid(Session["UserUID"].ToString()), new Guid(DDlProject.SelectedValue));
                }
                else
                {
                    ds = getdt.GetWorkPackages_ForUser_by_ProjectUID(new Guid(Session["UserUID"].ToString()), new Guid(DDlProject.SelectedValue));
                }
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DDLWorkPackage.DataTextField = "Name";
                    DDLWorkPackage.DataValueField = "WorkPackageUID";
                    DDLWorkPackage.DataSource = ds;
                    DDLWorkPackage.DataBind();

                    addDailyProgress.HRef = "/_modal_pages/add-dailyprogress.aspx?ProjectUID=" + DDlProject.SelectedValue + "&WorkPackageUID=" + DDLWorkPackage.SelectedValue;
                }
                BindPhysicalProgress();

                SetSession();
            }
        }

        protected void GrdDailyProgress_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string uid = e.Values[0].ToString();
            int cnt = getdt.DailyProgressDelete(uid, Session["UserUID"].ToString());

            BindPhysicalProgress();
        }

        private void SetSession()
        {
            if (Session["ProjectUID"] != null)
                Session.Remove("ProjectUID");
            if (Session["WorkPackageUID"] != null)
                Session.Remove("WorkPackageUID");
            if (Session["DPR_UID"] != null)
                Session.Remove("DPR_UID");
            if (DDLDailyReportMaster.SelectedValue != "")
            {
                Session["DPR_UID"] = DDLDailyReportMaster.SelectedValue;
            }
            if (DDlProject.SelectedValue != "")
            {
                Session["ProjectUID"] = DDlProject.SelectedValue;
            }
            if (DDLWorkPackage.SelectedValue != "")
            {
                Session["WorkPackageUID"] = DDLWorkPackage.SelectedValue;
            }
        }
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                Guid projectUid = Guid.Empty, workPackageUid = Guid.Empty, DPR_UID = Guid.Empty;
                if (DDLDailyReportMaster.SelectedValue != "")
                {
                    DPR_UID = new Guid(DDLDailyReportMaster.SelectedValue);
                }
                if (DDlProject.SelectedValue != "")
                {
                    projectUid = new Guid(DDlProject.SelectedValue);
                }
                if (DDLWorkPackage.SelectedValue != "")
                {
                    workPackageUid = new Guid(DDLWorkPackage.SelectedValue);
                }

                for (int count = 0; count < GrdPhysicalProgress.Rows.Count; count++)
                {
                    string uid = GrdPhysicalProgress.Rows[count].Cells[0].Text;
                    Guid UID = Guid.NewGuid();
                    if (!string.IsNullOrEmpty(uid))
                        UID = new Guid(uid);

                    TextBox txtZoneName = (TextBox)GrdPhysicalProgress.Rows[count].FindControl("txtZoneName");
                    TextBox txtVillageName = (TextBox)GrdPhysicalProgress.Rows[count].FindControl("txtVillageName");
                    TextBox txtPipeDia = (TextBox)GrdPhysicalProgress.Rows[count].FindControl("txtPipeDia");
                    TextBox txtQuantity = (TextBox)GrdPhysicalProgress.Rows[count].FindControl("txtQuantity");
                    TextBox txtReviseQty = (TextBox)GrdPhysicalProgress.Rows[count].FindControl("txtReviseQty");
                    TextBox txtPipesReceived = (TextBox)GrdPhysicalProgress.Rows[count].FindControl("txtPipesReceived");
                    TextBox txtPreviousQty = (TextBox)GrdPhysicalProgress.Rows[count].FindControl("txtPreviousQty");
                    TextBox txtTodaysQty = (TextBox)GrdPhysicalProgress.Rows[count].FindControl("txtTodaysQty");
                    TextBox txtTotalQty = (TextBox)GrdPhysicalProgress.Rows[count].FindControl("txtTotalQty");
                    TextBox txtBalance = (TextBox)GrdPhysicalProgress.Rows[count].FindControl("txtBalance");
                    TextBox txtRemarks = (TextBox)GrdPhysicalProgress.Rows[count].FindControl("txtRemarks");

                    int cnt = getdt.InsertorUpdateDailyProgress(UID, DPR_UID, projectUid, workPackageUid, txtZoneName.Text, txtVillageName.Text, txtPipeDia.Text,
                         txtQuantity.Text, txtReviseQty.Text, txtPipesReceived.Text, txtPreviousQty.Text, txtTodaysQty.Text, txtTotalQty.Text, txtBalance.Text, txtRemarks.Text, DateTime.Now);

                    if(cnt == 0)
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error while saving data.');</script>");
                        return;
                    }
                    
                }
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Data saved successfully.');</script>");
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code : APPP-01 There is a problem with these feature. Please contact system admin.');</script>");
            }
        }


        private void BindPhysicalProgress()
        {
            Guid projectUid = Guid.Empty, workPackageUid = Guid.Empty, DPR_UID = Guid.Empty;
            if (DDLDailyReportMaster.SelectedValue != "")
            {
                DPR_UID = new Guid(DDLDailyReportMaster.SelectedValue);
            }
            if (DDlProject.SelectedValue != "")
            {
                projectUid = new Guid(DDlProject.SelectedValue);
            }
            if (DDLWorkPackage.SelectedValue != "")
            {
                workPackageUid = new Guid(DDLWorkPackage.SelectedValue);
            }

            DataSet ds = new DataSet();
            ds = getdt.GetDailyProgress(DPR_UID, projectUid, workPackageUid);
            if(ds.Tables[0].Rows.Count == 0 && DDLDailyReportMaster.SelectedIndex > 0)
            {
                // Now copy the data from previous report 
                Guid DPR_UID_Previous = new Guid(DDLDailyReportMaster.Items[DDLDailyReportMaster.SelectedIndex - 1].Value);
                DataSet dsPreviousReport = getdt.GetDailyProgress(DPR_UID_Previous, projectUid, workPackageUid);
                if(dsPreviousReport != null && dsPreviousReport.Tables[0] != null && dsPreviousReport.Tables[0].Rows.Count > 0)
                {
                    SavePreviousReportData(dsPreviousReport, DPR_UID, projectUid, workPackageUid);
                    ds = getdt.GetDailyProgress(DPR_UID, projectUid, workPackageUid);
                }
            }

            GrdPhysicalProgress.DataSource = ds;
            GrdPhysicalProgress.DataBind();
            if (ds.Tables[0].Rows.Count > 0)
            {
                btnexport.Visible = false;
            }
            else
            {
                btnexport.Visible = false;
            }
        }
        
        private void SavePreviousReportData(DataSet ds, Guid desinationUID, Guid projectUid, Guid workPackageUid)
        {
            if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dataRow in ds.Tables[0].Rows)
                {
                    string totalQty = dataRow["TotalQuantity"].ToString();
                    string PreviousQuantity = dataRow["PreviousQuantity"].ToString();
                    string TodaysQuantity = dataRow["TodaysQuantity"].ToString();
                    PreviousQuantity = totalQty;
                    TodaysQuantity = "0";

                    int cnt = getdt.InsertorUpdateDailyProgress(Guid.NewGuid(), desinationUID, projectUid, workPackageUid, dataRow["ZoneName"].ToString(), dataRow["VillageName"].ToString(), dataRow["PipeDia"].ToString(),
                     dataRow["Quantity"].ToString(), dataRow["RevisedQuantity"].ToString(), dataRow["PipesReceived"].ToString(), totalQty, TodaysQuantity, totalQty, dataRow["Balance"].ToString(), dataRow["Remarks"].ToString(), DateTime.Now);

                }
            }
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            //required to avoid the runtime error "  
            //Control 'GridView1' of type 'GridView' must be placed inside a form tag with runat=server."  
        }

        private void BindDailyReportMasterMaster()
        {
            DataSet ds = getdt.GetDailyProgressReport();
            DDLDailyReportMaster.DataTextField = "Description";
            DDLDailyReportMaster.DataValueField = "DPR_UID";
            DDLDailyReportMaster.DataSource = ds;
            DDLDailyReportMaster.DataBind();
        }
        protected void btncopy_Click(object sender, EventArgs e)
        {
            try
            {
                int index = DDLDailyReportMaster.SelectedIndex - 1;
                Guid sourcemeetingUID = new Guid(DDLDailyReportMaster.Items[index].Value);

                Guid projectUid = Guid.Empty, workPackageUid = Guid.Empty, desinationUID = Guid.Empty;
                if (DDLDailyReportMaster.SelectedValue != "")
                    desinationUID = new Guid(DDLDailyReportMaster.SelectedValue);
                if (DDlProject.SelectedValue != "")
                    projectUid = new Guid(DDlProject.SelectedValue);
                if (DDLWorkPackage.SelectedValue != "")
                    workPackageUid = new Guid(DDLWorkPackage.SelectedValue);

                DataSet ds = new DataSet();
                ds = getdt.GetDailyProgress(sourcemeetingUID, projectUid, workPackageUid);
                if(ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    foreach(DataRow dataRow in ds.Tables[0].Rows)
                    {
                        string totalQty = dataRow["TotalQuantity"].ToString();
                        string PreviousQuantity = dataRow["PreviousQuantity"].ToString();
                        string TodaysQuantity = dataRow["TodaysQuantity"].ToString();
                        if(cbCopy.Checked)
                        {
                            PreviousQuantity = totalQty;
                            TodaysQuantity = "0";
                        }

                        int cnt = getdt.InsertorUpdateDailyProgress(Guid.NewGuid(), desinationUID, projectUid, workPackageUid, dataRow["ZoneName"].ToString(), dataRow["VillageName"].ToString(), dataRow["PipeDia"].ToString(),
                         dataRow["Quantity"].ToString(), dataRow["RevisedQuantity"].ToString(), dataRow["PipesReceived"].ToString(), PreviousQuantity, TodaysQuantity, totalQty, dataRow["Balance"].ToString(), dataRow["Remarks"].ToString(), DateTime.Now);

                    }
                }

                DDLDailyReportMaster_SelectedIndexChanged(sender, e);
            }
            catch (Exception ex)
            {

            }
        }
        protected void btnexport_Click(object sender, EventArgs e)
        {
            string sDate1 = "";
            DateTime CDate1 = DateTime.Now;

            sDate1 = CDate1.ToString("dd/MM/yyyy");
            sDate1 = getdt.ConvertDateFormat(sDate1);
            CDate1 = Convert.ToDateTime(sDate1);

            //to allow paging=false & change style.
            GrdPhysicalProgress.HeaderStyle.ForeColor = System.Drawing.Color.Black;
            GrdPhysicalProgress.BorderColor = System.Drawing.Color.Black;
            GrdPhysicalProgress.Font.Name = "Arial";
            GrdPhysicalProgress.DataSource = getdt.GetAllProjectPhysicalProgress();
            GrdPhysicalProgress.AllowPaging = true;
            //GrdDocumentMaster.Columns[0].Visible = false; //export won't work if there's a link in the gridview
            GrdPhysicalProgress.DataBind();

            StringWriter stw = new StringWriter();
            HtmlTextWriter htextw = new HtmlTextWriter(stw);
            htextw.AddStyleAttribute("font-size", "9pt");
            htextw.AddStyleAttribute("color", "Black");

            GrdPhysicalProgress.RenderControl(htextw); //Name of the Panel

            //var sb1 = new StringBuilder();
            //GridDiv.RenderControl(new HtmlTextWriter(new StringWriter(sb1)));

            string s = htextw.InnerWriter.ToString();
            string HTMLstring = "<html><head><meta http-equiv='Content-Type' content='text/html; charset=utf-8' /></head> <body>" +
                "<div style='width:100%; margin:auto;'><div style='width:100%; float:left; line-height:25px;' align='center'>";
            HTMLstring += "<h2>" + WebConfigurationManager.AppSettings["Domain"] + "</h2>";
            HTMLstring += "<h3>Report Name: Physical Progress Achieved</h3>" +
            "</div> <div style='width:100%; float:left;'><br/><br/></div>";
            HTMLstring += "<div style='width:100%; float:left;'><h4>Physical Progress achieved upto " + CDate1.ToString("MMM yyyy") + "</h4><br/><br/></div>";
            HTMLstring += "<div style='width:100%; float:left;'>" +
            s +
            "</div>";
            HTMLstring += "</div></body></html>";


            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                {
                    StringBuilder sb = new StringBuilder();

                    //Export HTML String as PDF.
                    StringReader sr = new StringReader(HTMLstring);
                    Document pdfDoc = new Document(PageSize.A4, 25f, 25f, 35f, 25f);

                    iTextSharp.text.Font foot = new iTextSharp.text.Font();
                    foot.Size = 10;
                    HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                    pdfDoc.Footer = new HeaderFooter(new Phrase("Date : " + DateTime.Now.ToString("dd/MM/yyyy") + "                      Page: ", foot), true);
                    pdfDoc.Open();
                    htmlparser.Parse(sr);
                    pdfDoc.Close();
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "attachment;filename=Report_PhysicalProgress_" + DateTime.Now.Ticks + ".pdf");
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.Write(pdfDoc);
                    Response.End();

                }


            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (DDlProject.SelectedValue != "")
            {
                
            }
        }
        protected void DDLDailyReportMaster_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDLDailyReportMaster.SelectedIndex > 0)
            {
                btncopy.Visible = false;
                cbCopy.Visible = false;
            }
            else
            {
                btncopy.Visible = false;
                cbCopy.Visible = false;
            }
            if (DDLDailyReportMaster.SelectedValue != "")
            {
                SetSession();
                BindPhysicalProgress();
            }
        }

        protected void GrdPhysicalProgress_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Label lblAwarded = (Label)e.Row.FindControl("LblAward");
                //if (lblAwarded.Text != "")
                //{
                //    AwardedCost += float.Parse(lblAwarded.Text);
                //}
                //Label lblExpenditure = (Label)e.Row.FindControl("LblExpenditure");
                //if (lblExpenditure.Text != "")
                //{
                //    ExpenditureCost += float.Parse(lblExpenditure.Text);
                //}
            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                //e.Row.Cells[3].Text = AwardedCost.ToString("N2");
                //e.Row.Cells[5].Text = ExpenditureCost.ToString("N2");
            }
        }

        protected void TxtId_TextChanged(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            GridViewRow gvr = (GridViewRow)tb.Parent.Parent;
            int rowindex = gvr.RowIndex;

            decimal todaysQty = 0, previousQty = 0, totalQty = 0, revisedQty = 0, balanceQty = 0;
            TextBox txtPreviousQty = (TextBox)GrdPhysicalProgress.Rows[rowindex].FindControl("txtPreviousQty");
            TextBox txtTodaysQty = (TextBox)GrdPhysicalProgress.Rows[rowindex].FindControl("txtTodaysQty");
            TextBox txtTotalQty = (TextBox)GrdPhysicalProgress.Rows[rowindex].FindControl("txtTotalQty");
            TextBox txtReviseQty = (TextBox)GrdPhysicalProgress.Rows[rowindex].FindControl("txtReviseQty");
            TextBox txtBalance = (TextBox)GrdPhysicalProgress.Rows[rowindex].FindControl("txtBalance");

            if (!string.IsNullOrEmpty(txtReviseQty.Text))
                Decimal.TryParse(txtReviseQty.Text.Trim(), out revisedQty);
            if (!string.IsNullOrEmpty(txtPreviousQty.Text))
                Decimal.TryParse(txtPreviousQty.Text.Trim(), out previousQty);
            if (!string.IsNullOrEmpty(txtTodaysQty.Text))
                Decimal.TryParse(txtTodaysQty.Text.Trim(), out todaysQty);
            totalQty = todaysQty + previousQty;
            balanceQty = revisedQty - totalQty;

            txtTotalQty.Text = totalQty.ToString();
            txtBalance.Text = balanceQty.ToString();

            if(tb.ID == "txtReviseQty")
            {
                TextBox txtPipesReceived = (TextBox)GrdPhysicalProgress.Rows[rowindex].FindControl("txtPipesReceived");
                txtPipesReceived.Focus();
            }
            else if (tb.ID == "txtPreviousQty")
            {
                txtTodaysQty.Focus();
            }
            else if (tb.ID == "txtTodaysQty")
            {
                txtTotalQty.Focus();
            }
            else if (tb.ID == "txtTotalQty")
            {
                txtBalance.Focus();
            }


        }

        protected void grdResult_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }
    }
}