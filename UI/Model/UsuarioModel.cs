using Dominio;
using Repositorio;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace UI.Model
{
    public class UsuarioModel
    {
        private UsuarioRepositorio _usuarioRepositorio = new UsuarioRepositorio();

        // ================= CRIAR USUÁRIO =================
        public async Task<bool> CriarUsuario(string nome, string email, string senha)
        {
            Usuarios novoUsuario = new Usuarios(nome, email, Codificar(senha));
            return await _usuarioRepositorio.AddIfEmailNotExist(novoUsuario);
        }

        // ================= LOGIN =================
        public async Task<string> Entrar(string email, string senha)
        {
            Usuarios login = new Usuarios(email, Codificar(senha));
            Usuarios usuario = await _usuarioRepositorio.GetByEmailSenhaAsync(login);

            if (usuario != null)
                return usuario.Nome;

            return "";
        }

        // ================= VALIDAR SENHA =================
        public async Task<bool> ValidarSenha(int usuarioId, string senhaDigitada)
        {
            var usuario = await _usuarioRepositorio.GetByIdAsync(usuarioId);

            if (usuario == null)
                return false;

            string senhaHash = Codificar(senhaDigitada);

            return usuario.Senha == senhaHash;
        }

        // ================= CRIPTOGRAFIA (MD5) =================
        public static string Codificar(string texto)
        {
            using (var md5 = MD5.Create())
            {
                byte[] bytes = Encoding.ASCII.GetBytes(texto);
                byte[] hash = md5.ComputeHash(bytes);

                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < hash.Length; i++)
                {
                    sb.Append(hash[i].ToString("X2"));
                }

                return sb.ToString();
            }
        }
    }
}