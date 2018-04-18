
namespace Funkmap.Common.Models
{
    public class ImageProcessorOptions
    {
        public ImageProcessorOptions()
        {
            Size = 200;
            MiniSize = 80;
        }
        public int Size { get; set; }

        public int MiniSize { get; set; }
    }
}
