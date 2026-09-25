using System;
using IPC2_Proy02_202602_202308221.Modelos;
using IPC2_Proy02_202602_202308221.Structures;

namespace IPC2_Proy02_202602_202308221.Structures
{
    public class ArbolCategorias
    {
        private readonly NodoArbolCategoria raizOculta;
 
        public ArbolCategorias()
        {
            raizOculta = new NodoArbolCategoria(new Categoria("CATALOGO"));
        }
 
        // INSERTAR
         public void InsertarCategoria(string? nombre, string? nombrePadre = null)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                throw new ArgumentException("El nombre de la categoria no puede estar vacio.");
            }
 
            if (Existe(nombre))
            {
                throw new InvalidOperationException($"Ya existe una categoria llamada \"{nombre}\".");
            }
 
            NodoArbolCategoria nodoPadre;
 
            if (string.IsNullOrWhiteSpace(nombrePadre))
            {
                nodoPadre = raizOculta;
            }
            else
            {
                NodoArbolCategoria? encontrado = BuscarNodo(nombrePadre);
                if (encontrado == null)
                {
                    throw new InvalidOperationException($"No existe la categoria padre \"{nombrePadre}\".");
                }
                nodoPadre = encontrado;
            }
 
            NodoArbolCategoria nuevoNodo = new NodoArbolCategoria(new Categoria(nombre));
            nuevoNodo.Padre = nodoPadre;
            InsertarHijoOrdenado(nodoPadre, nuevoNodo);
        }
 
        private void InsertarHijoOrdenado(NodoArbolCategoria padre, NodoArbolCategoria nuevoHijo)
        {
            // no hay hijos todavia, o el nuevo va antes que el primero.
            if (padre.PrimerHijo == null ||
                string.Compare(nuevoHijo.Dato!.Nombre, padre.PrimerHijo.Dato!.Nombre, StringComparison.OrdinalIgnoreCase) < 0)
            {
                nuevoHijo.SiguienteHermano = padre.PrimerHijo;
                padre.PrimerHijo = nuevoHijo;
                return;
            }
 
            // recorrer la cadena de hermanos hasta el lugar correcto.
            NodoArbolCategoria actual = padre.PrimerHijo;
            while (actual.SiguienteHermano != null &&
                   string.Compare(nuevoHijo.Dato!.Nombre, actual.SiguienteHermano.Dato!.Nombre, StringComparison.OrdinalIgnoreCase) > 0)
            {
                actual = actual.SiguienteHermano;
            }
 
            nuevoHijo.SiguienteHermano = actual.SiguienteHermano;
            actual.SiguienteHermano = nuevoHijo;
        }
 
        // UTILIDADES
 
        public bool Existe(string nombre)
        {
            return BuscarNodo(nombre) != null;
        }
 
        public NodoArbolCategoria? BuscarNodo(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                return null;
            }
            return BuscarRecursivo(raizOculta.PrimerHijo, nombre);
        }
 
        public NodoArbolCategoria? BuscarRecursivo(NodoArbolCategoria? nodo, string? nombre)
        {
            if (nodo == null)
            {
                return null;
            }
 
            if (string.Equals(nodo.Dato?.Nombre, nombre, StringComparison.OrdinalIgnoreCase))
            {
                return nodo;
            }
 
            NodoArbolCategoria? enHijos = BuscarRecursivo(nodo.PrimerHijo, nombre);
 
            if (enHijos != null)
            {
                return enHijos;
            }
 
            return BuscarRecursivo(nodo.SiguienteHermano, nombre);
        }
 
        public ArbolLibros ObtenerLibrosDeCategoria(string? nombre)
        {
            NodoArbolCategoria? nodo = BuscarNodo(nombre ?? string.Empty);
            if (nodo == null)
            {
                throw new InvalidOperationException($"No existe la categoria \"{nombre}\".");
            }
            return nodo.LibrosPropios!;
        }
 
        // RECORRIDO
 
        public void RecorrerEstructura(ListaEnlazadaCategorias resultado)
        {
            RecorrerRecursivo(raizOculta.PrimerHijo, 0, resultado);
        }
 
        // Igual que RecorrerEstructura, pero empezando desde una subcategoria especifica
        public void RecorrerEstructuraDesde(string nombreCategoria, ListaEnlazadaCategorias resultado)
        {
            NodoArbolCategoria? nodo = BuscarNodo(nombreCategoria);
            if (nodo == null)
            {
                throw new InvalidOperationException($"No existe la categoria \"{nombreCategoria}\".");
            }
 
            resultado.Agregar(nodo.Dato, 0);
            RecorrerRecursivo(nodo.PrimerHijo, 1, resultado);
        }
 
        private void RecorrerRecursivo(NodoArbolCategoria? nodo, int nivel, ListaEnlazadaCategorias resultado)
        {
            if (nodo == null) return;
            resultado.Agregar(nodo.Dato, nivel);
            RecorrerRecursivo(nodo.PrimerHijo, nivel + 1, resultado);
            RecorrerRecursivo(nodo.SiguienteHermano, nivel, resultado);
        }
    }
}