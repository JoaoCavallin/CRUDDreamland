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

            if (itens != null)
            {
                foreach (var p in itens)
                {
                    var item = new NovaVendaCollection(
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
                if (!int.TryParse(boxCodProduto.Text, out int codigo) || codigo <= 0)
                {
                    MessageBox.Show("Código inválido! Digite apenas números positivos.");
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
                if (!int.TryParse(boxCodProduto.Text, out int codigoProduto) || codigoProduto <= 0)
                {
                    MessageBox.Show("Código inválido! Digite apenas números positivos.");
                    return;
                }

                if (!int.TryParse(boxQuantidade.Text, out int quantidade) || quantidade <= 0)
                {
                    MessageBox.Show("Quantidade inválida! Deve ser maior que zero.");
                    return;
                }

                Produtos produto = await vModel.ProcurarProduto(codigoProduto);

                if (produto == null)
                {
                    MessageBox.Show("Produto não encontrado!");
                    return;
                }

                // VALIDAÇÃO DE ESTOQUE AO ADICIONAR
                if (produto.QuantidadeEstoque < quantidade)
                {
                    MessageBox.Show($"Estoque insuficiente! Disponível: {produto.QuantidadeEstoque}");
                    return;
                }

                // VERIFICA DUPLICADO
                bool jaAdicionado = itensVenda.Any(i => i.ProdutoId == produto.IdProduto);
                if (jaAdicionado)
                {
                    MessageBox.Show("Este produto já foi adicionado à venda.");
                    return;
                }

                produtos.Add(produto);

                var vendaItem = new NovaVendaCollection(
                    produto.IdProduto,
                    produto.Nome,
                    produto.Preco,
                    quantidade
                );

                itensVenda.Add(vendaItem);

                boxCodProduto.Clear();
                boxQuantidade.Clear();
                blockNomeProduto.Text = string.Empty;

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
            if (itensVenda.Count == 0)
            {
                MessageBox.Show("Adicione ao menos um produto à venda.");
                return;
            }

            if (!decimal.TryParse(blockTotal.Text, out decimal total) || total <= 0)
            {
                MessageBox.Show("Total inválido.");
                return;
            }

            if (boxPagamento.SelectedItem == null || boxStatus.SelectedItem == null)
            {
                MessageBox.Show("Selecione forma de pagamento e status.");
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
            }

            if (status)
            {
                MessageBox.Show("Venda salva com sucesso!");
                Close();
            }
            else
            {
                MessageBox.Show("Erro ao salvar venda!", "ERRO",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // ================= REMOVER ITEM =================
        private void BtnRemoverSelecionado(object sender, RoutedEventArgs e)
        {
            var item = gridVendaProduto.SelectedItem as NovaVendaCollection;

            if (item == null)
            {
                MessageBox.Show("Selecione um item no grid.");
                return;
            }

            itensVenda.Remove(item);

            var produto = produtos.FirstOrDefault(p => p.IdProduto == item.ProdutoId);
            if (produto != null)
                produtos.Remove(produto);

            AtualizarTotal();
        }
    }
}