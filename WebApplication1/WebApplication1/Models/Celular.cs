using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Celular
    {
        public int Numero { get; set; }

        public string Marca { get; set; }

        public bool Novo { get; set; }

        public static List<Celular> GerarLista() 
        { 
            var lista = new List<Celular>();
            lista.Add(new Celular { Marca = "Samsung", Novo=true, Numero=998503442});
            lista.Add(new Celular { Marca = "LG", Novo=false, Numero=997508862});
            lista.Add(new Celular { Marca = "Iphone", Novo=false, Numero=996508002});

            return lista;
        }
    }
}