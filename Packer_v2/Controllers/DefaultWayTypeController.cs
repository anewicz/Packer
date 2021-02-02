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
    public class DefaultWayTypeController : Controller
    {
        private PackerContext db = new PackerContext();

        // GET: DefaultWayType
        [Authorize(Roles = "View")]
        public ActionResult Index()
        {
            var defaultWayType = db.DefaultWayType.Include(d => d.TypeSolution).Include(d => d.WayType);
            return View(defaultWayType.ToList());
        }

        // GET: DefaultWayType/Details/5
        [Authorize(Roles = "View")]
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DefaultWayType defaultWayType = db.DefaultWayType.Find(id);
            if (defaultWayType == null)
            {
                return HttpNotFound();
            }
            return View(defaultWayType);
        }

        // GET: DefaultWayType/Create
        [Authorize(Roles = "Create")]
        public ActionResult Create()
        {
            ViewBag.IdTypeSolution = new SelectList(db.TypeSolution, "IdTypeSolution", "NmTypeSolution");
            ViewBag.IdWayType = new SelectList(db.WayType, "IdWayType", "NmProject");
            return View();
        }

        // POST: DefaultWayType/Create
        // Para proteger-se contra ataques de excesso de postagem, ative as propriedades específicas às quais deseja se associar. 
        // Para obter mais detalhes, confira https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdDefaultWayType,NmDefaultWayPatch,DefaultWayPath,RemoteIp,IdTypeSolution,IdWayType")] DefaultWayType defaultWayType)
        {
            if (ModelState.IsValid)
            {
                db.DefaultWayType.Add(defaultWayType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdTypeSolution = new SelectList(db.TypeSolution, "IdTypeSolution", "NmTypeSolution", defaultWayType.IdTypeSolution);
            ViewBag.IdWayType = new SelectList(db.WayType, "IdWayType", "NmProject", defaultWayType.IdWayType);
            return View(defaultWayType);
        }

        // GET: DefaultWayType/Edit/5
        [Authorize(Roles = "Edit")]
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DefaultWayType defaultWayType = db.DefaultWayType.Find(id);
            if (defaultWayType == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdTypeSolution = new SelectList(db.TypeSolution, "IdTypeSolution", "NmTypeSolution", defaultWayType.IdTypeSolution);
            ViewBag.IdWayType = new SelectList(db.WayType, "IdWayType", "NmProject", defaultWayType.IdWayType);
            return View(defaultWayType);
        }

        // POST: DefaultWayType/Edit/5
        // Para proteger-se contra ataques de excesso de postagem, ative as propriedades específicas às quais deseja se associar. 
        // Para obter mais detalhes, confira https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdDefaultWayType,NmDefaultWayPatch,DefaultWayPath,RemoteIp,IdTypeSolution,IdWayType")] DefaultWayType defaultWayType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(defaultWayType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdTypeSolution = new SelectList(db.TypeSolution, "IdTypeSolution", "NmTypeSolution", defaultWayType.IdTypeSolution);
            ViewBag.IdWayType = new SelectList(db.WayType, "IdWayType", "NmProject", defaultWayType.IdWayType);
            return View(defaultWayType);
        }

        // GET: DefaultWayType/Delete/5
        [Authorize(Roles = "Delete")]
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DefaultWayType defaultWayType = db.DefaultWayType.Find(id);
            if (defaultWayType == null)
            {
                return HttpNotFound();
            }
            return View(defaultWayType);
        }

        // POST: DefaultWayType/Delete/5
        [Authorize(Roles = "Delete")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            DefaultWayType defaultWayType = db.DefaultWayType.Find(id);
            db.DefaultWayType.Remove(defaultWayType);
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
