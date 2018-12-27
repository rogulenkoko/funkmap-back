namespace Funkmap.Auth.Services
{
    /// <summary>
    /// Context for password restore
    /// </summary>
    public class RestoreContext
    {
        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Restore password confirmation code
        /// </summary>
        public string Code { get; set; }
    }
}
