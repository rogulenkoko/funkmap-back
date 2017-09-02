using System;
using System.Threading.Tasks;
using Funkmap.Common.Logger;
using Microsoft.Owin;

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
            try
            {
                await this.Next.Invoke(context);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }
    }
}
