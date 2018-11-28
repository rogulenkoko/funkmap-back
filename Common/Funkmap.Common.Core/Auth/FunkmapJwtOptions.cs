using Microsoft.Extensions.Configuration;

namespace Funkmap.Common.Core.Auth
{
    /// <summary>
    /// Specific Funkmap JWT options
    /// </summary>
    public class FunkmapJwtOptions
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configuration"><see cref="IConfiguration "/></param>
        public FunkmapJwtOptions(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Issuer => _configuration["Auth:Issuer"];
        public string Audience => _configuration["Auth:Audience"];

        public string Key => _configuration["Auth:Key"];

        public string TokenUrl => _configuration["Auth:BandmapTokenUrl"];
    }
}