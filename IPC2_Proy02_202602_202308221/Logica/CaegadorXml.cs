using System;
using System.Globalization;
using System.Xml.Linq;
using IPC2_Proy02_202602_202308221.Structures;

namespace IPC2_Proy02_202602_202308221.Logica
{
    // Lee un archivo de configuracion XML y llama a Catalogo para ir poblando las estructuras.
    public class CargadorXml
    {
        private readonly Catalogo catalogo;
 
        public CargadorXml(Catalogo catalogo)
        {
            this.catalogo = catalogo;
        }
 
        public ResultadoCarga CargarArchivo(string rutaArchivo)
        {
            ResultadoCarga resultado = new ResultadoCarga();
 
            XDocument documento;
            try
            {
                documento = XDocument.Load(rutaArchivo);
            }
            catch (Exception ex)
            {
                resultado.Errores.Agregar($"No se pudo leer el archivo XML: {ex.Message}");
                return resultado;
            }
 
            XElement? raiz = documento.Root;
            if (raiz == null)
            {
                resultado.Errores.Agregar("El archivo XML esta vacio o mal formado.");
                return resultado;
            }
 
            // listaCategorias es opcional
            XElement? listaCategorias = raiz.Element("listaCategorias");
            if (listaCategorias != null)
            {
                CargarCategorias(listaCategorias, resultado);
            }
 
            // listaLibros es opcional
            XElement? listaLibros = raiz.Element("listaLibros");
            if (listaLibros != null)
            {
                CargarLibros(listaLibros, resultado);
            }
 
            return resultado;
        }
 
        // ---------------- CATEGORIAS 
 
        private void CargarCategorias(XElement listaCategorias, ResultadoCarga resultado)
        {

            ListaEnlazadaTexto pendientesNombre = new ListaEnlazadaTexto();
            ListaEnlazadaTexto pendientesPadre = new ListaEnlazadaTexto();
 
            foreach (XElement elementoCategoria in listaCategorias.Elements("categoria"))
            {
                string nombre = elementoCategoria.Value.Trim();
                string? padre = (string?)elementoCategoria.Attribute("padre");
                padre = string.IsNullOrWhiteSpace(padre) ? null : padre.Trim();
 
                IntentarAgregarCategoria(nombre, padre, resultado, pendientesNombre, pendientesPadre);
            }
 
            ResolverPendientes(pendientesNombre, pendientesPadre, resultado);
        }
 
        private void IntentarAgregarCategoria(
            string nombre, string? padre, ResultadoCarga resultado,
            ListaEnlazadaTexto pendientesNombre, ListaEnlazadaTexto pendientesPadre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                resultado.Errores.Agregar("Se encontro una categoria sin nombre; se omitio.");
                return;
            }
 
            // Si tiene padre pero ese padre todavia no existe, se deja pendiente para reintentarlo en la siguiente pasada.
            if (padre != null && !catalogo.ExisteCategoria(padre))
            {
                pendientesNombre.Agregar(nombre);
                pendientesPadre.Agregar(padre);
                return;
            }
 
            try
            {
                catalogo.AgregarCategoria(nombre, padre);
                resultado.CategoriasAgregadas++;
            }
            catch (Exception ex)
            {
                resultado.Errores.Agregar($"Categoria \"{nombre}\": {ex.Message}");
            }
        }
 
        private void ResolverPendientes(
            ListaEnlazadaTexto pendientesNombre, ListaEnlazadaTexto pendientesPadre, ResultadoCarga resultado)
        {
            bool huboProgreso = true;
 
            while (huboProgreso && !pendientesNombre.EstaVacia)
            {
                huboProgreso = false;
 
                ListaEnlazadaTexto siguientesNombre = new ListaEnlazadaTexto();
                ListaEnlazadaTexto siguientesPadre = new ListaEnlazadaTexto();
 
                NodoTexto? nodoNombre = pendientesNombre.Cabeza;
                NodoTexto? nodoPadre = pendientesPadre.Cabeza;
 
                while (nodoNombre != null && nodoPadre != null)
                {
                    string nombre = nodoNombre.Dato!;
                    string padre = nodoPadre.Dato!;
 
                    if (catalogo.ExisteCategoria(padre))
                    {
                        try
                        {
                            catalogo.AgregarCategoria(nombre, padre);
                            resultado.CategoriasAgregadas++;
                            huboProgreso = true;
                        }
                        catch (Exception ex)
                        {
                            resultado.Errores.Agregar($"Categoria \"{nombre}\": {ex.Message}");
                        }
                    }
                    else
                    {
                        siguientesNombre.Agregar(nombre);
                        siguientesPadre.Agregar(padre);
                    }
 
                    nodoNombre = nodoNombre.Siguiente;
                    nodoPadre = nodoPadre.Siguiente;
                }
 
                pendientesNombre = siguientesNombre;
                pendientesPadre = siguientesPadre;
            }
 
            // Lo que quede pendiente aqui es porque su categoria padre nunca aparecio, ni en este archivo ni en lo ya cargado antes.
            NodoTexto? nombreRestante = pendientesNombre.Cabeza;
            NodoTexto? padreRestante = pendientesPadre.Cabeza;
            while (nombreRestante != null && padreRestante != null)
            {
                resultado.Errores.Agregar(
                    $"Categoria \"{nombreRestante.Dato}\": no existe la categoria padre \"{padreRestante.Dato}\".");
                nombreRestante = nombreRestante.Siguiente;
                padreRestante = padreRestante.Siguiente;
            }
        }
 
        // ---------------- LIBROS 
 
        private void CargarLibros(XElement listaLibros, ResultadoCarga resultado)
        {
            foreach (XElement elementoLibro in listaLibros.Elements("libro"))
            {
                string isbnTexto = elementoLibro.Element("ISBN")?.Value.Trim() ?? "";
                string titulo = elementoLibro.Element("titulo")?.Value.Trim() ?? "";
                string autor = elementoLibro.Element("autor")?.Value.Trim() ?? "";
                string categoriaLibro = elementoLibro.Element("categoria")?.Value.Trim() ?? "";
 
                if (!int.TryParse(isbnTexto, NumberStyles.Integer, CultureInfo.InvariantCulture, out int isbn))
                {
                    resultado.Errores.Agregar($"Libro con ISBN invalido (\"{isbnTexto}\"); se omitio.");
                    continue;
                }
 
                try
                {
                    catalogo.RegistrarLibro(isbn, titulo, autor, categoriaLibro);
                    resultado.LibrosAgregados++;
                }
                catch (Exception ex)
                {
                    resultado.Errores.Agregar($"Libro ISBN {isbn}: {ex.Message}");
                }
            }
        }
    }
}