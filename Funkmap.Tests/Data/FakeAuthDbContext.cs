using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Funkmap.Auth.Data;
using Funkmap.Auth.Data.Entities;

namespace Funkmap.Tests.Data
{
    public class FakeAuthDbContext : AuthContext
    {
        public FakeAuthDbContext() : base("TestConnection")
        {
            Database.SetInitializer(new AuthTestDbContextInitializer());
        }

        public class AuthTestDbContextInitializer : CreateDatabaseIfNotExists<FakeAuthDbContext>
        {
            protected override void Seed(FakeAuthDbContext context)
            {
                var repository = new AuthRepository(context);
                var u1 = new UserEntity()
                {
                    Login = "rogulenkoko",
                    Password = "1"
                };

                var u2 = new UserEntity()
                {
                    Login = "test",
                    Password = "1"
                };

                repository.Add(u1);
                repository.Add(u2);
                repository.SaveAsync().Wait();
            }
        }
    }
}
