using Dominio;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using UI.Model;

namespace UI
{
    public partial class NovaVenda : Window
    {
        VendaModel vModel = new VendaModel();
        List<Produtos> produtos = new List<Produtos>();

        private int idVendaEdicao = -1;
        private int _clienteId;

        public NovaVenda(int clienteId, string nomeCliente, string cpfCliente)
        {
            InitializeComponent();

            _clienteId = clienteId;

            blockNomeCliente.Text = nomeCliente;
            blockCpfCliente.Text = cpfCliente;
        }

        //  EDIÇÃO
        public NovaVenda(int idVenda, int clienteId, string nomeCliente, string cpfCliente, decimal valorTotal, List<Produtos> itens = null)
        {
            InitializeComponent();

            idVendaEdicao = idVenda;
            _clienteId = clienteId;

            blockNomeCliente.Text = nomeCliente;
            blockCpfCliente.Text = cpfCliente;
            blockTotal.Text = valorTotal.ToString();

            produtos = itens != null ? new List<Produtos>(itens) : new List<Produtos>();

            ConfirmarVenda.Content = "Atualizar Venda";
        }

        private async void boxCodProduto_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var produto = await vModel.ProcurarProduto(int.Parse(boxCodProduto.Text));

                if (produto != null)
                {
                    blockNomeProduto.Text = produto.Descricao;
                }
                else
                {
                    MessageBox.Show("Produto não encontrado!");
                }
            }
        }

        private async void boxQuantidade_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                int codigoProduto = int.Parse(boxCodProduto.Text);

                Produtos produto = await vModel.ProcurarProduto(codigoProduto);

                if (produto == null)
                {
                    MessageBox.Show("Produto inválido!");
                    return;
                }

                produtos.Add(produto);

                var quantidade = int.Parse(boxQuantidade.Text);

                NovaVendaCollection vendas = new NovaVendaCollection(
                    produto.IdProduto,
                    produto.Descricao,
                    produto.Preco,
                    quantidade
                );

                gridVendaProduto.Items.Add(vendas);

                blockTotal.Text = (
                    decimal.Parse(blockTotal.Text) + vendas.Total
                ).ToString();
            }
        }

        private async void btnConfirmarVenda(object sender, RoutedEventArgs e)
        {
            if (!decimal.TryParse(blockTotal.Text, out decimal total))
            {
                MessageBox.Show("Total inválido.");
                return;
            }

            bool status;

            if (idVendaEdicao == -1)
            {
                status = await vModel.NovaVenda(
                    _clienteId,
                    total,
                    produtos
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
                    produtos
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
    }

    class NovaVendaCollection
    {
        public int ProdutoId { get; set; }
        public string ProdutoNome { get; set; }
        public decimal Preco { get; set; }
        public int QuantidadeProduto { get; set; }
        public decimal Total { get; set; }

        public NovaVendaCollection(int produtoId, string produtoNome, decimal preco, int quantidadeProduto)
        {
            ProdutoId = produtoId;
            ProdutoNome = produtoNome;
            Preco = preco;
            QuantidadeProduto = quantidadeProduto;
            Total = quantidadeProduto * preco;
        }
    }
}