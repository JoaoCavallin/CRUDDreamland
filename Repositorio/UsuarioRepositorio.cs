using Dominio;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Repositorio
{
    public class UsuarioRepositorio : IRepositorio<Usuarios>
    {
        private readonly Context _context = new Context();

        public void Add(Usuarios usuario)
        {
            _context.Usuarios.Add(usuario);
        }

        public void Delete(Usuarios usuario)
        {
            _context.Usuarios.Remove(usuario);
        }

        public void Update(Usuarios usuario)
        {
            _context.Usuarios.Update(usuario);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }

        public async Task<Usuarios[]> GetAllAsync()
        {
            return await _context.Usuarios.ToArrayAsync();
        }

        public async Task<Usuarios> GetByIdAsync(int usuarioId)
        {
            return await _context.Usuarios.Where(U => U.IdUsuario == usuarioId).FirstOrDefaultAsync();
        }

        public async Task<bool> AddIfEmailNotExist(Usuarios usuario)
        {
            Usuarios emailexist = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == usuario.Email);
            if (emailexist == null)
            {
                await _context.Usuarios.AddAsync(usuario);
                return await SaveChangesAsync();
            }
            else
            {
                return false;
            }
        }

        public async Task<Usuarios> GetByEmailSenhaAsync(Usuarios usuario)
        {
            Usuarios user = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == usuario.Email && u.Senha == usuario.Senha);

            return user;
        }

    }
}
