using Dominio;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using UI.Model;

namespace UI
{
    public partial class Principal : Window
    {
        ProdutoModel _pModel = new ProdutoModel();
        VendaModel _vModel = new VendaModel();
        ClienteModel _cModel = new ClienteModel();

        public Principal(string usuarioAtual)
        {
            InitializeComponent();

            BoxUsuarioAtual.Text = usuarioAtual;

            //  Mostrar detalhes ao selecionar
            gridProdutos.RowDetailsVisibilityMode = DataGridRowDetailsVisibilityMode.VisibleWhenSelected;
        }

        // ================= PRODUTOS =================

        private void BtnCadastroProduto(object sender, RoutedEventArgs e)
        {
            new NovoProduto().ShowDialog();
            BtnConsultarProduto(null, null); // refresh
        }

        private void btnEditarProduto(object sender, RoutedEventArgs e)
        {
            var produto = (Produtos)gridProdutos.SelectedItem;

            if (produto != null)
            {
                new NovoProduto(produto).ShowDialog();
                BtnConsultarProduto(null, null);
            }
            else
            {
                MessageBox.Show("Selecione um produto");
            }
        }

        private async void BtnConsultarProduto(object sender, RoutedEventArgs e)
        {
            gridProdutos.ItemsSource = await _pModel.ListarProdutos();
        }

        private void btnExcluirProduto(object sender, RoutedEventArgs e)
        {
            var produto = (Produtos)gridProdutos.SelectedItem;

            if (produto != null)
            {
                var confirm = MessageBox.Show("Deseja excluir o produto?", "Confirmação",
                    MessageBoxButton.YesNo);

                if (confirm == MessageBoxResult.Yes)
                {
                    _pModel.ExcluirProduto(produto.IdProduto);
                    BtnConsultarProduto(null, null);
                }
            }
            else
            {
                MessageBox.Show("Selecione um produto");
            }
        }

        //  BUSCA
        private async void BuscarProduto(object sender, TextChangedEventArgs e)
        {
            var texto = boxBuscaProduto.Text.ToLower();

            var lista = await _pModel.ListarProdutos();

            gridProdutos.ItemsSource = lista
                .Where(p => p.Nome.ToLower().Contains(texto)
                         || (p.Marca != null && p.Marca.ToLower().Contains(texto)))
                .ToList();
        }

        // ================= VENDAS =================

        private void BtnNovaVendaDialog(object sender, RoutedEventArgs e)
        {
            new NomeCpf().ShowDialog();

            BtnConsultarVenda(null, null);
        }

        private async void BtnConsultarVenda(object sender, RoutedEventArgs e)
        {
            gridVendas.ItemsSource = await _vModel.ListarVendas();
        }

        private void BtnEditarVenda(object sender, RoutedEventArgs e)
        {
            var venda = gridVendas.SelectedItem;

            if (venda != null)
            {
                MessageBox.Show("Tela de edição de venda ainda não implementada 😄");
                //  depois você cria: new EditarVenda(venda).ShowDialog();
            }
            else
            {
                MessageBox.Show("Selecione uma venda");
            }
        }

        private async void BtnExcluirVenda(object sender, RoutedEventArgs e)
        {
            var venda = gridVendas.SelectedItem as Vendas;

            if (venda == null)
            {
                MessageBox.Show("Selecione uma venda");
                return;
            }

            var confirm = MessageBox.Show(
                $"Deseja excluir a venda ID {venda.IdVenda}?",
                "Confirmação",
                MessageBoxButton.YesNo);

            if (confirm != MessageBoxResult.Yes)
                return;

            try
            {
                btnExcluirVenda.IsEnabled = false;

                var sucesso = await _vModel.ExcluirVenda(venda.IdVenda);

                if (sucesso)
                {
                    MessageBox.Show("Venda excluída com sucesso!");
                    BtnConsultarVenda(null, null);
                }
                else
                {
                    MessageBox.Show("Erro ao excluir venda!");
                }
            }
            finally
            {
                btnExcluirVenda.IsEnabled = true;
            }
        }

        // ================= CLIENTES =================

        private async void BtnConsultarClientes(object sender, RoutedEventArgs e)
        {
            gridClientes.ItemsSource = await _cModel.ListarClientes();
        }

        private void BtnNovoCliente(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Tela de cadastro de cliente ainda não criada 😄");
            // new NovoCliente().ShowDialog();
        }

        private void BtnEditarCliente(object sender, RoutedEventArgs e)
        {
            var cliente = gridClientes.SelectedItem;

            if (cliente != null)
            {
                MessageBox.Show("Tela de edição de cliente ainda não criada 😄");
            }
            else
            {
                MessageBox.Show("Selecione um cliente");
            }
        }

        private void BtnExcluirCliente(object sender, RoutedEventArgs e)
        {
            dynamic cliente = gridClientes.SelectedItem;

            if (cliente != null)
            {
                var confirm = MessageBox.Show("Excluir cliente?", "Confirmação",
                    MessageBoxButton.YesNo);

                if (confirm == MessageBoxResult.Yes)
                {
                    _cModel.ExcluirCliente(cliente.IdCliente);
                    BtnConsultarClientes(null, null);
                }
            }
            else
            {
                MessageBox.Show("Selecione um cliente");
            }
        }
    }
}