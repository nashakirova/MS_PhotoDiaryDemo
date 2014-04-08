using System;
using System.Collections.Generic;
using System.Device.Location;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Resources;

namespace PhotoDiary
{

    public struct ImageInfo
    {
        public BitmapImage Image { get; set; }
        public DateTime Taken { get; set; }
    }

    public class PictureStore
    {


        public PictureStore()
        {
            if (locator==null)
            {
                locator = new GeoCoordinateWatcher();
                locator.PositionChanged += (s, args) =>
                    {
                        coord = args.Position.Location;
                    };
                locator.Start();
            }
        }

        public static int count = 0;

        public static GeoCoordinateWatcher locator = null;
        public static GeoCoordinate coord = null;

        public static void AddPicture(Stream img)
        {
            var iso = IsolatedStorageFile.GetUserStoreForApplication();
            if (!iso.DirectoryExists("Pictures")) iso.CreateDirectory("Pictures");
            var fn = DateTime.Now.ToString("s").Replace(':','_');
            using (var f = new IsolatedStorageFileStream(@"Pictures\"+fn,FileMode.Create,iso))
            {
                img.CopyTo(f);
                f.Close();
            }
            if (locator!=null && coord !=null)
            {
                if (!iso.DirectoryExists("Locations")) iso.CreateDirectory("Locations");
                using (var f = new IsolatedStorageFileStream(@"Locations\" + fn, FileMode.Create, iso))
                {
                    using (var tw = new StreamWriter(f, UTF8Encoding.UTF8))
                    {
                        tw.WriteLine(coord.Latitude);
                        tw.WriteLine(coord.Longitude);
                    }
                    f.Close();
                }
            }
            count++;
        }

        public static void AddFromUrl(string url)
        {
            var wc = new WebClient();
            wc.OpenReadCompleted += (s, args) =>
                {
                    var resInfo = new StreamResourceInfo(args.Result, null);
                    AddPicture(resInfo.Stream);
                };
            wc.OpenReadAsync(new Uri(url));
        }

        public List<ImageInfo> Images 
        { 
            get {
                var l = new List<ImageInfo>();
                var iso = IsolatedStorageFile.GetUserStoreForApplication();
                if (!iso.DirectoryExists("Pictures")) return l;
                foreach (var fn in iso.GetFileNames(@"Pictures\*"))
                {
                    var bmp = new BitmapImage();
                    using (var f = iso.OpenFile(@"Pictures\"+fn,FileMode.Open, FileAccess.Read))
                    {
                        bmp.SetSource(f);
                        var dt = DateTime.Parse(fn.Replace('_', ':'));
                        l.Add(new ImageInfo() { Image = bmp, Taken = dt });
                    }
                }
                return l;
            } 
        }

        public static List<GeoCoordinate> Coordinates
        {
            get
            {
                var l = new List<GeoCoordinate>();
                var iso = IsolatedStorageFile.GetUserStoreForApplication();
                if (!iso.DirectoryExists("Locations")) return l;
                foreach (var fn in iso.GetFileNames(@"Locations\*"))
                {
                    var bmp = new BitmapImage();
                    using (var f = iso.OpenFile(@"Locations\" + fn, FileMode.Open, FileAccess.Read))
                    {
                        using (var tr = new StreamReader(f))
                        {
                            var loc = new GeoCoordinate(double.Parse(tr.ReadLine()), double.Parse(tr.ReadLine()));
                            l.Add(loc);
                        }
                    }
                }
                return l;
            }
        }


    }
}
