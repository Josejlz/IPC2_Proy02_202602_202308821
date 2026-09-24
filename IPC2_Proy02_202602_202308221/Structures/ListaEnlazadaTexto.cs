namespace IPC2_Proy02_202602_202308221.Structures
{
    
    public class ListaEnlazadaTexto
    {
        private NodoTexto? cabeza;
        private NodoTexto? cola;
        private int cantidad;

        public ListaEnlazadaTexto()
        {
            cabeza = null;
            cola = null;
            cantidad = 0;
        }

        public int Cantidad => cantidad;
        public bool EstaVacia => cabeza ==null;
        public NodoTexto? Cabeza => cabeza;

        public void Agregar(string? texto)
        {
            NodoTexto nuevo = new NodoTexto(texto);
 
            if (cabeza == null)
            {
                cabeza = nuevo;
                cola = nuevo;
            }
            else
            {
                cola!.Siguiente = nuevo;
                cola = nuevo;
            }
 
            cantidad++;
        } 

    }
}