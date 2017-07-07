using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Funkmap.Tests.Images
{
    public static class ImageProvider
    {
        public static byte[] GetAvatar(string name)
        {
            using (var stream = new MemoryStream())
            {
                var path = Path.GetFullPath($"{AppDomain.CurrentDomain.BaseDirectory}..\\..\\..\\Images\\{name}");
                Image.FromFile(path).Save(stream, ImageFormat.Jpeg);
                return stream.ToArray();
            }
        }
    }
}
