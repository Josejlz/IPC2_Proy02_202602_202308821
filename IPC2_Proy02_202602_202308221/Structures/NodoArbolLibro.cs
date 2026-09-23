using System;
using IPC2_Proy02_202602_202308221.Modelos;

namespace IPC2_Proy02_202602_202308221.Structures
{
    public class NodoArbolLibro
    {
        public Libro? Dato {get; set;}
        public NodoArbolLibro? Izq {get; set;}
        public NodoArbolLibro? Der {get; set;}

        public NodoArbolLibro(Libro? dato)
        {
            Dato = dato;
            Izq = null;
            Der = null;
        }

    }
}