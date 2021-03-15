using Microsoft.EntityFrameworkCore;
using RestauranteDominio.Enums;
using RestauranteRepositorios.Services.ServiceMesa;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using RestauranteDominio;

namespace RestauranteRepositorios.Services
{
    public class ComandaService
    {
        private readonly RestauranteContexto _contexto;
        private readonly MesaService _mesaService;
        private readonly ProdutoService _produtoService;

        public ComandaService(RestauranteContexto contexto, MesaService mesaService, ProdutoService produtoService)
        {
            _contexto = contexto;
            _mesaService = mesaService;
            _produtoService = produtoService;
        }
        
        //ADICIONA NOVA COMANDA
        public async Task AdicionarComanda(AdicionarComandaModel model)
        {
            if (model.QtdePessoasMesa > 4)
                throw new Exception("Quantidade máxima de 4 pessoas por mesa!");

            var comanda = new Comanda()
            {
                DataHoraEntrada = model.DataHoraEntrada,
                Valor = model.Valor,
                ComandaPaga = false,
                QtdePessoasMesa = model.QtdePessoasMesa,
                MesaId = model.MesaId
            };

            await _mesaService.OcuparMesa(comanda.MesaId);

            _contexto.Comanda.Add(comanda);
            await _contexto.SaveChangesAsync();

            int comandaId = comanda.ComandaId;

            var produto = await _produtoService.ObterProduto(1);
            _ = produto ?? throw new Exception("Produto inexistente");
            var valorTotalPedido = produto.ValorProduto * model.QtdePessoasMesa;

            var rodizio = new Pedido()
            {
                ProdutoId = 1,
                ComandaId = comandaId,
                QtdeProduto = model.QtdePessoasMesa,
                ValorPedido = valorTotalPedido,
                StatusPedidoId = (int)StatusPedidoEnum.Realizado
            };

            _contexto.Pedido.Add(rodizio);
            await _contexto.SaveChangesAsync();

            await AtualizarValorComanda(comandaId);
        }

        //ENCERRA E PAGA COMANDA
        public async Task EncerrarComanda(int comandaId)
        {
            var comanda = _contexto.Comanda
                        .Where(c => c.ComandaId == comandaId && c.ComandaPaga == false)
                        .OrderBy(c => c.ComandaId)
                        .FirstOrDefault();

            _ = comanda ?? throw new Exception("Comanda já esta paga ou inexistente!");

            await _mesaService.DesocuparMesa(comanda.MesaId);

            comanda.DataHoraSaida = DateTime.Now;
            comanda.ComandaPaga = true;

            await _contexto.SaveChangesAsync();  
        }

        //CANCELA COMANDA
        public async Task CancelarComanda(int comandaId)
        {
            var comanda = _contexto.Comanda
                        .Where(c => c.ComandaId == comandaId && c.ComandaPaga == false)
                        .OrderBy(c => c.ComandaId)
                        .LastOrDefault();

            _ = comanda ?? throw new Exception("Comanda já esta paga ou inexistente!");
            comanda.DataHoraSaida = DateTime.Now;
            comanda.Valor = 0;
            comanda.ComandaPaga = true;

            await _contexto.SaveChangesAsync();

            await _mesaService.DesocuparMesa(comanda.MesaId);
        }

        //BUSCA A COMANDAS PAGAS
        public async Task<ComandaFinalizadaModel> BuscarComandaPaga(int comandaId)
        {

            var comanda = await _contexto.Comanda
                        .Where(c => c.ComandaId == comandaId)
                        .Include(c => c.Pedidos)
                        .ThenInclude(p => p.StatusPedido)
                        .Include(c => c.Pedidos)
                        .ThenInclude(p => p.Produto)
                        .Select(comanda => new
                        {
                            comanda.ComandaId,
                            comanda.MesaId,
                            comanda.DataHoraEntrada,
                            comanda.DataHoraSaida,
                            comanda.Valor,
                            comanda.ComandaPaga,
                            comanda.QtdePessoasMesa,
                            comanda.Pedidos
                        }).FirstOrDefaultAsync();

            var res = new ComandaFinalizadaModel
            {
                ComandaId = comanda.ComandaId,
                MesaId = comanda.MesaId,
                DataHoraEntrada = comanda.DataHoraEntrada,
                DataHoraSaida = comanda.DataHoraSaida,
                Valor = comanda.Valor,
                ComandaPaga = comanda.ComandaPaga,
                QtdePessoasMesa = comanda.QtdePessoasMesa,
            };

            res.Pedidos = comanda.Pedidos.Select(p => new BuscarPedidoModel
            {
                PedidoId = p.PedidoId,
                ProdutoId = p.ProdutoId,
                Produto = new ListarProdutosModel
                {
                    ProdutoId = p.ProdutoId,
                    NomeProduto = p.Produto.NomeProduto,
                    ValorProduto = p.Produto.ValorProduto,
                    QtdePermitida = p.Produto.QtdePermitida,
                },
                ComandaId = p.ComandaId,
                QtdeProduto = p.QtdeProduto,
                ValorPedido = p.ValorPedido,
                StatusPedido = p.StatusPedido
            }).ToList();

            _ = comanda ?? throw new Exception("Comanda inexistente!");

            return res;
        }


        //BUSCA COMANDAS NÃO FINALIZADAS
        public async Task<ComandaModel> BuscarComandaAberta(int comandaId)
        {
             var comanda = await _contexto.Comanda
                        .Where(c => c.ComandaId == comandaId)
                        .Include(c => c.Pedidos)
                        .ThenInclude(p => p.StatusPedido)
                        .Include(c => c.Pedidos)
                        .ThenInclude(p => p.Produto)
                        .Select(comanda => new 
                        {
                            comanda.ComandaId,
                            comanda.MesaId,
                            comanda.DataHoraEntrada,
                            comanda.Valor,
                            comanda.ComandaPaga,
                            comanda.QtdePessoasMesa,
                            comanda.Pedidos
                        }).OrderBy(c => c.ComandaId).FirstOrDefaultAsync();

            var res = new ComandaModel
            {
                ComandaId = comanda.ComandaId,
                MesaId = comanda.MesaId,
                DataHoraEntrada = comanda.DataHoraEntrada,
                Valor = comanda.Valor,
                ComandaPaga = comanda.ComandaPaga,
                QtdePessoasMesa = comanda.QtdePessoasMesa,
            };

            res.Pedidos = comanda.Pedidos.Select(p => new BuscarPedidoModel {
                PedidoId = p.PedidoId,
                ProdutoId = p.ProdutoId,
                Produto = new ListarProdutosModel
                {
                    ProdutoId = p.ProdutoId,
                    NomeProduto = p.Produto.NomeProduto,
                    ValorProduto = p.Produto.ValorProduto,
                    QtdePermitida = p.Produto.QtdePermitida,
                },
                ComandaId = p.ComandaId,
                QtdeProduto = p.QtdeProduto,
                ValorPedido = p.ValorPedido,
                StatusPedido = p.StatusPedido
            }).ToList();

            _ = comanda ?? throw new Exception("Comanda inexistente!");

            return res;
        }
        public async Task AtualizarValorComanda(int comandaId)
        {
            var pedidos = _contexto.Pedido
                        .Where(p => p.ComandaId == comandaId && p.StatusPedidoId == (int)StatusPedidoEnum.Realizado && p.ValorPedido > 0)
                        .Select(p => p.ValorPedido)
                        .Sum();

            var comanda = _contexto.Comanda
                        .Where(c => c.ComandaId == comandaId).FirstOrDefault();

            comanda.Valor = pedidos;

            await _contexto.SaveChangesAsync();
        }
    }
}
