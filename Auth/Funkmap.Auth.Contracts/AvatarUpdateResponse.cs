using Funkmap.Common.Models;

namespace Funkmap.Auth.Contracts
{
    public class AvatarUpdateResponse : BaseResponse
    {
        public string AvatarPath { get; set; }
    }
}
