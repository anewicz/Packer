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
    public class SolutionController : Controller
    {
        private PackerContext db = new PackerContext();

        // GET: Solution
        [Authorize(Roles = "View")]
        public ActionResult Index()
        {
            var solution = db.Solution.Include(s => s.Project).Include(s => s.TypeSolution);
            return View(solution.ToList());
        }

        // GET: Solution/Details/5
        [Authorize(Roles = "View")]
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Solution solution = db.Solution.Find(id);
            if (solution == null)
            {
                return HttpNotFound();
            }
            return View(solution);
        }

        // GET: Solution/Create
        [Authorize(Roles = "Create")]
        public ActionResult Create()
        {
            ViewBag.IdProject = new SelectList(db.Project, "IdProject", "NmProject");
            ViewBag.IdTypeSolution = new SelectList(db.TypeSolution, "IdTypeSolution", "NmTypeSolution");
            return View();
        }

        // POST: Solution/Create
        // Para proteger-se contra ataques de excesso de postagem, ative as propriedades específicas às quais deseja se associar. 
        // Para obter mais detalhes, confira https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdSolution,NmSolution,DeSolution,DevPathFrontEnd,DevPathWcf,IdTypeSolution,IdProject,DtRegister,DtLastModification")] Solution solution)
        {
            solution.DtRegister = DateTime.Now;
            solution.DtLastModification = DateTime.Now;

            if (ModelState.IsValid)
            {
                db.Solution.Add(solution);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdProject = new SelectList(db.Project, "IdProject", "NmProject", solution.IdProject);
            ViewBag.IdTypeSolution = new SelectList(db.TypeSolution, "IdTypeSolution", "NmTypeSolution", solution.IdTypeSolution);
            return View(solution);
        }

        // GET: Solution/Edit/5
        [Authorize(Roles = "Edit")]
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Solution solution = db.Solution.Find(id);
            if (solution == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdProject = new SelectList(db.Project, "IdProject", "NmProject", solution.IdProject);
            ViewBag.IdTypeSolution = new SelectList(db.TypeSolution, "IdTypeSolution", "NmTypeSolution", solution.IdTypeSolution);
            return View(solution);
        }

        // POST: Solution/Edit/5
        // Para proteger-se contra ataques de excesso de postagem, ative as propriedades específicas às quais deseja se associar. 
        // Para obter mais detalhes, confira https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdSolution,NmSolution,DeSolution,DevPathFrontEnd,DevPathWcf,IdTypeSolution,IdProject,DtRegister,DtLastModification")] Solution solution)
        {
            solution.DtRegister = db.Solution.Where(x => x.IdSolution == solution.IdSolution).Select(x => x.DtRegister).FirstOrDefault();
            solution.DtLastModification = DateTime.Now;

            if (ModelState.IsValid)
            {
                db.Entry(solution).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdProject = new SelectList(db.Project, "IdProject", "NmProject", solution.IdProject);
            ViewBag.IdTypeSolution = new SelectList(db.TypeSolution, "IdTypeSolution", "NmTypeSolution", solution.IdTypeSolution);
            return View(solution);
        }

        // GET: Solution/Delete/5
        [Authorize(Roles = "Delete")]
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Solution solution = db.Solution.Find(id);
            if (solution == null)
            {
                return HttpNotFound();
            }
            return View(solution);
        }

        // POST: Solution/Delete/5
        [Authorize(Roles = "Delete")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Solution solution = db.Solution.Find(id);
            db.Solution.Remove(solution);
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
