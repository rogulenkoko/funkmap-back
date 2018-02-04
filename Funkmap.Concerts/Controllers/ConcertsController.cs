using System.Web.Http;
using Funkmap.Common.Auth;
using Funkmap.Common.Cqrs.Abstract;
using Funkmap.Concerts.Command.Commands;
using Funkmap.Concerts.Models;

namespace Funkmap.Concerts.Controllers
{
    [RoutePrefix("concert")]
    public class ConcertsController : ApiController
    {
        private readonly ICommandBus _commandBus;
        private readonly IQueryContext _queryContext;

        public ConcertsController(ICommandBus commandBus, IQueryContext queryContext)
        {
            _commandBus = commandBus;
            _queryContext = queryContext;
        }
        
        [Route("active")]
        [HttpGet]
        public IHttpActionResult GetAllActiveConcerts()
        {
            return Ok();
        }

        [Route("create")]
        [Authorize]
        [HttpPost]
        public IHttpActionResult CreateConcert(CreateConcertRequest request)
        {
            var login = Request.GetLogin();


            var command = new CreateConcertCommand()
            {
                CreatorLogin = login,
                Name = request.Name,
                Description = request.Description,
                Participants = request.Participants,
                AfficheBytes = request.AfficheBytes,
                PeriodBeginUtc = request.PeriodBegin.ToUniversalTime(),
                PeriodEndUtc = request.PeriodEnd.ToUniversalTime(),
                DateUtc = request.Date.ToUniversalTime()
            };

            _commandBus.Execute(command);

            return Ok();
        }
    }
}
