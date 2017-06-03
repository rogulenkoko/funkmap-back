using System.Data.Entity;
using Funkmap.Musician.Data.Abstract;
using Funkmap.Musician.Data.Configurations;
using Funkmap.Musician.Data.Entities;

namespace Funkmap.Musician.Data
{
    public class MusicianContext : DbContext, IMusicianContext
    {

        public MusicianContext() : base("MusicianConnection") { }

        public MusicianContext(string nameOrConnectionString) : base(nameOrConnectionString) { }
        public DbSet<MusicianEntity> Musicians { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new MusicianConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
