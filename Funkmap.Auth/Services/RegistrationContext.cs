
using System;
using Funkmap.Auth.Data.Entities;

namespace Funkmap.Module.Auth.Services
{
    public class RegistrationContext
    {
        public string Code { get; set; }

        public UserEntity User { get; }

        public RegistrationContext(UserEntity user)
        {
            if (user == null)
            {
                throw new ArgumentException("user is null");
            }
            
            User = user;
        }
    }
}
