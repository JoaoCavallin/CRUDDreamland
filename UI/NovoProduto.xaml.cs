using Dominio;
using Dominio.Enum;
using System;
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

            // 🔥 Popular ComboBoxes com enums
            boxCategoria.ItemsSource = Enum.GetValues(typeof(CategoriaProduto));
            boxGenero.ItemsSource = Enum.GetValues(typeof(Genero));
            boxCondicao.ItemsSource = Enum.GetValues(typeof(Condicao));
        }

        public NovoProduto(Produtos produto) : this()
        {
            _produto = produto;

            if (_produto != null)
            {
                boxDescricao.Text = _produto.Descricao;
                boxCodBarras.Text = _produto.CodigoBarras;

                boxPrecoCusto.Text = _produto.Custo?.ToString() ?? "";
                boxPrecoVenda.Text = _produto.Preco.ToString();

                boxAtivo.IsChecked = _produto.Ativo;

                // Setar enums no ComboBox
                if (!string.IsNullOrEmpty(_produto.Categoria))
                    boxCategoria.SelectedItem = Enum.Parse(typeof(CategoriaProduto), _produto.Categoria);

                if (!string.IsNullOrEmpty(_produto.Genero))
                    boxGenero.SelectedItem = Enum.Parse(typeof(Genero), _produto.Genero);

                if (!string.IsNullOrEmpty(_produto.Condicao))
                    boxCondicao.SelectedItem = Enum.Parse(typeof(Condicao), _produto.Condicao);
            }
        }

        private void btnConfirmarProduto(object sender, RoutedEventArgs e)
        {
            //   VALIDAÇÃO
            if (string.IsNullOrWhiteSpace(boxNome.Text))
            {
                MessageBox.Show("Nome é obrigatório");
                return;
            }

            if (!decimal.TryParse(boxPrecoVenda.Text, out var preco))
            {
                MessageBox.Show("Preço de venda inválido");
                return;
            }

            decimal.TryParse(boxPrecoCusto.Text, out var custo);

            if (boxCategoria.SelectedItem == null ||
                boxGenero.SelectedItem == null ||
                boxCondicao.SelectedItem == null)
            {
                MessageBox.Show("Selecione Categoria, Gênero e Condição");
                return;
            }

            var categoria = boxCategoria.SelectedItem.ToString();
            var genero = boxGenero.SelectedItem.ToString();
            var condicao = boxCondicao.SelectedItem.ToString();

            //  NOVO PRODUTO
            if (_produto == null)
            {
                _pModel.AdicionarProduto(
                    nome: boxNome.Text,
                    descricao: boxDescricao.Text,
                    categoria: categoria,
                    preco: preco,
                    custo: custo == 0 ? (decimal?)null : custo,
                    quantidadeEstoque: 0,
                    marca: boxMarca.Text,
                    tamanho: "",
                    genero: genero,
                    condicao: condicao,
                    codigoBarras: boxCodBarras.Text,
                    ativo: boxAtivo.IsChecked == true
                );

                MessageBox.Show("Produto cadastrado com sucesso!");
            }
            else
            {
                //  EDITAR PRODUTO
                _pModel.EditarProduto(
                    id: _produto.IdProduto,
                    nome: boxNome.Text,
                    descricao: boxDescricao.Text,
                    categoria: categoria,
                    preco: preco,
                    custo: custo == 0 ? (decimal?)null : custo,
                    quantidadeEstoque: _produto.QuantidadeEstoque,
                    marca: boxMarca.Text,
                    tamanho: _produto.Tamanho ?? "",
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


    }
}