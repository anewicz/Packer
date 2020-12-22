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
    public class DbSolutionController : Controller
    {
        private PackerContext db = new PackerContext();

        // GET: DbSolution
        public ActionResult Index()
        {
            var dbSolution = db.DbSolution.Include(d => d.Dtbase).Include(d => d.Solution);
            return View(dbSolution.ToList());
        }

        // GET: DbSolution/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DbSolution dbSolution = db.DbSolution.Find(id);
            if (dbSolution == null)
            {
                return HttpNotFound();
            }
            return View(dbSolution);
        }

        // GET: DbSolution/Create
        public ActionResult Create()
        {
            ViewBag.IdDtbase = new SelectList(db.Dtbase, "IdDtbase", "FullNmDatabase");
            ViewBag.IdSolution = new SelectList(db.Solution, "IdSolution", "FullNmSolution");
            return View();
        }

        // POST: DbSolution/Create
        // Para proteger-se contra ataques de excesso de postagem, ative as propriedades específicas às quais deseja se associar. 
        // Para obter mais detalhes, confira https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdDbSolution,IdDtbase,IdSolution")] DbSolution dbSolution)
        {
            if (ModelState.IsValid)
            {
                db.DbSolution.Add(dbSolution);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdDtbase = new SelectList(db.Dtbase, "IdDtbase", "FullNmDatabase", dbSolution.IdDtbase);
            ViewBag.IdSolution = new SelectList(db.Solution, "IdSolution", "FullNmSolution", dbSolution.IdSolution);
            return View(dbSolution);
        }

        // GET: DbSolution/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DbSolution dbSolution = db.DbSolution.Find(id);
            if (dbSolution == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdDtbase = new SelectList(db.Dtbase, "IdDtbase", "FullNmDatabase", dbSolution.IdDtbase);
            ViewBag.IdSolution = new SelectList(db.Solution, "IdSolution", "FullNmSolution", dbSolution.IdSolution);
            return View(dbSolution);
        }

        // POST: DbSolution/Edit/5
        // Para proteger-se contra ataques de excesso de postagem, ative as propriedades específicas às quais deseja se associar. 
        // Para obter mais detalhes, confira https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdDbSolution,IdDtbase,IdSolution")] DbSolution dbSolution)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dbSolution).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdDtbase = new SelectList(db.Dtbase, "IdDtbase", "FullNmDatabase", dbSolution.IdDtbase);
            ViewBag.IdSolution = new SelectList(db.Solution, "IdSolution", "FullNmSolution", dbSolution.IdSolution);
            return View(dbSolution);
        }

        // GET: DbSolution/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DbSolution dbSolution = db.DbSolution.Find(id);
            if (dbSolution == null)
            {
                return HttpNotFound();
            }
            return View(dbSolution);
        }

        // POST: DbSolution/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            DbSolution dbSolution = db.DbSolution.Find(id);
            db.DbSolution.Remove(dbSolution);
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
