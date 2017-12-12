using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Funkmap.Common.Abstract;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Funkmap.Common.Azure
{
    public class AzureFileStorage : IFileStorage
    {
        private readonly CloudBlobContainer _container;

        public AzureFileStorage(CloudBlobClient blobClient, string containerName)
        {
            _container = blobClient.GetContainerReference(containerName);

            if(!_container.Exists()) _container.Create(BlobContainerPublicAccessType.Container);
        }

        public async Task<string> UploadFromBytesAsync(string fileName, byte[] bytes)
        {
            var blob = _container.GetBlockBlobReference(fileName);
            await blob.UploadFromByteArrayAsync(bytes,0, bytes.Length);
            return $"{_container.Uri}/{fileName}";
        }

        public async Task<byte[]> DownloadAsBytesAsync(string fileName)
        {
            CloudBlockBlob blob = _container.ListBlobs().Select(x => x as CloudBlockBlob).FirstOrDefault(x => x.Name == fileName);
            if (blob == null) return null;


            byte[] result;

            using (var memoryStream = new MemoryStream())
            {
                blob.DownloadToStream(memoryStream);
                result = memoryStream.ToArray();
            }
            return result;
        }
    }
}
