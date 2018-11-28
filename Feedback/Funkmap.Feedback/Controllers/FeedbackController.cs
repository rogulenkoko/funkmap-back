using System;
using System.Linq;
using System.Threading.Tasks;
using Funkmap.Common.Models;
using Funkmap.Cqrs.Abstract;
using Funkmap.Feedback.Command.Commands;
using Funkmap.Feedback.Domain;
using Funkmap.Feedback.Models;
using Microsoft.AspNetCore.Mvc;

namespace Funkmap.Feedback.Controllers
{
    /// <summary>
    /// Controller for Funkmap service feedback 
    /// </summary>
    [Route("api/feedback")]
    public class FeedbackController : Controller
    {
        private readonly ICommandBus _commandBus;

        /// <summary>
        /// FeedbackController constructor
        /// </summary>
        /// <param name="commandBus"><inheritdoc cref="ICommandBus"/></param>
        public FeedbackController(ICommandBus commandBus)
        {
            _commandBus = commandBus;
        }

        /// <summary>
        /// Send feedback.
        /// </summary>
        /// <param name="item"><see cref="FeedbackItem"/></param>
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> SaveFeedback([FromBody]FeedbackItem item)
        {
            try
            {
                var feedbackCommand = new FeedbackCommand(item);
                await _commandBus.ExecuteAsync(feedbackCommand);

                return Ok(new BaseResponse {Success = true});
            }
            catch (Exception e)
            {
                return Ok(new BaseResponse {Success = false, Error = e.Message});
            }
        }
    }
}