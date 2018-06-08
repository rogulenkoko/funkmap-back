using System;

namespace Funkmap.Auth.Contracts
{
    public class User
    {
        public string Login { get; set; }
        
        public string Name { get; set; }
        
        public string Email { get; set; }
        
        public string Locale { get; set; }

        public string AvatarUrl { get; set; }

        public DateTime LastVisitDateUtc { get; set; }
    }
}
