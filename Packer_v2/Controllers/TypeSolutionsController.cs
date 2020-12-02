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
    public class TypeSolutionsController : Controller
    {
        private PackerContext db = new PackerContext();

        // GET: TypeSolutions
        public ActionResult Index()
        {
            return View(db.TypeSolution.ToList());
        }

        // GET: TypeSolutions/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TypeSolution typeSolution = db.TypeSolution.Find(id);
            if (typeSolution == null)
            {
                return HttpNotFound();
            }
            return View(typeSolution);
        }

        // GET: TypeSolutions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TypeSolutions/Create
        // Para proteger-se contra ataques de excesso de postagem, ative as propriedades específicas às quais deseja se associar. 
        // Para obter mais detalhes, confira https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdTypeSolution,NmTypeSolution")] TypeSolution typeSolution)
        {
            if (ModelState.IsValid)
            {
                db.TypeSolution.Add(typeSolution);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(typeSolution);
        }

        // GET: TypeSolutions/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TypeSolution typeSolution = db.TypeSolution.Find(id);
            if (typeSolution == null)
            {
                return HttpNotFound();
            }
            return View(typeSolution);
        }

        // POST: TypeSolutions/Edit/5
        // Para proteger-se contra ataques de excesso de postagem, ative as propriedades específicas às quais deseja se associar. 
        // Para obter mais detalhes, confira https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdTypeSolution,NmTypeSolution")] TypeSolution typeSolution)
        {
            if (ModelState.IsValid)
            {
                db.Entry(typeSolution).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(typeSolution);
        }

        // GET: TypeSolutions/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TypeSolution typeSolution = db.TypeSolution.Find(id);
            if (typeSolution == null)
            {
                return HttpNotFound();
            }
            return View(typeSolution);
        }

        // POST: TypeSolutions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            TypeSolution typeSolution = db.TypeSolution.Find(id);
            db.TypeSolution.Remove(typeSolution);
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
