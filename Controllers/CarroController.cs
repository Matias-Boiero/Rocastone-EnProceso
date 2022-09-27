using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Rocastone.Data;
using Rocastone.Models;
using Rocastone.Models.ViewModels;
using Rocastone.Utilidades;
using System.Security.Claims;
using System.Text;

namespace Rocastone.Controllers
{
    [Authorize] //solo puede ver losproductos que agrego el usuario si se loguea
    public class CarroController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHost; //para utilizar templates
        private readonly IEmailSender _emailSender;

        public CarroController(ApplicationDbContext context, IWebHostEnvironment webHost, IEmailSender emailSender)
        {
            _context = context;
            _webHost = webHost;
            _emailSender = emailSender;
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

        //GET
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Resumen")]

        public async Task<IActionResult> ResumenPost(ProductoUsuarioViewModel productoUsuarioVM)
        {
            //Traigo el template del wwwroot que sera la plantilla que me llegara por mail cuando el usuario haga la orden
            var rutaTemplate = _webHost.WebRootPath + Path.DirectorySeparatorChar.ToString()
                                 + "templates" + Path.DirectorySeparatorChar.ToString()
                                 + "PlantillaOrden.html";

            var subject = "Nueva orden";
            string HtmlBody = "";

            //Abro el template y lo leo
            using (StreamReader sr = System.IO.File.OpenText(rutaTemplate))
            {
                HtmlBody = sr.ReadToEnd();
            }

            StringBuilder productoListaSB = new StringBuilder();
            foreach (var prod in productoUsuarioVM.ProductoLista)
            {
                //Creo Html en el foreach con el .Append
                productoListaSB.Append($" - Nombre: { prod.Nombre } <span style='font-size:14px;'> (ID: { prod.Id })</span><br />");

                string MessageBody = string.Format(HtmlBody, productoUsuarioVM.ApplicationUser.Nombre,
                    productoUsuarioVM.ApplicationUser.Apellido,
                    productoUsuarioVM.ApplicationUser.Email,
                    productoUsuarioVM.ApplicationUser.Telefono,
                    productoUsuarioVM.ApplicationUser.Provincia,
                    productoUsuarioVM.ApplicationUser.Ciudad,
                    productoUsuarioVM.ApplicationUser.Direccion,
                    productoUsuarioVM.ApplicationUser.CodigoPostal,
                    productoListaSB.ToString());
                await _emailSender.SendEmailAsync(WebConstantes.EmailAdmin, subject, MessageBody);
            }

            return RedirectToAction(nameof(Confirmacion));
        }

        public IActionResult Confirmacion()
        {
            HttpContext.Session.Clear(); //Limpio la session para quitar los productos del carro de compras al enviar la orden.
            return View();
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
