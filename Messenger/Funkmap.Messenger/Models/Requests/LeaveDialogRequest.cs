using System.ComponentModel.DataAnnotations;

namespace Funkmap.Messenger.Models.Requests
{
    public class LeaveDialogRequest
    {
        [Required]
        public string DialogId { get; set; }

        [Required]
        public string Login { get; set; }
    }
}
