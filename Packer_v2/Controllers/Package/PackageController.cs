using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
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

            Int64 idSolutionSel = vm.Ticket.IdSolution;
            Int64 idProjectSel = db.Solution.Where(x => x.IdSolution == idSolutionSel).Select(x => x.IdProject).FirstOrDefault();
            Int64 idEpsSel = db.Project.Where(x => x.IdProject == idProjectSel).Select(x => x.IdEps).FirstOrDefault();

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

                verifyIfCreateDirectory(ticket.IdTicket.ToString(), "Logs");

                return Json(new { status = "OK", description = "Ticket Salvo com Sucesso!", partialView = partial }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                ViewBag.MsgSavePackage = "Deu ruim!! Olhá só quirida ..." + ex.Message.ToString();

                verifyIfCreateDirectory(ticket.IdTicket.ToString(), "Logs");
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

                        string pathQuerysSave = verifyIfCreateDirectory(idTicket, "Querys");

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

        #region CaminhosGerarLocalizar

        public string verifyIfCreateDirectory(string idTicket, string Complement, bool MustDropAndRecreate = false)
        {
            var nomeDiretorio = Server.MapPath("~/App_Data/Packages") + "\\Ticket_" + idTicket + "\\" + Complement + "\\";

            if (MustDropAndRecreate && Directory.Exists(nomeDiretorio))
            {
                Directory.Delete(nomeDiretorio, true);
            }

            if (!Directory.Exists(nomeDiretorio))
                Directory.CreateDirectory(nomeDiretorio);

            return nomeDiretorio;
        }

        #endregion

        #region GerarTxtTicketTemplate
        public void CreateTicketTemplate(Package _pPackage, string _pFolder)
        {

            string txtTemplate = "h2. Envio Homologação Técnica \n\n| *Impacto na versão em produção:*| Não |\n| *Deploy pode ser feito em operação:*| Não |\n| *Período para Deploy:*| Manhã Noite |\n";
            txtTemplate += $"\n|*O que foi feito:*| {_pPackage.ticket.DeNote} |";
            txtTemplate += $"\n|*Observações:*| {_pPackage.ticket.DeNote} |";
            txtTemplate += $"\n\n";
            txtTemplate += $"\n|*Versão do CRM:*| pendente |";
            txtTemplate += $"\n|*Versão do Bin Studio:*| pendente |";
            txtTemplate += $"\n|*Caminho do TTC:*| {_pPackage.ticket.Solution.Project.WayPatch} |";
            txtTemplate += $"\n|*Caminho do FrontEnd:*| {_pPackage.ticket.Solution.DevPathFrontEnd} |";
            txtTemplate += $"\n|*Caminho do WCF:*| {_pPackage.ticket.Solution.DevPathWcf} |";
            txtTemplate += $"\n|*Base:*| {_pPackage.dtbase.Where(x => x.IsCore == false).Select(x => x.NmDevDatabase).FirstOrDefault()} |";
            txtTemplate += $"\n|*Id Projeto:*| {_pPackage.ticket.Solution.Project.IdProjectAyty} |";
            txtTemplate += $"\n|*Caminho da startup:*| {_pPackage.ticket.Solution.Project.Eps.WayStartup} |";
            txtTemplate += $"\n\n";
            txtTemplate += $"\n|*Caminho do pacote:*| pendente |";

            using (StreamWriter file = new StreamWriter(_pFolder + "\\TemplateChamado.txt", false))
            {
                file.WriteLine(txtTemplate);
                file.Close();
            }
        }

        #endregion

        #region GerarSQLQuerys
        public void CreateSqlQuerys(Package _pPackage)
        {
            string _packageFolder = verifyIfCreateDirectory(_pPackage.ticket.IdTicket.ToString(), "Pacote\\Database", true);
            string _packageFolderRb = verifyIfCreateDirectory(_pPackage.ticket.IdTicket.ToString(), "Pacote\\Database_Rollback", true);
            string _packageQuerys = verifyIfCreateDirectory(_pPackage.ticket.IdTicket.ToString(), "Querys");
            string baseCore = _pPackage.dtbase.Where(x => x.IsCore == true).Select(x => x.NmDevDatabase).FirstOrDefault();
            string objects = string.Empty;
            int counter = 0;

            foreach (var qr in _pPackage.querys)
            {
                counter++;
                using (StreamWriter file = new StreamWriter($"{_packageFolder}\\{_pPackage.ticket.NmTicket.ToUpper()}_{counter.ToString("000")}_{qr.IdSqlComandType}_{qr.IdSqlItenType}_{qr.NmSqlObject.ToUpper()}.sql", false))
                {
                    string conteudo = $"USE {qr.Dtbase.NmPrdDatabase} \nGO\n\n";
                    conteudo += System.IO.File.ReadAllText($"{_packageQuerys}{qr.NmFile}", Encoding.GetEncoding("iso-8859-15"));
                    conteudo = conteudo.ToUpper();

                    file.WriteLine(conteudo);
                    file.Close();

                    objects += "," + qr.NmSqlObject;
                }

            }


            using (StreamWriter file = new StreamWriter($"{_packageFolder}\\{_pPackage.ticket.NmTicket.ToUpper()}_000_CREATE_PACKAGE_LOG.sql", false))
            {
                string conteudo = System.IO.File.ReadAllText($"{Server.MapPath("~/App_Data/Models/Querys")}\\Model_000_CREATE_PACKAGE_LOG.txt", Encoding.GetEncoding("iso-8859-15"));
                conteudo = conteudo.Replace("@COREDB@", baseCore);
                conteudo = conteudo.Replace("@OBJECTS@", objects.Substring(1, (objects.Length - 1)));
                conteudo = conteudo.Replace("@IDPROJECT@", _pPackage.ticket.Solution.Project.IdProjectAyty.ToString());
                conteudo = conteudo.Replace("@NUTICKET@", _pPackage.ticket.NmTicket.ToString());
                conteudo = conteudo.Replace("@OBSERVATION@", _pPackage.ticket.DeNote.ToString());
                conteudo = conteudo.ToUpper();

                file.WriteLine(conteudo);
                file.Close();
            }


            using (StreamWriter file = new StreamWriter($"{_packageFolder}\\{_pPackage.ticket.NmTicket.ToUpper()}_999_UPDATE_PACKAGE_LOG.sql", false))
            {
                string conteudo = System.IO.File.ReadAllText($"{Server.MapPath("~/App_Data/Models/Querys")}\\Model_999_UPDATE_PACKAGE_LOG.txt", Encoding.GetEncoding("iso-8859-15"));
                conteudo = conteudo.Replace("@COREDB@", baseCore);
                conteudo = conteudo.Replace("@OBJECTS@", objects.Substring(1, (objects.Length - 1)));
                conteudo = conteudo.Replace("@IDPROJECT@", _pPackage.ticket.Solution.Project.IdProjectAyty.ToString());
                conteudo = conteudo.Replace("@NUTICKET@", _pPackage.ticket.NmTicket.ToString());
                conteudo = conteudo.Replace("@OBSERVATION@", _pPackage.ticket.DeNote.ToString());
                conteudo = conteudo.ToUpper();

                file.WriteLine(conteudo);
                file.Close();
            }


        }
        #endregion

        #region GerarPacote
        [HttpPost]
        public JsonResult GeneratePackage(Int64 idTicket)
        {

            Package package = new Package();
            package.ticket = db.Ticket.Where(x => x.IdTicket == idTicket).FirstOrDefault();
            package.querys = db.Query.Where(x => x.IdTicket == idTicket).ToList();
            package.dtbase = new List<Dtbase>();

            var dbSolution = db.DbSolution.Where(x => x.IdSolution == package.ticket.IdSolution).ToList();

            foreach (var s in dbSolution)
            {
                package.dtbase.Add(db.Dtbase.Where(x => x.IdDtbase == s.IdDtbase).FirstOrDefault());
            }

            try
            {
                var _packageFolder = verifyIfCreateDirectory(idTicket.ToString(), "Pacote", true);

                //verifyIfCreateDirectory(idTicket.ToString(), "Pacote\\ApplicationServer\\", true);
                //verifyIfCreateDirectory(idTicket.ToString(), "Pacote\\FileServer\\", true);
                //verifyIfCreateDirectory(idTicket.ToString(), "Pacote\\Evidences\\", true);

                CreateTicketTemplate(package, _packageFolder);


                CreateSqlQuerys(package);

                return Json(new { status = "OK", description = "Pacote Gerado com Sucesso!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                verifyIfCreateDirectory(idTicket.ToString(), "Logs");
                return Json(new { status = "NOK", description = "Erro ao Gerar Pacote- Exception: " + ex.Message.ToString() + " | InnerException" + ex.InnerException.InnerException.ToString() + " | StackTrace" + ex.StackTrace.ToString(), IdTicket = 0 }, JsonRequestBehavior.AllowGet);
            }
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

