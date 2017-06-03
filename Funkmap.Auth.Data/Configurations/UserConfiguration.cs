using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            Property(x => x.Email).HasColumnName("Email").IsOptional();
        }
    }
}
