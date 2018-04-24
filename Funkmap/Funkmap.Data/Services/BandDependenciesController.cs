using System.Threading.Tasks;
using Funkmap.Common.Cqrs.Abstract;
using Funkmap.Domain;
using Funkmap.Domain.Abstract.Repositories;
using Funkmap.Domain.Events;
using Funkmap.Domain.Models;

namespace Funkmap.Data.Services
{
    public class BandDependenciesController : IEventHandler<ProfileDeletedEvent>, IEventHandler<ProfileUpdatedEvent>
    {
        private readonly IBandRepository _bandRepository;
        private readonly IMusicianRepository _musicianRepository;

        private readonly IEventBus _eventBus;

        public BandDependenciesController(IBandRepository bandRepository, 
                                          IMusicianRepository musicianRepository,
                                          IEventBus eventBus)
        {
            _bandRepository = bandRepository;
            _musicianRepository = musicianRepository;
            _eventBus = eventBus;
        }

        public void InitHandlers()
        {
            _eventBus.Subscribe<ProfileDeletedEvent>(Handle);
            _eventBus.Subscribe<ProfileUpdatedEvent>(Handle);
        }

        public Task Handle(ProfileDeletedEvent @event)
        {
            return CleanDeletedDependencies(@event);
        }

        public Task Handle(ProfileUpdatedEvent @event)
        {
            return ProcessDependenciesAsync(@event);
        }

        public async Task ProcessDependenciesAsync(ProfileUpdatedEvent updatedEvent)
        {
            switch (updatedEvent.Profile.EntityType)
            {
                case EntityType.Musician:
                    await _bandRepository.ProcessMusicianDependenciesAsync(updatedEvent.Profile as Musician, updatedEvent.UpdatedProfile as Musician);
                    break;

                case EntityType.Band:
                    await _musicianRepository.ProcessBandDependenciesAsync(updatedEvent.Profile as Band, updatedEvent.UpdatedProfile as Band);
                    break;
            }
        }

        private async Task CleanDeletedDependencies(ProfileDeletedEvent deletedEvent)
        {
            var deletedProfile = deletedEvent.Profile;

            switch (deletedProfile.EntityType)
            {
                case EntityType.Musician:
                    await _bandRepository.ProcessMusicianDependenciesAsync(deletedProfile as Musician);
                    break;

                case EntityType.Band:
                    await _musicianRepository.ProcessBandDependenciesAsync(deletedProfile as Band);
                    break;
            }
        }
    }
}
