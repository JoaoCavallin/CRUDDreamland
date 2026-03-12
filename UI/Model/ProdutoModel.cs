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

        public async Task<Produto[]> ListarProdutos()
        {
            return await _produtoRepositorio.GetAllAsync();
        }

        public async void AdicionarProduto
        (
            string descricao,
            UnidadeMedida unidadeDeMedida,
            string codBarras,
            decimal precoCusto,
            decimal precoVenda,
            bool ativo,
            ProdutoGrupo produtoGrupo
        )
        {
            Produto produto = new Produto(descricao, unidadeDeMedida, codBarras, precoCusto, precoVenda, DateTime.Now, ativo, produtoGrupo);

            _produtoRepositorio.Add(produto);
            await _produtoRepositorio.SaveChangesAsync();
        }

        public void ExcluirProduto(int id)
        {
            using (var context = new Context())
            {
                var produto = context.Produtos.FirstOrDefault(p => p.Id == id);

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
        public async void EditarProduto
        (
            int Id,
            string descricao,
            UnidadeMedida unidadeDeMedida,
            string codBarras,
            decimal precoCusto,
            decimal precoVenda,
            bool ativo,
            ProdutoGrupo produtoGrupo
        )
        {
            Produto atualizarProduto = new Produto(Id, descricao, unidadeDeMedida, codBarras, precoCusto, precoVenda, DateTime.Now, ativo, produtoGrupo);

            _produtoRepositorio.Update(atualizarProduto);
            await _produtoRepositorio.SaveChangesAsync();
        }


    }
}
