//////////////////////////////////////////////////////////////////////////
// StoryBlocksController.cs -    Controller for StoryBlock model class  //
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
using System.IO;

namespace StoryMaker.Controllers
{
    [HandleError]
    public class StoryBlocksController : Controller
    {
        private StoryMakerContext db = new StoryMakerContext();

        // GET: StoryBlocks
        [Authorize(Roles = "admin,developer,user")]
        public ActionResult Index()
        {
            var storyBlocks = from storyBlock in db.StoryBlocks where storyBlock.archivalStatus == false select storyBlock;
            return View(storyBlocks.ToList());
        }

        // GET: StoryBlocks/Details/5
        [Authorize(Roles = "admin,developer,user")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StoryBlock storyBlock = db.StoryBlocks.Find(id);
            ViewBag.archival_stat = storyBlock.archivalStatus;
            if (storyBlock == null)
            {
                return HttpNotFound();
            }
            return View(storyBlock);
        }

        // GET: StoryBlocks/Create
        [Authorize(Roles = "developer")]
        public ActionResult Create()
        {
            ViewBag.collageId = new SelectList(db.Collages, "collageId", "collageName");
            return View();
        }

        // POST: StoryBlocks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "developer")]
        public ActionResult Create([Bind(Include = "storyBlockId,storyBlockName,storyBlockCaption,storyBlockDescription,storyBlockLocation,storyBlockOrderNumber,archivalStatus,uploadDate,collageId,temp_string1,temp_string2,temp_int1,temp_int2")] StoryBlock storyBlock, HttpPostedFileBase clientUploadedFile)
        {
            storyBlock.uploadDate = DateTime.Now;
            storyBlock.archivalStatus = false;
            //storyBlock.storyBlockDescription = storyBlock.temp_string1;
            string acceptedFileType = "image/jpeg,image/gif,image/tiff,image/png,image/jpg"; //Accepts the mentioned file types for upload
            string imageName,image_Path,updated_Path;
            if (clientUploadedFile != null && clientUploadedFile.ContentLength > 0)         //checks for empty files
            {
                if (acceptedFileType.Split(",".ToCharArray()).Contains(clientUploadedFile.ContentType)) // checks for valid extensions
                {
                    if (clientUploadedFile.ContentLength < 5000000)                   // supports up to 5MB size files
                    {
                        imageName = System.IO.Path.GetFileName(clientUploadedFile.FileName);
                        Collage collage1 = db.Collages.Find(storyBlock.collageId);
                        image_Path = System.IO.Path.Combine(Server.MapPath(collage1.collageLocation), imageName); 
                        updated_Path = collage1.collageLocation + "/" + imageName;                                  // stores image path as a static path along with image name
                        clientUploadedFile.SaveAs(image_Path);
                        using (MemoryStream m_stream = new MemoryStream())                                  // Creates a stream and store to memory
                        {
                            clientUploadedFile.InputStream.CopyTo(m_stream);
                            byte[] array = m_stream.GetBuffer();
                        }
                        storyBlock.storyBlockLocation = updated_Path;
                        if (ModelState.IsValid)
                        {
                            db.StoryBlocks.Add(storyBlock);
                            db.SaveChanges();
                            return RedirectToAction("Index");
                        }
                    }
                    else
                    {
                        ViewBag.errorMessage = "The file to be uploaded is exceeding the size limit. Please upload a file lessthan 5MB";
                    }
                }
                else
                {
                    ViewBag.errorMessage = "Please upload the following type of files: image/jpeg,image/gif,image/tiff,image/png,image/jpg";
                }
            }
            else
            {
                ViewBag.errorMessage = "Please upload a non empty file";
            }
            
            
            ViewBag.collageId = new SelectList(db.Collages, "collageId", "collageName", storyBlock.collageId);
            return View(storyBlock);
        }

        // GET: StoryBlocks/Edit/5
        [Authorize(Roles = "developer")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StoryBlock storyBlock = db.StoryBlocks.Find(id);
            if (storyBlock == null)
            {
                return HttpNotFound();
            }
            ViewBag.archival_stat = storyBlock.archivalStatus;
            ViewBag.collageId = new SelectList(db.Collages, "collageId", "collageName", storyBlock.collageId);
            TempData["StoryBlock_Temp"] = storyBlock;
            return View(storyBlock);
        }

        // POST: StoryBlocks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "developer")]
        public ActionResult Edit([Bind(Include = "storyBlockId,storyBlockName,storyBlockCaption,storyBlockDescription,storyBlockLocation,storyBlockOrderNumber,archivalStatus,uploadDate,collageId,temp_string1,temp_string2,temp_int1,temp_int2")] StoryBlock storyBlock, HttpPostedFileBase clientUploadedFile)
        {
            storyBlock.uploadDate = DateTime.Now;
            StoryBlock StoryBlock_Temp = TempData["StoryBlock_Temp"] as StoryBlock;
            storyBlock.archivalStatus = StoryBlock_Temp.archivalStatus;
            string acceptedFileType = "image/jpeg,image/gif,image/tiff,image/png,image/jpg";
            string imageName, image_Path, updated_Path;
     
            if (clientUploadedFile != null && clientUploadedFile.ContentLength > 0)               // update image
            {
                if (acceptedFileType.Split(",".ToCharArray()).Contains(clientUploadedFile.ContentType))
                {
                    if (clientUploadedFile.ContentLength < 5000000)
                    {
                        imageName = System.IO.Path.GetFileName(clientUploadedFile.FileName);
                        Collage collage1 = db.Collages.Find(storyBlock.collageId);
                        image_Path = System.IO.Path.Combine(Server.MapPath(collage1.collageLocation), imageName);
                        updated_Path = collage1.collageLocation + "/" + imageName;
                        clientUploadedFile.SaveAs(image_Path);
                        using (MemoryStream m_stream = new MemoryStream())
                        {
                            clientUploadedFile.InputStream.CopyTo(m_stream);
                            byte[] array = m_stream.GetBuffer();
                        }
                        storyBlock.storyBlockLocation = updated_Path;
                        if (ModelState.IsValid)
                        {
                            db.Entry(storyBlock).State = EntityState.Modified;
                            db.SaveChanges();
                            return RedirectToAction("Index");
                        }
                    }
                    else
                    {
                        ViewBag.errorMessage = "The file to be uploaded is exceeding the size limit. Please upload a file lessthan 5MB";
                    }
                }
                else
                {
                    ViewBag.errorMessage = "Please upload the following type of files: image/jpeg,image/gif,image/tiff,image/png,image/jpg";
                }
            }
            else
            {
                storyBlock.storyBlockLocation = StoryBlock_Temp.storyBlockLocation;
                if (ModelState.IsValid)
                {
                    db.Entry(storyBlock).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            ViewBag.collageId = new SelectList(db.Collages, "collageId", "collageName", storyBlock.collageId);
            TempData.Remove("StoryBlock_Temp");
            return View(storyBlock);
        }

        // GET: StoryBlocks/Delete/5
        [Authorize(Roles = "developer")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StoryBlock storyBlock = db.StoryBlocks.Find(id);
            ViewBag.archival_stat = storyBlock.archivalStatus;
            if (storyBlock == null)
            {
                return HttpNotFound();
            }
            return View(storyBlock);
        }

        // POST: StoryBlocks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "developer")]
        public ActionResult DeleteConfirmed(int id)
        {
            StoryBlock storyBlock = db.StoryBlocks.Find(id);
            db.StoryBlocks.Remove(storyBlock);
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
