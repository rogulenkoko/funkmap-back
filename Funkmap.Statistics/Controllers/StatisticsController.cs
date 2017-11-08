using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Funkmap.Statistics.Data.Repositories.Abstract;

namespace Funkmap.Statistics.Controllers
{
    [RoutePrefix("api/statistics")]
    public class StatisticsController : ApiController
    {

        private readonly IEnumerable<IStatisticsRepository> _repositories;

        public StatisticsController(IEnumerable<IStatisticsRepository> repositories)
        {
            _repositories = repositories;
        }


    }
}
