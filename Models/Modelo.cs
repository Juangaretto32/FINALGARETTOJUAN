namespace FINALGARETTOJUAN.Models
{
    public class Modelo
    {
        public int Id { get; set; }
        public string? Descripcion { get; set; }

        public string? import {  get; set; }
        public List<Producto> Productos { get; set; } = new List<Producto>();

    }
}
