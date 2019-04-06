using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Build_Power_App.Controllers
{
    public class ProductController : Controller
    {
        private BuildPowerEntities db = new BuildPowerEntities();
        // GET: Products
        public ActionResult Products()
        {
            ViewBag.categories = db.ProductCategories;
            return View(db.Products.ToList());
        }

        public ActionResult AddProduct()
        {
            ViewBag.categories = db.ProductCategories;
            return PartialView();
        }

        [HttpPost]
        public ActionResult AddProduct(FormCollection form, HttpPostedFileBase file)
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

                Product yangi = new Product();
                yangi.Name = Request.Form["productname"];
                yangi.Description = Request.Form["description"];
                string str = Request.Form["category"];
                yangi.CategoryID = int.Parse(Request.Form["category"]);
                yangi.Price = decimal.Parse(Request.Form["price"]);
                yangi.SalePrice = decimal.Parse(Request.Form["sale"]);
                yangi.isSale = decimal.Parse(Request.Form["sale"]) > 0 ? 1 : 0;
                yangi.Picture = "/Content/images/" + file.FileName;
                yangi.language = Request.Form["language"];
                yangi.ArrivedTime = DateTime.Now;
                yangi.code = db.ProductCategories.FirstOrDefault(x => x.ID == yangi.CategoryID).Code;

                db.Products.Add(yangi);
                db.SaveChanges();

                return RedirectToAction("Products");

            }
        }

        public ActionResult EditProduct(int id)
        {
            ViewBag.model = db.Products.Find(id);
            ViewBag.categories = db.ProductCategories.ToList();

            return PartialView();
        }

        [HttpPost]
        public ActionResult EditProduct(int id, FormCollection form, HttpPostedFileBase file)
        {
            Product yangi = db.Products.Find(id);
           
            yangi.Name = Request.Form["productname"];
            yangi.Description = Request.Form["description"];
            string str = Request.Form["category"];
            yangi.CategoryID = int.Parse(Request.Form["category"]);
            yangi.Price = decimal.Parse(Request.Form["price"]);
            yangi.SalePrice = decimal.Parse(Request.Form["sale"]);
            yangi.isSale = decimal.Parse(Request.Form["sale"]) > 0 ? 1 : 0;
            yangi.language = Request.Form["language"];
            yangi.ArrivedTime = DateTime.Now;
            yangi.code = db.ProductCategories.FirstOrDefault(x => x.ID == yangi.CategoryID).Code;

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

            return RedirectToAction("Products");
        }

        public ActionResult DetailsProduct(int id)
        {
            ViewBag.model = db.Products.Find(id);
            if (ViewBag.model != null)
                return PartialView(ViewBag.model);
            return HttpNotFound();
        }

        public ActionResult DeleteProduct(int id)
        {
            ViewBag.model = db.Products.Find(id);
            if (ViewBag.model != null)
                return PartialView(ViewBag.model);
            return HttpNotFound();

        }
        [HttpPost]
        public ActionResult DeleteProduct(int id, FormCollection collection)
        {
            try
            {
                db.Products.Remove(db.Products.Find(id));
                db.SaveChanges();

                return RedirectToAction("Products");
            }
            catch
            {
                return View();
            }

        }
    }
}