using System.Diagnostics;
using System.Text;
using IPC2_Proy02_202602_202308221.Modelos;
using IPC2_Proy02_202602_202308221.Structures;

namespace IPC2_Proy02_202602_202308221.Logica
{
    // Construye el archivo .dot con los libros de una categoria (en el
    // mismo orden ascendente que entrega ListaEnlazadaLibros) e invoca el
    // ejecutable "dot" de Graphviz para convertirlo en una imagen PNG.
    public class GeneradorGraphviz
    {
        // ---------------- construccion del .dot ----------------

        public string GenerarDot(ListaEnlazadaLibros libros, string tituloCategoria)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("digraph Catalogo {");
            sb.AppendLine("    rankdir=LR;");
            sb.AppendLine("    labelloc=\"t\";");
            sb.AppendLine($"    label=\"Libros de: {EscaparTexto(tituloCategoria)}\";");
            sb.AppendLine("    node [shape=box, style=filled, fillcolor=\"#FFF3B0\", fontname=\"Helvetica\"];");

            if (libros.EstaVacia)
            {
                sb.AppendLine("    \"vacio\" [label=\"(sin libros propios)\", shape=plaintext];");
                sb.AppendLine("}");
                return sb.ToString();
            }

            // Se recorre la lista enlazada 
            NodoLibro? actual = libros.Cabeza;
            string? idNodoAnterior = null;

            while (actual != null)
            {
                Libro? libro = actual.Dato;
                if (libro != null)
                {
                    string idNodo = "libro_" + libro.ISBN;
                    string etiqueta = $"ISBN: {libro.ISBN}\\n{EscaparTexto(libro.Titulo)}\\n{EscaparTexto(libro.Autor)}";
                    sb.AppendLine($"    \"{idNodo}\" [label=\"{etiqueta}\"];");

                    if (idNodoAnterior != null)
                    {
                        sb.AppendLine($"    \"{idNodoAnterior}\" -> \"{idNodo}\";");
                    }

                    idNodoAnterior = idNodo;
                }

                actual = actual.Siguiente;
            }

            sb.AppendLine("}");
            return sb.ToString();
        }

        private string EscaparTexto(string? texto)
        {
            if (string.IsNullOrEmpty(texto))
            {
                return "";
            }

            // Escapa lo minimo necesario para no romper la sintaxis del .dot.
            return texto
                .Replace("\\", "\\\\")
                .Replace("\"", "\\\"")
                .Replace("\n", " ")
                .Replace("\r", " ");
        }

        // ---------------- INVOCAR GRAPHVIZ ----------------

        public byte[] GenerarImagenPng(string contenidoDot)
        {
            string rutaDot = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName() + ".dot");
            string rutaPng = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName() + ".png");

            try
            {
                File.WriteAllText(rutaDot, contenidoDot);

                ProcessStartInfo info = new ProcessStartInfo
                {
                    FileName = "dot",
                    Arguments = $"-Tpng \"{rutaDot}\" -o \"{rutaPng}\"",
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (Process? proceso = Process.Start(info))
                {
                    if (proceso == null)
                    {
                        throw new InvalidOperationException("No se pudo iniciar el proceso de Graphviz (dot).");
                    }

                    string errorSalida = proceso.StandardError.ReadToEnd();
                    proceso.WaitForExit();

                    if (proceso.ExitCode != 0)
                    {
                        throw new InvalidOperationException($"Graphviz (dot) termino con error: {errorSalida}");
                    }
                }

                if (!File.Exists(rutaPng))
                {
                    throw new InvalidOperationException("Graphviz no genero la imagen esperada.");
                }

                return File.ReadAllBytes(rutaPng);
            }
            finally
            {
                if (File.Exists(rutaDot)) File.Delete(rutaDot);
                if (File.Exists(rutaPng)) File.Delete(rutaPng);
            }
        }
    }
}