using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;

namespace Vimpel_Accounting.AppFolder.ClassFolder
{
    class LoadReadImageClass
    {

        public static BitmapImage GetImageFromBytes(byte[] array)
        {
            if (array != null)
            {

                using (MemoryStream ms = new MemoryStream(array, 0, array.Length))
                {

                    var image = new BitmapImage();

                    image.BeginInit();


                    image.CacheOption = BitmapCacheOption.OnLoad;

                    image.StreamSource = ms;

                    image.EndInit();

                    return image;
                }
            }
            return null;
        }


        public static byte[] SetImageToBytes(string fileName)
        {

            Bitmap bitmap = new Bitmap(fileName);
            ImageFormat imageFormat = bitmap.RawFormat;

            var imageToConvert = Image.FromFile(fileName);

            using (MemoryStream ms = new MemoryStream())
            {
                imageToConvert.Save(ms, imageFormat);


                return ms.ToArray();
            }

        }
    }
}
