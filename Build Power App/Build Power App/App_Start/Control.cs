using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Build_Power_App.App_Start
{
    public static class Control
    {
        public static int IDMessage { get; set; }
        public static List<Order> cart = new List<Order>();
        public static User user = new User();
        public static User admin = new User();
        public static bool isRu = false;
        public static bool payment = false;
        public static int smsinfo {get;set;}
        public static string ordernumber { get; set; }

    }
}