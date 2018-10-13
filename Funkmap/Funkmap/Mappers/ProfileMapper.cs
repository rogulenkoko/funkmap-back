using System;
using System.Collections.Generic;
using System.Linq;
using Funkmap.Domain.Enums;
using Funkmap.Domain.Models;

namespace Funkmap.Mappers
{
    public static class ProfileMapper
    {
        public static List<SearchItem> ToSearchItems(this IReadOnlyCollection<Profile> source)
        {
            return source?.Select(x => x.ToSearchItem()).ToList();
        }

        public static SearchItem ToSearchItem(this Profile source)
        {
            if (source == null) return null;

            string address = String.Empty;

            var shop = source as Shop;
            if (shop != null)
            {
                address = shop.Address;
            }

            var studio = source as Studio;
            if (studio != null)
            {
                address = studio?.Address;
            }

            var point = source as RehearsalPoint;
            if (point != null)
            {
                address = point?.Address;
            }

            var musician = source as Musician;
            var band = source as Band;

            return new SearchItem()
            {
                AvatarMiniUrl = source.AvatarMiniUrl,
                AvatarUrl = source.AvatarMiniUrl,
                Login = source.Login,
                UserLogin = source.UserLogin,
                Title = source.Name,
                Longitude = source.Location.Longitude,
                Latitude = source.Location.Latitude,
                Type = source.EntityType,
                Instrument = musician?.Instrument ?? Instruments.None,
                Expirience = musician?.Expirience ?? Expiriences.None,

                Address = address,
                Website = shop?.Website,

                Styles = band?.Styles
            };
        }
    }
}
