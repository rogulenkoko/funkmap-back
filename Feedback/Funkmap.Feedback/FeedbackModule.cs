using System;
using Autofac;
using Funkmap.Cqrs.Abstract;
using Funkmap.Feedback.Command;
using Funkmap.Feedback.EventHandlers;

namespace Funkmap.Feedback
{
    /// <summary>
    /// IoC module for domain layer
    /// </summary>
    public static class FeedbackModule
    {
        /// <summary>
        /// Register all domain layer dependencies
        /// </summary>
        /// <param name="builder"><see cref="ContainerBuilder"/></param>
        public static void RegisterFeedbackModule(this ContainerBuilder builder)
        {
            builder.RegisterType<FeedbackSavedEventHandler>()
                .As<IEventHandler<FeedbackSavedEvent>>()
                .As<IEventHandler>()
                .OnActivated(x => x.Instance.InitHandlers())
                .AutoActivate()
                .SingleInstance();

            Console.WriteLine("Feedback module has been loaded.");
        }
    }
}
