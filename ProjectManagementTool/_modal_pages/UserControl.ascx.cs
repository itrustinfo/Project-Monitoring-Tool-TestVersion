using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagementTool
{
    public partial class UserControl : System.Web.UI.UserControl
    {
        //Declare the event that we want to raise (we'll handle this in the parent page)
        public event EventHandler RemoveUserControl;

        protected internal void btnRemove_Click(object sender, System.EventArgs e)
        {
            RemoveUserControl(sender, e);
        }
    }
}