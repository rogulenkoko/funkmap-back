using System.Threading.Tasks;
using System.Web.Http;
using Funkmap.Common.Models;
using Funkmap.Statistics.Data.Entities;
using Funkmap.Statistics.Data.Services;
using Funkmap.Statistics.Models;
using Funkmap.Statistics.Models.Requests;
using Funkmap.Statistics.Services;
using Swashbuckle.Swagger.Annotations;

namespace Funkmap.Statistics.Controllers
{
    /// <summary>
    /// Statistics controller
    /// </summary>
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

        /// <summary>
        /// All staistics by profiles:
        /// Profile types - 1
        /// Distribution by city - 2
        /// Top profiles by favourite marks - 3
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerOperation("profileStatistics")]
        [Route("profileStatistics")]
        public async Task<IHttpActionResult> GetProfileStatistics()
        {
            
            ProfileStatistics response = await _statisticsBuilder.BuildProfileStatisticsAsync();

            return Ok(response);
        }


        /// <summary>
        /// All staistics by musicians:
        /// By sex - 4
        /// By instrument - 5
        /// Top music styles - 6
        /// In band / single - 7
        /// By age - 8
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerOperation("musicianStatistics")]
        [Route("musicianStatistics")]
        public async Task<IHttpActionResult> GetMusicianStatistics()
        {
            MusicianStatistics response = await _statisticsBuilder.BuildMusicianStatisticsAsync();

            return Ok(response);
        }

        /// <summary>
        /// All staistics by profiles with date range
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [SwaggerOperation("profileStatisticsWithDate")]
        [Route("profileStatisticsWithDate")]
        public async Task<IHttpActionResult> GetProfileStatistics(DateRequest request)
        {

            ProfileStatistics response = await _statisticsBuilder.BuildProfileStatisticsAsync(request);

            return Ok(response);
        }

        /// <summary>
        /// All staistics by musicians with date range
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [SwaggerOperation("musicianStatisticsWithDate")]
        [Route("musicianStatisticsWithDate")]
        public async Task<IHttpActionResult> GetMusicianStatistics(DateRequest request)
        {
            MusicianStatistics response = await _statisticsBuilder.BuildMusicianStatisticsAsync(request);

            return Ok(response);
        }

        /// <summary>
        /// Build statistics request
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerOperation("buildStatistics")]
        [Route("buildStatistics")]
        public async Task<IHttpActionResult> BuildStatistics()
        {
            await _merger.MergeStatistics();
            return Ok(new BaseResponse() {Success = true});
        }

        /// <summary>
        /// Specific statistics by type
        /// Types:
        /// Profile types - 1
        /// Distribution by city - 2
        /// Top profiles by favourite marks - 3
        /// By sex - 4
        /// By instrument - 5
        /// Top music styles - 6
        /// In band / single - 7
        /// By age - 8
        /// </summary>
        /// <param name="type">Statistics type</param>
        /// <returns></returns>
        [HttpGet]
        [SwaggerOperation("buildSpecificStatistics")]
        [Route("buildSpecificStatistics/{type}")]
        public async Task<IHttpActionResult> BuildSpecificStatistics(StatisticsType type)
        {
            var statistics = await _statisticsBuilder.BuildSpecificStatistic(type);
            return Ok(statistics);
        }
    }
}
