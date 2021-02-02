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
    public class DtbaseController : Controller
    {
        private PackerContext db = new PackerContext();

        // GET: Dtbase
        [Authorize(Roles = "View")]
        public ActionResult Index()
        {
            var dtbase = db.Dtbase.Include(d => d.DevDbIp).Include(d => d.PrdDbIp);
            return View(dtbase.ToList());
        }

        // GET: Dtbase/Details/5
        [Authorize(Roles = "View")]
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dtbase dtbase = db.Dtbase.Find(id);
            if (dtbase == null)
            {
                return HttpNotFound();
            }
            return View(dtbase);
        }

        // GET: Dtbase/Create
        [Authorize(Roles = "Create")]
        public ActionResult Create()
        {
            ViewBag.IdDevDbIp = new SelectList(db.DbIp, "IdDbIp", "NmIp");
            ViewBag.IdPrdDbIp = new SelectList(db.DbIp, "IdDbIp", "NmIp");
            return View();
        }

        // POST: Dtbase/Create
        // Para proteger-se contra ataques de excesso de postagem, ative as propriedades específicas às quais deseja se associar. 
        // Para obter mais detalhes, confira https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdDtbase,IsActive,IsCore,IdDevDbIp,NmDevDatabase,IdPrdDbIp,NmPrdDatabase")] Dtbase dtbase)
        {
            if (ModelState.IsValid)
            {
                db.Dtbase.Add(dtbase);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdDevDbIp = new SelectList(db.DbIp, "IdDbIp", "NmIp", dtbase.IdDevDbIp);
            ViewBag.IdPrdDbIp = new SelectList(db.DbIp, "IdDbIp", "NmIp", dtbase.IdPrdDbIp);
            return View(dtbase);
        }

        // GET: Dtbase/Edit/5
        [Authorize(Roles = "Edit")]
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dtbase dtbase = db.Dtbase.Find(id);
            if (dtbase == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdDevDbIp = new SelectList(db.DbIp, "IdDbIp", "NmIp", dtbase.IdDevDbIp);
            ViewBag.IdPrdDbIp = new SelectList(db.DbIp, "IdDbIp", "NmIp", dtbase.IdPrdDbIp);
            return View(dtbase);
        }

        // POST: Dtbase/Edit/5
        // Para proteger-se contra ataques de excesso de postagem, ative as propriedades específicas às quais deseja se associar. 
        // Para obter mais detalhes, confira https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdDtbase,IsActive,IsCore,IdDevDbIp,NmDevDatabase,IdPrdDbIp,NmPrdDatabase")] Dtbase dtbase)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dtbase).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdDevDbIp = new SelectList(db.DbIp, "IdDbIp", "NmIp", dtbase.IdDevDbIp);
            ViewBag.IdPrdDbIp = new SelectList(db.DbIp, "IdDbIp", "NmIp", dtbase.IdPrdDbIp);
            return View(dtbase);
        }

        // GET: Dtbase/Delete/5
        [Authorize(Roles = "Delete")]
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dtbase dtbase = db.Dtbase.Find(id);
            if (dtbase == null)
            {
                return HttpNotFound();
            }
            return View(dtbase);
        }

        // POST: Dtbase/Delete/5
        [Authorize(Roles = "Delete")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Dtbase dtbase = db.Dtbase.Find(id);
            db.Dtbase.Remove(dtbase);
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
