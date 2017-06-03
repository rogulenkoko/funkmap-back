using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Funkmap.Auth.Data.Abstract;
using Funkmap.Auth.Data.Entities;
using Funkmap.Common.Data;

namespace Funkmap.Auth.Data
{
    public class AuthRepository : Repository<UserEntity>, IAuthRepository
    {
        public AuthRepository(AuthContext context) : base(context)
        {
        }

        public bool Login(string login, string password)
        {
            var isExist = Context.Set<UserEntity>().Any(x => x.Login == login && x.Password == password);
            return isExist;
        }
    }
}
