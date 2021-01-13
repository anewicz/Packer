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
using Xceed.Document.NET;
using Xceed.Words.NET;

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

        #region SaveQuerysParcialView
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

                VerifyIfCreateDirectory(ticket.IdTicket.ToString(), "Logs");

                return Json(new { status = "OK", description = "Ticket Salvo com Sucesso!", partialView = partial }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                ViewBag.MsgSavePackage = "Deu ruim!! Olhá só quirida ..." + ex.Message.ToString();

                VerifyIfCreateDirectory(ticket.IdTicket.ToString(), "Logs");
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



        #region UploadQuerysPasta
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

                        string pathQuerysSave = VerifyIfCreateDirectory(idTicket, "Querys");

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

        #endregion 

        #region CriarRetornarPastas

        public string VerifyIfCreateDirectory(string idTicket, string Complement, bool MustDropAndRecreate = false)
        {
            var nomeDiretorio = Server.MapPath("~/App_Data/Packages") + "\\IdTicket_" + idTicket + "\\" + Complement + "\\";

            try
            {
                if (MustDropAndRecreate && Directory.Exists(nomeDiretorio))
                {
                    Directory.Delete(nomeDiretorio, true);
                }

                if (!Directory.Exists(nomeDiretorio))
                    Directory.CreateDirectory(nomeDiretorio);
            }
            catch { return nomeDiretorio; }

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
            string _packageFolder = VerifyIfCreateDirectory(_pPackage.ticket.IdTicket.ToString(), "Pacote\\Database", true);
            string _packageFolderRb = VerifyIfCreateDirectory(_pPackage.ticket.IdTicket.ToString(), "Pacote\\Database_Rollback", true);
            string _packageQuerys = VerifyIfCreateDirectory(_pPackage.ticket.IdTicket.ToString(), "Querys");
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

        #region GerarDocx
        public void CreateDocx(Package _pPackage)
        {
            string _ModelsFolder = Server.MapPath("~/App_Data/Models/Image");
            string _PackageFolder = VerifyIfCreateDirectory(_pPackage.ticket.IdTicket.ToString(), "Pacote");

            using (var docX = DocX.Create($"{_PackageFolder}\\Roteiro_{_pPackage.ticket.NmTicket}.docx"))
            {
                int margin = 30;
                docX.MarginLeft = margin + 10;
                docX.MarginFooter = margin;
                docX.MarginRight = margin;
                docX.MarginTop = margin;


                var image = docX.AddImage($"{_ModelsFolder}\\logo_color.png");
                // altura e largura da imagem.
                var picture = image.CreatePicture();
                var maxWidth = Convert.ToInt32(docX.PageWidth - docX.MarginLeft - docX.MarginRight);
                if (picture.Width > maxWidth)
                {
                    var ratio = (double)maxWidth / (double)picture.Width;
                    picture.Width = maxWidth;
                    picture.Height = Convert.ToInt32(picture.Height * ratio);
                }

                var p = docX.InsertParagraph();
                p.AppendPicture(picture).Alignment = Alignment.center;

                docX.InsertParagraph("Instrução Para Deploy - Produção").Color(System.Drawing.Color.FromArgb(43)).FontSize(16).Font("Arial").Alignment = Alignment.center;

                var tb1_generalInfo = docX.InsertParagraph();
                tb1_generalInfo.LineSpacingAfter = 5;
                tb1_generalInfo.LineSpacingBefore = 5;
                var table1 = tb1_generalInfo.InsertTableAfterSelf(11, 4);
                table1.AutoFit = AutoFit.Window;
                table1.Alignment = Alignment.both;

                table1.Rows[0].MergeCells(0, 3);
                InsertDfParagraphDocx(table1, 0, $"INFORMAÇÕES SOBRE A MUDANÇA", false, _BoldTipe: "bold");
                InsertDfParagraphDocx(table1, 1, $"EPS:|{_pPackage.ticket.Solution.Project.Eps.NmEps}");
                InsertDfParagraphDocx(table1, 2, $"Ticket*:|{_pPackage.ticket.NmTicket}|Descrição:|{_pPackage.ticket.DeNote}", false);
                InsertDfParagraphDocx(table1, 3, $"Objetivo*:|{_pPackage.ticket.DeNote}");
                InsertDfParagraphDocx(table1, 4, $"Impacto*:|{_pPackage.ticket.DeImpact}");
                InsertDfParagraphDocx(table1, 5, $"Risco de Não Fazer*:|{_pPackage.ticket.DeRiskOfNotDoing}");
                InsertDfParagraphDocx(table1, 6, $"Risco de Fazer*:|{_pPackage.ticket.DeRiskOfDoing}");
                InsertDfParagraphDocx(table1, 7, $"Contingência*:|{_pPackage.ticket.DeContingency}");
                InsertDfParagraphDocx(table1, 8, $"Pré-Requisitos:|{_pPackage.ticket.DePrerequisites}");
                InsertDfParagraphDocx(table1, 9, $"Indisponibilidade*:|{_pPackage.ticket.DeUnavailability}");
                InsertDfParagraphDocx(table1, 10, $"Tempo de Execução*:|{_pPackage.ticket.DeRuntime}");
                InsertBorderParagraphDocx(table1);



                var tb2_generalInfo = docX.InsertParagraph();
                tb2_generalInfo.LineSpacingAfter = 0;
                tb2_generalInfo.LineSpacingBefore = 0;
                var table2 = tb2_generalInfo.InsertTableAfterSelf(3, 4);
                table2.AutoFit = AutoFit.Window;
                table2.Alignment = Alignment.both;

                table2.Rows[0].MergeCells(0, 3);
                InsertDfParagraphDocx(table2, 0, $"PESSOAS ENVOLVIDAS NA MUDANÇA", false, _BoldTipe: "bold");
                InsertDfParagraphDocx(table2, 1, $"NOME|ÁREA|E-MAIL|TELEFONE", false, _BoldTipe: "bold");
                InsertDfParagraphDocx(table2, 2, $"Dayane Michalewicz|Full Dev Application|dayane.rodrigues@code7.com|(48) 3298.7429", false, _BoldTipe: "normal");
                InsertBorderParagraphDocx(table2);



                var tb3_generalInfo = docX.InsertParagraph();
                tb3_generalInfo.LineSpacingAfter = 0;
                tb3_generalInfo.LineSpacingBefore = 0;
                var table3 = tb3_generalInfo.InsertTableAfterSelf(4, 4);
                table3.AutoFit = AutoFit.Window;
                table3.Alignment = Alignment.both;

                table3.Rows[0].MergeCells(0, 3);
                InsertDfParagraphDocx(table3, 0, $"PASTAS ENVOLVIDAS", false, _BoldTipe: "bold");
                InsertDfParagraphDocx(table3, 1, $"NOME|DESCRIÇÃO", _BoldTipe: "bold");
                InsertDfParagraphDocx(table3, 2, $"Database|Contém scripts SQL.");
                InsertDfParagraphDocx(table3, 3, $"FileServer|Contém os arquivos da atualização do CRM.");
                InsertBorderParagraphDocx(table3);



                var tb4_generalInfo = docX.InsertParagraph();
                tb4_generalInfo.LineSpacingAfter = 0;
                tb4_generalInfo.LineSpacingBefore = 0;
                var table4 = tb4_generalInfo.InsertTableAfterSelf(4, 4);
                table4.AutoFit = AutoFit.Window;
                table4.Alignment = Alignment.both;

                table4.Rows[0].MergeCells(0, 3);
                InsertDfParagraphDocx(table4, 0, $"ROTEIRO DE IMPLANTAÇÃO", false, _BoldTipe: "bold");
                InsertDfParagraphDocx(table4, 1, $"Atv. 1:|No servidor 172.28.200.77 \nExecutar todos scripts presentes na pasta DataBase, na ordem sequencial em que se encontram:");
                InsertDfParagraphDocx(table4, 2, $"Atv. 2:|Contém scripts SQL.");
                InsertDfParagraphDocx(table4, 3, $"FileServer|Contém os arquivos da atualização do CRM.");
                InsertBorderParagraphDocx(table4);


                //// Terceiro parágrafo (multiplas formatações)
                //var paragrafo3 = docX.InsertParagraph();
                //paragrafo3.LineSpacingAfter = 8;
                //paragrafo3.Append("Um pedaço da frase normal, ");
                //var negrito = paragrafo3.Append("outro pedaço negrito, ");
                //negrito.Bold();
                //var sublinhado = paragrafo3.Append("outro sublinhado");
                //sublinhado.UnderlineStyle(UnderlineStyle.singleLine);


                docX.Save();
            }

        }
        #endregion

        #region MetodosDocx
        public void InsertBorderParagraphDocx(Table table)
        {
            table.SetBorder(TableBorderType.Top, new Border());
            table.SetBorder(TableBorderType.Bottom, new Border());
            table.SetBorder(TableBorderType.Left, new Border());
            table.SetBorder(TableBorderType.Right, new Border());
            table.SetBorder(TableBorderType.InsideH, new Border());
            table.SetBorder(TableBorderType.InsideV, new Border());
        }

        public void InsertDfParagraphDocx(Table table, int _NRow, string _pText, bool _MustMerge = true, int _FontSize = 10, int _FromArgb = 23, string _FontFamily = "Calibri", string _BoldTipe = "interlayer")
        {
            var texts = _pText.Split('|');
            if (_MustMerge)
                table.Rows[_NRow].MergeCells(1, 3);

            for (int i = 0; i < texts.Length; i++)
            {
                if (_BoldTipe == "interlayer")
                {
                    if (i % 2 == 0)
                        table.Rows[_NRow].Cells[i].InsertParagraph(texts[i]).Bold().FontSize(_FontSize).Font(_FontFamily).Color(System.Drawing.Color.FromArgb(_FromArgb)).Alignment = Alignment.right;
                    else
                        table.Rows[_NRow].Cells[i].InsertParagraph(texts[i]).FontSize(_FontSize).Font(_FontFamily).Color(System.Drawing.Color.FromArgb(_FromArgb)).Alignment = Alignment.left;
                }
                else if (_BoldTipe == "normal")
                    table.Rows[_NRow].Cells[i].InsertParagraph(texts[i]).FontSize(_FontSize).Font(_FontFamily).Color(System.Drawing.Color.FromArgb(_FromArgb)).Alignment = Alignment.center;
                else if (_BoldTipe == "bold")
                    table.Rows[_NRow].Cells[i].InsertParagraph(texts[i]).Bold().FontSize(_FontSize).Font(_FontFamily).Color(System.Drawing.Color.FromArgb(_FromArgb)).Alignment = Alignment.center;

            }
        }
        #endregion 



        #region FinalizarPacote
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
                var _packageFolder = VerifyIfCreateDirectory(idTicket.ToString(), "Pacote", true);

                //verifyIfCreateDirectory(idTicket.ToString(), "Pacote\\ApplicationServer\\", true);
                //verifyIfCreateDirectory(idTicket.ToString(), "Pacote\\FileServer\\", true);
                //verifyIfCreateDirectory(idTicket.ToString(), "Pacote\\Evidences\\", true);

                CreateTicketTemplate(package, _packageFolder);

                CreateSqlQuerys(package);
                CreateDocx(package);

                return Json(new { status = "OK", description = "Pacote Gerado com Sucesso!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                VerifyIfCreateDirectory(idTicket.ToString(), "Logs");
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


