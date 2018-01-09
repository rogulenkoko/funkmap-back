
using System;
using Funkmap.Auth.Data.Entities;

namespace Funkmap.Module.Auth.Services
{
    public class RegistrationContext
    {
        public string Login { get; }

        public string Email
        {
            get { return User.Email; }
            set { User.Email = value; }
        }

        public string Code { get; set; }

        public UserEntity User { get; }

        public RegistrationContext(UserEntity user)
        {
            if (user == null)
            {
                throw new ArgumentException("user is null");
            }

            Login = user.Login;
            User = user;

        }
    }
}
