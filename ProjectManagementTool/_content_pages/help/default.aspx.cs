using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace ProjectManagementTool._content_pages.help
{
    public partial class _default : System.Web.UI.Page
    {
        DBGetData dbgetdata = new DBGetData();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Username"] == null)
            {
                Response.Redirect("~/Login.aspx");
            }
            else
            {
                if (!Page.IsPostBack)
                {
                    PopulateTreeView();
                    trHelp.Nodes[0].Selected = true;
                    BindActivities();
                }
            }
        }
        public void PopulateTreeView()
        {
            try
            {
                DataTable dtHelpCategories = dbgetdata.GetHelpCategories();
                foreach (DataRow row in dtHelpCategories.Rows)
                {
                    TreeNode child = new TreeNode
                    {
                        Text = row["Category"].ToString(),
                        Value = row["Id"].ToString(),
                        //Target = row["FilePath"].ToString(),
                        Target = row["HTML_Text"].ToString(),
                        ToolTip = row["Category"].ToString()
                    };
                    trHelp.Nodes.Add(child);
                }

            }
            catch (Exception ex)
            {

            }
        }
        protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
        {
            BindActivities();
        }

        public void BindActivities()
        {
            try
            {
                string filePath = trHelp.SelectedNode.Target;
                lblHelpCategory.Text = trHelp.SelectedNode.Text;
                string path = HttpContext.Current.Server.MapPath(filePath);
                string content = System.IO.File.ReadAllText(path);
                dvHelp.InnerHtml = content;
            }
            catch(Exception ex)
            {

            }
        }

        /// <summary>
        /// Read Html file
        /// </summary>
        /// <param name="htmlFileNameWithPath"></param>
        /// <returns></returns>
        public static System.Text.StringBuilder ReadHtmlFile(string htmlFileNameWithPath)
        {
            System.Text.StringBuilder htmlContent = new System.Text.StringBuilder();
            string line;
            try
            {
                using (System.IO.StreamReader htmlReader = new System.IO.StreamReader(HttpContext.Current.Server.MapPath("~/" + htmlFileNameWithPath)))
                {

                    while ((line = htmlReader.ReadLine()) != null)
                    {
                        htmlContent.Append(line);
                    }
                }
            }
            catch (Exception objError)
            {
                throw objError;
            }

            return htmlContent;
        }
    }
}