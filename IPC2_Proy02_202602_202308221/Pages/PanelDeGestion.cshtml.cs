using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using IPC2_Proy02_202602_202308221.Logica;
using IPC2_Proy02_202602_202308221.Modelos;
using IPC2_Proy02_202602_202308221.Structures;

namespace IPC2_Proy02_202602_202308221.Pages;

public class PanelDeGestionModel : PageModel
{
    private readonly Catalogo catalogo;
 
    public PanelDeGestionModel(Catalogo catalogo)
    {
        this.catalogo = catalogo;
    }
 
    // --- Registrar ---
    [BindProperty]
    public int? NuevoIsbn { get; set; }
    [BindProperty]
    public string? NuevoTitulo { get; set; }
    [BindProperty]
    public string? NuevoAutor { get; set; }
    [BindProperty]
    public string? NuevaCategoria { get; set; }
 
    // --- Eliminar ---
    [BindProperty]
    public int? IsbnEliminar { get; set; }
 
    // --- Buscar (GET, no modifica nada) ---
    [BindProperty(SupportsGet = true)]
    public int? Buscar { get; set; }
 
    public Libro? LibroEncontrado { get; private set; }
    public string? ErrorBusqueda { get; private set; }
 
    // Siempre visibles, se recalculan en cada carga de la pagina.
    public Libro? LibroMenor { get; private set; }
    public Libro? LibroMayor { get; private set; }
 
    public ListaEnlazadaCategorias CategoriasDisponibles { get; private set; } = new ListaEnlazadaCategorias();
 
    public string? Mensaje { get; private set; }
    public bool MensajeEsError { get; private set; }
 
    public void OnGet()
    {
        CargarDatosComunes();
        CargarBusquedaSiAplica();
        LeerMensajeTemporal();
    }
 
    public IActionResult OnPostRegistrar()
    {
        try
        {
            if (NuevoIsbn == null)
            {
                throw new ArgumentException("El ISBN es obligatorio y debe ser numerico.");
            }
 
            catalogo.RegistrarLibro(NuevoIsbn.Value, NuevoTitulo ?? "", NuevoAutor ?? "", NuevaCategoria ?? "");
            TempData["Mensaje"] = $"Libro con ISBN {NuevoIsbn} registrado correctamente.";
            TempData["MensajeEsError"] = false;
        }
        catch (Exception ex)
        {
            TempData["Mensaje"] = ex.Message;
            TempData["MensajeEsError"] = true;
        }
 
        return RedirectToPage();
    }
 
    public IActionResult OnPostEliminar()
    {
        try
        {
            if (IsbnEliminar == null)
            {
                throw new ArgumentException("Indica el ISBN del libro a eliminar.");
            }
 
            catalogo.EliminarLibro(IsbnEliminar.Value);
            TempData["Mensaje"] = $"Libro con ISBN {IsbnEliminar} eliminado correctamente.";
            TempData["MensajeEsError"] = false;
        }
        catch (Exception ex)
        {
            TempData["Mensaje"] = ex.Message;
            TempData["MensajeEsError"] = true;
        }
 
        return RedirectToPage();
    }
 
    private void CargarDatosComunes()
    {
        LibroMenor = catalogo.ObtenerMenor();
        LibroMayor = catalogo.ObtenerMayor();
        CategoriasDisponibles = catalogo.ObtenerEstructura();
    }
 
    private void CargarBusquedaSiAplica()
    {
        if (Buscar == null)
        {
            return;
        }
 
        LibroEncontrado = catalogo.BuscarLibro(Buscar.Value);
        if (LibroEncontrado == null)
        {
            ErrorBusqueda = $"No existe ningun libro con ISBN {Buscar}.";
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