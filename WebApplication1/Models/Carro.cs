using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Carro
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "A placa é obrigatória.")]
        public string Placa { get; set; }

        [Required(ErrorMessage = "O ano é obrigatório.")]
        public int Ano { get; set; }

        [Required(ErrorMessage = "A cor é obrigatória.")]
        public string Cor { get; set; }

        [Required(ErrorMessage = "A data de fabricação é obrigatória.")]
        [DataType(DataType.Date)]
        [Display(Name = "Data de Fabricação")]
        public DateTime DataFabricacao { get; set; }

        public static void GerarLista(HttpSessionStateBase session)
        {
            if (session["ListaCarro"] == null)
            {
                session["ListaCarro"] = new List<Carro>();
            }
        }

        public void Adicionar(HttpSessionStateBase session)
        {
            var lista = session["ListaCarro"] as List<Carro>;
            this.Id = lista.Count;
            lista.Add(this);
        }

        public static Carro Procurar(HttpSessionStateBase session, int id)
        {
            var lista = session["ListaCarro"] as List<Carro>;
            return lista.FirstOrDefault(c => c.Id == id);
        }

        public void Editar(HttpSessionStateBase session, int id)
        {
            var lista = session["ListaCarro"] as List<Carro>;
            var original = lista.FirstOrDefault(c => c.Id == id);
            if (original != null)
            {
                original.Placa = this.Placa;
                original.Ano = this.Ano;
                original.Cor = this.Cor;
                original.DataFabricacao = this.DataFabricacao;
            }
        }

        public void Excluir(HttpSessionStateBase session)
        {
            var lista = session["ListaCarro"] as List<Carro>;
            lista.RemoveAll(c => c.Id == this.Id);
        }
    }
}
