//////////////////////////////////////////////////////////////////////////
// DiagramController.cs -        Controller for handling view which     //
//                             displays ER diagram and Page Flow Diagra //
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

namespace StoryMaker.Controllers
{
    public class DiagramController : Controller
    {
        // GET: Diagram
        public ActionResult Index()
        {
            ViewBag.path1 = "~/Content/ERDiagram.jpg";
            ViewBag.path2 = "~/Content/PageFlow.jpg";
            return View();
        }
    }
}