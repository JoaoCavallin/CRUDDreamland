using Dominio;
using Dominio.Enum;
using Repositorio;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace UI.Model
{
    public class ProdutoModel
    {
        private ProdutoRepositorio _produtoRepositorio = new ProdutoRepositorio();

        public async Task<Produtos[]> ListarProdutos()
        {
            return await _produtoRepositorio.GetAllAsync();
        }

        public async Task AdicionarProduto(
            string nome,
            string descricao,
            string categoria,
            decimal preco,
            decimal? custo,
            int quantidadeEstoque,
            string marca,
            string tamanho,
            string genero,
            string condicao,
            string codigoBarras,
            bool ativo
        )
        {
            Produtos produto = new Produtos
            {
                Nome = nome,
                Descricao = descricao,
                Categoria = categoria,
                Preco = preco,
                Custo = custo,
                QuantidadeEstoque = quantidadeEstoque,
                Marca = marca,
                Tamanho = tamanho,
                Genero = genero,
                Condicao = condicao,
                CodigoBarras = codigoBarras,
                DataCadastro = DateTime.Now,
                Ativo = ativo
            };

            _produtoRepositorio.Add(produto);
            await _produtoRepositorio.SaveChangesAsync();
        }

        public void ExcluirProduto(int id)
        {
            using (var context = new Context())
            {
                var produto = context.Produtos.FirstOrDefault(p => p.IdProduto == id);

                if (produto != null)
                {
                    context.Produtos.Remove(produto);
                    context.SaveChanges();
                    MessageBox.Show("Produto excluído com sucesso!");
                }
                else
                {
                    MessageBox.Show("Produto não encontrado.");
                }
            }
        }
        public async Task EditarProduto(
             int id,
             string nome,
             string descricao,
             string categoria,
             decimal preco,
             decimal? custo,
             int quantidadeEstoque,
             string marca,
             string tamanho,
             string genero,
             string condicao,
             string codigoBarras,
             bool ativo
 )
        {
            Produtos atualizarProduto = new Produtos
            {
                IdProduto = id,
                Nome = nome,
                Descricao = descricao,
                Categoria = categoria,
                Preco = preco,
                Custo = custo,
                QuantidadeEstoque = quantidadeEstoque,
                Marca = marca,
                Tamanho = tamanho,
                Genero = genero,
                Condicao = condicao,
                CodigoBarras = codigoBarras,
                DataCadastro = DateTime.Now,
                Ativo = ativo
            };

            _produtoRepositorio.Update(atualizarProduto);
            await _produtoRepositorio.SaveChangesAsync();
        }


    }
}
