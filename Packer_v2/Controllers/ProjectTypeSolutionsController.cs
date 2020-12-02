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
    public class ProjectTypeSolutionsController : Controller
    {
        private PackerContext db = new PackerContext();

        // GET: ProjectTypeSolutions
        public ActionResult Index()
        {
            var projectTypeIntegrations = db.ProjectTypeIntegrations.Include(p => p.Project).Include(p => p.TypeSolution);
            return View(projectTypeIntegrations.ToList());
        }

        // GET: ProjectTypeSolutions/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectTypeSolution projectTypeSolution = db.ProjectTypeIntegrations.Find(id);
            if (projectTypeSolution == null)
            {
                return HttpNotFound();
            }
            return View(projectTypeSolution);
        }

        // GET: ProjectTypeSolutions/Create
        public ActionResult Create()
        {
            ViewBag.IdProject = new SelectList(db.Project, "IdProject", "NmProject");
            ViewBag.IdTypeSolution = new SelectList(db.TypeSolution, "IdTypeSolution", "NmTypeSolution");
            return View();
        }

        // POST: ProjectTypeSolutions/Create
        // Para proteger-se contra ataques de excesso de postagem, ative as propriedades específicas às quais deseja se associar. 
        // Para obter mais detalhes, confira https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdProjectTypeIntegration,NmTypeIntegration,DeIntegration,IdTypeSolution,IdProject")] ProjectTypeSolution projectTypeSolution)
        {
            if (ModelState.IsValid)
            {
                db.ProjectTypeIntegrations.Add(projectTypeSolution);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdProject = new SelectList(db.Project, "IdProject", "NmProject", projectTypeSolution.IdProject);
            ViewBag.IdTypeSolution = new SelectList(db.TypeSolution, "IdTypeSolution", "NmTypeSolution", projectTypeSolution.IdTypeSolution);
            return View(projectTypeSolution);
        }

        // GET: ProjectTypeSolutions/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectTypeSolution projectTypeSolution = db.ProjectTypeIntegrations.Find(id);
            if (projectTypeSolution == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdProject = new SelectList(db.Project, "IdProject", "NmProject", projectTypeSolution.IdProject);
            ViewBag.IdTypeSolution = new SelectList(db.TypeSolution, "IdTypeSolution", "NmTypeSolution", projectTypeSolution.IdTypeSolution);
            return View(projectTypeSolution);
        }

        // POST: ProjectTypeSolutions/Edit/5
        // Para proteger-se contra ataques de excesso de postagem, ative as propriedades específicas às quais deseja se associar. 
        // Para obter mais detalhes, confira https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdProjectTypeIntegration,NmTypeIntegration,DeIntegration,IdTypeSolution,IdProject")] ProjectTypeSolution projectTypeSolution)
        {
            if (ModelState.IsValid)
            {
                db.Entry(projectTypeSolution).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdProject = new SelectList(db.Project, "IdProject", "NmProject", projectTypeSolution.IdProject);
            ViewBag.IdTypeSolution = new SelectList(db.TypeSolution, "IdTypeSolution", "NmTypeSolution", projectTypeSolution.IdTypeSolution);
            return View(projectTypeSolution);
        }

        // GET: ProjectTypeSolutions/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectTypeSolution projectTypeSolution = db.ProjectTypeIntegrations.Find(id);
            if (projectTypeSolution == null)
            {
                return HttpNotFound();
            }
            return View(projectTypeSolution);
        }

        // POST: ProjectTypeSolutions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            ProjectTypeSolution projectTypeSolution = db.ProjectTypeIntegrations.Find(id);
            db.ProjectTypeIntegrations.Remove(projectTypeSolution);
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
