using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProjectManager;

namespace ProjectManagementTool
{
    public partial class test : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string type = Security.Decrypt("fimziihQpuvxM9C2iQpsxA==");
        }
    }
}