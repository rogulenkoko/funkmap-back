using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac.Features.AttributeFilters;
using Funkmap.Common.Abstract;
using Funkmap.Messenger.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Funkmap.Messenger.Command.Repositories
{
    internal interface IMessengerCommandRepository
    {
        Task<ICollection<string>> GetDialogMembers(string dialogId);
        Task AddMessage(MessageEntity message);
        Task AddDialog(DialogEntity dialog);
        Task<DialogEntity> UpdateLastMessageDate(string dialogId, DateTime lastMessageDateTime);
        Task MakeDialogMessagesRead(string dialogId, string login, DateTime readTime);
    }

    internal class MessengerCommandRepository : IMessengerCommandRepository
    {

        private readonly IMongoCollection<MessageEntity> _messagesCollection;
        private readonly IMongoCollection<DialogEntity> _dialogCollection;
        private readonly IFileStorage _fileStorage;

        public MessengerCommandRepository(IMongoCollection<DialogEntity> dialogCollection,
                                          IMongoCollection<MessageEntity> messagesCollection,
            [KeyFilter(MessengerCollectionNameProvider.MessengerStorage)]IFileStorage fileStorage)
        {
            _messagesCollection = messagesCollection;
            _dialogCollection = dialogCollection;
            _fileStorage = fileStorage;
        }

        public async Task<ICollection<string>> GetDialogMembers(string dialogId)
        {
            var filter = Builders<DialogEntity>.Filter.Eq(x => x.Id, new ObjectId(dialogId));
            var projection = Builders<DialogEntity>.Projection.Include(x => x.Participants);
            var dialog = await _dialogCollection.Find(filter).Project<DialogEntity>(projection).FirstOrDefaultAsync();
            var members = dialog?.Participants;
            return members;
        }

        public async Task AddMessage(MessageEntity message)
        {
            if (message.Content != null && message.Content.Count > 0)
            {
                Parallel.ForEach(message.Content, item =>
                {
                    var fileId = _fileStorage.UploadFromBytesAsync(item.FileName, item.FileBytes).GetAwaiter().GetResult();
                    item.FileId = fileId;
                });
            }
            

            await _messagesCollection.InsertOneAsync(message);
        }

        public async Task AddDialog(DialogEntity dialog)
        {
            await _dialogCollection.InsertOneAsync(dialog);
        }

        public async Task<DialogEntity> UpdateLastMessageDate(string dialogId, DateTime lastMessageDateTime)
        {
            if (String.IsNullOrEmpty(dialogId)) throw new ArgumentException(nameof(dialogId));
            if (lastMessageDateTime == DateTime.MinValue) throw new ArgumentException(nameof(lastMessageDateTime));

            var update = Builders<DialogEntity>.Update.Set(x => x.LastMessageDate, lastMessageDateTime);
            var dialog = await _dialogCollection.FindOneAndUpdateAsync(x => x.Id == new ObjectId(dialogId), update);
            return dialog;
        }

        public async Task MakeDialogMessagesRead(string dialogId, string login, DateTime readTime)
        {
            var readFilter = Builders<MessageEntity>.Filter.AnyEq(x => x.ToParticipants, login)
                             & Builders<MessageEntity>.Filter.Lte(x => x.DateTimeUtc, readTime)
                             & Builders<MessageEntity>.Filter.Eq(x => x.DialogId, new ObjectId(dialogId))
                             & Builders<MessageEntity>.Filter.Ne(x => x.Sender, login);

            var update = Builders<MessageEntity>.Update.Pull(x => x.ToParticipants, login).Set(x => x.IsRead, true);

            await _messagesCollection.UpdateManyAsync(readFilter, update);
        }
    }
}
