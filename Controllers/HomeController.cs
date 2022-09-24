using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rocastone.Data;
using Rocastone.Models;
using Rocastone.Models.ViewModels;
using Rocastone.Utilidades;
using System.Diagnostics;

namespace Rocastone.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            //Para ver la lista de productos y categorias en el index del HomeController
            HomeViewModel homeViewModel = new HomeViewModel()
            {
                Productos = _context.Productos.Include(c => c.Categoria).Include(t => t.TipoAplicacion),
                Categorias = _context.Categorias
            };
            return View(homeViewModel);
        }

        //GET Detalle
        public async Task<IActionResult> Detalle(int? id)
        {
            List<CarroCompras> carroComprasLista = new List<CarroCompras>(); //aqui se almacenaran los productos agregados al carro de compras
            if (HttpContext.Session.GetObject<IEnumerable<CarroCompras>>(WebConstantes.SessionCarroCompras) != null
                && HttpContext.Session.GetObject<IEnumerable<CarroCompras>>(WebConstantes.SessionCarroCompras).Count() > 0)
            {
                carroComprasLista = HttpContext.Session.GetObject<List<CarroCompras>>(WebConstantes.SessionCarroCompras);
            }

            //instancio el objeto  ProductoViewModel productoVM ya  que trabajare con objetos de varios modelos dif.
            DetalleViewModel detalleViewModel = new DetalleViewModel()
            {
                Producto = await _context.Productos.Include(c => c.Categoria).Include(t => t.TipoAplicacion)
                                                    .FirstOrDefaultAsync(p => p.Id == id),
                ExisteEnCarro = false,
            };

            //Hago el foreach para que si el producto ya esta en el carro me aparezca el boton "remover del carro"
            foreach (var item in carroComprasLista)
            {
                if (item.ProductoId == id)
                {
                    detalleViewModel.ExisteEnCarro = true;
                }
            }

            return View(detalleViewModel);
        }

        [HttpPost, ActionName("Detalle")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DetallePost(int id)
        {
            List<CarroCompras> carroComprasLista = new List<CarroCompras>(); //aqui se almacenaran los productos agregados al carro de compras
            if (HttpContext.Session.GetObject<IEnumerable<CarroCompras>>(WebConstantes.SessionCarroCompras) != null
                && HttpContext.Session.GetObject<IEnumerable<CarroCompras>>(WebConstantes.SessionCarroCompras).Count() > 0)
            {
                carroComprasLista = HttpContext.Session.GetObject<List<CarroCompras>>(WebConstantes.SessionCarroCompras);
            }
            carroComprasLista.Add(new CarroCompras
            {
                ProductoId = id,
            });
            HttpContext.Session.SetObject(WebConstantes.SessionCarroCompras, carroComprasLista); //modifico el carrito con sus nuevos valores
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> RemoverDeCarro(int? id)
        {
            List<CarroCompras> carroComprasLista = new List<CarroCompras>(); //aqui se almacenaran los productos agregados al carro de compras
            if (HttpContext.Session.GetObject<IEnumerable<CarroCompras>>(WebConstantes.SessionCarroCompras) != null
                && HttpContext.Session.GetObject<IEnumerable<CarroCompras>>(WebConstantes.SessionCarroCompras).Count() > 0)
            {
                carroComprasLista = HttpContext.Session.GetObject<List<CarroCompras>>(WebConstantes.SessionCarroCompras);
            }
            //creo una variable para constatar que el producto a remover existe
            var productoRemover = carroComprasLista.FirstOrDefault(item => item.ProductoId == id);
            if (productoRemover != null)
            {
                carroComprasLista.Remove(productoRemover);
            }


            HttpContext.Session.SetObject(WebConstantes.SessionCarroCompras, carroComprasLista); //modifico el carrito con sus nuevos valores
            return RedirectToAction(nameof(Index));
        }




        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}