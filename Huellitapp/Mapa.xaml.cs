using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Geolocation;
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
    public sealed partial class Mapa : Page
    {
        Geolocator g = new Geolocator();
        public Mapa()
        {
            this.InitializeComponent();

            /*Geoposition myLocation = await g.GetGeopositionAsync();
            var latitud = myLocation.Coordinate.Latitude;
            var longitud = myLocation.Coordinate.Longitude;
            Map MyMap = new Map();*/
            //Contenedor.Children.Add(MyMap);
            //ContentPanel.Children.Add(MyMap);
            //ContentPanel.Children.Add(MyMap);
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Specify a known location
            BasicGeoposition cityPosition = new BasicGeoposition() { Latitude = 2.459167, Longitude = -76.600278 };
            Geopoint cityCenter = new Geopoint(cityPosition);

            // Set map location
            MapControl1.Center = cityCenter;
            MapControl1.ZoomLevel = 14;
            MapControl1.LandmarksVisible = true;
        }
    }
}
