using System;
using System.Collections.Generic;
using System.Linq;
using Funkmap.Data.Entities.Entities;
using Funkmap.Data.Entities.Entities.Abstract;
using Funkmap.Domain;
using Funkmap.Domain.Models;

namespace Funkmap.Data.Mappers
{
    public static class BaseModelMapper
    {
        public static List<Profile> ToSpecificProfiles(this IReadOnlyCollection<BaseEntity> source)
        {
            return source?.Select(x => x.ToSpecificProfile()).ToList();
        }

        public static Profile ToSpecificProfile(this BaseEntity source)
        {
            if (source == null) return null;
            switch (source.EntityType)
            {
                case EntityType.Musician: return (source as MusicianEntity).ToModel();
                case EntityType.Band: return (source as BandEntity).ToModel();
                case EntityType.RehearsalPoint: return (source as RehearsalPointEntity).ToModel();
                case EntityType.Studio: return (source as StudioEntity).ToModel();
                case EntityType.Shop: return (source as ShopEntity).ToModel();

                default: throw new ArgumentException("Неизвестный тип профиля");
            }
        }
    }
}
