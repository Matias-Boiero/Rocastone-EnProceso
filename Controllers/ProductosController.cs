using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Rocastone.Data;
using Rocastone.Models;
using Rocastone.Models.ViewModels;


namespace Rocastone.Controllers
{
    [Authorize(Roles = "Admin")]
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


        //GET UPSERT
        public async Task<IActionResult> Upsert(int? id)
        {
            //instancio el objeto  ProductoViewModel productoVM ya  que trabajare con objetos de varios modelos dif.
            ProductoViewModel productoVM = new ProductoViewModel()
            {
                Producto = new Producto(),
                CategoriaLista = _context.Categorias.Select(c => new SelectListItem
                {
                    Text = c.NombreCategoria,
                    Value = c.Id.ToString()
                }),
                TipoAplicacionLista = _context.TiposAplicaciones.Select(c => new SelectListItem
                {
                    Text = c.Nombre,
                    Value = c.Id.ToString()
                })
            };

            if (id == null)
            {
                // Crear nuevo producto... se activa el asp-action="Upsert" que no tiene id porque lo llamo desde el boton crear nuevo producto

                return View(productoVM);
            }
            else //si el objeto trae un id se activa el boton de actualizar
            {
                // Actualizar el producto... se activa el asp-route-id=@item.Id="Upsert" porque lo llamo desde el boton actualizar producto

                productoVM.Producto = await _context.Productos.FindAsync(id);
                if (productoVM.Producto == null)
                {
                    return NotFound();
                }
                return View(productoVM);
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(ProductoViewModel productoViewModel)
        {
            if (ModelState.IsValid)
            {
                //variables para capturar las imagenes que los usuarios elijan 
                var files = HttpContext.Request.Form.Files;
                string webRootPath = _webHostEnvironment.WebRootPath;

                if (productoViewModel.Producto.Id == 0)
                {
                    //CREAR
                    string upload = webRootPath + WebConstantes.ImagenRuta; //ruta local donde guardare las imagenes
                    string fileName = Guid.NewGuid().ToString(); //Se le asigna un ID Guid a la img que se grabara
                    string extension = Path.GetExtension(files[0].FileName); //se obtiene el tipo de extension (png,jpg) del archivo

                    using (FileStream fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }

                    productoViewModel.Producto.imgUrl = fileName + extension;
                    await _context.Productos.AddAsync(productoViewModel.Producto);
                }
                else
                {
                    // ACTUALIZAR
                    Producto objProducto = await _context.Productos.AsNoTracking().FirstOrDefaultAsync(p => p.Id == productoViewModel.Producto.Id);
                    if (files.Count > 0)//el usuario carga una nueva imagen
                    {
                        string upload = webRootPath + WebConstantes.ImagenRuta; //ruta local donde guardare las imagenes
                        string fileName = Guid.NewGuid().ToString(); //Se le asigna un ID Guid a la img que se grabara
                        string extension = Path.GetExtension(files[0].FileName);

                        //ahora se borrara la imagen anterior y se cargara la  nueva
                        var anteriorFile = Path.Combine(upload, objProducto.imgUrl);
                        if (System.IO.File.Exists(anteriorFile))//buscamos que la imagen este en nuestro directorio guardada
                        {
                            System.IO.File.Delete(anteriorFile); //procedemos a borrar la foto anterior del directorio
                        }
                        //fin borrar imagen anterior

                        using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                        {
                            files[0].CopyTo(fileStream);
                        }
                        productoViewModel.Producto.imgUrl = fileName + extension;
                    }
                    else //si cambia cualquier dato y deja la foto que tenía entonces lo de abajo
                    {
                        productoViewModel.Producto.imgUrl = objProducto.imgUrl;
                    }
                    _context.Productos.Update(productoViewModel.Producto);
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //Se llenan nuevamente las listas por si algo falla
            productoViewModel.CategoriaLista = _context.Categorias.Select(c => new SelectListItem
            {
                Text = c.NombreCategoria,
                Value = c.Id.ToString()
            });
            productoViewModel.TipoAplicacionLista = _context.TiposAplicaciones.Select(c => new SelectListItem
            {
                Text = c.Nombre,
                Value = c.Id.ToString()
            });

            return View(productoViewModel);
        }


        //GET
        public async Task<IActionResult> Eliminar(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Producto producto = await _context.Productos.Include(c => c.Categoria)
                                                        .Include(t => t.TipoAplicacion)
                                                        .FirstOrDefaultAsync(p => p.Id == id);
            if (producto == null)
            {
                return NotFound();
            }
            return View(producto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Eliminar(Producto producto)
        {
            if (producto == null)
            {
                return NotFound();
            }
            // Eliminar la imagen
            string upload = _webHostEnvironment.WebRootPath + WebConstantes.ImagenRuta;

            // borrar la imagen anterior
            var anteriorFile = Path.Combine(upload, producto.imgUrl);
            if (System.IO.File.Exists(anteriorFile))
            {
                System.IO.File.Delete(anteriorFile);
            }
            // fin Borrar imagen anterior

            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }



    }
}
