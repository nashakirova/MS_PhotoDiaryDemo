using System;
using System.Collections.Generic;
using System.IO;
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
        public static List<ImageInfo> piclist = new List<ImageInfo>();
        public static void AddPicture(ImageInfo Inf)
        { 
            piclist.Add(Inf);
        }

        public List<ImageInfo> Images { get { return piclist; } }

    }
}
