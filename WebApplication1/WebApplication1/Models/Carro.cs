using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Carro
    {
        public string Placa { get; set; }
        public int Ano { get; set; }
        public string Cor { get; set; }

        public static List<Carro> GerarLista()
        {
            var lista = new List<Carro>();
            lista.Add(new Carro { Placa = "FVJ1889", Ano = 2020, Cor = "Preto" });
            lista.Add(new Carro { Placa = "FLP9972", Ano = 1999, Cor = "Azul" });
            lista.Add(new Carro { Placa = "VPI5640", Ano = 2024, Cor = "Verde" });

            return lista;
        }
    }
}