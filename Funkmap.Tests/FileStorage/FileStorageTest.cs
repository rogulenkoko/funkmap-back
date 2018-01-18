using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Funkmap.Common.Abstract;
using Funkmap.Common.Azure;
using Funkmap.Tests.Images;
using Microsoft.Azure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Funkmap.Tests.FileStorage
{
    [TestClass]
    public class FileStorageTest
    {
        private IFileStorage _azureStorage;

        [TestInitialize]
        public void Initialize()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("azureStorage"));
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            _azureStorage = new AzureFileStorage(blobClient, "test");
        }

        [TestMethod]
        public void AzureStorageTest()
        {
            //https://funkmapstorage.blob.core.windows.net/test/test.png


            var file = ImageProvider.GetAvatar("beatles-avatar.jpg").ToArray();

            var fileName = $"{Guid.NewGuid()}.png";
            var resultLink = _azureStorage.UploadFromBytesAsync(fileName, file).GetAwaiter().GetResult();

            var downloaded = _azureStorage.DownloadAsBytesAsync(resultLink).GetAwaiter().GetResult();
            Assert.AreEqual(downloaded.Length, file.Length);
        }
    }
}
