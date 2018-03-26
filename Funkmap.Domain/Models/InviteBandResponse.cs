
namespace Funkmap.Domain.Models
{
    public class InviteBandResponse
    {
        public bool Success { get; set; }

        public string Error { get; set; }

        public bool IsOwner { get; set; }

        public string OwnerLogin { get; set; }

        //todo подумать может убрать
        public string BandName { get; set; }

        public string MusicianLogin { get; set; }
    }
}
