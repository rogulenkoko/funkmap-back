using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Funkmap.Messenger.Command.Abstract;
using Funkmap.Messenger.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Funkmap.Messenger.Command.Repositories
{
    public class MessengerCommandRepository : IMessengerCommandRepository
    {

        private readonly IMongoCollection<MessageEntity> _messagesCollection;
        private readonly IMongoCollection<DialogEntity> _dialogCollection;

        public MessengerCommandRepository(IMongoCollection<DialogEntity> dialogCollection,
                                          IMongoCollection<MessageEntity> messagesCollection)
        {
            _messagesCollection = messagesCollection;
            _dialogCollection = dialogCollection;
        }

        public async Task<DialogEntity> GetDialogAsync(string id)
        {
            var filter = Builders<DialogEntity>.Filter.Eq(x => x.Id, new ObjectId(id));
            var dialog = await _dialogCollection.Find(filter).SingleOrDefaultAsync();
            return dialog;
        }

        public async Task UpdateDialogAsync(DialogEntity dialog)
        {

            
            var filter = Builders<DialogEntity>.Filter.Eq(x => x.Id, dialog.Id);
            await _dialogCollection.ReplaceOneAsync(filter, dialog);
        }

        public async Task<ICollection<string>> GetDialogMembersAsync(string dialogId)
        {
            var filter = Builders<DialogEntity>.Filter.Eq(x => x.Id, new ObjectId(dialogId));
            var projection = Builders<DialogEntity>.Projection.Include(x => x.Participants);
            var dialog = await _dialogCollection.Find(filter).Project<DialogEntity>(projection).FirstOrDefaultAsync();
            var members = dialog?.Participants;
            return members;
        }

        public async Task AddMessageAsync(MessageEntity message)
        {
            await _messagesCollection.InsertOneAsync(message);
        }

        public async Task AddDialogAsync(DialogEntity dialog)
        {
            await _dialogCollection.InsertOneAsync(dialog);
        }

        public async Task<DialogEntity> UpdateLastMessageDateAsync(string dialogId, DateTime lastMessageDateTime)
        {
            if (String.IsNullOrEmpty(dialogId)) throw new ArgumentException(nameof(dialogId));
            if (lastMessageDateTime == DateTime.MinValue) throw new ArgumentException(nameof(lastMessageDateTime));

            var update = Builders<DialogEntity>.Update.Set(x => x.LastMessageDate, lastMessageDateTime);
            var dialog = await _dialogCollection.FindOneAndUpdateAsync(x => x.Id == new ObjectId(dialogId), update);
            return dialog;
        }

        public async Task MakeDialogMessagesReadAsync(string dialogId, string login, DateTime readTime)
        {
            var readFilter = Builders<MessageEntity>.Filter.AnyEq(x => x.ToParticipants, login)
                             & Builders<MessageEntity>.Filter.Lte(x => x.DateTimeUtc, readTime)
                             & Builders<MessageEntity>.Filter.Eq(x => x.DialogId, new ObjectId(dialogId))
                             & Builders<MessageEntity>.Filter.Ne(x => x.Sender, login);

            var update = Builders<MessageEntity>.Update.Pull(x => x.ToParticipants, login).Set(x => x.IsRead, true);

            await _messagesCollection.UpdateManyAsync(readFilter, update);
        }

        public async Task<DialogEntity> GetDialogByParticipants(string[] participants)
        {
            var filter = Builders<DialogEntity>.Filter.All(x => x.Participants, participants) & Builders<DialogEntity>.Filter.Eq(x=>x.DialogType, DialogType.Base);

            var dialog = await _dialogCollection.Find(filter).SingleOrDefaultAsync();

            return dialog;
        }
    }
}
