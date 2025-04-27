using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using OfficeOpenXml;
using System.Drawing;

namespace WebApplication1.Controllers
{
    public class EventoController : Controller
    {
        public ActionResult Index() => RedirectToAction("Listar");

        public ActionResult Listar()
        {
            Evento.GerarLista(Session);
            return View(Session["ListaEvento"] as List<Evento>);
        }

        public ActionResult Create() => View(new Evento());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Evento evento)
        {
            if (ModelState.IsValid)
            {
                evento.Adicionar(Session);
                return RedirectToAction("Listar");
            }
            return View(evento);
        }

        public ActionResult Edit(int id)
        {
            return View(Evento.Procurar(Session, id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Evento evento)
        {
            if (ModelState.IsValid)
            {
                evento.Editar(Session, id);
                return RedirectToAction("Listar");
            }
            return View(evento);
        }

        public ActionResult Exibir(int id)
        {
            return View(Evento.Procurar(Session, id));
        }

        [HttpPost]
        public ActionResult DeleteAjax(int id)
        {
            var evento = Evento.Procurar(Session, id);
            if (evento != null)
            {
                evento.Excluir(Session);
                return Json(new { sucesso = true });
            }
            return new HttpStatusCodeResult(404, "Evento não encontrado");
        }

        public ActionResult DownloadPdf()
        {
            var lista = Session["ListaEvento"] as List<Evento>;
            if (lista == null || !lista.Any())
                return RedirectToAction("Listar");

            using (MemoryStream ms = new MemoryStream())
            {
                Document doc = new Document(PageSize.A4);
                PdfWriter.GetInstance(doc, ms);
                doc.Open();
                doc.Add(new Paragraph("Lista de Eventos"));
                doc.Add(new Paragraph(" "));

                foreach (var evento in lista)
                {
                    doc.Add(new Paragraph($"Local: {evento.Local} - Data: {evento.Data.ToShortDateString()}"));
                }

                doc.Close();
                return File(ms.ToArray(), "application/pdf", "ListaDeEventos.pdf");
            }
        }

        public ActionResult DownloadExcel()
        {
            var lista = Session["ListaEvento"] as List<Evento>;
            if (lista == null || !lista.Any())
                return RedirectToAction("Listar");

            using (var pacote = new ExcelPackage())
            {
                var planilha = pacote.Workbook.Worksheets.Add("Eventos");
                planilha.Cells[1, 1].Value = "Local";
                planilha.Cells[1, 2].Value = "Data";

                planilha.Row(1).Style.Font.Bold = true;
                planilha.Row(1).Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                planilha.Row(1).Style.Fill.BackgroundColor.SetColor(Color.LightGray);

                for (int i = 0; i < lista.Count; i++)
                {
                    var evento = lista[i];
                    planilha.Cells[i + 2, 1].Value = evento.Local;
                    planilha.Cells[i + 2, 2].Value = evento.Data.ToShortDateString();
                }

                planilha.Cells.AutoFitColumns();
                return File(pacote.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Eventos.xlsx");
            }
        }
    }
}
