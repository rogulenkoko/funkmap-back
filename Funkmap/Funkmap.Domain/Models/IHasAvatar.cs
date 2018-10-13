namespace Funkmap.Domain.Models
{
    public interface IHasAvatar
    {
        string Login { get; set; }
        string AvatarUrl { get; set; }
        string AvatarMiniUrl { get; set; }
    }
}
