using System.Windows;
using UI.Model;

namespace UI
{
    public partial class NomeCpf : Window
    {
        public NomeCpf()
        {
            InitializeComponent();
        }

        private async void BtnContinuarVenda(object sender, RoutedEventArgs e)
        {
            var nome = boxNomeCliente.Text;
            var cpf = boxCpfCliente.Text;

            if (string.IsNullOrWhiteSpace(nome) || string.IsNullOrWhiteSpace(cpf))
            {
                MessageBox.Show("Preencha nome e CPF!");
                return;
            }

            ClienteModel cModel = new ClienteModel();

            // 🔥 BUSCAR CLIENTE
            var cliente = await cModel.BuscarPorNomeCpf(nome, cpf);

            if (cliente == null)
            {
                MessageBox.Show("Cliente não encontrado! Cadastre primeiro.");
                return;
            }

            // 🔥 PASSA O ID PRA PRÓXIMA TELA
            NovaVenda novaVenda = new NovaVenda(cliente.IdCliente, cliente.ClienteNome, cliente.ClienteDocumento);

            Close();
            novaVenda.ShowDialog();
        }
    }
}