using Build_Power_App.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Build_Power_App.Controllers
{
    public class AdminController : Controller
    {
        private BuildPowerEntities db = new BuildPowerEntities();

        // GET: Admin
        public ActionResult Index()
        {
            if (Control.user.RoleID != 1)
            {
                ViewBag.users = db.Users;
                ViewBag.orders = db.Orders;
                ViewBag.messages = db.Messages.Take(4);
                ViewBag.reviews = db.Reviews.OrderByDescending(x => x.DateTime).Take(4);
                ViewBag.news = db.News.OrderByDescending(x => x.DateTime).Take(4);


                ViewBag.totalorders = db.Orders.Count(x => x.ID > 0);
                ViewBag.totalusers = db.Users.Count(x => x.ID > 0);

                ViewBag.totalmessages = db.Messages.Count(x => x.ID > 0);

                ViewBag.totalreviews = db.Reviews.Count(x => x.ID > 0);

                return View();
            }
            else
            {
                return RedirectToAction("orders", "order");
            }
        
        }



    }
}