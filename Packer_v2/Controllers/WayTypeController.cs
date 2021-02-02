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
    public class WayTypeController : Controller
    {
        private PackerContext db = new PackerContext();

        // GET: WayType
        [Authorize(Roles = "View")]
        public ActionResult Index()
        {
            var wayType = db.WayType.Include(w => w.Folder);
            return View(wayType.ToList());
        }

        // GET: WayType/Details/5
        [Authorize(Roles = "View")]
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WayType wayType = db.WayType.Find(id);
            if (wayType == null)
            {
                return HttpNotFound();
            }
            return View(wayType);
        }

        // GET: WayType/Create
        [Authorize(Roles = "View")]
        public ActionResult Create()
        {
            ViewBag.IdFolder = new SelectList(db.Folder, "IdFolder", "NmFolder");
            return View();
        }

        // POST: WayType/Create
        // Para proteger-se contra ataques de excesso de postagem, ative as propriedades específicas às quais deseja se associar. 
        // Para obter mais detalhes, confira https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "View")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdWayType,IsWayDev,NmProject,IdFolder")] WayType wayType)
        {
            if (ModelState.IsValid)
            {
                db.WayType.Add(wayType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdFolder = new SelectList(db.Folder, "IdFolder", "NmFolder", wayType.IdFolder);
            return View(wayType);
        }

        // GET: WayType/Edit/5
        [Authorize(Roles = "Edit")]
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WayType wayType = db.WayType.Find(id);
            if (wayType == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdFolder = new SelectList(db.Folder, "IdFolder", "NmFolder", wayType.IdFolder);
            return View(wayType);
        }

        // POST: WayType/Edit/5
        // Para proteger-se contra ataques de excesso de postagem, ative as propriedades específicas às quais deseja se associar. 
        // Para obter mais detalhes, confira https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdWayType,IsWayDev,NmProject,IdFolder")] WayType wayType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(wayType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdFolder = new SelectList(db.Folder, "IdFolder", "NmFolder", wayType.IdFolder);
            return View(wayType);
        }

        // GET: WayType/Delete/5
        [Authorize(Roles = "Delete")]
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WayType wayType = db.WayType.Find(id);
            if (wayType == null)
            {
                return HttpNotFound();
            }
            return View(wayType);
        }

        // POST: WayType/Delete/5
        [Authorize(Roles = "Delete")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            WayType wayType = db.WayType.Find(id);
            db.WayType.Remove(wayType);
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
