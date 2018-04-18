
namespace Funkmap.Domain.Parameters
{
    public class UpdateFavoriteParameter
    {
        public string UserLogin { get; set; }
        public string ProfileLogin { get; set; }
        public bool IsFavorite { get; set; }
    }
}
