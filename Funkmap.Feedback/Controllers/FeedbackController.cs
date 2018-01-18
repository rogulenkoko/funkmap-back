using System.Linq;
using System.Web.Http;
using Funkmap.Common.Cqrs.Abstract;
using Funkmap.Common.Filters;
using Funkmap.Common.Models;
using Funkmap.Feedback.Command.Commands;
using Funkmap.Feedback.Models;

namespace Funkmap.Feedback.Controllers
{
    [RoutePrefix("api/feedback")]
    [ValidateRequestModel]
    public class FeedbackController : ApiController
    {
        private readonly ICommandBus _commandBus;

        public FeedbackController(ICommandBus commandBus)
        {
            _commandBus = commandBus;
        }
        
        [HttpPost]
        [Route("save")]
        public IHttpActionResult SaveFeedback(FeedbackItem item)
        {
            _commandBus.Execute(new FeedbackCommand(item.FeedbackType, item.Message)
            {
                Content = item.Content?.Select(x=> new FeedbackContent() {Name = x.Name,Data = x.Data}).ToList()
            });

            return Ok(new BaseResponse() {Success = true});
        }
    }
}
