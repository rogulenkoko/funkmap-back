using System.Threading.Tasks;
using System.Web.Http;
using Funkmap.Common.Models;
using Funkmap.Statistics.Data.Services;
using Funkmap.Statistics.Models;
using Funkmap.Statistics.Models.Requests;
using Funkmap.Statistics.Services;

namespace Funkmap.Statistics.Controllers
{
    [RoutePrefix("api/statistics")]
    public class StatisticsController : ApiController
    {
        private readonly IStatisticsBuilder _statisticsBuilder;
        private readonly IStatisticsMerger _merger;

        public StatisticsController(IStatisticsBuilder statisticsBuilder, IStatisticsMerger merger)
        {
            _statisticsBuilder = statisticsBuilder;
            _merger = merger;
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

        [HttpPost]
        [Route("profileStatisticsWithDate")]
        public async Task<IHttpActionResult> GetProfileStatistics(DateRequest request)
        {

            ProfileStatistics response = await _statisticsBuilder.BuildProfileStatisticsAsync(request);

            return Ok(response);
        }

        [HttpPost]
        [Route("musicianStatisticsWithDate")]
        public async Task<IHttpActionResult> GetMusicianStatistics(DateRequest request)
        {
            MusicianStatistics response = await _statisticsBuilder.BuildMusicianStatisticsAsync(request);

            return Ok(response);
        }

        [HttpGet]
        [Route("buildStatistics")]
        public async Task<IHttpActionResult> BuildStatistics()
        {
            await _merger.MergeStatistics();
            return Ok(new BaseResponse() {Success = true});
        }
    }
}
