using Funkmap.Common.Data.Mongo.Entities;
using Funkmap.Messenger.Data.Entities;
using Funkmap.Messenger.Models;

namespace Funkmap.Messenger.Mappers
{
    public static class DialogAvatarInfoMapper
    {
        public static DialogAvatarInfo ToAvatarInfo(this DialogEntity source)
        {
            if (source == null) return null;
            return new DialogAvatarInfo()
            {
                Bytes = source.Avatar?.Image?.AsByteArray,
                Id = source.Id.ToString()
            };
        }
    }
}
