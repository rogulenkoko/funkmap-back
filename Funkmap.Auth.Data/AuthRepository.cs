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

        public async Task<UserEntity> Login(string login, string password)
        {
            var isExist = await Context.Set<UserEntity>().SingleOrDefaultAsync(x => x.Login == login && x.Password == password);
            return isExist;
        }
    }
}
