using System.Collections.Generic;
using Funkmap.Domain.Enums;

namespace Funkmap.Models
{
    public class BandPreviewModel : ProfilePreview
    {
        public List<Styles> Styles { get; set; }

        public List<Instruments> DesiredInstruments { get; set; }
    }
}
