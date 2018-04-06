using System;

namespace Funkmap.Data.Tools
{
    public class ImageNameBuilder
    {
        public static string BuildAvatarName(string login)
        {
            return $"Avatar_{login}.png";
        }

        public static string BuildAvatarMiniName(string login)
        {
            return $"AvatarMini_{login}.png";
        }
    }
}
