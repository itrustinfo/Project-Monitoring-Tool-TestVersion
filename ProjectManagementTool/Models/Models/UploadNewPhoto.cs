using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectManagementTool.Models
{
    public class UploadSitePhoto
    {
        public Guid PrjUID { get; set; }
        public Guid WorkPackage { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
    }
}