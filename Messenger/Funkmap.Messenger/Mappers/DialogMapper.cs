using System.Linq;
using Funkmap.Common.Data.Mongo.Entities;
using Funkmap.Messenger.Entities;
using Funkmap.Messenger.Models;
using Funkmap.Messenger.Query.Responses;

namespace Funkmap.Messenger.Mappers
{
    public static class DialogMapper
    {
        public static DialogModel ToModel(this DialogWithLastMessage source, string userLogin)
        {
            if (source == null) return null;

            if (source.DialogType == DialogType.Base)
            {
                source.Name = source.Participants.Single(x => x != userLogin);
            }

            return new DialogModel()
            {
                DialogId = source.DialogId,
                Name = source.Name,
                LastMessage = source.LastMessage?.ToModel(),
                Participants = source.Participants,
                CreatorLogin = source.CreatorLogin,
                NewMessagesCount = source.NewMessagesCount,
                DialogType = source.DialogType,
                AvatarId = source.AvatarId
            };
        }

        public static DialogsNewMessagesCountModel ToModel(this DialogsNewMessagesCountResult source)
        {
            if (source == null) return null;
            return new DialogsNewMessagesCountModel()
            {
                DialogId = source.DialogId,
                NewMessagesCount = source.NewMessagesCount
            };
        }
    }
}
