using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Funkmap.Tests.Data
{
    public static class ImageProvider
    {
        public static byte[] GetAvatar()
        {
            using (var stream = new MemoryStream())
            {
                var path = Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory + "..\\..\\..\\Images\\avatar.jpg");
                Image.FromFile(path).Save(stream, ImageFormat.Jpeg);
                return stream.ToArray();
            }
        }
    }
}
