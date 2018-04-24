using System.Collections.Generic;
using Microsoft.Build.Framework;

namespace Funkmap.Models.Requests
{
    public class UpdateBandMemberRequest
    {
        /// <summary>
        /// Invited musician login
        /// </summary>
        [Required]
        public string MusicianLogin { get; set; }

        /// <summary>
        /// Inviting band login
        /// </summary>
        [Required]
        public string BandLogin { get; set; }
    }

    public class UpdateBandMembersRequest
    {
        /// <summary>
        /// Invited musicians logins
        /// </summary>
        [Required]
        public List<string> MusicianLogins { get; set; }

        /// <summary>
        ///  Inviting band login
        /// </summary>
        [Required]
        public string BandLogin { get; set; }
    }
}
