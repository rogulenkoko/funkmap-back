using System.Collections.Generic;
using System.Threading.Tasks;
using Funkmap.Messenger.Data.Entities;
using Funkmap.Messenger.Data.Parameters;

namespace Funkmap.Messenger.Data.Repositories.Abstract
{
    public interface IDialogRepository
    {
        Task CreateAsync(DialogEntity item);
        Task<ICollection<DialogEntity>> GetUserDialogsAsync(UserDialogsParameter parameter);
        Task<ICollection<string>> GetDialogMembers(string id);

    }
}
