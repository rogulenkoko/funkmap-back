using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Funkmap.Common.Models;
using Funkmap.Feedback.Command;
using Funkmap.Feedback.Domain;
using Funkmap.Feedback.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Newtonsoft.Json;
using Xunit;

namespace Funkmap.Feedback.Tests
{
    public class FeedbackApiTest
    {
        private readonly HttpClient _client;
        private readonly IConfiguration _configuration;
        
        public FeedbackApiTest()
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
            Assert.Equal(HttpStatusCode.BadRequest, httpResponse.StatusCode);

            feedbackItem.FeedbackType = FeedbackType.Bug;
            httpResponse = await _client.PostAsJsonAsync(apiUrl, feedbackItem);
            Assert.Equal(HttpStatusCode.BadRequest, httpResponse.StatusCode);

            feedbackItem.Message = "Some message";
            httpResponse = await _client.PostAsJsonAsync(apiUrl, feedbackItem);
            Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);
            
            var responseJson = await httpResponse.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<BaseResponse>(responseJson);
            Assert.NotNull(response);
            Assert.True(response.Success);
            Assert.True(string.IsNullOrEmpty(response.Error));
        }
        
        [Fact]
        public async Task CreateFeedbackCreatedTest()
        {
            var apiUrl = "api/feedback";
            var uniqueMessage = Guid.NewGuid().ToString();
            var feedbackItem = new FeedbackItem
            {
                Message = uniqueMessage,
                FeedbackType = FeedbackType.Another
            };
            
            var httpResponse = await _client.PostAsJsonAsync(apiUrl, feedbackItem);
            Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);

            var builder = new ContainerBuilder();
            builder.RegisterFeedbackCommandModule(_configuration);
            var container = builder.Build();
            var collection = container.Resolve<IMongoCollection<FeedbackEntity>>();
            var savedFeedbackItem = await collection.Find(x => true)
                .Sort(Builders<FeedbackEntity>.Sort.Descending(x => x.Created))
                .Limit(1)
                .SingleOrDefaultAsync();
            
            Assert.NotNull(savedFeedbackItem);
            Assert.Equal(uniqueMessage, savedFeedbackItem.Message);
        }
    }
}