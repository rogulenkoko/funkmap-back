using System;

namespace Funkmap.Auth.Contracts.Models
{
    public class UserLastVisitDateResponse
    {
        public DateTime? LastVisitDateUtc { get; set; }
    }

    public class UserLastVisitDateRequest
    {
        public string Login { get; set; }
    }

    public class UserUpdateLastVisitDateRequest : UserLastVisitDateRequest
    {
        
    }

    public class UserUpdateLastVisitDateResponse
    {
        public bool Success { get; set; }
    }
}
