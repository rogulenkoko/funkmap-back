using System.Data.Entity;
using Funkmap.Musician.Data.Abstract;
using Funkmap.Musician.Data.Configurations;
using Funkmap.Musician.Data.Entities;

namespace Funkmap.Musician.Data
{
    public class MusicianContext : DbContext, IMusicianContext
    {

        public MusicianContext() : base("FunkmapConnection") { }

        public MusicianContext(string nameOrConnectionString) : base(nameOrConnectionString) { }
        public DbSet<MusicianEntity> Musicians { get; set; }
        public DbSet<BandEntity> Bands { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new MusicianConfiguration());
            modelBuilder.Configurations.Add(new BandConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
