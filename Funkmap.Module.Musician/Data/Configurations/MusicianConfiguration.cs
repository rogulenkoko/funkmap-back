using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Funkmap.Module.Musician.Data.Configurations
{
    public class MusicianConfiguration : EntityTypeConfiguration<MusicianEntity>
    {
        public MusicianConfiguration()
        {
            ToTable("Musician");

            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.Login).HasColumnName("Login").IsRequired();
            Property(x => x.Name).HasColumnName("Name").IsRequired();

            Property(x => x.BirthDate).HasColumnName("BirthDate").IsOptional();
            Property(x => x.Description).HasColumnName("Description").IsOptional();
            Property(x => x.Sex).HasColumnName("Sex").IsOptional();
            Property(x => x.Expirience).HasColumnName("Expirience").IsOptional();
        }
    }
}
