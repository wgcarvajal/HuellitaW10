using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Huellitapp.Models
{
    public class Mascota : INotifyPropertyChanged
    {

        public static string TABLA = "mascota";//nombre de la tabla en la base de datos
        //atributos de la tabla mascotas
        public static string ID = "objectId";// identificador de la tabla mascota
        public static string NOMBRE = "masnombre";//nombre mascota
        public static string TIPO = "tiponombre";// tipo mascota Adultos o Cachorros
        public static string NOMBREUSUARIO = "username";//nombre de usuario del propietario actual de la mascota
        public static string DESCRIPCION = "masdescripcion";
        public static string EDAD = "masedad";


        private string id;
        private string nombre;       
        private string tipo;
        private string nombreUsuario;
        private string descripcion;
        private string descripcionCorta;
        private string edad;
        
        private ObservableCollection<FotoMascota> fotos;

        public event PropertyChangedEventHandler PropertyChanged;

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
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Nombre"));
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

        public string Descripcion
        {
            get
            {
                
                return descripcion;
            }
            set
            {
                descripcion = value;
            }
        }

        public string Edad
        {
            get
            {
                return edad;
            }
            set
            {
                edad = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Edad"));
            }
        }

        public string DescripcionCorta
        {
            get
            {

                return descripcionCorta;
            }
            set
            {
                descripcionCorta = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("DescripcionCorta"));
            }
        }

        
    }
    
}
