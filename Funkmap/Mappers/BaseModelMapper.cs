using System;
using Funkmap.Data.Entities;
using Funkmap.Data.Entities.Entities;
using Funkmap.Data.Entities.Entities.Abstract;
using Funkmap.Models;

namespace Funkmap.Mappers
{
    public static class BaseModelMapper
    {
        public static BaseModel ToSpecificModel(this BaseEntity source)
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

        public static BaseModel ToSpecificPreviewModel(this BaseEntity source)
        {
            if (source == null) return null;
            switch (source.EntityType)
            {
                case EntityType.Musician: return (source as MusicianEntity).ToPreviewModel();
                case EntityType.Band: return (source as BandEntity).ToPreviewModel();
                case EntityType.RehearsalPoint: return (source as RehearsalPointEntity).ToPreviewModel();
                case EntityType.Studio: return (source as StudioEntity).ToPreviewModel();
                case EntityType.Shop: return (source as ShopEntity).ToPreviewModel();

                default: throw new ArgumentException("Неизвестный тип профиля");
            }
        }
    }
}
