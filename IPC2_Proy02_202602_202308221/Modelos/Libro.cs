
namespace IPC2_Proy02_202602_202308221.Modelos
{
    public  class Libro
    {
        public int ISBN { get; set; }
        public string Titulo { get; set;}

        public string Autor { get; set; }

        public string Categoria {get; set;}

        public Libro(int isbn, string titulo, string autor, string categoria)
        {

            if (string.IsNullOrWhiteSpace(titulo))
            {
                throw new ArgumentException("El titulo no puede estar vacio");
            }
            if (string.IsNullOrWhiteSpace(autor))
            {
                throw new ArgumentException("El autor no puede estar vacio");
            }
            if (string.IsNullOrWhiteSpace(categoria))
            {
                throw new ArgumentException("La categoria no puede estar vacia");
            }

            ISBN = isbn;
            Titulo = titulo;
            Autor = autor;
            Categoria = categoria;
        }

        public override string ToString()
        {
            return $"[{ISBN}] {Titulo} - {Autor} ({Categoria})";
        }
        

    }
}