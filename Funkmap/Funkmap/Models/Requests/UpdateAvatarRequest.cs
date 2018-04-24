namespace Funkmap.Models.Requests
{
    public class UpdateAvatarRequest
    {
        /// <summary>
        /// Profile login
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Photo bytes or base64 string (null or empty array for deleting)
        /// </summary>
        public byte[] Photo { get; set; }
    }
}
