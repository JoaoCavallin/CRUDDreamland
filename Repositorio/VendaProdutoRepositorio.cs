using Dominio;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public async Task<List<ProdutoVendas>> GetByVendaIdAsync(int idVenda)
        {
            return await _context.ProdutoVendas
                .Where(p => p.VendaId == idVenda)
                .ToListAsync();
        }

        public void RemoveRange(List<ProdutoVendas> itens)
        {
            _context.ProdutoVendas.RemoveRange(itens);
        }

    }
}
