using System;
using System.IO;
using System.Threading.Tasks;
using Funkmap.Common.Logger;
using Microsoft.Owin;
using Microsoft.Owin.Logging;

namespace Funkmap.Common
{
    public class FunkmapMiddleware : OwinMiddleware
    {
        private readonly IFunkmapLogger<FunkmapMiddleware> _logger;

        public FunkmapMiddleware(OwinMiddleware next, IFunkmapLogger<FunkmapMiddleware> logger) : base(next)
        {
            _logger = logger;
        }

        public async override Task Invoke(IOwinContext context)
        {
            //todo
            _logger.Info($"Запрос {context.Request.Method}: {context.Request.Path}");
            try
            {
                await this.Next.Invoke(context);
            }
            catch (Exception e)
            {
                _logger.Error(e);
                throw;
            }
            
        }
    }
}
