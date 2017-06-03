using System.Data.Entity;
using Funkmap.Auth.Data.Abstract;
using Funkmap.Auth.Data.Configurations;
using Funkmap.Auth.Data.Entities;

namespace Funkmap.Auth.Data
{
    public class AuthContext : DbContext, IAuthContext
    {
        public AuthContext() : base("FunkmapConnection") { }
        public AuthContext(string nameOrConnectionString) : base(nameOrConnectionString) { }
        public DbSet<UserEntity> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new UserConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
