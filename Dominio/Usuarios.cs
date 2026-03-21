using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio
{
    public class Usuarios
    {
        [Key]
        public int IdUsuario { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nome { get; set; }

        [Required]
        [MaxLength(150)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MaxLength(255)]
        public string Senha { get; set; }

        public DateTime DataCriacao { get; set; } = DateTime.Now;

        public bool Ativo { get; set; } = true;

        public bool ADM { get; set; } = true;

        public Usuarios() { }

        public Usuarios(string email, string senha)
        {
            Email = email;
            Senha = senha;
        }

        public Usuarios(string nome, string email, string senha)
            : this(email, senha)
        {
            Nome = nome;
        }
    }
}
