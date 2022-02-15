using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectManagementTool
{
    /// <summary>
    /// Summary description for FileUploadHandler
    /// </summary>
    public class FileUploadHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            HttpPostedFile file = context.Request.Files[0];
            string fname = context.Server.MapPath("Documents/" + file.FileName);
            file.SaveAs(fname);
            context.Response.ContentType = "text/plain";
            context.Response.Write("File Uploaded Successfully!");
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}