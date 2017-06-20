using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;

namespace Funkmap.Common
{
    public class FunkmapHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            //todo здесь логируем все реквесты и их параметры

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
