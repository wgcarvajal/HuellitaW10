using Huellitapp.Models;
using Parse;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
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
    public sealed partial class AgregarMascotaPage : Page
    {
        private StorageFile file;
        private string tipomascota;
        private Frame rootFrame;

        public AgregarMascotaPage()
        {
            this.InitializeComponent();
            rootFrame = Window.Current.Content as Frame;
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            SystemNavigationManager.GetForCurrentView().BackRequested += AgregarMascotaPage_BackRequested;
        }

        public interface IPrincipalPage
        {
            void notificarSeAgregoLaMascota(Mascota mascota);
        }
        IPrincipalPage principalPage;

        private void AgregarMascotaPage_BackRequested(object sender, BackRequestedEventArgs e)
        {
            if (e.Handled == false)
            {
                e.Handled = true;
                rootFrame.GoBack();
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ArrayList arrayList = e.Parameter as ArrayList;
            principalPage = arrayList[0] as IPrincipalPage;
            tipomascota =arrayList[1] as string;                 
            titulo.Text = "Agregar "+tipomascota;
        }

        private async void seleccionarImagen(object sender, RoutedEventArgs e)
        {
            FileOpenPicker picker = new FileOpenPicker();
            picker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add(".png");
            file = await picker.PickSingleFileAsync();

            BitmapImage image = new BitmapImage();
            try
            {
                using (var stream = await file.OpenAsync(FileAccessMode.Read))
                {
                    await image.SetSourceAsync(stream);
                }
            }
            catch(Exception exception)
            {

            } 
            ImageBrush brush = new ImageBrush();
            brush.Stretch = Stretch.UniformToFill;            
            brush.ImageSource = image;
            imagen.Fill = brush;
        }

        private async void btnAceptar(object sender, RoutedEventArgs e)
        {
            if(file!=null && nombre.Text!="")
            {
                var stream = await file.OpenAsync(FileAccessMode.Read);

                ParseFile fileP = new ParseFile(file.Name, stream.AsStream());

                await fileP.SaveAsync();

                ObservableCollection<FotoMascota> fotos = new ObservableCollection<FotoMascota>();
                Mascota mascota = new Mascota();
                FotoMascota foto = new FotoMascota();
                foto.Url = fileP.Url.OriginalString;                
                mascota.Nombre = nombre.Text;
                mascota.Tipo = tipomascota;
                mascota.NombreUsuario = ParseUser.CurrentUser.Username;
                
                var parseMascota = new ParseObject(Mascota.TABLA)
                {
                    { Mascota.TIPO, tipomascota },
                    { Mascota.NOMBRE, nombre.Text },
                    { Mascota.NOMBREUSUARIO, ParseUser.CurrentUser.Username},
                };
                await parseMascota.SaveAsync();
                mascota.Id = parseMascota.ObjectId;
                foto.IdMascota = parseMascota.ObjectId;
                var imagenMascota = new ParseObject(FotoMascota.TABLA)
                {
                    {FotoMascota.IDMASCOTA,parseMascota.ObjectId},
                    {FotoMascota.IMAGEN,fileP},
                };
                await imagenMascota.SaveAsync();
                foto.Id = imagenMascota.ObjectId;
                fotos.Add(foto);
                mascota.Fotos = fotos;
                principalPage.notificarSeAgregoLaMascota(mascota);
                rootFrame.GoBack();
            }
            else
            {
                var dlg = new Windows.UI.Popups.MessageDialog("Por favor ingrese todos los campos.");
                await dlg.ShowAsync();
            }
            
        }
    }
}
