using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Evento
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O local é obrigatório.")]
        public string Local { get; set; }

        [Required(ErrorMessage = "A data do evento é obrigatória.")]
        [DataType(DataType.Date)]
        [Display(Name = "Data do Evento")]
        public DateTime Data { get; set; }

        public static void GerarLista(HttpSessionStateBase session)
        {
            if (session["ListaEvento"] == null)
            {
                session["ListaEvento"] = new List<Evento>();
            }
        }

        public void Adicionar(HttpSessionStateBase session)
        {
            var lista = session["ListaEvento"] as List<Evento>;
            this.Id = lista.Count;
            lista.Add(this);
        }

        public static Evento Procurar(HttpSessionStateBase session, int id)
        {
            var lista = session["ListaEvento"] as List<Evento>;
            return lista.FirstOrDefault(e => e.Id == id);
        }

        public void Editar(HttpSessionStateBase session, int id)
        {
            var lista = session["ListaEvento"] as List<Evento>;
            var original = lista.FirstOrDefault(e => e.Id == id);
            if (original != null)
            {
                original.Local = this.Local;
                original.Data = this.Data;
            }
        }

        public void Excluir(HttpSessionStateBase session)
        {
            var lista = session["ListaEvento"] as List<Evento>;
            lista.RemoveAll(e => e.Id == this.Id);
        }
    }
}
