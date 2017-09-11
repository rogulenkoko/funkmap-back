using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Autofac;
using Funkmap.Auth.Data;
using Funkmap.Auth.Data.Entities;
using Funkmap.Common.Modules;
using Funkmap.Common.RedisMq;
using Funkmap.Messenger.Services;
using Funkmap.Module.Auth;
using Funkmap.Module.Auth.Services;
using Funkmap.Notifications.Contracts.Funkmap;
using Funkmap.Notifications.Services;
using Funkmap.Services;
using Funkmap.Tests.Auth.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceStack.Messaging;
using ServiceStack.Messaging.Redis;
using ServiceStack.Redis;

namespace Funkmap.Tests.Notifications
{
    [TestClass]
    public class NotificationsTest
    {
        private IContainer _container;

        [TestInitialize]
        public void Initialize()
        {
            var builder = new ContainerBuilder();
            var redisMqModule = new RedisMqModule();
            redisMqModule.Register(builder);

            var serviceType = typeof(Service<,>).MakeGenericType(new[] {typeof(Request), typeof(Response)});
            builder.RegisterType(serviceType).As<IService>();

            _container = builder.Build();

           

        }

        [TestMethod]
        public void InviteBandTest()
        {
            var specificNotificationService = _container.Resolve<FunkmapNotificationService>();

            var baseNotificationService = _container.Resolve<BandInviteNotificationsService>();

            var request = new InviteToGroupRequest()
            {
                InvitedMusicianLogin = "test",
                InviterLogin = "rogulenkoko",
                GroupLogin = "beatles",
                
            };
            specificNotificationService.InviteMusicianToGroup(request);
            Thread.Sleep(5000);
            baseNotificationService.PublishBackRequest(new InviteToGroupBackRequest() {RequestId = request.Id, Answer = true});
        }


        [TestMethod]
        public void AutofacMagic()
        {
            var col = _container.Resolve<IEnumerable<IService>>();
            col.FirstOrDefault().PublishBack(new Response());
            Thread.Sleep(5000);
        }

        public interface IService
        {
            void PublishBack(IResponse response);
        }

        public class Service<TRequest, TResponse> : RedisMqProducer, IRedisMqConsumer, IService where  TRequest : class, IRequest
                                                                                                where TResponse : class , IResponse
        {
            private readonly IMessageService _messageService;
            public Service(IMessageFactory redisMqFactory, IMessageService messageService) : base(redisMqFactory)
            {
                _messageService = messageService;
            }

            public void InitHandlers()
            {
                _messageService.RegisterHandler<TRequest>(req=> Publish(req?.GetBody()));
            }

            private bool Publish(TRequest request)
            {
                return true;
            }

            public void PublishBack(IResponse response)
            {
                Publish<TResponse>(response as TResponse);
            }
        }


        public interface IRequest
        {
            string Property { get; }   
        }

        public class Request : IRequest
        {
            public string Property => "test";
        }

        public interface IResponse
        {
            string ResponseProperty { get; }
        }

        public class Response : IResponse
        {
            public string ResponseProperty => "testresponse";
        }
    }
}
