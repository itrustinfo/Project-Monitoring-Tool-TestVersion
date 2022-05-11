using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagementTool._content_pages.finance_mobilisation_advance
{
    public partial class _default : System.Web.UI.Page
    {
        TaskUpdate TKUpdate = new TaskUpdate();
        DBGetData getdata = new DBGetData();
        

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadProjects();
                SelectedProjectWorkpackage("Project");
                ddlProject_SelectedIndexChanged(sender, e);

                MobilisationAdvanceBind();
            }
        }
       
        protected void ddlworkpackage_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            if (!string.IsNullOrEmpty(ddlworkpackage.SelectedValue))
            {
                Session["ProjectUID"] = ddlProject.SelectedValue;
                Session["WorkPackageUID"] = ddlworkpackage.SelectedValue;
            }
        }

        private void LoadProjects()
        {
            try
            {
                DataTable ds = new DataTable();
                if (Session["TypeOfUser"].ToString() == "U" || Session["TypeOfUser"].ToString() == "MD" || Session["TypeOfUser"].ToString() == "VP")
                {
                    ds = TKUpdate.GetProjects();
                }
                else if (Session["TypeOfUser"].ToString() == "PA")
                {
                    //ds = TKUpdate.GetProjects_by_UserUID(new Guid(Session["UserUID"].ToString()));
                    ds = TKUpdate.GetAssignedProjects_by_UserUID(new Guid(Session["UserUID"].ToString()));
                }
                else
                {
                    //ds = TKUpdate.GetProjects();
                    ds = TKUpdate.GetAssignedProjects_by_UserUID(new Guid(Session["UserUID"].ToString()));
                }
                ddlProject.DataSource = ds;
                ddlProject.DataTextField = "ProjectName";
                ddlProject.DataValueField = "ProjectUID";
                ddlProject.DataBind();
            }
            catch (Exception ex)
            {
            }
        }

        internal void SelectedProjectWorkpackage(string pType)
        {
            if (!IsPostBack && Session["Project_Workpackage"] != null)
            {
                string[] selectedValue = Session["Project_Workpackage"].ToString().Split('_');
                if (selectedValue.Length > 1)
                {
                    if (pType == "Project")
                    {
                        ddlProject.SelectedValue = selectedValue[0];
                    }
                    else
                    {
                        ddlworkpackage.SelectedValue = selectedValue[1];
                    }
                }
                else
                {
                    if (pType == "Project")
                    {
                        ddlProject.SelectedValue = Session["Project_Workpackage"].ToString();
                    }
                }
            }

        }

        protected void ddlProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlProject.SelectedValue != "")
            {

               // divStatus.Visible = false;
               // divStatusMonth.Visible = true;
                DataSet ds = new DataSet();
                if (Session["TypeOfUser"].ToString() == "U" || Session["TypeOfUser"].ToString() == "MD" || Session["TypeOfUser"].ToString() == "VP")
                {
                    ds = getdata.GetWorkPackages_By_ProjectUID(new Guid(ddlProject.SelectedValue));
                }
                else if (Session["TypeOfUser"].ToString() == "PA")
                {
                    ds = getdata.GetWorkPackages_ForUser_by_ProjectUID(new Guid(Session["UserUID"].ToString()), new Guid(ddlProject.SelectedValue));
                }
                else
                {
                    ds = getdata.GetWorkPackages_ForUser_by_ProjectUID(new Guid(Session["UserUID"].ToString()), new Guid(ddlProject.SelectedValue));
                }
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlworkpackage.DataTextField = "Name";
                    ddlworkpackage.DataValueField = "WorkPackageUID";
                    ddlworkpackage.DataSource = ds;
                    ddlworkpackage.DataBind();
                    SelectedProjectWorkpackage("Workpackage");

                   // AddData.HRef = "~/_modal_pages/add-Fin-Month-data.aspx?WorkPackageUID=" + ddlworkpackage.SelectedValue;
                    //Loadtasks(ddlworkpackage.SelectedValue);
                    //LoadSTask(ddlTask.SelectedItem.Value);

                    DataSet dschild = getdata.GetTasksForWorkPackages(ddlworkpackage.SelectedValue);
                    if (dschild.Tables[0].Rows.Count > 0)
                    {
                        //TreeView1.Nodes.Clear();
                       // PopulateTreeView(dschild, null, "", 0);
                        //  TreeView1.Nodes[0].Selected = true;
                       // TreeView1.CollapseAll();
                        //TreeView1.Nodes[0].Expand();
                        ActivityHeading.Text = "WorkPackage : " + ddlworkpackage.SelectedItem.ToString() + " (Mobilization Advance)";
                        //divStatus.Visible = true;
                        ////LoadTaskPayments(TreeView1.SelectedNode.Value);
                        ////FillAllowedPayment();
                        //FinanceMileStoneBind(TreeView1.SelectedNode.Value);
                       // FinanceMileStoneMonthBind(ddlworkpackage.SelectedValue);
                    }

                    Session["Project_Workpackage"] = ddlProject.SelectedValue + "_" + ddlworkpackage.SelectedValue;

                    Session["ProjectUID"] = ddlProject.SelectedValue;
                    Session["WorkPackageUID"] = ddlworkpackage.SelectedValue;

                    MobilisationAdvanceBind();
                }
            }
        }

       
        private void MobilisationAdvanceBind()
        {
            if (Session["ProjectUID"] is null || Session["WorkPackageUID"] is null)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "Warning", "<script language='javascript'>alert('Project ID/WorkPackageID can not be null.');</script>");
                return;
            }

            GridView1.DataSource = null;
            divStatus.Visible = true;
            DataSet ds = getdata.GetMobilisationAdvance(new Guid(Session["ProjectUID"].ToString()), new Guid(Session["WorkPackageUID"].ToString()));
            GridView1.DataSource = ds;
            GridView1.DataBind();
            
            decimal nettotal = GetNetAdvance(ds);

            if (GridView1.FooterRow != null)
            {
                GridView1.FooterRow.Cells[3].Text = "Net Balance (Pending Mobilization Advance) : " + Math.Abs(nettotal).ToString();

               // GridView1.FooterRow.Cells[4].Text = nettotal >= 0 ? "Debit" : "Credit";
            }
            
        }

        private decimal GetNetAdvance(DataSet ds)
        {
            decimal advance = 0;

            foreach(DataRow item in ds.Tables[0].Rows)
            {
                advance = advance + (item.ItemArray[4].ToString() == "Credit" ? -Convert.ToDecimal(item.ItemArray[3].ToString()) : Convert.ToDecimal(item.ItemArray[3].ToString()));
            }

            return advance;
        }

        protected void OnPaging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            MobilisationAdvanceBind();
        }

        protected void OnRowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            MobilisationAdvanceBind();
        }

        protected void OnRowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = GridView1.Rows[e.RowIndex];

            string mobilisationadvanceUID  = GridView1.DataKeys[e.RowIndex].Values[0].ToString();
            string invoice_no = (row.Cells[1].Controls[0] as TextBox).Text;
            string given_date = (row.Cells[2].Controls[0] as TextBox).Text;
            string advance_amount = (row.Cells[3].Controls[0] as TextBox).Text;
            string transaction_type = (row.Cells[4].Controls[0] as TextBox).Text;

            int result = 0;

            result = getdata.InsertMobilisationAdvance(new Guid(mobilisationadvanceUID), new Guid(Session["ProjectUID"].ToString()), new Guid(Session["WorkPackageUID"].ToString()), invoice_no , Convert.ToDateTime(given_date), Convert.ToDecimal(advance_amount), transaction_type, true);

            GridView1.EditIndex = -1;
            MobilisationAdvanceBind();
           
        }

        protected void OnRowCancelingEdit(object sender, EventArgs e)
        {
            GridView1.EditIndex = -1;
            MobilisationAdvanceBind();
        }

        protected void OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string mobilisationadvanceUID = GridView1.DataKeys[e.RowIndex].Values[0].ToString();

            int result = getdata.DeleteMobilisationAdvance(new Guid(mobilisationadvanceUID),new Guid(Session["UserUID"].ToString()));

            MobilisationAdvanceBind();
        }

        protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Right;

                if (e.Row.Cells[4].Text == "Debit")
                {
                    e.Row.Cells[5].Text  = "";
                    e.Row.Cells[6].Text  = "";
                }
                     
                string item = e.Row.Cells[0].Text;
                foreach (LinkButton button in e.Row.Cells[6].Controls.OfType<LinkButton>())
                {
                    if (button.CommandName == "Delete")
                    {
                        button.Attributes["onclick"] = "if(!confirm('Do you want to delete " + item + "?')){ return false; };";
                    }
                }
            }

          
        }

      
        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            MobilisationAdvanceBind();
        }

        protected void GridView1_RowUpdated(object sender, GridViewUpdatedEventArgs e)
        {
            MobilisationAdvanceBind();
        }
    }

}