using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WebApplication1.Models;

namespace WebApplication1
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            OfficeOpenXml.ExcelPackage.License.SetNonCommercialPersonal("Kevin Silva");
        }
        protected void Session_Start()
        {
            List<Aluno> listaAluno = new List<Aluno>
                {
                    new Aluno { Id = 1, Nome = "Alice Silva", Email = "alice.silva@email.com", DataNascimento = new DateTime(2000, 5, 15) },
                    new Aluno { Id = 2, Nome = "Bruno Souza", Email = "bruno.souza@email.com", DataNascimento = new DateTime(1999, 8, 22) },
                    new Aluno { Id = 3, Nome = "Carla Mendes", Email = "carla.mendes@email.com", DataNascimento = new DateTime(2001, 2, 10) },
                    new Aluno { Id = 4, Nome = "Daniel Rocha", Email = "daniel.rocha@email.com", DataNascimento = new DateTime(2000, 11, 5) },
                    new Aluno { Id = 5, Nome = "Eduarda Lima", Email = "eduarda.lima@email.com", DataNascimento = new DateTime(1998, 7, 30) },
                    new Aluno { Id = 6, Nome = "Felipe Martins", Email = "felipe.martins@email.com", DataNascimento = new DateTime(1997, 3, 18) },
                    new Aluno { Id = 7, Nome = "Gabriela Alves", Email = "gabriela.alves@email.com", DataNascimento = new DateTime(2002, 6, 25) },
                    new Aluno { Id = 8, Nome = "Henrique Costa", Email = "henrique.costa@email.com", DataNascimento = new DateTime(1999, 9, 9) },
                    new Aluno { Id = 9, Nome = "Isabela Dias", Email = "isabela.dias@email.com", DataNascimento = new DateTime(2001, 12, 3) },
                    new Aluno { Id = 10, Nome = "João Pereira", Email = "joao.pereira@email.com", DataNascimento = new DateTime(2000, 1, 20) }
                };

            Session["ListaAluno"] = listaAluno;

            List<Carro> listaCarro = new List<Carro>
                {
                    new Carro { Placa = "ABC-1234", Ano = 2015, Cor = "Preto", DataFabricacao = new DateTime(2015, 4, 10) },
                    new Carro { Placa = "XYZ-5678", Ano = 2020, Cor = "Vermelho", DataFabricacao = new DateTime(2020, 7, 15) },
                    new Carro { Placa = "QWE-9012", Ano = 2018, Cor = "Branco", DataFabricacao = new DateTime(2018, 6, 20) },
                    new Carro { Placa = "RTY-3456", Ano = 2022, Cor = "Azul", DataFabricacao = new DateTime(2022, 3, 5) },
                    new Carro { Placa = "JKL-7890", Ano = 2017, Cor = "Prata", DataFabricacao = new DateTime(2017, 11, 25) }
                };
            Session["ListaCarro"] = listaCarro;

        }
    }

}