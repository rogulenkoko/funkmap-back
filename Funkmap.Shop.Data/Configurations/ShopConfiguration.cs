using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Funkmap.Shop.Data.Entities;

namespace Funkmap.Shop.Data.Configurations
{
    public class ShopConfiguration : EntityTypeConfiguration<ShopEntity>
    {
        public ShopConfiguration()
        {
            ToTable("Shop");

            HasKey(x => x.Id);

            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.StoreName).HasColumnName("StoreName").IsRequired();
            Property(x => x.Longitude).HasColumnName("Longitude").IsRequired();
            Property(x => x.Latitude).HasColumnName("Latitude").IsRequired();

            Property(x => x.URLShop).HasColumnName("URLShop").IsOptional();
        }

    }
}
