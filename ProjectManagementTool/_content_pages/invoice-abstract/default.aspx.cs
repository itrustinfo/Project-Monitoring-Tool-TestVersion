using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
namespace ProjectManagementTool._content_pages.invoice_abstract
{
    public partial class _default : System.Web.UI.Page
    {
        DBGetData dbgetdata = new DBGetData();
        TaskUpdate gettk = new TaskUpdate();
        double DeductionTotal = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Session["Username"] == null)
                {
                    Response.Redirect("~/Login.aspx");
                }
                else
                {
                    if (!IsPostBack)
                    {
                        BindTreeview();
                        //BindDataforInvoice();
                        TreeView1.Nodes[0].Selected = true;
                        BindDataforRABills();
                        AddDependency.HRef = "/_modal_pages/add-rabill-rabillitem-invoice.aspx?InvoiceId=" + TreeView1.SelectedNode.Value;
                    }
                    //if (!IsPostBack)
                    //{
                    //    BindTreeview();
                    //    BindDataforInvoice();
                    //    AddDependency.HRef = "/_modal_pages/add-rabill-rabillitem-invoice.aspx?InvoiceId=" + TreeView1.SelectedNode.Value;
                    //}
                }

            }
        }
        private void BindDataforRABills()
        {

            grdInvoiceDeatils.Visible = false;
            grdRABillItems.Visible = true;
            DataTable dt = dbgetdata.GetInvoiceDetails(TreeView1.SelectedNode.Value, TreeView1.SelectedNode.Target);
            if (TreeView1.SelectedNode.Target == "RABill")
            {
                grdInvoiceDeatils.DataSource = dt;
                grdInvoiceDeatils.DataBind();
                grdRABillItems.Visible = false;
                grdInvoiceDeatils.Visible = true;
                lblBOQName.Text = "RA Bill Name:" + TreeView1.SelectedNode.Text;
                InvoiceDetails.Visible = false;
            }
            else
            {
                InvoiceDetails.Visible = true;
                LblInvoiceGrossAmount.Text = dbgetdata.GetInvoice_Abstract_Sum(TreeView1.SelectedNode.Value).ToString();
                if (dt.Rows.Count > 0)
                {
                    DataSet ds = dbgetdata.GetInvoice_Recoveries_by_InvoiceId(new Guid(dt.Rows[0]["InvoiceUId"].ToString()));
                    GrdDeductions.DataSource = ds;
                    GrdDeductions.DataBind();
                }
                LblNetAmount.Text = "" + (Convert.ToDouble(LblInvoiceGrossAmount.Text) - DeductionTotal);
                lblBOQName.Text = "Invoice Number: " + TreeView1.SelectedNode.ToolTip;

                grdRABillItems.DataSource = dt;
                grdRABillItems.DataBind();

                grdRABillItems.Visible = true;
                grdInvoiceDeatils.Visible = false;
            }
        }
        public void PopulateTreeView(DataTable dtParent, TreeNode treeNode, string ParentUID, int Level)
        {
            foreach (DataRow row in dtParent.Rows)
            {

                TreeNode child = new TreeNode
                {

                    Text = row["Invoice_Number"].ToString() ,
                    Value = row["InvoiceUid"].ToString(),
                    Target =   "Invoice",
                    ToolTip =  row["Invoice_Number"].ToString() ,

                };



                TreeView1.Nodes.Add(child);
                DataSet dsRABills = new DataSet();

                dsRABills = dbgetdata.GetRABills(new Guid(child.Value));
               
                if (dsRABills.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row1 in dsRABills.Tables[0].Rows)
                    {
                        
                        // PopulateTreeView(dsRABills.Tables[0], child, child.Value, 1);
                        TreeNode child1 = new TreeNode
                        {

                            Text = row1["RABillNumber"].ToString(),
                            Value = row1["RABillUId"].ToString(),
                            Target = "RABill",
                            ToolTip = row1["RABillNumber"].ToString(),

                        };
                        child.ChildNodes.Add(child1);
                       // TreeView1.Nodes.Add(child1);
                    }
                }
               
            }

        }

        protected void retrieveNodes(TreeNode node, string SelectedVal)
        {
            if (node.Value == SelectedVal)
            {
                node.Selected = true;
                node.Expand();
                node.Parent.Expand();
                BindTreeview();
            }
            else
            {
                if (node.ChildNodes.Count != 0)
                {
                    foreach (TreeNode tn in node.ChildNodes)
                    {
                        if (tn.Value == SelectedVal)
                        {
                            tn.Selected = true;
                            tn.Expand();
                            if (tn.Parent != null)
                            {
                                tn.Parent.Expand();
                                if (tn.Parent.Parent != null)
                                {
                                    tn.Parent.Parent.Expand();

                                    if (tn.Parent.Parent.Parent != null)
                                    {
                                        tn.Parent.Parent.Parent.Expand();

                                        if (tn.Parent.Parent.Parent.Parent != null)
                                        {
                                            tn.Parent.Parent.Parent.Parent.Expand();

                                            if (tn.Parent.Parent.Parent.Parent.Parent != null)
                                            {
                                                tn.Parent.Parent.Parent.Parent.Parent.Expand();

                                                if (tn.Parent.Parent.Parent.Parent.Parent.Parent != null)
                                                {
                                                    tn.Parent.Parent.Parent.Parent.Parent.Parent.Expand();
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        else
                        {
                            if (tn.ChildNodes.Count != 0)
                            {
                                retrieveNodes(tn, SelectedVal);
                            }
                        }
                    }
                }
            }
        }
        public string LimitCharts(string Desc)
        {
            if (Desc.Length > 120)
            {
                return Desc.Substring(0, 120) + "  . . .";
            }
            else
            {
                return Desc;
            }
        }
        public void BindTreeview()
        {
            TreeView1.Nodes.Clear();


            
            
            DataTable ds = new DataTable();
               //ds = gettk.GetAllProjects();
                ds = dbgetdata.getInvoiceList();
           
            if (ds.Rows.Count > 0)
            {
                PopulateTreeView(ds, null, "", 0);
                if (Session["SelectedActivity"] != null)
                {
                    TreeView1.CollapseAll();
                    string UID = Session["SelectedActivity"].ToString();
                    for (int i = 0; i < TreeView1.Nodes.Count; i++)
                    {
                        retrieveNodes(TreeView1.Nodes[i], UID);

                    }
             
                }
                else
                {
                    TreeView1.Nodes[0].Selected = true;
         
                    TreeView1.CollapseAll();
                    TreeView1.Nodes[0].Expand();
                 
                }

               // BindDataforInvoice_RABills();
            }

        }

        //private void BindDataforRABills()
        //{
        //    grdInvoiceDeatils.Visible = false;
        //    grdRABillItems.Visible = true;
        //    DataTable dt = dbgetdata.GetInvoiceDetails(TreeView1.SelectedNode.Value, TreeView1.SelectedNode.Target);
        //    grdRABillItems.DataSource = dt;
        //    grdRABillItems.DataBind();
        //}
        private void BindDataforInvoice()
        {
            grdInvoiceDeatils.Visible = true;         
            grdRABillItems.Visible = false;
            DataTable dt = dbgetdata.GetInvoiceDetails(TreeView1.SelectedNode.Value);
            grdInvoiceDeatils.DataSource = dt;
            grdInvoiceDeatils.DataBind();
        }

        protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
        {
            if (TreeView1.SelectedNode.Target == "RABill")
            {
                lblBOQName.Text = "RA Bill Name:" + TreeView1.SelectedNode.Text;
                btnAddRABills.Text = "+Add RA Bills Item";
                lblheader.Text = " Add RA Bill Item";
                AddDependency.Visible = false;
                BindDataforRABills();
                
                // AddDependency.HRef = "/_modal_pages/add-rabill-rabillitem-invoice.aspx?RABillUid=" + TreeView1.SelectedNode.Value;
            }
            else
            {
                
                lblBOQName.Text = "Invoice Number: " + TreeView1.SelectedNode.ToolTip;
                btnAddRABills.Text = "+Add RA Bills ";
                lblheader.Text = " Add RA Bill";
                AddDependency.Visible = true;
               AddDependency.HRef = "/_modal_pages/add-rabill-rabillitem-invoice.aspx?InvoiceId=" + TreeView1.SelectedNode.Value;
                //BindDataforInvoice();
                BindDataforRABills();
            }
            
        }

        protected void GrdDeductions_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblTot = (Label)e.Row.FindControl("lbltotal");
                lblTot.Text = DeductionTotal.ToString();

            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[1].Text != "")
                {
                    DeductionTotal += Convert.ToDouble(e.Row.Cells[1].Text);
                }
            }
        }

        public string GetDeductionName(string UID)
        {
            return dbgetdata.GetDeductionMasterName_by_UID(new Guid(UID));
        }
    }
}