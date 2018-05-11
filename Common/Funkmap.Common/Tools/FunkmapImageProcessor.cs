using System.Drawing;
using System.IO;
using ImageProcessor;
using ImageProcessor.Imaging;
using ImageProcessor.Imaging.Formats;

namespace Funkmap.Common.Owin.Tools
{
    public static class FunkmapImageProcessor
    {
        /// <summary>
        /// Обрезание квадратных фото
        /// </summary>
        /// <param name="image"></param>
        /// <param name="imageSize"></param>
        /// <returns></returns>
        public static byte[] MinifyImage(byte[] image, int imageSize = 80)
        {
            if (image == null || image.Length == 0) return image;
            var size = new ResizeLayer(new Size(imageSize, imageSize));
            using (var memoryStream = new MemoryStream())
            {
                using (var imageFactory = new ImageFactory(true))
                {
                    imageFactory.Load(image)
                        .Resize(size)
                        .Save(memoryStream);
                }

                return memoryStream.ToArray();
            }
        }

        /// <summary>
        /// Обрезание и сжатие прямоугольных фото
        /// </summary>
        /// <param name="photoBytes"></param>
        /// <param name="maxWidth"></param>
        /// <returns></returns>
        public static byte[] MinifyImageWithMaxWidth(byte[] photoBytes, int maxWidth)
        {
            if (photoBytes == null || photoBytes.Length == 0) return photoBytes;
            
            var format = new PngFormat() { Quality = 100};

            using (MemoryStream inStream = new MemoryStream(photoBytes))
            {
                Bitmap startBitmap = new Bitmap(inStream);
                var c = (double)maxWidth / (double)startBitmap.Width;
                if (c >= 1) return photoBytes;

                using (var outStream = new MemoryStream())
                {
                    using (ImageFactory imageFactory = new ImageFactory(true))
                    {
                        var factory = imageFactory.Load(photoBytes);
                        var newSize = new Size((int)(factory.Image.Width * c), (int)(factory.Image.Height * c));
                        factory.Resize(newSize);
                        factory.Format(format).Save(outStream);
                    }

                    return outStream.ToArray();
                }
            }
        }
    }
}
