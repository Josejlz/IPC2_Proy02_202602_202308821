using IPC2_Proy02_202602_202308221.Modelos;

namespace IPC2_Proy02_202602_202308221.Structures
{
    public class NodoArbolCategoria
    {
        public Categoria? Dato {get; set;}
        public NodoArbolCategoria? Padre {get; set;}
        public NodoArbolCategoria? PrimerHijo {get; set;}
        public NodoArbolCategoria? SiguienteHermano {get; set;}
 
        public ArbolLibros? LibrosPropios { get; private set; }
 
        public NodoArbolCategoria(Categoria dato)
        {
            Dato = dato;
            Padre = null;
            PrimerHijo = null;
            SiguienteHermano =  null;
            LibrosPropios = new ArbolLibros();
        }
 
    }
}