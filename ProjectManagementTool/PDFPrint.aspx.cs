using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace ProjectManagementTool
{
    public partial class PDFPrint : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Username"] == null)
            {
                Response.Redirect("~/Login.aspx");
            }
            if (!IsPostBack)
            {
                Print();
            }
        }

        private void Print()
        {
            if (Session["Print"] != null)
            {
                string outPath = "/RegExcel/printpdf.pdf";
                iframe1.Src = WebConfigurationManager.AppSettings["SiteName"] + outPath + "#toolbar=0&navpanes=0";
                Session["Print"] = null;
               
            }
            
        }
    }
}