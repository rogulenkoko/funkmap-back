using Autofac;
using Funkmap.Common.Abstract;
using Funkmap.Common.Cqrs.Abstract;
using Funkmap.Feedback.Command.CommandHandler;
using Funkmap.Feedback.Command.Commands;

namespace Funkmap.Feedback.Command
{
    public class FeedbackCommandModule : IFunkmapModule
    {
        public void Register(ContainerBuilder builder)
        {
            builder.RegisterType<FeedbackCommandHandler>().As<ICommandHandler<FeedbackCommand>>();
        }
    }
}
