using Microsoft.Build.Framework;

namespace Funkmap.Models.Requests
{
    public class UpdateBandMembersRequest
    {
        [Required]
        public string MusicianLogin { get; set; }

        [Required]
        public string BandLogin { get; set; }
    }
}
