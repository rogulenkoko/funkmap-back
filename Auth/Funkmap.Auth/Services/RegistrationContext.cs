using System;
using Funkmap.Auth.Contracts;

namespace Funkmap.Auth.Services
{
    /// <summary>
    /// Context for user's registration
    /// </summary>
    public class RegistrationContext
    {
        /// <summary>
        /// Confirmation code
        /// </summary>
        public string Code { get; set; }

        /// <see cref="User"/>
        public User User { get; }
        
        private string _password;

        /// <summary>
        /// User's password
        /// </summary>
        public string Password
        {
            get => CryptoProvider.ComputeHash(_password);
            private set => _password = value;
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="user"><see cref="User"/></param>
        /// <param name="password">User's password</param>
        public RegistrationContext(User user, string password)
        {
            User = user ?? throw new ArgumentException("User can not be null.");
            Password = password;
        }
    }
}
