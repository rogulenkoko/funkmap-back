using System;

namespace Funkmap.Auth.Contracts
{
    /// <summary>
    /// User model
    /// </summary>
    public class User
    {
        /// <summary>
        /// Login
        /// </summary>
        public string Login { get; set; }
        
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }
        
        /// <summary>
        /// Locale
        /// </summary>
        public string Locale { get; set; }

        /// <summary>
        /// Avatar url
        /// </summary>
        public string AvatarUrl { get; set; }

        /// <summary>
        /// Last visit date
        /// </summary>
        public DateTime LastVisitDateUtc { get; set; }

        /// <summary>
        /// Is external service user
        /// </summary>
        public bool IsSocial { get; set; }
    }
}
