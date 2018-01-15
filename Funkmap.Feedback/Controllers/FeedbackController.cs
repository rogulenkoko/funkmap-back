using System.Threading.Tasks;
using System.Web.Http;
using Funkmap.Common.Filters;
using Funkmap.Common.Models;
using Funkmap.Feedback.Models;

namespace Funkmap.Feedback.Controllers
{
    [RoutePrefix("api/feedback")]
    [ValidateRequestModel]
    public class FeedbackController : ApiController
    {

        [HttpPost]
        [Route("validate/{login}")]
        public async Task<IHttpActionResult> Register(FeedbackItem item)
        {
            

            return Ok(new BaseResponse() {Success = true});
        }
    }
}
