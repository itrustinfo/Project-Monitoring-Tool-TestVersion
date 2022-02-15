using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProjectManager.DAL;

namespace ProjectManagementTool._modal_pages
{
    public partial class add_generaldocumentstructure : System.Web.UI.Page
    {
        GeneralDocuments GD = new GeneralDocuments();
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
                    if (Request.QueryString["StructureUID"] != null)
                    {
                        BindGeneralDocumentStructure(Request.QueryString["StructureUID"]);
                    }
                }
            }
        }

        private void BindGeneralDocumentStructure(string StructureUID)
        {
            DataSet ds = GD.GetGeneralDocumentStructure_By_StructureUID(new Guid(StructureUID));
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtstructurename.Text = ds.Tables[0].Rows[0]["Structure_Name"].ToString();
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                Guid StructureUID;
                if (Request.QueryString["StructureUID"] == null)
                {
                    StructureUID = Guid.NewGuid();
                }
                else
                {
                    StructureUID = new Guid(Request.QueryString["StructureUID"]);
                }

                int cnt = GD.GeneralDocumentStructure_InsertorUpdate(StructureUID, txtstructurename.Text, new Guid(Request.QueryString["ParentUID"]), new Guid(Session["UserUID"].ToString()));
                if (cnt > 0)
                {
                    Session["GeneralDocumentStructureUID"] = StructureUID;
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
                }
                else if (cnt == -1)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('" + txtstructurename.Text + " already exists. Try with different name.');</script>");
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('There is a problem with this feature. Please contact system admin.');</script>");
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('There is a problem with this feature. Please contact system admin. Description : " + ex.Message + "');</script>");
            }
        }
    }
}