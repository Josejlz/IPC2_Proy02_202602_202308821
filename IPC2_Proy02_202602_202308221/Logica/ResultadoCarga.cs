using IPC2_Proy02_202602_202308221.Structures;
 
namespace IPC2_Proy02_202602_202308221.Logica
{
    // Resultado de cargar un archivo XML
    public class ResultadoCarga
    {
        public int CategoriasAgregadas { get; set; }
        public int LibrosAgregados { get; set; }
        public ListaEnlazadaTexto Errores { get; }
 
        public ResultadoCarga()
        {
            CategoriasAgregadas = 0;
            LibrosAgregados = 0;
            Errores = new ListaEnlazadaTexto();
        }
    }
}