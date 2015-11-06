using Parse;
using System;
using System.Collections.Generic;
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
using Windows.UI.Xaml.Navigation;

// La plantilla de elemento Página en blanco está documentada en http://go.microsoft.com/fwlink/?LinkId=234238

namespace Huellitapp
{
    /// <summary>
    /// Una página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    /// 
    public static class Utils
    {
        public static bool IsAny<T>(this IEnumerable<T> data)
        {
            return data != null && data.Any();
        }
    }
    public sealed partial class RegistroPage : Page
    {
        Frame rootFrame;
        public RegistroPage()
        {
            this.InitializeComponent();
            rootFrame = Window.Current.Content as Frame;
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            SystemNavigationManager.GetForCurrentView().BackRequested += RegistroPage_BackRequested;       
        }

        private void RegistroPage_BackRequested(object sender, BackRequestedEventArgs e)
        {
            if (e.Handled == false)
            {
                e.Handled = true;
                rootFrame.GoBack();
            }
        }

        public async void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            int bandera = 0;
            var user = new ParseUser()
            {
                Username = usuario.Text,
                Password = password.Password,
                Email = email.Text,                
            };
            user["nombre"] = nombre.Text;
            if(usuario.Text!="")
            {
                var query = ParseObject.GetQuery("User").WhereEqualTo("username", usuario.Text);
                IEnumerable<ParseObject> results = await query.FindAsync();
                if(results.IsAny())
                {
                    bandera = 1;
                    var dlg = new Windows.UI.Popups.MessageDialog("Ya existe el nombre de usuario");
                    await dlg.ShowAsync();
                }
            }
            else
            {
                bandera = 1;
                var dlg = new Windows.UI.Popups.MessageDialog("Digite su nombre de usuario por favor");
                await dlg.ShowAsync();
            }
            if (email.Text != "")
            {
                var query = ParseObject.GetQuery("User").WhereEqualTo("email", email.Text);
                IEnumerable<ParseObject> results2 = await query.FindAsync();
                if (results2.IsAny())
                {
                    bandera = 1;
                    var dlg = new Windows.UI.Popups.MessageDialog("El correo digitado ya está en uso");
                    await dlg.ShowAsync();
                }
            }
            else
            {
                bandera = 1;
                var dlg = new Windows.UI.Popups.MessageDialog("Digite su correo por favor");
                await dlg.ShowAsync();
            }
            if(password.Password=="")
            {
                bandera = 1;
                var dlg = new Windows.UI.Popups.MessageDialog("Digite su contraseña por favor");
                await dlg.ShowAsync();
            }
            if (nombre.Text == "")
            {
                bandera = 1;
                var dlg = new Windows.UI.Popups.MessageDialog("Digite nombre por favor");
                await dlg.ShowAsync();
            }
            if (bandera==0)
            {
                await user.SignUpAsync();
                var dlg = new Windows.UI.Popups.MessageDialog("Usuario registrado con exito");
                await dlg.ShowAsync();
                rootFrame.GoBack();
            }
        }

    }
}
