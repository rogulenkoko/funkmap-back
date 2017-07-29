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
            //тут логируем до входа в контроллер
            await this.Next.Invoke(context);
            //тут логируем после входа в контроллер
        }
    }
}
