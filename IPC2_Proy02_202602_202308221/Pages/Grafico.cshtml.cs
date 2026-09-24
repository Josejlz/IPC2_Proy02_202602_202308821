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

    // GET /Grafico?categoria=NombreCategoria
    // No renderiza el .cshtml: devuelve la imagen PNG directamente, lista
    // para usarse en un <img src="/Grafico?categoria=..."> desde cualquier pagina.
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

        byte[] imagen;
        try
        {
            imagen = generador.GenerarImagenPng(dot);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"No se pudo generar la imagen con Graphviz: {ex.Message}");
        }

        return File(imagen, "image/png");
    }
}