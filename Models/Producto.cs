using FINALGARETTOJUAN.ViewModels;

namespace FINALGARETTOJUAN.Models
{
    public class Producto
    {

     
        public int Id   { get; set; }
        public string? Descripcion   { get; set; }

        public string? Foto { get; set; }
        public int IdModelo { get; set; }
        public Modelo? Modelo { get; set; }

        public int IdTipo { get; set; }
        public Tipo? Tipo { get; set; }

        public int Price { get; set; }
        public List<Stock>? movimientos { get; set; }

        public int IdStock { get; set; }
        public Stock? stock { get; set; }


    }
}
