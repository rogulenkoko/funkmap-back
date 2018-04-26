using Funkmap.Messenger.Contracts;

namespace Funkmap.Messenger.Entities.Mappers
{
    public static class DialogMapper
    {
        public static Dialog ToDialog(this DialogEntity source)
        {
            if (source == null) return null;
            return new Dialog()
            {
                Name = source.Name,
                Id = source.Id.ToString(),
                LastMessage = source.LastMessage.ToModel(),
                Participants = source.Participants,
                AvatarId = source.AvatarId,
                DialogType = source.DialogType,
                CreatorLogin = source.CreatorLogin,
                Avatar = source.Avatar.ToImageInfo(),
                LastMessageDate = source.LastMessageDate
            };
        }
    }
}
