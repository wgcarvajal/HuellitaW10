﻿using Huellitapp.Models;
using System;
using System.Collections.Generic;
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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// La plantilla de elemento Página en blanco está documentada en http://go.microsoft.com/fwlink/?LinkId=234238

namespace Huellitapp
{
    /// <summary>
    /// Una página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class ConfirmarEliminarFoto : Page
    {
        Frame rootFrame;

        public ConfirmarEliminarFoto()
        {
            this.InitializeComponent();
            rootFrame = Window.Current.Content as Frame;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            FotoMascota foto = e.Parameter as FotoMascota;
            ImageBrush brush = new ImageBrush();
            brush.Stretch = Stretch.Uniform;

            BitmapImage image = new BitmapImage(new Uri(foto.Url));
            brush.ImageSource = image;
            imagenMascota.Fill = brush;
        }

        public interface IEditarMascotaPage
        {
            void aceptarEliminarFoto();
        }

        IEditarMascotaPage editarMascotaPage;

        private void btnCancelar(object sender, RoutedEventArgs e)
        {
            rootFrame.GoBack();
        }

        private void btnAceptar(object sender, RoutedEventArgs e)
        {
            editarMascotaPage.aceptarEliminarFoto();
            rootFrame.GoBack();
        }
    }
}
