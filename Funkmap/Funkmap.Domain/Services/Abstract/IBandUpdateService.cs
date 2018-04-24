using System.Threading.Tasks;
using Funkmap.Domain.Models;
using Funkmap.Domain.Parameters;

namespace Funkmap.Domain.Services.Abstract
{
    public interface IBandUpdateService
    {
        Task<InviteBandResponse> HandleInviteBandChanges(UpdateBandMemberParameter membersParameter, string userLogin);
    }
}
