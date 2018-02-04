using System;
using System.Threading.Tasks;
using Akka.Actor;
using Funkmap.Common.Cqrs.Abstract;
using Funkmap.Concerts.Command.Commands;
using Funkmap.Concerts.Messages;
using Funkmap.Concerts.Query.Queries;
using Funkmap.Concerts.Query.Responses;

namespace Funkmap.Concerts.Actors
{
    public class SchedulerActor : ReceiveActor
    {
        private readonly IQueryContext _queryContext;
        private readonly FinishActor _finishActor;
        private readonly ActivationActor _activationActor;

        public SchedulerActor(IQueryContext queryContext, FinishActor finishActor, ActivationActor activationActor)
        {
            _queryContext = queryContext;
            _finishActor = finishActor;
            _activationActor = activationActor;

            Receive<InitializeSchedulersMessage>(async message => await InitializeSchedulers(message));
        }

        private async Task InitializeSchedulers(InitializeSchedulersMessage message)
        {
            var reponse = await _queryContext.Execute<NotFinishedQuery, NotFinishedConcertResponse>(new NotFinishedQuery());

            if(reponse.Concerts == null || reponse.Concerts.Count == 0) return;

            var now = DateTime.UtcNow;

            foreach (var concert in reponse.Concerts)
            {

                //планировщик на удаление события
                TimeSpan finishedTimespan = concert.PeriodEndUtc - now;

                if (finishedTimespan.Ticks < 0)
                {
                    finishedTimespan = TimeSpan.Zero;
                }

                var finishCommand = new FinishConcertMessage(concert.ConcertId);
                Context.System.Scheduler.ScheduleTellOnce(finishedTimespan, _finishActor, finishCommand, Self);


                //планировщик на состояние активности

                TimeSpan activationTimespan = now - concert.PeriodBeginUtc;
                if (activationTimespan.Ticks < 0)
                {
                    activationTimespan = TimeSpan.Zero;
                }

                var activateCommand = new ActivationMessage(concert.ConcertId);
                Context.System.Scheduler.ScheduleTellOnce(activationTimespan, _activationActor, activateCommand, Self);
            }
        }
    }
}
