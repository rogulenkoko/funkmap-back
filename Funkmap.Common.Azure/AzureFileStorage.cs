using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Funkmap.Common.Abstract;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Funkmap.Common.Azure
{
    public class AzureFileStorage : IFileStorage
    {
        private readonly CloudBlobContainer _container;

        private readonly string _extension = "png";

        public AzureFileStorage(CloudBlobClient blobClient, string containerName)
        {
            _container = blobClient.GetContainerReference(containerName);
            _container.CreateIfNotExists(BlobContainerPublicAccessType.Container);

        }

        public async Task<string> UploadFromBytesAsync(string fileName, byte[] bytes)
        {
            var blob = _container.GetBlockBlobReference($"{fileName}.{_extension}");
            await blob.UploadFromByteArrayAsync(bytes, 0, bytes.Length);
            return $"{_container.Uri}/{fileName}.{_extension}";
        }

        public async Task<byte[]> DownloadAsBytesAsync(string fullFilePath)
        {
            if (String.IsNullOrEmpty(fullFilePath)) throw new ArgumentException("Пустой путь файла");

            var name = fullFilePath.Replace($"{_container.Uri}/", "");
            CloudBlockBlob blob = _container.ListBlobs().Select(x => x as CloudBlockBlob).FirstOrDefault(x => x.Name == name);
            if (blob == null) return null;

            byte[] result;

            using (var memoryStream = new MemoryStream())
            {
                await blob.DownloadToStreamAsync(memoryStream);
                result = memoryStream.ToArray();
            }
            return result;
        }

        public async Task DeleteAsync(string fullFilePath)
        {
            var name = fullFilePath.Replace(_container.Uri.ToString(), "");
            CloudBlockBlob blob = _container.GetBlockBlobReference(name);
            await blob.DeleteAsync();
        }
    }
}
