using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Funkmap.Data.Entities;
using Funkmap.Models;
using Funkmap.Module.Auth.Models;


namespace Funkmap.Tests.web
{
    public class WebTestServise: TestServerBase
    {

        private HttpProxy _httpProxy;
        private string _ser;

        public WebTestServise(string serverUri)
        {
            _ser = serverUri;
            _httpProxy = new HttpProxy(serverUri);
        }

        public string Login()
        {
            var stringContent1 = new StringContent(
                "username:timosha\npassword: 123\ngrant_type: password",
                Encoding.UTF8,
                "application/x-www-form-urlencoded"
            );
            var stringContent = new StringContent(
                "username=timosha&password=123&grant_type=password",
                Encoding.UTF8, 
                "application/x-www-form-urlencoded"
                );
            KeyValuePair<string, object> qwe = new KeyValuePair<string, object>("Content-Type", "application/x-www-form-urlencoded");
            KeyValuePair<string, object> qwe1 = new KeyValuePair<string, object>("username", "timosha");
            KeyValuePair<string, object> qwe2 = new KeyValuePair<string, object>("password", "123");
            KeyValuePair<string, object> qwe3 = new KeyValuePair<string, object>("grant_type", "password");


            
            var requestMessage =new HttpRequestMessage()
            {
                Content = stringContent1,
                //Headers = {{"Content-Type", "application/x-www-form-urlencoded"}},
                Method = HttpMethod.Post,
                Properties = { qwe1,qwe2,qwe3},
                RequestUri = new Uri(_ser+"/token")
            };
            
            var t = new HttpMessageContent(requestMessage)
            {

            };
            t.Headers.ContentType.MediaType = "application/x-www-form-urlencoded";
            //t.HttpRequestMessage.
            //var q = _httpProxy.Post<string>(t, "/token");
            var response = CallServer(x => x.PostAsync("api/token", t)).GetAwaiter().GetResult();
            
            return null;
        }

        

        /* public string Login()
         {
           
             var stringContent = new StringContent("username=rogulenkoko&password=1&grant_type=password", Encoding.UTF8, "application/x-www-form-urlencoded");
             var token = "";
             var r = new HttpRequestMessage()
             {
                 Content = stringContent,
                 Headers = { { "Authorization", $"Bearer {token}" } },
                 Method = new HttpMethod("POST")
             };
             var t = new HttpMessageContent(r);

             var task = CallServer(x => x.PostAsync("/token", t));
            var q = _httpProxy.Post<string, string>("username=rogulenkoko&password=1&grant_type=password", "/token").GetAwaiter().GetResult();

             HttpResponseMessage response = CallServer(x => x.PostAsync("/token", t)).GetAwaiter().GetResult();
             string body = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            
            
             return "";
         }*/
        
        public BandModel GetBandModel(int bandId)
        {
            return _httpProxy.Get<BandModel>($"/band/get/{bandId}").GetAwaiter().GetResult();
        }

        private string createURL(string URL, string value, string key)
        {
           string url = URL + "?" + value + "=" + key;
           return url;
        }


    

    }
}
