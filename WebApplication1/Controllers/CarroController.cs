using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iTextSharp.text.pdf;
using iTextSharp.text;
using WebApplication1.Models;
using OfficeOpenXml;
using System.Drawing;

namespace WebApplication1.Controllers
{
    public class CarroController : Controller
    {
        public ActionResult Index() => RedirectToAction("Listar");

        public ActionResult Listar()
        {
            Carro.GerarLista(Session);
            return View(Session["ListaCarro"] as List<Carro>);
        }

        // Criação de novo carro
        public ActionResult Create() => View(new Carro());

        // Criação do carro (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Carro carro)
        {
            if (ModelState.IsValid)
            {
                carro.Adicionar(Session);
                return RedirectToAction("Listar");
            }
            return View(carro);
        }

        // Edição de carro
        public ActionResult Edit(int id) => View(Carro.Procurar(Session, id));

        // Edição de carro (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Carro carro)
        {
            if (ModelState.IsValid)
            {
                carro.Editar(Session, id);
                return RedirectToAction("Listar");
            }
            return View(carro);
        }

        public ActionResult Delete(int id) => View(Carro.Procurar(Session, id));

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Carro carro)
        {
            Carro.Procurar(Session, id)?.Excluir(Session);
            return RedirectToAction("Listar");
        }

        public ActionResult Exibir(int id)
        {
            return View((Session["ListaCarro"] as List<Carro>).ElementAt(id));
        }

        [HttpPost]
        public ActionResult DeleteAjax(int id)
        {
            var carro = Carro.Procurar(Session, id);
            if (carro != null)
            {
                carro.Excluir(Session);
                return Json(new { sucesso = true });
            }
            return new HttpStatusCodeResult(404, "Carro não encontrado");
        }

        public ActionResult GerarPdf()
        {
            var listaCarros = Session["ListaCarro"] as List<Carro>;
            if (listaCarros == null || !listaCarros.Any())
                return RedirectToAction("Listar");

            using (var output = new MemoryStream())
            {
                var document = new Document(PageSize.A4);
                var writer = PdfWriter.GetInstance(document, output);
                document.Open();

                var tituloFonte = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
                var titulo = new Paragraph("Lista de Carros", tituloFonte)
                {
                    Alignment = Element.ALIGN_CENTER
                };
                document.Add(titulo);
                document.Add(new Paragraph(" "));
                document.Add(new Paragraph(" "));

                var tabela = new PdfPTable(4);
                tabela.AddCell("Placa");
                tabela.AddCell("Ano");
                tabela.AddCell("Cor");
                tabela.AddCell("Data de Fabricação");

                foreach (var carro in listaCarros)
                {
                    tabela.AddCell(carro.Placa);
                    tabela.AddCell(carro.Ano.ToString());
                    tabela.AddCell(carro.Cor);
                    tabela.AddCell(carro.DataFabricacao.ToString("dd/MM/yyyy"));
                }

                document.Add(tabela);
                document.Close();

                return File(output.ToArray(), "application/pdf", "ListaCarros.pdf");
            }
        }

        public ActionResult GerarExcel()
        {
            var listaCarros = Session["ListaCarro"] as List<Carro>;
            if (listaCarros == null || !listaCarros.Any())
                return RedirectToAction("Listar");

            using (var pacote = new ExcelPackage())
            {
                var planilha = pacote.Workbook.Worksheets.Add("Carros");
                planilha.Cells[1, 1].Value = "Placa";
                planilha.Cells[1, 2].Value = "Ano";
                planilha.Cells[1, 3].Value = "Cor";
                planilha.Cells[1, 4].Value = "Data de Fabricação";

                planilha.Row(1).Style.Font.Bold = true;
                planilha.Row(1).Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                planilha.Row(1).Style.Fill.BackgroundColor.SetColor(Color.LightGray);

                for (int i = 0; i < listaCarros.Count; i++)
                {
                    var carro = listaCarros[i];
                    planilha.Cells[i + 2, 1].Value = carro.Placa;
                    planilha.Cells[i + 2, 2].Value = carro.Ano;
                    planilha.Cells[i + 2, 3].Value = carro.Cor;
                    planilha.Cells[i + 2, 4].Value = carro.DataFabricacao.ToString("dd/MM/yyyy");
                }

                planilha.Cells.AutoFitColumns();
                return File(pacote.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Carros.xlsx");
            }
        }
    }
}
