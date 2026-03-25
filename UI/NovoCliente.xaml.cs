using Dominio;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using UI.Model;

namespace UI
{
    public partial class NovoCliente : Window
    {
        ClienteModel _cModel = new ClienteModel();
        private Clientes _cliente;

        public NovoCliente()
        {
            InitializeComponent();
        }

        public NovoCliente(Clientes cliente) : this()
        {
            _cliente = cliente;

            if (_cliente != null)
            {
                boxNome.Text = _cliente.ClienteNome;
                boxCPF.Text = _cliente.ClienteDocumento;
                boxEmail.Text = _cliente.ClienteEmail;
                boxTelefone.Text = _cliente.ClienteTelefone;
                boxSaldo.Text = _cliente.Saldo.ToString();
                boxAtivo.IsChecked = _cliente.Ativo;
            }
        }

        private async void btnConfirmarCliente(object sender, RoutedEventArgs e)
        {
            await ConfirmarCliente();
        }

        private async Task ConfirmarCliente()
        {
            // VALIDAÇÃO
            if (string.IsNullOrWhiteSpace(boxNome.Text))
            {
                MessageBox.Show("Nome é obrigatório");
                return;
            }

            if (string.IsNullOrWhiteSpace(boxCPF.Text))
            {
                MessageBox.Show("CPF é obrigatório");
                return;
            }

            decimal.TryParse(boxSaldo.Text, out var saldo);

            // NOVO CLIENTE
            if (_cliente == null)
            {
                await _cModel.AdicionarCliente(
                    nome: boxNome.Text,
                    documento: boxCPF.Text,
                    email: boxEmail.Text,
                    telefone: boxTelefone.Text,
                    saldo: saldo,
                    ativo: boxAtivo.IsChecked == true
                );

            }
            // EDITAR CLIENTE
            else
            {
                await _cModel.EditarCliente(
                    id: _cliente.IdCliente,
                    nome: boxNome.Text,
                    documento: boxCPF.Text,
                    email: boxEmail.Text,
                    telefone: boxTelefone.Text,
                    saldo: saldo,
                    ativo: boxAtivo.IsChecked == true
                );

            }

            this.Close();
        }

        private void ApenasNumero(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !decimal.TryParse(e.Text, out _);
        }
    }
}