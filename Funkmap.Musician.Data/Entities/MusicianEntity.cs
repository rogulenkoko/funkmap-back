using System;
using Funkmap.Common.Data.Abstract;

namespace Funkmap.Musician.Data.Entities
{
    public class MusicianEntity : Entity
    {
        public string Login { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public Sex Sex { get; set; }
        public DateTime BirthDate { get; set; }
        public int Expirience { get; set; }
        public Styles Styles { get; set; }
        public InstrumentType Instrument { get; set; }

        public byte[] AvatarImage { get; set; }

        public string VkLink { get; set; }
        public string YouTubeLink { get; set; }
        public string FacebookLink { get; set; }

        public long? BandId { get; set; }
        public virtual BandEntity Band { get; set; }


    }

    public enum Sex
    {
        Male = 1,
        Female = 2
    }
    
    [Flags]
    public enum Styles
    {
        HipHop = 0x01,
        Rock = 0x02,
        Funk = 0x04
    }
    
    [Flags]
    public enum InstrumentType
    {
        Bass = 1,
        Drums = 2,
        Vocal = 4,
        Brass = 8,
        Guitar = 16,
        Keyboard = 32
    }
}
