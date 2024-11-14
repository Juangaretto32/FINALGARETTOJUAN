using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FINALGARETTOJUAN.Data;
using FINALGARETTOJUAN.Models;
using Microsoft.AspNetCore.Authorization;

namespace FINALGARETTOJUAN.Controllers
{
    [Authorize]
    public class TiposController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public TiposController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: Tipos
        public async Task<IActionResult> Index()
        {
            return View(await _context.Tipos.ToListAsync());
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
                            List<Tipo> ListaTipos = new List<Tipo>();

                            StreamReader fileContent = new StreamReader(file, System.Text.Encoding.Default);
                            do
                            {
                                renglones.Add(fileContent.ReadLine());
                            }
                            while (!fileContent.EndOfStream);
                            foreach (var fila in renglones)
                            {
                                Tipo tipo = new Tipo();
                                string[] data = fila.Split(";");
                                if (data.Length == 1)
                                {
                                    tipo.Descripcion = data[0].Trim();
                                    ListaTipos.Add(tipo);
                                }


                            }
                            if (ListaTipos.Count > 0)
                            {
                                _context.AddRange(ListaTipos);
                                await _context.SaveChangesAsync();
                            }

                        }






                    }
                }
            }
            return View("Index", await _context.Tipos.ToListAsync());
        }

        // GET: Tipos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tipos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Descripcion")] Tipo tipo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tipo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tipo);
        }

        // GET: Tipos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipo = await _context.Tipos.FindAsync(id);
            if (tipo == null)
            {
                return NotFound();
            }
            return View(tipo);
        }

        // POST: Tipos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Descripcion")] Tipo tipo)
        {
            if (id != tipo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tipo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TipoExists(tipo.Id))
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
            return View(tipo);
        }

        // GET: Tipos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipo = await _context.Tipos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tipo == null)
            {
                return NotFound();
            }

            return View(tipo);
        }

        // POST: Tipos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tipo = await _context.Tipos.FindAsync(id);
            if (tipo != null)
            {
                _context.Tipos.Remove(tipo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TipoExists(int id)
        {
            return _context.Tipos.Any(e => e.Id == id);
        }
    }
}
