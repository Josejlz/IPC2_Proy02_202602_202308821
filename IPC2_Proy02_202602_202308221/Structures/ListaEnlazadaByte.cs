using System;
using IPC2_Proy02_202602_202308221.Modelos;

namespace IPC2_Proy02_202602_202308221.Structures
{
    // Lista enlazada de (Categoria, nivel)
    public class ListaEnlazadaByte
    {
        private NodoByte? cabeza;
        private NodoByte? cola;
        private int cantidad;
 
        public ListaEnlazadaByte()
        {
            cabeza = null;
            cola = null;
            cantidad = 0;
        }
 
        public int Cantidad => cantidad;
        public bool EstaVacia => cabeza == null;
 
        public void Agregar(byte valor)
        {
            NodoByte nuevoNodo = new NodoByte(valor);
 
            if (cabeza == null)
            {
                cabeza = nuevoNodo;
                cola = nuevoNodo;
            }
            else
            {
                cola!.Siguiente = nuevoNodo;
                cola = nuevoNodo;
            }
 
            cantidad++;
        }

        public NodoByte? ObtenerCabeza()
        {
            return cabeza;
        }
 
    }
}