using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Funkmap.Module.Musician.Abstract;
using Funkmap.Module.Musician.Data.Configurations;

namespace Funkmap.Module.Musician.Data
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
