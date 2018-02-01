using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Funkmap.Common.Auth;

namespace Funkmap.Concerts.Controllers
{
    [RoutePrefix("concert")]
    public class ConcertsController : ApiController
    {
        [Route("active")]
        [HttpGet]
        public IHttpActionResult GetAllActiveConcerts()
        {
            return Ok();
        }

        [Route("create")]
        [HttpPost]
        public IHttpActionResult CreateConcert()
        {
            var login = Request.GetLogin();
            return Ok();
        }
    }
}
