using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagementTool.UserControls
{
    public partial class TaskSchedule : System.Web.UI.UserControl
    {
        public event EventHandler RemoveUserControl;
        protected internal void btnRemove_Click(object sender, System.EventArgs e)
        {
            RemoveUserControl(sender, e);
        }
        //protected void Page_Load(object sender, EventArgs e)
        //{

        //}


    }
}