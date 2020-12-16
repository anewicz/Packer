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
    public class TicketController : Controller
    {
        private PackerContext db = new PackerContext();
        private Ticket selectedTicket = new Ticket();

        // GET: Ticket
        public ActionResult Index()
        {
            var ticket = db.Ticket.Include(t => t.Solution);
            return View(ticket.ToList());
        }

        // GET: Ticket/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Ticket.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // GET: Ticket/Create
        public ActionResult Create()
        {
            ViewBag.IdSolution = new SelectList(db.Solution, "IdSolution", "NmTSolution");
            return View();
        }

        // POST: Ticket/Create
        // Para proteger-se contra ataques de excesso de postagem, ative as propriedades específicas às quais deseja se associar. 
        // Para obter mais detalhes, confira https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdTicket,DtRegister,DtLastModification,TicketLink,NmTicket,IdSolution,DeTicket,DeNote,DeImpact,DeRiskOfNotDoing,DeRiskOfDoing,DeContingency,DePrerequisites,DeUnavailability,DeRuntime")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                if (ticket.DtRegister == null || ticket.DtRegister == DateTime.MinValue)
                    ticket.DtRegister = DateTime.Now;

                ticket.DtLastModification = DateTime.Now;

                db.Ticket.Add(ticket);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdSolution = new SelectList(db.Solution, "IdSolution", "NmTSolution", ticket.IdSolution);
            return View(ticket);
        }

        // GET: Ticket/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Ticket.Find(id);
            selectedTicket = ticket;
            if (ticket == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdSolution = new SelectList(db.Solution, "IdSolution", "NmTSolution", ticket.IdSolution);
            return View(ticket);
        }

        // POST: Ticket/Edit/5
        // Para proteger-se contra ataques de excesso de postagem, ative as propriedades específicas às quais deseja se associar. 
        // Para obter mais detalhes, confira https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdTicket,DtRegister,DtLastModification,TicketLink,NmTicket,IdSolution,DeTicket,DeNote,DeImpact,DeRiskOfNotDoing,DeRiskOfDoing,DeContingency,DePrerequisites,DeUnavailability,DeRuntime")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                ticket.DtLastModification = DateTime.Now;
       //         ticket.DtRegister =  ;

                db.Entry(ticket).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdSolution = new SelectList(db.Solution, "IdSolution", "NmTSolution", ticket.IdSolution);
            return View(ticket);
        }

        // GET: Ticket/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Ticket.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // POST: Ticket/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Ticket ticket = db.Ticket.Find(id);
            db.Ticket.Remove(ticket);
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
