using Build_Power_App.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Build_Power_App.Controllers
{
    public class NewController : Controller
    {

        private BuildPowerEntities db = new BuildPowerEntities();

        // GET: News
        public ActionResult News()
        {
            ViewBag.categories = db.Categories.ToList();
            ViewBag.authors = db.UserRoles.ToList();
            return View(db.News);
        }

        public ActionResult EditNews(int id)
        {
            ViewBag.model = db.News.Find(id);

            ViewBag.categories = db.Categories;

            return PartialView();
        }

        [HttpPost]
        public ActionResult EditNews(int id, FormCollection form, HttpPostedFileBase file)
        {
            News yangi = db.News.Find(id);
            yangi.Title = Request.Form["Title"];
            yangi.Description = Request.Form["description"];
            yangi.DateTime = DateTime.Now;
            yangi.CategoryID = int.Parse(Request.Form["category"]);
           

            if (file != null)
            {
                string path = Path.Combine(Server.MapPath("~/Content/images/"), Path.GetFileName(file.FileName));
                file.SaveAs(path);
                path = Path.Combine(Server.MapPath("~/Content/images/"), Path.GetFileName(file.FileName));
                file.SaveAs(path);
                yangi.Picture = "/Content/images/" + file.FileName;
            }
            else
            {
                yangi.Picture = yangi.Picture;
            }

            db.Entry(yangi).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("News");
        }

        public ActionResult DetailsNews(int id)
        {
            ViewBag.model = db.News.Find(id);
            ViewBag.categories = db.Categories;
            ViewBag.roles = db.UserRoles;
            ViewBag.users = db.Users;

            if (ViewBag.model != null)
                return PartialView(ViewBag.model);
            return HttpNotFound();
        }

        public ActionResult DeleteNews(int id)
        {
            ViewBag.model = db.News.Find(id);
            ViewBag.categories = db.Categories;
            ViewBag.roles = db.UserRoles;
            ViewBag.users = db.Users;
            if (ViewBag.model != null)
                return PartialView(ViewBag.model);
            return HttpNotFound();

        }
        [HttpPost]
        public ActionResult DeleteNews(int id, FormCollection collection)
        {
            try
            {
                db.News.Remove(db.News.Find(id));
                db.SaveChanges();

                return RedirectToAction("News");
            }
            catch
            {
                return View();
            }

        }

        public ActionResult AddNews()
        {
            ViewBag.categories = db.Categories;
            return PartialView();
        }

        [HttpPost]
        public ActionResult AddNews(FormCollection form, HttpPostedFileBase file)
        {

            if (file == null)
            {
                ViewBag.mess = "Fill the Form....";
                return PartialView();
            }
            else
            {
                string path = Path.Combine(Server.MapPath("~/Content/images/"), Path.GetFileName(file.FileName));
                file.SaveAs(path);

                path = Path.Combine(Server.MapPath("~/Content/images/"), Path.GetFileName(file.FileName));
                file.SaveAs(path);

                News yangi = new News();
                yangi.Title = Request.Form["Title"];
                yangi.Description = Request.Form["description"];
                yangi.CategoryID = int.Parse(Request.Form["author"]);
                yangi.DateTime = DateTime.Now;
                yangi.AuthorID = Status.UserID;

               
                yangi.Picture = "/Content/images/" + file.FileName;

                db.News.Add(yangi);
                db.SaveChanges();

                return RedirectToAction("News");

            }
        }
    }
}