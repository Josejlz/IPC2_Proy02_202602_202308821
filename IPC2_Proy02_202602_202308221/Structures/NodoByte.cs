using System;
using IPC2_Proy02_202602_202308221.Modelos;
namespace IPC2_Proy02_202602_202308221.Structures
{

    public class NodoByte
    {
        public byte? Dato {get; set;}
        public NodoByte? Siguiente {get; set;}

        public NodoByte(byte? dato)
        {
            Dato = dato;
            Siguiente = null;
        }
    }

}