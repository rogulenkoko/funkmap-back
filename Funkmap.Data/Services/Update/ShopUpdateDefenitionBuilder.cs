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
    public class ShopUpdateDefenitionBuilder : IUpdateDefenitionBuilder
    {
        public EntityType EntityType => EntityType.Shop;
        public UpdateDefinition<BaseEntity> Build(Profile profile)
        {
            var band = profile as Shop;

            if (band == null)
            {
                throw new ArgumentException("BandUpdateDefenitionBuilder can process only Band profiles.");
            }

            var update = Builders<BaseEntity>.Update;

            var updateDefenitions = new List<UpdateDefinition<BaseEntity>>();

            if (band.Website != null)
            {
                updateDefenitions.Add(update.Set(x => (x as ShopEntity).Website, band.Website));
            }

            return updateDefenitions.Count == 0 ? null : Builders<BaseEntity>.Update.Combine(updateDefenitions);
        }
    }
}
