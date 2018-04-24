using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Funkmap.Domain.Abstract.Repositories;
using Funkmap.Domain.Models;
using Funkmap.Domain.Parameters;
using Funkmap.Domain.Services.Abstract;

namespace Funkmap.Domain.Services
{
    public class BandUpdateService : IBandUpdateService
    {
        private readonly IBaseQueryRepository _baseQueryRepository;
        private readonly IBaseCommandRepository _commandRepository;

        public BandUpdateService(IBaseQueryRepository baseQueryRepository,
                                 IBaseCommandRepository commandRepository)
        {
            _baseQueryRepository = baseQueryRepository;
            _commandRepository = commandRepository;
        }

        public async Task<InviteBandResponse> HandleInviteBandChanges(UpdateBandMemberParameter membersParameter, string userLogin)
        {
            var musician = await _baseQueryRepository.GetAsync<Musician>(membersParameter.MusicianLogin);

            if (musician == null) throw new ArgumentNullException(nameof(musician));
            var musicianOwnerLogin = musician.UserLogin;

            var bandProfile = await _baseQueryRepository.GetAsync(membersParameter.BandLogin);

            var band = bandProfile as Band;

            if (band == null) throw new ArgumentNullException(nameof(band));

            var response = new InviteBandResponse() { MusicianLogin = membersParameter.MusicianLogin };

            if ((band.InvitedMusicians != null && band.InvitedMusicians.Contains(musician.Login)) || (band.Musicians != null && band.Musicians.Contains(musician.Login)))
            {
                response.Error = "Musician is already in band or invited.";
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

                var parameter = new CommandParameter<Profile>()
                {
                    Parameter = musician,
                    UserLogin = userLogin
                };
                await _commandRepository.UpdateAsync(parameter);

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

            var parameter1 = new CommandParameter<Profile>()
            {
                Parameter = band,
                UserLogin = band.UserLogin
            };

            await _commandRepository.UpdateAsync(parameter1);

            return response;
        }
    }
}
