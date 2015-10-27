//////////////////////////////////////////////////////////////////////////
// CollagesController.cs -       Controller for Collages model class    //
//                               aacepts input from user and interacts  //
//                               with view and model accordingly.       //
//                               Provides CRUD methods                  //
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
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using StoryMaker.Models;

namespace StoryMaker.Controllers
{
    [HandleError]
    public class CollagesController : Controller
    {
        private StoryMakerContext db = new StoryMakerContext();

        // GET: Collages
        [Authorize(Roles = "admin,developer,user")]
        public ActionResult Index()
        {
            var collages = from collage in db.Collages where collage.archivalStatus == false select collage;
            return View(collages.ToList());
        }

        // GET: Collages/Details/5
        [Authorize(Roles = "admin,developer,user")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Collage collage = db.Collages.Find(id);
            ViewBag.collageName = collage.collageName;
            ViewBag.archival_stat = collage.archivalStatus;
            if (collage == null)
            {
                return HttpNotFound();
            }
            return View(collage);
        }

        // GET: Collages/Create
        [Authorize(Roles = "developer")]
        public ActionResult Create()
        {
            ViewBag.storyId = new SelectList(db.Stories, "storyId", "storyName");
            return View();
        }

        // POST: Collages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "developer")]
        public ActionResult Create([Bind(Include = "collageId,collageName,collageDescription,orderNumber,collageLocation,archivalStatus,uploadDate,storyId,temp_string1,temp_string2,temp_int1,temp_int2")] Collage collage)
        {
            Story story = db.Stories.Find(collage.storyId);
            collage.collageLocation = story.storyLocation;
            collage.uploadDate = DateTime.Now;
            collage.archivalStatus = false;
            if (ModelState.IsValid)
            {
                db.Collages.Add(collage);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.storyId = new SelectList(db.Stories, "storyId", "storyName", collage.storyId);
            return View(collage);
        }

        // GET: Collages/Edit/5
        [Authorize(Roles = "developer")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Collage collage = db.Collages.Find(id);
            if (collage == null)
            {
                return HttpNotFound();
            }
            ViewBag.archival_stat = collage.archivalStatus;
            ViewBag.storyId = new SelectList(db.Stories, "storyId", "storyName", collage.storyId);
            TempData["Collage_Temp"] = collage;
            return View(collage);
        }

        // POST: Collages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "developer")]
        public ActionResult Edit([Bind(Include = "collageId,collageName,collageDescription,orderNumber,collageLocation,archivalStatus,uploadDate,storyId,temp_string1,temp_string2,temp_int1,temp_int2")] Collage collage)
        {
            collage.uploadDate = DateTime.Now;
            Collage collage_temp = TempData["Collage_Temp"] as Collage;
            collage.collageLocation = collage_temp.collageLocation;
            collage.archivalStatus = collage_temp.archivalStatus;
            if (ModelState.IsValid)
            {
                db.Entry(collage).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.storyId = new SelectList(db.Stories, "storyId", "storyName", collage.storyId);
            return View(collage);
        }

        // GET: Collages/Delete/5
        [Authorize(Roles = "developer")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Collage collage = db.Collages.Find(id);
            if (collage == null)
            {
                return HttpNotFound();
            }
            ViewBag.archival_stat = collage.archivalStatus;
            return View(collage);
        }

        // POST: Collages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "developer")]
        public ActionResult DeleteConfirmed(int id)
        {
            Collage collage = db.Collages.Find(id);
            db.Collages.Remove(collage);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
