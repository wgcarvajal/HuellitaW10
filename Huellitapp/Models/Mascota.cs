using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Huellitapp.Models
{
    public class Mascota
    {

        public static string TABLA = "mascota";//nombre de la tabla en la base de datos
        //atributos de la tabla mascotas
        public static string ID = "objectID";// identificador de la tabla mascota
        public static string NOMBRE = "masnombre";//nombre mascota
        public static string TIPO = "tiponombre";// tipo mascota Adultos o Cachorros
        public static string NOMBREUSUARIO = "username";//nombre de usuario del propietario actual de la mascota


        private string id;
        private string nombre;       
        private string tipo;
        private string nombreUsuario;
        private ObservableCollection<FotoMascota> fotos;


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

        public string Nombre
        {
            get
            {
                return nombre;
            }
            set
            {
                nombre = value;
            }
        }        
        public string Tipo
        {
            get
            {
                return tipo;
            }
            set
            {
                tipo = value;
            }
        }
        public string NombreUsuario
        {
            get
            {
                return nombreUsuario;
            }
            set
            {
                nombreUsuario = value;
            }
        }

        public ObservableCollection<FotoMascota> Fotos
        {
            get
            {
                if(fotos==null)
                {
                    fotos = new ObservableCollection<FotoMascota>();
                }
                return fotos;
            }
            set
            {
                fotos = value;
            }
        }
        
    }
    
}
