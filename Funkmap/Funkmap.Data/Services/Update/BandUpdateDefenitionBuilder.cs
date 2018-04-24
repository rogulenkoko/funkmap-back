using System;
using System.Collections.Generic;
using System.Linq;
using Funkmap.Data.Entities.Entities;
using Funkmap.Data.Entities.Entities.Abstract;
using Funkmap.Data.Services.Abstract;
using Funkmap.Domain;
using Funkmap.Domain.Models;
using MongoDB.Driver;

namespace Funkmap.Data.Services.Update
{
    public class BandUpdateDefenitionBuilder : IUpdateDefenitionBuilder
    {
        public EntityType EntityType => EntityType.Band;
        public UpdateDefinition<BaseEntity> Build(Profile profile)
        {
            var band = profile as Band;

            if (band == null)
            {
                throw new ArgumentException("BandUpdateDefenitionBuilder can process only Band profiles.");
            }

            var update = Builders<BaseEntity>.Update;

            var updateDefenitions = new List<UpdateDefinition<BaseEntity>>();
            
            if (band.Styles != null)
            {
                band.Styles = band.Styles.Distinct().ToList();

                updateDefenitions.Add(update.Set(x => (x as BandEntity).Styles, band.Styles));
            }

            if (band.DesiredInstruments != null)
            {
                band.DesiredInstruments = band.DesiredInstruments.Distinct().ToList();

                updateDefenitions.Add(update.Set(x => (x as BandEntity).DesiredInstruments, band.DesiredInstruments));
            }

            if (band.InvitedMusicians != null)
            {
                band.InvitedMusicians = band.InvitedMusicians.Distinct().ToList();

                updateDefenitions.Add(update.Set(x => (x as BandEntity).InvitedMusicians, band.InvitedMusicians));
            }

            if (band.Musicians != null)
            {
                band.Musicians = band.Musicians.Distinct().ToList();

                updateDefenitions.Add(update.Set(x => (x as BandEntity).MusicianLogins, band.Musicians));
            }

            return updateDefenitions.Count == 0 ? null : Builders<BaseEntity>.Update.Combine(updateDefenitions);
        }
    }
}
