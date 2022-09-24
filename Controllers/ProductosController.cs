using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rocastone.Data;
using Rocastone.Models;

namespace Rocastone.Controllers
{
    public class ProductosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment; //PARA EL MANEJO DE IMAGENES

        public ProductosController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Producto> lista = await _context.Productos.Include(c => c.Categoria).Include(t => t.TipoAplicacion).ToListAsync();
            return View(lista);

        }


    }
}
