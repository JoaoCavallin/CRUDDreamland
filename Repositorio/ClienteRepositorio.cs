using Dominio;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Repositorio
{
    public class ClienteRepositorio : IRepositorio<Clientes>
    {
        private readonly Context _context = new Context();

        public void Add(Clientes cliente)
        {
            _context.Clientes.Add(cliente);
        }

        public void Delete(Clientes cliente)
        {
            _context.Clientes.Remove(cliente);
        }

        public void Update(Clientes cliente)
        {
            _context.Clientes.Update(cliente);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }

        public async Task<Clientes[]> GetAllAsync()
        {
            return await _context.Clientes.AsNoTracking().ToArrayAsync();
        }

        public async Task<Clientes> GetByIdAsync(int clienteId)
        {
            return await _context.Clientes
                .Where(c => c.IdCliente == clienteId)
                .FirstOrDefaultAsync();
        }

        public async Task<Clientes> GetByDocumentoAsync(string documento)
        {
            return await _context.Clientes
                .FirstOrDefaultAsync(c => c.ClienteDocumento == documento);
        }
        public async Task DescontarSaldo(int clienteId, decimal valor)
        {
            var cliente = await _context.Clientes
                .Where(c => c.IdCliente == clienteId)
                .FirstOrDefaultAsync();

            if (cliente != null)
            {
                cliente.Saldo -= valor;
                _context.Clientes.Update(cliente);
                await _context.SaveChangesAsync();
            }
        }
    }
}