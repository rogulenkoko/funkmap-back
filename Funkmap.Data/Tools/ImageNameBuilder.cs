using Funkmap.Data.Entities.Abstract;

namespace Funkmap.Data.Tools
{
    public class ImageNameBuilder
    {
        public static string BuildAvatarName(BaseEntity entity)
        {
            return $"Avatar_{entity.Login}";
        }

        public static string BuildAvatarMiniName(BaseEntity entity)
        {
            return $"AvatarMini_{entity.Login}";
        }
    }
}
