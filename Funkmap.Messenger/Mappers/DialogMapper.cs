using Funkmap.Common.Data.Mongo.Entities;
using Funkmap.Messenger.Data.Entities;
using Funkmap.Messenger.Data.Objects;
using Funkmap.Messenger.Models;

namespace Funkmap.Messenger.Mappers
{
    public static class DialogMapper
    {
        public static Dialog ToModel(this DialogEntity source, string userLogin)
        {
            if (source == null) return null;
            return new Dialog()
            {
                DialogId = source.Id.ToString(),
                Name = source.Name,
                LastMessage = source.LastMessage.ToModel(),
                Participants = source.Participants,
                CreatorLogin = source.CreatorLogin
            };
        }

        public static DialogsNewMessagesCountModel ToModel(this DialogsNewMessagesCountResult source)
        {
            if (source == null) return null;
            return new DialogsNewMessagesCountModel()
            {
                DialogId = source.DialogId.ToString(),
                NewMessagesCount = source.NewMessagesCount
            };
        }

        public static DialogEntity ToEntity(this Dialog source)
        {
            if (source == null) return null;
            return new DialogEntity()
            {
                Name = source.Name,
                Participants = source.Participants,
                Avatar = source.Avatar == null ? null : new ImageInfo() { Image = source.Avatar }
            };
        }
    }
}
