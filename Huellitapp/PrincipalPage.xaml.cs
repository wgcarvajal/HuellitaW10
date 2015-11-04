using Huellitapp.Models;
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
   
    public sealed partial class PrincipalPage : Page , MascotasPage.IMascotaSeleccionada
    {
        public PrincipalPage()
        {
            this.InitializeComponent();
            this.Loaded += PrincipalPage_Loaded;           
        }

        private void PrincipalPage_Loaded(object sender, RoutedEventArgs e)
        {
            Contenido.Navigate(typeof(MascotasPage),this);
        }

        private ObservableCollection<MenuItem> menuList;

        public ObservableCollection<MenuItem> MenuList
        {
            get
            {
                if (menuList == null)
                {
                    menuList = new ObservableCollection<MenuItem>();

                    MenuItem item = new MenuItem() { Name = "Favoritos", Icon = "Favorite" };
                    MenuItem item1 = new MenuItem() { Name = "Halloween", Icon = "Emoji2" };
                    MenuItem item2 = new MenuItem() { Name = "Recientes", Icon = "Camera" };
                    MenuItem item3 = new MenuItem() { Name = "Mapa", Icon = "Map" };
                    MenuItem item4 = new MenuItem() { Name = "Calendario", Icon = "Calendar" };

                    menuList.Add(item);
                    menuList.Add(item1);
                    menuList.Add(item2);
                    menuList.Add(item3);
                    menuList.Add(item4);

                }
                return menuList;
            }
            set { menuList = value; }
        }       

        private void showMenu(object sender, RoutedEventArgs e)
        {
            if (split.IsPaneOpen)
                split.IsPaneOpen = false;
            else
                split.IsPaneOpen = true;

        }

        public void mascotaSeleccionada(Mascota mascota)
        {
            mostrarDialogo(mascota);
           
        }

        public async void mostrarDialogo(Mascota m)
        {
            var dlg = new Windows.UI.Popups.MessageDialog("Nombre Mascota :"+m.Nombre);
            //dlg.Title = "Mensaje";
            await dlg.ShowAsync();
        }
    }
}
