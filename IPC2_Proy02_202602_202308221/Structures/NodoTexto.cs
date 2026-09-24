namespace IPC2_Proy02_202602_202308221.Structures
{
    public class NodoTexto
    {
        public string? Dato {get; set;}
        public NodoTexto? Siguiente {get; set;}

        public NodoTexto(string? dato)
        {
            Dato = dato;
            Siguiente = null;
        }

    }
}