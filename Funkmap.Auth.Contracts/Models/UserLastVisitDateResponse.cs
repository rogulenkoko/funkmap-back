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
}
