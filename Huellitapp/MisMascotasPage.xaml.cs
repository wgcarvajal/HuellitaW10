using Huellitapp.Models;
using Parse;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
    public sealed partial class MisMascotasPage : Page
    {
        private Frame rootFrame;
        private ObservableCollection<Mascota> mascotasAdultos;
        private ObservableCollection<Mascota> mascotasCachorros;

        public MisMascotasPage()
        {
            this.InitializeComponent();
            rootFrame = Window.Current.Content as Frame;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            principalPage = e.Parameter as IPrincipalPage;            
        }

        public interface IPrincipalPage
        {
            void quitarAppBarButton();
            void ponerAppBarButton();
            void seleccionarMascotaActiva();
        }

        IPrincipalPage principalPage;

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
            .WhereEqualTo(Mascota.TIPO, "Adultos");
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
                mascota.Fotos = fotosMascotas;
                mascotasAdultos.Add(mascota);
            }

        }

        private async void loadDataCachorros()
        {

            var query = ParseObject.GetQuery(Mascota.TABLA)
            .WhereEqualTo(Mascota.TIPO, "Cachorros");
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
                mascota.Fotos = fotosMascotas;
                mascotasCachorros.Add(mascota);
            }

        }       

        private void seleccionarMascotaAdulta(object sender, SelectionChangedEventArgs e)
        {
            if(gridMascotasAdultas.SelectedIndex!=-1)
            {
                principalPage.seleccionarMascotaActiva();
            }           

        }

        private void seleccionarMascotaCachorro(object sender, SelectionChangedEventArgs e)
        {
            if(gridMascotasCachorros.SelectedIndex!=-1)
            {
                principalPage.seleccionarMascotaActiva();
            }          

        }

        private void pivoteSeleccionado(object sender, SelectionChangedEventArgs e)
        {           

            if (pivote.SelectedIndex==2)
            {
                principalPage.quitarAppBarButton();                
            }
            else
            {               
                principalPage.ponerAppBarButton();                
            }
            gridMascotasAdultas.SelectedIndex = -1;
            gridMascotasCachorros.SelectedIndex = -1;
        }
        
    }
}
