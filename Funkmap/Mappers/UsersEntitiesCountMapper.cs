using System;
using System.Collections.Generic;
using System.Linq;
using Funkmap.Common;
using Funkmap.Data.Objects;
using Funkmap.Models;

namespace Funkmap.Mappers
{
    public static class UsersEntitiesCountMapper
    {
        public static ICollection<UsersEntitiesCountModel> ToCountModels(this ICollection<UserEntitiesCountInfo> source)
        {
            if (source == null) return null;
            var types = (EntityType[])Enum.GetValues(typeof(EntityType));

            var result = new List<UsersEntitiesCountModel>();

            foreach (var type in types)
            {
                var withExistingCount = source.FirstOrDefault(x => x.EntityType == type);
                result.Add(new UsersEntitiesCountModel()
                {
                    Type = type,
                    Count = withExistingCount?.Count ?? 0,
                    Logins = withExistingCount?.Logins
                });
            }

            return result;
        }
    }
}
