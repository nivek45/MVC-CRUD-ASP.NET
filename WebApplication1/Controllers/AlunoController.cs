using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using OfficeOpenXml;
using System.Drawing;

namespace WebApplication1.Controllers
{
    public class AlunoController : Controller
    {
        public ActionResult Index() => RedirectToAction("Listar");


        public ActionResult Exibir(int id)
        {
            return View((Session["ListaAluno"] as List<Aluno>).ElementAt(id));
        }
        public ActionResult Listar()
        {
            Aluno.GerarLista(Session);
            return View(Session["ListaAluno"] as List<Aluno>);
        }

        public ActionResult Create() => View(new Aluno());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Aluno aluno)
        {
            if (ModelState.IsValid)
            {
                aluno.Adicionar(Session);
                return RedirectToAction("Listar");
            }
            return View(aluno);
        }

        public ActionResult Edit(int id)
        {
            return View(Aluno.Procurar(Session, id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Aluno aluno)
        {
            if (ModelState.IsValid)
            {
                aluno.Editar(Session, id);
                return RedirectToAction("Listar");
            }
            return View(aluno);
        }

        [HttpPost]
        public ActionResult DeleteAjax(int id)
        {
            var aluno = Aluno.Procurar(Session, id);
            if (aluno != null)
            {
                aluno.Excluir(Session);
                return Json(new { sucesso = true });
            }
            return new HttpStatusCodeResult(404, "Aluno não encontrado");
        }

        public ActionResult DownloadPdf()
        {
            var lista = Session["ListaAluno"] as List<Aluno>;
            if (lista == null || !lista.Any())
                return RedirectToAction("Listar");

            using (MemoryStream ms = new MemoryStream())
            {
                Document doc = new Document(PageSize.A4);
                PdfWriter.GetInstance(doc, ms);
                doc.Open();
                doc.Add(new Paragraph("Lista de Alunos"));
                doc.Add(new Paragraph(" "));

                foreach (var aluno in lista)
                {
                    doc.Add(new Paragraph($"Nome: {aluno.Nome} - Email: {aluno.Email} - Nascimento: {aluno.DataNascimento.ToShortDateString()}"));
                }

                doc.Close();
                return File(ms.ToArray(), "application/pdf", "ListaDeAlunos.pdf");
            }
        }

        public ActionResult DownloadExcel()
        {
            

            var lista = Session["ListaAluno"] as List<Aluno>;
            if (lista == null || !lista.Any())
                return RedirectToAction("Listar");

            using (var pacote = new ExcelPackage())
            {
                var planilha = pacote.Workbook.Worksheets.Add("Alunos");
                planilha.Cells[1, 1].Value = "Nome";
                planilha.Cells[1, 2].Value = "Email";
                planilha.Cells[1, 3].Value = "Data de Nascimento";

                planilha.Row(1).Style.Font.Bold = true;
                planilha.Row(1).Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                planilha.Row(1).Style.Fill.BackgroundColor.SetColor(Color.LightGray);

                for (int i = 0; i < lista.Count; i++)
                {
                    var aluno = lista[i];
                    planilha.Cells[i + 2, 1].Value = aluno.Nome;
                    planilha.Cells[i + 2, 2].Value = aluno.Email;
                    planilha.Cells[i + 2, 3].Value = aluno.DataNascimento.ToShortDateString();
                }

                planilha.Cells.AutoFitColumns();
                return File(pacote.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Alunos.xlsx");
            }
        }
    }
}
