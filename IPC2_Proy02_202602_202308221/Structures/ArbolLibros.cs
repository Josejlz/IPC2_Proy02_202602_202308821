using System;
using IPC2_Proy02_202602_202308221.Modelos;
using IPC2_Proy02_202602_202308221.Structures;
namespace IPC2_Proy02_202602_202308221.Structures
{
    public class ArbolLibros
    {
        private NodoArbolLibro? raiz;
        private int cantidad;

        public ArbolLibros()
        {
            cantidad = 0;
            raiz = null;
        }

        public bool EstaVacio => (raiz == null);
        public int Cantidad => cantidad;

        public void Insertar(Libro libro)
        {
            
        }


        
 
        
        
    }    
}
