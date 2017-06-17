using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
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
            var user = await Context.Set<UserEntity>().SingleOrDefaultAsync(x => x.Login == login && x.Password == password);
            return user;
        }

        public async Task<bool> CheckIfExist(string login)
        {
            var isExist = await Context.Set<UserEntity>().AnyAsync(x => x.Login == login);
            return isExist;
        }

        public async Task<byte[]> GetAvatarAsync(string login)
        {
            var user = await Context.Set<UserEntity>().FirstOrDefaultAsync(x => x.Login == login);
            return user?.Avatar;
        }

        public async Task<bool> SaveAvatarAsync(string login, byte[] image)
        {
            var user = await Context.Set<UserEntity>().FirstOrDefaultAsync(x => x.Login == login);
            if (user == null)
            {
                return false;
            }
            user.Avatar = image;
            try
            {
                Context.Set<UserEntity>().AddOrUpdate(user);
                await Context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
