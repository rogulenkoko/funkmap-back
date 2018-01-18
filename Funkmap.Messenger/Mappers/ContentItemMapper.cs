using Funkmap.Messenger.Entities;
using Funkmap.Messenger.Models;
using Funkmap.Messenger.Query.Responses;

namespace Funkmap.Messenger.Mappers
{
    public static class ContentItemMapper
    {
        public static ContentItemEntity ToEntity(this ContentItemModel source)
        {
            if (source == null) return null;
            return new ContentItemEntity()
            {
                ContentType = source.ContentType,
                
                FileName = source.Name,
                Size = source.Size,
                FileId = source.DataUrl
            };
        }

        public static ContentItemModel ToModel(this ContentItemEntity source)
        {
            if (source == null) return null;
            return new ContentItemModel()
            {
                Name = source.FileName,
                ContentType = source.ContentType,
                Size = source.Size,
                DataUrl = source.FileId
            };
        }

        public static ContentItemModel ToModel(this ContentItem source)
        {
            if (source == null) return null;
            return new ContentItemModel()
            {
                Name = source.FileName,
                ContentType = source.ContentType,
                Size = source.Size,
                Data = source.FileBytes,
                DataUrl = source.FileId
            };
        }
    }
}
