using System;
using IPC2_Proy02_202602_202308221.Modelos;
using IPC2_Proy02_202602_202308221.Structures;

namespace IPC2_Proy02_202602_202308221.Structures
{
    public class ArbolCategorias
    {
        private readonly NodoArbolCategoria? raizOculta;

        public ArbolCategorias()
        {
            raizOculta = new NodoArbolCategoria(new Categoria("CATALOGO"));
        }

        public void InsertarCategoria(string? nombre, string? nombrePadre = null)
        {
            if ()
            {
                
            }
        }


        //Utilidades

        public bool Existe(string nombre)
        {
            return BuscarNodo(nombre) != null;
        }

        public NodoArbolCategoria? BuscarNodo(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre)){
                return null;
            }
            return BuscarRecursivo(raizOculta?.PrimerHijo, nombre);
        }

        public NodoArbolCategoria? BuscarRecursivo(NodoArbolCategoria? nodo, string? nombre)
        {
            if (nodo==null)
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
            NodoArbolCategoria? nodo = BuscarNodo(nombre);
        }

        
    }
}