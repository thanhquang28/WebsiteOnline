using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace WebsiteCofffee.Areas.Admin.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage ="Please input Username")]
        public String UserName { set; get; }
        [Required(ErrorMessage = "Please input Password")]
        public String PassWord { set; get; }
        public bool RememberMe { set; get; }

    }
}