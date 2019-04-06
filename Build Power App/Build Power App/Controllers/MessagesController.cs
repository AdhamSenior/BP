using Build_Power_App.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace Build_Power_App.Controllers
{
    public class MessagesController : Controller
    {
        // GET: Messages

      private  BuildPowerEntities db = new BuildPowerEntities();
        public ActionResult List()
        {
            ViewBag.messages = db.Messages;
            return View();
        }

   
        public ActionResult Answer(int id)
        {
            ViewBag.model = db.Messages.Find(id);
            Control.IDMessage = id;

            if (ViewBag.model != null)
                return PartialView();
            return HttpNotFound();
        }

        [HttpPost]
        public ActionResult Answer(FormCollection form)
        {
            Message kimga = db.Messages.FirstOrDefault(x=>x.ID== Control.IDMessage);
            string title = form["title"];
            string description = form["description"];


            var fromAddress = new MailAddress("dnetseniordeveloper@gmail.com", "Construction Admin");
            var toAddress = new MailAddress(kimga.Email, kimga.Name);
            const string fromPassword = "Web@1234";
             string subject = title;
             string body = description;

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



            return RedirectToAction("List", "Messages");
        }

    }
}