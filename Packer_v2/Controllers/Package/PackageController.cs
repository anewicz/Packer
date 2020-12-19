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

        public JsonResult GetSolutionsByProject(int pIdProject)
        {
            var solutions = db.Solution.Where(x => x.IdProject == pIdProject).ToList();
            var vm = new PackageViewModel();
            vm.Solutions = solutions;

            var partial = PartialView("_GetSolutionsByProjectDropDownList", vm).RenderToString();

            return Json(new { status = "ok", partialView = partial }, JsonRequestBehavior.AllowGet);
        }


    }
}
