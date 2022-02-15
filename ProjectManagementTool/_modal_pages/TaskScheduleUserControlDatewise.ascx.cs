using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagementTool._modal_pages
{
    public partial class TaskScheduleUserControlDatewise : System.Web.UI.UserControl
    {
        //Declare the event that we want to raise (we'll handle this in the parent page)
        public event EventHandler RemoveUserControlDatewise;
        protected internal void btnRemove_Click(object sender, System.EventArgs e)
        {
            //Raise this event so the parent page can handle it   
            RemoveUserControlDatewise(sender, e);
        }

        //protected void Page_Load(object sender, EventArgs e)
        //{

        //}
    }
}