using Huellitapp.Models;
using Parse;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public sealed partial class EditarMascotaPage : Page, ConfirmarEliminarFoto.IEditarMascotaPage
    {
        private Mascota mascota;
        Frame rootFrame;
        public EditarMascotaPage()
        {
            this.InitializeComponent();
            rootFrame = Window.Current.Content as Frame;
            Loaded += EditarMascotaPage_Loaded;
            this.NavigationCacheMode = NavigationCacheMode.Enabled;
        }
        private void EditarMascotaPage_Loaded(object sender, RoutedEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            SystemNavigationManager.GetForCurrentView().BackRequested += EditarMascotaPage_BackRequested;
        }

        private void EditarMascotaPage_BackRequested(object sender, BackRequestedEventArgs e)
        {
            if (e.Handled == false)
            {
                e.Handled = true;
                rootFrame.GoBack();
            }
        }

        public Mascota Mascotta
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

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            mascota = e.Parameter as Mascota;
            textNombre.Text = mascota.Nombre;
            cargarComboEdad();
            int indice = mascota.Edad.IndexOf(" ");
            string edad = mascota.Edad.Substring(0, indice);
            comboEdad.SelectedIndex = Convert.ToInt32(edad) -1;
            if(mascota.Tipo.Equals("Adultos"))
            {
                textAniosOmeses.Text = "Años";
            }
            else
            {
                textAniosOmeses.Text = "Meses";
            }

            textDescripcion.Text = mascota.Descripcion; 
            if(pivote.SelectedIndex==0)
            {
                pagina.BottomAppBar = null;
            }          
            gridEditarGaleria.SelectedIndex = -1;
            
        }
        private void cargarComboEdad()
        {

            for (int i = 0; i < 12; i++)
            {
                comboEdad.Items.Add((i + 1) + "");
            }
            
        }

        private async void aceptarEdicion(object sender, RoutedEventArgs e)
        {
            if(textNombre.Text!="" && textDescripcion.Text!="")
            {
                mascota.Nombre = textNombre.Text;
                mascota.Descripcion = textDescripcion.Text;
                if(mascota.Descripcion.Length > 45)
                {
                    mascota.DescripcionCorta= mascota.Descripcion.Substring(0, 45) + " . . . .";
                }
                else
                {
                    mascota.DescripcionCorta = mascota.Descripcion;
                }
                mascota.Edad = (string)comboEdad.SelectedItem +" "+textAniosOmeses.Text ;
                var query = ParseObject.GetQuery(Mascota.TABLA)
                .WhereEqualTo(Mascota.ID, mascota.Id);
                IEnumerable<ParseObject> mascotas = await query.FindAsync();
                ParseObject m = mascotas.ElementAt(0);
                m[Mascota.DESCRIPCION] = mascota.Descripcion;
                m[Mascota.NOMBRE] = mascota.Nombre;
                m[Mascota.EDAD] = comboEdad.SelectedItem;
                await m.SaveAsync();
                rootFrame.GoBack();
            }
        }

        private void privoteSeleccionado(object sender, SelectionChangedEventArgs e)
        {
            if(pivote.SelectedIndex==1)
            {
                CommandBar commandBar = new CommandBar();
                AppBarButton appBarButton = new AppBarButton();
                appBarButton.Icon = new SymbolIcon(Symbol.Add);
                appBarButton.Label = "Agregar";
                appBarButton.Click += agregarFoto;
                commandBar.PrimaryCommands.Add(appBarButton);
                pagina.BottomAppBar = commandBar;

            }
            else
            {
                pagina.BottomAppBar = null;
                gridEditarGaleria.SelectedIndex = -1;
            }
        }

        private void agregarFoto(object sender, RoutedEventArgs e)
        {
            rootFrame.Navigate(typeof(AgregarFotoMascotaPage),mascota);
        }

        private void seleccionarFoto(object sender, SelectionChangedEventArgs e)
        {
            if(gridEditarGaleria.SelectedIndex!=-1)
            {
                CommandBar commandBar = new CommandBar();
                AppBarButton appBarButton = new AppBarButton();
                appBarButton.Icon = new SymbolIcon(Symbol.Add);
                appBarButton.Label = "Agregar";
                appBarButton.Click += agregarFoto;
                commandBar.PrimaryCommands.Add(appBarButton);
                appBarButton = new AppBarButton();
                appBarButton.Icon = new SymbolIcon(Symbol.Delete);
                appBarButton.Label = "Eliminar";
                appBarButton.Click += eliminarFoto;
                commandBar.PrimaryCommands.Add(appBarButton);
                pagina.BottomAppBar = commandBar;
            }
        }

        private void eliminarFoto(object sender, RoutedEventArgs e)
        {
            rootFrame.Navigate(typeof(ConfirmarEliminarFoto), mascota.Fotos.ElementAt(gridEditarGaleria.SelectedIndex));
        }

        public void aceptarEliminarFoto()
        {
            
        }
    }
}
