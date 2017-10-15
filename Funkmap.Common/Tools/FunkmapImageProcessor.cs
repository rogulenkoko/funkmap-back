using System.Drawing;
using System.IO;
using ImageProcessor;
using ImageProcessor.Imaging;

namespace Funkmap.Common.Tools
{
    public static class FunkmapImageProcessor
    {
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
    }
}
