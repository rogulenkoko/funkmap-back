using System;
using System.Threading.Tasks;
using Funkmap.Common.Logger;
using Microsoft.Owin;

namespace Funkmap.Console
{
    public class FunkmapMiddleware : OwinMiddleware
    {
        private readonly IFunkmapLogger<FunkmapMiddleware> _logger;

        public FunkmapMiddleware(OwinMiddleware next, IFunkmapLogger<FunkmapMiddleware> logger) : base(next)
        {
            _logger = logger;
        }

        public override async Task Invoke(IOwinContext context)
        {
            //todo
            _logger.Info($"Запрос {context.Request.Method}: {context.Request.Path}");
            try
            {
                await Next.Invoke(context);
            }
            catch (Exception e)
            {
                _logger.Error(e);
                throw;
            }
            
        }
    }
}
