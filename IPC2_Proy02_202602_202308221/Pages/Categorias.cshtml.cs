using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using IPC2_Proy02_202602_202308221.Logica;
using IPC2_Proy02_202602_202308221.Structures;

namespace IPC2_Proy02_202602_202308221.Pages;

public class CategoriasModel : PageModel
{
    private readonly Catalogo catalogo;

    public CategoriasModel(Catalogo catalogo)
    {
        this.catalogo = catalogo;
    }

    [BindProperty]
    public string? NuevaCategoriaNombre { get; set; }

    [BindProperty]
    public string? NuevaCategoriaPadre { get; set; }

    // "desde" y "libros" viajan por query string (GET) y tambien como
    // campos ocultos del formulario (POST), para conservar el contexto
    // que se estaba viendo despues de agregar una categoria.
    [BindProperty(SupportsGet = true)]
    public string? Desde { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? Libros { get; set; }

    public ListaEnlazadaCategorias EstructuraCompleta { get; private set; } = new ListaEnlazadaCategorias();
    public ListaEnlazadaCategorias? Estructura { get; private set; }
    public string? ErrorEstructura { get; private set; }

    public ListaEnlazadaLibros? LibrosCategoria { get; private set; }
    public string? ErrorLibros { get; private set; }

    public string? Mensaje { get; private set; }
    public bool MensajeEsError { get; private set; }

    public void OnGet()
    {
        CargarEstructura();
        CargarLibrosSiAplica();
        LeerMensajeTemporal();
    }

    public IActionResult OnPostAgregar()
    {
        try
        {
            catalogo.AgregarCategoria(NuevaCategoriaNombre, NuevaCategoriaPadre);
            TempData["Mensaje"] = $"Categoria \"{NuevaCategoriaNombre}\" agregada correctamente.";
            TempData["MensajeEsError"] = false;
        }
        catch (Exception ex)
        {
            TempData["Mensaje"] = ex.Message;
            TempData["MensajeEsError"] = true;
        }

        // Patron Post-Redirect-Get: se redirige a la misma pagina
        // conservando el filtro "desde" que estuviera activo.
        return RedirectToPage(new { desde = Desde });
    }

    private void CargarEstructura()
    {
        EstructuraCompleta = catalogo.ObtenerEstructura();

        if (string.IsNullOrWhiteSpace(Desde))
        {
            Estructura = EstructuraCompleta;
            return;
        }

        try
        {
            Estructura = catalogo.ObtenerEstructuraDesde(Desde);
        }
        catch (Exception ex)
        {
            ErrorEstructura = ex.Message;
            Estructura = EstructuraCompleta;
        }
    }

    private void CargarLibrosSiAplica()
    {
        if (string.IsNullOrWhiteSpace(Libros))
        {
            return;
        }

        try
        {
            LibrosCategoria = catalogo.ObtenerLibrosDeCategoria(Libros);
        }
        catch (Exception ex)
        {
            ErrorLibros = ex.Message;
        }
    }

    private void LeerMensajeTemporal()
    {
        if (TempData["Mensaje"] is string mensaje)
        {
            Mensaje = mensaje;
            MensajeEsError = TempData["MensajeEsError"] is bool esError && esError;
        }
    }
}

