using Microsoft.Build.Framework;

namespace Funkmap.Models.Requests
{
    public class UpdateFavoriteRequest
    {
        [Required]
        public string EntityLogin { get; set; }

        [Required]
        public bool IsFavorite { get; set; }
    }
}
