
using System.Threading.Tasks;

namespace Funkmap.Common.Abstract
{
    public interface IFileStorage
    {
        Task<string> UploadFromBytesAsync(string fileName, byte[] bytes);

        /// <summary>
        /// Скачивает файл по указанному пути
        /// </summary>
        /// <param name="fullFilePath">ВАЖНО! Полный путь до файла</param>
        /// <returns></returns>
        Task<byte[]> DownloadAsBytesAsync(string fileName);


        /// <summary>
        /// Удаляет файл по указанному пути
        /// </summary>
        /// <param name="fullFilePath">ВАЖНО! Полный путь до файла</param>
        /// <returns></returns>
        Task DeleteAsync(string fullFilePath);
    }
}
