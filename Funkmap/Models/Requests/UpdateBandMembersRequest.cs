using System.Collections.Generic;
using Microsoft.Build.Framework;

namespace Funkmap.Models.Requests
{
    public class UpdateBandMemberRequest
    {
        /// <summary>
        /// Логин приглашаемого музыканта
        /// </summary>
        [Required]
        public string MusicianLogin { get; set; }

        /// <summary>
        /// Группа, в которую приглашается музыкант
        /// </summary>
        [Required]
        public string BandLogin { get; set; }
    }

    public class UpdateBandMembersRequest
    {
        /// <summary>
        /// Логин приглашаемого музыканта
        /// </summary>
        [Required]
        public List<string> MusicianLogins { get; set; }

        /// <summary>
        /// Группа, в которую приглашается музыкант
        /// </summary>
        [Required]
        public string BandLogin { get; set; }
    }
}
