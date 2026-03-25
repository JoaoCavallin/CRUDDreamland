using Dominio;
using Repositorio;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace UI.Model
{
    public class ClienteModel
    {
        private ClienteRepositorio _clienteRepositorio = new ClienteRepositorio();

        public async Task<Clientes[]> ListarClientes()
        {
            return await _clienteRepositorio.GetAllAsync();
        }

        public async Task AdicionarCliente(
            string documento,
            string nome,
            string email,
            string telefone,
            decimal saldo,
            bool ativo
        )
        {
            Clientes cliente = new Clientes
            {
                ClienteDocumento = documento,
                ClienteNome = nome,
                ClienteEmail = email,
                ClienteTelefone = telefone,
                Saldo = saldo,
                DataCadastro = DateTime.Now,
                Ativo = ativo
            };

            _clienteRepositorio.Add(cliente);
            await _clienteRepositorio.SaveChangesAsync();

            MessageBox.Show("Cliente cadastrado com sucesso!");
        }

        public async Task ExcluirCliente(int id)
        {
            using (var context = new Context())
            {
                var cliente = context.Clientes.FirstOrDefault(c => c.IdCliente == id);

                if (cliente != null)
                {
                    context.Clientes.Remove(cliente);
                    context.SaveChanges();
                    MessageBox.Show("Cliente excluído com sucesso!");
                }
                else
                {
                    MessageBox.Show("Cliente não encontrado.");
                }
            }
        }

        public async Task EditarCliente(
            int id,
            string documento,
            string nome,
            string email,
            string telefone,
            decimal saldo,
            bool ativo
        )
        {
            Clientes cliente = new Clientes
            {
                IdCliente = id,
                ClienteDocumento = documento,
                ClienteNome = nome,
                ClienteEmail = email,
                ClienteTelefone = telefone,
                Saldo = saldo,
                DataCadastro = DateTime.Now,
                Ativo = ativo
            };

            _clienteRepositorio.Update(cliente);
            await _clienteRepositorio.SaveChangesAsync();

            MessageBox.Show("Cliente atualizado com sucesso!");
        }

        public async Task<Clientes> BuscarPorNomeCpf(string nome, string cpf)
        {
            var clientes = await _clienteRepositorio.GetAllAsync();

            return clientes.FirstOrDefault(c =>
                c.ClienteNome == nome &&
                c.ClienteDocumento == cpf
            );
        }

        // =============== Validações malucas ===============

        public bool ValidarCPF(string cpf)
        {
            cpf = Regex.Replace(cpf, @"\D", "");

            if (cpf.Length != 11)
                return false;

            return true;
        }

        public bool ValidarEmail(string email)
        {
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.com$");
        }

        public bool ValidarTelefone(string telefone)
        {
            string numeros = Regex.Replace(telefone, @"\D", "");
            return numeros.Length >= 10 && numeros.Length <= 13;
        }

        private void ApenasNumero(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !decimal.TryParse(e.Text, out _);
        }
    }
}