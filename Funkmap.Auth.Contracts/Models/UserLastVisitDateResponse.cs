using System;

namespace Funkmap.Auth.Contracts.Models
{
   public class UserUpdateLastVisitDateRequest
    {
        public string Login { get; set; }
    }

    public class UserUpdateLastVisitDateResponse
    {
        public bool Success { get; set; }
    }
}
