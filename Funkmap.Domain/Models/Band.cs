using System.Collections.Generic;
using Funkmap.Domain.Enums;

namespace Funkmap.Domain.Models
{
    public class Band : Profile
    {
        public List<Styles> Styles { get; set; }
        public List<Instruments> DesiredInstruments { get; set; }
        public List<string> Musicians { get; set; }
        public List<string> InvitedMusicians { get; set; }
    }
}
