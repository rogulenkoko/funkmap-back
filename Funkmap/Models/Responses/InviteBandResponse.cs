using Funkmap.Common.Models;

namespace Funkmap.Models.Responses
{
    public class InviteBandResponse : BaseResponse
    {
        public bool Success { get; set; }

        public bool IsOwner { get; set; }

        public string OwnerLogin { get; set; }

        //todo подумать может убрать
        public string BandName { get; set; }

        public string MusicianLogin { get; set; }
    }
}
