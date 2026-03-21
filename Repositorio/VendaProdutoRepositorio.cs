using Dominio;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositorio
{
    public class VendaProdutoRepositorio
    {
        private readonly Context _context = new Context();

        public void Add(ProdutoVendas venda)
        {
            _context.ProdutoVendas.Add(venda);
        }

        public void AddRange(List<ProdutoVendas> VendaProduto)
        {
            _context.ProdutoVendas.AddRange(VendaProduto);
        }

        public void Delete(ProdutoVendas venda)
        {
            _context.ProdutoVendas.Remove(venda);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}
