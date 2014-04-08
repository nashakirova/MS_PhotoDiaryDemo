using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Windows.Devices.Sensors;
using System.Windows.Threading;

namespace PhotoDiary
{
    public partial class GraviViewer : PhoneApplicationPage
    {

        List<ImageInfo> list;
        int current;
        Accelerometer acc;
        DispatcherTimer dt = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(1) };
        double count = 0;

        public GraviViewer()
        {
            InitializeComponent();
            list = new PictureStore().Images;
            current = list.Count / 2;
            Display();
            acc = Accelerometer.GetDefault();
            dt.Tick += dt_Tick;
            dt.Start();
        }

        void dt_Tick(object sender, EventArgs e)
        {
            var rd = acc.GetCurrentReading();
            if (Math.Abs(count)>1.2)
            {
                if (count < 0 && current>0) current--;
                if (count > 0 && current<list.Count-1) current++;
                Display();
                count = 0;
            }
            else
            {
                count += rd.AccelerationX;
            }
        }

        private void Display()
        {
            txt.Text = list[current].Taken.ToString();
            img.Source = list[current].Image;
        }
    }
}