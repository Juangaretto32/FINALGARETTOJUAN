namespace FINALGARETTOJUAN.Models
{
    public class Stock
    {
        public int Id { get; set; }

        public int Cantidad {  get; set; }
           
        public int IdProducto { get; set; }
        public Producto? producto { get; set; }

    }
}
