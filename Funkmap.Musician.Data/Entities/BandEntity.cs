using System;
using System.Collections.Generic;
using Funkmap.Common.Data.Abstract;
using Funkmap.Common.Data.Tools;

namespace Funkmap.Musician.Data.Entities
{
    public class BandEntity : Entity
    {
        public BandEntity()
        {
            VideoLinks = new PersistableStringCollection();
        }
        public string Name { get; set; }
        public string Login { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double ShowPrice { get; set; }
        public InstrumentType DesiredInstruments { get; set; }
       
        public PersistableStringCollection VideoLinks { get; set; }
        public virtual ICollection<MusicianEntity> Musicians { get; set; }
    }
}
