using System;
using IPC2_Proy02_202602_202308221.Modelos;

namespace IPC2_Proy02_202602_202308221.Structures
{
    // Lista enlazada simple de (Categoria, nivel)
    public class ListaEnlazadaCategorias
    {
        private NodoCategoriaNivel? cabeza;
        private NodoCategoriaNivel? cola;
        private int cantidad;
 
        public ListaEnlazadaCategorias()
        {
            cabeza = null;
            cola = null;
            cantidad = 0;
        }
 
        public int Cantidad => cantidad;
        public bool EstaVacia => cabeza == null;
 
        // Se expone la cabeza para que quien consuma la lista pueda recorrerla el mismo con un while
        public NodoCategoriaNivel? Cabeza => cabeza;
 
        public void Agregar(Categoria? categoria, int nivel)
        {
            NodoCategoriaNivel nuevo = new NodoCategoriaNivel(categoria, nivel);
 
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