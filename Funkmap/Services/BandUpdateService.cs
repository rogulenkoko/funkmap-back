using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Funkmap.Common;
using Funkmap.Data.Entities;
using Funkmap.Data.Entities.Abstract;
using Funkmap.Data.Repositories.Abstract;
using Funkmap.Models;
using Funkmap.Models.Requests;
using Funkmap.Models.Responses;
using Funkmap.Services.Abstract;

namespace Funkmap.Services
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

        public async Task<InviteBandResponse> HandleInviteBandChanges(UpdateBandMembersRequest membersRequest, string userLogin)
        {
            var musician = await _musicianRepository.GetAsync(membersRequest.MusicianLogin);
            if (musician == null) throw new ArgumentNullException(nameof(musician));
            var musicianOwnerLogin = musician.UserLogin;

            var band = await _bandRepository.GetAsync(membersRequest.BandLogin);
            if (band == null) throw new ArgumentNullException(nameof(band));

            var response = new InviteBandResponse();

            if ((band.InvitedMusicians != null && band.InvitedMusicians.Contains(musician.Login)) || (band.MusicianLogins != null && band.MusicianLogins.Contains(musician.Login)))
            {
                response.Error = "musician is already in band or invited";
                response.Success = false;
                return response;
            }

            if (musicianOwnerLogin == userLogin)
            {
                if (band.MusicianLogins == null) band.MusicianLogins = new List<string>();
                band.MusicianLogins.Add(musician.Login);
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

        public async Task CreateDependencies(UpdateBandMembersRequest request, bool needToAdd = true)
        {
            var band = await _bandRepository.GetAsync(request.BandLogin);
            if (band == null) return;

            if (needToAdd)
            {
                var musician = await _musicianRepository.GetAsync(request.MusicianLogin);
                if (musician == null) return;


                if (band.MusicianLogins == null) band.MusicianLogins = new List<string>();

                if (!band.MusicianLogins.Contains(request.MusicianLogin))
                {
                    band.MusicianLogins.Add(request.MusicianLogin);
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

        public void CleanDeletedDependencies(BaseEntity deletedEntity)
        {
            if (deletedEntity == null) throw new ArgumentNullException(nameof(deletedEntity));

            switch (deletedEntity.EntityType)
            {
                case EntityType.Musician:
                    _bandRepository.CleanMusiciansDependencies(deletedEntity as MusicianEntity);
                    break;

                case EntityType.Band:
                    _musicianRepository.CleanBandDependencies(deletedEntity as BandEntity);
                    break;
            }
        }

        public async Task CleanDependencies(CleanDependenciesParameter parameter)
        {
            if (parameter == null) throw new ArgumentNullException(nameof(parameter));

            MusicianEntity musician;
            BandEntity band;

            switch (parameter.EntityType)
            {
                case EntityType.Musician:
                    musician = await _musicianRepository.GetAsync(parameter.EntityLogin);
                    band = await _bandRepository.GetAsync(parameter.FromEntityLogin);

                    _musicianRepository.CleanBandDependencies(band, musician.Login);
                    _bandRepository.CleanMusiciansDependencies(musician, band.Login);
                    break;

                case EntityType.Band:
                    band = await _bandRepository.GetAsync(parameter.EntityLogin);
                    musician = await _musicianRepository.GetAsync(parameter.FromEntityLogin);

                    _musicianRepository.CleanBandDependencies(band, musician.Login);
                    _bandRepository.CleanMusiciansDependencies(musician, band.Login);
                    break;
            }
        }
    }
}
