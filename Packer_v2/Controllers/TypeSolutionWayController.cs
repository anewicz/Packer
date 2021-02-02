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
        [Authorize(Roles = "View")]
        public ActionResult Index()
        {
            var typeSolutiontWay = db.TypeSolutiontWay.Include(t => t.TypeSolution).Include(t => t.WayType);
            return View(typeSolutiontWay.ToList());
        }

        // GET: TypeSolutionWay/Details/5
        [Authorize(Roles = "View")]
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TypeSolutionWay typeSolutionWay = db.TypeSolutiontWay.Find(id);
            if (typeSolutionWay == null)
            {
                return HttpNotFound();
            }
            return View(typeSolutionWay);
        }

        // GET: TypeSolutionWay/Create
        [Authorize(Roles = "Create")]
        public ActionResult Create()
        {
            ViewBag.IdTypeSolution = new SelectList(db.TypeSolution, "IdTypeSolution", "NmTypeSolution");
            ViewBag.IdWayType = new SelectList(db.WayType, "IdWayType", "NmProject");
            return View();
        }

        // POST: TypeSolutionWay/Create
        // Para proteger-se contra ataques de excesso de postagem, ative as propriedades específicas às quais deseja se associar. 
        // Para obter mais detalhes, confira https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdTypeSolutionDefaultWay,IdTypeSolution,IdWayType")] TypeSolutionWay typeSolutionWay)
        {
            if (ModelState.IsValid)
            {
                db.TypeSolutiontWay.Add(typeSolutionWay);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdTypeSolution = new SelectList(db.TypeSolution, "IdTypeSolution", "NmTypeSolution", typeSolutionWay.IdTypeSolution);
            ViewBag.IdWayType = new SelectList(db.WayType, "IdWayType", "NmProject", typeSolutionWay.IdWayType);
            return View(typeSolutionWay);
        }

        // GET: TypeSolutionWay/Edit/5
        [Authorize(Roles = "Edit")]
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TypeSolutionWay typeSolutionWay = db.TypeSolutiontWay.Find(id);
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
        [Authorize(Roles = "Edit")]
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
        [Authorize(Roles = "Delete")]
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TypeSolutionWay typeSolutionWay = db.TypeSolutiontWay.Find(id);
            if (typeSolutionWay == null)
            {
                return HttpNotFound();
            }
            return View(typeSolutionWay);
        }

        // POST: TypeSolutionWay/Delete/5
        [Authorize(Roles = "Delete")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            TypeSolutionWay typeSolutionWay = db.TypeSolutiontWay.Find(id);
            db.TypeSolutiontWay.Remove(typeSolutionWay);
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
