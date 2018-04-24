using Funkmap.Auth.Domain.Models;

namespace Funkmap.Auth.Models
{
    public class UserResponse
    {
        public bool IsExists { get; set; }
        public User User { get; set; }
    }
}
