using System.Threading.Tasks;
using Funkmap.Common.Abstract;
using MongoDB.Bson;
using MongoDB.Driver.GridFS;

namespace Funkmap.Common.Data.Mongo
{
    public class GridFsFileStorage : IFileStorage
    {
        private readonly IGridFSBucket _bucket;

        public GridFsFileStorage(IGridFSBucket bucket)
        {
            _bucket = bucket;
        }

        public async Task<string> UploadFromBytesAsync(string fileName, byte[] bytes)
        {
            var objectId = await _bucket.UploadFromBytesAsync(fileName, bytes);
            return objectId.ToString();
        }

        public async Task<byte[]> DownloadAsBytesAsync(string fileName)
        {
            return await _bucket.DownloadAsBytesAsync(new ObjectId(fileName));
        }


    }
}
