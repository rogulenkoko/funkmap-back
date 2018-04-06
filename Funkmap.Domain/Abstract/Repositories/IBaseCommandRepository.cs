using System.Threading.Tasks;
using Funkmap.Common.Models;
using Funkmap.Domain.Models;
using Funkmap.Domain.Parameters;

namespace Funkmap.Domain.Abstract.Repositories
{
    public interface IBaseCommandRepository
    {
        Task<ICommandResponse> UpdateAsync(ICommandParameter<Profile> model);

        Task<ICommandResponse> CreateAsync(ICommandParameter<Profile> model);

        Task<ICommandResponse> UpdateAvatarAsync(ICommandParameter<AvatarUpdateParameter> parameter, ImageProcessorOptions options = null);

        Task<ICommandResponse<Profile>> DeleteAsync(ICommandParameter<string> parameter);

        Task<ICommandResponse> UpdateFavoriteAsync(UpdateFavoriteParameter parameter);
    }
}
