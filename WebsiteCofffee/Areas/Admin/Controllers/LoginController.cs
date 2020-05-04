using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteCofffee.Areas.Admin.Models;
using Model.DAO;
using WebsiteCofffee.Common;
using System.Web.Security;

namespace WebsiteCofffee.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        // GET: Admin/Login
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                var result = dao.Login(model.UserName, Encryptor.MD5Hash(model.PassWord));
                if (result==1)
                {
                    var user = dao.GetByID(model.UserName);
                    var userSession = new UserLogin();
                    userSession.UserName = user.Username;
                    userSession.UserID = user.ID;
                    Session.Add(CommonConstants.USER_SESSION,userSession );

                    //set timeout for session
                    FormsAuthentication.SetAuthCookie(user.Username, false);
                    Session.Timeout = 120;//unit: minutes

                    //make variable for session
                    Session["Username"] = user.Username; 
                    Session["UserId"] = user.ID;
                    Session["NameOfUser"] = user.Name;
                    Session["DirectionAva"] = user.UrlImage;

                    return RedirectToAction("Index", "Home");
                }
                else if (result == -1)
                {
                    ModelState.AddModelError("", "Account has been locked, please contact to Admin");
                }
                else if (result == -2)
                {
                    ModelState.AddModelError("", "Wrong password");
                }
                else if (result == 0)
                {
                    ModelState.AddModelError("", "Account doesn't exist");
                }
                else
                {
                    ModelState.AddModelError("", "Something went wrong...");
                }
            }
            return View("Index");
        }
    }
}