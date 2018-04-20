using System;
using Funkmap.Auth.Domain.Models;

namespace Funkmap.Auth.Services
{
    public class RegistrationContext
    {
        public string Code { get; set; }

        public User User { get; }
        
        private string _password;

        public string Password
        {
            get { return CryptoProvider.ComputeHash(_password); }
            private set { _password = value; }
        }

        public RegistrationContext(User user, string password)
        {
            if (user == null)
            {
                throw new ArgumentException("User can not be null.");
            }
            
            User = user;
            Password = password;
        }
    }
}
