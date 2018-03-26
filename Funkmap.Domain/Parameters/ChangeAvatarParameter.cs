namespace Funkmap.Domain.Parameters
{
    public class ChangeAvatarParameter
    {
        public string Login { get; set; }
        public string UserLogin { get; set; }
        public byte[] Avatar { get; set; }
    }
}
