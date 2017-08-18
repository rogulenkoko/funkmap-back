using System;
using System.Threading.Tasks;
using Microsoft.Owin;

namespace Funkmap.Common
{
    public class FunkmapMiddleware : OwinMiddleware
    {
        public FunkmapMiddleware(OwinMiddleware next) : base(next)
        {
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
