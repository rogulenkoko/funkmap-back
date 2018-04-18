using System;
using System.Collections.Generic;
using System.Linq;
using Funkmap.Data.Entities.Entities;
using Funkmap.Data.Entities.Entities.Abstract;
using Funkmap.Domain.Enums;
using Funkmap.Domain.Models;

namespace Funkmap.Data.Mappers
{
    public static class SearchItemMapper
    {
        public static List<SearchItem> ToSearchItems(this IReadOnlyCollection<BaseEntity> source)
        {
            return source?.Select(x => x.ToSearchItem()).ToList();
        }

        public static SearchItem ToSearchItem(this BaseEntity source)
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

            return new SearchItem()
            {
                AvatarId = source.PhotoMiniId,
                Login = source.Login,
                UserLogin = source.UserLogin,
                Title = source.Name,
                Longitude = source.Location.Coordinates.Longitude,
                Latitude = source.Location.Coordinates.Latitude,
                Type = source.EntityType,
                Instrument = (source as MusicianEntity)?.Instrument ?? Instruments.None,
                Expirience = (source as MusicianEntity)?.ExpirienceType ?? Expiriences.None,

                Address = address,
                Website = (source as ShopEntity)?.Website,

                Styles = (source as BandEntity)?.Styles
            };
        }
    }
}
