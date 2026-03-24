using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio
{
    public class Clientes
    {
        [Key]
        public int IdCliente { get; set; }

        [Required]
        [MaxLength(20)]
        public string ClienteDocumento { get; set; }

        [Required]
        [MaxLength(40)]
        public string ClienteNome { get; set; }

        [MaxLength(150)]
        public string ClienteEmail { get; set; }

        [MaxLength(20)]
        public string ClienteTelefone { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Saldo { get; set; } = 0;

        public DateTime DataCadastro { get; set; } = DateTime.Now;

        public bool Ativo { get; set; } = true;

        //  Relacionamento com Vendas
        public ICollection<Vendas> Vendas { get; set; }
    }
}