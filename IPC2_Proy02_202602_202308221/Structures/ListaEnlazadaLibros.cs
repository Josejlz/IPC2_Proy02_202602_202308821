using System;
using IPC2_Proy02_202602_202308221.Modelos;
namespace IPC2_Proy02_202602_202308221.Structures
{
    // Lista enlazada simple de libros. RecorridoInOrden la llena ascendentemente de ISBN mientras recorre el BST
    public class ListaEnlazadaLibros
    {
        private NodoLibro? cabeza;
        private NodoLibro? cola;
        private int cantidad;
 
        public ListaEnlazadaLibros()
        {
            cabeza = null;
            cola = null;
            cantidad = 0;
        }
 
        public int Cantidad => cantidad;
        public bool EstaVacia => cabeza == null;
 
        // Se expone la cabeza para recorrer la lista desde afuera
        public NodoLibro? Cabeza => cabeza;
 
        // Se agrega siempre al final, para conservar el orden en que van llegando (el orden ascendente que entrega el recorrido in-orden).
        public void Agregar(Libro? libro)
        {
            NodoLibro nuevo = new NodoLibro(libro);
 
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