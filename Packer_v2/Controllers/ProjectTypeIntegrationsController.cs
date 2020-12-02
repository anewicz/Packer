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
    public class ProjectTypeIntegrationsController : Controller
    {
        private PackerContext db = new PackerContext();

        // GET: ProjectTypeIntegrations
        public ActionResult Index()
        {
            var projectTypeIntegrations = db.ProjectTypeIntegrations.Include(p => p.Project).Include(p => p.TypeIntegration);
            return View(projectTypeIntegrations.ToList());
        }

        // GET: ProjectTypeIntegrations/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectTypeIntegration projectTypeIntegration = db.ProjectTypeIntegrations.Find(id);
            if (projectTypeIntegration == null)
            {
                return HttpNotFound();
            }
            return View(projectTypeIntegration);
        }

        // GET: ProjectTypeIntegrations/Create
        public ActionResult Create()
        {
            ViewBag.IdProject = new SelectList(db.Project, "IdProject", "NmProject");
            ViewBag.IdTypeIntegration = new SelectList(db.TypeIntegrations, "IdTypeIntegration", "NmTypeIntegration");
            return View();
        }

        // POST: ProjectTypeIntegrations/Create
        // Para proteger-se contra ataques de excesso de postagem, ative as propriedades específicas às quais deseja se associar. 
        // Para obter mais detalhes, confira https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdProjectTypeIntegration,NmTypeIntegration,DeIntegration,IdTypeIntegration,IdProject")] ProjectTypeIntegration projectTypeIntegration)
        {
            if (ModelState.IsValid)
            {
                db.ProjectTypeIntegrations.Add(projectTypeIntegration);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdProject = new SelectList(db.Project, "IdProject", "NmProject", projectTypeIntegration.IdProject);
            ViewBag.IdTypeIntegration = new SelectList(db.TypeIntegrations, "IdTypeIntegration", "NmTypeIntegration", projectTypeIntegration.IdTypeIntegration);
            return View(projectTypeIntegration);
        }

        // GET: ProjectTypeIntegrations/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectTypeIntegration projectTypeIntegration = db.ProjectTypeIntegrations.Find(id);
            if (projectTypeIntegration == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdProject = new SelectList(db.Project, "IdProject", "NmProject", projectTypeIntegration.IdProject);
            ViewBag.IdTypeIntegration = new SelectList(db.TypeIntegrations, "IdTypeIntegration", "NmTypeIntegration", projectTypeIntegration.IdTypeIntegration);
            return View(projectTypeIntegration);
        }

        // POST: ProjectTypeIntegrations/Edit/5
        // Para proteger-se contra ataques de excesso de postagem, ative as propriedades específicas às quais deseja se associar. 
        // Para obter mais detalhes, confira https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdProjectTypeIntegration,NmTypeIntegration,DeIntegration,IdTypeIntegration,IdProject")] ProjectTypeIntegration projectTypeIntegration)
        {
            if (ModelState.IsValid)
            {
                db.Entry(projectTypeIntegration).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdProject = new SelectList(db.Project, "IdProject", "NmProject", projectTypeIntegration.IdProject);
            ViewBag.IdTypeIntegration = new SelectList(db.TypeIntegrations, "IdTypeIntegration", "NmTypeIntegration", projectTypeIntegration.IdTypeIntegration);
            return View(projectTypeIntegration);
        }

        // GET: ProjectTypeIntegrations/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectTypeIntegration projectTypeIntegration = db.ProjectTypeIntegrations.Find(id);
            if (projectTypeIntegration == null)
            {
                return HttpNotFound();
            }
            return View(projectTypeIntegration);
        }

        // POST: ProjectTypeIntegrations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            ProjectTypeIntegration projectTypeIntegration = db.ProjectTypeIntegrations.Find(id);
            db.ProjectTypeIntegrations.Remove(projectTypeIntegration);
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
