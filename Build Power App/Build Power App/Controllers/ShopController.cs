using Build_Power_App.App_Start;
using Build_Power_App.Models;
using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Build_Power_App.Controllers
{
    public class ShopController : Controller
    {
        private BuildPowerEntities db = new BuildPowerEntities();
        // GET: Shop
        public ActionResult Index()
        {
            ViewBag.products = db.Products.ToList();
            ViewBag.cartcount = Control.cart.Count();
            ViewBag.cart = Control.cart;
            return View();
        }

        [HttpPost]
        public ActionResult AddToCart(FormCollection form)
        {
            string str = form["id"];
            int id = int.Parse(form["id"]);
            int quantity = int.Parse(form["quantity"]);

            Product pr = db.Products.Find(id);

            Control.cart.Add(new Order()
            {
                OrderDate = DateTime.Now,
                ProductID = id,
                Quantity = quantity,
                Status = 1,
                TotalPrice = pr.Price * quantity,
                UserID = Control.user.ID
            });


            ViewBag.products = db.Products;
            ViewBag.cartcount = Control.cart.Count();
            ViewBag.cart = Control.cart;
            return View("Index");
            


        }

        public ActionResult SingleProduct(int id)
        {
            ViewBag.cart = Control.cart;
            ViewBag.total = Control.cart.Sum(x => x.TotalPrice);

            ViewBag.products = db.Products;
            ViewBag.cartcount = Control.cart.Count();
            ViewBag.product = db.Products.Find(id);
         
            
            Product producttt = db.Products.Find(id);
            ViewBag.comments = db.Reviews.Where(x => x.ItemID == producttt.ID && x.ParentID == 0 && x.isproduct == 1).ToList();
            ViewBag.commentcount = db.Reviews.Where(x => x.ItemID == producttt.ID).ToList().Count();
            ViewBag.realetedone = db.Products.Where(x => x.CategoryID == producttt.CategoryID).ToList().Take(2);
            ViewBag.realetedtwo = db.Products.Where(x => x.CategoryID == producttt.CategoryID).ToList().Skip(2).Take(2);
            ViewBag.allcomments = db.Reviews.ToList();
            ViewBag.childcomments = db.Reviews.Where(x => x.ItemID == producttt.ID && x.ParentID != 0 && x.isproduct == 1).ToList();
            return View();
        }



        public ActionResult filterlow()
        {


            ViewBag.products = db.Products.ToList();


            ViewBag.products = db.Products.OrderByDescending(x => x.Price);



            ViewBag.cartcount = Control.cart.Count();
            ViewBag.cart = Control.cart;
            return View("index");
        }


        public ActionResult filterhigh()
        {


            ViewBag.products = db.Products.ToList();


            ViewBag.products = db.Products.OrderBy(x => x.Price);



            ViewBag.cartcount = Control.cart.Count();
            ViewBag.cart = Control.cart;
            return View("index");
        }


        public ActionResult filteradded()
        {


            ViewBag.products = db.Products.ToList();


            ViewBag.products = db.Products.OrderBy(x => x.ArrivedTime);



            ViewBag.cartcount = Control.cart.Count();
            ViewBag.cart = Control.cart;
            return View("index");
        }

        public ActionResult filtersorting()
        {


            ViewBag.products = db.Products.ToList();


            ViewBag.products = db.Products.OrderByDescending(x => x.ArrivedTime);



            ViewBag.cartcount = Control.cart.Count();
            ViewBag.cart = Control.cart;
            return View("index");
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

            if (commentid != 0)
            {
                habar.ParentID = commentid;

            }
            else
            {
                habar.ParentID = 0;
            }

            habar.isproduct = 1;


            db.Reviews.Add(habar);
            db.SaveChanges();


            return RedirectToAction($"SingleProduct/{id}", id);

        }
        public ActionResult Cart()
        {
            ViewBag.cart = Control.cart;
            ViewBag.total = Control.cart.Sum(x => x.TotalPrice);

            if (Control.payment)
            {
                ViewBag.success = "Successful payment! We sent you a notice of the order to your mobile phone number. Our managers will call you as soon as possible. respectfully";
                Control.payment = false;
            }

            ViewBag.products = db.Products;
            ViewBag.cartcount = Control.cart.Count();
            return View();
        }

        public ActionResult Clear(string page,string pageRoute)
        {
            Control.cart.RemoveAll(x=>x.Quantity>0);
            ViewBag.cart = Control.cart;
            ViewBag.total = Control.cart.Sum(x => x.TotalPrice);

            ViewBag.products = db.Products;
            ViewBag.cartcount = Control.cart.Count();
            if(pageRoute== "AddToCart")
            {
                return RedirectToAction("index", page);
            }
            return RedirectToAction(pageRoute, page);
        }
        public ActionResult Clear1()
        {
            Control.cart.RemoveAll(x => x.Quantity > 0);
            ViewBag.cart = Control.cart;
            ViewBag.total = Control.cart.Sum(x => x.TotalPrice);

            ViewBag.products = db.Products;
            ViewBag.cartcount = Control.cart.Count();
           
            return RedirectToAction("cart", "shop");
        }
        [HttpPost]
        public ActionResult updatecart(FormCollection collection, List<Product> cart)
        {
            string value = collection["num-product1"];
            string id = collection["iddd"];
            string[] quantity = value.Split(',');
            string[] ides = id.Split(',');

            for (int i = 0; i < ides.Length; i++)
            {
                foreach (var item in Control.cart)
                {
                    if (Convert.ToInt32(ides[i]) == item.ProductID)
                    {
                        int quan = Convert.ToInt32(quantity[i]);
                        item.Quantity = quan;
                        item.TotalPrice = item.Quantity * db.Products.FirstOrDefault(x=>x.ID == item.ProductID).Price;
                    }
                }
            }
            ViewBag.cart = Control.cart;
            ViewBag.total = Control.cart.Sum(x => x.TotalPrice);
         
            ViewBag.products = db.Products;
            return View("cart");
        }
        public ActionResult cash()
        {
            ViewBag.success = "Successful Payment";
            Control.payment = true;

            foreach (var item in Control.cart)
            {
                item.Status = 2;
                item.OrderNumber = Control.ordernumber;
                db.Orders.Add(item);
            }
            db.SaveChanges();
            Control.smsinfo = 4;
            return RedirectToAction("sendsms", "sms");
        }

        public ActionResult PaymentWithPaypal(string Cancel = null)
        {
            //getting the apiContext  
            APIContext apiContext = PaypalConfiguration.GetAPIContext();
            try
            {
                //A resource representing a Payer that funds a payment Payment Method as paypal  
                //Payer Id will be returned when payment proceeds or click to pay  
                string payerId = Request.Params["PayerID"];
                if (string.IsNullOrEmpty(payerId))
                {
                    //this section will be executed first because PayerID doesn't exist  
                    //it is returned by the create function call of the payment class  
                    // Creating a payment  
                    // baseURL is the url on which paypal sendsback the data.  
                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/Shop/PaymentWithPayPal?";
                    //here we are generating guid for storing the paymentID received in session  
                    //which will be used in the payment execution  
                    var guid = Convert.ToString((new Random()).Next(100000));
                    //CreatePayment function gives us the payment approval url  
                    //on which payer is redirected for paypal account payment  
                    var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid);
                    //get links returned from paypal in response to Create function call  
                    var links = createdPayment.links.GetEnumerator();
                    string paypalRedirectUrl = null;
                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;
                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            //saving the payapalredirect URL to which user will be redirected for payment  
                            paypalRedirectUrl = lnk.href;
                        }
                    }
                    // saving the paymentID in the key guid  
                    Session.Add(guid, createdPayment.id);
                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    // This function exectues after receving all parameters for the payment  
                    var guid = Request.Params["guid"];
                    var executedPayment = ExecutePayment(apiContext, payerId, Session[guid] as string);
                    //If executed payment failed then we will show payment failure message to user  
                    if (executedPayment.state.ToLower() != "approved")
                    {
                        ViewBag.error = "Payment failed";
                        Control.payment = false;
                        ViewBag.cart = Control.cart;
                        ViewBag.total = Control.cart.Sum(x => x.TotalPrice);

                        ViewBag.products = db.Products;
                        ViewBag.cartcount = Control.cart.Count();
                        return View("cart");
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.error = "Payment failed";
                Control.payment = false;
                ViewBag.cart = Control.cart;
                ViewBag.total = Control.cart.Sum(x => x.TotalPrice);

                ViewBag.products = db.Products;
                ViewBag.cartcount = Control.cart.Count();
                return View("cart");
            }
            //on successful payment, show success page to user. 
            ViewBag.success = "Successful Payment";
            Control.payment = true;

            foreach (var item in Control.cart)
            {
                item.Status = 2;
                item.OrderNumber = Control.ordernumber;
                db.Orders.Add(item);
            }
            db.SaveChanges();
            Control.smsinfo = 1;
            return RedirectToAction("sendsms", "sms");
           
        }
        private PayPal.Api.Payment payment;
        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution()
            {
                payer_id = payerId
            };
            this.payment = new Payment()
            {
                id = paymentId
            };
            return this.payment.Execute(apiContext, paymentExecution);
        }
        private Payment CreatePayment(APIContext apiContext, string redirectUrl)
        {
            //create itemlist and add item objects to it  
            var itemList = new ItemList()
            {
                items = new List<Item>()
            };

            //Adding Item Details like name, currency, price etc  

            foreach (var item in Control.cart)
            {
                Product pr = db.Products.Find(item.ProductID);
                itemList.items.Add(new Item()
                {
                    
                    name = pr.Name,
                    currency = "USD",
                    price = Math.Round(Convert.ToDouble(pr.Price),2).ToString(),
                    quantity = item.Quantity.ToString(),
                    sku = "sku"
                });
            }
            
            var payer = new Payer()
            {
                payment_method = "paypal"
            };
            // Configure Redirect Urls here with RedirectUrls object  
            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl + "&Cancel=true",
                return_url = redirectUrl
            };

            var totalsumma = Control.cart.Sum(x => x.TotalPrice);
            // Adding Tax, shipping and Subtotal details  
            var details = new Details()
            {
                tax = "20",
                shipping = "10",
                subtotal = Math.Round(Convert.ToDouble(totalsumma), 2).ToString()
            };


            //Final amount with details  
            var amount = new Amount()
            {

                currency = "USD",
                total = (Convert.ToInt32(details.subtotal) + Convert.ToInt32(details.shipping) + Convert.ToInt32(details.tax)).ToString(), // Total must be equal to sum of tax, shipping and subtotal.  
                details = details
            };
            var transactionList = new List<Transaction>();
            Control.ordernumber = Convert.ToString((new Random()).Next(10000000));
            // Adding description about the transaction  
            transactionList.Add(new Transaction()
            {
                description = "Transaction description",
                invoice_number = Control.ordernumber, //Generate an Invoice No  
                amount = amount,
                item_list = itemList
            });

            this.payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };
            // Create a payment using a APIContext  
            return this.payment.Create(apiContext);
        }


    }
}