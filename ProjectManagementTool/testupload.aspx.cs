using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagementTool
{
    public partial class testupload : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (Request.Files.Count > 0)
            {
                HttpFileCollection attachments = Request.Files;
                for (int i = 0; i < attachments.Count; i++)
                {
                    System.Threading.Thread.Sleep(8000);
                }
            }
                //foreach (HttpPostedFile uploadedFile in FileUploadDoc.PostedFiles)
                //{
                //    System.Threading.Thread.Sleep(2000);
                //}

                //foreach (HttpPostedFile uploadedFile in FileUploadDoc.PostedFiles)
                //{
                //    System.Threading.Thread.Sleep(5000);
                //}
            }
    }
}