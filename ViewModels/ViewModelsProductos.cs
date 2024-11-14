using FINALGARETTOJUAN.Models;

namespace FINALGARETTOJUAN.ViewModels
{
    public class ViewModelsProductos
    {
        public List<Producto> Productos { get; set; }
        public paginador Paginador { get; set; } = new paginador();



    }
}
