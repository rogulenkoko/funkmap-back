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
    public class MusicianUpdateDefinitionBuilder : IUpdateDefinitionBuilder
    {
        public EntityType EntityType => EntityType.Musician;
        public UpdateDefinition<BaseEntity> Build(Profile profile)
        {
            var musician = profile as Musician;

            if (musician == null)
            {
                throw new ArgumentException($"{nameof(MusicianUpdateDefinitionBuilder)} can process only Musician profiles.");
            }

            var update = Builders<BaseEntity>.Update;

            var updateDefinitions = new List<UpdateDefinition<BaseEntity>>();

            if (musician.BandLogins != null)
            {
                musician.BandLogins = musician.BandLogins.Distinct().ToList();

                updateDefinitions.Add(update.Set(x => (x as MusicianEntity).BandLogins, musician.BandLogins));
            }

            if (musician.Instrument != Instruments.None)
            {
                updateDefinitions.Add(update.Set(x => (x as MusicianEntity).Instrument, musician.Instrument));
            }

            if (musician.Styles != null)
            {
                musician.Styles = musician.Styles.Distinct().ToList();

                updateDefinitions.Add(update.Set(x => (x as MusicianEntity).Styles, musician.Styles));
            }

            if (musician.BirthDate.HasValue)
            {
                updateDefinitions.Add(update.Set(x=> (x as MusicianEntity).BirthDate, musician.BirthDate.Value));
            }

            if (musician.Experience != Experiences.None)
            {
                updateDefinitions.Add(update.Set(x => (x as MusicianEntity).ExperienceType, musician.Experience));
            }

            if (musician.Sex.HasValue)
            {
                updateDefinitions.Add(update.Set(x => (x as MusicianEntity).Sex, musician.Sex.Value));
            }

            return updateDefinitions.Count == 0 ? null : Builders<BaseEntity>.Update.Combine(updateDefinitions);
        }
    }
}
