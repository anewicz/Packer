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
    public class FoldersController : Controller
    {
        private PackerContext db = new PackerContext();

        // GET: Folders
        public ActionResult Index()
        {
            return View(db.Folders.ToList());
        }

        // GET: Folders/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Folder folder = db.Folders.Find(id);
            if (folder == null)
            {
                return HttpNotFound();
            }
            return View(folder);
        }

        // GET: Folders/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Folders/Create
        // Para proteger-se contra ataques de excesso de postagem, ative as propriedades específicas às quais deseja se associar. 
        // Para obter mais detalhes, confira https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdFolder,NmFolder,DeFolder")] Folder folder)
        {
            if (ModelState.IsValid)
            {
                db.Folders.Add(folder);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(folder);
        }

        // GET: Folders/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Folder folder = db.Folders.Find(id);
            if (folder == null)
            {
                return HttpNotFound();
            }
            return View(folder);
        }

        // POST: Folders/Edit/5
        // Para proteger-se contra ataques de excesso de postagem, ative as propriedades específicas às quais deseja se associar. 
        // Para obter mais detalhes, confira https://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: Folders/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Folder folder = db.Folders.Find(id);
            if (folder == null)
            {
                return HttpNotFound();
            }
            return View(folder);
        }

        // POST: Folders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Folder folder = db.Folders.Find(id);
            db.Folders.Remove(folder);
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
