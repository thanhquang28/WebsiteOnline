using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Model.DAO;
using Model.EF;
using WebsiteCofffee.Areas.Admin.Models;
using WebsiteCofffee.Common;

namespace WebsiteCofffee.Areas.Admin.Controllers
{
    public class UsersController : BaseController
    {
        private OnlineShopDbContext db = new OnlineShopDbContext();

        //get info of session
       

        // GET: Admin/Users
        public ActionResult Index()
        {
            return View(db.Users.OrderByDescending(x=>x.ID).ToList());
        }

        // GET: Admin/Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            Session["DirImgUser"] = user.UrlImage;
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Admin/Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Username,Password,Name,Address,Email,Phone,CreateDate,CreatedBy,ModifiedDate,ModifiedBy,Status")] User user, HttpPostedFileBase image)
        {
            //check if username has been created
            var dao = new UserDao();
            var userindatabase = dao.GetUserExisted(user.Username);
            if (userindatabase == 1 )
            {
                ModelState.AddModelError("", "Username has been created");
            }
            else
            {
                if (image!=null && image.ContentLength>0)
                {
                    user.Image = new byte[image.ContentLength]; //store in binary formate
                    image.InputStream.Read(user.Image, 0, image.ContentLength);
                    string fileName = System.IO.Path.GetFileName(image.FileName);
                    string urlImage = Server.MapPath("~/Images/" + fileName);
                    image.SaveAs(urlImage);

                    user.UrlImage = "Images/" + fileName;
                }
                if (ModelState.IsValid)
                {
                    user.CreateDate = DateTime.Now;
                    user.CreatedBy = (string)System.Web.HttpContext.Current.Session["Username"];
                    user.Status = true;
                    var encryptedMd5Pass = Encryptor.MD5Hash(user.Password);
                    user.Password = encryptedMd5Pass;
                    db.Users.Add(user);
                    db.SaveChanges();
                    SetAlert("User was successfully added!", "Success");
                    return RedirectToAction("Index");
                }
            }

            return View(user);
        }

        // GET: Admin/Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Admin/Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Username,Password,Name,Address,Email,Phone,CreateDate,CreatedBy,ModifiedDate,ModifiedBy,Status")] User user, HttpPostedFileBase editImage)
        {
            if (ModelState.IsValid)
            {
                user.ModifiedDate = DateTime.Now;
                user.ModifiedBy = (string)System.Web.HttpContext.Current.Session["Username"];

                //test if has value of image input
                if (editImage != null && editImage.ContentLength > 0)
                {
                    user.Image = new byte[editImage.ContentLength];
                    editImage.InputStream.Read(user.Image, 0, editImage.ContentLength);
                    string fileName = System.IO.Path.GetFileName(editImage.FileName);
                    string urlImage = Server.MapPath("~/Images/" + fileName);
                    editImage.SaveAs(urlImage);

                    user.UrlImage = "Images/" + fileName;
                    db.Users.AddOrUpdate(user);
                    db.SaveChanges();
                }
                else
                {
                    //edit each column by id. Except Image and UrlImage to void error of Entity Framework
                    db.Database.ExecuteSqlCommand("update [dbo].[User] set  " +
                        "Name = '"+user.Name+"', " +
                        "Address='"+user.Address+"'," +
                        "Email='"+user.Email+"'," +
                        "Phone='"+user.Phone+"'," +
                        "ModifiedDate='"+user.ModifiedDate+"'," +
                        "ModifiedBy='"+user.ModifiedBy+"'," +
                        "Status='"+user.Status+"' " +
                        "where ID='"+user.ID+"';");
                }

                SetAlert("User was successfully edited!", "Success");
                return RedirectToAction("Details", new { id = user.ID });
            }
            return View(user);
        }

        // GET: Admin/Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            
            if (user == null)
            {
                return HttpNotFound();
            }
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // POST: Admin/Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}
