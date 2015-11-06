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
    public sealed partial class AgregarFotoMascotaPage : Page
    {
        private Mascota mascota;
        Frame rootFrame;
        private StorageFile file;
        public AgregarFotoMascotaPage()
        {
            this.InitializeComponent();
            rootFrame = Window.Current.Content as Frame;           
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            SystemNavigationManager.GetForCurrentView().BackRequested += AgregarFotoMascotaPage_BackRequested;
        }

        private void AgregarFotoMascotaPage_BackRequested(object sender, BackRequestedEventArgs e)
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
                ImageBrush brush = new ImageBrush();
                brush.Stretch = Stretch.UniformToFill;
                brush.ImageSource = image;
                imagen.Fill = brush;
            }
            catch (Exception exception)
            {

            }
            
        }

        private async void btnAceptar(object sender, RoutedEventArgs e)
        {
            if (file != null)
            {
                var stream = await file.OpenAsync(FileAccessMode.Read);

                ParseFile fileP = new ParseFile(file.Name, stream.AsStream());

                await fileP.SaveAsync();
                               
                FotoMascota foto = new FotoMascota();
                foto.Url = fileP.Url.OriginalString;         
                
                foto.IdMascota = mascota.Id;
                var imagenMascota = new ParseObject(FotoMascota.TABLA)
                {
                    {FotoMascota.IDMASCOTA,mascota.Id},
                    {FotoMascota.IMAGEN,fileP},
                };
                await imagenMascota.SaveAsync();
                foto.Id = imagenMascota.ObjectId;
                mascota.Fotos.Add(foto);                             
                rootFrame.GoBack();
            }
            else
            {
                var dlg = new Windows.UI.Popups.MessageDialog("Debe seleccionar una imagen.");
                await dlg.ShowAsync();
            }
        }
    }
}
