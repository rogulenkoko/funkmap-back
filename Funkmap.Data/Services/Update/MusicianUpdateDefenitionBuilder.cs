using System;
using System.Collections.Generic;
using System.Linq;
using Funkmap.Data.Entities.Entities;
using Funkmap.Data.Entities.Entities.Abstract;
using Funkmap.Data.Services.Abstract;
using Funkmap.Domain;
using Funkmap.Domain.Enums;
using Funkmap.Domain.Models;
using MongoDB.Driver;

namespace Funkmap.Data.Services.Update
{
    public class MusicianUpdateDefenitionBuilder : IUpdateDefenitionBuilder
    {
        public EntityType EntityType => EntityType.Musician;
        public UpdateDefinition<BaseEntity> Build(Profile profile)
        {
            var musician = profile as Musician;

            if (musician == null)
            {
                throw new ArgumentException("MusicianUpdateDefenitionBuilder can process only Musician profiles.");
            }

            var update = Builders<BaseEntity>.Update;

            var updateDefenitions = new List<UpdateDefinition<BaseEntity>>();

            if (musician.BandLogins != null)
            {
                musician.BandLogins = musician.BandLogins.Distinct().ToList();

                updateDefenitions.Add(update.Set(x => (x as MusicianEntity).BandLogins, musician.BandLogins));
            }

            if (musician.Instrument != Instruments.None)
            {
                updateDefenitions.Add(update.Set(x => (x as MusicianEntity).Instrument, musician.Instrument));
            }

            if (musician.Styles != null)
            {
                musician.Styles = musician.Styles.Distinct().ToList();

                updateDefenitions.Add(update.Set(x => (x as MusicianEntity).Styles, musician.Styles));
            }

            if (musician.BirthDate.HasValue)
            {
                updateDefenitions.Add(update.Set(x=> (x as MusicianEntity).BirthDate, musician.BirthDate.Value));
            }

            if (musician.Expirience != Expiriences.None)
            {
                updateDefenitions.Add(update.Set(x => (x as MusicianEntity).ExpirienceType, musician.Expirience));
            }

            if (musician.Sex.HasValue)
            {
                updateDefenitions.Add(update.Set(x => (x as MusicianEntity).Sex, musician.Sex.Value));
            }

            return updateDefenitions.Count == 0 ? null : Builders<BaseEntity>.Update.Combine(updateDefenitions);
        }
    }
}
