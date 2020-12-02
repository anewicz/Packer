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
    public class WayTypesController : Controller
    {
        private PackerContext db = new PackerContext();

        // GET: WayTypes
        public ActionResult Index()
        {
            var wayTypes = db.WayTypes.Include(w => w.Folder);
            return View(wayTypes.ToList());
        }

        // GET: WayTypes/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WayType wayType = db.WayTypes.Find(id);
            if (wayType == null)
            {
                return HttpNotFound();
            }
            return View(wayType);
        }

        // GET: WayTypes/Create
        public ActionResult Create()
        {
            ViewBag.IdFolder = new SelectList(db.Folders, "IdFolder", "NmFolder");
            return View();
        }

        // POST: WayTypes/Create
        // Para proteger-se contra ataques de excesso de postagem, ative as propriedades específicas às quais deseja se associar. 
        // Para obter mais detalhes, confira https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdWayType,NmProject,IdFolder")] WayType wayType)
        {
            if (ModelState.IsValid)
            {
                db.WayTypes.Add(wayType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdFolder = new SelectList(db.Folders, "IdFolder", "NmFolder", wayType.IdFolder);
            return View(wayType);
        }

        // GET: WayTypes/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WayType wayType = db.WayTypes.Find(id);
            if (wayType == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdFolder = new SelectList(db.Folders, "IdFolder", "NmFolder", wayType.IdFolder);
            return View(wayType);
        }

        // POST: WayTypes/Edit/5
        // Para proteger-se contra ataques de excesso de postagem, ative as propriedades específicas às quais deseja se associar. 
        // Para obter mais detalhes, confira https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdWayType,NmProject,IdFolder")] WayType wayType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(wayType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdFolder = new SelectList(db.Folders, "IdFolder", "NmFolder", wayType.IdFolder);
            return View(wayType);
        }

        // GET: WayTypes/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WayType wayType = db.WayTypes.Find(id);
            if (wayType == null)
            {
                return HttpNotFound();
            }
            return View(wayType);
        }

        // POST: WayTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            WayType wayType = db.WayTypes.Find(id);
            db.WayTypes.Remove(wayType);
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
