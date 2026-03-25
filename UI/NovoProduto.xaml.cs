using Dominio;
using Dominio.Enum;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using UI.Model;

namespace UI
{
    public partial class NovoProduto : Window
    {
        ProdutoModel _pModel = new ProdutoModel();
        private Produtos _produto;

        public NovoProduto()
        {
            InitializeComponent();

            boxCategoria.ItemsSource = Enum.GetValues(typeof(CategoriaProduto));
            boxGenero.ItemsSource = Enum.GetValues(typeof(Genero));
            boxCondicao.ItemsSource = Enum.GetValues(typeof(Condicao));
        }

        public NovoProduto(Produtos produto) : this()
        {
            _produto = produto;

            if (_produto != null)
            {
                boxNome.Text = _produto.Nome;
                boxMarca.Text = _produto.Marca;

                boxDescricao.Text = _produto.Descricao;
                boxCodBarras.Text = _produto.CodigoBarras;

                boxPrecoCusto.Text = _produto.Custo?.ToString() ?? "";
                boxPrecoVenda.Text = _produto.Preco.ToString();

                boxAtivo.IsChecked = _produto.Ativo;

                boxTamanho.Text = _produto.Tamanho;

                boxEstoque.Text = _produto.QuantidadeEstoque.ToString();

                if (!string.IsNullOrEmpty(_produto.Categoria))
                    boxCategoria.SelectedItem = Enum.Parse(typeof(CategoriaProduto), _produto.Categoria);

                if (!string.IsNullOrEmpty(_produto.Genero))
                    boxGenero.SelectedItem = Enum.Parse(typeof(Genero), _produto.Genero);

                if (!string.IsNullOrEmpty(_produto.Condicao))
                    boxCondicao.SelectedItem = Enum.Parse(typeof(Condicao), _produto.Condicao);
            }
        }

        private async void btnConfimarProduto_Click(object sender, RoutedEventArgs e)
        {
            await btnConfirmarProduto(sender, e);
        }

        private async Task btnConfirmarProduto(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(boxNome.Text))
            {
                MessageBox.Show("Nome é obrigatório.");
                return;
            }

            if (boxNome.Text.Length > 100)
            {
                MessageBox.Show("Nome deve ter no máximo 100 caracteres.");
                return;
            }

            if (!decimal.TryParse(boxPrecoVenda.Text, out var preco) || preco <= 0)
            {
                MessageBox.Show("Preço de venda inválido. Deve ser maior que zero.");
                return;
            }

            decimal.TryParse(boxPrecoCusto.Text, out var custo);

            if (custo < 0)
            {
                MessageBox.Show("Preço de custo não pode ser negativo.");
                return;
            }

            if (!int.TryParse(boxEstoque.Text, out var estoque) || estoque < 0)
            {
                MessageBox.Show("Estoque inválido. Deve ser um número inteiro maior ou igual a zero.");
                return;
            }

            if (!string.IsNullOrWhiteSpace(boxCodBarras.Text))
            {
                var codBarras = boxCodBarras.Text.Trim();
                if (!System.Text.RegularExpressions.Regex.IsMatch(codBarras, @"^\d+$"))
                {
                    MessageBox.Show("Código de barras deve conter apenas números.");
                    return;
                }

                if (codBarras.Length != 8 && codBarras.Length != 12 && codBarras.Length != 13 && codBarras.Length != 14)
                {
                    MessageBox.Show("Código de barras inválido. Deve ter 8, 12, 13 ou 14 dígitos.");
                    return;
                }
            }

            if (boxCategoria.SelectedItem == null ||
                boxGenero.SelectedItem == null ||
                boxCondicao.SelectedItem == null)
            {
                MessageBox.Show("Selecione Categoria, Gênero e Condição.");
                return;
            }

            var categoria = boxCategoria.SelectedItem.ToString();
            var genero = boxGenero.SelectedItem.ToString();
            var condicao = boxCondicao.SelectedItem.ToString();

            // NOVO PRODUTO
            if (_produto == null)
            {
                await _pModel.AdicionarProduto(
                    nome: boxNome.Text,
                    descricao: boxDescricao.Text,
                    categoria: categoria,
                    preco: preco,
                    custo: custo == 0 ? (decimal?)null : custo,
                    quantidadeEstoque: estoque,
                    marca: boxMarca.Text,
                    tamanho: boxTamanho.Text,
                    genero: genero,
                    condicao: condicao,
                    codigoBarras: boxCodBarras.Text,
                    ativo: boxAtivo.IsChecked == true
                );

                MessageBox.Show("Produto cadastrado com sucesso!");
            }
            // EDITAR PRODUTO
            else
            {
                await _pModel.EditarProduto(
                    id: _produto.IdProduto,
                    nome: boxNome.Text,
                    descricao: boxDescricao.Text,
                    categoria: categoria,
                    preco: preco,
                    custo: custo == 0 ? (decimal?)null : custo,
                    quantidadeEstoque: estoque,
                    marca: boxMarca.Text,
                    tamanho: boxTamanho.Text,
                    genero: genero,
                    condicao: condicao,
                    codigoBarras: boxCodBarras.Text,
                    ativo: boxAtivo.IsChecked == true
                );

                MessageBox.Show("Produto atualizado com sucesso!");
            }

            this.Close();
        }

        private void ApenasNumero(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !decimal.TryParse(e.Text, out _);
        }

        private void ApenasInteiro(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !int.TryParse(e.Text, out _);
        }
    }
}