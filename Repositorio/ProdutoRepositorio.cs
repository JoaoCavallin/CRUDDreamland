using Dominio;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Repositorio
{
    public class ProdutoRepositorio : IRepositorio<Produtos>
    {
        private readonly Context _context = new Context();

        public void Add(Produtos produto)
        {
            _context.Produtos.Add(produto);
        }

        public void Delete(Produtos produto)
        {
            _context.Produtos.Remove(produto);
        }

        public void Update(Produtos produto)
        {
            _context.Produtos.Update(produto);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }

        public async Task<Produtos[]> GetAllAsync()
        {
            return await _context.Produtos.ToArrayAsync();
        }

        public async Task<Produtos> GetByIdAsync(int ProdutoId)
        {
            return await _context.Produtos.Where(P => P.IdProduto == ProdutoId).FirstOrDefaultAsync();
        }
    }
}
