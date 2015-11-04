using Huellitapp.Models;
using Parse;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


// La plantilla de elemento Página en blanco está documentada en http://go.microsoft.com/fwlink/?LinkId=234238

namespace Huellitapp
{
    /// <summary>
    /// Una página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class MascotasPage : Page
    {
        private Frame rootFrame;
       
        
        public MascotasPage()
        {
            this.InitializeComponent();            
            rootFrame = Window.Current.Content as Frame;
        }

        private ObservableCollection<Mascota> mascotas;

        public ObservableCollection<Mascota> Mascotas
        {
            get
            {
                if (mascotas == null)
                {
                    mascotas = new ObservableCollection<Mascota>();
                    loadData();
                }
                return mascotas;
            }
            set
            {
                mascotas = value;

            }
        }

        private async void loadData()
        {

            var query = ParseObject.GetQuery(MascotaParse.TABLA);            
            IEnumerable<ParseObject> results = await query.FindAsync();            
            foreach(ParseObject parseObject in results)
            {
                Mascota mascota = new Mascota();
                var queryfoto= ParseObject.GetQuery("fotomascota")
                .WhereEqualTo("mascota", parseObject.ObjectId);
                IEnumerable<ParseObject> fotos = await queryfoto.FindAsync();
                ParseObject foto = fotos.ElementAt<ParseObject>(0);
                var applicantResumeFile = foto.Get<ParseFile>("foto");
                mascota.Url = applicantResumeFile.Url.OriginalString;
                mascota.Nombre = (String )parseObject["masnombre"];
                mascotas.Add(mascota);
            }            
            
        }       

        private void logout(object sender, RoutedEventArgs e)
        {
            ParseUser.LogOut();
            rootFrame.Navigate(typeof(MainPage));
        }

        private void agregar(object sender, RoutedEventArgs e)
        {
            Mascota mascota = new Mascota();
            mascota.Url = "http://www.masimagenesbonitas.com/wp-content/uploads/2015/08/u-saludito.jpg";
            mascota.Nombre = "pruebita";
            mascotas.Add(mascota);
        }
    }
}
