using Dominio;
using Dominio.Enum;
using System;
using System.Linq;
using System.Windows;
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

            boxGrupo.ItemsSource = Enum.GetValues(typeof(ProdutoGrupo)).Cast<ProdutoGrupo>();
            boxUnMedida.ItemsSource = Enum.GetValues(typeof(UnidadeMedida)).Cast<UnidadeMedida>();
        }

        public NovoProduto(Produtos produto) : this()
        {
            _produto = produto;

            if (_produto != null)
            {
                // Map existing UI fields to the new Produtos properties.
                // The UI has limited fields, so use Nome as the main text and store Descricao if available.
                boxDescricao.Text = string.IsNullOrEmpty(_produto.Descricao) ? _produto.Nome : _produto.Descricao;
                // UnidadeDeMedida no longer exists on Produtos; leave boxUnMedida as-is if present.
                if (_produto.CodigoBarras != null)
                    boxCodBarras.Text = _produto.CodigoBarras;
                boxPrecoCusto.Text = _produto.Custo?.ToString() ?? string.Empty;
                boxPrecoVenda.Text = _produto.Preco.ToString();
                boxAtivo.IsEnabled = _produto.Ativo;
                // Use Categoria to populate the group box
                if (!string.IsNullOrEmpty(_produto.Categoria))
                    boxGrupo.Text = _produto.Categoria;
            }
        }


        private void btnConfirmarProduto(object sender, RoutedEventArgs e)
        {
            if (_produto == null)
            {
                decimal.TryParse(boxPrecoCusto.Text, out var custo);
                decimal.TryParse(boxPrecoVenda.Text, out var preco);

                _pModel.AdicionarProduto(
                    nome: boxDescricao.Text,
                    descricao: boxDescricao.Text,
                    categoria: boxGrupo.Text,
                    preco: preco,
                    custo: custo == 0 ? (decimal?)null : custo,
                    quantidadeEstoque: 0,
                    marca: string.Empty,
                    tamanho: string.Empty,
                    genero: string.Empty,
                    condicao: string.Empty,
                    codigoBarras: boxCodBarras.Text,
                    ativo: boxAtivo.IsEnabled
                );

                MessageBox.Show("Produto Adicionado com sucesso!");
            }
            else
            {
                decimal.TryParse(boxPrecoCusto.Text, out var custo);
                decimal.TryParse(boxPrecoVenda.Text, out var preco);

                _pModel.EditarProduto(
                    id: _produto.IdProduto,
                    nome: boxDescricao.Text,
                    descricao: boxDescricao.Text,
                    categoria: boxGrupo.Text,
                    preco: preco,
                    custo: custo == 0 ? (decimal?)null : custo,
                    quantidadeEstoque: _produto.QuantidadeEstoque,
                    marca: _produto.Marca ?? string.Empty,
                    tamanho: _produto.Tamanho ?? string.Empty,
                    genero: _produto.Genero ?? string.Empty,
                    condicao: _produto.Condicao ?? string.Empty,
                    codigoBarras: boxCodBarras.Text,
                    ativo: boxAtivo.IsEnabled
                );

                MessageBox.Show("Produto Atualizado com sucesso!");
            }
        }
    }
}
