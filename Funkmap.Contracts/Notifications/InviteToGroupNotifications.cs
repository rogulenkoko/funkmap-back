using System;

namespace Funkmap.Contracts.Notifications
{
    public class InviteToBandRequest
    {
        public string BandLogin { get; set; }
        public string BandName { get; set; }
        public string InvitedMusicianLogin { get; set; }
    }
}
