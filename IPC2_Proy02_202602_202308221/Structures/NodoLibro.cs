using System;
using IPC2_Proy02_202602_202308221.Modelos;
namespace IPC2_Proy02_202602_202308221.Structures
{

    public class NodoLibro
    {
        public Libro? Dato {get; set;}
        public NodoLibro? Siguiente {get; set;}

        public NodoLibro(Libro? dato)
        {
            Dato = dato;
            Siguiente = null;
        }
    }

}