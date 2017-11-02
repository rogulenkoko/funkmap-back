using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

namespace Funkmap.Tests.web
{
    public class HttpProxy : TestServerBase
    {
        private readonly Func<string> _tokenFactory;
        private string _webServerUri;
        private string ContentType = "application/x-www-form-urlencoded";
        public const string AuthenticateHeader = "Funkmap.Module.Auth";

        public HttpProxy(string webServerUriFactory)
            : this(webServerUriFactory, null)
        {
        }

        public HttpProxy(string webServerUriFactory, Func<string> tokenFactory)
        {
            if (webServerUriFactory == null) throw new ArgumentNullException(nameof(webServerUriFactory));
            _webServerUri = webServerUriFactory;
            _tokenFactory = tokenFactory;
        }

        public Task<TOut> Get<TOut>(string uri)
        {
            return Start(() =>
            {
                using (HttpClient client = GetHttpClient())
                {
                    var response = client.GetAsync(_webServerUri + uri).ConfigureAwait(false).GetAwaiter().GetResult();
                    response.EnsureSuccessStatusCode();
                    return GetResponseObject<TOut>(response).ConfigureAwait(false).GetAwaiter().GetResult();
                    
                }
            }, uri);
        }

        public Task Get(string uri)
        {
            return Start(() =>
            {
                using (HttpClient client = GetHttpClient())
                {
                    var response = client.GetAsync(_webServerUri + uri).ConfigureAwait(false).GetAwaiter().GetResult();
                    response.EnsureSuccessStatusCode();
                }
            }, uri);
        }

        public Task Post<TIn>(TIn input, string uri)
        {
            return Start(() =>
            {
                using (HttpClient client = GetHttpClient())
                {
                    var response = client.PostAsync(_webServerUri + uri, GetContent(input)).GetAwaiter().GetResult();
                    response.EnsureSuccessStatusCode();
                }
            }, uri);
        }

        public Task<TOut> Post<TOut>(HttpMessageContent input, string uri)
        {
            return Start(() =>
            {
                using (HttpClient client = GetHttpClientBase())
                {
                    
                    var response = client.PostAsync(_webServerUri + uri,input).GetAwaiter().GetResult();
                    response.EnsureSuccessStatusCode();
                    var tt = GetResponseObject<TOut>(response).ConfigureAwait(false).GetAwaiter().GetResult();
                    return tt;
                }
            }, uri);
        }

        public Task Put<TIn>(TIn input, string uri)
        {
            return Start((() =>
            {
                using (HttpClient client = GetHttpClient())
                {
                    var response =
                        client.PutAsync(_webServerUri + uri, GetContent(input))
                            .ConfigureAwait(false)
                            .GetAwaiter()
                            .GetResult();
                    response.EnsureSuccessStatusCode();
                }
            }), uri);
        }

        public Task<TOut> Put<TIn, TOut>(TIn input, string uri)
        {
            return Start(() =>
            {
                using (HttpClient client = GetHttpClient())
                {
                    var response = client.PutAsync(_webServerUri + uri, GetContent(input)).ConfigureAwait(false)
                        .GetAwaiter().GetResult();
                    response.EnsureSuccessStatusCode();
                    return GetResponseObject<TOut>(response).GetAwaiter().GetResult();
                    ;
                }
            }, uri);
        }

        private HttpClient GetHttpClient()
        {
            //var client = new HttpClient();
            //string token = _tokenFactory?.Invoke();
            //if (!string.IsNullOrEmpty(token))
            //{
            //    client.DefaultRequestHeaders.Add(AuthenticateHeader, token);
            //}
            var client = GetHttpClientBase();
            return client;
        }

        private HttpContent GetContent(object obj)
        {
            return new StringContent(Serialize(obj), Encoding.UTF8, ContentType);
        }

        private async Task<TOut> GetResponseObject<TOut>(HttpResponseMessage response)
        {
            string responseBody = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return Deserialize<TOut>(responseBody);
        }

        private T Deserialize<T>(string responseBody)
        {
            return JsonConvert.DeserializeObject<T>(responseBody);
        }

        private string Serialize(object request)
        {
            return JsonConvert.SerializeObject(request);
        }


        private Task<T> Start<T>(Func<T> action, string uri)
        {
            var stopwatch = Stopwatch.StartNew();
            return Task.Factory.StartNew(() => { return action.Invoke(); }).ContinueWith((t) =>
            {
                stopwatch.Stop();
                Debug.WriteLine($"Request on {_webServerUri + uri} time: {stopwatch.Elapsed}");
                return t.Result;
            });
        }

        private Task Start(Action action, string uri)
        {
            var stopwatch = Stopwatch.StartNew();
            return Task.Factory.StartNew(() => { action.Invoke(); }).ContinueWith((t) =>
            {
                stopwatch.Stop();
                Debug.WriteLine($"Request on {_webServerUri + uri} time: {stopwatch.Elapsed}");
            });
        }
    }
}