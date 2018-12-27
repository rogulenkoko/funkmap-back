using Funkmap.Common.Models;

namespace Funkmap.Auth.Contracts
{
    /// <summary>
    /// Avatar update response model
    /// </summary>
    public class AvatarUpdateResponse : BaseResponse
    {
        /// <summary>
        /// Avatar url
        /// </summary>
        public string AvatarPath { get; set; }
    }
}
