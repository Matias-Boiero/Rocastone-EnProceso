using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rocastone.Data;
using Rocastone.Models;

namespace Rocastone.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TipoAplicaciones : Controller
    {
        private readonly ApplicationDbContext _context;

        public TipoAplicaciones(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<TipoAplicacion> lista = await _context.TiposAplicaciones.ToListAsync();
            return View(lista);
        }

        public async Task<IActionResult> Crear()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Crear(TipoAplicacion tipoAplicaciones)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _context.TiposAplicaciones.AddAsync(tipoAplicaciones);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));

                }
                catch (DbUpdateException dbUpdateException)
                {

                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe una aplicación con el mismo nombre.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }
            return View(tipoAplicaciones); //me retorna a la vista tipoAplicacion por si me quedo algun dato por llenar que no me vacie el form
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            TipoAplicacion tipoAplicacion = await _context.TiposAplicaciones.FindAsync(id);
            if (id == null || tipoAplicacion == null)
            {
                return NotFound();
            }

            return View(tipoAplicacion);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, TipoAplicacion tipoAplicacion)
        {

            if (id != tipoAplicacion.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.TiposAplicaciones.Update(tipoAplicacion);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index), new { id });
                }
                catch (DbUpdateException dbUpdateException)
                {

                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe una categoría con el mismo nombre.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                    }
                }
            }
            return View(tipoAplicacion);
        }

        public async Task<IActionResult> Eliminar(int? id)
        {
            if (id == null || _context.Categorias == null)
            {
                return NotFound();
            }
            TipoAplicacion tipoAplicacion = await _context.TiposAplicaciones.FindAsync(id);
            if (tipoAplicacion == null)
            {
                return NotFound();
            }
            return View(tipoAplicacion);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Eliminar(int? id, TipoAplicacion tipoAplicacion)
        {
            if (tipoAplicacion == null || id == null)
            {
                return NotFound();
            }
            _context.TiposAplicaciones.Remove(tipoAplicacion);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
