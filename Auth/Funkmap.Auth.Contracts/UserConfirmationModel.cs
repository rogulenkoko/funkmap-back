namespace Funkmap.Auth.Contracts
{
    /// <summary>
    /// User confirmation model
    /// </summary>
    public class UserConfirmationModel
    {
        /// <inheritdoc cref="User"/>
        public User User { get; set; }

        /// <summary>
        /// Confirmation code
        /// </summary>
        public string Code { get; set; }
    }
}
