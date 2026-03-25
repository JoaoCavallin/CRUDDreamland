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
        private ClienteRepositorio _clienteRepositorio = new ClienteRepositorio();

        // ================= PRODUTO =================
        public async Task<Produtos> ProcurarProduto(int id)
        {
            return await _produtoRepositorio.GetByIdAsync(id);
        }

        // ================= LISTAR PRODUTOS PRA GRID =================
        public async Task<List<NovaVendaCollection>> ListarProdutosParaVenda()
        {
            var produtos = await _produtoRepositorio.GetAllAsync();

            var lista = new List<NovaVendaCollection>();

            foreach (var p in produtos)
            {
                lista.Add(new NovaVendaCollection
                {
                    ProdutoId = p.IdProduto,
                    ProdutoNome = p.Nome,
                    Preco = p.Preco,
                    QuantidadeProduto = 1,
                    Total = p.Preco
                });
            }

            return lista;
        }

        // ================= LISTAR VENDAS =================
        public async Task<Vendas[]> ListarVendas()
        {
            var vendas = await _vendaRepositorio.GetAllAsync();

            foreach (var venda in vendas)
            {
                var cliente = await _clienteRepositorio.GetByIdAsync(venda.ClienteId);

                if (cliente != null)
                {
                    venda.ClienteNome = cliente.ClienteNome;
                    venda.ClienteDocumento = cliente.ClienteDocumento;
                }
            }

            return vendas;
        }

        // ================= NOVA VENDA =================
        public async Task<bool> NovaVenda(
            int clienteId,
            decimal valorTotal,
            List<Produtos> produtos,
            string formaPagamento = null,
            string statusVenda = null)
        {
            try
            {
                var cliente = await _clienteRepositorio.GetByIdAsync(clienteId);

                if (cliente == null)
                    return false;

                Vendas venda = new Vendas
                {
                    ClienteId = clienteId,
                    ValorTotal = valorTotal,
                    FormaPagamento = formaPagamento,
                    StatusVenda = string.IsNullOrEmpty(statusVenda) ? "Concluida" : statusVenda,
                    DataVenda = DateTime.Now
                };

                _vendaRepositorio.Add(venda);
                await _vendaRepositorio.SaveChangesAsync();

                List<ProdutoVendas> vendaProdutos = new List<ProdutoVendas>();

                foreach (Produtos produto in produtos)
                {
                    int quantidade = 1;
                    decimal precoUnitario = produto.Preco;

                    vendaProdutos.Add(new ProdutoVendas
                    {
                        ProdutoId = produto.IdProduto,
                        VendaId = venda.IdVenda,
                        Quantidade = quantidade,
                        PrecoUnitario = precoUnitario,
                        Subtotal = precoUnitario * quantidade
                    });
                }

                _vendaProdutoRepositorio.AddRange(vendaProdutos);

                return await _vendaProdutoRepositorio.SaveChangesAsync();
            }
            catch
            {
                return false;
            }
        }

        // ================= EXCLUIR VENDA =================
        public async Task<bool> ExcluirVenda(int idVenda)
        {
            try
            {
                var itensVenda = await _vendaProdutoRepositorio.GetByVendaIdAsync(idVenda);

                if (itensVenda != null && itensVenda.Count > 0)
                {
                    _vendaProdutoRepositorio.RemoveRange(itensVenda);
                    await _vendaProdutoRepositorio.SaveChangesAsync();
                }

                var venda = await _vendaRepositorio.GetByIdAsync(idVenda);

                if (venda == null)
                    return false;

                _vendaRepositorio.Remove(venda);

                return await _vendaRepositorio.SaveChangesAsync();
            }
            catch
            {
                return false;
            }
        }

        // ================= EDITAR VENDA =================
        public async Task<bool> EditarVenda(
            int idVenda,
            int clienteId,
            decimal valorTotal,
            List<Produtos> produtos,
            string formaPagamento = null,
            string statusVenda = null)
        {
            try
            {
                var vendaExistente = await _vendaRepositorio.GetByIdAsync(idVenda);

                if (vendaExistente == null)
                    return false;

                var cliente = await _clienteRepositorio.GetByIdAsync(clienteId);

                if (cliente == null)
                    return false;

                vendaExistente.ClienteId = clienteId;
                vendaExistente.ValorTotal = valorTotal;
                vendaExistente.FormaPagamento = formaPagamento;
                vendaExistente.StatusVenda = string.IsNullOrEmpty(statusVenda) ? "Concluida" : statusVenda;

                _vendaRepositorio.Update(vendaExistente);
                await _vendaRepositorio.SaveChangesAsync();

                var itensVendaAntigos = await _vendaProdutoRepositorio.GetByVendaIdAsync(idVenda);

                if (itensVendaAntigos != null && itensVendaAntigos.Count > 0)
                {
                    _vendaProdutoRepositorio.RemoveRange(itensVendaAntigos);
                    await _vendaProdutoRepositorio.SaveChangesAsync();
                }

                List<ProdutoVendas> vendaProdutosNovos = new List<ProdutoVendas>();

                foreach (Produtos produto in produtos)
                {
                    int quantidade = 1;
                    decimal precoUnitario = produto.Preco;

                    vendaProdutosNovos.Add(new ProdutoVendas
                    {
                        ProdutoId = produto.IdProduto,
                        VendaId = idVenda,
                        Quantidade = quantidade,
                        PrecoUnitario = precoUnitario,
                        Subtotal = precoUnitario * quantidade
                    });
                }

                _vendaProdutoRepositorio.AddRange(vendaProdutosNovos);

                return await _vendaProdutoRepositorio.SaveChangesAsync();
            }
            catch
            {
                return false;
            }
        }
    }

    // ================= CLASSE DO GRID =================
    public class NovaVendaCollection
    {
        public int ProdutoId { get; set; }
        public string ProdutoNome { get; set; }
        public decimal Preco { get; set; }
        public int QuantidadeProduto { get; set; }
        public decimal Total { get; set; }
    }
}