using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio
{
    public class Vendas
    {
        [Key]
        public int IdVenda { get; set; }

        [Required]
        [MaxLength(20)]
        public string ClienteDocumento { get; set; }

        [Required]
        [MaxLength(40)]
        public string ClienteNome { get; set; }

        public DateTime DataVenda { get; set; } = DateTime.Now;

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal ValorTotal { get; set; }

        [MaxLength(50)]
        public string FormaPagamento { get; set; }

        [MaxLength(20)]
        public string StatusVenda { get; set; } = "Concluida";

        public List<ProdutoVendas> ProdutoVendas { get; set; }

        public Vendas() { }

        public Vendas(string clienteDocumento, string clienteNome, decimal valorTotal)
        {
            ClienteDocumento = clienteDocumento;
            ClienteNome = clienteNome;
            ValorTotal = valorTotal;
        }
    }
}
