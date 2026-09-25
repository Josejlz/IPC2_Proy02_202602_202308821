using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using IPC2_Proy02_202602_202308221.Logica;
using IPC2_Proy02_202602_202308221.Structures;

namespace IPC2_Proy02_202602_202308221.Pages;

public class GraficoModel : PageModel
{
    private readonly Catalogo catalogo;
    private readonly GeneradorGraphviz generador;

    public GraficoModel(Catalogo catalogo)
    {
        this.catalogo = catalogo;
        generador = new GeneradorGraphviz();
    }

    // GET /Grafico?categoria=NombreCategoria No renderiza el .cshtml: devuelve la imagen PNG directamente, lista para usarse en un <img src="/Grafico?categoria=...">.
    public IActionResult OnGet(string? categoria)
    {
        if (string.IsNullOrWhiteSpace(categoria))
        {
            return BadRequest("Debes indicar una categoria (?categoria=Nombre).");
        }

        ListaEnlazadaLibros libros;
        try
        {
            libros = catalogo.ObtenerLibrosDeCategoria(categoria);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }

        string dot = generador.GenerarDot(libros, categoria);

        ListaEnlazadaByte imagen;
        try
        {
            imagen = generador.GenerarImagenPng(dot);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"No se pudo generar la imagen con Graphviz: {ex.Message}");
        }

        Stream flujoImagen = new ListaEnlazadaByteStream(imagen);

        return File(flujoImagen, "image/png");
    }


    public class ListaEnlazadaByteStream : Stream
    {
        private NodoByte? actual;
        private readonly int longitud;
        private int posicion;
 
        public ListaEnlazadaByteStream(ListaEnlazadaByte lista)
        {
            actual = lista.ObtenerCabeza();
            longitud = lista.Cantidad;
            posicion = 0;
        }
 
        public override bool CanRead => true;
        public override bool CanSeek => false;
        public override bool CanWrite => false;
        public override long Length => longitud;
 
        public override long Position
        {
            get => posicion;
            set => throw new NotSupportedException("Este stream no soporta busqueda.");
        }
 
        public override int Read(byte[] buffer, int offset, int count)
        {
            int leidos = 0;
 
            while (leidos < count && actual != null)
            {
                buffer[offset + leidos] = actual.Dato ?? 0;
                actual = actual.Siguiente;
                leidos++;
                posicion++;
            }
 
            return leidos;
        }
 
        public override void Flush()
        {
            // solo para herencia :v
        }
 
        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException("Este stream no soporta busqueda.");
        }
 
        public override void SetLength(long value)
        {
            throw new NotSupportedException("Este stream no soporta cambiar su longitud.");
        }
 
        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException("Este stream es de solo lectura.");
        }
    }

}