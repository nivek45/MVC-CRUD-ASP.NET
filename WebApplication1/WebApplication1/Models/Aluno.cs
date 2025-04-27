using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Ajax.Utilities;

namespace WebApplication1.Models
{
    public class Aluno
    {
        public string Nome { get; set; }
        public string RA { get; set; }

        public static void GerarLista(HttpSessionStateBase session)
        {
            if (session["ListaAluno"] != null)
            {
                if(((List<Aluno>)session["ListaAluno"]).Count > 0)
                {
                    return;
                }
            }
            var lista = new List<Aluno>();
            lista.Add(new Aluno { Nome = "Xibiu", RA = "69" });
            lista.Add(new Aluno { Nome = "Trozoba", RA = "45" });
            lista.Add(new Aluno { Nome = "Willian", RA = "689" });

            session.Remove("ListaAluno");
            session.Add("ListaAluno", lista);
        }

    }
}