using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio
{
    public class ProdutoVendas
    {
        [Key]
        public int IdProdutoVenda { get; set; }

        [Required]
        public int VendaId { get; set; }

        public Vendas Venda { get; set; }

        [Required]
        public int ProdutoId { get; set; }

        public Produtos Produto { get; set; }

        [Required]
        public int Quantidade { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal PrecoUnitario { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Subtotal { get; set; }

        public ProdutoVendas() { }
    }
}
