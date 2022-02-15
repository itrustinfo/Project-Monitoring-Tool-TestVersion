using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectManager
{
    public class DBUtility
    {
        public string GetConnectionString()
        {
            return System.Configuration.ConfigurationManager.ConnectionStrings["PMConnectionString"].ToString();
        }
    }
}