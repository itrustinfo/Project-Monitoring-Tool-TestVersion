using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectManagementTool.Models
{
    public class TaskNode
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ParentId { get; set; }
        public Boolean TaskSelected { get; set; }
    }
}