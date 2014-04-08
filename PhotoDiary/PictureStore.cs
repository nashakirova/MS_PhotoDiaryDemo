using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace PhotoDiary
{

    public struct ImageInfo
    {
        public BitmapImage Image { get; set; }
        public DateTime Taken { get; set; }
    }

    public class PictureStore
    {

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

    }
}
