using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using Funkmap.Common.Models;
using Funkmap.Feedback.Domain;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Xunit;

namespace Funkmap.Feedback.Tests
{
    public class FeedbackTest
    {
        private readonly HttpClient _client;
        private readonly IConfiguration _configuration;
        
        public FeedbackTest()
        {
            _configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json").Build();

            var server = new TestServer(new WebHostBuilder()
                .UseStartup<FeedbackStartup>()
                .ConfigureServices(x => x.AddAutofac())
                .UseConfiguration(_configuration)
            );
            _client = server.CreateClient();
        }
        
        [Fact]
        public async Task CreateFeedbackValidateTest()
        {
            var apiUrl = "api/feedback";
            var feedbackItem = new FeedbackItem();
            var httpResponse = await _client.PostAsJsonAsync(apiUrl, feedbackItem);
            Assert.Equal(httpResponse.StatusCode, HttpStatusCode.BadRequest);

            feedbackItem.FeedbackType = FeedbackType.Bug;
            httpResponse = await _client.PostAsJsonAsync(apiUrl, feedbackItem);
            Assert.Equal(httpResponse.StatusCode, HttpStatusCode.BadRequest);

            feedbackItem.Message = "Some message";
            httpResponse = await _client.PostAsJsonAsync(apiUrl, feedbackItem);
            Assert.Equal(httpResponse.StatusCode, HttpStatusCode.OK);
            
            var responseJson = await httpResponse.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<BaseResponse>(responseJson);
            Assert.NotNull(response);
            Assert.True(response.Success);
            Assert.True(string.IsNullOrEmpty(response.Error));
        }
        
        [Fact]
        public async Task CreateFeedbackNotificationTest()
        {
            var apiUrl = "api/feedback";
            var feedbackItem = new FeedbackItem
            {
                Message = "Some message",
                FeedbackType = FeedbackType.Another
            };
            
            var httpResponse = await _client.PostAsJsonAsync(apiUrl, feedbackItem);
            Assert.Equal(httpResponse.StatusCode, HttpStatusCode.OK);
            
            var responseJson = await httpResponse.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<BaseResponse>(responseJson);
            Assert.NotNull(response);
            Assert.True(response.Success);
            Assert.True(string.IsNullOrEmpty(response.Error));
        }
    }
}