using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManager._content_pages.users
{
    public partial class Default : System.Web.UI.Page
    {
        DBGetData getdt = new DBGetData();
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
                    BindUsers();
                }
            }
        }
        private void BindUsers()
        {
            
            if (Session["TypeOfUser"].ToString() == "U" || Session["TypeOfUser"].ToString() =="MD" || Session["TypeOfUser"].ToString() == "VP")
            {
                GrdUsers.DataSource = getdt.getAllUsers();

                GrdUsers.DataBind();

                
                
            }
            else if (Session["TypeOfUser"].ToString() == "PA")
            {
                GrdUsers.DataSource = getdt.getUsers_by_Projects_Admin(new Guid(Session["UserUID"].ToString()));
                GrdUsers.DataBind();
            }
            else
            {
                GrdUsers.DataSource = getdt.getUsers_by_AdminUnder(new Guid(Session["UserUID"].ToString()));
                GrdUsers.DataBind();
            }
        }

        public string getUserType(string sType)
        {
            return getdt.GetUserRolesDesc_by_RoleName(sType);
        }
        protected void GrdUsers_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdUsers.PageIndex = e.NewPageIndex;
            BindUsers();
        }

        protected void GrdUsers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string UID = e.CommandArgument.ToString();
            if(e.CommandName=="delete")
            {
                int cnt = getdt.User_Delete(new Guid(UID), new Guid(Session["UserUID"].ToString()));
                if (cnt > 0)
                {
                    BindUsers();
                }
            }
        }

        protected void GrdUsers_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
           
                BindUsers();
          
        }

        protected void GrdUser_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (string.IsNullOrEmpty(TxtSearch.Value)) return;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
               if (!e.Row.Cells[0].Text.ToLower().Contains(TxtSearch.Value.ToLower()))
                {
                    e.Row.Visible = false;
                }
            }
        }

        protected void BtnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("Default.aspx");
        }
    }
}