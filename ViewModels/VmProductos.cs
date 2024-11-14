using FINALGARETTOJUAN.Models;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace FINALGARETTOJUAN.ViewModels
{
    public class VmProductos
    {

       
        public List<Producto> ListaProductos { get; set; }
        public int? IdModelo { get; set; }
        public SelectList ListarModelo { get; set; }

        public int? IdTipo { get; set; }
        public SelectList ListarTipo { get; set; }

        public paginador paginadorVM { get; set; }

        


    }
}
