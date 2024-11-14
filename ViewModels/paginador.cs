using DocumentFormat.OpenXml.Office2013.Excel;

namespace FINALGARETTOJUAN.ViewModels
{
    public class paginador
    {
        public int paginaActual { get; set; }
        public int cantRegistros { get; set; }

        //Cantidad De Registros x pagina se lo doy en el index valor: 3
        public int cantRegistrosPagina { get; set; }

        public int cantPaginas => (int)Math.Ceiling((decimal)cantRegistros / cantRegistrosPagina);

        public Dictionary<string, string> filtros { get; set; } = new Dictionary<string, string>();

    }
}
