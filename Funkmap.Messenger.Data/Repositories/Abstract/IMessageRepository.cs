using System.Collections.Generic;
using System.Threading.Tasks;
using Funkmap.Messenger.Data.Objects;
using Funkmap.Messenger.Data.Parameters;
using Funkmap.Messenger.Entities;

namespace Funkmap.Messenger.Data.Repositories.Abstract
{
    public interface IMessageRepository
    {
        Task<ICollection<MessageEntity>> GetDialogMessagesAsync(DialogMessagesParameter parameter);
        ICollection<ContentItem> GetMessagesContent(string[] contentIds);
        Task<ICollection<DialogEntity>> GetDialogsWithNewMessagesAsync(DialogsNewMessagesParameter paramete);
        Task<ICollection<DialogsNewMessagesCountResult>> GetDialogNewMessagesCount(DialogsNewMessagesParameter parameter);
        Task AddMessage(MessageEntity message);
    }
}
