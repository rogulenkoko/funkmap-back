using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Funkmap.Middleware;
using Microsoft.Owin.Testing;

namespace Funkmap.Tests.web
{
    public abstract class TestServerBase
    {
        public async Task<T> CallServer<T>(Func<HttpClient, Task<T>> callback)
        {
            var server = TestServer.Create<Startup>();
            return await callback(server.HttpClient);
            
        }

        public HttpClient GetHttpClientBase()
        {
            var server = TestServer.Create<Startup>();
            return server.HttpClient;
        }
    }
}