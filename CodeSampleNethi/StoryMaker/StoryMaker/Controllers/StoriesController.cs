//////////////////////////////////////////////////////////////////////////
// StoriesController.cs -        Controller for Story model class       //
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
    public class StoriesController : Controller
    {
        private StoryMakerContext db = new StoryMakerContext();
        [Authorize(Roles = "admin")]
        public ActionResult UnArchiveIndex()
        {
            var stories = from story in db.Stories where story.archivalStatus == true select story;
            return View(stories.ToList());
        }
        // GET: Stories
        [Authorize(Roles= "admin,developer,user")]
        public ActionResult Index()
        {
            var stories = from story in db.Stories where story.archivalStatus == false select story;
            return View(stories.ToList());
        }

        // GET: Stories/Details/5
        [Authorize(Roles="admin,developer,user")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Story story = db.Stories.Find(id);
            if (story == null)
            {
                return HttpNotFound();
            }
            ViewBag.storyName_tmp = story.storyName;
            ViewBag.archival_stat = story.archivalStatus;
            return View(story);
        }

        // GET: Stories/Create
        [Authorize(Roles = "developer")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Stories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "developer")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "storyId,storyName,storyDescription,storyLocation,archivalStatus,uploadDate,temp_string1,temp_string2,temp_int1,temp_int2")] Story story)
        {
            string storyDataPath, storyVirtualPath, storyNameNoSpace, storyPhysicalPath;
            story.uploadDate = DateTime.Now;
            story.archivalStatus = false;
            storyDataPath = "~/StoryData/";
            storyNameNoSpace = story.storyName.Replace(" ", "");
            storyVirtualPath = System.IO.Path.Combine(storyDataPath, storyNameNoSpace);
            storyPhysicalPath = Server.MapPath(storyVirtualPath);
            System.IO.Directory.CreateDirectory(storyPhysicalPath);
            story.storyLocation = storyVirtualPath;

            if (ModelState.IsValid)
            {
                db.Stories.Add(story);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(story);
        }

        // GET: Stories/Edit/5
        [Authorize(Roles = "developer")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Story story = db.Stories.Find(id);
            if (story == null)
            {
                return HttpNotFound();
            }
            ViewBag.archival_stat = story.archivalStatus;
            TempData["Story_Temp"] = story;
            return View(story);
        }

        // POST: Stories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "developer")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "storyId,storyName,storyDescription,storyLocation,archivalStatus,uploadDate,temp_string1,temp_string2,temp_int1,temp_int2")] Story story)
        {
            story.uploadDate = DateTime.Now;
            Story story_temp = TempData["Story_Temp"] as Story;
            story.archivalStatus = story_temp.archivalStatus;
            story.storyLocation = story_temp.storyLocation;
            if (ModelState.IsValid)
            {
                db.Entry(story).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(story);
        }

        // GET: Stories/Delete/5
        [Authorize(Roles = "admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Story story = db.Stories.Find(id);
            if (story == null)
            {
                return HttpNotFound();
            }
            return View(story);
        }

        [Authorize(Roles = "admin")]
        // POST: Stories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Story story = db.Stories.Find(id);
            db.Stories.Remove(story);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "developer,user")]
        public ActionResult SlideShow(int? id)
        {
            string imageLocation = "", imageCaption = "", imageDescription=""; 
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Story story = db.Stories.Find(id);
            ViewBag.archival_stat = story.archivalStatus;
            if (story == null)
            {
                return HttpNotFound();
            }
            var collages_Temp = from collage in story.Collages orderby collage.orderNumber select collage;
            foreach(Collage collage in collages_Temp)
            {
                var storyBlock_Temp = from storyBlock in collage.StoryBlocks orderby storyBlock.storyBlockOrderNumber select storyBlock;
                foreach(StoryBlock storyBlock in storyBlock_Temp)
                {
                    imageLocation = imageLocation + "'" + storyBlock.storyBlockLocation.Substring(1) + "'" + ",";
                    imageCaption = imageCaption + "~" + storyBlock.storyBlockCaption;
                    imageDescription = imageDescription + "~" + storyBlock.temp_string1;
                }
            }
            imageLocation = imageLocation.Substring(0, imageLocation.Length - 1);
            imageCaption = imageCaption.Substring(1, imageCaption.Length - 1);
            imageDescription = imageDescription.Substring(1, imageDescription.Length - 1);
            ViewBag.imageLocation = imageLocation;
            ViewBag.imageCaption = imageCaption;
            ViewBag.imageDescription = imageDescription;
            return View(story);
        }

        // GET: Stories/Archive/
        [Authorize(Roles = "admin")]
        public ActionResult ArchiveStory(int? id)
        {
            if(id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Story story = db.Stories.Find(id);
            if (story == null)
            {
                return HttpNotFound();
            }
            return View(story);
        }

        // POST: Stories/Archive/
        [Authorize(Roles = "admin")]
        [HttpPost, ActionName("ArchiveStory")]
        [ValidateAntiForgeryToken]

        public ActionResult ArchiveStory_post(int id)
        {
            Story story = db.Stories.Find(id);
            story.archivalStatus = true;
            foreach(Collage collage in story.Collages)
            {
                collage.archivalStatus = true;
                foreach (var storyblock in collage.StoryBlocks)
                {
                    storyblock.archivalStatus = true;
                    db.Entry(storyblock).State = EntityState.Modified;
                }
                db.Entry(collage).State = EntityState.Modified;
            }
            db.Entry(story).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Stories/UnArchive/
        [Authorize(Roles = "admin")]
        public ActionResult UnArchiveStory(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Story story = db.Stories.Find(id);
            if (story == null)
            {
                return HttpNotFound();
            }
            return View(story);
        }

        [HttpPost, ActionName("UnArchiveStory")]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]

        public ActionResult UnArchiveStory_post(int id)
        {
            Story story = db.Stories.Find(id);
            story.archivalStatus = false;
            foreach (Collage collage in story.Collages)
            {
                collage.archivalStatus = false;
                foreach (var storyblock in collage.StoryBlocks)
                {
                    storyblock.archivalStatus = false;
                    db.Entry(storyblock).State = EntityState.Modified;
                }
                db.Entry(collage).State = EntityState.Modified;
            }
            db.Entry(story).State = EntityState.Modified;
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
