using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Microsoft.Ajax.Utilities;

namespace WebApplication1.Models
{
    public class Aluno
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "E-mail inválido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "A data de nascimento é obrigatória.")]
        [DataType(DataType.Date)]
        [Display(Name = "Data de Nascimento")]
        public DateTime DataNascimento { get; set; }

        // Métodos auxiliares (exemplo simplificado para sessão)
        public static void GerarLista(HttpSessionStateBase session)
        {
            if (session["ListaAluno"] == null)
            {
                session["ListaAluno"] = new List<Aluno>();
            }
        }


        public void Adicionar(HttpSessionStateBase session)
        {
            var lista = session["ListaAluno"] as List<Aluno>;
            this.Id = lista.Count;
            lista.Add(this);
        }

        public static Aluno Procurar(HttpSessionStateBase session, int id)
        {
            var lista = session["ListaAluno"] as List<Aluno>;
            return lista.FirstOrDefault(a => a.Id == id);
        }

        public void Editar(HttpSessionStateBase session, int id)
        {
            var lista = session["ListaAluno"] as List<Aluno>;
            var original = lista.FirstOrDefault(a => a.Id == id);
            if (original != null)
            {
                original.Nome = this.Nome;
                original.Email = this.Email;
                original.DataNascimento = this.DataNascimento;
            }
        }

        public void Excluir(HttpSessionStateBase session)
        {
            var lista = session["ListaAluno"] as List<Aluno>;
            lista.RemoveAll(a => a.Id == this.Id);
        }
    }
}
