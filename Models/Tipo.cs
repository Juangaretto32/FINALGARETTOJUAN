namespace FINALGARETTOJUAN.Models
{
    public class Tipo
    {
        public int Id { get; set; }
        public string? Descripcion { get; set; }

        public string? import { get; set; }
        // Relación uno a muchos: un tipo puede tener muchos productos
        public List<Producto> Productos { get; set; } = new List<Producto>();


    }
}
