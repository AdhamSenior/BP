using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Build_Power_App.Controllers
{
    public class UserController : Controller
    {

        private BuildPowerEntities db = new BuildPowerEntities();
        // GET: Users
        public ActionResult Users()
        {
            ViewBag.roles = db.UserRoles;
            return View(db.Users);
        }

        public ActionResult EditUsers(int id)
        {
            ViewBag.model = db.Users.Find(id);

            ViewBag.roles = db.UserRoles;

            return PartialView();
        }

        [HttpPost]
        public ActionResult EditUsers(int id, FormCollection form)
        {
            User yangi = db.Users.Find(id);
            yangi.Firstname = Request.Form["firstname"];
            yangi.Lastname = Request.Form["lastname"];
            yangi.RoleID = int.Parse(Request.Form["role"]);
            yangi.Phonenumber = Request.Form["phonenumber"];
            yangi.Email = Request.Form["email"];
            yangi.Username = Request.Form["username"];
            yangi.Password = Request.Form["password"];


            db.Entry(yangi).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Users");
        }
        public ActionResult DetailsUsers(int id)
        {
            ViewBag.model = db.Users.Find(id);
            int roleid = ViewBag.model.RoleID;
            ViewBag.roles = db.UserRoles.FirstOrDefault(x=>x.ID == roleid).Name;

            if (ViewBag.model != null)
                return PartialView(ViewBag.model);
            return HttpNotFound();
        }

        public ActionResult DeleteUsers(int id)
        {
            ViewBag.model = db.Users.Find(id);
            int roleid = ViewBag.model.RoleID;
            ViewBag.roles = db.UserRoles.FirstOrDefault(x => x.ID == roleid).Name;

            if (ViewBag.model != null)
                return PartialView(ViewBag.model);
            return HttpNotFound();

        }
        [HttpPost]
        public ActionResult DeleteUsers(int id, FormCollection collection)
        {
            try
            {
                db.Users.Remove(db.Users.Find(id));
                db.SaveChanges();

                return RedirectToAction("Users");
            }
            catch
            {
                return View();
            }

        }


        public ActionResult AddUsers()
        {
            ViewBag.roles = db.UserRoles;
            return PartialView();
        }


        [HttpPost]
        public ActionResult AddUserss()
        {
            User yangi = new Build_Power_App.User();
            yangi.Firstname = Request.Form["firstname"];
            yangi.Lastname = Request.Form["lastname"];
            yangi.RoleID = int.Parse(Request.Form["role"]);
            yangi.Phonenumber = Request.Form["phonenumber"];
            yangi.Email = Request.Form["email"];
            yangi.Username = Request.Form["username"];
            yangi.Password = Request.Form["password"];
            yangi.DateTime = DateTime.Now;

            db.Users.Add(yangi);
            db.SaveChanges();

            return RedirectToAction("Users");
        }
    }
}