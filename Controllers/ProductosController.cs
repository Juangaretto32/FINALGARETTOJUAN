using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FINALGARETTOJUAN.Data;
using FINALGARETTOJUAN.Models;
using ClosedXML.Excel;
using FINALGARETTOJUAN.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace FINALGARETTOJUAN.Controllers
{
    [Authorize]
    public class ProductosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductosController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }


        [AllowAnonymous]
        // GET: Productos
        public async Task<IActionResult> Index(int? IdTipo,int? IdModelo, int pagina = 1)
        {
            paginador paginador = new paginador
            {
                paginaActual = pagina,
                cantRegistrosPagina = 3,
            };

            var applicationDbContext = _context.Productos
                .Include(p => p.Modelo)
                .Include(e => e.Tipo).AsQueryable();

      

            if (IdTipo.HasValue)
            {
                applicationDbContext = applicationDbContext.Where(e => e.IdTipo == IdTipo);
                paginador.filtros.Add("IdTipo", IdTipo.ToString());
            }
            if (IdModelo.HasValue)
            {
                applicationDbContext = applicationDbContext.Where(e => e.IdModelo == IdModelo);
                paginador.filtros.Add("IdModelo", IdModelo.ToString());
            }

            paginador.cantRegistros = applicationDbContext.Count();

            applicationDbContext = applicationDbContext
                .Skip(paginador.cantRegistrosPagina *(pagina -1))
                .Take(paginador.cantRegistrosPagina);


            VmProductos modelo = new VmProductos
            {
                ListaProductos = await applicationDbContext.ToListAsync(),
                ListarModelo = new SelectList(_context.Modelos,"Id","Descripcion"),
                ListarTipo = new SelectList(_context.Tipos,"Id","Descripcion"),

                paginadorVM = paginador
            };


            return View(modelo);
        }

        // GET: Productos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos
                .Include(p => p.Tipo)  // Asegúrate de cargar el Tipo
                .Include(p => p.Modelo)
                .Include(p => p.stock)// Asegúrate de cargar el Modelo            
                .FirstOrDefaultAsync(m => m.Id == id);

            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

            // GET: Productos/Create
            public IActionResult Create()
        {
            ViewData["IdTipo"] = new SelectList(_context.Tipos, "Id", "Descripcion");
            ViewData["IdModelo"] = new SelectList(_context.Modelos, "Id", "Descripcion");

            return View();
        }

        // POST: Productos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Descripcion,IdModelo,IdTipo,Price,IdStock")] Producto producto)
        {

            if (ModelState.IsValid)
            {

                var archivo = HttpContext.Request.Form.Files;

                //Verificar si se subio al menos un archivo

                if (archivo != null && archivo.Count > 0)
                {
                    var archivoFoto = archivo[0];
                    //Verificar si el archivo tiene al menos un elemento
                    if (archivoFoto.Length > 0)
                    {
                        // Ruta de destino en la carpeta "wwwroot/Images"
                        var rutaDestino = Path.Combine(_env.WebRootPath, "Fotografia");
                        if (!Directory.Exists(rutaDestino))
                        {
                            Directory.CreateDirectory(rutaDestino);
                        }

                        // Generar un nombre único para el archivo usando Guid
                        var extArch = Path.GetExtension(archivoFoto.FileName); // Obtener la extensión del archivo
                        var archivoDestino = Guid.NewGuid().ToString() + extArch; // Generar nombre único

                        // Guardar el archivo en la carpeta "wwwroot/Images"
                        var rutaArchivo = Path.Combine(rutaDestino, archivoDestino);

                        using (var filestream = new FileStream(rutaArchivo, FileMode.Create))
                        {
                            await archivoFoto.CopyToAsync(filestream);
                        }

                        // Guardar el nombre del archivo en la propiedad "Foto" del modelo Accesorio
                        producto.Foto = archivoDestino;
                    }

                }
                _context.Add(producto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(producto);
        }

        // GET: Productos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }
            ViewData["IdTipo"] = new SelectList(_context.Tipos, "Id", "Descripcion");
            ViewData["IdModelo"] = new SelectList(_context.Modelos, "Id", "Descripcion");
            return View(producto);
        }

        // POST: Productos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Descripcion,IdModelo,IdTipo,Foto,Price")] Producto producto)
        {
            if (id != producto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                var archivo = HttpContext.Request.Form.Files;

                //Verificar si se subio al menos un archivo

                if (archivo != null && archivo.Count > 0)
                {
                    var archivoFoto = archivo[0];
                    //Verificar si el archivo tiene al menos un elemento
                    if (archivoFoto.Length > 0)
                    {
                        // Ruta de destino en la carpeta "wwwroot/Images"
                        var rutaDestino = Path.Combine(_env.WebRootPath, "Fotografia");
                        if (!Directory.Exists(rutaDestino))
                        {
                            Directory.CreateDirectory(rutaDestino);
                        }

                        // Generar un nombre único para el archivo usando Guid
                        var extArch = Path.GetExtension(archivoFoto.FileName); // Obtener la extensión del archivo
                        var archivoDestino = Guid.NewGuid().ToString() + extArch; // Generar nombre único

                        // Guardar el archivo en la carpeta "wwwroot/Images"
                        var rutaArchivo = Path.Combine(rutaDestino, archivoDestino);
                        string fotoAnterior = Path.Combine(rutaDestino, producto.Foto);
                        if(System.IO.File.Exists(fotoAnterior))
                            System.IO.File.Delete(fotoAnterior);
                        using (var filestream = new FileStream(rutaArchivo, FileMode.Create))
                        {
                            await archivoFoto.CopyToAsync(filestream);
                        }

                        // Guardar el nombre del archivo en la propiedad "Foto" del modelo Accesorio
                        producto.Foto = archivoDestino;
                    }

                }
                try
                {
                    _context.Update(producto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductoExists(producto.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(producto);
        }

        // GET: Productos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Cargar Producto con las relaciones de Tipo y Modelo
            var producto = await _context.Productos
                .Include(p => p.Tipo)  // Incluye la relación con Tipo
                .Include(p => p.Modelo) // Incluye la relación con Modelo
                .FirstOrDefaultAsync(m => m.Id == id);

            if (producto == null)
            {
                return NotFound();
            }

            // Cargar los SelectLists para los campos de Tipo y Modelo
            ViewData["IdTipo"] = new SelectList(_context.Tipos, "Id", "Descripcion", producto.IdTipo);
            ViewData["IdModelo"] = new SelectList(_context.Modelos, "Id", "Descripcion", producto.IdModelo);

            // Pasar el producto a la vista
            return View(producto);
        }

        // POST: Productos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto != null)
            {
                _context.Productos.Remove(producto);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductoExists(int id)
        {
            return _context.Productos.Any(e => e.Id == id);
        }






    }
}
