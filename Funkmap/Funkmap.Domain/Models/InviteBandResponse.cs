
namespace Funkmap.Domain.Models
{
    public class InviteBandResponse
    {
        public bool Success { get; set; }

        public string Error { get; set; }


        /// <summary>
        /// Owner of the musician (user).
        /// If you invite your own musician you haven't to confirm invitation.
        /// </summary>
        public bool IsOwner { get; set; }

        /// <summary>
        /// Musician's owner login
        /// </summary>
        public string OwnerLogin { get; set; }
        
        public string BandName { get; set; }

        public string MusicianLogin { get; set; }
    }
}
