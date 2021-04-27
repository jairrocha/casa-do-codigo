using CasaDoCodigo.Models;
using CasaDoCodigo.Models.ViewModels;
using CasaDoCodigo.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasaDoCodigo.Controllers
{
    public class PedidoController : Controller
    {

        private readonly IProdutoRepository produtoRepository;
        private readonly IPedidoRepository pedidoRepository;
        private readonly IItemPedidoRepository itemPedidoRepository;

        public PedidoController(IProdutoRepository produtoRepository,
                                IPedidoRepository pedidoRepository,
                                IItemPedidoRepository itemPedidoRepository)
        {
            this.produtoRepository = produtoRepository;
            this.pedidoRepository = pedidoRepository;
            this.itemPedidoRepository = itemPedidoRepository;
        }

        public IActionResult Carrossel()
        {

            return View(produtoRepository.GetProdutos());
        }
        public IActionResult Carrinho(string codigo)
        {
            if (!String.IsNullOrEmpty(codigo))
            {
                pedidoRepository.AddItem(codigo);
            }


            IList<ItemPedido> items =
                              pedidoRepository.GetPedido().Itens;

            var carrinhoViewModel = new CarrinhoViewModel(items);

            return View(carrinhoViewModel);

        }
        public IActionResult Cadastro()
        {

            var pedido = pedidoRepository.GetPedido();

            if (pedido == null)
            {
                return RedirectToAction("Carrossel");
            }

            return View(pedido.Cadastro);
        }

        [HttpPost]
        [ValidateAntiForgeryToken] //protege de ataque CSRF(Cross-site request forgery)
        public IActionResult Resumo(Cadastro cadastro)
        {

            if (ModelState.IsValid)
            {
                return View(pedidoRepository.UpdateCadastro(cadastro));

            }

            return RedirectToAction("Cadastro");

        }

        [HttpPost]
        [ValidateAntiForgeryToken] /*CodMud1> Protegendo método do ajax (o ajax deve enviar token para acessar 
                                                               'verifique a mudança feita no ajax.') CodMud1>*/
        public UpdateQuantidadeResponse UpdateQuantidade([FromBody] ItemPedido itemPedido)
        {
            return pedidoRepository.UpdateQuantidade(itemPedido);
        }


    }
}
