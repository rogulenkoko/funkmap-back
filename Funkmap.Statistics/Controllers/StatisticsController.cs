using System.Threading.Tasks;
using System.Web.Http;
using Funkmap.Statistics.Models;
using Funkmap.Statistics.Services;

namespace Funkmap.Statistics.Controllers
{
    [RoutePrefix("api/statistics")]
    public class StatisticsController : ApiController
    {
        private readonly IStatisticsBuilder _statisticsBuilder;

        public StatisticsController(IStatisticsBuilder statisticsBuilder)
        {
            _statisticsBuilder = statisticsBuilder;
        }

        [HttpGet]
        [Route("profileStatistics")]
        public async Task<IHttpActionResult> GetProfileStatistics()
        {
            ProfileStatistics response = await _statisticsBuilder.BuildProfileStatisticsAsync();

            return Ok(response);
        }

        [HttpGet]
        [Route("musicianStatistics")]
        public async Task<IHttpActionResult> GetMusicianStatistics()
        {
            MusicianStatistics response = await _statisticsBuilder.BuildMusicianStatisticsAsync();

            return Ok(response);
        }
    }
}
