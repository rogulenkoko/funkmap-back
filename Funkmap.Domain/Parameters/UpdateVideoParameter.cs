using Funkmap.Domain.Models;

namespace Funkmap.Domain.Parameters
{
    public class UpdateVideoParameter
    {
        public VideoInfo Info { get; set; }

        public string Login { get; set; }
    }
}
