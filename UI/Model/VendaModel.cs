using Dominio;
using Repositorio;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UI.Model
{
    public class VendaModel
    {
        private VendaProdutoRepositorio _vendaProdutoRepositorio = new VendaProdutoRepositorio();
        private ProdutoRepositorio _produtoRepositorio = new ProdutoRepositorio();
        private VendaRepositorio _vendaRepositorio = new VendaRepositorio();

        public async Task<Produtos> ProcurarProduto(int id)
        {
            return await _produtoRepositorio.GetByIdAsync(id);
        }

        public async Task<Vendas[]> ListarVendas()
        {
            return await _vendaRepositorio.GetAllAsync();
        }

        public async Task<bool> NovaVenda(string clienteDocumento, string clienteNome, decimal total, string obs, List<Produtos> produtos)
        {
            List<ProdutoVendas> vendaProdutos = new List<ProdutoVendas>();
            Vendas venda = new Vendas(clienteDocumento, clienteNome, total, obs, DateTime.Now);

            _vendaRepositorio.Add(venda);
            await _vendaRepositorio.SaveChangesAsync();

            foreach (Produtos produto in produtos)
            {
                vendaProdutos.Add(
                    new ProdutoVendas { PrecoUnitario = venda.Total, ProdutoId = produto.IdProduto, VendaId = venda.IdVenda }
                    );
            }

            _vendaProdutoRepositorio.AddRange(vendaProdutos);

            return await _vendaProdutoRepositorio.SaveChangesAsync();
        }
    }
}