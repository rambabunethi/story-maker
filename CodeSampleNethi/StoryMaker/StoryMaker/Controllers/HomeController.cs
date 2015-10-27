//////////////////////////////////////////////////////////////////////////
// HomeController.cs -           Controller for Home and Contact View   //
//                                                                      //
// Language:     C#                                                     //
// Platform:     Lenovo G 50-80, Windows 8.1,                           //
//               .Net Version 4.5, MVC 5                                //
// Application:  CSE686 - Internet Programming, Spring 2015             //
// Author:       Rambabu Nethi, Syracuse University                     //
//               (315) 728-8883, rnethi@syr.edu                         //
// Release Date: Version 1.0 May 1st - 2015                             //
//////////////////////////////////////////////////////////////////////////


using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace StoryMaker.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            var x = Server.MapPath("~/Content/contact.xml");    
            XDocument document = XDocument.Load(x);
            var contacts = from y in document.Elements("contact").Elements() select y;
            var contact_details = "";
            foreach(var contact in contacts)
            {
                var contact_ = contact.Elements();
                foreach(var details in contact_)
                {
                    contact_details =contact_details+details.Name + ":   " + details.Value+"#" ; 
                }
            }
            string[] contactsArray = contact_details.Split('#');
            ViewBag.contacts = contactsArray;
            return View();
        }
    }
}