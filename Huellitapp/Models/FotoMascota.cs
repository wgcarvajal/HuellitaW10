using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Huellitapp.Models
{
    public class FotoMascota
    {
        public static string TABLA = "fotomascota";//nombre de la tabla en la base de datos
        //atributos de la tabla mascotas
        public static string ID = "objectID";// identificador de la tabla mascota
        public static string IMAGEN = "foto";//nombre mascota        
        public static string IDMASCOTA = "mascota";//nombre de usuario del propietario actual de la mascota


        private string id;
        private string url;
        private string idMascota;


        public string Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }
        public string Url
        {
            get
            {
                return url;
            }
            set
            {
                url = value;
            }
        }

        public string IdMascota
        {
            get
            {
                return idMascota;
            }
            set
            {
                idMascota = value;
            }
        }

    }
}
