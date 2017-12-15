namespace Funkmap.Models.Requests
{
    public class UpdateAvatarRequest
    {
        public string Login { get; set; }
        public byte[] Photo { get; set; }
    }
}
