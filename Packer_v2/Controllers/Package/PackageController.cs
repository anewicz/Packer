using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Packer_v2.Context;
using Packer_v2.Models;

namespace Packer_v2.Controllers
{
    public class PackageController : Controller
    {
        private PackerContext db = new PackerContext();

        #region PackageIndex
        public ActionResult PackageList()
        {
            var ticket = db.Ticket.Include(t => t.Solution).Include(t => t.Status).OrderByDescending(x => x.IdTicket);
            return View(ticket.ToList());
        }
        #endregion 

        #region CreateOrEditView

        // GET: Package
        public ActionResult CreateOrEdit(long? idTicket)
        {
            var vm = new PackageViewModel();

            if (idTicket > 0)
                vm = this.Edit(idTicket);
            else
                vm = this.Create();


            return View("CreateOrEdit", vm);
        }
        #endregion 

        #region CreateParcialView
        private PackageViewModel Create()
        {
            var vm = new PackageViewModel();
            vm.EpsList = db.Eps.ToList();
            vm.Projects = new List<Project>();
            vm.Solutions = new List<Solution>();
            vm.Dtbases = new List<Dtbase>();
            vm.Querys = new List<Query>();
            vm.Ticket = new Ticket();
            vm.Ticket.DtRegister = DateTime.Now;
            vm.StatusList = db.Status.ToList();

            var queryList = db.Query.ToList();
            vm.Querys = queryList;

            ViewBag.HasQuerysTicket = 1;

            return vm;
        }
        #endregion

        #region  EditParcialView

        private PackageViewModel Edit(long? idTicket)
        {
            var vm = new PackageViewModel();
            vm.Ticket = db.Ticket.Where(x => x.IdTicket == idTicket).FirstOrDefault();
            vm.EpsList = db.Eps.ToList();
            vm.StatusList = db.Status.ToList();
            vm.Dtbases = new List<Dtbase>();

            Int64 idSolutionSel = db.Solution.Where(x => x.IdSolution == vm.Ticket.IdSolution).Select(x => x.IdSolution).FirstOrDefault();
            Int64 idProjectSel = db.Project.Where(x => x.IdProject == idSolutionSel).Select(x => x.IdProject).FirstOrDefault();
            Int64 idEpsSel = db.Eps.Where(x => x.IdEps == idProjectSel).Select(x => x.IdEps).FirstOrDefault();

            vm.Projects = db.Project.Where(x => x.IdEps == idEpsSel).ToList();
            vm.Solutions = db.Solution.Where(x => x.IdSolution == idSolutionSel).ToList();
            vm.Querys = db.Query.Where(x => x.IdTicket == vm.Ticket.IdTicket).ToList();

            var queryList = db.Query.ToList();
            vm.Querys = queryList;

            var querysCount = db.Query.Where(x => x.IdTicket == vm.Ticket.IdTicket).Count();

            ViewBag.SolutionSelect = idSolutionSel > 0 ? idSolutionSel : 0;
            ViewBag.ProjectSelect = idProjectSel > 0 ? idProjectSel : 0;
            ViewBag.EpsSelect = idEpsSel > 0 ? idEpsSel : 0;
            ViewBag.HasQuerysTicket = querysCount > 0 ? 1 : 0;

            return vm;
        }

        #endregion

        #region SaveTicketInsertBD

        [HttpPost]
        public JsonResult SaveTicket(Ticket ticket)
        {
            ticket.DtLastModification = DateTime.Now;

            try
            {
                if (ticket.IdTicket == 0 || ticket.IdTicket == null)
                {
                    ticket.DtRegister = DateTime.Now;
                    db.Ticket.Add(ticket);
                    db.SaveChanges();
                }
                else
                {
                    db.Entry(ticket).State = EntityState.Modified;
                    db.SaveChanges();
                }

                var vm = new PackageViewModel();
                vm.Ticket = ticket;

                var partial = PartialView("_IdTicket", vm).RenderToString();

                return Json(new { status = "OK", description = "Ticket Salvo com Sucesso!", partialView = partial }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                ViewBag.MsgSavePackage = "Deu ruim!! Olhá só quirida ..." + ex.Message.ToString();
                return Json(new { status = "NOK", description = "Erro ao Salvar - Exception: " + ex.Message.ToString() + " | InnerException" + ex.InnerException.InnerException.ToString() + " | StackTrace" + ex.StackTrace.ToString(), IdTicket = 0 }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion


        #region ProjetcsDpDwnLists
        public JsonResult GetProjectsByEps(int pIdEps)
        {
            var projects = db.Project.Where(x => x.IdEps == pIdEps).ToList();
            var vm = new PackageViewModel();
            vm.Projects = projects;

            var partial = PartialView("_GetProjectsByEpsDropDownList", vm).RenderToString();

            return Json(new { status = "OK", description = "Sucesso!", partialView = partial }, JsonRequestBehavior.AllowGet);
        }
        #endregion 

        #region SolutionsDpDwnLists
        public JsonResult GetSolutionsByProject(int pIdProject)
        {
            var solutions = db.Solution.Where(x => x.IdProject == pIdProject).ToList();
            var vm = new PackageViewModel();
            vm.Solutions = solutions;

            var partial = PartialView("_GetSolutionsByProjectDropDownList", vm).RenderToString();

            return Json(new { status = "OK", description = "Sucesso!", partialView = partial }, JsonRequestBehavior.AllowGet);
        }

        #endregion 

        #region DataBasesDpDwnLists
        public JsonResult GetDatabasesBySolution(int pIdSolution)
        {

            var IdsSolutions = db.DbSolution.Where(x => x.IdSolution == pIdSolution).ToList();

            var _Dtbases = new List<Dtbase>();
            foreach (var s in IdsSolutions)
            {
                var inserir = db.Dtbase.Where(x => x.IdDtbase == s.IdDtbase).FirstOrDefault();
                _Dtbases.Add(inserir);
            }

            var vm = new PackageViewModel();
            vm.Dtbases = _Dtbases;

            var partial = PartialView("_GetDatabasesBySolutionDropDownList", vm).RenderToString();

            return Json(new { status = "OK", description = "Sucesso!", partialView = partial }, JsonRequestBehavior.AllowGet);
        }

        #endregion 


        #region QuerysParcialView
        [HttpPost]
        public JsonResult InsertAndReturnPartialQuerys(List<Query> querys)
        {
            Int64 IdTicket = 0;

            foreach (var query in querys)
            {
                db.Query.Add(query);
                db.SaveChanges();
                IdTicket = query.IdTicket;
            }

            ViewBag.SaveSqlMensage = "Arquivos adicionados com sucesso.";

            var vm = new PackageViewModel();
            vm.Ticket = db.Ticket.Where(x => x.IdTicket == IdTicket).FirstOrDefault();
            vm.Querys = db.Query.Where(x => x.IdTicket == IdTicket).ToList();
            vm.Query = new Query();

            var partial = PartialView("_GetQuerysList", vm).RenderToString();

            return Json(new { status = "OK", description = "Querys Adicionadas com Sucesso!", partialView = partial }, JsonRequestBehavior.AllowGet);

        }

        #endregion CarregarParcialQuerys

        #region CopiarQuerysPasta
        [HttpPost]
        public JsonResult UploadQuerys(UploadFileResult _pFiles, string idTicket)
        {
            List<Query> TemporaryQuerys = new List<Query>();

            try
            {
                string fileName = "";
                string sendingFiles = "";

                var count = 0;
                foreach (var file in _pFiles.File)
                {
                    if (file.ContentLength > 0)
                    {
                        var Query = new Query();

                        var dirPath = Server.MapPath("~/App_Data/Packages/Querys");
                        string pathQuerysSave = verifyIfCreateDirectory(dirPath, idTicket);

                        fileName = Path.GetFileName(file.FileName);
                        var way = pathQuerysSave + fileName;//Path.Combine(Server.MapPath("~/App_Data/Packages/Querys"), fileName);
                        file.SaveAs(way);
                        count++;

                        Query.IdQuery = count;
                        Query.IsActive = true;
                        Query.NmFile = fileName;

                        TemporaryQuerys.Add(Query);
                    }

                    sendingFiles = sendingFiles + " , " + fileName;
                }
                ViewBag.Mensage = "Arquivos enviados  : " + sendingFiles + " , com sucesso.";
            }
            catch (Exception ex)
            {
                ViewBag.Mensage = "Erro : " + ex.Message;
                return Json(new { status = "NOK", description = "Erro ao Salvar - Exception: " + ex.Message.ToString() + " | InnerException" + ex.InnerException.InnerException.ToString() + " | StackTrace" + ex.StackTrace.ToString(), IdTicket = 0 }, JsonRequestBehavior.AllowGet);
            }

            Int64 _IdTicket = Int64.Parse(idTicket);
            Int64 _IdSolution = db.Ticket.Where(x => x.IdTicket == _IdTicket).Select(x => x.IdSolution).FirstOrDefault();


            var vm = new PackageViewModel();
            vm.Querys = TemporaryQuerys;
            vm.IdSolution = _IdSolution;

            var partial = PartialView("_ManageQuerys", vm).RenderToString();

            return Json(new { status = "OK", description = "Arquivos Adicionados com Sucesso!", partialView = partial }, JsonRequestBehavior.AllowGet);
        }

        #endregion CopiarQuerysPasta

        #region UrlSalvarQuerys

        public string verifyIfCreateDirectory(string nomeDiretorio, string idTicket)
        {
            nomeDiretorio += "\\Ticket_" + idTicket + "\\";

            if (!Directory.Exists(nomeDiretorio))
                Directory.CreateDirectory(nomeDiretorio);

            return nomeDiretorio;
        }

        #endregion

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

