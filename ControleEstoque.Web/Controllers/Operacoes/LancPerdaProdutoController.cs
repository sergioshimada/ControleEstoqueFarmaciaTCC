using ControleEstoque.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ControleEstoque.Web.Controllers
{
    public class LancPerdaProdutoController : Controller
    {
        // GET: LancPerdaProduto
        private const int _quantMaxLinhasPorPagina = 5;

        public ActionResult Index()
        {
            ViewBag.ListaTamPag = new SelectList(new int[] { _quantMaxLinhasPorPagina, 10, 15, 20 }, _quantMaxLinhasPorPagina);


            List<PerdaProdutoModel> list = PerdaProdutoModel.RecuperarLista();

            return View(list);
        }
    }
}