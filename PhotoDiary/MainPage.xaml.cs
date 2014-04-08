using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using PhotoDiary.Resources;
using Microsoft.Phone.Tasks;
using System.Windows.Media.Imaging;
using System.Xml.Linq;

namespace PhotoDiary
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var camera = new PhotoChooserTask();
            // var camera = new CameraCaptureTask();
            camera.Completed += camera_Completed;
            camera.Show();
        }

        void camera_Completed(object sender, PhotoResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {
                PictureStore.AddPicture(e.ChosenPhoto);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Diary.xaml", UriKind.Relative));
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/GraviViewer.xaml", UriKind.Relative));
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            WebClient wc = new WebClient();
            wc.DownloadStringCompleted += (s, args) =>
                {
                    var xdoc = XDocument.Parse(args.Result);
                    XNamespace media = "http://search.yahoo.com/mrss";
                    var res = (from z in xdoc.Descendants("item")
                               select
                                (from x in z.Descendants(media + "thumbnail")
                                 select x.Attribute("url").Value).First()).First();
                    PictureStore.AddFromUrl(res);
                };
            wc.DownloadStringAsync(new Uri("http://fotki.yandex.ru/top/rss2"));
        }

        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}