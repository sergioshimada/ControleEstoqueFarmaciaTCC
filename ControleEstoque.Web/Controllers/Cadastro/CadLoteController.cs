

using ControleEstoque.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ControleEstoque.Web.Controllers { 
    public class CadLoteController : Controller
    { 
        private const int _quantMaxLinhasPorPagina = 5;
        
        public ActionResult Index()
        {
            ViewBag.ListaProdutos = GrupoProdutoModel.RecuperarLista();
            ViewBag.ListaTamPag = new SelectList(new int[] { _quantMaxLinhasPorPagina, 10, 15, 20 }, _quantMaxLinhasPorPagina);
            ViewBag.QuantMaxLinhasPorPagina = _quantMaxLinhasPorPagina;
            ViewBag.PaginaAtual = 1;

            var lista = LoteModel.RecuperarLista(ViewBag.PaginaAtual, _quantMaxLinhasPorPagina);
            var quant = LoteModel.RecuperarQuantidade();

            var difQuantPaginas = (quant % ViewBag.QuantMaxLinhasPorPagina) > 0 ? 1 : 0;
            ViewBag.QuantPaginas = (quant / ViewBag.QuantMaxLinhasPorPagina) + difQuantPaginas;

            return View(lista);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ProdutoPagina(int pagina, int tamPag)
        {
            var lista = LoteModel.RecuperarLista(pagina, tamPag);

            return Json(lista);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RecuperarProduto(int id)
        {
            return Json(LoteModel.RecuperarPeloId(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ExcluirProduto(int id)
        {
            return Json(LoteModel.ExcluirPeloId(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SalvarProduto(LoteModel model)
        {
            var resultado = "OK";
            var mensagens = new List<string>();
            var idSalvo = string.Empty;

            if (!ModelState.IsValid)
            {
                resultado = "AVISO";
                mensagens = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();
            }
            else
            {
                var id = model.Salvar();
                if (id > 0)
                {
                    idSalvo = id.ToString();
                }
                else
                {
                    resultado = "ERRO";
                }
            }

            return Json(new { Resultado = resultado, Mensagens = mensagens, IdSalvo = idSalvo });
        }
    }
}