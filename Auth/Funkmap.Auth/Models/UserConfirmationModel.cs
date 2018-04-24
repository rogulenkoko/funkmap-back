using Funkmap.Auth.Domain.Models;

namespace Funkmap.Auth.Models
{
    public class UserConfirmationModel
    {
        public User User { get; set; }
        public string Code { get; set; }
    }
}
