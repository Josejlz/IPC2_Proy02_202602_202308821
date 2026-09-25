using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.AspNetCore.Mvc.RazorPages;
using IPC2_Proy02_202602_202308221.Logica;
 
namespace IPC2_Proy02_202602_202308221.Pages;
 
public class CargarModel : PageModel
{
    private readonly Catalogo catalogo;
 
    // Catalogo se inyecta como Singleton (ver Program.cs) para que el
    // catalogo cargado persista entre peticiones: cada archivo XML que se
    // suba se suma al mismo catalogo en memoria.
    public CargarModel(Catalogo catalogo)
    {
        this.catalogo = catalogo;
    }
 
    [BindProperty]
    public IFormFile? ArchivoXml { get; set; }
 
    public ResultadoCarga? Resultado { get; private set; }
    public string? MensajeError { get; private set; }
    public int TotalLibros => catalogo.CantidadLibros;
 
    public void OnGet()
    {
    }
 
    public async Task OnPostAsync()
    {
        if (ArchivoXml == null || ArchivoXml.Length == 0)
        {
            MensajeError = "Selecciona un archivo XML antes de cargarlo.";
            return;
        }
 
        // recibe el archivo de ruta
        string rutaTemporal = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName() + ".xml");
 
        try
        {
            await using (FileStream flujo = new FileStream(rutaTemporal, FileMode.Create))
            {
                await ArchivoXml.CopyToAsync(flujo);
            }
 
            CargadorXml cargador = new CargadorXml(catalogo);
            Resultado = cargador.CargarArchivo(rutaTemporal);
        }
        catch (Exception ex)
        {
            MensajeError = $"Ocurrio un error inesperado al procesar el archivo: {ex.Message}";
        }
        finally
        {
            if (System.IO.File.Exists(rutaTemporal))
            {
                System.IO.File.Delete(rutaTemporal);
            }
        }
    }
}