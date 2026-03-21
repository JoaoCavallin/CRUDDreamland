using Dominio.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio
{
    public class Produtos
    {
        [Key]
        public int IdProduto { get; set; }

        [Required]
        [MaxLength(150)]
        public string Nome { get; set; }

        [MaxLength(300)]
        public string Descricao { get; set; }

        [Required]
        [MaxLength(100)]
        public string Categoria { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Preco { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal? Custo { get; set; }

        public int QuantidadeEstoque { get; set; } = 0;

        [MaxLength(100)]
        public string Marca { get; set; }

        [MaxLength(20)]
        public string Tamanho { get; set; }

        [MaxLength(20)]
        public string Genero { get; set; }

        [MaxLength(20)]
        public string Condicao { get; set; }

        [MaxLength(50)]
        public string CodigoBarras { get; set; }

        public DateTime DataCadastro { get; set; } = DateTime.Now;

        public bool Ativo { get; set; } = true;

        public List<ProdutoVendas> ProdutoVendas { get; set; }
    }
}
