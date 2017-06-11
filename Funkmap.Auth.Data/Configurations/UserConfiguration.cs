using System.Data.Entity.ModelConfiguration;
using Funkmap.Auth.Data.Entities;

namespace Funkmap.Auth.Data.Configurations
{
    public class UserConfiguration : EntityTypeConfiguration<UserEntity>
    {
        public UserConfiguration()
        {
            ToTable("User");

            HasKey(x => x.Login);
            Property(x => x.Login).IsRequired();

            Property(x => x.Password).HasColumnName("Password").IsRequired();
            Property(x => x.Email).HasColumnName("Email").IsRequired();

            Property(x => x.Avatar).HasColumnName("Avatar").IsOptional();
        }
    }
}
