using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Build_Power_App.Controllers
{
    public class ProjectController : Controller
    {
        private BuildPowerEntities db = new BuildPowerEntities();
        // GET: Project
        public ActionResult Projects()
        {
            return View(db.Projects.ToList());
        }

        public ActionResult AddProject()
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult AddProject(FormCollection form, HttpPostedFileBase file)
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

                Project yangi = new Project();
                yangi.ProjectName = Request.Form["projectname"];
                yangi.Description = Request.Form["description"];
                yangi.CompanyWebsite = Request.Form["website"];
                yangi.CompanyName = Request.Form["company"];
                yangi.Location = Request.Form["location"];
                yangi.Price = Request.Form["price"];
                yangi.Picture = "/Content/images/" + file.FileName;

                db.Projects.Add(yangi);
                db.SaveChanges();

                return RedirectToAction("Projects");

            }
        }


        public ActionResult EditProject(int id)
        {
            ViewBag.model = db.Projects.Find(id);
            
            
            return PartialView();
        }
        [HttpPost]
        public ActionResult EditProject(int id, FormCollection form, HttpPostedFileBase file)
        {
            Project yangi = db.Projects.Find(id);
            yangi.ProjectName = Request.Form["projectname"];
            yangi.Description = Request.Form["description"];
            yangi.CompanyWebsite = Request.Form["website"];
            yangi.CompanyName = Request.Form["company"];
            yangi.Location = Request.Form["location"];
            yangi.Price = Request.Form["price"];

            if (file != null)
            {
                string path = Path.Combine(Server.MapPath("~/Content/images/"), Path.GetFileName(file.FileName));
                file.SaveAs(path);
                path = Path.Combine(Server.MapPath("~/Content/images/"), Path.GetFileName(file.FileName));
                file.SaveAs(path);                
                yangi.Picture = "/Content/images/" + file.FileName ;
            } 
            else
            {               
                yangi.Picture = yangi.Picture;
            }

            db.Entry(yangi).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Projects");            
        }
        public ActionResult DetailsProject(int id)
        {
            ViewBag.model = db.Projects.Find(id);
            if (ViewBag.model != null)
                return PartialView(ViewBag.model);
            return HttpNotFound();
        }

        public ActionResult DeleteProject(int id)
        {
            ViewBag.model = db.Projects.Find(id);
            if (ViewBag.model != null)
                return PartialView(ViewBag.model);
            return HttpNotFound();
          
        }
        [HttpPost]
        public ActionResult DeleteProject(int id,  FormCollection collection)
        {
            try
            {
                db.Projects.Remove(db.Projects.Find(id));
                db.SaveChanges();

                return RedirectToAction("Projects");
            }
            catch
            {
                return View();
            }
          
        }

    }
}