using System;
using System.Collections.Generic;
using Funkmap.Data.Entities.Entities;
using Funkmap.Data.Entities.Entities.Abstract;
using Funkmap.Data.Services.Abstract;
using Funkmap.Domain;
using Funkmap.Domain.Models;
using MongoDB.Driver;

namespace Funkmap.Data.Services.Update
{
    public class ShopUpdateDefinitionBuilder : IUpdateDefinitionBuilder
    {
        public EntityType EntityType => EntityType.Shop;
        public UpdateDefinition<BaseEntity> Build(Profile profile)
        {
            var band = profile as Shop;

            if (band == null)
            {
                throw new ArgumentException($"{nameof(ShopUpdateDefinitionBuilder)} can process only Band profiles.");
            }

            var update = Builders<BaseEntity>.Update;

            var updateDefinitions = new List<UpdateDefinition<BaseEntity>>();

            if (band.Website != null)
            {
                updateDefinitions.Add(update.Set(x => (x as ShopEntity).Website, band.Website));
            }

            return updateDefinitions.Count == 0 ? null : Builders<BaseEntity>.Update.Combine(updateDefinitions);
        }
    }
}
