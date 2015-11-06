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
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// La plantilla de elemento Página en blanco está documentada en http://go.microsoft.com/fwlink/?LinkId=234238

namespace Huellitapp
{
    /// <summary>
    /// Una página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class MascotaPage : Page
    {
        private Frame rootFrame;
        private Mascota mascota;
        private ObservableCollection<Mensaje> mensajes;     


        public MascotaPage()
        {
            this.InitializeComponent();
            rootFrame = Window.Current.Content as Frame;
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            SystemNavigationManager.GetForCurrentView().BackRequested += MascotaPage_BackRequested;
        }

        public Mascota Mascota
        {
            get
            {
                return mascota;
            }
            set
            {
                mascota = value;
            }

        }

        public ObservableCollection<Mensaje> Mensajes
        {
            get
            {
                if (mensajes == null)
                {
                    mensajes = new ObservableCollection<Mensaje>();
                    loadMensajes();
                }
                return mensajes;
            }
            set
            {
                mensajes = value;
            }
        }

        private void MascotaPage_BackRequested(object sender, BackRequestedEventArgs e)
        {
            if (e.Handled == false)
            {
                e.Handled = true;
                rootFrame.GoBack();                
            }
           
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            mascota = e.Parameter as Mascota;
            nombreMascota.Text = mascota.Nombre;
            int index = mascota.Edad.IndexOf(" ");
            textEdad.Text = mascota.Edad.Substring(0,index);
            textDescripcion.Text = mascota.Descripcion;
            if(mascota.Tipo.Equals("Adultos"))
            {
                textmesoAnio.Text = "Años";
            }
            else
            {
                textmesoAnio.Text = "Meses";
            }

            ImageBrush brush = new ImageBrush();
            brush.Stretch = Stretch.Uniform;

            BitmapImage image = new BitmapImage(new Uri(mascota.Fotos.ElementAt(0).Url));
            brush.ImageSource = image;
            imagenMascota.Fill = brush;

            pivotComentario.Header = "Comunicate con " + mascota.Nombre;
        }
        
        private async void loadMensajes()
        {
           
            var queryDueno = ParseUser.Query
            .WhereEqualTo("username", mascota.NombreUsuario);
            IEnumerable<ParseUser> duenoResults = await queryDueno.FindAsync();
            ParseUser dueno = duenoResults.First();



            var query = ParseObject.GetQuery(Mensaje.TABLA)
            .WhereEqualTo(Mensaje.MASCOTA, mascota.Id)
            .WhereEqualTo(Mensaje.ORIGEN, ParseUser.CurrentUser.Username);

            var query2 = ParseObject.GetQuery(Mensaje.TABLA)
            .WhereEqualTo(Mensaje.MASCOTA, mascota.Id)
            .WhereEqualTo(Mensaje.DESTINO, ParseUser.CurrentUser.Username)
            .Or(query)
            .OrderBy(Mensaje.FECHA);           

            IEnumerable<ParseObject> results = await query2.FindAsync();
            foreach(ParseObject msg in results)
            {               
                Mensaje mensaje = new Mensaje();
                mensaje.Message = (string)msg[Mensaje.MENSAJE];                
                mensaje.Mascota = mascota;               
                if (((String)msg[Mensaje.ORIGEN]).Equals(ParseUser.CurrentUser.Username))
                {
                    mensaje.Origen = ParseUser.CurrentUser;
                    mensaje.OrigenAlias = "Tú";
                    mensaje.Destino = dueno;
                }
                else
                {
                    mensaje.Origen = dueno;
                    mensaje.OrigenAlias = "Cuidador de "+ mascota.Nombre;
                    mensaje.Destino = ParseUser.CurrentUser;
                }
                mensajes.Add(mensaje);               
            }
        }

        private async void btnEnviar(object sender, RoutedEventArgs e)
        {
            if(textMensaje.Text!="")
            {
                Mensaje mensaje = new Mensaje();
                mensaje.Mascota = mascota;
                mensaje.Origen = ParseUser.CurrentUser;
                mensaje.OrigenAlias = "Tú";
                var queryDueno = ParseUser.Query
                .WhereEqualTo("username", mascota.NombreUsuario);
                IEnumerable<ParseUser> duenoResults = await queryDueno.FindAsync();
                ParseUser dueno = duenoResults.First();
                mensaje.Destino = dueno;
                mensaje.Message = textMensaje.Text;
                textMensaje.Text = "";
                mensajes.Add(mensaje);
                var parseMensaje = new ParseObject(Mensaje.TABLA)
                {
                    { Mensaje.MASCOTA, mascota.Id },
                    { Mensaje.MENSAJE, mensaje.Message },
                    { Mensaje.DESTINO, dueno.Username},
                    { Mensaje.ORIGEN, ParseUser.CurrentUser.Username},
                };
                await parseMensaje.SaveAsync();
            }
        }
    }
    
}
