using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
