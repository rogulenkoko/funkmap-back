using Funkmap.Data.Entities.Entities;
using Funkmap.Domain.Models;

namespace Funkmap.Data.Mappers
{
    public static class ProAccountMapper
    {
        public static ProAccountEntity ToEntity(this ProAccount source)
        {
            if (source == null) return null;

            return new ProAccountEntity
            {
                ExpireAt = source.ExpireAt,
                UserLogin = source.UserLogin
            };
        }

        public static ProAccount ToModel(this ProAccountEntity source)
        {
            if (source == null) return null;

            return new ProAccount
            {
                ExpireAt = source.ExpireAt,
                UserLogin = source.UserLogin
            };
        }
    }
}
