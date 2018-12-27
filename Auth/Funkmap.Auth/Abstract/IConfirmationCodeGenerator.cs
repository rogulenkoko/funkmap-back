
namespace Funkmap.Auth.Abstract
{
    /// <summary>
    /// Confirmation code generator
    /// </summary>
    public interface IConfirmationCodeGenerator
    {
        /// <summary>
        /// Generate confirmation code
        /// </summary>
        string Generate();
    }
}
