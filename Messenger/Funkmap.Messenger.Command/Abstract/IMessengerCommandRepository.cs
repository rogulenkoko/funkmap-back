using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Funkmap.Messenger.Entities;

namespace Funkmap.Messenger.Command.Abstract
{
    internal interface IMessengerCommandRepository
    {
        Task<DialogEntity> GetDialogAsync(string id);
        Task UpdateDialogAsync(DialogEntity dialog);
        Task<ICollection<string>> GetDialogMembersAsync(string dialogId);
        Task AddMessageAsync(MessageEntity message);
        Task AddDialogAsync(DialogEntity dialog);
        Task<DialogEntity> UpdateLastMessageDateAsync(string dialogId, DateTime lastMessageDateTime);
        Task MakeDialogMessagesReadAsync(string dialogId, string login, DateTime readTime);

        Task<DialogEntity> GetDialogByParticipants(IReadOnlyCollection<string> participants);
    }
}
