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
    public sealed partial class MascotasPage : Page,PrincipalPage.IQuitarSeleccion
    {
        private Frame rootFrame;
        private ObservableCollection<Mascota> mascotasAdultos;
        private ObservableCollection<Mascota> mascotasCachorros;


        public MascotasPage()
        {
            this.InitializeComponent();            
            rootFrame = Window.Current.Content as Frame;
           
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            mascotaSelecionada= e.Parameter as IMascotaSeleccionada;
            ((PrincipalPage)e.Parameter).setGridview(this);
        }

        public interface IMascotaSeleccionada
        {
            void setMascotaSeleccionada(Mascota mascota);
        }

        IMascotaSeleccionada mascotaSelecionada;

        public ObservableCollection<Mascota> MascotasAdultos
        {
            get
            {
                if (mascotasAdultos == null)
                {
                    mascotasAdultos = new ObservableCollection<Mascota>();
                    loadDataAdultos();
                }
                return mascotasAdultos;
            }
            set
            {
                mascotasAdultos = value;

            }
        }

        public ObservableCollection<Mascota> MascotasCachorros
        {
            get
            {
                if (mascotasCachorros == null)
                {
                    mascotasCachorros = new ObservableCollection<Mascota>();
                    loadDataCachorros();
                }
                return mascotasCachorros;
            }
            set
            {
                mascotasCachorros = value;

            }
        }

        private async void loadDataAdultos()
        {

            var query = ParseObject.GetQuery(Mascota.TABLA)
            .WhereEqualTo(Mascota.TIPO, "Adultos")
            .WhereNotEqualTo(Mascota.NOMBREUSUARIO,ParseUser.CurrentUser.Username);
            IEnumerable<ParseObject> results = await query.FindAsync();                        
            foreach(ParseObject parseObject in results)
            {
                Mascota mascota = new Mascota();
                var queryfoto= ParseObject.GetQuery(FotoMascota.TABLA)
                .WhereEqualTo(FotoMascota.IDMASCOTA, parseObject.ObjectId);
                IEnumerable<ParseObject> fotos = await queryfoto.FindAsync();
                ObservableCollection<FotoMascota> fotosMascotas = new ObservableCollection<FotoMascota>();
                foreach (ParseObject foto in fotos)
                {
                    FotoMascota fotoMascota = new FotoMascota();
                    fotoMascota.Id = foto.ObjectId;
                    fotoMascota.IdMascota = (string)foto[FotoMascota.IDMASCOTA];
                    fotoMascota.Url = foto.Get<ParseFile>(FotoMascota.IMAGEN).Url.OriginalString;
                    fotosMascotas.Add(fotoMascota);
                }
                mascota.Nombre = (String )parseObject[Mascota.NOMBRE];
                mascota.Tipo = (string)parseObject[Mascota.TIPO];
                mascota.NombreUsuario = (string)parseObject[Mascota.NOMBREUSUARIO];
                mascota.Id = parseObject.ObjectId;
                mascota.Descripcion = (string)parseObject[Mascota.DESCRIPCION];
                if (mascota.Descripcion.Length > 45)
                {
                    mascota.DescripcionCorta = mascota.Descripcion.Substring(0,45)+" . . . .";
                }
                else
                {
                    mascota.DescripcionCorta = mascota.Descripcion;
                }                
                mascota.Edad = (string)parseObject[Mascota.EDAD] +" Años";
                mascota.Fotos = fotosMascotas;               
                mascotasAdultos.Add(mascota);
            }            
            
        }

        private async void loadDataCachorros()
        {

            var query = ParseObject.GetQuery(Mascota.TABLA)
            .WhereEqualTo(Mascota.TIPO, "Cachorros")
            .WhereNotEqualTo(Mascota.NOMBREUSUARIO, ParseUser.CurrentUser.Username);
            IEnumerable<ParseObject> results = await query.FindAsync();
            foreach (ParseObject parseObject in results)
            {
                Mascota mascota = new Mascota();
                var queryfoto = ParseObject.GetQuery(FotoMascota.TABLA)
                .WhereEqualTo(FotoMascota.IDMASCOTA, parseObject.ObjectId);
                IEnumerable<ParseObject> fotos = await queryfoto.FindAsync();
                ObservableCollection<FotoMascota> fotosMascotas = new ObservableCollection<FotoMascota>();
                foreach (ParseObject foto in fotos)
                {
                    FotoMascota fotoMascota = new FotoMascota();
                    fotoMascota.Id = foto.ObjectId;
                    fotoMascota.IdMascota = (string)foto[FotoMascota.IDMASCOTA];
                    fotoMascota.Url = foto.Get<ParseFile>(FotoMascota.IMAGEN).Url.OriginalString;
                    fotosMascotas.Add(fotoMascota);
                }
                mascota.Nombre = (String)parseObject[Mascota.NOMBRE];
                mascota.Tipo = (string)parseObject[Mascota.TIPO];
                mascota.NombreUsuario = (string)parseObject[Mascota.NOMBREUSUARIO];
                mascota.Id = parseObject.ObjectId;
                mascota.Descripcion = (string)parseObject[Mascota.DESCRIPCION];
                if(mascota.Descripcion.Length > 45)
                {
                    mascota.DescripcionCorta = mascota.Descripcion.Substring(0, 45)+" . . . .";
                }
                else
                {
                    mascota.DescripcionCorta = mascota.Descripcion;
                }
                mascota.Edad = (string)parseObject[Mascota.EDAD] + " Meses";
                mascota.Fotos = fotosMascotas;
                mascotasCachorros.Add(mascota);
            }

        }



        private void logout(object sender, RoutedEventArgs e)
        {
            ParseUser.LogOut();
            rootFrame.Navigate(typeof(MainPage));
        }

        private void seleccionarMascotaAdulta(object sender, SelectionChangedEventArgs e)
        {
            if(gridMascotasAdultas.SelectedIndex !=-1)
            {               
                mascotaSelecionada.setMascotaSeleccionada(mascotasAdultos.ElementAt(gridMascotasAdultas.SelectedIndex));
            }
            
        }

        private void seleccionarMascotaCachorro(object sender, SelectionChangedEventArgs e)
        {
            if (gridMascotasCachorros.SelectedIndex != -1)
            {
                mascotaSelecionada.setMascotaSeleccionada(mascotasCachorros.ElementAt(gridMascotasCachorros.SelectedIndex));
            }

        }

        public void setQuitarSeleccion()
        {
            gridMascotasCachorros.SelectedIndex = -1;
            gridMascotasAdultas.SelectedIndex = -1;
        }
    }
}
