namespace Funkmap.Domain.Models
{
    public class CanCreateProfileResult
    {
        public CanCreateProfileResult(bool canCreate, string reason = null)
        {
            CanCreate = canCreate;
            Reason = reason;
        }

        public bool CanCreate { get; }
        public string Reason { get; }
    }
}
