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
   
    public sealed partial class PrincipalPage : Page , MascotasPage.IMascotaSeleccionada, MisMascotasPage.IPrincipalPage,AgregarMascotaPage.IPrincipalPage
    {
        private Frame rootFrame;

        public interface IMisMascotasPage
        {
            void actualizarColleccionMascota(Mascota mascota);
        }

        IMisMascotasPage misMascotasPage;

        public interface IQuitarSeleccion
        {
            void setQuitarSeleccion();
        }

        IQuitarSeleccion quitarSeleccion;

        public PrincipalPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Enabled;
            Loaded += PrincipalPage_Loaded;            
            Contenido.Navigate(typeof(MascotasPage), this);           
            rootFrame = Window.Current.Content as Frame;
        }

        private void PrincipalPage_Loaded(object sender, RoutedEventArgs e)
        {
            if(menu.SelectedIndex==-1 || menu.SelectedIndex==4)
            {
                menu.SelectedIndex = 0;
            }            
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            quitarSeleccion.setQuitarSeleccion();
        }        

        private ObservableCollection<MenuItem> menuList;

        public ObservableCollection<MenuItem> MenuList
        {
            get
            {
                if (menuList == null)
                {
                    menuList = new ObservableCollection<MenuItem>();

                    MenuItem item = new MenuItem() { Name = "Inicio", Icon = "Home" };
                    MenuItem item1 = new MenuItem() { Name = "Mis publicaciones", Icon = "Emoji2" };
                    MenuItem item2 = new MenuItem() { Name = "Recientes", Icon = "Camera" };
                    MenuItem item3 = new MenuItem() { Name = "Mapa", Icon = "Map" };
                    MenuItem item4 = new MenuItem() { Name = "Cerrar sesión", Icon = "ClosePane" };        

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

        public void setMascotaSeleccionada(Mascota mascota)
        {
            rootFrame.Navigate(typeof(MascotaPage),mascota);
        }

        public void setGridview(IQuitarSeleccion interfaz)
        {
            quitarSeleccion = interfaz;           
        }

        public void setMisMascotasPage(IMisMascotasPage interfaz)
        {
            misMascotasPage = interfaz;
        }

        private void seleccionMenu(object sender, SelectionChangedEventArgs e)
        {
            switch(menu.SelectedIndex)
            {
                case 0:
                    Contenido.Navigate(typeof(MascotasPage), this);
                    paginaPrincipal.BottomAppBar = null;
                break;
                case 1:
                    Contenido.Navigate(typeof(MisMascotasPage),this);
                    CommandBar commandBar = new CommandBar();
                    AppBarButton appBarButton = new AppBarButton();
                    appBarButton.Icon =new SymbolIcon(Symbol.Add);
                    appBarButton.Label = "Agregar";
                    commandBar.PrimaryCommands.Add(appBarButton);
                    paginaPrincipal.BottomAppBar = commandBar;
                break;
                case 4:
                    ParseUser.LogOut();
                    rootFrame.Navigate(typeof(MainPage));
                break;
            }           
        }

        public void quitarAppBarButton()
        {
            paginaPrincipal.BottomAppBar = null;
        }

        public void adultosPonerAppBarButton()
        {
            CommandBar commandBar = new CommandBar();
            AppBarButton appBarButton = new AppBarButton();
            appBarButton.Icon = new SymbolIcon(Symbol.Add);
            appBarButton.Label = "Agregar";
            appBarButton.Click += agregarAdulto;
            commandBar.PrimaryCommands.Add(appBarButton);
            paginaPrincipal.BottomAppBar = commandBar;
        }

        public void cachorrosPonerAppBarButton()
        {
            CommandBar commandBar = new CommandBar();
            AppBarButton appBarButton = new AppBarButton();
            appBarButton.Icon = new SymbolIcon(Symbol.Add);
            appBarButton.Label = "Agregar";
            appBarButton.Click += agregarCachorro;
            commandBar.PrimaryCommands.Add(appBarButton);
            paginaPrincipal.BottomAppBar = commandBar;
        }


        public void seleccionarAdultoActivo()
        {            
            CommandBar commandBar = new CommandBar();
            AppBarButton appBarButton = new AppBarButton();
            appBarButton.Icon = new SymbolIcon(Symbol.Add);
            appBarButton.Label = "Agregar";
            appBarButton.Click += agregarAdulto;            
            commandBar.PrimaryCommands.Add(appBarButton);
            appBarButton = new AppBarButton();
            appBarButton.Icon = new SymbolIcon(Symbol.Delete);
            appBarButton.Label = "Eliminar";
            commandBar.PrimaryCommands.Add(appBarButton);
            appBarButton = new AppBarButton();
            appBarButton.Icon = new SymbolIcon(Symbol.Edit);
            appBarButton.Label = "Editar";
            commandBar.PrimaryCommands.Add(appBarButton);
            paginaPrincipal.BottomAppBar = commandBar;
        }

        public void seleccionarCachorroActivo()
        {
            CommandBar commandBar = new CommandBar();
            AppBarButton appBarButton = new AppBarButton();
            appBarButton.Icon = new SymbolIcon(Symbol.Add);
            appBarButton.Label = "Agregar";
            appBarButton.Click += agregarCachorro;
            commandBar.PrimaryCommands.Add(appBarButton);
            appBarButton = new AppBarButton();
            appBarButton.Icon = new SymbolIcon(Symbol.Delete);
            appBarButton.Label = "Eliminar";
            commandBar.PrimaryCommands.Add(appBarButton);
            appBarButton = new AppBarButton();
            appBarButton.Icon = new SymbolIcon(Symbol.Edit);
            appBarButton.Label = "Editar";
            commandBar.PrimaryCommands.Add(appBarButton);
            paginaPrincipal.BottomAppBar = commandBar;
        }

        private void agregarAdulto(object sender, RoutedEventArgs e)
        {
            ArrayList arrayList = new ArrayList();
            arrayList.Add(this);
            arrayList.Add("Adultos");
            rootFrame.Navigate(typeof(AgregarMascotaPage),arrayList);
        }

        private void agregarCachorro(object sender, RoutedEventArgs e)
        {
            ArrayList arrayList = new ArrayList();
            arrayList.Add(this);
            arrayList.Add("Cachorros");
            rootFrame.Navigate(typeof(AgregarMascotaPage), arrayList);
        }

        public void notificarSeAgregoLaMascota(Mascota mascota)
        {
            misMascotasPage.actualizarColleccionMascota(mascota);
        }

        
    }
}
