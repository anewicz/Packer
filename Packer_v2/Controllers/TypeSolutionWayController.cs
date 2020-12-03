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
    public class TypeSolutionWayController : Controller
    {
        private PackerContext db = new PackerContext();

        // GET: TypeSolutionWay
        public ActionResult Index()
        {
            var typeSolutionDefaultWay = db.TypeSolutionDefaultWay.Include(t => t.TypeSolution).Include(t => t.WayType);
            return View(typeSolutionDefaultWay.ToList());
        }

        // GET: TypeSolutionWay/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TypeSolutionWay typeSolutionWay = db.TypeSolutionDefaultWay.Find(id);
            if (typeSolutionWay == null)
            {
                return HttpNotFound();
            }
            return View(typeSolutionWay);
        }

        // GET: TypeSolutionWay/Create
        public ActionResult Create()
        {
            ViewBag.IdTypeSolution = new SelectList(db.TypeSolution, "IdTypeSolution", "NmTypeSolution");
            ViewBag.IdWayType = new SelectList(db.WayType, "IdWayType", "NmProject");
            return View();
        }

        // POST: TypeSolutionWay/Create
        // Para proteger-se contra ataques de excesso de postagem, ative as propriedades específicas às quais deseja se associar. 
        // Para obter mais detalhes, confira https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdTypeSolutionDefaultWay,IdTypeSolution,IdWayType")] TypeSolutionWay typeSolutionWay)
        {
            if (ModelState.IsValid)
            {
                db.TypeSolutionDefaultWay.Add(typeSolutionWay);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdTypeSolution = new SelectList(db.TypeSolution, "IdTypeSolution", "NmTypeSolution", typeSolutionWay.IdTypeSolution);
            ViewBag.IdWayType = new SelectList(db.WayType, "IdWayType", "NmProject", typeSolutionWay.IdWayType);
            return View(typeSolutionWay);
        }

        // GET: TypeSolutionWay/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TypeSolutionWay typeSolutionWay = db.TypeSolutionDefaultWay.Find(id);
            if (typeSolutionWay == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdTypeSolution = new SelectList(db.TypeSolution, "IdTypeSolution", "NmTypeSolution", typeSolutionWay.IdTypeSolution);
            ViewBag.IdWayType = new SelectList(db.WayType, "IdWayType", "NmProject", typeSolutionWay.IdWayType);
            return View(typeSolutionWay);
        }

        // POST: TypeSolutionWay/Edit/5
        // Para proteger-se contra ataques de excesso de postagem, ative as propriedades específicas às quais deseja se associar. 
        // Para obter mais detalhes, confira https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdTypeSolutionDefaultWay,IdTypeSolution,IdWayType")] TypeSolutionWay typeSolutionWay)
        {
            if (ModelState.IsValid)
            {
                db.Entry(typeSolutionWay).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdTypeSolution = new SelectList(db.TypeSolution, "IdTypeSolution", "NmTypeSolution", typeSolutionWay.IdTypeSolution);
            ViewBag.IdWayType = new SelectList(db.WayType, "IdWayType", "NmProject", typeSolutionWay.IdWayType);
            return View(typeSolutionWay);
        }

        // GET: TypeSolutionWay/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TypeSolutionWay typeSolutionWay = db.TypeSolutionDefaultWay.Find(id);
            if (typeSolutionWay == null)
            {
                return HttpNotFound();
            }
            return View(typeSolutionWay);
        }

        // POST: TypeSolutionWay/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            TypeSolutionWay typeSolutionWay = db.TypeSolutionDefaultWay.Find(id);
            db.TypeSolutionDefaultWay.Remove(typeSolutionWay);
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
