using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
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
        
        /// <summary>
        /// Send feedback.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPost]
        [ResponseType(typeof(BaseResponse))]
        [Route("")]
        public async Task<IHttpActionResult> SaveFeedback(FeedbackItem item)
        {
            try
            {
                await _commandBus.ExecuteAsync(new FeedbackCommand(item.FeedbackType, item.Message)
                {
                    Content = item.Content?.Select(x => new FeedbackContent() { Name = x.Name, Data = x.Data }).ToList()
                });

                return Ok(new BaseResponse() { Success = true });
            }
            catch (Exception e)
            {
                return Ok(new BaseResponse() { Success = false });
            }
        }
    }
}
