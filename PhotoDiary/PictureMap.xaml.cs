using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Maps.Controls;
using System.Windows.Shapes;
using System.Windows.Media;

namespace PhotoDiary
{
    public partial class PictureMap : PhoneApplicationPage
    {
        public PictureMap()
        {
            InitializeComponent();
            var ml = new MapLayer();
            foreach (var x in PictureStore.Coordinates)
            {
                var me = new MapOverlay()
                {
                    GeoCoordinate = x,
                    Content = new Ellipse() { Fill = new SolidColorBrush(Colors.Red), Height = 5, Width = 5 }
                };
                ml.Add(me);
            }
            theMap.Layers.Add(ml);
        }
    }
}