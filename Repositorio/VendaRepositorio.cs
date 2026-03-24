using Dominio;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Repositorio
{
    public class VendaRepositorio : IRepositorio<Vendas>
    {
        private readonly Context _context = new Context();

        public void Add(Vendas venda)
        {
            _context.Vendas.Add(venda);
        }

        public void Delete(Vendas venda)
        {
            _context.Vendas.Remove(venda);
        }

        public void Update(Vendas venda)
        {
            _context.Vendas.Update(venda);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }

        public async Task<Vendas[]> GetAllAsync()
        {
            return await _context.Vendas.AsNoTracking().ToArrayAsync();
        }

        public async Task<Vendas> GetByIdAsync(int vendaId)
        {
            return await _context.Vendas.Where(V => V.IdVenda == vendaId).FirstOrDefaultAsync();
        }

        public void Remove(Vendas venda)
        {
            _context.Vendas.Remove(venda);
        }
    }
}
