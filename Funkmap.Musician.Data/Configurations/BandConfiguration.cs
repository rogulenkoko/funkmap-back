using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Funkmap.Musician.Data.Entities;

namespace Funkmap.Musician.Data.Configurations
{
    public class BandConfiguration : EntityTypeConfiguration<BandEntity>
    {
        public BandConfiguration()
        {
            ToTable("Band");

            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.Login).HasColumnName("Login").IsRequired();
            Property(x => x.Name).HasColumnName("Name").IsRequired();
            Property(x => x.Longitude).HasColumnName("Longitude").IsRequired();
            Property(x => x.Latitude).HasColumnName("Latitude").IsRequired();

            HasMany(x => x.Musicians).WithOptional(x => x.Band).HasForeignKey(x => x.BandId);

            Property(x => x.ShowPrice).HasColumnName("ShowPrice").IsOptional();
            Property(x => x.DesiredInstruments).HasColumnName("DesiredInstruments").IsOptional();
            Property(x => x.VideoLinks.SerializedValue).HasColumnName("VideoLinks").IsOptional();
        }
    }
}
