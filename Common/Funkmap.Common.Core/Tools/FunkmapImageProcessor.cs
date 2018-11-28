using System.IO;
using ImageProcessorCore;

namespace Funkmap.Common.Core.Tools
{
    /// <summary>
    /// Tool for images
    /// </summary>
    public static class FunkmapImageProcessor
    {
        /// <summary>
        /// Cut square photos
        /// </summary>
        /// <param name="image">Image bytes</param>
        /// <param name="imageSize">Image size</param>
        public static byte[] MinifyImage(byte[] image, int imageSize = 80)
        {
            if (image == null || image.Length == 0) return image;
            var size = new ResizeOptions
            {
                Size = new Size(imageSize, imageSize),
            };
            using (var outputStream = new MemoryStream())
            using (var memoryStream = new MemoryStream(image))
            {
                new Image(memoryStream)
                    .Resize(size)
                    .SaveAsPng(outputStream);

                return outputStream.ToArray();
            }
        }

        /// <summary>
        /// Cute and compress square photos
        /// </summary>
        /// <param name="photoBytes">Photo bytes</param>
        /// <param name="maxWidth">Max width</param>
        public static byte[] MinifyImageWithMaxWidth(byte[] photoBytes, int maxWidth)
        {
            if (photoBytes == null || photoBytes.Length == 0) return photoBytes;

            using (var inStream = new MemoryStream(photoBytes))
            {
                var startBitmap = new Image(inStream);
                var c = (double) maxWidth / (double) startBitmap.Width;
                if (c >= 1) return photoBytes;

                using (var outStream = new MemoryStream())
                using (var stream = new MemoryStream(photoBytes))
                {
                    var factory = new Image(stream);
                    var newSize = new Size((int) (factory.Width * c), (int) (factory.Height * c));
                    factory.Resize(new ResizeOptions() {Size = newSize});
                    factory.SaveAsPng(outStream);
                    return outStream.ToArray();
                }
            }
        }
    }
}