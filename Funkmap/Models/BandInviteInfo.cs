using System.Collections.Generic;

namespace Funkmap.Models
{
    public class BandInviteInfo
    {
        /// <summary>
        /// Bands in which you can invite musicians
        /// </summary>
        public ICollection<BandPreviewModel> AvailableBands { get; set; }
    }
}
