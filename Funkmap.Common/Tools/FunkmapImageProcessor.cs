using System.Drawing;
using System.IO;
using ImageProcessor;
using ImageProcessor.Imaging;

namespace Funkmap.Common.Tools
{
    public static class FunkmapImageProcessor
    {
        public static byte[] MinifyImage(byte[] image)
        {
            var size = new ResizeLayer(new Size(80, 80));
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
