using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Packer_v2.Context;
using Packer_v2.Models;

namespace Packer_v2.Controllers
{
    public class FolderController : Controller
    {
        private PackerContext db = new PackerContext();

        // GET: Folder
        [Authorize(Roles = "View")]
        public ActionResult Index()
        {
            return View(db.Folder.ToList());
        }

        // GET: Folder/Details/5
        [Authorize(Roles = "View")]
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Folder folder = db.Folder.Find(id);
            if (folder == null)
            {
                return HttpNotFound();
            }
            return View(folder);
        }

        // GET: Folder/Create
        [Authorize(Roles = "Create")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Folder/Create
        // Para proteger-se contra ataques de excesso de postagem, ative as propriedades específicas às quais deseja se associar. 
        // Para obter mais detalhes, confira https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdFolder,NmFolder,DeFolder")] Folder folder)
        {
            if (ModelState.IsValid)
            {
                db.Folder.Add(folder);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(folder);
        }

        // GET: Folder/Edit/5
        [Authorize(Roles = "Edit")]
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Folder folder = db.Folder.Find(id);
            if (folder == null)
            {
                return HttpNotFound();
            }
            return View(folder);
        }

        // POST: Folder/Edit/5
        // Para proteger-se contra ataques de excesso de postagem, ative as propriedades específicas às quais deseja se associar. 
        // Para obter mais detalhes, confira https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdFolder,NmFolder,DeFolder")] Folder folder)
        {
            if (ModelState.IsValid)
            {
                db.Entry(folder).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(folder);
        }

        // GET: Folder/Delete/5
        [Authorize(Roles = "Delete")]
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Folder folder = db.Folder.Find(id);
            if (folder == null)
            {
                return HttpNotFound();
            }
            return View(folder);
        }

        // POST: Folder/Delete/5
        [Authorize(Roles = "Delete")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Folder folder = db.Folder.Find(id);
            db.Folder.Remove(folder);
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
