using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;
using OfficeOpenXml;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class CelularController : Controller
    {
        public ActionResult Index() => RedirectToAction("Listar");

        public ActionResult Listar()
        {
            Celular.GerarLista(Session);
            return View(Session["ListaCelular"] as List<Celular>);
        }

        public ActionResult Create() => View(new Celular());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Celular celular)
        {
            if (ModelState.IsValid)
            {
                celular.Adicionar(Session);
                return RedirectToAction("Listar");
            }
            return View(celular);
        }

        public ActionResult Edit(int id)
        {
            return View(Celular.Procurar(Session, id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Celular celular)
        {
            if (ModelState.IsValid)
            {
                celular.Editar(Session, id);
                return RedirectToAction("Listar");
            }
            return View(celular);
        }

        [HttpPost]
        public ActionResult DeleteAjax(int id)
        {
            var celular = Celular.Procurar(Session, id);
            if (celular != null)
            {
                celular.Excluir(Session);
                return Json(new { sucesso = true });
            }
            return new HttpStatusCodeResult(404, "Celular não encontrado");
        }

        public ActionResult Exibir(int id)
        {
            return View((Session["ListaCelular"] as List<Celular>).ElementAt(id));
        }

        public ActionResult DownloadPdf()
        {
            var lista = Session["ListaCelular"] as List<Celular>;
            if (lista == null || !lista.Any())
                return RedirectToAction("Listar");

            using (MemoryStream ms = new MemoryStream())
            {
                Document doc = new Document(PageSize.A4);
                PdfWriter.GetInstance(doc, ms);
                doc.Open();
                doc.Add(new Paragraph("Lista de Celulares"));
                doc.Add(new Paragraph(" "));

                foreach (var celular in lista)
                {
                    doc.Add(new Paragraph($"Marca: {celular.Marca} - Número: {celular.Numero} - Novo: {(celular.Novo ? "Sim" : "Não")} - Data de Compra: {celular.DataCompra.ToShortDateString()}"));
                }

                doc.Close();
                return File(ms.ToArray(), "application/pdf", "ListaDeCelulares.pdf");
            }
        }

        public ActionResult DownloadExcel()
        {
            var lista = Session["ListaCelular"] as List<Celular>;
            if (lista == null || !lista.Any())
                return RedirectToAction("Listar");

            using (var pacote = new ExcelPackage())
            {
                var planilha = pacote.Workbook.Worksheets.Add("Celulares");
                planilha.Cells[1, 1].Value = "Marca";
                planilha.Cells[1, 2].Value = "Número";
                planilha.Cells[1, 3].Value = "Novo";
                planilha.Cells[1, 4].Value = "Data de Compra";

                planilha.Row(1).Style.Font.Bold = true;
                planilha.Row(1).Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                planilha.Row(1).Style.Fill.BackgroundColor.SetColor(Color.LightGray);

                for (int i = 0; i < lista.Count; i++)
                {
                    var celular = lista[i];
                    planilha.Cells[i + 2, 1].Value = celular.Marca;
                    planilha.Cells[i + 2, 2].Value = celular.Numero;
                    planilha.Cells[i + 2, 3].Value = celular.Novo ? "Sim" : "Não";
                    planilha.Cells[i + 2, 4].Value = celular.DataCompra.ToShortDateString();
                }

                planilha.Cells.AutoFitColumns();
                return File(pacote.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Celulares.xlsx");
            }
        }
    }
}
