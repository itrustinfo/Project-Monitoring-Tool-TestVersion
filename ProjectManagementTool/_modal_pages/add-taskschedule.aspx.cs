using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagementTool._modal_pages
{
    public partial class add_taskschedule : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AddAndRemoveDynamicControls();
        }

        private void AddAndRemoveDynamicControls()
        {
            //Determine which control fired the postback event. 
            Control c = GetPostBackControl(Page);

            if ((c != null))
            {
                //If the add button was clicked, increase the count to let the page know we want to display an additional user control
                if (c.ID.ToString() == "btnAdd")
                {
                    ltlCount.Text = (Convert.ToInt16(ltlCount.Text) + 1).ToString();
                }
            }

            //Be sure everything in the placeholder control is cleared out
            ph1.Controls.Clear();

            int ControlID = 0;

            //Since these are dynamic user controls, re-add them every time the page loads.
            for (int i = 0; i <= (Convert.ToInt16(ltlCount.Text) - 1); i++)
            {
                try
                {
                    UserControl DynamicUserControl = (UserControl)LoadControl("UserControl.ascx");

                    //If this particular control id has been deleted from the page, DO NOT use it again.  If we do, it will
                    //pick up the viewstate data from the old item that had this control id, instead of generating
                    //a completely new control.  Instead, increment the control id so we're guaranteed to get a "new"
                    //control that doesn't have any lingering information in the viewstate.           
                    while (InDeletedList("uc" + ControlID) == true)
                    {
                        ControlID += 1;
                    }

                    //Note that if the item has not been deleted from the page, we DO want it to use the same control id
                    //as it used before, so it will automatically maintain the viewstate information of the user control
                    //for us.
                    DynamicUserControl.ID = "uc" + ControlID;

                    //Add an event handler to this control to raise an event when the delete button is clicked
                    //on the user control
                    DynamicUserControl.RemoveUserControl += this.HandleRemoveUserControl;

                    //Finally, add the user control to the panel
                    ph1.Controls.Add(DynamicUserControl);

                    //Increment the control id for the next round through the loop
                    ControlID += 1;
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                }
            }
        }

        private bool InDeletedList(string ControlID)
        {
            //Determine if the passed in user control id has been stored in the list of controls that were previously deleted off the page
            string[] DeletedList = ltlRemoved.Text.Split('|');
            for (int i = 0; i <= DeletedList.GetLength(0) - 1; i++)
            {
                if (ControlID.ToLower() == DeletedList[i].ToLower())
                {
                    return true;
                }
            }
            return false;
        }

        public void HandleRemoveUserControl(object sender, EventArgs e)
        {
            //This handles delete event fired from the user control
            Button remove = (sender as Button);
            //Get the user control that fired this event, and remove it
            UserControl DynamicUserControl = (UserControl)remove.Parent;
            ph1.Controls.Remove((UserControl)remove.Parent);

            //Keep a pipe delimited list of which user controls were removed.  This will increase the
            //viewstate size if the user keeps removing dynamic controls, but under normal use
            //this is such a small increase in size that it shouldn't be an issue.
            ltlRemoved.Text += DynamicUserControl.ID + "|";

            //Also, now that we've removed a user control decrement the count of total user controls on the page
            ltlCount.Text = (Convert.ToInt16(ltlCount.Text) - 1).ToString();
        }

        protected void btnAdd_Click(object sender, System.EventArgs e)
        {
            //Handled in page load
        }

        protected void btnInsert_Click(object sender, System.EventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[] { new DataColumn("StateId"), new DataColumn("State") });
            foreach (Control c in ph1.Controls)
            {
                //Find the specific user control that we added to this placeholder, and then get the selected values
                //for the dropdownlist, checkbox, and textbox and print them to the screen.
                if (c.GetType().Name.ToLower() == "usercontrol_ascx")
                {
                    UserControl uc = (UserControl)c;
                    TextBox tbState = uc.FindControl("txtState") as TextBox;
                    TextBox tbStateId = uc.FindControl("txtStateId") as TextBox;
                    if (!string.IsNullOrEmpty(tbStateId.Text.Trim()) && !string.IsNullOrEmpty(tbState.Text.Trim()))
                    {
                        dt.Rows.Add(tbStateId.Text.Trim(), tbState.Text.Trim());
                        //Insert(tbStateId.Text.Trim(), tbState.Text.Trim());
                    }
                }
            }
            //gvInsertedRecords.DataSource = dt;
            //gvInsertedRecords.DataBind();
        }

        //private void Insert(string state, string id)
        //{
        //    string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
        //    using (SqlConnection con = new SqlConnection(constr))
        //    {
        //        string query = "INSERT INTO tblStates VALUES (@StateId, @State)";
        //        using (SqlCommand cmd = new SqlCommand(query))
        //        {
        //            cmd.Connection = con;
        //            cmd.Parameters.AddWithValue("@StateId", id);
        //            cmd.Parameters.AddWithValue("@State", state);
        //            con.Open();
        //            cmd.ExecuteNonQuery();
        //            con.Close();
        //        }
        //    }
        //}

        //Find the control that caused the postback.
        public Control GetPostBackControl(Page page)
        {
            Control control = null;

            string ctrlname = page.Request.Params.Get("__EVENTTARGET");
            if ((ctrlname != null) & ctrlname != string.Empty)
            {
                control = page.FindControl(ctrlname);
            }
            else
            {
                foreach (string ctl in page.Request.Form)
                {
                    Control c = page.FindControl(ctl);
                    if (c is System.Web.UI.WebControls.Button)
                    {
                        control = c;
                        break;
                    }
                }
            }
            return control;
        }
    }
}