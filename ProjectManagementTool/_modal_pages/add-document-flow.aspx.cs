using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagementTool._modal_pages
{
    public partial class add_document_flow : System.Web.UI.Page
    {
        DBGetData getdata = new DBGetData();
        TaskUpdate TKUpdate = new TaskUpdate();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Username"] == null)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
            }
            else
            {
                this.UnobtrusiveValidationMode = System.Web.UI.UnobtrusiveValidationMode.None;
                if (!IsPostBack)
                {
                    BindDocumentFlows();
                    HideAllDiv();
                }
            }
        }

        private void BindDocumentFlows()
        {   
            DataSet ds = getdata.GetDocumentFlows();
            DDLDocumentFlow.DataTextField = "Flow_Name";
            DDLDocumentFlow.DataValueField = "FlowMasterUID";
            DDLDocumentFlow.DataSource = ds;
            DDLDocumentFlow.DataBind();
            DDLDocumentFlow.Items.Insert(0, new ListItem("--Select--", ""));
        }

        protected void DDLDocumentFlow_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(DDLDocumentFlow.SelectedValue))
                {
                    BindDocumentFlows(new Guid(DDLDocumentFlow.SelectedValue));
                }
                else
                {
                    HideAllDiv();
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error : " + ex.Message + "');</script>");
            }
        }
        private void BindDocumentFlows(Guid FlowUID)
        {
            HideAllDiv();
            DataSet ds = getdata.GetDocumentFlows_by_UID(FlowUID);
            if(ds != null && ds.Tables[0].Rows.Count == 1)
            {
                int stepsCount = 0;
                int.TryParse(ds.Tables[0].Rows[0]["Steps_count"].ToString(), out stepsCount);
                txtFlowStep1.Text = ds.Tables[0].Rows[0]["FlowStep1_DisplayName"].ToString();
                txtFlowStep2.Text = ds.Tables[0].Rows[0]["FlowStep2_DisplayName"].ToString();
                txtFlowStep3.Text = ds.Tables[0].Rows[0]["FlowStep3_DisplayName"].ToString();
                txtFlowStep4.Text = ds.Tables[0].Rows[0]["FlowStep4_DisplayName"].ToString();
                txtFlowStep5.Text = ds.Tables[0].Rows[0]["FlowStep5_DisplayName"].ToString();
                txtFlowStep6.Text = ds.Tables[0].Rows[0]["FlowStep6_DisplayName"].ToString();
                txtFlowStep7.Text = ds.Tables[0].Rows[0]["FlowStep7_DisplayName"].ToString();
                txtFlowStep8.Text = ds.Tables[0].Rows[0]["FlowStep8_DisplayName"].ToString();
                txtFlowStep9.Text = ds.Tables[0].Rows[0]["FlowStep9_DisplayName"].ToString();
                txtFlowStep10.Text = ds.Tables[0].Rows[0]["FlowStep10_DisplayName"].ToString();
                txtFlowStep11.Text = ds.Tables[0].Rows[0]["FlowStep11_DisplayName"].ToString();
                txtFlowStep12.Text = ds.Tables[0].Rows[0]["FlowStep12_DisplayName"].ToString();
                txtFlowStep13.Text = ds.Tables[0].Rows[0]["FlowStep13_DisplayName"].ToString();
                txtFlowStep14.Text = ds.Tables[0].Rows[0]["FlowStep14_DisplayName"].ToString();
                txtFlowStep15.Text = ds.Tables[0].Rows[0]["FlowStep15_DisplayName"].ToString();
                txtFlowStep16.Text = ds.Tables[0].Rows[0]["FlowStep16_DisplayName"].ToString();
                txtFlowStep17.Text = ds.Tables[0].Rows[0]["FlowStep17_DisplayName"].ToString();
                txtFlowStep18.Text = ds.Tables[0].Rows[0]["FlowStep18_DisplayName"].ToString();
                txtFlowStep19.Text = ds.Tables[0].Rows[0]["FlowStep19_DisplayName"].ToString();
                txtFlowStep20.Text = ds.Tables[0].Rows[0]["FlowStep20_DisplayName"].ToString();

                
                int currentNumber = GetNumber(ds.Tables[0].Rows[0]["FlowStep1_Duration"].ToString());
                txtFlowDuration1.Text = currentNumber.ToString();

                currentNumber = SetNumber(ds.Tables[0].Rows[0]["FlowStep2_Duration"].ToString(), currentNumber, 2);
                currentNumber = SetNumber(ds.Tables[0].Rows[0]["FlowStep3_Duration"].ToString(), currentNumber, 3);
                currentNumber = SetNumber(ds.Tables[0].Rows[0]["FlowStep4_Duration"].ToString(), currentNumber, 4);
                currentNumber = SetNumber(ds.Tables[0].Rows[0]["FlowStep5_Duration"].ToString(), currentNumber, 5);
                currentNumber = SetNumber(ds.Tables[0].Rows[0]["FlowStep6_Duration"].ToString(), currentNumber, 6);
                currentNumber = SetNumber(ds.Tables[0].Rows[0]["FlowStep7_Duration"].ToString(), currentNumber, 7);
                currentNumber = SetNumber(ds.Tables[0].Rows[0]["FlowStep8_Duration"].ToString(), currentNumber, 8);
                currentNumber = SetNumber(ds.Tables[0].Rows[0]["FlowStep9_Duration"].ToString(), currentNumber, 9);
                currentNumber = SetNumber(ds.Tables[0].Rows[0]["FlowStep10_Duration"].ToString(), currentNumber, 10);
                currentNumber = SetNumber(ds.Tables[0].Rows[0]["FlowStep11_Duration"].ToString(), currentNumber, 11);
                currentNumber = SetNumber(ds.Tables[0].Rows[0]["FlowStep12_Duration"].ToString(), currentNumber, 12);
                currentNumber = SetNumber(ds.Tables[0].Rows[0]["FlowStep13_Duration"].ToString(), currentNumber, 13);
                currentNumber = SetNumber(ds.Tables[0].Rows[0]["FlowStep14_Duration"].ToString(), currentNumber, 14);
                currentNumber = SetNumber(ds.Tables[0].Rows[0]["FlowStep15_Duration"].ToString(), currentNumber, 15);
                currentNumber = SetNumber(ds.Tables[0].Rows[0]["FlowStep16_Duration"].ToString(), currentNumber, 16);
                currentNumber = SetNumber(ds.Tables[0].Rows[0]["FlowStep17_Duration"].ToString(), currentNumber, 17);
                currentNumber = SetNumber(ds.Tables[0].Rows[0]["FlowStep18_Duration"].ToString(), currentNumber, 18);
                currentNumber = SetNumber(ds.Tables[0].Rows[0]["FlowStep19_Duration"].ToString(), currentNumber, 19);
                currentNumber = SetNumber(ds.Tables[0].Rows[0]["FlowStep20_Duration"].ToString(), currentNumber, 20);


                ShowDiv(stepsCount);
            }
        }

        private int SetNumber(string currentValue, int LastNumber, int step)
        {
            int currentNumber = 0;
            if (!string.IsNullOrEmpty(currentValue))
                int.TryParse(currentValue, out currentNumber);
            if (currentNumber >= LastNumber)
            {
                if (step == 1)
                    txtFlowDuration1.Text = (currentNumber - LastNumber).ToString();
                else if (step == 2)
                    txtFlowDuration2.Text = (currentNumber - LastNumber).ToString();
                else if (step == 3)
                    txtFlowDuration3.Text = (currentNumber - LastNumber).ToString();
                else if (step == 4)
                    txtFlowDuration4.Text = (currentNumber - LastNumber).ToString();
                else if (step == 5)
                    txtFlowDuration5.Text = (currentNumber - LastNumber).ToString();
                else if (step == 6)
                    txtFlowDuration6.Text = (currentNumber - LastNumber).ToString();
                else if (step == 7)
                    txtFlowDuration7.Text = (currentNumber - LastNumber).ToString();
                else if (step == 8)
                    txtFlowDuration8.Text = (currentNumber - LastNumber).ToString();
                else if (step == 9)
                    txtFlowDuration9.Text = (currentNumber - LastNumber).ToString();
                else if (step == 10)
                    txtFlowDuration10.Text = (currentNumber - LastNumber).ToString();
                else if (step == 11)
                    txtFlowDuration11.Text = (currentNumber - LastNumber).ToString();
                else if (step == 12)
                    txtFlowDuration12.Text = (currentNumber - LastNumber).ToString();
                else if (step == 13)
                    txtFlowDuration13.Text = (currentNumber - LastNumber).ToString();
                else if (step == 14)
                    txtFlowDuration14.Text = (currentNumber - LastNumber).ToString();
                else if (step == 15)
                    txtFlowDuration15.Text = (currentNumber - LastNumber).ToString();
                else if (step == 16)
                    txtFlowDuration16.Text = (currentNumber - LastNumber).ToString();
                else if (step == 17)
                    txtFlowDuration17.Text = (currentNumber - LastNumber).ToString();
                else if (step == 18)
                    txtFlowDuration18.Text = (currentNumber - LastNumber).ToString();
                else if (step == 19)
                    txtFlowDuration19.Text = (currentNumber - LastNumber).ToString();
                else if (step == 20)
                    txtFlowDuration20.Text = (currentNumber - LastNumber).ToString();
            }
            else
            {
                if (step == 1)
                    txtFlowDuration1.Text = string.Empty;
                else if (step == 2)
                    txtFlowDuration2.Text = string.Empty;
                else if (step == 3)
                    txtFlowDuration3.Text = string.Empty;
                else if (step == 4)
                    txtFlowDuration4.Text = string.Empty;
                else if (step == 5)
                    txtFlowDuration5.Text = string.Empty;
                else if (step == 6)
                    txtFlowDuration6.Text = string.Empty;
                else if (step == 7)
                    txtFlowDuration7.Text = string.Empty;
                else if (step == 8)
                    txtFlowDuration8.Text = string.Empty;
                else if (step == 9)
                    txtFlowDuration9.Text = string.Empty;
                else if (step == 10)
                    txtFlowDuration10.Text = string.Empty;
                else if (step == 11)
                    txtFlowDuration11.Text = string.Empty;
                else if (step == 12)
                    txtFlowDuration12.Text = string.Empty;
                else if (step == 13)
                    txtFlowDuration13.Text = string.Empty;
                else if (step == 14)
                    txtFlowDuration14.Text = string.Empty;
                else if (step == 15)
                    txtFlowDuration15.Text = string.Empty;
                else if (step == 16)
                    txtFlowDuration16.Text = string.Empty;
                else if (step == 17)
                    txtFlowDuration17.Text = string.Empty;
                else if (step == 18)
                    txtFlowDuration18.Text = string.Empty;
                else if (step == 19)
                    txtFlowDuration19.Text = string.Empty;
                else if (step == 20)
                    txtFlowDuration20.Text = string.Empty;

            }
            return currentNumber;
        }
        private int GetNumber(string inputNumber)
        {
            int number = 0;
            int.TryParse(inputNumber, out number);
            return number;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                int step1 = 0, step2 = 0, step3 = 0, step4 = 0, step5 = 0, step6 = 0, step7 = 0, step8 = 0, step9 = 0, step10 = 0;
                int step11 = 0, step12 = 0, step13 = 0, step14 = 0, step15 = 0, step16 = 0, step17 = 0, step18 = 0, step19 = 0, step20 = 0;

                int totalStepsCount = 0;
                DataSet ds = getdata.GetDocumentFlows_by_UID(new Guid(DDLDocumentFlow.SelectedValue));
                if (ds != null && ds.Tables[0].Rows.Count == 1)
                {
                    int.TryParse(ds.Tables[0].Rows[0]["Steps_count"].ToString(), out totalStepsCount);
                }
                else
                {
                    Response.Write("<script>alert('Select a document flow');</script>");
                    return;
                }
                if(1 <= totalStepsCount)
                    step1 = SetFlowDuration(0, txtFlowDuration1.Text);
                if (2 <= totalStepsCount)
                    step2 = SetFlowDuration(step1, txtFlowDuration2.Text);
                if (3 <= totalStepsCount)
                    step3 = SetFlowDuration(step2, txtFlowDuration3.Text);
                if (4 <= totalStepsCount)
                    step4 = SetFlowDuration(step3, txtFlowDuration4.Text);
                if (5 <= totalStepsCount)
                    step5 = SetFlowDuration(step4, txtFlowDuration5.Text);
                if (6 <= totalStepsCount)
                    step6 = SetFlowDuration(step5, txtFlowDuration6.Text);
                if (7 <= totalStepsCount)
                    step7 = SetFlowDuration(step6, txtFlowDuration7.Text);
                if (8 <= totalStepsCount)
                    step8 = SetFlowDuration(step7, txtFlowDuration8.Text);
                if (9 <= totalStepsCount)
                    step9 = SetFlowDuration(step8, txtFlowDuration9.Text);
                if (10 <= totalStepsCount)
                    step10 = SetFlowDuration(step9, txtFlowDuration10.Text);
                if (11 <= totalStepsCount)
                    step11 = SetFlowDuration(step10, txtFlowDuration11.Text);
                if (12 <= totalStepsCount)
                    step12 = SetFlowDuration(step11, txtFlowDuration12.Text);
                if (13 <= totalStepsCount)
                    step13 = SetFlowDuration(step12, txtFlowDuration13.Text);
                if (14 <= totalStepsCount)
                    step14 = SetFlowDuration(step13, txtFlowDuration14.Text);
                if (15 <= totalStepsCount)
                    step15 = SetFlowDuration(step14, txtFlowDuration15.Text);
                if (16 <= totalStepsCount)
                    step16 = SetFlowDuration(step15, txtFlowDuration16.Text);
                if (17 <= totalStepsCount)
                    step17 = SetFlowDuration(step16, txtFlowDuration17.Text);
                if (18 <= totalStepsCount)
                    step18 = SetFlowDuration(step17, txtFlowDuration18.Text);
                if (19 <= totalStepsCount)
                    step19 = SetFlowDuration(step18, txtFlowDuration19.Text);
                if (20 <= totalStepsCount)
                    step20 = SetFlowDuration(step19, txtFlowDuration20.Text);

                // now save the data
                int result = getdata.InsertorUpdateDocumentFlowDisplayMaster(new Guid(DDLDocumentFlow.SelectedValue), txtFlowStep1.Text, step1,
                    txtFlowStep2.Text, step2,
                    txtFlowStep3.Text, step3,
                    txtFlowStep4.Text, step4,
                    txtFlowStep5.Text, step5,
                    txtFlowStep6.Text, step6,
                    txtFlowStep7.Text, step7,
                    txtFlowStep8.Text, step8,
                    txtFlowStep9.Text, step9,
                    txtFlowStep10.Text, step10,
                    txtFlowStep11.Text, step11,
                    txtFlowStep12.Text, step12,
                    txtFlowStep13.Text, step13,
                    txtFlowStep14.Text, step14,
                    txtFlowStep15.Text, step15,
                    txtFlowStep16.Text, step16,
                    txtFlowStep17.Text, step17,
                    txtFlowStep18.Text, step18,
                    txtFlowStep19.Text, step19,
                    txtFlowStep20.Text, step20);


                Response.Write("<script>alert('Document display flow saved successfully.');</script>");
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error : " + ex.Message + "');</script>");
            }
        }

        private int SetFlowDuration(int previousNumber, string duration)
        {
            int step = 0;
            if (string.IsNullOrEmpty(duration))
            {
                return 0;
            }
            step = Convert.ToInt32(duration);
            step = step + previousNumber;
            return step;
        }

        private void HideAllDiv()
        {
            divStep1.Visible = false;
            divStep2.Visible = false;
            divStep3.Visible = false;
            divStep4.Visible = false;
            divStep5.Visible = false;
            divStep6.Visible = false;
            divStep7.Visible = false;
            divStep8.Visible = false;
            divStep9.Visible = false;
            divStep10.Visible = false;
            divStep11.Visible = false;
            divStep12.Visible = false;
            divStep13.Visible = false;
            divStep14.Visible = false;
            divStep15.Visible = false;
            divStep16.Visible = false;
            divStep17.Visible = false;
            divStep18.Visible = false;
            divStep19.Visible = false;
            divStep20.Visible = false;
        }

        private void ShowDiv(int step)
        {
            if (1 <= step)
                divStep1.Visible = true;
            if (2 <= step)
                divStep2.Visible = true;
            if (3 <= step)
                divStep3.Visible = true;
            if (4 <= step)
                divStep4.Visible = true;
            if (5 <= step)
                divStep5.Visible = true;
            if (6 <= step)
                divStep6.Visible = true;
            if (7 <= step)
                divStep7.Visible = true;
            if (8 <= step)
                divStep8.Visible = true;
            if (9 <= step)
                divStep9.Visible = true;
            if (10 <= step)
                divStep10.Visible = true;
            if (11 <= step)
                divStep11.Visible = true;
            if (12 <= step)
                divStep12.Visible = true;
            if (13 <= step)
                divStep13.Visible = true;
            if (14 <= step)
                divStep14.Visible = true;
            if (15 <= step)
                divStep15.Visible = true;
            if (16 <= step)
                divStep16.Visible = true;
            if (17 <= step)
                divStep17.Visible = true;
            if (18 <= step)
                divStep18.Visible = true;
            if (19 <= step)
                divStep19.Visible = true;
            if (20 <= step)
                divStep20.Visible = true;
        }

    }
}