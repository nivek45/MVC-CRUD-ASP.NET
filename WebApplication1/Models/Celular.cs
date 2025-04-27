using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Celular
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O número é obrigatório.")]
        public string Numero { get; set; }

        [Required(ErrorMessage = "A marca é obrigatória.")]
        public string Marca { get; set; }

        [Display(Name = "Está Novo?")]
        public bool Novo { get; set; }

        [Required(ErrorMessage = "A data de compra é obrigatória.")]
        [DataType(DataType.Date)]
        [Display(Name = "Data de Compra")]
        public DateTime DataCompra { get; set; }

        // Métodos auxiliares
        public static void GerarLista(HttpSessionStateBase session)
        {
            if (session["ListaCelular"] == null)
            {
                var lista = new List<Celular>
                {
                    new Celular { Id = 0, Marca = "Samsung", Novo = true, Numero = "(15) 99850-3442", DataCompra = new DateTime(2020, 12, 2) },
                    new Celular { Id = 1, Marca = "LG", Novo = false, Numero = "(15) 99750-8862", DataCompra = new DateTime(2025, 3, 12) },
                    new Celular { Id = 2, Marca = "Iphone", Novo = false, Numero = "(11) 99650-8002", DataCompra = new DateTime(2021, 9, 22) }
                };

                session["ListaCelular"] = lista;
            }
        }

        public string NumeroFormatado
        {
            get
            {
                if (string.IsNullOrEmpty(Numero))
                    return "";

                if (Numero.Length == 11)
                    return Convert.ToUInt64(Numero).ToString(@"(00) 00000-0000");
                else if (Numero.Length == 10)
                    return Convert.ToUInt64(Numero).ToString(@"(00) 0000-0000");
                else
                    return Numero;
            }
        }
        public void Adicionar(HttpSessionStateBase session)
        {
            var lista = session["ListaCelular"] as List<Celular>;

            if (lista == null)
            {
                lista = new List<Celular>();
                session["ListaCelular"] = lista;
            }

            this.Id = lista.Count > 0 ? lista.Max(c => c.Id) + 1 : 0;
            lista.Add(this);
        }

        public static Celular Procurar(HttpSessionStateBase session, int id)
        {
            var lista = session["ListaCelular"] as List<Celular>;

            if (lista == null)
                return null;

            return lista.FirstOrDefault(c => c.Id == id);
        }

        public void Editar(HttpSessionStateBase session, int id)
        {
            var lista = session["ListaCelular"] as List<Celular>;

            if (lista == null)
                return;

            var original = lista.FirstOrDefault(c => c.Id == id);
            if (original != null)
            {
                original.Numero = this.Numero;
                original.Marca = this.Marca;
                original.Novo = this.Novo;
                original.DataCompra = this.DataCompra;
            }
        }

        public void Excluir(HttpSessionStateBase session)
        {
            var lista = session["ListaCelular"] as List<Celular>;

            if (lista == null)
                return;

            lista.RemoveAll(c => c.Id == this.Id);
        }
    }
}
