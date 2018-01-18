using System;

namespace Funkmap.Data.Tools
{
    public class ImageNameBuilder
    {
        public static string BuildAvatarName(string login)
        {
            return $"Avatar_{login}_{DateTime.UtcNow:yyyyMMddhhmmss}.png";
        }

        public static string BuildAvatarMiniName(string login)
        {
            return $"AvatarMini_{login}_{DateTime.UtcNow:yyyyMMddhhmmss}.png";
        }
    }
}
