using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FINALGARETTOJUAN.Data;
using FINALGARETTOJUAN.Models;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Authorization;

namespace FINALGARETTOJUAN.Controllers
{
    [Authorize]
    public class ModelosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ModelosController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: Modelos
        public async Task<IActionResult> Index()
        {

            return View(await _context.Modelos.ToListAsync());
        }

        
        public async Task<IActionResult> Importar()
        {
            if (ModelState.IsValid)
            {
                var archivos = HttpContext.Request.Form.Files;

                //Verificar si se subio al menos un archivo
                if (archivos != null && archivos.Count > 0)
                {
                    var archivo = archivos[0];
                    //Verificar si el archivo tiene al menos un elemento
                    if (archivo.Length > 0)
                    {
                        
                        var pathDestino = Path.Combine(_env.WebRootPath, "importaciones");

                        var extArch = Path.GetExtension(archivo.FileName); 
                        var archivoDestino = Guid.NewGuid().ToString();
                        archivoDestino = archivoDestino.Replace("-", "");
                        archivoDestino += Path.GetExtension(archivo.FileName);
                        var rutaDestino = Path.Combine(pathDestino, archivoDestino);

                        using (var filestream = new FileStream(rutaDestino, FileMode.Create))
                        {
                            await archivo.CopyToAsync(filestream);
                        };
                        using (var file = new FileStream(rutaDestino, FileMode.Open))
                        {
                            List<string> renglones = new List<string>();
                            List<Modelo> ListaModelos = new List<Modelo>();

                            StreamReader fileContent = new StreamReader(file, System.Text.Encoding.Default);
                            do
                            {
                                renglones.Add(fileContent.ReadLine());
                            }
                            while (!fileContent.EndOfStream);
                            foreach (var fila in renglones)
                            {
                                Modelo modelo = new Modelo();
                                string[] data = fila.Split(";");
                                if (data.Length == 1)
                                {
                                    modelo.Descripcion = data[0].Trim();
                                    ListaModelos.Add(modelo);
                                }


                            }
                            if(ListaModelos.Count > 0)
                            {
                                _context.AddRange(ListaModelos);
                                await _context.SaveChangesAsync();
                            }

                        }


                        



                    }
                }
            }
                return View("Index",await _context.Modelos.ToListAsync());
        }


        // GET: Modelos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Modelos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Descripcion")] Modelo modelo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(modelo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(modelo);
        }

        // GET: Modelos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var modelo = await _context.Modelos.FindAsync(id);
            if (modelo == null)
            {
                return NotFound();
            }
            return View(modelo);
        }

        // POST: Modelos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Descripcion")] Modelo modelo)
        {
            if (id != modelo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(modelo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ModeloExists(modelo.Id))
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
            return View(modelo);
        }

        // GET: Modelos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var modelo = await _context.Modelos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (modelo == null)
            {
                return NotFound();
            }

            return View(modelo);
        }

        // POST: Modelos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var modelo = await _context.Modelos.FindAsync(id);
            if (modelo != null)
            {
                _context.Modelos.Remove(modelo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ModeloExists(int id)
        {
            return _context.Modelos.Any(e => e.Id == id);
        }
    }
}
