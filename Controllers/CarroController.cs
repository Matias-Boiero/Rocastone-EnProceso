using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rocastone.Data;
using Rocastone.Models;
using Rocastone.Models.ViewModels;
using Rocastone.Utilidades;
using System.Security.Claims;

namespace Rocastone.Controllers
{
    [Authorize] //solo puede ver losproductos que agrego el usuario si se loguea
    public class CarroController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CarroController(ApplicationDbContext context)
        {
            _context = context;
        }

        //Get
        public IActionResult Index() //Obtengo la vista del carro de compras al hacer click sobre él
        {
            List<CarroCompras> carroCompraList = new List<CarroCompras>(); //Creo una lista de compras de productos obteniendo  Id del producto

            if (HttpContext.Session.GetObject<IEnumerable<CarroCompras>>(WebConstantes.SessionCarroCompras) != null &&
                HttpContext.Session.GetObject<IEnumerable<CarroCompras>>(WebConstantes.SessionCarroCompras).Count() > 0)
            {
                carroCompraList = HttpContext.Session.GetObject<List<CarroCompras>>(WebConstantes.SessionCarroCompras);
            }

            List<int> prodEnCarro = carroCompraList.Select(c => c.ProductoId).ToList();
            IEnumerable<Producto> prodList = _context.Productos.Where(p => prodEnCarro.Contains(p.Id));

            return View(prodList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Index")]

        public IActionResult IndexPost() //Obtengo la vista del carro de compras al hacer click sobre él
        {
            return RedirectToAction("Resumen");
        }

        public IActionResult Resumen()
        {
            //Traer al usuario conectado de la siguiente manera
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            //Traigo los datos de la session
            List<CarroCompras> carroCompraList = new List<CarroCompras>(); //Creo una lista de compras de productos obteniendo  Id del producto

            if (HttpContext.Session.GetObject<IEnumerable<CarroCompras>>(WebConstantes.SessionCarroCompras) != null &&
                HttpContext.Session.GetObject<IEnumerable<CarroCompras>>(WebConstantes.SessionCarroCompras).Count() > 0)
            {
                carroCompraList = HttpContext.Session.GetObject<List<CarroCompras>>(WebConstantes.SessionCarroCompras);
            }

            List<int> prodEnCarro = carroCompraList.Select(c => c.ProductoId).ToList();
            IEnumerable<Producto> prodList = _context.Productos.Where(p => prodEnCarro.Contains(p.Id));

            //Traigo los datos de usuario
            ProductoUsuarioViewModel productoUsuario = new ProductoUsuarioViewModel()
            {
                ApplicationUser = _context.Usuarios.FirstOrDefault(u => u.Id == claim.Value),
                ProductoLista = prodList,
            };
            return View(productoUsuario);

        }


        //Metodo para eliminar los productos del carro
        public async Task<IActionResult> Remover(int id)
        {
            List<CarroCompras> carroCompraList = new List<CarroCompras>(); //Creo una lista de compras de productos obteniendo el id del producto agregado

            if (HttpContext.Session.GetObject<IEnumerable<CarroCompras>>(WebConstantes.SessionCarroCompras) != null &&
                HttpContext.Session.GetObject<IEnumerable<CarroCompras>>(WebConstantes.SessionCarroCompras).Count() > 0)
            {
                carroCompraList = HttpContext.Session.GetObject<List<CarroCompras>>(WebConstantes.SessionCarroCompras);
            }

            carroCompraList.Remove(carroCompraList.FirstOrDefault(p => p.ProductoId == id));
            HttpContext.Session.SetObject(WebConstantes.SessionCarroCompras, carroCompraList); //Actualizo el carro
            return RedirectToAction("Index");
        }
    }
}
