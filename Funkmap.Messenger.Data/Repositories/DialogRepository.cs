using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Funkmap.Common.Data.Mongo;
using Funkmap.Messenger.Data.Entities;
using Funkmap.Messenger.Data.Parameters;
using Funkmap.Messenger.Data.Repositories.Abstract;
using MongoDB.Driver;

namespace Funkmap.Messenger.Data.Repositories
{
    public class DialogRepository : IDialogRepository
    {
        private readonly IMongoCollection<DialogEntity> _collection;

        public DialogRepository(IMongoCollection<DialogEntity> collection)
        {
            _collection = collection;
        }
        
        public async Task<ICollection<DialogEntity>> GetUserDialogsAsync(UserDialogsParameter parameter)
        {
            var projection = Builders<DialogEntity>.Projection.Exclude(x => x.Messages);

            var dialogs = await _collection
                .Find(dialog => dialog.Participants.Any(participant => participant == parameter.Login))
                .Project<DialogEntity>(projection)
                .Skip(parameter.Skip)
                .Limit(parameter.Take)
                .ToListAsync();
            return  dialogs;
        }

        public async Task<ICollection<MessageEntity>> GetDialogMessagesAsync(DialogMessagesParameter parameter)
        {
            var filter = Builders<DialogEntity>.Filter.All(x => x.Participants, parameter.Members);

            var countProjection = Builders<DialogEntity>.Projection.Include(x => x.MessagesCount);
            var dialog = await _collection.Find(filter).Project<DialogEntity>(countProjection).SingleOrDefaultAsync();
            var messagesCount = dialog.MessagesCount;

            var skip = -(parameter.Skip + parameter.Take);
            var take = skip + messagesCount + parameter.Take;

            var projection = Builders<DialogEntity>.Projection.Slice(x => x.Messages, skip, take);
            
            dialog = await _collection.Find(filter).Project<DialogEntity>(projection).SingleOrDefaultAsync();
            var messages = dialog?.Messages;
            return messages;
        }

        public async Task CreateAsync(DialogEntity item)
        {
            await _collection.InsertOneAsync(item);
        }
    }
}
