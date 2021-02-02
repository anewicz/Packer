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
    public class EpsController : Controller
    {
        private PackerContext db = new PackerContext();

        // GET: Eps
        [Authorize(Roles = "View")]
        public ActionResult Index()
        {
            return View(db.Eps.ToList());
        }

        // GET: Eps/Details/5
        [Authorize(Roles = "View")]
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Eps eps = db.Eps.Find(id);
            if (eps == null)
            {
                return HttpNotFound();
            }
            return View(eps);
        }

        // GET: Eps/Create
        [Authorize(Roles = "Create")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Eps/Create
        // Para proteger-se contra ataques de excesso de postagem, ative as propriedades específicas às quais deseja se associar. 
        // Para obter mais detalhes, confira https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdEps,NmEps,WayStartup")] Eps eps)
        {
            if (ModelState.IsValid)
            {
                db.Eps.Add(eps);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(eps);
        }

        // GET: Eps/Edit/5
        [Authorize(Roles = "Edit")]
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Eps eps = db.Eps.Find(id);
            if (eps == null)
            {
                return HttpNotFound();
            }
            return View(eps);
        }

        // POST: Eps/Edit/5
        // Para proteger-se contra ataques de excesso de postagem, ative as propriedades específicas às quais deseja se associar. 
        // Para obter mais detalhes, confira https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdEps,NmEps,WayStartup")] Eps eps)
        {
            if (ModelState.IsValid)
            {
                db.Entry(eps).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(eps);
        }

        // GET: Eps/Delete/5
        [Authorize(Roles = "Delete")]
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Eps eps = db.Eps.Find(id);
            if (eps == null)
            {
                return HttpNotFound();
            }
            return View(eps);
        }

        // POST: Eps/Delete/5
        [Authorize(Roles = "Delete")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Eps eps = db.Eps.Find(id);
            db.Eps.Remove(eps);
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
