using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Configuration;

using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

using Twilio.TwiML;
using Twilio.AspNet.Mvc;
using Build_Power_App.App_Start;

namespace Build_Power_App.Controllers
{
    public class SmsController : TwilioController
    {
        // Send: Sms
        public ActionResult SendSms()
        {
            var accountSid = "AC7587e8e9f653e26d88d40a0255a212dd";
            var authToken = "13836bc1ef19c4ccca2f8173792337d5";

            string accountsid = ConfigurationManager.AppSettings["TwilioAccountSid"];
            string token = ConfigurationManager.AppSettings["TwilioAuthToken"];

            TwilioClient.Init(accountSid, authToken);
            User user = Control.user;
            var to = new PhoneNumber(user.Phonenumber);
            var from = new PhoneNumber("+18646332366");

            var total = Control.cart.Sum(x => x.TotalPrice);
            // 30 is for tax and shipping;
            total = total + 30;
            if (Build_Power_App.App_Start.Control.isRu == true)
            {


                if (Control.smsinfo == 1)
                {
                    var message = MessageResource.Create(
                                  to: to,
                                  from: from,
                                  body: $"Здравствуйте {user.Firstname},                           Благодарим Вас за покупку.                     Ваш номер заказа : {Control.ordernumber},                       Общая сумма: ${Math.Round(Convert.ToDouble(total), 2)}; Ваш адрес доставки: {Control.user.address}, Доставка будет произведена в течение 3 рабочих дней. .            С уважением, Build Power LLC");
                    BuildPowerEntities db = new BuildPowerEntities();
                    //return Content(message.Sid);
                    ViewBag.success = "Successful Payment";
                    ViewBag.cart = Control.cart;
                    ViewBag.total = Control.cart.Sum(x => x.TotalPrice);

                    ViewBag.products = db.Products;
                    ViewBag.cartcount = Control.cart.Count();
                    return RedirectToAction("cart", "shop");
                }
                else
                {
                    var message = MessageResource.Create(
                                  to: to,
                                  from: from,
                                  body: $"Здравствуйте {user.Firstname},                             Ваш заказ в пути.                           Пожалуйста ожидайте заказ завтра в 14:00.                            С уважением, Build Power LLC");
                    BuildPowerEntities db = new BuildPowerEntities();
                    //return Content(message.Sid);
                    ViewBag.success = "Successful Payment";
                    ViewBag.cart = Control.cart;
                    ViewBag.total = Control.cart.Sum(x => x.TotalPrice);

                    ViewBag.products = db.Products;
                    ViewBag.cartcount = Control.cart.Count();
                    ViewBag.products = db.Products;
                    ViewBag.users = db.Users;
                    return RedirectToAction("orders", "order");
                }
            }
            else
            {

                if (Control.smsinfo == 1)
                {
                    var message = MessageResource.Create(
                                  to: to,
                                  from: from,
                                  body: $"Dear {user.Firstname},                           Thank you for your purchase.                          Your order number is : {Control.ordernumber},                       Total price: ${Math.Round(Convert.ToDouble(total), 2)}; Your delivery address is: {Control.user.address}, Delivery will be done within 3 business days.             Best regards, Build Power LLC");
                    BuildPowerEntities db = new BuildPowerEntities();
                    //return Content(message.Sid);
                    ViewBag.success = "Successful Payment";
                    ViewBag.cart = Control.cart;
                    ViewBag.total = Control.cart.Sum(x => x.TotalPrice);

                    ViewBag.products = db.Products;
                    ViewBag.cartcount = Control.cart.Count();
                    return RedirectToAction("cart", "shop");
                }
                else if (Control.smsinfo == 4)
                {
                    var message = MessageResource.Create(
                                 to: to,
                                 from: from,
                                 body: $"Dear {user.Firstname},                           Thank you for your purchase.                          Your order number is : {Control.ordernumber},                       Total price: ${Math.Round(Convert.ToDouble(total), 2)}; Your delivery address is: {Control.user.address}, Delivery will be done within 3 business days.             Best regards, Build Power LLC");
                    BuildPowerEntities db = new BuildPowerEntities();
                    //return Content(message.Sid);
                    ViewBag.success = "Successful Payment";
                    ViewBag.cart = Control.cart;
                    ViewBag.total = Control.cart.Sum(x => x.TotalPrice);

                    ViewBag.products = db.Products;
                    ViewBag.cartcount = Control.cart.Count();
                    return RedirectToAction("cart", "shop");
                }
                else 
                {
                    var message = MessageResource.Create(
                                  to: to,
                                  from: from,
                                  body: $"Dear {user.Firstname},                             Your order is on the way                          Please be ready to receive it tomorrow 14:00.                             Best regards, Build Power LLC");
                    BuildPowerEntities db = new BuildPowerEntities();
                    //return Content(message.Sid);
                    ViewBag.success = "Successful Payment";
                    ViewBag.cart = Control.cart;
                    ViewBag.total = Control.cart.Sum(x => x.TotalPrice);

                    ViewBag.products = db.Products;
                    ViewBag.cartcount = Control.cart.Count();
                    ViewBag.products = db.Products;
                    ViewBag.users = db.Users;
                    return RedirectToAction("orders", "order");
                }
            }


       
        }
    }
}