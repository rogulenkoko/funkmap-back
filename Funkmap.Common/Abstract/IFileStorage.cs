
using System.Threading.Tasks;

namespace Funkmap.Common.Abstract
{
    public interface IFileStorage
    {
        Task<string> UploadFromBytesAsync(string fileName, byte[] bytes);

        Task<byte[]> DownloadAsBytesAsync(string fileName);
    }
}
