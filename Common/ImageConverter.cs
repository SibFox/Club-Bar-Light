using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace BarLight.Common
{
    public static class ImageConverter
    {
        public static BitmapImage ImageFromBuffer(Byte[] bytes)
        {
            try
            {
                MemoryStream ms = new MemoryStream(bytes);
                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                bi.StreamSource = ms;
                bi.EndInit();
                return bi;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return null;
        }

        //public static Byte[] BufferFromImage(BitmapImage source)
        //{
        //    Stream stream = source.StreamSource;
        //    Byte[] buffer = null;
        //    if (stream != null && stream.Length > 0)
        //    {
        //        using (BinaryReader br = new BinaryReader(stream))
        //        {
        //            buffer = br.ReadBytes((Int32)stream.Length);
        //        }
        //    }
        //    return buffer;
        //}
    }
}
