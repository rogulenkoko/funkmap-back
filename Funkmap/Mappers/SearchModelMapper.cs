using System;
using Funkmap.Data.Entities.Entities;
using Funkmap.Data.Entities.Entities.Abstract;
using Funkmap.Models;

namespace Funkmap.Mappers
{
    public static class SearchModelMapper
    {
        public static SearchModel ToSearchModel(this BaseEntity source)
        {
            if (source == null) return null;

            string address = String.Empty;

            if (source is ShopEntity)
            {
                address = (source as ShopEntity)?.Address;
            }

            if (source is StudioEntity)
            {
                address = (source as StudioEntity)?.Address;
            }

            if (source is RehearsalPointEntity)
            {
                address = (source as RehearsalPointEntity)?.Address;
            }

            return new SearchModel()
            {
                AvatarId = source.PhotoMiniId,
                Login = source.Login,
                UserLogin = source.UserLogin,
                Title = source.Name,
                Longitude = source.Location.Coordinates.Longitude,
                Latitude = source.Location.Coordinates.Latitude,
                Type = source.EntityType,
                Instrument = (source as MusicianEntity)?.Instrument ?? InstrumentType.None,
                Expirience = (source as MusicianEntity)?.ExpirienceType ?? ExpirienceType.None,

                Address = address,
                Website = (source as ShopEntity)?.Website,

                Styles = (source as BandEntity)?.Styles
            };
        }
    }
}
