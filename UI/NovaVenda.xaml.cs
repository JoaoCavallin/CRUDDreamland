using Dominio;
using Dominio.Enum;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using UI.Model;

namespace UI
{
    public partial class NovaVenda : Window
    {
        ObservableCollection<NovaVendaCollection> itensVenda = new ObservableCollection<NovaVendaCollection>();
        VendaModel vModel = new VendaModel();
        List<Produtos> produtos = new List<Produtos>();

        private int idVendaEdicao = -1;
        private int _clienteId;

        // ================= NOVA VENDA =================
        public NovaVenda(int clienteId, string nomeCliente, string cpfCliente)
        {
            InitializeComponent();

            _clienteId = clienteId;

            blockNomeCliente.Text = nomeCliente;
            blockCpfCliente.Text = cpfCliente;

            boxPagamento.ItemsSource = Enum.GetValues(typeof(FormaPagamento));
            boxStatus.ItemsSource = Enum.GetValues(typeof(StatusVenda));

            gridVendaProduto.ItemsSource = itensVenda;

            // 🔥 limpar estado
            itensVenda.Clear();
            produtos.Clear();
            AtualizarTotal();
        }

        // ================= EDIÇÃO =================
        public NovaVenda(int idVenda, int clienteId, string nomeCliente, string cpfCliente, decimal valorTotal, List<Produtos> itens = null)
        {
            InitializeComponent();

            idVendaEdicao = idVenda;
            _clienteId = clienteId;

            blockNomeCliente.Text = nomeCliente;
            blockCpfCliente.Text = cpfCliente;

            boxPagamento.ItemsSource = Enum.GetValues(typeof(FormaPagamento));
            boxStatus.ItemsSource = Enum.GetValues(typeof(StatusVenda));

            gridVendaProduto.ItemsSource = itensVenda;

            ConfirmarVenda.Content = "Atualizar Venda";

            // 🔥 carregar itens se vierem
            if (itens != null)
            {
                foreach (var p in itens)
                {
                    var item = new NovaVendaGridItem(
                        p.IdProduto,
                        p.Nome,
                        p.Preco,
                        1
                    );

                    itensVenda.Add(item);
                    produtos.Add(p);
                }
            }

            AtualizarTotal();
        }

        // ================= BUSCAR PRODUTO =================
        private async void boxCodProduto_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (!int.TryParse(boxCodProduto.Text, out int codigo))
                {
                    MessageBox.Show("Código inválido!");
                    return;
                }

                var produto = await vModel.ProcurarProduto(codigo);

                if (produto != null)
                {
                    blockNomeProduto.Text = produto.Nome;
                }
                else
                {
                    MessageBox.Show("Produto não encontrado!");
                }
            }
        }

        // ================= ADICIONAR PRODUTO =================
        private async void boxQuantidade_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (!int.TryParse(boxCodProduto.Text, out int codigoProduto))
                {
                    MessageBox.Show("Código inválido!");
                    return;
                }

                if (!int.TryParse(boxQuantidade.Text, out int quantidade))
                {
                    MessageBox.Show("Quantidade inválida!");
                    return;
                }

                Produtos produto = await vModel.ProcurarProduto(codigoProduto);

                if (produto == null)
                {
                    MessageBox.Show("Produto inválido!");
                    return;
                }

                produtos.Add(produto);

                var vendaItem = new NovaVendaGridItem(
                    produto.IdProduto,
                    produto.Nome,
                    produto.Preco,
                    quantidade
                );

                itensVenda.Add(vendaItem);

                AtualizarTotal();
            }
        }

        // ================= CONSULTAR PRODUTOS =================
        private async void BtnConsultarProdutos(object sender, RoutedEventArgs e)
        {
            var lista = await vModel.ListarProdutosParaVenda();

            itensVenda.Clear();
            produtos.Clear();

            foreach (var item in lista)
            {
                itensVenda.Add(item);
            }

            AtualizarTotal();
        }

        // ================= ATUALIZAR TOTAL =================
        private void AtualizarTotal()
        {
            decimal total = itensVenda.Sum(i => i.Total);
            blockTotal.Text = total.ToString("F2");
        }

        // ================= CONFIRMAR VENDA =================
        private async void btnConfirmarVenda(object sender, RoutedEventArgs e)
        {
            if (!decimal.TryParse(blockTotal.Text, out decimal total))
            {
                MessageBox.Show("Total inválido.");
                return;
            }

            if (boxPagamento.SelectedItem == null || boxStatus.SelectedItem == null)
            {
                MessageBox.Show("Selecione forma de pagamento e status");
                return;
            }

            var formaPagamento = (FormaPagamento)boxPagamento.SelectedItem;
            var statusVenda = (StatusVenda)boxStatus.SelectedItem;

            bool status;

            if (idVendaEdicao == -1)
            {
                status = await vModel.NovaVenda(
                    _clienteId,
                    total,
                    produtos,
                    formaPagamento.ToString(),
                    statusVenda.ToString()
                );

                if (status)
                {
                    MessageBox.Show("Venda cadastrada com sucesso!");
                    Close();
                }
                else
                {
                    MessageBox.Show("Erro ao cadastrar venda!", "ERRO",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                status = await vModel.EditarVenda(
                    idVendaEdicao,
                    _clienteId,
                    total,
                    produtos,
                    formaPagamento.ToString(),
                    statusVenda.ToString()
                );

                if (status)
                {
                    MessageBox.Show("Venda atualizada com sucesso!");
                    Close();
                }
                else
                {
                    MessageBox.Show("Erro ao atualizar venda!", "ERRO",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // ================= REMOVER ITEM =================
        private void BtnRemoverSelecionado(object sender, RoutedEventArgs e)
        {
            var item = gridVendaProduto.SelectedItem as NovaVendaGridItem;

            if (item == null)
            {
                MessageBox.Show("Selecione um item no grid");
                return;
            }

            itensVenda.Remove(item);

            var produto = produtos.FirstOrDefault(p => p.IdProduto == item.ProdutoId);
            if (produto != null)
                produtos.Remove(produto);

            AtualizarTotal();
        }
    }

    // ================= CLASSE DO GRID =================
    class NovaVendaGridItem
    {
        public int ProdutoId { get; set; }
        public string ProdutoNome { get; set; }
        public decimal Preco { get; set; }
        public int QuantidadeProduto { get; set; }
        public decimal Total { get; set; }

        public NovaVendaGridItem(int produtoId, string produtoNome, decimal preco, int quantidadeProduto)
        {
            ProdutoId = produtoId;
            ProdutoNome = produtoNome;
            Preco = preco;
            QuantidadeProduto = quantidadeProduto;
            Total = quantidadeProduto * preco;
        }
    }
}