using System;
using System.Linq;
using System.Threading.Tasks;
using Funkmap.Common.Models;
using Funkmap.Cqrs.Abstract;
using Funkmap.Feedback.Command.Commands;
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
                await _commandBus.ExecuteAsync(new FeedbackCommand(item.FeedbackType, item.Message)
                {
                    Content = item.Content?.Select(x => new FeedbackContent {Name = x.Name, Data = x.Data}).ToList()
                });

                return Ok(new BaseResponse() {Success = true});
            }
            catch (Exception e)
            {
                return Ok(new BaseResponse() {Success = false, Error = e.Message});
            }
        }
    }
}