namespace Funkmap.Module.Auth.Models
{
    public class ConfirmationRequest
    {
        public string Login { get; set; }
        public string Code { get; set; }
    }
}
