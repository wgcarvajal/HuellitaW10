using Parse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Huellitapp.Models
{
    public class Mensaje
    {
        public static string TABLA="mensaje";
        public static string FECHA = "createdAt";
        public static string MASCOTA = "idMascota";
        public static string ORIGEN = "usuarioOrigen";
        public static string DESTINO = "usuarioDestino";
        public static string MENSAJE = "mensaje";

        private DateTime fecha;
        private Mascota  mascota;
        private ParseUser origen;
        private string origenAlias;
        private ParseUser destino;
        private string message;

        public DateTime Fecha
        {
            get { return fecha; }
            set { fecha = value; }
        }
        public Mascota Mascota
        {
            get { return mascota; }
            set { mascota = value; }
        }

        public ParseUser Origen
        {
            get { return origen; }
            set { origen = value; }
        }
        public ParseUser Destino
        {
            get { return destino; }
            set { destino = value; }
        }

        public string Message
        {
            get { return message; }
            set { message = value; }
        }

        public string OrigenAlias
        {
            get { return origenAlias; }
            set { origenAlias = value; }
        }

    }
}
