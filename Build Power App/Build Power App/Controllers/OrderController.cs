using Build_Power_App.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Build_Power_App.Controllers
{
    public class OrderController : Controller
    {
        private BuildPowerEntities db = new BuildPowerEntities();
        // GET: Order
        public ActionResult Orders()
        {
            if (Build_Power_App.App_Start.Control.user.RoleID == 1)
            {
                ViewBag.products = db.Products;
                ViewBag.users = db.Users;
                return View(db.Orders.Where(x=>x.UserID == Control.user.ID).ToList());
            }
            else
                 {
                ViewBag.products = db.Products;
                ViewBag.users = db.Users;
                return View(db.Orders.ToList());
            }
        }

        public ActionResult DeleteOrder(int id)
        {
            ViewBag.order = db.Orders.Find(id);
            string clientname = db.Users.FirstOrDefault(x => x.ID == (db.Orders.FirstOrDefault(y => y.ID == id).UserID)).Firstname;
            string phonenumber = db.Users.FirstOrDefault(x => x.ID == (db.Orders.FirstOrDefault(y => y.ID == id).UserID)).Phonenumber;
            Product pr = db.Products.FirstOrDefault(x => x.ID == (db.Orders.FirstOrDefault(y => y.ID == id).ProductID));

            ViewBag.clientname = clientname;
            ViewBag.phonenumber = phonenumber;
            ViewBag.product = pr;

            return View();
        }

        [HttpPost]
        public ActionResult DeleteOrderr(int id)
        {
            string on = db.Orders.FirstOrDefault(x => x.ID == id).OrderNumber;
            List<Order> result = new List<Order>();
            foreach (var item in db.Orders)
            {
                if (item.OrderNumber == on)
                {
                    result.Add(item);
                }
            }

            db.Orders.RemoveRange(result);

            db.SaveChanges();

            ViewBag.products = db.Products;
            ViewBag.users = db.Users;
            return View("orders", db.Orders.ToList());

        }

        public ActionResult DeliveryOrder(int id)
        {
            ViewBag.order = db.Orders.Find(id);
            string clientname = db.Users.FirstOrDefault(x => x.ID == (db.Orders.FirstOrDefault(y => y.ID == id).UserID)).Firstname;
            string phonenumber = db.Users.FirstOrDefault(x => x.ID == (db.Orders.FirstOrDefault(y => y.ID == id).UserID)).Phonenumber;
            Product pr = db.Products.FirstOrDefault(x => x.ID == (db.Orders.FirstOrDefault(y => y.ID == id).ProductID));

            ViewBag.clientname = clientname;
            ViewBag.phonenumber = phonenumber;
            ViewBag.product = pr;

            return View();
        }

        [HttpPost]
        public ActionResult Delivery(int id)
        {
            string on = db.Orders.FirstOrDefault(x => x.ID == id).OrderNumber;
            foreach (var item in db.Orders)
            {
                if(item.OrderNumber == on)
                {
                    item.Status = item.Status + 1;
                }
            }
            db.SaveChanges();

            Control.smsinfo = 0;
            return RedirectToAction("sendsms", "sms");

            ViewBag.products = db.Products;
            ViewBag.users = db.Users;
            return View("orders",db.Orders.ToList());
          
        }
    }
}