using Build_Power_App.App_Start;
using Build_Power_App.Models;
using CaptchaMvc.HtmlHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace Build_Power_App.Controllers
{
    public class HomeController : Controller
    {
        private BuildPowerEntities db = new BuildPowerEntities();
        // GET: Home
        public ActionResult Index()
        {
            ViewBag.news = db.News.OrderByDescending(x=>x.DateTime).Take(2).ToList();
            ViewBag.news2 = db.News.OrderByDescending(x => x.DateTime).Skip(2).Take(3).ToList();
            ViewBag.news3 = db.News.OrderByDescending(x => x.DateTime).Skip(5).Take(4).ToList();
            ViewBag.projects = "current";

            ViewBag.cart = Control.cart;
            ViewBag.total = Control.cart.Sum(x => x.TotalPrice);
            ViewBag.products = db.Products;
            
            List<Message> list = new List<Message>();
            int length = db.Messages.Count();
            int i = 0;            foreach (var item in db.Messages)
            {
                if (i >= length - 3)
                {
                    list.Add(item);
                    if (i == 4) break;
                }
                i++;
            }
            ViewBag.messages = list;


            ViewBag.cartcount = Control.cart.Count();
            ViewBag.projects = db.Projects.Take(8).ToList();


            return View();
        }     
        public ActionResult About()
        {
            ViewBag.about = "current";

            ViewBag.cart = Control.cart;
            ViewBag.total = Control.cart.Sum(x => x.TotalPrice);
            ViewBag.products = db.Products;
            ViewBag.cartcount = Control.cart.Count();

            return View();
        }

        public ActionResult ChoosePayment()
        {


            ViewBag.cart = Control.cart;
            ViewBag.total = Control.cart.Sum(x => x.TotalPrice);
            ViewBag.products = db.Products;
            ViewBag.cartcount = Control.cart.Count();
            return View();
        }

        [HttpPost]
        public ActionResult search( string q)
        {
            ViewBag.resultproducts  = db.Products.Where(x=>x.Name.Contains(q) || x.Description.Contains(q)).ToList();
            ViewBag.resultnews = db.News.Where(x => x.Title.Contains(q) || x.Description.Contains(q)).ToList();
            ViewBag.resultprojects = db.Projects.Where(x => x.ProjectName.Contains(q) || x.Description.Contains(q) || x.Location.Contains(q) || x.CompanyName.Contains(q)).ToList();
            ViewBag.categories = db.Categories;
            ViewBag.roles = db.UserRoles;
            ViewBag.users = db.Users;

            ViewBag.text = q;

            ViewBag.cart = Control.cart;
            ViewBag.total = Control.cart.Sum(x => x.TotalPrice);
            ViewBag.products = db.Products;
            ViewBag.cartcount = Control.cart.Count();
            return View();
        }
        public ActionResult Blog()
        {
            ViewBag.blog = "current";
            BuildPowerEntities db = new BuildPowerEntities();
            ViewBag.news = db.News.Take(4).ToList();
            ViewBag.categories = db.Categories;
            ViewBag.roles = db.UserRoles;
            ViewBag.users = db.Users;

            ViewBag.cart = Control.cart;
            ViewBag.total = Control.cart.Sum(x => x.TotalPrice);
            ViewBag.products = db.Products;
            ViewBag.cartcount = Control.cart.Count();

            return View();
        }

        public ActionResult SingleBlog(int id)
        {
            ViewBag.cart = Control.cart;
            ViewBag.total = Control.cart.Sum(x => x.TotalPrice);
            ViewBag.categories = db.Categories.ToList();
            ViewBag.products = db.Products;
            ViewBag.cartcount = Control.cart.Count();
            ViewBag.news = db.News.Find(id);
            News yangilik = db.News.Find(id);
            ViewBag.comments = db.Reviews.Where(x => x.ItemID == yangilik.ID && x.ParentID==0 && x.isproduct==0).ToList();
            ViewBag.commentcount = db.Reviews.Where(x => x.ItemID == yangilik.ID).ToList().Count();
            ViewBag.realetedone = db.News.Where(x => x.CategoryID == yangilik.CategoryID).ToList().Take(2);
            ViewBag.realetedtwo = db.News.Where(x => x.CategoryID == yangilik.CategoryID).ToList().Skip(2).Take(2);
            ViewBag.allcomments = db.Reviews.ToList();
            ViewBag.childcomments = db.Reviews.Where(x => x.ItemID == yangilik.ID && x.ParentID != 0 && x.isproduct == 0).ToList();
            return View();
        }

        [HttpPost]
        public ActionResult SubmitComment(FormCollection form, int id, int commentid)
        {
            Review habar = new Review();

            habar.Name = form["author"];
            habar.Email = form["email"];
            habar.Comment = form["comment"];
            habar.DateTime = DateTime.Now;

            habar.ItemID = id;

            if( commentid != 0)
            {
                habar.ParentID = commentid;

            }
            else
            {
                habar.ParentID = 0;
            }

            habar.isproduct = 0;


            db.Reviews.Add(habar);
            db.SaveChanges();


             return RedirectToAction($"SingleBlog/{id}",id);

        }

        public ActionResult blogbyCategory(int id)
        {
            ViewBag.news = db.News.Where(x => x.CategoryID == id).Take(4).ToList();
            ViewBag.blog = "current";
          
            ViewBag.categories = db.Categories;
            ViewBag.roles = db.UserRoles;
            ViewBag.users = db.Users;

            ViewBag.cart = Control.cart;
            ViewBag.total = Control.cart.Sum(x => x.TotalPrice);
            ViewBag.products = db.Products;
            ViewBag.cartcount = Control.cart.Count();
            return View("blog");
        }

        public ActionResult Condition()
        {
            ViewBag.cart = Control.cart;
            ViewBag.total = Control.cart.Sum(x => x.TotalPrice);
            ViewBag.products = db.Products;
            ViewBag.cartcount = Control.cart.Count();
            return View();
        }

        public ActionResult GetAllBlog()
        {
            ViewBag.blog = "current";
            BuildPowerEntities db = new BuildPowerEntities();
            ViewBag.news = db.News.ToList();
            ViewBag.categories = db.Categories;
            ViewBag.roles = db.UserRoles;
            ViewBag.users = db.Users;

            ViewBag.cart = Control.cart;
            ViewBag.total = Control.cart.Sum(x => x.TotalPrice);
            ViewBag.products = db.Products;
            ViewBag.cartcount = Control.cart.Count();

            return View("Blog");
        }
        public ActionResult Investors()
        {
            ViewBag.investors = "current";

            ViewBag.cart = Control.cart;
            ViewBag.total = Control.cart.Sum(x => x.TotalPrice);
            ViewBag.products = db.Products;
            ViewBag.cartcount = Control.cart.Count();

            return View();
        }
        public ActionResult projects()
        {
            ViewBag.projects = "current";
            BuildPowerEntities db = new BuildPowerEntities();
            ViewBag.projects = db.Projects.Take(8).ToList();

            ViewBag.cart = Control.cart;
            ViewBag.total = Control.cart.Sum(x => x.TotalPrice);
            ViewBag.products = db.Products;
            ViewBag.cartcount = Control.cart.Count();

            return View();
        }
        public ActionResult FAQ()
        {
            ViewBag.faq = "current";

            ViewBag.cart = Control.cart;
            ViewBag.total = Control.cart.Sum(x => x.TotalPrice);
            ViewBag.products = db.Products;
            ViewBag.cartcount = Control.cart.Count();

            return View();
        }
        public ActionResult warranty()
        {
            ViewBag.faq = "current";

            ViewBag.cart = Control.cart;
            ViewBag.total = Control.cart.Sum(x => x.TotalPrice);
            ViewBag.products = db.Products;
            ViewBag.cartcount = Control.cart.Count();

            return View();
        }
        public ActionResult Contact()
        {
            ViewBag.contact = "current";

            ViewBag.cart = Control.cart;
            ViewBag.total = Control.cart.Sum(x => x.TotalPrice);
            ViewBag.products = db.Products;
            ViewBag.cartcount = Control.cart.Count();

            return View();
        }

        [HttpPost]
        public ActionResult Contact(FormCollection form)
        {
            Message habar = new Message();

            habar.Name = form["name"];
            habar.Email = form["email"];
            habar.Phone = form["phone"];
            habar.Subject = form["subject"];
            habar.Description = form["message"];

            BuildPowerEntities db = new BuildPowerEntities();
            db.Messages.Add(habar);
            db.SaveChanges();
            ViewBag.info = "Sizning Murojatingiz qabul qilindi!";
            ViewBag.contact = "current";

            ViewBag.cart = Control.cart;
            ViewBag.total = Control.cart.Sum(x => x.TotalPrice);
            ViewBag.products = db.Products;
            ViewBag.cartcount = Control.cart.Count();


            return View();
        }
        public ActionResult MessageContact()
        {
            ViewBag.info = "Sizning Murojatingiz qabul qilindi!";

            ViewBag.cart = Control.cart;
            ViewBag.total = Control.cart.Sum(x => x.TotalPrice);
            ViewBag.products = db.Products;
            ViewBag.cartcount = Control.cart.Count();

            return View();
        }


        public ActionResult Login()
        {

            return View();
        }

        public ActionResult ChangetoRu()
        {
            ViewBag.news = db.News.OrderByDescending(x => x.DateTime).Take(2).ToList();
            ViewBag.news2 = db.News.OrderByDescending(x => x.DateTime).Skip(2).Take(3).ToList();
            ViewBag.news3 = db.News.OrderByDescending(x => x.DateTime).Skip(5).Take(4).ToList();
            ViewBag.home = "current";

            ViewBag.cart = Control.cart;
            ViewBag.total = Control.cart.Sum(x => x.TotalPrice);
            ViewBag.products = db.Products;

            List<Message> list = new List<Message>();
            int length = db.Messages.Count();
            int i = 0; foreach (var item in db.Messages)
            {
                if (i >= length - 3)
                {
                    list.Add(item);
                    if (i == 4) break;
                }
                i++;
            }
            ViewBag.messages = list;

            ViewBag.cartcount = Control.cart.Count();
            ViewBag.projects = db.Projects.Take(8).ToList();

            Control.isRu = true;


            return View("index");
        }
        public ActionResult ChangetoEn()
        {
            ViewBag.news = db.News.OrderByDescending(x => x.DateTime).Take(2).ToList();
            ViewBag.news2 = db.News.OrderByDescending(x => x.DateTime).Skip(2).Take(3).ToList();
            ViewBag.news3 = db.News.OrderByDescending(x => x.DateTime).Skip(5).Take(4).ToList();
            ViewBag.home = "current";

            ViewBag.cart = Control.cart;
            ViewBag.total = Control.cart.Sum(x => x.TotalPrice);
            ViewBag.products = db.Products;

            List<Message> list = new List<Message>();
            int length = db.Messages.Count();
            int i = 0; foreach (var item in db.Messages)
            {
                if (i >= length - 3)
                {
                    list.Add(item);
                    if (i == 4) break;
                }
                i++;
            }
            ViewBag.messages = list;

            ViewBag.cartcount = Control.cart.Count();
            ViewBag.projects = db.Projects.Take(8).ToList();

            Control.isRu = false;


            return View("index");
        }

        [HttpPost]
        public ActionResult Login(FormCollection form)
        {
            string your_name = form["your_name"];
            string your_pass = form["your_pass"];


            if (your_name == "" && your_pass == "")
            {
                ViewBag.error = "Please enter login details!!!";
                return View();
            }
            if (your_name == "" && your_pass != "")
            {
                ViewBag.error = "Username field is empty!!!";
                return View();
            }
            if (your_pass == "" && your_name != "")
            {
                ViewBag.error = "Password field is empty!!!";
                return View();
            }

            if (your_name != null && your_pass != null && your_name != "" && your_pass != "")
            {
                BuildPowerEntities db = new BuildPowerEntities();
                User user = new User();
                user = db.Users.FirstOrDefault(x => x.Email == your_name && x.Password == your_pass);
                if (user == null)
                {
                    ViewBag.error = "You entered incorrect details!!!";
                    return View();
                }

                Status.UserID = user.ID;
                Control.user = user;
                Control.payment = false;
                Control.cart.RemoveAll(x => x.Quantity > 0);
                ViewBag.cart = Control.cart;
                ViewBag.total = Control.cart.Sum(x => x.TotalPrice);

                ViewBag.products = db.Products;
                ViewBag.cartcount = Control.cart.Count();

                return RedirectToAction("Condition", "Home");
            }
            else
            {
                ViewBag.error = "You entered incorrect details!!!";
                return View();
            }
        }


        public ActionResult Registration()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Registration(User us, string name, string email, string pass, string re_pass, string address, string phone)
        {

            if (name == "" || name == null && email == null || email == "" && pass == "" || pass == null && re_pass == "" || re_pass == null)
            {
                ViewBag.error = "Fill in empty fields!!!";
                return View();
            }

            if (name == null || name == "")
            {
                ViewBag.error = "Firstname field is empty!!!";
                return View();
            }

            if (email == null || email == "")
            {
                ViewBag.error = "Email field is empty!!!";
                return View();
            }


            if (pass == null || pass == "")
            {
                ViewBag.error = "Password field is empty!!!";
                return View();
            }



            if (this.IsCaptchaValid("The answer is not correct!"))
            {
                BuildPowerEntities db = new BuildPowerEntities();

                if (name != null && email != null && pass != "" && re_pass != "")
                {
                    if (pass != re_pass)
                    {
                        ViewBag.error = "Password doesn't match!!!";
                        return View();
                    }
                    else
                    {
                        var result = db.Users.FirstOrDefault(x => x.Username == email);
                        if (result != null)
                        {
                            ViewBag.error = "This Username is taken!!!";
                            return View();
                        }

                        User user = new User();
                        user.Firstname = name;
                        user.Username = email;
                        user.Password = pass;
                        user.Email = email;
                        user.RoleID = 1;
                        user.address = address;
                        user.Phonenumber = phone;

                        db.Users.Add(user);
                        db.SaveChanges();

                        Status.UserID = user.ID;

                        var fromAddress = new MailAddress("buildpower.uz@gmail.com", "Construction Admin");
                        var toAddress = new MailAddress(email, name);
                        const string fromPassword = "web@1234";
                        const string subject = "[BuildPower] Registration Confirmation";
                        const string body = "We are excited to tell you that your account is" +
                    " successfully created!  ";

                        var smtp = new SmtpClient
                        {
                            Host = "smtp.gmail.com",
                            Port = 587,
                            EnableSsl = true,
                            DeliveryMethod = SmtpDeliveryMethod.Network,
                            UseDefaultCredentials = false,
                            Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                        };
                        using (var message = new MailMessage(fromAddress, toAddress)
                        {
                            Subject = subject,
                            Body = body
                        })
                        {
                            smtp.Send(message);
                        }


                        return RedirectToAction("login", "home");
                    }

                }
                else
                {
                    ViewBag.error = "Fill the form!!!";
                    return View();
                }
            }

            ViewBag.error = "Please verify you are not Robot!";
            return View();

        }
    }
}