using Huellitapp.Models;
using System;
using System.Collections;
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
using Windows.UI.Xaml.Navigation;

// La plantilla de elemento Página en blanco está documentada en http://go.microsoft.com/fwlink/?LinkId=234238

namespace Huellitapp
{
    /// <summary>
    /// Una página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class ConfirmarEliminar : Page
    {
        Frame rootFrame;

        public ConfirmarEliminar()
        {
            this.InitializeComponent();
            rootFrame = Window.Current.Content as Frame;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ArrayList arrayList = e.Parameter as ArrayList;
            principalPage = arrayList[0] as IPrincipalPage;
            string nombreMascota= arrayList[1] as string;
            textMensaje.Text = "Esta seguro que deseas eliminar la mascota "+nombreMascota+"?";            
        }

        public interface IPrincipalPage
        {
            void aceptarEliminar();           
        }

        IPrincipalPage principalPage;

        private void btnAceptar(object sender, RoutedEventArgs e)
        {
            principalPage.aceptarEliminar();
            rootFrame.GoBack();
        }

        private void btnCancelar(object sender, RoutedEventArgs e)
        {
            rootFrame.GoBack();
        }
    }
}
