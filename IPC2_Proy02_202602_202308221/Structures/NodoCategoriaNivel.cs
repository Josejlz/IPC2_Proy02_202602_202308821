using System;
using IPC2_Proy02_202602_202308221.Modelos;

namespace IPC2_Proy02_202602_202308221.Structures
{
    // Nodo especifico para guardar una Categoria junto con su nivel
    public class NodoCategoriaNivel
    {
        public Categoria? Dato { get; set; }
        public int Nivel { get; set; }
        public NodoCategoriaNivel? Siguiente { get; set; }
 
        public NodoCategoriaNivel(Categoria? dato, int nivel)
        {
            Dato = dato;
            Nivel = nivel;
            Siguiente = null;
        }
    }
}