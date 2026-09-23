using System;
using IPC2_Proy02_202602_202308221.Modelos;
namespace IPC2_Proy02_202602_202308221.Structures
{
    public class ArbolLibros
    {
        private NodoArbolLibro? raiz;
        private int cantidad;
 
        public ArbolLibros()
        {
            raiz = null;
            cantidad = 0;
        }
 
        public bool EstaVacio => raiz == null;
        public int Cantidad => cantidad;
 
        // INSERTAR
        public void Insertar(Libro libro)
        {
            if (Existe(libro.ISBN))
            {
                throw new InvalidOperationException($"Ya existe un libro con ISBN {libro.ISBN}.");
            }
            raiz = InsertarRecursivo(raiz, libro);
            cantidad++;
        }
        private NodoArbolLibro InsertarRecursivo(NodoArbolLibro? actual, Libro libro)
        {
            if (actual == null) return new NodoArbolLibro(libro);
            if (libro.ISBN < actual.Dato!.ISBN) actual.Izq = InsertarRecursivo(actual.Izq, libro);
            else actual.Der = InsertarRecursivo(actual.Der, libro);
            return actual;
        }
 
        // BUSCAR
        public Libro? Buscar(int isbn)
        {
            return BuscarRecursivo(raiz, isbn);
        }
        private Libro? BuscarRecursivo(NodoArbolLibro? actual, int isbn)
        {
            if (actual == null) return null;
            if (actual.Dato!.ISBN == isbn) return actual.Dato;
            if (isbn < actual.Dato.ISBN) return BuscarRecursivo(actual.Izq, isbn);
            return BuscarRecursivo(actual.Der, isbn);
        }
 
        public bool Existe(int isbn)
        {
            return Buscar(isbn) != null;
        }
 
        // ELIMINAR
        public void Eliminar(int isbn)
        {
            if (!Existe(isbn))
            {
                throw new InvalidOperationException($"No existe ningun libro con ISBN {isbn}.");
            }
            raiz = EliminarRecursivo(raiz, isbn);
            cantidad--;
        }
        private NodoArbolLibro? EliminarRecursivo(NodoArbolLibro? actual, int isbn)
        {
            if (actual == null) return null;
 
            if (isbn < actual.Dato!.ISBN)
            {
                actual.Izq = EliminarRecursivo(actual.Izq, isbn);
            }
            else if (isbn > actual.Dato.ISBN)
            {
                actual.Der = EliminarRecursivo(actual.Der, isbn);
            }
            else
            {
                // Nodo encontrado
                if (actual.Izq == null) return actual.Der;
                if (actual.Der == null) return actual.Izq;
 
                // se reemplaza el dato por el del sucesor in-orden y luego se elimina ese sucesor.
                NodoArbolLibro sucesor = ObtenerNodoMinimo(actual.Der);
                actual.Dato = sucesor.Dato;
                actual.Der = EliminarRecursivo(actual.Der, sucesor.Dato!.ISBN);
            }
            return actual;
        }
 
        // OBTENER MINIMO Y MAXIMO
        public Libro? ObtenerMinimo()
        {
            if (raiz == null) return null;
            return ObtenerNodoMinimo(raiz).Dato;
        }
        public Libro? ObtenerMaximo()
        {
            if (raiz == null) return null;
            return ObtenerNodoMaximo(raiz).Dato;
        }
        private NodoArbolLibro ObtenerNodoMinimo(NodoArbolLibro actual)
        {
            while (actual.Izq != null) actual = actual.Izq;
            return actual;
        }
        private NodoArbolLibro ObtenerNodoMaximo(NodoArbolLibro actual)
        {
            while (actual.Der != null) actual = actual.Der;
            return actual;
        }
 
        // RECORRIDO IN-ORDEN (entrega orden ascendente de ISBN)
        public void RecorridoInOrden(ListaEnlazadaLibros resultado)
        {
            InOrdenRecursivo(raiz, resultado);
        }
        private void InOrdenRecursivo(NodoArbolLibro? actual, ListaEnlazadaLibros resultado)
        {
            if (actual == null) return;
            InOrdenRecursivo(actual.Izq, resultado);
            resultado.Agregar(actual.Dato); // Se agrega a nuestra propia lista
            InOrdenRecursivo(actual.Der, resultado);
        }
    }
}