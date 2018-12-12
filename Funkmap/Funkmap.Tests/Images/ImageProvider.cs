using System;
using System.Collections.Concurrent;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Funkmap.Tests.Images
{
    public static class ImageProvider
    {
        private static ConcurrentDictionary<string, byte[]> _images = new ConcurrentDictionary<string, byte[]>();

        public static byte[] GetAvatar(string name)
        {
            if (_images.ContainsKey(name))
            {
                return _images[name];
            }
            using (var stream = new MemoryStream())
            {
                var path = Path.GetFullPath($"{AppDomain.CurrentDomain.BaseDirectory}..\\..\\..\\Images\\{name}");
                Image.FromFile(path).Save(stream, ImageFormat.Jpeg);
                var result = stream.ToArray();
                _images.TryAdd(name, result);
                return result;
            }
           
        }
    }
}
