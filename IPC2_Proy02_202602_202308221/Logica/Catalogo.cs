using System;
using IPC2_Proy02_202602_202308221.Modelos;
using IPC2_Proy02_202602_202308221.Structures;

namespace IPC2_Proy02_202602_202308221.Logica
{
    // El controller
    public class Catalogo
    {
        private readonly ArbolLibros librosGlobal;
        private readonly ArbolCategorias categorias;

        public Catalogo()
        {
            librosGlobal = new ArbolLibros();
            categorias = new ArbolCategorias();
        }

        public int CantidadLibros => librosGlobal.Cantidad;

        // Categorias

        public void AgregarCategoria(string? nombre, string? nombrePadre = null)
        {
            categorias.InsertarCategoria(nombre, nombrePadre);
        }

        public bool ExisteCategoria(string nombre)
        {
            return categorias.Existe(nombre);
        }

        // Estructura completa del catalogo 
        public ListaEnlazadaCategorias ObtenerEstructura()
        {
            ListaEnlazadaCategorias resultado = new ListaEnlazadaCategorias();
            categorias.RecorrerEstructura(resultado);
            return resultado;
        }

        // Igual, pero empezando desde una subcategoria especifica
        public ListaEnlazadaCategorias ObtenerEstructuraDesde(string nombreCategoria)
        {
            ListaEnlazadaCategorias resultado = new ListaEnlazadaCategorias();
            categorias.RecorrerEstructuraDesde(nombreCategoria, resultado);
            return resultado;
        }

        // Libros que pertenecen a una categoria (sin contar los de sus subcategorias), en orden ascendente de ISBN.
        public ListaEnlazadaLibros ObtenerLibrosDeCategoria(string nombreCategoria)
        {
            ArbolLibros arbolLocal = categorias.ObtenerLibrosDeCategoria(nombreCategoria);
            ListaEnlazadaLibros resultado = new ListaEnlazadaLibros();
            arbolLocal.RecorridoInOrden(resultado);
            return resultado;
        }

        // Libros

        public void RegistrarLibro(int isbn, string titulo, string autor, string nombreCategoria)
        {
            if (!categorias.Existe(nombreCategoria))
            {
                throw new InvalidOperationException(
                    $"No existe la categoria \"{nombreCategoria}\"; registra la categoria antes que el libro.");
            }

            if (librosGlobal.Existe(isbn))
            {
                throw new InvalidOperationException($"Ya existe un libro con ISBN {isbn}.");
            }

            Libro libro = new Libro(isbn, titulo, autor, nombreCategoria);

            // El mismo objeto Libro se inserta en dos arboles distintos. En el indice del catalogo, y el  local de categoria.
            librosGlobal.Insertar(libro);
            categorias.ObtenerLibrosDeCategoria(nombreCategoria).Insertar(libro);
        }

        public Libro? BuscarLibro(int isbn)
        {
            return librosGlobal.Buscar(isbn);
        }

        public bool ExisteLibro(int isbn)
        {
            return librosGlobal.Existe(isbn);
        }

        public void EliminarLibro(int isbn)
        {
            Libro? libro = librosGlobal.Buscar(isbn);
            if (libro == null)
            {
                throw new InvalidOperationException($"No existe ningun libro con ISBN {isbn}.");
            }

            librosGlobal.Eliminar(isbn);

            // Tambien se quita del indice local de su categoria.
            if (categorias.Existe(libro.Categoria))
            {
                categorias.ObtenerLibrosDeCategoria(libro.Categoria).Eliminar(isbn);
            }
        }

        public Libro? ObtenerMenor()
        {
            return librosGlobal.ObtenerMinimo();
        }

        public Libro? ObtenerMayor()
        {
            return librosGlobal.ObtenerMaximo();
        }

        // Todos los libros del catalogo, en orden ascendente de ISBN.
        public ListaEnlazadaLibros ObtenerTodosAscendente()
        {
            ListaEnlazadaLibros resultado = new ListaEnlazadaLibros();
            librosGlobal.RecorridoInOrden(resultado);
            return resultado;
        }
    }
}
