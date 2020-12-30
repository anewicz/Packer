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


        // GET: Eps
        public ActionResult Index()
        {
            var vm = this.GetPackage();
            return View("Index", vm);
        }

        private PackageViewModel GetPackage()
        {
            var vm = new PackageViewModel();
            vm.EpsList = db.Eps.ToList();
            vm.Projects = new List<Project>();
            vm.Solutions = new List<Solution>();
            vm.Dtbases = new List<Dtbase>();
            vm.Querys = new List<Query>();

            var queryList = db.Queries.ToList();
            vm.Querys = queryList;

            return vm;
        }

        public JsonResult GetProjectsByEps(int pIdEps)
        {
            var projects = db.Project.Where(x => x.idEps == pIdEps).ToList();
            var vm = new PackageViewModel();
            vm.Projects = projects;

            var partial = PartialView("_GetProjectsByEpsDropDownList", vm).RenderToString();

            return Json(new { status = "ok", partialView = partial }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult InsertAndReturnPartialQuerys(List<Query> querys)
        {

            foreach (var query in querys)
            {
                db.Queries.Add(query);
                db.SaveChanges();
            }

            ViewBag.SaveSqlMensage = "Arquivos adicionados com sucesso.";

            var queryList = db.Queries.ToList();
            var vm = new PackageViewModel();
            vm.Querys = queryList;
            vm.Query = new Query();

            var partial = PartialView("_GetQuerysList", vm).RenderToString();

            return Json(new { status = "ok", partialView = partial }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetSolutionsByProject(int pIdProject)
        {
            var solutions = db.Solution.Where(x => x.IdProject == pIdProject).ToList();
            var vm = new PackageViewModel();
            vm.Solutions = solutions;

            var partial = PartialView("_GetSolutionsByProjectDropDownList", vm).RenderToString();

            return Json(new { status = "ok", partialView = partial }, JsonRequestBehavior.AllowGet);
        }

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

            return Json(new { status = "ok", partialView = partial }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UploadQuerys(UploadFileResult _pFiles)
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

                        fileName = Path.GetFileName(file.FileName);
                        var way = Path.Combine(Server.MapPath("~/App_Data/Packages/Querys"), fileName);
                        file.SaveAs(way);
                        count++;

                        Query.IdQuery = count;
                        Query.IsActive = true;
                        Query.NmFile = fileName;

                        TemporaryQuerys.Add(Query);
                    }

                    sendingFiles = sendingFiles + " , " + fileName;
                }
                ViewBag.Mensagem = "Arquivos enviados  : " + sendingFiles + " , com sucesso.";
            }
            catch (Exception ex)
            {
                ViewBag.Mensagem = "Erro : " + ex.Message;
            }


            var vm = new PackageViewModel();
            vm.Querys = TemporaryQuerys;

            var partial = PartialView("_GetQuerys", vm).RenderToString();

            return Json(new { status = "ok", partialView = partial }, JsonRequestBehavior.AllowGet);
        }


    }
}
