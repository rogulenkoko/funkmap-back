using Funkmap.Common.Data.Mongo.Entities;
using Funkmap.Messenger.Contracts;

namespace Funkmap.Messenger.Entities.Mappers
{
    public static class ContentItemMapper
    {
        public static ContentItem ToModel(this ContentItemEntity source)
        {
            if (source == null) return null;
            return new ContentItem()
            {
                FileName = source.FileName,
                ContentType = source.ContentType,
                Size = source.Size,
                FileId = source.FileId
            };
        }

        public static ImageInfo ToImageInfo(this ImageInfoEntity source)
        {
            if (source == null) return null;

            return new ImageInfo()
            {
                Image = source.Image?.AsByteArray
            };
        }

        public static ContentItemEntity ToEntity(this ContentItem source)
        {
            if (source == null) return null;
            return new ContentItemEntity()
            {
                ContentType = source.ContentType,
                FileName = source.FileName,
                FileId = source.FileId
            };
        }
    }
}
