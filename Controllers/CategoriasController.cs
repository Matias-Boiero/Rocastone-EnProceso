using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rocastone.Data;
using Rocastone.Models;

namespace Rocastone.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoriasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoriasController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Categoria> lista = await _context.Categorias.ToListAsync();
            return View(lista);
        }

        //Get
        public async Task<IActionResult> Crear()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Categoria categoria)
        {
            //_context.Categorias.Add(categoria);
            //_context.SaveChanges();
            //return RedirectToAction("Index");

            if (ModelState.IsValid) //if (ModelState.IsValid) significa que cumpla con todos los data notations
            {
                //creo un try catch para evitar que se registren dos categorias con el mismo nombre
                try
                {
                    await _context.Categorias.AddAsync(categoria);
                    await _context.SaveChangesAsync(); //para grabar los datos, el await espera al submit
                    return RedirectToAction(nameof(Index));
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

                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }
            return View(categoria); //me retorna a la vista categoria por si me quedo algun dato por llenar que no me vacie el form
        }

        //GET
        public async Task<IActionResult> Editar(int? id)
        {
            //Carlo la lista de categorias buscando sus Ids
            if (id == null || _context.Categorias == null)
            {
                return NotFound();
            }

            Categoria categoria = await _context.Categorias.FindAsync(id);
            if (categoria == null)
            {
                return NotFound();
            }
            return View(categoria);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, Categoria categoria)
        {
            if (id != categoria.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Categorias.Update(categoria);
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
            return View(categoria);
        }


        //GET
        public async Task<IActionResult> Eliminar(int? id)
        {
            if (id == null || _context.Categorias == null)
            {
                return NotFound();
            }
            Categoria categoria = await _context.Categorias.FindAsync(id);
            if (categoria == null)
            {
                return NotFound();
            }
            return View(categoria);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Eliminar(int? id, Categoria categoria)
        {
            if (categoria == null || id == null)
            {
                return NotFound();
            }
            _context.Categorias.Remove(categoria);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


    }
}
