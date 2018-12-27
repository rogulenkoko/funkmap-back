﻿using Funkmap.Auth.Contracts;
using Funkmap.Auth.Data.Entities;

namespace Funkmap.Auth.Data.Mappers
{
    internal static class UserMapper
    {
        internal static User ToUser(this UserEntity source)
        {
            if (source == null) return null;
            return new User
            {
                Login = source.Login,
                Name = source.Name,
                AvatarUrl = source.AvatarUrl,
                Email = source.Email,
                Locale = source.Locale,
                LastVisitDateUtc = source.LastVisitDateUtc,
                IsSocial = source.IsSocial
            };
        }

        internal static UserEntity ToEntity(this User source, string password)
        {
            if (source == null) return null;
            return new UserEntity
            {
                Login = source.Login,
                Name = source.Name,
                Email = source.Email,
                Locale = source.Locale,
                Password = password,
                AvatarUrl = source.AvatarUrl,
                LastVisitDateUtc = source.LastVisitDateUtc,
                IsSocial = false
            };
        }

        internal static UserEntity ToSocialEntity(this User source)
        {
            if (source == null) return null;
            return new UserEntity
            {
                AvatarUrl = source.AvatarUrl,
                Login = source.Login,
                Name = source.Name,
                Email = source.Email,
                Locale = source.Locale,
                LastVisitDateUtc = source.LastVisitDateUtc,
                IsSocial = true
            };
        }
    }
}
