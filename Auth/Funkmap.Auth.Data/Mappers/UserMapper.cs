using Funkmap.Auth.Contracts;
using Funkmap.Auth.Data.Entities;

namespace Funkmap.Auth.Data.Mappers
{
    public static class UserMapper
    {
        public static User ToUser(this UserEntity source)
        {
            if (source == null) return null;
            return new User()
            {
                Login = source.Login,
                Name = source.Name,
                AvatarUrl = source.AvatarUrl,
                Email = source.Email,
                Locale = source.Locale,
                LastVisitDateUtc = source.LastVisitDateUtc
            };
        }

        public static UserEntity ToEntity(this User source, string password)
        {
            if (source == null) return null;
            return new UserEntity()
            {
                Login = source.Login,
                Name = source.Name,
                Email = source.Email,
                Locale = source.Locale,
                Password = password,
                LastVisitDateUtc = source.LastVisitDateUtc
            };
        }
    }
}
