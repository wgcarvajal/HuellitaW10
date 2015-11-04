using Parse;
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
using Windows.UI.Xaml.Navigation;

// La plantilla de elemento Página en blanco está documentada en http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Huellitapp
{
    /// <summary>
    /// Página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private Frame rootFrame;
        public MainPage()
        {
            this.InitializeComponent();
            this.Loaded += MainPage_Loaded;
            rootFrame = Window.Current.Content as Frame;
            
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (ParseUser.CurrentUser != null)
            {
                rootFrame.Navigate(typeof(PrincipalPage));
            }
        }       

        private async void iniciarSesion(object sender, RoutedEventArgs e)
        {

            try
            {
                await ParseUser.LogInAsync(user.Text, pass.Text);
                rootFrame.Navigate(typeof(PrincipalPage));

            }
            catch (Exception exception)
            {

            }


        }
    }
}
