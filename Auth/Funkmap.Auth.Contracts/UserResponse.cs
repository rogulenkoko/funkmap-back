namespace Funkmap.Auth.Contracts
{
    /// <summary>
    /// User response model
    /// </summary>
    public class UserResponse
    {
        /// <summary>
        /// Is user exists
        /// </summary>
        public bool IsExists { get; set; }

        /// <inheritdoc cref="User"/>
        public User User { get; set; }
    }
}
