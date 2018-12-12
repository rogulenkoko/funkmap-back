using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Funkmap.Messenger.Models.Requests
{
    public class InviteParticipantsRequest
    {
        [Required]
        public string DialogId { get; set; }

        [Required]
        public ICollection<string> InvitedUserLogins { get; set; }
    }
}
