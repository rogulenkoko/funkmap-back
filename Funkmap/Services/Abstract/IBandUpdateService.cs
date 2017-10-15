
using System.Threading.Tasks;
using Funkmap.Models.Requests;
using Funkmap.Models.Responses;

namespace Funkmap.Services.Abstract
{
    public interface IBandUpdateService
    {
        Task<InviteBandResponse> HandleInviteBandChanges(UpdateBandMembersRequest membersRequest, string userLogin);
    }
}
