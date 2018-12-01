using System.ComponentModel.DataAnnotations;

namespace Funkmap.Models.Requests
{
    public class UpdateFavoriteRequest
    {
        /// <summary>
        /// Profile login
        /// </summary>
        [Required]
        public string EntityLogin { get; set; }

        /// <summary>
        /// Favourite flag (need to add ot delete)
        /// </summary>
        [Required]
        public bool IsFavorite { get; set; }
    }
}
