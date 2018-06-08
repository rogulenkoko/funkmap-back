namespace Funkmap.Auth.Contracts
{
    public class UserConfirmationModel
    {
        public User User { get; set; }
        public string Code { get; set; }
    }
}
