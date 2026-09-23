namespace IPC2_Proy02_202602_202308221.Modelos
{
    public  class Categoria
    {

     public string Nombre {get; set;}

     public Categoria(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                throw new ArgumentException("El nombre de la categoria no puede estar vacio");
            }
            Nombre = nombre;
        }

        public override string ToString()
        {
            return Nombre;
        }

    }
}