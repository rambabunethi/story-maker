//////////////////////////////////////////////////////////////////////////
// ErrorHandlingController.cs -  Controller for Handling errors         //
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
    [HandleError]
    public class ErrorHandlingController : Controller
    {

        // GET: Error
        public ActionResult Index()
        {
            Response.StatusCode = 500;
            return View();
        }

        //
        // GET: /Error/PageNotFound
        public ActionResult PageNotFound()
        {
            Response.StatusCode = 404;
            return View();
        }
    }
}