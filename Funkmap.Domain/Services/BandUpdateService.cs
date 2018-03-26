using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Funkmap.Domain.Abstract.Repositories;
using Funkmap.Domain.Models;
using Funkmap.Domain.Parameters;
using Funkmap.Domain.Services.Abstract;

namespace Funkmap.Domain.Services
{
    public class BandUpdateService : IBandUpdateService, IDependenciesController
    {
        private readonly IBandRepository _bandRepository;
        private readonly IMusicianRepository _musicianRepository;
        private readonly IBaseRepository _baseRepository;

        public BandUpdateService(IBandRepository bandRepository, IMusicianRepository musicianRepository, IBaseRepository baseRepository)
        {
            _bandRepository = bandRepository;
            _musicianRepository = musicianRepository;
            _baseRepository = baseRepository;
        }

        public async Task<InviteBandResponse> HandleInviteBandChanges(UpdateBandMemberParameter membersParameter, string userLogin)
        {
            var musician = await _baseRepository.GetAsync<Musician>(membersParameter.MusicianLogin);

            if (musician == null) throw new ArgumentNullException(nameof(musician));
            var musicianOwnerLogin = musician.UserLogin;

            var bandProfile = await _baseRepository.GetAsync(membersParameter.BandLogin);

            var band = bandProfile as Band;

            if (band == null) throw new ArgumentNullException(nameof(band));

            var response = new InviteBandResponse() { MusicianLogin = membersParameter.MusicianLogin };

            if ((band.InvitedMusicians != null && band.InvitedMusicians.Contains(musician.Login)) || (band.Musicians != null && band.Musicians.Contains(musician.Login)))
            {
                response.Error = "Musician is already in band or invited";
                response.Success = false;
                return response;
            }

            if (musicianOwnerLogin == userLogin)
            {
                if (band.Musicians == null) band.Musicians = new List<string>();
                band.Musicians.Add(musician.Login);
                response.IsOwner = true;

                if (musician.BandLogins == null) musician.BandLogins = new List<string>();
                if (!musician.BandLogins.Contains(band.Login))
                {
                    musician.BandLogins.Add(band.Login);
                }
                _baseRepository.UpdateAsync(musician);

            }
            else
            {
                if (band.InvitedMusicians == null) band.InvitedMusicians = new List<string>();
                band.InvitedMusicians.Add(musician.Login);
                response.IsOwner = false;

            }

            response.Success = true;

            response.BandName = band.Name;
            response.OwnerLogin = musicianOwnerLogin;

            _baseRepository.UpdateAsync(band);

            return response;
        }

        public async Task CreateDependenciesAsync(UpdateBandMemberParameter request, bool needToAdd = true)
        {
            var band = await _baseRepository.GetAsync<Band>(request.BandLogin);
            if (band == null) return;

            if (needToAdd)
            {
                var musician = await _baseRepository.GetAsync<Musician>(request.MusicianLogin);
                if (musician == null) return;


                if (band.Musicians == null) band.Musicians = new List<string>();

                if (!band.Musicians.Contains(request.MusicianLogin))
                {
                    band.Musicians.Add(request.MusicianLogin);
                }

                if (band.InvitedMusicians != null && band.InvitedMusicians.Contains(request.MusicianLogin))
                {
                    band.InvitedMusicians.Remove(request.MusicianLogin);
                }

                if (musician.BandLogins == null) musician.BandLogins = new List<string>();

                if (!musician.BandLogins.Contains(band.Login))
                {
                    musician.BandLogins.Add(band.Login);
                }

                _baseRepository.UpdateAsync(musician);
            }
            else
            {
                if (band.InvitedMusicians != null && band.InvitedMusicians.Contains(request.MusicianLogin))
                {
                    band.InvitedMusicians.Remove(request.MusicianLogin);
                }
            }

            await _baseRepository.UpdateAsync(band);
        }

        public void CleanDeletedDependencies(Profile deletedEntity)
        {
            if (deletedEntity == null) throw new ArgumentNullException(nameof(deletedEntity));

            switch (deletedEntity.EntityType)
            {
                case EntityType.Musician:
                    _bandRepository.CleanMusiciansDependencies(deletedEntity as Musician);
                    break;

                case EntityType.Band:
                    _musicianRepository.CleanBandDependencies(deletedEntity as Band);
                    break;
            }
        }

        public async Task CleanDependenciesAsync(CleanDependenciesParameter parameter)
        {
            if (parameter == null) throw new ArgumentNullException(nameof(parameter));

            Musician musician;
            Band band;

            switch (parameter.EntityType)
            {
                case EntityType.Musician:
                    musician = await _baseRepository.GetAsync<Musician>(parameter.EntityLogin);
                    band = await _baseRepository.GetAsync<Band>(parameter.FromEntityLogin);

                    await _musicianRepository.CleanBandDependencies(band, musician.Login);
                    await _bandRepository.CleanMusiciansDependencies(musician, band.Login);
                    break;

                case EntityType.Band:
                    band = await _baseRepository.GetAsync<Band>(parameter.EntityLogin);
                    musician = await _baseRepository.GetAsync<Musician>(parameter.FromEntityLogin);

                    await _musicianRepository.CleanBandDependencies(band, musician.Login);
                    await _bandRepository.CleanMusiciansDependencies(musician, band.Login);
                    break;
            }
        }
    }
}
