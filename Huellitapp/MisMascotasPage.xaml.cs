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
    public sealed partial class MisMascotasPage : Page, PrincipalPage.IMisMascotasPage
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
            ((PrincipalPage)e.Parameter).setMisMascotasPage(this);
            principalPage = e.Parameter as IPrincipalPage;            
        }

        public interface IPrincipalPage
        {
            void quitarAppBarButton();
            void adultosPonerAppBarButton();
            void cachorrosPonerAppBarButton();
            void seleccionarAdultoActivo(Mascota adulto);
            void seleccionarCachorroActivo(Mascota cachorro);
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
            .WhereEqualTo(Mascota.TIPO, "Adultos")
            .WhereEqualTo("username",ParseUser.CurrentUser.Username);
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
                if (mascota.Descripcion.Length > 45)
                {
                    mascota.DescripcionCorta = mascota.Descripcion.Substring(0, 45) + " . . . .";
                }
                else
                {
                    mascota.DescripcionCorta = mascota.Descripcion;
                }
                mascota.Edad = (string)parseObject[Mascota.EDAD] + " años";
                mascota.Fotos = fotosMascotas;
                mascotasAdultos.Add(mascota);
            }

        }

        private async void loadDataCachorros()
        {

            var query = ParseObject.GetQuery(Mascota.TABLA)
            .WhereEqualTo(Mascota.TIPO, "Cachorros")
            .WhereEqualTo("username", ParseUser.CurrentUser.Username);
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
                if (mascota.Descripcion.Length > 45)
                {
                    mascota.DescripcionCorta = mascota.Descripcion.Substring(0, 45) + " . . . .";
                }
                else
                {
                    mascota.DescripcionCorta = mascota.Descripcion;
                }
                mascota.Edad = (string)parseObject[Mascota.EDAD] + " meses";
                mascota.Fotos = fotosMascotas;
                mascotasCachorros.Add(mascota);
            }

        }       

        private void seleccionarMascotaAdulta(object sender, SelectionChangedEventArgs e)
        {
            if(gridMascotasAdultas.SelectedIndex!=-1)
            {
                principalPage.seleccionarAdultoActivo(mascotasAdultos.ElementAt(gridMascotasAdultas.SelectedIndex));
            }           

        }

        private void seleccionarMascotaCachorro(object sender, SelectionChangedEventArgs e)
        {
            if(gridMascotasCachorros.SelectedIndex!=-1)
            {
                principalPage.seleccionarCachorroActivo(mascotasCachorros.ElementAt(gridMascotasCachorros.SelectedIndex));
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
                if(pivote.SelectedIndex==0)
                {
                    principalPage.adultosPonerAppBarButton();
                } 
                else
                {
                    principalPage.cachorrosPonerAppBarButton();
                }             
                             
            }
            gridMascotasAdultas.SelectedIndex = -1;
            gridMascotasCachorros.SelectedIndex = -1;
        }

        public void actualizarColleccionMascota(Mascota mascota)
        {
            if(pivote.SelectedIndex==0)
            {
                mascotasAdultos.Add(mascota);
            }
            else
            {
                if(pivote.SelectedIndex==1)
                {
                    mascotasCachorros.Add(mascota);
                }
            }
        }

        public void eliminarMascotaSeleccionada()
        {
            Mascota mascota;
            if (gridMascotasAdultas.SelectedIndex!=-1)
            {
                mascota = mascotasAdultos.ElementAt(gridMascotasAdultas.SelectedIndex);
                mascotasAdultos.RemoveAt(gridMascotasAdultas.SelectedIndex);
                principalPage.adultosPonerAppBarButton();
            }
            else
            {
                mascota = mascotasCachorros.ElementAt(gridMascotasCachorros.SelectedIndex);
                mascotasCachorros.RemoveAt(gridMascotasCachorros.SelectedIndex);
                principalPage.cachorrosPonerAppBarButton();
            }
            eliminarMascota(mascota);
            gridMascotasAdultas.SelectedIndex = -1;
            gridMascotasCachorros.SelectedIndex = -1;
        }

        private async void eliminarMascota(Mascota mascota)
        {
            var dlg = new Windows.UI.Popups.MessageDialog(mascota.Id);
            await dlg.ShowAsync();
            var queryMascota = ParseObject.GetQuery(Mascota.TABLA)
            .WhereEqualTo(Mascota.ID, mascota.Id);
            IEnumerable<ParseObject> mascotas = await queryMascota.FindAsync();
            foreach(ParseObject m in mascotas)
            {
                await m.DeleteAsync();
            }
            
            var queryMensaje = ParseObject.GetQuery(Mensaje.TABLA)
            .WhereEqualTo(Mensaje.MASCOTA, mascota.Id);
            IEnumerable<ParseObject> mensajes = await queryMensaje.FindAsync();
            foreach(ParseObject mensaje in mensajes)
            {
                await mensaje.DeleteAsync();
            }
            var queryFoto = ParseObject.GetQuery(FotoMascota.TABLA)
            .WhereEqualTo(FotoMascota.IDMASCOTA, mascota.Id);
            IEnumerable<ParseObject> fotos = await queryFoto.FindAsync();
            foreach (ParseObject foto in fotos)
            {
                await foto.DeleteAsync();
            }
        }
    }
}
