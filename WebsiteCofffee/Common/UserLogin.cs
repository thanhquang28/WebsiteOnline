using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebsiteCofffee.Common
{
    [Serializable]
    public class UserLogin
    {
        public long UserID { set; get; }
        public String UserName { set; get; }
    }
}