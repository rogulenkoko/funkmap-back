using System;
using Funkmap.Domain;
using Funkmap.Domain.Models;
using Funkmap.Models;

namespace Funkmap.Mappers
{
    public static class ProfilePreviewMapper
    {

        public static ProfilePreview ToSpecificPreviewModel(this Profile source)
        {
            if (source == null) return null;
            switch (source.EntityType)
            {
                case EntityType.Musician: return (source as Musician).ToPreviewModel();
                case EntityType.Band: return (source as Band).ToPreviewModel();
                case EntityType.RehearsalPoint: return (source as RehearsalPoint).ToPreviewModel();
                case EntityType.Studio: return (source as Studio).ToPreviewModel();
                case EntityType.Shop: return (source as Shop).ToPreviewModel();

                default: throw new ArgumentException("Неизвестный тип профиля");
            }
        }
        public static BandPreviewModel ToPreviewModel(this Band source)
        {
            if (source == null) return null;
            return new BandPreviewModel()
            {
                Login = source.Login,
                Name = source.Name,
                AvatarMiniId = source.AvatarMiniId,
                AvatarId = source.AvatarId,
                VkLink = source.VkLink,
                YoutubeLink = source.YoutubeLink,
                FacebookLink = source.FacebookLink,
                DesiredInstruments = source.DesiredInstruments,
                Styles = source.Styles,
                SoundCloudLink = source.SoundCloudLink,
                UserLogin = source.UserLogin
            };
        }

        public static MusicianPreviewModel ToPreviewModel(this Musician source)
        {
            if (source == null) return null;
            return new MusicianPreviewModel()
            {
                Login = source.Login,
                Styles = source.Styles,
                Name = source.Name,
                AvatarMiniId = source.AvatarMiniId,
                AvatarId = source.AvatarId,
                Expirience = source.Expirience,
                VkLink = source.VkLink,
                YouTubeLink = source.YoutubeLink,
                FacebookLink = source.FacebookLink,
                SoundCloudLink = source.SoundCloudLink,
                UserLogin = source.UserLogin,
                Instrument = source.Instrument
            };
        }

        public static RehearsalPointPreviewModel ToPreviewModel(this RehearsalPoint source)
        {
            if (source == null) return null;
            return new RehearsalPointPreviewModel()
            {
                Login = source.Login,
                Name = source.Name,
                AvatarMiniId = source.AvatarMiniId,
                AvatarId = source.AvatarId,
                VkLink = source.VkLink,
                YoutubeLink = source.YoutubeLink,
                FacebookLink = source.FacebookLink,
                WorkingHoursDescription = source.WorkingHoursDescription,
                Address = source.Address,
                UserLogin = source.UserLogin
            };
        }

        public static ShopPreviewModel ToPreviewModel(this Shop source)
        {
            if (source == null) return null;
            return new ShopPreviewModel()
            {
                Login = source.Login,
                Name = source.Name,
                AvatarMiniId = source.AvatarMiniId,
                AvatarId = source.AvatarId,
                VkLink = source.VkLink,
                YoutubeLink = source.YoutubeLink,
                FacebookLink = source.FacebookLink,
                WorkingHoursDescription = source.WorkingHoursDescription,
                WebSite = source.Website,
                Address = source.Address,
                UserLogin = source.UserLogin
            };
        }

        public static StudioPreviewModel ToPreviewModel(this Studio source)
        {
            if (source == null) return null;
            return new StudioPreviewModel()
            {
                Login = source.Login,
                Name = source.Name,
                AvatarMiniId = source.AvatarMiniId,
                AvatarId = source.AvatarId,
                VkLink = source.VkLink,
                YoutubeLink = source.YoutubeLink,
                FacebookLink = source.FacebookLink,
                WorkingHoursDescription = source.WorkingHoursDescription,
                Address = source.Address,
                SoundCloudLink = source.SoundCloudLink,
                UserLogin = source.UserLogin
            };
        }
    }
}
